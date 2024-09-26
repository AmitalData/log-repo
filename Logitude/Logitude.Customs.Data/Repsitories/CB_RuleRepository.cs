 
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
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System.Transactions;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System.Data.Common;
using Simplog.Data.InfrastructureModel;
using Logitude.Customs.Data.EntityLists;
using System.Data.SqlClient;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class CB_RuleRepository:IRepository<CB_Rule>
   {
        
		public List<CB_Rule> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public List<CB_RuleList> GetCustomsBookRulesData(int customsItemId)
        {
            try
            {
                List<CB_RuleList> results = new List<CB_RuleList>();
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
                            var result = new CB_RuleList
                            {
                                //CustomsItemID = reader["CustomsItemID"] != DBNull.Value ? (int)reader["CustomsItemID"] : 0,
                                //ID = reader["ID"] != DBNull.Value ? (int)reader["ID"] : 0,
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
   