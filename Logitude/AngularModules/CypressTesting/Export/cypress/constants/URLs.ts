export class URLs {
    public static readonly InvoicesGetSingle = '**/cardviews/getsingle/?**';
    public static readonly APInvoices = '**/apinvoices';
    public static readonly ARInvoices = '**/arinvoices';
    public static readonly ARInvoicesGetSingle = '**/arinvoices/getsingle?**';
    public static readonly APInvoiceViewsGetByFilters = '**/cardviews/getbyfilters?**';
    public static readonly ARInvoiceViewsGetByFilters = '**/arinvoiceviews/getbyfilters?**';
    public static readonly APPayments = '**/appayments';
    public static readonly ARPayments = '**/arpayments';
    public static readonly GLAccounts = '**/glaccounts';
    public static readonly Bankaccounts = '**/bankaccounts';
    public static readonly PartnersDomain = '**/PartnersDomain';
    public static readonly CustomerGetSingle = '**/customers/getsingle?**';
    public static readonly GLAccountGetSingle = '**/fullaccountingsettingviews/getsingle/?**';
    public static readonly ChartOfAccounts = '**/chartofaccounts';
    public static readonly ChartOfAccountsGetSingle = "**/chartofaccounts/getsingle?**";
    public static readonly VendorsGetSingle = "**/vendors/getsingle?**";
    public static readonly Journals = "**/journals";
    public static readonly InterestBases = "**/interestbasestypes";
    public static readonly BankDeposits = "**/bankdeposits";
    public static readonly CashbookViewGetSingle = "**/cashbookviews/getsingle/?**";

    public static readonly CurrencyRate = '**/currencyRate';


    public static readonly Declarations = '**/declarations';
    public static readonly SupplierInvoice = '**/supplierinvoices';


    public static GetFilterSearch(filterBy: string) {
        return '**/getbyfilters?**' + filterBy + '**'
    }

    public static GetQuickSearch(filterBy: string) {
        return '**/GetQuickSearch?**' + filterBy + '**'
    }
}