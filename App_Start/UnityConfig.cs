using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using weatherApp.CacheProvider;
using weatherApp.DAL;

namespace weatherApp
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            //register dataaccess layer,apihelper and httpclient for dependency injection
            container.RegisterType<IDataAccessLayer, DataAccessLayer>();                       
            container.RegisterType<IApiHelper, ApiHelper>();
            container.RegisterType<System.Net.Http.HttpClient>();
            container.RegisterType<ICacheService, CacheService>();





            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}