using System;
using System.Web.Http;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using UmbracoDemo.DependencyInjection;
using UmbracoDemo.Services;
using Component = Castle.MicroKernel.Registration.Component;

namespace UmbracoDemo
{
    public class DemoApplication : UmbracoApplication
    {
        private readonly Lazy<IWindsorContainer> _container = new Lazy<IWindsorContainer>(Initialize);

        protected override void OnApplicationStarted(object sender, EventArgs e)
        {
            base.OnApplicationStarted(sender, e);

            var resolver = new WindsorDependencyResolver(
                _container.Value, 
                DependencyResolver.Current, 
                GlobalConfiguration.Configuration.DependencyResolver);
            DependencyResolver.SetResolver(resolver);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }

        public static IWindsorContainer Initialize()
        {
            var container = new WindsorContainer();

            RegisterStandardClasses(container);

            container.Register(Component
                .For<IEmailService>()
                .ImplementedBy<EmailService>()
                .LifestyleSingleton());

            container.Register(Component
                .For<ICityRepository>()
                .ImplementedBy<CityRepository>()
                .LifestyleSingleton());

            return container;
        }

        private static void RegisterStandardClasses(WindsorContainer container)
        {
            // Register all controllers from current assembly 
            RegisterFromAssembly<DemoApplication, ApiController>(container);
            RegisterFromAssembly<DemoApplication, Controller>(container);
            RegisterFromAssembly<DemoApplication, SurfaceController>(container);
            RegisterFromAssembly<DemoApplication, RenderMvcController>(container);
        }

        private static void RegisterFromAssembly<TAssemblyClass, TService>(IWindsorContainer container)
        {
            container
                .Register(Classes.FromAssemblyContaining<TAssemblyClass>()
                    .BasedOn<TService>()
                    .LifestyleTransient());
        }
    }
}
