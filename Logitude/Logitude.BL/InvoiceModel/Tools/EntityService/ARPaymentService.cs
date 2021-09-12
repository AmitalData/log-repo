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
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Microsoft.Practices.Unity;
using Simplog.Data.Helpers;
using Logitude.BL.InvoiceModel.EntityQueries;
using Simplog.Data.CommonDataModel;
using Logitude.BL.InvoiceModel.EntityOtherServices;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Def.BLExt;
using Logitude.BL.Resolvers;
using Logitude.BL.InvoiceModel.CoreBL;

namespace Logitude.BL.InvoiceModel.Tools.EntityService
{
    public class ARPaymentService
    {
        const string PartnerTypeId_Customer = "CS";
       const string ChequeARPaymentAccountingMethod= "CH";
        private int tenant;
        public ARPayment paymentPoco { get; set; }
        private ARPaymentPM paymentPM;
        private IInvoiceContext objectContext;
        private ICommonDataContext myCommonContext;
        private ARInvoiceRepository invoiceRepository;
        private ARPaymentRepository paymentRepository;
        private ARInvoicePaymentRepository invoicePaymentRepository;
        private AccountingSettingRepository accountingSettingRepository;
        private AccountingSystemRepository accountingSystemRepository;
        private bool isNewEntity;
        private bool isVoidingInvoice;
        private ContactPM loggedContact;
        private List<ARPaymentInvoicePM> changedList;
        private TenantRepository tenantRepository;
        private Tenant tenantPOCO; 
        private ICashBookQueryServiceExt cashBookQuery;
        private CashBookPM PaymentCashbook;
        private string cashBookMethodType = "";
        SATInterfaceHelper sATInterfaceHelper;
        private bool SetVoided = false;
        public ARPaymentService(IInvoiceContext objectContext, int tenant)
        {
            this.sATInterfaceHelper = new SATInterfaceHelper();

            this.tenant = tenant;
            this.objectContext = objectContext;
            this.myCommonContext = CommonDataContext.GetContext(tenant);
            this.invoiceRepository = new ARInvoiceRepository(this.objectContext);
            this.paymentRepository = new ARPaymentRepository(this.objectContext);
            this.invoicePaymentRepository = new ARInvoicePaymentRepository(this.objectContext);
            this.accountingSettingRepository = new AccountingSettingRepository(myCommonContext);
            this.accountingSystemRepository = new AccountingSystemRepository(myCommonContext);
            this.changedList = new List<ARPaymentInvoicePM>();

            loggedContact = GetLoggedContactPM(tenant);

            this.GetAccountingSystem();
        }

        private bool isTransferToDropbox;
        private bool TransferToDropboxActivated;
        private bool transferToFTPActivated;
        private bool canTransferToFTP;
        private AccountingSetting accountingSetting;
        private bool isTransferEnabled = false;
        private void GetAccountingSystem()
        {
            this.accountingSetting = accountingSettingRepository.GetSingleAccountSetting(tenant);
            if (accountingSetting != null)
            {
                this.TransferToDropboxActivated = accountingSetting.TransferToDropboxActivated;
                this.transferToFTPActivated = accountingSetting.TransferToFTPActivated;

                AccountingSystem accountingSystem = accountingSystemRepository.GetSingleAccountingSystem(accountingSetting.AccountingSystemCode);
                if (accountingSystem != null)
                {
                    this.isTransferToDropbox = accountingSystem.CanTransferToDropbox;
                    this.canTransferToFTP = accountingSystem.CanTransferToFTP;

                    if (accountingSetting.IsARPaymentsTransferEnabled && accountingSystem.AllowARPaymentsTransfer)
                    {
                        isTransferEnabled = true;
                    }
                }
            }
        }

        /*
         *  CREATE
         */
        public void Create(ARPaymentPM _arpaymentPM)
        {
            _arpaymentPM.IsFullAccounting = IsFullAccActivated();

            isNewEntity = true;
            paymentPM = _arpaymentPM;
            isVoidingInvoice = paymentPM.SetVoided;
            changedList = _arpaymentPM.PaymentInvoices;

            paymentPoco = new ARPayment();

            InitializeComponent();

            ARPaymentValidator.Validate(paymentPM, paymentPoco, isNewEntity, objectContext, PaymentCashbook);
            ARPaymentTracing.Trace(_arpaymentPM, paymentPoco, isNewEntity);

            InitializeTransferComponents();

            //will reset in mapentity()
            var setApproved = _arpaymentPM.SetApproved;
            var setCancelApproved = _arpaymentPM.SetCancelApproval;
            var setVoided = _arpaymentPM.SetVoided;

            ARPaymentMapping.MapEntity(_arpaymentPM, paymentPoco, isNewEntity);

            paymentRepository.Add(paymentPoco);
            paymentRepository.SubmitChanges();
            SubmitPaymentInvoices(_arpaymentPM);

            UpdatePaymentOpenAmount();

            ARPaymentHelper service = new ARPaymentHelper();
            service.ARPaymentQuickbooksValidating(_arpaymentPM, setApproved, false, paymentPoco, objectContext, myCommonContext, isVoidingInvoice, setCancelApproved, _arpaymentPM.SetReSendQBO, false);

            BuildSearchFields();

            AccountingPaymentMethod ARPaymentMethod = GetARPaymentMethod();

            if (ARPaymentMethod != null && ARPaymentMethod.Code == "CH" && IsInternalAccountingSystem(_arpaymentPM.Tenant))//"CH" == Cheque
            {
                CreateChequeInCashBook(_arpaymentPM);
            }


            TraceConnected();

            FillFullAccountingPaymentInvoices(_arpaymentPM);

            ValidateFullAccounting(_arpaymentPM); // depends on Payment.PaymentInvoices


            if (IsAccountingActivated && setApproved || (IsAccountingActivated && _arpaymentPM.IsExternalEntity))
            {
                FullAccountingARPaymentApproveService approveService = new FullAccountingARPaymentApproveService(_arpaymentPM, tenant, isNewEntity);
                approveService.ApproveARPayment();
            }


            paymentPoco.ValueDate = _arpaymentPM.ValueDate;
            paymentRepository.Update(paymentPoco);
            paymentRepository.SubmitChanges();
            if (_arpaymentPM.IsFullAccounting)
            {
                CreateInterestTransactionLine(_arpaymentPM);

            }

            GetPaymentForeignFields();

            BuildEntitiesNumbers();

            VoidARPaymentInFullAccounting(_arpaymentPM, setVoided);

            // DropBox
            CreateARPaymentMessage(setApproved);

            //// Full Accounting => Reconciliation
            //if (theEntityPm.IsFullAccounting == true)
            //{
            //    //check glaccountid
            //    if (string.IsNullOrEmpty(theEntityPm.GLAccountId))
            //    {
            //        throw new ApplicationException("Hey! no glaccount provided!!"); // this case shouldn't be correct because glaccountid should be filled in the client, otherwise check client
            //    }

            //    //CreateReconciliationService _recoSvc = new CreateReconciliationService();
            //    CreateReconciliationForARPayment(theEntityPm);

            //    UpdateTransactions(theEntityPm);
            //}

        }

        private void FillFullAccountingPaymentInvoices(ARPaymentPM _arpaymentPM)
        {
            GLAccountPM gla = FillGLAccountFields(_arpaymentPM);

            // Full Accounting => Reconciliation
            if (_arpaymentPM.IsFullAccounting == true)
            {
                if (string.IsNullOrEmpty(_arpaymentPM.GLAccountId))
                    throw new ApplicationException("Hey! no glaccount provided!!");

                FillPaymentInvoicesFromInvoicesTransactions(_arpaymentPM, (bool)gla.IsMultiCurrency);
            }
        }
        private void CheckFullAccountingNumericFields(ARPaymentPM payment)
        {
            bool isFullAccounting = IsFullAccActivated();
            if (isFullAccounting && (payment.AccountingPaymentMethodCode == "CH" || payment.AccountingPaymentMethodCode == "BT"))
            {
                if (payment.Bank != null)
                {
                    ReturnExceptionForNumericFields(payment.Bank, "ARPayment.F.Bank", payment.Tenant);

                }
                if (payment.BankBranch != null)
                {
                    ReturnExceptionForNumericFields(payment.BankBranch, "ARPayment.F.BankBranch", payment.Tenant);

                }
                if (payment.Account != null)
                {
                    ReturnExceptionForNumericFields(payment.Account, "ARPayment.F.Account", payment.Tenant);
                }
            }
        }
        private void ReturnExceptionForNumericFields(string field, string fieldTextCode, int tenant)
        {
            int n;
            var isNumeric = int.TryParse(field, out n);
            if (!isNumeric)
            {

                ReturnException(fieldTextCode, tenant);
            }

        }
        private void ReturnException(string fieldTextCode, int tenant)
        {
            bool useLocal = true;
            var user = GetLoggedContact(tenant);
            if (user != null) useLocal = !(GetLoggedContact(tenant).DontShowLocal);

            string[] translatedText = TextCodesTranslator.TranslateText("Accounting.General.O.FieldMustBeNumeric", tenant, useLocal).Split(',');
            throw new Exception(translatedText[0] + TextCodesTranslator.TranslateText(fieldTextCode, tenant, useLocal) + translatedText[1]);

        }

