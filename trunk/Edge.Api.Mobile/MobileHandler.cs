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
			IPerformanceManager manager = new PerformanceManager();
			DateTime fromDate;
			DateTime toDate;
			var themes = theme.Split(',').ToList().ConvertAll(int.Parse);
			var countries = country.Split(',').ToList().ConvertAll(int.Parse);
			themes.RemoveAll(x => x < 1);
			countries.RemoveAll(x => x < 1);

			if (DateTime.TryParseExact(from, "ddMMyyyy", null, DateTimeStyles.None, out fromDate) &&
				DateTime.TryParseExact(to, "ddMMyyyy", null, DateTimeStyles.None, out toDate))
			{
				return manager.GetPerformance(accountID, fromDate, toDate, themes, countries);
			}
			return null;
		}

		[UriMapping(Method = "GET", Template = "performanceroas/accounts/{accountID}/from={from}/to={to}/theme={theme}/country={country}")]
		public List<RoasPerformance> GetRoasPerformance(int accountID, string from, string to, string theme, string country)
		{
			IPerformanceManager manager = new PerformanceManager();
			DateTime fromDate;
			DateTime toDate;
			var themes = theme.Split(',').ToList().ConvertAll(int.Parse);
			var countries = country.Split(',').ToList().ConvertAll(int.Parse);
			themes.RemoveAll(x => x < 1);
			countries.RemoveAll(x => x < 1);

			if (DateTime.TryParseExact(from, "ddMMyyyy", null, DateTimeStyles.None, out fromDate) &&
				DateTime.TryParseExact(to, "ddMMyyyy", null, DateTimeStyles.None, out toDate))
			{
				return manager.GetRoasPerformance(accountID, fromDate, toDate, themes, countries);
			}
			return null;
		}

		[UriMapping(Method = "GET", Template = "performancecampaign/accounts/{accountID}/from={from}/to={to}/theme={theme}/country={country}")]
		public CampaignPerformanceResponse GetCampaignPerformance(int accountID, string from, string to, string theme, string country)
		{
			IPerformanceManager manager = new PerformanceManager();
			DateTime fromDate;
			DateTime toDate;
			var themes = theme.Split(',').ToList().ConvertAll(int.Parse);
			var countries = country.Split(',').ToList().ConvertAll(int.Parse);
			themes.RemoveAll(x => x < 1);
			countries.RemoveAll(x => x < 1);

			if (DateTime.TryParseExact(from, "ddMMyyyy", null, DateTimeStyles.None, out fromDate) &&
				DateTime.TryParseExact(to, "ddMMyyyy", null, DateTimeStyles.None, out toDate))
			{
				return manager.GetCampaignPerformance(accountID, fromDate, toDate, themes, countries);
			}
			return null;
		} 
		#endregion

		#region Settings
		[UriMapping(Method = "GET", Template = "settings/themes?account={accountID}")]
		public List<SegmentInfo> GetThemes(int accountID)
		{
			ISettingsManager manager = new SettingsManager();
			return manager.GetSegmentInfo(accountID, SegmentType.Theme);
		}

		[UriMapping(Method = "GET", Template = "settings/countries?account={accountID}")]
		public List<SegmentInfo> GetCountries(int accountID)
		{
			ISettingsManager manager = new SettingsManager();
			return manager.GetSegmentInfo(accountID, SegmentType.Country);
		}

		[UriMapping(Method = "GET", Template = "settings/accounts?user={userID}")]
		public List<SegmentInfo> GetAccounts(int userID)
		{
			ISettingsManager manager = new SettingsManager();
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
