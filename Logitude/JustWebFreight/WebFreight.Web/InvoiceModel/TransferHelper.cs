using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.Helpers;

namespace WebFreight.Web.InvoiceModel
{
    public class TransferHelper
    {
        private int tenant;
        private List<string> invoiceIds;
        private string entityTypeCode;
        
        private bool isJournalMode;
        private bool isExternalCodesFromTable;
        private IInvoiceContext objectContext;

        private PaymentTermRepository paymentTermRepository;
        private ChargeTypeAccountingRepository chargeTypeAccountingRepository;
        private ExternalSystemsTablesCodeRepository externalSystemsTablesCodeRepository;
        private CardExternalCodeByCurrencyRepository cardExternalCodeByCurrencyRepository;
        private ARInvoiceRepository aRInvoiceRepository;
        private ARPaymentRepository arPaymentRepository;
        private ARInvoiceLineRepository aRInvoiceLineRepository;
        private ARInvoiceTotalVATRepository aRInvoiceTotalVATRepository;
        private APInvoiceRepository aPInvoiceRepository;
        private APInvoiceLineRepository aPInvoiceLineRepository;
        private APInvoiceTotalVATRepository aPInvoiceTotalVATRepository;
        AccountingSetting accountingSetting;
        public TransferHelper(int tenant, string entityTypeCode)
        {
            this.tenant = tenant;
            this.entityTypeCode = entityTypeCode;
            this.objectContext = InvoiceContext.GetContext(tenant);

            AccountingSettingRepository accountingSettingRepository = new AccountingSettingRepository(tenant);
            this.accountingSetting = accountingSettingRepository.GetSingleAccountSetting(tenant);

            AccountingSystemRepository accountingSystemRepository = new AccountingSystemRepository(tenant);
            AccountingSystem accountingSystem = accountingSystemRepository.GetSingleAccountingSystem(accountingSetting.AccountingSystemCode);

            this.isJournalMode = accountingSystem.IsJournalMode;
            this.isExternalCodesFromTable = accountingSystem.IsExternalCodesFromTable;
        }

