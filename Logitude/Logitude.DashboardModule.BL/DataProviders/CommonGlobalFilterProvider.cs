using System;

namespace Logitude.DashboardModule.BL.DataProviders
{
    public static class CommonGlobalFilterProvider
    {
        const string ShipmentAnalyticsMetaData = "ShipmentAnalytics";
        const string APInvoiceAnalyticsMetaData = "APInvoiceAnalytics";
        const string ARInvoiceAnalyticMetaData = "ARInvoiceAnalytics";
        const string QuoteAnalyticMetaData = "QuoteAnalytics";
        const string OpportunityAnalyticMetaData = "OpportunityAnalytics";

        public static CommonGlobalFilterItem GetFilterItem(string FieldName, string tableName)
        {
            switch (tableName)
            {
                case ShipmentAnalyticsMetaData: return GetShipmentCommonGlobalFilterItem(FieldName);
                case APInvoiceAnalyticsMetaData: return GetAPInvoiceCommonGlobalFilterItem(FieldName);
                case ARInvoiceAnalyticMetaData: return GetARInvoiceCommonGlobalFilterItem(FieldName);
                case QuoteAnalyticMetaData: return GetQuoteCommonGlobalFilterItem(FieldName);
                case OpportunityAnalyticMetaData: return GetOpportunityCommonGlobalFilterItem(FieldName);
                default: throw new Exception($"Meta Data Name {tableName} not Provided Common Filters");
            }
        }

        private static CommonGlobalFilterItem GetOpportunityCommonGlobalFilterItem(string fieldName)
        {
            switch (fieldName)
            {
                case "CreateDate": return new CommonGlobalFilterItem("CreateDate", "DateTime");
                default: return null;
            }
        }

        private static CommonGlobalFilterItem GetQuoteCommonGlobalFilterItem(string fieldName)
        {
            switch (fieldName)
            {
                case "CreateDate": return new CommonGlobalFilterItem("OpenDate", "DateTime");
                case "Number": return new CommonGlobalFilterItem("QuoteNumber", "Text");
                default: return null;
            }
        }

        private static CommonGlobalFilterItem GetARInvoiceCommonGlobalFilterItem(string fieldName)
        {
            switch (fieldName)
            {
                case "CreateDate": return new CommonGlobalFilterItem("CreateDate", "DateTime");
                case "Number": return new CommonGlobalFilterItem("InvoiceNumber", "Text");
                default: return null;
            }
        }

        private static CommonGlobalFilterItem GetAPInvoiceCommonGlobalFilterItem(string fieldName)
        {
            switch (fieldName)
            {
                case "CreateDate": return new CommonGlobalFilterItem("CreateDate", "DateTime");
                case "Number": return new CommonGlobalFilterItem("InvoiceNumber", "Text");
                default: return null;
            }
        }

        private static CommonGlobalFilterItem GetShipmentCommonGlobalFilterItem(string fieldName)
        {
            switch (fieldName)
            {
                case "CreateDate": return new CommonGlobalFilterItem("CreateDateTime", "DateTime");
                case "Number": return new CommonGlobalFilterItem("ShipmentNumber", "Text");
                default: return null;
            }
        }

        public class CommonGlobalFilterItem
        {
            public CommonGlobalFilterItem(string name, string type)
            {
                Name = name;
                Type = type;
            }

            public string Name { get; set; }
            public string Type { get; set; }
        }
    }
}
