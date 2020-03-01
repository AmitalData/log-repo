using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.CRM
{
    public class MonthlyConversionManager
    {
        private int tenant;

        private DateTime fromDate;
        private DateTime toDate;
        private bool isByCreateDate = true;
        private string opportunityTypeCode = null;
        private string countryId = null;
        private string ownerId = null;
        private string businessUnitId = null;
        private string leadSources = null;
        private string resellerId = null;

        public MonthlyConversionManager(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;

            MemoryStream memoryStream = new MemoryStream(xmlFilters);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)xmlSerializer.Deserialize(memoryStream);

            QueryFilterItem filterItem_IsByCreateDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "IsByCreateDate").FirstOrDefault();
            QueryFilterItem filterItem_FromDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
            QueryFilterItem filterItem_DataType = queryOperations.QueryFilterItems.Where(d => d.FieldName == "DataType").FirstOrDefault();
            QueryFilterItem filterItem_CountryId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CountryId").FirstOrDefault();
            QueryFilterItem filterItem_OwnerId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "OwnerId").FirstOrDefault();
            QueryFilterItem filterItem_BusinessUnitId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "BusinessUnitId").FirstOrDefault();
            QueryFilterItem filterItem_LeadSources = queryOperations.QueryFilterItems.Where(d => d.FieldName == "LeadSources").FirstOrDefault();
            QueryFilterItem filterItem_Reseller = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ResellerId").FirstOrDefault();

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            fromDate = todayDate;
            toDate = todayDate;
            
            if (filterItem_IsByCreateDate != null)
            {
                if (filterItem_IsByCreateDate.FieldValue != null)
                {
                    isByCreateDate = (bool)filterItem_IsByCreateDate.FieldValue;
                }
            }

            if (filterItem_FromDate != null)
            {
                DateTime.TryParse(filterItem_FromDate.FieldValue.ToString(), out fromDate);

                if (filterItem_ToDate != null)
                {
                    DateTime.TryParse(filterItem_ToDate.FieldValue.ToString(), out toDate);
                }
            }
            
            if (filterItem_DataType != null)
            {
                if (filterItem_DataType.FieldValue != null)
                {
                    opportunityTypeCode = filterItem_DataType.FieldValue.ToString();
                }
            }
            
            if (filterItem_CountryId != null)
            {
                if (filterItem_CountryId.FieldValue != null)
                {
                    countryId = filterItem_CountryId.FieldValue.ToString();
                }
            }
            
            if (filterItem_OwnerId != null)
            {
                if (filterItem_OwnerId.FieldValue != null)
                {
                    ownerId = filterItem_OwnerId.FieldValue.ToString();
                }
            }
            
            if (filterItem_BusinessUnitId != null)
            {
                if (filterItem_BusinessUnitId.FieldValue != null)
                {
                    businessUnitId = filterItem_BusinessUnitId.FieldValue.ToString();
                }
            }
            
            if (filterItem_LeadSources != null)
            {
                if (filterItem_LeadSources.FieldValue != null)
                {
                    leadSources = filterItem_LeadSources.FieldValue.ToString();
                }
            }

            List<string> myLeadSourcesList = new List<string>();
            if (!string.IsNullOrEmpty(leadSources))
            {
                leadSources = leadSources.Replace(" ", "");

                if (leadSources.ToLower() == "all")
                {
                    //LeadSourceRepository leadSourceRepository = new LeadSourceRepository(tenant);
                    //IQueryable<LeadSource> iQueryable = leadSourceRepository.GetLeadSources(tenant).Where(d => !d.InActive);
                    //if (iQueryable.Count() > 0)
                    //{
                    //    myLeadSourcesList = iQueryable.Select(s => s.Id).ToList();
                    //}
                }

                else
                {
                    leadSources = leadSources.Trim(',');
                    string[] myLeadSources = leadSources.Split(',');
                    myLeadSourcesList = myLeadSources.ToList();
                }
            }

            
            if (filterItem_Reseller != null)
            {
                if (filterItem_Reseller.FieldValue != null)
                {
                    resellerId = filterItem_Reseller.FieldValue.ToString();
                }
            }
        }

        public byte[] GetData()
        {
            OpportunityMonthlyConversionDataProvider myDataProvider = new OpportunityMonthlyConversionDataProvider();

            myDataProvider = this.LoadDataProvider();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(OpportunityMonthlyConversionDataProvider));
            MemoryStream memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream, myDataProvider);
            memoryStream.Seek(0, SeekOrigin.Begin);
            StreamReader streamReader = new StreamReader(memoryStream);
            string content = streamReader.ReadToEnd();
            byte[] bytearray = memoryStream.ToArray();
            return bytearray;
        }

        private OpportunityMonthlyConversionDataProvider LoadDataProvider()
        {
            OpportunityMonthlyConversionDataProvider myDataProvider = new OpportunityMonthlyConversionDataProvider();
            






            return myDataProvider;
        }
    }
}