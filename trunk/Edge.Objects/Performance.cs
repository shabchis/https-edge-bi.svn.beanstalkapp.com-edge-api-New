using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using Edge.Core.Data;
using System.Data;
using Edge.Core.Configuration;
using Edge.Object.Utilities.MobileUtillity;


namespace Edge.Objects
{
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

		public Performance7Days(SqlDataReader sqlDataReader)
		{
			this.Date = Convert.ToString(sqlDataReader[0]);
			this.Cost = Convert.ToDouble(sqlDataReader[1]);
			this.Clicks = Convert.ToDouble(sqlDataReader[2]);
			this.Acq1 = Convert.ToDouble(sqlDataReader[3]);
			this.Acq2 = Convert.ToDouble(sqlDataReader[4]);
			this.CPA = Convert.ToDouble(sqlDataReader[5]);
			this.CPR = Convert.ToDouble(sqlDataReader[6]);
		}
	}

	public class Performance
	{
		public static List<Performance7Days> GetPerformanceByAccountIDAndPeriod(int accountID, int period)
		{
			List<Performance7Days> performance = new List<Performance7Days>();
			List<BOMeasure> accountMeasures = MobileUtillity.GetAccountMeasuers(accountID);

			

			using (SqlConnection conn = new SqlConnection(AppSettings.GetConnectionString("Edge.Core.Data.DataManager.Connection", "String")))
			{
				using (SqlCommand sqlCommand = DataManager.CreateCommand(@"Mobile_Performance_GetAccountPerformance", CommandType.StoredProcedure))
				{
					SqlParameter account = new SqlParameter("accountId", accountID);
					SqlParameter timePeriod = new SqlParameter("period", period);

					SqlParameter acq1FieldName = new SqlParameter("acq1FieldName", (accountMeasures.First(m=>m.MeasureBaseID==BaseMeasure.ACQ1)).MdxFieldName);
					SqlParameter acq2FieldName = new SqlParameter("acq2FieldName", (accountMeasures.First(m => m.MeasureBaseID == BaseMeasure.ACQ2)).MdxFieldName);
					SqlParameter CPAFieldName = new SqlParameter("CPAFieldName", (accountMeasures.First(m => m.MeasureBaseID == BaseMeasure.ACQ2_CPA)).MdxFieldName);
					SqlParameter CPRFieldName = new SqlParameter("CPRFieldName", (accountMeasures.First(m => m.MeasureBaseID == BaseMeasure.ACQ1_CPA)).MdxFieldName);

					sqlCommand.Parameters.Add(account);
					sqlCommand.Parameters.Add(timePeriod);
					sqlCommand.Parameters.Add(acq1FieldName);
					sqlCommand.Parameters.Add(acq2FieldName);
					sqlCommand.Parameters.Add(CPAFieldName);
					sqlCommand.Parameters.Add(CPRFieldName);

					sqlCommand.Connection = conn;
					conn.Open();
					
					using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
					{
						while (sqlDataReader.Read())
						{
							performance.Add(new Performance7Days(sqlDataReader));
						}
					}
				}
			}
			return performance;
		}

	}

	public class BOMeasure
	{
		public string MeasureDisplayName;
		public string MdxFieldName;
		public int MeasureBaseID;


		public BOMeasure(SqlDataReader sqlDataReader)
		{
			this.MeasureBaseID = Convert.ToInt32(sqlDataReader[0]);
			this.MeasureDisplayName = Convert.ToString(sqlDataReader[2]);
			this.MdxFieldName = Convert.ToString(sqlDataReader[1]);
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
