using Bogus;

using EFBulkExtensions.Model;

using EFCore.BulkExtensions;

using Microsoft.AspNetCore.Mvc;

namespace EFBulkExtensions.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var employees = _context.Employees.ToList();
            return Ok(employees);
        }

        // get request to fetch employees list with pagination and sorting support
        [HttpGet("list")]
        public IActionResult GetEmployeeList([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string sort = "id", [FromQuery] string sortDir = "asc")
        {
            var employees = _context.Employees.AsQueryable();

            // sorting
            if (sortDir == "asc")
            {
                switch (sort)
                {
                    case "id":
                        employees = employees.OrderBy(e => e.Id);
                        break;
                    case "name":
                        employees = employees.OrderBy(e => e.FullName);
                        break;
                    case "email":
                        employees = employees.OrderBy(e => e.Email);
                        break;
                    case "address":
                        employees = employees.OrderBy(e => e.Address);
                        break;
                    case "phone":
                        employees = employees.OrderBy(e => e.Phone);
                        break;
                    case "dob":
                        employees = employees.OrderBy(e => e.DateOfBirth);
                        break;
                    case "doj":
                        employees = employees.OrderBy(e => e.DateOfJoining);
                        break;
                }
            }
            else
            {
                switch (sort)
                {
                    case "id":
                        employees = employees.OrderByDescending(e => e.Id);
                        break;
                    case "name":
                        employees = employees.OrderByDescending(e => e.FullName);
                        break;
                    case "email":
                        employees = employees.OrderByDescending(e => e.Email);
                        break;
                    case "address":
                        employees = employees.OrderByDescending(e => e.Address);
                        break;
                    case "phone":
                        employees = employees.OrderByDescending(e => e.Phone);
                        break;
                    case "dob":
                        employees = employees.OrderByDescending(e => e.DateOfBirth);
                        break;
                    case "doj":
                        employees = employees.OrderByDescending(e => e.DateOfJoining);
                        break;
                }
            }

            // pagination
            var totalRecords = employees.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            var data = employees.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Ok(new
            {
                data,
                totalRecords,
                totalPages
            });
        }


        [HttpPost]
        public async Task<string> Post([FromBody] List<Employee> employees)
        {
            employees = new Faker<Employee>()
                .RuleFor(e => e.Id, f => Guid.NewGuid())
                .RuleFor(e => e.FullName, f => f.Name.FullName())
                .RuleFor(e => e.Email, f => f.Internet.Email())
                .RuleFor(e => e.Address, f => f.Address.FullAddress())
                .RuleFor(e => e.Phone, f => f.Phone.PhoneNumber())
                .RuleFor(e => e.DateOfBirth, f => f.Date.Past(50, DateTime.Now.AddYears(-18)))
                .RuleFor(e => e.DateOfJoining, f => f.Date.Past(5, DateTime.Now))
                .Generate(100000);

            //Track the time 
            var watch = System.Diagnostics.Stopwatch.StartNew();
            await _context.BulkInsertAsync(employees);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("Time taken to insert 100000 records using EfCore bulk Extensions: " + elapsedMs + "ms");

            return "Time taken to insert 100000 records using EfCore bulk Extensions: " + elapsedMs + "ms";
        }

        [HttpPost("bulk")]
        public async Task<string> BulkInsert([FromBody] List<Employee> employees)
        {
            employees = new Faker<Employee>()
                .RuleFor(e => e.Id, f => Guid.NewGuid())
                .RuleFor(e => e.FullName, f => f.Name.FullName())
                .RuleFor(e => e.Email, f => f.Internet.Email())
                .RuleFor(e => e.Address, f => f.Address.FullAddress())
                .RuleFor(e => e.Phone, f => f.Phone.PhoneNumber())
                .RuleFor(e => e.DateOfBirth, f => f.Date.Past(50, DateTime.Now.AddYears(-18)))
                .RuleFor(e => e.DateOfJoining, f => f.Date.Past(5, DateTime.Now))
                .Generate(100000);

            //Track the time 
            var watch = System.Diagnostics.Stopwatch.StartNew();
            await _context.AddRangeAsync(employees);
            await _context.SaveChangesAsync();

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("Time taken to insert 100000 records EF Core: " + elapsedMs + "ms");

            return "Time taken to insert 100000 records EF Core: " + elapsedMs + "ms";
        }

        //Api to find all duplicate email ids
        [HttpGet("duplicate")]
        public IActionResult GetDuplicateEmails()
        {
            var duplicateEmails = _context.Employees.GroupBy(e => e.Email).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
            return Ok(duplicateEmails);
        }

        //find all duplicate email ids and update them
        [HttpPut("duplicate")]
        public async Task<IActionResult> UpdateDuplicateEmails()
        {
            var duplicateEmails = _context.Employees.GroupBy(e => e.Email).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
            var employees = _context.Employees.Where(e => duplicateEmails.Contains(e.Email)).ToList();
            employees.ForEach(e => e.Email += Guid.NewGuid().ToString()[..5]);
            await _context.BulkInsertOrUpdateAsync(employees);
            return Ok();
        }
    }
}
