using System.Text;
using Database;
using Database.Models;
using Microsoft.AspNetCore.Mvc;

namespace OnlineStore.Controllers;

public class UsersController : Controller
{
    private OnlineStoreContext OnlineStoreContext { get; }
    
    public UsersController(OnlineStoreContext onlineStoreContext)
    {
        OnlineStoreContext = onlineStoreContext;
    }
    
    [HttpPost("createUser")]
    public IActionResult CreateUser([FromQuery] string phoneNumber, [FromQuery] string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return BadRequest("Пустой пароль");
        }
        
        var passwordCrypt = BCrypt.Net.BCrypt.HashPassword(password);
        var passwordBytes = Encoding.ASCII.GetBytes(passwordCrypt); 
        
        var newUser = new User
        {
            PhoneNumber = phoneNumber,
            Password = passwordBytes
        };

        OnlineStoreContext.Users.Add(newUser);
        OnlineStoreContext.SaveChanges();

        return Ok();
    }

    [HttpPost("signIn")]
    public IActionResult SignIn([FromQuery] string phoneNumber, [FromQuery] string password)
    {
        var user = OnlineStoreContext.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber);
        if (user == null)
        {
            return BadRequest("Неверный логин или пароль");
        }

        var userPassword = Encoding.ASCII.GetString(user.Password!);

        return BCrypt.Net.BCrypt.Verify(password, userPassword) ? Ok() : BadRequest("Неверный логин или пароль");
    }
}