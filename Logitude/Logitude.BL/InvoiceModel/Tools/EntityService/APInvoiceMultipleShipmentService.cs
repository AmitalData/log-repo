using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.DataMapping;
using Logitude.BL.InvoiceModel.Tools.TraceEvents;
using Logitude.BL.InvoiceModel.Tools.Validating;
using Logitude.BL.Security;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.CommonDataModel;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.ExternalService;
using Logitude.BL.InvoiceModel.Tools.Behaviours;
using Logitude.BL.InvoiceModel.Tools.Behaviours.APInvoiceBehaviours;
using Logitude.BL.InvoiceModel.EntityOtherServices;

namespace Logitude.BL.InvoiceModel.Tools.EntityService
{
    public class APInvoiceMultipleShipmentService
    {
        private int tenant;
        private bool isNewEntity;
        //private bool isUpdateTotalVats;
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
        private ShipmentPayableRepository shipmentPayableRepository;
        private string QBOAPPaymentId;
        private bool IsTransferEnabled;
        private bool CanTransferToFTP;
        private bool TransferToFTPActivated;
        private bool setApproved;
        public APInvoiceMultipleShipmentService(IInvoiceContext objectContext, APInvoicePM entityPM)
        {
            this.tenant = entityPM.Tenant;
            this.entityPM = entityPM;
            //this.isUpdateTotalVats = false;
            this.objectContext = objectContext;
            this.myCommonContext = CommonDataContext.GetContext(tenant);
            this.invoiceRepository = new APInvoiceRepository(objectContext);
            this.invoiceLineRepository = new APInvoiceLineRepository(objectContext);
            this.invoiceTotalVatRepository = new APInvoiceTotalVATRepository(objectContext);
            this.invoiceEntityRepository = new APInvoiceEntityRepository(objectContext);
            this.invoicePaymentRepository = new APInvoicePaymentRepository(objectContext);
            this.paymentRepository = new APPaymentRepository(objectContext);
            this.shipmentPayableRepository = new ShipmentPayableRepository(tenant);
            this.GetLoggedContact();
            this.GetAccountingSystemData();
        }

        private void GetLoggedContact()
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

        private AccountingSetting myAccountingSetting;
        private void GetAccountingSystemData()
        {
            AccountingSettingRepository rep = new AccountingSettingRepository(tenant);
            this.myAccountingSetting = rep.GetSingleAccountingSetting(tenant);
        }

        private List<APInvoicePaymentPM> invoicePaymentsChangeSet = new List<APInvoicePaymentPM>();
        private List<APInvoiceMultipleShipmentPM> invoiceShipmentsChangeSet = new List<APInvoiceMultipleShipmentPM>();
        public void SetChangeSets(List<APInvoiceMultipleShipmentPM> invoiceMultipleShipmentsChangeSet, List<APInvoicePaymentPM> invoicePaymentsChangeSet)
        {
            if (invoicePaymentsChangeSet != null)
            {
                this.invoicePaymentsChangeSet = invoicePaymentsChangeSet;
            }

            if (invoiceMultipleShipmentsChangeSet != null)
            {
                this.invoiceShipmentsChangeSet = invoiceMultipleShipmentsChangeSet;
            }
        }

        public void Create()
        {
            this.isNewEntity = true;
            this.invoice = new APInvoice();

            this.InitializeComponents();

            APInvoiceValidator.Validate(entityPM, invoice, isNewEntity, this.objectContext, myCommonContext);
            APInvoiceTracing.Trace(entityPM, invoice, isNewEntity);

            this.CreateInvoiceEntities();
            this.BuildSearchFields();
            setApproved = entityPM.SetApproved;
            APInvoiceMultipleShipmentsHelper helper = new APInvoiceMultipleShipmentsHelper();
            helper.APInvoiceMultipleShipmentsQuickbooksValidating(entityPM, setApproved, isNewEntity, this.objectContext, this.myCommonContext);


            EntityAutomationService entityAutomationService = new EntityAutomationService(new EntityAutomationArgs() { Poco = invoice, EntityPM = entityPM, OldEntityPM = new APInvoicePM(), AutomationType = "OnCreate", ObjectTableName = "APInvoice", Tenant = entityPM.Tenant, EntityId = entityPM.Id });
            entityAutomationService.RunAutomation();

            APInvoiceMapping.MapEntity(entityPM, invoice, isNewEntity);
            this.invoiceRepository.Add(invoice);
            this.invoiceRepository.SubmitChanges();

            this.GetForeignFields();
            this.RunStoredProcedures();

            this.CreateAPInvoiceMessage();
        }

