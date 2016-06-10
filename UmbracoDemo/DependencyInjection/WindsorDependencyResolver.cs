using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;
using IDependencyResolver = System.Web.Mvc.IDependencyResolver;

namespace UmbracoDemo.DependencyInjection
{
    internal class WindsorDependencyResolver : IDependencyResolver,
        System.Web.Http.Dependencies.IDependencyResolver
    {
        private readonly IWindsorContainer _container;
        private readonly IDependencyResolver _current;

        public WindsorDependencyResolver(IWindsorContainer container, IDependencyResolver current)
        {
            _container = container;
            _current = current;
        }

        public object GetService(Type serviceType)
        {
            try
            {
                var currentService = _current.GetService(serviceType);
                if (currentService != null)
                    return currentService;

                return _container.Resolve(serviceType);
            }
            catch (Exception) { }
            return null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                var currentServiceList = _current.GetServices(serviceType);
                if (currentServiceList != null)
                    return currentServiceList;

                return _container.ResolveAll(serviceType).Cast<object>();
            }
            catch (Exception) { }

            return Enumerable.Empty<object>();
        }

        public IDependencyScope BeginScope()
        {
            return new WindsorWebApiDependencyScope(_container);
        }

        public void Dispose()
        {
        }
    }


    public class WindsorWebApiDependencyScope : IDependencyScope
    {
        private readonly IDisposable disposable;

        private readonly IWindsorContainer _windsorContainer;

        public WindsorWebApiDependencyScope(IWindsorContainer windsorContainer)
        {
            _windsorContainer = windsorContainer;
            disposable = _windsorContainer.BeginScope();
        }

        public object GetService(Type serviceType)
        {
            return _windsorContainer.Resolve(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _windsorContainer.ResolveAll(serviceType).Cast<object>();
        }

        public void Dispose()
        {
            disposable.Dispose();
        }
    }
}