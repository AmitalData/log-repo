using Devart.Data.Oracle;
using Logitude.Customs.Data;
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

namespace Logitude.Customs.BL.Helpers
{
    public class CustomsStoredProcedures
    {
        public static void UpdateSupplierInvoiceItemsSequence(string declarationId,int counterKey, int tenant)
        {
            string strConnString = GetConnection(tenant);
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {

                using (OracleConnection cn = new OracleConnection(strConnString))
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = cn;
                    cmd.CommandText =
                        //LogitudeDBSchema.LOGITUDE_MAIN.ToString() + "usp_GetNextTableIdValue";
                    DbContextBaseUtil.GetStoredProcedureName("usp_UpdateInvoiceItemsSequence", LogitudeDBSchema.LOGITUDE_MAIN,
                    cmd.Connection.ConnectionString);
                    cmd.CommandType = CommandType.StoredProcedure;
                    /*
                      v_pLastNumber OUT VARCHAR2,
--                    v_pTableName IN VARCHAR2 
                     * */

                    OracleParameter parameter1 = new OracleParameter("v_DeclarationId", OracleDbType.VarChar);
                    OracleParameter parameter2 = new OracleParameter("v_Tenant", OracleDbType.Integer);
                    OracleParameter parameter3 = new OracleParameter("v_CounterKey", OracleDbType.Integer);

                    parameter1.Direction = ParameterDirection.Input;
                    parameter2.Direction = ParameterDirection.Input;
                    parameter3.Direction = ParameterDirection.Input;

                    parameter1.Value = declarationId;
                    parameter2.Value = tenant;
                    parameter3.Value = counterKey;

                    cmd.Parameters.Add(parameter1);
                    cmd.Parameters.Add(parameter2);
                    cmd.Parameters.Add(parameter3);

                    try
                    {
                        cn.Open();
                        cmd.ExecuteNonQuery();
                        

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
                    SqlCommand cmd = new SqlCommand("Customs.usp_UpdateInvoiceItemsSequence", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter param1 = new SqlParameter("@DeclarationId", SqlDbType.VarChar);
                    param1.Direction = ParameterDirection.Input;
                    param1.Value = declarationId;
                    cmd.Parameters.Add(param1);

                    SqlParameter param3 = new SqlParameter("@Tenant", SqlDbType.Int);
                    param3.Direction = ParameterDirection.Input;
                    param3.Value = tenant;
                    cmd.Parameters.Add(param3);

                    SqlParameter param2 = new SqlParameter("@CounterKey", SqlDbType.Int);
                    param2.Direction = ParameterDirection.Input;
                    param2.Value = counterKey;
                    cmd.Parameters.Add(param2);





                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }
            }

        }
        public static void UpdateSupplierInvoiceItemsSequenceOracle(string declarationId, int counterKey, int tenant)
        {
            
            string strConnString=  GetConnection(tenant);

                using (var cn = new OracleConnection(strConnString))
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = cn;
                    cmd.CommandText =

                        DbContextBaseUtil.GetStoredProcedureName("usp_UpdateInvoiceItemsSequence", LogitudeDBSchema.LOGITUDE_MAIN, //for test
                        cmd.Connection.ConnectionString);
                //DbContextBaseUtil.GetStoredProcedureName("usp_UpdateInvoiceItemsSequenc1", LogitudeDBSchema.LOGITUDE_MAIN,
                //      cmd.Connection.ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                    /*
                     create or replace PROCEDURE usp_UpdateInvoiceItemsSequence(
    v_DeclarationId IN VARCHAR2 DEFAULT NULL ,
    v_Tenant        IN NUMBER DEFAULT NULL ,
    v_CounterKey    IN NUMBER DEFAULT NULL )
AS */
                    try
                    {
                        OracleParameter v_DeclarationId = new OracleParameter("v_DeclarationId", OracleDbType.VarChar);
                        OracleParameter v_Tenant = new OracleParameter("v_Tenant", OracleDbType.Number);
                        OracleParameter v_CounterKey = new OracleParameter("v_CounterKey", OracleDbType.Number);


                        v_DeclarationId.Direction = ParameterDirection.Input;
                        v_Tenant.Direction = ParameterDirection.Input;
                        v_CounterKey.Direction = ParameterDirection.Input;


                        v_DeclarationId.Value = declarationId;
                        v_Tenant.Value = tenant;
                        v_CounterKey.Value = counterKey;


                        cmd.Parameters.Add(v_DeclarationId);
                        cmd.Parameters.Add(v_Tenant);
                        cmd.Parameters.Add(v_CounterKey);
                        


                        cn.Open();
                        cmd.ExecuteNonQuery();
                        cn.Close();
                       
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Exception: {0}", ex.ToString());
                        throw;
                    }

                    cn.Close();
                }

                
            }

