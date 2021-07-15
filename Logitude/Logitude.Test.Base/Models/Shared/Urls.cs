namespace Logitude.Test.Base.Models.Shared
{
    public static class Urls
    {
        public static string ShipmentController = "Shipment";
        public static string DirectController = "Direct";
        public static string AddressController = "Addresses";
        public static string StatesController = "States";
        public static string CountriesController = "Countries";
        public static string AuthenticationController = "Authentication";
        public static string UserViewsGetByFilters = "UserViews/GetByFilters";
        public static string ContactViewsGetByFilters = "ContactViews/GetByFilters";
        public static string AddressViewsGetByFilters = "AddressViews/GetByFilters";
        public static string ShipmentViewsGetByFilters = "ShipmentViews/GetByFilters";
        public static string FTPDetailViewsGetByFilters = "FTPDetailViews/GetByFilters";
        public static string APInvoiceViewsGetByFilters = "APInvoiceViews/GetByFilters";
        public static string APPaymentViewsGetByFilters = "APPaymentViews/GetByFilters";
        public static string ARInvoiceViewsGetByFilters = "ARInvoiceViews/GetByFilters";
        public static string ARPaymentViewsGetByFilters = "ARPaymentViews/GetByFilters";
        public static string PortViewsGetByFilters = "PortViews/GetByFilters";
        public static string PortViewsGetTenantImportByFilters = "PortViews/GetTenantImportByFilters";
        public static string CountryViewsGetByFilters = "CountryViews/GetByFilters";
        public static string GlobalZoneViewsGetByFilters = "GlobalZoneViews/GetByFilters";
        public static string StateViewsGetByFilters = "StateViews/GetByFilters";
        public static string OpportunityTypeViewsGetByFilters = "OpportunityTypeViews/GetByFilters";
        public static string StageViewsGetByFilters = "StageViews/GetByFilters";

        public static string SpecialServicesTypesController = "SpecialServicesTypes";
        public static string SpecialServicesTypeViewsGetByFilters = "SpecialServicesTypeViews/GetByFilters";

        public static string QuoteController = "Quotes";
        public static string QuoteViewsGetByFilters = "Quoteviews/Getbyfilters";

        public static string CrossDockController = "warehouseentries";
        public static string CrossReleaseGetController = "warehousereleases";
        public static string CrossReleaseController = "WarehouseReleaseExtended/postwarehousereleasepm";
        public static string ActivitiesController = "Activities";
        public static string OpportunitiesController = "Opportunities";
        //public static string QuotesGetSingle(string id)
        //{
        //    return "Quotes/GetSingle?id=" + id;
        //}

        public static string APInvoicesController = "APInvoices";
        public static string ARInvoicesController = "ARInvoices";
        //public static string APInvoiceViewsGetByFilters = "APInvoiceViews/getbyfilters";
        public static string CargoTrackingSearchController = "CargoTrackingSearch";



        #region Shipment Prepare Data URls
        //locations
        public static string VesselsController = "Vessels";
        public static string IncotermsController = "Incoterms";
        public static string CreditCardController = "creditcardtypes";
        public static string MoveTypesController = "MoveTypes";
        public static string PackageTypesController = "PackageTypes";
        public static string ShipmentSubTypesController = "ShipmentSubTypes";
        public static string CurrencyViewsGetByFilters = "CurrencyViews/GetByFilters";
        public static string IncotermViewsGetByFilters = "IncotermViews/GetByFilters";
        public static string CreditCardTypeViewsGetByFilters = "CreditCardTypeViews/GetByFilters";
        public static string MeasurementViewsGetByFilters = "MeasurementViews/GetByFilters";
        public static string ChargeTypeViewsGetByFilters = "ChargesTypeViews/GetByFilters";
        public static string PackageTypeViewsGetByFilters = "PackageTypeViews/GetByFilters";
        public static string PaymentTermViewsGetByFilters = "PaymentTermViews/GetByFilters";
        public static string VatTypeViewsGetByFilters = "VatTypeViews/GetByFilters";
        public static string QuoteStageViewsGetByFilters = "QuoteStageViews/GetByFilters";
        public static string VesselViewsGetByFilters = "VesselViews/GetByFilters";
        public static string MoveTypeViewsGetByFilters = "MoveTypeViews/GetByFilters";
        public static string ShipmentSubTypeViewsGetByFilters = "ShipmentSubTypeViews/GetByFilters";

        //partners
        public static string PartnersDomainController = "PartnersDomain/PostPartnerAddress";
        public static string VendorViewsGetByFilters = "VendorViews/GetByFilters";
        public static string AgentViewsGetByFilters = "AgentViews/GetByFilters";
        public static string CustomerViewsGetByFilters = "CustomerViews/GetByFilters";
        public static string CustomAgentViewsGetByFilters = "CustomAgentViews/GetByFilters";
        public static string ShippingAgentViewsGetByFilters = "ShippingAgentViews/GetByFilters";
        public static string TruckerViewsGetByFilters = "TruckerViews/GetByFilters";
        public static string AirlineViewsGetByFilters = "AirlineViews/GetByFilters";
        public static string ShippingLineViewsGetByFilters = "ShippingLineViews/GetByFilters";
        public static string WarehouseViewsGetByFilters = "WarehouseViews/GetByFilters";
        public static string CarrierViewsGetTenantImportByFilters = "CarrierViews/GetTenantImportByFilters";
        public static string CountryCityViewsGetByFilters = "CountryCityViews/GetByFilters";
        public static string CountryCities = "CountryCities";
        public static string ChargesTypes = "ChargesTypes";
        public static string Airlines = "airlines";
        #endregion

        public static string TenantsGetSingle(int id)
        {
            return "Tenants/GetSingle?id=" + id.ToString();
        }

        public static string TenantsUpdate(int id)
        {
            return "Tenants/" + id.ToString();
        }

        public static string ContactsGetSingle(string id)
        {
            return "Contacts/GetSingle?id=" + id;
        }

        public static string AddressesGetSingle(string id)
        {
            return "Addresses/GetSingle?id=" + id;
        }

        public static string ShipmentGetSingle(string id)
        {
            return "Shipment/GetSingle?id=" + id;
        }

        public static string FTPDetailsGetSingle(string id)
        {
            return "FTPDetails/GetSingle?id=" + id;
        }

        public static string APInvoicesGetSingle(string id)
        {
            return "APInvoices/GetSingle?id=" + id;
        }

        public static string APPaymentsGetSingle(string id)
        {
            return "APPayments/GetSingle?id=" + id;
        }

        public static string ARInvoicesGetSingle(string id)
        {
            return "ARInvoices/GetSingle?id=" + id;
        }

        public static string ARPaymentsGetSingle(string id)
        {
            return "ARPayments/GetSingle?id=" + id;
        }

        public static string AirlineGetSingle(string id)
        {
            return "airlines/getsingle?id=" + id;
        }

        public static string CommonDomainGetPortCopyToCurrentTenant(string portId)
        {
            return "CommonDomain/GetPortCopyToCurrentTenant?entityId=" + portId;
        }

        public static string PartnersDomainGetCarrierCopyToCurrentTenant(string carrierId)
        {
            return "PartnersDomain/GetCarrierCopyToCurrentTenant?entityId=" + carrierId;
        }

        public static string CommonDomainGetCopyCurrencyToTenant(string currencyId)
        {
            return "CommonDomain/GetCopyCurrencyToTenant?currencyId=" + currencyId + "&CurrencyRate=4&RateDate=2019-6-24%2015:2:53.564";
        }

        public static string QuoteGetSingle(string id)
        {
            return "Quotes/GetSingle?id=" + id;
        }
        public static string QuoteGetSingleList(string id)
        {
            return "Quoteviews/getsingle/?id=" + id;
        }

        public static string CrossDockGetSingle(string id)
        {
            return "warehouseentries/GetSingle?id=" + id;
        }

        public static string CargoTrackingShipmentGetSingleList(string securityKey, int tenant)
        {
            return "CargoTrackingSearch/GetSingleShipmentList?SecurityKey=" + securityKey + "&tenant=" + tenant;
        }

        public static string CrossDockReleaseGetSingle(string id)
        {
            return "warehousereleases/getsingle?id=" + id;
        }

        public static string VesselGetSingle(string id)
        {
            return "vessels/getsingle?id=" + id;
        }

        public static string ActivitySingle(string id)
        {
            return "activities/GetSingle?id=" + id;
        }

    }
}