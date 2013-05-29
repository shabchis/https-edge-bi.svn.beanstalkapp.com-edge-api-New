using System;
using System.Web.Http;
using Edge.Api.Handlers;
using Edge.Objects;

namespace Edge.WebApi.Mobile.Controllers
{
	public class LoginController : ApiController
	{
		//public SessionResponseData LogIn(SessionRequestData sessionData)
		public ExtendedSessionResponseData GetLoginInfo(string email = "", string password = "", 
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
			try
			{
				var response = handler.LogIn(sessionData);
				return new ExtendedSessionResponseData {UserID = response.UserID, Session = response.Session};
			}
			catch (Exception ex)
			{
				return new ExtendedSessionResponseData 
				{
					HasError = true, 
					ErrorMsg = ex.Message,
					DisplayError = (ex is MobileApiException) ? (ex as MobileApiException).DisplayMessage : null
				};
			}
		}
	}
}