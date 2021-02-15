export class AccountingSelectors {
   //#region APInvoice
   public static readonly APInvoiceVendor = '#APInvoice_VendorId';
   public static readonly APInvoiceInvoiceNumber = '#APInvoice_InvoiceNumber';
   public static readonly APInvoiceAmountInInvoice = '#APInvoice_AmountInInvoiceCurrency';
   public static readonly APInvoiceInvoiceCurrency = '#APInvoice_InvoiceCurrencyId';
   public static readonly APInvoiceInvoiceExchangeRate = '#APInvoice_InvoiceCurrencyExchangeRate';
   public static readonly APInvoiceInvoiceDate = '#date_APInvoice_InvoiceDate';
   public static readonly APInvoicePaymentTerm = '#APInvoice_PaymentTermId';
   public static readonly APInvoiceDueDate = '#date_APInvoice_DueDate';
   public static readonly APInvoiceVATNumber = '#APInvoice_VATNumber'
   public static readonly OkCreateAPInvoiceButton = '#Ok-CreateAPInvoice';
   public static readonly APInvoiceLineCheckBox = '#CheckBox_0_0_LBL';
   public static readonly APInvoiceVatType = '#APInvoice_VatTypeId';
   public static readonly APInvoiceSaveButton = '#APInvoiceBSave';
   public static readonly APInvoiceApproveButton = '#APInvoiceBApprove';
   public static readonly APInvoiceCancelApprovalButton = '#APInvoiceBCancelApproval';
   public static readonly APInvoiceVoidButton = '#APInvoiceBVoid';
   public static readonly ReceiveInvoiceButton = '#ReceiveInvoice';
   public static readonly APInvoiceLineForiegnCurrencyAmount = '#APInvoiceLine_ForiegnCurrencyAmount';
   public static readonly APInvoiceAmountInInvoiceCurrency = '#APInvoice_AmountInInvoiceCurrency';

   //#endregion
   //#region Receivable tab
   public static readonly ReceivableAccounting = '#RECEIVABLEAccounting';
   //#endregion
   //#region APPayment
   public static readonly NewAPPayment = '#NewAPPayment';
   public static readonly APPaymentVendor = '#APPayment_VendorId';
   public static readonly APPaymentMethod = '#APPayment_AccountingPaymentMethodId';
   public static readonly APPaymentAmount = '#APPayment_AmountInPaymentCurrency';
   public static readonly APPaymentCurrency = '#APPayment_PaymentCurrencyId';
   public static readonly APPaymentCurrencyExchangeRate = '#APPayment_PaymentCurrencyExchangeRate';
   public static readonly APPaymentRegisterDate = '#date_APPayment_RegisterDate';
   public static readonly APPaymentBranch = '#APPayment_BranchId';
   public static readonly APPaymentSaveButton = '#APPayment-Save';
   public static readonly APPaymentApproveButton = '#APPaymentBApprove';
   public static readonly PayableAccountingTab = '#PAYABLEAccounting';
   public static readonly QueryLink = '.QueryLink';
   public static readonly EditShipmentLine = 'button[id^="Edit_"]';
   //#endregion
   //#region ARPayment
   public static readonly ARPaymentTabInsideShipment = '#ARInvoiceTHARPayments';
   public static readonly NewARPayment = '#NewPayment';
   public static readonly ARPaymentPartner = '#ARPayment_PartnerId';
   public static readonly ARPaymentPaymentMethod = '#ARPayment_AccountingPaymentMethodId';
   public static readonly ARPaymentAmount = '#ARPayment_AmountInPaymentCurrency';
   public static readonly OkAddARPayment = '#ok-AddARPayment';
   public static readonly ARPaymentSave = '#ARPayment-Save';
   public static readonly ARPaymentBApprove = '#ARPaymentBApprove';
   //#endregion
   //#region ARInvoice
   public static readonly ARInvoiceBranch = '#ARInvoice_BranchId';
   public static readonly ARInvoiceVatNumber = '#ARInvoice_VatNumber';
   public static readonly ARInvoiceDueDate = '#date_ARInvoice_DueDate';
   public static readonly ARInvoicePaymentTerm = '#ARInvoice_PaymentTermId';
   public static readonly ARInvoiceInvoiceDate = '#date_ARInvoice_InvoiceDate';
   public static readonly ARInvoiceInvoiceCurrency = '#ARInvoice_InvoiceCurrencyId';
   public static readonly ARInvoicePartner = "#ARInvoice_PartnerId"
   public static readonly CreateCreditNoteARInvoiceButton = '#CreateCreditNote';
   public static readonly CreateARInvoiceButton = '#CreateARInvoice';
   public static readonly ARInvoiceExchangeRate = '#ARInvoice_InvoiceCurrencyExchangeRate';
   public static readonly OkCreateARInvoiceButton = '#Ok-CreateARInvoice';
   public static readonly ARInvoiceVatType = '#ARInvoice_VatTypeId';
   public static readonly ARInvoiceApproveButton = '#ARInvoiceBApprove';
   public static readonly ARInvoiceSetAsSentButton = '#ARInvoiceBSetAsSent';
   public static readonly ARInvoiceSaveButton = '#ARInvoiceBSaveAsDraft';
   public static readonly ARInvoiceVoidButton = '#ARInvoiceBVoid';
   public static readonly ARInvoiceCancelDraftButton = '#ARInvoiceBCancelDraft';
   public static readonly CreateCustomsCreditNote = '#CreateCustomsCreditNote';
   public static readonly CreateCustomsARInvoice = '#CreateCustomsInvoice';
   public static readonly ContainsCustoms = 'Customs';
   public static readonly GeneralSave = "#GeneralBSave"
   public static readonly VatTypeApplyToAll = '#VATApplyToAll';

   //#endregion
}