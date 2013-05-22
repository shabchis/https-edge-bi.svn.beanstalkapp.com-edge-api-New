using System.Web.Http;
using Edge.Api.Handlers;
using Edge.Objects;

namespace Edge.WebApi.Mobile.Controllers
{
	public class LoginController : ApiController
	{
		//public SessionResponseData LogIn(SessionRequestData sessionData)
		public SessionResponseData GetLoginInfo(string email = "", string password = "", 
										 int userId=0, string sessionId = "", 
										 OperationTypeEnum operationType = OperationTypeEnum.New, 
										 ApplicationType applicationType = ApplicationType.Mobile )
		{
			var sessionData = new SessionRequestData
				{
					Email = email,
					Password = password,
					UserID = userId,
					Session = sessionId,
					OperationType = operationType,
					ApplicationType = applicationType
				};

			var handler = new CoreHandler();
			return handler.LogIn(sessionData);
		}
	}
}