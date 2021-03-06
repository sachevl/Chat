﻿namespace Chat
{
    using System.Reflection;

    using Autofac;
    using Autofac.Integration.SignalR;
    using Autofac.Integration.WebApi;

    using Chat.Core.Repositories;

    public class AutofacContainerFactory
    {
        public static IContainer Create()
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterHubs(Assembly.GetExecutingAssembly());

            builder.RegisterType<UserInMemoryRepository>().As<IUserRepository>().SingleInstance();
            builder.RegisterType<AzureTableStorageMessageRepository>().As<IMessageRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CloudTableFactory>().As<ICloudTableFactory>().SingleInstance();

            return builder.Build();
        }
    }
}