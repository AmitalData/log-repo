using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.BL.Utils;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.Accounting.BL.CoreBL.Batch
{
     public  class BatchRevaluationService: BatchTaskExecutionsService
    {
        public BatchRevaluationService(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {

        }

        public override void RunCode()
        {
            RevaluationArgs parameterArgs = GetRevaluationArgsByXMLParams(BatchTaskExecution.PrametersXml);          
            try
            {
                RevaluationBatch revaluationBatch = new RevaluationBatch();
                revaluationBatch.RunAllOpenRevaluations(parameterArgs.Tenant);              
            }

            catch (Exception ex)
            {
                throw;
            }

        }
        private RevaluationArgs GetRevaluationArgsByXMLParams(string xmlParameters)
        {
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(RevaluationArgs));
            RevaluationArgs parameterArgs = serializer.Deserialize(stringReader) as RevaluationArgs;
            return parameterArgs;

        }


    }
}
