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
    public IActionResult CreateUser([FromQuery] string phoneNumber)
    {
        var newUser = new User 
        {
            PhoneNumber = phoneNumber
        };

        OnlineStoreContext.Users.Add(newUser);
        OnlineStoreContext.SaveChanges();

        return Ok();
    }
}