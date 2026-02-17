using Devart.Data.Oracle;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.Server.Tools.SQL
{
    public class ExecuteStoredProcedures
    {
        public static object Execute(string procedureName, int tenant, List<StoredProcedureParam> storedProcedureParams)
        {

            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                throw new Exception("to do ExecuteStoredProcedures.ExecuteOracle meanwhile there is only 1 ()real  Call"); 
                return ExecuteOracle(procedureName, tenant, storedProcedureParams);
            }
            else
            {
                return ExecuteMMSQL(procedureName, tenant, storedProcedureParams);
            }


        }

        public static string GetConnection(int tenant)
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

        static object ExecuteOracle(string procedureName, int tenant, List<StoredProcedureParam> storedProcedureParams)
        {
#if true
		    return null;
#else 
              
            string strConnString = GetConnection(tenant);
            object outValue = null;
           using (DbConnection cn = (CommonDataContext.GetContext(tenant) as DbContext).Database.Connection)
            {
                
                var cmd = new OracleCommand();
                cmd.Connection = cn as OracleConnection;
                cmd.CommandText = // "usp_UpdateQueueCommunicationLo";
                    procedureName.Substring(0, Math.Min(procedureName.Length, 30))
                cmd.CommandType = CommandType.StoredProcedure;


                foreach (StoredProcedureParam parameter in storedProcedureParams)
                {
                    OracleParameter param = ToOracleParameter(parameter);
                        ToOracleParamName(parameter.ParamName), parameter.ParamDBType, parameter.ParamSize);
                    param.Direction = parameter.Direction;
                    if (parameter.Value != null)
                    {
                        param.Value = parameter.Value;
                    }
                    cmd.Parameters.Add(param);
                }

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
                StoredProcedureParam outParam = storedProcedureParams.Where(d => d.Direction == ParameterDirection.Output).FirstOrDefault();
                if (outParam != null)
                {
                    outValue = cmd.Parameters[outParam.ParamName].Value;
                }
            }
            return outValue;
#endif

        }

        private static OracleParameter ToOracleParameter(StoredProcedureParam parameter)
        {
            var oracleParameter = new OracleParameter();
           
            return oracleParameter;
        }

    
        

        private static string ToOracleParamName(string p)
        {
            throw new NotImplementedException();
        }


        static object ExecuteMMSQL(string procedureName, int tenant, List<StoredProcedureParam> storedProcedureParams)
        {

            
            string strConnString = GetConnection(tenant);
            object outValue = null;
            using (SqlConnection cn = new SqlConnection(strConnString))
            {
                SqlCommand cmd = new SqlCommand(procedureName, cn);
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (StoredProcedureParam parameter in storedProcedureParams)
                {
                    SqlParameter param = new SqlParameter(parameter.ParamName, parameter.ParamDBType, parameter.ParamSize);
                    param.Direction = parameter.Direction;
                    if (parameter.Value != null)
                    {
                        param.Value = parameter.Value;
                    }
                    cmd.Parameters.Add(param);
                }

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
                StoredProcedureParam outParam = storedProcedureParams.Where(d => d.Direction == ParameterDirection.Output).FirstOrDefault();
                if (outParam != null)
                {
                    outValue = cmd.Parameters[outParam.ParamName].Value;
                }
            }
            return outValue;
        }
    }
    public class StoredProcedureParam
    {
        public string ParamName { get; set; }
        public SqlDbType ParamDBType { get; set; }
        public object Value { get; set; }
        public ParameterDirection Direction { get; set; }
        public int ParamSize { get; set; }
    }
}
