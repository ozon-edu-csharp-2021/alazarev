using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Domain.Contracts;
using OzonEdu.MerchApi.Domain.Contracts.DomainServices;
using OzonEdu.MerchApi.Domain.Contracts.StockApiService;
using OzonEdu.MerchApi.Infrastructure.Bus;
using OzonEdu.MerchApi.Infrastructure.Configuration;
using OzonEdu.MerchApi.Infrastructure.InfrastructureServices;
using OzonEdu.MerchApi.Infrastructure.Persistence;
using OzonEdu.MerchApi.Infrastructure.Persistence.Interfaces;
using OzonEdu.MerchApi.Infrastructure.Persistence.Repositories;
using OzonEdu.MerchApi.Infrastructure.Repositories;
using OzonEdu.MerchApi.Infrastructure.StockApi;
using OzonEdu.StockApi.Infrastructure.Repositories.Infrastructure.Interfaces;

namespace OzonEdu.MerchApi.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration, Type typeForScan)
        {
            var stockApiConfigurationSection = configuration.GetSection(nameof(StockApiOptions));
            var stockApiOptions = stockApiConfigurationSection.Get<StockApiOptions>();
            services.Configure<StockApiOptions>(stockApiConfigurationSection);
            
            services.AddGrpcClient<StockApiGrpc.StockApiGrpcClient>(o => { o.Address = new Uri(stockApiOptions.Url); });
            services.AddDomainServices(typeForScan);
            services.AddRepositories(typeForScan);
            services.AddScoped<IUnitOfWork, FakeUnitOfWork>();
            services.AddScoped<IMessageBus, FakeMessageBus>();
            services.AddScoped<IStockApiService, StockApiService>();

            services.AddDatabaseComponents(configuration);
            return services;
        }

        private static IServiceCollection AddDomainServices(this IServiceCollection services, Type typeForScan)
        {
            services.Scan(scan =>
            {
                scan.FromAssembliesOf(typeForScan)
                    .AddClasses(classes => classes.AssignableTo<IDomainService>())
                    .AsImplementedInterfaces();
            });

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services, Type typeForScan)
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            services.Scan(scan =>
            {
                scan.FromAssembliesOf(typeForScan)
                    .AddClasses(classes => classes.AssignableTo(typeof(IRepository<>)))
                    .AsImplementedInterfaces();
            });
            services.AddScoped<IMerchPackRepository, MerchPackRepository>();
            services.AddScoped<IMerchRequestRepository, MerchRequestRepository>();

            return services;
        }

        private static IServiceCollection AddDatabaseComponents(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<DatabaseConnectionOptions>(configuration.GetSection(nameof(DatabaseConnectionOptions)));
            services.AddScoped<IDbConnectionFactory<NpgsqlConnection>, NpgsqlConnectionFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IChangeTracker, ChangeTracker>();
            return services;
        }
    }
}