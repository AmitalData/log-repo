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

    public static readonly CountryCities="**/countrycities"
    public static readonly CountryCitiesGetSingle = "**/countrycities/getsingle?**";

    public static readonly GlobalZones = "**/globalzones"
    public static readonly GlobalZonesGetSingle = "**/globalzones/getsingle?**"
    
    public static readonly SpecialServicesTypes = "**/specialservicestypes"
    public static readonly SpecialServicesTypesGetSingle="**/specialservicestypes/getsingle?**"

    public static readonly Commodities="**/commodities"
    public static readonly CommoditiesGetSingle = "**/commodities/getsingle?**"

    public static readonly Regions= "**/regions"
    public static readonly RegionsGetSingle="**/regions/getsingle?**"

    public static readonly Shippingagents="**/shippingagents"
    public static readonly ShippingAgentGetSingle= "**/shippingagents/getsingle?**"

    public static readonly CustomAgents="**/customagents"
    public static readonly CustomAgentsGetSingle= "**/customagents/getsingle?**"

    public static readonly Truckers ="**/truckers"
    public static readonly TruckersGetSingle="**/truckers/getsingle?**"

    public static readonly MoveTypes ="**/movetypes"
    public static readonly MoveTypeGetSingle = "**/movetypes/getsingle?**"

    public static readonly CreditCardTypes = "**/creditcardtypes"
    public static readonly CreditCardTypesGetSingle = "**/creditcardtypes/getsingle?**"

    public static GetFilterSearch(filterBy: string) {
        return '**/getbyfilters?**' + filterBy + '**'
    }

    public static GetQuickSearch(CustomerNumber: string): string {
        return "**/GetCustomersQuickSearch?**" + CustomerNumber + "**";
    }
}