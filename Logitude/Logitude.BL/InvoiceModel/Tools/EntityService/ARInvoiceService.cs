using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Mocks;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Mocks;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.Tools.DataMapping;
using Logitude.BL.InvoiceModel.Tools.TraceEvents;
using Logitude.BL.InvoiceModel.Tools.Validating;
using Simplog.Data.InvoiceModel.Mocks;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.Helpers;
using Logitude.BL.CommonDataModel.Tools.EmailAlerts;
using System.Web;
using Logitude.BL.Security;
using Logitude.Server.Tools.Helpers;
using Logitude.SystemLogs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Logitude.Accounting.Data.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.CommonDataModel;
using Logitude.BL.InvoiceModel.EntityOtherServices;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using System.Text;
using System.IO;
using Logitude.BL.Resolvers;
using Logitude.BL.ExternalService;
using Logitude.BL.InvoiceModel.CloseTables;
using Logitude.BL.InvoiceModel.Tools.Behaviours.ARInvoiceBehaviours;

namespace Logitude.BL.InvoiceModel.Tools.EntityService
{
    public class ARInvoiceService
    {
        private const string InvoiceAutoCreditStatus = "AC";
        private const string InvoiceAlreadyReconciledMessage = "One or more invoices ledger transactions have been already reconciled";
        private int tenant;
        private bool isNewEntity;
        private bool isUpdateTotalVats;
        private bool isVoidingInvoice;
        private bool isApprovingInvoice;
        private bool isAutoCreditingInvoice;
        public ARInvoice invoice { get; set; }
        private ARInvoicePM entityPM;
        private string loggedContactId;
        private string loggedContactName;
        private IInvoiceContext objectContext;
        private ICommonDataContext myCommonContext;
        private IShipmentsContext myShipmentContext;
        private ARInvoiceRepository invoiceRepository;
        private ARInvoiceLineRepository invoiceLineRepository;
        private ARInvoiceTotalVATRepository invoiceTotalVatRepository;
        private ARInvoiceEntityRepository invoiceEntityRepository;
        private ARInvoicePaymentRepository invoicePaymentRepository;
        private ARPaymentRepository paymentRepository;
        private VatTypeRepository vatTypeRepository;
        private AccountingSettingRepository accountingSettingRepository;
        private AccountingSystemRepository accountingSystemRepository;
        private ContactRepository contactRepository;
        private List<string> allShipmentIds;
        private List<string> allActiveShipmentIds;
        private List<Shipment> allShipments;
        private List<ShipmentReceivable> allReceivables;
        private ShipmentRepository shipmentRepository;
        private ShipmentReceivableRepository shipmentReceivableRepository;
        private ARInvoiceChargesConstraintRepository ARInvoiceChargesConstraintRepository;
        SATInterfaceHelper sATInterfaceHelper;
        AccountingSetting accountingSetting;
        private Tenant TenantObject;
        private string QBOARPaymentId;
        List<VATTypesGroup> allVatGroups;
        public ARInvoiceService(IInvoiceContext objectContext, int tenant)
        {
            this.sATInterfaceHelper = new SATInterfaceHelper();
            this.tenant = tenant;
            this.isUpdateTotalVats = false;
            this.objectContext = objectContext;
            this.myCommonContext = CommonDataContext.GetContext(tenant);
            this.myShipmentContext = ShipmentsContext.GetContext(tenant);
 
            this.invoiceRepository = new ARInvoiceRepository(objectContext);
            this.invoiceLineRepository = new ARInvoiceLineRepository(objectContext);
            this.invoiceTotalVatRepository = new ARInvoiceTotalVATRepository(objectContext);
            this.invoiceEntityRepository = new ARInvoiceEntityRepository(objectContext);
            this.invoicePaymentRepository = new ARInvoicePaymentRepository(objectContext);
            this.paymentRepository = new ARPaymentRepository(objectContext);
            this.ARInvoiceChargesConstraintRepository = new ARInvoiceChargesConstraintRepository(objectContext);

            this.vatTypeRepository = new VatTypeRepository(myCommonContext);
            this.accountingSettingRepository = new AccountingSettingRepository(myCommonContext);
            this.accountingSystemRepository = new AccountingSystemRepository(myCommonContext);
            this.contactRepository = new ContactRepository(myCommonContext);
 
            allShipments = new List<Shipment>();
            allReceivables = new List<ShipmentReceivable>();
            shipmentRepository = new ShipmentRepository(myShipmentContext);
            shipmentReceivableRepository = new ShipmentReceivableRepository(myShipmentContext);            

            this.TenantObject = (from d in myCommonContext.Tenants where d.Id == tenant select d).FirstOrDefault();
            this.allVatGroups = (from d in myCommonContext.VATTypesGroups where d.Tenant == this.tenant select d).ToList();
            this.GetAccountingSystem();
        }
        string loggedUserEmail;
        public ARInvoiceService(IInvoiceContext objectContext, int tenant, string loggedUserEmail)
        {
            this.sATInterfaceHelper = new SATInterfaceHelper();
            this.tenant = tenant;
            this.isUpdateTotalVats = false;
            this.objectContext = objectContext;
            this.myCommonContext = CommonDataContext.GetContext(tenant);
            this.myShipmentContext = ShipmentsContext.GetContext(tenant);
            this.loggedUserEmail = loggedUserEmail;
            this.invoiceRepository = new ARInvoiceRepository(objectContext);
            this.invoiceLineRepository = new ARInvoiceLineRepository(objectContext);
            this.invoiceTotalVatRepository = new ARInvoiceTotalVATRepository(objectContext);
            this.invoiceEntityRepository = new ARInvoiceEntityRepository(objectContext);
            this.invoicePaymentRepository = new ARInvoicePaymentRepository(objectContext);
            this.paymentRepository = new ARPaymentRepository(objectContext);
            this.ARInvoiceChargesConstraintRepository = new ARInvoiceChargesConstraintRepository(objectContext);

            this.vatTypeRepository = new VatTypeRepository(myCommonContext);
            this.accountingSettingRepository = new AccountingSettingRepository(myCommonContext);
            this.accountingSystemRepository = new AccountingSystemRepository(myCommonContext);
            this.contactRepository = new ContactRepository(myCommonContext);

            allShipments = new List<Shipment>();
            allReceivables = new List<ShipmentReceivable>();
            shipmentRepository = new ShipmentRepository(myShipmentContext);
            shipmentReceivableRepository = new ShipmentReceivableRepository(myShipmentContext);

            this.TenantObject = (from d in myCommonContext.Tenants where d.Id == tenant select d).FirstOrDefault();
            this.allVatGroups = (from d in myCommonContext.VATTypesGroups where d.Tenant == this.tenant select d).ToList();
            Contact loggedContact = contactRepository.GetSingleContactByEmail(loggedUserEmail, tenant);
            this.loggedContactId = loggedContact.Id;

            this.GetAccountingSystem();
        }
        public ARInvoiceService(MockInvoiceContext objectContext, int tenant)
        {
            MockCommonContext commonMockContext = new MockCommonContext();
            MockShipmentContext shipmentMockContext = new MockShipmentContext();

            this.tenant = tenant;
            this.isUpdateTotalVats = false;
            this.invoiceRepository = new ARInvoiceRepository(objectContext);
            this.invoiceLineRepository = new ARInvoiceLineRepository(objectContext);
            this.invoiceTotalVatRepository = new ARInvoiceTotalVATRepository(objectContext);
            this.invoiceEntityRepository = new ARInvoiceEntityRepository(objectContext);
            this.invoicePaymentRepository = new ARInvoicePaymentRepository(objectContext);
            this.paymentRepository = new ARPaymentRepository(objectContext);
            this.ARInvoiceChargesConstraintRepository = new ARInvoiceChargesConstraintRepository(objectContext);

            this.vatTypeRepository = new VatTypeRepository(commonMockContext);
            this.accountingSettingRepository = new AccountingSettingRepository(commonMockContext);
            this.accountingSystemRepository = new AccountingSystemRepository(commonMockContext);
            this.contactRepository = new ContactRepository(commonMockContext);

            allShipments = new List<Shipment>();
            allReceivables = new List<ShipmentReceivable>();
            shipmentRepository = new ShipmentRepository(shipmentMockContext);
            shipmentReceivableRepository = new ShipmentReceivableRepository(shipmentMockContext);

            this.TenantObject = (from d in commonMockContext.Tenants where d.Id == tenant select d).FirstOrDefault();
            this.allVatGroups = (from d in myCommonContext.VATTypesGroups where d.Tenant == this.tenant select d).ToList();
            this.GetAccountingSystem();
        }

        private void GetLoggedContact()
        {
            ContactPM loggedContact = null;

            if (entityPM.IsFromConsolidationBatch)
            {
                ContactRepository contactRepository = new ContactRepository(myCommonContext);
                ContactQuery contactQuery = new ContactQuery(contactRepository);
                loggedContact = contactQuery.GetSinglePM(entityPM.UpdatedByUserId, tenant);

                if (loggedContact == null)
                {
                    loggedContact = contactQuery.GetSinglePM(entityPM.UpdatedByUserId, 0);
                }
            }

            else
            {
                loggedContact = LoggedContactResolver.GetLoggedContact(tenant);
            }

            if (loggedContact != null)
            {
                loggedContactId = loggedContact.Id;
                loggedContactName = loggedContact.EnglishName;
            }
        }

        private bool isJournal;
        private bool isExternal;
        private bool isTaxItemManaged;
        private bool isTransferToDropbox;
        private bool TransferToDropboxActivated;
        private bool transferToFTPActivated;
        private bool canTransferToFTP;
        private bool isTransferEnabled = false;
        private void GetAccountingSystem()
        {
        
            if(accountingSettingRepository == null)
                accountingSettingRepository = new AccountingSettingRepository(tenant);
            this.accountingSetting = accountingSettingRepository.GetSingleAccountSetting(tenant);
            if (this.accountingSetting != null)
            {
                this.TransferToDropboxActivated = accountingSetting.TransferToDropboxActivated;
                this.transferToFTPActivated = accountingSetting.TransferToFTPActivated;

                AccountingSystem accountingSystem = accountingSystemRepository.GetSingleAccountingSystem(accountingSetting.AccountingSystemCode);
                if (accountingSystem != null)
                {
                    this.isJournal = accountingSystem.IsJournalMode;
                    this.isExternal = accountingSystem.IsExternalCodesFromTable;
                    this.isTaxItemManaged = accountingSystem.IsTaxItemManaged;
                    this.isTransferToDropbox = accountingSystem.CanTransferToDropbox;
                    this.canTransferToFTP = accountingSystem.CanTransferToFTP;

                    if (accountingSetting.IsARInvoicesTransferEnabled && accountingSystem.AllowARInvoicesTransfer)
                    {
                        isTransferEnabled = true;
                    }
                }
            }
        }

        public void Create(ARInvoicePM theEntityPM)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPM;
            this.GetLoggedContact();
            this.isVoidingInvoice = this.entityPM.SetVoided;
            this.isApprovingInvoice = entityPM.SetApproved;


            this.invoice = new ARInvoice();

            if (entityPM.IssuedByUserId == null)
            {
                entityPM.IssuedByUserId = entityPM.CreatedByUserId;
            }

            this.ValidateInvoiceConnected();
            this.InitializeComponent();

            ARInvoiceValidator.Validate(entityPM, this.invoice, this.objectContext, this.myCommonContext, this.isNewEntity);
            ARInvoiceTracing.Trace(entityPM, invoice, isNewEntity, loggedContactId);

            if (entityPM.IsConsolidationInvoice)
            {
                this.UpdateConsolidationLines();
                this.InitializeTransferComponents();
            }

            else if (!entityPM.IsGeneralInvoice || entityPM.ARInvoiceTypeCode == "IT")
            {
                this.GetShipmentsData(entityPM.InvoiceLines);
                this.UpdateInvoiceEntities();
            }

            this.InitializeSalesmanField();

            this.BuildShipmentsNumbers();
            this.UpdateInvoiceLines();
            this.UpdateTotalVats();
            this.BuildSearchFields();
            
            // Full Accounting - Tax Fields Work 
            this.CalculationOfTaxReportfields(entityPM, isApprovingInvoice);
            CheckLinesVatExcempt(entityPM, isApprovingInvoice);
           
            ARInvoiceHelper helper = new ARInvoiceHelper(this.tenant, this.loggedContactId);
            helper.ARInvoiceQuickbooksValidating(invoice, entityPM, this.isApprovingInvoice, isNewEntity, this.objectContext, this.myCommonContext, isVoidingInvoice);

            SetSatStatus();

            EntityAutomationService entityAutomationService = new EntityAutomationService(new EntityAutomationArgs() { Poco = invoice  , EntityPM = entityPM , OldEntityPM = new ARInvoicePM(),  AutomationType = "OnCreate", ObjectTableName = "ARInvoice" ,  Tenant =entityPM.Tenant , EntityId = entityPM.Id});
            entityAutomationService.RunAutomation();

            if (!entityPM.IsConsolidationInvoice)
            {
                this.ComputeInvoiceAmounts();
            }

            SetPrintNotesForInterestInvoice(entityPM);
            ARInvoiceMapping.MapEntity(entityPM, invoice, isNewEntity, loggedContactId);
            invoiceRepository.Add(invoice);
            invoiceRepository.SubmitChanges();

            if (!entityPM.IsConsolidationInvoice && !entityPM.IsGeneralInvoice)
            {
                shipmentReceivableRepository.SubmitChanges();
            }

            if (entityPM.IsInvoiceNumberFromStock)
            {
                this.ARInvoiceStockNumber();
            }

            this.GetForeignFields();
            this.RunStoredProcedures();
            this.AfterServiceFinished();
            if (entityPM.ARInvoiceTypeCode == "IT")
            {                
                this.UpdateInterestReportFields(entityPM);
                this.UpdateInterestReportsConnectedInvoice(entityPM);
            }
        }

        private void InitializeSalesmanField()
        {
            if (this.isNewEntity)
            {
                if (entityPM.SalesmanUserId == null)
                {
                    if (entityPM.MainEntityId != null)
                    {
                        Shipment shipment = allShipments.Where(d => d.Id == entityPM.MainEntityId).FirstOrDefault();
                        if (shipment != null)
                        {
                            entityPM.SalesmanUserId = shipment.SalesmanUserId;
                        }
                    }
                }

                if (entityPM.SalesmanUserId == null)
                {
                    if (entityPM.BillToId != null)
                    {
                        CardRepository cardRepository = new CardRepository(entityPM.Tenant);
                        Card card = cardRepository.GetSingleCard(entityPM.BillToId, entityPM.Tenant);
                        if (card != null)
                        {
                            entityPM.SalesmanUserId = card.SalesmanUserId;
                        }
                    }
                }
            }
        }
        private void SetPrintNotesForInterestInvoice(ARInvoicePM invoice)
        {
            if (invoice.ARInvoiceTypeCode == "IT")
            {
                ChargesTypeQuery chargesTypeQuery = new ChargesTypeQuery(invoice.Tenant);
                List<ChargesTypePM> chargesTypes = chargesTypeQuery.GetChargesTypesByCode("INT", invoice.Tenant);
                if (chargesTypes.Count > 0)
                {
                    ChargesTypePM chargesType = chargesTypes.FirstOrDefault();
                    ARInvoiceLinePM interestInvoiceLine = invoice.InvoiceLines.Where(d => d.ChargesTypeId == chargesType.Id).FirstOrDefault();
                    UserPM userPM = GetLoggedUser(invoice.Tenant);
                    bool showLocal = userPM != null ? !userPM.DontShowLocalLabels : LoggedContactResolver.GetLoggedContactShowLocal(tenant);                 
                    if (interestInvoiceLine != null)
                        entityPM.PrintNotes =string.IsNullOrEmpty(entityPM.PrintNotes)? entityPM.PrintNotes + " " + (showLocal ? interestInvoiceLine.LocalDescription : interestInvoiceLine.Description) : entityPM.PrintNotes + ", " + (showLocal ? interestInvoiceLine.LocalDescription : interestInvoiceLine.Description);
                }
            }
        }
        private UserPM GetLoggedUser(int tenant)
        {
            UserQuery userQuery = new UserQuery(tenant);
            UserPM userPM = userQuery.GetSinglePMByEmail(loggedUserEmail, tenant);
            if (userPM == null)
            {
                userPM = userQuery.GetSinglePMByEmail(loggedUserEmail, 0);
            }
            return userPM;
        }
        private void UpdateInterestReportsConnectedInvoice()
        {

        }

        private void SetSatStatus()
        {
            if (string.IsNullOrEmpty(entityPM.SATTransferStatusCode)) entityPM.SATTransferStatusCode = "NT";
            if (string.IsNullOrEmpty(entityPM.SATInvoiceStatusCode)) entityPM.SATInvoiceStatusCode = "NO";


        }

        private void UpdateInterestReportFields(ARInvoicePM theEntityPM)
        {
            IInterestReportUpdateServiceExt InterestReportUpdate = ContainerAccessor.Container.Resolve(typeof(IInterestReportUpdateServiceExt), "InterestReportUpdateServiceExt", new ParameterOverride("", 1)) as IInterestReportUpdateServiceExt;
            InterestReportUpdate.UpdateConfirmCreateInvoice(null, tenant, null, theEntityPM.Id, theEntityPM.InvoiceNumber, theEntityPM.AmountInLocalCurrency, theEntityPM.InvoiceEntities[0].EntityId);

        }

        private void UpdateInterestReportsConnectedInvoice(ARInvoicePM theEntityPM)
        {
            IInterestReportsConnectedInvoiceUpdateServiceExt InterestReportsConnectedInvoiceUpdate = ContainerAccessor.Container.Resolve(typeof(IInterestReportsConnectedInvoiceUpdateServiceExt), "InterestReportsConnectedInvoiceUpdateServiceExt", new ParameterOverride("", 1)) as IInterestReportsConnectedInvoiceUpdateServiceExt;
            InterestReportsConnectedInvoiceUpdate.UpdateInterestLastBatchService(theEntityPM.InvoiceEntities[0].EntityId, tenant, null, theEntityPM.Id);
        }
        private void ValidateInvoiceConnected()
        {
            if (!entityPM.IsConsolidationInvoice && !entityPM.IsGeneralInvoice)
            {
                if (isNewEntity)
                {
                    List<string> allReceivablesIds = (from d in entityPM.InvoiceLines group d by d.ReceivableId into g select g.Key).ToList();

                    bool isReceivablesConnected = (from d in shipmentRepository.context.ShipmentReceivables
                                                   where d.Tenant == tenant
                                                   && d.ARInvoiceId != null
                                                   && d.ARInvoiceLineId != null
                                                   && allReceivablesIds.Contains(d.Id)
                                                   select d).Any();

                    if (isReceivablesConnected)
                    {
                        throw new ApplicationException("This invoice is already created");
                    }
                }

                else
                {
                    if (entityPM.IsAutoCredit || entityPM.StatusCode == "AC" || entityPM.StatusCode == "AR")
                    {
                        foreach (ARInvoiceLinePM item in entityPM.InvoiceLines)
                        {
                            item.EntityId = null;
                            item.ReceivableId = null;

                            if (item.ChangeSetOp == ChangeSetOperation.None)
                            {
                                item.ChangeSetOp = ChangeSetOperation.Update;
                            }
                        }
                    }

                    List<string> allReceivablesIds = (from d in entityPM.InvoiceLines where d.ReceivableId != null group d by d.ReceivableId into g select g.Key).ToList();

                    if (allReceivablesIds.Count > 0)
                    {
                        bool isReceivablesConnected = (from d in shipmentRepository.context.ShipmentReceivables
                                                       where d.Tenant == tenant
                                                       && d.ARInvoiceId != null
                                                       && d.ARInvoiceId != this.invoice.Id
                                                       && allReceivablesIds.Contains(d.Id)
                                                       select d).Any();

                        if (isReceivablesConnected)
                        {
                            throw new ApplicationException("Some of invoice lines is already connected to another invoice");
                        }
                    }
                }
            }
        }

        private bool isUpdatingPayments = false;
        private List<ARInvoiceLinePM> invoiceLinesChangeSet;
        private List<ConstituentPM> invoiceConstituentsChangeSet;
        private List<ARInvoicePaymentPM> invoicePaymentsChangeSet;
        public void SetChangeSet(List<ARInvoiceLinePM> invoiceLinesChangeSet, List<ARInvoicePaymentPM> invoicePaymentsChangeSet, List<ConstituentPM> invoiceConstituentsChangeSet)
        {
            this.invoiceLinesChangeSet = invoiceLinesChangeSet;
            this.invoicePaymentsChangeSet = invoicePaymentsChangeSet;
            this.invoiceConstituentsChangeSet = invoiceConstituentsChangeSet;
        }
        public void Update(ARInvoicePM theEntityPM, bool mapComposition = false)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPM;
            this.GetLoggedContact();

            this.isVoidingInvoice = entityPM.SetVoided;

            if (entityPM.IsAutoCredit)
            {
                entityPM.SetApproved = false;
            }

            this.isApprovingInvoice = entityPM.SetApproved;

            this.invoice = invoiceRepository.GetSingleInvoice(entityPM.Id);

            if (invoice.StatusCode == "AR")
            {
                if (this.entityPM.StatusCode == "AD")
                {
                    throw new ApplicationException("this invoice is already auto credited");
                }
            }

            this.ValidateInvoiceConnected();

            if (entityPM.SetApproved)
            {
                if (invoice.StatusCode == "LL")
                {
                    throw new ApplicationException("this invoice is Draft cancelled");
                }

                if (invoice.ApprovedDate != null)
                {
                    throw new ApplicationException("This invoice is already approved");
                }
            }

            if (this.entityPM.SetVoided)
            {
                this.sATInterfaceHelper.HandleInvoiceSATCancellation(entityPM, invoice);
            }

            this.ValidateHigherStatus();

            if (entityPM.SetCancelDraft)
            {
                this.CancelDraftInvoice();

                ARInvoiceValidator.Validate(entityPM, this.invoice, this.objectContext, this.myCommonContext, this.isNewEntity);
                ARInvoiceTracing.Trace(entityPM, invoice, isNewEntity, loggedContactId);

                if (isNewEntity)
                {
                    if (entityPM.NewConcurrencyGUID == null)
                    {
                        entityPM.NewConcurrencyGUID = Guid.NewGuid().ToString();
                    }
                }

                invoice.ConcurrencyGUID = entityPM.NewConcurrencyGUID;
                entityPM.ConcurrencyGUID = invoice.ConcurrencyGUID;
            }

