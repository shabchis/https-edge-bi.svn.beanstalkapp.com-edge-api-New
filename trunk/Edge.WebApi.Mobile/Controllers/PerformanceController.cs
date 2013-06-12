﻿using System;
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
				return manager.GetPerformance(accountId, from, to, theme, country);
			}
			catch (Exception ex)
			{
				Log.Write("Mobile API", String.Format("Daily performance report failed, ex: {0}. Parameters: account={1}, from={2}, to={3}, theme={4}, country={5}", 
										ex.Message, accountId, from, to, theme, country), LogMessageType.Error);
				return new DailyPerformanceResponse
				{
					HasError = true,
					ErrorMsg = ex.Message,
					DisplayError = (ex is MobileApiException) ? (ex as MobileApiException).DisplayMessage : "Failure in Daily Performance report"
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
				return manager.GetRoasPerformance(accountId, from, to, theme, country);
			}
			catch (Exception ex)
			{
				Log.Write("Mobile API", String.Format("ROAS performance report failed, ex: {0}. Parameters: account={1}, from={2}, to={3}, theme={4}, country={5}",
										ex.Message, accountId, from, to, theme, country), LogMessageType.Error);
				return new RoasPerformanceResponse
				{
					HasError = true,
					ErrorMsg = ex.Message,
					DisplayError = (ex is MobileApiException) ? (ex as MobileApiException).DisplayMessage : "Failure in ROAS Performance report"
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
				return manager.GetCampaignPerformance(accountId, from, to, theme, country);
			}
			catch (Exception ex)
			{
				Log.Write("Mobile API", String.Format("Campaign performance report failed, ex: {0}. Parameters: account={1}, from={2}, to={3}, theme={4}, country={5}",
										ex.Message, accountId, from, to, theme, country), LogMessageType.Error);
				return new CampaignPerformanceResponse
				{
					HasError = true,
					ErrorMsg = ex.Message,
					DisplayError = (ex is MobileApiException) ? (ex as MobileApiException).DisplayMessage : "Failure in Campaign Performance report"
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