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

namespace Logitude.BL.InvoiceModel.Tools.EntityService
{
    public class ARPaymentService
    {
        private int tenant;
        public ARPayment newPayment { get; set; }
        private ARPaymentPM entityPM;
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
        private CashBookPM cashBook;
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
        private void GetAccountingSystem()
        {
            AccountingSetting accountingSetting = accountingSettingRepository.GetSingleAccountSetting(tenant);
            AccountingSystem accountingSystem = accountingSystemRepository.GetSingleAccountingSystem(accountingSetting.AccountingSystemCode);
            this.isTransferToDropbox = accountingSystem.CanTransferToDropbox;
            this.TransferToDropboxActivated = accountingSetting.TransferToDropboxActivated;
        }

        /*
         *  CREATE
         */
        public void Create(ARPaymentPM _arpaymentPM)
        {

            _arpaymentPM.IsFullAccounting = IsFullAccActivated();


            isNewEntity = true;
            entityPM = _arpaymentPM;
            isVoidingInvoice = entityPM.SetVoided;
            changedList = _arpaymentPM.PaymentInvoices;

            newPayment = new ARPayment();

            InitializeComponent();

            ARPaymentValidator.Validate(_arpaymentPM, cashBook);
            ARPaymentTracing.Trace(_arpaymentPM, newPayment, isNewEntity);


            InitializeTransferComponents();

            //will reset in mapentity()
            var setApproved = _arpaymentPM.SetApproved;
            var setCancelApproved = _arpaymentPM.SetCancelApproval;
            var setVoided = _arpaymentPM.SetVoided;

            ARPaymentMapping.MapEntity(_arpaymentPM, newPayment, isNewEntity);

            paymentRepository.Add(newPayment);
            paymentRepository.SubmitChanges();
            SubmitPaymentInvoices(_arpaymentPM);

            UpdatePaymentOpenAmount();

            ARPaymentHelper service = new ARPaymentHelper();
            service.ARPaymentQuickbooksValidating(_arpaymentPM, setApproved, false, newPayment, objectContext, myCommonContext, isVoidingInvoice, setCancelApproved, _arpaymentPM.SetReSendQBO);

            BuildSearchFields();

            AccountingPaymentMethod ARPaymentMethod = GetARPaymentMethod();

            if (ARPaymentMethod != null && ARPaymentMethod.Code == "CH" && IsInternalAccountingSystem(_arpaymentPM.Tenant))//"CH" == Cheque
            {
                CreateChequeInCashBook(_arpaymentPM);
            }


            TraceConnected();

            FillFullAccountingPaymentInvoices(_arpaymentPM);

            ValidateFullAccounting(_arpaymentPM); // depends on Payment.PaymentInvoices


            // PaymentCheque And CashBook
            AddARPaymentChequeAndCashBook(_arpaymentPM, setApproved);
            newPayment.ValueDate = _arpaymentPM.ValueDate;
            paymentRepository.Update(newPayment);
            paymentRepository.SubmitChanges();

            GetPaymentForeignFields();

            BuildEntitiesNumbers();

            VoidARPaymentInFullAccounting(_arpaymentPM, setVoided);

            // DropBox
            CreateARInvoiceMessage(setApproved);

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
            if (isFullAccounting  && (payment.AccountingPaymentMethodCode == "CH" || payment.AccountingPaymentMethodCode == "BT"))
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
        private void ReturnExceptionForNumericFields(string field, string fieldTextCode ,int tenant)
        {
            int n;
            var isNumeric =  int.TryParse(field, out  n);
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

            string[] translatedText = TextCodesTranslator.TranslateText("Accounting.General.O.FieldMustBeNumeric",tenant, useLocal).Split(',');
            throw new Exception(translatedText[0] + TextCodesTranslator.TranslateText(fieldTextCode, tenant, useLocal) + translatedText[1]);

        }

        private AccountingPaymentMethod GetARPaymentMethod()
        {
            AccountingPaymentMethodRepository ARPaymentMethodRepository = new AccountingPaymentMethodRepository(objectContext);
            AccountingPaymentMethod ARPaymentMethod = ARPaymentMethodRepository.GetSingleAccountingPaymentMethod(newPayment.AccountingPaymentMethodId, tenant);
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
            ContactPM loggedUser = GetLoggedContactPM(theEntityPm.Tenant);
            theEntityPm.UpdatedByUserId = loggedUser?.Id;

            ValidateFullAccounting(theEntityPm);

            //get glaccount fields
            GLAccountPM gla = FillGLAccountFields(theEntityPm);

            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.SetVoided = theEntityPm.SetVoided;

            this.newPayment = paymentRepository.GetSingleARPayment(theEntityPm.Id);

            this.ValidateHigherStatus();

            this.InitializeComponent();

            ARPaymentValidator.Validate(theEntityPm, cashBook);
            ARPaymentTracing.Trace(theEntityPm, newPayment, isNewEntity);

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


            SATInterfaceSettingRepository sATInterfaceSettingRepository = new SATInterfaceSettingRepository(entityPM.Tenant);
            SATInterfaceSetting satSetting = sATInterfaceSettingRepository.GetSingleSATInterfaceSetting(entityPM.Tenant);
            if (satSetting.SATInterfaceCode == "PROF33")
            {
                if (this.entityPM.SetCancelApproval)
                {
                    if (newPayment.SATTransferStatusCode == "TD")
                        this.sATInterfaceHelper.SendPaymentSATCancellationRequest(entityPM, newPayment);
                    else if (newPayment.SATTransferStatusCode == "TG")
                        throw new ApplicationException("Can't cancel payment while being transfered to SAT");
                }

                if (theEntityPm.SetVoided)
                {
                    if (newPayment.SATTransferStatusCode == "TG")
                        throw new ApplicationException("Can't void payment while being transfered to SAT");
                    else if (newPayment.SATTransferStatusCode == "TE" && !string.IsNullOrEmpty(newPayment.SATXML))
                        throw new ApplicationException("Can't void payment because it wasn't cancelled by SAT");

                }
            }


            var setApproved = theEntityPm.SetApproved;
            var setCancelApproved = theEntityPm.SetCancelApproval;
            var setVoided = theEntityPm.SetVoided;
            var SetReSendQBO = theEntityPm.SetReSendQBO;

            //update amounts
            if (theEntityPm.IsFullAccounting == true)
                UpdateFullAccountPaymentAmount(theEntityPm, gla.ReconcileMethodCode == "0");


            // PaymentCheque And CashBook
           // this.AddARPaymentChequeAndCashBook(theEntityPm, theEntityPm.SetApproved); we do this only if it is new entity.
            this.VoidARPaymentInFullAccounting(theEntityPm, setVoided);

            this.InitializeTransferComponents();

            ARPaymentMapping.MapEntity(theEntityPm, newPayment, isNewEntity);
            paymentRepository.Update(newPayment);
            paymentRepository.SubmitChanges();
            invoicePaymentRepository.SubmitChanges();

            if (!theEntityPm.IsFullAccounting)
                UpdatePaymentOpenAmount();


            ARPaymentHelper service = new ARPaymentHelper();
            if (newPayment.ExternalAccountingEntityId != null || SetReSendQBO)
            {
                service.ARPaymentQuickbooksValidating(theEntityPm, true, false, newPayment, this.objectContext, this.myCommonContext, this.SetVoided, setCancelApproved, SetReSendQBO);
            }
            else
            {
                service.ARPaymentQuickbooksValidating(theEntityPm, setApproved, false, newPayment, this.objectContext, this.myCommonContext, this.SetVoided, setCancelApproved, SetReSendQBO);
            }
            this.BuildSearchFields();

            // DropBox
            this.CreateARInvoiceMessage(setApproved);


            paymentRepository.Update(newPayment);
            paymentRepository.SubmitChanges();

            // Full Accounting => Reconciliation
            if (theEntityPm.IsFullAccounting == true)
            {
                if (string.IsNullOrEmpty(theEntityPm.GLAccountId))
                    throw new ApplicationException("Hey! no glaccount provided!!");

                if (theEntityPm.StatusCode != "VD")
                    CreateReconciliationForARPayment(theEntityPm);
            }

            this.TraceConnected();
            this.GetPaymentForeignFields();
            this.BuildEntitiesNumbers();
        }



        private void ValidateHigherStatus()
        {
            if (!isNewEntity)
            {
                if (this.newPayment.StatusCode == "AD")
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

        private void CreateARInvoiceMessage(bool setApproved)
        {
            if (setApproved && this.isTransferToDropbox && this.TransferToDropboxActivated)
            {
                if (!string.IsNullOrEmpty(entityPM.TransferError))
                {
                    throw new ApplicationException(entityPM.TransferError);
                }
                else
                {
                    this.newPayment = paymentRepository.GetSingleARPayment(this.entityPM.Id);
                    List<ARPayment> entities = new List<ARPayment>();
                    entities.Add(this.newPayment);
                    ARPaymentMessageHelper myHelper = new ARPaymentMessageHelper(entities, this.newPayment.PaymentNo + ".xml", tenant, true);
                    myHelper.Transfer();
                }
            }
        }

        #region InitializeComponent
        private void InitializeComponent()
        {
            if (string.IsNullOrEmpty(entityPM.Id))
            {
                entityPM.Id = IdCounter.GetNumber("ARPayment", entityPM.Tenant).ToString();
            }
           
            if (!entityPM.IsExternalEntity && string.IsNullOrEmpty(entityPM.PaymentNo))
            {
                entityPM.PaymentNo = TableCounter.GetNumber(entityPM.Tenant, "ARPT", "DR", null).ToString();
            }

            if (entityPM.SetApproved)
            {
                if (entityPM.StatusCode != "AD")

                {
                    entityPM.StatusCode = "AD";
                    entityPM.ApprovedDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    entityPM.ApprovedByUserId = loggedContact.Id;
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

            if (entityPM.AccountingPaymentMethodCode == "CA")
            {
                entityPM.ValueDate = entityPM.RegisterDate;
            }
            CheckFullAccountingNumericFields(entityPM);
            tenantRepository = new TenantRepository(entityPM.Tenant);
            tenantPOCO = tenantRepository.GetSingleTenant(entityPM.Tenant);
            cashBookQuery = ContainerAccessor.Container.Resolve(typeof(ICashBookQueryServiceExt), "CashBookQueryServiceExt", new ParameterOverride("", 1)) as ICashBookQueryServiceExt;
            if (tenantPOCO.AccountingActivated)
            {
                cashBookMethodType = entityPM.AccountingPaymentMethodCode;
                if (entityPM.AccountingPaymentMethodCode == "CA")
                {
                    cashBookMethodType = "1";
                }
                else if (entityPM.AccountingPaymentMethodCode == "CH")
                {
                    cashBookMethodType = "2";
                }
                cashBook = cashBookQuery.GetByPaymentAndCurrencyAndBranch(entityPM.PaymentCurrencyId, cashBookMethodType, entityPM.BranchId, tenant);
                if (cashBook != null)
                {

                    entityPM.CashbookId = cashBook.Id;
                }
            }
        }

        #endregion

        #region PaymentInvoice
        private void CreatePaymentInvoice(ARPaymentInvoicePM item)
        {
            item.ARPaymentId = entityPM.Id;
            item.ForeignCurrencyId = entityPM.PaymentCurrencyId;
            item.Id = IdCounter.GetNumber("ARInvoicePayment", entityPM.Tenant);

            ARInvoicePayment newObject = new ARInvoicePayment();
            ARPaymentMapping.MapEntityInvoicePyament(item, newObject, true);
            invoicePaymentRepository.Add(newObject);
        }

        private void UpdatePaymentInvoice(ARPaymentInvoicePM item)
        {
            ARInvoicePayment invoicePayment = invoicePaymentRepository.GetSingleARInvoicePayment(item.Id, entityPM.Tenant);

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
        #endregion

        #region Update Amounts
        private void UpdatePaymentOpenAmount()
        {
            bool isClosed = entityPM.IsClosed;
            string StatusCode = entityPM.StatusCode;
            double? Amount = MethodHelper.Roundd(entityPM.AmountInPaymentCurrency, 2);
            double? PaidAmount = 0;
            double? OpenAmount = 0;

            List<ARInvoicePayment> allConnectedItems = invoicePaymentRepository.GetARInvoicePaymentByPaymentId(entityPM.Id, entityPM.Tenant).ToList();

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

            newPayment.IsClosed = entityPM.IsClosed = isClosed;
            newPayment.StatusCode = entityPM.StatusCode = StatusCode;
            newPayment.OpenAmount = entityPM.OpenAmount = OpenAmount;
            newPayment.AmountInPaymentCurrency = entityPM.AmountInPaymentCurrency = Amount;

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
                ARInvoice invoice = this.GetInvoice(myInvoiceId, tenant);

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
        #endregion

        #region SearchFields
        private void BuildSearchFields()
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.PaymentNo);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.StatusCode);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.AccountingPaymentMethodCode);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ChequeOrPaymentRef);

            #region Card
            if (!string.IsNullOrEmpty(entityPM.BillToId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.BillToId, tenant, true);
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
                foreach (ARPaymentInvoicePM item in entityPM.PaymentInvoices)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, item.ARInvoiceNumber);
                }
            }
            #endregion

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.InternalNotes);

