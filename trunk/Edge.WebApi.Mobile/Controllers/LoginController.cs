using System.Web.Http;
using Edge.Api.Handlers;
using Edge.Objects;

namespace Edge.WebApi.Mobile.Controllers
{
	public class LoginController : ApiController
	{
		public SessionResponseData LogIn(SessionRequestData sessionData)
		{
			var handler = new CoreHandler();
			return handler.LogIn(sessionData);
		}
	}
}