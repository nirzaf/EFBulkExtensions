namespace EFBulkExtensions.Model;

public class EmployeeInsertDto
{
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime DateOfJoining { get; set; }
}

public class EmployeeUpdateDto
{
    public Guid Id { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime DateOfJoining { get; set; }
}

public class EmployeeDeleteDto
{
    public Guid Id { get; set; }
}