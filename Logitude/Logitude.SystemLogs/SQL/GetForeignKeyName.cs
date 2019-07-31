using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.SystemLogs.SQL
{
    public class GetForeignKeyName
    {
        public static string Execute(string baseTableName, string foreignTableName, string foreignColumnName, int tenant)
        {
            string strConnString = GetConnection(tenant);
            string keyName = "";

            using (SqlConnection cn = new SqlConnection(strConnString))
            {
                SqlCommand cmd = new SqlCommand("dbo.usp_GetForeignKeyName", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param1 = new SqlParameter("@baseTableName", SqlDbType.VarChar, 500);
                param1.Direction = ParameterDirection.Input;
                param1.Value = baseTableName;
                cmd.Parameters.Add(param1);

                SqlParameter param2 = new SqlParameter("@foreignTableName", SqlDbType.VarChar, 500);
                param2.Direction = ParameterDirection.Input;
                param2.Value = foreignTableName;
                cmd.Parameters.Add(param2);

                SqlParameter param3 = new SqlParameter("@foreignColumnName", SqlDbType.VarChar, 500);
                param3.Direction = ParameterDirection.Input;
                param3.Value = foreignColumnName;
                cmd.Parameters.Add(param3);

                SqlParameter param4 = new SqlParameter("@keyName", SqlDbType.VarChar, 500);
                param4.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(param4);

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();

                object myValue = param4.Value;
                if (myValue != null)
                {
                    keyName = myValue.ToString();
                }
            }

            return keyName;
        }

        private static string GetConnection(int tenant)
        {
            GlobalDB currentDb;

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                currentDb = GlobalDBRepository.GetGlobalDBByTenant(tenant);
            }

            string dbConnectionInfo = currentDb.DBConnection;
            string dbSeconderyConnectionInfo = currentDb.SecondaryAzureDBConnection;

            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo,dbSeconderyConnectionInfo);
            WebFreightContext context = new WebFreightContext(connection);

            return context.Database.Connection.ConnectionString;
        }
    }
}
