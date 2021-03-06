﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;
using System.Data;
using Edge.Core.Data;
using System.Data.SqlClient;
using Edge.Core.Configuration;

namespace Edge.Objects
{
	[DataContract]
	[TableMap("User_GUI_UserGroup")]
	public class Group
	{
		[DataMember(Order = 0)]
		[FieldMap("GroupID", IsKey = true)]
		public int GroupID;

		[DataMember(Order = 1)]
		[FieldMap("Name")]
		public string Name;

		[DataMember(Order = 2)]
		[FieldMap("IsActive")]
		public bool IsActive;

		[DataMember(Order = 3)]
		[FieldMap("AccountAdmin")]
		public bool? IsAcountAdmin;

		[DataMember(Order=4)]
		[DictionaryMap(Command = "Group_AssignedPermission(@GroupID:Int)", IsStoredProcedure = true, ValueIsGenericList = true, KeyName = "AccountID", ValueFieldsName = "PermissionName,PermissionType,Value")]
		public Dictionary<int, List<AssignedPermission>> AssignedPermissions = new Dictionary<int, List<AssignedPermission>>();

		[DataMember(Order = 5)]
		[ListMap(Command = "SELECT T0.UserID,T1.Name FROM User_GUI_UserGroupUser T0 INNER JOIN User_GUI_User T1 ON T0.UserID=T1.UserID WHERE T0.GroupID=@GroupID:Int ORDER BY Name", IsStoredProcedure = false)]
		public List<User> Members = new List<User>();
		


		private static object CustomApply(FieldInfo info, IDataRecord reader)
		{
			throw new NotImplementedException();
		}

		public static List<Group> GetAllGroups()
		{
			List<Group> groups = new List<Group>();
			ThingReader<Group> thingReader;
			Func<FieldInfo, IDataRecord, object> customApply = CustomApply;
			using (SqlConnection conn = new SqlConnection(AppSettings.GetConnectionString("Edge.Core.Data.DataManager.Connection", "String")))
			{
				conn.Open();
				SqlCommand sqlCommand = DataManager.CreateCommand("SELECT GroupID,IsActive,Name,AccountAdmin FROM User_GUI_UserGroup ORDER BY Name");
				sqlCommand.Connection = conn;
				if (conn.State != ConnectionState.Open)
					conn.Open();

				thingReader = new ThingReader<Group>(sqlCommand.ExecuteReader(), CustomApply);
				while (thingReader.Read())
				{
					groups.Add((Group)thingReader.Current);
				}
			}
			if (groups != null && groups.Count > 0)
			{
				for (int i = 0; i < groups.Count; i++)
				{
					groups[i] = MapperUtility.ExpandObject<Group>(groups[i], customApply);
				}
			}
			return groups;
		}

