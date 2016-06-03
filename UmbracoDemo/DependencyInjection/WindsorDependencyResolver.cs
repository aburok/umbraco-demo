using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.Windsor;

namespace UmbracoDemo.DependencyInjection
{
    internal class WindsorDependencyResolver : IDependencyResolver
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
    }
}