        private AccountingPaymentMethod GetARPaymentMethod()
        {
            AccountingPaymentMethodRepository ARPaymentMethodRepository = new AccountingPaymentMethodRepository(objectContext);
            AccountingPaymentMethod ARPaymentMethod = ARPaymentMethodRepository.GetSingleAccountingPaymentMethod(paymentPoco.AccountingPaymentMethodId, tenant);
            return ARPaymentMethod;
        }

        private void SubmitPaymentInvoices(ARPaymentPM _arpaymentPM)
        {
            foreach (ARPaymentInvoicePM item in _arpaymentPM.PaymentInvoices)
            {
                item.ChangeSetOp = ChangeSetOperation.Insert;
                CreatePaymentInvoice(item);
            }

            invoicePaymentRepository.SubmitChanges();

        }

        private bool IsFullAccActivated()
        {
            bool isFullAccountingActivated = false;
            // Full Accounting
            TenantPM tenantPM = TenantQuery.GetSingleTenantPM(tenant, false);
            if (tenantPM != null && tenantPM.AccountingActivated == true)
                isFullAccountingActivated = true;
            return isFullAccountingActivated;
        }

        public void SetChangedList(List<ARPaymentInvoicePM> list)
        {
            this.changedList = list;
        }


        /*
         *  UPDATE
         */
        public void Update(ARPaymentPM theEntityPm, bool mapComposition = false)
        {
            paymentPM = theEntityPm;

            ContactPM loggedUser = GetLoggedContactPM(theEntityPm.Tenant);
            theEntityPm.UpdatedByUserId = loggedUser?.Id;

            ValidateFullAccounting(theEntityPm);

            //get glaccount fields
            GLAccountPM gla = FillGLAccountFields(theEntityPm);

            this.isNewEntity = false;

            this.SetVoided = theEntityPm.SetVoided;

            this.paymentPoco = paymentRepository.GetSingleARPayment(theEntityPm.Id);

            bool isErrorInTransfer = this.paymentPoco.TransferStatusCode == "ET" ? true : false;

            this.ValidateHigherStatus();

            this.InitializeComponent();

            ARPaymentValidator.Validate(paymentPM, paymentPoco, isNewEntity, objectContext, PaymentCashbook);

            ARPaymentTracing.Trace(theEntityPm, paymentPoco, isNewEntity);

            if (mapComposition)
            {
                this.changedList = theEntityPm.PaymentInvoices;
            }

            if (this.SetVoided)
            {
                foreach (ARPaymentInvoicePM item in changedList)
                {
                    this.DeletePaymentInvoice(item);
                }
            }

            else
            {
                foreach (ARPaymentInvoicePM item in changedList)
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
            }


            HandleSATTranserStatus(theEntityPm);

            var setApproved = theEntityPm.SetApproved;
            var setCancelApproved = theEntityPm.SetCancelApproval;
            var setVoided = theEntityPm.SetVoided;
            var SetReSendQBO = theEntityPm.SetReSendQBO;

            //update amounts
            if (theEntityPm.IsFullAccounting == true)
                UpdateFullAccountPaymentAmount(theEntityPm, gla.ReconcileMethodCode == "0");


            // PaymentCheque And CashBook

            if (IsAccountingActivated && setApproved || (IsAccountingActivated && theEntityPm.IsExternalEntity))
            {
                FullAccountingARPaymentApproveService approveService = new FullAccountingARPaymentApproveService(theEntityPm, tenant, isNewEntity);
                approveService.ApproveARPayment();
            }


            this.VoidARPaymentInFullAccounting(theEntityPm, setVoided);

            this.InitializeTransferComponents();

            ARPaymentMapping.MapEntity(theEntityPm, paymentPoco, isNewEntity);
            paymentRepository.Update(paymentPoco);
            paymentRepository.SubmitChanges();
            invoicePaymentRepository.SubmitChanges();

            if (!theEntityPm.IsFullAccounting)
                UpdatePaymentOpenAmount();


            ARPaymentHelper service = new ARPaymentHelper();
            if (paymentPoco.ExternalAccountingEntityId != null || SetReSendQBO)
            {
                service.ARPaymentQuickbooksValidating(theEntityPm, true, false, paymentPoco, this.objectContext, this.myCommonContext, this.SetVoided, setCancelApproved, SetReSendQBO, isErrorInTransfer);
            }
            else
            {
                service.ARPaymentQuickbooksValidating(theEntityPm, setApproved, false, paymentPoco, this.objectContext, this.myCommonContext, this.SetVoided, setCancelApproved, SetReSendQBO, isErrorInTransfer);
            }
            this.BuildSearchFields();

            // DropBox
            this.CreateARPaymentMessage(setApproved);

            theEntityPm.VoidedByJournalNumber = paymentPM.VoidedByJournalNumber;
            paymentRepository.Update(paymentPoco);
            paymentRepository.SubmitChanges();

            // Full Accounting => Reconciliation
            if (theEntityPm.IsFullAccounting == true)
            {
                if (string.IsNullOrEmpty(theEntityPm.GLAccountId))
                    throw new ApplicationException("Hey! no glaccount provided!!");

                if (theEntityPm.StatusCode != "VD" && theEntityPm.ARPaymentChequeReplicas.Count ==0)
                    CreateReconciliationForARPayment(theEntityPm);
            }

            this.TraceConnected();
            this.GetPaymentForeignFields();
            this.BuildEntitiesNumbers();
        }

        private void HandleSATTranserStatus(ARPaymentPM theEntityPm)
        {
            SATInterfaceSettingRepository sATInterfaceSettingRepository = new SATInterfaceSettingRepository(paymentPM.Tenant);
            SATInterfaceSetting satSetting = sATInterfaceSettingRepository.GetSingleSATInterfaceSetting(paymentPM.Tenant);
            if (satSetting.SATInterfaceCode == "PROF33")
            {
                if (this.paymentPM.SetCancelApproval)
                {
                    if (paymentPoco.SATTransferStatusCode == "TD")
                        this.sATInterfaceHelper.HandlePaymentSATCancellation(paymentPM, paymentPoco);
                    else if (paymentPoco.SATTransferStatusCode == "TG")
                        throw new ApplicationException("You are not allowed to cancel the payment while its status is Transferring to SAT");
                }

                if (theEntityPm.SetVoided)
                {
                    if (paymentPoco.SATTransferStatusCode == "TG")
                        throw new ApplicationException("You are not allowed to void the payment while its status is Transferring to SAT");
                    else if (paymentPoco.SATTransferStatusCode == "TE" && !string.IsNullOrEmpty(paymentPoco.SATXML))
                        throw new ApplicationException("Can't void payment because it wasn't cancelled by SAT");

                    if(paymentPoco.SATXML == null && paymentPoco.SATTransferStatusCode == "TE")
                    {
                        paymentPoco.SATTransferStatusCode = theEntityPm.SATTransferStatusCode = "ND";
                        paymentPoco.TransmissionError = theEntityPm.TransmissionError = null;
                    }

                }

                
            }
        }
 
        private JournalPM GetApprovedJournalByAccountingEntityId(ARPaymentPM aRPaymentPM)
        {
            IJournalQueryServiceExt journalQuery = ContainerAccessor.Container.Resolve(typeof(IJournalQueryServiceExt), "JournalQueryServiceExt", new ParameterOverride("", 1)) as IJournalQueryServiceExt;
            return journalQuery.GetApprovedJournalByAccountingEntityId(aRPaymentPM.Id, "3", aRPaymentPM.Tenant);
        }

