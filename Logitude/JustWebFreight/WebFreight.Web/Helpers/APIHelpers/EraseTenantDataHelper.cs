using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.Repsitories;
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
using System.Text;
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

            string procedureName = this.GetProcedureName(parameterArgs.Type);
            this.RunProcedure(parameterArgs, procedureName, BatchTaskExecution.Id);
        }

        private string GetProcedureName(string type)
        {
            string procedureName = "";
            switch (type)
            {
                case "B":
                    {
                        procedureName = "dbo.usp_DeleteBusinessRecords";
                        break;
                    }

                case "P":
                    {
                        procedureName = "dbo.usp_DeleteCustomerRecords";
                        break;
                    }

                case "T":
                    {
                        procedureName = "dbo.usp_DeleteTicketsRecords";
                        break;
                    }

                case "C":
                    {
                        procedureName = "dbo.usp_DeleteCRMRecords";
                        break;
                    }
            }

            return procedureName;
        }
        private void RunProcedure(EraseTenantDataArgs parameterArgs, string procedureName, string batchTaskId)
        {
            if (!string.IsNullOrEmpty(procedureName))
            {
                string strConnString = this.GetConnection(parameterArgs.EntityId);
                SqlConnection sqlConnection = new SqlConnection(strConnString);
                SqlCommand cmd = new SqlCommand(procedureName, sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    SqlParameter param1 = new SqlParameter("@Tenant", SqlDbType.VarChar);
                    param1.Direction = ParameterDirection.Input;
                    param1.Value = parameterArgs.EntityId;
                    cmd.Parameters.Add(param1);

                    sqlConnection.Open();
                    cmd.ExecuteNonQuery();
                }

                catch (SqlException ex)
                {
                    StringBuilder errorMessages = new StringBuilder();

                    for (int i = 0; i < ex.Errors.Count; i++)
                    {
                        errorMessages.Append("Index #" + i + "\n" +
                            "Message: " + ex.Errors[i].Message + "\n" +
                            "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                            "Source: " + ex.Errors[i].Source + "\n" +
                            "Procedure: " + ex.Errors[i].Procedure + "\n");

                        this.UpdateErrorMessage(errorMessages.ToString(), batchTaskId, parameterArgs.EntityId);
                    }
                }

                finally
                {
                    sqlConnection.Dispose();
                }
            }
        }
        private void UpdateErrorMessage(string errorMessage, string batchTaskExecutionId, int tenant)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                BatchTaskExecutionRepository iRepository = new BatchTaskExecutionRepository(tenant);
                BatchTaskExecution iBatchTaskExecution = iRepository.GetSingle(batchTaskExecutionId, tenant);
                if (iBatchTaskExecution != null)
                {
                    iBatchTaskExecution.ErrorLog = errorMessage;
                    iRepository.Update(iBatchTaskExecution);
                    iRepository.SubmitChanges();
                }

                scope.Complete();
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

            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo, dbSeconderyConnectionInfo);
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