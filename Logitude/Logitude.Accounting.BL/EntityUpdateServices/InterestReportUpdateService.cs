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
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.BL.Validators;
using Simplog.Data.Helpers;
using Logitude.Accounting.BL.CloseTables;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class InterestReportUpdateService
    {
        string oldLabel = TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0);
        string newLabel = TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0);
        protected override void OnCreating(InterestReportPM entityPM, EntityPM entityParentPM)
        {
            if (!entityPM.IsCreatedFromBatch)
            {
                CreateBatchTaskExecution(entityPM);
            }
        }

        protected override void UpdateComposition(InterestReportPM entityPM)
        {
            InterestReportLinesByDateUpdateService interestReportLinesByDateUpdateService = new InterestReportLinesByDateUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            interestReportLinesByDateUpdateService.UpdateMulti(entityPM.InterestReportLinesByDates, entityPM.DeletedInterestReportLinesByDates, entityPM, false);
        }
        private void CreateEvent(string eventCode, InterestReportPM interestReport, string Notes = null)
        {
            ContactPM contact = GetLoggedContact(interestReport.Tenant);
            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                EntityId = interestReport.Id,
                Tenant = interestReport.Tenant,
                UserId = contact.Id,
                ObjectTableName = "InterestReport",
                IsAddedManually = false,
                EventTypeCode = eventCode,
                Notes = Notes,
            });
        }


        protected override void OnUpdating(InterestReportPM entityPM, InterestReport entityPOCO)
        {
            entityPM.UpdateDateTime = DateTime.UtcNow;
            if (entityPM.InterestReportStatusCode == "3" && (entityPM.InterestReportStatusCode != entityPOCO.InterestReportStatusCode))
            {
                CancelInterestReport(entityPOCO, entityPM);
            }

            if (entityPM.ChangeSetOp == ChangeSetOperation.Update && !entityPM.IsUpdatedFromBatch)
            {
                if (entityPM.RecalculateData)
                {
                    CreateBatchTaskExecutionForRecalculatingData(entityPM);
                }
                if (entityPM.InterestCalculationDate != entityPOCO.InterestCalculationDate)
                {

                    CreateBatchTaskExecutionForRecalculatingData(entityPM);
                }

                if (entityPM.OpenBalance != entityPOCO.OpenBalance)
                {
                    CreateBatchTaskExecutionForRecalculatingData(entityPM);
                }


            }
            CreateEventForRecalculatingData(entityPM);

        }
        string eventNotes;
        private void CreateEventForRecalculatingData(InterestReportPM interestReport)
        {
            if (interestReport.IsUpdatedFromBatch && interestReport.InterestReportStatusCode == InterestReportStatuseValues.Draft)
            {

                 eventNotes = null;
                if (interestReport.GLAccountInterestCreditLimit != EntityPOCO.GLAccountInterestCreditLimit)
                {
                    eventNotes= SetEventNote("Credit Limit", EntityPOCO.GLAccountInterestCreditLimit.ToString(), interestReport.GLAccountInterestCreditLimit.ToString());

                 }
                if (interestReport.CreditAllotmentPercentage != EntityPOCO.CreditAllotmentPercentage)
                {
                    eventNotes = SetEventNote("Credit Allotment Percentage", EntityPOCO.CreditAllotmentPercentage.ToString(), interestReport.CreditAllotmentPercentage.ToString());
                }
                if (interestReport.CalculatedPostponedChequesCommision != EntityPOCO.CalculatedPostponedChequesCommision)
                {
                    eventNotes = SetEventNote("Calculated Postponed Cheques Commision", EntityPOCO.CalculatedPostponedChequesCommision.ToString(), interestReport.CalculatedPostponedChequesCommision.ToString());
                }

                if (eventNotes != null)
                    CreateEvent("IREC", interestReport, eventNotes);
            }

            CreateRecalculateEventForUpdateCalculationDate();
           
        }
        private string SetEventNote(string fieldLabel, string oldValue, string newValue)
        {
            return eventNotes+ fieldLabel + ": " + "old value: " + oldValue + ",new value: " + newValue + Environment.NewLine;


        }
        private void CreateRecalculateEventForUpdateCalculationDate()
        {
           string eventNotes = null;
            if (EntityPM.InterestCalculationDate != EntityPOCO.InterestCalculationDate)
            {

                string FieldLabel = "Interest Calculation Date";
                eventNotes =   FieldLabel + ": " + "old value: " + EntityPOCO.InterestCalculationDate + ",new value: "+ EntityPM.InterestCalculationDate + Environment.NewLine;
                CreateEvent("IREC", EntityPM, eventNotes);

            }

        }
        protected override void Trace(InterestReportPM entityPM, InterestReport entityPOCO, string changesXml)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                CreateEvent("IRCD", entityPM);
            }
            else
            {
                ContactPM contact = GetLoggedContact(entityPM.Tenant);
                bool showLocals = !contact.DontShowLocal;
                if ((entityPM.InterestReportStatusCode != entityPOCO.InterestReportStatusCode))
                {
                    switch (entityPM.InterestReportStatusCode)
                    {
                        case "4":
                            {
                                CreateEvent("IRCW", entityPM);
                                break;
                            }
                        case "6":
                            {
                                CreateEvent("IRFD", entityPM);
                                break;
                            }
                        case "2":
                            {
                                CreateEvent("IRIN", entityPM, TextCodesTranslator.TranslateText("InterestReport.F.ARInvoiceNumber", entityPM.Tenant, showLocals) + ": " + entityPM.ARInvoiceNumber);
                                break;
                            }
                        case "3":
                            {
                                CreateEvent("IRCN", entityPM);
                                break;
                            }
                        case "9":
                            {
                                CreateEvent("IRIF", entityPM, entityPM.InvoiceFailureReason);
                                break;
                            }

                    }

                }

                if (entityPM.OpenBalance != entityPOCO.OpenBalance)
                {
                    string notes = TranslateTextsClass.Translate("InterestReport.F.OpenBalance", entityPOCO.Tenant, showLocals) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", entityPOCO.Tenant, showLocals) + entityPOCO.OpenBalance  + TranslateTextsClass.Translate("Accounting.General.O.NewValue", entityPOCO.Tenant, showLocals) + entityPM.OpenBalance ;
                    CreateEvent("IRUP", entityPM, notes);
                }
            }
            base.Trace(entityPM, entityPOCO, changesXml);
        }
 
        private void CancelInterestReport(InterestReport entityPoco, InterestReportPM entityPM)
        {
              CancelOpenBalanceTransaction(entityPoco);

             if (entityPoco.InterestReportStatusCode == "4" || entityPoco.InterestReportStatusCode == "2") {
                GetAndUpdateRelatedInterestTransactions(entityPoco);
               
                if (entityPoco.InterestReportStatusCode == "2")
                {
                    CreateautoCreditInvoice(entityPM);
                }                
            }
        }

        private void CancelOpenBalanceTransaction(InterestReport interestReport)
        {
            InterestTransactionQueryService interestTransactionQueryService = new InterestTransactionQueryService(interestReport.Tenant);
            InterestTransactionPM interestTransaction = interestTransactionQueryService.GetOpenBalanceInterestTransactionsByInterestReportId(interestReport.Id, interestReport.Tenant);
            if (interestTransaction != null)
            {
                interestTransaction.ChangeSetOp = ChangeSetOperation.Update;
                interestTransaction.IsCancelled = true;
                interestTransaction.EntityId = interestReport.Id;
                InterestTransactionUpdateService interestTransactionUpdateService = new InterestTransactionUpdateService(MainContext, new Dictionary<string, IContext>(), interestReport.Tenant);
                interestTransactionUpdateService.Update(interestTransaction, true);
            }

        }
        private void GetAndUpdateRelatedInterestTransactions(InterestReport interestReport)
        {
            InterestTransactionQueryService interestTransactionQueryService = new InterestTransactionQueryService(interestReport.Tenant);
            List<InterestTransactionPM> interestTransactions = interestTransactionQueryService.GetInterestTransactionsByInterestReportId(interestReport.Id, interestReport.Tenant);
            foreach (InterestTransactionPM transaction in interestTransactions)
            {

                transaction.InterestReportId = null;
                transaction.IsClosed = false;
                transaction.ChangeSetOp = ChangeSetOperation.Update;
                if (transaction.InterestEntityTypeCode == "4")
                {
                    transaction.IsCancelled = true;
                }
                InterestTransactionUpdateService interestTransactionUpdateService = new InterestTransactionUpdateService(MainContext, new Dictionary<string, IContext>(), interestReport.Tenant);
                interestTransactionUpdateService.Update(transaction, true);
            }
        }
        private ARInvoicePM MapautoCreditInvoice(ARInvoicePM autoCreditInvoice,ARInvoicePM aRInvoice, InterestReportPM interestReport)
        {
            autoCreditInvoice.SetApproved = true;
            autoCreditInvoice.StatusCode = "AC";
            autoCreditInvoice.StatusName = "Auto Credit";
            autoCreditInvoice.IsAutoCredit = true;
            autoCreditInvoice.ARInvoiceTypeCode = aRInvoice.ARInvoiceTypeCode == "CI" ? "CC" : "CD";
            autoCreditInvoice.DebitAccount = aRInvoice.DebitAccount;
            autoCreditInvoice.TransferStatusCode = aRInvoice.TransferStatusCode;
            autoCreditInvoice.BillToAddressId = aRInvoice.BillToAddressId;
            autoCreditInvoice.BillToId = aRInvoice.BillToId;
            autoCreditInvoice.InternalNotes = aRInvoice.InternalNotes;
            autoCreditInvoice.InvoiceCurrencyExchangeRate = aRInvoice.InvoiceCurrencyExchangeRate;
            autoCreditInvoice.InvoiceCurrencyId = aRInvoice.InvoiceCurrencyId;
            autoCreditInvoice.InvoiceCurrencyCode = aRInvoice.InvoiceCurrencyCode;
            autoCreditInvoice.PrintNotes = aRInvoice.PrintNotes;
            autoCreditInvoice.PaymentTermId = aRInvoice.PaymentTermId;
            autoCreditInvoice.PrepaidCollectId = aRInvoice.PrepaidCollectId;
            autoCreditInvoice.LocalCurrencyId = aRInvoice.LocalCurrencyId;
            autoCreditInvoice.VatNumber = aRInvoice.VatNumber;
            autoCreditInvoice.CreatedByUserId = aRInvoice.CreatedByUserId;
            autoCreditInvoice.IssuedByUserId = aRInvoice.IssuedByUserId;
            autoCreditInvoice.PrintByUserId = aRInvoice.PrintByUserId;
            autoCreditInvoice.InvoiceDate = interestReport.InvoiceDate  != null? interestReport.InvoiceDate: DateTime.Now;// AutoCreditDate != null ?  AutoCreditDate : DateTool.GetCurrentDateAsUtc();
            autoCreditInvoice.DueDate = aRInvoice.DueDate;
            autoCreditInvoice.PrintDate = aRInvoice.PrintDate;
            autoCreditInvoice.Sent = aRInvoice.Sent;
            autoCreditInvoice.ExchangeRateDate = aRInvoice.ExchangeRateDate;
            autoCreditInvoice.BranchId = aRInvoice.BranchId;
            autoCreditInvoice.ExpectedPaymentDate = aRInvoice.ExpectedPaymentDate;
            autoCreditInvoice.ProfitCurrencyId = aRInvoice.ProfitCurrencyId;
            autoCreditInvoice.ProfitCurrencyCode = aRInvoice.ProfitCurrencyCode;
            autoCreditInvoice.ProfitCurrencyExchangeRate = aRInvoice.ProfitCurrencyExchangeRate;
            autoCreditInvoice.MainEntityId = aRInvoice.MainEntityId;
            autoCreditInvoice.MainEntityReference = aRInvoice.MainEntityReference;
            autoCreditInvoice.MainEntityStatus = aRInvoice.MainEntityStatus;
            autoCreditInvoice.AccountingExternalCode = aRInvoice.AccountingExternalCode;
            autoCreditInvoice.IsConstituentInvoice = aRInvoice.IsConstituentInvoice;
            autoCreditInvoice.IsConsolidationInvoice = aRInvoice.IsConsolidationInvoice;
            autoCreditInvoice.SubTotalInInvoiceCurrency = aRInvoice.SubTotalInInvoiceCurrency * -1;
            autoCreditInvoice.SubTotalInLocalCurrency = aRInvoice.SubTotalInLocalCurrency * -1;
            autoCreditInvoice.AmountInInvoiceCurrency = aRInvoice.AmountInInvoiceCurrency * -1;
            autoCreditInvoice.AmountInLocalCurrency = aRInvoice.AmountInLocalCurrency * -1;
            autoCreditInvoice.AmountInProfitCurrency = aRInvoice.AmountInProfitCurrency * -1;
            autoCreditInvoice.AmountDue = 0;
            autoCreditInvoice.AmountDueInLocalCurrency = 0;
            autoCreditInvoice.AmountDueInProfitCurrency = 0;
            autoCreditInvoice.CreditedByARInvoiceId = aRInvoice.Id;
            autoCreditInvoice.AutoCreditByARInvoiceNumber = aRInvoice.InvoiceNumber;
            autoCreditInvoice.IsGeneralInvoice = aRInvoice.IsGeneralInvoice;
            autoCreditInvoice.SalesmanUserId = aRInvoice.SalesmanUserId;
            autoCreditInvoice.SATPaymentMethodCode = aRInvoice.SATPaymentMethodCode;
            autoCreditInvoice.MetodoPagoCode = aRInvoice.MetodoPagoCode;
            autoCreditInvoice.IsInvoiceNumberFromStock = aRInvoice.IsInvoiceNumberFromStock;
            autoCreditInvoice.IsInvoiceNumberManuallySet = aRInvoice.IsInvoiceNumberManuallySet;
            autoCreditInvoice.Tenant = aRInvoice.Tenant;
            autoCreditInvoice.HasInterestFeature = true;
            autoCreditInvoice = CreateautoCreditInvoiceLines(autoCreditInvoice, aRInvoice);
            return autoCreditInvoice;

        }
        private void UpdateCreditedInvoice(ARInvoicePM aRInvoice, ARInvoicePM autoCreditInvoice, ARInvoiceService invoiceService)
        {
            aRInvoice.IsCancelled = true;
            aRInvoice.CancelledByARInvoiceId = autoCreditInvoice.Id;
            aRInvoice.StatusCode = "AR";
            aRInvoice.AmountDue = 0;
            aRInvoice.AmountDueInLocalCurrency = 0;
            aRInvoice.AmountDueInProfitCurrency = 0;
            invoiceService.Update(aRInvoice, true);
        }
        private void CreateautoCreditInvoice(InterestReportPM interestReport)
        {
           
            ARInvoicePM aRInvoice = GetInteresReportARInvoice(interestReport);
            if (aRInvoice != null)
            {
                ARInvoicePM autoCreditInvoice = new ARInvoicePM();
                autoCreditInvoice = MapautoCreditInvoice(autoCreditInvoice,aRInvoice, interestReport);
                IInvoiceContext invoiceContext = InvoiceContext.GetContext(aRInvoice.Tenant);
                ARInvoiceService invoiceService = new ARInvoiceService(invoiceContext, aRInvoice.Tenant);
                invoiceService.Create(autoCreditInvoice);
                UpdateCreditedInvoice(aRInvoice, autoCreditInvoice, invoiceService);             
            }
        }
        private ARInvoicePM GetInteresReportARInvoice(InterestReportPM interestReport)
        {
            ARInvoiceQuery aRInvoiceQuery = new ARInvoiceQuery(Tenant);
           return aRInvoiceQuery.GetSinglePM(interestReport.ARinvoiceId, Tenant);

        }

        private ARInvoicePM CreateautoCreditInvoiceLines(ARInvoicePM autoCreditInvoice, ARInvoicePM invoice)
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
            autoCreditInvoice.InvoiceLines.Add(newInvoiceLine);
            index++;
             }
            return autoCreditInvoice;
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

            InterestReportArgs args = new InterestReportArgs() { Tenant = entityPM.Tenant, InterestReportId = entityPM.Id, RecalculateData = entityPM.RecalculateData };
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
                }, Tenant);
        }

        private void CreateBatchTaskExecutionForRecalculatingData(InterestReportPM entityPM)
        {
            entityPM.InterestReportStatusCode = "5";
            entityPM.RecalculateData = true;
            CreateBatchTaskExecution(entityPM);
        }
        protected override void Validate(InterestReportPM entityPM)
        {
            ValidationResult result = InterestReportValidator.IsInterestReportValid(entityPM);
            if (result != null)
            {
                throw new ApplicationException(result.ErrorMessage);
            }
            base.Validate(entityPM);
        }

    }
}
