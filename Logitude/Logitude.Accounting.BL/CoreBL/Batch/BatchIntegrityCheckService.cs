using Logitude.Accounting.BL.CoreBL;
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
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Logitude.Accounting.BL.CoreBL.Batch
{
    public class BatchIntegrityCheckService : BatchTaskExecutionsService
    {
        public BatchIntegrityCheckService(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {

        }

        public override void RunCode()
        {
            // Deserilaize parameters
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(IntegrityCheckArgs)); // AccountingIntegrityInParam
            IntegrityCheckArgs parameterArgs = serializer.Deserialize(stringReader) as IntegrityCheckArgs;

            // Call the service
            RunService(parameterArgs);

        }

        private void RunService(IntegrityCheckArgs args)
        {
            bool shouldFix=false;
            string stringXML = string.Empty;
            string stringXML_toSend = string.Empty;
            try
            {


                // 1- get entity
                IAccountingContext MyContext = AccountingContext.GetContext(args.Tenant);
                AccountingIntegrityCheckUpdateService updateService = new AccountingIntegrityCheckUpdateService(MyContext, new Dictionary<string, IContext>(), args.Tenant);
                AccountingIntegrityCheckQueryService query = new AccountingIntegrityCheckQueryService(args.Tenant);
                AccountingIntegrityCheckPM entityPM = query.GetSingle(args.EntityId, false, false);
                entityPM.ChangeSetOp = ChangeSetOperation.Update;

                // Deserilaize parameters
                string xmlParameters = entityPM.ParametersXML;
                if (xmlParameters == null)
                {
                    // update status
                    entityPM.StatusCode = "5"; // Failed
                    entityPM.HasException = true;
                    entityPM.DoneDateTimeUTC = DateTime.UtcNow;
                    entityPM.ResultXML = "Service Error: " + "Dates are not selected";

                    updateService.Update(entityPM, true);

                    throw new ApplicationException("Dates are not selected");
                }

                System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
                XmlSerializer serializer = new XmlSerializer(typeof(AccountingIntegrityInParam));
                AccountingIntegrityInParam _params = serializer.Deserialize(stringReader) as AccountingIntegrityInParam;


                // 2- update status
                entityPM.StatusCode = "2"; // In Progress
                updateService.Update(entityPM, true);

                // 3- check parameters
                AccountingIntegrityService accountingIntegrityService = new AccountingIntegrityService();
                string errorMessage = accountingIntegrityService.CheckParams(_params);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    // update status
                    entityPM.StatusCode = "5"; // Failed
                    entityPM.HasException = true;
                    entityPM.DoneDateTimeUTC = DateTime.UtcNow;
                    entityPM.ResultXML = "Service Error: " + errorMessage;
                    updateService.Update(entityPM, true);

                    throw new ApplicationException(errorMessage);
                }

                // 4- run service
                AccountingIntegrityResult res;
                try
                {
                    res = accountingIntegrityService.CheckIntegrity(_params);
                }
                catch (Exception ex)
                {
                    // update status
                    entityPM.StatusCode = "5"; // Failed
                    entityPM.HasException = true;
                    entityPM.DoneDateTimeUTC = DateTime.UtcNow;
                    entityPM.ResultXML = "Service Error: " + ex.Message;

                    updateService.Update(entityPM, true);

                    throw new ApplicationException(ex.Message);
                }

                // 5- store resultXML
                if (res != null)
                {
                    // Check rows if have exception message
                    List<AccountingIntegrityStep> integritySteps = res.MyAccountingIntegrityStep;
                    foreach (AccountingIntegrityStep step in integritySteps)
                    {
                        if (step.BadRows > 0 && step.ShouldFix == true)
                        //if (!string.IsNullOrWhiteSpace(step.ExceptionMessage))
                        {
                            shouldFix = true;
                            entityPM.HasException = true;
                            break;
                        }
                    }
                    entityPM.ShouldFix = res.ShouldFix;
                    // serialize resultXML
                    stringXML = LogitudeXmlSerializer.SerializeObjectToXmlString<AccountingIntegrityResult>(res);
                    List<AccountingIntegrityStep> errorSteps = new List<AccountingIntegrityStep>();
                    res.MyAccountingIntegrityStep.ForEach(s =>
                    {
                        if (s != null && (!String.IsNullOrEmpty(s.ExceptionMessage) || s.ShouldFix || s.BadRows > 0))
                        {
                            errorSteps.Add(s);
                        }
                    });
                    AccountingIntegrityResult res_toSend = new AccountingIntegrityResult()
                    {
                        LedgerOpenAmount = res.LedgerOpenAmount,
                        MyAccountingIntegrityStep = errorSteps,
                        BalanceInLocalCurrencyResult = res.BalanceInLocalCurrencyResult,
                        DueLocalBalance = res.DueLocalBalance,
                        HasException = res.HasException,
                        JournalLineToLedgerResult = res.JournalLineToLedgerResult,
                        LedgerToMounthTotalResult = res.LedgerToMounthTotalResult,
                        ShouldFix = res.ShouldFix,
                        TotalOpenReconciliationResult = res.TotalOpenReconciliationResult,
                        InterestReportResult = res.InterestReportResult
                    };
                    stringXML_toSend = LogitudeXmlSerializer.SerializeObjectToXmlString<AccountingIntegrityResult>(res_toSend);

                    // update
                    entityPM.ResultXML = stringXML;
                    entityPM.StatusCode = "3"; // Check Completed
                    entityPM.DoneDateTimeUTC = DateTime.UtcNow;

                    // save 
                    updateService.Update(entityPM, true);
                }
            }
            finally
            {
                if (args.SendEmailWhileError)
                {
                    if (shouldFix)
                    {
                        SendEmailWhileError(args.Tenant, "has been failed", stringXML_toSend);
                    }
                    else
                    {
                        //if (DateTime.Now< new DateTime(2019, 09, 20))
                        //{
                        //    SendEmailWhileError(args.Tenant, "has been finish -without problem ");
                        //}
                    }
                }

            }
        }

        private void SendEmailWhileError(int tenant, string remark, string stringXML)
        {
            try
            {

                bool formatXml = true;
                if (formatXml)
                {
                    XDocument doc = XDocument.Parse(stringXML);
                    stringXML = doc.ToString();
                }
                


                var error = $"Integrity Check for tenant:{tenant} {remark}  (TASK 56708)";
                string emailbody = $"<div><div style='text-align:left;font-family:Verdana;font-weight:bold;font-size:14px'>{error}</div><xmp>{stringXML}</xmp></div></div>";

                EmailCommunicationParams emailParams = new EmailCommunicationParams();

                emailParams = new EmailCommunicationParams()
                {
                    From = "admin@fnarsoft.com",
                    To = "eyal@amital.co.il;ohad@amital.co.il",
                    Subject = error,
                    EmailBody = emailbody,
                    Tenant = tenant,
                    IsBodySecured = true,
                };

                Communications.AddEmailCommunicationLogQueue(emailParams, tenant);
            }
            catch (Exception ee)
            {

                ExceptionHandler.HandleException(ee, DateTime.Now, 0, "", "BatchIntegrityCheck WorkerRole" + this.GetType().Name, " : Run() Method", null);
            }
        }
    }
}
