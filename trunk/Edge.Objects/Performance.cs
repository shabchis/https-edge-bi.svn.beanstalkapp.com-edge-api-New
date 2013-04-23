using System.Runtime.Serialization;


namespace Edge.Objects
{
	#region Performance Objects
	public class Performance7Days
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
		[FieldMap("Acq1FieldName")]
		public string Acq1FieldName;

		[DataMember]
		[FieldMap("Acq2")]
		public double Acq2;

		[DataMember]
		[FieldMap("Acq2FieldName")]
		public string Acq2FieldName;

		[DataMember]
		[FieldMap("CPA")]
		public double CPA;

		[DataMember]
		[FieldMap("CPAFieldName")]
		public string CPAFieldName;

		[DataMember]
		[FieldMap("CPR")]
		public double CPR;

		[DataMember]
		[FieldMap("CPRFieldName")]
		public string CPRFieldName;
	}

	public class RoasPerformance
	{
		[DataMember(Order = 1)]
		[FieldMap("Month")]
		public string Month;

		[DataMember]
		[FieldMap("Cost")]
		public double Cost;

		[DataMember]
		[FieldMap("TotalDeposit")]
		public double TotalDeposit;

		[DataMember]
		[FieldMap("TotalDepositors")]
		public double TotalDepositors;

		[DataMember]
		[FieldMap("RoasPercentage")]
		public double RoasPercentage;

		[DataMember]
		[FieldMap("CostTotalPercentage")]
		public double CostTotalPercentage;

		public RoasPerformance() { }
	}

	public class CampaignPerformance : Performance7Days
	{
		[DataMember]
		[FieldMap("CampaignName")]
		public string CampaignName;
	} 
	#endregion
}
