using Confluent.Kafka;
using Devart.Data.Oracle;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs; 
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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
using System.Web.UI.WebControls;
namespace Logitude.Server.Tools.Helpers
{
    public partial class TableCounter
    {
        private static Object thisLock = new Object();
        public static Dictionary<string, string> counterState;
        public static string GetNumber(int tenant, string counterCode, string parameter1, string parameter2, Dictionary<string, string> additionalParameters = null, bool saveCounter = false)
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
                bool isCustomizedCounter = tableCounters.Where(c => c.IsCustomized).Any();
                if (isCustomizedCounter && FeatureToggleHelper.HasFeatureToggle("ICC", tenant) && additionalParameters != null && additionalParameters.ContainsKey("[CustomizeCounterParameter2]") && !string.IsNullOrEmpty(additionalParameters["[CustomizeCounterParameter2]"]))
                {
                    counterDef = tableCounters.Where(c => c.IsCustomized && c.Parameter2 == additionalParameters["[CustomizeCounterParameter2]"]).FirstOrDefault();
                }
                else counterDef = tableCounters.Where(c => c.Parameter1 == parameter1 && c.Parameter2 == parameter2).FirstOrDefault();
                scope1.Complete();
            }

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

            string prefix = null;
            if (counterDef.UniquePerPrefix)
            {
                prefix = counterDef.Prefix;
            }

            string branchCounterCode = null;
            if (FeatureToggleHelper.HasFeatureToggle("SPB", tenant) && counterDef.UsePerBranch && additionalParameters != null)
            {
                ValidateBranchCounterCode(additionalParameters);
                branchCounterCode = additionalParameters["[B]"];
            }

            int startNumber = counterDef.StartNumber;
            string number = null;
            string strConnString = GetConnection(tenant);
            string counterLastNumberValue = string.Empty;
            if (FeatureToggleHelper.HasFeatureToggle("LCP", tenant))
            {
                lock (thisLock)
                {
                    using (TransactionScope scope = TransactionFactory.GetNewReadCommittedTransaction())
                    {
                        counterLastNumberValue = ExecuteNextTableNumberValueProcedure(tenant, counter, prefix, startNumber, strConnString, branchCounterCode, saveCounter);
                        scope.Complete();
                    }
                }
            }
            else
            {
                counterLastNumberValue = ExecuteNextTableNumberValueProcedure(tenant, counter, prefix, startNumber, strConnString, branchCounterCode, saveCounter);
            }

            number = GetCounterLastNumberWithPrefixSuffix(counter, counterDef, tenant, counterLastNumberValue, additionalParameters);

