using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Edge.Objects.Performance;

namespace Edge.Api.Mobile.Performance
{
	public class MockPerformanceManager : IPerformanceManager
	{
		public DailyPerformanceResponse GetPerformance(int accountId, string fromDate, string toDate, string themes, string countries)
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
					Date = DateTime.Now.ToString("dd/MM/yy")
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

			response.PerformanceStatistics.Add(PerformanceStatisticsKeys.TotalClicks, Math.Round(response.PerformanceList.Select(x => x.Clicks).Sum(), 1));
			response.PerformanceStatistics.Add(PerformanceStatisticsKeys.TotalCost,	  Math.Round(response.PerformanceList.Select(x => x.Cost).Sum(), 1));
			response.PerformanceStatistics.Add(PerformanceStatisticsKeys.TotalAcq1,   Math.Round(response.PerformanceList.Select(x => x.Acq1).Sum(),1));
			response.PerformanceStatistics.Add(PerformanceStatisticsKeys.TotalAcq2,   Math.Round(response.PerformanceList.Select(x => x.Acq2).Sum(), 1));
			response.PerformanceStatistics.Add(PerformanceStatisticsKeys.TotalCPA,    Math.Round(response.PerformanceList.Select(x => x.CPA).Sum(), 1));
			response.PerformanceStatistics.Add(PerformanceStatisticsKeys.TotalCPR,    Math.Round(response.PerformanceList.Select(x => x.CPR).Sum(), 1));

			return response;
		}

		public RoasPerformanceResponse GetRoasPerformance(int accountId, string fromDate, string toDate, string themes, string countries)
		{
			var r = new Random();
			var list = new List<RoasPerformance>();
			for (var i = 0; i < 5; i++)
			{
				var performance = new RoasPerformance
				{
					Cost = (double)r.Next(1, 10000) / 100,
					CostTotalPercentage = r.Next(1, 100),
					Year = DateTime.Now.ToString("yyyy"),
					Month = r.Next(1,12).ToString(CultureInfo.InvariantCulture),
					RoasPercentage = r.Next(1, 100),
					TotalDeposit = r.Next(0, 1000),
					TotalDepositors = r.Next(0, 1000)
				};
				list.Add(performance);
			}
			return new RoasPerformanceResponse {PerformanceList = list};
		}

		public CampaignPerformanceResponse GetCampaignPerformance(int accountId, string fromDate, string toDate, string themes, string countries)
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
