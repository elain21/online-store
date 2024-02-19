namespace Database.Models;

public class User
{
    public int Id { get; set; }

    public string PhoneNumber { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public byte[]? Password { get; set; } = null;
}