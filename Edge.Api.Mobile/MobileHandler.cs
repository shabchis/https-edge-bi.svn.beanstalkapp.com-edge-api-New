using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Edge.Api.Handlers.Template;
using Edge.Objects;



namespace Edge.Api.Mobile.Handlers
{
	public class MobileHandler  :TemplateHandler
	{
		[UriMapping(Method = "GET", Template = "performance/accounts/{accountID}/{period}")]
		public List<Performance7Days> GetPerformanceLast7DaysByAccount(string accountID,string period)
		{
			List<Performance7Days> performance = new List<Performance7Days>();
			performance = Performance.GetPerformanceByAccountIDAndPeriod(int.Parse(accountID), int.Parse(period));

			return performance;
		}

		public override bool ShouldValidateSession
		{
			get { return true; }
		}
	}
}
