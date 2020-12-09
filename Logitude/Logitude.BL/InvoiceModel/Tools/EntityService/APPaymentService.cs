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
using Logitude.Server.Tools.Utils;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using Microsoft.Practices.Unity;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Data.Helpers;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using System.Xml.Serialization;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.BL.InvoiceModel.EntityOtherServices;

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
        private bool isTransferToDropbox;
        private bool TransferToDropboxActivated;
        bool setApproved;
        private bool transferToFTPActivated;
        private bool canTransferToFTP;
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
            this.GetAccountingSystem();
        }

        private bool isTransferEnabled = false;
        private void GetAccountingSystem()
        {
            AccountingSettingRepository accountingSettingRepository = new AccountingSettingRepository(myCommonContext);
            AccountingSystemRepository accountingSystemRepository = new AccountingSystemRepository(myCommonContext);
            AccountingSetting accountingSetting = accountingSettingRepository.GetSingleAccountSetting(tenant);
            if (accountingSetting != null)
            {
                this.TransferToDropboxActivated = accountingSetting.TransferToDropboxActivated;
                this.transferToFTPActivated = accountingSetting.TransferToFTPActivated;

                AccountingSystem accountingSystem = accountingSystemRepository.GetSingleAccountingSystem(accountingSetting.AccountingSystemCode);
                if (accountingSystem != null)
                {
                    this.isTransferToDropbox = accountingSystem.CanTransferToDropbox;
                    this.canTransferToFTP = accountingSystem.CanTransferToFTP;

                    if (accountingSetting.IsAPPaymentsTransferEnabled && accountingSystem.AllowAPPaymentsTransfer)
                    {
                        isTransferEnabled = true;
                    }
                }
            }
        }
       
        public void Create(APPaymentPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.changedList = theEntityPm.PaymentInvoices;
            this.payment = new APPayment();
            this.InitializeComponent();

            APPaymentValidator.Validate(this.entityPM, payment, isNewEntity);
            APPaymentTracing.Trace(theEntityPm, payment, isNewEntity);

            foreach (APPaymentInvoicePM item in theEntityPm.PaymentInvoices)
            {
                item.ChangeSetOp = ChangeSetOperation.Insert;
                this.CreatePaymentInvoice(item);
            }
            this.InitializeTransferComponents();
            setApproved = theEntityPm.SetApproved;
            var setVoided = theEntityPm.SetVoided;
            var setCancelApproved = theEntityPm.SetCancelApproval;
            APPaymentMapping.MapEntity(theEntityPm, payment, isNewEntity);
            paymentRepository.Add(payment);
            paymentRepository.SubmitChanges();
            invoicePaymentRepository.SubmitChanges();
            paymentCheque= this.CreateFullAccountingPaymentCheque(theEntityPm);
            if(paymentCheque != null)
            {
                payment.ChequeOrPaymentRef = paymentCheque.ChequeNumber;
                theEntityPm.ChequeOrPaymentRef = payment.ChequeOrPaymentRef;
               
            }
            this.UpdatePaymentOpenAmount();
            APPaymentHelper service = new APPaymentHelper();
            service.APPaymentQuickbooksValidating(theEntityPm, setApproved, false, payment, this.objectContext, this.myCommonContext, setCancelApproved);

            this.BuildSearchFields();

            // DropBox
            this.CreateAPPaymentMessage(setApproved);

            //Full Accounting 
            AddAPPaymentJournalAndJournalLines(theEntityPm, setApproved);
            this.VoidAPPaymentInFullAccounting(theEntityPm, setVoided);

            paymentRepository.Update(payment);
            paymentRepository.SubmitChanges();
            this.TraceConnected();
            this.GetForeignFields();
        }

        private void CreateAPPaymentMessage(bool setApproved)
        {
            if (setApproved && isTransferEnabled)
            {
                if ((this.isTransferToDropbox && this.TransferToDropboxActivated) || (this.canTransferToFTP && this.transferToFTPActivated))
                {
                    if (!string.IsNullOrEmpty(entityPM.TransferError))
                    {
                        throw new ApplicationException(entityPM.TransferError);
                    }
                    else
                    {
                        bool isDropBox = this.isTransferToDropbox && this.TransferToDropboxActivated;
                        bool isFTP = this.canTransferToFTP && this.transferToFTPActivated;

                        this.payment = paymentRepository.GetSingleAPPayment(this.entityPM.Id);
                        List<APPayment> entities = new List<APPayment>();
                        entities.Add(this.payment);

                        APPaymentMessageHelper myHelper = new APPaymentMessageHelper(entities, this.payment.PaymentNo + ".xml", tenant, isDropBox, isFTP);
                        myHelper.Transfer();
                    }
                }
            }
        }

        public FullAccountingSettingPM GetFullAccountingSetting(APPaymentPM entityPM)
        {
            IFullAccountingSettingQueryServiceExt query = ContainerAccessor.Container.Resolve(typeof(IFullAccountingSettingQueryServiceExt), "FullAccountingSettingQueryServiceExt", new ParameterOverride("", 1)) as IFullAccountingSettingQueryServiceExt;
           return query.GetFullAccountingSettingByTenant(tenant);

        }
        PaymentChequePM paymentCheque;

        GLAccountPM VendorGLAccount;
        private PaymentChequePM CreateFullAccountingPaymentCheque(APPaymentPM entityPM )
        {
            FullAccountingSettingPM accountingSettings = GetFullAccountingSettings(tenant);
            if (accountingSettings.AccountingActivated && accountingSettings.IsPaymentChequesActivated) {
                List<PaymentChequePM> PaymentCheques = GetPaymentChequesByAPPaymentId(entityPM);
                if (PaymentCheques.Count == 0)
                {
                    if (setApproved && (entityPM.AutomaticPaymentCheque || string.IsNullOrEmpty(entityPM.ChequeOrPaymentRef)) && entityPM.PaymentMethodCode=="CH")
                    {
                        SecurityUtility.CheckContactFeature("PaymentCheque", "NEW", entityPM.Tenant);
                         VendorGLAccount = GetGLAccountByCard(entityPM);
                        paymentCheque= CreatePaymentCheque(entityPM);
                        SubmitPaymentCheque(paymentCheque); 
                    }
                }
            }

            return paymentCheque;
        }
        private List<PaymentChequePM> GetPaymentChequesByAPPaymentId(APPaymentPM entityPM)
        {

            IPaymentChequeQueryServiceExt query = ContainerAccessor.Container.Resolve(typeof(IPaymentChequeQueryServiceExt), "PaymentChequeQueryServiceExt", new ParameterOverride("", 1)) as IPaymentChequeQueryServiceExt;
            return query.GetPaymentChequesByPaymentId(entityPM.Id, tenant);


        }
        public string GetTransferAccountIdByBankAccountId(APPaymentPM paymentPM)
        {
            IBankAccountQueryServiceExt bankAccountQuery = ContainerAccessor.Container.Resolve(typeof(IBankAccountQueryServiceExt), "BankAccountQueryServiceExt", new ParameterOverride("", 1)) as IBankAccountQueryServiceExt;
            BankAccountPM bankAccount = bankAccountQuery.GetByFirstOrDefault(paymentPM.BankAccountId, paymentPM.Tenant);
            if(bankAccount != null)
            {
                return bankAccount.TransferGLAcccountId;
            }
            else
            {
                return null;
            }
        }
        private void SubmitPaymentCheque(PaymentChequePM paymentCheque)
        {

            IPaymentChequeUpdateServiceExt paymentChequeUpdateService = ContainerAccessor.Container.Resolve(typeof(IPaymentChequeUpdateServiceExt), "PaymentChequeUpdateServiceExt", new ParameterOverride("", 1)) as IPaymentChequeUpdateServiceExt;
            paymentChequeUpdateService.Update(paymentCheque);

        }


        private PaymentChequePM CreatePaymentCheque(APPaymentPM entityPM)
        {
            PaymentChequePM paymentCheque = MapPaymentChequePM(entityPM);
            PaymentChequeLinePM paymentChequeLine = MapPaymentChequeLinePM(entityPM);
            paymentCheque.PaymentChequeLines.Add(paymentChequeLine);
            return paymentCheque;
        } 

        private PaymentChequePM MapPaymentChequePM(APPaymentPM payment)
        {
            PaymentChequePM paymentCheque = new PaymentChequePM();
            paymentCheque.CreateDate = DateTime.Today;
            paymentCheque.PayToGLAccountId = VendorGLAccount != null ? VendorGLAccount.Id : null;
            if (payment.PaymentChequeCreationPayToName == null)
            {
                paymentCheque.PayToName = VendorGLAccount != null ? (VendorGLAccount.NameForPrintingCheques != null? VendorGLAccount.NameForPrintingCheques:  ( VendorGLAccount.LocalName != null ? VendorGLAccount.LocalName : VendorGLAccount.EnglishName)) : null;
            }
            else { paymentCheque.PayToName = payment.PaymentChequeCreationPayToName; }
            paymentCheque.BankAccountId = payment.BankAccountId;
            paymentCheque.BankAccountGLAccountId = GetTransferAccountIdByBankAccountId(payment);
            paymentCheque.LocalAmount = (decimal?)payment.AmountInLocalCurrency - payment.TaxDeductionLocalAmount;
            paymentCheque.CurrencyId = payment.PaymentCurrencyId;
            paymentCheque.ExchangeRate = (decimal?)payment.PaymentCurrencyExchangeRate;
            paymentCheque.ForeignAmount = (decimal?)payment.AmountInPaymentCurrency;
            paymentCheque.ValueDate = payment.ValueDate;
            paymentCheque.ApprovedByUserId = payment.ApprovedByUserId;
            paymentCheque.ApproveDate = payment.ApprovedDateTime;
            paymentCheque.APPaymentId = payment.Id;
            paymentCheque.PaymentChequeStatusCode = "2";
            paymentCheque.ChangeSetOp = ChangeSetOperation.Insert;
            paymentCheque.Tenant = payment.Tenant;
            return paymentCheque;
        }

        private PaymentChequeLinePM MapPaymentChequeLinePM(APPaymentPM payment)
        {
            PaymentChequeLinePM paymentChequeLine = new PaymentChequeLinePM();
            if (payment.PaymentChequeCreationNotes == null || payment.PaymentChequeCreationNotes == "undefined")
            {
                paymentChequeLine.Notes = payment.PaymentNo;
            }
            else { paymentChequeLine.Notes = payment.PaymentChequeCreationNotes; }
            paymentChequeLine.Amount = (decimal?)payment.AmountInLocalCurrency - payment.TaxDeductionLocalAmount;
            paymentChequeLine.ChangeSetOp = ChangeSetOperation.Insert;
            paymentChequeLine.Line = 1;
            paymentChequeLine.Tenant = payment.Tenant;
            return paymentChequeLine;
        }

        public GLAccountPM GetGLAccountByCard(APPaymentPM paymentPM)
        {
            CardPM card = GetCardByVendorId(paymentPM);

           
            IGLAccountQueryServiceExt glAccountQuery = ContainerAccessor.Container.Resolve(typeof(IGLAccountQueryServiceExt), "GLAccountQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountQueryServiceExt;
           return glAccountQuery.GetSingleGLAccountPM(card.GLAccountId, paymentPM.Tenant);
           
        }

        public CardPM GetCardByVendorId(APPaymentPM paymentPM)
        {
            CardQuery cardQuery = new CardQuery(paymentPM.Tenant);
            return cardQuery.GetSinglePM(paymentPM.VendorId, paymentPM.Tenant);

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
                    List<PaymentChequePM> PaymentCheques = GetPaymentChequesByAPPaymentId(theEntityPm);
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
        private void CompleteCancelAPPaymentInFullAccounting(APPaymentPM aPPaymentPM )
        {
            
            JournalPM journalPM = GetJournalByAccountingEntityId(aPPaymentPM);
            if (journalPM != null)
            {
                VoidJournal(journalPM, aPPaymentPM);
               
            }
            
            List<PaymentChequePM> paymentCheques = GetPaymentChequesByAPPaymentId(aPPaymentPM);
            if (paymentCheques != null)
            {
                foreach (var item in paymentCheques)
                {
                    CancelPaymentCheque(item);
                    
                }
            }
        }
        private void CancelPaymentCheque(PaymentChequePM paymentCheque)
        {
            IPaymentChequeUpdateServiceExt paymentChequeUpdate = ContainerAccessor.Container.Resolve(typeof(IPaymentChequeUpdateServiceExt), "PaymentChequeUpdateServiceExt", new ParameterOverride("", 1)) as IPaymentChequeUpdateServiceExt;
            paymentCheque.PaymentChequeStatusCode = "4";
            paymentCheque.IsCancelled = true;
            paymentCheque.ChangeSetOp = ChangeSetOperation.Update;
            paymentCheque.CancelledByAPPayment = true;
            paymentChequeUpdate.Update(paymentCheque);
        }
        private void VoidJournal(JournalPM journalPM, APPaymentPM aPPaymentPM)
        {
            journalPM.APPaymentCancelDate = aPPaymentPM.AccountingCancelationDate;
            var journalUpdate = ContainerAccessor.Container.Resolve(typeof(IJournalVoidUpdateServiceExt), "JournalVoidUpdateServiceExt", new ParameterOverride("", 1)) as IJournalVoidUpdateServiceExt;
            journalUpdate.Update(journalPM, new StornoOverrideM()
            {
                AccountingEntityCode = "5",
                AccountingEntityId = aPPaymentPM.Id,
                AccountingEntityReference = aPPaymentPM.PaymentNo,
                AccountingDate = aPPaymentPM.AccountingCancelationDate,
                LineNotes = aPPaymentPM.CancelationNotes,
            });
           JournalPM voidedByJournal = GetApprovedJournalByAccountingEntityId(aPPaymentPM);

            entityPM.VoidedByJournalNumber = voidedByJournal != null?  voidedByJournal.JournalNumber: null;
        }

        private JournalPM GetJournalByAccountingEntityId(APPaymentPM aPPaymentPM)
        {
            IJournalQueryServiceExt journalQuery = ContainerAccessor.Container.Resolve(typeof(IJournalQueryServiceExt), "JournalQueryServiceExt", new ParameterOverride("", 1)) as IJournalQueryServiceExt;
            return journalQuery.GetJournalByAccountingEntityIdAndCode(aPPaymentPM.Id, "5" , aPPaymentPM.Tenant);


        }
        private JournalPM GetApprovedJournalByAccountingEntityId(APPaymentPM aPPaymentPM)
        {
            IJournalQueryServiceExt journalQuery = ContainerAccessor.Container.Resolve(typeof(IJournalQueryServiceExt), "JournalQueryServiceExt", new ParameterOverride("", 1)) as IJournalQueryServiceExt;
            return journalQuery.GetApprovedJournalByAccountingEntityId(aPPaymentPM.Id, "5", aPPaymentPM.Tenant);


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

            APPaymentValidator.Validate(entityPM, payment, isNewEntity);
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
             setApproved = theEntityPm.SetApproved;
            var setVoided = theEntityPm.SetVoided;
            var setCancelApproved = theEntityPm.SetCancelApproval;
            var SetReSendQBO = theEntityPm.SetReSendQBO;
            CreateFullAccountingPaymentCheque(theEntityPm);
            if (paymentCheque != null)
            {
               
                theEntityPm.ChequeOrPaymentRef = paymentCheque.ChequeNumber;

            }
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

            // DropBox
            this.CreateAPPaymentMessage(setApproved);

            //Full Accounting 
            AddAPPaymentJournalAndJournalLines(theEntityPm, setApproved);
            VoidAPPaymentInFullAccounting(theEntityPm, setVoided);
            theEntityPm.VoidedByJournalNumber = entityPM.VoidedByJournalNumber;
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
                CardRepository cardRep = new CardRepository(entityPM.Tenant);
                CurrencyRepository currencyRep = new CurrencyRepository(entityPM.Tenant);
                Card card = cardRep.GetSingleCard(entityPM.VendorId, entityPM.Tenant);
                Currency currency = currencyRep.GetSingleCurrency(entityPM.PaymentCurrencyId, entityPM.Tenant);
                string currencyError = "Currency External Id is missing";
                if (currency != null && !string.IsNullOrEmpty(currency.Code))
                {
                    currencyError = "Currency: " + currency.Code + ". External Id is missing";
                }

                if (card != null)
                {
                    AccountingSystemHelper accountingSystemHelper = new AccountingSystemHelper();
                    var payablesAccountingCard = accountingSystemHelper.GetGenericCreditAccount(card.Id, entityPM.PaymentCurrencyId, tenant, true);
                    if (FieldIsEmpty(payablesAccountingCard))
                    {
                        isReady = false;
                        myError = "Bill To: " + entityPM.VendorName + ". External Id is missing";
                    }
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
            List<APInvoicePayment> allConnectedItems = invoicePaymentRepository.GetAPInvoicePaymentByPaymentId(entityPM.Id, entityPM.Tenant).ToList();

            // To Avoid twice same invoice connected
            double? connectedAmount = 0;
            List<string> ids = new List<string>();            
            foreach (APInvoicePayment item in allConnectedItems)
            {
                if (!ids.Contains(item.APInvoiceId))
                {
                    if (item.PaymentAmount != null)
                    {
                        connectedAmount += item.PaymentAmount;
                    }

                    ids.Add(item.APInvoiceId);
                }
            }

            double? Amount = MethodHelper.Roundd(entityPM.AmountInPaymentCurrency, 2);
            double? ExternalAmount = MethodHelper.Roundd(entityPM.ExternalPaymentAmount, 2);
            double? PaidAmount = MethodHelper.Roundd(connectedAmount, 2);
            double? AllPaidAmount = MethodHelper.Roundd(PaidAmount + ExternalAmount, 2);

            if (AllPaidAmount > Amount)
            {
                throw new Exception("The amount paid is not suitable to the total payment amount!!");
            }

            else
            {
                bool isClosed = entityPM.IsClosed;
                string StatusCode = entityPM.StatusCode;
                double? OpenAmount = MethodHelper.Round((Amount - PaidAmount - ExternalAmount), 2);

                if (OpenAmount == 0)
                {
                    isClosed = true;

                    if (StatusCode == "AD")
                    {
                        StatusCode = "CL";
                    }
                }

                else if (OpenAmount == Amount)
                {
                    isClosed = false;

                    if (StatusCode != "VD" && StatusCode != "DR")
                    {
                        StatusCode = "AD";
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

                payment.IsClosed = entityPM.IsClosed = isClosed;
                payment.StatusCode = entityPM.StatusCode = StatusCode;
                payment.OpenAmount = entityPM.OpenAmount = OpenAmount;
                payment.AmountInPaymentCurrency = entityPM.AmountInPaymentCurrency = Amount;
            }

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
        public void UpdatePaymentOpenAmount_Old()
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
                                // it is allowed to have invoice with 0 amount and 0 amount due
                                if (allConnectedItems.Count > 0)
                                {
                                    invoice.IsClosed = true;
                                    invoice.StatusCode = "PD";
                                }
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
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.PrintNotes);

            #region Card
            if (!string.IsNullOrEmpty(entityPM.VendorId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.VendorId, tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.LocalName);
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
        private void AddAPPaymentJournalAndJournalLines(APPaymentPM paymentPM, bool setApproved)
        {
            int tenant = paymentPM.Tenant;
            if (setApproved )
            {

                TenantRepository tenantRepository = new TenantRepository(tenant);
                Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);
                if (tenantPOCO.AccountingActivated)
                {
                    UpdateCashBook(paymentPM);
                    CreateAPPaymentJournal(paymentPM, tenantPOCO);
                }
            }
        }

        private void CreateAPPaymentJournal(APPaymentPM paymentPM, Tenant tenantPOCO)
        {
            // Refactored by Abdullah

            JournalPM journal = CreateAPPaymentJournal(paymentPM);

            AddCreditJournalLineForBankGLAccount(paymentPM, journal);

            AddDebitJournalLineForVendorGLAccount(paymentPM, journal);

            if (paymentPM.TaxDeductionLocalAmount != 0 || paymentPM.VendorAddressId == tenantPOCO.AddressId)
                AddCreditJournalLineForTaxGLAccount(paymentPM, paymentPM.Tenant, tenantPOCO, journal);

            SubmitJournal(journal);
        }

        private static void SubmitJournal(JournalPM journal)
        {
            IJournalUpdateServiceExt journalUpdate = ContainerAccessor.Container.Resolve(typeof(IJournalUpdateServiceExt), "JournalUpdateServiceExt", new ParameterOverride("", 1)) as IJournalUpdateServiceExt;
            journalUpdate.Update(journal);
        }

        private void AddCreditJournalLineForTaxGLAccount(APPaymentPM paymentPM, int tenant, Tenant tenantPOCO, JournalPM journal)
        {
            GLAccountPM glAccount = getDebitGLAccount(paymentPM.VendorId, paymentPM.Tenant);
            FullAccountingSettingPM accountingSettings = GetFullAccountingSettings(tenant);

            JournalLinePM creditForTaxGLAccount = new JournalLinePM
            {
                Tenant = tenant,
                JournalId = journal.Id,
                Line = 1,
                ActionCode = "1",
                ActionTypeCodeEnum = MyJournalActionTypeEnum.Credit,
                DocumentDate = paymentPM.ValueDate.Value,
                AccountingDate = paymentPM.RegisterDate.Value,
                DueDate = paymentPM.ValueDate.Value,
                LocalAmount = (decimal)paymentPM.TaxDeductionLocalAmount,
                CurrencyId = paymentPM.PaymentCurrencyId,
                ExchangeRate = (decimal)paymentPM.PaymentCurrencyExchangeRate,
                Reference1 = paymentPM.PaymentNo,
                Reference2 = paymentPM.ChequeOrPaymentRef,
                Notes = paymentPM.PrintNotes,
                CreditAccountId = accountingSettings?.TaxWithholdingGLAccountId,
                DebitAccountId = glAccount?.Id,
                ChangeSetOp = ChangeSetOperation.Insert,
            };

            //ForeignAmount
            if (paymentPM.PaymentCurrencyId != tenantPOCO.CurrencyId)
                creditForTaxGLAccount.ForeignAmount = (decimal)paymentPM.TaxDeductionLocalAmount / (decimal)paymentPM.PaymentCurrencyExchangeRate;
            else
                creditForTaxGLAccount.ForeignAmount = paymentPM.TaxDeductionLocalAmount.Value;


            journal.JournalLines.Add(creditForTaxGLAccount);
        }

        private static FullAccountingSettingPM GetFullAccountingSettings(int tenant)
        {
            IFullAccountingSettingQueryServiceExt query = ContainerAccessor.Container.Resolve(typeof(IFullAccountingSettingQueryServiceExt), "FullAccountingSettingQueryServiceExt", new ParameterOverride("", 1)) as IFullAccountingSettingQueryServiceExt;
            FullAccountingSettingPM accountingSettings = query.GetFullAccountingSettingByTenant(tenant);
            return accountingSettings;
        }

        private void AddDebitJournalLineForVendorGLAccount(APPaymentPM paymentPM, JournalPM journal)
        {
            GLAccountPM glAccount = getDebitGLAccount(paymentPM.VendorId, paymentPM.Tenant);
            JournalLinePM debitForVendorGLAccount = new JournalLinePM
            {
                Tenant = tenant,
                JournalId = journal.Id,
                Line = 1,
                ActionCode = "2",
                ActionTypeCodeEnum = MyJournalActionTypeEnum.Debit,
                DocumentDate = paymentPM.ValueDate.Value,
                AccountingDate = paymentPM.RegisterDate.Value,
                DueDate = paymentPM.ValueDate.Value,
                LocalAmount = (decimal)paymentPM.AmountInLocalCurrency,
                CurrencyId = paymentPM.PaymentCurrencyId,
                ForeignAmount = (decimal)paymentPM.AmountInPaymentCurrency,
                ExchangeRate = (decimal)paymentPM.PaymentCurrencyExchangeRate,
                Reference1 = paymentPM.PaymentNo,
                Reference2 = paymentPM.ChequeOrPaymentRef,
                Notes = paymentPM.PrintNotes,
                DebitAccountId = glAccount != null ? glAccount.Id : null,
                ChangeSetOp = ChangeSetOperation.Insert
            };
            journal.JournalLines.Add(debitForVendorGLAccount);
        }

        private void AddCreditJournalLineForBankGLAccount(APPaymentPM paymentPM, JournalPM journal)
        {
            JournalLinePM creditForTaxGLAccount = new JournalLinePM
            {
                Tenant = tenant,
                JournalId = journal.Id,
                Line = 1,
                ActionCode = "1",
                ActionTypeCodeEnum = MyJournalActionTypeEnum.Credit,
                DocumentDate = paymentPM.ValueDate.Value,
                AccountingDate = paymentPM.RegisterDate.Value,
                DueDate = paymentPM.ValueDate.Value,
                LocalAmount = (decimal)paymentPM.AmountInLocalCurrency - (decimal)paymentPM.TaxDeductionLocalAmount,
                CurrencyId = paymentPM.PaymentCurrencyId,
                ForeignAmount = (decimal)paymentPM.AmountInPaymentCurrency - ((decimal)paymentPM.TaxDeductionLocalAmount / (decimal)paymentPM.PaymentCurrencyExchangeRate),
                ExchangeRate = (decimal)paymentPM.PaymentCurrencyExchangeRate,
                Reference1 = paymentPM.PaymentNo,
                Reference2 = paymentPM.ChequeOrPaymentRef,
                Notes = paymentPM.PrintNotes,
                CreditAccountId = GetCreditAccoutId(paymentPM),
                ChangeSetOp = ChangeSetOperation.Insert
            };

            journal.JournalLines.Add(creditForTaxGLAccount);

        }

        private static JournalPM CreateAPPaymentJournal(APPaymentPM paymentPM)
        {
            JournalPM journal = new JournalPM();
            journal.Tenant = paymentPM.Tenant;
            journal.JournalNumber = "1";
            journal.CreateDate = TenantServerConfigration.GetCurrentDateTime(paymentPM.Tenant);
            journal.AccountingDate = paymentPM.RegisterDate.Value;
            journal.TypeCode = "0";
            journal.StatusCode = "2";
            journal.CreatedByUserId = paymentPM.CreatedByUserId;
            journal.AccountingEntityCode = "5";
            journal.AccountingEntityId = paymentPM.Id;
            journal.AccountingEntityReference = paymentPM.PaymentNo;
            journal.UpdateDate = TenantServerConfigration.GetCurrentDateTime(paymentPM.Tenant);
            journal.UpdatedByUserId = paymentPM.UpdatedByUserId;
            journal.ApproveDate = TenantServerConfigration.GetCurrentDateTime(paymentPM.Tenant);
            journal.ApprovedByUserId = paymentPM.ApprovedByUserId;
            journal.ChangeSetOp = ChangeSetOperation.Insert;
            return journal;
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
                    if(cashBook.TotalAmount >= ((decimal)theEntityPm.AmountInPaymentCurrency - ((decimal)theEntityPm.TaxDeductionLocalAmount / (decimal)theEntityPm.PaymentCurrencyExchangeRate)))
                    {
                        // Update Total Amount
                        ICashBookUpdateServiceExt cashBookUpdate = ContainerAccessor.Container.Resolve(typeof(ICashBookUpdateServiceExt), "CashBookUpdateServiceExt", new ParameterOverride("", 1)) as ICashBookUpdateServiceExt;
                        if (cashBook.TotalAmount == null)
                        {
                            cashBook.TotalAmount = 0;
                        }
                        cashBook.TotalAmount -= ((decimal)theEntityPm.AmountInPaymentCurrency - ((decimal)theEntityPm.TaxDeductionLocalAmount / (decimal)theEntityPm.PaymentCurrencyExchangeRate));
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
                if(cashBook != null)
                {
                    creditAccoutId = cashBook?.AccountId;
                }
                else
                {
                    throw new ApplicationException("No cashbook connect to the account");
                }
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