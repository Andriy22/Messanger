using System.Security.Claims;
using System.Text;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Application.User.LoginCommand;

public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IJwtService _jwtService;
    

    public LoginCommandHandler(SignInManager<AppUser> signInManager, IJwtService jwtService)
    {
        this._signInManager = signInManager;
        this._jwtService = jwtService;
    }

    public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, false);

        if (!result.Succeeded)
        {
            Log.Warning("Failed login attempt for user {Email}. IsLockedOut: {IsLockedOut}, RequiresTwoFactor: {RequiresTwoFactor}", 
                request.Email, result.IsLockedOut, result.RequiresTwoFactor);
            throw new Exception("Invalid login attempt.");
        }    

        var user = await _signInManager.UserManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            Log.Error("User {Email} not found after successful password validation", request.Email);
            throw new Exception("Invalid login attempt.");
        }

        Log.Information("User {Email} successfully logged in", request.Email);
        
        return await _jwtService.GenerateJwtAsync(user);
    }
}