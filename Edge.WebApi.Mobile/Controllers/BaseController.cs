using System.Web;
using System.Web.Http;
using Edge.Api.Handlers.Template;
using Edge.Core.Configuration;
using Edge.Objects;

namespace Edge.WebApi.Mobile.Controllers
{
	public abstract class BaseController : ApiController
	{
		protected static int ValidateSession(string sessionId)
		{
			if (AppSettings.Get("MobileApi", "ValidateSession", false) == "false")
				return 178;

			int userId;
			ApplicationType appType;

			if (!TemplateHandler.IsSessionValid(sessionId, out userId, out appType))
				throw new HttpException("Invalid session");

			if (appType != ApplicationType.Mobile)
				throw new HttpException("Session is not valid for Mobile application");

			return userId;
		}
	}
}