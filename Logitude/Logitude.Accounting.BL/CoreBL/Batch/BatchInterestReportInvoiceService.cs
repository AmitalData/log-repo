using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.BL.CoreBL.InterestReport;
using Logitude.Accounting.BL.CoreBL.ReverseEngineer;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.BL.InterestService.HelperClasses;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.BL.ExtendedServices;
using Logitude.Infrastructure.Data;
using Logitude.Server.Tools;
using Logitude.Server.Tools.QueueService;
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
    public class BatchInterestReportInvoiceService : BatchTaskExecutionsService
    {
      
        public BatchInterestReportInvoiceService(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {
            
        }

        public override void RunCode()
        {
            InterestReportArguments interestReportArguments = GetInterestReportArgs();
            //CreateBatchTaskExecutionForChangeStatues(interestReportArguments);
            CreateBatchesInvoice(interestReportArguments);

        }

 
        private void CreateBatchesInvoice(InterestReportArguments interestReportArguments)
        {
            InterestReportArgs args = new InterestReportArgs();
            args.Tenant = interestReportArguments.Tenant;
            args.Email = interestReportArguments.Email;
            InterestReportQueryService interestReportQueryService = new InterestReportQueryService(interestReportArguments.Tenant);
            if (interestReportArguments.AllSelected)
            {
                List<InterestReportPM> interestReports = interestReportQueryService.GetNotInvoicedInterestReportsByDates(interestReportArguments.FromDate, interestReportArguments.ToDate, interestReportArguments.Tenant);

                if (interestReportArguments.ExcludedIds != null)
                {
                    interestReports = (from a in interestReports
                                       where !interestReportArguments.ExcludedIds.Contains(a.Id)
                                       select a).ToList();
                }
              
                foreach (InterestReportPM report in interestReports)
                {
                    args.ReportNumber = report.ReportNumber;
                    args.InterestReportId = report.Id;
                    args.Tenant = interestReportArguments.Tenant;
                    CreateBatchTaskExecutionForCreateInvoices(args);

                }
            }
            else
            {
                foreach (string ReportId in interestReportArguments.SelectedIds)
                {
                    InterestReportPM  interestReport = interestReportQueryService.GetSingle(ReportId, false,true);
                    args.ReportNumber = interestReport.ReportNumber;
                    args.InterestReportId = interestReport.Id;
                    args.Tenant = interestReportArguments.Tenant;
                    CreateBatchTaskExecutionForCreateInvoices(args);

                }
            }
           
        }

        private InterestReportArguments GetInterestReportArgs()
        {
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(InterestReportArguments));
            InterestReportArguments interestReportArgs = serializer.Deserialize(stringReader) as InterestReportArguments;
            return interestReportArgs;
        }
 

        private void CreateBatchTaskExecutionForCreateInvoices(InterestReportArgs args)
        {
            // 1- create BTE record
            BatchTaskExecutionPM taskExe;

            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(typeof(InterestReportArgs));
            serializer.Serialize(stringwriter, args);
            string xmlParameters = stringwriter.ToString();


            taskExe = new BatchTaskExecutionPM()
            {
                Subject = "Create Invoice For Interest Report # "+ args.ReportNumber,
                Tenant = args.Tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ClassName = "Logitude.Accounting.BL.CoreBL.Batch.BatchInterestReportInvoiceEachLineService,Logitude.Accounting.BL",
                CreateDate = DateTime.Now,
                PrametersXml = xmlParameters,
                StatusCode = "C",

            };


            IInfrastructureContext MyContext = InfrastructureContext.GetContext(args.Tenant);
            BatchTaskExecutionUpdateService bteUpdateService = new BatchTaskExecutionUpdateService(MyContext, new Dictionary<string, IContext>(), args.Tenant);
            bteUpdateService.Update(taskExe, true);

            // 2- Send to queue
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("batchtaskexecutionqueue", 0);
            queueservice.Send(new Dictionary<string, string>()
                {
                    { "BatchTaskExecutionId", taskExe.Id },
                    { "Tenant",  args.Tenant.ToString() }
                }, args.Tenant);
        }

    }
}
