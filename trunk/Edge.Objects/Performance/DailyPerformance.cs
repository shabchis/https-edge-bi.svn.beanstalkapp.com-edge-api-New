using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Edge.Objects.Performance
{
	public class DailyPerformance
	{
		[DataMember(Order = 1)]
		[FieldMap("Date")]
		public string Date;

		[DataMember]
		[FieldMap("Cost")]
		public double Cost;

		[DataMember]
		[FieldMap("Clicks")]
		public double Clicks;

		[DataMember]
		[FieldMap("Acq1")]
		public double Acq1;

		[DataMember]
		[FieldMap("Acq2")]
		public double Acq2;

		[DataMember]
		[FieldMap("CPA")]
		public double CPA;

		[DataMember]
		[FieldMap("CPR")]
		public double CPR;
	}

	public class DailyPerformanceResponse
	{
		[DataMember]
		[FieldMap("PerformanceList")]
		public List<DailyPerformance> PerformanceList;

		[DataMember]
		[FieldMap("Acq1FieldName")]
		public string Acq1FieldName;

		[DataMember]
		[FieldMap("Acq2FieldName")]
		public string Acq2FieldName;

		[DataMember]
		[FieldMap("CPAFieldName")]
		public string CPAFieldName;

		[DataMember]
		[FieldMap("CPRFieldName")]
		public string CPRFieldName;

		[DataMember]
		[FieldMap("PerformanceStatistics")]
		public Dictionary<string, double> PerformanceStatistics;
	}

	public static class PerformanceStatisticsKeys
	{
		public static string TotalClicks = "TotalClicks";
		public static string TotalCost = "TotalCost";
		public static string TotalAcq1 = "TotalAcq1";
		public static string TotalAcq2 = "TotalAcq2";
		public static string TotalCPA = "TotalCPA";
		public static string TotalCPR = "TotalCPR";

		public static string AvgClicks = "AvgClicks";
		public static string AvgCost = "AvgCost";
		public static string AvgAcq1 = "AvgAcq1";
		public static string AvgAcq2 = "AvgAcq2";
		public static string AvgCPA = "AvgCPA";
		public static string AvgCPR = "AvgCPR";
	}
}
