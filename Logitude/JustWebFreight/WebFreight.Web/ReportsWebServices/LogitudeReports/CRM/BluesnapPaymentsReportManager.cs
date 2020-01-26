using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.Bluesnap
{
    public class BluesnapPaymentsReportManager
    {
        private int tenant;
        private DateTime? fromDate = null;
        private DateTime? toDate = null;
        private bool showAllRecurringTenants = false;
        private BluesnapPaymentsDataProvider iDataProvider;

        public BluesnapPaymentsReportManager(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;
            MemoryStream memoryStream = new MemoryStream(xmlFilters);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations iQueryOperations = (QueryOperations)xmlSerializer.Deserialize(memoryStream);
            this.FilterByDates(iQueryOperations);
            this.FilterByShowAllRecurringTenants(iQueryOperations);
        }

        private void FilterByDates(QueryOperations iQueryOperations)
        {
            QueryFilterItem filterItem_FromDate = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
            if (filterItem_FromDate != null)
            {
                if (filterItem_FromDate.FieldValue != null)
                {
                    fromDate = (DateTime)filterItem_FromDate.FieldValue;
                }
            }
            if (filterItem_ToDate != null)
            {
                if (filterItem_ToDate.FieldValue != null)
                {
                    toDate = (DateTime)filterItem_ToDate.FieldValue;
                }
            }
        }

        private void FilterByShowAllRecurringTenants(QueryOperations iQueryOperations)
        {
            QueryFilterItem filterItem_ShowAllRecurringTenants = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ShowAllRecurringTenants").FirstOrDefault();

            if (filterItem_ShowAllRecurringTenants != null)
            {
                if (filterItem_ShowAllRecurringTenants.FieldValue != null)
                {
                    showAllRecurringTenants = Convert.ToBoolean(filterItem_ShowAllRecurringTenants.FieldValue);
                }
            }
        }

        public byte[] GetData()
        {
            this.LoadDataProvider();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(BluesnapPaymentsDataProvider));
            MemoryStream memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream, iDataProvider);
            memoryStream.Seek(0, SeekOrigin.Begin);
            StreamReader streamReader = new StreamReader(memoryStream);
            string content = streamReader.ReadToEnd();
            byte[] bytearray = memoryStream.ToArray();
            return bytearray;
        }

        private void LoadDataProvider()
        {
            this.iDataProvider = new BluesnapPaymentsDataProvider()
            {
                BlusnapTransactionsList = new List<BlusnapTransactionsList>(),
            };

            this.BuildReportHeader();
            this.BuildSourceData();
            this.BuildReportData();
        }

        private void BuildReportHeader()
        {
            iDataProvider.FromDate = this.fromDate;
            iDataProvider.ToDate = this.toDate;
            iDataProvider.ShowAllRecurringTenants = this.showAllRecurringTenants;
        }

        private void BuildSourceData()
        {


        }

        private void BuildReportData()
        {

        }
    }
}