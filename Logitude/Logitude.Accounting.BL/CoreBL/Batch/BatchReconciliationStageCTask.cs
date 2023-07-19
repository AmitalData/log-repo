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
    public class BatchReconciliationStageCTask : BatchTaskExecutionsService
    {
        private bool _retry;
        public BatchReconciliationStageCTask(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {
        }
        public override void RunCode()
        {
            int timespanlimit = 10;
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(ReconciliationStageCArg));
            var parameterArgs = serializer.Deserialize(stringReader) as ReconciliationStageCArg;
            try
            {
                _retry = true;
                while (_retry)
                {
                    _retry = false;
                    using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(timespanlimit)))
                    {
                        try
                        {

                            ReconciliationStageCBatch reconciliationStageCBatch = new ReconciliationStageCBatch();
                            reconciliationStageCBatch.RunReconciliationStageC(parameterArgs, timespanlimit - 1, ref _retry);
                            string responseText = reconciliationStageCBatch.ResponseText();
                            ChangeStatus("D", null, responseText);

                            scope.Complete();

                        }
                        catch (Exception e)
                        {
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                LogitudeSettings.HandleLogMe(ex.ToString(), true, "BatchReconciliationStageCTask", new DateTime(2023, 10, 1));
                throw;

            }
        }
    }
}