        public void Recalculate(List<string> ids)
        {
            if (ids.Count > 0)
            {
                this.invoiceIds = ids;
                this.paymentTermRepository = new PaymentTermRepository(tenant);
                this.chargeTypeAccountingRepository = new ChargeTypeAccountingRepository(tenant);
                this.externalSystemsTablesCodeRepository = new ExternalSystemsTablesCodeRepository(tenant);
                this.cardExternalCodeByCurrencyRepository = new CardExternalCodeByCurrencyRepository(tenant);

                if (entityTypeCode == "ARInvoice")
                {
                    this.aRInvoiceRepository = new ARInvoiceRepository(objectContext);
                    this.aRInvoiceLineRepository = new ARInvoiceLineRepository(objectContext);
                    this.aRInvoiceTotalVATRepository = new ARInvoiceTotalVATRepository(objectContext);

                    this.RecalculateARInvoiceData();

                    this.aRInvoiceRepository.SubmitChanges();
                    this.aRInvoiceLineRepository.SubmitChanges();
                    this.aRInvoiceTotalVATRepository.SubmitChanges();
                }

                else if (entityTypeCode == "APInvoice")
                {
                    aPInvoiceRepository = new APInvoiceRepository(objectContext);
                    aPInvoiceLineRepository = new APInvoiceLineRepository(objectContext);
                    aPInvoiceTotalVATRepository = new APInvoiceTotalVATRepository(objectContext);

                    this.RecalculateAPInvoiceData();

                    aPInvoiceRepository.SubmitChanges();
                    aPInvoiceLineRepository.SubmitChanges();
                    aPInvoiceTotalVATRepository.SubmitChanges();
                }

                else if (entityTypeCode == "ARPayment")
                {
                    arPaymentRepository = new ARPaymentRepository(objectContext);
                    this.RecalculateARPaymentData();
                    arPaymentRepository.SubmitChanges();
                }

                else if (entityTypeCode == "APPayment")
                {
                    arPaymentRepository = new ARPaymentRepository(objectContext);
                    this.RecalculateARPaymentData();
                    arPaymentRepository.SubmitChanges();


                }
            }
        }
        private void RecalculateARInvoiceData()
        {
            List<ARInvoice> invoices = aRInvoiceRepository.GetInvoicesListFromIdList(invoiceIds, tenant);
            IQueryable<ARInvoiceLine> lines = aRInvoiceLineRepository.GetInvoiceLinesByTenant(tenant);
            IQueryable<ARInvoiceTotalVAT> totalVATs = aRInvoiceTotalVATRepository.GetInvoiceTotalVATsByTenant(tenant);
            IQueryable<ChargeTypeAccounting> iQueryable_ChargeTypeAccounting = chargeTypeAccountingRepository.GetChargeTypeAccountings(tenant);
            IQueryable<ExternalSystemsTablesCode> iQueryable_SystemExternals = externalSystemsTablesCodeRepository.GetExternalSystemsTablesCodes(tenant);
            IQueryable<CardExternalCodeByCurrency> iQueryable_CardExternals = cardExternalCodeByCurrencyRepository.GetCardExternalCodeByCurrenciesByTenant(tenant);

            foreach (ARInvoice entity in invoices)
            {
                if(FieldIsEmpty(entity.DebitAccount))               
                {
                    if (isExternalCodesFromTable)
                    {
                        CardExternalCodeByCurrency myCardExternal = (from d in iQueryable_CardExternals where d.CardId == entity.BillToId && d.CurrencyId == entity.InvoiceCurrencyId select d).FirstOrDefault();
                        if (myCardExternal != null)
                        {
                            entity.DebitAccount = myCardExternal.ExternalRecievableTableId;
                        }
                    }

                    else
                    {
                        Card myCard = CardRepository.GetSingleCard(entity.BillToId, tenant, true);
                        AccountingSystemHelper accountingSystemHelper = new AccountingSystemHelper();
                        entity.DebitAccount = accountingSystemHelper.GetGenericCreditAccount(myCard.Id, entity.InvoiceCurrencyId, tenant, false);
                    }
                }

                if (FieldIsEmpty(entity.AccountingExternalCode))
                {
                    Currency myCurrency = CurrencyRepository.GetSingleCurrency(entity.InvoiceCurrencyId, tenant, true);
                    entity.AccountingExternalCode = myCurrency.AccountingExternalCode;
                }

                if (FieldIsEmpty(entity.PaymentTermExternalId))
                {
                    if (!string.IsNullOrEmpty(entity.PaymentTermId))
                    {
                        PaymentTerm myPaymentTerm = paymentTermRepository.GetSinglePaymentTerm(entity.PaymentTermId, tenant);
                        entity.PaymentTermExternalId = myPaymentTerm.ExternalId;
                    }
                }

                List<ARInvoiceLine> myLines = (from d in lines where d.ARInvoiceId == entity.Id select d).ToList();
                List<ARInvoiceTotalVAT> myTotalVATs = (from d in totalVATs where d.ARInvoiceId == entity.Id select d).ToList();

                #region lines
                foreach (ARInvoiceLine lineItem in myLines)
                {
                    if (FieldIsEmpty(lineItem.CreditAccount))                    
                    {
                        ChargesType myChargesType = ChargesTypeRepository.GetSingleChargesType(lineItem.ChargesTypeId, tenant, true);

                        if (myChargesType.AccountingVATSplit)
                        {
                            ChargeTypeAccounting myChargeTypeAccounting = (from d in iQueryable_ChargeTypeAccounting where d.ChargeTypeId == lineItem.ChargesTypeId && d.VatTypeId == lineItem.VatTypeId select d).FirstOrDefault();
                            if (myChargeTypeAccounting != null)
                            {
                                lineItem.CreditAccount = myChargeTypeAccounting.ReceivableCreditAccount;
                            }
                        }

                        else
                        {
                            if (isJournalMode)
                            {
                                lineItem.CreditAccount = myChargesType.ReceivableCreditAccount;
                            }

                            else
                            {
                                lineItem.CreditAccount = myChargesType.ReceivablesChargesTypeExtCode;
                            }
                        }
                    }
                }
                #endregion

                #region vats
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

                            else
                            {
                                itemVAT.ExternalVATCard = myVatType.ReceivablesExternalId;
                            }
                        }

                        if (FieldIsEmpty(itemVAT.ExternalTAXItemId))
                        {
                            itemVAT.ExternalTAXItemId = myVatType.ExternalTAXItemId;
                        }
                    }
                }
                #endregion

