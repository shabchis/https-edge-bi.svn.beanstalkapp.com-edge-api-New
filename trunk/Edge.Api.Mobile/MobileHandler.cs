using System;
using System.Collections.Generic;
using System.Linq;
using Edge.Api.Handlers.Template;
using Edge.Api.Mobile.Performance;
using Edge.Api.Mobile.Settings;
using Edge.Objects;
using System.Globalization;
using Edge.Objects.Performance;

namespace Edge.Api.Mobile.Handlers
{
	public class MobileHandler : TemplateHandler
	{
		#region Performance
		[UriMapping(Method = "GET", Template = "performance/accounts/{accountID}/from={from}/to={to}/theme={theme}/country={country}")]
		public DailyPerformanceResponse GetPerformance(int accountID, string from, string to, string theme, string country)
		{
			IPerformanceManager manager = new MockPerformanceManager();
			return manager.GetPerformance(accountID, from, to, theme, country);
		}

		[UriMapping(Method = "GET", Template = "performanceroas/accounts/{accountID}/from={from}/to={to}/theme={theme}/country={country}")]
		public RoasPerformanceResponse GetRoasPerformance(int accountID, string from, string to, string theme, string country)
		{
			IPerformanceManager manager = new MockPerformanceManager();
			return manager.GetRoasPerformance(accountID, from, to, theme, country);
		}

		[UriMapping(Method = "GET", Template = "performancecampaign/accounts/{accountID}/from={from}/to={to}/theme={theme}/country={country}")]
		public CampaignPerformanceResponse GetCampaignPerformance(int accountID, string from, string to, string theme, string country)
		{
			IPerformanceManager manager = new MockPerformanceManager();
			return manager.GetCampaignPerformance(accountID, from, to, theme, country);
		} 
		#endregion

		#region Settings
		[UriMapping(Method = "GET", Template = "settings/themes?account={accountID}")]
		public List<SegmentInfo> GetThemes(int accountID)
		{
			ISettingsManager manager = new MockSettingsManager();
			return manager.GetSegmentInfo(accountID, SegmentType.Theme);
		}

		[UriMapping(Method = "GET", Template = "settings/countries?account={accountID}")]
		public List<SegmentInfo> GetCountries(int accountID)
		{
			ISettingsManager manager = new MockSettingsManager();
			return manager.GetSegmentInfo(accountID, SegmentType.Country);
		}

		[UriMapping(Method = "GET", Template = "settings/accounts?user={userID}")]
		public List<SegmentInfo> GetAccounts(int userID)
		{
			ISettingsManager manager = new MockSettingsManager();
			return manager.GetAccounts(userID);
		} 
		#endregion

		#region Overrides
		public override bool ShouldValidateSession
		{
			// TODO: should return true
			get { return false; }
		} 
		#endregion
	}
}