        private void ValidateHigherStatus()
        {
            if (!isNewEntity)
            {
                if (this.paymentPoco.StatusCode == "AD")
                {
                    bool throwException = false;

                    if (string.IsNullOrEmpty(this.paymentPM.StatusCode) || this.paymentPM.StatusCode == "DR")
                    {
                        throwException = true;
                    }

                    else if (this.paymentPM.SetApproved)
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

        private List<ARInvoice> invoicesList;
        private ARInvoice GetInvoice(string invoiceId, int tenant)
        {
            ARInvoice myResult = null;

            if (invoicesList == null)
            {
                invoicesList = new List<ARInvoice>();
            }

            myResult = invoicesList.Where(d => d.Id == invoiceId && d.Tenant == tenant).FirstOrDefault();

            if (myResult == null)
            {
                myResult = invoiceRepository.GetSingleInvoice(invoiceId);

                if (myResult != null && !invoicesList.Contains(myResult))
                {
                    invoicesList.Add(myResult);
                }
            }

            return myResult;
        }

        private void CreateARPaymentMessage(bool setApproved)
        {
            if (setApproved && isTransferEnabled)
            {
                if ((this.isTransferToDropbox && this.TransferToDropboxActivated) || (this.canTransferToFTP && this.transferToFTPActivated))
                {
                    if (!string.IsNullOrEmpty(paymentPM.TransferError))
                    {
                        throw new ApplicationException(paymentPM.TransferError);
                    }
                    else
                    {
                        bool isDropBox = this.isTransferToDropbox && this.TransferToDropboxActivated;
                        bool isFTP = this.canTransferToFTP && this.transferToFTPActivated;

                        this.paymentPoco = paymentRepository.GetSingleARPayment(this.paymentPM.Id);
                        List<ARPayment> entities = new List<ARPayment>();
                        entities.Add(this.paymentPoco);

                        ARPaymentMessageHelper myHelper = new ARPaymentMessageHelper(entities, this.paymentPoco.PaymentNo + ".xml", tenant, isDropBox, isFTP);
                        myHelper.Transfer();
                    }
                }
            }
        }

        #region InitializeComponent
        private void InitializeComponent()
        {
            if (paymentPM.IsPaymentNumberManuallySet)
            {
                paymentPM.PaymentNo = MethodHelper.Trim(paymentPM.PaymentNo);
            }

            if (string.IsNullOrEmpty(paymentPM.Id))
            {
                paymentPM.Id = IdCounter.GetNumber("ARPayment", paymentPM.Tenant).ToString();
            }

            if (!paymentPM.IsPaymentNumberManuallySet)
            {
                if (!paymentPM.IsExternalEntity && string.IsNullOrEmpty(paymentPM.PaymentNo))
                {
                    paymentPM.PaymentNo = TableCounter.GetNumber(paymentPM.Tenant, "ARPT", "DR", null).ToString();
                }
            }

            if (paymentPM.SetApproved)
            {
                if (paymentPM.StatusCode != "AD")
                {
                    paymentPM.StatusCode = "AD";
                    paymentPM.ApprovedDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    paymentPM.ApprovedByUserId = loggedContact.Id;
                }

                UpdateDocOutNeedsRebuild();
            }

            else if (paymentPM.SetVoided)
            {
                if (paymentPM.StatusCode != "VD")
                {
                    paymentPM.StatusCode = "VD";
                }

                UpdateDocOutNeedsRebuild();
            }

            else if (paymentPM.SetCancelApproval)
            {
                if (paymentPM.StatusCode != "DR")
                {
                    paymentPM.StatusCode = "DR";
                }

                UpdateDocOutNeedsRebuild();
            }

            else if (string.IsNullOrEmpty(paymentPM.StatusCode))
            {
                paymentPM.StatusCode = "DR";
            }

            if (paymentPM.RegisterDate != null)
            {
                paymentPM.RegisterDate = paymentPM.RegisterDate.Value.Date;
            }

            if (paymentPM.AccountingPaymentMethodCode == "CA")
            {
                paymentPM.ValueDate = paymentPM.RegisterDate;
            }
            CheckFullAccountingNumericFields(paymentPM);
            tenantRepository = new TenantRepository(paymentPM.Tenant);
            tenantPOCO = tenantRepository.GetSingleTenant(paymentPM.Tenant);
            cashBookQuery = ContainerAccessor.Container.Resolve(typeof(ICashBookQueryServiceExt), "CashBookQueryServiceExt", new ParameterOverride("", 1)) as ICashBookQueryServiceExt;
            if (tenantPOCO.AccountingActivated)
            {
                cashBookMethodType = paymentPM.AccountingPaymentMethodCode;
                if (paymentPM.AccountingPaymentMethodCode == "CA")
                {
                    cashBookMethodType = "1";
                }
                else if (paymentPM.AccountingPaymentMethodCode == "CH")
                {
                    cashBookMethodType = "2";
                }
                PaymentCashbook = cashBookQuery.GetByPaymentAndCurrencyAndBranch(paymentPM.PaymentCurrencyId, cashBookMethodType, paymentPM.BranchId, tenant);
                if (PaymentCashbook != null)
                {

                    paymentPM.CashbookId = PaymentCashbook.Id;
                }
            }
        }

        #endregion

        #region PaymentInvoice
        private void CreatePaymentInvoice(ARPaymentInvoicePM item)
        {
            this.ValidateIfSameRecordAdded(item);

            item.ARPaymentId = paymentPM.Id;
            item.ForeignCurrencyId = paymentPM.PaymentCurrencyId;
            item.Id = IdCounter.GetNumber("ARInvoicePayment", paymentPM.Tenant);

            ARInvoicePayment newObject = new ARInvoicePayment();
            ARPaymentMapping.MapEntityInvoicePyament(item, newObject, true);
            invoicePaymentRepository.Add(newObject);
            //if (tenantPOCO != null && tenantPOCO.AccountingActivated)
            //{
            ////    CreateInterestTransactionLine(item);
            //}
        }

        private int originalEntityLineNumber = 0;
        private void CreateInterestTransactionLine(ARPaymentPM payment,bool isFromVoidARPayment=false)
        {
            if (tenantPOCO != null && 
                tenantPOCO.AccountingActivated && payment.AccountingPaymentMethodCode != ChequeARPaymentAccountingMethod
               && payment.BillToPartnerTypeId == PartnerTypeId_Customer)
            {
            InterestTransactionPM interestTransaction = MapInterestTransactionPMFromARPaymentPM(payment, isFromVoidARPayment,2);
            SaveInterestTransaction(interestTransaction);
            }
            else  if(payment.AccountingPaymentMethodCode == ChequeARPaymentAccountingMethod && isFromVoidARPayment)
            {
                CancelARPaymentChequesInterestTransactions(payment);
                
            }
        }
        private void CancelARPaymentChequesInterestTransactions(ARPaymentPM payment)
        {
            List<InterestTransactionPM> interestTransactions = GetARPaymentChequeInterestTransactions(payment);
            int MaxLineNumber = interestTransactions.Max(d => d.OriginalEntityLineNumber);
            foreach (InterestTransactionPM transaction in interestTransactions)
            {
                MaxLineNumber++;
                InterestTransactionPM interestTransaction = MapInterestTransactionPMFromARPaymentPM(payment, true, MaxLineNumber);
                SaveInterestTransaction(interestTransaction);
            }
        }
        private List<InterestTransactionPM> GetARPaymentChequeInterestTransactions(ARPaymentPM payment)
        {
            IInterestTransactionQueryServiceExt interestTransactionQueryServiceExt = ContainerAccessor.Container.Resolve(typeof(IInterestTransactionQueryServiceExt), "InterestTransactionQueryServiceExt", new ParameterOverride("", 1)) as IInterestTransactionQueryServiceExt;
            return  interestTransactionQueryServiceExt.GetInterestTransactionsByPaymentId(payment.Id, payment.Tenant);

        }
        private void SaveInterestTransaction(InterestTransactionPM interestTransaction)
        {
            IInterestTransactionUpdateServiceExt interestTransactionUpdateService = ContainerAccessor.Container.Resolve(typeof(IInterestTransactionUpdateServiceExt), "InterestTransactionUpdateServiceExt", new ParameterOverride("", 1)) as IInterestTransactionUpdateServiceExt;
            interestTransactionUpdateService.Create(interestTransaction);
        }
        private InterestTransactionPM MapInterestTransactionPMFromARPaymentPM(ARPaymentPM payment, bool isFromVoidARPayment,int lineNumber)
        {
            DateTime? dateForInterest = payment.ValueDate == null ? DateTime.Now : payment.ValueDate;
            GLAccountPM account = GetGLAccount(payment.BillToId, payment.Tenant);
            InterestTransactionPM interestTransaction = new InterestTransactionPM()
            {
                InterestEntityTypeCode = "2",
                EntityId = payment.Id,
                OriginalEntityLineNumber = isFromVoidARPayment ? lineNumber : 1,
                LocalAmount = isFromVoidARPayment ? (decimal)payment.AmountInLocalCurrency :
                                                                     (decimal)payment.AmountInLocalCurrency * -1,
                ForeignAmount = isFromVoidARPayment ? (decimal?)payment.AmountInPaymentCurrency :
                                                                     (decimal?)payment.AmountInPaymentCurrency * -1,

                InterestValueDate = (DateTime)dateForInterest,
                    Tenant = paymentPM.Tenant,
                    GLAccountId = account != null ? account.Id : null,
                    ChangeSetOp = ChangeSetOperation.Insert,
                    CurrencyId = payment.PaymentCurrencyId,
                };
            return interestTransaction;
        }
 
        private void UpdatePaymentInvoice(ARPaymentInvoicePM item)
        {
            ARInvoicePayment invoicePayment = invoicePaymentRepository.GetSingleARInvoicePayment(item.Id, paymentPM.Tenant);

            if (invoicePayment != null)
            {
                ARPaymentMapping.MapEntityInvoicePyament(item, invoicePayment, false);
                invoicePaymentRepository.Update(invoicePayment);
            }
        }

        private void DeletePaymentInvoice(ARPaymentInvoicePM item)
        {
            List<ARInvoicePayment> data = invoicePaymentRepository.GetARInvoicePayments(item.ARPaymentId, item.ARInvoiceId, tenant).ToList();

            if (data != null)
            {
                foreach (ARInvoicePayment deletedItem in data)
                {
                    invoicePaymentRepository.Remove(deletedItem);
                }
            }
        }

        private void ValidateIfSameRecordAdded(ARPaymentInvoicePM item)
        {
            IQueryable<ARInvoicePayment> invoicePayments = invoicePaymentRepository.GetARInvoicePayments(item.ARPaymentId, item.ARInvoiceId, paymentPM.Tenant);
            if (invoicePayments.Count() > 0)
            {
                throw new Exception("This payment already connected to same invoice");
            }
        }
        #endregion

        #region Update Amounts
        private void UpdatePaymentOpenAmount()
        {
            bool isClosed = paymentPM.IsClosed;
            string StatusCode = paymentPM.StatusCode;
            double? Amount = MethodHelper.Roundd(paymentPM.AmountInPaymentCurrency, 2);
            double? PaidAmount = 0;
            double? OpenAmount = 0;

            List<ARInvoicePayment> allConnectedItems = invoicePaymentRepository.GetARInvoicePaymentByPaymentId(paymentPM.Id, paymentPM.Tenant).ToList();

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

                foreach (ARInvoicePayment item in allConnectedItems)
                {
                    if (!ids.Contains(item.ARInvoiceId))
                    {
                        if (item.PaymentAmount != null)
                        {
                            PaidAmount += item.PaymentAmount;
                        }

                        ids.Add(item.ARInvoiceId);
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

            paymentPoco.IsClosed = paymentPM.IsClosed = isClosed;
            paymentPoco.StatusCode = paymentPM.StatusCode = StatusCode;
            paymentPoco.OpenAmount = paymentPM.OpenAmount = OpenAmount;
            paymentPoco.AmountInPaymentCurrency = paymentPM.AmountInPaymentCurrency = Amount;

            if (this.SetVoided)
            {
                foreach (ARPaymentInvoicePM item in changedList)
                {
                    UpdateInvoiceAmounts(item.ARInvoiceId);
                }
            }

            else
            {
                foreach (ARPaymentInvoicePM item in changedList.Where(d => d.ChangeSetOp != ChangeSetOperation.None))
                {
                    UpdateInvoiceAmounts(item.ARInvoiceId);
                }
            }
        }
        private void UpdateInvoiceAmounts(string myInvoiceId)
        {
            if (!string.IsNullOrEmpty(myInvoiceId))
            {
                ARInvoiceQuery aRInvoiceQuery = new ARInvoiceQuery(this.invoiceRepository);
                ARInvoicePM invoice = aRInvoiceQuery.GetSinglePM(myInvoiceId, tenant);

                //ARInvoice invoice = this.GetInvoice(myInvoiceId, tenant);

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

                        List<ARInvoicePayment> allConnectedItems = invoicePaymentRepository.GetARInvoicePaymentByInvoiceId(invoice.Id, invoice.Tenant).ToList();
                        if (allConnectedItems.Count > 0)
                        {
                            List<string> ids = new List<string>();

                            foreach (ARInvoicePayment item in allConnectedItems)
                            {
                                if (!ids.Contains(item.ARPaymentId))
                                {
                                    if (item.ForeignAmount != null)
                                    {
                                        PaidAmount += item.ForeignAmount;
                                    }

                                    ids.Add(item.ARPaymentId);
                                }
                            }
                        }

                        PaidAmount = MethodHelper.Roundd(PaidAmount.Value, 2);

                        double invoiceAmount = MethodHelper.Roundd(invoice.AmountInInvoiceCurrency.Value, 2);
                        if (invoice.ARInvoiceTypeCode == "CD")
                        {
                            if (invoiceAmount > 0)
                            {
                                invoiceAmount = invoiceAmount * -1;
                            }
                        }

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
                                throw new Exception("The Amount due is not suitable to the total amount paid, for invoice: " + invoice.InvoiceNumber);
                            }
                        }

                        else
                        {
                            throw new Exception("The Amount due is not suitable to the total amount paid, for invoice: " + invoice.InvoiceNumber);
                        }

                        this.UpdateInvoicePaidDate(invoice);


                        //invoiceRepository.Update(invoice);
                        ARInvoiceService aRInvoiceService = new ARInvoiceService(this.objectContext, this.tenant);
                        aRInvoiceService.Update(invoice);
                        #endregion
                    }
                }
            }
        }
        private void UpdateInvoicePaidDate(ARInvoicePM invoice)
        {
            if (invoice.AmountDue != 0)
            {
                invoice.PaidDate = null;
            }

            else
            {
                invoice.PaidDate = this.paymentPM.ValueDate;
            }
        }
        #endregion

        #region SearchFields
        private void BuildSearchFields()
        {
            string mySearchFields = "";


            MethodHelper.AddToSearchFields(ref mySearchFields, paymentPM.PaymentNo);
            MethodHelper.AddToSearchFields(ref mySearchFields, paymentPM.StatusCode);
            MethodHelper.AddToSearchFields(ref mySearchFields, paymentPM.AccountingPaymentMethodCode);
            if (paymentPM.ARPaymentChequeReplicas.Any())
            {
                var chequesNumbers = String.Join(",", paymentPM.ARPaymentChequeReplicas.Select(x => x.ChequeNumber));
                MethodHelper.AddToSearchFields(ref mySearchFields, chequesNumbers);
            }
            MethodHelper.AddToSearchFields(ref mySearchFields, paymentPM.PrintNotes);

            #region Card
            if (!string.IsNullOrEmpty(paymentPM.BillToId))
            {
                Card myCard = CardRepository.GetSingleCard(paymentPM.BillToId, tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.LocalName);
                }
            }
            #endregion

            #region Currency
            if (!string.IsNullOrEmpty(paymentPM.PaymentCurrencyId))
            {
                Currency myCurrency = CurrencyRepository.GetSingleCurrency(paymentPM.PaymentCurrencyId, tenant, true);
                if (myCurrency != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCurrency.Code);
                }
            }
            #endregion