            else
            {
                if (mapComposition)
                {
                    this.invoiceLinesChangeSet = this.entityPM.InvoiceLines;
                    this.invoicePaymentsChangeSet = this.entityPM.InvoicePayments;
                    this.invoiceConstituentsChangeSet = this.entityPM.ConstituentInvoices;
                }

                if (this.invoiceLinesChangeSet == null)
                {
                    this.invoiceLinesChangeSet = new List<ARInvoiceLinePM>();
                }

                if (this.invoiceConstituentsChangeSet == null)
                {
                    this.invoiceConstituentsChangeSet = new List<ConstituentPM>();
                }

                if (this.invoicePaymentsChangeSet == null)
                {
                    this.invoicePaymentsChangeSet = new List<ARInvoicePaymentPM>();
                }

                this.InitializeComponent();

                this.ARInvoiceStockNumber();

                ARInvoiceValidator.Validate(entityPM, this.invoice, this.objectContext, this.myCommonContext, this.isNewEntity);
                ARInvoiceTracing.Trace(entityPM, invoice, isNewEntity, loggedContactId);

                if (entityPM.IsConsolidationInvoice)
                {
                    this.UpdateConsolidationLines();
                    this.InitializeTransferComponents();
                }

                else if (!entityPM.IsGeneralInvoice)
                {
                    this.GetShipmentsData(invoiceLinesChangeSet);
                    this.UpdateInvoiceEntities();
                }

                this.UpdateInvoiceLines();
                this.UpdateTotalVats();
                this.BuildShipmentsNumbers();

                ARInvoiceHelper helper = new ARInvoiceHelper(this.tenant, this.loggedContactId);
                if (entityPM.SetReSendQBO)
                {
                    helper.ARInvoiceQuickbooksValidating(invoice, entityPM, true, isNewEntity, this.objectContext, this.myCommonContext, isVoidingInvoice);

                }
                else
                {
                    helper.ARInvoiceQuickbooksValidating(invoice, entityPM, this.isApprovingInvoice, isNewEntity, this.objectContext, this.myCommonContext, isVoidingInvoice);
                }

                // Full Accounting - Tax Fields Work 
                this.CalculationOfTaxReportfields(entityPM, isApprovingInvoice);

                EntityAutomationService entityAutomationService = new EntityAutomationService(new EntityAutomationArgs() { Poco = invoice, EntityPM = entityPM, OldEntityPM = new ARInvoicePM(), AutomationType = "OnUpdate", ObjectTableName = "ARInvoice", Tenant = entityPM.Tenant , EntityId = entityPM.Id });

                ARInvoiceMapping.MapEntity(entityPM, invoice, isNewEntity, loggedContactId); 

                 
                invoiceRepository.Update(invoice);
                invoiceRepository.SubmitChanges();

                if (!entityPM.IsConsolidationInvoice && !entityPM.IsGeneralInvoice)
                {
                    shipmentReceivableRepository.SubmitChanges();
                }

                bool isUpdatingPayments = true;
                if (invoice.IsAutoCredit || invoice.IsCancelled || invoice.IsConstituentInvoice)
                {
                    isUpdatingPayments = false;
                }

                else if (invoicePaymentsChangeSet.Where(d => d.ChangeSetOp == ChangeSetOperation.Insert || d.ChangeSetOp == ChangeSetOperation.Delete).Count() == 0)
                {
                    isUpdatingPayments = false;
                }

                if (isUpdatingPayments)
                {
                    this.UpdateInvoicePayments(invoicePaymentsChangeSet);
                    this.UpdateInvoiceAmountDue();
                    this.UpdatePaidDate();
                }
                
                this.BuildSearchFields();
                entityAutomationService.RunAutomation();
            }



            ARPaymentReferencesService ARPaymentReferencesService = new ARPaymentReferencesService(this.objectContext);
            invoice.PaymentReferences = ARPaymentReferencesService.GetARInvoicePaymentRefreneces(invoice); 
              
            invoiceRepository.Update(invoice);
            invoiceRepository.SubmitChanges();

            if (!String.IsNullOrEmpty(QBOARPaymentId))
            {
                ARPaymentHelper service = new ARPaymentHelper();
                ARPaymentQuery PaymentQuery = new ARPaymentQuery(paymentRepository);
                ARPaymentPM paymentPM = PaymentQuery.GetSinglePM(QBOARPaymentId, tenant);
                if (paymentPM.TransferStatusCode == "TR")
                {
                    ARPaymentRepository repository = new ARPaymentRepository(tenant);
                    ARPayment payment = repository.GetSingleARPayment(paymentPM.Id, tenant);
                    bool isErrorInTransfer = payment.TransferStatusCode == "ET" ? true : false;

                    service.ARPaymentQuickbooksValidating(paymentPM, true, false, payment, this.objectContext, this.myCommonContext, false, paymentPM.SetReSendQBO, isErrorInTransfer, false);
                }
            }

