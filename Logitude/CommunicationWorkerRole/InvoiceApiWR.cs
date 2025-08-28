using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.BL.Interfaces.Magaya;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Transactions;
using System.Xml.Linq;

namespace CommunicationWorkerRole
{
    class InvoiceApiWR : WorkerEntryPoint
    {

        DbQueueService queueService;
        QueueResponse response = null;
        InvoiceApiService invoiceApiService;
        InvoiceApiCommunicationLogPM invoiceApiCommunicationLog = null;
        string invoiceXml = null;
        int tenant = 0;
        ARInvoicePM aRInvoicePM = null;
        string exception = string.Empty;

        public InvoiceApiWR()
        {

        }

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "InvoiceApiWR";
            DoneItemsInRange = new Dictionary<DateTime, int>();
            ConnectClient();
            return base.OnStart();
        }


        public override void Run()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {

                        ExecuteQueue();
                    }
                    catch (Exception exception)
                    {
                        ExceptionHandler.HandleException(exception, DateTime.Now, 0, null, "InvoiceApi Worker Role queue worker role start", null, null);
                        Thread.Sleep(new TimeSpan(0, 0, 1));
                    }
                }
                else Thread.Sleep(new TimeSpan(0, 0, 1));
            }
        }






        public void ExecuteQueue(TimeSpan? timeSpan = null)
        {
            string selectedQueue = "InvoiceApiWR";
            Stopwatch stopwatch = null;
            if (timeSpan != null)
            {
                stopwatch = Stopwatch.StartNew();
            }

            response = null;
            while (true)
            {
                if (stopwatch != null && timeSpan != null)
                {
                    if (stopwatch.Elapsed > timeSpan)
                    {
                        return;
                    }
                }
                try
                {
                    queueService = new DbQueueService(selectedQueue, 0);
                    response = queueService.Receive(new TimeSpan(0, 0, 1));

                }
                catch (Exception)
                {

                    throw;
                }

                if (response == null || (response != null && response.MessageId == null))
                {
                    break;
                }

                if (response != null && response.MessageValues != null)
                {
                    try
                    {

                        if (response?.MessageValues?.ContainsKey("InvoiceApiId") == true)
                        {

                            invoiceXml = null;
                            string InvoiceApiId = response.MessageValues["InvoiceApiId"].ToString();
                            GeInvoiceApiLog(InvoiceApiId);
                            tenant = invoiceApiCommunicationLog.Tenant;
                            WorkOnce();
                        }

                    }
                    catch
                    {
                        throw;
                    }

                }



                Thread.Sleep(10);
            }
        }


        private void ConnectClient()
        {
            try
            {
                queueService = new DbQueueService();
                queueService.InitializeQueue("InvoiceApiWR", tenant);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "InvoiceApi worker role start", null, null);
            }
        }


        private void WorkOnce()
        {
            try
            {
                invoiceApiService = new InvoiceApiService();
                ProcessStep();
                queueService.Complete();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "InvoiceApi worker role start", null, null);
                queueService.CompleteAsFailed();
            }
        }

        private void ProcessStep()
        {


            if (invoiceApiCommunicationLog == null)
                throw new Exception("InvoiceApiCommunicationLog not found for GUID: " + response.MessageValues["Guid"]);

            switch (invoiceApiCommunicationLog.Step)
            {
                case InvoiceApiStepEnum.OpenInvoiceApiSession:
                case InvoiceApiStepEnum.GetInvoiceApiInvoice:
                    OpenInvoiceApiSession();
                    break;

                case InvoiceApiStepEnum.CloseInvoiceApiSession:
                    CloseInvoiceApiSession();
                    break;
                case InvoiceApiStepEnum.GenerateInvoice:
                    GenerateInvoice();
                    break;
                case InvoiceApiStepEnum.GetConfirmationNumber:
                    SetConfirmationNumberStatusInvoice();
                    break;
                case InvoiceApiStepEnum.ApproveInvoice:
                    ApproveInvoice();
                    break;
                case InvoiceApiStepEnum.PrintOrSendInvoice:
                    PrintOrSendInvoice();
                    break;

                default:
                    throw new Exception("Unknown step: " + invoiceApiCommunicationLog.Step);
            }
        }

        private void OpenInvoiceApiSession()
        {
            try
            {
                UpdateCommunicationStatus(InvoiceApiStepEnum.OpenInvoiceApiSession, InvoiceApiStatusEnum.InProgress);
                invoiceApiService = new InvoiceApiService();

                invoiceApiService.OpenConnection(tenant);
                UpdateCommunicationStatus(InvoiceApiStepEnum.OpenInvoiceApiSession, InvoiceApiStatusEnum.Done);
                GetInvoice();

            }
            catch (Exception ex)
            {
                UpdateCommunicationStatus(InvoiceApiStepEnum.OpenInvoiceApiSession, InvoiceApiStatusEnum.Failed, ex.Message);
                queueService.CompleteAsFailed();

            }

        }
        private void GetInvoice()
        {

            try
            {
                UpdateCommunicationStatus(InvoiceApiStepEnum.GetInvoiceApiInvoice, InvoiceApiStatusEnum.InProgress);
                invoiceXml = invoiceApiService.GetTransaction("IN", 0, invoiceApiCommunicationLog.ExternalID);
                if (string.IsNullOrWhiteSpace(invoiceXml))
                    throw new Exception("GetTransaction failed or returned empty XML");
                AddDocumentToApiCommunicationLog(System.Text.Encoding.UTF8.GetBytes(invoiceXml));
                UpdateCommunicationStatus(InvoiceApiStepEnum.GetInvoiceApiInvoice, InvoiceApiStatusEnum.Done);

                CloseInvoiceApiSession();
            }
            catch (Exception ex)
            {
                UpdateCommunicationStatus(InvoiceApiStepEnum.GetInvoiceApiInvoice, InvoiceApiStatusEnum.Failed, ex.Message);

                queueService.CompleteAsFailed();
            }


        }
        private void CloseInvoiceApiSession()
        {
            try
            {
                UpdateCommunicationStatus(InvoiceApiStepEnum.CloseInvoiceApiSession, InvoiceApiStatusEnum.InProgress);
                invoiceApiService.EndSession();

                UpdateCommunicationStatus(InvoiceApiStepEnum.CloseInvoiceApiSession, InvoiceApiStatusEnum.Done);
                GenerateInvoice();
            }
            catch (Exception ex)
            {
                UpdateCommunicationStatus(InvoiceApiStepEnum.CloseInvoiceApiSession, InvoiceApiStatusEnum.Failed, ex.Message);
                queueService.CompleteAsFailed();

            }
        }
        private void GenerateInvoice()
        {

            try
            {
                if (string.IsNullOrWhiteSpace(invoiceXml))
                    GetBlob();
                UpdateCommunicationStatus(InvoiceApiStepEnum.GenerateInvoice, InvoiceApiStatusEnum.InProgress);
                aRInvoicePM = MapXmlToArinvoice(invoiceXml);
                if (aRInvoicePM == null)
                {
                    throw new Exception("Failed to map XML to ARInvoicePM");
                }
                IInvoiceContext MyContext = InvoiceContext.GetContext(tenant);
                ARInvoiceService service = new ARInvoiceService(MyContext, tenant);
                 service.Create(aRInvoicePM);
                 invoiceApiCommunicationLog.ARInvoiceId = aRInvoicePM.Id;
                UpdateCommunicationStatus(InvoiceApiStepEnum.GenerateInvoice, InvoiceApiStatusEnum.Done);

                SetConfirmationNumberStatusInvoice(service);

            }
            catch (Exception ex)
            {
                UpdateCommunicationStatus(InvoiceApiStepEnum.GenerateInvoice, InvoiceApiStatusEnum.Failed, ex.Message);

                queueService.CompleteAsFailed();
            }

        }
        private void SetConfirmationNumberStatusInvoice(ARInvoiceService service = null)
        {
            try
            {
                if (service == null)
                {
                    IInvoiceContext MyContext = InvoiceContext.GetContext(tenant);
                    service = new ARInvoiceService(MyContext, tenant);

                }
                if (aRInvoicePM == null)
                {
                    GetARInvoice();
                }
                UpdateCommunicationStatus(InvoiceApiStepEnum.GetConfirmationNumber, InvoiceApiStatusEnum.InProgress);

                service.SetConfirmationNumberStatus(aRInvoicePM);
                service.Update(aRInvoicePM, false);
                UpdateCommunicationStatus(InvoiceApiStepEnum.GetConfirmationNumber, InvoiceApiStatusEnum.Done);

                ApproveInvoice(service);
            }
            catch (Exception ex)
            {
                UpdateCommunicationStatus(InvoiceApiStepEnum.GetConfirmationNumber, InvoiceApiStatusEnum.Failed, ex.Message);
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Error setting confirmation number status", null, null);
            }
        }
        public void ApproveInvoice(ARInvoiceService service = null)
        {
            try
            {
                if (service == null)
                {
                    IInvoiceContext MyContext = InvoiceContext.GetContext(tenant);
                    service = new ARInvoiceService(MyContext, tenant);

                }
                if (aRInvoicePM == null)
                {
                    GetARInvoice();
                }
                UpdateCommunicationStatus(InvoiceApiStepEnum.ApproveInvoice, InvoiceApiStatusEnum.InProgress);
                CreateEvent("UPEV", "Invoice added: " + aRInvoicePM.InvoiceNumber);

                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue("ARInvoiceApproveWR", tenant);
                queueservice.Send(new Dictionary<string, string>()
                {
                    { "ARInvoiceId", aRInvoicePM.Id },
                    { "Tenant", tenant.ToString() },
                    { "BatchIdFromInterestInvoice", null },
                    {"invoiceApiCommunicationLogId", invoiceApiCommunicationLog.Id}
                   }, tenant);
            }
            catch (Exception ex)
            {
                UpdateCommunicationStatus(InvoiceApiStepEnum.ApproveInvoice, InvoiceApiStatusEnum.Failed, ex.Message);

                queueService.CompleteAsFailed();
            }
        }
        private void PrintOrSendInvoice()
        {
            try
            {

                if (aRInvoicePM == null)
                {
                    GetARInvoice();
                }
                UpdateCommunicationStatus(InvoiceApiStepEnum.PrintOrSendInvoice, InvoiceApiStatusEnum.InProgress);

                IInvoiceContext invoiceContext = InvoiceContext.GetContext(tenant);

                ARInvoiceService invoiceService = new ARInvoiceService(invoiceContext, tenant);
                invoiceService.PrintOrSendInvoice(aRInvoicePM.Id, aRInvoicePM.InvoiceNumber, tenant, aRInvoicePM.CreatedByUserId);

                UpdateCommunicationStatus(InvoiceApiStepEnum.PrintOrSendInvoice, InvoiceApiStatusEnum.Done);

            }
            catch (Exception ex)
            {
                UpdateCommunicationStatus(InvoiceApiStepEnum.PrintOrSendInvoice, InvoiceApiStatusEnum.Failed, ex.Message);
                queueService.CompleteAsFailed();
            }

        }





        private ARInvoicePM MapXmlToArinvoice(string xml)
        {
            try
            {
                exception = string.Empty;
                ARInvoicePM aRInvoicePM = new ARInvoicePM();
                XDocument xdoc = XDocument.Parse(xml);
                XNamespace ns = "http://www.magaya.com/XMLSchema/V1";
                var invoice = xdoc.Descendants(ns + "Invoice").FirstOrDefault();

                if (invoice == null)
                {
                    throw new Exception("Invoice element not found in XML");
                }
                var tenantPM = TenantQuery.GetSingleTenantPM(tenant);

                CreateArinvoice(aRInvoicePM, invoice, tenantPM);
                CreateArinvoiceLines(aRInvoicePM , invoice, tenantPM);
                CalculatedTotals(aRInvoicePM);
                if (!string.IsNullOrWhiteSpace(exception))
                {
                    throw new Exception(exception);
                }
                return aRInvoicePM;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Error parsing XML", null, null);
                throw ex;
            }
        }

        private void CreateArinvoice(ARInvoicePM aRInvoicePM, XElement invoice , TenantPM tenantPM)
        {
            XNamespace ns = "http://www.magaya.com/XMLSchema/V1";

            if (invoice != null)
            {
                aRInvoicePM.IsGeneralInvoice = true;
                aRInvoicePM.IsExternalEntity = false;
                aRInvoicePM.SetApproved = false;
                aRInvoicePM.BillToPartnerTypeId = "CS";
                aRInvoicePM.Tenant = tenant;
                aRInvoicePM.ARInvoiceTypeCode = "IN";
                aRInvoicePM.LocalCurrencyId = tenantPM?.CurrencyId;
                aRInvoicePM.ProfitCurrencyId = tenantPM?.ProfitCurrencyId;
                aRInvoicePM.ProfitCurrencyExchangeRate = tenantPM.ProfitCurrencyRate;
                var EntityDefinition = invoice.Element(ns+"Entity");
                string entityID = EntityDefinition?.Element(ns+"EntityID")?.Value;
                if (!string.IsNullOrWhiteSpace(entityID))
                {
                    var cardQuery = new CardQuery(tenant);
                    ComputingPartnerTranslationHelper computingPartnerTranslationHelper = new ComputingPartnerTranslationHelper(tenant);
                    var logitudeEntityID = computingPartnerTranslationHelper.GetLogitudeCodeTranslation(entityID, "Magaya", "Card");
                    CardPM card = cardQuery.GetSinglePMByCode(logitudeEntityID, tenant);
                    if (card == null)
                    {
                        exception += $"Card not found for Code: {entityID} \n";
                    }
                    aRInvoicePM.BillToId = card?.Id;
                    aRInvoicePM.BillToName = card?.EnglishName;
                    aRInvoicePM.BillToLocalName = card?.LocalName;
                    aRInvoicePM.SalesmanUserId = card?.SalesmanUserId;
                    aRInvoicePM.VatNumber = card?.VatNumber;
                    aRInvoicePM.PaymentTermId = card?.PaymentTermId;
                    aRInvoicePM.BillToAddressId = card?.BillingAddressId;
                    aRInvoicePM.BillToDisplayNumber = card?.Code;
                    if (!string.IsNullOrWhiteSpace(card?.MainAddressId))
                    {
                        aRInvoicePM.BillToAddressId = card?.MainAddressId;
                    }
                }
                else
                {
                     exception += $"EntityID element not found in XML\n";
                }
                var invoiceNumber = invoice.Element(ns + "Number")?.Value;
                aRInvoicePM.DraftNumber = invoiceNumber;
                var dueDateStr = invoice.Element(ns + "DueDate")?.Value;
                DateTime dueDate;
                if (!string.IsNullOrWhiteSpace(dueDateStr) && DateTime.TryParse(dueDateStr, out dueDate))
                {
                    aRInvoicePM.DueDate = dueDate;
                }


                var totalAmountElement = invoice.Element(ns + "TotalAmount");
                if (totalAmountElement != null && totalAmountElement.Attribute("Currency") != null)
                {
                    string currency = totalAmountElement.Attribute( "Currency").Value;
                    var invoiceCurrency = GetCurrency(currency);
                    aRInvoicePM.InvoiceCurrencyId = invoiceCurrency?.Id;
                    aRInvoicePM.InvoiceCurrencyExchangeRate = GetRate(invoiceCurrency , tenantPM);

                }
                var createdOnStr = invoice.Element(ns + "CreatedOn")?.Value;
                DateTime createdOn;
                if (!string.IsNullOrWhiteSpace(createdOnStr) && DateTime.TryParse(createdOnStr, out createdOn))
                {
                    aRInvoicePM.InvoiceDate = createdOn;
                    //aRInvoicePM.referenceDate = createdOn;
                }
                string email = "system@tenant" + tenant + ".com";
                UserQuery userQuery = new UserQuery(tenant);
                UserPM loggedUser = userQuery.GetSingleUserByEmailOrIdAndTenantOrTenantZero(null, email, tenant);
                if (loggedUser != null)
                {
                    aRInvoicePM.CreatedByUserId = loggedUser.Id;
                    aRInvoicePM.BranchId = loggedUser.BranchId;

                }
              
            }
        }

        private void CreateArinvoiceLines(ARInvoicePM aRInvoicePM, XElement invoice , TenantPM tenantPM)
        {
            XNamespace ns = "http://www.magaya.com/XMLSchema/V1";
            if (invoice != null)
            {
                List<ARInvoiceLinePM> charges = invoice.Descendants(ns + "Charge")
                    .Select(c =>
                    {
                        var chargeDefinition = c.Element(ns + "ChargeDefinition");
                        string chargeTypeCode = chargeDefinition?.Element(ns + "Code")?.Value;
                        Logitude.BL.CommonDataModel.EntityPMs.ChargesTypePM chargeType = null;
                        double vatPercentage = 0;
                        if (string.IsNullOrEmpty(chargeTypeCode))
                        {
                            exception += "ChargeTypeCode is null or empty \n";
                        }
                        else {
                            ComputingPartnerTranslationHelper computingPartnerTranslationHelper = new ComputingPartnerTranslationHelper(tenant);
                            var logitudeChargeTypeCode =  computingPartnerTranslationHelper.GetLogitudeCodeTranslation(chargeTypeCode, "Magaya", "ChargesType");
                            var chargeTypeQuery = new ChargesTypeQuery(tenant);
                            if(logitudeChargeTypeCode !=null)
                                      chargeType = chargeTypeQuery.GetSinglePMByCode(logitudeChargeTypeCode, tenant);
                            if (chargeType == null)
                            {
                                chargeType = chargeTypeQuery.GetSinglePMByCode(chargeTypeCode, tenant);
                            }
                            if (chargeType == null)
                            {
                                exception += $"ChargeType not found for Code: {chargeTypeCode} \n";
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(chargeType.VatTypeId))
                                {
                                    var vatTypePercentageQuery = new VatTypePercentageQuery(tenant);
                                    var percentages = vatTypePercentageQuery.GetVatTypePercentagesForVatType(tenant, chargeType.VatTypeId);
                                    var percentagePM = percentages
                                        .OrderByDescending(d => d.FromDate).FirstOrDefault();
                                    vatPercentage = percentagePM?.Percentage ?? 0;
                                }
                                else
                                {
                                    exception += $"VatTypeId not found for Code: {chargeType.Code} \n";

                                }
                            }

                        }
                        var currencyEle = invoice.Element(ns + "Currency");
                        var accCurrency = new Logitude.BL.CommonDataModel.APIDataContract.ApiV1.Currency();
                        if (currencyEle != null && currencyEle.Attribute("Code") != null)
                        {
                            string currency = currencyEle.Attribute("Code").Value;
                             accCurrency = GetCurrency(currency);
                            if (accCurrency == null)
                            {
                                exception += $"Currency not found for Code: {currency} \n";
                            }
                        }
                        else
                        {
                            exception += "Currency element or attribute is null \n";
                        }


                        var rate = GetRate(accCurrency , tenantPM);
                        var quantity = (double?)c.Element(ns + "Quantity") ?? 0;
                        var unitPrice = (double?)c.Element(ns + "Price") ?? 0;
                        var foriegnCurrencyAmount = quantity * unitPrice;
                        // Calculate ProfitCurrencyAmount for each invoice line
                        double? profitCurrencyAmount = null;
                        if (aRInvoicePM.ProfitCurrencyExchangeRate.HasValue && aRInvoicePM.ProfitCurrencyExchangeRate.Value != 0)
                        {
                            profitCurrencyAmount = (foriegnCurrencyAmount * rate) / aRInvoicePM.ProfitCurrencyExchangeRate.Value;
                            // Round to 2 decimal places
                            profitCurrencyAmount = profitCurrencyAmount.HasValue ? Math.Round(profitCurrencyAmount.Value, 2) : 0;
                        }
                        else
                        {
                            profitCurrencyAmount = 0;
                        }
                        
                        return new ARInvoiceLinePM
                            {
                                Quantity = quantity,
                                UnitPrice = unitPrice,
                                ChargesTypeId = chargeType?.Id,
                                LocalDescription = chargeType?.LocalName,
                                Description = (string)c.Element(ns + "Description") ?? chargeType?.EnglishName,
                                VatTypeId = chargeType?.VatTypeId,
                               LineActionCode = chargeType != null && chargeType.IsExpense ? "2" : "1",
                            ForiegnCurrencyId = accCurrency?.Id,
                               ForiegnExchangeRate =rate,
                               ForiegnCurrencyAmount = foriegnCurrencyAmount,
                               LocalCurrencyAmount = foriegnCurrencyAmount*rate ,
                              InvoiceCurrencyAmount = foriegnCurrencyAmount * rate,
                                VatPercentage = vatPercentage,
                                Tenant = tenant,
                                ARInvoiceId = aRInvoicePM.Id,
                                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                                ProfitCurrencyAmount = profitCurrencyAmount,

                        };
                    })
                    .ToList();
                aRInvoicePM.InvoiceLines.AddRange(charges);
            }
        }

        private void CalculatedTotals(ARInvoicePM aRInvoicePM)
        {
            if (aRInvoicePM == null || aRInvoicePM.InvoiceLines == null || !aRInvoicePM.InvoiceLines.Any())
            {
                throw new Exception("ARInvoicePM or InvoiceLines are null or empty");
            }
            double sumLocalCurrencyAmount = 0;
            double sumInvoiceCurrencyAmount = 0;

            foreach (var item in aRInvoicePM.InvoiceLines)
            {
                if (item.LocalCurrencyAmount != null)
                    sumLocalCurrencyAmount += item.LocalCurrencyAmount.Value;
                if (item.InvoiceCurrencyAmount != null)
                    sumInvoiceCurrencyAmount += item.InvoiceCurrencyAmount.Value;
            }
            double subtotalLocal = sumLocalCurrencyAmount;
            double subtotalInvoice = sumInvoiceCurrencyAmount;
            var dataGroupList = new List<(string VatTypeCell, double LocalVat, double InvoiceVat)>();
            var vatGroups = aRInvoicePM.InvoiceLines
                .Where(f => f.VatTypeId != null) 
                .GroupBy(f => f.VatTypeId);
            foreach (var group in vatGroups)
            {
                double groupLocalVat = 0;
                double groupInvoiceVat = 0;
                foreach (var item in group)
                {
                    if (item.VatPercentage != null)
                    {
                        if (item.LocalCurrencyAmount != null)
                            groupLocalVat += item.VatPercentage.Value * item.LocalCurrencyAmount.Value / 100;
                        if (item.InvoiceCurrencyAmount != null)
                            groupInvoiceVat += item.VatPercentage.Value * item.InvoiceCurrencyAmount.Value / 100;
                    }
                }
                dataGroupList.Add((group.Key, groupLocalVat, groupInvoiceVat));
            }
            foreach (var vat in dataGroupList)
            {
                sumLocalCurrencyAmount += vat.LocalVat;
                sumInvoiceCurrencyAmount += vat.InvoiceVat;
            }

            aRInvoicePM.SubTotalInLocalCurrency = Math.Round(subtotalLocal, 2);
            aRInvoicePM.SubTotalInInvoiceCurrency = Math.Round(subtotalInvoice, 2);
            aRInvoicePM.AmountInLocalCurrency = Math.Round(sumLocalCurrencyAmount, 2);
            aRInvoicePM.AmountInInvoiceCurrency = Math.Round(sumInvoiceCurrencyAmount, 2);

            if (aRInvoicePM.ProfitCurrencyId == aRInvoicePM.InvoiceCurrencyId)
                aRInvoicePM.AmountInProfitCurrency = aRInvoicePM.AmountInInvoiceCurrency;
            else if (aRInvoicePM.ProfitCurrencyExchangeRate.HasValue && aRInvoicePM.ProfitCurrencyExchangeRate.Value != 0)
                aRInvoicePM.AmountInProfitCurrency = aRInvoicePM.AmountInLocalCurrency / aRInvoicePM.ProfitCurrencyExchangeRate.Value;
            else
                aRInvoicePM.AmountInProfitCurrency = 0;

            aRInvoicePM.AmountDue = aRInvoicePM.AmountInInvoiceCurrency ?? 0;
            aRInvoicePM.AmountDueInLocalCurrency = aRInvoicePM.AmountInLocalCurrency ?? 0;
            aRInvoicePM.AmountDueInProfitCurrency = aRInvoicePM.AmountInProfitCurrency ?? 0;
        }
        public void UpdateCommunicationStatus(string step, string status, string exception = null)
        {
            try
            {
                if (invoiceApiCommunicationLog != null && !string.IsNullOrEmpty(status) && !string.IsNullOrEmpty(step))
                {
                    IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
                    InvoiceApiCommunicationLogUpdateService invoiceApiCommunicationLogUpdateService = new InvoiceApiCommunicationLogUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
                    if (exception != null)
                    {                      
                        CreateEvent("ICFD", exception);

                    }
                    invoiceApiCommunicationLogUpdateService.UpdateCommunicationStatus(invoiceApiCommunicationLog, tenant, step, status, exception);

                }

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Error updating communication status", null, null);
            }
        }


        public void GeInvoiceApiLog(string invoiceApiId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(invoiceApiId))
                    throw new ArgumentException("InvoiceApiId is null or empty", nameof(invoiceApiId));

                var queryService = new InvoiceApiCommunicationLogQueryService(tenant);
                invoiceApiCommunicationLog = queryService.GetSingle(invoiceApiId, false, false) ?? throw new InvalidOperationException($"Log not found for InvoiceApiId: {invoiceApiId}");
                if (invoiceApiCommunicationLog == null)
                    throw new Exception("InvoiceApiCommunicationLog not found for InvoiceApiId: " + invoiceApiId);
                CreateEvent("UPEV", "WR Started");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SaveToBlob(byte[] byteData, Document document)
        {
            var filename = $"{document.Id}.{document.Extension}";
            var filePath = $"tenant{tenant}/{StorageAcountDetails.GetBlobNameByLocation(filename, document.Folder)}";


            var fileInfo = new BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = document.Tenant,
                FileSize = byteData.Length
            };

            var blobService = (IBlobService)ContainerAccessor.Container.Resolve(
                typeof(IBlobService), "StorageService", new ParameterOverride("", 1));

            blobService.Write(byteData, fileInfo);
            Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine($"SetBlob: {filePath}");
        }

        private void GetBlob()
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            DocumentRepository documentrepository = new DocumentRepository(commonContext);
            var document = documentrepository.GetSingleDocument(tenant, invoiceApiCommunicationLog.DocumentId);
            if (document == null)
            {
                throw new Exception($"Document not found for Id: {invoiceApiCommunicationLog.DocumentId}");
            }

            IBlobService iBlobService = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;

            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                Tenant = document.Tenant,
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                FileSize = document.FileSize

            };

            byte[] byteData = iBlobService.Read(fileInfo);
            if (byteData == null || byteData.Length == 0)
            {
                throw new Exception($"Blob not found for {document.Id}");
            }
            invoiceXml = System.Text.Encoding.UTF8.GetString(byteData);
        }
        public void AddDocumentToApiCommunicationLog(byte[] ByteData)
        {
            try
            {

                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
                    DocumentRepository documentrepository = new DocumentRepository(commonContext);
                    IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
                    InvoiceApiCommunicationLogUpdateService invoiceApiCommunicationLogUpdateService = new InvoiceApiCommunicationLogUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);

                    Document document = new Document()
                    {
                        CreateDate = DateTime.Now,
                        Extension = "xml",
                        FileSize = ByteData.Length,
                        Tenant = Convert.ToInt32(tenant),
                        Id = IdCounter.GetNumber("Document", tenant),
                        HasFile = true,
                        Folder = "others",
                    };

                    documentrepository.Add(document);
                    documentrepository.SubmitChanges();

                    invoiceApiCommunicationLog.DocumentId = document.Id;
                    invoiceApiCommunicationLog.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

                    invoiceApiCommunicationLogUpdateService.Update(invoiceApiCommunicationLog, true);


                    var stopwatch = System.Diagnostics.Stopwatch.StartNew();

                    SaveToBlob(ByteData, document);
                    CreateEvent("UPEV", "Document added:" + document.Id);
                    stopwatch.Stop();


                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void GetARInvoice()
        {
            ARInvoiceQuery aRInvoiceQueryService = new ARInvoiceQuery(tenant);
            aRInvoicePM = aRInvoiceQueryService.GetSinglePM(invoiceApiCommunicationLog?.ARInvoiceId, tenant);
            if (aRInvoicePM == null)
                throw new Exception("ARInvoicePM not found for Id: " + invoiceApiCommunicationLog?.ARInvoiceId);
        }



        private void CreateEvent(string eventCode, string Notes = null)
        {
            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                EntityId = invoiceApiCommunicationLog.Id,
                Tenant = tenant,
                ObjectTableName = "InvoiceApiCommunicationLog",
                IsAddedManually = false,
                EventTypeCode = eventCode,
                Notes = Notes,
            });
        }


        private Logitude.BL.CommonDataModel.APIDataContract.ApiV1.Currency GetCurrency(string currencyCode)
        {
            if (string.IsNullOrWhiteSpace(currencyCode))
            {
                return null;
            }
            ComputingPartnerTranslationHelper computingPartnerTranslationHelper = new ComputingPartnerTranslationHelper(tenant);
            var transCurrency = computingPartnerTranslationHelper.GetLogitudeCodeTranslation(currencyCode, "Magaya", "Currency");
            Logitude.BL.CommonDataModel.APIDataContract.ApiV1.Currency accCurrency = null;

            CurrencyQueryService currencyQueryService = new CurrencyQueryService(tenant);
            if(string.IsNullOrWhiteSpace(transCurrency))
            {
                transCurrency = currencyCode;
            }
            return currencyQueryService.GetCurrencyByCode(transCurrency , tenant);
            
        }

        private double? GetRate(Logitude.BL.CommonDataModel.APIDataContract.ApiV1.Currency accCurrency , TenantPM tenantPM)
        {
            RatesTableRepository ratesTablesRepository = new RatesTableRepository(tenant);
            var ratesTableQuery = new RatesTableQuery(ratesTablesRepository);

            double? rate = 1;
            if (tenantPM?.CurrencyId != accCurrency?.Id)
            {
                rate = ratesTableQuery.GetLastRateByValueDate(tenant, accCurrency?.Id, tenantPM?.CurrencyId, DateTime.Now)?.Rate;
            }
            return rate;
        }
    }


}
