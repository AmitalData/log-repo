using Logitude.CRM.BL.DataContracts;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.CRM.LogitudeCRMReport
{
    public class LogitudeCRMReportFilterService
    {
        private readonly LogitudeCRMReportFilter logitudeCRMReportFilter;
        private readonly byte[] xmlFilters;
        private readonly int tenant;

        public LogitudeCRMReportFilterService(byte[] xmlFilters, int tenant)
        {
            this.xmlFilters = xmlFilters;
            this.tenant = tenant;
            logitudeCRMReportFilter = new LogitudeCRMReportFilter();
        }

        public LogitudeCRMReportFilter Build()
        {
            QueryOperations iQueryOperations = BuildQueryOperation(xmlFilters);
            SetOppotunityFilter(iQueryOperations);
            SetCustomerStatusFilter(iQueryOperations);
            SetResellerFilter(iQueryOperations);
            SetShowNetFilter(iQueryOperations);
            SetExchangeRateFilter(iQueryOperations);
            SetYearFilter(iQueryOperations);
            return logitudeCRMReportFilter;
        }

        private QueryOperations BuildQueryOperation(byte[] xmlFilters)
        {
            MemoryStream memoryStream = new MemoryStream(xmlFilters);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations iQueryOperations = (QueryOperations)xmlSerializer.Deserialize(memoryStream);
            return iQueryOperations;
        }

        private void SetOppotunityFilter(QueryOperations iQueryOperations)
        {
            QueryFilterItem queryFilterItem = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "OpportunityTypes").FirstOrDefault();
            string opportunityTypes = null;
            if (queryFilterItem != null && queryFilterItem.FieldValue != null)
            {
                opportunityTypes = queryFilterItem.FieldValue.ToString();
            }
            logitudeCRMReportFilter.OpportunityTypes = (string.IsNullOrEmpty(opportunityTypes) || opportunityTypes.ToLower() == "all") ? GetOpportunityTypes() : opportunityTypes.Trim(',').Split(',').ToList();
        }

        private List<string> GetOpportunityTypes()
        {
            OpportunityTypeRepository additionalServiceRepository = new OpportunityTypeRepository(tenant);
            IQueryable<OpportunityType> opportunityTypes = additionalServiceRepository.GetAll(tenant);
            return opportunityTypes.Select(s => s.Id).ToList();
        }

        private void SetCustomerStatusFilter(QueryOperations iQueryOperations)
        {
            QueryFilterItem queryFilterItem = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "CustomerStatus").FirstOrDefault();
            if (queryFilterItem != null && queryFilterItem.FieldValue != null)
            {
                logitudeCRMReportFilter.CustomerStatus = queryFilterItem.FieldValue.ToString();
            }
        }

        private void SetResellerFilter(QueryOperations iQueryOperations)
        {
            QueryFilterItem queryFilterItem = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ResellerId").FirstOrDefault();

            if (queryFilterItem != null && queryFilterItem.FieldValue != null)
            {
                logitudeCRMReportFilter.ResellerId = queryFilterItem.FieldValue.ToString();
            }
        }

        private void SetShowNetFilter(QueryOperations iQueryOperations)
        {
            QueryFilterItem queryFilterItem = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ShowNet").FirstOrDefault();

            if (queryFilterItem != null && queryFilterItem.FieldValue != null)
            {
                logitudeCRMReportFilter.ShowNet = Convert.ToBoolean(queryFilterItem.FieldValue);
            }
        }

        private void SetExchangeRateFilter(QueryOperations iQueryOperations)
        {
            QueryFilterItem queryFilterItem = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ExchangeRate").FirstOrDefault();

            if (queryFilterItem != null && queryFilterItem.FieldValue != null)
            {
                logitudeCRMReportFilter.ExchangeRate = decimal.Parse(queryFilterItem.FieldValue.ToString());
            }
        }

        private void SetYearFilter(QueryOperations iQueryOperations)
        {
            QueryFilterItem queryFilterItem = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "Year").FirstOrDefault();
            if (queryFilterItem != null && queryFilterItem.FieldValue != null)
            {
                logitudeCRMReportFilter.Year = int.Parse(queryFilterItem.FieldValue.ToString());
            }
        }

    }
}