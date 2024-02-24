using System.Text;
using Core.Users.Commands;
using Database;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace OnlineStore.Controllers;

[ApiController]
public class UsersController : Controller
{
    private OnlineStoreContext OnlineStoreContext { get; }
    
    private IMediator Mediator { get; }
    
    public UsersController(OnlineStoreContext onlineStoreContext, IMediator mediator)
    {
        OnlineStoreContext = onlineStoreContext;
        Mediator = mediator;
    }
    
    [HttpPost("createUser")]
    public async Task<IActionResult> CreateUser(CreateUserCmd cmd)
    {
        var result = await Mediator.Send(cmd);

        return result ? Ok() : BadRequest();
    }

    [HttpPost("signIn")]
    public async Task<IActionResult> SignIn(SignInCmd cmd)
    {
        var result = await Mediator.Send(cmd);

        return result ? Ok() : BadRequest("Неверный логин или пароль");
    }
}