            entityPM.SearchFields = mySearchFields;
            newPayment.SearchFields = mySearchFields;
        }
        #endregion

        #region InternalAccountingSystem
        private static bool IsInternalAccountingSystem(int tenant)
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
                    //    Id = IdCounter.GetNumber("Customs.Tapag", entityPM.Tenant),
                    //    Tenant = entityPM.Tenant,
                    //    CustomerId = entityPM.CustomerId,
                    //    CustomsBranchCode = entityPM.CustomsBranchCode,
                    //    ImporterId = entityPM.ImporterId,
                    //    FollowDate = entityPM.FollowDate,
                    //    IsClosed = entityPM.IsClosed,
                    //    LeadingFileNumber = entityPM.LeadingFileNumber,
                    //    ProfessionUnitTypeCode = entityPM.ProfessionUnitTypeCode,
                    //    SpecializationTypeCode = entityPM.SpecializationTypeCode,
                    //    TapagNumber = entityPM.TapagNumber,
                    //    TapagTypeCode = entityPM.TapagTypeCode,
                    //    ValidityDate = entityPM.ValidityDate,
                    //    CreateDate = entityPM.CreateDate,
                    //    ChangeSetOp = ChangeSetOperation.Insert,
                    //};

                    //TapagUpdateService tapagUpdate = new TapagUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), entityPM.Tenant);
                    //tapagUpdate.Update(tapag, false);
                    //entityPM.TapagId = tapag.Id;


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
                                    Tenant = entityPM.Tenant,
                                    EventTypeCode = "COAR",
                                    UserId = loggedContact.Id,
                                    EntityId = item.ARInvoiceId,
                                    ObjectTableName = "ARInvoice",
                                    Notes = "Connected with Payment: " + entityPM.PaymentNo + " with Amount Due equals to: " + invoice.AmountDue,
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
                                    Tenant = entityPM.Tenant,
                                    EventTypeCode = "ARID",
                                    UserId = loggedContact.Id,
                                    EntityId = item.ARInvoiceId,
                                    ObjectTableName = "ARInvoice",
                                    Notes = "Disconnected from Payment: " + entityPM.PaymentNo,
                                });
                            }

                            break;
                        }

                    default: { break; }
                }
            }
        }

        #region ARPaymentChequeAndCashBook
        private void AddARPaymentChequeAndCashBook(ARPaymentPM theEntityPm, bool setApproved)
        {
            int tenant = theEntityPm.Tenant;
            ARPaymentChequePM arPaymentcheque = null;
            if (setApproved || theEntityPm.IsExternalEntity)
            {
                if (tenantPOCO != null && tenantPOCO.AccountingActivated)
                {
                    if (theEntityPm.AccountingPaymentMethodCode == "CH" || theEntityPm.AccountingPaymentMethodCode == "CA")
                    {
                        if (cashBook != null)
                        {
                            if (cashBookMethodType == "1")
                            {
                                // Update Total Amount
                                ICashBookUpdateServiceExt cashBookUpdate = ContainerAccessor.Container.Resolve(typeof(ICashBookUpdateServiceExt), "CashBookUpdateServiceExt", new ParameterOverride("", 1)) as ICashBookUpdateServiceExt;
                                if (cashBook.TotalAmount == null)
                                {
                                    cashBook.TotalAmount = 0;
                                }
                                cashBook.TotalAmount += (decimal)theEntityPm.AmountInPaymentCurrency;
                                cashBook.ChangeSetOp = ChangeSetOperation.Update;
                                cashBookUpdate.Update(cashBook);
                            }
                            else
                            {
                                arPaymentcheque = new ARPaymentChequePM();
                                if (theEntityPm.ARPaymentChequeReplicas.Count > 0)
                                {
                                    int LineNumberCounter = 1;
                                    if (!isNewEntity)
                                    {
                                        LineNumberCounter = theEntityPm.ARPaymentChequeReplicas.Max(d => d.LineNumber)+1;
                                    }
                                    DateTime? valueDate=null;
                                    foreach (ARPaymentChequeReplicaPM item in theEntityPm.ARPaymentChequeReplicas)
                                    {
                                        arPaymentcheque = new ARPaymentChequePM();
                                       
                                        if (LineNumberCounter == 1)
                                        {
                                            valueDate = item.ValueDate;
                                            theEntityPm.ValueDate = valueDate;
                                        }

                                        if(valueDate!= null && valueDate!= item.ValueDate)
                                        {
                                            throw new ApplicationException("value date should be the same for all payment cheques");
                                        }
                                        bool exist = CheckIfPaymentChequeExist(item.ChequeNumber, item.LineNumber, theEntityPm);
                                        if (!exist)
                                        {

                                            arPaymentcheque.PaymentId = theEntityPm.Id;
                                            arPaymentcheque.Tenant = tenant;
                                            arPaymentcheque.LineNumber = LineNumberCounter++;
                                            arPaymentcheque.ChequeNumber = item.ChequeNumber;
                                            arPaymentcheque.ValueDate = item.ValueDate;
                                            arPaymentcheque.BankBranch = item.BankBranch;
                                            arPaymentcheque.BankAccount = item.BankAccount;
                                            //arPaymentcheque.BankId = getBankCode(theEntityPm.Tenant);// "1-1";
                                            arPaymentcheque.BankId = item.BankId;
                                            arPaymentcheque.CurrencyId = theEntityPm.PaymentCurrencyId;
                                            arPaymentcheque.LocalAmount = (decimal)item.LocalAmount;
                                            arPaymentcheque.ForeignAmount = (decimal)item.ForeignAmount;
                                            arPaymentcheque.ChangeSetOp = ChangeSetOperation.Insert;
                                            arPaymentcheque.StatusCode = "1"; // In Cashbook  
                                            arPaymentcheque.ExchangeRate = (decimal)theEntityPm.PaymentCurrencyExchangeRate;
                                            arPaymentcheque.PaymentNumber = theEntityPm.PaymentNo;

                                            IARPaymentChequeUpdateServiceExt paymentUpdate = ContainerAccessor.Container.Resolve(typeof(IARPaymentChequeUpdateServiceExt), "ARPaymentChequeUpdateServiceExt", new ParameterOverride("", 1)) as IARPaymentChequeUpdateServiceExt;

                                            paymentUpdate.Update(arPaymentcheque);
                                            CreateCashBook(arPaymentcheque);
                                        }
                                        // Create Journal with lines for cash or cheque
                                        // this.CreateARPaymentChequeJournals(theEntityPm, arPaymentcheque);

                                    }
                                }
                                else
                                {
                                    bool exist = CheckIfPaymentChequeExist(theEntityPm.ChequeOrPaymentRef, 1, theEntityPm);
                                    if (!exist)
                                    {
                                        arPaymentcheque.PaymentId = theEntityPm.Id;
                                        arPaymentcheque.Tenant = tenant;
                                        arPaymentcheque.LineNumber = 1;
                                        arPaymentcheque.ChequeNumber = theEntityPm.ChequeOrPaymentRef;
                                        arPaymentcheque.ValueDate = theEntityPm.ValueDate.Value;
                                        arPaymentcheque.BankBranch = theEntityPm.BankBranch;
                                        arPaymentcheque.BankAccount = theEntityPm.Account;
                                        //arPaymentcheque.BankId = getBankCode(theEntityPm.Tenant);// "1-1";
                                        arPaymentcheque.BankId = theEntityPm.Bank;
                                        arPaymentcheque.CurrencyId = theEntityPm.PaymentCurrencyId;
                                        arPaymentcheque.LocalAmount = (decimal)theEntityPm.AmountInLocalCurrency.Value;
                                        arPaymentcheque.ForeignAmount = (decimal)theEntityPm.AmountInPaymentCurrency.Value;
                                        arPaymentcheque.ChangeSetOp = ChangeSetOperation.Insert;
                                        arPaymentcheque.StatusCode = "1"; // In Cashbook  
                                        arPaymentcheque.ExchangeRate = (decimal)theEntityPm.PaymentCurrencyExchangeRate;
                                        arPaymentcheque.PaymentNumber = theEntityPm.PaymentNo;

                                        IARPaymentChequeUpdateServiceExt paymentUpdate = ContainerAccessor.Container.Resolve(typeof(IARPaymentChequeUpdateServiceExt), "ARPaymentChequeUpdateServiceExt", new ParameterOverride("", 1)) as IARPaymentChequeUpdateServiceExt;
                                        paymentUpdate.Update(arPaymentcheque);
                                        CreateCashBook(arPaymentcheque);
                                    }
                                    // Create Journal with lines for cash or cheque
                                    //   this.CreateARPaymentChequeJournals(theEntityPm, arPaymentcheque);
                                }

                                if (theEntityPm.IsExternalEntity && theEntityPm.ARPaymentChequeReplicas.Count() >0 )
                                {


                                    MapPaymentChequeFieldsToPayment(theEntityPm.ARPaymentChequeReplicas.First(), theEntityPm);
                                }
                            }
                            this.CreateARPaymentChequeJournals(theEntityPm, arPaymentcheque);
                        }
                    }
                    else
                    {
                        if (isNewEntity)
                        {
                            // Create Journal with lines for bank transfer or credit card  
                            this.CreateARPaymentChequeJournals(theEntityPm, arPaymentcheque);
                        }
                    }
                }
            }
        }

        private bool CheckIfPaymentChequeExist(string chequeNumber, int lineNumber, ARPaymentPM payment)
        {
            ARPaymentChequeReplicaQuery aRPaymentChequeReplicaQuery = new ARPaymentChequeReplicaQuery(payment.Tenant);
            return aRPaymentChequeReplicaQuery.ChequeIfPaymentChequeReplicaExist(payment.Id, chequeNumber, lineNumber, payment.Tenant);


        }
        private void MapPaymentChequeFieldsToPayment(ARPaymentChequeReplicaPM paymentCheque, ARPaymentPM payment)
        {
            payment.Bank = paymentCheque.BankId;


        }
        private void CreateCashBook(ARPaymentChequePM aRPaymentCheque)
        {
            CashBookLinePM cashBookLine = new CashBookLinePM();
            cashBookLine.CashBookId = cashBook.Id;
            cashBookLine.Tenant = tenant;
            cashBookLine.ARPChequeId = aRPaymentCheque.Id;
            cashBookLine.ChangeSetOp = ChangeSetOperation.Insert;
            cashBookLine.IsDeposited = false;
            cashBookLine.ChequeNumber = aRPaymentCheque.ChequeNumber;
            cashBookLine.Bank = aRPaymentCheque.BankAccount;

            ICashBookLineUpdateServiceExt cashBookLineUpdate = ContainerAccessor.Container.Resolve(typeof(ICashBookLineUpdateServiceExt), "CashBookLineUpdateServiceExt", new ParameterOverride("", 1)) as ICashBookLineUpdateServiceExt;
            cashBookLineUpdate.Update(cashBookLine);

            // Update Total Amount
            ICashBookUpdateServiceExt cashBookUpdate = ContainerAccessor.Container.Resolve(typeof(ICashBookUpdateServiceExt), "CashBookUpdateServiceExt", new ParameterOverride("", 1)) as ICashBookUpdateServiceExt;
            if (cashBook.TotalAmount == null)
            {
                cashBook.TotalAmount = 0;
            }
            cashBook.TotalAmount += aRPaymentCheque.ForeignAmount;
            cashBook.ChangeSetOp = ChangeSetOperation.Update;
            cashBookUpdate.Update(cashBook);


        }

        private void CreateARPaymentChequeJournals(ARPaymentPM paymentPM, ARPaymentChequePM arPaymentcheque)
        {
            int counter = 0;
            // Insert Journal 
            JournalPM journal = new JournalPM();
            journal.Tenant = tenant;
            journal.JournalNumber = "1";
            journal.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            journal.AccountingDate = paymentPM.RegisterDate.Value;
            journal.TypeCode = "0";
            journal.StatusCode = "2";
            journal.CreatedByUserId = paymentPM.CreatedByUserId;
            journal.AccountingEntityCode = "3";
            journal.AccountingEntityId = paymentPM.Id;
            journal.AccountingEntityReference = paymentPM.PaymentNo;
            journal.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            journal.UpdatedByUserId = paymentPM.UpdatedByUserId;
            journal.ApproveDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            journal.ApprovedByUserId = paymentPM.UpdatedByUserId;
            journal.ChangeSetOp = ChangeSetOperation.Insert;

            // Insert Journal Lines 
            // [Credit]
            GLAccountPM glAccount = getGLAccount(paymentPM.BillToId, paymentPM.Tenant);
            JournalLinePM journalLine = new JournalLinePM();


            journalLine.Tenant = tenant;
            journalLine.JournalId = journal.Id;
            journalLine.Line = ++counter;
            journalLine.ActionCode = "1";//- Credit
            journalLine.ActionTypeCodeEnum = MyJournalActionTypeEnum.Credit;
            journalLine.CreditAccountId = glAccount != null ? glAccount.Id : null;
            journalLine.DocumentDate = paymentPM.RegisterDate.Value;
            journalLine.AccountingDate = paymentPM.RegisterDate.Value;
            journalLine.DueDate = arPaymentcheque != null ? arPaymentcheque.ValueDate : paymentPM.ValueDate.Value;
            journalLine.LocalAmount = (decimal)paymentPM.AmountInLocalCurrency;
            journalLine.CurrencyId = paymentPM.PaymentCurrencyId;
            journalLine.ForeignAmount = (decimal)paymentPM.AmountInPaymentCurrency;
            journalLine.ExchangeRate = (decimal)paymentPM.PaymentCurrencyExchangeRate;
            journalLine.Reference1 = paymentPM.PaymentNo;
            if (paymentPM.AccountingPaymentMethodCode == "BT")
            {
                journalLine.Reference2 = paymentPM.ChequeOrPaymentRef;
            }
            else
            {
                if (paymentPM.ARPaymentChequeReplicas.Count > 0)
                {
                    string reference = "";
                    int count = 0;
                    foreach (ARPaymentChequeReplicaPM cheque in paymentPM.ARPaymentChequeReplicas)
                    {
                        count++;
                       if(count == paymentPM.ARPaymentChequeReplicas.Count())
                        {
                            reference = reference + cheque.ChequeNumber;
                        }
                        else
                        {
                            reference = reference + cheque.ChequeNumber + ",";
                        }
                      
                    }

                    journalLine.Reference2 = reference;
                }
                else {
                    journalLine.Reference2 = arPaymentcheque != null ? arPaymentcheque.ChequeNumber : paymentPM.ChequeOrPaymentRef;
                }
            }

            if (paymentPM.AccountingPaymentMethodCode == "CH")
                journalLine.Notes = paymentPM.PrintNotes;
            else
                journalLine.Notes = paymentPM.InternalNotes;

            journalLine.ChangeSetOp = ChangeSetOperation.Insert;
            journal.JournalLines.Add(journalLine);

            // [Debit- Cheque] 



            if (paymentPM.AccountingPaymentMethodCode == "CH")
            {
                if (paymentPM.ARPaymentChequeReplicas.Count > 0)
                {
                    journal = CreateDebitJournalLine(paymentPM, journal, counter);

                }
                else
                {
                    journalLine = new JournalLinePM();
                    journalLine.Tenant = tenant;
                    journalLine.JournalId = journal.Id;
                    journalLine.Line = ++counter;
                    journalLine.ActionCode = "2";
                    journalLine.ActionTypeCodeEnum = MyJournalActionTypeEnum.Debit;
                    journalLine.DocumentDate = paymentPM.RegisterDate.Value;
                    journalLine.AccountingDate = paymentPM.RegisterDate.Value;
                    journalLine.DueDate = arPaymentcheque.ValueDate;
                    journalLine.LocalAmount = arPaymentcheque.LocalAmount;
                    journalLine.CurrencyId = arPaymentcheque.CurrencyId;
                    journalLine.ForeignAmount = arPaymentcheque.ForeignAmount;
                    journalLine.ExchangeRate = arPaymentcheque.ExchangeRate;
                    journalLine.Reference1 = arPaymentcheque.PaymentNumber;
                    journalLine.Reference2 = arPaymentcheque.ChequeNumber;
                    journalLine.DebitAccountId = cashBook.AccountId;
                    journalLine.CreditAccountId = glAccount != null ? glAccount.Id : null;
                    journalLine.ChangeSetOp = ChangeSetOperation.Insert;
                    journalLine.Notes = paymentPM.InternalNotes;
                    journal.JournalLines.Add(journalLine);
                }
            }
            // [Debit- Cash Book, Bank Transfer, Credit Card] 
            else
            {
                journalLine = new JournalLinePM();
                journalLine.Tenant = tenant;
                journalLine.JournalId = journal.Id;
                journalLine.Line = ++counter;
                journalLine.ActionCode = "2";
                journalLine.ActionTypeCodeEnum = MyJournalActionTypeEnum.Debit;
                journalLine.DocumentDate = paymentPM.RegisterDate.Value;
                journalLine.AccountingDate = paymentPM.RegisterDate.Value;
                journalLine.DueDate = paymentPM.ValueDate.Value;
                journalLine.LocalAmount = (decimal)paymentPM.AmountInLocalCurrency;
                journalLine.CurrencyId = paymentPM.PaymentCurrencyId;
                journalLine.ForeignAmount = (decimal)paymentPM.AmountInPaymentCurrency;
                journalLine.ExchangeRate = (decimal)paymentPM.PaymentCurrencyExchangeRate;
                journalLine.Reference1 = paymentPM.PaymentNo;
                journalLine.Reference2 = paymentPM.ChequeOrPaymentRef;
                journalLine.DebitAccountId = this.getGLAccountByPaymentMethodCode(paymentPM);
                journalLine.CreditAccountId = glAccount != null ? glAccount.Id : null;
                journalLine.ChangeSetOp = ChangeSetOperation.Insert;
                journal.JournalLines.Add(journalLine);
            }




            if (paymentPM.PaymentInvoices.Any())
            {
                var AutoReconcileARPaymentServiceExt = ContainerAccessor.Container.Resolve(typeof(IAutoReconcileServiceExt), "AutoReconcileServiceExt", new ParameterOverride("", 1)) as IAutoReconcileServiceExt;


                var AutoReconcileRecordList = new List<AutoReconcileRecord>();
                paymentPM.PaymentInvoices.ForEach(r =>
                {
                    //JournalPM invoiceJournal=  GetJournalByInvoiceNumber(r);
                    var item = new AutoReconcileRecord()
                    {
                        AccountingEntityId = r.ARInvoiceId,
                        LocalAmountToReconcile = Convert.ToDecimal(r.LocalAmount.GetValueOrDefault()),
                        ForeignAmountToReconcile = Convert.ToDecimal(r.ForeignAmount.GetValueOrDefault()),
                        ForeignCurrencyIdReconcile = r.ForeignCurrencyId,


                    };
                    AutoReconcileRecordList.Add(item);
                }
            );
                AutoReconcileARPaymentServiceExt.InitMust(glAccount, journal, AutoReconcileRecordList);
                AutoReconcileARPaymentServiceExt.InsertJournalReconcile();
            }

            IJournalUpdateServiceExt journalUpdate = ContainerAccessor.Container.Resolve(typeof(IJournalUpdateServiceExt), "JournalUpdateServiceExt", new ParameterOverride("", 1)) as IJournalUpdateServiceExt;
            journalUpdate.Update(journal);
        }

        private JournalPM CreateDebitJournalLine(ARPaymentPM payment, JournalPM journal, int counter)
        {

            JournalLinePM journalLine;
            GLAccountPM glAccount = getGLAccount(payment.BillToId, payment.Tenant);
            foreach (ARPaymentChequeReplicaPM cheque in payment.ARPaymentChequeReplicas)
            {
                journalLine = new JournalLinePM();
                journalLine.Tenant = tenant;
                journalLine.JournalId = journal.Id;
                journalLine.Line = ++counter;
                journalLine.ActionCode = "2";
                journalLine.ActionTypeCodeEnum = MyJournalActionTypeEnum.Debit;
                journalLine.DocumentDate = payment.RegisterDate.Value;
                journalLine.AccountingDate = payment.RegisterDate.Value;
                journalLine.DueDate = cheque.ValueDate;
                journalLine.LocalAmount = cheque.LocalAmount;
                journalLine.CurrencyId = payment.PaymentCurrencyId;
                journalLine.ForeignAmount = cheque.ForeignAmount;
                journalLine.ExchangeRate = (decimal)payment.PaymentCurrencyExchangeRate;
                journalLine.Reference1 = payment.PaymentNo;
                journalLine.Reference2 = cheque.ChequeNumber;
                journalLine.DebitAccountId = cashBook.AccountId;
                journalLine.CreditAccountId = glAccount != null ? glAccount.Id : null;
                journalLine.ChangeSetOp = ChangeSetOperation.Insert;
                journalLine.Notes = payment.InternalNotes;
                journal.JournalLines.Add(journalLine);
            }
            return journal;
        }






        private JournalPM GetJournalByInvoiceNumber(ARPaymentInvoicePM paymentInvoice)
        {
            ARInvoiceQuery invoiceQuery = new ARInvoiceQuery(paymentInvoice.Tenant);
            ARInvoicePM invoicePM = invoiceQuery.GetSingleInvoiceByInvoiceNumber(paymentInvoice.ARInvoiceNumber, paymentInvoice.Tenant);

            return null;

        }

        private GLAccountPM getGLAccount(string billToId, int tenant)
        {
            GLAccountPM glaAccount = null;
            CardRepository cardRep = new CardRepository(tenant);
            Card card = cardRep.GetSingleCard(billToId, tenant);
            if (card != null)
            {
                IGLAccountQueryServiceExt glAccountQuery = ContainerAccessor.Container.Resolve(typeof(IGLAccountQueryServiceExt), "GLAccountQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountQueryServiceExt;
                glaAccount = glAccountQuery.GetSingleGLAccountPM(card.GLAccountId, tenant);
            }

            return glaAccount;
        }

        private string getGLAccountByPaymentMethodCode(ARPaymentPM entityPm)
        {
            string glaAccountId = "";

            if (entityPm.AccountingPaymentMethodCode == "CA")
            {
                glaAccountId = cashBook.AccountId;
            }
            else if (entityPm.AccountingPaymentMethodCode == "CC")
            {
                CreditCardTypeQuery creditCardQuery = new CreditCardTypeQuery(entityPm.Tenant);
                CreditCardTypePM creditCard = new CreditCardTypePM();
                creditCard = creditCardQuery.GetSinglePM(entityPm.CreditCardTypeId, entityPm.Tenant);
                if (creditCard != null)
                {
                    IBankAccountQueryServiceExt bankAccountQuery = ContainerAccessor.Container.Resolve(typeof(IBankAccountQueryServiceExt), "BankAccountQueryServiceExt", new ParameterOverride("", 1)) as IBankAccountQueryServiceExt;
                    BankAccountPM bankAccount = bankAccountQuery.GetByFirstOrDefault(creditCard.BankAccountId, entityPm.Tenant);
                    if (bankAccount != null)
                    {
                        glaAccountId = bankAccount.GLAccountId;
                    }
                }
            }
            else if (entityPm.AccountingPaymentMethodCode == "BT")
            {
                IBankAccountQueryServiceExt bankAccountQuery = ContainerAccessor.Container.Resolve(typeof(IBankAccountQueryServiceExt), "BankAccountQueryServiceExt", new ParameterOverride("", 1)) as IBankAccountQueryServiceExt;
                BankAccountPM bankAccount = bankAccountQuery.GetByFirstOrDefault(entityPm.BankAccountId, entityPm.Tenant);
                if (bankAccount != null)
                {
                    glaAccountId = bankAccount.GLAccountId;
                }
            }
            return glaAccountId;
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
            if (newPayment.StatusCode != null)
            {
                ARPaymentStatusRepository myRepository = new ARPaymentStatusRepository(objectContext);
                ARPaymentStatus status = myRepository.GetSingleARPaymentStatus(newPayment.StatusCode);
                entityPM.StatusName = status.Name;
            }

            if (newPayment.TransferStatusCode != null)
            {
                ARPaymentTransferStatusRepository myRepository = new ARPaymentTransferStatusRepository(objectContext);
                ARPaymentTransferStatus status = myRepository.GetSingleARPaymentTransferStatus(newPayment.TransferStatusCode);
                entityPM.TransferStatusName = status.Name;
            }

        }
        private void BuildEntitiesNumbers()
        {
            string invoiceNumber = null;
            string shipmentNumber = null;
            List<ARPaymentInvoicePM> invoicesEntities = entityPM.PaymentInvoices.Where(d => d.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete).ToList();
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

            if (entityPM.InvoiceNumber != invoiceNumber || entityPM.ShipmentNumber != shipmentNumber)
            {
                entityPM.InvoiceNumber = invoiceNumber;
                entityPM.ShipmentNumber = shipmentNumber;
                newPayment.InvoiceNumber = entityPM.InvoiceNumber;
                newPayment.ShipmentNumber = entityPM.ShipmentNumber;
                paymentRepository.Update(newPayment);
                paymentRepository.SubmitChanges();
            }
        }

        #region Transfer 
        private void InitializeTransferComponents()
        {
            this.InitializeTransferFields();
            if (this.entityPM.SetApproved && this.isTransferToDropbox && this.TransferToDropboxActivated && string.IsNullOrEmpty(entityPM.TransferError))
            {
                this.entityPM.TransferStatusCode = "TR";
            }
        }
        private void InitializeTransferFields()
        {
            bool isInitializing = true;

            if (entityPM.SetReTransfer)
            {
                isInitializing = true;
            }

            else if (newPayment.TransferStatusCode == "TR")
            {
                isInitializing = false;
                entityPM.TransferError = null;
                entityPM.TransferStatusCode = "TR";
            }

            else if (newPayment.TransferStatusCode == "IP")
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
                CardRepository cardRep = new CardRepository(newPayment.Tenant);
                CurrencyRepository currencyRep = new CurrencyRepository(newPayment.Tenant);
                Card card = cardRep.GetSingleCard(newPayment.BillToId, newPayment.Tenant);
                Currency currency = currencyRep.GetSingleCurrency(newPayment.PaymentCurrencyId, newPayment.Tenant);
                string currencyError = "Currency External Id is missing";
                if (currency != null && !string.IsNullOrEmpty(currency.Code))
                {
                    currencyError = "Currency: " + currency.Code + ". External Id is missing";
                }
                if (card != null && FieldIsEmpty(card.ReceivablesAccountingCard))
                {
                    isReady = false;
                    myError = "Bill To: " + entityPM.BillToName + ". External Id is missing";
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

        private void UpdateDocOutNeedsRebuild()
        {
            DocumentOutRepository documentOutRepository = new DocumentOutRepository(myCommonContext);
            DocumentOut docOut = documentOutRepository.GetDocumentOutByEntityAndChildEntity(newPayment.Id, null);
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
                            ICashBookQueryServiceExt cashQuery = ContainerAccessor.Container.Resolve(typeof(ICashBookQueryServiceExt), "CashBookQueryServiceExt", new ParameterOverride("", 1)) as ICashBookQueryServiceExt;
                            CashBookPM cashBook = cashQuery.GetByPaymentAndCurrencyAndBranch(entityPm.PaymentCurrencyId, "1", entityPm.BranchId, tenant);
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
                                    cashBook.TotalAmount += (decimal)entityPm.AmountInPaymentCurrency;
                                    cashBook.ChangeSetOp = ChangeSetOperation.Update;
                                    cashBookUpdate.Update(cashBook);
                                    CreateVoidedARPaymentEvent("ARPayment Cancel");
                                    CancelJournal();
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
                                    CancelJournal();
                                }
                            }
                        }
                        else if (entityPm.AccountingPaymentMethodCode == "BT")
                        {
                            CreateVoidedARPaymentEvent("ARPayment Cancel");
                            CancelJournal();
                        }
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
            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                Tenant = entityPM.Tenant,
                EventTypeCode = "CAAR",
                UserId = loggedContact.Id,
                EntityId = entityPM.Id,
                ObjectTableName = "ARPayment",
                Notes = note,
            });
        }
        private void CancelJournal()
        {

            IJournalQueryServiceExt journalQuery = ContainerAccessor.Container.Resolve(typeof(IJournalQueryServiceExt), "JournalQueryServiceExt", new ParameterOverride("", 1)) as IJournalQueryServiceExt;
            JournalPM journalPM = journalQuery.GetJournalByAccountingEntityIdAndCode(entityPM.Id, "3", entityPM.Tenant);
            if (journalPM != null)
            {
                //journalPM.AccountingEntityReference = entityPM.PaymentNo;
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
                    AccountingEntityId = entityPM.Id,
                    AccountingEntityReference = entityPM.PaymentNo
                });
            }
        }

        #region Full Accounting
        public void CreateReconciliationForARPayment(ARPaymentPM paymentPM)
        {
            IAccountingContext ctx = AccountingContext.GetContext(paymentPM.Tenant);
            ReconciliationPM _reco = new ReconciliationPM();
            _reco.ChangeSetOp = ChangeSetOperation.Insert;
            _reco.Number = "get";
            _reco.Tenant = paymentPM.Tenant;
            _reco.AccountId = paymentPM.GLAccountId;
            _reco.AccountReconcileMethodCode = paymentPM.GLAccountRecoMethodCode;
            _reco.CreateDate = TenantServerConfigration.GetCurrentDateTime(paymentPM.Tenant);

            GLAccountListQueryService glaQuery = new GLAccountListQueryService(ctx);
            GLAccountList gla = glaQuery.GetByAccountId(paymentPM.GLAccountId, paymentPM.Tenant);
            if (gla != null)
            {
                _reco.AccountCurrencyId = gla.CurrencyId;
                _reco.CurrencyCode = gla.CurrencyCode;
            }

            // get payment line LT
            LedgerTransactionListQueryService ltListQuery = new LedgerTransactionListQueryService(ctx);
            List<LedgerTransactionList> accountingTransactionList = ltListQuery.GetByAccountId(paymentPM.GLAccountId, paymentPM.Tenant);
            LedgerTransactionList paymentTransaction = accountingTransactionList.Where(d => d.SourceNumber == paymentPM.PaymentNo).FirstOrDefault(); // 3- ARPayment
            if (paymentTransaction == null) throw new ApplicationException("Cannot find ledger transaction for this payment!");

            if (paymentPM.InvoicesLedgerTransactions.Count == 0)
                return;

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
            GLAccountPM gla = getGLAccount(paymentPM.BillToId, paymentPM.Tenant);
            if (gla != null)
            {
                paymentPM.GLAccountId = gla.Id;
                paymentPM.GLAccountRecoMethodCode = gla.ReconcileMethodCode;
            }
            return gla;
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
                        ForeignCurrencyId = invTrans.CurrencyId
                    };
                }
                else
                {
                    payInvPM = new ARPaymentInvoicePM()
                    {
                        ARInvoiceId = invTrans.SourceId,
                        LocalAmount = Convert.ToDouble(invTrans.AmountToReconcile * invTrans.ExchangeRate),
                        ForeignAmount = (double)invTrans.AmountToReconcile,
                        ForeignCurrencyId = invTrans.CurrencyId
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

        private void CheckLinesAmountToReconcileTotal(ARPaymentPM _payment)
        {

            ContactPM loggedContact = GetLoggedContactPM(_payment.Tenant);
            bool showLocal = loggedContact != null ? (!loggedContact.DontShowLocal) : false;


            decimal amount2reconcile = GetAmountToReconcileTotalFromPaymentInvoices(_payment);

            //decimal amount2reconcile = _payment.InvoicesLedgerTransactions.Sum(d => d.AmountToReconcile);
            if (_payment.OpenAmount == null)
            {
                _payment.OpenAmount = 0;
            }

            if (amount2reconcile > (decimal)_payment.OpenAmount)
                throw new ApplicationException(TextCodesTranslator.TranslateText("Accounting.O.ARP.paymentAmount2reconcileMSG", _payment.Tenant, showLocal));
        }

        private static decimal GetAmountToReconcileTotalFromPaymentInvoices(ARPaymentPM _payment)
        {
            bool useLocalRecoMethod = _payment.GLAccountRecoMethodCode == "0";
            decimal amount2reconcile = 0;

            if (useLocalRecoMethod)
                amount2reconcile = (decimal)_payment.PaymentInvoices.Sum(d => d.LocalAmount);   // amount to reconcile = Local Amount
            else
                amount2reconcile = (decimal)_payment.PaymentInvoices.Sum(d => d.ForeignAmount); // amount to reconcile = Foreign Amount
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
