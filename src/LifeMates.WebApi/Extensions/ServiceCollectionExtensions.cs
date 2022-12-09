using System.Text.Json.Serialization;
using LifeMates.Auth.Extensions;
using LifeMates.WebApi.Profiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace LifeMates.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(UserProfile));

        services
            .AddControllers()
            .AddJsonOptions(options => 
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));;
        
        services.AddAuth(configuration);
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "LifeMates API", Version = "v1" });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        });
        
        return services;
    }
}