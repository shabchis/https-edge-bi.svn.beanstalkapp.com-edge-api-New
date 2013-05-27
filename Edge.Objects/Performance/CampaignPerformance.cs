using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Edge.Objects.Performance
{
	public class CampaignPerformance : DailyPerformance
	{
		[DataMember]
		[FieldMap("CampaignName")]
		public string CampaignName;
	}

	public class CampaignPerformanceResponse : BaseResponse
	{
		[DataMember]
		[FieldMap("PerformanceList")]
		public List<CampaignPerformance> PerformanceList;

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
	}
}
