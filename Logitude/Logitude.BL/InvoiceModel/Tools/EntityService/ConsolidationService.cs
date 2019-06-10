using Logitude.BL.DataContracts;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

namespace Logitude.BL.InvoiceModel.Tools.EntityService
{
    public class ConsolidationService
    {
        private int tenant;
        private ARInvoicePM entityPM;
        private IInvoiceContext invoiceContext;
        public ConsolidationService(ARInvoicePM entityPM, IInvoiceContext objectContext)
        {
            this.tenant = entityPM.Tenant;
            this.entityPM = entityPM;
            this.invoiceContext = objectContext;
        }

        public void OnApprove()
        {
            List<ConsolidationServiceArgsItem> items
                = (from a in invoiceContext.ARInvoices
                   where
                   a.Tenant == tenant
                   && a.IsConstituentInvoice == true
                   && a.ConsolidationInvoiceId == entityPM.Id
                   && a.MainEntityId != null
                   select new ConsolidationServiceArgsItem()
                   {
                       ShipmentId = a.MainEntityId,
                       ConstituentId = a.Id,
                   }).ToList();

            this.RunBatchService(items, entityPM.InvoiceNumber);
        }
        public void OnVoid(List<ARInvoice> allConnectedInvoices)
        {
            List<ConsolidationServiceArgsItem> items
                = (from a in allConnectedInvoices
                   select new ConsolidationServiceArgsItem()
                   {
                       ShipmentId = a.MainEntityId,
                       ConstituentId = a.Id,
                   }).ToList();

            this.RunBatchService(items);
        }
        public void OnCreatingAutoCredit(List<ARInvoice> allConnectedInvoices)
        {
            List<ConsolidationServiceArgsItem> items
                = (from a in allConnectedInvoices
                   select new ConsolidationServiceArgsItem()
                   {
                       ShipmentId = a.MainEntityId,
                       ConstituentId = a.Id,
                   }).ToList();

            this.RunBatchService(items);
        }

        private void RunBatchService(List<ConsolidationServiceArgsItem> allConstituents, string ConsolidationNumber = null)
        {
            if (allConstituents.Count > 0)
            {
                string url = HttpContext.Current.Request.Url.AbsoluteUri;

                bool isLocalHost = false;
                if(url != null)
                {
                    if (url.ToLower().Contains("localhost"))
                    {
                        isLocalHost = true;
                    }
                }

                if (isLocalHost)
                {
                    foreach (ConsolidationServiceArgsItem item in allConstituents)
                    {
                        UpdateShipmentProfitClass.UpdateConstituentShipment(entityPM.Id, item.ConstituentId, tenant);
                    }

                    List<string> allShipmentsIds = (from d in allConstituents group d by d.ShipmentId into g select g.Key).ToList();

                    foreach (string iShipmentId in allShipmentsIds)
                    {
                        UpdateShipmentProfitClass.UpdateShipmentARInvoices(iShipmentId, tenant, ConsolidationNumber);
                        UpdateShipmentProfitClass.UpdateProfit(iShipmentId, tenant);
                    }
                }

                else
                {
                    ConsolidationServiceArgs args = new ConsolidationServiceArgs()
                    {
                        Tenant = tenant,
                        ConsolidationId = this.entityPM.Id,
                        ConsolidationNumber = ConsolidationNumber,
                        Items = allConstituents,
                    };

                    var stringwriter = new System.IO.StringWriter();
                    var serializer = new XmlSerializer(typeof(ConsolidationServiceArgs));
                    serializer.Serialize(stringwriter, args);
                    string xmlParameters = stringwriter.ToString();

                    BatchTaskExecutionRepository iRepository = new BatchTaskExecutionRepository(tenant);

                    BatchTaskExecution iBatchTaskExecution = new BatchTaskExecution()
                    {
                        Id = IdCounter.GetNumber("BatchTaskExecution", tenant),
                        Subject = "Update Consolidation Shipments",
                        Tenant = tenant,
                        ClassName = "WebFreight.Web.Helpers.APIHelpers.ConsolidationServiceBatch,WebFreight.Web",

                        CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                        CreatedByUserId = this.entityPM.UpdatedByUserId,
                        PrametersXml = xmlParameters,
                        StatusCode = "C",
                    };

                    iRepository.Add(iBatchTaskExecution);
                    iRepository.SubmitChanges();

                    IQueueService queueservice = new DbQueueService();
                    queueservice.InitializeQueue("batchtaskexecutionqueue", 0);
                    queueservice.Send(new Dictionary<string, string>()
                    {
                        { "BatchTaskExecutionId", iBatchTaskExecution.Id },
                        { "Tenant", tenant.ToString() }
                    });

                    this.entityPM.BatchTaskExecutionId = iBatchTaskExecution.Id;
                }
            }
        }
    }

    public class ConsolidationServiceArgs
    {
        public int Tenant { get; set; }
        public string ConsolidationId { get; set; }
        public string ConsolidationNumber { get; set; }
        public List<ConsolidationServiceArgsItem> Items { get; set; }
    }
    public class ConsolidationServiceArgsItem
    {
        public string ShipmentId { get; set; }
        public string ConstituentId { get; set; }
    }
}
