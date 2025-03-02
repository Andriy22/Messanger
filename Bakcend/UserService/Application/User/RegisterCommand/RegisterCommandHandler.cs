using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.User.RegisterCommand;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, string>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<RegisterCommandHandler> _logger;

    public RegisterCommandHandler(UserManager<AppUser> userManager, ILogger<RegisterCommandHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting user registration process for email: {Email}", request.Email);

        var user = new AppUser
        {
            Email = request.Email,
            UserName = request.Email
        };

        try
        {
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User successfully registered with ID: {UserId}", user.Id);
                return user.Id;
            }

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogError("User registration failed for email {Email}. Errors: {Errors}", 
                request.Email, errors);
            
            throw new ApplicationException($"Registration failed: {errors}");
        }
        catch (Exception ex) when (ex is not ApplicationException)
        {
            _logger.LogError(ex, "Unexpected error during user registration for email {Email}", 
                request.Email);
            throw;
        }
    }
}
