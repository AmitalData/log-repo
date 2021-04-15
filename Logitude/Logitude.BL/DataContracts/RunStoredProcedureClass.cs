using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
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
using System.Web;

namespace Logitude.BL.DataContracts
{
    public class RunStoredProcedureClass
    {
        public static void UpdateShipmentStatus(string shipmentId, int tenant)
        {
            string strConnString = GetConnection(tenant);
            using (SqlConnection cn = new SqlConnection(strConnString))
            {
                SqlCommand cmd = new SqlCommand("dbo.usp_ComputeShipmentStatus", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param1 = new SqlParameter("@ShipmentId", SqlDbType.VarChar);
                param1.Direction = ParameterDirection.Input;
                param1.Value = shipmentId;
                cmd.Parameters.Add(param1);

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }

        }


        public static  void UpdateCardSearcsRecords(string cardId, int tenant)
        {
            string strConnString = GetConnection(tenant);
            using (SqlConnection cn = new SqlConnection(strConnString))
            {
                SqlCommand cmd = new SqlCommand("usp_UpdateCardSearchFunction", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param1 = new SqlParameter("@CardId", SqlDbType.VarChar);
                param1.Direction = ParameterDirection.Input;
                param1.Value = cardId;
                cmd.Parameters.Add(param1);

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }



        public static void UpdateShipmentOperationalDate(string shipmentId, int tenant)
        {
            string strConnString = GetConnection(tenant);
            using (SqlConnection cn = new SqlConnection(strConnString))
            {
                SqlCommand cmd = new SqlCommand("dbo.usp_UpdateShipmentOperationalDate", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param1 = new SqlParameter("@ShipmentId", SqlDbType.VarChar);
                param1.Direction = ParameterDirection.Input;
                param1.Value = shipmentId;
                cmd.Parameters.Add(param1);

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public static void DeleteQBOTranslations(int tenant)
        {
            string strConnString = GetConnection(tenant);
            using (SqlConnection cn = new SqlConnection(strConnString))
            {
                SqlCommand cmd = new SqlCommand("dbo.usp_DeleteQBOTranslations", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param1 = new SqlParameter("@Tenant", SqlDbType.VarChar);
                param1.Direction = ParameterDirection.Input;
                param1.Value = tenant;
                cmd.Parameters.Add(param1);

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
        public static void UpdateShipmentFinalArrivalDate(string shipmentId, int tenant)
        {
            string strConnString = GetConnection(tenant);
            using (SqlConnection cn = new SqlConnection(strConnString))
            {
                SqlCommand cmd = new SqlCommand("dbo.usp_UpdateShipmentFinalArrivalDate", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param1 = new SqlParameter("@ShipmentId", SqlDbType.VarChar);
                param1.Direction = ParameterDirection.Input;
                param1.Value = shipmentId;
                cmd.Parameters.Add(param1);

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
        private static bool IsImportShipmentsAllowedForLogBox(TenantPM loggedTenant, ShipmentPM entityPM)
        {
            if (loggedTenant.CustomerTenantShareImportFile == true)
            {
                return (entityPM.DirectionId.ToUpper() == "I");
            }
            else
            {
                return false;
            }
        }

        private static bool IsExportShipmentsAllowedForLogBox(TenantPM loggedTenant, ShipmentPM entityPM)
        {
            if (loggedTenant.CustomerTenantShareExportFile == true) //&& FeatureToggleHelper.HasFeatureToggle("LEX", loggedTenant.Id)
            {
                return (entityPM.DirectionId.ToUpper() == "E" || entityPM.DirectionId.ToUpper() == "R");
            }
            else
            {
                return false;
            }
        }
        private static bool IsImporterTenantHasExportFeatureForExportShipments(int ImporterTenant, ShipmentPM entityPM)
        {
            if ((entityPM.DirectionId.ToUpper() == "E" || entityPM.DirectionId.ToUpper() == "R") && !FeatureToggleHelper.HasFeatureToggle("LEX", ImporterTenant))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static void CreateShipmentQueue(string shipmentId, int tenant)
        {
            try
            {

                var tenantQuery = new TenantQuery(tenant);
                var tenantPM = tenantQuery.GetSinglePM(tenant);
                var ShipmentQuery = new ShipmentQuery(tenant);
                var entityPM = ShipmentQuery.GetSinglePMWithoutComposition(shipmentId, tenant);
                if (entityPM != null && tenantPM != null)
                {
                    if (!tenantPM.IsDocumentsArchive && !entityPM.IsCancelled && tenantPM.IsCustomerTenantShare && (entityPM.DirectionId.ToUpper() == "C" || IsExportShipmentsAllowedForLogBox(tenantPM, entityPM) || IsImportShipmentsAllowedForLogBox(tenantPM, entityPM)))
                    {
                        CustomerTenantAccessQuery customerTenantAccessQuery = new CustomerTenantAccessQuery(tenant);
                        CustomerTenantAccessInfo customerTenantAccessInfo = customerTenantAccessQuery.GetCustomerTenantAccessInfo(tenant, entityPM.CustomerId);

                        if (customerTenantAccessInfo != null && customerTenantAccessInfo.HasAccess && IsImporterTenantHasExportFeatureForExportShipments(customerTenantAccessInfo.CustomerTenant, entityPM))
                        {
                            var ImporterTenant = customerTenantAccessInfo.CustomerTenant;
                            IQueueService queueservice = new DbQueueService();
                            queueservice.InitializeQueue("ImportersShipmentQueue", 0);
                            queueservice.Send(new Dictionary<string, string>() { { "ShipmentId", entityPM.Id }, { "Tenant", tenant.ToString() }, { "ImporterTenant", customerTenantAccessInfo.CustomerTenant.ToString() }, { "CustomerId", entityPM.CustomerId } }, tenant, null, entityPM.CustomerId);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }
                    ip = currentIP;
                }
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "web role", null, ip);
            }
        }

      
        public static void UpdateCustomConnectToShipment(string shipmentId, int tenant)
        {
            string strConnString = GetConnection(tenant);
            using (SqlConnection cn = new SqlConnection(strConnString))
            {
                SqlCommand cmd = new SqlCommand("dbo.usp_UpdateCustomConnectToShipment", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param1 = new SqlParameter("@ShipmentId", SqlDbType.VarChar);
                param1.Direction = ParameterDirection.Input;
                param1.Value = shipmentId;
                cmd.Parameters.Add(param1);

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
        public static void UpdateForeignPartnerCountryCode(string shipmentId, int tenant)
        {
            string strConnString = GetConnection(tenant);
            using (SqlConnection cn = new SqlConnection(strConnString))
            {
                SqlCommand cmd = new SqlCommand("dbo.usp_ComputeForeignPartnerCountryCode", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param1 = new SqlParameter("@ShipmentId", SqlDbType.VarChar);
                param1.Direction = ParameterDirection.Input;
                param1.Value = shipmentId;
                cmd.Parameters.Add(param1);

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
        public static void UpdateShipmentRegistryDate(string myShipmentId, int tenant)
        {
            string strConnString = GetConnection(tenant);
            using (SqlConnection cn = new SqlConnection(strConnString))
            {
                SqlCommand cmd = new SqlCommand("dbo.usp_UpdateShipmentRegistryDate", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param1 = new SqlParameter("@ShipmentId", SqlDbType.VarChar);
                param1.Direction = ParameterDirection.Input;
                param1.Value = myShipmentId;
                cmd.Parameters.Add(param1);

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
        public static void UpdateShipmentFirstApprovalDate(string myShipmentId, int tenant)
        {
            string strConnString = GetConnection(tenant);
            using (SqlConnection cn = new SqlConnection(strConnString))
            {
                SqlCommand cmd = new SqlCommand("dbo.usp_ComputeShipmentFirstApprovalDate", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param1 = new SqlParameter("@ShipmentId", SqlDbType.VarChar);
                param1.Direction = ParameterDirection.Input;
                param1.Value = myShipmentId;
                cmd.Parameters.Add(param1);

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public static void DDD()
        {

        }

        public static ShipmentsSummary GetShipmentsCounts(int tenant, string myDirectionId, string myTransportModeId, string loggedUserEmail, bool hasETDFeature, bool hasFollowupsFeature)
        {
            ShipmentsSummary myResult = new ShipmentsSummary();
            myResult.Id = tenant;

            if (string.IsNullOrEmpty(myDirectionId))
            {
                myDirectionId = ".";
            }

            if (string.IsNullOrEmpty(myTransportModeId))
            {
                myTransportModeId = ".";
            }

            string strConnString = GetConnection(tenant);
            using (SqlConnection cn = new SqlConnection(strConnString))
            {
                SqlCommand cmd = new SqlCommand("dbo.usp_GetShipmentsCounts", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter inParam1 = new SqlParameter("@Tenant", SqlDbType.Int);
                SqlParameter inParam2 = new SqlParameter("@DirectionId", SqlDbType.VarChar);
                SqlParameter inParam3 = new SqlParameter("@TransportModeId", SqlDbType.VarChar);
                SqlParameter inParam4 = new SqlParameter("@LoggedEmail", SqlDbType.VarChar);
                SqlParameter inParam5 = new SqlParameter("@HasETDFeature", SqlDbType.Bit);
                SqlParameter inParam6 = new SqlParameter("@HasFollowupsFeature", SqlDbType.Bit);
                inParam1.Direction = ParameterDirection.Input;
                inParam2.Direction = ParameterDirection.Input;
                inParam3.Direction = ParameterDirection.Input;
                inParam4.Direction = ParameterDirection.Input;
                inParam5.Direction = ParameterDirection.Input;
                inParam6.Direction = ParameterDirection.Input;
                inParam1.Value = tenant;
                inParam2.Value = myDirectionId;
                inParam3.Value = myTransportModeId;
                inParam4.Value = loggedUserEmail;
                inParam5.Value = hasETDFeature;
                inParam6.Value = hasFollowupsFeature;
                cmd.Parameters.Add(inParam1);
                cmd.Parameters.Add(inParam2);
                cmd.Parameters.Add(inParam3);
                cmd.Parameters.Add(inParam4);
                cmd.Parameters.Add(inParam5);
                cmd.Parameters.Add(inParam6);

                SqlParameter outParam1 = new SqlParameter("@OperationalOpen_Shipments", SqlDbType.Int);
                SqlParameter outParam2 = new SqlParameter("@OperationalOpen_ImportShipments", SqlDbType.Int);
                SqlParameter outParam3 = new SqlParameter("@OperationalOpen_Masters", SqlDbType.Int);
                SqlParameter outParam4 = new SqlParameter("@AccountingOpen_OpenReceivablesShipments", SqlDbType.Int);
                SqlParameter outParam5 = new SqlParameter("@AccountingOpen_OpenPayablesMasters", SqlDbType.Int);
                SqlParameter outParam6 = new SqlParameter("@EAWB_ExpectedDeparture", SqlDbType.Int);
                SqlParameter outParam7 = new SqlParameter("@EAWB_AirlineUpdates", SqlDbType.Int);
                SqlParameter outParam8 = new SqlParameter("@Others_AllFollowups", SqlDbType.Int);
                SqlParameter outParam9 = new SqlParameter("@Others_MyFollowups", SqlDbType.Int);
                SqlParameter outParam10 = new SqlParameter("@Others_CreditLimitBlocked", SqlDbType.Int);
                SqlParameter outParam11 = new SqlParameter("@FSR_FSRRequestLast7Days", SqlDbType.Int);
                outParam1.Direction = ParameterDirection.Output;
                outParam2.Direction = ParameterDirection.Output;
                outParam3.Direction = ParameterDirection.Output;
                outParam4.Direction = ParameterDirection.Output;
                outParam5.Direction = ParameterDirection.Output;
                outParam6.Direction = ParameterDirection.Output;
                outParam7.Direction = ParameterDirection.Output;
                outParam8.Direction = ParameterDirection.Output;
                outParam9.Direction = ParameterDirection.Output;
                outParam10.Direction = ParameterDirection.Output;
                outParam11.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outParam1);
                cmd.Parameters.Add(outParam2);
                cmd.Parameters.Add(outParam3);
                cmd.Parameters.Add(outParam4);
                cmd.Parameters.Add(outParam5);
                cmd.Parameters.Add(outParam6);
                cmd.Parameters.Add(outParam7);
                cmd.Parameters.Add(outParam8);
                cmd.Parameters.Add(outParam9);
                cmd.Parameters.Add(outParam10);
                cmd.Parameters.Add(outParam11);

                cn.Open();
                cmd.ExecuteNonQuery();

                myResult.OperationalOpenCount_DH = (int)cmd.Parameters["@OperationalOpen_Shipments"].Value;
                myResult.OperationalOpenCount_DC = (int)cmd.Parameters["@OperationalOpen_Masters"].Value;
                myResult.ImportShipmentsCount = (int)cmd.Parameters["@OperationalOpen_ImportShipments"].Value;
                myResult.AccountingOpenCount_DH = (int)cmd.Parameters["@AccountingOpen_OpenReceivablesShipments"].Value;
                myResult.AccountingOpenCount_DC = (int)cmd.Parameters["@AccountingOpen_OpenPayablesMasters"].Value;
                myResult.OperationalOpenCount_ETD = (int)cmd.Parameters["@EAWB_ExpectedDeparture"].Value;
                myResult.OperationalOpenCount_LWU = (int)cmd.Parameters["@EAWB_AirlineUpdates"].Value;
                myResult.AllFollowUpsCount = (int)cmd.Parameters["@Others_AllFollowups"].Value;
                myResult.MyFollowUpsCount = (int)cmd.Parameters["@Others_MyFollowups"].Value;
                myResult.CreditLimitBlockedCount = (int)cmd.Parameters["@Others_CreditLimitBlocked"].Value;
                myResult.LastSentFSRCount = (int)cmd.Parameters["@FSR_FSRRequestLast7Days"].Value;

                cn.Close();
            }

            return myResult;
        }

        public static string GetConnection(int tenant)
        {
            GlobalDB currentDb;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                currentDb = GlobalDBRepository.GetGlobalDBByTenant(tenant);
                scope.Complete();
            }

            string dbConnectionInfo = currentDb.DBConnection;
            string dbSeconderyConnectionInfo = currentDb.SecondaryAzureDBConnection;

            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo, dbSeconderyConnectionInfo);
            WebFreightContext context = new WebFreightContext(connection);

            return context.Database.Connection.ConnectionString;// entityBuilder.ConnectionString;
        }

        public static void RunEreaseTenantData(int tenant, string procedureName)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                string strConnString = GetConnection(tenant);
                using (SqlConnection cn = new SqlConnection(strConnString))
                {
                    SqlCommand cmd = new SqlCommand(procedureName, cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter param1 = new SqlParameter("@Tenant", SqlDbType.Int);
                    param1.Direction = ParameterDirection.Input;
                    param1.Value = tenant;
                    cmd.Parameters.Add(param1);
                    cmd.CommandTimeout = 6000;

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }

                scope.Complete();
            }
        }

        public static void RunChangeSystemCurrencyProcedure(string procedureName, string currencyCode, int tenant)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                string strConnString = GetConnection(tenant);
                using (SqlConnection cn = new SqlConnection(strConnString))
                {
                    SqlCommand cmd = new SqlCommand(procedureName, cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter param1 = new SqlParameter("@NewCurrencyCode", SqlDbType.VarChar);
                    SqlParameter param2 = new SqlParameter("@Tenant", SqlDbType.Int);
                    param1.Direction = ParameterDirection.Input;
                    param2.Direction = ParameterDirection.Input;
                    param1.Value = currencyCode;
                    param2.Value = tenant;
                    cmd.Parameters.Add(param1);
                    cmd.Parameters.Add(param2);

                    cmd.CommandTimeout = 6000;

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();

                }

                scope.Complete();
            }
        }
    }
}
