using System;
using System.Data.SqlClient;
using System.Runtime.Serialization;

namespace Edge.Objects
{
	public enum SegmentType
	{
		Language = 1,
		Region,
		Theme,
		Country,
		USP
	}

	public class SegmentInfo
	{
		[DataMember(Order = 1)]
		[FieldMap("Id")]
		public int Id;

		[DataMember]
		[FieldMap("Name")]
		public string Name;
	}

	public class BOMeasure
	{
		public string MeasureDisplayName;
		public string MdxFieldName;
		public int MeasureBaseID;


		public BOMeasure(SqlDataReader sqlDataReader)
		{
			MeasureBaseID = Convert.ToInt32(sqlDataReader[0]);
			MeasureDisplayName = Convert.ToString(sqlDataReader[2]);
			MdxFieldName = Convert.ToString(sqlDataReader[1]);
		}
	}

	public static class BaseMeasure
	{
		public static int ACQ1_CPA = 1;
		public static int ACQ1 = 5;
		public static int ACQ2_CPA = 9;
		public static int ACQ2 = 7;
	}
}
