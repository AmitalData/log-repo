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
    public class BatchReconciliationStageBTask : BatchTaskExecutionsService
    {
        public BatchReconciliationStageBTask(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {
        }
        public override void RunCode()
        {
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(ReconciliationStageBArg));
            var parameterArgs = serializer.Deserialize(stringReader) as ReconciliationStageBArg;
            try
            {


                using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(600)))
                {

                    ReconciliationStageBBatch reconciliationStageBBatch = new ReconciliationStageBBatch();
                    reconciliationStageBBatch.RunReconciliationStageB(parameterArgs);
                    string responseText = reconciliationStageBBatch.ResponseText();
                    ChangeStatus("D", null, responseText);

                    scope.Complete();

                }

            }
            catch (Exception ex)
            {

                LogitudeSettings.HandleLogMe(ex.ToString(), true, "BatchReconciliationStageBTask", new DateTime(2019, 10, 1));
                throw;

            }
        }
    }
}
