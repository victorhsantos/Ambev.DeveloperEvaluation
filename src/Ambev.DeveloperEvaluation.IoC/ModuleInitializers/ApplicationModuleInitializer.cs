using Ambev.DeveloperEvaluation.Application.Sales.AddSale;
using Ambev.DeveloperEvaluation.Application.Sales.AlterSale;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Common.Security;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

public class ApplicationModuleInitializer : IModuleInitializer
{
    public void Initialize(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();

        builder.Services.AddMassTransit(x =>
        {
            builder.Services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(builder.Configuration.GetConnectionString("RabbitMq"));
                    cfg.ReceiveEndpoint(nameof(SaleCreated), endpoint => { endpoint.Bind(nameof(AddSaleCommand)); });
                    cfg.ReceiveEndpoint(nameof(SaleAltered), endpoint => { endpoint.Bind(nameof(AlterSaleCommand)); });
                    cfg.ReceiveEndpoint(nameof(SaleCancelled), endpoint => { endpoint.Bind(nameof(DeleteSaleCommand)); });
                });
            });
        });
    }
}