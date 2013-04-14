using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Edge.Core.Configuration;
using Edge.Core.Data;
using Edge.Objects;

namespace Edge.Object.Utilities.MobileUtillity
{
	public static class MobileUtillity
	{
		public static List<BOMeasure> GetAccountMeasuers(int accountID)
		{
			List<BOMeasure> measures = GetMeasuresByAccountFromDB(accountID);
			return measures;
		}

		private static List<BOMeasure> GetMeasuresByAccountFromDB(int accountID)
		{
			List<BOMeasure> measures = new List<BOMeasure>();
			using (SqlConnection conn = new SqlConnection(AppSettings.GetConnectionString("Edge.Core.Data.DataManager.Connection", "String")))
			{
				using (SqlCommand sqlCommand = DataManager.CreateCommand(@"Mobile_Measure_GetConvAndCPAMeasures", CommandType.StoredProcedure))
				{
					SqlParameter account = new SqlParameter("accountId", accountID);
					sqlCommand.Parameters.Add(account);

					sqlCommand.Connection = conn;
					conn.Open();

					using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
					{
						while (sqlDataReader.Read())
						{
							measures.Add(new BOMeasure(sqlDataReader));
						}
					}
				}
			}
			
			return measures;
		}

		
	}

	
}
