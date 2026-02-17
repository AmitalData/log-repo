using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.Server.Tools.SQL
{
    public class GetPrimaryKeyName
    {
        public static string Execute(string tableName, string columnName, int tenant)
        {
            string strConnString = GetConnection(tenant);
            string keyName = "";
            using (SqlConnection cn = new SqlConnection(strConnString))
            {
                SqlCommand cmd = new SqlCommand("dbo.usp_GetPrimaryKeyName", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param1 = new SqlParameter("@tableName", SqlDbType.VarChar, 500);
                param1.Direction = ParameterDirection.Input;
                param1.Value = tableName;
                cmd.Parameters.Add(param1);

                SqlParameter param2 = new SqlParameter("@columnName", SqlDbType.VarChar, 500);
                param2.Direction = ParameterDirection.Input;
                param2.Value = columnName;
                cmd.Parameters.Add(param2);

                SqlParameter param3 = new SqlParameter("@keyName", SqlDbType.VarChar, 500);
                param3.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(param3);

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
                keyName = (string)cmd.Parameters["@keyName"].Value;
            }
            return keyName;
        }


        public static string ExecuteV2(string tableName, int tenant)
        {
            string strConnString = GetConnection(tenant);
            string keyName = "";
            using (SqlConnection cn = new SqlConnection(strConnString))
            {
                SqlCommand cmd = new SqlCommand("dbo.usp_GetPrimaryKeyName", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param1 = new SqlParameter("@tableName", SqlDbType.VarChar, 500);
                param1.Direction = ParameterDirection.Input;
                param1.Value = tableName;
                cmd.Parameters.Add(param1);

                SqlParameter param3 = new SqlParameter("@keyName", SqlDbType.VarChar, 500);
                param3.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(param3);

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
                keyName = (string)cmd.Parameters["@keyName"].Value;
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

            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo);
            WebFreightContext context = new WebFreightContext(connection);

            return context.Database.Connection.ConnectionString;
        }
    }
}
