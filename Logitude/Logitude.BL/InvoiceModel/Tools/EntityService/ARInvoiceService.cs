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

namespace Logitude.BL.InvoiceModel.Tools.EntityService
{
    public class ARInvoiceService
    {
        private int tenant;
        private bool isNewEntity;
        private bool isUpdateTotalVats;
        private bool isVoidingInvoice;
        private bool isApprovingInvoice;
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
        SATInterfaceHelper sATInterfaceHelper;
        AccountingSetting accountingSetting;
        private Tenant TenantObject;
        private string QBOARPaymentId;      
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


            this.vatTypeRepository = new VatTypeRepository(myCommonContext);
            this.accountingSettingRepository = new AccountingSettingRepository(myCommonContext);
            this.accountingSystemRepository = new AccountingSystemRepository(myCommonContext);
            this.contactRepository = new ContactRepository(myCommonContext);

            allShipments = new List<Shipment>();
            allReceivables = new List<ShipmentReceivable>();
            shipmentRepository = new ShipmentRepository(myShipmentContext);
            shipmentReceivableRepository = new ShipmentReceivableRepository(myShipmentContext);

            this.TenantObject = (from d in myCommonContext.Tenants where d.Id == tenant select d).FirstOrDefault();
            this.GetLoggedContact();
            this.GetAccountingSystem();
        }
        public ARInvoiceService(IInvoiceContext objectContext, int tenant, string loggedUserEmail)
        {
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

            this.vatTypeRepository = new VatTypeRepository(myCommonContext);
            this.accountingSettingRepository = new AccountingSettingRepository(myCommonContext);
            this.accountingSystemRepository = new AccountingSystemRepository(myCommonContext);
            this.contactRepository = new ContactRepository(myCommonContext);

            allShipments = new List<Shipment>();
            allReceivables = new List<ShipmentReceivable>();
            shipmentRepository = new ShipmentRepository(myShipmentContext);
            shipmentReceivableRepository = new ShipmentReceivableRepository(myShipmentContext);

            this.TenantObject = (from d in myCommonContext.Tenants where d.Id == tenant select d).FirstOrDefault();

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

            this.vatTypeRepository = new VatTypeRepository(commonMockContext);
            this.accountingSettingRepository = new AccountingSettingRepository(commonMockContext);
            this.accountingSystemRepository = new AccountingSystemRepository(commonMockContext);
            this.contactRepository = new ContactRepository(commonMockContext);

            allShipments = new List<Shipment>();
            allReceivables = new List<ShipmentReceivable>();
            shipmentRepository = new ShipmentRepository(shipmentMockContext);
            shipmentReceivableRepository = new ShipmentReceivableRepository(shipmentMockContext);

            this.TenantObject = (from d in commonMockContext.Tenants where d.Id == tenant select d).FirstOrDefault();

            this.GetLoggedContact();
            this.GetAccountingSystem();
        }
        private ContactPM loggedContact;

        private void GetLoggedContact()
        {

            string email = SecurityUtility.GetAuthenticatedUser();
           ContactQuery contactQuery = new ContactQuery(tenant);
            this.loggedContact = contactQuery.GetContactByNameAndTenant(email, tenant, true);

            if (this.loggedContact == null)
            {
                loggedContact = contactQuery.GetContactByEmailOnly(email, tenant);
                if (loggedContact != null)
                {
                    this.loggedContactId = loggedContact.Id;
                    this.loggedContactName = loggedContact.EnglishName;
                }
            }
            else
            {
                this.loggedContactId = loggedContact.Id;
                this.loggedContactName = loggedContact.EnglishName;

            }

           
        }

        private bool isJournal;
        private bool isExternal;
        private bool isTaxItemManaged;
        private bool isTransferToDropbox;
        private bool TransferToDropboxActivated;
        private void GetAccountingSystem()
        {
            this.accountingSetting = accountingSettingRepository.GetSingleAccountSetting(tenant);
            AccountingSystem accountingSystem = accountingSystemRepository.GetSingleAccountingSystem(accountingSetting.AccountingSystemCode);

            this.isJournal = accountingSystem.IsJournalMode;
            this.isExternal = accountingSystem.IsExternalCodesFromTable;
            this.isTaxItemManaged = accountingSystem.IsTaxItemManaged;
            this.isTransferToDropbox =  accountingSystem.CanTransferToDropbox;
            this.TransferToDropboxActivated = accountingSetting.TransferToDropboxActivated;
        }

        public void Create(ARInvoicePM theEntityPM)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPM;
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

            else if (!entityPM.IsGeneralInvoice)
            {
                this.GetShipmentsData(entityPM.InvoiceLines);
                this.UpdateInvoiceEntities();
            }

            this.UpdateInvoiceLines();
            this.UpdateTotalVats();
            this.BuildSearchFields();

