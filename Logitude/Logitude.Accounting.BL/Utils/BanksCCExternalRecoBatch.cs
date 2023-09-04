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
                string lastCheckedId = "";
                _retry = true;
                while (_retry)
                {
                    _retry = false;
                    using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(10)))
                    {
                        try
                        {
                            GetNextLTArgs nextLTArgs = new GetNextLTArgs()
                            {
                                Tenant = tenant,
                                AccountId = banksCCExternalRecoArg.AccountId,
                                ToAccountingDate = banksCCExternalRecoArg.ToAccountingDate,
                                LastCheckedId = lastCheckedId,
                                ThisTimeMadeCount = 0,
                                Stop = false,
                            };
                            TryOneTime(accContext, invContext, ref nextLTArgs, 9, tenant);
                            _TransactionsMade += nextLTArgs.ThisTimeMadeCount;
                            if (nextLTArgs.Stop)
                            {
                                _retry = false;
                            }

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



        private void TryOneTime(IAccountingContext accContext, IInvoiceContext invContext, ref GetNextLTArgs args, int timeoutinmin, int tenant)
        {
            var sw = Stopwatch.StartNew();
            APInvoiceRepository aPInvoiceRepository = new APInvoiceRepository(invContext);
            var aPInvoiceQuery = new APInvoiceQuery(aPInvoiceRepository);
            List<APInvoice_LT_DTO> aPInvoice_LT_list = aPInvoiceQuery.GetAPInvoicesForStatusUpdate(accContext, ref args);
            if (aPInvoice_LT_list != null && aPInvoice_LT_list.Count > 0)
            {
                Action<IEnumerable<APInvoice_LT_DTO>> ProcessOneBatch = oneBatch => ProcessOneBatchImpl(oneBatch, tenant);

                BanksCCExternalRecor.ProcessInBatches<APInvoice_LT_DTO>(aPInvoice_LT_list, 100, ProcessOneBatch, ref _retry);
            }
            if (sw.Elapsed.TotalMinutes > timeoutinmin)
            {
                string errorText = $"Timeout -Operate the method again ";
                _badList.Add(errorText);
                _retry = true;

            }

        }

        private void ProcessOneBatchImpl(IEnumerable<APInvoice_LT_DTO> aPInvoice_LT_list, int tenant)
        {

            //   List<string> aPInvoiceIdList = aPInvoice_LT_list.Select(aplt => aplt.APInvoiceId).ToList(); 
            //   APInvoiceQuery query = new APInvoiceQuery(aPInvoiceRepository);
            //   List<APInvoicePM> PMs = query.GetAllAPInvoicesPMsByIds(aPInvoiceIdList, tenant);
            //    if (PMs != null && PMs.Count > 0)
            //    {
            //  foreach (var aPInvoicePM in PMs)
            foreach (var one_item in aPInvoice_LT_list)
            {
                try
                {

                    //APInvoice_LT_DTO one_item = aPInvoice_LT_list.Where(item => item.APInvoiceId == aPInvoicePM.Id).FirstOrDefault();
                    if (one_item != null)
                    {
                        bool toUpdate = false;
                        string newStatusCode = "";
                        bool newIsClosed = false;
                        if (one_item.APInvoiceStatusCode != "AD" && ((one_item.LT_LocalAmountDebit != 0m && one_item.LT_OpenAmount == one_item.LT_LocalAmountDebit)
                                                        || (one_item.LT_LocalAmountDebit == 0m && one_item.LT_OpenAmount == one_item.LT_LocalAmountCredit * -1)))
                        {
                            newStatusCode = "AD";
                            newIsClosed = false;
                            toUpdate = true;
                        }

                        else if (one_item.APInvoiceStatusCode != "PP" && one_item.LT_OpenAmount != 0m)
                        {
                            newStatusCode = "PP";
                            newIsClosed = false;
                            toUpdate = true;
                        }
                        else if (one_item.APInvoiceStatusCode != "PD" && one_item.LT_OpenAmount == 0m)
                        {
                            newStatusCode = "PD";
                            newIsClosed = true;
                            toUpdate = true;
                        }
                        if (toUpdate)
                        {
                            //APInvoice poco = new APInvoice();
                            //APInvoiceMapping aPInvoiceMapping = new APInvoiceMapping();
                            //APInvoiceMapping.MapEntity(aPInvoicePM, poco, false);

                            //aPInvoiceRepository.Update(poco);
                            //aPInvoiceRepository.SubmitChanges();
                            Update_APInvoiceStatusCode(one_item.APInvoiceId, tenant, newStatusCode, newIsClosed);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorText = ex.Message;
                    if (ex.InnerException != null && !String.IsNullOrEmpty(ex.InnerException.Message))
                    {
                        errorText += ", " + ex.InnerException.Message;
                    }
                    _badList.Add(errorText);
                }
            }
            //     }
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

    public static class BanksCCExternalRecor
    {
        public static void ProcessInBatches<TItem>(this IEnumerable<TItem> items, int batchSize, Action<IEnumerable<TItem>> processBatch, ref bool retry)
        {

            //Batch the data
            var batches = items
              .Select((item, index) => new { item, index })
              .GroupBy(_ => _.index / batchSize, _ => _.item);
            var sw = Stopwatch.StartNew();
            //Process the batches.
            foreach (var batch in batches)
            {
                //Each batch would be IEnumerable<TItem>
                processBatch(batch);
                if (sw.Elapsed.TotalMinutes > 9)
                {
                    retry = true;
                    break;
                }
            }
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