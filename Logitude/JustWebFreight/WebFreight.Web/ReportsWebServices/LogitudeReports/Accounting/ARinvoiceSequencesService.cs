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
    public class ARinvoiceSequencesService
    {
        int tenant;
        DateTime fromDate;
        DateTime toDate;
        string vendorId;
        ARinvoiceSequencesReportParameters aRinvoiceSequencesReportParameters;
        public ARinvoiceSequencesService(byte[] xmlFilters, int Tenant)
        {
            this.tenant = Tenant;
            aRinvoiceSequencesReportParameters = new ARinvoiceSequencesReportParameters();
            MemoryStream memoryStream = new MemoryStream(xmlFilters);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations iQueryOperations = (QueryOperations)xmlSerializer.Deserialize(memoryStream);
            this.BuildReportFilters(iQueryOperations);


        }

        private void BuildReportFilters(QueryOperations queryOperations)
        {
            QueryFilterItem filterItem_FromDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime myStartDate = todayDate.AddMonths(-1);

            fromDate = new DateTime(myStartDate.Year, myStartDate.Month, 1);
            toDate = new DateTime(todayDate.Year, todayDate.Month, DateTime.DaysInMonth(todayDate.Year, todayDate.Month));

            if (filterItem_FromDate != null)
            {
                DateTime.TryParse(filterItem_FromDate.FieldValue.ToString(), out fromDate);
                aRinvoiceSequencesReportParameters.FromDate = fromDate;
            }

            if (filterItem_ToDate != null)
            {
                DateTime.TryParse(filterItem_ToDate.FieldValue.ToString(), out toDate);
                aRinvoiceSequencesReportParameters.ToDate = toDate;
            }

        }

        public byte[] GetData()
        {
            ARinvoiceSequencesReportData dataProvider = BuildDataProvider();
            return new ReportMemoryStreamService().Convert(dataProvider, typeof(ARinvoiceSequencesReportData), tenant);
        }

        private ARinvoiceSequencesReportData BuildDataProvider()
        {
            ARinvoiceSequencesReportData deductionReportPerVendorDataProvider = new ARinvoiceSequencesReportData();
            ARinvoiceSequencesReportDataProvider deductionReportDataProvider = new ARinvoiceSequencesReportDataProvider(tenant, aRinvoiceSequencesReportParameters);

            deductionReportPerVendorDataProvider = deductionReportDataProvider.GetARinvoiceSequencesReportData();



            return deductionReportPerVendorDataProvider;
        }
    }
}