using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Edge.Objects.Performance
{
	public class RoasPerformance
	{
		[DataMember(Order = 1)]
		[FieldMap("Year")]
		public string Year;

		[DataMember()]
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
	}

	public class RoasPerformanceResponse : BaseResponse
	{
		[DataMember]
		[FieldMap("PerformanceList")]
		public List<RoasPerformance> PerformanceList;

		[DataMember]
		[FieldMap("TotalDepositFieldName")]
		public string TotalDepositFieldName;

		[DataMember]
		[FieldMap("TotalDepositorsFieldName")]
		public string TotalDepositorsFieldName;

		[DataMember]
		[FieldMap("CostTotalPercentageFieldName")]
		public string CostTotalPercentageFieldName;
	}
}
