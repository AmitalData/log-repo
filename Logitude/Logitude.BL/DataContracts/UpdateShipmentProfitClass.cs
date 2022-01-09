using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Transactions;


namespace Logitude.BL.DataContracts
{
    public class UpdateShipmentProfitClass
    {
        public static void UpdatePayables(string shipmentId, int tenant, bool isInvoiceUpdated, string invoiceId)
        {
            string strConnString = GetConnection(tenant);
            using (SqlConnection cn = new SqlConnection(strConnString))
            {
                SqlCommand cmd = new SqlCommand("dbo.usp_UpdatePayablesData", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param1 = new SqlParameter("@ShipmentId_PARAM", SqlDbType.VarChar);
                param1.Direction = ParameterDirection.Input;
                param1.Value = shipmentId;
                cmd.Parameters.Add(param1);

                SqlParameter param2 = new SqlParameter("@IsInvoiceUpdated_PARAM", SqlDbType.Bit);
                param2.Direction = ParameterDirection.Input;
                param2.Value = isInvoiceUpdated;
                cmd.Parameters.Add(param2);

                SqlParameter param3 = new SqlParameter("@InvoiceId_PARAM", SqlDbType.VarChar);
                param3.Direction = ParameterDirection.Input;
                param3.Value = invoiceId == null ? "no invoice" : invoiceId;
                cmd.Parameters.Add(param3);

                cmd.CommandTimeout = 6000;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
        public static void UpdateReceivables(string shipmentId, int tenant, bool isInvoiceUpdated)
        {
            string strConnString = GetConnection(tenant);
            using (SqlConnection cn = new SqlConnection(strConnString))
            {
                SqlCommand cmd = new SqlCommand("dbo.usp_UpdateReceivablesData", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param1 = new SqlParameter("@ShipmentId_PARAM", SqlDbType.VarChar);
                param1.Direction = ParameterDirection.Input;
                param1.Value = shipmentId;
                cmd.Parameters.Add(param1);

                SqlParameter param2 = new SqlParameter("@IsInvoiceUpdated_PARAM", SqlDbType.Bit);
                param2.Direction = ParameterDirection.Input;
                param2.Value = isInvoiceUpdated;
                cmd.Parameters.Add(param2);

                cmd.CommandTimeout = 6000;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
        public static void UpdateProfit(string shipmentId, int tenant)
        {
            string strConnString = GetConnection(tenant);
            using (SqlConnection cn = new SqlConnection(strConnString))
            {
                SqlCommand cmd = new SqlCommand("dbo.usp_UpdateShipmentProfit", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param1 = new SqlParameter("@ShipmentId", SqlDbType.VarChar);
                param1.Direction = ParameterDirection.Input;
                param1.Value = shipmentId;
                cmd.Parameters.Add(param1);

                cmd.CommandTimeout = 6000;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
        public static void UpdateProfitFunction(string shipmentId, int tenant, bool isConsoleShipment)
        {
            string strConnString = GetConnection(tenant);
            using (SqlConnection cn = new SqlConnection(strConnString))
            {
                SqlCommand cmd = new SqlCommand("dbo.usp_UpdateShipmentProfitFunction", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param1 = new SqlParameter("@Tenant", SqlDbType.Int);
                SqlParameter param2 = new SqlParameter("@ShipmentId", SqlDbType.VarChar);
                SqlParameter param3 = new SqlParameter("@IsConsoleShipment", SqlDbType.Bit);
                param1.Direction = ParameterDirection.Input;
                param2.Direction = ParameterDirection.Input;
                param3.Direction = ParameterDirection.Input;
                param1.Value = tenant;
                param2.Value = shipmentId;
                param3.Value = isConsoleShipment;
                cmd.Parameters.Add(param1);
                cmd.Parameters.Add(param2);
                cmd.Parameters.Add(param3);

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
        public static void UpdateShipmentARInvoices(string shipmentId, int tenant, string ConsolidationNumber = null)
        {
            string strConnString = GetConnection(tenant);
            using (SqlConnection cn = new SqlConnection(strConnString))
            {
                SqlCommand cmd = new SqlCommand("dbo.usp_UpdateShipmentARInvoices", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param1 = new SqlParameter("@ShipmentId", SqlDbType.VarChar);
                param1.Direction = ParameterDirection.Input;
                param1.Value = shipmentId;
                cmd.Parameters.Add(param1);

                SqlParameter param2 = new SqlParameter("@ConsolidationNumber", SqlDbType.VarChar);
                param2.Direction = ParameterDirection.Input;

                if (ConsolidationNumber == null)
                {
                    param2.Value = DBNull.Value;
                }

                else
                {
                    param2.Value = ConsolidationNumber;
                }

                cmd.Parameters.Add(param2);

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public static void UpdateConstituentShipment(string ConsolidationId, string ConstituentId, int tenant)
        {
            string strConnString = GetConnection(tenant);
            using (SqlConnection cn = new SqlConnection(strConnString))
            {
                SqlCommand cmd = new SqlCommand("dbo.usp_UpdateConstituentShipment", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param1 = new SqlParameter("@ConstituentId", SqlDbType.VarChar);
                param1.Direction = ParameterDirection.Input;
                param1.Value = ConstituentId;
                cmd.Parameters.Add(param1);

                SqlParameter param2 = new SqlParameter("@ConsolidationId", SqlDbType.VarChar);
                param2.Direction = ParameterDirection.Input;
                param2.Value = ConsolidationId;
                cmd.Parameters.Add(param2);

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public static string GetConnection(int tenant)
        {
            GlobalDB currentDb;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                //GlobalDBRep = new GlobalDBRepository();
                currentDb = GlobalDBRepository.GetGlobalDBByTenant(tenant);

            }
            string dbConnectionInfo = currentDb.DBConnection;
            string dbSeconderyConnectionInfo = currentDb.SecondaryAzureDBConnection;

            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo, dbSeconderyConnectionInfo);
            WebFreightContext context = new WebFreightContext(connection);

            return context.Database.Connection.ConnectionString;// entityBuilder.ConnectionString;
        }
    }
}
