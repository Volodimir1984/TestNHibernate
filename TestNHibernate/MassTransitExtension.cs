using CompanyService.Consumers;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServicesInterfaces.Companies;
using System;
using CompanyService;
using ConstData;
using GreenPipes;
using ServicesInterfaces;
using ServicesInterfaces.Users;
using UserService.Consumers;

namespace TestNHibernate
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

                x.AddConsumer<ExceptionConsumer>();

                #region Company

                x.AddConsumer<GetCompanyConsumer>().Endpoint(e => e.Name = ConstStringForQueues.GetCompany);
                x.AddConsumer<GetCompaniesConsumer>().Endpoint(e => e.Name = ConstStringForQueues.GetCompanies);
                x.AddConsumer<UpdateCompanyConsumer>().Endpoint(e => e.Name = ConstStringForQueues.UpdateCompany);
                x.AddConsumer<CreateCompanyConsumer>().Endpoint(e => e.Name = ConstStringForQueues.CreateCompany);
                x.AddConsumer<DeleteCompanyConsumer>().Endpoint(e => e.Name = ConstStringForQueues.DeleteCompany);
                x.AddConsumer<GetCompanyWithCountUsersConsumer>().Endpoint(e => e.Name = ConstStringForQueues.GetCompanyWithCountUsers);

                x.AddRequestClient<ICompanyPrimaryData>(new Uri($"queue:{ConstStringForQueues.GetCompany}"));
                x.AddRequestClient<ICompaniesData>(new Uri($"queue:{ConstStringForQueues.GetCompanies}"));
                x.AddRequestClient<ICountUsersInCompany>(new Uri($"queue:{ConstStringForQueues.GetCompanyWithCountUsers}"));

                #endregion

                #region User

                x.AddConsumer<GetUsersConsumer>().Endpoint(e => e.Name = ConstStringForQueues.GetUsers);
                x.AddConsumer<GetUserConsumer>().Endpoint(e => e.Name = ConstStringForQueues.GetUser);
                x.AddConsumer<UpdateUserConsumer>().Endpoint(e => e.Name = ConstStringForQueues.UpdateUser);
                x.AddConsumer<CreateUserConsumer>().Endpoint(e => e.Name = ConstStringForQueues.CreateUser);
                x.AddConsumer<DeleteUserConsumer>().Endpoint(e => e.Name = ConstStringForQueues.DeleteUser);

                x.AddRequestClient<IUsersData>(new Uri($"queue:{ConstStringForQueues.GetUsers}"));
                x.AddRequestClient<IUserPrimaryData>(new Uri($"queue:{ConstStringForQueues.GetUser}"));

                #endregion

            });

            serviceCollection.AddMassTransitHostedService();

            return serviceCollection;
        }
    }
}
