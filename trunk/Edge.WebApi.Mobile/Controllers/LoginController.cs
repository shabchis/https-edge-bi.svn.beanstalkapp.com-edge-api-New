using System;
using System.Web.Http;
using Edge.Api.Handlers;
using Edge.Core.Utilities;
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
				Log.Write("Mobile API", String.Format("User {0} logged in to Mobile Application", response.UserID), LogMessageType.Debug);
				return new ExtendedSessionResponseData {UserID = response.UserID, Session = response.Session};
			}
			catch (Exception ex)
			{
				Log.Write("Mobile API", String.Format("Login failed for user email '{0}', ex: {1}", email, ex.Message), LogMessageType.Error);
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