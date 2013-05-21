using System.Collections.Generic;
using System.Web.Http;
using Edge.Api.Mobile.Settings;
using Edge.Core.Configuration;
using Edge.Objects;

namespace Edge.WebApi.Mobile.Controllers
{
    public class SettingsController : BaseController
    {
		[ActionName("Themes")]
		public List<SegmentInfo> GetThemes(string sessionId, int accountID)
		{
			ValidateSession(sessionId);

			var manager = GetManager();
			return manager.GetSegmentInfo(accountID, SegmentType.Theme);
		}

		[ActionName("Countries")]
		public List<SegmentInfo> GetCountries(string sessionId, int accountID)
		{
			ValidateSession(sessionId);

			var manager = GetManager(); 
			return manager.GetSegmentInfo(accountID, SegmentType.Country);
		}

		[ActionName("Accounts")]
		public List<SegmentInfo> GetAccounts(string sessionId)
		{
			var userId = ValidateSession(sessionId);

			var manager = GetManager();
			return manager.GetAccounts(userId);
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
