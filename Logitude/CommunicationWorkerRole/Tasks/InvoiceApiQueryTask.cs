using Logitude.Accounting.BL.Utils;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CommunicationWorkerRole.Tasks
{
    public class InvoiceApiQueryTask : TaskManagerBase
    {

        private StringBuilder stringBuilder;

        public InvoiceApiQueryTask(string Id, int tenant) : base(Id, tenant)
        {
            stringBuilder = new StringBuilder();
        }

        public override void StartTask()
        {
            bool failed = false;
            try
            {


                Log("InvoiceApiQueryBatch:Start", "Start");
                try
                   {
                    InvoiceApiQueryBatch invoiceApiQueryBatch = new InvoiceApiQueryBatch();
                    int tenant = this.Task != null ? this.Task.Tenant : 0 ;
                    string xml = Task?.SchedulerDetailsXML;
                    var element = xml !=null ? XElement.Parse(xml) :null;
                    XNamespace ns = element.GetDefaultNamespace();
                    string fromDate = element.Element(ns + "FromDate")?.Value;
                    string toDate = element.Element(ns + "ToDate")?.Value;

                    string startDate, endDate;
                    DateTime parsedFromDate, parsedToDate;
                    bool isFromDateValid = DateTime.TryParse(fromDate, out parsedFromDate);
                    bool isToDateValid = DateTime.TryParse(toDate, out parsedToDate);

                    if (isFromDateValid && isToDateValid && parsedFromDate <= parsedToDate)
                    {
                        startDate = parsedFromDate.Date.ToString("yyyy-MM-dd'T'00:00:00");
                        endDate = parsedToDate.Date.ToString("yyyy-MM-dd'T'00:00:00");
                    }
                    else
                    {
                        startDate = DateTime.UtcNow.AddDays(-1).Date.ToString("yyyy-MM-dd'T'00:00:00");
                        endDate = DateTime.UtcNow.Date.ToString("yyyy-MM-dd'T'00:00:00");
                    }

                    invoiceApiQueryBatch.RunInvoiceApiInvoicesQuery(startDate, endDate, tenant);
                    string responseText = invoiceApiQueryBatch.ResponseText();

                       Log(responseText, "responseText:");
                    }
                    catch (Exception ex)
                    {
                        failed = true;
                       Log(ex.Message, "Exception:");
                       ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", $"InvoiceApiQueryBatch()", null);
                    }

               
            }
            finally
            {
                if (failed)
                {
                    throw new Exception(stringBuilder.ToString());
                }
            }
        }


        private void Log(string message,string type)
        {
            stringBuilder.Append(DateTime.Now.ToString()).Append(type).Append(message).AppendLine();
        }

    }
}
