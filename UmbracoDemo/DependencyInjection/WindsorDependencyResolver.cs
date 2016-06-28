using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Castle.Windsor;

namespace UmbracoDemo.DependencyInjection
{
    public class WindsorDependencyResolver :
        System.Web.Mvc.IDependencyResolver,
        System.Web.Http.Dependencies.IDependencyResolver
    {
        private readonly IWindsorContainer _container;
        private readonly System.Web.Mvc.IDependencyResolver _mvcDependencyResolver;
        private readonly IDependencyResolver _webApiDependencyResolver;

        private readonly IDependencyScope _lifeTimeScope;

        public WindsorDependencyResolver(
            IWindsorContainer container,
            System.Web.Mvc.IDependencyResolver mvcDependencyResolver,
            System.Web.Http.Dependencies.IDependencyResolver webApiDependencyResolver)
        {
            _container = container;
            _mvcDependencyResolver = mvcDependencyResolver;
            _webApiDependencyResolver = webApiDependencyResolver;

            _lifeTimeScope = new WindsorWebApiDependencyScope(
                _container, 
                _mvcDependencyResolver, 
                _webApiDependencyResolver);
        }

        public object GetService(Type serviceType)
        {
            return _lifeTimeScope.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _lifeTimeScope.GetServices(serviceType);
        }

        public System.Web.Http.Dependencies.IDependencyScope BeginScope()
        {
            return new WindsorWebApiDependencyScope(_container, 
                _mvcDependencyResolver, 
                _webApiDependencyResolver);
        }

        public void Dispose()
        {
        }
    }
}