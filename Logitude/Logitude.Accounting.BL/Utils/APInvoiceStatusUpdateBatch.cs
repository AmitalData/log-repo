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

namespace Logitude.Accounting.BL.Utils
{
    public class APInvoiceStatusUpdateBatch
    {
        private string _ResponseText;
        private HttpStatusCode _StatusCode;
        private List<string> _badList;
        private int _InvoicesMade;
        private bool _retry;

        public APInvoiceStatusUpdateBatch()
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
        public void RunAPInvoiceStatusUpdate(APInvoiceStatusUpdateArg aPInvoiceStatusUpdateArg)
        {
            try
            {
                int tenant = aPInvoiceStatusUpdateArg.Tenant;

                _badList = new List<string>();
                _InvoicesMade = 0;
                BatchTaskExecutionPM batchTaskExecutionPM = aPInvoiceStatusUpdateArg.BatchTask;
                BatchTaskExecutionUpdateService batchTaskExecutionUpdateService = null;
                if (batchTaskExecutionPM != null)
                {
                    batchTaskExecutionUpdateService = GetBatchTaskUpdateServiceInstance(tenant);
                }

                IAccountingContext accContext = AccountingContext.GetContext(tenant); 
                IInvoiceContext invContext = InvoiceContext.GetContext(tenant);
                CardQuery cardQueryService = new CardQuery(tenant);
                GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(accContext);
                string lastCheckedId = "";
                int thisTimeMadeCount = 0;
                _retry = true;
                while (_retry)
                {
                    _retry = false;
                    using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(10)))
                    {
                        try
                        {
                            GetNextAPInvoicesArgs nextAPInvoicesArgs = new GetNextAPInvoicesArgs()
                            { 
                                Tenant = tenant,    
                                InvoiceNumber = aPInvoiceStatusUpdateArg.InvoiceNumber,
                                FromInvoiceDate = aPInvoiceStatusUpdateArg.FromInvoiceDate,
                                ToInvoiceDate = aPInvoiceStatusUpdateArg.ToInvoiceDate,
                                LastCheckedId = lastCheckedId,
                                ThisTimeMadeCount = 0,
                                Stop = false,
                            };
                            TryOneTime(accContext, invContext, ref nextAPInvoicesArgs, 9, tenant);
                            _InvoicesMade += nextAPInvoicesArgs.ThisTimeMadeCount;
                            if (nextAPInvoicesArgs.Stop) 
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



                _ResponseText = $"Made Invoices: {_InvoicesMade}, Errors: {String.Join(", \n", _badList.ToArray())}";
            }
            catch 
            {
                throw; 
            }
        }



        private void TryOneTime(IAccountingContext accContext, IInvoiceContext invContext, ref GetNextAPInvoicesArgs args, int timeoutinmin, int tenant)
        {
            var sw = Stopwatch.StartNew();
            APInvoiceRepository aPInvoiceRepository = new APInvoiceRepository(invContext);  
            var aPInvoiceQuery = new APInvoiceQuery(aPInvoiceRepository);
            List<APInvoice_LT_DTO> aPInvoice_LT_list = aPInvoiceQuery.GetAPInvoicesForStatusUpdate(accContext, ref args);
            if (aPInvoice_LT_list != null && aPInvoice_LT_list.Count > 0)
            {
                Action<IEnumerable<APInvoice_LT_DTO>> ProcessOneBatch =    oneBatch => ProcessOneBatchImpl(oneBatch, aPInvoiceRepository, tenant, invContext);

                APInvoiceStatusUpdater.ProcessInBatches<APInvoice_LT_DTO>(aPInvoice_LT_list, 100, ProcessOneBatch, ref _retry);
            }

        }

        private void ProcessOneBatchImpl(IEnumerable<APInvoice_LT_DTO> aPInvoice_LT_list, APInvoiceRepository aPInvoiceRepository, int tenant, IInvoiceContext invContext)
        {

            List<string> aPInvoiceIdList = aPInvoice_LT_list.Select(aplt => aplt.APInvoiceId).ToList(); 
            APInvoiceQuery query = new APInvoiceQuery(aPInvoiceRepository);
            List<APInvoicePM> PMs = query.GetAllAPInvoicesPMsByIds(aPInvoiceIdList, tenant);
            if (PMs != null && PMs.Count > 0)
            {
                foreach (var aPInvoicePM in PMs)
                {
                    try
                    {

                        bool toUpdate = false;
                        APInvoice_LT_DTO one_item = aPInvoice_LT_list.Where(item => item.APInvoiceId == aPInvoicePM.Id).FirstOrDefault();
                        if (one_item != null)
                        {
                            if (one_item.APInvoiceStatusCode != "AD" && ((one_item.LT_LocalAmountDebit != 0m && one_item.LT_OpenAmount == one_item.LT_LocalAmountDebit)
                                                            || (one_item.LT_LocalAmountDebit == 0m && one_item.LT_OpenAmount == one_item.LT_LocalAmountCredit * -1)))
                            {
                                aPInvoicePM.StatusCode = "AD";
                                aPInvoicePM.IsClosed = false;
                                toUpdate = true;
                            }

                            else if (one_item.APInvoiceStatusCode != "PP" && one_item.LT_OpenAmount != 0m)
                            {
                                aPInvoicePM.StatusCode = "PP";
                                aPInvoicePM.IsClosed = false;
                                toUpdate = true;
                            }
                            else if (one_item.APInvoiceStatusCode != "PD" && one_item.LT_OpenAmount == 0m)
                            {
                                aPInvoicePM.StatusCode = "PD";
                                aPInvoicePM.IsClosed = true;
                                toUpdate = true;
                            }
                            if (toUpdate)
                            {
                                APInvoice poco = new APInvoice();
                                APInvoiceMapping aPInvoiceMapping = new APInvoiceMapping();
                                APInvoiceMapping.MapEntity(aPInvoicePM, poco, false);

                                aPInvoiceRepository.Update(poco);
                                aPInvoiceRepository.SubmitChanges();
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
            }
        }

       

        private BatchTaskExecutionUpdateService GetBatchTaskUpdateServiceInstance(int tenant)
        {
            IInfrastructureContext context = InfrastructureContext.GetContext(tenant);
            BatchTaskExecutionUpdateService batchTaskExecutionUpdateService = new BatchTaskExecutionUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
            return batchTaskExecutionUpdateService;
        }


    }

    public static class APInvoiceStatusUpdater
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

    public class APInvoiceStatusUpdateArg
    {
        public int Tenant { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime FromInvoiceDate { get; set; }
        public DateTime ToInvoiceDate { get; set; }
        public bool Batch { get; set; }
        public BatchTaskExecutionPM BatchTask { get; set; }
    }
}