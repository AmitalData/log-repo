using Logitude.Accounting.BL.CoreBL.InterestReport;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Resolvers;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Simplog.Data.InvoiceModel;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class InterestReportUpdateService
    {
        protected override void OnCreating(InterestReportPM entityPM, EntityPM entityParentPM)
        {
            ContactPM contact = GetLoggedContact(entityPM.Tenant);
            bool showLocals = !contact.DontShowLocal;
            entityPM.CreateDateTime = DateTime.UtcNow;
            entityPM.InterestReportStatusCode = "1";
            entityPM.ReportNumber = CodeCounter.GetNumber("InterestReport", entityPM.Tenant).ToString();
            CardRepository cardRepository = new CardRepository(entityPM.Tenant);
            Card card = cardRepository.GetSingleCard(entityPM.CustomerId, entityPM.Tenant);
            InterestReportRepository interestReportRepository = new InterestReportRepository(entityPM.Tenant);
            InterestReport interestReport = interestReportRepository.GetSingleByCusstomerAndStatudDraft(entityPM.CustomerId, entityPM.Tenant);
            if (interestReport != null)
            {
                throw new Exception(TextCodesTranslator.TranslateText("InterestReport.O.CustomeralreadyhasaDraftinterest", entityPM.Tenant, showLocals)+" "+interestReport.ReportNumber);
            }
            if (card.GLAccountId == null)
            {
                throw new Exception(TextCodesTranslator.TranslateText("InterestReport.O.Customerisnotconnected", entityPM.Tenant, showLocals));
            }
            interestReport = interestReportRepository.GetSingleByGraterInterestCalculationDate(entityPM.CustomerId,entityPM.InterestCalculationDate, entityPM.Tenant);
            if (interestReport != null)
            {
                throw new Exception(TextCodesTranslator.TranslateText("InterestReport.O.Customeralreadyhasarecent", entityPM.Tenant, showLocals) + " " + interestReport.ReportNumber);
            }
            entityPM.GLAccountId = card.GLAccountId;
            GLAccountRepository gLAccountRepository = new GLAccountRepository(entityPM.Tenant);
            GLAccount gLAccount = gLAccountRepository.GetSingle(entityPM.GLAccountId, entityPM.Tenant);
            entityPM.GLAccountInterestCreditLimit = gLAccount.InterestCreditLimit;
            if (gLAccount.ActiveForInterest == false)
            {
                throw new Exception(TextCodesTranslator.TranslateText("InterestReport.O.Customerisnotdefined", entityPM.Tenant, showLocals));
            }
            CreateBatchTaskExecution(entityPM);
        }

        protected override void UpdateComposition(InterestReportPM entityPM)
        {
            InterestReportLinesByDateUpdateService interestReportLinesByDateUpdateService = new InterestReportLinesByDateUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            interestReportLinesByDateUpdateService.UpdateMulti(entityPM.InterestReportLinesByDates, entityPM.DeletedInterestReportLinesByDates, entityPM, false);
            ContactPM contactLocal = GetLoggedContact(entityPM.Tenant);
            bool showLocals = !contactLocal.DontShowLocal;
 
        }
         private void CreateEvent(string eventCode, InterestReportPM interestReport)
        {
            Contact contact = GetLoggedContact(interestReport);
            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                EntityId = interestReport.Id,
                Tenant = interestReport.Tenant,
                UserId = contact.Id,
                ObjectTableName = "InterestReport",
                IsAddedManually = false,
                EventTypeCode = eventCode,
            });
        }

        private Contact GetLoggedContact(InterestReportPM interestReport)
        {
            ContactRepository contactRep = new ContactRepository(interestReport.Tenant);
            string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(interestReport.Tenant);
           return  contactRep.GetSingleContactByEmail(resolveLoggingUserId, interestReport.Tenant);
        }

        protected override void OnUpdating(InterestReportPM entityPM, InterestReport entityPOCO)
        {
            entityPM.UpdateDateTime = DateTime.UtcNow;
            if(entityPM.InterestReportStatusCode == "3" && (entityPM.InterestReportStatusCode != entityPOCO.InterestReportStatusCode))
            {
                CancelInterestReport(entityPOCO,  entityPM);
            }

        }
        private void CancelInterestReport(InterestReport entityPoco, InterestReportPM entityPM)
        {
            if (entityPoco.InterestReportStatusCode == "1")
            {
                CreateEvent("IRCN", entityPM);
            }
            else if(entityPoco.InterestReportStatusCode == "4" || entityPoco.InterestReportStatusCode == "2") {
                InterestTransactionQueryService interestTransactionQueryService = new InterestTransactionQueryService(entityPoco.Tenant);
                List<InterestTransactionPM> interestTransactions = interestTransactionQueryService.GetInterestTransactionsByInterestReportId(entityPoco.Id, entityPoco.Tenant);
                foreach(InterestTransactionPM transaction in interestTransactions)
                {
                   
                    transaction.InterestReportId = null;
                    transaction.IsClosed = false;
                    InterestTransactionUpdateService interestTransactionUpdateService = new InterestTransactionUpdateService(MainContext, new Dictionary<string, IContext>(),entityPoco.Tenant);
                    interestTransactionUpdateService.Update(transaction, true);
                }
                if (entityPoco.InterestReportStatusCode == "2")
                {
                    CreateAutoCreditInvoice(EntityPOCO);
                }
                
             



            }

        }
        private void CreateAutoCreditInvoice(InterestReport interestReport)
        {
            ARInvoiceQuery aRInvoiceQuery = new ARInvoiceQuery(Tenant);
            ARInvoicePM aRInvoice = aRInvoiceQuery.GetSinglePM(interestReport.ARinvoiceId, Tenant);
            if (aRInvoice != null)
            {
                ARInvoicePM AutoCreditInvoice = new ARInvoicePM();
                AutoCreditInvoice.StatusCode = "AC";
                AutoCreditInvoice.StatusName = "Auto Credit";
                AutoCreditInvoice.IsAutoCredit = true;
                AutoCreditInvoice.ARInvoiceTypeCode = aRInvoice.ARInvoiceTypeCode == "CI" ? "CC" : "CD";
                AutoCreditInvoice.DebitAccount = aRInvoice.DebitAccount;
                AutoCreditInvoice.TransferStatusCode = aRInvoice.TransferStatusCode;
                AutoCreditInvoice.BillToAddressId = aRInvoice.BillToAddressId;
                AutoCreditInvoice.BillToId = aRInvoice.BillToId;
                AutoCreditInvoice.InternalNotes = aRInvoice.InternalNotes;
                AutoCreditInvoice.InvoiceCurrencyExchangeRate = aRInvoice.InvoiceCurrencyExchangeRate;
                AutoCreditInvoice.InvoiceCurrencyId = aRInvoice.InvoiceCurrencyId;
                AutoCreditInvoice.InvoiceCurrencyCode = aRInvoice.InvoiceCurrencyCode;
                AutoCreditInvoice.PrintNotes = aRInvoice.PrintNotes;
                AutoCreditInvoice.PaymentTermId = aRInvoice.PaymentTermId;
                AutoCreditInvoice.PrepaidCollectId = aRInvoice.PrepaidCollectId;
                AutoCreditInvoice.LocalCurrencyId = aRInvoice.LocalCurrencyId;
                AutoCreditInvoice.VatNumber = aRInvoice.VatNumber;
                AutoCreditInvoice.CreatedByUserId = aRInvoice.CreatedByUserId;
                AutoCreditInvoice.IssuedByUserId = aRInvoice.IssuedByUserId;
                AutoCreditInvoice.PrintByUserId = aRInvoice.PrintByUserId;
                AutoCreditInvoice.InvoiceDate = DateTime.Now;// AutoCreditDate != null ?  AutoCreditDate : DateTool.GetCurrentDateAsUtc();
                AutoCreditInvoice.DueDate = aRInvoice.DueDate;
                AutoCreditInvoice.PrintDate = aRInvoice.PrintDate;
                AutoCreditInvoice.Sent = aRInvoice.Sent;
                AutoCreditInvoice.ExchangeRateDate = aRInvoice.ExchangeRateDate;
                AutoCreditInvoice.BranchId = aRInvoice.BranchId;
                AutoCreditInvoice.ExpectedPaymentDate = aRInvoice.ExpectedPaymentDate;
                AutoCreditInvoice.ProfitCurrencyId = aRInvoice.ProfitCurrencyId;
                AutoCreditInvoice.ProfitCurrencyCode = aRInvoice.ProfitCurrencyCode;
                AutoCreditInvoice.ProfitCurrencyExchangeRate = aRInvoice.ProfitCurrencyExchangeRate;
                AutoCreditInvoice.MainEntityId = aRInvoice.MainEntityId;
                AutoCreditInvoice.MainEntityReference = aRInvoice.MainEntityReference;
                AutoCreditInvoice.MainEntityStatus = aRInvoice.MainEntityStatus;
                AutoCreditInvoice.AccountingExternalCode = aRInvoice.AccountingExternalCode;
                AutoCreditInvoice.IsConstituentInvoice = aRInvoice.IsConstituentInvoice;
                AutoCreditInvoice.IsConsolidationInvoice = aRInvoice.IsConsolidationInvoice;
                AutoCreditInvoice.SubTotalInInvoiceCurrency = aRInvoice.SubTotalInInvoiceCurrency * -1;
                AutoCreditInvoice.SubTotalInLocalCurrency = aRInvoice.SubTotalInLocalCurrency * -1;
                AutoCreditInvoice.AmountInInvoiceCurrency = aRInvoice.AmountInInvoiceCurrency * -1;
                AutoCreditInvoice.AmountInLocalCurrency = aRInvoice.AmountInLocalCurrency * -1;
                AutoCreditInvoice.AmountInProfitCurrency = aRInvoice.AmountInProfitCurrency * -1;
                AutoCreditInvoice.AmountDue = 0;
                AutoCreditInvoice.AmountDueInLocalCurrency = 0;
                AutoCreditInvoice.AmountDueInProfitCurrency = 0;
                AutoCreditInvoice.CreditedByARInvoiceId = aRInvoice.Id;
                AutoCreditInvoice.AutoCreditByARInvoiceNumber = aRInvoice.InvoiceNumber;
                AutoCreditInvoice.IsGeneralInvoice = aRInvoice.IsGeneralInvoice;
                AutoCreditInvoice.SalesmanUserId = aRInvoice.SalesmanUserId;
                AutoCreditInvoice.SATPaymentMethodCode = aRInvoice.SATPaymentMethodCode;
                AutoCreditInvoice.MetodoPagoCode = aRInvoice.MetodoPagoCode;
                AutoCreditInvoice.IsInvoiceNumberFromStock = aRInvoice.IsInvoiceNumberFromStock;
                AutoCreditInvoice.IsInvoiceNumberManuallySet = aRInvoice.IsInvoiceNumberManuallySet;

                AutoCreditInvoice = CreateAutoCreditInvoiceLines(AutoCreditInvoice, aRInvoice);
                IInvoiceContext invoiceContext = InvoiceContext.GetContext(aRInvoice.Tenant);

                ARInvoiceService invoiceService = new ARInvoiceService(invoiceContext, aRInvoice.Tenant);
                invoiceService.Create(AutoCreditInvoice);
                aRInvoice.IsCancelled = true;
                aRInvoice.CancelledByARInvoiceId = AutoCreditInvoice.Id;
                aRInvoice.StatusCode = "AR";
                aRInvoice.AmountDue = 0;
                aRInvoice.AmountDueInLocalCurrency = 0;
                aRInvoice.AmountDueInProfitCurrency = 0;
                invoiceService.Update(aRInvoice, true);
            }
        }
        private ARInvoicePM CreateAutoCreditInvoiceLines(ARInvoicePM AutoCreditInvoice, ARInvoicePM invoice)
        { int index = 1;
            foreach (ARInvoiceLinePM item in invoice.InvoiceLines){
             ARInvoiceLinePM newInvoiceLine = new ARInvoiceLinePM();
            newInvoiceLine.Tenant = item.Tenant;
            newInvoiceLine.ChargesTypeId = item.ChargesTypeId;
            newInvoiceLine.CreditAccount = item.CreditAccount;
            newInvoiceLine.Description = item.Description;
            newInvoiceLine.ForiegnCurrencyId = item.ForiegnCurrencyId;
            newInvoiceLine.ForiegnExchangeRate = item.ForiegnExchangeRate;
            newInvoiceLine.VatTypeId = item.VatTypeId;
            newInvoiceLine.LineNumber = index;
            newInvoiceLine.MeasurementId = item.MeasurementId;
            newInvoiceLine.EntityId = item.EntityId;
            newInvoiceLine.EntityReference = item.EntityReference;
            newInvoiceLine.ViewOrder = item.ViewOrder;
            newInvoiceLine.ExternalTAXItemId = item.ExternalTAXItemId;
            newInvoiceLine.ExternalVATCard = item.ExternalVATCard;
            newInvoiceLine.ForiegnCurrencyCode = item.ForiegnCurrencyCode;
            newInvoiceLine.InvoiceCurrencyCode = item.InvoiceCurrencyCode;
            newInvoiceLine.InvoiceLocalCurrencyCode = item.InvoiceLocalCurrencyCode;
            newInvoiceLine.MeasurementCode = item.MeasurementCode;
            newInvoiceLine.VatTypeName = item.VatTypeName;
            newInvoiceLine.IsExchangeRateFixed = item.IsExchangeRateFixed;
            newInvoiceLine.LocalDescription = item.LocalDescription;
            newInvoiceLine.PrepaidCollectId = item.PrepaidCollectId;
            newInvoiceLine.VatPercentage = item.VatPercentage;
            newInvoiceLine.Quantity = item.Quantity;
            newInvoiceLine.UnitPrice = item.UnitPrice * -1;
            newInvoiceLine.ForiegnCurrencyAmount = item.ForiegnCurrencyAmount * -1;
            newInvoiceLine.LocalCurrencyAmount = item.LocalCurrencyAmount * -1;
            newInvoiceLine.ProfitCurrencyAmount = item.ProfitCurrencyAmount * -1;
            newInvoiceLine.InvoiceCurrencyAmount = item.InvoiceCurrencyAmount * -1;
            newInvoiceLine.IsExpense = item.IsExpense;
            newInvoiceLine.GLAccountId = item.GLAccountId;
            newInvoiceLine.LineActionCode = "1";
            AutoCreditInvoice.InvoiceLines.Add(newInvoiceLine);
            index++;
             }
            return AutoCreditInvoice;
        }
        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }
        public static ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }

        private void CreateBatchTaskExecution(InterestReportPM entityPM)
        {
            // 1- create BTE record
            BatchTaskExecutionPM taskExe;

            InterestReportArgs args = new InterestReportArgs() { Tenant = entityPM.Tenant, InterestReportId = entityPM.Id};
            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(typeof(InterestReportArgs));
            serializer.Serialize(stringwriter, args);
            string xmlParameters = stringwriter.ToString();


            taskExe = new BatchTaskExecutionPM()
            {
                Subject = "Create Interest Report",
                Tenant = entityPM.Tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ClassName = "Logitude.Accounting.BL.CoreBL.Batch.BatchInterestReportService,Logitude.Accounting.BL",
                CreateDate = DateTime.Now,
                PrametersXml = xmlParameters,
                StatusCode = "C",

            };


            IInfrastructureContext MyContext = InfrastructureContext.GetContext(entityPM.Tenant);
            BatchTaskExecutionUpdateService bteUpdateService = new BatchTaskExecutionUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            bteUpdateService.Update(taskExe, true);

            // 2- Send to queue
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("batchtaskexecutionqueue", 0);
            queueservice.Send(new Dictionary<string, string>()
                {
                    { "BatchTaskExecutionId", taskExe.Id },
                    { "Tenant", entityPM.Tenant.ToString() }
                });
        }


    }
}
