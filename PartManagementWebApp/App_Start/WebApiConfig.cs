using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using PartManagementWebApp.Models;
using SharpRepository.Ioc.Windsor;

namespace PartManagementWebApp
{
    public class WindsorCompositionRoot : IHttpControllerActivator
    {
        private readonly IWindsorContainer _container;

        public WindsorCompositionRoot(IWindsorContainer container)
        {
            _container = container;
        }

        public IHttpController Create(
            HttpRequestMessage request,
            HttpControllerDescriptor controllerDescriptor,
            Type controllerType)
        {
            var controller =
                (IHttpController)_container.Resolve(controllerType);

            request.RegisterForDispose(
                new Release(
                    () => _container.Release(controller)));

            return controller;
        }

        private sealed class Release : IDisposable
        {
            private readonly Action _release;

            public Release(Action release)
            {
                _release = release;
            }

            public void Dispose()
            {
                _release();
            }
        }
    }
    public class RepositoriesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.RegisterSharpRepository();
        }
    }
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config, IWindsorContainer container)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Use camel case for JSON data.
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Part>("Parts");
            builder.EntitySet<ApplicationUser>("Users");
            config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
            RegisterControllerActivator(container);
        }
        private static void RegisterControllerActivator(IWindsorContainer container)
        {
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator),
                new WindsorCompositionRoot(container));
        }
    }
}
