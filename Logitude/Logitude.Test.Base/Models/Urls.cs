namespace Logitude.Test.Base.Models
{
    public static class Urls
    {
        public static string ShipmentController = "Shipment";
        public static string AddressController = "addresses";
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
        public static string PortViewsGetByFilters = "PortViews/getbyfilters";
        public static string PortViewsGetTenantImportByFilters = "PortViews/getTenantImportByFilters";
        public static string CountryViewsGetByFilters = "countryviews/getbyfilters";
        public static string StateViewsGetByFilters = "stateviews/getbyfilters";
        public static string StatesController = "states";
        public static string IntegrationTestGetBasePartners = "IntegrationTest/GetBasePartners";
        public static string IntegrationTestGetBaseLocations = "IntegrationTest/GetBaseLocations";
        public static string IntegrationTestGetBaseShipment = "IntegrationTest/GetBaseShipment";


        public static string PartnersDomainController = "PartnersDomain";
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


        public static string TenantsGetSingle(int id)
        {
            return "Tenants/GetSingle?id=" + id.ToString();
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

        public static string CommonDomainGetPortCopyToCurrentTenant(string portId)
        {
            return "CommonDomain/GetPortCopyToCurrentTenant?entityId=" + portId;
        }

        public static string PartnersDomainGetCarrierCopyToCurrentTenant(string carrierId)
        {
            return "PartnersDomain/GetCarrierCopyToCurrentTenant?entityId=" + carrierId;
        }
    }
}