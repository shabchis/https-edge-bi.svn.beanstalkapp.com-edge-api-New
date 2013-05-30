using System;
using System.Collections.Generic;
using System.Web.Http;
using Edge.Api.Mobile.Settings;
using Edge.Core.Configuration;
using Edge.Core.Utilities;
using Edge.Objects;

namespace Edge.WebApi.Mobile.Controllers
{
    public class SettingsController : BaseController
    {
		[ActionName("Themes")]
		public SettingsResponse GetThemes(string sessionId, int accountId)
		{
			try
			{
				ValidateSession(sessionId);

				Log.Write("Mobile API", String.Format("Themes list request for account={0}", accountId), LogMessageType.Debug);
				var manager = GetManager();
				return manager.GetSegmentInfo(accountId, SegmentType.Theme);
			}
			catch (Exception ex)
			{
				Log.Write("Mobile API", String.Format("Get themes failed for account {0}, ex: {1}", accountId,  ex.Message), LogMessageType.Error);
				return new SettingsResponse
					{
						HasError = true,
						ErrorMsg = ex.Message,
						DisplayError = (ex is MobileApiException) ? (ex as MobileApiException).DisplayMessage : String.Format("Failed to load Themes for account {0}", accountId)
					};
			}
		}

		[ActionName("Countries")]
		public SettingsResponse GetCountries(string sessionId, int accountId)
		{
			try
			{
				ValidateSession(sessionId);

				Log.Write("Mobile API", String.Format("Country list request for account={0}", accountId), LogMessageType.Debug);
				var manager = GetManager(); 
				return manager.GetSegmentInfo(accountId, SegmentType.Country);
			}
			catch (Exception ex)
			{
				Log.Write("Mobile API", String.Format("Get countries failed for account {0}, ex: {1}", accountId,  ex.Message), LogMessageType.Error);
				return new SettingsResponse
					{
						HasError = true,
						ErrorMsg = ex.Message,
						DisplayError = (ex is MobileApiException) ? (ex as MobileApiException).DisplayMessage : String.Format("Failed to load Countries for account {0}", accountId)
					};
			}
		}

		[ActionName("Accounts")]
		public SettingsResponse GetAccounts(string sessionId)
		{
			try
			{
				var userId = ValidateSession(sessionId);

				Log.Write("Mobile API", String.Format("Account list request for user={0}", userId), LogMessageType.Debug);
				var manager = GetManager();
				return manager.GetAccounts(userId);
			}
			catch (Exception ex)
			{
				Log.Write("Mobile API", String.Format("Get accounts failed, ex: {0}", ex.Message), LogMessageType.Error);
				return new SettingsResponse
					{
						HasError = true,
						ErrorMsg = ex.Message,
						DisplayError = (ex is MobileApiException) ? (ex as MobileApiException).DisplayMessage : "Failed to load Accounts"
					};
			}
		}

		#region Private Methods
		private ISettingsManager GetManager()
		{
			if (AppSettings.Get("MobileApi", "MockData", false) == "true")
			{
				return new MockSettingsManager();
			}
			return new SettingsManager();
		} 
		#endregion
    }
}
