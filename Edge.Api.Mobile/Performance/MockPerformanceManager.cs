using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Edge.Objects.Performance;

namespace Edge.Api.Mobile.Performance
{
	public class MockPerformanceManager : IPerformanceManager
	{
		public DailyPerformanceResponse GetPerformance(int accountId, DateTime fromDate, DateTime toDate, List<int> themeId, List<int> countryId)
		{
			var r = new Random();
			var list = new List<DailyPerformance>();
			for (var i = 0; i < 10; i++)
			{
				var performance = new DailyPerformance
				{
					Acq1 =  (double)r.Next(1, 1000)/100,
					Acq2 = (double)r.Next(1, 1000) / 100,
					CPA = (double)r.Next(1, 1000) / 100,
					CPR = (double)r.Next(1, 1000) / 100,
					Clicks = r.Next(0, 10),
					Cost = r.Next(0, 10),
					Date = DateTime.Now.ToString("ddMMyyyy")
				};
				list.Add(performance);
			}
			var response = new DailyPerformanceResponse
				{
					PerformanceList = list,
					Acq1FieldName = "Acq1 name",
					Acq2FieldName = "Acq2 name",
					CPAFieldName = "CPA name",
					CPRFieldName = "CPR name",
					PerformanceStatistics = new Dictionary<string, double>()
				};

			response.PerformanceStatistics.Add(PerformanceStatisticsKeys.TotalClicks, response.PerformanceList.Select(x => x.Clicks).Sum());
			response.PerformanceStatistics.Add(PerformanceStatisticsKeys.TotalCost, response.PerformanceList.Select(x => x.Cost).Sum());
			response.PerformanceStatistics.Add(PerformanceStatisticsKeys.TotalAcq1, response.PerformanceList.Select(x => x.Acq1).Sum());
			response.PerformanceStatistics.Add(PerformanceStatisticsKeys.TotalAcq2, response.PerformanceList.Select(x => x.Acq2).Sum());
			response.PerformanceStatistics.Add(PerformanceStatisticsKeys.TotalCPA, response.PerformanceList.Select(x => x.CPA).Sum());
			response.PerformanceStatistics.Add(PerformanceStatisticsKeys.TotalCPR, response.PerformanceList.Select(x => x.CPR).Sum());

			return response;
		}

		public List<RoasPerformance> GetRoasPerformance(int accountId, DateTime fromDate, DateTime toDate, List<int> themeId, List<int> countryId)
		{
			var r = new Random();
			var list = new List<RoasPerformance>();
			for (var i = 0; i < 5; i++)
			{
				var performance = new RoasPerformance
				{
					Cost = (double)r.Next(1, 10000) / 100,
					CostTotalPercentage = r.Next(1, 100),
					Month = r.Next(1,12).ToString(CultureInfo.InvariantCulture),
					RoasPercentage = r.Next(1, 100),
					TotalDeposit = r.Next(0, 1000),
					TotalDepositors = r.Next(0, 1000)
				};
				list.Add(performance);
			}
			return list;
		}

		public CampaignPerformanceResponse GetCampaignPerformance(int accountId, DateTime fromDate, DateTime toDate, List<int> themeId, List<int> countryId)
		{
			var r = new Random();
			var list = new List<CampaignPerformance>();
			for (var i = 0; i < 10; i++)
			{
				var performance = new CampaignPerformance
				{
					Acq1 = (double)r.Next(1, 1000) / 100,
					Acq2 = (double)r.Next(1, 1000) / 100,
					CPA = (double)r.Next(1, 1000) / 100,
					CPR = (double)r.Next(1, 1000) / 100,
					Clicks = r.Next(0, 10),
					Cost = r.Next(0, 10),
					CampaignName = String.Format("Campaign #{0}", i)
				};
				list.Add(performance);
			}

			var response = new CampaignPerformanceResponse
			{
				PerformanceList = list,
				Acq1FieldName = "Acq1 name",
				Acq2FieldName = "Acq2 name",
				CPAFieldName = "CPA name",
				CPRFieldName = "CPR name",
			};
			return response;
		}
	}
}
