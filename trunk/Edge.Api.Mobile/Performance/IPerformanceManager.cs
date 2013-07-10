using System;
using System.Collections.Generic;
using Edge.Objects.Performance;

namespace Edge.Api.Mobile.Performance
{
	/// <summary>
	/// Manager for executing performance MDX
	/// </summary>
	public interface IPerformanceManager
	{
		DailyPerformanceResponse GetPerformance(int accountId, string fromDate, string toDate, string themes, string countries);
		RoasPerformanceResponse GetRoasPerformance(int accountId, string fromDate, string toDate, string themes, string countries);
		CampaignPerformanceResponse GetCampaignPerformance(int accountId, string fromDate, string toDate, string themes, string countries);
		CampaignPerformanceResponse GetCountryPerformance(int accountId, string fromDate, string toDate, string themes, string countries);
	}
}
