using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.Infrastructure.BL.Helpers.BatchPrint
{
    public class BatchPrintManager : BatchTaskExecutionsService
    {
        BatchPrintManagerArgs batchPrintManagerArgs;
        public BatchPrintManager(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {
        }

        public override void RunCode()
        {
            batchPrintManagerArgs = DeserilaizeParameters();

            
        }
        private BatchPrintManagerArgs DeserilaizeParameters()
        {
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(BatchPrintManagerArgs));
            return serializer.Deserialize(stringReader) as BatchPrintManagerArgs;
        }
    }
}
