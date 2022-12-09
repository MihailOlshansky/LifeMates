using LifeMates.Domain.Models.Users;
using LifeMates.Storage.Profiles;
using LifeMates.Storage.ReadOnlyRepositories;
using LifeMates.Storage.Repositories;
using LifeMates.Storage.SharedKernel;
using LifeMates.Storage.SharedKernel.ReadOnlyRepositories;
using LifeMates.Storage.SharedKernel.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LifeMates.Storage.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddIdentityDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LifematesDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddIdentity<UserCredentials, IdentityRole>(opts => {
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            })
            .AddEntityFrameworkStores<LifematesDbContext>();
        
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ViewProfile));
        
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IUserLocationRepository, UserLocationRepository>();
        services.AddTransient<IUserCredentialsRepository, UserCredentialsRepository>();
        services.AddTransient<IUserSettingsRepository, UserSettingsRepository>();
        services.AddTransient<IUserLikesRepository, UserLikesRepository>();
        services.AddTransient<IUserDislikeRepository, UserDislikeRepository>();
        services.AddTransient<IUserChatRepository, UserChatRepository>();
        services.AddTransient<IReportRepository, ReportRepository>();
        services.AddTransient<IUserReadOnlyRepository, UserReadOnlyRepository>();
        services.AddTransient<IChatRepository, ChatRepository>();
        services.AddTransient<IChatReadOnlyRepository, ChatReadOnlyRepository>();
        services.AddTransient<IInterestRepository, InterestRepository>();
        services.AddTransient<IInterestReadOnlyRepository, InterestReadOnlyRepository>();
        services.AddTransient<IMessageRepository, MessageRepository>();

        return services;
    }
}