        public void Update(bool mapComposition = false)
        {
            if (mapComposition)
            {
                this.invoiceShipmentsChangeSet = entityPM.InvoiceMultipleShipments;
                this.invoicePaymentsChangeSet = entityPM.InvoicePayments;
            }

            this.isNewEntity = false;
            this.invoice = invoiceRepository.GetSingleAPInvoice(entityPM.Id, tenant);

            this.ValidateHigherStatus();

            this.InitializeComponents();

            APInvoiceValidator.Validate(entityPM, invoice, isNewEntity, this.objectContext, myCommonContext);
            APInvoiceTracing.Trace(entityPM, invoice, isNewEntity);
            
            this.UpdateInvoiceEntities();
            this.BuildSearchFields();
            setApproved = entityPM.SetApproved;

            APInvoiceMultipleShipmentsHelper helper = new APInvoiceMultipleShipmentsHelper();
            if (entityPM.SetReSendQBO)
            {
                helper.APInvoiceMultipleShipmentsQuickbooksValidating(entityPM, true, isNewEntity, this.objectContext, this.myCommonContext);

            }
            else
            {
                helper.APInvoiceMultipleShipmentsQuickbooksValidating(entityPM, setApproved, isNewEntity, this.objectContext, this.myCommonContext);
            }
            
            EntityAutomationService entityAutomationService = new EntityAutomationService(new EntityAutomationArgs() { Poco = invoice, EntityPM = entityPM, OldEntityPM = new APInvoicePM(), AutomationType = "OnUpdate", ObjectTableName = "APInvoice", Tenant = entityPM.Tenant, EntityId = entityPM.Id, EntityAutomationMappingPMFields = new EntityAutomationAPInvoiceMappingPMFields() });
            entityAutomationService.RunAutomation();

            APInvoiceMapping.MapEntity(entityPM, invoice, isNewEntity);
            this.CreateAPInvoiceMessage();

            invoiceRepository.Update(invoice);
            invoiceRepository.SubmitChanges();

            if (invoicePaymentsChangeSet != null)
            {
                if (invoicePaymentsChangeSet.Count > 0)
                {
                    bool isUpdatingPayments = true;

                    if (invoicePaymentsChangeSet.Where(d => d.ChangeSetOp == ChangeSetOperation.Insert || d.ChangeSetOp == ChangeSetOperation.Delete).Count() == 0)
                    {
                        isUpdatingPayments = false;
                    }

                    if (isUpdatingPayments)
                    {
                        this.UpdateInvoicePayments(invoicePaymentsChangeSet);
                        this.UpdateInvoiceAmountDue();
                        this.UpdatePaidDate();
                    }
                }
            }

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
            this.RunStoredProcedures();
        }

