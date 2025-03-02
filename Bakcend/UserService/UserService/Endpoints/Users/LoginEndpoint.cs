using Application.User.LoginCommand;
using MediatR;

namespace UserService.Endpoints.Users;

public class LoginEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/users/login", async (LoginCommand model, ISender sender, CancellationToken token) =>
        {
            try
            {
                var result = await sender.Send(model, token);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
                
            }
        });
    }
}