using Logitude.Accounting.BL.DataContract;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.DataProviders;
using WebFreight.Web.Services;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.Accounting
{
    public class TaxDeductionReportPerVendorService
    {
        int tenant;
        DateTime fromDate;
        DateTime toDate;
        string vendorId;
        TaxDeductionPerVendorReportParameters taxDeductionPerVendorReportParameters;
        public TaxDeductionReportPerVendorService(byte[] xmlFilters, int Tenant)
        {
            this.tenant = Tenant;
            // this.aRInvoiceDepositDataProvider = new ARInvoiceDepositDataProvider();
            taxDeductionPerVendorReportParameters = new TaxDeductionPerVendorReportParameters();
            MemoryStream memoryStream = new MemoryStream(xmlFilters);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations iQueryOperations = (QueryOperations)xmlSerializer.Deserialize(memoryStream);
            this.BuildReportFilters(iQueryOperations);


        }

        private void BuildReportFilters(QueryOperations queryOperations)
        {
            QueryFilterItem filterItem_FromDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
            QueryFilterItem filterItem_VendorId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "Vendor").FirstOrDefault();
            QueryFilterItem filterItem_CardId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CardId").FirstOrDefault();

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime myStartDate = todayDate.AddMonths(-1);

            fromDate = new DateTime(myStartDate.Year, myStartDate.Month, 1);
            toDate = new DateTime(todayDate.Year, todayDate.Month, DateTime.DaysInMonth(todayDate.Year, todayDate.Month));

            if (filterItem_FromDate != null)
            {
                DateTime.TryParse(filterItem_FromDate.FieldValue.ToString(), out fromDate);
                taxDeductionPerVendorReportParameters.FromDate = fromDate;
            }

            if (filterItem_ToDate != null)
            {
                DateTime.TryParse(filterItem_ToDate.FieldValue.ToString(), out toDate);
                taxDeductionPerVendorReportParameters.ToDate = toDate;
            }

            if (filterItem_VendorId != null)
            {
                if (filterItem_VendorId.FieldValue != null)
                {
                    taxDeductionPerVendorReportParameters.VendorId = filterItem_VendorId.FieldValue.ToString();
                }
            }

            if (filterItem_CardId != null)
            {
                if (filterItem_CardId.FieldValue != null)
                {
                    taxDeductionPerVendorReportParameters.CardId = filterItem_CardId.FieldValue.ToString();
                }
            }

        }

        public byte[] GetData()
        {
            TaxDeductionReportData dataProvider = BuildDataProvider();
            return new ReportMemoryStreamService().Convert(dataProvider, typeof(TaxDeductionReportData), tenant);
        }

        private TaxDeductionReportData BuildDataProvider()
        {
            TaxDeductionReportData deductionReportPerVendorDataProvider = new TaxDeductionReportData();
            TaxDeductionReportDataProvider deductionReportDataProvider = new TaxDeductionReportDataProvider(new TaxDeductionReportPM() , tenant, taxDeductionPerVendorReportParameters);

            deductionReportPerVendorDataProvider = deductionReportDataProvider.GetTaxDeductionReportData();



            return deductionReportPerVendorDataProvider;
        }
    }
}