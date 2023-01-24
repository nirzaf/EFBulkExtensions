namespace EFBulkExtensions.Model;

public class Employee
{
    public Guid Id { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime DateOfJoining { get; set; }
        
    //Return object with EmployeeInsertDto properties
    public static Employee InsertModel(EmployeeInsertDto dto)
    {
        return new()
        {
            Id = Guid.NewGuid(),
            FullName = dto.FullName,
            Email = dto.Email,
            Address = dto.Address,
            Phone = dto.Phone,
            DateOfBirth = dto.DateOfBirth,
            DateOfJoining = dto.DateOfJoining
        };
    }
        
    public static Employee UpdateModel(EmployeeUpdateDto dto)
    {
        return new()
        {
            Id = dto.Id,
            FullName = dto.FullName,
            Email = dto.Email,
            Address = dto.Address,
            Phone = dto.Phone,
            DateOfBirth = dto.DateOfBirth,
            DateOfJoining = dto.DateOfJoining
        };
    }
    
    public static Employee DeleteModel(EmployeeDeleteDto dto)
    {
        return new()
        {
            Id = dto.Id
        };
    }
}