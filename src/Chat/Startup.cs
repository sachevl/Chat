using Microsoft.Owin;

[assembly: OwinStartup(typeof(Chat.Startup))]

namespace Chat
{
    using System.Web.Http;

    using Autofac.Integration.SignalR;
    using Autofac.Integration.WebApi;

    using Microsoft.AspNet.SignalR;

    using Owin;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration httpConfig = new HttpConfiguration();
            httpConfig.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );

            var container = AutofacContainerFactory.Create();
            httpConfig.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            app.UseAutofacMiddleware(container);

            app.UseWebApi(httpConfig);

            var hubConfig = new HubConfiguration { Resolver = new AutofacDependencyResolver(container) };
            app.MapSignalR(hubConfig);
        }
    }
}