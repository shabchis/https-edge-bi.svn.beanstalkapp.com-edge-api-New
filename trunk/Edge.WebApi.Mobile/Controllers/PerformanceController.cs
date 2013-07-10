using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;
using Edge.Api.Mobile.Performance;
using Edge.Core.Configuration;
using Edge.Core.Utilities;
using Edge.Objects;
using Edge.Objects.Performance;

namespace Edge.WebApi.Mobile.Controllers
{
	public class PerformanceController : BaseController
	{
		[ActionName("DailyPerformance")]
		public DailyPerformanceResponse GetPerformance(string sessionId, int accountId, string from, string to, string theme="", string country="")
		{
			try
			{
				ValidatePermission(ValidateSession(sessionId), accountId);

				Log.Write("Mobile API", String.Format("Daily performance report request. Parameters: account={0}, from={1}, to={2}, theme={3}, country={4}", accountId, from, to, theme, country), LogMessageType.Debug);
				var manager = GetManager();
				var responce = manager.GetPerformance(accountId, from, to, theme, country);
				Log.Write("Mobile API", String.Format("Finished Daily performance report request. Response contains {0} records", responce.PerformanceList.Count), LogMessageType.Debug);
				if (responce.PerformanceList.Count == 0)
				{
					return new DailyPerformanceResponse
					{
						HasError = true,
						ErrorMsg = "No data found",
						DisplayError = "No data found for Daily performance report. Please refine report parameters."
					};
				}
				return responce;
			}
			catch (Exception ex)
			{
				Log.Write("Mobile API", String.Format("Daily performance report failed, ex: {0}. Parameters: account={1}, from={2}, to={3}, theme={4}, country={5}", 
										ex.Message, accountId, from, to, theme, country), LogMessageType.Error);
				return new DailyPerformanceResponse
				{
					HasError = true,
					ErrorMsg = ex.Message,
					DisplayError = (ex is MobileApiException) ? (ex as MobileApiException).DisplayMessage : "Failed to generate Daily performance report, please contact Support@edge.bi (Error: 'General')."
				};
			}

		}

		[ActionName("RoasPerformance")]
		public RoasPerformanceResponse GetRoasPerformance(string sessionId, int accountId, string from, string to, string theme = "", string country = "")
		{
			try
			{
				ValidatePermission(ValidateSession(sessionId), accountId);

				Log.Write("Mobile API", String.Format("ROAS performance report request. Parameters: account={0}, from={1}, to={2}, theme={3}, country={4}", accountId, from, to, theme, country), LogMessageType.Debug);
				var manager = GetManager();
				var responce = manager.GetRoasPerformance(accountId, from, to, theme, country);
				Log.Write("Mobile API", String.Format("Finished ROAS performance report request. Response contains {0} records", responce.PerformanceList.Count), LogMessageType.Debug);
				if (responce.PerformanceList.Count == 0)
				{
					return new RoasPerformanceResponse
					{
						HasError = true,
						ErrorMsg = "No data found",
						DisplayError = "No data found for ROAS performance report. Please refine report parameters."
					};
				}
				return responce;
			}
			catch (Exception ex)
			{
				Log.Write("Mobile API", String.Format("ROAS performance report failed, ex: {0}. Parameters: account={1}, from={2}, to={3}, theme={4}, country={5}",
										ex.Message, accountId, from, to, theme, country), LogMessageType.Error);
				return new RoasPerformanceResponse
				{
					HasError = true,
					ErrorMsg = ex.Message,
					DisplayError = (ex is MobileApiException) ? (ex as MobileApiException).DisplayMessage : "Failed to generate ROAS performance report, please contact Support@edge.bi (Error: 'General')."
				};
			}
		}

