using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;
using Logitude.CRM.Data.Repsitories;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityQueryServices;
using Logitude.CRM.BL.DataContracts;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.CRM
{
    public class LogitudeCRMReportService
    {
        private readonly int tenant;
        private readonly IBlobService storageservice;
        private readonly DocumentRepository documentRepository;
        private LogitudeCRMReportDataProvider iDataProvider;
        private LogitudeCRMReportFilter logitudeCRMReportFilter;
        public LogitudeCRMReportService(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;
            storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            documentRepository = new DocumentRepository(tenant);
            MemoryStream memoryStream = new MemoryStream(xmlFilters);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations iQueryOperations = (QueryOperations)xmlSerializer.Deserialize(memoryStream);
            logitudeCRMReportFilter = new LogitudeCRMReportFilter();
            this.BuildFilters(iQueryOperations);
        }

        private void BuildFilters(QueryOperations iQueryOperations)
        {
            SetOppotunityFilter(iQueryOperations);
            SetCustomerStatusFilter(iQueryOperations);
            SetResellerFilter(iQueryOperations);
            SetShowNetFilter(iQueryOperations);
            SetExchangeRateFilter(iQueryOperations);
        }

        private void SetOppotunityFilter(QueryOperations iQueryOperations)
        {
            QueryFilterItem queryFilterItem = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "OpportunityTypes").FirstOrDefault();
            string opportunityTypes = null;
            if (queryFilterItem != null)
            {
                if (queryFilterItem.FieldValue != null)
                {
                    opportunityTypes = queryFilterItem.FieldValue.ToString();
                }
            }
            List<string> myOpportunityTypes = new List<string>();
            if (!string.IsNullOrEmpty(opportunityTypes))
            {
                opportunityTypes = opportunityTypes.Replace(" ", "");
                if (opportunityTypes.ToLower() == "all")
                {
                    OpportunityTypeRepository additionalServiceRepository = new OpportunityTypeRepository(tenant);
                    IQueryable<OpportunityType> iQueryable = additionalServiceRepository.GetAll(tenant);
                    if (iQueryable.Count() > 0)
                    {
                        myOpportunityTypes = iQueryable.Select(s => s.Id).ToList();
                    }
                }
                else
                {
                    opportunityTypes = opportunityTypes.Trim(',');
                    string[] myServices = opportunityTypes.Split(',');
                    myOpportunityTypes = myServices.ToList();
                }
                logitudeCRMReportFilter.OpportunityTypes = myOpportunityTypes;
            }
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
            QueryFilterItem queryFilterItem = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ShowAllRecurringTenants").FirstOrDefault();

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
                logitudeCRMReportFilter.ShowNet = Convert.ToBoolean(queryFilterItem.FieldValue);
            }
        }

        public byte[] GetData()
        {
            this.LoadDataProvider();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(LogitudeCRMReportDataProvider));
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
            this.iDataProvider = new LogitudeCRMReportDataProvider();
            this.BuildReportData();
        }

        private void BuildReportData()
        {
            OpportunityQueryService opportunityQueryService = new OpportunityQueryService(tenant);
            List<OpportunityDetails> opportunityDetails = opportunityQueryService.GetLogitudeOpportunities(tenant, logitudeCRMReportFilter);

        }
    }

}

