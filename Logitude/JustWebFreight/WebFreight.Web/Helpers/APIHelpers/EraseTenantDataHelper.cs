using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
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
using System.Transactions;
using System.Web;
using System.Xml.Serialization;

namespace WebFreight.Web.Helpers.APIHelpers
{
    public class EraseTenantDataHelper : BatchTaskExecutionsService
    {
        public EraseTenantDataHelper(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {

        }

        public override void RunCode()
        {
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(EraseTenantDataArgs));
            EraseTenantDataArgs parameterArgs = serializer.Deserialize(stringReader) as EraseTenantDataArgs;

            switch (parameterArgs.Type)
            {
                case "B":
                    {
                        string strConnString = this.GetConnection(parameterArgs.EntityId);
                        using (SqlConnection cn = new SqlConnection(strConnString))
                        {
                            SqlCommand cmd = new SqlCommand("dbo.usp_DeleteBusinessRecords", cn);
                            cmd.CommandType = CommandType.StoredProcedure;

                            SqlParameter param1 = new SqlParameter("@Tenant", SqlDbType.VarChar);
                            param1.Direction = ParameterDirection.Input;
                            param1.Value = parameterArgs.EntityId;
                            cmd.Parameters.Add(param1);

                            cn.Open();
                            cmd.ExecuteNonQuery();
                            cn.Close();
                        }

                        break;
                    }

                case "P":
                    {
                        string strConnString = this.GetConnection(parameterArgs.EntityId);
                        using (SqlConnection cn = new SqlConnection(strConnString))
                        {
                            SqlCommand cmd = new SqlCommand("dbo.usp_DeleteCustomerRecords", cn);
                            cmd.CommandType = CommandType.StoredProcedure;

                            SqlParameter param1 = new SqlParameter("@Tenant", SqlDbType.VarChar);
                            param1.Direction = ParameterDirection.Input;
                            param1.Value = parameterArgs.EntityId;
                            cmd.Parameters.Add(param1);

                            cn.Open();
                            cmd.ExecuteNonQuery();
                            cn.Close();
                        }

                        break;
                    }

                case "T":
                    {
                        string strConnString = this.GetConnection(parameterArgs.EntityId);
                        using (SqlConnection cn = new SqlConnection(strConnString))
                        {
                            SqlCommand cmd = new SqlCommand("dbo.usp_DeleteTicketsRecords", cn);
                            cmd.CommandType = CommandType.StoredProcedure;

                            SqlParameter param1 = new SqlParameter("@Tenant", SqlDbType.VarChar);
                            param1.Direction = ParameterDirection.Input;
                            param1.Value = parameterArgs.EntityId;
                            cmd.Parameters.Add(param1);

                            cn.Open();
                            cmd.ExecuteNonQuery();
                            cn.Close();
                        }

                        break;
                    }

                case "C":
                    {
                        string strConnString = this.GetConnection(parameterArgs.EntityId);
                        using (SqlConnection cn = new SqlConnection(strConnString))
                        {
                            SqlCommand cmd = new SqlCommand("dbo.usp_DeleteCRMRecords", cn);
                            cmd.CommandType = CommandType.StoredProcedure;

                            SqlParameter param1 = new SqlParameter("@Tenant", SqlDbType.VarChar);
                            param1.Direction = ParameterDirection.Input;
                            param1.Value = parameterArgs.EntityId;
                            cmd.Parameters.Add(param1);

                            cn.Open();
                            cmd.ExecuteNonQuery();
                            cn.Close();
                        }

                        break;
                    }
            }
        }

        public string GetConnection(int tenant)
        {
            GlobalDB currentDb;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                currentDb = GlobalDBRepository.GetGlobalDBByTenant(tenant);
                scope.Complete();
            }

            string dbConnectionInfo = currentDb.DBConnection;
            string dbSeconderyConnectionInfo = currentDb.SecondaryAzureDBConnection;

            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo,dbSeconderyConnectionInfo);
            WebFreightContext context = new WebFreightContext(connection);

            return context.Database.Connection.ConnectionString;
        }
    }

    public class EraseTenantDataArgs
    {
        public string Type { get; set; }
        public int EntityId { get; set; }
    }
}