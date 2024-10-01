using Devart.Data.Oracle;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.Customs.BL.Helpers
{
    public class CustomsStoredProcedures
    {
        public static void Declaration_SetIsPaymentProtested(string declarationId, int tenant, bool IsPaymentProtested)
        {
            string strConnString = GetConnection(tenant);
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                using (OracleConnection con = new OracleConnection(strConnString))
                {
                    //string cmd =
                    //    "update Declarations dest SET dest.IsPaymentProtested= case when exists (select 1 from declarationpaymentmethods t1 where t1.declarationid =dest.id) then 1 else 0 end " +
                    //    $"where dest.id = '{declarationId}' and dest.tenant={tenant}  ";

                    //cmd =
                    //    "declare  " +
                    //    "   n NUMBER(5) := 0; " +
                    //    "begin   " +
                    //    $"   select count(*) into n from declarationpaymentmethods t1 where t1.declarationid ='{declarationId}' and t1.tenant={tenant} ;   " +
                    //    "   if n>0 then 	" +
                    //    $"       update Declarations dest SET dest.IsPaymentProtested= 1 where dest.id = '{declarationId}' and dest.tenant={tenant}  ;    " +
                    //    "   else     " +
                    //    $"       update Declarations dest SET dest.IsPaymentProtested= 0 where dest.id = '{declarationId}' and dest.tenant={tenant}  ;   " +
                    //    "   end if; " +
                    //    "END";
                    int intIsPaymentProtested = 0;
                    if (IsPaymentProtested)
                    {
                        intIsPaymentProtested = 1;
                    }
                    string cmd =
                        $"update Declarations dest SET dest.IsPaymentProtested= {intIsPaymentProtested} where dest.id = '{declarationId}' and dest.tenant={tenant}  ";

                    OracleCommand sqlCommand = new OracleCommand(cmd, con);

                    con.Open();
                    sqlCommand.ExecuteNonQuery();
                    con.Close();
                }
            }
            else
            {
                int intIsPaymentProtested = 0;
                if (IsPaymentProtested)
                {
                    intIsPaymentProtested = 1;
                }
                using (SqlConnection cn = new SqlConnection(strConnString))
                {
                    string cmd =$"update Customs.Declarations SET IsPaymentProtested= {intIsPaymentProtested} where id = '{declarationId}' and tenant={tenant}  ";


                    SqlCommand sqlCommand = new SqlCommand(cmd, cn);

                    cn.Open();
                    sqlCommand.ExecuteNonQuery();
                    cn.Close();
                }
            }
        }
        public static void UpdateSupplierInvoiceItemsSequence(string declarationId, int counterKey, int tenant)
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
                    SqlCommand cmd = new SqlCommand("dbo.usp_UpdateInvoiceItemsSequence", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter param1 = new SqlParameter("@V_DeclarationId", SqlDbType.VarChar);
                    param1.Direction = ParameterDirection.Input;
                    param1.Value = declarationId;
                    cmd.Parameters.Add(param1);

                    SqlParameter param3 = new SqlParameter("@V_Tenant", SqlDbType.Int);
                    param3.Direction = ParameterDirection.Input;
                    param3.Value = tenant;
                    cmd.Parameters.Add(param3);

                    SqlParameter param2 = new SqlParameter("@V_CounterKey", SqlDbType.Int);
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

            string strConnString = GetConnection(tenant);

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
                    SqlCommand cmd = new SqlCommand("dbo.usp_UPDATEPARENTINVOICEITEMSEQ", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter param1 = new SqlParameter("@V_DeclarationId", SqlDbType.VarChar);
                    param1.Direction = ParameterDirection.Input;
                    param1.Value = declarationId;
                    cmd.Parameters.Add(param1);

                    SqlParameter param3 = new SqlParameter("@V_Tenant", SqlDbType.Int);
                    param3.Direction = ParameterDirection.Input;
                    param3.Value = tenant;
                    cmd.Parameters.Add(param3);

                    SqlParameter param2 = new SqlParameter("@V_CounterKey", SqlDbType.Int);
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
                    SqlCommand cmd = new SqlCommand("dbo.usp_CopySupplierInvoiceItemsForDeclaration", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter param1 = new SqlParameter("@V_SourceDeclarationId", SqlDbType.VarChar);
                    param1.Direction = ParameterDirection.Input;
                    param1.Value = sourceDeclarationId;
                    cmd.Parameters.Add(param1);

                    SqlParameter param2 = new SqlParameter("@V_TargetDeclarationId", SqlDbType.VarChar);
                    param2.Direction = ParameterDirection.Input;
                    param2.Value = targetDeclarationId;
                    cmd.Parameters.Add(param2);

                    SqlParameter param3 = new SqlParameter("@V_Tenant", SqlDbType.Int);
                    param3.Direction = ParameterDirection.Input;
                    param3.Value = tenant;
                    cmd.Parameters.Add(param3);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }
            }
        }
        public static void DeleteCommunicationLogs(int tenant, int days, string from__, string to__, string subject__, string CommunicationStatusTypeCodeListAsString,Boolean IsCustoms)
        {
            string strConnString = GetConnection(tenant);
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                using (OracleConnection cn = new OracleConnection(strConnString))
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = cn;
                    cmd.CommandTimeout = 1000000;
                    cmd.CommandText =
                        DbContextBaseUtil.GetStoredProcedureName("usp_deletecommlogs", LogitudeDBSchema.LOGITUDE_MAIN,
                    cmd.Connection.ConnectionString);
                    cmd.CommandType = CommandType.StoredProcedure;


                    OracleParameter parameter1 = new OracleParameter("p_days", OracleDbType.Integer);
                    OracleParameter parameter2 = new OracleParameter("p_from", OracleDbType.VarChar);
                    OracleParameter parameter3 = new OracleParameter("p_to", OracleDbType.VarChar);
                    OracleParameter parameter4 = new OracleParameter("p_subject", OracleDbType.VarChar);
                    OracleParameter parameter5 = new OracleParameter("p_commstatustypecodelist", OracleDbType.VarChar);
                    OracleParameter parameter6 = new OracleParameter("isCustoms", OracleDbType.Boolean);


                    parameter1.Direction = ParameterDirection.Input;
                    parameter2.Direction = ParameterDirection.Input;
                    parameter3.Direction = ParameterDirection.Input;
                    parameter4.Direction = ParameterDirection.Input;
                    parameter5.Direction = ParameterDirection.Input;
                    parameter6.Direction = ParameterDirection.Input;


                    parameter1.Value = days;
                    parameter2.Value = from__;
                    parameter3.Value = to__;
                    parameter4.Value = subject__;
                    parameter5.Value = CommunicationStatusTypeCodeListAsString;
                    parameter6.Value = IsCustoms;



                    cmd.Parameters.Add(parameter1);
                    cmd.Parameters.Add(parameter2);
                    cmd.Parameters.Add(parameter3);
                    cmd.Parameters.Add(parameter4);
                    cmd.Parameters.Add(parameter5);
                    cmd.Parameters.Add(parameter6);



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
                    // must do
                }
            }
        }
        public static void DeleteQueueMessageMoreDetails(int tenant, int days)
        {
            string strConnString = GetConnection(tenant);
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                using (OracleConnection cn = new OracleConnection(strConnString))
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = cn;
                    cmd.CommandTimeout = 1000000;
                    cmd.CommandText =
                        DbContextBaseUtil.GetStoredProcedureName("usp_DeleteQueMessMoreDet", LogitudeDBSchema.LOGITUDE_MAIN,
                    cmd.Connection.ConnectionString);
                    cmd.CommandType = CommandType.StoredProcedure;


                    OracleParameter parameter1 = new OracleParameter("days", OracleDbType.Integer);


                    parameter1.Direction = ParameterDirection.Input;



                    parameter1.Value = days;
                   



                    cmd.Parameters.Add(parameter1);




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
                    // must do
                }
            }
        }
        public static void DeleteCustomsRequestSheets(int tenant, int days, string from__, string to__, string subject__, string CommunicationStatusTypeCodeListAsString)
        {
            string strConnString = GetConnection(tenant);
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                using (OracleConnection cn = new OracleConnection(strConnString))
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = cn;
                    cmd.CommandTimeout = 1000000;
                    cmd.CommandText =
                        DbContextBaseUtil.GetStoredProcedureName("usp_deletecustomsrequestsheets", LogitudeDBSchema.LOGITUDE_MAIN,
                    cmd.Connection.ConnectionString);
                    cmd.CommandType = CommandType.StoredProcedure;


                    OracleParameter parameter1 = new OracleParameter("p_days", OracleDbType.Integer);
                    OracleParameter parameter2 = new OracleParameter("p_from", OracleDbType.VarChar);
                    OracleParameter parameter3 = new OracleParameter("p_to", OracleDbType.VarChar);
                    OracleParameter parameter4 = new OracleParameter("p_subject", OracleDbType.VarChar);
                    OracleParameter parameter5 = new OracleParameter("p_commstatustypecodelist", OracleDbType.VarChar);


                    parameter1.Direction = ParameterDirection.Input;
                    parameter2.Direction = ParameterDirection.Input;
                    parameter3.Direction = ParameterDirection.Input;
                    parameter4.Direction = ParameterDirection.Input;
                    parameter5.Direction = ParameterDirection.Input;



                    parameter1.Value = days;
                    parameter2.Value = from__;
                    parameter3.Value = to__;
                    parameter4.Value = subject__;
                    parameter5.Value = CommunicationStatusTypeCodeListAsString;



                    cmd.Parameters.Add(parameter1);
                    cmd.Parameters.Add(parameter2);
                    cmd.Parameters.Add(parameter3);
                    cmd.Parameters.Add(parameter4);
                    cmd.Parameters.Add(parameter5);




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
                    // must do
                }
            }
        }

        public static void DeleteComLogSteps(int tenant, int days, string from__, string to__, string subject__, string CommunicationStatusTypeCodeListAsString,int StepNumber)
        {
            string strConnString = GetConnection(tenant);
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                using (OracleConnection cn = new OracleConnection(strConnString))
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = cn;
                    cmd.CommandTimeout = 1000000;
                    cmd.CommandText =
                        DbContextBaseUtil.GetStoredProcedureName("usp_deletecomlogsteps", LogitudeDBSchema.LOGITUDE_MAIN,
                    cmd.Connection.ConnectionString);
                    cmd.CommandType = CommandType.StoredProcedure;


                    OracleParameter parameter1 = new OracleParameter("p_days", OracleDbType.Integer);
                    OracleParameter parameter2 = new OracleParameter("p_from", OracleDbType.VarChar);
                    OracleParameter parameter3 = new OracleParameter("p_to", OracleDbType.VarChar);
                    OracleParameter parameter4 = new OracleParameter("p_subject", OracleDbType.VarChar);
                    OracleParameter parameter5 = new OracleParameter("p_commstatustypecodelist", OracleDbType.VarChar);
                    OracleParameter parameter6 = new OracleParameter("p_stepnumber", OracleDbType.Integer);


                    parameter1.Direction = ParameterDirection.Input;
                    parameter2.Direction = ParameterDirection.Input;
                    parameter3.Direction = ParameterDirection.Input;
                    parameter4.Direction = ParameterDirection.Input;
                    parameter5.Direction = ParameterDirection.Input;
                    parameter6.Direction = ParameterDirection.Input;



                    parameter1.Value = days;
                    parameter2.Value = from__;
                    parameter3.Value = to__;
                    parameter4.Value = subject__;
                    parameter5.Value = CommunicationStatusTypeCodeListAsString;
                    parameter6.Value = StepNumber;



                    cmd.Parameters.Add(parameter1);
                    cmd.Parameters.Add(parameter2);
                    cmd.Parameters.Add(parameter3);
                    cmd.Parameters.Add(parameter4);
                    cmd.Parameters.Add(parameter5);
                    cmd.Parameters.Add(parameter6);




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
                    // must do
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
                    SqlCommand cmd = new SqlCommand("dbo.usp_CopySuppInvoiceItemCers", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter param1 = new SqlParameter("@V_SourceDeclarationId", SqlDbType.VarChar);
                    param1.Direction = ParameterDirection.Input;
                    param1.Value = sourceDeclarationId;
                    cmd.Parameters.Add(param1);

                    SqlParameter param2 = new SqlParameter("@V_TargetDeclarationId", SqlDbType.VarChar);
                    param2.Direction = ParameterDirection.Input;
                    param2.Value = targetDeclarationId;
                    cmd.Parameters.Add(param2);

                    SqlParameter param3 = new SqlParameter("@V_Tenant", SqlDbType.Int);
                    param3.Direction = ParameterDirection.Input;
                    param3.Value = tenant;
                    cmd.Parameters.Add(param3);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void UpdateQueueMessageTenantPriority(int tenant, string CourierMasterId, string InterfaceTypeCode, int TenantPriority)
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
                    DbContextBaseUtil.GetStoredProcedureName("usp_UpdateQueueMessagePrio2", LogitudeDBSchema.LOGITUDE_MAIN,
                    cmd.Connection.ConnectionString);
                    cmd.CommandType = CommandType.StoredProcedure;
                    /*
                      v_pLastNumber OUT VARCHAR2,
--                    v_pTableName IN VARCHAR2 
                     * */

                    OracleParameter parameter1 = new OracleParameter("v_Tenant", OracleDbType.Integer);
                    OracleParameter parameter2 = new OracleParameter("v_CourierMasterId", OracleDbType.VarChar);
                    OracleParameter parameter3 = new OracleParameter("v_InterfaceTypeCode", OracleDbType.VarChar);
                    OracleParameter parameter4 = new OracleParameter("v_TenantPriority", OracleDbType.Integer);

                    parameter1.Direction = ParameterDirection.Input;
                    parameter2.Direction = ParameterDirection.Input;
                    parameter3.Direction = ParameterDirection.Input;
                    parameter4.Direction = ParameterDirection.Input;

                    parameter1.Value = tenant;
                    parameter2.Value = CourierMasterId;
                    parameter3.Value = InterfaceTypeCode;
                    parameter4.Value = TenantPriority;

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
                    SqlCommand cmd = new SqlCommand("Customs.usp_UpdateQueueMessagePrio2", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter param1 = new SqlParameter("@Tenant", SqlDbType.Int);
                    param1.Direction = ParameterDirection.Input;
                    param1.Value = tenant;
                    cmd.Parameters.Add(param1);

                    SqlParameter param2 = new SqlParameter("@CourierMasterId", SqlDbType.VarChar);
                    param2.Direction = ParameterDirection.Input;
                    param2.Value = CourierMasterId;
                    cmd.Parameters.Add(param2);

                    SqlParameter param3 = new SqlParameter("@InterfaceTypeCode", SqlDbType.VarChar);
                    param3.Direction = ParameterDirection.Input;
                    param3.Value = InterfaceTypeCode;
                    cmd.Parameters.Add(param3);


                    SqlParameter param4 = new SqlParameter("@TenantPriority", SqlDbType.Int);
                    param4.Direction = ParameterDirection.Input;
                    param4.Value = TenantPriority;
                    cmd.Parameters.Add(param4);


                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }
            }

        }
        public static void UpdateCourierHawbFromExcel(int tenant, string userid, List<string> courierhawbsList, out List<CourierHawbFromExcel> notFoundDeclarations)
        {
            string strConnString = GetConnection(tenant);


            using (Devart.Data.Oracle.OracleConnection cn = new Devart.Data.Oracle.OracleConnection(strConnString))
            {
                // Open the connection
                cn.Open();
                notFoundDeclarations = new List<CourierHawbFromExcel>();
                try
                {
                    using (Devart.Data.Oracle.OracleCommand command = new Devart.Data.Oracle.OracleCommand("usp_UpdateCourierHawbFromExcel", cn))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add the input parameters
                        OracleParameter parameter1 = new OracleParameter("v_Tenant", OracleDbType.Integer);
                        OracleParameter parameter2 = new OracleParameter("v_userid", OracleDbType.VarChar);

                        parameter1.Direction = ParameterDirection.Input;
                        parameter2.Direction = ParameterDirection.Input;


                        parameter1.Value = tenant;
                        parameter2.Value = userid;

                        command.Parameters.Add(parameter1);
                        command.Parameters.Add(parameter2);



                        //input list
                        OracleParameter stringsParam = command.Parameters.Add("v_courierhawbList", OracleDbType.Array);
                        stringsParam.Direction = ParameterDirection.Input;
                        OracleArray array = new OracleArray("VARCHAR_TABLE", cn, courierhawbsList);
                        stringsParam.Value = array;



                        // Add the output parameter for not_found_declarations
                        Devart.Data.Oracle.OracleParameter notFoundDeclarationsParam = command.Parameters.Add("not_found_declarations", Devart.Data.Oracle.OracleDbType.Cursor);
                        notFoundDeclarationsParam.Direction = ParameterDirection.Output;

                        command.ExecuteNonQuery();

                        // Read the output cursor into a DataTable
                        using (Devart.Data.Oracle.OracleDataReader reader = ((Devart.Data.Oracle.OracleCursor)notFoundDeclarationsParam.Value).GetDataReader())
                        {
                            while (reader.Read())
                            {
                                var courierHawbFromExcel = new CourierHawbFromExcel();
                                courierHawbFromExcel.DeclarationId = reader.GetString(2);
                                courierHawbFromExcel.CourierHawb=(reader.GetString(4));
                                courierHawbFromExcel.ErrorMessage = (reader.GetString(5));
                                notFoundDeclarations.Add(courierHawbFromExcel);
                            }
                        }
                    }
                }
                catch (OracleException ex)
                {
                    System.Console.WriteLine("Exception: {0}", ex.ToString());
                    throw;
                }
                finally
                {
                    cn.Close();
                }
            }


        }
        public static void UpdateJouranlLinesLineNumber(string journalId, int tenant)
        {
                 string strConnString = GetConnection(tenant);
                    
                using (SqlConnection cn = new SqlConnection(strConnString))
                {
                    SqlCommand cmd = new SqlCommand("dbo.USP_UPDATEJOURANLLINESSEQUENCE", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter param1 = new SqlParameter("@V_JOURANLID", SqlDbType.VarChar);
                    param1.Direction = ParameterDirection.Input;
                    param1.Value = journalId;
                    cmd.Parameters.Add(param1);

                    SqlParameter param3 = new SqlParameter("@V_Tenant", SqlDbType.Int);
                    param3.Direction = ParameterDirection.Input;
                    param3.Value = tenant;
                    cmd.Parameters.Add(param3);

                   





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
            CustomContext context = new CustomContext(connection);

            return context.Database.Connection.ConnectionString;// entityBuilder.ConnectionString;
        }
    }
}
