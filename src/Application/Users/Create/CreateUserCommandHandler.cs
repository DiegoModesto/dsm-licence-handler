using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.Create;

public sealed class CreateUserCommandHandler(IApplicationDbContext context)
    : ICommandHandler<CreateUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        if (await context.Users.AnyAsync(u => u.Email == command.Email, cancellationToken))
        {
            return Result.Failure<Guid>(error: UserErrors.EmailNotUnique);
        }
        
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = command.Email,
            FirstName = command.FirstName,
            LastName = command.LastName,
            PasswordHash = command.Password // TODO: Criar cripto hash
        };

        //TODO: Criar evento de registro de usuário
        //user.Raise(new UserRegisterDomainEvent(user.id));

        context.Users.Add(user);
        
        await context.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
