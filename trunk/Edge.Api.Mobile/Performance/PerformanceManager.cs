using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Edge.Core.Configuration;
using Edge.Core.Data;
using Edge.Objects;
using Edge.Objects.Performance;

namespace Edge.Api.Mobile.Performance
{
	/// <summary>
	/// Manager for executing performance MDX
	/// </summary>
	public class PerformanceManager : IPerformanceManager
	{
		#region Public Methods
		public DailyPerformanceResponse GetPerformance(int accountId, string from, string to, string themes, string countries)
		{
			var perfParam = new PerfromanceParams(accountId, from, to, themes, countries, PerformanceReportType.DailyPerformance);
			using (var connection = new SqlConnection(AppSettings.GetConnectionString("Edge.Core.Data.DataManager.Connection", "String")))
			{
				connection.Open();

				// get cube name and relevant measure config foe preparing MDX command
				var cubeName = GetAccountCubeName(accountId, connection);
				if (String.IsNullOrWhiteSpace(cubeName)) 
					throw new MobileApiException(String.Format("Cannot retrieve cube name for account {0}", accountId), 
												 String.Format("No cube defined for account {0}", accountId));

				var measureList = GetMeasures(accountId, connection);
				if (measureList.Count == 0 || measureList.Any(x => String.IsNullOrEmpty(x.MdxFieldName)))
					throw new MobileApiException(String.Format("Measures are not defined properly for account {0}, check if MDX name is defined", accountId),
												 String.Format("Measures are not defined properly for account {0}", accountId));

				// prepare SELECT and FROM
				var selectClause = String.Format(@"SELECT NON EMPTY {{[Time Dim].[Time Dim].[Day]}} ON ROWS,( {{ [Measures].[Cost],[Measures].[Clicks],[Measures].[{0}], [Measures].[{1}],[Measures].[{2}],[Measures].[{3}]}} ) ON COLUMNS",
												measureList.First(x => x.MeasureBaseID == BaseMeasure.ACQ1).MdxFieldName,
												measureList.First(x => x.MeasureBaseID == BaseMeasure.ACQ2).MdxFieldName,
												measureList.First(x => x.MeasureBaseID == BaseMeasure.ACQ1_CPA).MdxFieldName,
												measureList.First(x => x.MeasureBaseID == BaseMeasure.ACQ2_CPA).MdxFieldName);

				var fromClause = String.Format(@"FROM [{0}] WHERE ([Accounts Dim].[Accounts].[Account].&[{1}],{4}{5}{{[Time Dim].[DayCode].&[{2}]:[Time Dim].[DayCode].&[{3}] }})",
												cubeName,
												accountId,
												perfParam.FromDate.ToString("yyyyMMdd"),
												perfParam.ToDate.ToString("yyyyMMdd"),
												GetWhereClause(perfParam.Themes, "Theme"),
												GetWhereClause(perfParam.Countries, "Country"));

				using (var cmd = DataManager.CreateCommand("SP_ExecuteMDX", CommandType.StoredProcedure))
				{
					cmd.Parameters.AddWithValue("@WithMDX", " ");
					cmd.Parameters.AddWithValue("@SelectMDX", selectClause);
					cmd.Parameters.AddWithValue("@FromMDX", fromClause);
					cmd.Connection = connection;

					// execute MDX
					using (var reader = cmd.ExecuteReader())
					{
						var list = new List<DailyPerformance>();
						while (reader.Read())
						{
							list.Add(new DailyPerformance
								{
									Date   = reader[2] != DBNull.Value ? DateTime.ParseExact(reader[2].ToString(), "dd/MM/yyyy", null, DateTimeStyles.None).ToString("dd/MM/yy") : String.Empty,
									Cost   = reader[3] != DBNull.Value ? Math.Round(Convert.ToDouble(reader[3]),1) : 0,
									Clicks = reader[4] != DBNull.Value ? Math.Round(Convert.ToDouble(reader[4]),1) : 0,
									Acq1   = reader[5] != DBNull.Value ? Math.Round(Convert.ToDouble(reader[5]),1) : 0,
									Acq2   = reader[6] != DBNull.Value ? Math.Round(Convert.ToDouble(reader[6]),1) : 0,
									CPA    = reader[7] != DBNull.Value ? Math.Round(Convert.ToDouble(reader[7]),1) : 0,
									CPR    = reader[8] != DBNull.Value ? Math.Round(Convert.ToDouble(reader[8]),1) : 0,
								});
						}
						// prepare response
						var response = new DailyPerformanceResponse
						{
							PerformanceList = list,
							Acq1FieldName = measureList.First(x => x.MeasureBaseID == BaseMeasure.ACQ1).MeasureDisplayName,
							Acq2FieldName = measureList.First(x => x.MeasureBaseID == BaseMeasure.ACQ2).MeasureDisplayName,
							CPAFieldName = measureList.First(x => x.MeasureBaseID == BaseMeasure.ACQ1_CPA).MeasureDisplayName,
							CPRFieldName = measureList.First(x => x.MeasureBaseID == BaseMeasure.ACQ2_CPA).MeasureDisplayName,
							PerformanceStatistics = new Dictionary<string, double>()
						};
						// calculate statistics values (Sum and Average)
						response.PerformanceStatistics.Add(PerformanceStatisticsKeys.TotalClicks, Math.Round(response.PerformanceList.Select(x => x.Clicks).Sum(), 1));
						response.PerformanceStatistics.Add(PerformanceStatisticsKeys.TotalCost, Math.Round(response.PerformanceList.Select(x => x.Cost).Sum(), 1));
						response.PerformanceStatistics.Add(PerformanceStatisticsKeys.TotalAcq1, Math.Round(response.PerformanceList.Select(x => x.Acq1).Sum(), 1));
						response.PerformanceStatistics.Add(PerformanceStatisticsKeys.TotalAcq2, Math.Round(response.PerformanceList.Select(x => x.Acq2).Sum(), 1));
						response.PerformanceStatistics.Add(PerformanceStatisticsKeys.TotalCPA, Math.Round(response.PerformanceList.Select(x => x.CPA).Sum(), 1));
						response.PerformanceStatistics.Add(PerformanceStatisticsKeys.TotalCPR, Math.Round(response.PerformanceList.Select(x => x.CPR).Sum(), 1));

						response.PerformanceStatistics.Add(PerformanceStatisticsKeys.AvgClicks, response.PerformanceList.Count > 0 ? Math.Round(response.PerformanceList.Select(x => x.Clicks).Average(), 1) : 0);
						response.PerformanceStatistics.Add(PerformanceStatisticsKeys.AvgCost, response.PerformanceList.Count > 0 ? Math.Round(response.PerformanceList.Select(x => x.Cost).Average(), 1) : 0);
						response.PerformanceStatistics.Add(PerformanceStatisticsKeys.AvgAcq1, response.PerformanceList.Count > 0 ? Math.Round(response.PerformanceList.Select(x => x.Acq1).Average(), 1) : 0);
						response.PerformanceStatistics.Add(PerformanceStatisticsKeys.AvgAcq2, response.PerformanceList.Count > 0 ? Math.Round(response.PerformanceList.Select(x => x.Acq2).Average(), 1) : 0);
						response.PerformanceStatistics.Add(PerformanceStatisticsKeys.AvgCPA, response.PerformanceList.Count > 0 ? Math.Round(response.PerformanceList.Select(x => x.CPA).Average(), 1) : 0);
						response.PerformanceStatistics.Add(PerformanceStatisticsKeys.AvgCPR, response.PerformanceList.Count > 0 ? Math.Round(response.PerformanceList.Select(x => x.CPR).Average(), 1) : 0);

						return response;
					}
				}
			}
		}

