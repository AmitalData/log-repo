using Logitude.BL.QuoteModel.DataContracts;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;


namespace WebFreight.Web.ReportsWebServices.LogitudeReports.Quotes.SpotRate
{
    public class SpotRateQuoteReportFilterService
    {
        private readonly SpotRateQuoteReportFilter spotRateQuoteReportFilter;
        private readonly byte[] xmlFilters;
        private readonly int tenant;
        private QueryOperations iQueryOperations;

        public SpotRateQuoteReportFilterService(byte[] xmlFilters, int tenant)
        {
            this.xmlFilters = xmlFilters;
            this.tenant = tenant;
            spotRateQuoteReportFilter = new SpotRateQuoteReportFilter();
        }
        public SpotRateQuoteReportFilter Build()
        {
            this.iQueryOperations = BuildQueryOperation(xmlFilters);
            SetCustomerFilter();
            SetSalesmanFilter();
            SetOpenDateFilter();
            SetExpirationDateFilter();
            return spotRateQuoteReportFilter;
        }

        private QueryOperations BuildQueryOperation(byte[] xmlFilters)
        {
            MemoryStream memoryStream = new MemoryStream(xmlFilters);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(QueryOperations));
            return (QueryOperations)xmlSerializer.Deserialize(memoryStream);
        }


        private void SetCustomerFilter()
        {
            QueryFilterItem queryFilterItem = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "CustomerId").FirstOrDefault();
            if (queryFilterItem != null && queryFilterItem.FieldValue != null) spotRateQuoteReportFilter.CustomerId = queryFilterItem.FieldValue.ToString();
        }

        private void SetSalesmanFilter()
        {
            QueryFilterItem queryFilterItem = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "SalesmanId").FirstOrDefault();
            if (queryFilterItem != null && queryFilterItem.FieldValue != null) spotRateQuoteReportFilter.SalesmanId = queryFilterItem.FieldValue.ToString();
        }

        private void SetOpenDateFilter()
        {
            QueryFilterItem queryFilterItem = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "OpenDateGraterThan").FirstOrDefault();
            if (queryFilterItem != null && queryFilterItem.FieldValue != null) spotRateQuoteReportFilter.OpenDateGraterThan = (DateTime)queryFilterItem.FieldValue;
        }

        private void SetExpirationDateFilter()
        {
            QueryFilterItem queryFilterItem = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ExpirationDateLessThan").FirstOrDefault();
            if (queryFilterItem != null && queryFilterItem.FieldValue != null) spotRateQuoteReportFilter.ExpirationDateLessThan = (DateTime)queryFilterItem.FieldValue;
        }
    }

}