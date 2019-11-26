using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.Mocks;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Mocks;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Mocks;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.DataMapping;
using Logitude.BL.InvoiceModel.Tools.TraceEvents;
using Logitude.BL.InvoiceModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Security;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Logitude.BL.InvoiceModel.EntityOtherServices;
using Simplog.Data.CommonDataModel;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.Accounting.Data.Repositories;
using Simplog.Data.ShipmentsModel;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityLists;
using Simplog.Data.InfrastructureModel;

namespace Logitude.BL.InvoiceModel.Tools.EntityService
{
    public class APInvoiceNormalService
    {
        private int tenant;
        private bool isNewEntity;
        private bool isUpdateTotalVats;
        public APInvoice invoice { get; set; }
        private APInvoicePM entityPM;
        private string loggedContactId;
        private IInvoiceContext objectContext;
        private ICommonDataContext myCommonContext;
        private APInvoiceRepository invoiceRepository;
        private APInvoiceLineRepository invoiceLineRepository;
        private APInvoiceTotalVATRepository invoiceTotalVatRepository;
        private APInvoiceEntityRepository invoiceEntityRepository;
        private APInvoicePaymentRepository invoicePaymentRepository;
        private APPaymentRepository paymentRepository;
        private VatTypeRepository vatTypeRepository;
        private AccountingSettingRepository accountingSettingRepository;
        private AccountingSystemRepository accountingSystemRepository;
        private Shipment MainShipment;
        private string MainShipmentConcurrencyGUID;
        private List<string> allShipmentIds;
        private List<string> allPayablesIds;
        private List<Shipment> allShipments;
        private List<ShipmentPayable> allPayables;
        private ShipmentRepository shipmentRepository;
        private ShipmentPayableRepository shipmentPayableRepository;
        private string QBOAPPaymentId;

        public APInvoiceNormalService(IInvoiceContext objectContext, APInvoicePM entityPM)
        {
            this.tenant = entityPM.Tenant;
            this.entityPM = entityPM;
            this.isUpdateTotalVats = false;
            this.objectContext = objectContext;
            this.myCommonContext = CommonDataContext.GetContext(tenant);

            this.invoiceRepository = new APInvoiceRepository(objectContext);
            this.invoiceLineRepository = new APInvoiceLineRepository(objectContext);
            this.invoiceTotalVatRepository = new APInvoiceTotalVATRepository(objectContext);
            this.invoiceEntityRepository = new APInvoiceEntityRepository(objectContext);
            this.invoicePaymentRepository = new APInvoicePaymentRepository(objectContext);
            this.paymentRepository = new APPaymentRepository(objectContext);

            this.vatTypeRepository = new VatTypeRepository(myCommonContext);
            this.accountingSettingRepository = new AccountingSettingRepository(myCommonContext);
            this.accountingSystemRepository = new AccountingSystemRepository(myCommonContext);

            allShipments = new List<Shipment>();
            allPayables = new List<ShipmentPayable>();
            shipmentRepository = new ShipmentRepository(tenant);
            shipmentPayableRepository = new ShipmentPayableRepository(tenant);

            this.GetLoggedContact();
            this.GetAccountingSystem();
        }
        public APInvoiceNormalService(MockInvoiceContext objectContext, APInvoicePM entityPM)
        {
            MockCommonContext commonMockContext = new MockCommonContext();
            MockShipmentContext shipmentMockContext = new MockShipmentContext();

            this.tenant = entityPM.Tenant;
            this.entityPM = entityPM;
            this.isUpdateTotalVats = false;
            this.invoiceRepository = new APInvoiceRepository(objectContext);
            this.invoiceLineRepository = new APInvoiceLineRepository(objectContext);
            this.invoiceTotalVatRepository = new APInvoiceTotalVATRepository(objectContext);
            this.invoiceEntityRepository = new APInvoiceEntityRepository(objectContext);
            this.invoicePaymentRepository = new APInvoicePaymentRepository(objectContext);
            this.paymentRepository = new APPaymentRepository(objectContext);
            this.vatTypeRepository = new VatTypeRepository(commonMockContext);
            this.accountingSettingRepository = new AccountingSettingRepository(commonMockContext);
            this.accountingSystemRepository = new AccountingSystemRepository(commonMockContext);

            allShipments = new List<Shipment>();
            allPayables = new List<ShipmentPayable>();
            shipmentRepository = new ShipmentRepository(shipmentMockContext);
            shipmentPayableRepository = new ShipmentPayableRepository(shipmentMockContext);

            this.GetLoggedContact();
            this.GetAccountingSystem();
        }