		public RoasPerformanceResponse GetRoasPerformance(int accountId, string from, string to, string themes, string countries)
		{
			var perfParam = new PerfromanceParams(accountId, from, to, themes, countries, PerformanceReportType.RoasPerformance);
			using (var connection = new SqlConnection(AppSettings.GetConnectionString("Edge.Core.Data.DataManager.Connection", "String")))
			{
				connection.Open();

				// get cube name and relevant measure config foe preparing MDX command
				// get cube name and relevant measure config foe preparing MDX command
				var cubeName = GetAccountCubeName(accountId, connection);
				if (String.IsNullOrWhiteSpace(cubeName))
					throw new MobileApiException(String.Format("Cannot retrieve cube name for account {0}", accountId),
												 String.Format("No cube defined for account {0}", accountId));

				var measureList = GetMeasures(accountId, connection);
				if (!measureList.Any(x => x.IsDeposit))
					throw new MobileApiException(String.Format("There is no deposit field defined for account {0}", accountId),
												 String.Format("ROAS report is not relevant for account {0}", accountId)); 
				
				if (measureList.Count == 0 || measureList.Any(x => String.IsNullOrEmpty(x.MdxFieldName)))
					throw new MobileApiException(String.Format("Measures are not defined properly for account {0}, check if MDX name is defined", accountId),
												 String.Format("Measures are not defined properly for account {0}", accountId));

				// prepare WITH, SELECT and FROM
				var withClause = String.Format("WITH MEMBER [%ROAS] AS [Measures].[{0}]/ IIF([Measures].[Cost] = 0, NULL, [Measures].[Cost] ) * 100 ", measureList.First(x => x.IsDeposit).MdxFieldName);
				
				var selectClause = String.Format(@"SELECT ClosingPeriod([Time Dim].[Time Dim].[Month], [Time Dim].[Time Dim].CurrentMember).Lag(12) : ClosingPeriod([Time Dim].[Time Dim].[Month], [Time Dim].[Time Dim].CurrentMember) ON ROWS, 
												   ({{[Measures].[Cost],[Measures].[{0}],[%ROAS],[Measures].[{1}], [Measures].[{2}]}}) ON COLUMNS",
												measureList.First(x => x.IsDeposit).MdxFieldName,
												measureList.First(x => x.MeasureBaseID == BaseMeasure.ACQ2).MdxFieldName,
												measureList.First(x => x.MeasureBaseID == BaseMeasure.ACQ2_CPA).MdxFieldName);

				var fromClause = String.Format(@"FROM [{0}] WHERE ([Accounts Dim].[Accounts].[Account].&[{1}],{4}{5}{{[Time Dim].[DayCode].&[{2}]:[Time Dim].[DayCode].&[{3}]}})",
												cubeName,
												accountId,
												perfParam.FromDate.ToString("yyyyMMdd"),
												perfParam.ToDate.ToString("yyyyMMdd"),
												GetWhereClause(perfParam.Themes, "Theme"),
												GetWhereClause(perfParam.Countries, "Country"));

				using (var cmd = DataManager.CreateCommand("SP_ExecuteMDX", CommandType.StoredProcedure))
				{
					cmd.Parameters.AddWithValue("@WithMDX", withClause);
					cmd.Parameters.AddWithValue("@SelectMDX", selectClause);
					cmd.Parameters.AddWithValue("@FromMDX", fromClause);
					cmd.Connection = connection;

					// execute MDX
					using (var reader = cmd.ExecuteReader())
					{
						var list = new List<RoasPerformance>();
						while (reader.Read())
						{
							if (reader[1] != DBNull.Value)
							{
								list.Add(new RoasPerformance
								{
									Month = reader[1].ToString().Split(' ')[0],
									Year = reader[1].ToString().Split(' ')[1],
									Cost = reader[2] != DBNull.Value ? Math.Round(Convert.ToDouble(reader[2]), 1) : 0,
									TotalDeposit = reader[3] != DBNull.Value ? Math.Round(Convert.ToDouble(reader[3]), 1) : 0,
									TotalDepositors = reader[4] != DBNull.Value ? Math.Round(Convert.ToDouble(reader[4]), 1) : 0,
									RoasPercentage = reader[5] != DBNull.Value ? Math.Round(Convert.ToDouble(reader[5]), 1) : 0,
									CostTotalPercentage = reader[6] != DBNull.Value ? Math.Round(Convert.ToDouble(reader[6]), 1) : 0
								});
							}
						}
						return new RoasPerformanceResponse
							{
								PerformanceList = list,
								TotalDepositFieldName = measureList.First(x => x.IsDeposit).MeasureDisplayName,
								TotalDepositorsFieldName = measureList.First(x => x.MeasureBaseID == BaseMeasure.ACQ2).MeasureDisplayName,
								CostTotalPercentageFieldName = measureList.First(x => x.MeasureBaseID == BaseMeasure.ACQ2_CPA).MeasureDisplayName,
							};
					}
				}
			}
		}

