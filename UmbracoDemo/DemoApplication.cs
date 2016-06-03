using System;
using System.ComponentModel;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using UmbracoDemo.DependencyInjection;
using UmbracoDemo.Services;
using Component = Castle.MicroKernel.Registration.Component;

namespace UmbracoDemo
{
    public class DemoApplication : UmbracoApplication
    {
        private Lazy<IWindsorContainer> Container = new Lazy<IWindsorContainer>(Initialize);
             
        protected override void OnApplicationStarted(object sender, EventArgs e)
        {
            base.OnApplicationStarted(sender, e);

            var resolver = new WindsorDependencyResolver(Container.Value, DependencyResolver.Current);
            DependencyResolver.SetResolver(resolver);
        }

        public static IWindsorContainer Initialize()
        {
            var container = new WindsorContainer();

            container.Register(Component
                .For<IEmailService>()
                .ImplementedBy<EmailService>()
                .LifestyleSingleton());

            container.Install(FromAssembly.This())
                .Register((IRegistration)Classes.FromThisAssembly()
                    .BasedOn<SurfaceController>()
                    .LifestyleTransient());

            return container;
        }
    }
}
