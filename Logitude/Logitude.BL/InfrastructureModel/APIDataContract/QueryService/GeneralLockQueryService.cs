using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel;
using Logitude.Server.Tools.Utils;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Data.SqlClient;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.InfrastructureModel.APIDataContract.ApiV1
{
    public partial class GeneralLockQueryService
	{

        IWebFreightContext context;
		GeneralLockQuery query;

        public GeneralLockQueryService(int tenant = 0)
        {
            context = WebFreightContext.GetContext(tenant);
            query = new GeneralLockQuery(tenant);
        }
		public GeneralLockPM CheckIsLocked(int tenant, string sessionId, string userId, string entityId, string objectTableName, bool isFromCahnge = false)
		{
			return null;

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
		public void DeleteGeneralLockByEntity(int tenant, string entityId, string objectTableName, string sessionId)
		{
			ObjectTableQuery tablesQuery = new ObjectTableQuery(tenant);

			ObjectTablePM objectTable = tablesQuery.GetObjectTableByNameOrId(objectTableName, tenant);


			var repo = new GeneralLockRepository(tenant);

			using (var scope = TransactionFactory.GetTransaction())
			{
				repo.FastDeleteGeneralLock(tenant, entityId, objectTable?.Id, sessionId);

				repo.SubmitChanges();
				scope.Complete();
			}
		}
		public void DeleteGeneralLock(bool isFromUi)
		{
			var repo = new GeneralLockRepository(0);

			using (var scope = TransactionFactory.GetTransaction())
			{
				repo.FastDeleteGeneralLock(isFromUi);

				repo.SubmitChanges();
				scope.Complete();
			}

		}
		public void DeleteGeneralLockBySessionId(int tenant ,string sessionId)
		{
			var repo = new GeneralLockRepository(tenant);

			using (var scope = TransactionFactory.GetTransaction())
			{
				string session = sessionId.Split('_')[0];
				repo.FastDeleteGeneralLockBySessionId(tenant,session);

				repo.SubmitChanges();
				scope.Complete();
			}

		}
		public void DeleteGeneralLockByGeneralKey(int tenant, string generalKey)
		{
			var repo = new GeneralLockRepository(tenant);

			using (var scope = TransactionFactory.GetTransaction())
			{
				
				repo.FastDeleteGeneralLockByGeneralKey(tenant, generalKey);

				repo.SubmitChanges();
				scope.Complete();
			}

		}
	}
}