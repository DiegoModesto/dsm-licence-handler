using Application.Abstractions.Messaging;
using Application.Users.Login;
using SharedKernel;
using Web.API.Extensions;
using Web.API.Infrastructure;

namespace Web.API.Endpoints.Users;

internal sealed class Login : IEndpoint
{
    public sealed record Request(string Email, string Password);
    
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(pattern: "users/login", async (
                Request request,
                ICommandHandler<LoginUserCommand, string> handler,
                CancellationToken cancellationToken) =>
            {
                var command = new LoginUserCommand(request.Email, request.Password);
                
                Result<string> result = await handler.Handle(command, cancellationToken);
                
                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Users);
    }
}
