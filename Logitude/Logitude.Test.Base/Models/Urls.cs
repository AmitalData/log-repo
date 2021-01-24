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
        public static string IntegrationTestGetBasePartners = "IntegrationTest/GetBasePartners";
        public static string IntegrationTestGetBaseLocations = "IntegrationTest/GetBaseLocations";
        public static string IntegrationTestGetBaseShipment = "IntegrationTest/GetBaseShipment";


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

    }
}