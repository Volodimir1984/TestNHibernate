using ConstData;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using UsersService.Consumers;

namespace UsersService
{
    public static class MassTransitExtension
    {
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

                x.AddConsumer<GetUsersConsumer>().Endpoint(e => e.Name = ConstStringForQueues.GetUsers);
                x.AddConsumer<GetUserConsumer>().Endpoint(e => e.Name = ConstStringForQueues.GetUser);
                x.AddConsumer<UpdateUserConsumer>().Endpoint(e => e.Name = ConstStringForQueues.UpdateUser);
                x.AddConsumer<CreateUserConsumer>().Endpoint(e => e.Name = ConstStringForQueues.CreateUser);
                x.AddConsumer<DeleteUserConsumer>().Endpoint(e => e.Name = ConstStringForQueues.DeleteUser);

            });

            serviceCollection.AddMassTransitHostedService();

            return serviceCollection;
        }
    }
}