		public CampaignPerformanceResponse GetCampaignPerformance(int accountId, string from, string to, string themes, string countries)
		{
			var perfParam = new PerfromanceParams(accountId, from, to, themes, countries, PerformanceReportType.CampaignPerformance);
			using (var connection = new SqlConnection(AppSettings.GetConnectionString("Edge.Core.Data.DataManager.Connection", "String")))
			{
				connection.Open();

				// get cube name and relevant measure config foe preparing MDX command
				// get cube name and relevant measure config foe preparing MDX command
				var cubeName = GetAccountCubeName(accountId, connection);
				if (String.IsNullOrWhiteSpace(cubeName))
					throw new MobileApiException(String.Format("Cannot retrieve cube name for account {0}", accountId),
												 String.Format("No cube defined for account {0}", accountId));

				var measureList = GetMeasures(accountId, connection);
				if (measureList.Count == 0 || measureList.Any(x => String.IsNullOrEmpty(x.MdxFieldName)))
					throw new MobileApiException(String.Format("Measures are not defined properly for account {0}, check if MDX name is defined", accountId),
												 String.Format("Measures are not defined properly for account {0}", accountId));

				// prepare SELECT and FROM
				var selectClause = String.Format(@"SELECT NON EMPTY [Getways Dim].[Gateways].[Campaign].members ON ROWS,( {{ [Measures].[Cost],[Measures].[Clicks],[Measures].[{0}], [Measures].[{1}],[Measures].[{2}],[Measures].[{3}]}} ) ON COLUMNS",
												measureList.First(x => x.MeasureBaseID == BaseMeasure.ACQ1).MdxFieldName,
												measureList.First(x => x.MeasureBaseID == BaseMeasure.ACQ2).MdxFieldName,
												measureList.First(x => x.MeasureBaseID == BaseMeasure.ACQ1_CPA).MdxFieldName,
												measureList.First(x => x.MeasureBaseID == BaseMeasure.ACQ2_CPA).MdxFieldName);

				var fromClause = String.Format(@"FROM [{0}] WHERE ([Accounts Dim].[Accounts].[Account].&[{1}],{4}{5}{{[Time Dim].[DayCode].&[{2}]:[Time Dim].[DayCode].&[{3}]}})",
												cubeName,
												accountId,
												perfParam.FromDate.ToString("yyyyMMdd"),
												perfParam.ToDate.ToString("yyyyMMdd"),
												GetWhereClause(perfParam.Themes, "Theme"),
												GetWhereClause(perfParam.Countries, "Country"));

				using (var cmd = DataManager.CreateCommand("SP_ExecuteMDX", CommandType.StoredProcedure))
				{
					cmd.Parameters.AddWithValue("@WithMDX", " ");
					cmd.Parameters.AddWithValue("@SelectMDX", selectClause);
					cmd.Parameters.AddWithValue("@FromMDX", fromClause);
					cmd.Connection = connection;

					// execute MDX
					using (var reader = cmd.ExecuteReader())
					{
						var list = new List<CampaignPerformance>();
						while (reader.Read())
						{
							var performance = new CampaignPerformance
								{
									CampaignName = reader[1] != DBNull.Value ? reader[1].ToString() : String.Empty,
									Cost	= reader[2] != DBNull.Value ? Math.Round(Convert.ToDouble(reader[2]),1) : 0,
									Clicks	= reader[3] != DBNull.Value ? Math.Round(Convert.ToDouble(reader[3]),1) : 0,
									Acq1	= reader[4] != DBNull.Value ? Math.Round(Convert.ToDouble(reader[4]),1) : 0,
									Acq2	= reader[5] != DBNull.Value ? Math.Round(Convert.ToDouble(reader[5]),1) : 0,
									CPA		= reader[6] != DBNull.Value ? Math.Round(Convert.ToDouble(reader[6]),1) : 0,
									CPR		= reader[7] != DBNull.Value ? Math.Round(Convert.ToDouble(reader[7]), 1) : 0,
								};
							// add perfrmance to list only if it has at least ont value
							if (performance.Cost > 0 || performance.Clicks > 0 || performance.Acq1 > 0 || performance.Acq2 > 0 || performance.CPA > 0 || performance.CPR > 0)
								list.Add(performance);
						}
						var response = new CampaignPerformanceResponse
						{
							PerformanceList = list,
							Acq1FieldName = measureList.First(x => x.MeasureBaseID == BaseMeasure.ACQ1).MeasureDisplayName,
							Acq2FieldName = measureList.First(x => x.MeasureBaseID == BaseMeasure.ACQ2).MeasureDisplayName,
							CPAFieldName = measureList.First(x => x.MeasureBaseID == BaseMeasure.ACQ1_CPA).MeasureDisplayName,
							CPRFieldName = measureList.First(x => x.MeasureBaseID == BaseMeasure.ACQ2_CPA).MeasureDisplayName
						};
						return response;
					}
				}
			}
		} 
		#endregion

