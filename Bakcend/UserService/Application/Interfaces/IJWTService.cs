using Domain;

namespace Application.Interfaces;

public interface IJwtService
{
    Task<string> GenerateJwtAsync(AppUser user);
}