                ARTransferStatusHelper myStatusHelper = new ARTransferStatusHelper(entity, myLines, myTotalVATs, false);
                entity.TransferError = myStatusHelper.TransferError;
                entity.TransferStatusCode = myStatusHelper.TransferStatusCode;
            }
        }
        private void RecalculateAPInvoiceData()
        {
            List<APInvoice> invoices = aPInvoiceRepository.GetInvoicesListFromIdList(invoiceIds, tenant);
            IQueryable<APInvoiceLine> lines = aPInvoiceLineRepository.GetAPInvoiceLinesByTenant(tenant);
            IQueryable<APInvoiceTotalVAT> totalVATs = aPInvoiceTotalVATRepository.GetAPInvoiceTotalVats(tenant);
            IQueryable<ChargeTypeAccounting> iQueryable_ChargeTypeAccounting = chargeTypeAccountingRepository.GetChargeTypeAccountings(tenant);
            IQueryable<ExternalSystemsTablesCode> iQueryable_SystemExternals = externalSystemsTablesCodeRepository.GetExternalSystemsTablesCodes(tenant);
            IQueryable<CardExternalCodeByCurrency> iQueryable_CardExternals = cardExternalCodeByCurrencyRepository.GetCardExternalCodeByCurrenciesByTenant(tenant);

            foreach (APInvoice entity in invoices)
            {
                if (FieldIsEmpty(entity.CreditAccount))                
                {
                    if (isExternalCodesFromTable)
                    {
                        CardExternalCodeByCurrency myCardExternal = (from d in iQueryable_CardExternals where d.CardId == entity.VendorId && d.CurrencyId == entity.InvoiceCurrencyId select d).FirstOrDefault();
                        if (myCardExternal != null)
                        {
                            entity.CreditAccount = myCardExternal.ExternalRecievableTableId;
                        }
                    }

                    else
                    {
                        Card myCard = CardRepository.GetSingleCard(entity.VendorId, tenant, true);
                        AccountingSystemHelper accountingSystemHelper = new AccountingSystemHelper();
                        entity.CreditAccount = accountingSystemHelper.GetGenericCreditAccount(myCard.Id, entity.InvoiceCurrencyId, tenant, true);

                    }
                }

                if (FieldIsEmpty(entity.AccountingExternalCode))
                {
                    Currency myCurrency = CurrencyRepository.GetSingleCurrency(entity.InvoiceCurrencyId, tenant, true);
                    entity.AccountingExternalCode = myCurrency.AccountingExternalCode;
                }

                if (FieldIsEmpty(entity.PaymentTermExternalId))
                {
                    if (!string.IsNullOrEmpty(entity.PaymentTermId))
                    {
                        PaymentTerm myPaymentTerm = paymentTermRepository.GetSinglePaymentTerm(entity.PaymentTermId, tenant);
                        entity.PaymentTermExternalId = myPaymentTerm.ExternalId;
                    }
                }

                List<APInvoiceLine> myLines = (from d in lines where d.APInvoiceId == entity.Id select d).ToList();
                List<APInvoiceTotalVAT> myTotalVATs = (from d in totalVATs where d.APInvoiceId == entity.Id select d).ToList();

                #region lines
                foreach (APInvoiceLine lineItem in myLines)
                {
                    if (FieldIsEmpty(lineItem.DebitAccount))
                    {
                        ChargesType myChargesType = ChargesTypeRepository.GetSingleChargesType(lineItem.ChargesTypeId, tenant, true);

                        if (myChargesType.AccountingVATSplit)
                        {
                            ChargeTypeAccounting myChargeTypeAccounting = (from d in iQueryable_ChargeTypeAccounting where d.ChargeTypeId == lineItem.ChargesTypeId && d.VatTypeId == lineItem.VatTypeId select d).FirstOrDefault();
                            if (myChargeTypeAccounting != null)
                            {
                                lineItem.DebitAccount = myChargeTypeAccounting.PayableDebitAccount;
                            }
                        }

                        else
                        {
                            if (isJournalMode)
                            {
                                lineItem.DebitAccount = myChargesType.PayableDebitAccount;
                            }

                            else
                            {
                                lineItem.DebitAccount = myChargesType.ReceivablesChargesTypeExtCode;
                            }
                        }
                    }
                }
                #endregion

                #region vats
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

                            else
                            {
                                itemVAT.ExternalVATCard = myVatType.PayablesExternalId;
                            }
                        }

                        if (FieldIsEmpty(itemVAT.ExternalTAXItemId))
                        {
                            itemVAT.ExternalTAXItemId = myVatType.ExternalTAXItemId;
                        }
                    }
                }
                #endregion

                APTransferStatusHelper myStatusHelper = new APTransferStatusHelper(entity, myLines, myTotalVATs, false);
                entity.TransferError = myStatusHelper.TransferError;
                entity.TransferStatusCode = myStatusHelper.TransferStatusCode;
            }
        }
        private void RecalculateARPaymentData()
        {
            List<ARPayment> payments = arPaymentRepository.GetPaymentsListFromIdList(invoiceIds, tenant);
            foreach (ARPayment entity in payments)
            {
                ARPaymentTransferStatusHelper myStatusHelper = new ARPaymentTransferStatusHelper(entity);
                entity.TransferError = myStatusHelper.TransferError;
                entity.TransferStatusCode = myStatusHelper.TransferStatusCode;
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
    }

    public class ARTransferStatusHelper
    {
        public string TransferError { get; set; }
        public string TransferStatusCode { get; set; }
        public ARTransferStatusHelper(ARInvoice entity, List<ARInvoiceLine> myLines, List<ARInvoiceTotalVAT> myTotalVATs, bool isFromExternalTable)
        {
            if (entity.IsConstituentInvoice)
            {
                TransferError = "Constituent Invoice";
                TransferStatusCode = "NR";
            }

            else if (entity.TransferStatusCode == "TR")
            {
                TransferError = null;
                TransferStatusCode = "TR";
            }

            else if (entity.TransferStatusCode == "BL")
            {
                TransferError = null;
                TransferStatusCode = "BL";
            }

            else
            {
                bool isReady = true;
                string myError = "";                
                string externalCodeError = "External Code is required";
                string paymentTermError = "Payment Term External Id is required";
                string vatError = "External VAT Card is required";
                string linesError = "Credit Account is required";

                if (entity.DebitAccount == null || (entity.DebitAccount != null && string.IsNullOrEmpty(entity.DebitAccount.Trim())))
                {
                    isReady = false;
                    myError = "Debit Account is required";
                }

                if (isFromExternalTable)
                {
                    if (entity.PaymentTermExternalId == null || (entity.PaymentTermExternalId != null && string.IsNullOrEmpty(entity.PaymentTermExternalId.Trim())))
                    {
                        isReady = false;
                        myError = string.IsNullOrEmpty(myError) ? paymentTermError : myError + "," + paymentTermError;
                    }
                }

                else
                {
                    if (entity.AccountingExternalCode == null || (entity.AccountingExternalCode != null && string.IsNullOrEmpty(entity.AccountingExternalCode.Trim())))
                    {
                        isReady = false;
                        myError = string.IsNullOrEmpty(myError) ? externalCodeError : myError + "," + externalCodeError;
                    }
                }

                if (myLines.Where(d => d.CreditAccount == null || (d.CreditAccount != null && string.IsNullOrEmpty(d.CreditAccount.Trim()))).Any())
                {
                    isReady = false;
                    myError = string.IsNullOrEmpty(myError) ? linesError : myError + "," + linesError;
                }

                if (myTotalVATs.Where(d => d.VatPercent != null && d.VatPercent != 0 && (d.ExternalVATCard == null || (d.ExternalVATCard != null && string.IsNullOrEmpty(d.ExternalVATCard.Trim())))).Any())
                {
                    isReady = false;
                    myError = string.IsNullOrEmpty(myError) ? vatError : myError + "," + vatError;
                }

                if (isReady)
                {
                    TransferError = null;
                    TransferStatusCode = "RD";
                }

                else
                {
                    TransferError = myError;
                    TransferStatusCode = "NR";
                }
            }
        }        
        public ARTransferStatusHelper(ARInvoicePM entity, List<ARInvoiceLinePM> myLines, bool isFromExternalTable)
        {
            if (entity.IsConstituentInvoice)
            {
                TransferError = "Constituent Invoice";
                TransferStatusCode = "NR";
            }

            else if (entity.TransferStatusCode == "TR")
            {
                TransferError = null;
                TransferStatusCode = "TR";
            }

            else if (entity.TransferStatusCode == "BL")
            {
                TransferError = null;
                TransferStatusCode = "BL";
            }

            else
            {
                bool isReady = true;
                string myError = "";
                string externalCodeError = "External Code is required";
                string paymentTermError = "Payment Term External Id is required";
                string vatError = "External VAT Card is required";
                string linesError = "Credit Account is required";

                if (entity.DebitAccount == null || (entity.DebitAccount != null && string.IsNullOrEmpty(entity.DebitAccount.Trim())))
                {
                    isReady = false;
                    myError = "Debit Account is required";
                }

                if (isFromExternalTable)
                {
                    if (entity.PaymentTermExternalId == null || (entity.PaymentTermExternalId != null && string.IsNullOrEmpty(entity.PaymentTermExternalId.Trim())))
                    {
                        isReady = false;
                        myError = string.IsNullOrEmpty(myError) ? paymentTermError : myError + "," + paymentTermError;
                    }
                }

                else
                {
                    if (entity.AccountingExternalCode == null || (entity.AccountingExternalCode != null && string.IsNullOrEmpty(entity.AccountingExternalCode.Trim())))
                    {
                        isReady = false;
                        myError = string.IsNullOrEmpty(myError) ? externalCodeError : myError + "," + externalCodeError;
                    }
                }

                if (myLines.Where(d => d.CreditAccount == null || (d.CreditAccount != null && string.IsNullOrEmpty(d.CreditAccount.Trim()))).Any())
                {
                    isReady = false;
                    myError = string.IsNullOrEmpty(myError) ? linesError : myError + "," + linesError;
                }

                if (myLines.Where(d => d.VatPercentage != null && d.VatPercentage != 0 && (d.ExternalVATCard == null || (d.ExternalVATCard != null && string.IsNullOrEmpty(d.ExternalVATCard.Trim())))).Any())
                {
                    isReady = false;
                    myError = string.IsNullOrEmpty(myError) ? vatError : myError + "," + vatError;
                }

                if (isReady)
                {
                    TransferError = null;
                    TransferStatusCode = "RD";
                }

                else
                {
                    TransferError = myError;
                    TransferStatusCode = "NR";
                }
            }
        }
    }
    public class APTransferStatusHelper
    {
        public string TransferError { get; set; }
        public string TransferStatusCode { get; set; }
        public APTransferStatusHelper(APInvoice entity, List<APInvoiceLine> myLines, List<APInvoiceTotalVAT> myTotalVATs, bool isFromExternalTable)
        {
            if (entity.TransferStatusCode == "TR")
            {
                TransferError = null;
                TransferStatusCode = "TR";
            }

            else if (entity.TransferStatusCode == "BL")
            {
                TransferError = null;
                TransferStatusCode = "BL";
            }

            else
            {
                bool isReady = true;
                string myError = null;
                string externalCodeError = "External Code is required";
                string paymentTermError = "Payment Term External Id is required";
                string vatError = "External VAT Card is required";
                string linesError = "Debit Account is required";

                if (entity.CreditAccount == null || (entity.CreditAccount != null && string.IsNullOrEmpty(entity.CreditAccount.Trim())))
                {
                    isReady = false;
                    myError = "Credit Account is required";
                }

                if (isFromExternalTable)
                {
                    if (entity.PaymentTermExternalId == null || (entity.PaymentTermExternalId != null && string.IsNullOrEmpty(entity.PaymentTermExternalId.Trim())))
                    {
                        isReady = false;
                        myError = string.IsNullOrEmpty(myError) ? paymentTermError : myError + "," + paymentTermError;
                    }
                }

                else
                {
                    if (entity.AccountingExternalCode == null || (entity.AccountingExternalCode != null && string.IsNullOrEmpty(entity.AccountingExternalCode.Trim())))
                    {
                        isReady = false;
                        myError = string.IsNullOrEmpty(myError) ? externalCodeError : myError + "," + externalCodeError;
                    }
                }

                if (myLines.Where(d => d.DebitAccount == null || (d.DebitAccount != null && string.IsNullOrEmpty(d.DebitAccount.Trim()))).Any())
                {
                    isReady = false;
                    myError = string.IsNullOrEmpty(myError) ? linesError : myError + "," + linesError;
                }

                if (myTotalVATs.Where(d => d.VatPercent != null && d.VatPercent != 0 && (d.ExternalVATCard == null || (d.ExternalVATCard != null && string.IsNullOrEmpty(d.ExternalVATCard.Trim())))).Any())
                {
                    isReady = false;
                    myError = string.IsNullOrEmpty(myError) ? vatError : myError + "," + vatError;
                }

                if (isReady)
                {
                    TransferError = null;
                    TransferStatusCode = "RD";
                }

                else
                {
                    TransferError = myError;
                    TransferStatusCode = "NR";
                }
            }
        }
        public APTransferStatusHelper(APInvoicePM entity, List<APInvoiceLinePM> myLines, bool isFromExternalTable)
        {
            if (entity.TransferStatusCode == "TR")
            {
                TransferError = null;
                TransferStatusCode = "TR";
            }

            else if (entity.TransferStatusCode == "BL")
            {
                TransferError = null;
                TransferStatusCode = "BL";
            }

            else
            {
                bool isReady = true;
                string myError = null;
                string externalCodeError = "External Code is required";
                string paymentTermError = "Payment Term External Id is required";
                string vatError = "External VAT Card is required";
                string linesError = "Debit Account is required";

                if (entity.CreditAccount == null || (entity.CreditAccount != null && string.IsNullOrEmpty(entity.CreditAccount.Trim())))
                {
                    isReady = false;
                    myError = "Credit Account is required";
                }

                if (isFromExternalTable)
                {
                    if (entity.PaymentTermExternalId == null || (entity.PaymentTermExternalId != null && string.IsNullOrEmpty(entity.PaymentTermExternalId.Trim())))
                    {
                        isReady = false;
                        myError = string.IsNullOrEmpty(myError) ? paymentTermError : myError + "," + paymentTermError;
                    }
                }

                else
                {
                    if (entity.AccountingExternalCode == null || (entity.AccountingExternalCode != null && string.IsNullOrEmpty(entity.AccountingExternalCode.Trim())))
                    {
                        isReady = false;
                        myError = string.IsNullOrEmpty(myError) ? externalCodeError : myError + "," + externalCodeError;
                    }
                }

                if (myLines.Where(d => d.DebitAccount == null || (d.DebitAccount != null && string.IsNullOrEmpty(d.DebitAccount.Trim()))).Any())
                {
                    isReady = false;
                    myError = string.IsNullOrEmpty(myError) ? linesError : myError + "," + linesError;
                }

                if (myLines.Where(d => d.VatPercentage != null && d.VatPercentage != 0 && (d.ExternalVATCard == null || (d.ExternalVATCard != null && string.IsNullOrEmpty(d.ExternalVATCard.Trim())))).Any())
                {
                    isReady = false;
                    myError = string.IsNullOrEmpty(myError) ? vatError : myError + "," + vatError;
                }

                if (isReady)
                {
                    TransferError = null;
                    TransferStatusCode = "RD";
                }

                else
                {
                    TransferError = myError;
                    TransferStatusCode = "NR";
                }
            }
        }
    }
    public class ARPaymentTransferStatusHelper
    {
        public string TransferError { get; set; }
        public string TransferStatusCode { get; set; }
        public ARPaymentTransferStatusHelper(ARPayment entity)
        {
            if (entity.TransferStatusCode == "TR")
            {
                TransferError = null;
                TransferStatusCode = "TR";
            }

            else if (entity.TransferStatusCode == "BL")
            {
                TransferError = null;
                TransferStatusCode = "BL";
            }

            else
            {
                bool isReady = true;
                string myError = null;
                CardRepository cardRep = new CardRepository(entity.Tenant);
                CurrencyRepository currencyRep = new CurrencyRepository(entity.Tenant);
                Card card = cardRep.GetSingleCard(entity.BillToId, entity.Tenant);
                Currency currency = currencyRep.GetSingleCurrency(entity.PaymentCurrencyId, entity.Tenant);
                string currencyError = "Currency External Id is required";

               
                if (card != null)
                {
                    AccountingSystemHelper accountingSystemHelper = new AccountingSystemHelper();
                    var receivablesAccountingCard = accountingSystemHelper.GetGenericCreditAccount(card.Id, entity.PaymentCurrencyId, entity.Tenant, false);
                    if (string.IsNullOrEmpty(receivablesAccountingCard))
                    {
                        isReady = false;
                        myError = "Bill To External Id is required";
                    }
                }
                if (currency != null && string.IsNullOrEmpty(currency.AccountingExternalCode))
                {
                    isReady = false;
                    myError = string.IsNullOrEmpty(myError) ? currencyError : myError + "," + currencyError;
                }

                if (isReady)
                {
                    TransferStatusCode = "RD";
                    TransferError = null;
                }

                else
                {
                    TransferStatusCode = "NR";
                    TransferError = myError;
                }
            }
        }
        
    }
}