            this.GetForeignFields();
            this.RunStoredProcedures();
            this.AfterServiceFinished();
        }

         

        private void ARInvoiceStockNumber()
        {
            if (!this.isApprovingInvoice && !this.isVoidingInvoice)
            {
                IInvoiceContext context = InvoiceContext.GetContext(tenant);
                ARInvoiceStockLineRepository aRInvoiceStockLineRepository = new ARInvoiceStockLineRepository(tenant);
                ARInvoiceStockQuery aRInvoiceStockQuery = new ARInvoiceStockQuery(tenant);
                ARInvoiceStockService aRInvoiceStockService = new ARInvoiceStockService(context, tenant);
                ARInvoiceStockPM stock = new ARInvoiceStockPM();
                ARInvoiceStockLine stockLine = new ARInvoiceStockLine();

                if (this.isNewEntity)
                {
                    if (entityPM.ARInvoiceStockId != null)
                    {
                        stockLine = aRInvoiceStockLineRepository.GetSingleARInvoiceStockLine(entityPM.ARInvoiceStockId, tenant);
                        if (stockLine != null)
                        {
                            stockLine.IsUsed = true;
                            stockLine.ARInvoiceId = entityPM.Id;
                            stockLine.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                            stockLine.UpdatedByUserId = this.loggedContactId;
                            stockLine.ShipmentNumber = entityPM.MainEntityReference;
                            aRInvoiceStockLineRepository.Update(stockLine);
                            aRInvoiceStockLineRepository.SubmitChanges();
                            stock = aRInvoiceStockQuery.GetSinglePM(stockLine.ARInvoiceStockId, tenant);
                            aRInvoiceStockService.Update(stock, false);
                            this.CreateInvoiceStockEvent("NTFS", entityPM.Id, stockLine.Number + " added from " + stock.Name);
                        }
                    }
                }

                else
                {
                    if (entityPM.ARInvoiceStockId != this.invoice.ARInvoiceStockId)
                    {
                        stockLine = new ARInvoiceStockLine();
                        if (entityPM.ARInvoiceStockId == null)
                        {
                            stockLine = aRInvoiceStockLineRepository.GetSingleARInvoiceStockLine(this.invoice.ARInvoiceStockId, tenant);
                            stockLine.IsUsed = false;
                            stockLine.ARInvoiceId = null;
                            stockLine.ShipmentNumber = null;
                            stockLine.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                            stockLine.UpdatedByUserId = this.loggedContactId;
                            aRInvoiceStockLineRepository.Update(stockLine);
                            aRInvoiceStockLineRepository.SubmitChanges();
                            stock = aRInvoiceStockQuery.GetSinglePM(stockLine.ARInvoiceStockId, tenant);
                            this.CreateInvoiceStockEvent("NRTS", entityPM.Id, stockLine.Number + " returned to " + stock.Name);
                            this.entityPM.IsInvoiceNumberFromStock = false;
                        }

                        else
                        {
                            stockLine = aRInvoiceStockLineRepository.GetSingleARInvoiceStockLine(entityPM.ARInvoiceStockId, tenant);
                            stockLine.IsUsed = true;
                            stockLine.ARInvoiceId = entityPM.Id;
                            stockLine.ShipmentNumber = entityPM.MainEntityReference;
                            stockLine.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                            stockLine.UpdatedByUserId = this.loggedContactId;
                            aRInvoiceStockLineRepository.Update(stockLine);
                            aRInvoiceStockLineRepository.SubmitChanges();
                            stock = aRInvoiceStockQuery.GetSinglePM(stockLine.ARInvoiceStockId, tenant);
                            this.CreateInvoiceStockEvent("NTFS", entityPM.Id, stockLine.Number + " added from " + stock.Name);
                        }

                        aRInvoiceStockService.Update(stock, false);
                    }
                }
            }
        }

        private void CreateInvoiceStockEvent(String code, string entityId, string note)
        {
            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                EntityId = entityId,
                ObjectTableName = "ARInvoice",
                Tenant = tenant,
                UserId = loggedContactId,
                EventTypeCode = code,
                Notes = note,
            });
        }
        private void BuildFlatfile(ARInvoicePM ARInvoice)
        {

            AddressRepository addressRepository = new AddressRepository(tenant);
            AddressQuery addressQuery = new AddressQuery(addressRepository);
            AddressPM address = addressQuery.GetSingleAddressPM(ARInvoice.BillToAddressId, tenant);
            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
            ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRepository);
            ShipmentPM shipmentPM = shipmentQuery.GetSingleShipmentPMByNumber(ARInvoice.MainEntityReference, tenant);

            ChargesTypeRepository chargeTypeRepository = new ChargesTypeRepository(tenant);
            ChargesTypeQuery chargesTypeQuery = new ChargesTypeQuery(chargeTypeRepository);

            CardRepository cardRepository = new CardRepository(tenant);
            CardQuery cardQuery = new CardQuery(cardRepository);

            CardPM cardPM = cardQuery.GetSinglePM(ARInvoice.BillToId, tenant);

            ARInvoiceTotalVATRepository aRInvoiceTotalVATRepositoryRepository = new ARInvoiceTotalVATRepository(tenant);
            ARInvoiceTotalVATQuery arInvoiceTotalVatQuery = new ARInvoiceTotalVATQuery(aRInvoiceTotalVATRepositoryRepository);

            StringBuilder FlatFile = new StringBuilder();
            FlatFile.Append("INVOICE DATE|" + String.Format("{0:dd/MM/yyyy}", ((DateTime)ARInvoice.InvoiceDate)) + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("CODE|" + cardPM.Code + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("NAME CUSTOMER|" + ARInvoice.BillToName + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("VAT NO CUSTOMER|" + ARInvoice.VatNumber + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("ADDRESS 1 MAIN ADDRESS CUSTOMER|" + address.Address1 + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("ADDRESS 2 MAIN ADDRESS CUSTOMER|" + address.Address2 + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("ADDRESS MAIN CITY|" + address.City + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("ADDRESS MAIN ZIP CODE|" + address.ZipCode + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("ADDRESS MAIN STATE|" + address.StateEnglishName + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("ADDRESS MAIN COUNTRY|" + address.CountryName + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("REFERENCE NO|" + ARInvoice.MainEntityReference + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("BRANCH|" + shipmentPM.BranchName + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("MAWB|" + ARInvoice.MasterNumber + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("HAWB|" + ARInvoice.HouseNumber + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("INVOICE CUSTOMER REF|" + ARInvoice.CustomerRef + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("ORDER/BOOKING DATAILS QUANTITY|" + shipmentPM.NumberOfPackages + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("ORDER/BOOKING DATAILS GROSS WEIGHT(MARITIMO) CHARGEABLE WEIGHT (AEREO)|" + (shipmentPM.TransportModeId == "A" ? shipmentPM.ChargeableWeight : shipmentPM.GrossWeight) + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("ORDER/BOOKING VOLUME (CBM)|" + shipmentPM.VolumeInCBM + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("ROUTING FROM|" + shipmentPM.MainCarriageFromPortCode + " / " + shipmentPM.MainCarriageFromPortCountryCode + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("ROUTING TO|" + shipmentPM.MainCarriageFinalDestinationPortCode + " / " + shipmentPM.MainCarriageFinalDestinationPortCountryCode + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("DESCRIPCION OF GOODS|" + shipmentPM.DescriptionOfGoods + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("TYPE OF SHIPMENT|" + (shipmentPM.TransportModeId == "A" ? "Aereo" : shipmentPM.TransportModeId == "O" ? "MARITIMO" : "terrestre") + " | ");
            FlatFile.Append("\r\n");
            FlatFile.Append("MAIN CARRIAGE TRANSPORT|" + shipmentPM.MainCarriageCarrierName + "|");
            FlatFile.Append("\r\n");

            for (int i = 0; i < ARInvoice.InvoiceLines.Count; i++)
            {
                ChargesTypePM chargesTypePM = chargesTypeQuery.GetSinglePM(ARInvoice.InvoiceLines[i].ChargesTypeId, tenant);

                FlatFile.Append("RECEIVABLES" + (i + 1) + "|" + chargesTypePM.Code + "|" + ARInvoice.InvoiceLines[i].Quantity + "|" + chargesTypePM.ReceivableCreditAccount + "|" + chargesTypePM.EnglishName + "|" + ARInvoice.InvoiceLines[i].VatPercentage + "%||" + ARInvoice.InvoiceLines[i].InvoiceCurrencyAmount + "|");
                FlatFile.Append("\r\n");

            }
            FlatFile.Append("SUBTOTAL INVOICE|" + ARInvoice.SubTotalInInvoiceCurrency + "|");
            FlatFile.Append("\r\n");
            List<ARInvoiceTotalVATPM> arInvoicesTotalvats = arInvoiceTotalVatQuery.GetTotalVATs(ARInvoice.Id, tenant).ToList();
            for (int i = 0; i < arInvoicesTotalvats.Count; i++)
            {
                if (arInvoicesTotalvats[i].VATPercent == 16) {
                    FlatFile.Append("STD 16% INVOICE|" + arInvoicesTotalvats[i].InvoiceCurrencyVATAmount + "|");
                }
                else
                {
                    if (arInvoicesTotalvats[i].VATPercent != 0)
                        FlatFile.Append("TOTAL VAT TYPE " + arInvoicesTotalvats[i].VATPercent + "% |" + arInvoicesTotalvats[i].InvoiceCurrencyVATAmount + "|");
                }
                if (arInvoicesTotalvats[i].VATPercent != 0)
                    FlatFile.Append("\r\n");
            }
            FlatFile.Append("TOTAL INVOICE|" + ARInvoice.AmountInInvoiceCurrency + "|");
            FlatFile.Append("\r\n");

            FlatFile.Append("CURRENCY|" + ARInvoice.InvoiceCurrencyCode + "|");
            FlatFile.Append("\r\n");

            FlatFile.Append("NOTES|" + ARInvoice.PrintNotes + "|");
            FlatFile.Append("\r\n");
            //   String fileContainer = FlatFile.ToString();
            //  string path = @"D:\Flatfile.txt";

            //   File.WriteAllText(path, fileContainer);


        }

        private void ValidateHigherStatus()
        {
            if (!isNewEntity)
            {
                if (this.invoice.StatusCode == "AD")
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
                        string msg = "This invoice is already approved";
                        if (!string.IsNullOrEmpty(this.invoice.ApprovedByUserId))
                        {
                            ContactRepository contactRepository = new ContactRepository(tenant);
                            Contact myContact = contactRepository.GetSingleContact(this.invoice.ApprovedByUserId, tenant);

                            if (myContact != null)
                            {
                                msg += " by " + myContact.EnglishName;
                            }
                        }

                        throw new ApplicationException(msg);
                    }
                }

                if (this.entityPM.SetVoided)
                {
                    if (this.invoice.StatusCode == "AC")
                    {
                        throw new ApplicationException("This invoice is already Auto Credit");
                    }

                    else if (this.invoice.StatusCode == "AR")
                    {
                        throw new ApplicationException("This invoice is already Auto Credited");
                    }

                    else if (this.invoice.IsConstituentInvoice && this.invoice.ConsolidationInvoiceId != null)
                    {
                        throw new ApplicationException("This invoice is already Connected to Consolidation invoice");
                    }
                }
            }
        }

        private void CancelDraftInvoice()
        {
            if (invoice.StatusCode != "DR")
            {
                throw new ApplicationException("this invoice is not Draft");
            }

            if (entityPM.IsConsolidationInvoice)
            {
                this.DisconnectAllConnectedInvoices();
            }

            else
            {
                List<ARInvoiceEntity> dbEntities = invoiceEntityRepository.GetInvoiceEntitiesForInvoice(entityPM.Id, tenant).ToList();
                foreach (ARInvoiceEntity item in dbEntities)
                {
                    invoiceEntityRepository.Remove(item);
                }

                List<ARInvoiceLine> lines = invoiceLineRepository.GetInvoiceLinesByInvoiceId(entityPM.Id, tenant).ToList();
                List<string> allShipmentsIds = (from d in lines group d by d.EntityId into g select g.Key).ToList();
                this.allReceivables = shipmentReceivableRepository.GetShipmentReceivablesByEntityIds(allShipmentsIds, tenant);

                foreach (ARInvoiceLine item in lines)
                {
                    this.DisconnectReceivable(item.ReceivableId);
                    this.DeleteARInvoiceChargesConstraint(item);

                    item.ReceivableId = null;
                    invoiceLineRepository.Update(item);
                }
            }

            invoice.StatusCode = entityPM.StatusCode = "LL";
            invoice.UpdateDate = entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            invoice.UpdatedByUserId = entityPM.UpdatedByUserId = loggedContactId;
        }
        public void OnCreatingAutoCredit()
        {
            ARInvoice entityPOCO = invoiceRepository.GetSingleInvoice(this.entityPM.CreditedByARInvoiceId);
            if (entityPOCO != null)
            {
                if (entityPOCO.StatusCode == "AR")
                {
                    throw new ApplicationException("This invoice is already auto credited");
                }

                else if (entityPOCO.StatusCode == "VD")
                {
                    throw new ApplicationException("This invoice is already voided");
                }

                else
                {
                    this.isAutoCreditingInvoice = true;

                    ARInvoiceQuery entityQuery = new ARInvoiceQuery(invoiceRepository);
                    ARInvoicePM oldEntityPM = entityQuery.GetSinglePM(this.entityPM.CreditedByARInvoiceId, tenant);

                    //this.AddARInvoiceJournalAndJournalLines(this.entityPM, true);

                    // Update Old Invoice
                    if (oldEntityPM.IsConsolidationInvoice)
                    {
                        #region
                        this.allConnectedInvoices = invoiceRepository.GetConnectedInvoices(tenant, this.entityPM.CreditedByARInvoiceId).ToList();

                        foreach (ARInvoice item in allConnectedInvoices)
                        {
                            item.IsClosed = false;
                            item.StatusCode = "NT";
                            item.ConsolidationInvoiceId = null;

                            EventTracer.CreateTraceEvent(new EventTracerArgs()
                            {
                                EntityId = item.Id,
                                ObjectTableName = "ARInvoice",
                                Tenant = tenant,
                                UserId = loggedContactId,
                                EventTypeCode = "INDS",
                            });
                        }
                        #endregion
                    }

                    else
                    {
                        #region Disconnect Receivables
                        List<string> iReceivablesIds = (from a in oldEntityPM.InvoiceLines group a by new { a.ReceivableId } into gr select gr.Key.ReceivableId).ToList();
                        List<ShipmentReceivable> iReceivables = shipmentReceivableRepository.GetShipmentReceivablesByIds(iReceivablesIds, tenant);
                        if (iReceivables.Count > 0)
                        {
                            foreach (ShipmentReceivable item in iReceivables)
                            {
                                item.ARInvoiceId = null;
                                item.ARInvoiceLineId = null;
                                item.ShipmentReceivableLineStatusCode = "OAMT";
                                shipmentReceivableRepository.Update(item);
                            }

                            shipmentReceivableRepository.SubmitChanges();
                        }

                        List<ARInvoiceLine> lines = invoiceLineRepository.GetInvoiceLinesByInvoiceId(this.entityPM.CreditedByARInvoiceId, this.tenant).ToList();
                        foreach (ARInvoiceLine line in lines)
                        {
                            this.DeleteARInvoiceChargesConstraint(line);

                            line.ReceivableId = null;
                            invoiceLineRepository.Update(line);                            
                        }
                        #endregion
                    }

                    entityPOCO.IsCancelled = true;
                    entityPOCO.CancelledByARInvoiceId = this.entityPM.Id;
                    entityPOCO.StatusCode = "AR";
                    entityPOCO.AmountDue = 0;
                    entityPOCO.AmountDueInLocalCurrency = 0;
                    entityPOCO.AmountDueInProfitCurrency = 0;
                    invoiceRepository.Update(entityPOCO);
                    invoiceRepository.SubmitChanges();
                }
            }
        }

        private void CreateARInvoiceMessage(bool setApproved)
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

                        this.invoice = invoiceRepository.GetSingleInvoice(this.entityPM.Id);
                        List<ARInvoice> entities = new List<ARInvoice>();
                        entities.Add(this.invoice);

                        ARInvoiceMessageHelper myHelper = new ARInvoiceMessageHelper(entities, this.invoice.InvoiceNumber + ".xml", tenant, isDropBox, isFTP);
                        myHelper.Transfer();
                    }
                }
            }
        }

        #region InitializeComponent

        private void InitializeComponent()
        {
            if (entityPM.IsInvoiceNumberManuallySet)
            {
                entityPM.InvoiceNumber = MethodHelper.Trim(entityPM.InvoiceNumber);
            }

             SetBillToPartnerIdAndPrimaryContact();
            // DR: Draft
            // CN: Connected
            // NT: Not Connected
            // AD: Unpaid
            // PD: Paid
            // PP: Partially Paid
            // AC: Auto Credit
            // AR: Auto Credited
            // VD: Void

            entityPM.IsFullAccounting = IsFullAccountingActivated(entityPM.Tenant);

            if (string.IsNullOrEmpty(entityPM.Id))
            {
                entityPM.Id = IdCounter.GetNumber("ARInvoice", entityPM.Tenant).ToString();
            }

            if (entityPM.InvoiceDate != null)
            {
                entityPM.InvoiceDate = entityPM.InvoiceDate.Value.Date;
            }



            this.InitializeDueDate();
            this.InitializeBranchField();
            this.InitializeVATs();
            this.InitializeTransferComponents();
            this.InitializeAmountDueFields();
            this.InitializeCustomerWorkingDates();

            if (entityPM.IsConstituentInvoice)
            {
                if (isNewEntity)
                {
                    if (!entityPM.IsInvoiceNumberManuallySet)
                    {
                        this.GenerateInvoiceNumber();
                    }
                }

                if (entityPM.StatusCode == "VD" || entityPM.StatusCode == "AC" || entityPM.StatusCode == "AR")
                {
                    entityPM.IsClosed = true;
                    entityPM.ConsolidationInvoiceId = null;
                }

                else
                {
                    if (string.IsNullOrEmpty(entityPM.ConsolidationInvoiceId))
                    {
                        entityPM.IsClosed = false;
                        entityPM.StatusCode = "NT";
                    }

                    else
                    {
                        entityPM.IsClosed = true;
                        entityPM.StatusCode = "CN";
                    }
                }
            }

            if (entityPM.SetApproved)
            {
                entityPM.StatusCode = "AD";
                entityPM.ApprovedDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                entityPM.ApprovedByUserId = loggedContactId;

                if (string.IsNullOrEmpty(this.entityPM.MasterNumber) || string.IsNullOrEmpty(this.entityPM.HouseNumber))
                {
                    if (!string.IsNullOrEmpty(this.entityPM.MainEntityId))
                    {
                        Shipment myShipment = shipmentRepository.GetSingleShipment(this.entityPM.MainEntityId, tenant);
                        if (myShipment != null)
                        {
                            if (string.IsNullOrEmpty(this.entityPM.MasterNumber))
                            {
                                if (!string.IsNullOrEmpty(myShipment.MasterShipmentDataId))
                                {
                                    ShipmentMasterData myMaster = shipmentRepository.GetSingleShipmentMasterData(myShipment.MasterShipmentDataId, tenant);
                                    if (myMaster != null)
                                    {
                                        if (!string.IsNullOrEmpty(myMaster.Master))
                                        {
                                            if (!string.IsNullOrEmpty(myMaster.AirlinePrefix))
                                            {
                                                this.entityPM.MasterNumber = myMaster.AirlinePrefix + "-" + myMaster.Master;
                                            }

                                            else
                                            {
                                                this.entityPM.MasterNumber = myMaster.Master;
                                            }
                                        }
                                    }
                                }
                            }

                            if (string.IsNullOrEmpty(this.entityPM.HouseNumber))
                            {
                                this.entityPM.HouseNumber = myShipment.House;
                            }
                        }
                    }
                }

                if (!entityPM.IsInvoiceNumberManuallySet)
                {
                    this.GenerateInvoiceNumber();
                }

                this.UpdateNeedRebuild();
            }

            if (!isNewEntity)
            {
                if (entityPM.SetVoided)
                {
                    if (entityPM.StatusCode != "VD")
                    {
                        entityPM.StatusCode = "VD";
                    }

                    if (entityPM.IsConsolidationInvoice)
                    {
                        this.DisconnectAllConnectedInvoices();
                    }
                }

                if (invoice.StatusCode != "VD" && entityPM.StatusCode == "VD")
                {
                    DocumentOutRepository documentOutRepository = new DocumentOutRepository(myCommonContext);
                    DocumentOut docOut = documentOutRepository.GetDocumentOutByEntityAndChildEntity(invoice.MainEntityId, invoice.Id);
                    if (docOut != null)
                    {
                        docOut.NeedsRebuild = true;
                        documentOutRepository.Update(docOut);
                        documentOutRepository.SubmitChanges();
                    }

                    //DateTime startTime = DateTime.Now;
                    //DateTime endTime = DateTime.Now;
                    //int executionTime = (int)((endTime.Ticks - startTime.Ticks) / TimeSpan.TicksPerMillisecond);

                    //string logMessage = "Set DocumentOut NeedsRebuild: StartTime = " + startTime.ToString() + ", EndTime = " + endTime.ToString() + ", ExecutionTime: " + executionTime.ToString();
                    //AzureLog.SaveLogsInStorage(logMessage, "VD", DateTime.Now, "", "", 0, loggedContactId, loggedContactName, HttpContext.Current.Request.UserHostAddress);
                }
            }

            else if (entityPM.IsAutoCredit)
            {
                entityPM.StatusCode = "AC";

                this.GenerateInvoiceNumber();
            }

            if (!entityPM.IsConstituentInvoice)
            {
                if (string.IsNullOrEmpty(entityPM.StatusCode) || entityPM.StatusCode == "DR")
                {
                    entityPM.StatusCode = "DR";

                    if (string.IsNullOrEmpty(entityPM.DraftNumber))
                    {
                        entityPM.DraftNumber = CodeCounter.GetNumber("ARInvoice", entityPM.Tenant).ToString();
                    }
                }

                if (!entityPM.IsInvoiceNumberManuallySet && !entityPM.IsInvoiceNumberFromStock)
                {
                    if (string.IsNullOrEmpty(entityPM.InvoiceNumber))
                    {
                        entityPM.InvoiceNumber = entityPM.Id;
                    }
                }
            }
        }

        private void UpdateNeedRebuild()
        {
            DocumentOut docOut = null;
            DocumentOutRepository documentOutRepository = new DocumentOutRepository(myCommonContext);

            if (entityPM.IsConsolidationInvoice)
            {
                string objectTableId = new ObjectTableRepository(entityPM.Tenant).GetObjectTableIdByName("ARInvoice");

                docOut = documentOutRepository.GetDocumentOutByEntityId(invoice.Id, objectTableId, this.tenant);
            }

            else
            {
                docOut = documentOutRepository.GetDocumentOutByEntityAndChildEntity(invoice.MainEntityId, invoice.Id);
            }

            if (docOut != null)
            {
                docOut.NeedsRebuild = true;
                documentOutRepository.Update(docOut);
                documentOutRepository.SubmitChanges();
            }
        }

        private void SetBillToPartnerIdAndPrimaryContact()
        {
            CardRepository cardRepository = new CardRepository(entityPM.Tenant);
            Card card = cardRepository.GetSingleCard(entityPM.BillToId, entityPM.Tenant);
            entityPM.BillToPartnerTypeId = card != null ? card.PartnerTypeId : null;
            entityPM.BillToContactId = card != null ? card.PrimaryContactId : null;
        }
        private void InitializeDueDate()
        {
            if (entityPM.DueDate == null)
            {
                if (string.IsNullOrEmpty(entityPM.PaymentTermId))
                {
                    entityPM.DueDate = entityPM.InvoiceDate;
                }

                else
                {
                    PaymentTermRepository paymentTermRepository = new PaymentTermRepository(myCommonContext);
                    PaymentTerm myPaymentTerm = paymentTermRepository.GetSinglePaymentTerm(entityPM.PaymentTermId, tenant);

                    if (myPaymentTerm != null)
                    {
                        if (myPaymentTerm.IsManuallySet)
                        {
                            entityPM.DueDate = null;
                        }

                        else
                        {
                            DateTime? myComparativeDate = null;

                            if (entityPM.IsConsolidationInvoice)
                            {
                                myComparativeDate = entityPM.InvoiceDate;
                            }

                            else
                            {
                                if (myPaymentTerm.FromDateTypeCode == "SHI")
                                {
                                    myComparativeDate = entityPM.OperationalDate;

                                    if (myComparativeDate == null)
                                    {
                                        myComparativeDate = entityPM.InvoiceDate;
                                    }
                                }

                                else
                                {
                                    myComparativeDate = entityPM.InvoiceDate;
                                }
                            }

                            if (myComparativeDate != null)
                            {
                                if (myPaymentTerm.CurrentMonth)
                                {
                                    myComparativeDate = myComparativeDate.Value.AddMonths(1);

                                    int dateYear = myComparativeDate.Value.Year;
                                    int dateMonth = myComparativeDate.Value.Month;
                                    int dateDay = myComparativeDate.Value.Day;
                                    int dateHour = myComparativeDate.Value.Hour;
                                    int dateMinute = myComparativeDate.Value.Minute;
                                    int dateSecond = myComparativeDate.Value.Second;

                                    myComparativeDate = new DateTime(dateYear, dateMonth, 1, dateHour, dateMinute, dateSecond);
                                }

                                DateTime? date = myComparativeDate.Value.AddDays(Convert.ToDouble(myPaymentTerm.Days));

                                if (entityPM.DueDate != date)
                                {
                                    entityPM.DueDate = date;
                                }
                            }
                        }
                    }
                }
            }

            if (entityPM.DueDate != null)
            {
                entityPM.DueDate = entityPM.DueDate.Value.Date;
            }
        }
        private void InitializeBranchField()
        {
            if (string.IsNullOrEmpty(entityPM.BranchId))
            {
                if (entityPM.CreatedByUserId != null)
                {
                    User user = (from d in myCommonContext.Users where d.Id == entityPM.CreatedByUserId && d.Tenant == tenant select d).FirstOrDefault();
                    if (user != null)
                    {
                        entityPM.BranchId = user.BranchId;
                    }
                }
            }
        }

        private List<VatType> allVatTypes = new List<VatType>();
        private List<VatTypePercentagePM> allVatPercentages = new List<VatTypePercentagePM>();
        private void InitializeVATs()
        {
            this.allVatTypes = this.vatTypeRepository.GetVatTypes(this.tenant).ToList();

            VatTypePercentageRepository vatTypePercentageRepository = new VatTypePercentageRepository(myCommonContext);
            VatTypePercentageQuery myVatTypePercentageQuery = new VatTypePercentageQuery(vatTypePercentageRepository);
            this.allVatPercentages = myVatTypePercentageQuery.GetVatTypePercentagePMByDate(tenant, TenantServerConfigration.GetCurrentDateTime(tenant).Date);
        }

        private void GenerateInvoiceNumber()
        {
            if (!entityPM.IsExternalAPI)
            {
                if (!entityPM.IsInvoiceNumberManuallySet)
                {
                    if (!entityPM.IsInvoiceNumberFromStock)
                    {
                       
                        if (string.IsNullOrEmpty(entityPM.InvoiceNumber) || entityPM.InvoiceNumber == entityPM.Id)
                        {
                            if (entityPM.IsConstituentInvoice)
                            {
                                entityPM.InvoiceNumber = TableCounter.GetNumber(tenant, "CNST", "CNS", null);
                            }

                            else if (entityPM.IsConsolidationInvoice)
                            {
                                entityPM.InvoiceNumber = TableCounter.GetNumber(tenant, "INVC", "CON", null);
                            }

                            else
                            { 
                                entityPM.InvoiceNumber = TableCounter.GetNumber(tenant, "INVC", entityPM.ARInvoiceTypeCode, null);
                            }
                        }
                    }
                }
            }
        }

        private void InitializeAmountDueFields()
        {
            if (entityPM.StatusCode == "AC" || entityPM.StatusCode == "AR")
            {
                entityPM.AmountDue = 0;
                entityPM.AmountDueInLocalCurrency = 0;
                entityPM.AmountDueInProfitCurrency = 0;
            }

            else
            {
                if (this.isNewEntity)
                {
                    entityPM.AmountDue = entityPM.AmountInInvoiceCurrency == null ? 0 : entityPM.AmountInInvoiceCurrency.Value;
                    entityPM.AmountDueInLocalCurrency = entityPM.AmountDueInLocalCurrency == null ? 0 : entityPM.AmountDueInLocalCurrency.Value;
                    entityPM.AmountDueInProfitCurrency = entityPM.AmountDueInProfitCurrency == null ? 0 : entityPM.AmountDueInProfitCurrency.Value;
                }
                else
                {


                    bool isPaymentsChanged = false;

                    if (invoicePaymentsChangeSet.Where(d => d.ChangeSetOp == ChangeSetOperation.Insert || d.ChangeSetOp == ChangeSetOperation.Delete).Count() > 0)
                    {
                        isPaymentsChanged = true;
                    }

                    if (!isPaymentsChanged)
                    {
                        if (entityPM.StatusCode == "PP" || entityPM.StatusCode == "PD")
                        {

                        }

                        else
                        {
                            entityPM.AmountDue = entityPM.AmountInInvoiceCurrency == null ? 0 : entityPM.AmountInInvoiceCurrency.Value;
                            entityPM.AmountDueInLocalCurrency = entityPM.AmountDueInLocalCurrency == null ? 0 : entityPM.AmountDueInLocalCurrency.Value;
                            entityPM.AmountDueInProfitCurrency = entityPM.AmountDueInProfitCurrency == null ? 0 : entityPM.AmountDueInProfitCurrency.Value;
                        }
                    }
                }
            }
        }

        private void InitializeCustomerWorkingDates()
        {
            if (isNewEntity)
            {
                if (!entityPM.IsConstituentInvoice)
                {
                    if (!string.IsNullOrEmpty(entityPM.BillToId))
                    {
                        CardRepository cardRepository = new CardRepository(entityPM.Tenant);
                        Card card = cardRepository.GetSingleCard(entityPM.BillToId, entityPM.Tenant);
                        if (card != null)
                        {
                            if (card.PartnerTypeId == "CS" || card.PartnerTypeId == "PO")
                            {
                                CustomerRepository customerRepository = new CustomerRepository(entityPM.Tenant);
                                Customer customer = customerRepository.GetSingleCustomer(entityPM.BillToId, entityPM.Tenant, false);

                                if (customer != null)
                                {
                                    DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant).Date;

                                    if (customer.FirstInvoiceDate == null)
                                    {
                                        customer.FirstInvoiceDate = todayDate;
                                        customerRepository.Update(customer);
                                        customerRepository.SubmitChanges();

                                        CustomerPM dummyPM = new CustomerPM()
                                        {
                                            CreatedByUserId = card.CreatedByUserId,
                                            SalesmanUserId = customer.SalesmanUserId,
                                            EnglishName = card.EnglishName,
                                            VatNumber = card.VatNumber,
                                        };

                                        //CustomerQuery customerQuery = new CustomerQuery(customerRepository);
                                        //CustomerPM customerpm = customerQuery.GetSinglePM(customer.Id, entityPM.Tenant);
                                        CustomerEmailAlert customerEmailAlert = new CustomerEmailAlert();
                                        customerEmailAlert.SendEmailAlert(dummyPM, entityPM.Tenant, "GCFI", false);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        List<ARInvoice> allConnectedInvoices;
        private void DisconnectAllConnectedInvoices()
        {
            this.allConnectedInvoices = invoiceRepository.GetConnectedInvoices(tenant, entityPM.Id).ToList();

            if (allConnectedInvoices.Count > 0)
            {
                foreach (ARInvoice item in allConnectedInvoices)
                {
                    item.IsClosed = false;
                    item.StatusCode = "NT";
                    item.ConsolidationInvoiceId = null;

                    invoiceRepository.Update(item);

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = tenant,
                        EventTypeCode = "INDS",
                        UserId = loggedContactId,
                        EntityId = item.Id,
                        ObjectTableName = "ARInvoice",
                    });
                }

                invoiceRepository.SubmitChanges();
            }
        }

        #endregion

        #region Transfer
        private bool isInitializingExternalFields;
        private void InitializeTransferComponents()
        {
            this.InitializeExternalFields();
            this.InitializeTransferFields();
            this.InitializeGLAccountFields();

            if (this.entityPM.SetApproved && string.IsNullOrEmpty(entityPM.TransferError))
            {
                if (this.isTransferToDropbox && this.TransferToDropboxActivated)
                {
                    this.entityPM.TransferStatusCode = "TR";
                }
            }
        }
        private void InitializeExternalFields()
        {
            bool isInitializing = false;

            if (entityPM.StatusCode != null && entityPM.StatusCode != "DR")
            {
                isInitializing = true;
            }

            else if (entityPM.SetApproved)
            {
                isInitializing = true;
            }

            if (isInitializing)
            {
                this.isInitializingExternalFields = true;

                ChargeTypeAccountingRepository chargeTypeAccountingRepository = new ChargeTypeAccountingRepository(tenant);
                IQueryable<ChargeTypeAccounting> iQueryable_ChargeTypeAccounting = chargeTypeAccountingRepository.GetChargeTypeAccountings(tenant);

                #region Invoice
                if (FieldIsEmpty(entityPM.DebitAccount))
                {
                    if (isJournal)
                    {
                        Card myCard = CardRepository.GetSingleCard(entityPM.BillToId, tenant, true);
                        AccountingSystemHelper accountingSystemHelper = new AccountingSystemHelper();
                        entityPM.DebitAccount = accountingSystemHelper.GetGenericCreditAccount(myCard.Id, entityPM.InvoiceCurrencyId, tenant, false);
                    }

                    else if (isExternal)
                    {
                        CardExternalCodeByCurrencyRepository cardExternalCodeByCurrencyRepository = new CardExternalCodeByCurrencyRepository(tenant);
                        IQueryable<CardExternalCodeByCurrency> iQueryable_CardExternals = cardExternalCodeByCurrencyRepository.GetCardExternalCodeByCurrenciesByTenant(tenant);

                        CardExternalCodeByCurrency myCardExternal = (from d in iQueryable_CardExternals where d.CardId == entityPM.BillToId && d.CurrencyId == entityPM.InvoiceCurrencyId select d).FirstOrDefault();
                        if (myCardExternal != null)
                        {
                            entityPM.DebitAccount = myCardExternal.ExternalRecievableTableId;
                        }
                    }
                }

                if (FieldIsEmpty(entityPM.AccountingExternalCode))
                {
                    Currency myCurrency = CurrencyRepository.GetSingleCurrency(entityPM.InvoiceCurrencyId, tenant, true);
                    entityPM.AccountingExternalCode = myCurrency.AccountingExternalCode;
                }

                if (FieldIsEmpty(entityPM.PaymentTermExternalId))
                {
                    if (!string.IsNullOrEmpty(entityPM.PaymentTermId))
                    {
                        PaymentTermRepository paymentTermRepository = new PaymentTermRepository(tenant);
                        PaymentTerm myPaymentTerm = paymentTermRepository.GetSinglePaymentTerm(entityPM.PaymentTermId, tenant);
                        entityPM.PaymentTermExternalId = myPaymentTerm.ExternalId;
                    }
                }
                #endregion

                #region Lines

                List<ARInvoiceLinePM> lines = new List<ARInvoiceLinePM>();

                if (isNewEntity)
                {
                    lines = entityPM.InvoiceLines.ToList();
                }

                else
                {
                    lines = invoiceLinesChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
                }

                foreach (ARInvoiceLinePM line in lines)
                {
                    if (FieldIsEmpty(line.CreditAccount))
                    {
                        ChargesType myChargesType = ChargesTypeRepository.GetSingleChargesType(line.ChargesTypeId, tenant, true);

                        if (isJournal)
                        {
                            if (myChargesType.AccountingVATSplit)
                            {
                                ChargeTypeAccounting myChargeTypeAccounting = (from d in iQueryable_ChargeTypeAccounting where d.ChargeTypeId == line.ChargesTypeId && d.VatTypeId == line.VatTypeId select d).FirstOrDefault();
                                if (myChargeTypeAccounting != null)
                                {
                                    line.CreditAccount = myChargeTypeAccounting.ReceivableCreditAccount;
                                }
                            }

                            else
                            {
                                line.CreditAccount = myChargesType.ReceivableCreditAccount;
                            }
                        }

                        else
                        {
                            line.CreditAccount = myChargesType.ReceivablesChargesTypeExternalCode;
                        }
                    }

                    if (FieldIsEmpty(line.ExternalVATCard))
                    {
                        if (this.accountingSetting.AccountingSystemCode == "HV" || this.accountingSetting.AccountingSystemCode == "RH")
                        {
                            line.ExternalVATCard = this.accountingSetting.ReceivableVATCard;
                        }

                        else
                        {
                            VatType vatType = VatTypeRepository.GetSingleVatType(line.VatTypeId, tenant, true);

                            if(vatType != null)
                            {
                                line.ExternalVATCard = vatType.ReceivablesExternalId;
                            }
                        }
                    }

                    if (!isNewEntity)
                    {
                        if (line.ChangeSetOp == ChangeSetOperation.None)
                        {
                            UpdateInvoiceLine(line);
                        }
                    }
                }
                #endregion
            }
        }
        private void InitializeTransferFields()
        {

            bool isInitializing = true;

            if (entityPM.IsConstituentInvoice)
            {
                isInitializing = false;
                entityPM.TransferError = "Constituent Invoice";
                entityPM.TransferStatusCode = "NR";
            }

            else if (entityPM.IsTransferStatusSetManually)
            {
                isInitializing = false;
            }

            else if (entityPM.SetReTransfer)
            {
                isInitializing = true;
            }

            else if (invoice.TransferStatusCode == "TR")
            {
                isInitializing = false;
                entityPM.TransferError = null;
                entityPM.TransferStatusCode = "TR";
            }

            else if (entityPM.TransferStatusCode == "BL")
            {
                isInitializing = false;
                entityPM.TransferError = null;
                entityPM.TransferStatusCode = "BL";
            }

            else if (entityPM.TransferStatusCode == "IP" || entityPM.TransferStatusCode == "ET")
            {
                isInitializing = false;
            }

            if (isInitializing)
            {
                #region

                bool isReady = true;
                string myError = null;
                string ExternalCodeError = "Currency External Code is missing";
                string paymentTermError = "Payment Term External Id is missing";
                string vatError = "External VAT Card is missing";
                string linesError = " Charge Type Receivable Credit Account is missing";

                if (FieldIsEmpty(entityPM.DebitAccount))
                {
                    isReady = false;
                    myError = "Bill to Debit Account is missing";
                }

                if (isExternal)
                {
                    if (FieldIsEmpty(entityPM.PaymentTermExternalId))
                    {
                        isReady = false;
                        if (!FieldIsEmpty(entityPM.PaymentTermName))
                        {
                            paymentTermError = "Payment Term: " + entityPM.PaymentTermName + ". External Id is missing";
                        }
                        myError = string.IsNullOrEmpty(myError) ? paymentTermError : myError + "," + paymentTermError;
                    }
                }

                else
                {
                    if (FieldIsEmpty(entityPM.AccountingExternalCode))
                    {
                        isReady = false;
                        if (!FieldIsEmpty(entityPM.AccountingExternalName))
                        {
                            ExternalCodeError = "External Code: " + entityPM.AccountingExternalName + " is missing";
                        }
                        myError = string.IsNullOrEmpty(myError) ? ExternalCodeError : myError + "," + ExternalCodeError;
                    }
                }

                List<ARInvoiceLinePM> myLines = new List<ARInvoiceLinePM>();
                if (isNewEntity)
                {
                    myLines = entityPM.InvoiceLines.ToList();
                }

                else
                {
                    myLines = invoiceLinesChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
                }

                if (myLines.Count == 0)
                {
                    isReady = false;
                    myError = string.IsNullOrEmpty(myError) ? linesError : myError + "," + linesError;
                }

                else
                {
                    List<ARInvoiceLinePM> arInvoiceLines_CreditError = myLines.Where(d => d.CreditAccount == null || (d.CreditAccount != null && string.IsNullOrEmpty(d.CreditAccount.Trim()))).ToList();
                    if (arInvoiceLines_CreditError != null && arInvoiceLines_CreditError.Count() > 0)
                    {
                        isReady = false;
                        foreach (ARInvoiceLinePM item in arInvoiceLines_CreditError)
                        {
                            ChargesType myChargesType = ChargesTypeRepository.GetSingleChargesType(item.ChargesTypeId, tenant, true);
                            string myChargesTypeName = myChargesType != null ? myChargesType.EnglishName : "";
                            myError = string.IsNullOrEmpty(myError) ? myChargesTypeName + linesError : myError + "," + myChargesTypeName + linesError;
                        }
                    }

                    var myGroup = (from a in myLines
                                   where a.VatTypeId != null
                                   && a.VatPercentage != null
                                   && a.VatPercentage != 0
                                   group a by new { a.VatTypeId, a.VatPercentage, a.ExternalVATCard } into g
                                   select new
                                   {
                                       VatTypeId = g.Key.VatTypeId,
                                       VatPercentage = g.Key.VatPercentage,
                                       ExternalVATCard = g.Key.ExternalVATCard,
                                   });

                    foreach (var g in myGroup)
                    {
                        if (FieldIsEmpty(g.ExternalVATCard))
                        {
                            VatType lineVatType = this.allVatTypes.Where(d => d.Id == g.VatTypeId).FirstOrDefault();
                            var lineVatTypeName = lineVatType != null ? lineVatType.EnglishName : "";
                            isReady = false;
                            vatError = lineVatTypeName + " VAT External Id is missing";
                            myError = string.IsNullOrEmpty(myError) ? vatError : myError + "," + vatError;
                        }
                    }
                }

                if (isReady)
                {
                    entityPM.TransferStatusCode = "RD";
                    entityPM.TransferError = null;
                }

                else
                {
                    entityPM.TransferStatusCode = "NR";
                    if (!IsFullAccountingActivated(entityPM.Tenant))
                    {
                        entityPM.TransferError = myError;
                    }
                }
                #endregion
            }

        }
        Tenant tenantPOCO;
        private void InitializeGLAccountFields()
        {
            if (entityPM.SetApproved)
            {
                TenantRepository tenantRepository = new TenantRepository(tenant);
                tenantPOCO = tenantRepository.GetSingleTenant(tenant);

                if (tenantPOCO.AccountingActivated)
                {
                    List<ARInvoiceLinePM> lines = new List<ARInvoiceLinePM>();

                    if (isNewEntity)
                    {
                        lines = entityPM.InvoiceLines.ToList();
                    }

                    else
                    {
                        lines = invoiceLinesChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
                    }

                    ChargeTypeAccountingRepository chargeTypeAccountingRepository = new ChargeTypeAccountingRepository(tenant);
                    IQueryable<ChargeTypeAccounting> iQueryable_ChargeTypeAccounting = chargeTypeAccountingRepository.GetChargeTypeAccountings(tenant);



                    foreach (ARInvoiceLinePM line in lines)
                    {
                        ChargesType myChargesType = ChargesTypeRepository.GetSingleChargesType(line.ChargesTypeId, tenant, true);

                        if (line.GLAccountId == null)
                        {

                            if (myChargesType.AccountingVATSplit)
                            {
                                ChargeTypeAccounting myChargeTypeAccounting = (from d in iQueryable_ChargeTypeAccounting where d.ChargeTypeId == line.ChargesTypeId && d.VatTypeId == line.VatTypeId select d).FirstOrDefault();
                                if (myChargeTypeAccounting != null)
                                {
                                    line.GLAccountId = myChargeTypeAccounting.ReceivableCreditGLAccountId;
                                }
                            }

                            else
                            {
                                line.GLAccountId = myChargesType.ReceivableCreditGLAccountId;
                            }

                            if (string.IsNullOrEmpty(line.GLAccountId))
                            {
                                if (myChargesType.Code == "INT")
                                {
                                    bool showLocals = LoggedContactResolver.GetLoggedContactShowLocal(tenant);
                                    var msg = TextCodesTranslator.TranslateText("ARInvoice.O.TheReceivableGLAccountOfTheChargeNULL", tenant, showLocals);

                                    throw new Exception(msg);

                                }
                                else
                                {
                                    throw new Exception("The Receivable GLAccount of the Charge Type " + myChargesType.EnglishName + " is NULL");

                                }
                            }
                        }
                    }
                }
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

        #region Get Shipments Data
        private void GetShipmentsData(List<ARInvoiceLinePM> lines)
        {
            this.allShipmentIds = (from d in lines group d by d.EntityId into g select g.Key).ToList();
            this.allActiveShipmentIds = (from d in lines where d.ChangeSetOp != ChangeSetOperation.Delete group d by d.EntityId into g select g.Key).ToList();
            this.allShipments = shipmentRepository.GetShipmentsListFromIdList(allShipmentIds, tenant);
            this.allReceivables = shipmentReceivableRepository.GetShipmentReceivablesByEntityIds(allShipmentIds, entityPM.Tenant);

            // Ayman
            // Open shipment Receivables screen: issue new invoice or update draft invoice with new lines
            // keeps the invoice screen opened without saving
            // Open same shipment in new session and delete those receivables and save the shipment
            // return to the invoice screen session and save this invoice
            if (!this.entityPM.IsConsolidationInvoice && !this.entityPM.IsGeneralInvoice)
            {
                List<ARInvoiceLinePM> newLines = new List<ARInvoiceLinePM>();

                if (isNewEntity)
                {
                    newLines = lines.Where(d => d.ReceivableId != null).ToList();
                }

                else
                {
                    newLines = lines.Where(d => d.ChangeSetOp == ChangeSetOperation.Insert && d.ReceivableId != null).ToList();
                }

                if (newLines.Count > 0)
                {
                    foreach (ARInvoiceLinePM item in newLines)
                    {
                        ShipmentReceivable myReceivable = this.allReceivables.Where(d => d.Id == item.ReceivableId).FirstOrDefault();
                        if (myReceivable == null)
                        {
                            throw new ApplicationException("Some of invoice lines are missing receivables");
                        }
                    }
                }
            }

            Shipment shipment = allShipments.Where(d => d.Id == entityPM.MainEntityId).FirstOrDefault();
            if (shipment != null)
            {
                shipment.ConcurrencyGUID = Guid.NewGuid().ToString();

                if (shipment.IsNewARInvoiceBlocked)
                {
                    shipment.IsNewARInvoiceBlocked = false;
                }

                if (string.IsNullOrEmpty(entityPM.MainEntityReference))
                {
                    entityPM.MainEntityReference = shipment.ShipmentNumber;
                }

                shipmentRepository.Update(shipment);
                shipmentRepository.SubmitChanges();

                if (this.isNewEntity)
                {
                    ShipmentDataView f = shipmentRepository.GetSingleShipmentDataView(shipment.Id, tenant);

                    switch (shipment.DirectionId)
                    {
                        case "E": { this.entityPM.Description = "Export to " + f.MainCarriageFinalDestinationPortCode; break; }
                        case "I": { this.entityPM.Description = "Import from " + f.MainCarriageFromPortCode; break; }
                        case "D": { this.entityPM.Description = "Ship to " + f.MainCarriageToCity; break; }
                    }
                }
            }
        }
        #endregion

        #region Receivables

        private void UpdateReceivable(ARInvoiceLinePM item)
        {
            if (!entityPM.IsAutoCredit && !entityPM.IsCancelled)
            {
                if (!string.IsNullOrEmpty(item.ReceivableId))
                {
                    ShipmentReceivable myReceivable = allReceivables.Where(d => d.Id == item.ReceivableId && d.Tenant == item.Tenant).FirstOrDefault();

                    if (myReceivable != null)
                    {
                        Shipment shipment = allShipments.Where(d => d.Id == myReceivable.ShipmentId).FirstOrDefault();
                        string shipmentProfitCurrencyId = shipment.ProfitCurrencyId;

                        myReceivable.ShipmentReceivableLineStatusCode = this.GetLineStatusCode(myReceivable);
                        myReceivable.ARInvoiceLineId = item.Id;
                        myReceivable.ARInvoiceId = entityPM.Id;
                        myReceivable.Quantity = item.Quantity;
                        myReceivable.UnitPrice = item.UnitPrice;
                        myReceivable.Rate = item.ForiegnExchangeRate;
                        myReceivable.ProfitCurrencyExchangeRate = entityPM.ProfitCurrencyExchangeRate;


                        // Amount
                        double? TotalAmount = 0;
                        if (item.MeasurementCode == "STFE")
                        {
                            TotalAmount = item.ForiegnCurrencyAmount;
                        }

                        else
                        {
                            if (item.Quantity != null && item.UnitPrice != null)
                            {
                                if (string.IsNullOrEmpty(item.MeasurementCode))
                                {
                                    if (item.MeasurementId != null)
                                    {
                                        Measurement myMeasurement = (from d in myCommonContext.Measurements
                                                                     where d.Id == item.MeasurementId
                                                                     && d.Tenant == tenant
                                                                     select d).FirstOrDefault();

                                        if (myMeasurement != null)
                                        {
                                            item.MeasurementCode = myMeasurement.Code;
                                        }
                                    }
                                }

                                if (item.MeasurementCode == "PRVL" || item.MeasurementCode == "PRFR" || item.MeasurementCode == "PFCL")
                                {
                                    var price = item.UnitPrice / 100;
                                    TotalAmount = item.Quantity * price;
                                }

                                else
                                {
                                    TotalAmount = item.Quantity * item.UnitPrice;
                                }

                                /* MinMax Quote */
                                if (TotalAmount != null)
                                {
                                    if (myReceivable.QuoteSaleMinAmount != null)
                                    {
                                        if (TotalAmount < myReceivable.QuoteSaleMinAmount)
                                        {
                                            TotalAmount = myReceivable.QuoteSaleMinAmount;
                                        }
                                    }

                                    if (myReceivable.QuoteSaleMaxAmount != null)
                                    {
                                        if (TotalAmount > myReceivable.QuoteSaleMaxAmount)
                                        {
                                            TotalAmount = myReceivable.QuoteSaleMaxAmount;
                                        }
                                    }
                                }
                            }
                        }

                        myReceivable.TotalAmount = MethodHelper.Round(TotalAmount, 2);

                        // Amount Local
                        if (myReceivable.CurrencyId == this.TenantObject.CurrencyId)
                        {
                            myReceivable.TotalAmountLocal = myReceivable.TotalAmount;
                        }

                        else
                        {
                            myReceivable.TotalAmountLocal = MethodHelper.Round(myReceivable.TotalAmount * myReceivable.Rate, 2);
                        }

                        // Amount Profit
                        if (myReceivable.CurrencyId == shipmentProfitCurrencyId)
                        {
                            myReceivable.AmountInProfitCurrency = myReceivable.TotalAmount;
                        }

                        else
                        {
                            myReceivable.AmountInProfitCurrency = MethodHelper.Round(myReceivable.TotalAmountLocal / myReceivable.ProfitCurrencyExchangeRate, 2);
                        }

                        CalculateReceivableVatAmountFromInvoice(myReceivable, item);
                        shipmentReceivableRepository.Update(myReceivable);
                    }
                }
            }
        }

        private string GetLineStatusCode(ShipmentReceivable myReceivable)
        {

            /*
             
                AC	Auto Credit	AC,Auto Credit
                AD	Unpaid	AD,Unpaid
                AR	Auto Credited	AR,Auto Credited
                CN	Connected	CN,Connected
                DR	Draft	DR,Draft
                LL	Cancelled	LL,Cancelled
                NT	Not Connected	NT,Not Connected
                PD	Paid	PD,Paid
                PP	Partially Paid	PP,Partially Paid
                VD	Void	VD,Void

            */

            /*
            
                ACCT	Closed	acct,closed
                APPD	Approved	appd,approved
                DRFT	Draft	drft,draft
                EMPT	Empty	empt,empty
                OAMT	Open Amount	oamt,open amount

            */

            string myResult = "DRFT";

            if (entityPM.IsConstituentInvoice)
            {
                myResult = "OAMT";
            }

            else
            {
                switch (entityPM.StatusCode)
                {
                    case "NT":
                    case "CN":
                        {
                            if (this.isNewEntity)
                            {
                                //myResult = "OAMT";
                                myResult = "ACCT";
                            }

                            break;
                        }

                    case "AD":
                    case "PD":
                    case "PP":
                        {
                            myResult = "ACCT";
                            break;
                        }

                    case "VD":
                    case "LL":
                        {
                            myResult = "OAMT";
                            break;
                        }
                }
            }

            return myResult;
        }
        private void DisconnectReceivable(string receivableId)
        {
            ShipmentReceivable myReceivable = (from a in allReceivables where a.Id == receivableId select a).FirstOrDefault();
            if (myReceivable != null)
            {
                myReceivable.ARInvoiceLineId = null;
                myReceivable.ARInvoiceId = null;
                myReceivable.ShipmentReceivableLineStatusCode = "OAMT";
                this.CalculateReceivableVatAmount(myReceivable);
                shipmentReceivableRepository.Update(myReceivable);

                List<ShipmentReceivable> ChildReceivables = shipmentReceivableRepository.GetShipmentReceivablesByParentId(receivableId, tenant);
                foreach (ShipmentReceivable myChild in ChildReceivables)
                {
                    myChild.ARInvoiceLineId = null;
                    myChild.ARInvoiceId = null;
                    myChild.ShipmentReceivableLineStatusCode = "OAMT";
                    shipmentReceivableRepository.Update(myChild);
                }

                shipmentReceivableRepository.SubmitChanges();
            }
        }
        private void CalculateReceivableVatAmount(ShipmentReceivable receivable)
        {
            string receivableVatTypeId = GetReceivableVatTypeId(receivable);
            if (!string.IsNullOrEmpty(receivableVatTypeId))
            {
                this.InitializeVATs();
                VatType lineVatType = this.allVatTypes.Where(d => d.Id == receivableVatTypeId).FirstOrDefault();
                if (!lineVatType.IsMultiPercentage)
                {
                    var vatTypePercentagePM = this.allVatPercentages.Where(d => d.VatTypeId == receivableVatTypeId).FirstOrDefault();
                    if (vatTypePercentagePM != null)
                    {
                        var percentage = vatTypePercentagePM.Percentage;
                        receivable.VatAmountLocal = MethodHelper.Round(receivable.TotalAmountLocal + (receivable.TotalAmountLocal * percentage / 100), 2);
                        receivable.VatAmountProfit = MethodHelper.Round(receivable.AmountInProfitCurrency + (receivable.AmountInProfitCurrency * percentage / 100), 2);
                    }
                }
                else
                {
                    receivable.VatAmountLocal = CalculateReceivableVatAmountInMultiVat_OpenLine(lineVatType.Id, receivable.TotalAmountLocal);
                    receivable.VatAmountProfit = CalculateReceivableVatAmountInMultiVat_OpenLine(lineVatType.Id, receivable.AmountInProfitCurrency);
                }
            }
        }

        private string GetReceivableVatTypeId(ShipmentReceivable receivable)
        {
            string receivableVatTypeId = null;
            Shipment shipment = shipmentRepository.GetSingleShipment(receivable.ShipmentId, receivable.Tenant);
            if (!string.IsNullOrEmpty(shipment.CustomerId))
            {
                Card myCard = CardRepository.GetSingleCard(shipment.CustomerId, tenant, false);
                if (myCard != null)
                    receivableVatTypeId = myCard.VatTypeId;
            }

            if (string.IsNullOrEmpty(receivableVatTypeId))
                receivableVatTypeId = receivable.VatTypeId;

            return receivableVatTypeId;
        }

        private double? CalculateReceivableVatAmountInMultiVat_OpenLine(string vatTypeId, double? amount)
        {
            double? vatAmount = amount;
            List<VATTypesGroup> allVATTypesGroup = (from d in this.myCommonContext.VATTypesGroups where d.Tenant == this.tenant select d).ToList();
            List<VATTypesGroup> vatTypesGroup = allVATTypesGroup.Where(d => d.GroupVATTypeId == vatTypeId).ToList();
            foreach (VATTypesGroup itemGroup in vatTypesGroup)
            {
                VatTypePercentagePM myPercentagePM = this.allVatPercentages.Where(d => d.VatTypeId == itemGroup.SingleVATTypeId).FirstOrDefault();
                if (myPercentagePM != null)
                {
                    var vatTypePercentage = MethodHelper.GetValue(myPercentagePM.Percentage);
                    vatAmount = vatAmount + (amount * vatTypePercentage / 100);
                }
            }
            return MethodHelper.Round(vatAmount, 2);
        }

        private void CalculateReceivableVatAmountFromInvoice(ShipmentReceivable myReceivable, ARInvoiceLinePM arinvoiceline)
        {
            VatType lineVatType = this.allVatTypes.Where(d => d.Id == arinvoiceline.VatTypeId).FirstOrDefault();
            if (lineVatType != null)
            {
                if (!lineVatType.IsMultiPercentage)
                {
                    FillReceivableVatAmounts_SingleVat(myReceivable, arinvoiceline);
                   
                }
                else
                {
                    FillReceivableVatAmounts_MultiVat(myReceivable, arinvoiceline);
                }
            }
        }
        private void FillReceivableVatAmounts_SingleVat(ShipmentReceivable myReceivable, ARInvoiceLinePM arinvoiceline)
        {
            myReceivable.VatAmountLocal = MethodHelper.Round(arinvoiceline.LocalCurrencyAmount + arinvoiceline.LocalCurrencyAmount * arinvoiceline.VatPercentage / 100, 2);
            myReceivable.VatAmountProfit = MethodHelper.Round(arinvoiceline.ProfitCurrencyAmount + arinvoiceline.ProfitCurrencyAmount * arinvoiceline.VatPercentage / 100, 2);
            if (arinvoiceline.IsRegionalTax)
            {
                myReceivable.VatAmountLocal = MethodHelper.Round((myReceivable.VatAmountLocal + myReceivable.VatAmountLocal * (entityPM.RegionalTaxPercentage / 100)), 2);
                myReceivable.VatAmountProfit = MethodHelper.Round((myReceivable.VatAmountProfit + myReceivable.VatAmountProfit * (entityPM.RegionalTaxPercentage / 100)), 2);
            }
        }
        private void FillReceivableVatAmounts_MultiVat(ShipmentReceivable myReceivable, ARInvoiceLinePM arinvoiceline)
        {
            List<VATTypesGroup> vatTypesGroup = allVatGroups.Where(d => d.GroupVATTypeId == arinvoiceline.VatTypeId).ToList();
            myReceivable.VatAmountLocal = arinvoiceline.LocalCurrencyAmount;
            myReceivable.VatAmountProfit = arinvoiceline.ProfitCurrencyAmount;
            foreach (VATTypesGroup itemGroup in vatTypesGroup)
            {
                VatType vatType = this.allVatTypes.Where(d => d.Id == itemGroup.SingleVATTypeId).FirstOrDefault();
                VatTypePercentagePM myPercentagePM = this.allVatPercentages.Where(d => d.VatTypeId == itemGroup.SingleVATTypeId).FirstOrDefault();
                if (myPercentagePM != null)
                {
                    var vatTypePercentage = MethodHelper.GetValue(myPercentagePM.Percentage);
                    myReceivable.VatAmountLocal = myReceivable.VatAmountLocal + MethodHelper.Round(arinvoiceline.LocalCurrencyAmount * vatTypePercentage / 100, 2);
                    myReceivable.VatAmountProfit = myReceivable.VatAmountProfit + MethodHelper.Round(arinvoiceline.ProfitCurrencyAmount * vatTypePercentage / 100, 2);
                }
            }
        }
        #endregion

        #region Invoice Entities
        private void UpdateInvoiceEntities()
        {
            List<ARInvoiceEntity> dbEntities = invoiceEntityRepository.GetInvoiceEntitiesForInvoice(entityPM.Id, entityPM.Tenant).ToList();

            if (invoice.StatusCode != "VD" && entityPM.StatusCode == "VD")
            {
                foreach (ARInvoiceEntity item in dbEntities)
                {
                    invoiceEntityRepository.Remove(item);
                }
            }

            else if (invoice.StatusCode != "AD")
            {
                if (isNewEntity)
                {
                    this.CreateInvoiceEntities();
                }

                else if (entityPM.ARInvoiceTypeCode == "MN")
                {
                    foreach (ARInvoiceEntity item in dbEntities)
                    {
                        invoiceEntityRepository.Remove(item);
                    }

                    this.CreateInvoiceEntities();
                }
            }
        }

        private void CreateInvoiceEntities()
        {
            List<ObjectTable> tables = new ObjectTableRepository(entityPM.Tenant).context.ObjectTables.Where(d => (d.Tenant == 0 || d.Tenant == entityPM.Tenant) && (d.Name == "Shipment" || d.Name == "Master")).ToList();
            ObjectTable masterObjectTable = tables.Where(d => d.Name == "Master").FirstOrDefault();
            ObjectTable shipmentObjectTable = tables.Where(d => d.Name == "Shipment").FirstOrDefault();

            string masterObjectTableId = null;
            string shipmentObjectTableId = null;

            if (masterObjectTable != null)
            {
                masterObjectTableId = masterObjectTable.Id;
            }

            if (shipmentObjectTable != null)
            {
                shipmentObjectTableId = shipmentObjectTable.Id;
            }

            foreach (string entityId in allActiveShipmentIds)
            {
                string entityReference = null;
                string entityObjectTableId = null;

                Shipment myEntity = allShipments.Where(d => d.Id == entityId).FirstOrDefault();
                if (myEntity != null)
                {
                    entityReference = myEntity.ShipmentNumber;
                    entityObjectTableId = myEntity.ShipmentLevelCode == "C" ? masterObjectTableId : shipmentObjectTableId;
                }
                if (entityId != null)
                {

                    ARInvoiceEntity invoiceEntity = new ARInvoiceEntity()
                    {
                        Id = IdCounter.GetNumber("ARInvoiceEntity", entityPM.Tenant).ToString(),
                        Tenant = tenant,
                        ARInvoiceId = entityPM.Id,
                        EntityId = entityId,
                        EntityReference = entityReference,
                        ObjectTableId = entityObjectTableId,
                    };
                    invoiceEntityRepository.Add(invoiceEntity);
                }


            }
        }
        #endregion

        #region Total Vats
        List<InvoiceTotalsClass> group_data;
        private void UpdateTotalVats()
        {
            if (isUpdateTotalVats)
            {
                double? subTotal = 0;
                double? subTotal_Local = 0;
                double? sumOfVATsAmounts = 0;
                double? sumOfVATsAmounts_Local = 0;
                double? sumOfVATsAmounts_Profit = 0;
                double? Amount = 0;
                double? Amount_Local = 0;
                double? Amount_Profit = 0;

                List<ARInvoiceTotalVAT> dbTotalVats = invoiceTotalVatRepository.GetInvoiceTotalVatsForInvoice(entityPM.Id, entityPM.Tenant).ToList();
                foreach (ARInvoiceTotalVAT item in dbTotalVats)
                {
                    invoiceTotalVatRepository.Remove(item);
                }

                List<ARInvoiceLinePM> myDataLines = entityPM.InvoiceLines.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete && d.VatTypeId != null).ToList();
                if (myDataLines.Count > 0)
                {
                    subTotal = MethodHelper.Round(myDataLines.Sum(s => s.InvoiceCurrencyAmount), 2);
                    subTotal_Local = MethodHelper.Round(myDataLines.Sum(s => s.LocalCurrencyAmount), 2);

                    #region
                    List<VATTypesGroup> allVatGroups = (from d in myCommonContext.VATTypesGroups
                                                        where d.Tenant == this.tenant
                                                        select d).ToList();

                    List<InvoiceTotalsClass> group_Source = new List<InvoiceTotalsClass>();

                    foreach (ARInvoiceLinePM item in myDataLines)
                    {
                        #region
                        VatType lineVatType = this.allVatTypes.Where(d => d.Id == item.VatTypeId).FirstOrDefault();

                        if (lineVatType != null)
                        {
                            if (!lineVatType.IsMultiPercentage)
                            {
                                InvoiceTotalsClass newItem = new InvoiceTotalsClass()
                                {
                                    Id = item.VatTypeId,
                                    VatTypeId = item.VatTypeId,
                                    VatTypePercentage = item.VatPercentage,
                                    LocalCurrencyAmount = item.LocalCurrencyAmount,
                                    InvoiceCurrencyAmount = item.InvoiceCurrencyAmount,
                                    ProfitCurrencyAmount = item.ProfitCurrencyAmount,
                                    ExternalVatCard = item.ExternalVATCard,
                                    ExternalTAXItemId = lineVatType.ExternalTAXItemId,
                                };

                                if (string.IsNullOrEmpty(newItem.ExternalVatCard))
                                {
                                    if (this.accountingSetting.AccountingSystemCode == "HV" || this.accountingSetting.AccountingSystemCode == "RH")
                                    {
                                        newItem.ExternalVatCard = this.accountingSetting.ReceivableVATCard;
                                    }
                                    else
                                    {
                                        newItem.ExternalVatCard = lineVatType.ReceivablesExternalId;
                                    }
                                }

                                if (item.IsRegionalTax)
                                {
                                    newItem.LocalCurrencyAmount = item.LocalCurrencyAmount + newItem.LocalCurrencyAmount * (entityPM.RegionalTaxPercentage / 100);
                                    newItem.InvoiceCurrencyAmount = item.InvoiceCurrencyAmount + item.InvoiceCurrencyAmount * (entityPM.RegionalTaxPercentage / 100);
                                    newItem.ProfitCurrencyAmount = item.ProfitCurrencyAmount + item.ProfitCurrencyAmount * (entityPM.RegionalTaxPercentage / 100);

                                    InvoiceTotalsClass newRegionalTaxItem = new InvoiceTotalsClass()
                                    {
                                        Id = entityPM.RegionalTaxId,
                                        VatTypeId = entityPM.RegionalTaxId,
                                        VatTypePercentage = entityPM.RegionalTaxPercentage,
                                        LocalCurrencyAmount = item.LocalCurrencyAmount,
                                        InvoiceCurrencyAmount = item.InvoiceCurrencyAmount,
                                        ProfitCurrencyAmount = item.ProfitCurrencyAmount,
                                        ExternalVatCard = newItem.ExternalVatCard,
                                        ExternalTAXItemId = newItem.ExternalTAXItemId,
                                        IsRegionalTax = true,
                                    };

                                    group_Source.Add(newRegionalTaxItem);
                                }

                                group_Source.Add(newItem);
                            }

                            else
                            {
                                List<VATTypesGroup> myVatGroups = allVatGroups.Where(d => d.GroupVATTypeId == item.VatTypeId).ToList();
                                foreach (VATTypesGroup itemGroup in myVatGroups)
                                {
                                    InvoiceTotalsClass newItem = new InvoiceTotalsClass()
                                    {
                                        Id = itemGroup.SingleVATTypeId,
                                        VatTypeId = itemGroup.SingleVATTypeId,
                                        LocalCurrencyAmount = item.LocalCurrencyAmount,
                                        InvoiceCurrencyAmount = item.InvoiceCurrencyAmount,
                                        ProfitCurrencyAmount = item.ProfitCurrencyAmount,
                                        ExternalVatCard = item.ExternalVATCard,
                                    };

                                    VatType vatType = this.allVatTypes.Where(d => d.Id == itemGroup.SingleVATTypeId).FirstOrDefault();
                                    if (vatType != null)
                                    {
                                        newItem.ExternalTAXItemId = vatType.ExternalTAXItemId;
                                    }

                                    VatTypePercentagePM myPercentagePM = allVatPercentages.Where(d => d.VatTypeId == itemGroup.SingleVATTypeId).FirstOrDefault();
                                    if (myPercentagePM != null)
                                    {
                                        newItem.VatTypePercentage = myPercentagePM.Percentage;
                                    }

                                    if (string.IsNullOrEmpty(newItem.ExternalVatCard))
                                    {
                                        if (this.accountingSetting.AccountingSystemCode == "HV" || this.accountingSetting.AccountingSystemCode == "RH")
                                        {
                                            newItem.ExternalVatCard = this.accountingSetting.ReceivableVATCard;
                                        }
                                        else if (vatType != null)
                                        {
                                            newItem.ExternalVatCard = vatType.ReceivablesExternalId;
                                        }
                                    }

                                    group_Source.Add(newItem);
                                }
                            }
                        }
                        #endregion
                    }

                    group_data
                       = (from items in group_Source
                          group items by new { items.VatTypeId, items.VatTypePercentage, items.ExternalVatCard, items.ExternalTAXItemId, items.IsRegionalTax } into g
                          select new InvoiceTotalsClass()
                          {
                              Id = g.Key.VatTypeId,
                              VatTypeId = g.Key.VatTypeId,
                              VatTypePercentage = g.Key.VatTypePercentage,
                              LocalCurrencyAmount = g.Sum(s => s.LocalCurrencyAmount),
                              InvoiceCurrencyAmount = g.Sum(s => s.InvoiceCurrencyAmount),
                              ProfitCurrencyAmount = g.Sum(s => s.ProfitCurrencyAmount),
                              ExternalVatCard = g.Key.ExternalVatCard,
                              ExternalTAXItemId = g.Key.ExternalTAXItemId,
                              IsRegionalTax = g.Key.IsRegionalTax,
                          }).ToList();

                    foreach (InvoiceTotalsClass item in group_data)
                    {
                        ARInvoiceTotalVAT record = new ARInvoiceTotalVAT()
                        {
                            Id = IdCounter.GetNumber("ARInvoiceTotalVAT", entityPM.Tenant).ToString(),
                            Tenant = entityPM.Tenant,
                            ARInvoiceId = entityPM.Id,
                            VatTypeId = item.Id,
                            VatPercent = MethodHelper.Roundd(item.VatTypePercentage, 3),
                            LocalVatableAmount = MethodHelper.Roundd(item.LocalCurrencyAmount, 2),
                            InvoiceCurrencyVatableAmount = MethodHelper.Roundd(item.InvoiceCurrencyAmount, 2),
                            ProfitVatableAmount = MethodHelper.Round(item.ProfitCurrencyAmount, 2),
                            ExternalVATCard = item.ExternalVatCard,
                            ExternalTAXItemId = item.ExternalTAXItemId,
                            IsRegionalTax = item.IsRegionalTax,
                        };

                        record.LocalVATAmount = MethodHelper.Roundd((record.LocalVatableAmount * record.VatPercent / 100), 2);
                        record.InvoiceCurrencyVATAmount = MethodHelper.Roundd((record.InvoiceCurrencyVatableAmount * record.VatPercent / 100), 2);
                        record.ProfitCurrencyVATAmount = MethodHelper.Roundd((record.ProfitVatableAmount * record.VatPercent / 100), 2);
                        invoiceTotalVatRepository.Add(record);

                        sumOfVATsAmounts += record.InvoiceCurrencyVATAmount;
                        sumOfVATsAmounts_Local += record.LocalVATAmount;
                        sumOfVATsAmounts_Profit += record.ProfitCurrencyVATAmount;
                        if (IsFullAccountingActivated(entityPM.Tenant) && entityPM.BillToPartnerTypeId == "CS" && (entityPM.StatusCode == "AD" || entityPM.StatusCode == "AC") && record.LocalVATAmount != 0)
                        {
                            CreateInterestTransactionLine(null, record);
                        }
                    }

                    Amount = MethodHelper.Round(subTotal + sumOfVATsAmounts, 2);
                    Amount_Local = MethodHelper.Round(subTotal_Local + sumOfVATsAmounts_Local, 2);

                    if (entityPM.ProfitCurrencyId == entityPM.InvoiceCurrencyId)
                    {
                        Amount_Profit = Amount;
                    }

                    else
                    {
                        Amount_Profit = MethodHelper.Round(Amount_Local / entityPM.ProfitCurrencyExchangeRate, 2);
                    }
                    #endregion
                }

                entityPM.SubTotalInInvoiceCurrency = subTotal;
                entityPM.SubTotalInLocalCurrency = subTotal_Local;
                entityPM.AmountInInvoiceCurrency = Amount;
                entityPM.AmountInLocalCurrency = Amount_Local;
                entityPM.AmountInProfitCurrency = Amount_Profit;
                this.InitializeAmountDueFields();
            }
        }
        #endregion

        #region Constituent Lines
        private void UpdateConsolidationLines()
        {
            if (entityPM.IsConsolidationInvoice)
            {
                //DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(tenant);

                if (isNewEntity)
                {
                    if (entityPM.ConstituentInvoices.Count > 0)
                    {
                        #region
                        List<string> allInvoiceIds = entityPM.ConstituentInvoices.Select(s => s.Id).ToList();
                        List<ARInvoice> allInvoices = invoiceRepository.GetInvoicesListFromIdList(allInvoiceIds, tenant);

                        this.ValidateConstituentInvoiceConnected(allInvoices);

                        foreach (ARInvoice myInvoice in allInvoices)
                        {
                            myInvoice.IsClosed = true;
                            myInvoice.StatusCode = "CN";
                            myInvoice.ConsolidationInvoiceId = entityPM.Id;
                            invoiceRepository.Update(myInvoice);

                            EventTracer.CreateTraceEvent(new EventTracerArgs()
                            {
                                Tenant = tenant,
                                EventTypeCode = "UPIN",
                                UserId = loggedContactId,
                                EntityId = myInvoice.Id,
                                ObjectTableName = "ARInvoice",
                            });

                            EventTracer.CreateTraceEvent(new EventTracerArgs()
                            {
                                Tenant = tenant,
                                EventTypeCode = "INCN",
                                UserId = loggedContactId,
                                EntityId = myInvoice.Id,
                                ObjectTableName = "ARInvoice",
                            });
                        }

                        invoiceRepository.SubmitChanges();
                        this.BuildConsolidationInvoiceLines(allInvoiceIds);
                        #endregion
                    }
                }

                else
                {
                    #region

                    List<string> allInvoiceIds = invoiceConstituentsChangeSet.Select(s => s.Id).ToList();
                    List<ARInvoice> allInvoices = invoiceRepository.GetInvoicesListFromIdList(allInvoiceIds, tenant);

                    this.ValidateConstituentInvoiceConnected(allInvoices);

                    bool isConnectedInvoicesChanged = false;

                    foreach (ConstituentPM item in invoiceConstituentsChangeSet)
                    {
                        switch (item.ChangeSetOp)
                        {
                            case ChangeSetOperation.Insert:
                                {
                                    ARInvoice myInvoice = (from d in allInvoices where d.Id == item.Id select d).FirstOrDefault();
                                    if (myInvoice != null)
                                    {
                                        if (myInvoice.ConsolidationInvoiceId == null)
                                        {
                                            myInvoice.IsClosed = true;
                                            myInvoice.StatusCode = "CN";
                                            myInvoice.ConsolidationInvoiceId = entityPM.Id;
                                            invoiceRepository.Update(myInvoice);

                                            EventTracer.CreateTraceEvent(new EventTracerArgs()
                                            {
                                                Tenant = tenant,
                                                EventTypeCode = "UPIN",
                                                UserId = loggedContactId,
                                                EntityId = item.Id,
                                                ObjectTableName = "ARInvoice",
                                            });

                                            EventTracer.CreateTraceEvent(new EventTracerArgs()
                                            {
                                                Tenant = tenant,
                                                EventTypeCode = "INCN",
                                                UserId = loggedContactId,
                                                EntityId = item.Id,
                                                ObjectTableName = "ARInvoice",
                                            });

                                            isConnectedInvoicesChanged = true;
                                        }
                                    }

                                    break;
                                }

                            case ChangeSetOperation.Delete:
                                {
                                    var allConnectedCount = (from d in objectContext.ARInvoices
                                                             where d.Tenant == tenant
                                                             && d.IsConstituentInvoice
                                                             && d.ConsolidationInvoiceId == this.entityPM.Id
                                                             select d).Count();

                                    if (allConnectedCount > 1)
                                    {
                                        ARInvoice myInvoice = (from d in allInvoices where d.Id == item.Id select d).FirstOrDefault();
                                        if (myInvoice != null)
                                        {
                                            myInvoice.IsClosed = false;
                                            myInvoice.StatusCode = "NT";
                                            myInvoice.ConsolidationInvoiceId = null;

                                            invoiceRepository.Update(myInvoice);

                                            EventTracer.CreateTraceEvent(new EventTracerArgs()
                                            {
                                                Tenant = tenant,
                                                EventTypeCode = "UPIN",
                                                UserId = loggedContactId,
                                                EntityId = item.Id,
                                                ObjectTableName = "ARInvoice",
                                            });

                                            EventTracer.CreateTraceEvent(new EventTracerArgs()
                                            {
                                                Tenant = tenant,
                                                EventTypeCode = "INDS",
                                                UserId = loggedContactId,
                                                EntityId = item.Id,
                                                ObjectTableName = "ARInvoice",
                                            });
                                        }

                                        isConnectedInvoicesChanged = true;
                                    }

                                    break;
                                }
                        }
                    }
                    #endregion

                    if (isConnectedInvoicesChanged)
                    {
                        invoiceRepository.SubmitChanges();
                        List<string> activeInvoicesIds = invoiceConstituentsChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).Select(s => s.Id).ToList();

                        this.DeleteConsolidationInvoiceLines();
                        this.BuildConsolidationInvoiceLines(activeInvoicesIds);

                        int myLineNumber = 1;
                        foreach (ARInvoiceLinePM item in entityPM.InvoiceLines)
                        {
                            item.LineNumber = myLineNumber;
                            this.CreateInvoiceLine(item);
                            this.isUpdateTotalVats = true;
                            myLineNumber += 1;
                        }
                    }
                }
            }
        }

        private void ValidateConstituentInvoiceConnected(List<ARInvoice> allInvoices)
        {
            if (allInvoices.Count > 0)
            {
                ARInvoice connectedConstituentInvoice = allInvoices.Where(d => d.StatusCode == "CN" && d.ConsolidationInvoiceId != this.entityPM.Id).FirstOrDefault();
                if (connectedConstituentInvoice != null)
                {
                    throw new ApplicationException("Constituent Invoice: " + connectedConstituentInvoice.InvoiceNumber + " is connected to another Consolidation");
                }
            }
        }

        private void DeleteConsolidationInvoiceLines()
        {
            if (entityPM.IsConsolidationInvoice)
            {
                entityPM.InvoiceLines.Clear();

                List<ARInvoiceLine> lines = invoiceLineRepository.GetInvoiceLinesByInvoiceId(entityPM.Id, tenant).ToList();

                if (lines.Count() > 0)
                {
                    foreach (ARInvoiceLine item in lines)
                    {
                        invoiceLineRepository.Remove(item);
                    }

                    invoiceLineRepository.SubmitChanges();
                }
            }
        }

        private void BuildConsolidationInvoiceLines(List<string> allConnectedInvoiceIds)
        {
            if (entityPM.IsConsolidationInvoice)
            {
                if (allConnectedInvoiceIds.Count > 0)
                {
                    IQueryable<VatType> iQueryable_VatTypes = vatTypeRepository.GetVatTypes(tenant);
                    IQueryable<ARInvoiceLine> iQueryable_Lines = invoiceLineRepository.GetInvoiceLinesByTenant(tenant);

                    ChargesTypeRepository chargesTypeRepository = new ChargesTypeRepository(tenant);
                    IQueryable<ChargesType> iQueryable_ChargesTypes = chargesTypeRepository.GetChargesTypes(tenant);
                    VatTypePercentageRepository vatTypePercentageRepository = new VatTypePercentageRepository(tenant);

                    ChargeTypeAccountingRepository chargeTypeAccountingRepository = new ChargeTypeAccountingRepository(tenant);
                    IQueryable<ChargeTypeAccounting> iQueryable_ChargeTypeAccounting = chargeTypeAccountingRepository.GetChargeTypeAccountings(tenant);

                    List<ARInvoiceLine> allConnectedInvoiceLines = (from d in iQueryable_Lines where allConnectedInvoiceIds.Contains(d.ARInvoiceId) select d).ToList();

                    List<ARInvoiceLinePM> myNewLines = (from a in allConnectedInvoiceLines
                                                        group a by new
                                                        {
                                                            a.ChargesTypeId,
                                                            a.Description,
                                                            a.LocalDescription,
                                                            a.VatTypeId,
                                                            a.MeasurementId,
                                                            a.IsExpense
                                                        } into gr

                                                        select new ARInvoiceLinePM()
                                                        {
                                                            Tenant = tenant,
                                                            ChargesTypeId = gr.Key.ChargesTypeId,
                                                            Description = gr.Key.Description,
                                                            LocalDescription = gr.Key.LocalDescription,
                                                            VatTypeId = gr.Key.VatTypeId,
                                                            MeasurementId = gr.Key.MeasurementId,
                                                            ARInvoiceId = entityPM.Id,
                                                            IsExpense = gr.Key.IsExpense,
                                                            Quantity = 1,
                                                            IsExchangeRateFixed = false,
                                                            ExchangeRateDate = entityPM.ExchangeRateDate,
                                                            ForiegnCurrencyId = entityPM.InvoiceCurrencyId,
                                                            ForiegnExchangeRate = entityPM.InvoiceCurrencyExchangeRate,
                                                        }).ToList();

                    foreach (ARInvoiceLinePM item in myNewLines)
                    {
                        string vatExternalCard = null;
                        string receivableVATCard = null;
                        receivableVATCard = this.accountingSetting == null ? null : this.accountingSetting.ReceivableVATCard;

                        #region VatType
                        if (!string.IsNullOrEmpty(item.VatTypeId))
                        {
                            VatType vatType = (from f in iQueryable_VatTypes where f.Id == item.VatTypeId select f).FirstOrDefault();

                            if (vatType != null)
                            {
                                item.VatTypeName = vatType.EnglishName;
                                item.ExternalTAXItemId = vatType.ExternalTAXItemId;
                                vatExternalCard = vatType.ReceivablesExternalId;
                            }

                            VatTypePercentage vatTypePercentage = vatTypePercentageRepository.GetVatTypePercentageByDate(item.VatTypeId, tenant, entityPM.InvoiceDate);
                            if (vatTypePercentage != null)
                            {
                                item.VatPercentage = vatTypePercentage.Percentage;
                            }

                            if (this.accountingSetting.AccountingSystemCode == "HV" || this.accountingSetting.AccountingSystemCode == "RH")
                            {
                                item.ExternalVATCard = receivableVATCard;
                            }
                            else
                            {
                                item.ExternalVATCard = vatExternalCard;
                            }
                        }

                        #endregion

                        #region ChargesType
                        ChargesType myChargesType = (from f in iQueryable_ChargesTypes where f.Id == item.ChargesTypeId select f).FirstOrDefault();
                        if (myChargesType != null)
                        {
                            if (isJournal)
                            {
                                if (myChargesType.AccountingVATSplit)
                                {
                                    ChargeTypeAccounting myChargeTypeAccounting = (from d in iQueryable_ChargeTypeAccounting where d.ChargeTypeId == item.ChargesTypeId && d.VatTypeId == item.VatTypeId select d).FirstOrDefault();
                                    if (myChargeTypeAccounting != null)
                                    {
                                        item.CreditAccount = myChargeTypeAccounting.ReceivableCreditAccount;
                                    }
                                }

                                else
                                {
                                    item.CreditAccount = myChargesType.ReceivableCreditAccount;
                                }
                            }

                            else
                            {
                                item.CreditAccount = myChargesType.ReceivablesChargesTypeExternalCode;
                            }
                        }
                        #endregion

                        List<ARInvoiceLine> allMatchingLines = (from a in allConnectedInvoiceLines
                                                                where a.ChargesTypeId == item.ChargesTypeId
                                                                && a.VatTypeId == item.VatTypeId
                                                                && a.MeasurementId == item.MeasurementId
                                                                && a.Description == item.Description
                                                                && a.LocalDescription == item.LocalDescription
                                                                && a.IsExpense == item.IsExpense
                                                                select a).ToList();

                        item.LocalCurrencyAmount = MethodHelper.Round((double)allMatchingLines.Sum(d => d.LocalCurrencyAmount), 2);
                        item.InvoiceCurrencyAmount = MethodHelper.Round((double)allMatchingLines.Sum(d => d.InvoiceCurrencyAmount), 2);
                        item.ForiegnCurrencyAmount = item.InvoiceCurrencyAmount;
                        item.UnitPrice = item.InvoiceCurrencyAmount;

                        if (item.ForiegnCurrencyId == entityPM.ProfitCurrencyId)
                        {
                            item.ProfitCurrencyAmount = item.ForiegnCurrencyAmount;
                        }

                        else if (entityPM.ProfitCurrencyExchangeRate > 0)
                        {
                            double? myAmount = item.LocalCurrencyAmount / entityPM.ProfitCurrencyExchangeRate;
                            item.ProfitCurrencyAmount = MethodHelper.Round(myAmount, 2);
                        }

                        entityPM.InvoiceLines.Add(item);
                    }
                }
            }
        }

        #region (Old Code) Ayman says: Please dont remove it
        //private void BuildConsolidationInvoiceLines(List<string> allConnectedInvoiceIds)
        //{
        //    if (entityPM.IsConsolidationInvoice)
        //    {
        //        if (allConnectedInvoiceIds.Count > 0)
        //        {
        //            IQueryable<VatType> iQueryable_VatTypes = vatTypeRepository.GetVatTypes(tenant);
        //            IQueryable<ARInvoiceLine> iQueryable_Lines = invoiceLineRepository.GetInvoiceLinesByTenant(tenant);

        //            ChargesTypeRepository chargesTypeRepository = new ChargesTypeRepository(tenant);
        //            IQueryable<ChargesType> iQueryable_ChargesTypes = chargesTypeRepository.GetChargesTypes(tenant);
        //            VatTypePercentageRepository vatTypePercentageRepository = new VatTypePercentageRepository(tenant);

        //            #region Accounting System & ChargeType
        //            bool isAccountingSystemIsJournalMode = false;

        //            AccountingSettingRepository accountingSettingRepository = new AccountingSettingRepository(tenant);
        //            AccountingSetting accountingSetting = accountingSettingRepository.GetSingleAccountSetting(tenant);
        //            if (accountingSetting != null)
        //            {
        //                AccountingSystemRepository accountingSystemRepository = new AccountingSystemRepository(tenant);
        //                AccountingSystem accountingSystem = accountingSystemRepository.GetSingleAccountingSystem(accountingSetting.AccountingSystemCode);
        //                if (accountingSystem != null)
        //                {
        //                    if (accountingSystem.IsJournalMode)
        //                    {
        //                        isAccountingSystemIsJournalMode = true;
        //                    }
        //                }
        //            }
        //            #endregion

        //            IQueryable<ARInvoiceLine> allConnectedInvoiceLines = (from d in iQueryable_Lines where allConnectedInvoiceIds.Contains(d.ARInvoiceId) select d);

        //            List<ARInvoiceLinePM> newInvoiceLines = (from a in allConnectedInvoiceLines
        //                                                     group a by new
        //                                                     {
        //                                                         a.ChargesTypeId,
        //                                                         a.Description,
        //                                                         a.LocalDescription,
        //                                                         a.VatTypeId,
        //                                                         a.ForiegnCurrencyId,
        //                                                         a.MeasurementId
        //                                                     } into gr

        //                                                     select new ARInvoiceLinePM()
        //                                                     {
        //                                                         ChargesTypeId = gr.Key.ChargesTypeId,
        //                                                         Description = gr.Key.Description,
        //                                                         LocalDescription = gr.Key.LocalDescription,
        //                                                         VatTypeId = gr.Key.VatTypeId,
        //                                                         ForiegnCurrencyId = gr.Key.ForiegnCurrencyId,
        //                                                         MeasurementId = gr.Key.MeasurementId,
        //                                                         ARInvoiceId = entityPM.Id,
        //                                                         Tenant = tenant,
        //                                                         Quantity = 1,
        //                                                         IsExchangeRateFixed = false,
        //                                                         ExchangeRateDate = entityPM.ExchangeRateDate,
        //                                                     }).ToList();

        //            foreach (ARInvoiceLinePM item in newInvoiceLines)
        //            {
        //                #region VatType
        //                if (!string.IsNullOrEmpty(item.VatTypeId))
        //                {
        //                    VatType vatType = (from f in iQueryable_VatTypes where f.Id == item.VatTypeId select f).FirstOrDefault();

        //                    if (vatType != null)
        //                    {
        //                        item.VatTypeName = vatType.EnglishName;
        //                        item.ExternalVATCard = vatType.ExternalVATCard;
        //                        item.ExternalTAXItemId = vatType.ExternalTAXItemId;
        //                    }

        //                    VatTypePercentage vatTypePercentage = vatTypePercentageRepository.GetVatTypePercentageByDate(item.VatTypeId, tenant, entityPM.InvoiceDate);
        //                    if (vatTypePercentage != null)
        //                    {
        //                        item.VatPercentage = vatTypePercentage.Percentage;
        //                    }
        //                }
        //                #endregion

        //                #region ChargesType
        //                ChargesType chargesType = (from f in iQueryable_ChargesTypes where f.Id == item.ChargesTypeId select f).FirstOrDefault();
        //                if (chargesType != null)
        //                {
        //                    if (isAccountingSystemIsJournalMode)
        //                    {
        //                        item.CreditAccount = chargesType.ReceivableCreditAccount;
        //                    }

        //                    else
        //                    {
        //                        item.CreditAccount = chargesType.ChargesTypeExternalCode;
        //                    }
        //                }
        //                #endregion

        //                item.ForiegnCurrencyAmount = Math.Ro4und((double)allConnectedInvoiceLines.Sum(d => d.ForiegnCurrencyAmount), 2);
        //                item.LocalCurrencyAmount = Math.Rou4nd((double)allConnectedInvoiceLines.Sum(d => d.LocalCurrencyAmount), 2);
        //                item.ForiegnExchangeRate = Math.Rou4nd((double)(item.ForiegnCurrencyAmount / item.LocalCurrencyAmount), 5);
        //                item.UnitPrice = item.ForiegnCurrencyAmount;

        //                if (entityPM.InvoiceCurrencyId == item.ForiegnCurrencyId)
        //                {
        //                    item.InvoiceCurrencyAmount = item.ForiegnCurrencyAmount;
        //                }

        //                else if (entityPM.InvoiceCurrencyExchangeRate > 0)
        //                {
        //                    double? myAmount = item.LocalCurrencyAmount / entityPM.InvoiceCurrencyExchangeRate;
        //                    item.InvoiceCurrencyAmount = Math.Ro4und((double)myAmount, 2);
        //                }

        //                if (entityPM.ProfitCurrencyId == item.ForiegnCurrencyId)
        //                {
        //                    item.ProfitCurrencyAmount = item.ForiegnCurrencyAmount;
        //                }

        //                else if (entityPM.ProfitCurrencyExchangeRate > 0)
        //                {
        //                    double? myAmount = item.LocalCurrencyAmount / entityPM.ProfitCurrencyExchangeRate;
        //                    item.ProfitCurrencyAmount = Math.Ro4und((double)myAmount, 2);
        //                }

        //                entityPM.InvoiceLines.Add(item);
        //            }
        //        }
        //    }
        //}
        #endregion

        #endregion

        #region Invoice Lines
        private void UpdateInvoiceLines()
        {
            if (isNewEntity)
            {
                int myLineNumber = 1;
                foreach (ARInvoiceLinePM item in entityPM.InvoiceLines)
                {
                    item.LineNumber = myLineNumber;
                    this.CreateInvoiceLine(item);
                    this.UpdateReceivable(item);
                    this.isUpdateTotalVats = true;
                    myLineNumber += 1;
                    //if (IsFullAccountingActivated(entityPM.Tenant))
                    //{
                    //    CreateInterestTransactionLine(item, null);
                    //}
                }
            }

            else
            {
                int myLineNumber = invoiceLineRepository.GetBiggestLineNumber(entityPM.Id, tenant);

                foreach (ARInvoiceLinePM item in invoiceLinesChangeSet)
                {
                    if (invoice.StatusCode != "VD" && entityPM.StatusCode == "VD")
                    {
                        this.DisconnectReceivable(item.ReceivableId);
                        this.DisconnectInvoiceLine(item);
                    }

                    if (invoice.StatusCode != "AD" && entityPM.StatusCode == "AD")
                    {
                        this.UpdateReceivable(item);
                    }

                    switch (item.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                myLineNumber += 1;
                                item.LineNumber = myLineNumber;
                                this.CreateInvoiceLine(item);
                                this.UpdateReceivable(item);
                                this.isUpdateTotalVats = true;
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateInvoiceLine(item);
                                this.UpdateReceivable(item);
                                isUpdateTotalVats = true;
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DisconnectReceivable(item.ReceivableId);
                                this.DeleteInvoiceLine(item);
                                this.isUpdateTotalVats = true;
                                break;
                            }

                        default: { break; }
                    }

                }
            }
        }

        private void CreateInvoiceLine(ARInvoiceLinePM item)
        {
            item.Id = IdCounter.GetNumber("ARInvoiceLine", entityPM.Tenant).ToString();
            item.ARInvoiceId = entityPM.Id;

            this.ComputeInvoiceLineAmounts(item);

            ARInvoiceLine invoiceLine = new ARInvoiceLine();
            ARInvoiceMapping.MapInvoiceLine(item, invoiceLine, true);
            invoiceLineRepository.Add(invoiceLine);

            if (!this.isNewEntity)
            {
                invoiceLineRepository.SubmitChanges();
            }
            if (IsFullAccountingActivated(entityPM.Tenant) && entityPM.BillToPartnerTypeId == "CS" && (entityPM.StatusCode == "AD" || entityPM.StatusCode =="AC"))
            {
                CreateInterestTransactionLine(item, null);
            }

            this.CreateARInvoiceChargesConstraint(item);
        }

        int invoiceLineNumber = 0;
        DateTime? dateForInterest;
        private void  CreateInterestTransactionLine(ARInvoiceLinePM invoiceLine, ARInvoiceTotalVAT invoiceTotalVat)
        {
            ++invoiceLineNumber;
             dateForInterest = entityPM.DateForInterest == null ? DateTime.Now : entityPM.DateForInterest;
            GLAccountPM account = GetGLAccount(entityPM);
            InterestTransactionPM interestTransaction = new InterestTransactionPM();
            if (invoiceLine != null) {

                interestTransaction= CreateInterestTransactionLineForInvoiceLine(invoiceLine, account);
            }
            if (invoiceTotalVat != null)
            {
                interestTransaction= CreateInterestTransactionLineForVatLine(invoiceTotalVat, account);
            }
            IInterestTransactionUpdateServiceExt interestTransactionUpdateService = ContainerAccessor.Container.Resolve(typeof(IInterestTransactionUpdateServiceExt), "InterestTransactionUpdateServiceExt", new ParameterOverride("", 1)) as IInterestTransactionUpdateServiceExt;
            interestTransactionUpdateService.Create(interestTransaction);
        }
        private InterestTransactionPM CreateInterestTransactionLineForVatLine(ARInvoiceTotalVAT invoiceTotalVat,GLAccountPM account)
        {
            ARInvoiceLinePM invoiceLine = GetInvoiceLineForTotalVat(invoiceTotalVat);

            InterestTransactionPM InterestTransactionVatLine = new InterestTransactionPM()
            {

                InterestEntityTypeCode = "1",
                EntityId = invoiceTotalVat.ARInvoiceId,
                OriginalEntityLineNumber = invoiceLineNumber,
                LocalAmount = (decimal)invoiceTotalVat.LocalVATAmount,
                ForeignAmount = (decimal?)invoiceTotalVat.InvoiceCurrencyVATAmount,
                InterestValueDate = GetIntrestValueDate(invoiceLine),
                Tenant = entityPM.Tenant,
                GLAccountId = account != null ? account.Id : null,
                CurrencyId = entityPM.InvoiceCurrencyId,
                ChangeSetOp = ChangeSetOperation.Insert,
            };
            return InterestTransactionVatLine;
        }

        private DateTime GetIntrestValueDate(ARInvoiceLinePM invoiceLine)
        {
            DateTime dateForInterest;

            if (entityPM.ARInvoiceTypeCode == ARInvoiceTypeValues.InterestInvoice || entityPM.ARInvoiceTypeCode == ARInvoiceTypeValues.CreditNote)
                dateForInterest = entityPM.InvoiceDate.Value;
            else if (invoiceLine.ValueDate != null)
                dateForInterest = invoiceLine.ValueDate.Value;
            else
                dateForInterest = entityPM.DueDate.Value;

            return dateForInterest;
        }

        private ARInvoiceLinePM GetInvoiceLineForTotalVat(ARInvoiceTotalVAT invoiceTotalVat)
        {
            return entityPM.InvoiceLines.Where(d => d.VatTypeId == invoiceTotalVat.VatTypeId).FirstOrDefault();
        }

        private InterestTransactionPM CreateInterestTransactionLineForInvoiceLine(ARInvoiceLinePM invoiceLine, GLAccountPM account)
        {
            InterestTransactionPM interestTransaction = new InterestTransactionPM()
            {
                InterestEntityTypeCode = "1",
                EntityId = invoiceLine.ARInvoiceId,
                OriginalEntityLineNumber = invoiceLine.LineNumber,
                LocalAmount = (decimal)invoiceLine.LocalCurrencyAmount,
                GLAccountId = account != null? account.Id:null,
                ForeignAmount = (decimal?)invoiceLine.ForiegnCurrencyAmount,
                InterestValueDate = GetIntrestValueDate(invoiceLine),
                Tenant = invoiceLine.Tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                CurrencyId = invoiceLine.ForiegnCurrencyId,
               
            };
            return interestTransaction;
        }

        private void UpdateInvoiceLine(ARInvoiceLinePM item)
        {
            ARInvoiceLine invoiceLine = invoiceLineRepository.GetSingleInvoiceLine(item.Id);
            ARInvoiceMapping.MapInvoiceLine(item, invoiceLine, false);
            invoiceLineRepository.Update(invoiceLine);
        }
        private void DeleteInvoiceLine(ARInvoiceLinePM item)
        {
            ARInvoiceLine invoiceLine = invoiceLineRepository.GetSingleInvoiceLine(item.Id);
            if (invoiceLine != null)
            {
                this.DeleteARInvoiceChargesConstraint(invoiceLine);
                invoiceLineRepository.Remove(invoiceLine);                
            }
        }
        private void DisconnectInvoiceLine(ARInvoiceLinePM item)
        {
            ARInvoiceLine line = invoiceLineRepository.GetSingleInvoiceLine(item.Id);

            if (line != null)
            {
                this.DeleteARInvoiceChargesConstraint(line);
                line.ReceivableId = null;
                item.ReceivableId = null;
                invoiceLineRepository.Update(line);
            }
        }
        #endregion

        #region Payments
        private void UpdateInvoicePayments(List<ARInvoicePaymentPM> invoicePaymentsChangeSet)
        {
            foreach (ARInvoicePaymentPM item in invoicePaymentsChangeSet)
            {
                switch (item.ChangeSetOp)
                {
                    case ChangeSetOperation.Insert:
                        {
                            this.CreateInvoicePayment(item);
                            break;
                        }

                    case ChangeSetOperation.Update:
                        {
                            this.UpdateInvoicePayment(item);
                            break;
                        }

                    case ChangeSetOperation.Delete:
                        {
                            this.DeleteInvoicePayment(item);
                            break;
                        }

                    case ChangeSetOperation.None: { break; }
                    default: { break; }
                }
            }

            invoicePaymentRepository.SubmitChanges();
        }

        private void CreateInvoicePayment(ARInvoicePaymentPM itemPM)
        {
            this.ValidateIfSameRecordAdded(itemPM);

            itemPM.Id = IdCounter.GetNumber("ARInvoicePayment", tenant);

            ARInvoicePayment invoicePayment = new ARInvoicePayment()
            {
                Id = itemPM.Id,
                Tenant = tenant,
                ARInvoiceId = entityPM.Id,
            };

            ARInvoiceMapping.MapInvoicePayment(itemPM, invoicePayment, true);
            invoicePaymentRepository.Add(invoicePayment);

            string myPaymentNumber = itemPM.PaymentNumber;
            if (string.IsNullOrEmpty(myPaymentNumber))
            {
                if (!string.IsNullOrEmpty(itemPM.ARPaymentId))
                {
                    ARPayment myPayment = paymentRepository.GetSingleARPayment(itemPM.ARPaymentId);
                    if (myPayment != null)
                    {
                        myPaymentNumber = myPayment.PaymentNo;
                    }
                }
            }


            if (this.entityPM.TransferStatusCode=="TR")
            {
                QBOARPaymentId = itemPM.ARPaymentId;               
            }

            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                Tenant = tenant,
                EventTypeCode = "COAR",
                UserId = loggedContactId,
                EntityId = itemPM.ARInvoiceId,
                ObjectTableName = "ARInvoice",
                Notes = "Connected with Payment: " + myPaymentNumber + " with Amount Due equals to: " + invoice.AmountDue,
            });
        }

        private void UpdateInvoicePayment(ARInvoicePaymentPM itemPM)
        {
            ARInvoicePayment invoicePayment = invoicePaymentRepository.GetSingleARInvoicePayment(itemPM.Id, tenant);
            ARInvoiceMapping.MapInvoicePayment(itemPM, invoicePayment, false);
            invoicePaymentRepository.Update(invoicePayment);
        }

        private void DeleteInvoicePayment(ARInvoicePaymentPM itemPM)
        {
            ARInvoicePayment invoicePayment = invoicePaymentRepository.GetSingleARInvoicePayment(itemPM.Id, tenant);

            if (invoicePayment != null)
            {
                UpdatePaymentAmounts(itemPM, true);
                invoicePaymentRepository.Remove(invoicePayment);

                string myPaymentNumber = itemPM.PaymentNumber;
                if (string.IsNullOrEmpty(myPaymentNumber))
                {
                    if (!string.IsNullOrEmpty(itemPM.ARPaymentId))
                    {
                        ARPayment myPayment = paymentRepository.GetSingleARPayment(itemPM.ARPaymentId);
                        if (myPayment != null)
                        {
                            myPaymentNumber = myPayment.PaymentNo;
                        }
                    }
                }


                if (this.entityPM.TransferStatusCode == "TR")
                {
                    QBOARPaymentId = itemPM.ARPaymentId;
                }

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = tenant,
                    EventTypeCode = "ARID",
                    UserId = loggedContactId,
                    EntityId = itemPM.ARInvoiceId,
                    ObjectTableName = "ARInvoice",
                    Notes = "Disconnected from Payment: " + myPaymentNumber,
                });
            }
        }

        private void UpdateInvoiceAmountDue()
        {
            if (this.invoicePaymentsChangeSet != null)
            {
                List<ARInvoicePaymentPM> invoicepayments = (from a in this.invoicePaymentsChangeSet where a.ChangeSetOp != ChangeSetOperation.Delete select a).ToList();

                if (invoicepayments == null || invoicepayments.Count == 0)
                {
                    entityPM.IsClosed = false;
                    if (entityPM.StatusCode != "VD")
                    {
                        if (entityPM.StatusCode != "DR")
                        {
                            entityPM.AmountDue = entityPM.AmountInInvoiceCurrency.Value;
                            entityPM.StatusCode = "AD";
                        }
                    }
                }

                else
                {
                    double? conntectedPaymentAmount = MethodHelper.Round(invoicepayments.Sum(d => d.ForeignAmount), 2);

                    if (entityPM.ARInvoiceTypeCode == "CD" || entityPM.ARInvoiceTypeCode == "CC")
                    {
                        conntectedPaymentAmount = conntectedPaymentAmount * -1;
                    }

                    if (conntectedPaymentAmount <= entityPM.AmountInInvoiceCurrency)
                    {
                        entityPM.AmountDue = MethodHelper.Round((entityPM.AmountInInvoiceCurrency.Value - conntectedPaymentAmount), 2);

                        if (conntectedPaymentAmount < entityPM.AmountInInvoiceCurrency)
                        {
                            entityPM.StatusCode = "PP";
                            entityPM.IsClosed = false;
                        }

                        else
                        {
                            entityPM.StatusCode = "PD";
                            entityPM.IsClosed = true;
                        }
                    }

                    else
                    {
                        throw new Exception("The amount paid is not suitable to the amount due");
                    }
                }

                foreach (ARInvoicePaymentPM paymentInvoice in invoicepayments)
                {
                    UpdatePaymentAmounts(paymentInvoice, false);
                }

                entityPM.AmountDueInLocalCurrency = MethodHelper.Round((entityPM.AmountDue * entityPM.InvoiceCurrencyExchangeRate), 2);
                entityPM.AmountDueInProfitCurrency = MethodHelper.Round((entityPM.AmountDueInLocalCurrency / entityPM.ProfitCurrencyExchangeRate), 2);

                invoice.AmountDueInLocalCurrency = entityPM.AmountDueInLocalCurrency;
                invoice.AmountDueInProfitCurrency = entityPM.AmountDueInProfitCurrency;

                invoice.AmountDue = entityPM.AmountDue;
                invoice.StatusCode = entityPM.StatusCode;
                invoice.IsClosed = entityPM.IsClosed;
            }
        }

        private void UpdatePaymentAmounts(ARInvoicePaymentPM itemPM, bool isDelete)
        {
            ARPayment payment = paymentRepository.GetSingleARPayment(itemPM.ARPaymentId);

            if (payment != null)
            {
                if (payment.StatusCode == "VD")
                {
                    throw new Exception("Payment (" + payment.PaymentNo + ") is Voided");
                }

                else
                {
                    #region
                    double? invoicepaymentstotalamount = (from a in objectContext.ARInvoicePayments
                                                          where a.ARPaymentId == itemPM.ARPaymentId
                                                          && a.Tenant == tenant
                                                          select a).Sum(s => s.PaymentAmount);

                    if (invoicepaymentstotalamount == null)
                    {
                        invoicepaymentstotalamount = 0;
                    }

                    invoicepaymentstotalamount = MethodHelper.Round(invoicepaymentstotalamount, 2);

                    if (isDelete)
                    {
                        invoicepaymentstotalamount = MethodHelper.Round((invoicepaymentstotalamount - itemPM.PaymentAmount), 2);
                    }

                    if (invoicepaymentstotalamount <= payment.AmountInPaymentCurrency)
                    {
                        if (invoicepaymentstotalamount < 0)
                        {
                            invoicepaymentstotalamount = invoicepaymentstotalamount * -1;
                        }

                        payment.OpenAmount = MethodHelper.Round((payment.AmountInPaymentCurrency - invoicepaymentstotalamount), 2);

                        if (payment.OpenAmount == 0)
                        {
                            payment.IsClosed = true;

                            if (payment.StatusCode == "AD")
                            {                                
                                payment.StatusCode = "CL";
                            }
                        }

                        else
                        {
                            payment.IsClosed = false;

                            if (payment.StatusCode != "DR")
                            {
                                payment.StatusCode = "AD";
                            }
                        }
                    }

                    else
                    {
                        throw new Exception("The amount paid is not suitable to payment amount!");
                    }

                    paymentRepository.Update(payment);
                    #endregion
                }
            }
        }

        private void UpdatePaidDate()
        {
            if (entityPM.AmountDue != 0)
            {
                entityPM.PaidDate = null;
            }

            else
            {
                ARInvoicePaymentPM itemPM = invoicePaymentsChangeSet.Where(d => d.ChangeSetOp == ChangeSetOperation.Insert).FirstOrDefault();
                if(itemPM != null)
                {
                    entityPM.PaidDate = (from d in objectContext.ARPayments where d.Id == itemPM.ARPaymentId select d.ValueDate).FirstOrDefault();
                }
            }

            invoice.PaidDate = entityPM.PaidDate;
        }

        private void ValidateIfSameRecordAdded(ARInvoicePaymentPM item)
        {
            IQueryable<ARInvoicePayment> invoicePayments = invoicePaymentRepository.GetARInvoicePayments(item.ARPaymentId, item.ARInvoiceId, entityPM.Tenant);
            if (invoicePayments.Count() > 0)
            {
                throw new Exception("This invoice already connected to same payment");
            }
        }
        #endregion

        #region SearchField
        public void BuildSearchFields()
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.InvoiceNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.VatNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.DraftNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.HouseNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.MasterNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Description);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.CustomerRef);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.PrintNotes);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.InternalNotes);

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

            #region Entity References

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ShipmentsNumbers);

            //if (allActiveShipmentIds != null)
            //{
            //    if (allActiveShipmentIds.Count > 0)
            //    {
            //        List<Shipment> iActiveShipments = this.allShipments.Where(d => allActiveShipmentIds.Contains(d.Id)).ToList();
            //        foreach (Shipment iShipment in iActiveShipments)
            //        {
            //            MethodHelper.AddToSearchFields(ref mySearchFields, iShipment.ShipmentNumber);
            //        }
            //    }
            //}
            #endregion

            #region Payments
            if (entityPM.InvoicePayments != null)
            {
                foreach (ARInvoicePaymentPM item in entityPM.InvoicePayments)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, item.PaymentNumber);
                }
            }
            #endregion

            #region Custom Fields
            List<ObjectField> customFields = ObjectFieldRepository.GetCustomObjectFieldsByObjectTableName("ARInvoice", tenant).Where(o => o.DataTypeCode == "Text" || o.DataTypeCode == "nText").ToList();

            CustomFieldResolver customFieldResolver = new CustomFieldResolver();
            foreach (ObjectField field in customFields)
            {
                object value = customFieldResolver.GetFieldValue(entityPM, field, tenant);
                if (value != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, value.ToString());
                }
            }
            #endregion

            entityPM.SearchFields = mySearchFields;
            invoice.SearchFields = mySearchFields;
        }
        #endregion

        #region Journal & Journal Lines
        GLAccountPM glAccount;
        private void AddARInvoiceJournalAndJournalLines(ARInvoicePM theEntityPm, bool setApproved)
        {
            
            glAccount = GetGLAccount(theEntityPm);
            int tenant = theEntityPm.Tenant;
            if (setApproved)
            {
                TenantRepository tenantRepository = new TenantRepository(tenant);
                Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);

                if (tenantPOCO.AccountingActivated)
                {
                    // Insert Journal 
                    JournalPM journal = new JournalPM();
                    journal.Tenant = tenant;
                    journal.JournalNumber = "1";
                    journal.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    journal.AccountingDate = theEntityPm.InvoiceDate.Value;
                    //journal.DocumentDate = theEntityPm.InvoiceDate.Value;
                    //journal.DueDate = theEntityPm.InvoiceDate.Value;
                    journal.TypeCode = "0";
                    journal.StatusCode = "2";
                    journal.CreatedByUserId = theEntityPm.CreatedByUserId;
                    journal.AccountingEntityCode = "2";
                    journal.AccountingEntityId = theEntityPm.Id;
                    journal.AccountingEntityReference = theEntityPm.InvoiceNumber;
                    journal.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    journal.UpdatedByUserId = theEntityPm.UpdatedByUserId;
                    journal.ApproveDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    journal.ApprovedByUserId = theEntityPm.ApprovedByUserId;
                    journal.ChangeSetOp = ChangeSetOperation.Insert;
                    JournalLinePM journalLine;
                    // Insert Journal Lines 
                    if (theEntityPm.IsMultiCurrency)
                    {
                        journal = CreateJournalDebitLinesForMultiCurrencyInvoice(journal, theEntityPm );
                    }
                    else
                    {
                        // [Debit]
                        GLAccountPM glAccount = getDebitGLAccount(theEntityPm.BillToId, theEntityPm.Tenant);
                         journalLine = new JournalLinePM();
                        journalLine.Tenant = tenant;
                        journalLine.JournalId = journal.Id;
                        journalLine.Line = 1;
                        journalLine.ActionCode = "2";
                        journalLine.ActionTypeCodeEnum = JournalActionTypeEnum.Debit;
                        journalLine.DocumentDate = theEntityPm.InvoiceDate.Value;
                        journalLine.AccountingDate = theEntityPm.InvoiceDate.Value;
                        journalLine.DueDate = theEntityPm.DueDate.Value;
                        journalLine.LocalAmount = (decimal)theEntityPm.AmountInLocalCurrency;
                        journalLine.CurrencyId = theEntityPm.InvoiceCurrencyId;
                        journalLine.ForeignAmount = (decimal)theEntityPm.AmountInInvoiceCurrency;
                        journalLine.ExchangeRate = (decimal)theEntityPm.InvoiceCurrencyExchangeRate;
                        journalLine.Reference1 = theEntityPm.CustomerRef != null ? theEntityPm.CustomerRef: theEntityPm.InvoiceNumber;
                        journalLine.Reference2 = theEntityPm.MainEntityReference;
                        journalLine.Reference3 = !string.IsNullOrEmpty(theEntityPm.HouseNumber) ? theEntityPm.HouseNumber : theEntityPm.MasterNumber;
                        journalLine.Notes = theEntityPm.PrintNotes;
                        journalLine.DebitAccountId = this.glAccount == null ? "" : this.glAccount.Id;
                        journalLine.DebitControlAccountId = this.glAccount == null ? "" : this.glAccount.ControlAccountId;
                        journalLine.ChangeSetOp = ChangeSetOperation.Insert;
                        journal.JournalLines.Add(journalLine);
                    }
                    // [Credit]
                         journalLine = new JournalLinePM();
                        int counter = 1;
                        List<JournalLinePM> journalLines = (from d in theEntityPm.InvoiceLines
                                                            group d by new { d.GLAccountId, d.ForiegnCurrencyId, d.ForiegnExchangeRate, d.ValueDate } into g
                                                            select new JournalLinePM()
                                                            {
                                                                Tenant = tenant,
                                                                ActionCode = "1",
                                                                ActionTypeCodeEnum = JournalActionTypeEnum.Credit,
                                                                JournalId = journal.Id,
                                                                CreditAccountId = g.Key.GLAccountId,
                                                                Line = ++counter,
                                                                DocumentDate = theEntityPm.InvoiceDate.Value,
                                                                AccountingDate = theEntityPm.InvoiceDate.Value,
                                                                DueDate = g.Key.ValueDate == null ? theEntityPm.DueDate.Value : (DateTime)g.Key.ValueDate,
                                                                LocalAmount = (decimal)g.Sum(a => a.LocalCurrencyAmount),
                                                                CurrencyId = g.Key.ForiegnCurrencyId,
                                                                ForeignAmount = (decimal)g.Sum(a => a.ForiegnCurrencyAmount),
                                                                ExchangeRate = (decimal)g.Key.ForiegnExchangeRate,
                                                                Reference1 = theEntityPm.CustomerRef != null ? theEntityPm.CustomerRef : theEntityPm.InvoiceNumber,
                                                                Reference2 = theEntityPm.MainEntityReference,
                                                                Reference3 = !string.IsNullOrEmpty(theEntityPm.HouseNumber) ? theEntityPm.HouseNumber : theEntityPm.MasterNumber,
                                                                Notes = theEntityPm.PrintNotes,
                                                                DebitAccountId = glAccount == null ? "" : glAccount.Id,
                                                                DebitControlAccountId = glAccount == null ? "" : glAccount.ControlAccountId,
                                                            }).ToList();

                        journal.JournalLines.AddRange(journalLines);


                        // [Vats]
                        List<ARInvoiceTotalVAT> ARInvoiceTotalVATs = new List<ARInvoiceTotalVAT>();
                        ARInvoiceTotalVATRepository vatRepository = new ARInvoiceTotalVATRepository(tenant);
                        ARInvoiceTotalVATs = vatRepository.GetInvoiceTotalVatsForInvoiceWithoutZeroVATPercent(theEntityPm.Id, tenant).ToList();
                        counter = journal.JournalLines.Count();

                        // Accounting settings 
                        FullAccountingSettingPM accountingSettings = getFullAccountingSettings(theEntityPm.Tenant);
                        foreach (ARInvoiceTotalVAT vat in ARInvoiceTotalVATs)
                        {
                            journalLine = new JournalLinePM()
                            {
                                Tenant = tenant,
                                ActionCode = "1",
                                ActionTypeCodeEnum = JournalActionTypeEnum.Credit,
                                JournalId = journal.Id,
                                CreditAccountId = accountingSettings != null ? accountingSettings.VATOutputGLAccountId : "",
                                Line = ++counter,
                                DocumentDate = theEntityPm.InvoiceDate.Value,
                                AccountingDate = theEntityPm.InvoiceDate.Value,
                                DueDate = theEntityPm.DueDate.Value,
                                LocalAmount = (decimal)vat.LocalVATAmount,
                                CurrencyId = theEntityPm.InvoiceCurrencyId,
                                ForeignAmount = (decimal)vat.InvoiceCurrencyVATAmount,
                                ExchangeRate = (decimal)theEntityPm.InvoiceCurrencyExchangeRate,
                                Reference1 = theEntityPm.CustomerRef != null ? theEntityPm.CustomerRef : theEntityPm.InvoiceNumber,
                                Reference2 = theEntityPm.MainEntityReference,
                                Reference3 = !string.IsNullOrEmpty(theEntityPm.HouseNumber) ? theEntityPm.HouseNumber : theEntityPm.MasterNumber,
                                DebitAccountId = glAccount == null ? "" : glAccount.Id,
                                //     DebitControlAccountId = splittedByCurrencyAccount == null ? "" : splittedByCurrencyAccount.ControlAccountId,

                            };

                            journal.JournalLines.Add(journalLine);
                        }


                    if(entityPM.StatusCode == InvoiceAutoCreditStatus)
                        AutoReconcileAutoCreditInvoiceWithAutoCreditedInvoice(journal);


                    IJournalUpdateServiceExt journalUpdate = ContainerAccessor.Container.Resolve(typeof(IJournalUpdateServiceExt), "JournalUpdateServiceExt", new ParameterOverride("", 1)) as IJournalUpdateServiceExt;
                    AddAccountingEntitieJournal(journal, AccountingEntityJournalActions.ARInvoiceApprove);
                    journalUpdate.Update(journal);
                }
            }
        }

        private void AddAccountingEntitieJournal(JournalPM entityPM, string action, string ChildEntityId = null)
        {
            IAccountingEntityJournalUpdateServiceExt service = ContainerAccessor.Container.Resolve(typeof(IAccountingEntityJournalUpdateServiceExt), "AccountingEntityJournalUpdateServiceExt", new ParameterOverride("", 1)) as IAccountingEntityJournalUpdateServiceExt;
            service.AddAccountingEntitieJournal(entityPM, action, ChildEntityId);
        }

        private void AutoReconcileAutoCreditInvoiceWithAutoCreditedInvoice(JournalPM journal)
        {
            List<LedgerTransactionPM> autoCreditedInvoiceTransactions = GetAutoCreditedInvoiceTransactions(entityPM.Tenant, entityPM.AutoCreditByARInvoiceNumber);

            BlockReconciledTransactoins(autoCreditedInvoiceTransactions);

            CreateJounalReconcileForEachTransaction(journal, autoCreditedInvoiceTransactions);
        }

        private static void BlockReconciledTransactoins(List<LedgerTransactionPM> autoCreditedInvoiceTransactions)
        {
            bool hasReconciledLedgers = autoCreditedInvoiceTransactions.Any(transaction => transaction.IsReconciled);
            if (hasReconciledLedgers)
            {
                throw new ApplicationException(InvoiceAlreadyReconciledMessage);
            }
        }

        private static void CreateJounalReconcileForEachTransaction(JournalPM journal, List<LedgerTransactionPM> originalInvoiceTransactions)
        {
            originalInvoiceTransactions.ForEach(ledger =>
            {
                journal.JournalReconciles.Add(CreateJournalReconcileForLedgerTransaction(ledger, journal));
            });
        }

        private static JournalReconcilePM CreateJournalReconcileForLedgerTransaction(LedgerTransactionPM ledger, JournalPM journal)
        {
            return new JournalReconcilePM()
            {
                Tenant = journal.Tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                JournalId = journal.Id,
                Line = 1,
                LedgerTransactionId = ledger.Id,
                CurrencyId = ledger.OpenAmountCurrencyId,
                ReconciliationAmount = ledger.OpenAmount,
                IsPartial = false
            };
        }

        private List<LedgerTransactionPM> GetAutoCreditedInvoiceTransactions(int tenant, string autoCreditByARInvoiceNumber)
        {
            ARInvoicePM originalInvoice = GetOriginalInvoiceByAutoCreditNumber(tenant, autoCreditByARInvoiceNumber);
            return GetTransactionsByJournalId(tenant, originalInvoice?.JournalId);
        }

        private static List<LedgerTransactionPM> GetTransactionsByJournalId(int tenant, string journalId)
        {
            ILedgerTransactionQueryService ledgerTransactionQuery = ContainerAccessor.Container.Resolve(typeof(ILedgerTransactionQueryService), "LedgerTransactionQueryServiceExt", new ParameterOverride("", 1)) as ILedgerTransactionQueryService;
            return ledgerTransactionQuery.GetByJournalId(journalId, tenant);
        }

        private static ARInvoicePM GetOriginalInvoiceByAutoCreditNumber(int tenant, string autoCreditByARInvoiceNumber)
        {
            ARInvoiceQuery aRInvoiceQuery = new ARInvoiceQuery(tenant);
            var originalInvoice = aRInvoiceQuery.GetSingleInvoiceByInvoiceNumber(autoCreditByARInvoiceNumber, tenant);
            return originalInvoice;
        }

        private JournalPM CreateJournalDebitLinesForMultiCurrencyInvoice(JournalPM journal, ARInvoicePM invoice)
        {
            journal = CreateJournalDebitLinesFromInvoiceLines(journal, invoice);
            journal = CreateJournalDebitLineFromVat(journal, invoice);


            return journal;
        }
        int counter = 0;
        private JournalPM CreateJournalDebitLinesFromInvoiceLines(JournalPM journal, ARInvoicePM invoice)
        {
             counter = 1;
            List<JournalLinePM> journalLines = (from d in invoice.InvoiceLines
                                                group d by new { d.ForiegnCurrencyId } into g
                                                select new JournalLinePM()
                                                {
                                                    Tenant = tenant,
                                                    JournalId = journal.Id,
                                                    Line = 1,
                                                    ActionCode = "2",
                                                    ActionTypeCodeEnum = JournalActionTypeEnum.Debit,
                                                    DocumentDate = invoice.InvoiceDate.Value,
                                                    AccountingDate = invoice.InvoiceDate.Value,
                                                    DueDate = invoice.DueDate.Value,
                                                    LocalAmount = (decimal)g.Sum(a => a.LocalCurrencyAmount),
                                                    CurrencyId = g.Key.ForiegnCurrencyId,
                                                    ForeignAmount = (decimal)g.Sum(a => a.ForiegnCurrencyAmount),
                                                    ExchangeRate = (decimal?) g.Sum(a=> a.ForiegnExchangeRate)/g.Count(),//(decimal)g.Key.ForiegnExchangeRate,
                                                    Reference1 = invoice.CustomerRef != null ? invoice.CustomerRef : invoice.InvoiceNumber,
                                                    Reference2 = invoice.MainEntityReference,
                                                    Reference3 = !string.IsNullOrEmpty(invoice.HouseNumber) ? invoice.HouseNumber : invoice.MasterNumber,
                                                    Notes = invoice.PrintNotes,
                                                    DebitAccountId = glAccount == null ? "" : glAccount.Id,
                                                    DebitControlAccountId = glAccount == null ? "" : glAccount.ControlAccountId,
                                                    ChangeSetOp = ChangeSetOperation.Insert,
                                                }).ToList();

            journal.JournalLines.AddRange(journalLines);
            return journal;
        }
        private JournalPM CreateJournalDebitLineFromVat(JournalPM journal, ARInvoicePM invoice)
        {
            JournalLinePM invoiceCurrencyLine = journal.JournalLines.Where(d => d.CurrencyId == invoice.InvoiceCurrencyId).FirstOrDefault();
            List<ARInvoiceTotalVAT> ARInvoiceTotalVATs = GetARInvoiceTotalVATs(invoice);
            counter = journal.JournalLines.Count();
            foreach (ARInvoiceTotalVAT vat in ARInvoiceTotalVATs)
            {
                if (invoiceCurrencyLine != null)
                {
                    journal.JournalLines.Where(w => w.CurrencyId == invoice.InvoiceCurrencyId).ToList().ForEach(s => s.LocalAmount = s.ForeignAmount= s.LocalAmount + (decimal)vat.LocalVATAmount);
                }
                else
                {
                    JournalLinePM journalLine = CreateDebitJournalLineForVatLine(journal, invoice, vat);                 
                    journal.JournalLines.Add(journalLine);
                }
            }
            return journal;
        }
       private List<ARInvoiceTotalVAT> GetARInvoiceTotalVATs(ARInvoicePM invoice)
        {

            List<ARInvoiceTotalVAT> ARInvoiceTotalVATs = new List<ARInvoiceTotalVAT>();
            ARInvoiceTotalVATRepository vatRepository = new ARInvoiceTotalVATRepository(tenant);
            ARInvoiceTotalVATs = vatRepository.GetInvoiceTotalVatsForInvoiceWithoutZeroVATPercent(invoice.Id, tenant).ToList();
            return ARInvoiceTotalVATs;
        }
        private JournalLinePM CreateDebitJournalLineForVatLine(JournalPM journal, ARInvoicePM invoice, ARInvoiceTotalVAT vat)
        {
            JournalLinePM journaLine = new JournalLinePM
            {
                Tenant = tenant,
                ActionCode = "2",
                ActionTypeCodeEnum = JournalActionTypeEnum.Debit,
                JournalId = journal.Id,
                Line = ++counter,
                DocumentDate = invoice.InvoiceDate.Value,
                AccountingDate = invoice.InvoiceDate.Value,
                DueDate = invoice.DueDate.Value,
                LocalAmount = (decimal)vat.LocalVATAmount,
                CurrencyId = invoice.InvoiceCurrencyId,
                ForeignAmount = (decimal)vat.InvoiceCurrencyVATAmount,
                ExchangeRate = (decimal)invoice.InvoiceCurrencyExchangeRate,
                Reference1 = invoice.CustomerRef != null ? invoice.CustomerRef : invoice.InvoiceNumber,
                Reference2 = invoice.MainEntityReference,
                Reference3 = !string.IsNullOrEmpty(invoice.HouseNumber) ? invoice.HouseNumber : invoice.MasterNumber,
                DebitAccountId = glAccount == null ? "" : glAccount.Id,
            };

            return journaLine;
        }
        private FullAccountingSettingPM getFullAccountingSettings(int tenant)
        {
            FullAccountingSettingPM accountingSettings;
            IFullAccountingSettingQueryServiceExt query = ContainerAccessor.Container.Resolve(typeof(IFullAccountingSettingQueryServiceExt), "FullAccountingSettingQueryServiceExt", new ParameterOverride("", 1)) as IFullAccountingSettingQueryServiceExt;
            accountingSettings = query.GetFullAccountingSettingByTenant( tenant);
            return accountingSettings;
        }
        private GLAccountPM GetGLAccount(ARInvoicePM invoice)
        {
            IGLAccountQueryServiceExt glAccountQuery = ContainerAccessor.Container.Resolve(typeof(IGLAccountQueryServiceExt), "GLAccountQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountQueryServiceExt;
            if (invoice.BillToGLAccountId == null)
            {
                GLAccountPM debitGLAcount = getDebitGLAccount(invoice.BillToId, invoice.Tenant);                
                GLAccountPM splittedAccount = glAccountQuery.GetSplittedByCurrencyGLAccount(debitGLAcount.Id, invoice.Tenant, invoice.InvoiceCurrencyId);
                if (splittedAccount != null)
                    return splittedAccount;
                else
                    return debitGLAcount;
            }
            else
            {
                return glAccountQuery.GetSingleGLAccountPM(invoice.BillToGLAccountId, invoice.Tenant);
            }
        }

        private GLAccountPM getDebitGLAccount(string billToId, int tenant)
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
        #endregion

        #region Tax Report variables 
        private void CalculationOfTaxReportfields(ARInvoicePM theEntityPm, bool isApprovingInvoice)
        {
            int tenant = theEntityPm.Tenant;
            TenantRepository tenantRepository = new TenantRepository(tenant);
            Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);
            if (tenantPOCO.AccountingActivated && isApprovingInvoice)
            {
                if (theEntityPm.InvoiceLines != null && theEntityPm.InvoiceLines.Count() > 0)
                {
                    theEntityPm.TotalAmountForTaxReport = (decimal)theEntityPm.InvoiceLines.Where(d => d.LineActionCode == "1").Sum(a => a.LocalCurrencyAmount);
                    theEntityPm.TotaVatableAmountForTaxReport = (decimal)theEntityPm.InvoiceLines.Where(d => d.VatPercentage != 0 && d.LineActionCode == "1").Sum(a => a.LocalCurrencyAmount);

                    List<ARInvoiceTotalVAT> ARInvoiceTotalVATs = new List<ARInvoiceTotalVAT>();
                    ARInvoiceTotalVATRepository vatRepository = new ARInvoiceTotalVATRepository(tenant);
                    ARInvoiceTotalVATs = vatRepository.GetInvoiceTotalVatsForInvoiceWithoutZeroVATPercent(theEntityPm.Id, tenant).ToList();
                    if(ARInvoiceTotalVATs != null && ARInvoiceTotalVATs.Count() > 0)
                    {
                        theEntityPm.TotalVAT = (decimal)ARInvoiceTotalVATs.Sum(a=>a.LocalVATAmount);
                    }
                    else if ( group_data != null &&  group_data.Count() >0)
                    {
                        theEntityPm.TotalVAT = (decimal)group_data.Sum(a => MethodHelper.Roundd((a.LocalCurrencyAmount * MethodHelper.Roundd(a.VatTypePercentage, 2) / 100), 2));
                    }
                }
            }
        }
        #endregion 

        private void GetForeignFields()
        {
            //ARInvoiceRepository newARInvoiceRepository = new ARInvoiceRepository(this.tenant);
            //ARInvoiceQuery newARInvoiceQuery = new ARInvoiceQuery(newARInvoiceRepository);
            //this.entityPM = newARInvoiceQuery.GetSinglePM(this.entityPM.Id, this.tenant);

            ARInvoiceStatusRepository myRepository = new ARInvoiceStatusRepository(this.objectContext);
            ARInvoiceStatus myStatus = myRepository.GetSingleARInvoiceStatus(entityPM.StatusCode);
            if (myStatus != null)
            {
                entityPM.StatusName = myStatus.Name;
            }

            ARInvoiceTransferStatusRepository aRInvoiceTransferStatusRepository = new ARInvoiceTransferStatusRepository(this.objectContext);
            ARInvoiceTransferStatus t_status = aRInvoiceTransferStatusRepository.GetSingleARInvoiceTransferStatus(invoice.TransferStatusCode);
            entityPM.TransferStatusName = t_status.Name;

            TenantRepository tenantRepository = new TenantRepository(myCommonContext);
            Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);
            if (tenantPOCO != null && tenantPOCO.AccountingActivated)
            {
                JournalRepository rep = new JournalRepository(tenant);
                JournalEntity journal = rep.GetJournalByAccountingEntityIdAndTypeCode(entityPM.Id,"2", tenant);
                if (journal != null)
                {
                    entityPM.JournalId = journal.JournalId;
                    entityPM.JournalNumber = journal.JournalNumber;
                }
            }

            if (entityPM.IsConsolidationInvoice)
            {
                entityPM.ConstituentInvoices = (from d in this.objectContext.ARInvoices
                                                where d.Tenant == tenant
                                                && d.IsConstituentInvoice == true
                                                && d.ConsolidationInvoiceId == entityPM.Id
                                                select new ConstituentPM()
                                                {
                                                    Id = d.Id,
                                                    Tenant = d.Tenant,
                                                    ConsolidationInvoiceId = d.ConsolidationInvoiceId,
                                                }).ToList();
            }

            if (isNewEntity)
            {
                ARInvoiceEntityQuery arInvoiceEntityQuery = new ARInvoiceEntityQuery(invoiceEntityRepository);
                if(entityPM.ARInvoiceTypeCode != "IT")
                entityPM.InvoiceEntities = arInvoiceEntityQuery.GetInvoiceEntityPMsForInvoice(entityPM.Id, tenant);
            }
           
        }
        private void RunStoredProcedures()
        {
            if (!entityPM.IsConsolidationInvoice && !entityPM.IsGeneralInvoice)
            {
                if (entityPM.MainEntityId != null)
                {
                    UpdateShipmentProfitClass.UpdateReceivables(entityPM.MainEntityId, tenant, true);
                    UpdateShipmentProfitClass.UpdateProfit(entityPM.MainEntityId, entityPM.Tenant);

                    if (this.isApprovingInvoice || this.isVoidingInvoice)
                    {
                        // allActiveShipmentIds

                        UpdateShipmentProfitClass.UpdateShipmentARInvoices(entityPM.MainEntityId, entityPM.Tenant);
                    }

                    else if (isNewEntity)
                    {
                        if (this.entityPM.IsAutoCredit || this.entityPM.IsConstituentInvoice)
                        {
                            UpdateShipmentProfitClass.UpdateShipmentARInvoices(entityPM.MainEntityId, entityPM.Tenant);
                        }
                    }
                }
            }
        }
        private void AfterServiceFinished()
        {
            if (this.isNewEntity)
            {
                this.OnApprovingInvoice();

                if (this.isApprovingInvoice || entityPM.IsAutoCredit)
                {
                    this.sATInterfaceHelper.SendSATRequestFile(entityPM, invoice);
                    invoiceRepository.Update(invoice);
                    invoiceRepository.SubmitChanges();
                }

                if (entityPM.IsAutoCredit)
                {
                    this.OnCreatingAutoCredit();
                }
            }

            else
            {
                this.UpdatePaymentsNumbers();
                this.OnApprovingInvoice();
                this.OnVoidingInvoise();
            }
        }
        private void UpdatePaymentsNumbers()
        {
            if (this.isUpdatingPayments)
            {
                List<string> ids = invoicePaymentsChangeSet.Where(d => d.ChangeSetOp == ChangeSetOperation.Insert || d.ChangeSetOp == ChangeSetOperation.Delete).Select(s => s.ARPaymentId).ToList();

                if (ids.Count > 0)
                {
                    ARPaymentRepository myRepository = new ARPaymentRepository(objectContext);

                    List<ARPayment> allPayments = (from d in objectContext.ARPayments
                                                   where ids.Contains(d.Id)
                                                   select d).ToList();

                    foreach (ARPayment entity in allPayments)
                    {
                        string invoiceNumber = null;
                        string shipmentNumber = null;

                        var entityCount = (from d in objectContext.ARInvoicePayments
                                           where d.Tenant == this.tenant && d.ARPaymentId == entity.Id
                                           select d).Count();

                        if (entityCount > 0)
                        {
                            if (entityCount == 1)
                            {
                                invoiceNumber = this.entityPM.InvoiceNumber;
                                shipmentNumber = this.entityPM.MainEntityReference;
                            }

                            else
                            {
                                invoiceNumber = "Multi";
                                shipmentNumber = "Multi";
                            }
                        }

                        if (entity.InvoiceNumber != invoiceNumber || entity.ShipmentNumber != shipmentNumber)
                        {
                            entity.InvoiceNumber = invoiceNumber;
                            entity.ShipmentNumber = shipmentNumber;
                            myRepository.Update(entity);
                        }
                    }

                    myRepository.SubmitChanges();
                }
            }
        }
        private void OnVoidingInvoise()
        {
            if (this.isVoidingInvoice)
            {
                this.UpdateShipmentRegistryDate();
                this.UpdSatehipmentFirstApprovalDate();
            }
        }
        private void OnApprovingInvoice()
        {
            if (this.isApprovingInvoice)
            {
                // Journal Work
                if (tenantPOCO.AccountingActivated)
                {
                    this.AddARInvoiceJournalAndJournalLines(entityPM, this.isApprovingInvoice);
                }
              
                // DropBox
                this.CreateARInvoiceMessage(this.isApprovingInvoice);

                if (!this.isNewEntity)
                {
                    // Approval on Create is handled inside the Create Method
                    this.sATInterfaceHelper.SendSATRequestFile(entityPM, invoice);
                }

                this.UpdateShipmentRegistryDate();
                this.UpdSatehipmentFirstApprovalDate();
            }

            // when creating auto credit invoice: press on back button then save, the invoice should be transferred
            else if(entityPM.IsAutoCredit)
            {
                this.CreateARInvoiceMessage(true);
            }
        }
        private void UpdateShipmentRegistryDate()
        {
            if (this.entityPM.IsConsolidationInvoice)
            {
                #region
                List<string> allConstituentsIds = new List<string>();

                if (this.isNewEntity)
                {
                    allConstituentsIds = entityPM.ConstituentInvoices.Select(s => s.Id).ToList();
                }

                else
                {
                    allConstituentsIds = invoiceConstituentsChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).Select(s => s.Id).ToList();
                }

                if (allConstituentsIds.Count > 0)
                {
                    List<string> allConstituentsShipmentsIds
                        = (from d in objectContext.ARInvoices
                           where d.Tenant == this.tenant
                           && d.MainEntityId != null
                           && d.IsConstituentInvoice == true
                           && allConstituentsIds.Contains(d.Id)
                           select d.MainEntityId).ToList();

                    foreach (string id in allConstituentsShipmentsIds)
                    {
                        this.RunRegistryDateProcedure(id);
                    }
                }
                #endregion
            }

            else if (this.entityPM.MainEntityId != null)
            {
                this.RunRegistryDateProcedure(this.entityPM.MainEntityId);
            }
        }
        private void UpdSatehipmentFirstApprovalDate()
        {
            if (this.entityPM.IsConsolidationInvoice)
            {
                #region
                List<string> allConstituentsIds = new List<string>();

                if (this.isNewEntity)
                {
                    allConstituentsIds = entityPM.ConstituentInvoices.Select(s => s.Id).ToList();
                }

                else
                {
                    allConstituentsIds = invoiceConstituentsChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).Select(s => s.Id).ToList();
                }

                if (allConstituentsIds.Count > 0)
                {
                    List<string> allConstituentsShipmentsIds
                        = (from d in objectContext.ARInvoices
                           where d.Tenant == this.tenant
                           && d.MainEntityId != null
                           && d.IsConstituentInvoice == true
                           && allConstituentsIds.Contains(d.Id)
                           select d.MainEntityId).ToList();

                    foreach (string id in allConstituentsShipmentsIds)
                    {
                        this.RunFirstApprovalDateProcedure(id);
                    }
                }
                #endregion
            }

            else if (this.entityPM.MainEntityId != null)
            {
                this.RunFirstApprovalDateProcedure(this.entityPM.MainEntityId);
            }
        }

        private void RunRegistryDateProcedure(string myShipmentId)
        {
            RunStoredProcedureClass.UpdateShipmentRegistryDate(myShipmentId, entityPM.Tenant);
        }
        private void RunFirstApprovalDateProcedure(string myShipmentId)
        {
            RunStoredProcedureClass.UpdateShipmentFirstApprovalDate(myShipmentId, entityPM.Tenant);
        }

        private void CheckLinesVatExcempt(ARInvoicePM invoicePM, bool isApprovingInvoice)
         {
            if (IsFullAccountingActivated(invoicePM.Tenant) && isApprovingInvoice)
            {
                foreach (ARInvoiceLinePM line in invoicePM.InvoiceLines)
                {
                    CheckLineVatExcempt(invoicePM.Tenant, line);
                }
            }
        }

        private void CheckLineVatExcempt(int tenant, ARInvoiceLinePM line)
        {
            GLAccountPM glaccount = GetGLAccountById(tenant, line.GLAccountId);

            if (glaccount.IsVATExempt == true && line.VatPercentage != 0)
                ShowErrorMessage("ARInvoice.O.CanNotCreditExempt", tenant);
            //else if (glaccount.IsVATExempt != true && line.VatPercentage == 0)
            //    ShowErrorMessage("ARInvoice.O.CanNotCreditCardIsNotExempt", tenant);

        }

        private void ShowErrorMessage(string textCode, int tenant)
        {
            bool showLocals = LoggedContactResolver.GetLoggedContactShowLocal(tenant);
            var msg = TextCodesTranslator.TranslateText(textCode, tenant, showLocals);

            if (string.IsNullOrWhiteSpace(msg))
            {
                if (textCode == "ARInvoice.O.CanNotCreditExempt")
                    msg = "Can not credit card exempt VAT if the amount is not exempt";
                if (textCode == "ARInvoice.O.CanNotCreditCardIsNotExempt")
                    msg = "Can not credit card that is not exempt VAT if the amount is exempt";
            }

            throw new ApplicationException(msg);
        }

        private ChargesTypePM GetChargeTypeById(int tenant, string id)
        {
            ChargesTypeQuery chargesTypeQuery = new ChargesTypeQuery(tenant);
            ChargesTypePM chargeTypePM = chargesTypeQuery.GetSingle(id, tenant);
            return chargeTypePM;
        }

        private GLAccountPM GetGLAccountById(int tenant, string id)
        {
            IGLAccountQueryServiceExt glAccountQuery = ContainerAccessor.Container.Resolve(typeof(IGLAccountQueryServiceExt), "GLAccountQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountQueryServiceExt;
            GLAccountPM glaccount = glAccountQuery.GetSingleGLAccountPM(id, tenant);
            return glaccount;
        }

        private bool IsFullAccountingActivated(int tenant)
        {
            TenantRepository tenantRepository = new TenantRepository(tenant);
            Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);
            bool isFullAccountingActivated = tenantPOCO.AccountingActivated;
            return isFullAccountingActivated;
        }

        public void UpdateConsolidationShipments()
        {
            if (this.entityPM.IsConsolidationInvoice)
            {
                List<ConsolidationServiceArgsItem> items = new List<ConsolidationServiceArgsItem>();

                if (this.isVoidingInvoice || this.isAutoCreditingInvoice)
                {
                    items = (from a in allConnectedInvoices
                             select new ConsolidationServiceArgsItem()
                             {
                                 ShipmentId = a.MainEntityId,
                                 ConstituentId = a.Id,
                             }).ToList();
                }

                else if (isApprovingInvoice)
                {
                    items = (from a in objectContext.ARInvoices
                             where
                             a.Tenant == tenant
                             && a.IsConstituentInvoice == true
                             && a.ConsolidationInvoiceId == entityPM.Id
                             && a.MainEntityId != null
                             select new ConsolidationServiceArgsItem()
                             {
                                 ShipmentId = a.MainEntityId,
                                 ConstituentId = a.Id,
                             }).ToList();
                } 

                if (items.Count > 0)
                {
                    string ConsolidationNumber = this.entityPM.InvoiceNumber;

                    if (this.entityPM.StatusCode == "DR" || this.entityPM.StatusCode == "VD" || this.isAutoCreditingInvoice)
                    {
                        ConsolidationNumber = null;
                    }

                    foreach (ConsolidationServiceArgsItem item in items)
                    {
                        UpdateShipmentProfitClass.UpdateConstituentShipment(this.entityPM.Id, item.ConstituentId, this.tenant);
                    }

                    List<string> allShipmentsIds = (from d in items group d by d.ShipmentId into g select g.Key).ToList();

                    foreach (string iShipmentId in allShipmentsIds)
                    {                       
                        UpdateShipmentProfitClass.UpdateShipmentARInvoices(iShipmentId, this.tenant, ConsolidationNumber);
                        UpdateShipmentProfitClass.UpdateProfit(iShipmentId, this.tenant);
                    }
                }
            }
        }

        private void BuildShipmentsNumbers()
        {
            ARInvoiceShipmentsDataBehaviour invoiceShipmentsNumbersBehaviour = new ARInvoiceShipmentsDataBehaviour(entityPM);
            invoiceShipmentsNumbersBehaviour.CopmuteShipmentsData();
        }

        private void CreateARInvoiceChargesConstraint(ARInvoiceLinePM invoiceLine)
        {
            if (!string.IsNullOrEmpty(invoiceLine.ReceivableId))
            {
                ARInvoiceChargesConstraint aRInvoiceChargesConstraint = new ARInvoiceChargesConstraint();
                aRInvoiceChargesConstraint.Id = IdCounter.GetNumber("ARInvoiceChargesConstraint", entityPM.Tenant).ToString();
                aRInvoiceChargesConstraint.Tenant = tenant;
                aRInvoiceChargesConstraint.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                aRInvoiceChargesConstraint.InvoiceLineId = invoiceLine.Id;
                aRInvoiceChargesConstraint.ReceivableId = invoiceLine.ReceivableId;
                ARInvoiceChargesConstraintRepository.Add(aRInvoiceChargesConstraint);
            }
        }
        private void DeleteARInvoiceChargesConstraint(ARInvoiceLine invoiceLine)
        {
            ARInvoiceChargesConstraint aRInvoiceChargesConstraint = ARInvoiceChargesConstraintRepository.GetARInvoiceChargesConstraintByReceivableId(invoiceLine.ReceivableId, tenant);
            if (aRInvoiceChargesConstraint != null)
            {
                ARInvoiceChargesConstraintRepository.Remove(aRInvoiceChargesConstraint);
            }
        }

        private void ComputeInvoiceLineAmounts(ARInvoiceLinePM invoiceLine )
        {
            this.ComputeInvoiceLineLocalAmount(invoiceLine);
            this.ComputeInvoiceLineProfitAmount(invoiceLine);
            this.ComputeInvoiceLineInvoiceAmount(invoiceLine);           
        }
        private void ComputeInvoiceLineLocalAmount(ARInvoiceLinePM invoiceLine)
        {
            if (entityPM.LocalCurrencyId == invoiceLine.ForiegnCurrencyId)
            {
                invoiceLine.LocalCurrencyAmount = invoiceLine.ForiegnCurrencyAmount;
            }
            else
            {
                invoiceLine.LocalCurrencyAmount = MethodHelper.Round((invoiceLine.ForiegnCurrencyAmount * invoiceLine.ForiegnExchangeRate), 2);
            }
        }
        private void ComputeInvoiceLineProfitAmount(ARInvoiceLinePM invoiceLine)
        {
            if (entityPM.ProfitCurrencyId == invoiceLine.ForiegnCurrencyId)
            {
                invoiceLine.ProfitCurrencyAmount = invoiceLine.ForiegnCurrencyAmount;
            }
            else if (entityPM.ProfitCurrencyId == entityPM.LocalCurrencyId)
            {
                invoiceLine.ProfitCurrencyAmount = invoiceLine.LocalCurrencyAmount;
            }
            else
            {
                invoiceLine.ProfitCurrencyAmount = MethodHelper.Round((invoiceLine.LocalCurrencyAmount / entityPM.ProfitCurrencyExchangeRate), 2);
            }
        }
        private void ComputeInvoiceLineInvoiceAmount(ARInvoiceLinePM invoiceLine)
        {
            if (entityPM.InvoiceCurrencyId == invoiceLine.ForiegnCurrencyId)
            {
                invoiceLine.InvoiceCurrencyAmount = invoiceLine.ForiegnCurrencyAmount;
            }
            else if (entityPM.InvoiceCurrencyId == entityPM.LocalCurrencyId)
            {
                invoiceLine.InvoiceCurrencyAmount = invoiceLine.LocalCurrencyAmount;
            }
            else if (entityPM.InvoiceCurrencyId == entityPM.ProfitCurrencyId)
            {
                invoiceLine.InvoiceCurrencyAmount = invoiceLine.ProfitCurrencyAmount;
            }
            else
            {
                if (IsFullAccountingActivated(entityPM.Tenant))
                {
                    invoiceLine.InvoiceCurrencyAmount = SetInvoiceCurrencyAmountForFullAccountingTenant(invoiceLine);
                }
                else
                {
                    invoiceLine.InvoiceCurrencyAmount = MethodHelper.Round((invoiceLine.LocalCurrencyAmount / entityPM.InvoiceCurrencyExchangeRate), 2);
                }
            }
        }
        private double? SetInvoiceCurrencyAmountForFullAccountingTenant(ARInvoiceLinePM aRInvoiceLinePM)
        {
            if(aRInvoiceLinePM.InvoiceCurrencyExchangeRate != null)
            {
                return MethodHelper.Round((aRInvoiceLinePM.LocalCurrencyAmount / aRInvoiceLinePM.InvoiceCurrencyExchangeRate), 2);
            }
            else
            {
                return aRInvoiceLinePM.InvoiceCurrencyAmount = MethodHelper.Round((aRInvoiceLinePM.LocalCurrencyAmount / entityPM.InvoiceCurrencyExchangeRate), 2);

            }
        }
        private void ComputeInvoiceAmounts()
        {
            if (IsFullAccountingActivated(entityPM.Tenant))
            {
                return;
            }
            
            List<ARInvoiceLinePM> invoiceLines = entityPM.InvoiceLines.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
            entityPM.SubTotalInLocalCurrency = MethodHelper.Round(invoiceLines.Sum(s => s.LocalCurrencyAmount), 2);
            entityPM.SubTotalInInvoiceCurrency = MethodHelper.Round(invoiceLines.Sum(s => s.InvoiceCurrencyAmount), 2);
            entityPM.AmountInLocalCurrency = MethodHelper.Round(entityPM.SubTotalInLocalCurrency + entityPM.TotalVATs.Sum(s => s.LocalVATAmount), 2);
            entityPM.AmountInInvoiceCurrency = MethodHelper.Round(entityPM.SubTotalInInvoiceCurrency + entityPM.TotalVATs.Sum(s => s.InvoiceCurrencyVATAmount), 2);
        }
    }
}
