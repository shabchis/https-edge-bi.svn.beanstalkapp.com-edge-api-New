﻿using System.Collections.Generic;
using System.Data.SqlClient;
using Edge.Core.Configuration;
using Edge.Objects;

namespace Edge.Api.Mobile.Settings
{
	/// <summary>
	/// Manager for retrievals of System Settings (Measures, Themes, Countries, etc.)
	/// </summary>
	public class SettingsManager : ISettingsManager
	{
		public List<SegmentInfo> GetSegmentInfo(int accountId, SegmentType segmentType)
		{
			var list = new List<SegmentInfo>();
			using (var connection = new SqlConnection(AppSettings.GetConnectionString("Edge.Core.Data.DataManager.Connection", "String")))
			{
				using (var sqlCommand = new SqlCommand { Connection = connection })
				{
					sqlCommand.CommandText = "SELECT ValueId, Value FROM SegmentValue WHERE SegmentId=@segmentId AND (AccountId=@accountId or AccountId=-1)";
					sqlCommand.Parameters.AddWithValue("@accountId", accountId);
					sqlCommand.Parameters.AddWithValue("@segmentId", (int)segmentType);
					connection.Open();

					using (var reader = sqlCommand.ExecuteReader())
					{
						while (reader.Read())
							list.Add(new SegmentInfo { Id = int.Parse(reader["ValueId"].ToString()), Name = reader["Value"].ToString() });
					}
				}
			}
			return list;
		}
	}
}
