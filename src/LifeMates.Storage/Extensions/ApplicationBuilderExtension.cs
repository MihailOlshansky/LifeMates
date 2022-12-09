using LifeMates.Domain.Models.Users;
using LifeMates.Domain.Shared.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LifeMates.Storage.Extensions;

public static class ApplicationBuilderExtension
{
    public static void UpdateIdentityDatabase(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope();

        using var context = serviceScope.ServiceProvider.GetService<LifematesDbContext>();

        if (context is null)
        {
            throw new InvalidOperationException("Не подключена контекст для учетных данных");
        }
        
        context.Database.Migrate();
    }
    
    public static void InitializeDb(
        this IApplicationBuilder app,
        IConfiguration configuration)
    {
        using var serviceScope = app.ApplicationServices
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope();
        
        using var context = serviceScope.ServiceProvider.GetService<LifematesDbContext>();
        using var userManager = serviceScope.ServiceProvider.GetService<UserManager<UserCredentials>>();
        
        if (context.Roles.Any())
        {
            return;
        }
        
        var role = new IdentityRole()
        {
            Name = RoleTypes.Admin.ToString(),
            NormalizedName = RoleTypes.Admin.ToString().ToUpper()
        };
        
        context.Roles.Add(role);

        var user = new User("admin", default, default, default, default, 
            new UserLocation(1000, 1000), // не на земле 
            new UserSettings(UserGender.NonBinary), 
            new List<UserImage>(),
            new List<UserContact>(),
            new List<UserInterest>());

        context.Users.Add(user);
        
        context.SaveChanges();

        var adminCredentials = configuration.GetSection("AdminCredentials");

        var admin = new UserCredentials(user.Id, adminCredentials["Email"], adminCredentials["UserName"]);

        _ = userManager!.CreateAsync(admin, adminCredentials["Password"]).Result;
        _ = userManager.AddToRoleAsync(admin, RoleTypes.Admin.ToString()).Result;
    }
}