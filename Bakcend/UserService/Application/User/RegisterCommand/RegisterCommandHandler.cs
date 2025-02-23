using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.User.RegisterCommand;

public class RegisterCommandHandler(UserManager<AppUser> userManager) : IRequestHandler<RegisterCommand, string>
{
    public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new AppUser
        {
            Email = request.Email,
            UserName = request.Email
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            return user.Id;
        }

        throw new Exception(message: result.Errors.FirstOrDefault()?.Description);
    }
}   
