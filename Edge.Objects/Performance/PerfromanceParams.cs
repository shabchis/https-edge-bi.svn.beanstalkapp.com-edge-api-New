using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Edge.Core.Configuration;

namespace Edge.Objects.Performance
{
	public class PerfromanceParams
	{
		public int AccountId { get; set; }
		public DateTime FromDate { get; set; }
		public DateTime ToDate { get; set; }

		public List<int> Themes { get; set; }
		public List<int> Countries { get; set; }

		public PerfromanceParams(int accountId, string from, string to, string themes, string countries, PerformanceReportType reportType)
		{
			try
			{
				AccountId = accountId;
				FromDate = DateTime.ParseExact(from, "ddMMyyyy", null, DateTimeStyles.None);
				ToDate = DateTime.ParseExact(to, "ddMMyyyy", null, DateTimeStyles.None);
				Themes = !String.IsNullOrEmpty(themes) ? themes.Split(',').ToList().ConvertAll(int.Parse) : null;
				Countries = !String.IsNullOrEmpty(countries) ? countries.Split(',').ToList().ConvertAll(int.Parse) : null;
			}
			catch (Exception ex)
			{
				throw new ArgumentException(String.Format("Wrong parameters format: dates=ddMMyyyy, lists=INTs seperated by comma, ex: {0}", ex.Message));
			}
			CheckTimeRange(reportType);
		}

		private void CheckTimeRange(PerformanceReportType reportType)
		{
			var maxTimerange = 0;
			if (int.TryParse(AppSettings.Get("MobileApi", String.Format("{0}MaxTimeInterval",reportType), false), out maxTimerange))
			{
				var daysDiff = ToDate.Subtract(FromDate).TotalDays;
				if (maxTimerange > 0 && maxTimerange < daysDiff)
				{
					throw new ArgumentException(String.Format("Invalid date parameters, max time interval allowed for report '{1}' is {0} days. To exceed please change in configuration.", maxTimerange, reportType));
				}
			}
		}
	}
}
