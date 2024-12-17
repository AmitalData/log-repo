using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel;
using Logitude.Server.Tools.Utils;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Data.SqlClient;

namespace Logitude.BL.InfrastructureModel.APIDataContract.ApiV1
{
    public partial class GeneralLockQueryService
	{

        IWebFreightContext context;
		GeneralLockQuery query;

        public GeneralLockQueryService(int tenant)
        {
            context = WebFreightContext.GetContext(tenant);
            //service = new ObjectTableService(context, tenant); 
            query = new GeneralLockQuery(tenant);
        }
		public GeneralLockPM CheckIsLocked(int tenant, string sessionId, string userId, string entityId, string objectTableName, bool isFromCahnge = false)
		{
			var generalLockQuery = new GeneralLockQuery(tenant);
			ObjectTableQuery tablesQuery = new ObjectTableQuery(tenant);

			ObjectTablePM objectTable = tablesQuery.GetObjectTableByNameOrId(objectTableName, tenant);

			var lockPoco = generalLockQuery.GetSingleGeneralLock(tenant, entityId, objectTable?.Id);

			if (objectTable.IsLock)
			{
				if (lockPoco == null)
				{
					if (isFromCahnge)
					{
						string entityId2 = null;
						string objectTableId2 = null;
						if (objectTable.RelatedEntity != null && objectTable.ThisKey != null && objectTable.RelatedKey != null)
						{
							ObjectTablePM objectTable2 = tablesQuery.GetObjectTableByDBName(objectTable.RelatedEntity, tenant);
							if (objectTable2 != null && objectTable2.IsLock)
							{
								entityId2 = ExecuteQuery(tenant, entityId, objectTable);
								objectTableId2 = objectTable2.Id;
							}
						}
						var concurrentKiller = new ConcurrentKiller();
						concurrentKiller.LockByobjectAndUser(tenant, userId, entityId, objectTable?.Id, entityId2, objectTableId2, sessionId);
					}
				}
				else if (lockPoco.SessionId == sessionId)
				{
					lockPoco = null;
				}


			}
			return lockPoco;
		}


		public string ExecuteQuery(int tenant, string entityId, ObjectTablePM objectTable)
		{
			string strConnString = TenantServerConfigration.GetDbConnection(0);

			var result = string.Empty;

			//בכל אחד הראשון זה לפי מה לעשות את הקשר ביניהם והשני לפי מה להחזיר
			string[] ThisKey = objectTable.ThisKey.Split(',');
			string[] RelatedKey = objectTable.RelatedKey.Split(',');

			string sqlQuery = @"SELECT b." + RelatedKey[0] + @" as res
                                 FROM " + objectTable.DBTableName + @" AS a
                                 JOIN " + objectTable.RelatedEntity + @" AS b ON 
                                     a.tenant = b.tenant AND
                                     a." + ThisKey[1] + "= b." + RelatedKey[1] +
							 @" WHERE 
                                     a.tenant = " + tenant + @" AND 
                                     a." + ThisKey[0] + "= '" + entityId + "'";


			using (SqlConnection connection = new SqlConnection())
			{
				connection.ConnectionString = strConnString;
				connection.Open();
				using (var cmd = new SqlCommand(sqlQuery, connection))
				{
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							result = reader["res"].ToString();
						}
						connection.Close();
					}
				}
			}
			return result;
		}
		public void DeleteGeneralLock(int tenant, string entityId, string objectTableName, string sessionId)
		{
			var generalLockQuery = new GeneralLockQuery(tenant);
			ObjectTableQuery tablesQuery = new ObjectTableQuery(tenant);

			ObjectTablePM objectTable = tablesQuery.GetObjectTableByName(objectTableName, tenant);

			var concurrentKiller = new ConcurrentKiller();
			concurrentKiller.FreeGeneralLock(tenant, entityId, objectTable?.Id, sessionId);

		}
	}
}