        public static void UpdateParentSupplierInvoiceItemsSequence(string declarationId, int counterKey, int tenant)
        {
            string strConnString = GetConnection(tenant);
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {

                using (OracleConnection cn = new OracleConnection(strConnString))
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = cn;
                    cmd.CommandText =
                    //LogitudeDBSchema.LOGITUDE_MAIN.ToString() + "usp_GetNextTableIdValue";
                    DbContextBaseUtil.GetStoredProcedureName("usp_UpdateParentInvoiceItemsSequence", LogitudeDBSchema.LOGITUDE_MAIN,
                    cmd.Connection.ConnectionString);
                    cmd.CommandType = CommandType.StoredProcedure;
                    /*
                      v_pLastNumber OUT VARCHAR2,
--                    v_pTableName IN VARCHAR2 
                     * */

                    OracleParameter parameter1 = new OracleParameter("v_DeclarationId", OracleDbType.VarChar);
                    OracleParameter parameter2 = new OracleParameter("v_Tenant", OracleDbType.Integer);
                    OracleParameter parameter3 = new OracleParameter("v_CounterKey", OracleDbType.Integer);

                    parameter1.Direction = ParameterDirection.Input;
                    parameter2.Direction = ParameterDirection.Input;
                    parameter3.Direction = ParameterDirection.Input;

                    parameter1.Value = declarationId;
                    parameter2.Value = tenant;
                    parameter3.Value = counterKey;

                    cmd.Parameters.Add(parameter1);
                    cmd.Parameters.Add(parameter2);
                    cmd.Parameters.Add(parameter3);

                    try
                    {
                        cn.Open();
                        cmd.ExecuteNonQuery();


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
                    SqlCommand cmd = new SqlCommand("Customs.usp_UpdateParentInvoiceItemsSequence", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter param1 = new SqlParameter("@DeclarationId", SqlDbType.VarChar);
                    param1.Direction = ParameterDirection.Input;
                    param1.Value = declarationId;
                    cmd.Parameters.Add(param1);

                    SqlParameter param3 = new SqlParameter("@Tenant", SqlDbType.Int);
                    param3.Direction = ParameterDirection.Input;
                    param3.Value = tenant;
                    cmd.Parameters.Add(param3);

                    SqlParameter param2 = new SqlParameter("@CounterKey", SqlDbType.Int);
                    param2.Direction = ParameterDirection.Input;
                    param2.Value = counterKey;
                    cmd.Parameters.Add(param2);





                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }
            }

        }
        public static void UpdateParentSupplierInvoiceItemsSequenceOracle(string declarationId, int counterKey, int tenant)
        {

            string strConnString = GetConnection(tenant);

            using (var cn = new OracleConnection(strConnString))
            {
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = cn;
                cmd.CommandText =

                    DbContextBaseUtil.GetStoredProcedureName("usp_UpdateParentInvoiceItemSeq", LogitudeDBSchema.LOGITUDE_MAIN,
                    cmd.Connection.ConnectionString);
                //DbContextBaseUtil.GetStoredProcedureName("usp_UpdateParentInvoiceItemSe1", LogitudeDBSchema.LOGITUDE_MAIN,
                //   cmd.Connection.ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                /*
                 create or replace PROCEDURE usp_UpdateInvoiceItemsSequence(
v_DeclarationId IN VARCHAR2 DEFAULT NULL ,
v_Tenant        IN NUMBER DEFAULT NULL ,
v_CounterKey    IN NUMBER DEFAULT NULL )
AS */
                try
                {
                    OracleParameter v_DeclarationId = new OracleParameter("v_DeclarationId", OracleDbType.VarChar);
                    OracleParameter v_Tenant = new OracleParameter("v_Tenant", OracleDbType.Number);
                    OracleParameter v_CounterKey = new OracleParameter("v_CounterKey", OracleDbType.Number);


                    v_DeclarationId.Direction = ParameterDirection.Input;
                    v_Tenant.Direction = ParameterDirection.Input;
                    v_CounterKey.Direction = ParameterDirection.Input;


                    v_DeclarationId.Value = declarationId;
                    v_Tenant.Value = tenant;
                    v_CounterKey.Value = counterKey;


                    cmd.Parameters.Add(v_DeclarationId);
                    cmd.Parameters.Add(v_Tenant);
                    cmd.Parameters.Add(v_CounterKey);



                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();

                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Exception: {0}", ex.ToString());
                    throw;
                }

                cn.Close();
            }


        }

        public static void CopySupplierInvoiceItems(string sourceDeclarationId, string targetDeclarationId, int tenant)
        {
            string strConnString = GetConnection(tenant);
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                using (OracleConnection cn = new OracleConnection(strConnString))
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = cn;
                    cmd.CommandText =
                      
                    DbContextBaseUtil.GetStoredProcedureName("usp_CopySupplierInvoiceItems", LogitudeDBSchema.LOGITUDE_MAIN,
                    cmd.Connection.ConnectionString);
                    cmd.CommandType = CommandType.StoredProcedure;
                   

                    OracleParameter parameter1 = new OracleParameter("v_SourceDeclarationId", OracleDbType.VarChar);
                    OracleParameter parameter2 = new OracleParameter("v_TargetDeclarationId", OracleDbType.VarChar);
                    OracleParameter parameter3 = new OracleParameter("v_Tenant", OracleDbType.Integer);
                    OracleParameter parameter4 = new OracleParameter("cv_1", OracleDbType.Cursor);

                    parameter1.Direction = ParameterDirection.Input;
                    parameter2.Direction = ParameterDirection.Input;
                    parameter3.Direction = ParameterDirection.Input;
                    parameter4.Direction = ParameterDirection.Output;
                    

                    parameter1.Value = sourceDeclarationId;
                    parameter2.Value = targetDeclarationId;
                    parameter3.Value = tenant;

                    cmd.Parameters.Add(parameter1);
                    cmd.Parameters.Add(parameter2);
                    cmd.Parameters.Add(parameter3);
                    cmd.Parameters.Add(parameter4);
                    

                    try
                    {
                        cn.Open();
                        cmd.ExecuteNonQuery();


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
                    SqlCommand cmd = new SqlCommand("Customs.usp_CopySupplierInvoiceItemsForDeclaration", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter param1 = new SqlParameter("@SourceDeclarationId", SqlDbType.VarChar);
                    param1.Direction = ParameterDirection.Input;
                    param1.Value = sourceDeclarationId;
                    cmd.Parameters.Add(param1);

                    SqlParameter param2 = new SqlParameter("@TargetDeclarationId", SqlDbType.VarChar);
                    param2.Direction = ParameterDirection.Input;
                    param2.Value = targetDeclarationId;
                    cmd.Parameters.Add(param2);

                    SqlParameter param3 = new SqlParameter("@Tenant", SqlDbType.Int);
                    param3.Direction = ParameterDirection.Input;
                    param3.Value = tenant;
                    cmd.Parameters.Add(param3);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }
            }
        }

        public static void CopySupplierInvoiceItemsCer(string sourceDeclarationId, string targetDeclarationId, int tenant)
        {
            string strConnString = GetConnection(tenant);
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                using (OracleConnection cn = new OracleConnection(strConnString))
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = cn;
                    cmd.CommandText =
                       
                    DbContextBaseUtil.GetStoredProcedureName("usp_CopySuppInvoiceItemCers", LogitudeDBSchema.LOGITUDE_MAIN,
                    cmd.Connection.ConnectionString);
                    cmd.CommandType = CommandType.StoredProcedure;
                 

                    OracleParameter parameter1 = new OracleParameter("v_SourceDeclarationId", OracleDbType.VarChar);
                    OracleParameter parameter2 = new OracleParameter("v_TargetDeclarationId", OracleDbType.VarChar);
                    OracleParameter parameter3 = new OracleParameter("v_Tenant", OracleDbType.Integer);
                    OracleParameter parameter4 = new OracleParameter("cv_1", OracleDbType.Cursor);

                    parameter1.Direction = ParameterDirection.Input;
                    parameter2.Direction = ParameterDirection.Input;
                    parameter3.Direction = ParameterDirection.Input;
                    parameter4.Direction = ParameterDirection.Output;

                    parameter1.Value = sourceDeclarationId;
                    parameter2.Value = targetDeclarationId;
                    parameter3.Value = tenant;

                    cmd.Parameters.Add(parameter1);
                    cmd.Parameters.Add(parameter2);
                    cmd.Parameters.Add(parameter3);
                    cmd.Parameters.Add(parameter4);

                    try
                    {
                        cn.Open();
                        cmd.ExecuteNonQuery();


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
                    SqlCommand cmd = new SqlCommand("Customs.usp_CopySupplierInvoiceItemsCerForDeclaration", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter param1 = new SqlParameter("@SourceDeclarationId", SqlDbType.VarChar);
                    param1.Direction = ParameterDirection.Input;
                    param1.Value = sourceDeclarationId;
                    cmd.Parameters.Add(param1);

                    SqlParameter param2 = new SqlParameter("@TargetDeclarationId", SqlDbType.VarChar);
                    param2.Direction = ParameterDirection.Input;
                    param2.Value = targetDeclarationId;
                    cmd.Parameters.Add(param2);

                    SqlParameter param3 = new SqlParameter("@Tenant", SqlDbType.Int);
                    param3.Direction = ParameterDirection.Input;
                    param3.Value = tenant;
                    cmd.Parameters.Add(param3);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }
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

            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo,dbSeconderyConnectionInfo);
            CustomContext context = new CustomContext(connection);

            return context.Database.Connection.ConnectionString;// entityBuilder.ConnectionString;
        }
    }
}
