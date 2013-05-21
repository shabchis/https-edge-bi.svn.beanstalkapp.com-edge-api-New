using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Edge.WebApi.Mobile.App_Start;

namespace Edge.WebApi.Mobile
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class WebApiApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			//BundleConfig.RegisterBundles(BundleTable.Bundles);
				
			//Add support for jsonp
			var config = GlobalConfiguration.Configuration;
			config.Formatters.Insert(0,new JsonpMediaTypeFormatter());
		}
	}
}