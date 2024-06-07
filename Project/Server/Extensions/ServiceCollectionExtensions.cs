﻿using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Repository.Contracts;
using Server.Repository.Services;

namespace Server.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddDbContext<DataContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("DCS"));
        });
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();
    }
}