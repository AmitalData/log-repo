using Devart.Data.Oracle;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Validators;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Def.EntityQueryServicesExt;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools.Contracts;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Utils;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.Customs.CustomsMessaging.Tasks
{
    public class EnqueueSucceedRequestsheets : ICustomsEnqueueSucceedRequestsheets
    {
        private readonly string procName = "QueueEnqueue_succeedRequSheets";

        public void StartRun(string taskId, int seedDefaultTenant)
        {

            // var customsSettingQueryService = new CustomsSettingQueryService(seedDefaultTenant);
            // var allCustomsSetting = customsSettingQueryService.GetAll();
            // allCustomsSetting.ForEach(t => RunPerTenant(t));

            string strConnString = TenantServerConfigration.GetDbConnection(seedDefaultTenant);

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
            {
                if (LogitudeSettings.DatabaseManagementSystem == "oracle")
                {
                    using (OracleConnection cn = new OracleConnection(strConnString))
                    {
                        OracleCommand cmd = new OracleCommand();
                        cmd.Connection = cn;
                        cmd.CommandText = DbContextBaseUtil.GetStoredProcedureName(procName, LogitudeDBSchema.LOGITUDE_MAIN, cmd.Connection.ConnectionString);
                        cmd.CommandType = CommandType.StoredProcedure;

                        try
                        {
                            cn.Open();
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex, string.Format("Failed to execute the procedure {0}", procName));
                           throw;
                        }
                    }
                }
                else
                {
                    using (SqlConnection cn = new SqlConnection(strConnString))
                    {
                        SqlCommand cmd = new SqlCommand("[dbo].[" + procName + "]", cn);
                        cmd.CommandType = CommandType.StoredProcedure;

                        try
                        {
                            cn.Open();
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex,string.Format("Failed to execute the procedure {0}", procName));
                            throw;
                        }
                    }
                }

                scope.Complete();
            }
        }
    }
}