            return number;
        }
        private static void ValidateBranchCounterCode(Dictionary<string, string> additionalParameters)
        {
            if (string.IsNullOrEmpty(additionalParameters["[BranchName]"]))
            {
                throw new Exception("Branch Field is required");
            }
            if (string.IsNullOrEmpty(additionalParameters["[B]"]))
            {
                throw new Exception("The Counter Code of the " + additionalParameters["[BranchName]"] + " Branch is required.");
            }
        }
        private static string ExecuteNextTableNumberValueProcedure(int tenant, Counter counter, string prefix, int startNumber, string strConnString, string branchCounterCode,bool saveCounter)
        {
#if false
            if (LogitudeSettings.IsCostomsDeploy)
            {
                return TableCounter.ExecuteNextTableNumberValueProcedureCustoms(tenant, counter, prefix, startNumber, strConnString);
            }
#endif
            string counterLastNumberValue;
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                using (OracleConnection cn = new OracleConnection(strConnString))
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = cn;
                    cmd.CommandText =
                    DbContextBaseUtil.GetStoredProcedureName("usp_GetNextTableNumberValue", LogitudeDBSchema.LOGITUDE_MAIN,
                    cmd.Connection.ConnectionString);
                    cmd.CommandType = CommandType.StoredProcedure;

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
                    
                    string procedureName = string.IsNullOrEmpty(branchCounterCode)   ? "dbo.usp_GetNextTableNumberValueWithSnapshotView" : "dbo.usp_GetNextTableNumberValueSupportBranchCounterCodeWithSnapshot";
                    SqlCommand cmd = new SqlCommand(procedureName, cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter lastValuePar = new SqlParameter("@pLastValue", SqlDbType.Int);
                    SqlParameter counterIdPar = new SqlParameter("@pCounterId", SqlDbType.NVarChar);
                    SqlParameter tenantPar = new SqlParameter("@pTenant", SqlDbType.Int);
                    SqlParameter prefixPar = new SqlParameter("@pPrefix", SqlDbType.NVarChar);
                    SqlParameter startNumberPar = new SqlParameter("@pStartNumber", SqlDbType.Int);
                    SqlParameter counterStateIdPar = new SqlParameter("@pCounterStateId", SqlDbType.Int);

                    lastValuePar.Direction = ParameterDirection.Output;
                    counterIdPar.Direction = ParameterDirection.Input;
                    tenantPar.Direction = ParameterDirection.Input;
                    prefixPar.Direction = ParameterDirection.Input;
                    startNumberPar.Direction = ParameterDirection.Input;
                    counterStateIdPar.Direction = ParameterDirection.Output;

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
                    cmd.Parameters.Add(counterStateIdPar);

                    AddbranchCounterCodeParameter(branchCounterCode, cmd);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                    counterLastNumberValue = cmd.Parameters["@pLastValue"].Value.ToString();
                    if(saveCounter)
                    {
                        SaveCounter(cmd.Parameters["@pCounterStateId"].Value.ToString(), tenant, counter.ObjectTableId, cmd.Parameters["@pLastValue"].Value.ToString());
                    }


                }
            }

            return counterLastNumberValue;
        }

        private static void AddbranchCounterCodeParameter(string branchCounterCode, SqlCommand cmd)
        {
            if (string.IsNullOrEmpty(branchCounterCode)) return;
            SqlParameter branchCounterCodePar = new SqlParameter("@pBranchCounterCode", SqlDbType.VarChar);
            branchCounterCodePar.Direction = ParameterDirection.Input;
            if (branchCounterCode != null)
            {
                branchCounterCodePar.Value = branchCounterCode;
            }
            else
            {
                branchCounterCodePar.Value = DBNull.Value;
            }
            cmd.Parameters.Add(branchCounterCodePar);
        }

        private static string GetCounterLastNumberWithPrefixSuffix(Counter counter, CounterDefinition counterDef, int tenant, string counterLastNumberValue, Dictionary<string, string> additionalParameters)
        {
            string counterPrefix = !string.IsNullOrEmpty(counterDef.Prefix) ? counterDef.Prefix : "";
            string counterSuffix = !string.IsNullOrEmpty(counterDef.Suffix) ? counterDef.Suffix : "";
         
            ResolveCounterPrefixSuffixVariables(tenant, additionalParameters, ref counterPrefix, ref counterSuffix);
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
            CounterDefinitionRepository counterDefinitionRep = new CounterDefinitionRepository(tenant);
            CounterRepository counterRepository = new CounterRepository(tenant);
            Counter counter = counterRepository.GetCounterByCode(counterCode, tenant);
            string counterId = counter.Id;

            List<CounterDefinition> tableCounters = counterDefinitionRep.GetCounterDefinitionsByCounterId(counterId, tenant).ToList();
            CounterDefinition counterDef = tableCounters.Where(c => c.Parameter1 == parameter1 && c.Parameter2 == parameter2).FirstOrDefault();

            string counterPrefix = !string.IsNullOrEmpty(counterDef.Prefix) ? counterDef.Prefix : "";
            string counterSuffix = !string.IsNullOrEmpty(counterDef.Suffix) ? counterDef.Suffix : "";

            ResolveCounterPrefixSuffixVariables(tenant, additionalParameters, ref counterPrefix, ref counterSuffix);

            return counterPrefix;
        }

        public static string GetConnection(int tenant)
        {
            GlobalDB currentDb;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                currentDb = GlobalDBRepository.GetGlobalDBByTenant(tenant);

            }
            string dbConnectionInfo = currentDb.DBConnection;
            string dbSeconderyConnectionInfo = currentDb.SecondaryAzureDBConnection;

            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo, dbSeconderyConnectionInfo);
            WebFreightContext context = new WebFreightContext(connection);

            return context.Database.Connection.ConnectionString;
        }

        public static void SaveCounter(string counterStateId, int tenant, string objectTableId, string lastValue)
        {
            counterState = counterState ?? new Dictionary<string, string>();


            string key = $"{counterStateId}_{tenant}_{objectTableId}";
            
            // Remove any existing keys that match the tenant and objectTableId
            counterState.Keys
             .Where(k => k.EndsWith($"_{tenant}_{objectTableId}"))
             .ToList()
             .ForEach(k => counterState.Remove(k));
            
            // Store the new value
            counterState[key] = lastValue;
            
        }

        public static bool DoesCounterDefinitionExist(string counterCode, int tenant, string parameter1)
        {
            var counterRepository = new CounterRepository(tenant);
            var definitionRepository = new CounterDefinitionRepository(tenant);

            var counter = counterRepository.GetCounterByCode(counterCode, tenant);
            if (counter?.Id == null)
                return false;

            var definitions = definitionRepository
                .GetCounterDefinitionsByCounterId(counter.Id, tenant);

            return definitions.Any(d => d.Parameter1 == parameter1 && d.StartNumber != -1);
        }
    }
}
