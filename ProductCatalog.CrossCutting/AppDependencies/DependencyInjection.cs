using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using ProductCatalog.Application.Products.Commands.Validations;
using ProductCatalog.Domain.Abstractions;
using ProductCatalog.Infrastructure.Context;
using ProductCatalog.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.CrossCutting.AppDependencies
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            // 1. Lendo a Connection String do appsettings.json da API
            var postgresConnection = configuration.GetConnectionString("DefaultConnection");

            // 2. EF Core para os Commands (Escrita)
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(postgresConnection,
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

            // 3. Dapper para as Queries (Leitura) - O equivalente à linha vermelha da imagem
            services.AddScoped<IDbConnection>(provider =>
            {
                // Cria a conexão com o PostgreSQL passando a string de conexão
                var config = provider.GetRequiredService<IConfiguration>();
                var connectionString = config.GetConnectionString("DefaultConnection");

                return new NpgsqlConnection(connectionString);
            });

            // 3. Registrando as abstrações (Interfaces) com suas implementações concretas
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductDapperRepository, ProductDapperRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            var myhandlers = AppDomain.CurrentDomain.Load("ProductCatalog.Application");
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssemblies(myhandlers);
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            services.AddValidatorsFromAssembly(Assembly.Load("ProductCatalog.Application"));

            return services;
        }
    }
}
