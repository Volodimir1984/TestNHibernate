using ConstData;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using CompaniesService.Consumers;

namespace CompaniesService
{
    public static class MassTransitExtension
    {
        //Extension method for turn on MassTransit
        public static IServiceCollection AddMassTransitService(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var massTransitSection = configuration.GetSection("MassTransit");
            var url = massTransitSection.GetValue<string>("Url");
            var host = massTransitSection.GetValue<string>("Host");
            var userName = massTransitSection.GetValue<string>("UserName");
            var password = massTransitSection.GetValue<string>("Password");

            if (massTransitSection == null || url == null || host == null)
            {
                throw new Exception(ConstStringForException.ErrorConnectToRabbitMq);
            }

            serviceCollection.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host($"rabbitmq://{url}/{host}", configurator =>
                    {
                        configurator.Username(userName);
                        configurator.Password(password);
                    });

                    cfg.ClearMessageDeserializers();
                    cfg.UseRawJsonSerializer();
                    cfg.ConfigureEndpoints(context, KebabCaseEndpointNameFormatter.Instance);
                });


                x.AddConsumer<GetCompanyConsumer>().Endpoint(e => e.Name = ConstStringForQueues.GetCompany);
                x.AddConsumer<GetCompaniesConsumer>().Endpoint(e => e.Name = ConstStringForQueues.GetCompanies);
                x.AddConsumer<UpdateCompanyConsumer>().Endpoint(e => e.Name = ConstStringForQueues.UpdateCompany);
                x.AddConsumer<CreateCompanyConsumer>().Endpoint(e => e.Name = ConstStringForQueues.CreateCompany);
                x.AddConsumer<DeleteCompanyConsumer>().Endpoint(e => e.Name = ConstStringForQueues.DeleteCompany);
                x.AddConsumer<GetCompanyWithCountUsersConsumer>().Endpoint(e => e.Name = ConstStringForQueues.GetCompanyWithCountUsers);

            });

            serviceCollection.AddMassTransitHostedService();

            return serviceCollection;
        }
    }
}
