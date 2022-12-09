using FluentValidation;
using LifeMates.Core.Commands.User;
using LifeMates.Core.Mediatr.Behaviors;
using LifeMates.Core.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace LifeMates.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddMediatR(typeof(CreateUserCommand).Assembly);
        
        services.AddTransient<IAuthService, AuthService>();
        
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(FluentResultsPipelineBehavior<,>));
        services.AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>(ServiceLifetime.Singleton);
        
        return services;
    }
}