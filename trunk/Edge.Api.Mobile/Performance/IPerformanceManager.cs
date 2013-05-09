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
		DailyPerformanceResponse GetPerformance(int accountId, DateTime fromDate, DateTime toDate, List<int> themeId, List<int> countryId);
		List<RoasPerformance> GetRoasPerformance(int accountId, DateTime fromDate, DateTime toDate, List<int> themeId, List<int> countryId);
		CampaignPerformanceResponse GetCampaignPerformance(int accountId, DateTime fromDate, DateTime toDate, List<int> themeId, List<int> countryId);
	}
}
