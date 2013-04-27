using System;
using System.Collections.Generic;
using System.Globalization;
using Edge.Objects;

namespace Edge.Api.Mobile.Performance
{
	public class MockPerformanceManager : IPerformanceManager
	{
		public List<Performance7Days> GetPerformance(int accountId, DateTime fromDate, DateTime toDate)
		{
			var r = new Random();
			var list = new List<Performance7Days>();
			for (var i = 0; i < 10; i++)
			{
				var performance = new Performance7Days
				{
					Acq1 =  (double)r.Next(1, 1000)/100,
					Acq1FieldName = "Acq1 name",
					Acq2 = (double)r.Next(1, 1000) / 100,
					Acq2FieldName = "Acq2 name",
					CPA = (double)r.Next(1, 1000) / 100,
					CPAFieldName = "CPA name",
					CPR = (double)r.Next(1, 1000) / 100,
					CPRFieldName = "CPR name",
					Clicks = r.Next(0, 10),
					Cost = r.Next(0, 10),
					Date = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")
				};
				list.Add(performance);
			}
			return list;
		}

		public List<RoasPerformance> GetRoasPerformance(int accountId, DateTime fromDate, DateTime toDate)
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

		public List<CampaignPerformance> GetCampaignPerformance(int accountId, DateTime fromDate, DateTime toDate, int themeId, int countryId)
		{
			var r = new Random();
			var list = new List<CampaignPerformance>();
			for (var i = 0; i < 10; i++)
			{
				var performance = new CampaignPerformance
				{
					Acq1 = (double)r.Next(1, 1000) / 100,
					Acq1FieldName = "Acq1 name",
					Acq2 = (double)r.Next(1, 1000) / 100,
					Acq2FieldName = "Acq2 name",
					CPA = (double)r.Next(1, 1000) / 100,
					CPAFieldName = "CPA name",
					CPR = (double)r.Next(1, 1000) / 100,
					CPRFieldName = "CPR name",
					Clicks = r.Next(0, 10),
					Cost = r.Next(0, 10),
					CampaignName = String.Format("Campaign #{0}", i)
				};
				list.Add(performance);
			}
			return list;
		}
	}
}
