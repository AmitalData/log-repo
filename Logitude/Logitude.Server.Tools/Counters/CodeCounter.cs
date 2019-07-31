using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Transactions;
using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using Simplog.Server.Infrastructure.Helpers;
using Devart.Data.Oracle;

namespace Logitude.Server.Tools.Counters
{
    public class CodeCounter
    {
        private static Object thisLock = new Object();
        public static int GetNumber(string tableName, int tenant)
        {
            int number = 0;
            string strConnString = GetConnection(tenant);
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {

                using (OracleConnection cn = new OracleConnection(strConnString))
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = cn;
                    cmd.CommandText = DbContextBaseUtil.GetStoredProcedureName("usp_GetNextTableCodeValue", LogitudeDBSchema.LOGITUDE_MAIN,
                        cmd.Connection.ConnectionString);
                    cmd.CommandType = CommandType.StoredProcedure;
                    /*
                      v_pLastNumber OUT NUMBER,
--    v_pTableName IN NVARCHAR2,
--    v_pTenant    IN NUMBER )
                     * */
                    try
                    {
                        OracleParameter lastNumberPar = new OracleParameter("v_pLastNumber", OracleDbType.Number);
                        OracleParameter tableNamePar = new OracleParameter("v_pTableName", OracleDbType.VarChar);
                        OracleParameter tenantPar = new OracleParameter("v_pTenant", OracleDbType.Number);


                        lastNumberPar.Direction = ParameterDirection.Output;
                        tableNamePar.Direction = ParameterDirection.Input;
                        tenantPar.Direction = ParameterDirection.Input;

                        tenantPar.Value = tenant;
                        tableNamePar.Value = tableName;

                        cmd.Parameters.Add(lastNumberPar);
                        cmd.Parameters.Add(tableNamePar);
                        cmd.Parameters.Add(tenantPar);
                        cn.Open();
                        cmd.ExecuteNonQuery();
                        cn.Close();
                        number = Convert.ToInt32(cmd.Parameters["v_pLastNumber"].Value);

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
                lock (thisLock)
                {
                    using (TransactionScope scope = TransactionFactory.GetNewReadCommittedTransaction())
                    {
                        //using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                        //{
                        using (SqlConnection cn = new SqlConnection(strConnString))
                        {
                            SqlCommand cmd = new SqlCommand("dbo.usp_GetNextTableCodeValue", cn);
                            cmd.CommandType = CommandType.StoredProcedure;

                            SqlParameter lastNumberPar = new SqlParameter("@pLastNumber", SqlDbType.Int);
                            SqlParameter tableNamePar = new SqlParameter("@pTableName", SqlDbType.NVarChar);
                            SqlParameter tenantPar = new SqlParameter("@pTenant", SqlDbType.Int);


                            lastNumberPar.Direction = ParameterDirection.Output;
                            tableNamePar.Direction = ParameterDirection.Input;
                            tenantPar.Direction = ParameterDirection.Input;

                            tenantPar.Value = tenant;
                            tableNamePar.Value = tableName;

                            cmd.Parameters.Add(lastNumberPar);
                            cmd.Parameters.Add(tenantPar);
                            cmd.Parameters.Add(tableNamePar);
                            cn.Open();
                            cmd.ExecuteNonQuery();
                            cn.Close();
                            number = (int)cmd.Parameters["@pLastNumber"].Value;

                            //}
                            //
                        }

                        scope.Complete();
                        return number;
                    }
                }

            }
        }
        public static int GetNumber_Ticket(string tableName, int tenant)
        {
            int number = 0;
            string strConnString = GetConnection(tenant);
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {

                using (OracleConnection cn = new OracleConnection(strConnString))
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = cn;
                    cmd.CommandText = DbContextBaseUtil.GetStoredProcedureName("usp_GetNextTableCodeValue", LogitudeDBSchema.LOGITUDE_MAIN,
                        cmd.Connection.ConnectionString);
                    cmd.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        OracleParameter lastNumberPar = new OracleParameter("v_pLastNumber", OracleDbType.Number);
                        OracleParameter tableNamePar = new OracleParameter("v_pTableName", OracleDbType.VarChar);
                        OracleParameter tenantPar = new OracleParameter("v_pTenant", OracleDbType.Number);


                        lastNumberPar.Direction = ParameterDirection.Output;
                        tableNamePar.Direction = ParameterDirection.Input;
                        tenantPar.Direction = ParameterDirection.Input;

                        tenantPar.Value = tenant;
                        tableNamePar.Value = tableName;

                        cmd.Parameters.Add(lastNumberPar);
                        cmd.Parameters.Add(tableNamePar);
                        cmd.Parameters.Add(tenantPar);
                        cn.Open();
                        cmd.ExecuteNonQuery();
                        cn.Close();
                        number = Convert.ToInt32(cmd.Parameters["v_pLastNumber"].Value);

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
                lock (thisLock)
                {
                    //using (TransactionScope scope = TransactionFactory.GetNewReadCommittedTransaction())
                    //{
                        //using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                        //{
                        using (SqlConnection cn = new SqlConnection(strConnString))
                        {
                            SqlCommand cmd = new SqlCommand("dbo.usp_GetNextTableCodeValue", cn);
                            cmd.CommandType = CommandType.StoredProcedure;

                            SqlParameter lastNumberPar = new SqlParameter("@pLastNumber", SqlDbType.Int);
                            SqlParameter tableNamePar = new SqlParameter("@pTableName", SqlDbType.NVarChar);
                            SqlParameter tenantPar = new SqlParameter("@pTenant", SqlDbType.Int);

                            lastNumberPar.Direction = ParameterDirection.Output;
                            tableNamePar.Direction = ParameterDirection.Input;
                            tenantPar.Direction = ParameterDirection.Input;

                            tenantPar.Value = tenant;
                            tableNamePar.Value = tableName;

                            cmd.Parameters.Add(lastNumberPar);
                            cmd.Parameters.Add(tenantPar);
                            cmd.Parameters.Add(tableNamePar);
                            cn.Open();
                            cmd.ExecuteNonQuery();
                            cn.Close();
                            number = (int)cmd.Parameters["@pLastNumber"].Value;
                            //}
                            //
                        }
                       // scope.Complete();
                        return number;
                   // }
                }

            }
        }
        public static int GetNumber(string tableName, int tenant, string connectionString)
        {

            int number = 0;


            string strConnString = connectionString;//ConfigurationManager.ConnectionStrings["str"].ConnectionString;
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {

                using (OracleConnection cn = new OracleConnection(strConnString))
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = cn;
                    cmd.CommandText = 
                    DbContextBaseUtil.GetStoredProcedureName("usp_GetNextTableCodeValue", LogitudeDBSchema.LOGITUDE_MAIN,
                    cmd.Connection.ConnectionString);
                    cmd.CommandType = CommandType.StoredProcedure;
                    /*
                      v_pLastNumber OUT NUMBER,
--    v_pTableName IN NVARCHAR2,
--    v_pTenant    IN NUMBER )
                     * */
                    try
                    {
                        OracleParameter lastNumberPar = new OracleParameter("v_pLastNumber", OracleDbType.Number);
                        OracleParameter tableNamePar = new OracleParameter("v_pTableName", OracleDbType.NVarChar);
                        OracleParameter tenantPar = new OracleParameter("v_pTenant", OracleDbType.Number);


                        lastNumberPar.Direction = ParameterDirection.Output;
                        tableNamePar.Direction = ParameterDirection.Input;
                        tenantPar.Direction = ParameterDirection.Input;

                        tenantPar.Value = tenant;
                        tableNamePar.Value = tableName;

                        cmd.Parameters.Add(lastNumberPar);
                        cmd.Parameters.Add(tenantPar);
                        cmd.Parameters.Add(tableNamePar);
                        cn.Open();
                        cmd.ExecuteNonQuery();
                        cn.Close();
                        number = (int)cmd.Parameters["@pLastNumber"].Value;

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
                using (SqlConnection cn = new SqlConnection(strConnString))
                {
                    SqlCommand cmd = new SqlCommand("dbo.usp_GetNextTableCodeValue", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter lastNumberPar = new SqlParameter("@pLastNumber", SqlDbType.Int);
                    SqlParameter tableNamePar = new SqlParameter("@pTableName", SqlDbType.NVarChar);
                    SqlParameter tenantPar = new SqlParameter("@pTenant", SqlDbType.Int);


                    lastNumberPar.Direction = ParameterDirection.Output;
                    tableNamePar.Direction = ParameterDirection.Input;
                    tenantPar.Direction = ParameterDirection.Input;

                    tenantPar.Value = tenant;
                    tableNamePar.Value = tableName;

                    cmd.Parameters.Add(lastNumberPar);
                    cmd.Parameters.Add(tenantPar);
                    cmd.Parameters.Add(tableNamePar);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                    number = (int)cmd.Parameters["@pLastNumber"].Value;

                }

                return number;

            }
        }
        public static string GetConnection(int tenant)
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


    public interface ICodeCounter
    {
        int GetNumber(string tableName, int tenant);
        int GetNumber(string tableName, int tenant, string connectionString);
    }
    public class CodeCounterWrapper : ICodeCounter
    {
        private bool _InNewTransaction;
        private static Object thisLock = new Object();

        public CodeCounterWrapper(bool inNewTransaction)
        {
            _InNewTransaction = inNewTransaction;
        }
        public int GetNumber(string tableName, int tenant)
        {
            if (!_InNewTransaction)
            {
                return CodeCounter.GetNumber(tableName, tenant);
            }
            int res;
            lock (thisLock)
            {
                using (TransactionScope scope = TransactionFactory.GetNewReadCommittedTransaction())
                {
                    res = CodeCounter.GetNumber(tableName, tenant);
                    scope.Complete();

                }
            }
            return res;
        }

        public int GetNumber(string tableName, int tenant, string connectionString)
        {
            

            if (!_InNewTransaction)
            {
                return CodeCounter.GetNumber(tableName, tenant, connectionString);
            }
            int res;
            lock (thisLock)
            {
                using (TransactionScope scope = TransactionFactory.GetNewReadCommittedTransaction())
                {
                    res = CodeCounter.GetNumber(tableName, tenant, connectionString);
                    scope.Complete();

                }
            }
            return res;
        }
    }

}