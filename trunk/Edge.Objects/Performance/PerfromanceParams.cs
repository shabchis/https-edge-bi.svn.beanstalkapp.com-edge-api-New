using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
				throw new MobileApiException(String.Format("Wrong parameters format: dates=ddMMyyyy, lists=INTs seperated by comma, ex: {0}", ex.Message),
											 "Failed to generate performance report, please contact Support@edge.bi (Error: 'Request parameters').");
			}
			// validate from < to 
			if (FromDate > ToDate)
				throw new MobileApiException(String.Format("From date {0} cannot be greater then to date {1}", FromDate.ToString("dd-MM-yyyy"), ToDate.ToString("dd-MM-yyyy")),
											 String.Format("From date {0} cannot be greater then to date {1}. Please fix.", FromDate.ToString("dd-MM-yyyy"), ToDate.ToString("dd-MM-yyyy")));
			// validate from <= Today
			if (FromDate > DateTime.Now)
				throw new MobileApiException(String.Format("Report cannot be executed for future dates (start date = {0}). Please fix.", FromDate.ToString("dd-MM-yyyy")),
											 String.Format("Report cannot be executed for future dates (start date = {0}). Please fix.", FromDate.ToString("dd-MM-yyyy")));
			
			// validate time interval according to configuration
			CheckTimeRange(reportType);
		}

		private void CheckTimeRange(PerformanceReportType reportType)
		{
			int maxTimerange;
			if (int.TryParse(AppSettings.Get("MobileApi", String.Format("{0}MaxTimeInterval",reportType), false), out maxTimerange))
			{
				var daysDiff = ToDate.Subtract(FromDate).TotalDays;
				if (maxTimerange > 0 && maxTimerange < daysDiff)
				{
					throw new MobileApiException(String.Format("Invalid date parameters, max time interval allowed for report '{1}' is {0} days. To exceed please change in configuration.", maxTimerange, reportType),
												 String.Format("Incorrect Time Selection: Maximum time period for this report is {0} days.", maxTimerange));
				}
			}
		}
	}
}
