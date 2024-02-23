using System.Text;
using Database;
using Database.Models;
using MediatR;

namespace Core.Users.Commands;

public class CreateUserCmd : IRequest<bool>
{
    public string PhoneNumber { get; set; } = null!;

    public string Password { get; set; } = null!;
}

public class CreateUserCmdHandler : IRequestHandler<CreateUserCmd, bool>
{
    private readonly OnlineStoreContext _onlineStoreContext;
    
    public CreateUserCmdHandler(OnlineStoreContext onlineStoreContext)
    {
        _onlineStoreContext = onlineStoreContext;
    }
    
    public async Task<bool> Handle(CreateUserCmd request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Password))
            return false;

        try
        {
            var passwordCrypt = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var passwordBytes = Encoding.ASCII.GetBytes(passwordCrypt); 
        
            var newUser = new User
            {
                PhoneNumber = request.PhoneNumber,
                Password = passwordBytes
            };

            await _onlineStoreContext.Users.AddAsync(newUser, cancellationToken);
            await _onlineStoreContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);

            return false;
        }

        return true;
    }
}