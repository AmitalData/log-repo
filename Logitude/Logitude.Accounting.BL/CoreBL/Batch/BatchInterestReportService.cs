using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.BL.CoreBL.InterestReport;
using Logitude.Accounting.BL.CoreBL.ReverseEngineer;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
using Logitude.Server.Tools;
using Logitude.SystemLogs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.Accounting.BL.CoreBL.Batch
{
    public class BatchInterestReportService : BatchTaskExecutionsService
    {
        private InterestReportDataCalculations interestReportDataCalculation;
        public BatchInterestReportService(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {
            
        }

        public override void RunCode()
        {
            InterestReportArgs interestReportArgs = GetInterestReportArgs();
            interestReportDataCalculation = new InterestReportDataCalculations(interestReportArgs);
            interestReportDataCalculation.StartCalculations();
        }

        private InterestReportArgs GetInterestReportArgs()
        {
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(InterestReportArgs));
            InterestReportArgs interestReportArgs = serializer.Deserialize(stringReader) as InterestReportArgs;
            return interestReportArgs;
        }


    }
}