            #region Invoices
            if (paymentPM.PaymentInvoices != null)
            {
                foreach (ARPaymentInvoicePM item in paymentPM.PaymentInvoices)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, item.ARInvoiceNumber);
                }
            }
            #endregion

            MethodHelper.AddToSearchFields(ref mySearchFields, paymentPM.InternalNotes);

            paymentPM.SearchFields = mySearchFields;
            paymentPoco.SearchFields = mySearchFields;
        }
        #endregion

        #region InternalAccountingSystem
        private bool IsInternalAccountingSystem(int tenant)
        {
            bool rv = false;
            //AccountingSettingRepository accountingSettingRepository = new AccountingSettingRepository();
            AccountingSettingRepository accountingSettingRepository = new AccountingSettingRepository(tenant);
            AccountingSetting accountingSetting = accountingSettingRepository.GetSingleAccountSetting(tenant);
            if (accountingSetting != null)
            {
                rv = (accountingSetting.AccountingSystemCode == "LA"); //"Logitude Accounting"
            }
            return rv;
        }

        private void CreateChequeInCashBook(ARPaymentPM theEntityPm)
        {
            //1// Get CashBook (we need insert a code into validation also)
            //AccountingContext accountingContext = new AccountingContext();
            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            CashBookRepository cashBookRepository = new CashBookRepository(accountingContext);
            List<CashBook> cashBookList = cashBookRepository.GetByCurrencyAndTypeAndBranch(theEntityPm.PaymentCurrencyId, "2", theEntityPm.BranchId, theEntityPm.Tenant);//"2" == Cheques
            if (cashBookList != null)
            {
                CashBook cashBook = cashBookList.FirstOrDefault();
                if (cashBook != null)
                {

                    //TapagPM tapag = new TapagPM()
                    //{
                    //    Id = IdCounter.GetNumber("Customs.Tapag", paymentPM.Tenant),
                    //    Tenant = paymentPM.Tenant,
                    //    CustomerId = paymentPM.CustomerId,
                    //    CustomsBranchCode = paymentPM.CustomsBranchCode,
                    //    ImporterId = paymentPM.ImporterId,
                    //    FollowDate = paymentPM.FollowDate,
                    //    IsClosed = paymentPM.IsClosed,
                    //    LeadingFileNumber = paymentPM.LeadingFileNumber,
                    //    ProfessionUnitTypeCode = paymentPM.ProfessionUnitTypeCode,
                    //    SpecializationTypeCode = paymentPM.SpecializationTypeCode,
                    //    TapagNumber = paymentPM.TapagNumber,
                    //    TapagTypeCode = paymentPM.TapagTypeCode,
                    //    ValidityDate = paymentPM.ValidityDate,
                    //    CreateDate = paymentPM.CreateDate,
                    //    ChangeSetOp = ChangeSetOperation.Insert,
                    //};

                    //TapagUpdateService tapagUpdate = new TapagUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), paymentPM.Tenant);
                    //tapagUpdate.Update(tapag, false);
                    //paymentPM.TapagId = tapag.Id;


                    //   ARPaymentCheck cheque = new ARPaymentCheck();
                    //   MapChequeFromPayment(theEntityPm, cheque, true, accountingContext);
                    //   ARPaymentCheckRepository ARPaymentCheckRepository = new ARPaymentCheckRepository(accountingContext);
                    //   ARPaymentCheckRepository.Add(cheque);

                    //   PutChequeLine(theEntityPm, cashBook, cheque, true, accountingContext);
                }
            }
        }

        //private void MapChequeFromPayment(ARPaymentPM aRPaymentPM, ARPaymentCheck cheque, bool isNewState, AccountingContext accountingContext)
        //{
        //    if (isNewState)
        //    {
        //        cheque.Tenant = aRPaymentPM.Tenant;
        //        if (cheque.Id == null || cheque.Id == "") cheque.Id = IdCounter.GetNumber("ARPaymentCheck", cheque.Tenant);
        //        cheque.LineNumber = 1;
        //    }
        //    cheque.PaymentId = aRPaymentPM.Id;
        //    BankCodeRepository bankCodeRepository = new BankCodeRepository(accountingContext);
        //    BankCode bankCode = bankCodeRepository.GetSingleByCode(aRPaymentPM.Bank, tenant);
        //    if (bankCode != null)
        //    {
        //        cheque.BankId = bankCode.Id;
        //    }
        //    cheque.BankBranch = aRPaymentPM.BankBranch;
        //    cheque.BankAccount = aRPaymentPM.Account;
        //    cheque.ChequeNumber = aRPaymentPM.ChequeOrPaymentRef;
        //    if (aRPaymentPM.ValueDate.HasValue)
        //    {
        //        cheque.ValueDate = aRPaymentPM.ValueDate.Value;
        //    }
        //    cheque.CurrencyId = aRPaymentPM.PaymentCurrencyId;
        //    if (aRPaymentPM.AmountInPaymentCurrency.HasValue)
        //    {
        //        cheque.ForeignAmount = (decimal)Math.Round(aRPaymentPM.AmountInPaymentCurrency.Value, 2);
        //    }
        //    if (aRPaymentPM.AmountInLocalCurrency.HasValue)
        //    {
        //        cheque.LocalAmount = (decimal)Math.Round(aRPaymentPM.AmountInLocalCurrency.Value, 2);
        //    }

        //    cheque.SearchFields = cheque.ChequeNumber + "," + cheque.BankAccount;
        //}

        //private void PutChequeLine(ARPaymentPM aRPaymentPM, CashBook cashBook, ARPaymentCheck cheque, bool isNewState, AccountingContext accountingContext)
        //{
        //    CashBookLine cashBookLine = new CashBookLine()
        //    {
        //        CashBookId = cashBook.Id,
        //        ARPChequeId = cheque.Id,
        //        IsDeposited = false,
        //    };
        //}
        #endregion

        private void TraceConnected()
        {
            foreach (ARPaymentInvoicePM item in changedList)
            {
                switch (item.ChangeSetOp)
                {
                    case ChangeSetOperation.Insert:
                        {
                            ARInvoice invoice = this.GetInvoice(item.ARInvoiceId, tenant);
                            if (invoice != null)
                            {
                                EventTracer.CreateTraceEvent(new EventTracerArgs()
                                {
                                    Tenant = paymentPM.Tenant,
                                    EventTypeCode = "COAR",
                                    UserId = loggedContact.Id,
                                    EntityId = item.ARInvoiceId,
                                    ObjectTableName = "ARInvoice",
                                    Notes = "Connected with Payment: " + paymentPM.PaymentNo + " with Amount Due equals to: " + invoice.AmountDue,
                                });
                            }

                            break;
                        }

                    case ChangeSetOperation.Delete:
                        {
                            ARInvoice invoice = this.GetInvoice(item.ARInvoiceId, tenant);
                            if (invoice != null)
                            {
                                EventTracer.CreateTraceEvent(new EventTracerArgs()
                                {
                                    Tenant = paymentPM.Tenant,
                                    EventTypeCode = "ARID",
                                    UserId = loggedContact.Id,
                                    EntityId = item.ARInvoiceId,
                                    ObjectTableName = "ARInvoice",
                                    Notes = "Disconnected from Payment: " + paymentPM.PaymentNo,
                                });
                            }

                            break;
                        }

                    default: { break; }
                }
            }
        }

        public bool IsAccountingActivated { 
            get { return tenantPOCO != null && tenantPOCO.AccountingActivated; }
        }

        #region ARPaymentChequeAndCashBook
        private void AddARPaymentChequeAndCashBookLinesAndJournal(ARPaymentPM arpaymentPM, bool setApproved)
        {

            if (IsAccountingActivated && (setApproved || arpaymentPM.IsExternalEntity))
            {
                //UpdateCashCashbookTotal(arpaymentPM);

                //var paymentForChequeCashbook = cashBook != null && arpaymentPM.AccountingPaymentMethodCode == "CH";
                //if (paymentForChequeCashbook)
                //{
                //    ARPaymentChequePM arPaymentcheque = CreateChequeAndCashbookLinesForPayment(arpaymentPM);
                //    CreatePaymentJournal(arpaymentPM, arPaymentcheque);
                //}
                //else
                //    CreatePaymentJournal(arpaymentPM);
            }
        }

        GLAccountPM paymentGLAccount = null;

        public bool IsCashPayment
        {
            get { return paymentPM.AccountingPaymentMethodCode == "CA"; }
        }

        public bool IsChequePayment
        {
            get { return paymentPM.AccountingPaymentMethodCode == "CH"; }
        }



       

    
       
        private GLAccountPM GetPaymentGLAccount()
        {
            GLAccountPM glAccount = null;
            if (paymentPM.IsFullAccounting)
            {
                glAccount = GetGLAccount(paymentPM.BillToId, paymentPM.Tenant);
                paymentGLAccount = glAccount;
            }
            return glAccount;
        }


        private GLAccountPM GetGLAccount(string billToId, int tenant)
        {
            GLAccountPM glaAccount = null;
            CardRepository cardRep = new CardRepository(tenant);
            Card card = cardRep.GetSingleCard(billToId, tenant);
            if (card != null)
            {
                IGLAccountQueryServiceExt glAccountQuery = ContainerAccessor.Container.Resolve(typeof(IGLAccountQueryServiceExt), "GLAccountQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountQueryServiceExt;
                glaAccount = glAccountQuery.GetSingleGLAccountPM(card.GLAccountId, tenant);

                if (glaAccount!=null && glaAccount.IsMultiCurrency.Value)
                {
                  string splitByCurrencyAccountId=  GetAccountIdForGLAccountCurrency(glaAccount, paymentPM.PaymentCurrencyId);
                    glaAccount= glAccountQuery.GetSingleGLAccountPM(splitByCurrencyAccountId, tenant);
                }
                else return glaAccount;
            }


            return glaAccount;
        }

       
        private string getBankCode(int tenant)
        {
            string bankId = "";
            IBankCodeQueryServiceExt bankCodeQuery = ContainerAccessor.Container.Resolve(typeof(IBankCodeQueryServiceExt), "BankCodeQueryServiceExt", new ParameterOverride("", 1)) as IBankCodeQueryServiceExt;
            BankCodePM bankCode = bankCodeQuery.GetByFirstOrDefault(tenant);
            if (bankCode != null)
                bankId = bankCode.Id;
            return bankId;
        }

        #endregion 


        private void GetPaymentForeignFields()
        {
            if (paymentPoco.StatusCode != null)
            {
                ARPaymentStatusRepository myRepository = new ARPaymentStatusRepository(objectContext);

                ARPaymentStatus status = myRepository.GetSingleARPaymentStatus(paymentPoco.StatusCode);
                paymentPM.StatusName = status.Name;
            }

            if (paymentPoco.TransferStatusCode != null)
            {
                ARPaymentTransferStatusRepository myRepository = new ARPaymentTransferStatusRepository(objectContext);

                ARPaymentTransferStatus status = myRepository.GetSingleARPaymentTransferStatus(paymentPoco.TransferStatusCode);
                paymentPM.TransferStatusName = status.Name;
            }

        }
        private void BuildEntitiesNumbers()
        {
            string invoiceNumber = null;
            string shipmentNumber = null;
            List<ARPaymentInvoicePM> invoicesEntities = paymentPM.PaymentInvoices.Where(d => d.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete).ToList();
            if (invoicesEntities.Count > 0)
            {
                if (invoicesEntities.Count == 1)
                {
                    string invoiceId = invoicesEntities.Select(s => s.ARInvoiceId).FirstOrDefault();

                    var data = (from d in objectContext.ARInvoices
                                where d.Tenant == this.tenant
                                && d.Id == invoiceId
                                select new
                                {
                                    InvoiceNumber = d.InvoiceNumber,
                                    ShipmentNumber = d.MainEntityReference
                                }).FirstOrDefault();

                    invoiceNumber = data.InvoiceNumber;
                    shipmentNumber = data.ShipmentNumber;
                }

                else
                {
                    invoiceNumber = "Multi";
                    shipmentNumber = "Multi";
                }
            }

            if (paymentPM.InvoiceNumber != invoiceNumber || paymentPM.ShipmentNumber != shipmentNumber)
            {
                
                paymentPM.InvoiceNumber = invoiceNumber;
                paymentPM.ShipmentNumber = shipmentNumber;
                paymentPoco.InvoiceNumber = paymentPM.InvoiceNumber;
                paymentPoco.ShipmentNumber = paymentPM.ShipmentNumber;
                paymentRepository.Update(paymentPoco);
                paymentRepository.SubmitChanges();
            }
        }

        #region Transfer 
        private void InitializeTransferComponents()
        {
            this.InitializeTransferFields();

            if (this.paymentPM.SetApproved && string.IsNullOrEmpty(paymentPM.TransferError))
            {
                if (this.isTransferToDropbox && this.TransferToDropboxActivated)
                {
                    this.paymentPM.TransferStatusCode = "TR";
                }
            }
        }
        private void InitializeTransferFields()
        {
            bool isInitializing = true;

            if (paymentPM.SetReTransfer)
            {
                isInitializing = true;
            }

            else if (paymentPoco.TransferStatusCode == "TR")
            {
                isInitializing = false;
                paymentPM.TransferError = null;
                paymentPM.TransferStatusCode = "TR";
            }

            else if (paymentPoco.TransferStatusCode == "IP")
            {
                isInitializing = false;
                paymentPM.TransferError = null;
            }

            else if (paymentPM.TransferStatusCode == "BL")
            {
                isInitializing = false;
                paymentPM.TransferError = null;
                paymentPM.TransferStatusCode = "BL";
            }

            if (isInitializing)
            {
                #region
                bool isReady = true;
                string myError = null;
                
                CardRepository cardRep = new CardRepository(paymentPoco.Tenant);
                CurrencyRepository currencyRep = new CurrencyRepository(paymentPoco.Tenant);
                Card card = cardRep.GetSingleCard(paymentPM.BillToId, paymentPM.Tenant);
                Currency currency = currencyRep.GetSingleCurrency(paymentPoco.PaymentCurrencyId, paymentPoco.Tenant);
                string currencyError = "Currency External Id is missing";
                if (currency != null && !string.IsNullOrEmpty(currency.Code))
                {
                    currencyError = "Currency: " + currency.Code + ". External Id is missing";
                }

                if (card != null)
                {
                    AccountingSystemHelper accountingSystemHelper = new AccountingSystemHelper();
                    var receivablesAccountingCard = accountingSystemHelper.GetGenericCreditAccount(card.Id, paymentPM.PaymentCurrencyId, tenant, false);
                    if (FieldIsEmpty(receivablesAccountingCard))
                    {
                        isReady = false;
                        myError = "Bill To: " + paymentPM.BillToName + ". External Id is missing";
                    }
                }

                if (currency != null && FieldIsEmpty(currency.AccountingExternalCode))
                {
                    isReady = false;
                    myError = string.IsNullOrEmpty(myError) ? currencyError : myError + "," + currencyError;
                }

                if (isReady)
                {
                    paymentPM.TransferStatusCode = "RD";
                    paymentPM.TransferError = null;
                }

                else
                {
                    paymentPM.TransferStatusCode = "NR";
                    paymentPM.TransferError = myError;
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

        private void UpdateDocOutNeedsRebuild()
        {
            DocumentOutRepository documentOutRepository = new DocumentOutRepository(myCommonContext);
            DocumentOut docOut = documentOutRepository.GetDocumentOutByEntityAndChildEntity(paymentPoco.Id, null);
            if (docOut != null)
            {
                docOut.NeedsRebuild = true;
                documentOutRepository.Update(docOut);
                documentOutRepository.SubmitChanges();
            }
        }

        private void VoidARPaymentInFullAccounting(ARPaymentPM entityPm, bool setVoided)
        {
            int tenant = entityPm.Tenant;
            bool useLocal = true;
            var user = GetLoggedContact(tenant);
            if (user != null) useLocal = !(GetLoggedContact(tenant).DontShowLocal);
            if (setVoided)
            {
                TenantRepository tenantRepository = new TenantRepository(tenant);
                Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);

                if (tenantPOCO.AccountingActivated)
                {
                    if (entityPm.AccountingPaymentMethodCode == "CA" || entityPm.AccountingPaymentMethodCode == "CH" || entityPm.AccountingPaymentMethodCode == "BT")
                    {
                        this.ValidateVoidedARPaymentFullAccounting(entityPm);
                        if (entityPm.AccountingPaymentMethodCode == "CA")
                        {
                            CashBookPM cashBook = GetPaymentCashbook();

                            if (cashBook == null)
                            {
                                string msg = TranslateTextsClass.Translate("ARPayment.M.ARPaymentCashbook", entityPm.Tenant, useLocal);
                                throw new ApplicationException(msg);
                            }
                            else
                            {
                                if (cashBook.TotalAmount < (decimal)entityPm.AmountInPaymentCurrency)
                                {
                                    string msg = TranslateTextsClass.Translate("ARPayment.M.ARpaymentValueHigherThanCashbookValue", entityPm.Tenant, useLocal);
                                    throw new ApplicationException(msg);
                                }
                                else
                                {
                                    // Update Total Amount
                                    ICashBookUpdateServiceExt cashBookUpdate = ContainerAccessor.Container.Resolve(typeof(ICashBookUpdateServiceExt), "CashBookUpdateServiceExt", new ParameterOverride("", 1)) as ICashBookUpdateServiceExt;
                                    cashBook.TotalAmount = cashBook.TotalAmount - (decimal)entityPm.AmountInPaymentCurrency;
                                    // cashBook.TotalAmount += (decimal)entityPm.AmountInPaymentCurrency;
                                    cashBook.ChangeSetOp = ChangeSetOperation.Update;
                                    cashBookUpdate.Update(cashBook);
                                    CreateVoidedARPaymentEvent("ARPayment Cancel");
                                    CancelJournal(entityPm);
                                    //CancelledInterestTransactions(entityPm);
                                    CreateInterestTransactionLine(entityPm,true);
                                }
                            }
                        }
                        else if (entityPm.AccountingPaymentMethodCode == "CH")
                        {
                            IARPaymentChequeQueryServiceExt query = ContainerAccessor.Container.Resolve(typeof(IARPaymentChequeQueryServiceExt), "ARPaymentChequeQueryServiceExt", new ParameterOverride("", 1)) as IARPaymentChequeQueryServiceExt;
                            List<ARPaymentChequePM> aRPaymentCheques = query.GetListByPaymentId(entityPm.Id, tenant);
                            if (aRPaymentCheques != null)
                            {
                                var list = aRPaymentCheques.Where(a => a.StatusCode == "6" || a.StatusCode == "3" || a.StatusCode == "2").ToList(); // 2-In bank / 6 (ReDeemed-נפרע) or 3- In bank account 
                                if (list != null && list.Count() != 0)
                                {
                                    string msg = TranslateTextsClass.Translate("ARPayment.M.CANTCancelARPayment", entityPm.Tenant, useLocal);
                                    //    + "{ ";
                                    //foreach (var item in list)
                                    //{
                                    //    msg += "Cheque No.: " + item.ChequeNumber + ", Status: " + item.StatusName;
                                    //}
                                    throw new ApplicationException(msg);
                                }
                                else
                                {
                                    IARPaymentChequeUpdateServiceExt paymentUpdate = ContainerAccessor.Container.Resolve(typeof(IARPaymentChequeUpdateServiceExt), "ARPaymentChequeUpdateServiceExt", new ParameterOverride("", 1)) as IARPaymentChequeUpdateServiceExt;
                                    foreach (var item in aRPaymentCheques)
                                    {
                                        item.ChangeSetOp = ChangeSetOperation.Update;
                                        item.StatusCode = "5";
                                        paymentUpdate.Update(item);
                                        CreateVoidedARPaymentEvent("Returned To Customer - Cheque Number: " + item.ChequeNumber);
                                    }

                                    ICashBookQueryServiceExt cashQuery = ContainerAccessor.Container.Resolve(typeof(ICashBookQueryServiceExt), "CashBookQueryServiceExt", new ParameterOverride("", 1)) as ICashBookQueryServiceExt;
                                    ICashBookUpdateServiceExt cashBookUpdate = ContainerAccessor.Container.Resolve(typeof(ICashBookUpdateServiceExt), "CashBookUpdateServiceExt", new ParameterOverride("", 1)) as ICashBookUpdateServiceExt;
                                    CashBookPM cashBook = cashQuery.GetByPaymentAndCurrencyAndBranch(entityPm.PaymentCurrencyId, "2", entityPm.BranchId, tenant);
                                    cashBook.TotalAmount = cashBook.TotalAmount - aRPaymentCheques.Sum(a => a.ForeignAmount);
                                    cashBook.ChangeSetOp = ChangeSetOperation.Update;
                                    cashBookUpdate.Update(cashBook);
                                    CreateVoidedARPaymentEvent("ARPayment Cancel");
                                    CancelJournal(entityPm);
                                    //CancelledInterestTransactions(entityPm);
                                    CreateInterestTransactionLine(entityPm, true);
                                }
                            }
                        }
                        else if (entityPm.AccountingPaymentMethodCode == "BT")
                        {
                            CreateVoidedARPaymentEvent("ARPayment Cancel");
                            CancelJournal(entityPm);
                            CreateInterestTransactionLine(entityPm, true);
                            CancelledInterestTransactions(entityPm);
                        }
                    }
                }
            }
        }

        private CashBookPM GetPaymentCashbook()
        {
            ICashBookQueryServiceExt cashQuery = ContainerAccessor.Container.Resolve(typeof(ICashBookQueryServiceExt), "CashBookQueryServiceExt", new ParameterOverride("", 1)) as ICashBookQueryServiceExt;
            CashBookPM cashBook = cashQuery.GetByPaymentAndCurrencyAndBranch(paymentPM.PaymentCurrencyId, "1", paymentPM.BranchId, tenant);
            PaymentCashbook = cashBook;
            return cashBook;
        }

        public Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }
        private ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }


            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }

        private void ValidateVoidedARPaymentFullAccounting(ARPaymentPM entityPm)
        {

            var errors = "";
            IAccountingContext myContext = AccountingContext.GetContext(tenant);
            AccountingPeriodListQueryService accountingPeriodQuery = new AccountingPeriodListQueryService(myContext);
            var now = TenantServerConfigration.GetCurrentDateTime(tenant);
            AccountingPeriodList accountingPeriodList = accountingPeriodQuery.GetByYear(entityPm.RegisterDate.Value.Year, "1", tenant);
            if (accountingPeriodList != null && entityPm.RegisterDate != null)
            {
                var month = entityPm.RegisterDate.Value.Month;
                if (month > accountingPeriodList.OpenMonth || month <= accountingPeriodList.ClosedMonth)
                {
                    string msg = TranslateTextsClass.Translate("ARPayment.M.ClosedMonth", tenant);
                    errors += msg + ";";
                }
            }
            else
            {
                string msg = TranslateTextsClass.Translate("ARPayment.M.ClosedMonth", tenant);
                errors += msg + ";";
            }

            if (!string.IsNullOrEmpty(errors))
            {
                errors = errors.TrimEnd(';');
                throw new ApplicationException(errors);
            }
        }
        private void CreateVoidedARPaymentEvent(string note)
        {
            //EventTracer.CreateTraceEvent(new EventTracerArgs()
            //{
            //    Tenant = paymentPM.Tenant,
            //    EventTypeCode = "CAAR",
            //    UserId = loggedContact.Id,
            //    EntityId = paymentPM.Id,
            //    ObjectTableName = "ARPayment",
            //    Notes = note,
            //});
        }

        public void CancelledInterestTransactions(ARPaymentPM entityPm)
        {
            IInterestReportUpdateServiceExt InterestReportUpdate = ContainerAccessor.Container.Resolve(typeof(IInterestReportUpdateServiceExt), "InterestReportUpdateServiceExt", new ParameterOverride("", 1)) as IInterestReportUpdateServiceExt;
            InterestReportUpdate.CancelledInterestTransactionsByARPayment(entityPm.Id, entityPm.Tenant);
        }

        private void CancelJournal(ARPaymentPM entityPm)
        {

            IJournalQueryServiceExt journalQuery = ContainerAccessor.Container.Resolve(typeof(IJournalQueryServiceExt), "JournalQueryServiceExt", new ParameterOverride("", 1)) as IJournalQueryServiceExt;
            JournalPM journalPM = journalQuery.GetJournalByAccountingEntityIdAndCode(paymentPM.Id, "3", paymentPM.Tenant);
            if (journalPM != null)
            {
                //journalPM.AccountingEntityReference = paymentPM.PaymentNo;
                //journalPM.AccountingEntityCode = "3";
                //journalPM.StatusCode = "3";
                //journalPM.ChangeSetOp = ChangeSetOperation.Update;

                //IJournalUpdateServiceExt journalUpdate = ContainerAccessor.Container.Resolve(typeof(IJournalUpdateServiceExt), "JournalUpdateServiceExt", new ParameterOverride("", 1)) as IJournalUpdateServiceExt;

                var journalUpdate = ContainerAccessor.Container.Resolve(typeof(IJournalVoidUpdateServiceExt), "JournalVoidUpdateServiceExt", new ParameterOverride("", 1)) as IJournalVoidUpdateServiceExt;

                //journal.AccountingEntityCode = "3";
                //journal.AccountingEntityId = theEntityPm.Id;
                //journal.AccountingEntityReference = theEntityPm.PaymentNo;

                journalUpdate.Update(journalPM, new StornoOverrideM()
                {
                    AccountingEntityCode = "3",
                    LineNotes = paymentPM.CancelationNotes,
                    AccountingEntityId = paymentPM.Id,
                    AccountingDate = entityPm.AccountingCancelationDate,
                    AccountingEntityReference = paymentPM.PaymentNo
                });
                JournalPM voidedByJournal = GetApprovedJournalByAccountingEntityId(entityPm);

                paymentPM.VoidedByJournalNumber = voidedByJournal != null ? voidedByJournal.JournalNumber : null;

            }
        }

        #region Full Accounting
        public void CreateReconciliationForARPayment(ARPaymentPM paymentPM)
        {
          
            if (paymentPM.InvoicesLedgerTransactions.Count == 0)
                return;

            IAccountingContext ctx = AccountingContext.GetContext(paymentPM.Tenant);
            ReconciliationPM _reco = new ReconciliationPM();
            _reco.ChangeSetOp = ChangeSetOperation.Insert;
            _reco.Number = "get";
            _reco.Tenant = paymentPM.Tenant;
            _reco.AccountId = paymentPM.GLAccountId;
            _reco.AccountReconcileMethodCode = paymentPM.GLAccountRecoMethodCode;
            _reco.CreateDate = TenantServerConfigration.GetCurrentDateTime(paymentPM.Tenant);

            GLAccountListQueryService glaQuery = new GLAccountListQueryService(ctx);
            GLAccountCurrencyList splittedAccount = null;
            GLAccountList gla = glaQuery.GetByAccountId(paymentPM.GLAccountId, paymentPM.Tenant);
            if (gla != null)
            {
                _reco.AccountCurrencyId = gla.CurrencyId;
                _reco.CurrencyCode = gla.CurrencyCode;

                GLAccountCurrencyListQueryService glaCurrencyQuery = new GLAccountCurrencyListQueryService(ctx);
                splittedAccount = glaCurrencyQuery.GetByAccountAndCurrency(paymentPM.GLAccountId, paymentPM.PaymentCurrencyId, paymentPM.Tenant);

            }

            // get payment line LT
            LedgerTransactionListQueryService ltListQuery = new LedgerTransactionListQueryService(ctx);
 
            string accountId = GetGLAccountIdForReconciledTransactions(paymentPM.GLAccountId, paymentPM.Tenant, paymentPM.PaymentCurrencyId);

            List<LedgerTransactionList> accountingTransactionList = ltListQuery.GetByAccountId(splittedAccount != null ? splittedAccount.GLAccountId : accountId, paymentPM.Tenant);
            LedgerTransactionList paymentTransaction = accountingTransactionList.Where(d => d.SourceNumber == paymentPM.PaymentNo).FirstOrDefault(); // 3- ARPayment
            if (paymentTransaction == null) throw new ApplicationException("Cannot find ledger transaction for this payment!");

            

            // reco payment line
            var _recoPYLine = CreatePaymentRecoLine(paymentPM);
            _recoPYLine.TransactionId = paymentTransaction.Id;
            _reco.ReconciliationLines.Add(_recoPYLine);

            // invoices lines
            int line = 2;
            foreach (LedgerTransactionPM invoiceLT in paymentPM.InvoicesLedgerTransactions)
            {
                var _recoInLine = CreateInvoiceRecoLine(paymentPM, invoiceLT, line++);
                _reco.ReconciliationLines.Add(_recoInLine);
            }

            //call reco service
            var recoService = ContainerAccessor.Container.Resolve(typeof(IReconciliationServiceExt), "ReconciliationServiceExt", new ParameterOverride("", 1)) as IReconciliationServiceExt;
            recoService.CreateReconciliation(_reco);


        }
        public void UpdateTransactions(ARPaymentPM paymentPM)
        {
            foreach (LedgerTransactionPM invoiceLT in paymentPM.InvoicesLedgerTransactions)
            {
                //update ledger transaction
                ///....

            }
        }
        private string GetGLAccountIdForReconciledTransactions(string glAccountId, int tenant, string paymentCurrencyId)
        {
            IGLAccountQueryServiceExt glAccountQuery = ContainerAccessor.Container.Resolve(typeof(IGLAccountQueryServiceExt), "GLAccountQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountQueryServiceExt;
            
            GLAccountPM gLAccount = glAccountQuery.GetSingleGLAccountPM(glAccountId, tenant);
            if (gLAccount != null)
            {
                if (gLAccount.IsMultiCurrency.Value)
                {
                  return  GetAccountIdForGLAccountCurrency(gLAccount,paymentCurrencyId);                  
                }
                else
                    return gLAccount.Id;

            }
            return null;
        }

        private string GetAccountIdForGLAccountCurrency(GLAccountPM gLAccount, string paymentCurrencyId)
        {
            GLAccountCurrencyRepository glAccountCurrencyRepository = new GLAccountCurrencyRepository(gLAccount.Tenant);
            GLAccountCurrency gLAccountCurrency = glAccountCurrencyRepository.GetEntityByCurrencyAndGLAccountId(gLAccount.Id, paymentCurrencyId, gLAccount.Tenant);
            if (gLAccountCurrency != null)
            {
                return gLAccountCurrency.GLAccountId;
            }
            else return gLAccount.Id;

        }
        private ReconciliationLinePM CreatePaymentRecoLine(ARPaymentPM paymentPM)
        {
            ReconciliationLinePM _paymentLine = new ReconciliationLinePM();
            _paymentLine.ChangeSetOp = ChangeSetOperation.Insert;
            _paymentLine.Tenant = paymentPM.Tenant;
            _paymentLine.Line = 1;

            // validate
            if (string.IsNullOrEmpty(paymentPM.GLAccountRecoMethodCode))
                throw new ApplicationException("No reco method provided in payment!");

            bool useLocalRecoMethod = paymentPM.GLAccountRecoMethodCode == "0";


            //amount
            decimal invoiceAmountToReconcileSum = paymentPM.InvoicesLedgerTransactions.Sum(d => d.AmountToReconcile);
            _paymentLine.ReconciliationAmount = invoiceAmountToReconcileSum * -1;

            //currency
            _paymentLine.CurrencyId = useLocalRecoMethod ? paymentPM.LocalCurrencyId : paymentPM.PaymentCurrencyId;

            //isPartial
            _paymentLine.IsPartial = Convert.ToDecimal(paymentPM.OpenAmount) == _paymentLine.ReconciliationAmount;

            //GroupNumber
            _paymentLine.GroupNumber = 1;

            return _paymentLine;
        }
        private ReconciliationLinePM CreateInvoiceRecoLine(ARPaymentPM paymentPM, LedgerTransactionPM invoiceTransactionPM, int line)
        {
            ReconciliationLinePM _invoiceLine = new ReconciliationLinePM();
            _invoiceLine.ChangeSetOp = ChangeSetOperation.Insert;
            _invoiceLine.Tenant = paymentPM.Tenant;
            _invoiceLine.TransactionId = invoiceTransactionPM.Id;
            _invoiceLine.Line = line;

            // validate
            if (string.IsNullOrEmpty(paymentPM.GLAccountRecoMethodCode))
                throw new ApplicationException("No reco method provided in payment!");

            bool useLocalRecoMethod = paymentPM.GLAccountRecoMethodCode == "0";

            //amount
            _invoiceLine.ReconciliationAmount = invoiceTransactionPM.AmountToReconcile;

            //currency
            _invoiceLine.CurrencyId = useLocalRecoMethod ? paymentPM.LocalCurrencyId : paymentPM.PaymentCurrencyId;

            //isPartial
            _invoiceLine.IsPartial = invoiceTransactionPM.AmountToReconcile != CalculateInvoiceAmount(invoiceTransactionPM, useLocalRecoMethod);

            //GroupNumber
            _invoiceLine.GroupNumber = 1;

            return _invoiceLine;
        }

        private decimal CalculateInvoiceAmount(LedgerTransactionPM transaction, bool useLocalRecoMethod)
        {

            if (useLocalRecoMethod)
            { // 0-local currency

                if (transaction.LocalAmountCredit == 0)
                {
                    return transaction.LocalAmountDebit;
                }
                else
                {
                    return -1 * transaction.LocalAmountCredit;
                }

            }
            else
            { // 1-foreign currency

                if (transaction.ForeignAmountCredit == 0)
                {
                    return transaction.ForeignAmountDebit;
                }
                else
                {
                    return -1 * transaction.ForeignAmountCredit;
                }

            }

        }

        GLAccountPM FillGLAccountFields(ARPaymentPM paymentPM)
        {
            if (paymentPM.IsFullAccounting)
            {
                GLAccountPM gla = GetGLAccount(paymentPM.BillToId, paymentPM.Tenant);
                if (gla != null)
                {
                    paymentPM.GLAccountId = gla.Id;
                    paymentPM.GLAccountRecoMethodCode = gla.ReconcileMethodCode;
                }

                return gla;
            }
            return null;
        }

        void FillPaymentInvoicesFromInvoicesTransactions(ARPaymentPM paymentPM, bool isMultiCurrency)
        {
            bool useLocalRecoMethod = paymentPM.GLAccountRecoMethodCode == "0";

            foreach (LedgerTransactionPM invTrans in paymentPM.InvoicesLedgerTransactions)
            {
                ARPaymentInvoicePM payInvPM;
                if (useLocalRecoMethod || isMultiCurrency)
                {
                    payInvPM = new ARPaymentInvoicePM()
                    {
                        ARInvoiceId = invTrans.SourceId,
                        LocalAmount = (double)invTrans.AmountToReconcile,
                        ForeignAmount = (double)(invTrans.AmountToReconcile / invTrans.ExchangeRate),
                        ForeignCurrencyId = invTrans.CurrencyId,
                        ARPaymentId = this.paymentPM.Id,
                    };
                }
                else
                {
                    payInvPM = new ARPaymentInvoicePM()
                    {
                        ARInvoiceId = invTrans.SourceId,
                        LocalAmount = Convert.ToDouble(invTrans.AmountToReconcile * invTrans.ExchangeRate),
                        ForeignAmount = (double)invTrans.AmountToReconcile,
                        ForeignCurrencyId = invTrans.CurrencyId,
                        ARPaymentId = this.paymentPM.Id,
                    };
                }

                paymentPM.PaymentInvoices.Add(payInvPM);
             
            }


        }

        private void ValidateFullAccounting(ARPaymentPM _payment)
        {
            if (_payment.IsFullAccounting)
            {
                CheckLinesAmountToReconcileLimit(_payment);
                CheckCreditLinesAmountToReconcile(_payment);
                CheckLinesAmountToReconcileTotal(_payment);
            }

        }

        private void CheckLinesAmountToReconcileLimit(ARPaymentPM _payment)
        {
            ContactPM loggedContact = GetLoggedContactPM(_payment.Tenant);
            bool showLocal = loggedContact != null ? (!loggedContact.DontShowLocal) : false;

            // Validate lines amount to reconcile
            foreach (var invoice in _payment.InvoicesLedgerTransactions)
            {
                if(invoice.AmountToReconcile > 0)
                {
                    if(invoice.AmountToReconcile > CalculateInvoiceAmount(invoice, _payment.GLAccountRecoMethodCode == "0"))
                        throw new ApplicationException(TextCodesTranslator.TranslateText("Reconciliations.O.ErrorsInSelectedLines", _payment.Tenant, showLocal));
                }
                else
                {
                    if (invoice.AmountToReconcile < CalculateInvoiceAmount(invoice, _payment.GLAccountRecoMethodCode == "0"))
                        throw new ApplicationException(TextCodesTranslator.TranslateText("Reconciliations.O.ErrorsInSelectedLines", _payment.Tenant, showLocal));
                }
            }
            
        }
        private void CheckCreditLinesAmountToReconcile(ARPaymentPM _payment)
        {
            bool showLocal = LoggedContactResolver.GetLoggedContactShowLocal(_payment.Tenant);


            var linesAmountToReconcileSum = _payment.InvoicesLedgerTransactions.Sum(d => d.AmountToReconcile);
            if(linesAmountToReconcileSum < 0)
                throw new ApplicationException(TextCodesTranslator.TranslateText("Reconciliation.O.CantReconcileCreditInvoiceOnly", _payment.Tenant, showLocal));

        }
        private void CheckLinesAmountToReconcileTotal(ARPaymentPM _payment)
        {
            if(_payment.IsClosed && _payment.IsExternalEntity)
                return;
            
            ContactPM loggedContact = GetLoggedContactPM(_payment.Tenant);
            bool showLocal = loggedContact != null ? (!loggedContact.DontShowLocal) : false;


            decimal amount2reconcile = GetAmountToReconcileTotalFromPaymentInvoices(_payment);

            //decimal amount2reconcile = _payment.InvoicesLedgerTransactions.Sum(d => d.AmountToReconcile);
            if (_payment.OpenAmount == null)
            {
                _payment.OpenAmount = 0;
            }

            if (amount2reconcile > (decimal)_payment.OpenAmountInLocalCurrency)
                throw new ApplicationException(TextCodesTranslator.TranslateText("Accounting.O.ARP.paymentAmount2reconcileMSG", _payment.Tenant, showLocal));
        }

        private decimal GetAmountToReconcileTotalFromPaymentInvoices(ARPaymentPM _payment)
        {
            bool useLocalRecoMethod = _payment.GLAccountRecoMethodCode == "0";
            decimal amount2reconcile = 0;

            if (_payment.PaymentInvoices.Count > 0)
            {

                if (useLocalRecoMethod)
                    amount2reconcile = (decimal)_payment.PaymentInvoices.Sum(d => d.LocalAmount);   // amount to reconcile = Local Amount
                else
                    amount2reconcile = (decimal)_payment.PaymentInvoices.Sum(d => d.ForeignAmount); // amount to reconcile = Foreign Amount
            }
            else
            {
                    amount2reconcile = (decimal)_payment.InvoicesLedgerTransactions.Sum(d => d.AmountToReconcile);   // amount to reconcile = Local Amount
            }

            return amount2reconcile;
        }

        void UpdateFullAccountPaymentAmount(ARPaymentPM paymentPM, bool useLocalRecoMethod)
        {
            //
            // update payment amount:
            decimal amount2reconcile = paymentPM.InvoicesLedgerTransactions.Sum(d => d.AmountToReconcile);
            //paymentPM.OpenAmount
            //    = (useLocalRecoMethod ? paymentPM.AmountInLocalCurrency : paymentPM.AmountInPaymentCurrency)
            //       - (double) amount2reconcile;

            // amount sent updated from clientt
            //paymentPM.OpenAmount = paymentPM.OpenAmount - (double)amount2reconcile;


            //
            // update invoices amount
            //foreach (LedgerTransactionPM invTrans in paymentPM.InvoicesTransactions)
            //{
            //    //get invoice
            //    ARInvoice invoice = GetInvoice(invTrans.SourceId, tenant);

            //    //update
            //    double? invoiceAmountDue = MethodHelper.Round((invoice.AmountDue - (double)invTrans.AmountToReconcile), 2);

            //    invoice.AmountDue = invoiceAmountDue;
            //    invoice.AmountDueInLocalCurrency = MethodHelper.Round(invoice.AmountDue * invoice.InvoiceCurrencyExchangeRate, 2);
            //    invoice.AmountDueInProfitCurrency = MethodHelper.Round(invoice.AmountDueInLocalCurrency / invoice.ProfitCurrencyExchangeRate, 2);

            //    invoiceRepository.Update(invoice);
            //}





        }

        public ContactPM GetLoggedContactPM(int tenant)
        {
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }
        #endregion
    }
}

