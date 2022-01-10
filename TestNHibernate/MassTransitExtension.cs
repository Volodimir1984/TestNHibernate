using ConstData;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServicesInterfaces.Companies;
using ServicesInterfaces.Users;
using System;

namespace TestNHibernate
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


                #region Company

                x.AddRequestClient<ICompanyPrimaryData>(new Uri($"queue:{ConstStringForQueues.GetCompany}"));
                x.AddRequestClient<ICompaniesData>(new Uri($"queue:{ConstStringForQueues.GetCompanies}"));
                x.AddRequestClient<ICountUsersInCompany>(new Uri($"queue:{ConstStringForQueues.GetCompanyWithCountUsers}"));
                x.AddRequestClient<ICompanyUpdate>(new Uri($"queue:{ConstStringForQueues.UpdateCompany}"));
                x.AddRequestClient<ICompanyCreate>(new Uri($"queue:{ConstStringForQueues.CreateCompany}"));
                x.AddRequestClient<IDeleteCompany>(new Uri($"queue:{ConstStringForQueues.DeleteCompany}"));

                #endregion

                #region User

                x.AddRequestClient<IUsersData>(new Uri($"queue:{ConstStringForQueues.GetUsers}"));
                x.AddRequestClient<IUserPrimaryData>(new Uri($"queue:{ConstStringForQueues.GetUser}"));
                x.AddRequestClient<IUserUpdate>(new Uri($"queue:{ConstStringForQueues.UpdateUser}"));
                x.AddRequestClient<IUserCreate>(new Uri($"queue:{ConstStringForQueues.CreateUser}"));
                x.AddRequestClient<IUserDelete>(new Uri($"queue:{ConstStringForQueues.DeleteUser}"));

                #endregion

            });

            serviceCollection.AddMassTransitHostedService();

            return serviceCollection;
        }

    }
}
