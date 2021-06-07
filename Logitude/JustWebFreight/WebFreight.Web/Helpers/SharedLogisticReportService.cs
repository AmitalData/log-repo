using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityQueryServices;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.Repsitories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Controllers.ShardLogistics;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.Helpers
{
    public class SharedLogisticReportService
    {
        private QueryOperations queryOperations = null;
        private SharedLogisticReportFilters sharedLogisticReportFilters = null;
        private List<QueryColumnPM> queryColumns = null;
        private SharedLogisticsSetting sharedLogisticsSetting = null;
        private FilterSerializer filterSerializer = new FilterSerializer();
        public int tenant;

        public SharedLogisticReportService(SharedLogisticReportFilters sharedLogisticReportFilters , int tenant)
        {
            sharedLogisticReportFilters.PartnerId = "1-11143";
            this.sharedLogisticReportFilters = sharedLogisticReportFilters;
            this.tenant = tenant;
            sharedLogisticsSetting = GetSharedLogisticsSetting();
            queryColumns = GetQueryColumnsByQueryCode(sharedLogisticReportFilters, tenant);
            FilterQueryColumnsBySharedLogisticsSetting();
        }


   
        public void ExportReportToExcelFile()
        {
            queryOperations = GetNewInStanceFromQueryOperations();
            ExportToExcelArgs exportToExcelArgs = GetExportToExcelArgs();
            byte[] excelfileData = new ExportToExcelHelper().ExportQueryToExcel(exportToExcelArgs);
            StorageDataService.WriteFileOnStorage(new StorageDataArgs() { FileName = sharedLogisticReportFilters.ReportName, FolderName = "others", Extension = "xls", Tenant = tenant, FileData = excelfileData });

        }



        private SharedLogisticsSetting GetSharedLogisticsSetting()
        {
            SharedLogisticsSettingRepository sharedLogisticsSettingRepository = new SharedLogisticsSettingRepository(tenant);
            return sharedLogisticsSettingRepository.GetSingle(tenant.ToString(), tenant);
        }


        private List<QueryColumnPM> GetQueryColumnsByQueryCode(SharedLogisticReportFilters sharedLogisticReportFilters, int tenant)
        {
            QueryColumnQuery queryColumnQuery = new QueryColumnQuery(tenant);
            return queryColumnQuery.GetQueryColumnsByQueryCode(0, sharedLogisticReportFilters.QueryCode).ToList();
        }


        private void FilterQueryColumnsBySharedLogisticsSetting()
        {
            if (!sharedLogisticsSetting.IsShipperShared)
            {
                queryColumns = queryColumns.Where(d => d.ObjectFieldName != "ShipperId").ToList();
            }

            if (!sharedLogisticsSetting.IsConsigneeShared)
            {
                queryColumns = queryColumns.Where(d => d.ObjectFieldCode != "ConsigneeId").ToList();
            }

        }


        private ExportToExcelArgs GetExportToExcelArgs()
        {

            return new ExportToExcelArgs() {
               XmlFilters = filterSerializer.SerializeFilterItems(queryOperations),
               QueryCode = sharedLogisticReportFilters.QueryCode, 
               QueryColumns = queryColumns,
               Tenant = tenant,
               UserId = sharedLogisticReportFilters.ContactId, 
           };
        }

        private QueryOperations GetNewInStanceFromQueryOperations()
        {
            return queryOperations = new QueryOperations()
            {
                ObjectTableName = sharedLogisticReportFilters.ObjectTableName,
                SortByColumnName = sharedLogisticReportFilters.SortByColumnName,
                SortDirectin = sharedLogisticReportFilters.SortDirectin,
                QuerySection = sharedLogisticReportFilters.QuerySection,
                QueryFilterItems = sharedLogisticReportFilters.QueryFilterItems,

            };
        }
    }
}