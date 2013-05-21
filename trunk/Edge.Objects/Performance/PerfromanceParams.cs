using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Edge.Objects.Performance
{
	public class PerfromanceParams
	{
		public int AccountId { get; set; }
		public DateTime FromDate { get; set; }
		public DateTime ToDate { get; set; }

		public List<int> Themes { get; set; }
		public List<int> Countries { get; set; }

		public PerfromanceParams(int accountId, string from, string to, string themes, string countries)
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
		}
	}
}
