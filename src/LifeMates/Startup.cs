using AutoMapper;
using LifeMates.Core.Extensions;
using LifeMates.Storage;
using LifeMates.Storage.Extensions;
using LifeMates.WebApi.Extensions;
using Microsoft.AspNetCore.Identity;

namespace LifeMates;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddIdentityDb(Configuration)
            .AddRepositories()
            .AddCore()
            .AddWebApi(Configuration);
    }

    public void Configure(
        IApplicationBuilder app, 
        ILoggerFactory loggerFactory, 
        IMapper mapper)
    {
        mapper.ConfigurationProvider.AssertConfigurationIsValid();
        
        app.UpdateIdentityDatabase();
        app.InitializeDb(Configuration);
        
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}