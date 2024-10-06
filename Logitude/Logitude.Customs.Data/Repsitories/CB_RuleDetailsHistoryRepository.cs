 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using Logitude.Customs.Data.EntityLists;
using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System.Data.Common;
using System.Data.SqlClient;
using System.Transactions;
using System.Data;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class CB_RuleDetailsHistoryRepository:IRepository<CB_RuleDetailsHistory>
   {
        
		public List<CB_RuleDetailsHistory> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public List<CB_RuleDetailsHistoryList> GetCustomsBookRulesData(int customsItemId)
        {
            try
            {
                List<CB_RuleDetailsHistoryList> results = new List<CB_RuleDetailsHistoryList>();
                string strConnString = GetConnection(0);
                using (SqlConnection connection = new SqlConnection(strConnString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "usp_CustomsBookRulesDetails";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CustomsItemID", customsItemId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var result = new CB_RuleDetailsHistoryList
                            {
                                ID = reader["RD_ID"] != DBNull.Value ? (int)reader["RD_ID"] : 0,
                                RuleID = reader["RuleID"] != DBNull.Value ? (int)reader["RuleID"] : 0,
                                Title = reader["Title"] != DBNull.Value ? (string)reader["Title"] : null,
                                Rules = reader["Rules"] != DBNull.Value ? (string)reader["Rules"] : null,
                                UpdateDate = reader["UpdateDate"] != DBNull.Value ? (DateTime?)reader["UpdateDate"] : null,
                                ChangeRequestTypePriority = reader["ChangeRequestTypePriority"] != DBNull.Value ? (int)reader["ChangeRequestTypePriority"] : 0,
                                OrderinalPostion = reader["OrderinalPostion"] != DBNull.Value ? (int)reader["OrderinalPostion"] : 0,
                                EntityStatusID = reader["EntityStatusID"] != DBNull.Value ? (string)reader["EntityStatusID"] : null,
                                Parent_RuleDetailsHistoryID = reader["Parent_RuleDetailsHistoryID"] != DBNull.Value ? (int)reader["Parent_RuleDetailsHistoryID"] : 0,
                            };
                            results.Add(result);
                        }
                    }
                    connection.Close();
                }

                return results;
            }

            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        private static string GetConnection(int tenant)
        {
            GlobalDB currentDb;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                currentDb = GlobalDBRepository.GetGlobalDBByTenant(tenant);
                scope.Complete();
            }

            string dbConnectionInfo = currentDb.DBConnection;
            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo);
            WebFreightContext context = new WebFreightContext(connection);
            return context.Database.Connection.ConnectionString;
        }
    }

}
   