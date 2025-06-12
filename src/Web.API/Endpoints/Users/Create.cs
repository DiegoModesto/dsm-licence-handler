using Application.Abstractions.Messaging;
using Application.Users.Create;
using SharedKernel;
using Web.API.Extensions;
using Web.API.Infrastructure;

namespace Web.API.Endpoints.Users;

internal sealed class Create : IEndpoint
{
    public sealed record Request(string Email, string FirstName, string LastName, string Password);
    
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(pattern: "users", async (
            Request request,
            ICommandHandler<CreateUserCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateUserCommand(
                Email: request.Email,
                FirstName: request.FirstName,
                LastName: request.LastName,
                Password: request.Password);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Created, CustomResults.Problem);
        })
        .HasPermission(Permissions.UsersAccess)
        .WithTags(Tags.Users);
    }
}
