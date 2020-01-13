using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Transactions;
using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System;
using Simplog.Server.Infrastructure.Helpers;
using Devart.Data.Oracle;
using System.Diagnostics;
using Logitude.Server.Tools.Helpers;

namespace Logitude.Server.Tools.Counters
{
    public class IdCounter //: IIdCounter
    {

        public static IIdCounter FakeOverrideIIdCounter { get; set; }

        public static string GetNumber(string connectionString, string tableName)
        {
            if (FakeOverrideIIdCounter!=null)
            {
                return FakeOverrideIIdCounter.GetNumber(connectionString, tableName);
            }
            string number = null;
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                
                using (OracleConnection cn = new OracleConnection(connectionString))
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = cn;
                    cmd.CommandText = 
                        //LogitudeDBSchema.LOGITUDE_MAIN.ToString() + "usp_GetNextTableIdValue";
                    DbContextBaseUtil.GetStoredProcedureName("usp_GetNextTableIdValue", LogitudeDBSchema.LOGITUDE_MAIN ,
                    cmd.Connection.ConnectionString);
                    cmd.CommandType = CommandType.StoredProcedure;
                    /*
                      v_pLastNumber OUT VARCHAR2,
--                    v_pTableName IN VARCHAR2 
                     * */

                    OracleParameter lastNumberPar = new OracleParameter("v_pLastNumber", OracleDbType.VarChar, 100);
                    OracleParameter tableNamePar = new OracleParameter("v_pTableName", OracleDbType.VarChar);

                    lastNumberPar.Direction = ParameterDirection.Output;
                    tableNamePar.Direction = ParameterDirection.Input;

                    tableNamePar.Value = tableName;

                    cmd.Parameters.Add(lastNumberPar);
                    cmd.Parameters.Add(tableNamePar);
                    try
                    {
                        cn.Open();
                        cmd.ExecuteNonQuery();
                        number = (string)cmd.Parameters["v_pLastNumber"].Value;

                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Exception: {0}", ex.ToString());
                        throw;
                    }

                    cn.Close();
                }

                return number;
            }

