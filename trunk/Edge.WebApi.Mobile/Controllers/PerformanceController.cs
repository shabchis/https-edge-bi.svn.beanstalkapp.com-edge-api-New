using System;
using System.Collections.Generic;
using System.Web.Http;
using Edge.Api.Mobile.Performance;
using Edge.Core.Configuration;
using Edge.Objects;
using Edge.Objects.Performance;

namespace Edge.WebApi.Mobile.Controllers
{
	public class PerformanceController : BaseController
	{
		[ActionName("DailyPerformance")]
		public DailyPerformanceResponse GetPerformance(string sessionId, int accountId, string from, string to, string theme="", string country="")
		{
			try
			{
				ValidateSession(sessionId);

				var manager = GetManager();
				return manager.GetPerformance(accountId, from, to, theme, country);
			}
			catch (Exception ex)
			{
				return new DailyPerformanceResponse
				{
					HasError = true,
					ErrorMsg = ex.Message,
					DisplayError = (ex is MobileApiException) ? (ex as MobileApiException).DisplayMessage : null
				};
			}

		}

		[ActionName("RoasPerformance")]
		public RoasPerformanceResponse GetRoasPerformance(string sessionId, int accountId, string from, string to, string theme = "", string country = "")
		{
			try
			{
				ValidateSession(sessionId);

				var manager = GetManager();
				return manager.GetRoasPerformance(accountId, from, to, theme, country);
			}
			catch (Exception ex)
			{
				return new RoasPerformanceResponse
				{
					HasError = true,
					ErrorMsg = ex.Message,
					DisplayError = (ex is MobileApiException) ? (ex as MobileApiException).DisplayMessage : null
				};
			}
		}

		[ActionName("CampaignPerformance")]
		public CampaignPerformanceResponse GetCampaignPerformance(string sessionId, int accountId, string from, string to, string theme = "", string country = "")
		{
			try
			{
				ValidateSession(sessionId);

				var manager = GetManager();
				return manager.GetCampaignPerformance(accountId, from, to, theme, country);
			}
			catch (Exception ex)
			{
				return new CampaignPerformanceResponse
				{
					HasError = true,
					ErrorMsg = ex.Message,
					DisplayError = (ex is MobileApiException) ? (ex as MobileApiException).DisplayMessage : null
				};
			}
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