		public static Group GetGroupByID(int groupID)
		{
			Group group = null;
			ThingReader<Group> thingReader;
			Func<FieldInfo, IDataRecord, object> customApply = CustomApply;
			using (SqlConnection conn = new SqlConnection(AppSettings.GetConnectionString("Edge.Core.Data.DataManager.Connection", "String")))
			{
				SqlCommand sqlCommand = DataManager.CreateCommand(@"SELECT GroupID,
																			Name,
																			IsActive,
																			AccountAdmin     
																			FROM User_GUI_UserGroup
																			WHERE GroupID=@GroupID:Int");
				sqlCommand.Parameters["@GroupID"].Value = groupID;
				sqlCommand.Connection = conn;
				conn.Open();

				thingReader = new ThingReader<Group>(sqlCommand.ExecuteReader(), CustomApply);
				if (thingReader.Read())
				{
					group = (Group)thingReader.Current;
				}
			}
			if (group != null)
			{
				group = MapperUtility.ExpandObject<Group>(group, customApply);
			}

			return group;
		}
		public void AssignUser(int userID)
		{
			using (SqlConnection conn = new SqlConnection(AppSettings.GetConnectionString("Edge.Core.Data.DataManager.Connection", "String")))
			{
				SqlCommand sqlCommand = DataManager.CreateCommand(@"INSERT INTO User_GUI_UserGroupUser
																	(GroupID,UserID)
																	VALUES
																	(@GroupID,@UserID)");
				sqlCommand.Connection = conn;
				conn.Open();
				sqlCommand.Parameters["@GroupID"].Value = this.GroupID;
				sqlCommand.Parameters["@UserID"].Value = userID;

				sqlCommand.ExecuteNonQuery();

			}
		}
		public static List<User> GetUserAssociateUsers(int ID)
		{
			List<User> associateUsers = new List<User>();

			using (SqlConnection conn = new SqlConnection(AppSettings.GetConnectionString("Edge.Core.Data.DataManager.Connection", "String")))
			{
				SqlCommand sqlCommand = DataManager.CreateCommand(@"SELECT DISTINCT  T0.UserID ,T1.Name
																	FROM User_GUI_UserGroupUser T0
																	INNER JOIN User_GUI_User T1 ON T0.UserID=T1.UserID 
																	WHERE GroupID=@GroupID:Int");
				sqlCommand.Connection = conn;
				conn.Open();
				sqlCommand.Parameters["@GroupID"].Value = ID;

				using (ThingReader<User> thingReader = new ThingReader<User>(sqlCommand.ExecuteReader(), null))
				{
					while (thingReader.Read())
					{
						associateUsers.Add((User)thingReader.Current);
					}

				}

			}
			return associateUsers;
		}

		public  int GroupOperations(SqlOperation sqlOperation)
		{
			SqlTransaction sqlTransaction = null;
			int returnValue = -1;
			try
			{
				//Insert/Update/Remove user (also this clean all permissions and assigned groups)
				string command = @"Group_Operations(@Action:Int,@Name:NvarChar,@AccountAdmin:bit,@IsActive:bit,@GroupID:Int)";
				SqlConnection sqlConnection = new SqlConnection(DataManager.ConnectionString);
				sqlConnection.Open();
				sqlTransaction = sqlConnection.BeginTransaction("SaveGroup");
				returnValue=this.GroupID = Convert.ToInt32(MapperUtility.SaveOrRemoveSimpleObject<Group>(command, CommandType.StoredProcedure, sqlOperation, this, sqlConnection, sqlTransaction));
				int lastAccountID = -999;
				bool AddFictive = true;
				//insert the new permission
				foreach (KeyValuePair<int, List<AssignedPermission>> assignedPermissionPerAccount in this.AssignedPermissions)
				{
					foreach (AssignedPermission assignedPermission in assignedPermissionPerAccount.Value)
					{
						if (assignedPermission.PermissionName != "FictivePermission")
						{
							if (lastAccountID == assignedPermissionPerAccount.Key)
								AddFictive = false;
							else
								AddFictive = true;
							SqlCommand sqlCommand = DataManager.CreateCommand("Permissions_Operations(@AccountID:Int,@TargetID:Int,@TargetIsGroup:Bit,@PermissionType:NvarChar,@Value:Bit,@AddFictive:Bit)", CommandType.StoredProcedure);
							sqlCommand.Connection = sqlConnection;
							sqlCommand.Transaction = sqlTransaction;
							sqlCommand.Parameters["@AccountID"].Value = assignedPermissionPerAccount.Key;
							sqlCommand.Parameters["@TargetID"].Value = this.GroupID;
							sqlCommand.Parameters["@TargetIsGroup"].Value = 1;
							sqlCommand.Parameters["@PermissionType"].Value = assignedPermission.PermissionType;
							sqlCommand.Parameters["@Value"].Value = assignedPermission.Value;
							sqlCommand.Parameters["@AddFictive"].Value = AddFictive;
							sqlCommand.ExecuteNonQuery();
							lastAccountID = assignedPermissionPerAccount.Key;
						}
					}
				}
				foreach (User user in this.Members)
				{
					SqlCommand sqlCommand = DataManager.CreateCommand(@"INSERT INTO User_GUI_UserGroupUser
																	(GroupID,UserID)
																	VALUES
																	(@GroupID:Int,@UserID:Int)");
					sqlCommand.Parameters["@GroupID"].Value = this.GroupID;
					sqlCommand.Parameters["@UserID"].Value = user.UserID;
					sqlCommand.Connection = sqlConnection;
					sqlCommand.Transaction = sqlTransaction;
					sqlCommand.ExecuteNonQuery();

				}

				sqlTransaction.Commit();


			}			
			catch (Exception ex)
			{
				if (sqlTransaction != null)
					sqlTransaction.Rollback();
				throw ex;
			}
			return returnValue;
				
		}
		
	}
}
