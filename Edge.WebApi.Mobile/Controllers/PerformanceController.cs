using System.Collections.Generic;
using System.Web.Http;
using Edge.Api.Mobile.Performance;
using Edge.Core.Configuration;
using Edge.Objects.Performance;

namespace Edge.WebApi.Mobile.Controllers
{
	public class PerformanceController : BaseController
	{
		[ActionName("DailyPerformance")]
		public DailyPerformanceResponse GetPerformance(string sessionId, int accountID, string from, string to, string theme="", string country="")
		{
			ValidateSession(sessionId);

			var manager = GetManager();
			return manager.GetPerformance(accountID, from, to, theme, country);
		}

		[ActionName("RoasPerformance")]
		public List<RoasPerformance> GetRoasPerformance(string sessionId, int accountID, string from, string to, string theme = "", string country = "")
		{
			ValidateSession(sessionId);

			var manager = GetManager();
			return manager.GetRoasPerformance(accountID, from, to, theme, country);
		}

		[ActionName("CampaignPerformance")]
		public CampaignPerformanceResponse GetCampaignPerformance(string sessionId, int accountID, string from, string to, string theme = "", string country = "")
		{
			ValidateSession(sessionId);

			var manager = GetManager();
			return manager.GetCampaignPerformance(accountID, from, to, theme, country);
		}

		#region Private Methods
		private IPerformanceManager GetManager()
		{
			if (AppSettings.Get("MobileApi", "MockData", false) == "true")
			{
				return new MockPerformanceManager();
			}
			return new PerformanceManager();
		}
		#endregion

		
	}
}