		#region Private Methods
		private List<BOMeasure> GetMeasures(int accountId, SqlConnection connection)
		{
			var measures = new List<BOMeasure>();
			using (var cmd = DataManager.CreateCommand("Mobile_Measure_GetConvAndCPAMeasures", CommandType.StoredProcedure))
			{
				cmd.Parameters.AddWithValue("@accountId", accountId);
				cmd.Connection = connection;

				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
						measures.Add(new BOMeasure(reader));
				}
			}
			return measures;
		}
		
		private string GetAccountCubeName(int accountId, SqlConnection connection)
		{
			using (var cmd = new SqlCommand{Connection = connection})
			{
				cmd.CommandText = "SELECT AnalysisSettings FROM User_GUI_Account WHERE Account_Id=@accountId";
				cmd.Parameters.AddWithValue("@accountId", accountId);
				
				var result = cmd.ExecuteScalar();
				if (result != null)
				{
					var xml = result.ToString();
					var doc = XDocument.Parse(xml);
					var elm = doc.Element("AnalysisSettings");
					if (elm != null)
						return elm.Attribute("CubeName").Value;
				}
			}
			return null;
		}

		private string GetWhereClause(ICollection<int> values, string columnName)
		{
			var whereClause = String.Empty;
			if (values != null && values.Count > 0)
			{
				whereClause = values.Aggregate(String.Empty, (current, value) => String.Format(",[Getways Dim].[{2}].&[{0}]{1}", value, current, columnName));
				whereClause = String.Format("{{{0}}},", whereClause.Remove(0, 1));
			}
			return whereClause;
		}
		#endregion
	}
}
