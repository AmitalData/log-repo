export class Urls {
    public static readonly GetByFilter = "**/getbyfilters?**";

    public static readonly Contacts = "**/contacts";
    public static readonly ContactsGetSingle = "**/contacts/getsingle?**";
    public static readonly ContactViewsGetByFilters = "**/contactviews/getbyfilters?**GetCount=false**";

    public static readonly Vendor = "**/vendors";
    public static readonly PartnersDomain = "**/PartnersDomain";
    public static readonly VendorsGetSingle = "**/vendors/getsingle?**";
    public static readonly VendorsviewGetSingle = "**/vendorviews/getsingle/?**";

    public static readonly Vessels = "**/vessels";
    public static readonly VesselsGetSingle = "**/vessels/getsingle?**";
    public static readonly VesselviewGetSingle = "**/vesselviews/getsingle/?**";

    public static readonly QuoteTemplateExtended = "**/QuoteTemplateExtended";
    public static readonly QuoteTemplatetextdesigns = "**/quotetemplatetextdesigns";
    public static readonly QuotetemplateSettings = "**/quotetemplatesettings";
    public static readonly Quotetemplatesections = "**/quotetemplatesections";
    public static readonly QuoteTemplateGetSingle = "**/quotetemplatesettings/getsingle?**";
    public static readonly PutQuoteTemplateTextDesignPMs = "**/PutQuoteTemplateTextDesignPMs";
    public static readonly PutQuoteTemplateHeaderFields = "**/PutQuoteTemplateHeaderFields";

    public static readonly AccountingSettings = "**/accountingsettings"

    public static readonly Tenants = "**/tenants"
    public static readonly PostChangePassword = '**/PostChangeUserPassword'

    public static readonly Countries = "**/countries"
    public static readonly CountriesGetSingle = "**/countries/getsingle?**";

    public static readonly States = "**/states"
    public static readonly StatesGetSingle = "**/states/getsingle?**";

    public static readonly CurrencyRate = "**/ratestables"
    public static readonly GetCurrenciesExchangeRateValue = "**/GetCurrenciesExchangeRateByValueDate?**"
    public static readonly ratestableviewsGetByFilter = "**/ratestableviews/getbyfilters?**"



    public static GetFilterSearch(filterBy: string) {
        return '**/getbyfilters?**' + filterBy + '**'
    }

    public static GetQuickSearch(CustomerNumber: string): string {
        return "**/GetCustomersQuickSearch?**" + CustomerNumber + "**";
    }
}