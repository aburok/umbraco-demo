using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;

namespace UmbracoDemo.DependencyInjection
{
    public class WindsorWebApiDependencyScope :
        System.Web.Http.Dependencies.IDependencyScope
    {
        private readonly IWindsorContainer _windsorContainer;

        private readonly System.Web.Mvc.IDependencyResolver _mvcDependencyResolver;
        private readonly IDependencyResolver _webApiDependencyResolver;
        private readonly IDisposable _disposable;


        public WindsorWebApiDependencyScope(
            IWindsorContainer container,
            System.Web.Mvc.IDependencyResolver mvcDependencyResolver,
            System.Web.Http.Dependencies.IDependencyResolver webApiDependencyResolver)
        {
            _mvcDependencyResolver = mvcDependencyResolver;
            _webApiDependencyResolver = webApiDependencyResolver;
            _windsorContainer = container;

            _disposable = _windsorContainer.BeginScope();
        }

        public object GetService(Type serviceType)
        {
            try
            {
                var serviceFromWindsor = _windsorContainer.Resolve(serviceType);
                if (serviceFromWindsor != null)
                    return serviceFromWindsor;
            }
            catch (Exception) { /* ignored */ }

            try
            {
                var serviceFromMvc = _mvcDependencyResolver.GetService(serviceType);
                if (serviceFromMvc != null)
                    return serviceFromMvc;
            }
            catch (Exception) { /*ignored*/ }

            try
            {
                var serviceFromWebApi = _webApiDependencyResolver.GetService(serviceType);
                if (serviceFromWebApi != null)
                    return serviceFromWebApi;
            }
            catch (Exception) { /* ignored */ }

            return null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                var servicesFromWindsor = _windsorContainer.ResolveAll(serviceType)
                    .Cast<object>();
                if (servicesFromWindsor != null && servicesFromWindsor.Any())
                    return servicesFromWindsor;
            }
            catch (Exception) { /* ignored */ }

            try
            {
                var currentServiceList = _mvcDependencyResolver.GetServices(serviceType);
                if (currentServiceList != null && currentServiceList.Any())
                    return currentServiceList;
            }
            catch (Exception) { /* ignored */ }

            try
            {
                var webApiServices = _webApiDependencyResolver.GetServices(serviceType);
                if (webApiServices != null && webApiServices.Any())
                    return webApiServices;
            }
            catch (Exception) { /* ignored */ }


            return Enumerable.Empty<object>();
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}