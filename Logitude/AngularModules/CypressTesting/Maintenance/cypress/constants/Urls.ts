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

    public static readonly CountryCities = "**/countrycities"
    public static readonly CountryCitiesGetSingle = "**/countrycities/getsingle?**";

    public static readonly GlobalZones = "**/globalzones"
    public static readonly GlobalZonesGetSingle = "**/globalzones/getsingle?**"

    public static readonly BankAccounts = "**/bankaccountlites"
    public static readonly BankAccountsGetSingle = "**/bankaccountlites/getsingle?**"
    public static readonly BankAccountsviewGetSingle = "**/bankaccountliteviews/getsingle/?**";

    public static readonly PackageTypes = "**/packagetypes"
    public static readonly PackageTypesGetSingle = "**/packagetypes/getsingle?**"
    public static readonly PackageTypesviewGetSingle = "**/packagetypeviews/getsingle/?**";

    public static readonly Branches = "**/branches"
    public static readonly BranchesGetSingle = "**/branches/getsingle?**"
    public static readonly BranchesviewGetSingle = "**/branchviews/getsingle/?**";
    public static readonly BrancheAddress = "**/addresses"

    public static readonly GetTenatCurrencies = "**/GetCopyCurrencyToTenant?**"
    public static readonly Currencies = "**/currencies"
    public static readonly CurrenciesGetSingle = "**/currencies/getsingle?**"
    public static readonly CurrenciesviewGetList = "**/currencyviews/getbyfilters?**";
    public static readonly CurrenciesviewGetSingle = "**/currencyviews/getsingle/?**";

    public static readonly FBLStocks = "**/FBLStockExtened/GetCreateFBLStocksOperation?**"
    public static readonly FBLStocksDelete = "**/FBLStockExtened/GetDeleteFBLStocksOperation?**";
    public static readonly FBLStocksGetAll= "**/FBLStockExtened/GetAllFBLStockPMsByTenant?**";

    public static readonly SpecialServicesTypes = "**/specialservicestypes"
    public static readonly SpecialServicesTypesGetSingle = "**/specialservicestypes/getsingle?**"

    public static readonly Commodities = "**/commodities"
    public static readonly CommoditiesGetSingle = "**/commodities/getsingle?**"

    public static readonly Regions = "**/regions"
    public static readonly RegionsGetSingle = "**/regions/getsingle?**"

    public static readonly Shippingagents = "**/shippingagents"
    public static readonly ShippingAgentGetSingle = "**/shippingagents/getsingle?**"

    public static readonly CustomAgents = "**/customagents"
    public static readonly CustomAgentsGetSingle = "**/customagents/getsingle?**"

    public static readonly Truckers = "**/truckers"
    public static readonly TruckersGetSingle = "**/truckers/getsingle?**"

    public static readonly MoveTypes = "**/movetypes"
    public static readonly MoveTypeGetSingle = "**/movetypes/getsingle?**"

    public static readonly ShipmentSubTypes = "**/shipmentsubtypes"
    public static readonly ShipmentSubTypesGetSingle = "**/shipmentsubtypes/getsingle?**"

    public static readonly CreditCardTypes = "**/creditcardtypes"
    public static readonly CreditCardTypesGetSingle = "**/creditcardtypes/getsingle?**"

    public static readonly Warehouses = "**/PartnersDomain";
    public static readonly PUTWarehouses = "**/warehouses";
    public static readonly WarehousesGetSingle = "**/warehouses/getsingle?**";
    public static readonly WarehouseviewGetSingle = "**/warehouseviews/getsingle/?**";

    public static readonly PaymentTerms = "**/paymentterms";
    public static readonly PaymentTermsGetSingle = "**/paymentterms/getsingle?**";
    public static readonly PaymentTermsviewGetSingle = "**/paymenttermviews/getsingle/?**";

    public static readonly VatTypes = "**/vattypes";
    public static readonly VatTypesGetSingle = "**/vattypes/getsingle?**";
    public static readonly VatTypesviewGetSingle = "**/vattypeviews/getsingle/?**";

    public static readonly AccountingPaymentMethods = "**/accountingpaymentmethods";
    public static readonly AccountingPaymentMethodsGetSingle = "**/accountingpaymentmethods/getsingle?**";
    public static readonly AccountingPaymentMethodsviewGetSingle = "**/accountingpaymentmethodviews/getsingle/?**";

    public static readonly BusinessUnits = "**/businessunits";
    public static readonly BusinessUnitsGetSingle = "**/businessunits/getsingle?**";
    public static readonly BusinessUnitsviewGetSingle = "**/businessunitviews/getsingle/?**";

    public static readonly Departments = "**/departments";
    public static readonly DepartmentsGetSingle = "**/departments/getsingle?**";
    public static readonly DepartmentsviewGetSingle = "**/departmentviews/getsingle/?**";

    public static readonly OccasionTypes = "**/occasiontypes";
    public static readonly OccasionTypesGetSingle = "**/occasiontypes/getsingle?**";
    public static readonly OccasionTypesviewGetSingle = "**/occasiontypeviews/getsingle/?**";

    public static GetFilterSearch(filterBy: string) {
        return '**/getbyfilters?**' + filterBy + '**'
    }

    public static GetQuickSearch(CustomerNumber: string): string {
        return "**/GetCustomersQuickSearch?**" + CustomerNumber + "**";
    }
}