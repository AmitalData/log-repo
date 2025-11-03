export class ARInvoiceSelectors {

    public static readonly NewInvoiceMenu = '#NewInvoice';
    public static readonly NewGeneralInvoice = '#NewGeneralInvoice';
    public static readonly BillTo = '#ARInvoice_BillToId';
    public static readonly Currency = '#ARInvoice_InvoiceCurrencyId';
    public static readonly InvoiceDate = '#date_ARInvoice_InvoiceDate';
    public static readonly PaymentTerm = '#ARInvoice_PaymentTermId';
    public static readonly VatNumber = '#ARInvoice_VatNumber';
    public static readonly Branch = '#ARInvoice_BranchId';
    public static readonly DueDate = '#date_ARInvoice_DueDate';
    public static readonly AddInvoiceLine = '#Add';
    public static readonly InvoiceLineChargesType = '#ARInvoiceLine_ChargesTypeId';
    public static readonly InvoiceLineLocalDescription = '#ARInvoiceLine_LocalDescription';
    public static readonly InvoiceLineVatType = '#ARInvoiceLine_VatTypeId';
    public static readonly InvoiceLineForiegnCurrency = '#ARInvoiceLine_ForiegnCurrencyId';
    public static readonly Quantity = '#ARInvoiceLine_Quantity';
    public static readonly UnitPrice = '#ARInvoiceLine_UnitPrice';

    public static readonly LogLovARInvoiceInvoiceCurrency = "#LogLov_ARInvoice_InvoiceCurrencyId"
    public static readonly ARInvoicePartner = "#ARInvoice_PartnerId"
    public static readonly CreateCreditNoteARInvoiceButton = '#CreateCreditNote';
    public static readonly CreateARInvoiceButton = '#CreateARInvoice';
    public static readonly ARInvoiceExchangeRate = '#ARInvoice_InvoiceCurrencyExchangeRate';
    public static readonly OkCreateARInvoiceButton = '#Ok-AddARInvoice';
    public static readonly ARInvoiceVatType = '#ARInvoice_VatTypeId';
    public static readonly ARInvoiceApproveButton = "button[id^='ARInvoiceBApprove']:last";
    public static readonly ARInvoiceSetAsSentButton = '#ARInvoiceBSetAsSent';
    public static readonly ARInvoiceAutoCreditButton = '#ARInvoiceBAutoCredit';
    public static readonly ARInvoiceSaveButton = '#ARInvoiceBSaveAsDraft';
    public static readonly ARInvoiceVoidButton = '#ARInvoiceBVoid';
    public static readonly ARInvoiceCancelDraftButton = '#ARInvoiceBCancelDraft';
    public static readonly CreateCustomsCreditNote = '#CreateCustomsCreditNote';
    public static readonly CreateCustomsARInvoice = '#CreateCustomsInvoice';
    public static readonly ContainsCustoms = 'Customs';
    public static readonly GeneralSave = "#GeneralBSave"
    public static readonly VatTypeApplyToAll = '#VATApplyToAll';
    public static readonly APInvoiceBranch = '#APInvoice_BranchId';
    public static readonly LogLovARInvoicePartnerId = "#LogLov_ARInvoice_PartnerId"
    public static readonly IsConsolidationChecked = "checkbox[id^='IsConsolidationChecked']"
    public static readonly ARInvoiceHeaderStatusName = "[id^='ARInvoiceHeaderStatusName']:last"
}