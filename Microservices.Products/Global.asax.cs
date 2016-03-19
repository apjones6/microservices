using Newtonsoft.Json.Serialization;
using System.Web;
using System.Web.Http;

namespace Microservices.Products
{
	public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
		{
			GlobalConfiguration.Configure(x =>
			{
				x.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
				x.MapHttpAttributeRoutes();
			});
        }
    }
}