        #region Initialize
        private void InitializeComponents()
        {
            SetVendorDetails();
            if (string.IsNullOrEmpty(entityPM.Id))
            {
                entityPM.Id = IdCounter.GetNumber("APInvoice", entityPM.Tenant).ToString();
            }

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

            if (entityPM.SetApproved)
            {
                if (entityPM.AmountInInvoiceCurrency == 0)
                {
                    entityPM.StatusCode = "PD";
                }

                else
                {
                    entityPM.StatusCode = "AD";
                }

                entityPM.ApprovedDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                entityPM.ApprovedByUserId = loggedContactId;
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

            this.InitializeLinkedData();
            this.InitializeObjectTables();
            this.InitializeTransferComponents();

            entityPM.SubTotalInLocalCurrency = (double?)MethodHelper.Round(allActiveInvoiceLines.Sum(s => s.LocalCurrencyAmount), 2);
            entityPM.SubTotalInInvoiceCurrency = (double?)MethodHelper.Round(allActiveInvoiceLines.Sum(s => s.InvoiceCurrencyAmount), 2);
            SetAmountDue();
           
        }
        private void SetAmountDue()
        {
            if (isNewEntity)
            {
                this.MapAmountToAmountDue();
            }

            else
            {
                if (entityPM.StatusCode != "PP" && entityPM.StatusCode != "PD")
                {
                    if (entityPM.InvoicePayments.Where(d => d.ChangeSetOp == ChangeSetOperation.Insert || d.ChangeSetOp == ChangeSetOperation.Delete).Count() == 0)
                    {
                        this.MapAmountToAmountDue();
                    }
                }
            }
        }
        private void MapAmountToAmountDue()
        {
            entityPM.AmountDue = entityPM.AmountInInvoiceCurrency == null ? 0 : entityPM.AmountInInvoiceCurrency.Value;
            entityPM.AmountDueInLocalCurrency = entityPM.AmountDueInLocalCurrency == null ? 0 : entityPM.AmountDueInLocalCurrency.Value;
            entityPM.AmountDueInProfitCurrency = entityPM.AmountDueInProfitCurrency == null ? 0 : entityPM.AmountDueInProfitCurrency.Value;
        }
        #endregion

        private void SetVendorDetails()
        {
            CardRepository cardRepository = new CardRepository(entityPM.Tenant);
            Card card = cardRepository.GetSingleCard(entityPM.VendorId, entityPM.Tenant);
            entityPM.VendorContactId = card != null ? card.PrimaryContactId : null;
            entityPM.VendorVatNumber = card != null ? card.VatNumber : null;
        }

        #region LinkedData
        private List<APInvoiceMultipleShipmentPM> allInvoiceShipments;
        private List<APInvoiceMultipleShipmentPM> allActiveInvoiceShipments;
        private List<string> allShipmentIds;
        private List<string> allActiveShipmentIds;
        private List<APInvoiceLine> allInvoiceLines;
        private List<APInvoiceLine> allActiveInvoiceLines;
        private List<APInvoiceTotalVAT> allInvoiceVATs;
        private List<APInvoiceEntity> allInvoiceEntities;
        
        private List<ShipmentPayable> allPayables;
        private void InitializeLinkedData()
        {
            if (isNewEntity)
            {
                allInvoiceShipments = entityPM.InvoiceMultipleShipments;
                allActiveInvoiceShipments = allInvoiceShipments?.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();

                allShipmentIds = allInvoiceShipments?.Select(s => s.ShipmentId).ToList();                
                allActiveShipmentIds = allActiveInvoiceShipments?.Select(s => s.ShipmentId).ToList();

                allInvoiceLines = new List<APInvoiceLine>();
                allActiveInvoiceLines = new List<APInvoiceLine>();

                allInvoiceVATs = new List<APInvoiceTotalVAT>();
                allInvoiceEntities = new List<APInvoiceEntity>();
                allPayables = new List<ShipmentPayable>();
            }

            else
            {
                allInvoiceShipments = invoiceShipmentsChangeSet;
                allActiveInvoiceShipments = allInvoiceShipments?.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();

                allShipmentIds = allInvoiceShipments?.Select(s => s.ShipmentId).ToList();
                allActiveShipmentIds = allActiveInvoiceShipments?.Select(s => s.ShipmentId).ToList();

                allInvoiceLines = invoiceLineRepository.GetInvoiceLinesByInvoiceId(entityPM.Id, tenant).ToList();
                allActiveInvoiceLines = allInvoiceLines?.Where(d => allActiveShipmentIds.Contains(d.EntityId)).ToList();

                allInvoiceVATs = invoiceTotalVatRepository.GetInvoiceTotalVatsByInvoiceId(entityPM.Id, tenant).ToList();
                allInvoiceEntities = invoiceEntityRepository.GetInvoiceEntitiesForInvoice(entityPM.Id, entityPM.Tenant).ToList();                
                allPayables = shipmentPayableRepository.GetShipmentPayablesByEntityIds(allShipmentIds, entityPM.Tenant);
            }
        }
        #endregion

        #region ObjectTables
        string masterObjectTableId = null;
        string shipmentObjectTableId = null;
        private void InitializeObjectTables()
        {
            List<ObjectTable> myObjectTables = new ObjectTableRepository(entityPM.Tenant).context.ObjectTables.Where(d => (d.Tenant == 0 || d.Tenant == entityPM.Tenant) && (d.Name == "Shipment" || d.Name == "Master")).ToList();

            ObjectTable masterObjectTable = myObjectTables.Where(d => d.Name == "Master").FirstOrDefault();
            ObjectTable shipmentObjectTable = myObjectTables.Where(d => d.Name == "Shipment").FirstOrDefault();

            if (masterObjectTable != null)
            {
                masterObjectTableId = masterObjectTable.Id;
            }

            if (shipmentObjectTable != null)
            {
                shipmentObjectTableId = shipmentObjectTable.Id;
            }
        }
        #endregion

        #region Transfer
        private bool isJournalMode;
        private bool isExternalCodesFromTable;
        private bool isInitializingExternalFields;
        private void InitializeTransferComponents()
        {
            AccountingSettingRepository accountingSettingRepository = new AccountingSettingRepository(tenant);
            AccountingSetting accountingSetting = accountingSettingRepository.GetSingleAccountSetting(tenant);

            AccountingSystemRepository accountingSystemRepository = new AccountingSystemRepository(tenant);
            AccountingSystem accountingSystem = accountingSystemRepository.GetSingleAccountingSystem(accountingSetting.AccountingSystemCode);

            this.isJournalMode = accountingSystem.IsJournalMode;
            this.isExternalCodesFromTable = accountingSystem.IsExternalCodesFromTable;

            if (accountingSetting != null && accountingSystem != null)
            {
                this.CanTransferToFTP = accountingSystem.CanTransferToFTP;
                this.TransferToFTPActivated = accountingSetting.TransferToFTPActivated;

                if (accountingSetting.IsAPInvoicesTransferEnabled && accountingSystem.AllowAPInvoicesTransfer)
                {
                    this.IsTransferEnabled = true;
                }
            }

            this.InitializeExternalFields();
            this.InitializeTransferFields();
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
                if(FieldIsEmpty(entityPM.CreditAccount))
                {
                    if (isExternalCodesFromTable)
                    {
                        CardExternalCodeByCurrencyRepository cardExternalCodeByCurrencyRepository = new CardExternalCodeByCurrencyRepository(tenant);
                        IQueryable<CardExternalCodeByCurrency> iQueryable_CardExternals = cardExternalCodeByCurrencyRepository.GetCardExternalCodeByCurrenciesByTenant(tenant);

                        CardExternalCodeByCurrency myCardExternal = (from d in iQueryable_CardExternals where d.CardId == entityPM.VendorId && d.CurrencyId == entityPM.InvoiceCurrencyId select d).FirstOrDefault();
                        if (myCardExternal != null)
                        {
                            entityPM.CreditAccount = myCardExternal.ExternalRecievableTableId;
                        }
                    }

                    else
                    {
                        Card myCard = CardRepository.GetSingleCard(entityPM.VendorId, tenant, true);
                        AccountingSystemHelper accountingSystemHelper = new AccountingSystemHelper();
                        entityPM.CreditAccount = accountingSystemHelper.GetGenericCreditAccount(myCard.Id, entityPM.InvoiceCurrencyId, tenant, true);
                    }
                }

                if (entityPM.AccountingExternalCode == null || (entityPM.AccountingExternalCode != null && string.IsNullOrEmpty(entityPM.AccountingExternalCode.Trim())))
                {
                    Currency myCurrency = CurrencyRepository.GetSingleCurrency(entityPM.InvoiceCurrencyId, tenant, true);
                    entityPM.AccountingExternalCode = myCurrency.AccountingExternalCode;
                }

                if (entityPM.PaymentTermExternalId == null || (entityPM.PaymentTermExternalId != null && string.IsNullOrEmpty(entityPM.PaymentTermExternalId.Trim())))
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
                foreach (APInvoiceLine line in allInvoiceLines)
                {
                    if (FieldIsEmpty(line.DebitAccount))
                    {
                        ChargesType myChargesType = ChargesTypeRepository.GetSingleChargesType(line.ChargesTypeId, tenant, true);

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
                            if (isJournalMode)
                            {
                                line.DebitAccount = myChargesType.PayableDebitAccount;
                            }

                            else
                            {
                                line.DebitAccount = myChargesType.PayablesChargesTypeExternalCode;
                            }
                        }

                        invoiceLineRepository.Update(line);
                    }
                }
                #endregion

                #region VATs
                foreach (APInvoiceTotalVAT itemVAT in allInvoiceVATs)
                {
                    if (FieldIsEmpty(itemVAT.ExternalVATCard) || FieldIsEmpty(itemVAT.ExternalTAXItemId))
                    {
                        VatType myVatType = VatTypeRepository.GetSingleVatType(itemVAT.VatTypeId, tenant, true);

                        if (FieldIsEmpty(itemVAT.ExternalVATCard))
                        {
                            if (this.myAccountingSetting.AccountingSystemCode == "HV" || this.myAccountingSetting.AccountingSystemCode == "RH")
                            {
                                itemVAT.ExternalVATCard = this.myAccountingSetting.PayableVATCard;
                            }
                            else if(myVatType != null)
                            {
                                itemVAT.ExternalVATCard = myVatType.PayablesExternalId;
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
                string ExternalCodeError = "External Code is required";
                string paymentTermError = "Payment Term External Id is required";
                string vatError = "External VAT Card is required";
                string linesError = "Debit Account is required";

                if (FieldIsEmpty(entityPM.CreditAccount))
                {
                    isReady = false;
                    myError = "Credit Account is required";
                }

                if (isExternalCodesFromTable)
                {
                    if (FieldIsEmpty(entityPM.PaymentTermExternalId))
                    {
                        isReady = false;
                        myError = string.IsNullOrEmpty(myError) ? paymentTermError : myError + "," + paymentTermError;
                    }
                }

                else
                {
                    if (FieldIsEmpty(entityPM.AccountingExternalCode))
                    {
                        isReady = false;
                        myError = string.IsNullOrEmpty(myError) ? ExternalCodeError : myError + "," + ExternalCodeError;
                    }
                }

                if (isNewEntity)
                {
                    isReady = false;
                    myError = string.IsNullOrEmpty(myError) ? linesError : myError + "," + linesError;
                }

                else
                {
                    if (allActiveInvoiceLines.Count == 0)
                    {
                        //isReady = false;
                        //myError = string.IsNullOrEmpty(myError) ? linesError : myError + "," + linesError;
                    }

                    else
                    {
                        if (allActiveInvoiceLines.Where(d => d.DebitAccount == null || (d.DebitAccount != null && string.IsNullOrEmpty(d.DebitAccount.Trim()))).Any())
                        {
                            isReady = false;
                            myError = string.IsNullOrEmpty(myError) ? linesError : myError + "," + linesError;
                        }

                        var myGroup = (from a in allActiveInvoiceLines
                                       where a.VatTypeId != null
                                       && a.VatPercentage != null
                                       && a.VatPercentage != 0
                                       group a by new { a.VatTypeId, a.VatPercentage } into g
                                       select new
                                       {
                                           VatTypeId = g.Key.VatTypeId,
                                           VatPercentage = g.Key.VatPercentage,
                                       });

                        foreach (var g in myGroup)
                        {
                            APInvoiceTotalVAT myTotalVAT = allInvoiceVATs.Where(d => d.VatTypeId == g.VatTypeId && d.VatPercent == g.VatPercentage).FirstOrDefault();
                            if (myTotalVAT != null)
                            {
                                if (FieldIsEmpty(myTotalVAT.ExternalVATCard))
                                {
                                    isReady = false;
                                    myError = string.IsNullOrEmpty(myError) ? vatError : myError + "," + vatError;
                                    break;
                                }
                            }
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

        #region Entities
        private void CreateInvoiceEntities()
        {
            if (allInvoiceShipments.Count > 0)
            {
                var entityIdAndObjectTables
                    = (from a in allInvoiceShipments
                       group a by new { a.ShipmentId, a.ShipmentNumber, a.ShipmentLevelCode, a.IndexOrder } into gr
                       select new
                       {
                           EntityId = gr.Key.ShipmentId,
                           EntityReference = gr.Key.ShipmentNumber,
                           ShipmentLevelCode = gr.Key.ShipmentLevelCode,
                           IndexOrder = gr.Key.IndexOrder
                       }).ToList();

                foreach (var item in entityIdAndObjectTables)
                {
                    string myObjectTableId = item.ShipmentLevelCode == "C" ? masterObjectTableId : shipmentObjectTableId;

                    APInvoiceEntity invoiceEntity = new APInvoiceEntity()
                    {
                        Id = IdCounter.GetNumber("APInvoiceEntity", entityPM.Tenant).ToString(),
                        Tenant = tenant,
                        APInvoiceId = entityPM.Id,                        
                        EntityId = item.EntityId,                        
                        EntityReference = item.EntityReference,
                        ObjectTableId = myObjectTableId,
                        IndexOrder = item.IndexOrder,
                    };

                    invoiceEntityRepository.Add(invoiceEntity);
                }
            }
        }

        private void UpdateInvoiceEntities()
        {            
            if (invoice.StatusCode != "VD" && entityPM.StatusCode == "VD")
            {
                foreach (APInvoiceLine line in allInvoiceLines)
                {
                    DisconnectPayable(line.EntityPayableId, line.ForiegnCurrencyAmount);

                    line.EntityId = null;
                    line.EntityPayableId = null;
                    invoiceLineRepository.Update(line);
                }
                
                this.UpdateAllPayablesAccountedAmount(allPayables);

                foreach (APInvoiceEntity invoiceEntity in allInvoiceEntities)
                {
                    invoiceEntityRepository.Remove(invoiceEntity);
                    this.RunShipmentSQL(invoiceEntity.EntityId);
                }
            }

            else
            {
                foreach (APInvoiceMultipleShipmentPM item in allInvoiceShipments)
                {
                    switch (item.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                string myObjectTableId = item.ShipmentLevelCode == "C" ? masterObjectTableId : shipmentObjectTableId;

                                APInvoiceEntity invoiceEntity = new APInvoiceEntity()
                                {
                                    Id = IdCounter.GetNumber("APInvoiceEntity", entityPM.Tenant).ToString(),
                                    Tenant = tenant,
                                    APInvoiceId = entityPM.Id,                                    
                                    EntityId = item.ShipmentId,                                    
                                    EntityReference = item.ShipmentNumber,
                                    ObjectTableId = myObjectTableId,
                                    IndexOrder = item.IndexOrder,
                                };

                                invoiceEntityRepository.Add(invoiceEntity);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                List<APInvoiceEntity> myInvoiceEntities = allInvoiceEntities.Where(d => d.EntityId == item.ShipmentId).ToList();
                                List<APInvoiceLine> myInvoiceLines = allInvoiceLines.Where(d => d.EntityId == item.ShipmentId).ToList();

                                foreach (APInvoiceEntity invoiceEntity in myInvoiceEntities)
                                {
                                    invoiceEntityRepository.Remove(invoiceEntity);
                                }

                                foreach (APInvoiceTotalVAT itemVAT in allInvoiceVATs)
                                {
                                    invoiceTotalVatRepository.Remove(itemVAT);
                                }

                                foreach (APInvoiceLine itemLine in myInvoiceLines)
                                {
                                    DisconnectPayable(itemLine.EntityPayableId, itemLine.ForiegnCurrencyAmount);
                                    invoiceLineRepository.Remove(itemLine);
                                }

                                List<InvoiceTotalsClass> group
                                    = (from a in allActiveInvoiceLines
                                       where a.EntityId != item.ShipmentId
                                       group a by new { a.VatTypeId, a.VatPercentage } into g
                                       select new InvoiceTotalsClass()
                                       {
                                           Id = g.Key.VatTypeId,
                                           VatTypeId = g.Key.VatTypeId,
                                           VatTypePercentage = g.Key.VatPercentage,
                                           LocalCurrencyAmount = g.Sum(s => s.LocalCurrencyAmount),
                                           InvoiceCurrencyAmount = g.Sum(s => s.InvoiceCurrencyAmount),
                                           ProfitCurrencyAmount = g.Sum(s => s.ProfitCurrencyAmount),
                                       }).ToList();

                                foreach (InvoiceTotalsClass itemTotal in group)
                                {
                                    APInvoiceTotalVAT myRecord = new APInvoiceTotalVAT()
                                    {
                                        Id = IdCounter.GetNumber("APInvoiceTotalVAT", entityPM.Tenant).ToString(),
                                        Tenant = entityPM.Tenant,
                                        APInvoiceId = entityPM.Id,
                                        VatTypeId = itemTotal.Id,
                                        VatPercent = MethodHelper.Roundd(itemTotal.VatTypePercentage, 3),
                                        LocalVatableAmount = MethodHelper.Roundd(itemTotal.LocalCurrencyAmount, 2),
                                        InvoiceCurrencyVatableAmount = MethodHelper.Roundd(itemTotal.InvoiceCurrencyAmount, 2),
                                        ProfitVatableAmount = MethodHelper.Roundd(itemTotal.ProfitCurrencyAmount, 2),
                                    };

                                    if (isInitializingExternalFields)
                                    {
                                        VatType myVatType = VatTypeRepository.GetSingleVatType(itemTotal.VatTypeId, tenant, true);
                                        if (myVatType != null)
                                        {                                            
                                            myRecord.ExternalTAXItemId = myVatType.ExternalTAXItemId;

                                            if (this.myAccountingSetting.AccountingSystemCode == "HV" || this.myAccountingSetting.AccountingSystemCode == "RH")
                                            {
                                                myRecord.ExternalVATCard = this.myAccountingSetting.PayableVATCard;
                                            }
                                            else
                                            {
                                                myRecord.ExternalVATCard = myVatType.PayablesExternalId;
                                            }
                                        }
                                    }

                                    myRecord.LocalVATAmount = MethodHelper.Roundd((myRecord.LocalVatableAmount * myRecord.VatPercent / 100), 2);
                                    myRecord.InvoiceCurrencyVATAmount = MethodHelper.Roundd((myRecord.InvoiceCurrencyVatableAmount * myRecord.VatPercent / 100), 2);
                                    myRecord.ProfitCurrencyVATAmount = MethodHelper.Roundd((myRecord.ProfitVatableAmount * myRecord.VatPercent / 100), 2);
                                    invoiceTotalVatRepository.Add(myRecord);
                                }

                                this.UpdateAllPayablesAccountedAmount(allPayables.Where(d=>d.ShipmentId == item.ShipmentId).ToList());
                                this.RunShipmentSQL(item.ShipmentId);
                                break;
                            }
                    }
                }
            }
        }

        private void DisconnectPayable(string payableId, double? myForiegnCurrencyAmount)
        {
            ShipmentPayable payable = (from a in allPayables where a.Id == payableId select a).FirstOrDefault();

            if (payable != null)
            {
                if (payable.ShipmentPayableAmountTypeCode == "NEXP")
                {
                    List<ShipmentPayable> childPayables = shipmentPayableRepository.GetChildPayablesByParentPayable(payable.Id, tenant);

                    foreach (ShipmentPayable insideItem in childPayables)
                    {
                        shipmentPayableRepository.Remove(insideItem);
                    }

                    allPayables.Remove(payable);
                    shipmentPayableRepository.Remove(payable);                    
                }

                else
                {
                    payable.CorrectionAmount = null;
                    payable.CorrectionByUserId = null;
                    payable.CorrectionDate = null;
                    payable.CorrectionNote = null;
                    myForiegnCurrencyAmount = myForiegnCurrencyAmount == null ? 0 : myForiegnCurrencyAmount.Value;
                    double? myAccountedAmount = payable.AccountedAmount == null ? 0 : payable.AccountedAmount.Value;
                    double? myExpectedAmount = payable.ExpectedAmount == null ? 0 : payable.ExpectedAmount.Value;
                    double? myCorrectionAmount = payable.CorrectionAmount == null ? 0 : payable.CorrectionAmount.Value;

                    double? myOtherInvoicesAmounts = myAccountedAmount - myForiegnCurrencyAmount;
                    double? myOpenAmount = myExpectedAmount - myOtherInvoicesAmounts - myCorrectionAmount;

                    payable.OpenAmount = (double?)MethodHelper.Round(myOpenAmount, 2);
                    payable.OpenAmountInLocalCurrency = (double?)MethodHelper.Round(payable.OpenAmount * payable.Rate, 2);
                    payable.OpenAmountInProfitCurrency = (double?)MethodHelper.Round(payable.OpenAmountInLocalCurrency / payable.ProfitCurrencyExchangeRate, 2);
                    shipmentPayableRepository.Update(payable);
                }

                shipmentPayableRepository.SubmitChanges();
            }
        }

        private void UpdateAllPayablesAccountedAmount(List<ShipmentPayable> myPayables)
        {
            if (myPayables.Count > 0)
            {
                this.invoiceLineRepository.SubmitChanges();

                List<string> allPayablesIds = allPayables.Select(s => s.Id).ToList();
                List<APInvoiceLine> allPayablesInvoicesLines = invoiceLineRepository.GetPayablesInvoicesLines(allPayablesIds, tenant);

                foreach (ShipmentPayable payable in myPayables)
                {
                    List<APInvoiceLine> myInvoiceslines = allPayablesInvoicesLines.Where(d => d.EntityPayableId == payable.Id).ToList();

                    double? myAccountedAmount = myInvoiceslines.Sum(s => s.ForiegnCurrencyAmount);
                    double? myAccountedAmount_Local = myInvoiceslines.Sum(s => s.LocalCurrencyAmount);
                    double? myAccountedAmount_Profit = myInvoiceslines.Sum(s => s.ProfitCurrencyAmount);

                    payable.AccountedAmount = (double?)MethodHelper.Round(myAccountedAmount, 2);
                    payable.AccountedAmountInLocalCurrency = (double?)MethodHelper.Round(myAccountedAmount_Local, 2);
                    payable.AccountedAmountInProfitCurrency = (double?)MethodHelper.Round(myAccountedAmount_Profit, 2);

                    this.SetStatusCode(payable);

                    shipmentPayableRepository.Update(payable);
                }

                shipmentPayableRepository.SubmitChanges();
            }
        }

        private void SetStatusCode(ShipmentPayable entity)
        {
            if (entity.ShipmentPayableAmountTypeCode == "NEXP")
            {
                entity.ShipmentPayableLineStatusCode = "ACCT";
            }

            else if (entity.Quantity == null || entity.UnitPrice == null)
            {
                entity.ShipmentPayableLineStatusCode = "EMPT";
            }

            else if (entity.ExpectedAmount == entity.OpenAmount)
            {
                entity.ShipmentPayableLineStatusCode = "OAMT";
            }

            else if (entity.ExpectedAmount == entity.AccountedAmount)
            {
                entity.ShipmentPayableLineStatusCode = "ACCT";
            }

            else
            {
                entity.ShipmentPayableLineStatusCode = "PACC";
            }
        }
        private void RunShipmentSQL(string myShipmentId)
        {
            UpdateShipmentProfitClass.UpdatePayables(myShipmentId, tenant,true);
            UpdateShipmentProfitClass.UpdateProfit(myShipmentId, tenant);
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
                List<APInvoicePaymentPM> invoicepayments = invoicePaymentsChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();

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
                    double? Amount = MethodHelper.Roundd(payment.AmountInPaymentCurrency, 2);
                    double? ExternalAmount = MethodHelper.Roundd(payment.ExternalPaymentAmount, 2);
                    double? PaidAmount = (from a in objectContext.APInvoicePayments where a.APPaymentId == itemPM.APPaymentId && a.Tenant == tenant select a).Sum(s => s.PaymentAmount);

                    if (PaidAmount == null)
                    {
                        PaidAmount = 0;
                    }

                    if (isDelete)
                    {
                        PaidAmount -= itemPM.PaymentAmount;
                    }

                    PaidAmount = MethodHelper.Round(PaidAmount, 2);
                    double? AllPaidAmount = MethodHelper.Roundd(PaidAmount + ExternalAmount, 2);

                    if (AllPaidAmount > Amount)
                    {
                        throw new Exception("The amount paid is not suitable to the total payment amount!!");
                    }

                    else
                    {
                        if (PaidAmount < 0)
                        {
                            PaidAmount = PaidAmount * -1;
                        }

                        double? OpenAmount = MethodHelper.Round((Amount - PaidAmount - ExternalAmount), 2);

                        payment.OpenAmount = OpenAmount;

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

                        paymentRepository.Update(payment);
                    }
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
                APInvoicePaymentPM itemPM = invoicePaymentsChangeSet.Where(d => d.ChangeSetOp == ChangeSetOperation.Insert).FirstOrDefault();
                if (itemPM != null)
                {
                    entityPM.PaidDate = (from d in objectContext.APPayments where d.Id == itemPM.APPaymentId select d.ValueDate).FirstOrDefault();
                }
            }

            invoice.PaidDate = entityPM.PaidDate;
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
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.InternalNotes);

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
            List<string> allActiveShipmentNumbers = allInvoiceShipments?.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).Select(s => s.ShipmentNumber).ToList();

            foreach (string shipmentNumbers in allActiveShipmentNumbers)
            {
                MethodHelper.AddToSearchFields(ref mySearchFields, shipmentNumbers);
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

            entityPM.SearchFields = mySearchFields;
            invoice.SearchFields = mySearchFields;
        }
        #endregion

        private void GetForeignFields()
        {
            int tenant = entityPM.Tenant;

            APInvoiceStatusRepository aPInvoiceStatusRepository = new APInvoiceStatusRepository(tenant);
            APInvoiceStatus status = aPInvoiceStatusRepository.GetSingleAPInvoiceStatus(invoice.StatusCode);
            entityPM.StatusName = status.Name;

            APInvoiceTransferStatusRepository aPInvoiceTransferStatusRepository = new APInvoiceTransferStatusRepository(tenant);
            APInvoiceTransferStatus t_status = aPInvoiceTransferStatusRepository.GetSingleAPInvoiceTransferStatus(invoice.TransferStatusCode);
            entityPM.TransferStatusName = t_status.Name;
        }
        private void RunStoredProcedures()
        {
            if (!entityPM.IsMultipleEntities)
            {
                if (entityPM.MainEntityId != null)
                {
                    UpdateShipmentProfitClass.UpdatePayables(entityPM.MainEntityId, tenant,true);
                    UpdateShipmentProfitClass.UpdateProfit(entityPM.MainEntityId, tenant);
                }
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

        private void CreateAPInvoiceMessage()
        {
            if (setApproved && IsTransferEnabled)
            {
                if (CanTransferToFTP && TransferToFTPActivated)
                {
                    if (!string.IsNullOrEmpty(entityPM.TransferError))
                    {
                        throw new ApplicationException(entityPM.TransferError);
                    }
                    else
                    {
                        bool isFTP = CanTransferToFTP && TransferToFTPActivated;

                        this.invoice = invoiceRepository.GetSingleAPInvoice(this.entityPM.Id, this.entityPM.Tenant);
                        List<APInvoice> entities = new List<APInvoice>();
                        entities.Add(this.invoice);

                        APInvoiceMessageHelper myHelper = new APInvoiceMessageHelper(entities, this.invoice.InvoiceNumber + ".xml", tenant, false, isFTP);
                        myHelper.Transfer();
                    }
                }
            }
        }
    }
}
