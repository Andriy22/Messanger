
using Application.User.RegisterCommand;
using MediatR;

namespace UserService.Endpoints.Users;

public class RegisterEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("api/users/register", async (
            RegisterCommand model,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            try
            {
                await sender.Send(model, cancellationToken);

                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });
    }
}
