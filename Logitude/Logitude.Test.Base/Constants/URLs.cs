namespace Logitude.Test.Base.Constants
{
    public static class URLs
    {
        //public static string UserAuthentication = "Authentication";
        //public static string TenantsGetSingle = "Tenants/GetSingle?id=";
        //public static string UserViewsGetByFilters = "UserViews/GetByFilters";
        //public static string ContactViewsGetByFilters = "ContactViews/GetByFilters";

        public static string UserAuthentication()
        {
            return "Authentication";
        }

        public static string TenantsGetSingle(int id)
        {
            return "Tenants/GetSingle?id=" + id.ToString();
        }

        public static string UserViewsGetByFilters()
        {
            return "UserViews/GetByFilters";
        }

        public static string ContactViewsGetByFilters()
        {
            return "ContactViews/GetByFilters";
        }

        public static string ContactsGetSingle(string id)
        {
            return "Contacts/GetSingle?id=" + id;
        }

        public static string AddressViewsGetByFilters()
        {
            return "AddressViews/GetByFilters";
        }

        public static string AddressesGetSingle(string id)
        {
            return "Addresses/GetSingle?id=" + id;
        }

        public static string ShipmentViewsGetByFilters()
        {
            return "ShipmentViews/GetByFilters";
        }

        public static string ShipmentGetSingle(string id)
        {
            return "Shipment/GetSingle?id=" + id;
        }

        public static string Shipment()
        {
            return "Shipment";
        }

        public static string FTPDetailViewsGetByFilters()
        {
            return "FTPDetailViews/GetByFilters";
        }

        public static string FTPDetailsGetSingle(string id)
        {
            return "FTPDetails/GetSingle?id=" + id;
        }

        public static string APInvoiceViewsGetByFilters()
        {
            return "APInvoiceViews/GetByFilters";
        }

        public static string APInvoicesGetSingle(string id)
        {
            return "APInvoices/GetSingle?id=" + id;
        }

        public static string APPaymentViewsGetByFilters()
        {
            return "APPaymentViews/GetByFilters";
        }

        public static string APPaymentsGetSingle(string id)
        {
            return "APPayments/GetSingle?id=" + id;
        }

        public static string ARInvoiceViewsGetByFilters()
        {
            return "ARInvoiceViews/GetByFilters";
        }

        public static string ARInvoicesGetSingle(string id)
        {
            return "ARInvoices/GetSingle?id=" + id;
        }

        public static string ARPaymentViewsGetByFilters()
        {
            return "ARPaymentViews/GetByFilters";
        }

        public static string ARPaymentsGetSingle(string id)
        {
            return "ARPayments/GetSingle?id=" + id;
        }
    }
}