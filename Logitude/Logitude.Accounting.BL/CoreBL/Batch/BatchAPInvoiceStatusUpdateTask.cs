using Logitude.Accounting.BL.Utils;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.Accounting.BL.CoreBL.Batch
{
    public class BatchAPInvoiceStatusUpdateTask : BatchTaskExecutionsService
    {
        public BatchAPInvoiceStatusUpdateTask(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {
        }
        public override void RunCode()
        {
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(APInvoiceStatusUpdateArg));
            var parameterArgs = serializer.Deserialize(stringReader) as APInvoiceStatusUpdateArg;
            try
            {


                using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(300)))
                {

                    APInvoiceStatusUpdateBatch APInvoiceStatusUpdateBatch = new APInvoiceStatusUpdateBatch();
                    APInvoiceStatusUpdateBatch.RunAPInvoiceStatusUpdate(parameterArgs);
                    string responseText = APInvoiceStatusUpdateBatch.ResponseText();
                    ChangeStatus("D", null, responseText);

                    scope.Complete();

                }

            }
            catch (Exception ex)
            {

                LogitudeSettings.HandleLogMe(ex.ToString(), true, "BatchAPInvoiceStatusUpdateTask", new DateTime(2019, 10, 1));
                throw;

            }
        }
    }
}
