using Microsoft.EntityFrameworkCore;
using Bogus;

namespace EFBulkExtensions.Model
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasKey(e => e.Id); 
            //enable temporal table 
            modelBuilder.Entity<Employee>().ToTable(nameof(Employees), b=>b.IsTemporal());
            
            /*
            var employees = new Faker<Employee>()
                .RuleFor(e => e.Id, f => Guid.NewGuid())
                .RuleFor(e => e.FullName, f => f.Name.FullName())
                .RuleFor(e => e.Email, f => f.Internet.Email())
                .RuleFor(e => e.Address, f => f.Address.FullAddress())
                .RuleFor(e => e.Phone, f => f.Phone.PhoneNumber())
                .RuleFor(e => e.DateOfBirth, f => f.Date.Past(50, DateTime.Now.AddYears(-18)))
                .RuleFor(e => e.DateOfJoining, f => f.Date.Past(5, DateTime.Now));
            
            modelBuilder.Entity<Employee>().HasData(employees.Generate(5000));
            */

            base.OnModelCreating(modelBuilder);
        }
    }
}
