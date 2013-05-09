using System.Runtime.Serialization;

namespace Edge.Objects.Performance
{
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
	}
}
