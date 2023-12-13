using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.CoreBL;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data;
using Logitude.Accounting.Data.Repositories;
using System.Diagnostics;
using Logitude.BL.InvoiceModel.EntityQueries;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.InvoiceModel;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.Server.Tools;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.BL.InvoiceModel.Tools.DataMapping;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.Helpers;
using System.Data.SqlClient;
using System.Data;


namespace Logitude.Accounting.BL.Utils
{
    public class BanksCCExternalRecoBatch
    {
        private string _ResponseText;
        private HttpStatusCode _StatusCode;
        private List<string> _badList;
        private int _TransactionsMade;
        private bool _retry;

        public BanksCCExternalRecoBatch()
        {
            _ResponseText = "";
            _StatusCode = HttpStatusCode.Accepted;
        }

        public string ResponseText()
        {
            return _ResponseText;
        }

        public HttpStatusCode StatusCode()
        {
            return _StatusCode;
        }
        public void RunBanksCCExternalReco(BanksCCExternalRecoArg banksCCExternalRecoArg)
        {
            try
            {
                int tenant = banksCCExternalRecoArg.Tenant;

                _badList = new List<string>();
                _TransactionsMade = 0;
                BatchTaskExecutionPM batchTaskExecutionPM = banksCCExternalRecoArg.BatchTask;
                BatchTaskExecutionUpdateService batchTaskExecutionUpdateService = null;
                if (batchTaskExecutionPM != null)
                {
                    batchTaskExecutionUpdateService = GetBatchTaskUpdateServiceInstance(tenant);
                }

                IAccountingContext accContext = AccountingContext.GetContext(tenant);
                IInvoiceContext invContext = InvoiceContext.GetContext(tenant);
                string accountingDateStr = banksCCExternalRecoArg.ToAccountingDate.ToString("yyyy-MM-dd HH:mm:ss.fff"); // '2018 - 09 - 04 05:37:31.370'
                                                                                                                        //  string lastCheckedId = "";
                _retry = true;
                while (_retry)
                {
                    _retry = false;
                    using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(3)))
                    {
                        try
                        {
                            Update_LT(tenant, accountingDateStr, banksCCExternalRecoArg.AccountId);



                            scope.Complete();
                        }
                        catch (Exception e)
                        {
                            throw;
                        }
                    }
                }



                _ResponseText = $"Made Invoices: {_TransactionsMade}, Errors: {String.Join(", \n", _badList.ToArray())}";
            }
            catch
            {
                throw;
            }
        }



        private static int Update_LT(int tenant, string accountingDateStr, string accountId)
        {

            string strConnString = TenantServerConfigration.GetDbConnection(tenant);

            using (SqlConnection connection = new SqlConnection(strConnString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                        "UPDATE LedgerTransactions SET IsExternalReconcile = 1 " +
                        "WHERE Tenant = @V_tenant and AccountingDate<@V_AccountingDateStr and AccountId = @V_AccountId and IsExternalReconcile = 0";


                    command.CommandType = CommandType.Text;

                    command.Parameters.Add("@V_tenant", SqlDbType.Int);
                    command.Parameters["@V_tenant"].Value = tenant;

                    command.Parameters.Add("@V_AccountingDateStr", SqlDbType.VarChar);
                    command.Parameters["@V_AccountingDateStr"].Value = accountingDateStr;

                    command.Parameters.Add("@V_AccountId", SqlDbType.VarChar);
                    command.Parameters["@V_AccountId"].Value = accountId;

                    //string tmp = command.CommandText.ToString();
                    //foreach (SqlParameter p in command.Parameters)
                    //{
                    //    tmp = tmp.Replace('@' + p.ParameterName.ToString(), "'" + p.Value.ToString() + "'");
                    //}

                    int rows = command.ExecuteNonQuery();
                    connection.Close();
                    return rows;
                }
            }

        }

        private BatchTaskExecutionUpdateService GetBatchTaskUpdateServiceInstance(int tenant)
        {
            IInfrastructureContext context = InfrastructureContext.GetContext(tenant);
            BatchTaskExecutionUpdateService batchTaskExecutionUpdateService = new BatchTaskExecutionUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
            return batchTaskExecutionUpdateService;
        }


    }

    public class BanksCCExternalRecoArg
    {
        public int Tenant { get; set; }
        public string AccountId { get; set; }
        public DateTime ToAccountingDate { get; set; }
        public bool Batch { get; set; }
        public BatchTaskExecutionPM BatchTask { get; set; }
    }
}