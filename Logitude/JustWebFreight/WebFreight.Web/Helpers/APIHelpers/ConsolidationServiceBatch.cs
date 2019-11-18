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

            
            ConsolidationService iConsolidationService = new ConsolidationService();
            iConsolidationService.RunBatchService(args, BatchTaskExecution.Id);
        }

    }
}