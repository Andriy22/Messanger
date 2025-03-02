using MediatR;

namespace Application.User.LoginCommand;

public class LoginCommand : IRequest<string>
{
    public required string Email { get; set; }
    
    public required string Password { get; set; }
}