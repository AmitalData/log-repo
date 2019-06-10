using Logitude.BL.DataContracts;
using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                foreach (var item in allConstituents)
                {
                    UpdateShipmentProfitClass.UpdateConstituentShipment(entityPM.Id, item.ConstituentId, tenant);
                }

                var allShipmentsIds = (from d in allConstituents group d by d.ShipmentId into g select g.Key).ToList();

                foreach (var iShipmentId in allShipmentsIds)
                {
                    UpdateShipmentProfitClass.UpdateShipmentARInvoices(iShipmentId, tenant, ConsolidationNumber);
                    UpdateShipmentProfitClass.UpdateProfit(iShipmentId, tenant);
                }
            }
        }

        private void RunBatchService()
        {
            ConsolidationServiceArgs args = new ConsolidationServiceArgs()
            {
                //LoggedUserEmail = entityPM,
                //Tenant = tenant
            };

            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(typeof(ConsolidationServiceArgs));
            serializer.Serialize(stringwriter, args);
            string xmlParameters = stringwriter.ToString();

            //BatchTaskExecutionPM taskExe = new BatchTaskExecutionPM()
            //{
            //    Subject = "Generate Tariffs",
            //    Tenant = tenant,
            //    ChangeSetOp = ChangeSetOperation.Insert,
            //    ClassName = "WebFreight.Web.Helpers.APIHelpers.GenerateTariffsHelper,WebFreight.Web",
            //    CreateDate = DateTime.Now,
            //    PrametersXml = xmlParameters,
            //    StatusCode = "C",
            //};

            //IInfrastructureContext MyContext = InfrastructureContext.GetContext(tenant);
            //BatchTaskExecutionUpdateService bteUpdateService = new BatchTaskExecutionUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
            //bteUpdateService.Update(taskExe, true);

            //IQueueService queueservice = new DbQueueService();
            //queueservice.InitializeQueue("batchtaskexecutionqueue", 0);
            //queueservice.Send(new Dictionary<string, string>()
            //    {
            //        { "BatchTaskExecutionId", taskExe.Id },
            //        { "Tenant", tenant.ToString() }
            //    });
        }
    }
    public class ConsolidationServiceArgs
    {

    }

    public class ConsolidationServiceArgsItem
    {
        public string ShipmentId { get; set; }
        public string ConstituentId { get; set; }
    }
}
