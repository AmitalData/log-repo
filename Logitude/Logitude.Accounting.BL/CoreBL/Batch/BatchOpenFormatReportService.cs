using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
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
   public class BatchOpenFormatReportService : BatchTaskExecutionsService
    {
        public BatchOpenFormatReportService(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {

        }

        public override void RunCode()
        {
           
            // Deserilaize parameters
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(PNCFileArgs));
            PNCFileArgs parameterArgs = serializer.Deserialize(stringReader) as PNCFileArgs;
            IContext MainContext = AccountingContext.GetContext(parameterArgs.Tenant);
            OpenFormatReportUpdateService openFormatReportUpdateService = new OpenFormatReportUpdateService(MainContext, new Dictionary<string, IContext>(), parameterArgs.Tenant);

            // Call the service
            OpenFormatReportQueryService openFormatReportQueryService = new OpenFormatReportQueryService(parameterArgs.Tenant);
            OpenFormatReportPM openFormatReportPM = openFormatReportQueryService.GetSingle(parameterArgs.ReportId, false, false);
            OpenFormatReportService openFormatReportService = new OpenFormatReportService();
            try
            {
                
                DocumentsFilingPM docFilingPM = openFormatReportService.CreateBKMVDATAFile(parameterArgs.ReportId, parameterArgs.Tenant, parameterArgs.TestingMode);

                DocumentsFilingPM INIdocFilingPM = openFormatReportService.CreateINIFile(parameterArgs.ReportId, parameterArgs.Tenant);

                 

            }

            catch (Exception ex)
            {

              
                openFormatReportPM.StatusTypeCode = "4";
                openFormatReportPM.ErrorMessage = ex.Message;
                openFormatReportPM.ChangeSetOp = ChangeSetOperation.Update;
                openFormatReportUpdateService.Update(openFormatReportPM, true);
                throw;

            }

        }


    }
}
