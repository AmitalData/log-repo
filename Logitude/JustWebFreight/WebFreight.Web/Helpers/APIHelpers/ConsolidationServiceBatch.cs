using Logitude.BL.DataContracts;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace WebFreight.Web.Helpers.APIHelpers
{
    public class ConsolidationServiceBatch : BatchTaskExecutionsService
    {
        public ConsolidationServiceBatch(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {

        }

        public override void RunCode()
        {
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(ConsolidationServiceArgs));
            ConsolidationServiceArgs args = serializer.Deserialize(stringReader) as ConsolidationServiceArgs;

            if (args.Items.Count > 0)
            {
                foreach (ConsolidationServiceArgsItem item in args.Items)
                {
                    UpdateShipmentProfitClass.UpdateConstituentShipment(args.ConsolidationId, item.ConstituentId, args.Tenant);
                }

                List<string> allShipmentsIds = (from d in args.Items group d by d.ShipmentId into g select g.Key).ToList();

                foreach (string iShipmentId in allShipmentsIds)
                {
                    UpdateShipmentProfitClass.UpdateShipmentARInvoices(iShipmentId, args.Tenant, args.ConsolidationNumber);
                    UpdateShipmentProfitClass.UpdateProfit(iShipmentId, args.Tenant);
                }
            }
        }

    }
}