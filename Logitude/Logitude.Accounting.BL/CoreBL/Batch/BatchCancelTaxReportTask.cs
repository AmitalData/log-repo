using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
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
   public class BatchCancelTaxReportTask : BatchTaskExecutionsService
    {

        public BatchCancelTaxReportTask(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {

        }

        public override void RunCode()
        {
            // Deserilaize parameters
            PNCFileArgs parameterArgs = DeserilaizeParameters();
            TaxReportPM taxReportPM = GetTaxReportById(parameterArgs);
            

            try
            {
                SetReportAsCancelled(taxReportPM);
                SubmitTaxReport(taxReportPM);
            }

            catch (Exception ex)
            {
                SetReportAsFailedToCancel(taxReportPM);
                SubmitTaxReport(taxReportPM);

                throw;

            }
        }

        private static void SubmitTaxReport(TaxReportPM taxReportPM)
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(taxReportPM.Tenant);
            TaxReportUpdateService taxReportUpdateService = new TaxReportUpdateService(accountingContext, new Dictionary<string, IContext>(), taxReportPM.Tenant);
            taxReportPM.ChangeSetOp = ChangeSetOperation.Update;
            taxReportUpdateService.Update(taxReportPM, true);
        }

        private PNCFileArgs DeserilaizeParameters()
        {
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(PNCFileArgs));
            PNCFileArgs parameterArgs = serializer.Deserialize(stringReader) as PNCFileArgs;
            return parameterArgs;
        }
        private static TaxReportPM GetTaxReportById(PNCFileArgs parameterArgs)
        {
            TaxReportQueryService taxReportQueryService = new TaxReportQueryService(parameterArgs.Tenant);
            TaxReportPM taxReportPM = taxReportQueryService.GetSingle(parameterArgs.ReportId, false, false);
            return taxReportPM;
        }

        private static void SetReportAsCancelled(TaxReportPM taxReportPM)
        {
            taxReportPM.IsCancelled = true;
            taxReportPM.StatusCode = VatReportStatusValues.Cancelled;

            DateTime stopLogAt = new DateTime(2023, 06, 01);
            string text = "BatchCancelTaxReportTask.SetReportAsCancelled(*1*): " + taxReportPM.Id + " taxReportPM.StatusCode : " + taxReportPM.StatusCode;
            ULog(text, stopLogAt);

        }
        private static void SetReportAsFailedToCancel(TaxReportPM taxReportPM)
        {
            taxReportPM.IsCancelled = false;
            taxReportPM.StatusCode = VatReportStatusValues.CancelationFailed;

            DateTime stopLogAt = new DateTime(2023, 06, 01);
            string text = "BatchCancelTaxReportTask.SetReportAsFailedToCancel(*1*): " + taxReportPM.Id + " taxReportPM.StatusCode : " + taxReportPM.StatusCode;
            ULog(text, stopLogAt);

        }

        private static void ULog(string text, DateTime stopLogAt)
        {
            string log_text = text + System.Environment.NewLine;
            log_text = log_text + String.Format("{0:HH:mm:ss.ffff}", DateTime.Now.ToString()) + System.Environment.NewLine;
            System.Diagnostics.StackTrace t = new System.Diagnostics.StackTrace();
            log_text = log_text + t.ToString();
            LogitudeSettings.HandleLogMe(log_text, false, "TaxReportPMToPOCO", new DateTime(2023, 6, 1));
        }

    }
}
