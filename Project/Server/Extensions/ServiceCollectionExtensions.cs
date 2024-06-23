using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Repository.Contracts;
using Server.Repository.Implementations;

namespace Server.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataContext(configuration);
        services.AddRepositories();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }

    public static IServiceCollection AddDataContext(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddDbContext<DataContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("DCS"));
        });
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        //services.AddScoped<IServerCountryRepository, ServerCountryRepository>();
        //services.AddScoped<IServerCategoryRepository, ServerCategoryRepository>();
        //services.AddScoped<IServerBookRepository, ServerBookRepository>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        //services.AddScoped<IServerReviewRepository, ServerReviewRepository>();
        //services.AddScoped<IServerReviewerRepository, ServerReviewerRepository>();
    }

    public static IApplicationBuilder ConfigureMiddleware(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        return app;
    }
}
