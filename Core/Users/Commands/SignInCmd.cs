using System.Text;
using Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Users.Commands;

public class SignInCmd : IRequest<bool>
{
    public string PhoneNumber { get; set; } = null!;

    public string Password { get; set; } = null!;
}

public class SignInCmdHandler : IRequestHandler<SignInCmd, bool>
{
    private readonly OnlineStoreContext _onlineStoreContext;
    
    public SignInCmdHandler(OnlineStoreContext onlineStoreContext)
    {
        _onlineStoreContext = onlineStoreContext;
    }
    
    public async Task<bool> Handle(SignInCmd request, CancellationToken cancellationToken)
    {
        var user = await _onlineStoreContext.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber, cancellationToken);
        if (user == null)
        {
            return false;
        }

        var userPassword = Encoding.ASCII.GetString(user.Password!);

        return BCrypt.Net.BCrypt.Verify(request.Password, userPassword);
    }
}