export class AccountingURLs {
    public static readonly APInvoices = '**/apinvoices';
    public static readonly ARInvoices = '**/arinvoices';
    public static readonly ARInvoicesGetSingle = '**/arinvoices/getsingle?**';
    public static readonly APInvoiceViewsGetByFilters = '**/apinvoiceviews/getbyfilters?**';
    public static readonly ARInvoiceViewsGetByFilters = '**/arinvoiceviews/getbyfilters?**';
    public static readonly APPayments = '**/appayments';
    public static readonly ARPayments = '**/arpayments';
    public static readonly InvoiceDomain = '**/InvoiceDomain';
    public static readonly ConsilidationInvoiceDomain = "**/ConsilidationInvoiceDomain"
    public static readonly VatTypePercentageCall = "**/GetVatTypePercentagePMByDate?**"
    public static readonly EntityResourceAccountingPeriod="**/EntityResource?objectTableName=AccountingPeriod&**"
    public static readonly PerformancelogsPostLogsList="**/performancelogs/PostLogsList"

}