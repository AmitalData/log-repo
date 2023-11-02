export class AccountingURLs {
    public static readonly APInvoices = '**/apinvoices';
    public static readonly ARInvoices = '**/arinvoices';
    public static readonly ARInvoicesGetSingle = '**/arinvoices/getsingle?**';
    public static readonly APInvoiceViewsGetByFilters = '**/apinvoiceviews/getbyfilters?**';
    public static readonly ARInvoiceViewsGetByFilters = '**/arinvoiceviews/getbyfilters?**';
    public static readonly APPayments = '**/appayments';
    public static readonly ARPayments = '**/arpayments';
    public static readonly InvoicesGetSingle = '**/cardviews/getsingle/?**';
    public static readonly InvoiceDomain = '**/InvoiceDomain';
    public static readonly APInvoicesGetSingle = '**/apinvoices/getsingle?**';
    public static readonly ConsilidationInvoiceDomain = "**/ConsilidationInvoiceDomain"
    public static readonly VatTypePercentageCall = "**/GetVatTypePercentagePMByDate?**"
    public static readonly EntityResourceAccountingPeriod="**/EntityResource?objectTableName=AccountingPeriod&**"
    public static readonly PerformancelogsPostLogsList="**/performancelogs/PostLogsList"
    public static readonly DocumentTypeTemplateExtended="**/DocumentTypeTemplateExtended/getdocumenttypetemplatelistsfordocumenttype/**"
    public static readonly DocumentTypeExtended="**/DocumentTypeExtended/getsingledocumenttype/**"
    public static readonly DocumentTypeCustomField="**/DocumentTypeCustomField/**"
    public static readonly CargoSerialData='**/declarations'
}