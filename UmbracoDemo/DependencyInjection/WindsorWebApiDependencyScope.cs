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
                var currentService = _mvcDependencyResolver.GetService(serviceType);
                if (currentService != null)
                    return currentService;
            }
            catch (Exception) { /*ignored*/ }

            try
            {
                var webApiService = _webApiDependencyResolver.GetService(serviceType);
                if (webApiService != null)
                    return webApiService;
            }
            catch (Exception) { /* ignored */ }

            return _windsorContainer.Resolve(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                var currentServiceList = _mvcDependencyResolver.GetServices(serviceType);
                if (currentServiceList != null)
                    return currentServiceList;
            }
            catch (Exception) { /* ignored */ }

            try
            {
                var webApiServices = _webApiDependencyResolver.GetServices(serviceType);
                if (webApiServices != null)
                    return webApiServices;
            }
            catch (Exception) { /* ignored */ }

            try
            { 
                return _windsorContainer.ResolveAll(serviceType).Cast<object>();
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