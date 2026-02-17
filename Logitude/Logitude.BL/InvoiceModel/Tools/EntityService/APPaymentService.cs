using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.DataMapping;
using Logitude.BL.InvoiceModel.Tools.TraceEvents;
using Logitude.BL.InvoiceModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Logitude.Server.Tools;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using Microsoft.Practices.Unity;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Data.Helpers;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using System.Xml.Serialization;

namespace Logitude.BL.InvoiceModel.Tools.EntityService
{
    public class APPaymentService
    {               
        private int tenant;
        public APPayment payment { get; set; }
        private APPaymentPM entityPM;
        private ICommonDataContext myCommonContext;
        private IInvoiceContext objectContext;
        private APInvoiceRepository invoiceRepository;
        private APPaymentRepository paymentRepository;
        private APInvoicePaymentRepository invoicePaymentRepository;
        private bool isNewEntity;
        private ContactPM loggedContact;
        private List<APPaymentInvoicePM> changedList;
        private bool SetVoided = false;
        public APPaymentService(IInvoiceContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.myCommonContext = CommonDataContext.GetContext(tenant);
            this.invoiceRepository = new APInvoiceRepository(this.objectContext);
            this.paymentRepository = new APPaymentRepository(this.objectContext);
            this.invoicePaymentRepository = new APInvoicePaymentRepository(this.objectContext);
            this.changedList = new List<APPaymentInvoicePM>();
            this.loggedContact = new ContactQuery(tenant).GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), tenant, true);
        }

        public void Create(APPaymentPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.changedList = theEntityPm.PaymentInvoices;
            this.payment = new APPayment();
            this.InitializeComponent();

            APPaymentValidator.Validate(theEntityPm);
            APPaymentTracing.Trace(theEntityPm, payment, isNewEntity);

            foreach (APPaymentInvoicePM item in theEntityPm.PaymentInvoices)
            {
                item.ChangeSetOp = ChangeSetOperation.Insert;
                this.CreatePaymentInvoice(item);
            }
            this.InitializeTransferComponents();
            var setApproved = theEntityPm.SetApproved;
            var setVoided = theEntityPm.SetVoided;
            var setCancelApproved = theEntityPm.SetCancelApproval;
            APPaymentMapping.MapEntity(theEntityPm, payment, isNewEntity);
            paymentRepository.Add(payment);
            paymentRepository.SubmitChanges();
            invoicePaymentRepository.SubmitChanges();
            
            this.UpdatePaymentOpenAmount();
            APPaymentHelper service = new APPaymentHelper();
            service.APPaymentQuickbooksValidating(theEntityPm, setApproved, false, payment, this.objectContext, this.myCommonContext, setCancelApproved);

            this.BuildSearchFields();

            //Full Accounting 
            AddAPPaymentJournalAndJournalLines(theEntityPm, setApproved);
            this.VoidAPPaymentInFullAccounting(theEntityPm, setVoided);

            paymentRepository.Update(payment);
            paymentRepository.SubmitChanges();
            this.TraceConnected();
            this.GetForeignFields();
        }

        private void VoidAPPaymentInFullAccounting(APPaymentPM theEntityPm, bool setVoided)
        {
            TenantRepository tenantRepository = new TenantRepository(tenant);
            Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);
            if (tenantPOCO.AccountingActivated && setVoided)
            {
                bool useLocal = true;
                var user = GetLoggedContact(tenant);
                if (user != null) useLocal = !(GetLoggedContact(tenant).DontShowLocal);
                if(theEntityPm.PaymentMethodCode == "CH")
                {
                    IPaymentChequeQueryServiceExt query = ContainerAccessor.Container.Resolve(typeof(IPaymentChequeQueryServiceExt), "PaymentChequeQueryServiceExt", new ParameterOverride("", 1)) as IPaymentChequeQueryServiceExt;
                    List<PaymentChequePM> PaymentCheques = query.GetPaymentChequesByPaymentId(theEntityPm.Id, tenant);
                    if (PaymentCheques != null)
                    {
                        var list = PaymentCheques.Where(a => a.PaymentChequeStatusCode == "3").ToList();
                        if (list != null && list.Count() != 0)
                        {
                            string msg = TranslateTextsClass.Translate("APPayment.M.CheckPaymentChequesBeforeCancel", theEntityPm.Tenant, useLocal);
                            throw new ApplicationException(msg);
                        }
                        else
                        {
                            CompleteCancelAPPaymentInFullAccounting(theEntityPm);
                        }
                    }
                }
                else
                {
                    CompleteCancelAPPaymentInFullAccounting(theEntityPm);
                }
            }
        }
        private void CompleteCancelAPPaymentInFullAccounting(APPaymentPM theEntityPm)
        {
            IJournalQueryServiceExt journalQuery = ContainerAccessor.Container.Resolve(typeof(IJournalQueryServiceExt), "JournalQueryServiceExt", new ParameterOverride("", 1)) as IJournalQueryServiceExt;
            JournalPM journalPM = journalQuery.GetJournalIdByAccountingEntityId(theEntityPm.Id, theEntityPm.Tenant);
            if (journalPM != null)
            {
                var journalUpdate = ContainerAccessor.Container.Resolve(typeof(IJournalVoidUpdateServiceExt), "JournalVoidUpdateServiceExt", new ParameterOverride("", 1)) as IJournalVoidUpdateServiceExt;
                journalUpdate.Update(journalPM, new StornoOverrideM()
                {
                    AccountingEntityCode = "5",
                    AccountingEntityId = theEntityPm.Id,
                    AccountingEntityReference = theEntityPm.PaymentNo,
                });
            }

            // Update Payment Cheques
            IPaymentChequeQueryServiceExt paymentChequeQuery = ContainerAccessor.Container.Resolve(typeof(IPaymentChequeQueryServiceExt), "PaymentChequeQueryServiceExt", new ParameterOverride("", 1)) as IPaymentChequeQueryServiceExt;
            List<PaymentChequePM> paymentCheques = paymentChequeQuery.GetPaymentChequesByPaymentId(theEntityPm.Id, theEntityPm.Tenant);
            if (paymentCheques != null)
            {
                foreach (var item in paymentCheques)
                {
                    IPaymentChequeUpdateServiceExt paymentChequeUpdate = ContainerAccessor.Container.Resolve(typeof(IPaymentChequeUpdateServiceExt), "PaymentChequeUpdateServiceExt", new ParameterOverride("", 1)) as IPaymentChequeUpdateServiceExt;
                    item.PaymentChequeStatusCode = "4";
                    item.ChangeSetOp = ChangeSetOperation.Update;
                    paymentChequeUpdate.Update(item);
                }
            }
        }
        private List<APPaymentInvoicePM> APPaymentInvoiceChangeSet;
        public void SetChangeSet(List<APPaymentInvoicePM> APPaymentInvoiceChangeSet)
        {
            this.APPaymentInvoiceChangeSet = APPaymentInvoiceChangeSet;
        }

        public void Update(APPaymentPM theEntityPm, bool mapComposition = false)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            if (mapComposition)
            {
                this.changedList = theEntityPm.PaymentInvoices;
            }
            else
            {
                this.changedList = APPaymentInvoiceChangeSet;
            }

            this.payment = paymentRepository.GetSingleAPPayment(theEntityPm.Id);

            this.ValidateHigherStatus();

            this.InitializeComponent();

            APPaymentValidator.Validate(theEntityPm);
            APPaymentTracing.Trace(theEntityPm, payment, isNewEntity);

            foreach (APPaymentInvoicePM item in changedList)
            {
                switch (item.ChangeSetOp)
                {
                    case ChangeSetOperation.Insert:
                        {
                            this.CreatePaymentInvoice(item);
                            break;
                        }

                    case ChangeSetOperation.Update:
                        {
                            this.UpdatePaymentInvoice(item);
                            break;
                        }

                    case ChangeSetOperation.Delete:
                        {
                            this.DeletePaymentInvoice(item);
                            break;
                        }

                    default: { break; }
                }
            }
            this.InitializeTransferComponents();
            var setApproved = theEntityPm.SetApproved;
            var setVoided = theEntityPm.SetVoided;
            var setCancelApproved = theEntityPm.SetCancelApproval;
            var SetReSendQBO = theEntityPm.SetReSendQBO;

            APPaymentMapping.MapEntity(theEntityPm, payment, isNewEntity);   
            paymentRepository.Update(payment);
            paymentRepository.SubmitChanges();           
            invoicePaymentRepository.SubmitChanges();

            this.UpdatePaymentOpenAmount();

            APPaymentHelper service = new APPaymentHelper();
            if (payment.ExternalAccountingEntityId != null || SetReSendQBO)
            {
                service.APPaymentQuickbooksValidating(theEntityPm, true, false, payment, this.objectContext, this.myCommonContext, setCancelApproved);
            }
            else
            {
                service.APPaymentQuickbooksValidating(theEntityPm, setApproved, false, payment, this.objectContext, this.myCommonContext, setCancelApproved);
            }

            this.BuildSearchFields();

            //Full Accounting 
            AddAPPaymentJournalAndJournalLines(theEntityPm, setApproved);
            VoidAPPaymentInFullAccounting(theEntityPm, setVoided);

            paymentRepository.Update(payment);
            paymentRepository.SubmitChanges();
            this.TraceConnected();
            this.GetForeignFields();
        }

        private void ValidateHigherStatus()
        {
            if (!isNewEntity)
            {
                if (this.payment.StatusCode == "AD")
                {
                    bool throwException = false;

                    if (string.IsNullOrEmpty(this.entityPM.StatusCode) || this.entityPM.StatusCode == "DR")
                    {
                        throwException = true;
                    }

                    else if (this.entityPM.SetApproved)
                    {
                        throwException = true;
                    }

                    if (throwException)
                    {
                        string msg = "This payment is already approved";                        
                        throw new ApplicationException(msg);
                    }
                }
            }
        }

        private List<APInvoice> invoicesList;
        private APInvoice GetInvoice(string invoiceId, int tenant)
        {
            APInvoice myResult = null;

            if (invoicesList == null)
            {
                invoicesList = new List<APInvoice>();
            }

            myResult = invoicesList.Where(d => d.Id == invoiceId && d.Tenant == tenant).FirstOrDefault();

            if (myResult == null)
            {
                myResult = invoiceRepository.GetSingleAPInvoice(invoiceId, tenant);

                if (myResult != null && !invoicesList.Contains(myResult))
                {
                    invoicesList.Add(myResult);
                }
            }

            return myResult;
        }

        #region InitializeComponent
        private void InitializeComponent()
        {
            if (string.IsNullOrEmpty(entityPM.Id))
            {
                entityPM.Id = IdCounter.GetNumber("APPayment", entityPM.Tenant).ToString();
            }

            if (string.IsNullOrEmpty(entityPM.PaymentNo))
            {
                entityPM.PaymentNo = TableCounter.GetNumber(entityPM.Tenant, "APPT", "DR", null).ToString();
            }

            if (entityPM.SetApproved)
            {
                if (entityPM.StatusCode != "AD")
                {
                    entityPM.StatusCode = "AD";
                }
                UpdateDocOutNeedsRebuild();
            }

            else if (entityPM.SetVoided)
            {
                if (entityPM.StatusCode != "VD")
                {
                    entityPM.StatusCode = "VD";
                }
                UpdateDocOutNeedsRebuild();
            }

            else if (entityPM.SetCancelApproval)
            {
                if (entityPM.StatusCode != "DR")
                {
                    entityPM.StatusCode = "DR";
                }
                UpdateDocOutNeedsRebuild();
            }

            else if (string.IsNullOrEmpty(entityPM.StatusCode))
            {
                entityPM.StatusCode = "DR";
            }

            if (entityPM.RegisterDate != null)
            {
                entityPM.RegisterDate = entityPM.RegisterDate.Value.Date;
            }
        }
        #endregion

        #region Transfer 
        private void InitializeTransferComponents()
        {
            this.InitializeTransferFields();           
        }
        private void InitializeTransferFields()
        {
            bool isInitializing = true; 

             if (payment.TransferStatusCode == "TR")
            {
                isInitializing = false;
                entityPM.TransferError = null;
                entityPM.TransferStatusCode = "TR";
            }

            else if (payment.TransferStatusCode == "IP")
            {
                isInitializing = false;
                entityPM.TransferError = null;
            }

            else if (entityPM.TransferStatusCode == "BL")
            {
                isInitializing = false;
                entityPM.TransferError = null;
                entityPM.TransferStatusCode = "BL";
            }

            if (isInitializing)
            {
                #region
                bool isReady = true;
                string myError = null;
                CardRepository cardRep = new CardRepository(payment.Tenant);
                CurrencyRepository currencyRep = new CurrencyRepository(payment.Tenant);
                Card card = cardRep.GetSingleCard(payment.VendorId, payment.Tenant);
                Currency currency = currencyRep.GetSingleCurrency(payment.PaymentCurrencyId, payment.Tenant);
                string currencyError = "Currency External Id is missing";
                if (currency != null && !string.IsNullOrEmpty(currency.Code))
                {
                    currencyError = "Currency: " + currency.Code + ". External Id is missing";
                }
                if (card != null && FieldIsEmpty(card.PayablesAccountingCard))
                {
                    isReady = false;
                    myError = "Bill To: " + entityPM.VendorName + ". External Id is missing";
                }
                if (currency != null && FieldIsEmpty(currency.AccountingExternalCode))
                {
                    isReady = false;
                    myError = string.IsNullOrEmpty(myError) ? currencyError : myError + "," + currencyError;
                }

                if (isReady)
                {
                    entityPM.TransferStatusCode = "RD";
                    entityPM.TransferError = null;
                }

                else
                {
                    entityPM.TransferStatusCode = "NR";
                    entityPM.TransferError = myError;
                }
                #endregion
            }
        }

        private bool FieldIsEmpty(string myField)
        {
            bool myResult = false;

            if (myField == null || (myField != null && string.IsNullOrEmpty(myField.Trim())))
            {
                myResult = true;
            }

            return myResult;
        }
        #endregion 

        #region PaymentInvoice
        private void CreatePaymentInvoice(APPaymentInvoicePM item)
        {
            item.APPaymentId = entityPM.Id;
            item.ForeignCurrencyId = entityPM.PaymentCurrencyId;
            item.Id = IdCounter.GetNumber("APInvoicePayment", entityPM.Tenant);

            APInvoicePayment newObject = new APInvoicePayment();
            APPaymentMapping.MapEntityInvoicePyament(item, newObject, true);
            invoicePaymentRepository.Add(newObject);
        }

        private void UpdatePaymentInvoice(APPaymentInvoicePM item)
        {
            APInvoicePayment invoicePayment = invoicePaymentRepository.GetSingleAPInvoicePayment(item.Id, entityPM.Tenant);

            if (invoicePayment != null)
            {
                APPaymentMapping.MapEntityInvoicePyament(item, invoicePayment, false);
                invoicePaymentRepository.Update(invoicePayment);
            }
        }

        private void DeletePaymentInvoice(APPaymentInvoicePM item)
        {
            List<APInvoicePayment> data = invoicePaymentRepository.GetAPInvoicePayments(item.APPaymentId, item.APInvoiceId, tenant).ToList();

            if (data != null)
            {
                foreach (APInvoicePayment deletedItem in data)
                {
                    invoicePaymentRepository.Remove(deletedItem);
        }
            }
        }
        #endregion

        #region Update Amounts
        public void UpdatePaymentOpenAmount()
        {
            bool isClosed = entityPM.IsClosed;
            string StatusCode = entityPM.StatusCode;
            double? Amount = MethodHelper.Roundd(entityPM.AmountInPaymentCurrency, 2);
            double? PaidAmount = 0;
            double? OpenAmount = 0;

            List<APInvoicePayment> allConnectedItems = invoicePaymentRepository.GetAPInvoicePaymentByPaymentId(entityPM.Id, entityPM.Tenant).ToList();

            if (allConnectedItems.Count == 0)
            {
                isClosed = false;
                OpenAmount = Amount;

                if (StatusCode != "VD" && StatusCode != "DR")
                {
                        StatusCode = "AD";
                    }
                }

            else
            {
                List<string> ids = new List<string>();

                foreach (APInvoicePayment item in allConnectedItems)
                {
                    if (!ids.Contains(item.APInvoiceId))
                    {
                        if (item.PaymentAmount != null)
                        {
                            PaidAmount += item.PaymentAmount;
                        }

                        ids.Add(item.APInvoiceId);
                    }
                }

                PaidAmount = MethodHelper.Roundd(PaidAmount, 2);

                if (PaidAmount <= Amount)
                {
                    OpenAmount = MethodHelper.Round((Amount - PaidAmount), 2);

                    if (OpenAmount == 0)
                    {
                        isClosed = true;

                        if (StatusCode == "AD")
                        {
                            StatusCode = "CL";
                        }                   
                    }

                    else
                    {
                        isClosed = false;

                        if (StatusCode != "DR")
                        {
                            StatusCode = "AD";
                        }
                    }
                }

                else
                {
                    throw new Exception("The amount paid is not suitable to the total payment amount!!");
                }
            }

            payment.IsClosed = entityPM.IsClosed = isClosed;
            payment.StatusCode = entityPM.StatusCode = StatusCode;
            payment.OpenAmount = entityPM.OpenAmount = OpenAmount;
            payment.AmountInPaymentCurrency = entityPM.AmountInPaymentCurrency = Amount;

            if (this.SetVoided)
            {
                foreach (APPaymentInvoicePM item in changedList)
                {
                    UpdateInvoiceAmounts(item.APInvoiceId);
                }
            }

            else
            {
                foreach (APPaymentInvoicePM item in changedList.Where(d => d.ChangeSetOp != ChangeSetOperation.None))
                {
                    UpdateInvoiceAmounts(item.APInvoiceId);
                }
            }
        }

        private void UpdateInvoiceAmounts(string myInvoiceId)
        {
            if (!string.IsNullOrEmpty(myInvoiceId))
            {
                APInvoice invoice = this.GetInvoice(myInvoiceId, tenant);

                if (invoice != null)
                {
                    if (invoice.StatusCode == "VD")
                    {
                        throw new Exception("Invoice (" + invoice.InvoiceNumber + ") is Voided");
                    }

                    else
                    {
                        #region
                        bool IsClosed = invoice.IsClosed;
                        string StatusCode = invoice.StatusCode;
                        double? Amount = MethodHelper.Roundd(invoice.AmountInInvoiceCurrency, 2);
                        double? PaidAmount = 0;

                        List<APInvoicePayment> allConnectedItems = invoicePaymentRepository.GetAPInvoicePaymentByInvoiceId(invoice.Id, invoice.Tenant).ToList();
                        if (allConnectedItems.Count > 0)
                        {
                            List<string> ids = new List<string>();

                            foreach (APInvoicePayment item in allConnectedItems)
                            {
                                if (!ids.Contains(item.APPaymentId))
                                {
                                    if (item.ForeignAmount != null)
                                    {
                                        PaidAmount += item.ForeignAmount;
                                    }

                                    ids.Add(item.APPaymentId);
                                }
                            }
                        }

                        PaidAmount = MethodHelper.Roundd(PaidAmount.Value, 2);

                        double invoiceAmount = MethodHelper.Roundd(invoice.AmountInInvoiceCurrency.Value, 2);

                        if ((invoiceAmount < 0) || (PaidAmount <= invoiceAmount))
                        {
                            invoice.IsClosed = false;
                            if (invoice.StatusCode == "PD" || invoice.StatusCode == "PP")
                            {
                                if (PaidAmount != 0)
                                {
                                    invoice.StatusCode = "PP";
                                }

                                else
                                {
                                    invoice.StatusCode = "AD";
                                }
                            }

                            double? invoiceAmountDue = MethodHelper.Round((invoiceAmount - PaidAmount), 2);

                            invoice.AmountDue = invoiceAmountDue;
                            invoice.AmountDueInLocalCurrency = MethodHelper.Round((invoice.AmountDue * invoice.InvoiceCurrencyExchangeRate), 2);
                            invoice.AmountDueInProfitCurrency = MethodHelper.Round((invoice.AmountDueInLocalCurrency / invoice.ProfitCurrencyExchangeRate), 2);
                            if (invoiceAmountDue == 0)
                            {
                                invoice.IsClosed = true;
                                invoice.StatusCode = "PD";
                            }

                            else if (invoiceAmountDue > 0 && invoiceAmountDue < invoiceAmount)
                            {
                                invoice.IsClosed = false;
                                invoice.StatusCode = "PP";
                            }

                            else if (invoiceAmountDue < 0 && invoiceAmountDue > invoiceAmount)
                            {
                                invoice.IsClosed = false;
                                invoice.StatusCode = "PP";
                            }

                            else if (invoiceAmountDue < 0 && invoiceAmount > 0)
                            {
                                throw new Exception("The Amount due is not suitable to the total amount paid");
                            }
                        }

                        else
                        {
                            throw new Exception("The Amount due is not suitable to the total amount paid!!");
                        }

                        invoiceRepository.Update(invoice);
                        #endregion
                    }
                }
            }
        }

        private void UpdateInvoiceAmounts_Old(APPaymentInvoicePM paymentInvoice)
        {
            APInvoice invoice = this.GetInvoice(paymentInvoice.APInvoiceId, tenant);

            if (invoice != null)
            {
                double? allConnectedPaymentsAmount = 0;

                invoice.AmountInInvoiceCurrency = MethodHelper.Roundd(invoice.AmountInInvoiceCurrency, 2);

                IQueryable<APInvoicePayment> allInvoicePayments = invoicePaymentRepository.GetAPInvoicePaymentByInvoiceId(invoice.Id, invoice.Tenant);

                if (allInvoicePayments != null && allInvoicePayments.Count() > 0)
                {
                    List<string> paymentIds = new List<string>();

                    foreach (APInvoicePayment item in allInvoicePayments)
                    {
                        if (!paymentIds.Contains(item.APPaymentId))
                        {
                            if (item.ForeignAmount != null)
                            {
                                allConnectedPaymentsAmount += item.ForeignAmount;
                            }

                            paymentIds.Add(item.APPaymentId);
                        }
                    }
                }

                allConnectedPaymentsAmount = MethodHelper.Roundd(allConnectedPaymentsAmount, 2);

                if ((invoice.AmountInInvoiceCurrency < 0) || (allConnectedPaymentsAmount <= invoice.AmountInInvoiceCurrency))
                {
                    invoice.IsClosed = false;
                    if (invoice.StatusCode == "PD" || invoice.StatusCode == "PP")
                    {
                        if (allConnectedPaymentsAmount != 0)
                        {
                            invoice.StatusCode = "PP";
                        }

                        else
                        {
                            invoice.StatusCode = "AD";
                        }

                    }

                    invoice.AmountDue = MethodHelper.Round((invoice.AmountInInvoiceCurrency.Value - allConnectedPaymentsAmount.Value), 2);
                    invoice.AmountDueInLocalCurrency = MethodHelper.Round((invoice.AmountDue * invoice.InvoiceCurrencyExchangeRate), 2);
                    invoice.AmountDueInProfitCurrency = MethodHelper.Round((invoice.AmountDueInLocalCurrency / invoice.ProfitCurrencyExchangeRate), 2);

                    if (invoice.AmountDue == 0)
                    {
                        invoice.IsClosed = true;
                        invoice.StatusCode = "PD";
                    }

                    else if (invoice.AmountDue > 0 && invoice.AmountDue < invoice.AmountInInvoiceCurrency)
                    {
                        invoice.IsClosed = false;
                        invoice.StatusCode = "PP";
                    }

                    else if (invoice.AmountDue < 0 && invoice.AmountDue > invoice.AmountInInvoiceCurrency)
                    {
                        invoice.IsClosed = false;
                        invoice.StatusCode = "PP";
                    }

                    else if (invoice.AmountDue < 0 && invoice.AmountInInvoiceCurrency > 0)
                    {
                        throw new Exception("The amount paid is not suitable to the amount due");
                    }
                }

                else
                {
                    throw new Exception("The amount paid is not suitable to the amount due");
                }

                invoiceRepository.Update(invoice);
            }
        }
        #endregion

        #region SearchField
        private void BuildSearchFields()
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.PaymentNo);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.StatusCode);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.PaymentMethodCode);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ChequeOrPaymentRef);

            #region Card
            if (!string.IsNullOrEmpty(entityPM.VendorId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.VendorId, tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                }
            }
            #endregion

            #region Currency
            if (!string.IsNullOrEmpty(entityPM.PaymentCurrencyId))
            {
                Currency myCurrency = CurrencyRepository.GetSingleCurrency(entityPM.PaymentCurrencyId, tenant, true);
                if (myCurrency != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCurrency.Code);
                }
            }
            #endregion

            #region Invoices
            if (entityPM.PaymentInvoices != null)
            {
                foreach (APPaymentInvoicePM item in entityPM.PaymentInvoices)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, item.APInvoiceNumber);
                }
            }
            #endregion

            entityPM.SearchFields = mySearchFields;
            payment.SearchFields = mySearchFields;
        }
        #endregion

        private void TraceConnected()
        {
            foreach (APPaymentInvoicePM item in changedList)
            {
                switch (item.ChangeSetOp)
                {
                    case ChangeSetOperation.Insert:
                        {
                            APInvoice invoice = this.GetInvoice(item.APInvoiceId, tenant);
                            if (invoice != null)
                            {
                                EventTracer.CreateTraceEvent(new EventTracerArgs()
                                {
                                    Tenant = entityPM.Tenant,
                                    EventTypeCode = "COIN",
                                    UserId = loggedContact.Id,
                                    EntityId = item.APInvoiceId,
                                    ObjectTableName = "APInvoice",
                                    Notes = "Connected with Payment: " + entityPM.PaymentNo + " with Amount Due equals to: " + invoice.AmountDue,
                                });
                            }

                            break;
                        }

                    case ChangeSetOperation.Delete:
                        {
                            APInvoice invoice = this.GetInvoice(item.APInvoiceId, tenant);
                            if (invoice != null)
                            {
                                EventTracer.CreateTraceEvent(new EventTracerArgs()
                                {
                                    Tenant = entityPM.Tenant,
                                    EventTypeCode = "APID",
                                    UserId = loggedContact.Id,
                                    EntityId = item.APInvoiceId,
                                    ObjectTableName = "APInvoice",
                                    Notes = "Disconnected from Payment: " + entityPM.PaymentNo,
                                });
                            } 
                            break;
                        }

                    default: { break; }
                }
            }
        }

        public void GetForeignFields()
        {          
            if (payment.StatusCode != null)
            {
                APPaymentStatusRepository myRepository = new APPaymentStatusRepository(objectContext);
                APPaymentStatus status = myRepository.GetSingleAPPaymentStatus(payment.StatusCode);
                entityPM.StatusName = status.Name;
            }

            if (payment.TransferStatusCode != null)
            {
                APPaymentTransferStatusRepository myRepository = new APPaymentTransferStatusRepository(objectContext);
                APPaymentTransferStatus status = myRepository.GetSingleAPPaymentTransferStatus(payment.TransferStatusCode);
                entityPM.TransferStatusName = status.Name;
            }
        }

        private void UpdateDocOutNeedsRebuild()
        {
            DocumentOutRepository documentOutRepository = new DocumentOutRepository(myCommonContext);
            DocumentOut docOut = documentOutRepository.GetDocumentOutByEntityAndChildEntity(payment.Id, null);
            if (docOut != null)
            {
                docOut.NeedsRebuild = true;
                documentOutRepository.Update(docOut);
                documentOutRepository.SubmitChanges();
            }
        }

        #region Journal & Journal Lines
        private void AddAPPaymentJournalAndJournalLines(APPaymentPM theEntityPm, bool setApproved)
        {
            int tenant = theEntityPm.Tenant;
            if (setApproved)
            {

                ///throw new Exception("[ARInvoicePayments]");

                TenantRepository tenantRepository = new TenantRepository(tenant);
                Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);
                if (tenantPOCO.AccountingActivated)
                {
                    this.UpdateCashBook(theEntityPm);
                    // Insert Journal 
                    JournalPM journal = new JournalPM();
                    journal.Tenant = tenant;
                    journal.JournalNumber = "1";
                    journal.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    journal.AccountingDate = theEntityPm.RegisterDate.Value;
                    journal.TypeCode = "0";
                    journal.StatusCode = "2";
                    journal.CreatedByUserId = theEntityPm.CreatedByUserId;
                    journal.AccountingEntityCode = "5";
                    journal.AccountingEntityId = theEntityPm.Id;
                    journal.AccountingEntityReference = theEntityPm.PaymentNo;
                    journal.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    journal.UpdatedByUserId = theEntityPm.UpdatedByUserId;
                    journal.ApproveDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    journal.ApprovedByUserId = theEntityPm.ApprovedByUserId;
                    journal.ChangeSetOp = ChangeSetOperation.Insert;

                    // Insert Journal Lines 
                    // [Credit]
                    JournalLinePM journalLine = new JournalLinePM();
                    journalLine.Tenant = tenant;
                    journalLine.JournalId = journal.Id;
                    journalLine.Line = 1;
                    journalLine.ActionCode = "1";
                    journalLine.ActionTypeCodeEnum = MyJournalActionTypeEnum.Credit;
                    journalLine.DocumentDate = theEntityPm.ValueDate.Value;
                    journalLine.AccountingDate = theEntityPm.RegisterDate.Value;
                    journalLine.DueDate = theEntityPm.ValueDate.Value;
                    journalLine.LocalAmount = (decimal)theEntityPm.AmountInLocalCurrency - (decimal)theEntityPm.TaxDeductionLocalAmount;
                    journalLine.CurrencyId = theEntityPm.PaymentCurrencyId;
                    journalLine.ForeignAmount = (decimal)theEntityPm.AmountInPaymentCurrency - ((decimal)theEntityPm.TaxDeductionLocalAmount/(decimal)theEntityPm.PaymentCurrencyExchangeRate);
                    journalLine.ExchangeRate = (decimal)theEntityPm.PaymentCurrencyExchangeRate;
                    journalLine.Reference1 = theEntityPm.PaymentNo;
                    journalLine.Reference2 = theEntityPm.ChequeOrPaymentRef;
                    journalLine.Notes = theEntityPm.InternalNotes;
                    journalLine.CreditAccountId = GetCreditAccoutId(theEntityPm);
                    journalLine.ChangeSetOp = ChangeSetOperation.Insert;
                    journal.JournalLines.Add(journalLine);
                    // [Debit ]
                    GLAccountPM glAccount = getDebitGLAccount(theEntityPm.VendorId, theEntityPm.Tenant);
                    journalLine = new JournalLinePM();
                    journalLine.Tenant = tenant;
                    journalLine.JournalId = journal.Id;
                    journalLine.Line = 1;
                    journalLine.ActionCode = "2";
                    journalLine.ActionTypeCodeEnum = MyJournalActionTypeEnum.Debit;
                    journalLine.DocumentDate = theEntityPm.RegisterDate.Value;
                    journalLine.AccountingDate = theEntityPm.RegisterDate.Value;
                    journalLine.DueDate = theEntityPm.ValueDate.Value;
                    journalLine.LocalAmount = (decimal)theEntityPm.AmountInLocalCurrency;
                    journalLine.CurrencyId = theEntityPm.PaymentCurrencyId;
                    journalLine.ForeignAmount = (decimal)theEntityPm.AmountInPaymentCurrency;
                    journalLine.ExchangeRate = (decimal)theEntityPm.PaymentCurrencyExchangeRate;
                    journalLine.Reference1 = theEntityPm.PaymentNo;
                    journalLine.Reference2 = theEntityPm.ChequeOrPaymentRef;
                    journalLine.Notes = theEntityPm.InternalNotes;
                    journalLine.DebitAccountId = glAccount != null ? glAccount.Id : null;
                    journalLine.ChangeSetOp = ChangeSetOperation.Insert;
                    journal.JournalLines.Add(journalLine);
                    // [Credit]
                    if (theEntityPm.TaxDeductionLocalAmount == 0 && theEntityPm.VendorAddressId != tenantPOCO.AddressId)
                    {
                        // nothing
                    }
                    else
                    {
                        journalLine = new JournalLinePM();
                        journalLine.Tenant = tenant;
                        journalLine.JournalId = journal.Id;
                        journalLine.Line = 1;
                        journalLine.ActionCode = "1";
                        journalLine.ActionTypeCodeEnum = MyJournalActionTypeEnum.Credit;
                        journalLine.DocumentDate = theEntityPm.RegisterDate.Value;
                        journalLine.AccountingDate = theEntityPm.RegisterDate.Value;
                        journalLine.DueDate = theEntityPm.ValueDate.Value;
                        journalLine.LocalAmount = (decimal)theEntityPm.TaxDeductionLocalAmount;
                        journalLine.CurrencyId = theEntityPm.PaymentCurrencyId;
                        var foreignAmount = theEntityPm.TaxDeductionLocalAmount;
                        if (theEntityPm.PaymentCurrencyId != tenantPOCO.CurrencyId)
                        {
                            foreignAmount = (decimal)theEntityPm.TaxDeductionLocalAmount / (decimal)theEntityPm.PaymentCurrencyExchangeRate;
                        }
                        journalLine.ForeignAmount = (decimal)foreignAmount;
                        journalLine.ExchangeRate = (decimal)theEntityPm.PaymentCurrencyExchangeRate;
                        journalLine.Reference1 = theEntityPm.PaymentNo;
                        journalLine.Reference2 = theEntityPm.ChequeOrPaymentRef;
                        journalLine.Notes = theEntityPm.InternalNotes;
                        IFullAccountingSettingQueryServiceExt query = ContainerAccessor.Container.Resolve(typeof(IFullAccountingSettingQueryServiceExt), "FullAccountingSettingQueryServiceExt", new ParameterOverride("", 1)) as IFullAccountingSettingQueryServiceExt;
                        FullAccountingSettingPM accountingSettings = query.GetFullAccountingSettingByTenant(tenant);
                        journalLine.CreditAccountId = accountingSettings != null ? accountingSettings.TaxWithholdingGLAccountId : null;
                        journalLine.DebitAccountId = glAccount != null ? glAccount.Id : null;
                        journalLine.ChangeSetOp = ChangeSetOperation.Insert;
                        journal.JournalLines.Add(journalLine);
                    }

                    var serializer = new XmlSerializer(typeof(JournalPM));
                    var stringwriter = new System.IO.StringWriter();
                    serializer.Serialize(stringwriter, journal);
                    string xmlParameters = stringwriter.ToString();

                    IJournalUpdateServiceExt journalUpdate = ContainerAccessor.Container.Resolve(typeof(IJournalUpdateServiceExt), "JournalUpdateServiceExt", new ParameterOverride("", 1)) as IJournalUpdateServiceExt;
                    journalUpdate.Update(journal);
                }
            }
        }

        private void UpdateCashBook(APPaymentPM theEntityPm)
        {
            if (theEntityPm.PaymentMethodCode == "CA")
            {
                bool useLocal = true;
                var user = GetLoggedContact(tenant);
                if (user != null) useLocal = !(GetLoggedContact(tenant).DontShowLocal);
                ICashBookQueryServiceExt cashBookQuery = ContainerAccessor.Container.Resolve(typeof(ICashBookQueryServiceExt), "CashBookQueryServiceExt", new ParameterOverride("", 1)) as ICashBookQueryServiceExt;
                CashBookPM cashBook = cashBookQuery.GetByPaymentAndCurrencyAndBranch(theEntityPm.PaymentCurrencyId, "1", theEntityPm.BranchId, theEntityPm.Tenant);
                if (cashBook != null)
                {
                    if(cashBook.TotalAmount >= (decimal)theEntityPm.AmountInPaymentCurrency)
                    {
                        // Update Total Amount
                        ICashBookUpdateServiceExt cashBookUpdate = ContainerAccessor.Container.Resolve(typeof(ICashBookUpdateServiceExt), "CashBookUpdateServiceExt", new ParameterOverride("", 1)) as ICashBookUpdateServiceExt;
                        if (cashBook.TotalAmount == null)
                        {
                            cashBook.TotalAmount = 0;
                        }
                        cashBook.TotalAmount -= (decimal)theEntityPm.AmountInPaymentCurrency;
                        cashBook.ChangeSetOp = ChangeSetOperation.Update;
                        cashBookUpdate.Update(cashBook);
                    }
                    else
                    {
                        throw new ApplicationException(TranslateTextsClass.Translate("APPayment.M.FullAccountingCashBookCheck", tenant, useLocal));
                    }
                }
            }
        }
        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }
        private static ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }
            ContactPM loggedContact = new ContactQuery(tenant).GetContactByEmailOnly(
                AuthenticationUtil.ResolveUserIdentityName(tenant)
                , tenant);
            if (loggedContact == null)
            {
                loggedContact = new ContactQuery(tenant).GetContactByEmailOnly("system@tenant" + tenant + ".com", tenant);
            }
            loggedContact = loggedContact ?? new Logitude.BL.CommonDataModel.EntityPMs.ContactPM() { DontShowLocal = true };
            return loggedContact;
        }

        private GLAccountPM getDebitGLAccount(string vendorId, int tenant)
        {
            GLAccountPM glaAccount = null;
            CardRepository cardRep = new CardRepository(tenant);
            Card card = cardRep.GetSingleCard(vendorId, tenant);
            if (card != null)
            {
                IGLAccountQueryServiceExt glAccountQuery = ContainerAccessor.Container.Resolve(typeof(IGLAccountQueryServiceExt), "GLAccountQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountQueryServiceExt;
                glaAccount = glAccountQuery.GetSingleGLAccountPM(card.GLAccountId, tenant);
            }

            return glaAccount;
        }
        private string GetCreditAccoutId(APPaymentPM theEntityPm)
        {
            string creditAccoutId = null;
            if (theEntityPm.PaymentMethodCode == "CA")
            {
                ICashBookQueryServiceExt cashBookQuery = ContainerAccessor.Container.Resolve(typeof(ICashBookQueryServiceExt), "CashBookQueryServiceExt", new ParameterOverride("", 1)) as ICashBookQueryServiceExt;
                CashBookPM cashBook = cashBookQuery.GetByPaymentAndCurrencyAndBranch(theEntityPm.PaymentCurrencyId,"1",theEntityPm.BranchId, theEntityPm.Tenant);
                creditAccoutId = cashBook != null ? cashBook.AccountId: null; 
            }
            else if (theEntityPm.PaymentMethodCode == "CH")
            {
                IBankAccountQueryServiceExt bankAccountQuery = ContainerAccessor.Container.Resolve(typeof(IBankAccountQueryServiceExt), "BankAccountQueryServiceExt", new ParameterOverride("", 1)) as IBankAccountQueryServiceExt;
                BankAccountPM bankAccount = bankAccountQuery.GetByFirstOrDefault(theEntityPm.BankAccountId, theEntityPm.Tenant);
                creditAccoutId = bankAccount != null ? bankAccount.TransferGLAcccountId : null;
            }
            else
            {
                IBankAccountQueryServiceExt bankAccountQuery = ContainerAccessor.Container.Resolve(typeof(IBankAccountQueryServiceExt), "BankAccountQueryServiceExt", new ParameterOverride("", 1)) as IBankAccountQueryServiceExt;
                BankAccountPM bankAccount = bankAccountQuery.GetByFirstOrDefault(theEntityPm.BankAccountId, theEntityPm.Tenant);
                creditAccoutId = bankAccount != null ? bankAccount.GLAccountId : null;
            }
            return creditAccoutId;
        }
        #endregion
    }
}