		[ActionName("CampaignPerformance")]
		public CampaignPerformanceResponse GetCampaignPerformance(string sessionId, int accountId, string from, string to, string theme = "", string country = "")
		{
			try
			{
				ValidatePermission(ValidateSession(sessionId), accountId);
				
				Log.Write("Mobile API", String.Format("Campaign performance report request. Parameters: account={0}, from={1}, to={2}, theme={3}, country={4}", accountId, from, to, theme, country), LogMessageType.Debug);
				var manager = GetManager();
				var responce = manager.GetCampaignPerformance(accountId, from, to, theme, country);
				Log.Write("Mobile API", String.Format("Finished Campaign performance report request. Response contains {0} records", responce.PerformanceList.Count), LogMessageType.Debug);
				if (responce.PerformanceList.Count == 0)
				{
					return new CampaignPerformanceResponse
					{
						HasError = true,
						ErrorMsg = "No data found",
						DisplayError = "No data found for Campaing performance report. Please refine report parameters."
					};
				} return responce;
			}
			catch (Exception ex)
			{
				Log.Write("Mobile API", String.Format("Campaign performance report failed, ex: {0}. Parameters: account={1}, from={2}, to={3}, theme={4}, country={5}",
										ex.Message, accountId, from, to, theme, country), LogMessageType.Error);
				return new CampaignPerformanceResponse
				{
					HasError = true,
					ErrorMsg = ex.Message,
					DisplayError = (ex is MobileApiException) ? (ex as MobileApiException).DisplayMessage : "Failed to generate Campaign performance report, please contact Support@edge.bi (Error: 'General')."
				};
			}
		}

		[ActionName("CountryPerformance")]
		public CampaignPerformanceResponse GetCountryPerformance(string sessionId, int accountId, string from, string to, string theme = "", string country = "")
		{
			try
			{
				ValidatePermission(ValidateSession(sessionId), accountId);

				Log.Write("Mobile API", String.Format("Country performance report request. Parameters: account={0}, from={1}, to={2}, theme={3}, country={4}", accountId, from, to, theme, country), LogMessageType.Debug);
				var manager = GetManager();
				var responce = manager.GetCountryPerformance(accountId, from, to, theme, country);
				Log.Write("Mobile API", String.Format("Finished Country performance report request. Response contains {0} records", responce.PerformanceList.Count), LogMessageType.Debug);
				if (responce.PerformanceList.Count == 0)
				{
					return new CampaignPerformanceResponse
					{
						HasError = true,
						ErrorMsg = "No data found",
						DisplayError = "No data found for Country performance report. Please refine report parameters."
					};
				} return responce;
			}
			catch (Exception ex)
			{
				Log.Write("Mobile API", String.Format("Country performance report failed, ex: {0}. Parameters: account={1}, from={2}, to={3}, theme={4}, country={5}",
										ex.Message, accountId, from, to, theme, country), LogMessageType.Error);
				return new CampaignPerformanceResponse
				{
					HasError = true,
					ErrorMsg = ex.Message,
					DisplayError = (ex is MobileApiException) ? (ex as MobileApiException).DisplayMessage : "Failed to generate Country performance report, please contact Support@edge.bi (Error: 'General')."
				};
			}
		}


		#region Private Methods
		private IPerformanceManager GetManager()
		{
			if (AppSettings.Get("MobileApi", "MockData", false) == "true")
			{
				return new MockPerformanceManager();
			}
			return new PerformanceManager();
		}

		private void ValidatePermission(int userId, int accountId)
		{
			// no permission validation if configured 
			if (AppSettings.Get("MobileApi", "ValidateSession", false) == "false") return;

			// real validation
			using (var connection = new SqlConnection(AppSettings.GetConnectionString("Edge.Core.Data.DataManager.Connection", "String")))
			{
				using (var sqlCommand = new SqlCommand("User_ValidatePermission", connection))
				{
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.Parameters.AddWithValue("@userId", userId);
					sqlCommand.Parameters.AddWithValue("@accountId", accountId);
					sqlCommand.Parameters.AddWithValue("@permissionType", "Performance");
					sqlCommand.Parameters.AddWithValue("@applicationType", "Mobile");
					connection.Open();

					var isValid = sqlCommand.ExecuteScalar().ToString();
					if (isValid != "1")
						throw new MobileApiException(String.Format("User {0} has not 'Performance' permission for account {1}", userId, accountId), 
													 String.Format("User {0} has not 'Performance' permission for account {1}", userId, accountId));
					
				}
			}
		}
		#endregion
	}
}