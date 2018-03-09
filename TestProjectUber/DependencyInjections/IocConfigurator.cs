using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestProjectUber.DbLayer;
using TestProjectUber.Services;
using Unity;
using Unity.Injection;

namespace TestProjectUber.DependencyInjections
{
    public static class IocConfigurator
    {
        public static void ConfigureIocUnityContainer()
        {
            IUnityContainer container = new UnityContainer();
            registerServices(container);
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static void registerServices(IUnityContainer container)
        {
            container.RegisterType<ICoursesContext, CoursesContext>();
        }
    }
}