        private void GetLoggedContact()
        {
            if (entityPM.CreatedFromAPI)
            {
                this.loggedContactId = entityPM.CreatedByUserId;
            }

            else
            {
                ContactRepository contactRepository = new ContactRepository(tenant);

                string email = HttpContext.Current.User.Identity.Name;

                if (email != null)
                {
                    Contact loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);
                    this.loggedContactId = loggedContact.Id;
                }

                else
                {
                    ContactPM loggedContact = new ContactQuery(tenant).GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), tenant, true);
                    this.loggedContactId = loggedContact.Id;
                }
            }
        }

        private bool isJournal;
        private bool isExternal;
        private bool isTaxItemManaged;
        private bool isTransferToDropbox;
        private bool TransferToDropboxActivated;
        AccountingSetting accountingSetting;
        private void GetAccountingSystem()
        {
            AccountingSettingRepository accountingSettingRepository = new AccountingSettingRepository(tenant);
            this.accountingSetting = accountingSettingRepository.GetSingleAccountSetting(tenant);

            AccountingSystemRepository accountingSystemRepository = new AccountingSystemRepository(tenant);
            AccountingSystem accountingSystem = accountingSystemRepository.GetSingleAccountingSystem(accountingSetting.AccountingSystemCode);

            this.isJournal = accountingSystem.IsJournalMode;
            this.isExternal = accountingSystem.IsExternalCodesFromTable;
            this.isTaxItemManaged = accountingSystem.IsTaxItemManaged;
            this.isTransferToDropbox = accountingSystem.CanTransferToDropbox;
            this.TransferToDropboxActivated = accountingSetting.TransferToDropboxActivated;
        }

        private void ValidateInvoiceCreated()
        {
            if (isNewEntity)
            {
                List<string> allPayablesIds = (from d in entityPM.InvoiceLines group d by d.EntityPayableId into g select g.Key).ToList();

                bool isAlreadyConnected = (from d in invoiceEntityRepository.context.APInvoiceLines
                                           where d.Tenant == tenant
                                           && d.EntityId == entityPM.MainEntityId
                                           && allPayablesIds.Contains(d.EntityPayableId)
                                           select d).Any();

                if (isAlreadyConnected)
                {
                    //throw new ApplicationException("This invoice is already created");
                }
            }
        }

        private List<APInvoiceLinePM> invoiceLinesChangeSet;
        private List<APInvoicePaymentPM> invoicePaymentsChangeSet;
        private bool IsSetVoided = false;
        private bool IsSetApproved = false;
        private bool IsAlreadyVoided = false;
        public void SetChangeSets(List<APInvoiceLinePM> invoiceLinesChangeSet, List<APInvoicePaymentPM> invoicePaymentsChangeSet)
        {
            this.invoiceLinesChangeSet = invoiceLinesChangeSet;
            this.invoicePaymentsChangeSet = invoicePaymentsChangeSet;
        }

        public void Create()
        {
            this.isNewEntity = true;
            this.invoice = new APInvoice();

            this.IsSetApproved = entityPM.SetApproved;

            this.ValidateInvoiceCreated();

            this.InitializeComponent();

            APInvoiceValidator.Validate(entityPM, this.objectContext, this.MainShipmentConcurrencyGUID);
            APInvoiceTracing.Trace(entityPM, invoice, isNewEntity);

            if (!entityPM.IsGeneralInvoice)
            {
                if (entityPM.CreatedFromAPI)
                {
                    this.GeneratePayablesFromInvoiceLines_FromAPI();

                }
                else
                {
                    List<APInvoiceLinePM> lines = new List<APInvoiceLinePM>();
                    if (isNewEntity)
                    {
                        lines = entityPM.InvoiceLines.ToList();
                    }
                    else
                    {
                        lines = invoiceLinesChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
                    }
                    this.BuildUnexpectedPayables(lines);
                    this.GetShipmentsData(entityPM.InvoiceLines);
                }

                this.UpdateInvoiceEntities();
            }

            this.UpdateInvoiceLines();
            this.UpdateTotalVats();
            this.BuildSearchFields();

            var setApproved = entityPM.SetApproved;
            var setVoided = entityPM.SetVoided;

            APInvoiceHelper helper = new APInvoiceHelper();
            helper.APInvoiceQuickbooksValidating(entityPM, setApproved, isNewEntity, this.objectContext, this.myCommonContext);
            APInvoiceMapping.MapEntity(entityPM, invoice, isNewEntity);
            invoiceRepository.Add(invoice);
            invoiceRepository.SubmitChanges();

            if (!entityPM.IsGeneralInvoice)
            {
                this.UpdateAllPayablesAccountedAmountAndStatus();
            }

            this.UpdateInvoiceAmountDue();

            // Journal Work
            this.AddAPInvoiceJournalAndJournalLines(entityPM, setApproved);
            this.VoidAPInvoiceInFullAccounting(entityPM, setVoided);

            // DropBox
            this.CreateAPInvoiceMessage(setApproved);

            this.GetForeignFields();

            if (!entityPM.IsGeneralInvoice)
            {
                //if (!entityPM.CreatedFromAPI)
                //{
                this.RunStoredProcedures();
                // }
            }
        }

        List<APInvoiceLinePM> UnexpectedPayablesInvoiceLines = new List<APInvoiceLinePM>();
        private void GeneratePayablesFromInvoiceLines_FromAPI()
        {
            List<APInvoiceLinePM> invoiceLines = entityPM.InvoiceLines.ToList();
            Shipment shipment = shipmentRepository.GetSingleShipment(entityPM.MainEntityId, tenant);
            List<ShipmentPayable> payables = shipmentPayableRepository.GetShipemntPayablesByShipmentId(shipment.Id, tenant).ToList();

            if (shipment.ShipmentLevelCode == "H")
            {
                payables = payables.Where(a => a.ShipmentPayableLineStatusCode != "ACCT" && a.ShipmentPayableLineStatusCode != "OAMT" && a.ShipmentPayableAmountTypeCode != "ACCU").ToList();
            }
            else
            {
                payables = payables.Where(a => a.ShipmentPayableLineStatusCode != "ACCT").ToList();
            }

            foreach (APInvoiceLinePM aPInvoiceLine in invoiceLines)
            {
                //List<ShipmentPayable> myLines = payables.Where(a => a.ChargesTypeId == aPInvoiceLine.ChargesTypeId && (a.Measurement != null && a.Measurement.Code == aPInvoiceLine.ContainerTypeCode && a.Quantity == aPInvoiceLine.Quantity)).ToList();
                List<ShipmentPayable> myLines = payables.Where(a => a.ChargesTypeId == aPInvoiceLine.ChargesTypeId).ToList();

                if (myLines == null || (myLines != null && myLines.Count() == 0))
                {
                    this.UnexpectedPayablesInvoiceLines.Add(aPInvoiceLine);
                }
                else
                {
                    this.GeneratePayableLine_ChargeTypes(aPInvoiceLine, myLines);
                }
            }

            if (this.UnexpectedPayablesInvoiceLines.Count() > 0)
            {
                this.BuildUnexpectedPayables(this.UnexpectedPayablesInvoiceLines);
            }
            this.GetShipmentsData(invoiceLines);
        }

        private void GeneratePayableLine_ChargeTypes(APInvoiceLinePM aPInvoiceLine, List<ShipmentPayable> shipmentPayable)
        {
            List<ShipmentPayable> shipmentPayables_SameVendor = shipmentPayable.Where(a => a.VendorId == entityPM.VendorId || a.VendorId == null).ToList();
            if (shipmentPayables_SameVendor != null)
            {
                var matchedContainerLines = shipmentPayables_SameVendor.Where(a => a.Measurement != null && a.Measurement.Code == aPInvoiceLine.ContainerTypeCode && a.Quantity == aPInvoiceLine.Quantity && a.CurrencyId == aPInvoiceLine.ForiegnCurrencyId).ToList();
                if ((matchedContainerLines != null && matchedContainerLines.Count() == 1) || shipmentPayables_SameVendor.Count() == 1)
                {
                    ShipmentPayable shipmentPayableLine = this.GetMatchedShipmentPayableLine_ForAPI(matchedContainerLines, shipmentPayables_SameVendor, aPInvoiceLine);

                    if ((aPInvoiceLine.ForiegnCurrencyId != null && aPInvoiceLine.ForiegnCurrencyId == shipmentPayableLine.CurrencyId) || aPInvoiceLine.ForiegnCurrencyId == null)
                    {
                        if (entityPM.InvoiceCurrencyId != shipmentPayableLine.CurrencyId)
                        {
                            this.GetRates(tenant);
                            double lineAmount = aPInvoiceLine.InvoiceCurrencyAmount != null ? aPInvoiceLine.InvoiceCurrencyAmount.Value : 0;
                            aPInvoiceLine.InvoiceCurrencyAmount = Math.Round(CalculateLocalAmount(lineAmount, aPInvoiceLine.ForiegnCurrencyId,shipmentPayableLine.CurrencyId,tenant), 2);
                        }
                        aPInvoiceLine.EntityPayableId = shipmentPayableLine.Id;
                        this.ComputeOpenAmount(aPInvoiceLine, shipmentPayableLine);
                    }
                    else
                    {
                        aPInvoiceLine.AmountTypeCode = "EXPT";
                        this.UnexpectedPayablesInvoiceLines.Add(aPInvoiceLine);
                    }
                }
                else
                {
                    aPInvoiceLine.AmountTypeCode = "EXPT";
                    this.UnexpectedPayablesInvoiceLines.Add(aPInvoiceLine);
                }
            }
            else
            {
                this.UnexpectedPayablesInvoiceLines.Add(aPInvoiceLine);
            }
        }

        private ShipmentPayable GetMatchedShipmentPayableLine_ForAPI(List<ShipmentPayable> matchedContainerLines, List<ShipmentPayable> shipmentPayables_SameVendor, APInvoiceLinePM aPInvoiceLine)
        {
            ShipmentPayable shipmentPayableLine;
            if (matchedContainerLines.Count() == 1)
            {
                shipmentPayableLine = matchedContainerLines.FirstOrDefault();
                aPInvoiceLine.AmountTypeCode = "EXPT";
                if (shipmentPayableLine.UnitPrice == null)
                {
                    shipmentPayableLine.UnitPrice = Round(aPInvoiceLine.InvoiceCurrencyAmount / aPInvoiceLine.Quantity, 2);
                }
            }
            else
            {
                shipmentPayableLine = shipmentPayables_SameVendor.FirstOrDefault();
            }
            return shipmentPayableLine;
        }

        private void ComputeOpenAmount(APInvoiceLinePM invoiceLine, ShipmentPayable shipmentPayableLine)
        {
            if (invoiceLine.AmountTypeCode == "NEXP")
            {
                invoiceLine.OpenAmount = null;
            }
            else
            {
                var expect = shipmentPayableLine.ExpectedAmount == null ? 0 : shipmentPayableLine.ExpectedAmount;
                var amount = invoiceLine.InvoiceCurrencyAmount == null ? 0 : invoiceLine.InvoiceCurrencyAmount;
                var others = invoiceLine.OtherInvoicesAmounts == null ? 0 : invoiceLine.OtherInvoicesAmounts;
                var corre = invoiceLine.CorrectionAmount == null ? 0 : invoiceLine.CorrectionAmount;
                double? open = expect - others - amount - corre;
                invoiceLine.OpenAmount = Math.Round(open.Value, 2);
            }
        }

        private double CalculateLocalAmount(double amount, string convertedCurrencyId, string currencyId, int tenant)
        {
            var tenantCurrency = GetTenantCurrency(tenant);
            double amountInTariffCurr, amountInConvertedCurr;

            if (convertedCurrencyId == currencyId)
            {
                amountInTariffCurr = amount;
            }

            else
            {
                if (tenantCurrency == currencyId)
                    amountInTariffCurr = amount;
                else
                {
                    RatesTableList rateList = RatesList.Find(d => d.BaseCurrencyId == tenantCurrency && d.ForeignCurrencyId == currencyId);
                    var rate = rateList == null ? 0 : rateList.Rate;
                    amountInTariffCurr = amount * (double)rate;

                }

                if (tenantCurrency == convertedCurrencyId)
                    amountInConvertedCurr = amountInTariffCurr;

                else
                {
                    RatesTableList rateList = RatesList.Find(d => d.BaseCurrencyId == tenantCurrency && d.ForeignCurrencyId == convertedCurrencyId);
                    var rate = rateList == null ? 0 : rateList.Rate;
                    amountInTariffCurr = amountInTariffCurr / (double)rate;
                }
            }

            return amountInTariffCurr;
        }

        private string GetTenantCurrency(int tenant)
        {
            TenantRepository tRepo = new TenantRepository(tenant);
            Tenant t = tRepo.GetSingleByTenant(tenant);
            var tenantCurrency = (t == null ? null : t.CurrencyId);
            return tenantCurrency;
        }

        List<RatesTableList> RatesList;
        private void GetRates(int tenant)
        {
            IWebFreightContext MyContext = WebFreightContext.GetContext(tenant);
            RatesTableRepository ratesTableRepository = new RatesTableRepository(MyContext);
            IQueryable<RatesTable> entityPocos = ratesTableRepository.GetRatesTables(tenant);

            RatesTableQuery ratesTableQuery = new RatesTableQuery(ratesTableRepository);
            IQueryable<RatesTableList> entityLists = ratesTableQuery.GetIQueryableEntityList(entityPocos);
            entityLists = entityLists.OrderByDescending(r => r.ValueDate);

            this.RatesList = entityLists.ToList();
        }

        private void VoidAPInvoiceInFullAccounting(APInvoicePM entityPM, bool setVoided)
        {
            TenantRepository tenantRepository = new TenantRepository(tenant);
            Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);

            if (tenantPOCO.AccountingActivated && setVoided)
            {
                IJournalQueryServiceExt journalQuery = ContainerAccessor.Container.Resolve(typeof(IJournalQueryServiceExt), "JournalQueryServiceExt", new ParameterOverride("", 1)) as IJournalQueryServiceExt;
                JournalPM journalPM = journalQuery.GetJournalByAccountingEntityIdAndCode(entityPM.Id, "4", entityPM.Tenant);
                if (journalPM != null)
                {
                    var journalUpdate = ContainerAccessor.Container.Resolve(typeof(IJournalVoidUpdateServiceExt), "JournalVoidUpdateServiceExt", new ParameterOverride("", 1)) as IJournalVoidUpdateServiceExt;
                    journalUpdate.Update(journalPM, new StornoOverrideM()
                    {
                        AccountingEntityCode = "4",
                        AccountingEntityId = entityPM.Id,
                        AccountingEntityReference = entityPM.InvoiceNumber
                    });
                }
            }
        }

        public void Update(bool mapComposition = false)
        {
            if (mapComposition)
            {
                this.invoiceLinesChangeSet = entityPM.InvoiceLines;
                this.invoicePaymentsChangeSet = entityPM.InvoicePayments;
            }

            this.isNewEntity = false;

            this.invoice = invoiceRepository.GetSingleAPInvoice(entityPM.Id, tenant);

            this.IsSetVoided = entityPM.SetVoided;
            this.IsSetApproved = entityPM.SetApproved;
            this.IsAlreadyVoided = this.invoice.StatusCode == "VD" ? true : false;

            this.ValidateHigherStatus();

            this.InitializeComponent();

            APInvoiceValidator.Validate(entityPM, this.objectContext, this.MainShipmentConcurrencyGUID);
            APInvoiceTracing.Trace(entityPM, invoice, isNewEntity);

            if (!entityPM.IsGeneralInvoice)
            {
                List<APInvoiceLinePM> lines = invoiceLinesChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
                this.BuildUnexpectedPayables(lines);
                this.GetShipmentsData(invoiceLinesChangeSet);
                this.UpdateInvoiceEntities();
            }

            this.UpdateInvoiceLines();
            this.UpdateTotalVats();

            // Journal Work
            this.AddAPInvoiceJournalAndJournalLines(entityPM, entityPM.SetApproved);
            this.VoidAPInvoiceInFullAccounting(entityPM, entityPM.SetVoided);

            APInvoiceHelper helper = new APInvoiceHelper();
            if (entityPM.SetReSendQBO)
            {
                helper.APInvoiceQuickbooksValidating(entityPM, true, isNewEntity, this.objectContext, this.myCommonContext);

            }
            else
            {
                helper.APInvoiceQuickbooksValidating(entityPM, IsSetApproved, isNewEntity, this.objectContext, this.myCommonContext);
            }



            APInvoiceMapping.MapEntity(entityPM, invoice, isNewEntity);

            // DropBox
            this.CreateAPInvoiceMessage(IsSetApproved);

            invoiceRepository.Update(invoice);
            invoiceRepository.SubmitChanges();

            if (!entityPM.IsGeneralInvoice)
            {
                this.UpdateAllPayablesAccountedAmountAndStatus();
            }

            this.UpdateInvoicePayments(invoicePaymentsChangeSet);
            this.UpdateInvoiceAmountDue();
            this.BuildSearchFields();

            invoiceRepository.Update(invoice);
            invoiceRepository.SubmitChanges();

            if (!String.IsNullOrEmpty(QBOAPPaymentId))
            {

                APPaymentHelper service = new APPaymentHelper();
                APPaymentQuery PaymentQuery = new APPaymentQuery(paymentRepository);
                APPaymentPM paymentPM = PaymentQuery.GetSinglePM(QBOAPPaymentId, tenant);
                if (paymentPM.TransferStatusCode == "TR")
                {
                    APPaymentRepository repository = new APPaymentRepository(tenant);
                    APPayment payment = repository.GetSingleAPPayment(paymentPM.Id, tenant);

                    service.APPaymentQuickbooksValidating(paymentPM, true, false, payment, this.objectContext, this.myCommonContext, false, false);
                }
            }

            this.GetForeignFields();

            if (!entityPM.IsGeneralInvoice)
            {
                this.RunStoredProcedures();
            }
        }

        private void ValidateHigherStatus()
        {
            if (!isNewEntity)
            {
                if (this.invoice.StatusCode == "AD")
                {
                    bool throwException = false;

                    if (string.IsNullOrEmpty(this.entityPM.StatusCode) || this.entityPM.StatusCode == "WA")
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
            }
        }

        private void CreateAPInvoiceMessage(bool setApproved)
        {
            if (setApproved && this.isTransferToDropbox && this.TransferToDropboxActivated)
            {
                if (!string.IsNullOrEmpty(entityPM.TransferError))
                {
                    throw new ApplicationException(entityPM.TransferError);
                }
                else
                {
                    this.invoice = invoiceRepository.GetSingleAPInvoice(this.entityPM.Id, this.entityPM.Tenant);
                    List<APInvoice> entities = new List<APInvoice>();
                    entities.Add(this.invoice);
                    APInvoiceMessageHelper myHelper = new APInvoiceMessageHelper(entities, this.invoice.InvoiceNumber + ".xml", tenant, true);
                    myHelper.Transfer();
                }
            }
        }

        #region InitializeComponent

        DateTime? todayDateTime = null;
        private void InitializeComponent()
        {
            todayDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

            if (this.isNewEntity)
            {
                entityPM.Id = IdCounter.GetNumber("APInvoice", entityPM.Tenant).ToString();
                entityPM.CreateDate = todayDateTime;
            }

            entityPM.UpdateDate = todayDateTime;

            if (entityPM.DueDate != null)
            {
                entityPM.DueDate = entityPM.DueDate.Value.Date;
            }

            if (entityPM.InvoiceDate != null)
            {
                entityPM.InvoiceDate = entityPM.InvoiceDate.Value.Date;
            }

            if (string.IsNullOrEmpty(entityPM.InternalNumber))
            {
                entityPM.InternalNumber = TableCounter.GetNumber(entityPM.Tenant, "APIC", "IN", null);
            }

            if (!string.IsNullOrEmpty(this.entityPM.MainEntityId))
            {
                this.MainShipment = shipmentRepository.GetSingleShipment(this.entityPM.MainEntityId, tenant);

                if (this.MainShipment != null)
                {
                    this.MainShipmentConcurrencyGUID = this.MainShipment.ConcurrencyGUID;
                }
            }

            if (entityPM.SetApproved)
            {
                entityPM.StatusCode = "AD";
                entityPM.ApprovedDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                entityPM.ApprovedByUserId = loggedContactId;

                if (string.IsNullOrEmpty(this.entityPM.MasterNumber) || string.IsNullOrEmpty(this.entityPM.HouseNumber))
                {
                    if (this.MainShipment != null)
                    {
                        if (string.IsNullOrEmpty(this.entityPM.MasterNumber))
                        {
                            if (!string.IsNullOrEmpty(this.MainShipment.MasterShipmentDataId))
                            {
                                ShipmentMasterData myMaster = shipmentRepository.GetSingleShipmentMasterData(this.MainShipment.MasterShipmentDataId, tenant);
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
                            this.entityPM.HouseNumber = this.MainShipment.House;
                        }
                    }

                }
            }

            else if (entityPM.SetVoided)
            {
                if (entityPM.StatusCode != "VD")
                {
                    entityPM.StatusCode = "VD";
                }
            }

            else if (entityPM.SetCancelApproval)
            {
                if (entityPM.StatusCode != "WA")
                {
                    entityPM.StatusCode = "WA";
                }
            }

            else if (string.IsNullOrEmpty(entityPM.StatusCode))
            {
                entityPM.StatusCode = "WA";
            }

            this.InitializeVATs();
            this.InitializeTransferComponents();
            this.InitializeAmountDueFields();
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

        private void InitializeAmountDueFields()
        {
            entityPM.AmountDue = entityPM.AmountInInvoiceCurrency == null ? 0 : entityPM.AmountInInvoiceCurrency.Value;
            entityPM.AmountDueInLocalCurrency = entityPM.AmountInLocalCurrency == null ? 0 : entityPM.AmountInLocalCurrency.Value;
            entityPM.AmountDueInProfitCurrency = entityPM.AmountInProfitCurrency == null ? 0 : entityPM.AmountInProfitCurrency.Value;
        }
        private void InitializeGLAccountFields()
        {
            if (entityPM.SetApproved)
            {
                TenantRepository tenantRepository = new TenantRepository(tenant);
                Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);

                if (tenantPOCO.AccountingActivated)
                {
                    List<APInvoiceLinePM> lines = new List<APInvoiceLinePM>();

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

                    foreach (APInvoiceLinePM line in lines)
                    {
                        ChargesType myChargesType = ChargesTypeRepository.GetSingleChargesType(line.ChargesTypeId, tenant, true);

                        if (line.ChargeTypeGLAccountId == null)
                        {

                            if (myChargesType.AccountingVATSplit)
                            {
                                ChargeTypeAccounting myChargeTypeAccounting = (from d in iQueryable_ChargeTypeAccounting where d.ChargeTypeId == line.ChargesTypeId && d.VatTypeId == line.VatTypeId select d).FirstOrDefault();
                                if (myChargeTypeAccounting != null)
                                {
                                    line.ChargeTypeGLAccountId = myChargeTypeAccounting.PayableDebitGLAcountId;
                                }
                            }
                            else
                            {

                                if (!string.IsNullOrWhiteSpace(line.DebitAccount))
                                {
                                    GLAccountPM glaAccount = GetGLAccountForLine(line);
                                    line.ChargeTypeGLAccountId = glaAccount?.Id;
                                }
                                else
                                {
                                    line.ChargeTypeGLAccountId = myChargesType.PayableDebitGLAcountId;
                                }

                            }

                            if (string.IsNullOrEmpty(line.ChargeTypeGLAccountId))
                            {
                                throw new Exception("The Payabel GLAccount of the Charge Type " + myChargesType.EnglishName + " is NULL");
                            }
                        }
                    }
                }
            }
        }

        private GLAccountPM GetGLAccountForLine(APInvoiceLinePM line)
        {
            IGLAccountQueryServiceExt glAccountQuery = ContainerAccessor.Container.Resolve(typeof(IGLAccountQueryServiceExt), "GLAccountQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountQueryServiceExt;
            GLAccountPM glaAccount = glAccountQuery.GetGLAccountByDisplayNumber(line.DebitAccount, tenant);
            return glaAccount;
        }
        #endregion

        #region Transfer
        private bool isInitializingExternalFields;
        private void InitializeTransferComponents()
        {
            this.InitializeExternalFields();
            this.InitializeTransferFields();
            this.InitializeGLAccountFields();
            // DropBox
            if (this.entityPM.SetApproved && this.isTransferToDropbox && this.TransferToDropboxActivated && string.IsNullOrEmpty(entityPM.TransferError))
            {
                this.entityPM.TransferStatusCode = "TR";
            }
        }
        private void InitializeExternalFields()
        {
            bool isInitializing = false;

            if (entityPM.StatusCode != null && entityPM.StatusCode != "WA")
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
                if (FieldIsEmpty(entityPM.CreditAccount))
                {
                    if (isJournal)
                    {
                        Card myCard = CardRepository.GetSingleCard(entityPM.VendorId, tenant, true);
                        entityPM.CreditAccount = myCard.PayablesAccountingCard;
                    }

                    else if (isExternal)
                    {
                        CardExternalCodeByCurrencyRepository cardExternalCodeByCurrencyRepository = new CardExternalCodeByCurrencyRepository(tenant);
                        IQueryable<CardExternalCodeByCurrency> iQueryable_CardExternals = cardExternalCodeByCurrencyRepository.GetCardExternalCodeByCurrenciesByTenant(tenant);

                        CardExternalCodeByCurrency myCardExternal = (from d in iQueryable_CardExternals where d.CardId == entityPM.VendorId && d.CurrencyId == entityPM.InvoiceCurrencyId select d).FirstOrDefault();
                        if (myCardExternal != null)
                        {
                            entityPM.CreditAccount = myCardExternal.ExternalRecievableTableId;
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

                List<APInvoiceLinePM> lines = new List<APInvoiceLinePM>();

                if (isNewEntity)
                {
                    lines = entityPM.InvoiceLines.ToList();
                }

                else
                {
                    lines = invoiceLinesChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
                }

                foreach (APInvoiceLinePM line in lines)
                {
                    if (FieldIsEmpty(line.DebitAccount))
                    {
                        ChargesType myChargesType = ChargesTypeRepository.GetSingleChargesType(line.ChargesTypeId, tenant, true);

                        if (isJournal)
                        {
                            if (myChargesType.AccountingVATSplit)
                            {
                                ChargeTypeAccounting myChargeTypeAccounting = (from d in iQueryable_ChargeTypeAccounting where d.ChargeTypeId == line.ChargesTypeId && d.VatTypeId == line.VatTypeId select d).FirstOrDefault();
                                if (myChargeTypeAccounting != null)
                                {
                                    line.DebitAccount = myChargeTypeAccounting.PayableDebitAccount;
                                }
                            }

                            else
                            {
                                line.DebitAccount = myChargesType.PayableDebitAccount;
                            }
                        }

                        else
                        {
                            line.DebitAccount = myChargesType.ReceivablesChargesTypeExternalCode;
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
                List<APInvoiceTotalVAT> myTotalVATs = invoiceTotalVatRepository.GetInvoiceTotalVatsByInvoiceId(entityPM.Id, tenant).ToList();
                foreach (APInvoiceTotalVAT itemVAT in myTotalVATs)
                {
                    if (FieldIsEmpty(itemVAT.ExternalVATCard) || FieldIsEmpty(itemVAT.ExternalTAXItemId))
                    {
                        VatType myVatType = VatTypeRepository.GetSingleVatType(itemVAT.VatTypeId, tenant, true);

                        if (FieldIsEmpty(itemVAT.ExternalVATCard))
                        {
                            if (this.accountingSetting.AccountingSystemCode == "HV" || this.accountingSetting.AccountingSystemCode == "RH")
                            {
                                itemVAT.ExternalVATCard = this.accountingSetting.PayableVATCard;
                            }

                            else if (myVatType != null)
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

            if (entityPM.IsTransferStatusSetManually)
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

            else if (invoice.TransferStatusCode == "IP")
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

            else if (entityPM.TransferStatusCode == "IP" || entityPM.TransferStatusCode == "ET")
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
                string linesError = "Debit Account is missing";

                if (FieldIsEmpty(entityPM.CreditAccount))
                {
                    isReady = false;
                    myError = "Credit Account is missing";
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

                List<APInvoiceLinePM> myLines = new List<APInvoiceLinePM>();
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
                    if (myLines.Where(d => d.DebitAccount == null || (d.DebitAccount != null && string.IsNullOrEmpty(d.DebitAccount.Trim()))).Any())
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
        private void GetShipmentsData(List<APInvoiceLinePM> lines)
        {
            this.allShipmentIds = (from d in lines group d by d.EntityId into g select g.Key).ToList();
            this.allPayablesIds = (from d in lines group d by d.EntityPayableId into g select g.Key).ToList();
            this.allShipments = shipmentRepository.GetShipmentsListFromIdList(allShipmentIds, tenant);
            this.allPayables = shipmentPayableRepository.GetShipmentPayablesFromIdList(allPayablesIds, tenant);

            // Ayman
            // Open shipment Payables screen: issue new invoice or update draft invoice with new lines
            // keeps the invoice screen opened without saving
            // Open same shipment in new session and delete those payables and save the shipment
            // return to the invoice screen session and save this invoice
            if (!this.entityPM.IsMultipleEntities)
            {
                List<APInvoiceLinePM> newLines = new List<APInvoiceLinePM>();

                if (isNewEntity)
                {
                    newLines = lines.Where(d => d.EntityPayableId != null).ToList();
                }

                else
                {
                    newLines = lines.Where(d => d.ChangeSetOp == ChangeSetOperation.Insert && d.EntityPayableId != null).ToList();
                }

                if (newLines.Count > 0)
                {
                    foreach (APInvoiceLinePM item in newLines)
                    {
                        ShipmentPayable myPayable = this.allPayables.Where(d => d.Id == item.EntityPayableId).FirstOrDefault();
                        if (myPayable == null)
                        {
                            throw new ApplicationException("Some of invoice lines are missing payables");
                        }
                    }
                }
            }

            Shipment shipment = allShipments.Where(d => d.Id == entityPM.MainEntityId).FirstOrDefault();

            if (shipment != null)
            {
                shipment.ConcurrencyGUID = Guid.NewGuid().ToString();

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

        #region Payables

        private void UpdatePayable(APInvoiceLinePM item, ShipmentPayable shipmentPayable = null)
        {
            //if (!entityPM.CreatedFromAPI)
            //{
            ShipmentPayable payable;
            if (shipmentPayable != null)
            {
                payable = shipmentPayable;
            }
            else
            {
                payable = (from a in allPayables where a.Id == item.EntityPayableId select a).FirstOrDefault();
            }
            if (payable != null)
            {
                string shipmentProfitCurrencyId = shipmentRepository.GetShipmentProfitCurrencyId(payable.ShipmentId);

                bool isConnectedToOthers = (from d in invoiceLineRepository.context.APInvoiceLines
                                            where d.Tenant == tenant
                                            && d.EntityPayableId == payable.Id
                                            && (d.APInvoiceId != item.APInvoiceId)
                                            select d).Any();

                if (!isConnectedToOthers)
                {
                    payable.Rate = item.ForiegnExchangeRate;
                    payable.ProfitCurrencyExchangeRate = entityPM.ProfitCurrencyExchangeRate;

                    payable.ExpectedAmountLocal = MethodHelper.Round(payable.ExpectedAmount * payable.Rate, 2);

                    if (payable.CurrencyId == shipmentProfitCurrencyId)
                    {
                        payable.ExpectedAmountInProfitCurrency = payable.ExpectedAmount;
                    }

                    else
                    {
                        payable.ExpectedAmountInProfitCurrency = MethodHelper.Round(payable.ExpectedAmountLocal / payable.ProfitCurrencyExchangeRate, 2);
                    }
                }

                if (item.CorrectionAmount == null || item.CorrectionAmount == 0)
                {
                    payable.CorrectionAmount = null;
                    payable.CorrectionByUserId = null;
                    payable.CorrectionDate = null;
                    payable.CorrectionNote = null;
                }

                else
                {
                    payable.CorrectionAmount = item.CorrectionAmount;
                    payable.CorrectionByUserId = entityPM.UpdatedByUserId;
                    payable.CorrectionDate = TenantServerConfigration.GetCurrentDateTime(payable.Tenant);
                    payable.CorrectionNote = item.CorrectionNote;
                }

                payable.VendorId = entityPM.VendorId;
                payable.OpenAmount = item.OpenAmount;
                payable.OpenAmountInLocalCurrency = Round(payable.OpenAmount * payable.Rate, 2);

                if (payable.CurrencyId == shipmentProfitCurrencyId)
                {
                    payable.OpenAmountInProfitCurrency = payable.OpenAmount;
                }

                else
                {
                    payable.OpenAmountInProfitCurrency = Round(payable.OpenAmountInLocalCurrency / payable.ProfitCurrencyExchangeRate, 2);
                }

                shipmentPayableRepository.Update(payable);
            }
            //}
        }

        private void DisconnectPayable(string payableId, double? myForiegnCurrencyAmount)
        {
            if (!entityPM.CreatedFromAPI)
            {
                ShipmentPayable myPayable = (from a in allPayables where a.Id == payableId select a).FirstOrDefault();
                if (myPayable != null)
                {
                    if (myPayable.ShipmentPayableAmountTypeCode == "NEXP")
                    {
                        List<ShipmentPayable> ChildPayables = shipmentPayableRepository.GetChildPayablesByParentPayable(myPayable.Id, tenant);
                        foreach (ShipmentPayable myChild in ChildPayables)
                        {
                            shipmentPayableRepository.Remove(myChild);
                        }

                        shipmentPayableRepository.Remove(myPayable);
                        allPayables.Remove(myPayable);
                    }

                    else
                    {
                        myPayable.CorrectionAmount = null;
                        myPayable.CorrectionByUserId = null;
                        myPayable.CorrectionDate = null;
                        myPayable.CorrectionNote = null;

                        myForiegnCurrencyAmount = myForiegnCurrencyAmount == null ? 0 : myForiegnCurrencyAmount.Value;
                        double? myAccountedAmount = myPayable.AccountedAmount == null ? 0 : myPayable.AccountedAmount.Value;
                        double? myExpectedAmount = myPayable.ExpectedAmount == null ? 0 : myPayable.ExpectedAmount.Value;
                        double? myCorrectionAmount = myPayable.CorrectionAmount == null ? 0 : myPayable.CorrectionAmount.Value;

                        double? myOtherInvoicesAmounts = myAccountedAmount - myForiegnCurrencyAmount;
                        double? myOpenAmount = myExpectedAmount - myOtherInvoicesAmounts - myCorrectionAmount;

                        myPayable.OpenAmount = Round(myOpenAmount, 2);
                        myPayable.OpenAmountInLocalCurrency = Round(myPayable.OpenAmount * myPayable.Rate, 2);
                        myPayable.OpenAmountInProfitCurrency = Round(myPayable.OpenAmountInLocalCurrency / myPayable.ProfitCurrencyExchangeRate, 2);
                        shipmentPayableRepository.Update(myPayable);
                    }

                    shipmentPayableRepository.SubmitChanges();
                }
            }
        }

        private void BuildUnexpectedPayables(List<APInvoiceLinePM> lines)
        {
            if (!this.IsAlreadyVoided)
            {
                if (lines.Where(d => d.EntityPayableId == null).Any())
                {
                    foreach (APInvoiceLinePM invoicelinePM in entityPM.InvoiceLines.Where(d => d.EntityPayableId == null))
                    {
                        ShipmentPayable payable = new ShipmentPayable()
                        {
                            Id = IdCounter.GetNumber("ShipmentPayable", entityPM.Tenant),
                            VendorId = entityPM.VendorId,
                            ChargesTypeId = invoicelinePM.ChargesTypeId,
                            CurrencyId = entityPM.InvoiceCurrencyId,
                            ShipmentPayableLineStatusCode = "ACCT",
                            ShipmentPayableAmountTypeCode = "NEXP",
                            Rate = entityPM.InvoiceCurrencyExchangeRate,
                            ProfitCurrencyExchangeRate = entityPM.ProfitCurrencyExchangeRate,
                            ShipmentId = invoicelinePM.EntityId,
                            Tenant = entityPM.Tenant,
                            UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant),
                            ValueDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant),
                            CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant),
                            CreatedByUserId = entityPM.UpdatedByUserId,
                            UpdateByUserId = entityPM.UpdatedByUserId,
                            AWBPrint = false,
                            IsEditedByUser = false,
                            IsFromQuote = false,
                            Quantity = invoicelinePM.Quantity,
                           
                        };

                        if(invoicelinePM.ContainerTypeId != null)
                        {
                            PackageTypeRepository packageRepository = new PackageTypeRepository(tenant);
                            var package = packageRepository.GetSinglePackageType(invoicelinePM.ContainerTypeId, tenant);
                            if(package != null)
                            {
                                payable.MeasurementId = package.MeasurementId;
                            }
                            if (this.entityPM.CreatedFromAPI && payable.UnitPrice == null)
                            {
                                payable.UnitPrice = Round(invoicelinePM.InvoiceCurrencyAmount / invoicelinePM.Quantity, 2);
                            }
                        }

                        invoicelinePM.EntityPayableId = payable.Id;
                        shipmentPayableRepository.Add(payable);
                    }

                    shipmentPayableRepository.SubmitChanges();
                }
            }
        }

      

        private void UpdateAllPayablesAccountedAmountAndStatus()
        {
            if (!this.IsAlreadyVoided)
            {
                if (allPayables.Count > 0)
                {
                    List<string> allPayablesIds = allPayables.Select(s => s.Id).ToList();
                    List<APInvoiceLine> allPayablesInvoicesLines = invoiceLineRepository.GetPayablesInvoicesLines(allPayablesIds, tenant);

                    foreach (ShipmentPayable myPayable in allPayables)
                    {
                        List<APInvoiceLine> myInvoiceslines = allPayablesInvoicesLines.Where(d => d.EntityPayableId == myPayable.Id).ToList();

                        double? myAccountedAmount = myInvoiceslines.Sum(s => s.ForiegnCurrencyAmount);
                        double? myAccountedAmount_Local = myInvoiceslines.Sum(s => s.LocalCurrencyAmount);
                        double? myAccountedAmount_Profit = myInvoiceslines.Sum(s => s.ProfitCurrencyAmount);

                        myPayable.AccountedAmount = Round(myAccountedAmount, 2);
                        myPayable.AccountedAmountInLocalCurrency = Round(myAccountedAmount_Local, 2);
                        myPayable.AccountedAmountInProfitCurrency = Round(myAccountedAmount_Profit, 2);

                        this.SetStatusCode(myPayable);
                        shipmentPayableRepository.Update(myPayable);

                        List<ShipmentPayable> ChildPayables = shipmentPayableRepository.GetChildPayablesByParentPayable(myPayable.Id, tenant);
                        foreach (ShipmentPayable myChild in ChildPayables)
                        {
                            myChild.ShipmentPayableLineStatusCode = myPayable.ShipmentPayableLineStatusCode;
                            shipmentPayableRepository.Remove(myChild);
                        }
                    }

                    shipmentPayableRepository.SubmitChanges();
                }
            }
        }

        private void SetStatusCode(ShipmentPayable entity)
        {
            if (!this.IsAlreadyVoided)
            {
                if (entity.ShipmentPayableAmountTypeCode == "NEXP")
                {
                    entity.ShipmentPayableLineStatusCode = "ACCT";
                }

                else if (entity.Quantity == null || entity.UnitPrice == null)
                {
                    entity.ShipmentPayableLineStatusCode = "EMPT";
                }

                else
                {
                    if (entity.OpenAmount == null)
                    {
                        entity.OpenAmount = 0;
                    }

                    if (entity.AccountedAmount == null)
                    {
                        entity.AccountedAmount = 0;
                    }

                    if (entity.OpenAmount != 0 && entity.AccountedAmount != 0)
                    {
                        entity.ShipmentPayableLineStatusCode = "PACC";
                    }

                    else if (entity.OpenAmount != 0)
                    {
                        entity.ShipmentPayableLineStatusCode = "OAMT";
                    }

                    else if (entity.AccountedAmount != 0)
                    {
                        entity.ShipmentPayableLineStatusCode = "ACCT";
                    }
                }
            }
        }
        #endregion

        #region InvoiceEntities
        private void UpdateInvoiceEntities()
        {
            List<APInvoiceEntity> dbEntities = invoiceEntityRepository.GetInvoiceEntitiesForInvoice(entityPM.Id, entityPM.Tenant).ToList();

            if (invoice.StatusCode != "VD" && entityPM.StatusCode == "VD")
            {
                foreach (APInvoiceEntity invoiceEntity in dbEntities)
                {
                    invoiceEntityRepository.Remove(invoiceEntity);
                }
            }

            else if (invoice.StatusCode != "AD")
            {
                if (entityPM.InvoiceLines.Where(d => d.ObjectTableId == null).Any())
                {
                    List<ObjectTable> tables = new ObjectTableRepository(entityPM.Tenant).context.ObjectTables.Where(d => (d.Tenant == 0 || d.Tenant == entityPM.Tenant) && (d.Name == "Shipment" || d.Name == "Master")).ToList();

                    foreach (APInvoiceLinePM item in entityPM.InvoiceLines.Where(d => d.ObjectTableId == null))
                    {
                        string shipmentLevelCode = shipmentRepository.GetShipmentLevelCode(item.EntityId);

                        ObjectTable table = (shipmentLevelCode == "C") ? tables.Where(d => d.Name == "Master").FirstOrDefault() : tables.Where(d => d.Name == "Shipment").FirstOrDefault();
                        if (table != null)
                        {
                            item.ObjectTableId = table.Id;
                        }
                    }
                }

                if (isNewEntity)
                {
                    this.CreateInvoiceEntities();
                }
            }
        }

        private void CreateInvoiceEntities()
        {
            var entityIdAndObjectTables
                = (from a in entityPM.InvoiceLines
                   group a by new { a.EntityId, a.ObjectTableId, a.EntityReference } into gr
                   select new
                   {
                       EntityId = gr.Key.EntityId,
                       ObjectTableId = gr.Key.ObjectTableId,
                       EntityReference = gr.Key.EntityReference
                   }).ToList();

            foreach (var item in entityIdAndObjectTables)
            {
                APInvoiceEntity invoiceEntity = new APInvoiceEntity()
                {
                    APInvoiceId = entityPM.Id,
                    ObjectTableId = item.ObjectTableId,
                    EntityId = item.EntityId,
                    Tenant = entityPM.Tenant,
                    EntityReference = item.EntityReference,
                    Id = IdCounter.GetNumber("APInvoiceEntity", entityPM.Tenant).ToString(),
                };

                invoiceEntityRepository.Add(invoiceEntity);
            }
        }
        #endregion

        #region TotalVats
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

                List<APInvoiceTotalVAT> dbTotalVats = invoiceTotalVatRepository.GetInvoiceTotalVatsByInvoiceId(entityPM.Id, entityPM.Tenant).ToList();
                foreach (APInvoiceTotalVAT item in dbTotalVats)
                {
                    invoiceTotalVatRepository.Remove(item);
                }

                List<APInvoiceLinePM> myDataLines = entityPM.InvoiceLines.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete && d.VatTypeId != null).ToList();
                if (myDataLines.Count > 0)
                {
                    subTotal = MethodHelper.Round(myDataLines.Sum(s => s.InvoiceCurrencyAmount), 2);
                    subTotal_Local = MethodHelper.Round(myDataLines.Sum(s => s.LocalCurrencyAmount), 2);

                    #region
                    List<VATTypesGroup> allVatGroups = (from d in myCommonContext.VATTypesGroups
                                                        where d.Tenant == this.tenant
                                                        select d).ToList();

                    List<InvoiceTotalsClass> group_Source = new List<InvoiceTotalsClass>();

                    foreach (APInvoiceLinePM item in myDataLines)
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
                                        newItem.ExternalVatCard = this.accountingSetting.PayableVATCard;
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
                                    };

                                    VatType vatType = this.allVatTypes.Where(d => d.Id == itemGroup.SingleVATTypeId).FirstOrDefault();
                                    if (vatType != null)
                                    {
                                        newItem.ExternalTAXItemId = vatType.ExternalTAXItemId;
                                    }

                                    if (this.accountingSetting != null)
                                    {
                                        newItem.ExternalVatCard = this.accountingSetting.PayableVATCard;
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
                                            newItem.ExternalVatCard = this.accountingSetting.PayableVATCard;
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
                           group items by new { items.VatTypeId, items.VatTypePercentage, items.ExternalVatCard, items.ExternalTAXItemId, items.VatRecognizedPercentage } into g
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
                               VatRecognizedPercentage = g.Key.VatRecognizedPercentage
                           }).ToList();

                    foreach (InvoiceTotalsClass item in group_data)
                    {
                        APInvoiceTotalVAT record = new APInvoiceTotalVAT()
                        {
                            Id = IdCounter.GetNumber("APInvoiceTotalVAT", entityPM.Tenant).ToString(),
                            Tenant = entityPM.Tenant,
                            APInvoiceId = entityPM.Id,
                            VatTypeId = item.Id,
                            VatPercent = MethodHelper.Roundd(item.VatTypePercentage, 2),
                            LocalVatableAmount = MethodHelper.Roundd(item.LocalCurrencyAmount, 2),
                            InvoiceCurrencyVatableAmount = MethodHelper.Roundd(item.InvoiceCurrencyAmount, 2),
                            ProfitVatableAmount = MethodHelper.Round(item.ProfitCurrencyAmount, 2),
                            ExternalVATCard = item.ExternalVatCard,
                            ExternalTAXItemId = item.ExternalTAXItemId,
                            // VatRecognizedPercentage = item.VatRecognizedPercentage,
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

        #region InvoiceLines
        private void UpdateInvoiceLines()
        {
            if (isNewEntity)
            {
                int lineNumber = 0;
                foreach (APInvoiceLinePM item in entityPM.InvoiceLines)
                {
                    lineNumber += 1;
                    item.LineNumber = lineNumber;
                    this.CreateInvoiceLine(item);
                    this.UpdatePayable(item);
                    this.isUpdateTotalVats = true;
                }
            }

            else
            {
                if (invoiceLinesChangeSet.Count > 0)
                {
                    int lastLineNumber = (from a in invoiceLinesChangeSet select a.LineNumber).Max();
                    foreach (APInvoiceLinePM item in invoiceLinesChangeSet)
                    {
                        if (invoice.StatusCode != "VD" && entityPM.StatusCode == "VD")
                        {
                            this.DisconnectPayable(item.EntityPayableId, item.ForiegnCurrencyAmount);
                            this.DisconnectInvoiceLine(item);
                        }

                        else
                        {
                            switch (item.ChangeSetOp)
                            {
                                case ChangeSetOperation.Insert:
                                    {
                                        lastLineNumber += 1;
                                        item.LineNumber = lastLineNumber;
                                        this.CreateInvoiceLine(item);
                                        this.UpdatePayable(item);
                                        this.isUpdateTotalVats = true;
                                        break;
                                    }

                                case ChangeSetOperation.Update:
                                    {
                                        this.UpdateInvoiceLine(item);
                                        this.UpdatePayable(item);
                                        isUpdateTotalVats = true;
                                        break;
                                    }

                                case ChangeSetOperation.Delete:
                                    {
                                        this.DeleteInvoiceLine(item);
                                        this.isUpdateTotalVats = true;
                                        break;
                                    }

                                default: { break; }
                            }
                        }
                    }
                }
            }
        }

        private void CreateInvoiceLine(APInvoiceLinePM item)
        {
            item.APInvoiceId = entityPM.Id;
            item.Tenant = tenant;
            APInvoiceLine invoiceLine = new APInvoiceLine();
            APInvoiceMapping.MapInvoiceLine(item, invoiceLine, true);
            invoiceLineRepository.Add(invoiceLine);
        }
        private void UpdateInvoiceLine(APInvoiceLinePM item)
        {
            APInvoiceLine invoiceLine = invoiceLineRepository.GetSingleAPInvoiceLine(item.APInvoiceId, item.LineNumber, entityPM.Tenant);
            APInvoiceMapping.MapInvoiceLine(item, invoiceLine, false);
            invoiceLineRepository.Update(invoiceLine);
        }
        private void DeleteInvoiceLine(APInvoiceLinePM item)
        {
            APInvoiceLine line = invoiceLineRepository.GetSingleAPInvoiceLine(item.APInvoiceId, item.LineNumber, entityPM.Tenant);

            this.DisconnectPayable(line.EntityPayableId, line.ForiegnCurrencyAmount);

            invoiceLineRepository.Remove(line);
        }
        private void DisconnectInvoiceLine(APInvoiceLinePM item)
        {
            APInvoiceLine line = invoiceLineRepository.GetSingleAPInvoiceLine(item.APInvoiceId, item.LineNumber, entityPM.Tenant);
            line.EntityPayableId = null;
            item.EntityPayableId = null;
            invoiceLineRepository.Update(line);
        }
        #endregion

        #region Payments
        private void UpdateInvoicePayments(List<APInvoicePaymentPM> invoicePaymentsChangeSet)
        {
            foreach (APInvoicePaymentPM item in invoicePaymentsChangeSet)
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

        private void CreateInvoicePayment(APInvoicePaymentPM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("APInvoicePayment", entityPM.Tenant);

            APInvoicePayment invoicePayment = new APInvoicePayment()
            {
                Id = itemPM.Id,
                Tenant = tenant,
                APInvoiceId = entityPM.Id,
            };

            APInvoiceMapping.MapInvoicePayment(itemPM, invoicePayment, true);
            invoicePaymentRepository.Add(invoicePayment);

            string myPaymentNumber = itemPM.PaymentNumber;
            if (string.IsNullOrEmpty(myPaymentNumber))
            {
                if (!string.IsNullOrEmpty(itemPM.APPaymentId))
                {
                    APPayment myPayment = paymentRepository.GetSingleAPPayment(itemPM.APPaymentId);
                    if (myPayment != null)
                    {
                        myPaymentNumber = myPayment.PaymentNo;
                    }
                }
            }

            if (this.entityPM.TransferStatusCode == "TR")
            {
                QBOAPPaymentId = itemPM.APPaymentId;
            }


            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                Tenant = tenant,
                EventTypeCode = "COIN",
                UserId = loggedContactId,
                EntityId = itemPM.APInvoiceId,
                ObjectTableName = "APInvoice",
                Notes = "Connected with Payment: " + myPaymentNumber + " with Amount Due equals to: " + invoice.AmountDue,
            });
        }

        private void UpdateInvoicePayment(APInvoicePaymentPM itemPM)
        {
            APInvoicePayment invoicePayment = invoicePaymentRepository.GetSingleAPInvoicePayment(itemPM.Id, tenant);
            APInvoiceMapping.MapInvoicePayment(itemPM, invoicePayment, false);
            invoicePaymentRepository.Update(invoicePayment);
        }

        private void DeleteInvoicePayment(APInvoicePaymentPM itemPM)
        {
            APInvoicePayment invoicePayment = invoicePaymentRepository.GetSingleAPInvoicePayment(itemPM.Id, tenant);
            UpdatePaymentAmounts(itemPM, true);
            invoicePaymentRepository.Remove(invoicePayment);

            string myPaymentNumber = itemPM.PaymentNumber;
            if (string.IsNullOrEmpty(myPaymentNumber))
            {
                if (!string.IsNullOrEmpty(itemPM.APPaymentId))
                {
                    APPayment myPayment = paymentRepository.GetSingleAPPayment(itemPM.APPaymentId);
                    if (myPayment != null)
                    {
                        myPaymentNumber = myPayment.PaymentNo;
                    }
                }
            }


            if (this.entityPM.TransferStatusCode == "TR")
            {
                QBOAPPaymentId = itemPM.APPaymentId;
            }


            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                Tenant = tenant,
                EventTypeCode = "APID",
                UserId = loggedContactId,
                EntityId = itemPM.APInvoiceId,
                ObjectTableName = "APInvoice",
                Notes = "Disconnected from Payment: " + myPaymentNumber,
            });
        }

        private void UpdateInvoiceAmountDue()
        {
            if (this.invoicePaymentsChangeSet != null)
            {
                List<APInvoicePaymentPM> invoicepayments = (from a in this.invoicePaymentsChangeSet where a.ChangeSetOp != ChangeSetOperation.Delete select a).ToList();

                if (invoicepayments == null || invoicepayments.Count == 0)
                {
                    entityPM.IsClosed = false;

                    if (entityPM.StatusCode != "VD")
                    {
                        if (entityPM.StatusCode != "WA")
                        {
                            entityPM.AmountDue = entityPM.AmountInInvoiceCurrency.Value;
                            entityPM.StatusCode = "AD";
                        }
                    }
                }

                else
                {
                    double? conntectedPaymentAmount = MethodHelper.Round(invoicepayments.Sum(d => d.ForeignAmount), 2);

                    if ((entityPM.AmountInInvoiceCurrency < 0) || (conntectedPaymentAmount <= entityPM.AmountInInvoiceCurrency))
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

                foreach (APInvoicePaymentPM paymentInvoice in invoicepayments)
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

        private void UpdatePaymentAmounts(APInvoicePaymentPM itemPM, bool isDelete)
        {
            APPayment payment = paymentRepository.GetSingleAPPayment(itemPM.APPaymentId);

            if (payment != null)
            {
                if (payment.StatusCode == "VD")
                {
                    throw new Exception("Payment (" + payment.PaymentNo + ") is Voided");
                }

                else
                {
                    #region
                    double? invoicepaymentstotalamount = (from a in objectContext.APInvoicePayments
                                                          where a.APPaymentId == itemPM.APPaymentId
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
                        throw new Exception("The amount paid is not suitable to the total payment amount!!");
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
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.VATNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.HouseNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.MasterNumber);

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

            #region Entity References

            List<APInvoiceLinePM> myLines = new List<APInvoiceLinePM>();
            if (isNewEntity)
            {
                myLines = entityPM.InvoiceLines.ToList();
            }

            else
            {
                myLines = invoiceLinesChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
            }

            var entityIdAndObjectTables = (from a in myLines
                                           group a by new { a.EntityId, a.ObjectTableId, a.EntityReference } into gr
                                           select new
                                           {
                                               EntityId = gr.Key.EntityId,
                                               ObjectTableId = gr.Key.ObjectTableId,
                                               EntityReference = gr.Key.EntityReference
                                           }).ToList();

            foreach (var item in entityIdAndObjectTables)
            {
                MethodHelper.AddToSearchFields(ref mySearchFields, item.EntityReference);
            }
            #endregion

            #region Payments
            if (entityPM.InvoicePayments != null)
            {
                foreach (APInvoicePaymentPM item in entityPM.InvoicePayments)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, item.PaymentNumber);
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

        private double? Round(double? myValue, int digits)
        {
            double? myResult = myValue;

            if (myValue != null && digits >= 1 && digits <= 15)
            {
                string mySTR = String.Format("{0:N" + digits + "}", myValue);

                myResult = Convert.ToDouble(mySTR);
            }

            return myResult;
        }

        private void GetForeignFields()
        {
            int tenant = entityPM.Tenant;

            APInvoiceStatusRepository aPInvoiceStatusRepository = new APInvoiceStatusRepository(tenant);
            APInvoiceStatus status = aPInvoiceStatusRepository.GetSingleAPInvoiceStatus(invoice.StatusCode);
            entityPM.StatusName = status.Name;

            APInvoiceTransferStatusRepository aPInvoiceTransferStatusRepository = new APInvoiceTransferStatusRepository(tenant);
            APInvoiceTransferStatus t_status = aPInvoiceTransferStatusRepository.GetSingleAPInvoiceTransferStatus(invoice.TransferStatusCode);
            entityPM.TransferStatusName = t_status.Name;

            if (isNewEntity)
            {
                APInvoiceEntityQuery apInvoiceEntityQuery = new APInvoiceEntityQuery(invoiceEntityRepository);
                entityPM.InvoiceEntities = apInvoiceEntityQuery.GetInvoiceEntitiesPMForInvoice(invoice.Id, tenant);
            }

            //Full Accounting 
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

            if (!string.IsNullOrEmpty(entityPM.MainEntityId))
            {
                IShipmentsContext iShipmentsContext = ShipmentsContext.GetContext(tenant);
                entityPM.ShipmentConcurrencyGUID = (from d in iShipmentsContext.Shipments where d.Id == entityPM.MainEntityId select d.ConcurrencyGUID).FirstOrDefault();
                entityPM.ShipmentNewConcurrencyGUID = Guid.NewGuid().ToString();
            }
        }

        private void RunStoredProcedures()
        {
            if (!entityPM.IsMultipleEntities)
            {
                if (entityPM.MainEntityId != null)
                {
                    UpdateShipmentProfitClass.UpdatePayables(entityPM.MainEntityId, tenant, true);
                    UpdateShipmentProfitClass.UpdateProfit(entityPM.MainEntityId, tenant);
                }
            }
        }

        #region Journal & Journal Lines
        private void AddAPInvoiceJournalAndJournalLines(APInvoicePM theEntityPm, bool setApproved)
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
                    journal.AccountingDate = theEntityPm.AccountingDate != null ? theEntityPm.AccountingDate.Value : TenantServerConfigration.GetCurrentDateTime(tenant);
                    journal.TypeCode = "0";
                    journal.StatusCode = "2";
                    journal.CreatedByUserId = theEntityPm.CreatedByUserId;
                    journal.AccountingEntityCode = "4";
                    journal.AccountingEntityId = theEntityPm.Id;
                    journal.AccountingEntityReference = theEntityPm.InvoiceNumber;
                    journal.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    journal.UpdatedByUserId = theEntityPm.UpdatedByUserId;
                    journal.ApproveDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    journal.ApprovedByUserId = theEntityPm.ApprovedByUserId;
                    journal.ChangeSetOp = ChangeSetOperation.Insert;

                    // Insert Journal Lines 
                    // [Credit-Vendor]
                    GLAccountPM glAccount = getCreditGLAccount(theEntityPm.VendorId, theEntityPm.Tenant);
                    JournalLinePM journalLine = new JournalLinePM();
                    journalLine.Tenant = tenant;
                    journalLine.JournalId = journal.Id;
                    journalLine.Line = 1;
                    journalLine.ActionCode = "1";
                    journalLine.ActionTypeCodeEnum = MyJournalActionTypeEnum.Credit;
                    journalLine.DocumentDate = theEntityPm.InvoiceDate.Value;
                    journalLine.AccountingDate = theEntityPm.AccountingDate != null ? theEntityPm.AccountingDate.Value : TenantServerConfigration.GetCurrentDateTime(tenant);
                    journalLine.DueDate = theEntityPm.DueDate.Value;
                    journalLine.LocalAmount = (decimal)theEntityPm.AmountInLocalCurrency;
                    journalLine.CurrencyId = theEntityPm.InvoiceCurrencyId;
                    journalLine.ForeignAmount = (decimal)theEntityPm.AmountInInvoiceCurrency;
                    journalLine.ExchangeRate = (decimal)theEntityPm.InvoiceCurrencyExchangeRate;
                    journalLine.Reference1 = theEntityPm.InvoiceNumber;
                    journalLine.Reference2 = theEntityPm.MainEntityReference;
                    journalLine.Reference3 = !string.IsNullOrEmpty(theEntityPm.HouseNumber) ? theEntityPm.HouseNumber : theEntityPm.MasterNumber;
                    journalLine.Notes = theEntityPm.InternalNotes;
                    journalLine.CreditAccountId = glAccount.Id;
                    //journalLine.CreditControlAccountId = glAccount == null ? "" : glAccount.ControlAccountId;
                    journalLine.ChangeSetOp = ChangeSetOperation.Insert;
                    journal.JournalLines.Add(journalLine);

                    // [Debit]
                    journalLine = new JournalLinePM();
                    int counter = 1;
                    List<JournalLinePM> journalLines = (from d in theEntityPm.InvoiceLines
                                                        group d by new { d.ChargeTypeGLAccountId, d.ForiegnCurrencyId, d.ForiegnExchangeRate } into g
                                                        select new JournalLinePM()
                                                        {
                                                            Tenant = tenant,
                                                            ActionCode = "2",
                                                            ActionTypeCodeEnum = MyJournalActionTypeEnum.Debit,
                                                            JournalId = journal.Id,
                                                            DebitAccountId = g.Key.ChargeTypeGLAccountId,
                                                            CreditAccountId = theEntityPm.VendorGLAccountId,
                                                            Line = ++counter,
                                                            DocumentDate = theEntityPm.InvoiceDate.Value,
                                                            AccountingDate = theEntityPm.AccountingDate != null ? theEntityPm.AccountingDate.Value : TenantServerConfigration.GetCurrentDateTime(tenant),
                                                            DueDate = theEntityPm.DueDate.Value,
                                                            LocalAmount = (decimal)g.Sum(a => a.VatRecognizedPercentage == null ? a.LocalCurrencyAmount : (a.LocalCurrencyAmount + ((a.VatPercentage / 100) * ((1 - a.VatRecognizedPercentage) * a.LocalCurrencyAmount)))),
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
                    //List<APInvoiceTotalVAT> APInvoiceTotalVATs = new List<APInvoiceTotalVAT>();

                    APInvoiceTotalVATRepository vatRepository = new APInvoiceTotalVATRepository(tenant);
                    APInvoiceTotalVATQuery aPInvoiceTotalVATQuery = new APInvoiceTotalVATQuery(tenant);
                    //APInvoiceTotalVATs = vatRepository.GetInvoiceTotalVatsForInvoiceWithoutZeroVATPercent(theEntityPm.Id, tenant).ToList();
                    List<APInvoiceTotalVATPM> totalVats = aPInvoiceTotalVATQuery.GetInvoiceTotalVatsForInvoiceWithoutZeroVATPercent(theEntityPm.Id, tenant);
                    counter = journal.JournalLines.Count();

                    // Accounting settings 
                    FullAccountingSettingPM accountingSettings = getFullAccountingSettings(theEntityPm.Tenant);
                    foreach (APInvoiceTotalVATPM vat in totalVats)
                    {
                        journalLine = new JournalLinePM()
                        {
                            Tenant = tenant,
                            ActionCode = "2",
                            ActionTypeCodeEnum = MyJournalActionTypeEnum.Debit,
                            JournalId = journal.Id,
                            DebitAccountId = accountingSettings != null ? accountingSettings.VATInputsGLAccountId : "",
                            Line = ++counter,
                            DocumentDate = theEntityPm.InvoiceDate.Value,
                            AccountingDate = theEntityPm.AccountingDate != null ? theEntityPm.AccountingDate.Value : TenantServerConfigration.GetCurrentDateTime(tenant),
                            DueDate = theEntityPm.DueDate.Value,
                            LocalAmount = vat.VatRecognizedPercentage != null ? (((decimal)vat.VatRecognizedPercentage / 100) * (decimal)vat.LocalVATAmount) : (decimal)vat.LocalVATAmount,
                            CurrencyId = theEntityPm.InvoiceCurrencyId,
                            ForeignAmount = (decimal)vat.InvoiceCurrencyVATAmount,
                            ExchangeRate = (decimal)theEntityPm.InvoiceCurrencyExchangeRate,
                            Reference1 = theEntityPm.InvoiceNumber,
                            Reference2 = theEntityPm.MainEntityReference,

                            Reference3 = !string.IsNullOrEmpty(theEntityPm.HouseNumber) ? theEntityPm.HouseNumber : theEntityPm.MasterNumber,
                            CreditAccountId = theEntityPm.VendorGLAccountId,
                        };

                        journal.JournalLines.Add(journalLine);
                    }

                    IJournalUpdateServiceExt journalUpdate = ContainerAccessor.Container.Resolve(typeof(IJournalUpdateServiceExt), "JournalUpdateServiceExt", new ParameterOverride("", 1)) as IJournalUpdateServiceExt;
                    journalUpdate.Update(journal);
                }
            }
        }

        private GLAccountPM getCreditGLAccount(string vendorId, int tenant)
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

        private FullAccountingSettingPM getFullAccountingSettings(int tenant)
        {
            FullAccountingSettingPM accountingSettings;
            IFullAccountingSettingQueryServiceExt query = ContainerAccessor.Container.Resolve(typeof(IFullAccountingSettingQueryServiceExt), "FullAccountingSettingQueryServiceExt", new ParameterOverride("", 1)) as IFullAccountingSettingQueryServiceExt;
            accountingSettings = query.GetFullAccountingSettingByTenant(tenant);
            return accountingSettings;
        }

        #endregion 
    }
}
