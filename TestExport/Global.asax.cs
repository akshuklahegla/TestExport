using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;
using TestExport.Api_Start;
using NSwag.AspNet.Owin;

namespace TestExport
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            RouteTable.Routes.MapOwinPath("swagger", app =>
            {
                app.UseSwaggerUi3(typeof(Global).Assembly, settings =>
                {
                    settings.MiddlewareBasePath = "/swagger";
                    settings.GeneratorSettings.DefaultUrlTemplate = "api/{controller}/{id}";  //this is the default one
                    //settings.GeneratorSettings.DefaultUrlTemplate = "api/{controller}/{action}/{id}";
                });
            });
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            
        }

    }
}
