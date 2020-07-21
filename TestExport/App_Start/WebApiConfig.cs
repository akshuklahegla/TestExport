using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using Newtonsoft.Json.Serialization;
//using Swashbuckle.Application;

namespace TestExport.Api_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

           
            // Web API routes
             config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

        }
    }
}
