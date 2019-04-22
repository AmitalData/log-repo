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
using System.Transactions;
using System.Web;

namespace WebFreight.Web.CommonDataModel
{
    public class CommonModelProcedureClass
    {
        public static void ExecuteSingleCustomerActualData(string customerId, int tenant)
        {
            string strConnString = GetConnection(tenant);

            using (SqlConnection cn = new SqlConnection(strConnString))
            {
                SqlCommand cmd = new SqlCommand("dbo.usp_UpdateCustomerActualData", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param1 = new SqlParameter("@CustomerId_PARAM", SqlDbType.VarChar);
                param1.Direction = ParameterDirection.Input;
                param1.Value = customerId;
                cmd.Parameters.Add(param1);

                SqlParameter param2 = new SqlParameter("@Tenant", SqlDbType.Int);
                param2.Direction = ParameterDirection.Input;
                param2.Value = tenant;
                cmd.Parameters.Add(param2);

                cmd.CommandTimeout = 10800; //3 Hours

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public static void ExecuteTenantCustomersActualData(int tenant)
        {
            string strConnString = GetConnection(tenant);
            using (SqlConnection cn = new SqlConnection(strConnString))
            {
                SqlCommand cmd = new SqlCommand("dbo.usp_UpdateTenantCustomersActualData", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param1 = new SqlParameter("@Tenant", SqlDbType.Int);
                param1.Direction = ParameterDirection.Input;
                param1.Value = tenant;
                cmd.Parameters.Add(param1);
                cmd.CommandTimeout = 18000; //3 Hours
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public static void ExecuteAllTenantsCustomersActualData()
        {
            int tenant = 0;

            string strConnString = GetConnection(tenant);
            using (SqlConnection cn = new SqlConnection(strConnString))
            {
                SqlCommand cmd = new SqlCommand("dbo.usp_UpdateAllTenantsCustomersActualData", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public static bool IsExistsCustomerActualDataHistory(int tenant, DateTime? startDateTime)
        {
            bool isExists = false;

            string strConnString = GetConnection(tenant);

            using (SqlConnection cn = new SqlConnection(strConnString))
            {
                SqlCommand cmd = new SqlCommand("select top 1 Id from CustomerActualDataHistory where Tenant = @Tenant and CONVERT(date,StartDateTime) = CONVERT(date,@StartDateTime)", cn);

                cmd.Parameters.AddWithValue("@Tenant", tenant);
                cmd.Parameters.AddWithValue("@StartDateTime", startDateTime);
 
                cn.Open();

                var iResult = cmd.ExecuteScalar();
                if (iResult != null)
                {
                    isExists = true;
                }

                cn.Close();
            }

            return isExists;
        }


        public static void InsertCustomerActualDataHistory(int tenant, DateTime? startDateTime, DateTime? endDateTime)
        {
            string strConnString = GetConnection(tenant);

            using (SqlConnection conn = new SqlConnection(strConnString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"insert into CustomerActualDataHistory(Tenant, StartDateTime, EndDateTime) VALUES(@Tenant, @StartDateTime, @EndDateTime)";

                    cmd.Parameters.AddWithValue("@Tenant", tenant);
                    cmd.Parameters.AddWithValue("@StartDateTime", startDateTime);
                    cmd.Parameters.AddWithValue("@EndDateTime", endDateTime);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        private static string GetConnection(int tenant)
        {
            GlobalDB currentDb;

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
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