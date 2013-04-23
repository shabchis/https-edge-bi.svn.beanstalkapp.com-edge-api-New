using System;
using System.Collections.Generic;
using Edge.Objects;

namespace Edge.Api.Mobile.Performance
{
	/// <summary>
	/// Manager for executing performance MDX
	/// </summary>
	public interface IPerformanceManager
	{
		List<Performance7Days> GetPerformance(int accountId, DateTime fromDate, DateTime toDate);
		List<RoasPerformance> GetRoasPerformance(int accountId, DateTime fromDate, DateTime toDate, string depositFieldName, string depositorFieldName);
		List<CampaignPerformance> GetCampaignPerformance(int accountId, DateTime fromDate, DateTime toDate, int themeId, int countryId);
	}
}
