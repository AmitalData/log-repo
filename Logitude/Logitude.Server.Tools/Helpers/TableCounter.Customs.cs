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
using System.Data.Entity;

namespace Logitude.Server.Tools.Helpers
{
    public partial class TableCounter
    {
#if false
        private static string ExecuteNextTableNumberValueProcedureCustoms(int tenant, Counter counter, string prefix, int startNumber, string strConnString)
        {
            if (!LogitudeSettings.IsCostomsDeploy)
            {
                throw new Exception("ReceiveCustoms  use in LogitudeSettings.IsCostomsDeploy");
            }
            string counterLastNumberValue = "";

            using (var context = WebFreightContext.GetContext(tenant) as DbContext)
            {
                var cmd = context.Database.Connection.CreateCommand();
                if (LogitudeSettings.DatabaseManagementSystem == "oracle")
                {
                    cmd.CommandText = DbContextBaseUtil.GetStoredProcedureName("usp_GetNextTableNumberValue", LogitudeDBSchema.LOGITUDE_MAIN, cmd.Connection.ConnectionString);
                }
                else
                {
                    cmd.CommandText = "[dbo].[usp_GetNextTableNumberValue]";
                }

                cmd.CommandType = CommandType.StoredProcedure;
                /*
                 v_pLastValue OUT NUMBER,
--    v_pTenant      IN NUMBER,
--    v_pCounterId   IN NVARCHAR2,
--    v_pPrefix      IN NVARCHAR2,
--    v_pStartNumber IN NUMBER */
                try
                {
                    //OracleParameter lastValuePar = new OracleParameter("v_pLastValue", OracleDbType.Number);
                    var lastValuePar = cmd.CreateParameter(); lastValuePar.ParameterName = "v_pLastValue"; lastValuePar.DbType = DbType.Double;
                    //OracleParameter counterIdPar = new OracleParameter("v_pCounterId", OracleDbType.NVarChar);
                    var counterIdPar = cmd.CreateParameter(); counterIdPar.ParameterName = "v_pCounterId"; counterIdPar.DbType = DbType.String;
                    //OracleParameter tenantPar = new OracleParameter("v_pTenant", OracleDbType.Number);
                    var tenantPar = cmd.CreateParameter(); tenantPar.ParameterName = "v_pTenant"; tenantPar.DbType = DbType.Double;
                    //OracleParameter prefixPar = new OracleParameter("v_pPrefix", OracleDbType.NVarChar);
                    var prefixPar = cmd.CreateParameter(); prefixPar.ParameterName = "V_PPREFIX"; prefixPar.DbType = DbType.String;
                    //OracleParameter startNumberPar = new OracleParameter("v_pStartNumber", OracleDbType.Number);
                    var startNumberPar = cmd.CreateParameter(); startNumberPar.ParameterName = "v_pStartNumber"; startNumberPar.DbType = DbType.Double;

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


                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    
                    //number = (counterDef.Prefix != null ? counterDef.Prefix + cmd.Parameters["v_pLastValue"].Value : counterDef.Prefix + cmd.Parameters["v_pLastValue"].Value);
                    counterLastNumberValue = cmd.Parameters["v_pLastValue"].Value.ToString();


                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Exception: {0}", ex.ToString());
                    throw;
                }
                finally
                {
                    cmd.Connection.Close();
                }

                
            }



            return counterLastNumberValue;
        }


#endif
    }
}
