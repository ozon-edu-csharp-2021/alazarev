using System;
using Microsoft.Extensions.DependencyInjection;
using OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.HumanResourceManagerAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Domain.Contracts;
using OzonEdu.MerchApi.Domain.Contracts.DomainServices;
using OzonEdu.MerchApi.Domain.Contracts.StockApiService;
using OzonEdu.MerchApi.Infrastructure.Bus;
using OzonEdu.MerchApi.Infrastructure.InfrastructureServices;
using OzonEdu.MerchApi.Infrastructure.Repositories;
using OzonEdu.MerchApi.Infrastructure.Uow;

namespace OzonEdu.MerchApi.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, Type typeForScan)
        {
            services.AddDomainServices(typeForScan);
            services.AddRepositories(typeForScan);
            services.AddScoped<IUnitOfWork, FakeUnitOfWork>();
            services.AddScoped<IMessageBus, FakeMessageBus>();
            services.AddScoped<IStockApiService, FakeStockApiService>();
            return services;
        }

        public static IServiceCollection AddDomainServices(this IServiceCollection services, Type typeForScan)
        {
            services.Scan(scan =>
            {
                scan.FromAssembliesOf(typeForScan)
                    .AddClasses(classes => classes.AssignableTo<IDomainService>())
                    .AsImplementedInterfaces();
            });

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services, Type typeForScan)
        {
            services.Scan(scan =>
            {
                scan.FromAssembliesOf(typeForScan)
                    .AddClasses(classes => classes.AssignableTo(typeof(IRepository<>)))
                    .AsImplementedInterfaces();
            });
            services.AddScoped<IEmployeeRepository, FakeEmployeeRepository>();
            services.AddScoped<IMerchPackRepository, FakeMerchPackRepository>();
            services.AddScoped<IMerchRequestRepository, FakeMerchRequestRepository>();
            services.AddScoped<IHumanResourceManagerRepository, FakeHumanResourceManagerRepository>();

            return services;
        }
    }
}