            ARInvoiceHelper helper = new ARInvoiceHelper();
            helper.ARInvoiceQuickbooksValidating(entityPM, this.isApprovingInvoice, isNewEntity,this.objectContext,this.myCommonContext,isVoidingInvoice);
         
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
            this.OnApprovingInvoice();
          
            if (this.isApprovingInvoice || entityPM.IsAutoCredit)
            {
                this.sATInterfaceHelper.SendSATRequestFile(entityPM, invoice);
                invoiceRepository.Update(invoice);
                invoiceRepository.SubmitChanges();
            }
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
            this.isVoidingInvoice = entityPM.SetVoided;
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

                if(invoice.ApprovedDate != null)
                {
                    throw new ApplicationException("This invoice is already approved");
                }
            }

            if (this.entityPM.SetVoided)
            {
                this.sATInterfaceHelper.SendSATCancellationRequest(entityPM, invoice);
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
                
                ARInvoiceHelper helper = new ARInvoiceHelper();
                if (entityPM.SetReSendQBO)
                {
                    helper.ARInvoiceQuickbooksValidating(entityPM, true, isNewEntity, this.objectContext, this.myCommonContext, isVoidingInvoice);

                }
                else
                {
                    helper.ARInvoiceQuickbooksValidating(entityPM, this.isApprovingInvoice, isNewEntity, this.objectContext, this.myCommonContext, isVoidingInvoice);
                }

                // Full Accounting - Tax Fields Work 
                this.CalculationOfTaxReportfields(entityPM, isApprovingInvoice);
               

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
                }

                this.BuildSearchFields();

               
            }

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
                    service.ARPaymentQuickbooksValidating(paymentPM, true, false, payment, this.objectContext, this.myCommonContext, false, paymentPM.SetReSendQBO, false);
                }
            }

            this.GetForeignFields();
            this.RunStoredProcedures();
            this.UpdatePaymentsNumbers();
            this.OnVoidingInvoise();
            this.OnApprovingInvoice();
        }

        private void ARInvoiceStockNumber()
        {
            if(!this.isApprovingInvoice && !this.isVoidingInvoice)
            {
                IInvoiceContext context = InvoiceContext.GetContext(tenant);
                ARInvoiceStockLineRepository aRInvoiceStockLineRepository = new ARInvoiceStockLineRepository(tenant);
                ARInvoiceStockQuery aRInvoiceStockQuery = new ARInvoiceStockQuery(tenant);
                ARInvoiceStockService aRInvoiceStockService = new ARInvoiceStockService(context, tenant);
                ARInvoiceStockPM stock = new ARInvoiceStockPM();
                ARInvoiceStockLine stockLine = new ARInvoiceStockLine();

                if (this.isNewEntity)
                {
                    if(entityPM.ARInvoiceStockId != null)
                    {
                        stockLine = aRInvoiceStockLineRepository.GetSingleARInvoiceStockLine(entityPM.ARInvoiceStockId, tenant);
                        if(stockLine != null)
                        {
                            stockLine.IsUsed = true;
                            stockLine.ARInvoiceId = entityPM.Id;
                            stockLine.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                            stockLine.UpdatedByUserId = this.loggedContact.Id;
                            stockLine.ShipmentNumber = entityPM.MainEntityReference;
                            aRInvoiceStockLineRepository.Update(stockLine);
                            aRInvoiceStockLineRepository.SubmitChanges();
                            stock = aRInvoiceStockQuery.GetSinglePM(stockLine.ARInvoiceStockId, tenant);
                            aRInvoiceStockService.Update(stock,false);
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
                            stockLine = aRInvoiceStockLineRepository.GetSingleARInvoiceStockLine( this.invoice.ARInvoiceStockId, tenant);
                            stockLine.IsUsed = false;
                            stockLine.ARInvoiceId = null;
                            stockLine.ShipmentNumber = null;
                            stockLine.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                            stockLine.UpdatedByUserId = this.loggedContact.Id;
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
                            stockLine.UpdatedByUserId = this.loggedContact.Id;
                            aRInvoiceStockLineRepository.Update(stockLine);
                            aRInvoiceStockLineRepository.SubmitChanges();
                            stock = aRInvoiceStockQuery.GetSinglePM(stockLine.ARInvoiceStockId, tenant);
                            this.CreateInvoiceStockEvent("NTFS", entityPM.Id, stockLine.Number + " added from "+ stock.Name);
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
            FlatFile.Append("INVOICE DATE|" + String.Format("{0:dd/MM/yyyy}", ((DateTime)ARInvoice.InvoiceDate))+"|");
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
          List<ARInvoiceTotalVATPM> arInvoicesTotalvats=arInvoiceTotalVatQuery.GetTotalVATs(ARInvoice.Id, tenant).ToList();
            for(int i=0;i< arInvoicesTotalvats.Count; i++)
            {
                if (arInvoicesTotalvats[i].VATPercent == 16) {
                    FlatFile.Append("STD 16% INVOICE|"+ arInvoicesTotalvats [i].InvoiceCurrencyVATAmount+ "|");                   
                }
                else
                {
                    if(arInvoicesTotalvats[i].VATPercent != 0)
                    FlatFile.Append("TOTAL VAT TYPE "+ arInvoicesTotalvats[i] .VATPercent+ "% |" + arInvoicesTotalvats[i].InvoiceCurrencyVATAmount + "|");
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
                }
            }

            invoice.StatusCode = entityPM.StatusCode = "LL";
            invoice.UpdateDate = entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            invoice.UpdatedByUserId = entityPM.UpdatedByUserId = loggedContactId;
        }
        public string CreateAutoCredit(string entityId, bool IsInvoiceNumberManuallySet, string AutoCreditManualNumber, DateTime? AutoCreditDate)
        {
            ARInvoice entityPOCO = invoiceRepository.GetSingleInvoice(entityId);

            if (entityPOCO.StatusCode == "AR")
            {
                throw new ApplicationException("this invoice is already auto credited");
            }

            else
            {
                ARInvoiceQuery entityQuery = new ARInvoiceQuery(invoiceRepository);
                ARInvoicePM oldEntityPM = entityQuery.GetSinglePM(entityId, tenant);

                // Build New Invoice
                ARInvoicePM newInvoicePM = new ARInvoicePM()
                {
                    #region
                    Tenant = tenant,
                    StatusCode = "AC",
                    IsAutoCredit = true,
                    ARInvoiceTypeCode = oldEntityPM.ARInvoiceTypeCode == "CI" ? "CC" : "CD",
                    DebitAccount = oldEntityPM.DebitAccount,
                    TransferStatusCode = oldEntityPM.TransferStatusCode,
                    BillToAddressId = oldEntityPM.BillToAddressId,
                    BillToId = oldEntityPM.BillToId,
                    InternalNotes = oldEntityPM.InternalNotes,
                    InvoiceCurrencyExchangeRate = oldEntityPM.InvoiceCurrencyExchangeRate,
                    InvoiceCurrencyId = oldEntityPM.InvoiceCurrencyId,
                    PrintNotes = oldEntityPM.PrintNotes,
                    PaymentTermId = oldEntityPM.PaymentTermId,
                    PrepaidCollectId = oldEntityPM.PrepaidCollectId,
                    LocalCurrencyId = oldEntityPM.LocalCurrencyId,
                    VatNumber = oldEntityPM.VatNumber,
                    CreatedByUserId = oldEntityPM.CreatedByUserId,
                    IssuedByUserId = oldEntityPM.IssuedByUserId,
                    PrintByUserId = oldEntityPM.PrintByUserId,
                    InvoiceDate = AutoCreditDate != null ? AutoCreditDate.Value : TenantServerConfigration.GetCurrentDateTime(tenant).Date,
                    DueDate = oldEntityPM.DueDate,
                    PrintDate = oldEntityPM.PrintDate,
                    Sent = oldEntityPM.Sent,
                    ExchangeRateDate = oldEntityPM.ExchangeRateDate,
                    BranchId = oldEntityPM.BranchId,
                    ExpectedPaymentDate = oldEntityPM.ExpectedPaymentDate,
                    ProfitCurrencyId = oldEntityPM.ProfitCurrencyId,
                    ProfitCurrencyExchangeRate = oldEntityPM.ProfitCurrencyExchangeRate,
                    MainEntityId = oldEntityPM.MainEntityId,
                    MainEntityReference = oldEntityPM.MainEntityReference,
                    MainEntityStatus = oldEntityPM.MainEntityStatus,
                    AccountingExternalCode = oldEntityPM.AccountingExternalCode,                    
                    IsConstituentInvoice = oldEntityPM.IsConstituentInvoice,
                    IsConsolidationInvoice = oldEntityPM.IsConsolidationInvoice,
                    SubTotalInInvoiceCurrency = oldEntityPM.SubTotalInInvoiceCurrency * -1,
                    SubTotalInLocalCurrency = oldEntityPM.SubTotalInLocalCurrency * -1,
                    AmountInInvoiceCurrency = oldEntityPM.AmountInInvoiceCurrency * -1,
                    AmountInLocalCurrency = oldEntityPM.AmountInLocalCurrency * -1,
                    AmountInProfitCurrency = oldEntityPM.AmountInProfitCurrency * -1,
                    AmountDue = 0,
                    AmountDueInLocalCurrency = 0,
                    AmountDueInProfitCurrency = 0,
                    CreditedByARInvoiceId = entityId,
                    IsGeneralInvoice = oldEntityPM.IsGeneralInvoice,
                    SalesmanUserId = oldEntityPM.SalesmanUserId,
                    SATPaymentMethodCode = oldEntityPM.SATPaymentMethodCode,
                    MetodoPagoCode = oldEntityPM.MetodoPagoCode,
                    #endregion
                };

                if (newInvoicePM.IsConsolidationInvoice)
                {
                    newInvoicePM.MainEntityId = null;
                    newInvoicePM.MainEntityReference = null;
                    newInvoicePM.MainEntityStatus = null;
                }

                if (newInvoicePM.IsConstituentInvoice)
                {
                    newInvoicePM.IsClosed = true;
                    newInvoicePM.ConsolidationInvoiceId = null;
                }

                if (IsInvoiceNumberManuallySet)
                {
                    newInvoicePM.IsInvoiceNumberManuallySet = true;
                    newInvoicePM.InvoiceNumber = AutoCreditManualNumber;
                }

                int i = 1;
                foreach (ARInvoiceLinePM item in oldEntityPM.InvoiceLines)
                {
                    #region
                    ARInvoiceLinePM newInvoiceLine = new ARInvoiceLinePM()
                    {
                        Tenant = item.Tenant,
                        ChargesTypeId = item.ChargesTypeId,
                        CreditAccount = item.CreditAccount,
                        Description = item.Description,
                        ForiegnCurrencyId = item.ForiegnCurrencyId,
                        ForiegnExchangeRate = item.ForiegnExchangeRate,
                        VatTypeId = item.VatTypeId,
                        LineNumber = i,
                        MeasurementId = item.MeasurementId,
                        EntityId = item.EntityId,
                        EntityReference = item.EntityReference,
                        ViewOrder = item.ViewOrder,
                        ExternalTAXItemId = item.ExternalTAXItemId,
                        ExternalVATCard = item.ExternalVATCard,
                        ForiegnCurrencyCode = item.ForiegnCurrencyCode,
                        InvoiceCurrencyCode = item.InvoiceCurrencyCode,
                        InvoiceLocalCurrencyCode = item.InvoiceLocalCurrencyCode,
                        MeasurementCode = item.MeasurementCode,
                        VatTypeName = item.VatTypeName,
                        IsExchangeRateFixed = item.IsExchangeRateFixed,
                        LocalDescription = item.LocalDescription,
                        PrepaidCollectId = item.PrepaidCollectId,
                        VatPercentage = item.VatPercentage,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice * -1,
                        ForiegnCurrencyAmount = item.ForiegnCurrencyAmount * -1,
                        LocalCurrencyAmount = item.LocalCurrencyAmount * -1,
                        ProfitCurrencyAmount = item.ProfitCurrencyAmount * -1,
                        InvoiceCurrencyAmount = item.InvoiceCurrencyAmount * -1,
                        IsExpense = item.IsExpense,
                        GLAccountId = item.GLAccountId,
                    };

                    newInvoicePM.InvoiceLines.Add(newInvoiceLine);
                    i++;
                    #endregion
                }

                this.Create(newInvoicePM);
                this.AddARInvoiceJournalAndJournalLines(newInvoicePM, true);

                // Update Old Invoice
                if (oldEntityPM.IsConsolidationInvoice)
                {
                    #region
                    List<ARInvoice> iConstituentInvoices = invoiceRepository.GetConnectedInvoices(tenant, entityId).ToList();

                    foreach (ARInvoice item in iConstituentInvoices)
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

                    List<ARInvoiceLine> lines = invoiceLineRepository.GetInvoiceLinesByInvoiceId(entityId, this.tenant).ToList();
                    foreach (ARInvoiceLine line in lines)
                    {
                        line.ReceivableId = null;
                        invoiceLineRepository.Update(line);
                    }
                    #endregion
                }

                entityPOCO.IsCancelled = true;
                entityPOCO.CancelledByARInvoiceId = newInvoicePM.Id;
                entityPOCO.StatusCode = "AR";
                entityPOCO.AmountDue = 0;
                entityPOCO.AmountDueInLocalCurrency = 0;
                entityPOCO.AmountDueInProfitCurrency = 0;
                invoiceRepository.Update(entityPOCO);
                invoiceRepository.SubmitChanges();            
                return newInvoicePM.Id;
            }
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
                    this.invoice = invoiceRepository.GetSingleInvoice(this.entityPM.Id);
                    List<ARInvoice> entities = new List<ARInvoice>();
                    entities.Add(this.invoice);
                    ARInvoiceMessageHelper myHelper = new ARInvoiceMessageHelper(entities, this.invoice.InvoiceNumber + ".xml", tenant, true);
                    myHelper.Transfer();
                }
            }
        }

        #region InitializeComponent

        private void InitializeComponent()
        {
            // DR: Draft
            // CN: Connected
            // NT: Not Connected
            // AD: Unpaid
            // PD: Paid
            // PP: Partially Paid
            // AC: Auto Credit
            // AR: Auto Credited
            // VD: Void

            if (string.IsNullOrEmpty(entityPM.Id))
            {
                entityPM.Id = IdCounter.GetNumber("ARInvoice", entityPM.Tenant).ToString();
            }

            if (entityPM.InvoiceDate != null)
            {
                entityPM.InvoiceDate = entityPM.InvoiceDate.Value.Date;
            }

            if (this.isNewEntity)
            {
                if (string.IsNullOrEmpty(entityPM.SalesmanUserId))
                {
                    if (!string.IsNullOrEmpty(entityPM.BillToId))
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

                if(string.IsNullOrEmpty(this.entityPM.MasterNumber) || string.IsNullOrEmpty(this.entityPM.HouseNumber))
                {
                    if (!string.IsNullOrEmpty(this.entityPM.MainEntityId))
                    {
                        Shipment myShipment = shipmentRepository.GetSingleShipment(this.entityPM.MainEntityId, tenant);
                        if(myShipment != null)
                        {
                            if(string.IsNullOrEmpty(this.entityPM.MasterNumber))
                            {
                                if (!string.IsNullOrEmpty(myShipment.MasterShipmentDataId))
                                {
                                    ShipmentMasterData myMaster = shipmentRepository.GetSingleShipmentMasterData(myShipment.MasterShipmentDataId, tenant);
                                    if (myMaster != null)
                                    {
                                        if (!string.IsNullOrEmpty(myMaster.Master))
                                        {
                                            if(!string.IsNullOrEmpty(myMaster.AirlinePrefix))
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

                DocumentOutRepository documentOutRepository = new DocumentOutRepository(myCommonContext);
                DocumentOut docOut = documentOutRepository.GetDocumentOutByEntityAndChildEntity(invoice.MainEntityId, invoice.Id);
                if (docOut != null)
                {
                    docOut.NeedsRebuild = true;
                    documentOutRepository.Update(docOut);
                    documentOutRepository.SubmitChanges();
                }
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

                                        CustomerQuery customerQuery = new CustomerQuery(customerRepository);
                                        CustomerPM customerpm = customerQuery.GetSinglePM(customer.Id, entityPM.Tenant);
                                        CustomerEmailAlert customerEmailAlert = new CustomerEmailAlert();
                                        customerEmailAlert.SendEmailAlert(customerpm, entityPM.Tenant, "GCFI", false);

                                        customerRepository.Update(customer);
                                        customerRepository.SubmitChanges();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void DisconnectAllConnectedInvoices()
        {
            List<ARInvoice> allConnectedInvoices = invoiceRepository.GetConnectedInvoices(tenant, entityPM.Id).ToList();

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

            if (this.entityPM.SetApproved && this.isTransferToDropbox && this.TransferToDropboxActivated && string.IsNullOrEmpty(entityPM.TransferError))
            {
                this.entityPM.TransferStatusCode = "TR";
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
                        entityPM.DebitAccount = myCard.ReceivablesAccountingCard;
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

                    if (!isNewEntity)
                    {
                        if (line.ChangeSetOp == ChangeSetOperation.None)
                        {
                            UpdateInvoiceLine(line);
                        }
                    }
                }
                #endregion

                #region VATs
                List<ARInvoiceTotalVAT> myTotalVATs = invoiceTotalVatRepository.GetInvoiceTotalVatsForInvoice(entityPM.Id, tenant).ToList();
                foreach (ARInvoiceTotalVAT itemVAT in myTotalVATs)
                {
                    if (FieldIsEmpty(itemVAT.ExternalVATCard) || FieldIsEmpty(itemVAT.ExternalTAXItemId))
                    {
                        VatType myVatType = VatTypeRepository.GetSingleVatType(itemVAT.VatTypeId, tenant, true);

                        if (FieldIsEmpty(itemVAT.ExternalVATCard))
                        {
                            if (this.accountingSetting.AccountingSystemCode == "HV" || this.accountingSetting.AccountingSystemCode == "RH")
                            {
                                itemVAT.ExternalVATCard = this.accountingSetting.ReceivableVATCard;
                            }

                            else if(myVatType != null)
                            {
                                itemVAT.ExternalVATCard = myVatType.ExternalVATCard;
                            }
                        }

                        if (FieldIsEmpty(itemVAT.ExternalTAXItemId))
                        {
                            itemVAT.ExternalTAXItemId = myVatType.ExternalTAXItemId;
                        }

                        invoiceTotalVatRepository.Update(itemVAT);
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

            else if (entityPM.TransferStatusCode == "IP" || entityPM.TransferStatusCode=="ET")
            {
                isInitializing = false;            
            }

            if (isInitializing)
            {
                #region

                bool isReady = true;
                string myError = null;
                string ExternalCodeError = "External Code is missing";
                string paymentTermError = "Payment Term External Id is missing";
                string vatError = "External VAT Card is missing";
                string linesError = "Credit Account is missing";

                if (FieldIsEmpty(entityPM.DebitAccount))
                {
                    isReady = false;
                    myError = "Debit Account is missing";
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
                    if (myLines.Where(d => d.CreditAccount == null || (d.CreditAccount != null && string.IsNullOrEmpty(d.CreditAccount.Trim()))).Any())
                    {
                        isReady = false;
                        myError = string.IsNullOrEmpty(myError) ? linesError : myError + "," + linesError;
                    }

                    var myGroup = (from a in myLines
                                   where a.VatTypeId != null
                                   && a.VatPercentage != null
                                   && a.VatPercentage != 0
                                   group a by new { a.VatTypeId, a.VatPercentage, a.ExternalVATCard, a.ExternalTAXItemId } into g
                                   select new
                                   {
                                       VatTypeId = g.Key.VatTypeId,
                                       VatPercentage = g.Key.VatPercentage,
                                       ExternalVATCard = g.Key.ExternalVATCard,
                                       ExternalTAXItemId = g.Key.ExternalTAXItemId,
                                   });

                    foreach (var g in myGroup)
                    {
                        if (FieldIsEmpty(g.ExternalVATCard))
                        {
                            isReady = false;
                            vatError = "External VAT Card is missing";
                            myError = string.IsNullOrEmpty(myError) ? vatError : myError + "," + vatError;
                            break;
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
                    entityPM.TransferError = myError;
                }
                #endregion
            }
        }
        private void InitializeGLAccountFields()
        {
            if (entityPM.SetApproved)
            {
                TenantRepository tenantRepository = new TenantRepository(tenant);
                Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);

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
                                throw new Exception("The Receivable GLAccount of the Charge Type " + myChargesType.EnglishName + " is NULL");
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
                    foreach(ARInvoiceLinePM item in newLines)
                    {
                        ShipmentReceivable myReceivable = this.allReceivables.Where(d => d.Id == item.ReceivableId).FirstOrDefault();
                        if(myReceivable == null)
                        {
                            throw new ApplicationException("Some of invoice lines are missing receivables");
                        }
                    }
                }
            }
            
            Shipment shipment = allShipments.Where(d => d.Id == entityPM.MainEntityId).FirstOrDefault();
            if(shipment != null)
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

                            if (item.MeasurementCode == "PRVL" || item.MeasurementCode == "PRFR")
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

            switch (entityPM.StatusCode)
            {
                case "NT":
                case "CN":
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
                shipmentReceivableRepository.Update(myReceivable);

                List<ShipmentReceivable> ChildReceivables = shipmentReceivableRepository.GetShipmentReceivablesByParentId(receivableId, tenant);
                foreach(ShipmentReceivable myChild in ChildReceivables)
                {
                    myChild.ARInvoiceLineId = null;
                    myChild.ARInvoiceId = null;
                    myChild.ShipmentReceivableLineStatusCode = "OAMT";
                    shipmentReceivableRepository.Update(myChild);
                }

                shipmentReceivableRepository.SubmitChanges();
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
                                        newItem.ExternalVatCard = lineVatType.ExternalVATCard;
                                    }
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
                                            newItem.ExternalVatCard = vatType.ExternalVATCard;
                                        }
                                    }

                                    group_Source.Add(newItem);
                                }
                            }
                        }
                        #endregion
                    }

                    List<InvoiceTotalsClass> group_data
                        = (from items in group_Source
                           group items by new { items.VatTypeId, items.VatTypePercentage, items.ExternalVatCard, items.ExternalTAXItemId } into g
                           select new InvoiceTotalsClass()
                           {
                               Id = g.Key.VatTypeId,
                               VatTypeId = g.Key.VatTypeId,
                               VatTypePercentage = g.Key.VatTypePercentage,
                               LocalCurrencyAmount = g.Sum(s => s.LocalCurrencyAmount),
                               InvoiceCurrencyAmount = g.Sum(s => s.InvoiceCurrencyAmount),
                               ProfitCurrencyAmount = g.Sum(s => s.ProfitCurrencyAmount),
                               ExternalVatCard = g.Key.ExternalVatCard,
                               ExternalTAXItemId = g.Key.ExternalTAXItemId
                           }).ToList();

                    foreach (InvoiceTotalsClass item in group_data)
                    {
                        ARInvoiceTotalVAT record = new ARInvoiceTotalVAT()
                        {
                            Id = IdCounter.GetNumber("ARInvoiceTotalVAT", entityPM.Tenant).ToString(),
                            Tenant = entityPM.Tenant,
                            ARInvoiceId = entityPM.Id,
                            VatTypeId = item.Id,
                            VatPercent = MethodHelper.Roundd(item.VatTypePercentage, 2),
                            LocalVatableAmount = MethodHelper.Roundd(item.LocalCurrencyAmount, 2),
                            InvoiceCurrencyVatableAmount = MethodHelper.Roundd(item.InvoiceCurrencyAmount, 2),
                            ProfitVatableAmount = MethodHelper.Round(item.ProfitCurrencyAmount, 2),
                            ExternalVATCard = item.ExternalVatCard,
                            ExternalTAXItemId = item.ExternalTAXItemId
                        };

                        record.LocalVATAmount = MethodHelper.Roundd((record.LocalVatableAmount * record.VatPercent / 100), 2);
                        record.InvoiceCurrencyVATAmount = MethodHelper.Roundd((record.InvoiceCurrencyVatableAmount * record.VatPercent / 100), 2);
                        record.ProfitCurrencyVATAmount = MethodHelper.Roundd((record.ProfitVatableAmount * record.VatPercent / 100), 2);
                        invoiceTotalVatRepository.Add(record);

                        sumOfVATsAmounts += record.InvoiceCurrencyVATAmount;
                        sumOfVATsAmounts_Local += record.LocalVATAmount;
                        sumOfVATsAmounts_Profit += record.ProfitCurrencyVATAmount;
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
                                vatExternalCard = vatType.ExternalVATCard;
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
                                                                &&a.IsExpense == item.IsExpense
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

            ARInvoiceLine invoiceLine = new ARInvoiceLine();
            ARInvoiceMapping.MapInvoiceLine(item, invoiceLine, true);
            invoiceLineRepository.Add(invoiceLine);

            if (!this.isNewEntity)
            {
                invoiceLineRepository.SubmitChanges();
            }
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
            invoiceLineRepository.Remove(invoiceLine);
        }
        private void DisconnectInvoiceLine(ARInvoiceLinePM item)
        {
            ARInvoiceLine line = invoiceLineRepository.GetSingleInvoiceLine(item.Id);
            line.ReceivableId = null;
            item.ReceivableId = null;
            invoiceLineRepository.Update(line);
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
            if (allActiveShipmentIds != null)
            {
                if (allActiveShipmentIds.Count > 0)
                {
                    List<Shipment> iActiveShipments = this.allShipments.Where(d => allActiveShipmentIds.Contains(d.Id)).ToList();
                    foreach (Shipment iShipment in iActiveShipments)
                    {
                        MethodHelper.AddToSearchFields(ref mySearchFields, iShipment.ShipmentNumber);
                    }
                }
            }
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

            if (mySearchFields.Length > 4000)
            {
                mySearchFields = mySearchFields.Substring(0, 4000);
            }

            entityPM.SearchFields = mySearchFields;
            invoice.SearchFields = mySearchFields;
        }
        #endregion

        #region Journal & Journal Lines
        private void AddARInvoiceJournalAndJournalLines(ARInvoicePM theEntityPm, bool setApproved)
        {
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

                    // Insert Journal Lines 
                    // [Debit]
                    GLAccountPM glAccount = getDebitGLAccount(theEntityPm.BillToId, theEntityPm.Tenant);
                    JournalLinePM journalLine = new JournalLinePM();
                    journalLine.Tenant = tenant;
                    journalLine.JournalId = journal.Id;
                    journalLine.Line = 1;
                    journalLine.ActionCode = "2";
                    journalLine.ActionTypeCodeEnum = MyJournalActionTypeEnum.Debit;
                    journalLine.DocumentDate = theEntityPm.InvoiceDate.Value;
                    journalLine.AccountingDate = theEntityPm.InvoiceDate.Value;
                    journalLine.DueDate = theEntityPm.DueDate.Value;
                    journalLine.LocalAmount = (decimal)theEntityPm.AmountInLocalCurrency;
                    journalLine.CurrencyId = theEntityPm.InvoiceCurrencyId;
                    journalLine.ForeignAmount = (decimal)theEntityPm.AmountInInvoiceCurrency;
                    journalLine.ExchangeRate = (decimal)theEntityPm.InvoiceCurrencyExchangeRate;
                    journalLine.Reference1 = theEntityPm.InvoiceNumber;
                    journalLine.Reference2 = theEntityPm.MainEntityReference;
                    journalLine.Reference3 = !string.IsNullOrEmpty(theEntityPm.HouseNumber) ? theEntityPm.HouseNumber : theEntityPm.MasterNumber;
                    journalLine.Notes = theEntityPm.InternalNotes;
                    journalLine.DebitAccountId = glAccount == null ? "" : glAccount.Id;
                    journalLine.DebitControlAccountId = glAccount == null ? "" : glAccount.ControlAccountId;
                    journalLine.ChangeSetOp = ChangeSetOperation.Insert;
                    journal.JournalLines.Add(journalLine);

                    // [Credit]
                    journalLine = new JournalLinePM();
                    int counter = 1;
                    List<JournalLinePM> journalLines = (from d in theEntityPm.InvoiceLines
                                                        group d by new { d.GLAccountId, d.ForiegnCurrencyId, d.ForiegnExchangeRate } into g
                                                        select new JournalLinePM()
                                                        {
                                                            Tenant = tenant,
                                                            ActionCode = "1",
                                                            ActionTypeCodeEnum = MyJournalActionTypeEnum.Credit,
                                                            JournalId = journal.Id,
                                                            CreditAccountId = g.Key.GLAccountId,
                                                            Line = ++counter,
                                                            DocumentDate = theEntityPm.InvoiceDate.Value,
                                                            AccountingDate = theEntityPm.InvoiceDate.Value,
                                                            DueDate = theEntityPm.DueDate.Value,
                                                            LocalAmount = (decimal)g.Sum(a => a.LocalCurrencyAmount),
                                                            CurrencyId = g.Key.ForiegnCurrencyId,
                                                            ForeignAmount = (decimal)g.Sum(a => a.ForiegnCurrencyAmount),
                                                            ExchangeRate = (decimal)g.Key.ForiegnExchangeRate,
                                                            Reference1 = theEntityPm.InvoiceNumber,
                                                            Reference2 = theEntityPm.MainEntityReference,
                                                            Reference3 = !string.IsNullOrEmpty(theEntityPm.HouseNumber) ? theEntityPm.HouseNumber : theEntityPm.MasterNumber,
                                                            Notes = theEntityPm.InternalNotes,
                                                        }).ToList();

                    journal.JournalLines.AddRange(journalLines);

                    // [Vats]
                    List<ARInvoiceTotalVAT> ARInvoiceTotalVATs = new List<ARInvoiceTotalVAT>();
                    ARInvoiceTotalVATRepository vatRepository = new ARInvoiceTotalVATRepository(tenant);
                    ARInvoiceTotalVATs = vatRepository.GetInvoiceTotalVatsForInvoiceWithoutZeroVATPercent(theEntityPm.Id, tenant).ToList();
                    counter = journal.JournalLines.Count();

                    // Accounting settings 
                    FullAccountingSettingPM accountingSettings =  getFullAccountingSettings(theEntityPm.Tenant);
                    foreach (ARInvoiceTotalVAT vat in ARInvoiceTotalVATs)
                    {
                        journalLine = new JournalLinePM()
                        {
                            Tenant = tenant,
                            ActionCode = "1",
                            ActionTypeCodeEnum = MyJournalActionTypeEnum.Credit,
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
                            Reference1 = theEntityPm.InvoiceNumber,
                            Reference2 = theEntityPm.MainEntityReference,
                            Reference3 = !string.IsNullOrEmpty(theEntityPm.HouseNumber) ? theEntityPm.HouseNumber : theEntityPm.MasterNumber,
                        };

                        journal.JournalLines.Add(journalLine);
                    }

                    IJournalUpdateServiceExt journalUpdate = ContainerAccessor.Container.Resolve(typeof(IJournalUpdateServiceExt), "JournalUpdateServiceExt", new ParameterOverride("", 1)) as IJournalUpdateServiceExt;
                    journalUpdate.Update(journal);
                }
            }
        }

        private FullAccountingSettingPM getFullAccountingSettings(int tenant)
        {
            FullAccountingSettingPM accountingSettings;
            IFullAccountingSettingQueryServiceExt query = ContainerAccessor.Container.Resolve(typeof(IFullAccountingSettingQueryServiceExt), "FullAccountingSettingQueryServiceExt", new ParameterOverride("", 1)) as IFullAccountingSettingQueryServiceExt;
            accountingSettings = query.GetFullAccountingSettingByTenant( tenant);
            return accountingSettings;
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
                JournalEntity journal = rep.GetJournalByAccountingEntityId(entityPM.Id, tenant);
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

                        UpdateShipmentProfitClass.UpdateARInvoices(entityPM.MainEntityId, entityPM.Tenant);
                    }

                    else if(isNewEntity && this.entityPM.IsAutoCredit)
                    {
                        UpdateShipmentProfitClass.UpdateARInvoices(entityPM.MainEntityId, entityPM.Tenant);
                    }
                }
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
            }
        }
        private void OnApprovingInvoice()
        {
            if (this.isApprovingInvoice)
            {
                // Journal Work
                this.AddARInvoiceJournalAndJournalLines(entityPM, this.isApprovingInvoice);

              
                // DropBox
                this.CreateARInvoiceMessage(this.isApprovingInvoice);

                if (!this.isNewEntity)
                {
                    // Approval on Create is handled inside the Create Method
                    this.sATInterfaceHelper.SendSATRequestFile(entityPM, invoice);
                }

                this.UpdateShipmentRegistryDate();
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
        private void RunRegistryDateProcedure(string myShipmentId)
        {
            RunStoredProcedureClass.UpdateShipmentRegistryDate(myShipmentId, entityPM.Tenant);
        }
    }
}