            else
            {

                string strConnString = connectionString;
            using (SqlConnection cn = new SqlConnection(strConnString))
            {


                SqlParameter lastNumberPar = new SqlParameter("@pLastNumber", SqlDbType.VarChar, 100);
                SqlParameter tableNamePar = new SqlParameter("@pTableName", SqlDbType.VarChar);

                lastNumberPar.Direction = ParameterDirection.Output;
                tableNamePar.Direction = ParameterDirection.Input;

                tableNamePar.Value = tableName;

                SqlCommand cmd = new SqlCommand("usp_GetNextTableIdValue", cn);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.Add(lastNumberPar);
                cmd.Parameters.Add(tableNamePar);
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
                number = (string)cmd.Parameters["@pLastNumber"].Value;

            }
            // scope.Complete();
            //}
            return number;

            }
        }

        private static Dictionary<string, Queue<string>> TablesKeyDic = new Dictionary<string, Queue<string>>();
        private static Queue<string> IdStoreQueue = new Queue<string>();

        private static SqlCommand cmd;
        private static Queue<string> tableIdsQueue = new Queue<string>();

        private static string GetIdsRangeFromDataBase(string tableName, string strConnString, int tenant)
        {

            int startNumber;
            int endNumber;
            string dbString;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                using (SqlConnection cn = new SqlConnection(strConnString))
                {

                    SqlCommand cmd = new SqlCommand("usp_GetNextTableIdsRange", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter startNumberPar = new SqlParameter("@pStartNumber", SqlDbType.Int);
                    SqlParameter endNumberPar = new SqlParameter("@pEndNumber", SqlDbType.Int);
                    SqlParameter dbStringNumber = new SqlParameter("@DBStringNumber", SqlDbType.VarChar, 50);
                    SqlParameter tableNamePar = new SqlParameter("@pTableName", SqlDbType.VarChar);
                    SqlParameter numberOfIds = new SqlParameter("@pNumberOfIds", SqlDbType.VarChar);


                    startNumberPar.Direction = ParameterDirection.Output;
                    endNumberPar.Direction = ParameterDirection.Output;
                    dbStringNumber.Direction = ParameterDirection.Output;
                    tableNamePar.Direction = ParameterDirection.Input;
                    numberOfIds.Direction = ParameterDirection.Input;

                    tableNamePar.Value = tableName;
                    numberOfIds.Value = 20;


                    cmd.Parameters.Add(startNumberPar);
                    cmd.Parameters.Add(numberOfIds);
                    cmd.Parameters.Add(endNumberPar);
                    cmd.Parameters.Add(tableNamePar);
                    cmd.Parameters.Add(dbStringNumber);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();

                    startNumber = (int)cmd.Parameters["@pStartNumber"].Value;
                    endNumber = (int)cmd.Parameters["@pEndNumber"].Value;
                    dbString = (string)cmd.Parameters["@DBStringNumber"].Value;

                    tableIdsQueue = new Queue<string>();
                    for (int i = startNumber; i <= endNumber; i++)
                    {
                        tableIdsQueue.Enqueue(dbString + "-" + i);
                    }


                    string id = tableIdsQueue.Dequeue();

                    if (TablesKeyDic.Keys.Contains(tableName))
                    {
                        TablesKeyDic[tableName] = tableIdsQueue;
                    }
                    else
                    {
                        TablesKeyDic.Add(tableName, tableIdsQueue);
                    }

                    return id;
                }
            }
        }
        private static Object thisLock = new Object();
        public static string GetNumber(string tableName, int tenant)
        {
            if (FakeOverrideIIdCounter != null)
            {
                return FakeOverrideIIdCounter.GetNumber(tableName, tenant);
            }
            string number = null;
            string strConnString = GetConnection(tenant);
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                
                var sw = Stopwatch.StartNew();
                lock (thisLock)
                    using (TransactionScope scope = TransactionFactory.GetNewReadCommittedTransaction())
                using (OracleConnection cn = new OracleConnection(strConnString))
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = cn;
                    cmd.CommandText = 
                        //LogitudeDBSchema.LOGITUDE_MAIN.ToString() + "." +   "usp_GetNextTableIdValue";
                    DbContextBaseUtil.GetStoredProcedureName("usp_GetNextTableIdValue", LogitudeDBSchema.LOGITUDE_MAIN,
                    cmd.Connection.ConnectionString);
                    cmd.CommandType = CommandType.StoredProcedure;
                    /*
                      v_pLastNumber OUT VARCHAR2,
           --                    v_pTableName IN VARCHAR2 
                     * */

                    OracleParameter lastNumberPar = new OracleParameter("v_pLastNumber", OracleDbType.VarChar, 100);
                    OracleParameter tableNamePar = new OracleParameter("v_pTableName", OracleDbType.VarChar);

                    lastNumberPar.Direction = ParameterDirection.Output;
                    tableNamePar.Direction = ParameterDirection.Input;

                    tableNamePar.Value = tableName;

                    cmd.Parameters.Add(lastNumberPar);
                    cmd.Parameters.Add(tableNamePar);
                    try
                    {
                        cn.Open();
                        cmd.ExecuteNonQuery();
                        number = (string)cmd.Parameters["v_pLastNumber"].Value;

                    }
                    catch (Exception ex)
            {
                        System.Console.WriteLine("Exception: {0}", ex.ToString());
                        throw;
                    }
                    scope.Complete();
                    cn.Close();
                }

                LogMessagingUtil.Instance.AppendLine($"GetNumber({tableName}):took:{sw.Elapsed}");
                return number;
            }

            else
            {
                lock (thisLock)
                {
                    using (TransactionScope scope = TransactionFactory.GetNewReadCommittedTransaction())
                {
                    using (SqlConnection cn = new SqlConnection(strConnString))
                    {


                        SqlParameter lastNumberPar = new SqlParameter("@pLastNumber", SqlDbType.VarChar, 100);
                        SqlParameter tableNamePar = new SqlParameter("@pTableName", SqlDbType.VarChar);

                        lastNumberPar.Direction = ParameterDirection.Output;
                        tableNamePar.Direction = ParameterDirection.Input;

                        tableNamePar.Value = tableName;

                        SqlCommand cmd = new SqlCommand("usp_GetNextTableIdValue", cn);
                        cmd.CommandType = CommandType.StoredProcedure;


                        cmd.Parameters.Add(lastNumberPar);
                        cmd.Parameters.Add(tableNamePar);
                        cn.Open();
                        cmd.ExecuteNonQuery();
                        cn.Close();
                        number = (string)cmd.Parameters["@pLastNumber"].Value;

                    }

                    scope.Complete();


                    return number;
                }
            }

            
            }
        }

//************************************************************************************************************************************
        //public
        static string GetConnection(int tenant)
        {
            GlobalDBRepository globalDbRep;
            GlobalDB currentDb;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                //GlobalDBRep = new GlobalDBRepository();
                currentDb = GlobalDBRepository.GetGlobalDBByTenant(tenant);

            }

            string dbConnectionInfo = currentDb.DBConnection;
            string dbSeconderyConnectionInfo = currentDb.SecondaryAzureDBConnection;

            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo,dbSeconderyConnectionInfo);
            WebFreightContext context = new WebFreightContext(connection);

            return context.Database.Connection.ConnectionString;// entityBuilder.ConnectionString;
        }


    }
    public interface IIdCounter
    {
        string GetNumber(string tableName, int tenant);

        string GetNumber(string connectionString, string tableName);
    }

    public class IdCounterWrapper : IIdCounter
    {
        public string GetNumber(string tableName, int tenant)
        {
            return IdCounter.GetNumber(tableName, tenant);
        }

        public string GetNumber(string connectionString, string tableName)
        {
            return IdCounter.GetNumber(connectionString ,tableName);
        }
    }
    public class FakeIdCounter : IIdCounter
    {
        public Func<string, int, string> GetNumberBytableNameStringtenantInt { get; set; }
        public string GetNumber(string tableName, int tenant)
        {
            return GetNumberBytableNameStringtenantInt(tableName, tenant);
        }

        public Func<string, string, string> GetNumberBystringConnectionStringStringtableName { get; set; }
        public string GetNumber(string connectionString, string tableName)
        {
            return GetNumberBystringConnectionStringStringtableName(connectionString, tableName);
        }
    }
}


    
