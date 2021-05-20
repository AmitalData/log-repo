using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;

using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;


using System.Data.Common;
using Simplog.Data.InfrastructureModel;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using Devart.Data.Oracle;
using Simplog.Data.Helpers;

namespace Logitude.Server.Tools.Helpers
{
    public class TableCounter
    {
        public static string GetNumber(int tenant, string counterCode, string parameter1, string parameter2, Dictionary<string, string> additionalParameters = null)
        {
            Counter counter = null;
            CounterDefinition counterDef = null;
            List<CounterDefinition> tableCounters = null;
            using (TransactionScope scope1 = TransactionFactory.GetNewTransaction())
            {
                CounterRepository counterRepository = new CounterRepository(tenant);
                CounterDefinitionRepository counterDefRep = new CounterDefinitionRepository(tenant);
                counter = counterRepository.GetCounterByCode(counterCode, tenant);
                string counterId = counter.Id;
                tableCounters = counterDefRep.GetCounterDefinitionsByCounterId(counterId, tenant).ToList();
                counterDef = tableCounters.Where(c => c.Parameter1 == parameter1 && c.Parameter2 == parameter2).FirstOrDefault();
                scope1.Complete();
            }

            // to be deleted
            if (counterDef == null)
            {
                if (counterCode == "MAST" || counterCode == "SHIP" || counterCode == "QUOT")
                {
                    if (parameter1 == "D")
                    {
                        parameter1 = "E";
                        parameter2 = "A";
                        counterDef = tableCounters.Where(c => c.Parameter1 == parameter1 && c.Parameter2 == parameter2).FirstOrDefault();
                    }
                }
            }
            //******

            string prefix = null;
            if (counterDef.UniquePerPrefix)
            {
                prefix = counterDef.Prefix;
            }

            int startNumber = counterDef.StartNumber;
            string number = null;
            string counterLastNumberValue;
            string strConnString = GetConnection(tenant);//ConfigurationManager.ConnectionStrings["str"].ConnectionString;
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {

                using (OracleConnection cn = new OracleConnection(strConnString))
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = cn;
                    cmd.CommandText =
                        //LogitudeDBSchema.LOGITUDE_MAIN.ToString() + "." +  "usp_GetNextTableNumberValue";
                        DbContextBaseUtil.GetStoredProcedureName("usp_GetNextTableNumberValue", LogitudeDBSchema.LOGITUDE_MAIN,
                        cmd.Connection.ConnectionString);
                    cmd.CommandType = CommandType.StoredProcedure;
                    /*
                     v_pLastValue OUT NUMBER,
--    v_pTenant      IN NUMBER,
--    v_pCounterId   IN NVARCHAR2,
--    v_pPrefix      IN NVARCHAR2,
--    v_pStartNumber IN NUMBER */
                    try
                    {
                        OracleParameter lastValuePar = new OracleParameter("v_pLastValue", OracleDbType.Number);
                        OracleParameter counterIdPar = new OracleParameter("v_pCounterId", OracleDbType.NVarChar);
                        OracleParameter tenantPar = new OracleParameter("v_pTenant", OracleDbType.Number);
                        OracleParameter prefixPar = new OracleParameter("v_pPrefix", OracleDbType.NVarChar);
                        OracleParameter startNumberPar = new OracleParameter("v_pStartNumber", OracleDbType.Number);

                        lastValuePar.Direction = ParameterDirection.Output;
                        counterIdPar.Direction = ParameterDirection.Input;
                        tenantPar.Direction = ParameterDirection.Input;
                        prefixPar.Direction = ParameterDirection.Input;
                        startNumberPar.Direction = ParameterDirection.Input;

                        counterIdPar.Value = counter.Id;
                        tenantPar.Value = tenant;

                        startNumberPar.Value = startNumber;

                        if (prefix != null)
                        {
                            prefixPar.Value = prefix;
                        }
                        else
                        {
                            prefixPar.Value = DBNull.Value;
                        }

                        cmd.Parameters.Add(lastValuePar);
                        cmd.Parameters.Add(tenantPar);
                        cmd.Parameters.Add(counterIdPar);
                        cmd.Parameters.Add(prefixPar);
                        cmd.Parameters.Add(startNumberPar);


                        cn.Open();
                        cmd.ExecuteNonQuery();
                        cn.Close();
                        //number = (counterDef.Prefix != null ? counterDef.Prefix + cmd.Parameters["v_pLastValue"].Value : counterDef.Prefix + cmd.Parameters["v_pLastValue"].Value);
                        counterLastNumberValue = cmd.Parameters["v_pLastValue"].Value.ToString();


                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Exception: {0}", ex.ToString());
                        throw;
                    }

                    cn.Close();
                }


            }
            else
            {
                using (SqlConnection cn = new SqlConnection(strConnString))
                {
                    SqlCommand cmd = new SqlCommand("dbo.usp_GetNextTableNumberValue", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter lastValuePar = new SqlParameter("@pLastValue", SqlDbType.Int);
                    SqlParameter counterIdPar = new SqlParameter("@pCounterId", SqlDbType.NVarChar);
                    SqlParameter tenantPar = new SqlParameter("@pTenant", SqlDbType.Int);
                    SqlParameter prefixPar = new SqlParameter("@pPrefix", SqlDbType.NVarChar);
                    SqlParameter startNumberPar = new SqlParameter("@pStartNumber", SqlDbType.Int);

                    lastValuePar.Direction = ParameterDirection.Output;
                    counterIdPar.Direction = ParameterDirection.Input;
                    tenantPar.Direction = ParameterDirection.Input;
                    prefixPar.Direction = ParameterDirection.Input;
                    startNumberPar.Direction = ParameterDirection.Input;

                    counterIdPar.Value = counter.Id;
                    tenantPar.Value = tenant;

                    startNumberPar.Value = startNumber;

                    if (prefix != null)
                    {
                        prefixPar.Value = prefix;
                    }
                    else
                    {
                        prefixPar.Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(lastValuePar);
                    cmd.Parameters.Add(tenantPar);
                    cmd.Parameters.Add(prefixPar);
                    cmd.Parameters.Add(counterIdPar);
                    cmd.Parameters.Add(startNumberPar);


                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                    //number = (counterDef.Prefix != null ? counterDef.Prefix + cmd.Parameters["@pLastValue"].Value : counterDef.Prefix + cmd.Parameters["@pLastValue"].Value);
                    counterLastNumberValue = cmd.Parameters["@pLastValue"].Value.ToString();


                }


            }

            number = GetCounterLastNumberWithPrefixSuffix(counter, counterDef, tenant, counterLastNumberValue, additionalParameters);

            return number;
        }

        private static string GetCounterLastNumberWithPrefixSuffix(Counter counter, CounterDefinition counterDef, int tenant, string counterLastNumberValue, Dictionary<string, string> additionalParameters)
        {
            string counterPrefix = !string.IsNullOrEmpty(counterDef.Prefix) ? counterDef.Prefix : "";
            string counterSuffix = !string.IsNullOrEmpty(counterDef.Suffix) ? counterDef.Suffix : "";


            //if (!string.IsNullOrEmpty(counterPrefix))
            //{
            //number = (counterDef.Prefix != null ? counterDef.Prefix + cmd.Parameters["v_pLastValue"].Value : counterDef.Prefix + cmd.Parameters["v_pLastValue"].Value);

            //[MM],[YY] or [YYYY],[B]
            ResolveCounterPrefixSuffixVariables(tenant, additionalParameters, ref counterPrefix, ref counterSuffix);
            //YYYShipEEE (15 - 9) + 3 
            if (counterDef.CounterSize != null && counterDef.CounterSize.Value > 0 && (counterPrefix + counterLastNumberValue + counterSuffix).Length < counterDef.CounterSize.Value)
            {
                int sizeOfStartNumber = (counterDef.CounterSize.Value - (counterPrefix + counterSuffix).Length);
                counterLastNumberValue = counterLastNumberValue.ToString().PadLeft(sizeOfStartNumber, '0');
            }

            counterLastNumberValue = counterPrefix + counterLastNumberValue + counterSuffix;

            return counterLastNumberValue;
        }

        private static void ResolveCounterPrefixSuffixVariables(int tenant, Dictionary<string, string> additionalParameters, ref string counterPrefix, ref string counterSuffix)
        {
            DateTime date = TenantServerConfigration.GetCurrentDateTime(tenant);
            string MM = date.ToString("MM");
            string YY = date.ToString("yy");
            string YYYY = date.ToString("yyyy");

            counterPrefix = counterPrefix.Replace("[MM]", MM).Replace("[YY]", YY).Replace("[YYYY]", YYYY);
            counterSuffix = counterSuffix.Replace("[MM]", MM).Replace("[YY]", YY).Replace("[YYYY]", YYYY);
            if (additionalParameters != null)
            {
                foreach (var k in additionalParameters.Keys)
                {
                    if (k == "[B]" && !FeatureToggleHelper.HasFeatureToggle("BCC", tenant))
                    {
                        continue;
                    }

                    counterPrefix = counterPrefix.Replace(k, additionalParameters[k]);
                    counterSuffix = counterSuffix.Replace(k, additionalParameters[k]);
                }
            }

        }

        public static string GetCounterPrefix(int tenant, string counterCode, string parameter1, string parameter2, Dictionary<string, string> additionalParameters = null)
        {
            //ObjectTabelRepository tablesRepository = new ObjectTabelRepository();
            //ObjectTablePM table = tablesRepository.GetObjectTableByCode(objectTableName, tenant);

            CounterDefinitionRepository counterDefinitionRep = new CounterDefinitionRepository(tenant);
            CounterRepository counterRepository = new CounterRepository(tenant);
            Counter counter = counterRepository.GetCounterByCode(counterCode, tenant);
            //string objectTableId = counter.ObjectTableId;
            string counterId = counter.Id;

            List<CounterDefinition> tableCounters = counterDefinitionRep.GetCounterDefinitionsByCounterId(counterId, tenant).ToList();
            CounterDefinition counterDef = tableCounters.Where(c => c.Parameter1 == parameter1 && c.Parameter2 == parameter2).FirstOrDefault();

            string counterPrefix = !string.IsNullOrEmpty(counterDef.Prefix) ? counterDef.Prefix : "";
            string counterSuffix = !string.IsNullOrEmpty(counterDef.Suffix) ? counterDef.Suffix : "";

            ResolveCounterPrefixSuffixVariables(tenant, additionalParameters, ref counterPrefix, ref counterSuffix);
            //ResolveCounterPrefixVariables(counter, counterDef, tenant, counterLastNumberValue, additionalParameters);

            return counterPrefix;
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
