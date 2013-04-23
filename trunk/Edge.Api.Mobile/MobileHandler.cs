﻿using System;
using System.Collections.Generic;
using Edge.Api.Handlers.Template;
using Edge.Api.Mobile.Performance;
using Edge.Api.Mobile.Settings;
using Edge.Objects;
using System.Globalization;

namespace Edge.Api.Mobile.Handlers
{
	public class MobileHandler : TemplateHandler
	{
		#region Performance
		[UriMapping(Method = "GET", Template = "performance/accounts/{accountID}/from={from}/to={to}")]
		public List<Performance7Days> GetPerformance(int accountID, string from, string to)
		{
			IPerformanceManager manager = new PerformanceManager();
			DateTime fromDate;
			DateTime toDate;
			if (DateTime.TryParseExact(from, "ddMMyyyy", null, DateTimeStyles.None, out fromDate) &&
				DateTime.TryParseExact(to, "ddMMyyyy", null, DateTimeStyles.None, out toDate))
			{
				return manager.GetPerformance(accountID, fromDate, toDate);
			}
			return null;
		}

		[UriMapping(Method = "GET", Template = "performanceroas/accounts/{accountID}/from={from}/to={to}/deposit={depositFieldName}/depositor={depositorFieldName}")]
		public List<RoasPerformance> GetRoasPerformance(int accountID, string from, string to, string depositFieldName, string depositorFieldName)
		{
			IPerformanceManager manager = new MockPerformanceManager();
			DateTime fromDate;
			DateTime toDate;

			if (DateTime.TryParseExact(from, "ddMMyyyy", null, DateTimeStyles.None, out fromDate) &&
				DateTime.TryParseExact(to, "ddMMyyyy", null, DateTimeStyles.None, out toDate))
			{
				return manager.GetRoasPerformance(accountID, DateTime.Now, DateTime.Now, depositFieldName, depositorFieldName);
			}
			return null;
		}

		[UriMapping(Method = "GET", Template = "performancecampaign/accounts/{accountID}/from={from}/to={to}/theme={themeId}/country={countryId}")]
		public List<CampaignPerformance> GetCampaignPerformance(int accountID, string from, string to, int themeId, string countryId)
		{
			IPerformanceManager manager = new MockPerformanceManager();
			DateTime fromDate;
			DateTime toDate;

			if (DateTime.TryParseExact(from, "ddMMyyyy", null, DateTimeStyles.None, out fromDate) &&
				DateTime.TryParseExact(to, "ddMMyyyy", null, DateTimeStyles.None, out toDate))
			{
				return manager.GetCampaignPerformance(accountID, DateTime.Now, DateTime.Now, themeId, countryId);
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
