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
        private SharedLogisticReportFilters sharedLogisticReportFilters = null;
        private List<QueryColumnPM> queryColumns = null;
        private SharedLogisticsSetting sharedLogisticsSetting = null;
        private List<QueryFilterItem> queryFilterItems = null;
        private FilterSerializer filterSerializer = new FilterSerializer();
        private int tenant;
        private int tenantZero = 0;
        private string reportExcelFileName = string.Empty;

        public SharedLogisticReportService(SharedLogisticReportFilters sharedLogisticReportFilters , int tenant)
        {
            this.sharedLogisticReportFilters = sharedLogisticReportFilters;
            this.tenant = tenant;
            reportExcelFileName = sharedLogisticReportFilters.ObjectTableName + "_" + sharedLogisticReportFilters.QueryCode + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss");
            queryFilterItems = sharedLogisticReportFilters.QueryFilterItems.Where(d => d.FieldValue != null).ToList();
            sharedLogisticsSetting = GetSharedLogisticsSetting();
            queryColumns = GetQueryColumnsByQueryCode(sharedLogisticReportFilters.QueryCode);
            FilterQueryColumnsBasedOnSharedLogisticsSetting();
        }

  
        public ExportReportResult ExportReportToExcelFileOnStorage()
        {
            ExportToExcelArgs exportToExcelArgs = GetExportToExcelArgs();
            byte[] excelfileData = new ExportToExcelHelper().ExportQueryToExcel(exportToExcelArgs);
            WriteFileOnStorage(excelfileData);
            return new ExportReportResult { FileName = reportExcelFileName };
        }


        private ExportToExcelArgs GetExportToExcelArgs()
        {
            QueryOperations queryOperations = GetNewInStanceFromQueryOperations();
            return new ExportToExcelArgs()
            {
                XmlFilters = filterSerializer.SerializeFilterItems(queryOperations),
                QueryCode = sharedLogisticReportFilters.QueryCode,
                QueryColumns = queryColumns,
                Tenant = tenant,
                UserId = sharedLogisticReportFilters.ContactId,
            };
        }


        private QueryOperations GetNewInStanceFromQueryOperations()
        {
            return new QueryOperations()
            {
                ObjectTableName = sharedLogisticReportFilters.ObjectTableName,
                SortByColumnName = sharedLogisticReportFilters.SortByColumnName,
                SortDirectin = sharedLogisticReportFilters.SortDirectin,
                QuerySection = sharedLogisticReportFilters.QuerySection,
                QueryFilterItems = queryFilterItems,

            };
        }

        private void WriteFileOnStorage(byte[] excelfileData)
        {
            StorageDataArgs storageDataArgs = new StorageDataArgs()
            {
                FileName = reportExcelFileName,
                FolderName = "others",
                Extension = "xls",
                Tenant = tenant,
                FileData = excelfileData
            };

            StorageDataService.WriteFileOnStorage(storageDataArgs);
        }


        private SharedLogisticsSetting GetSharedLogisticsSetting()
        {
            SharedLogisticsSettingRepository sharedLogisticsSettingRepository = new SharedLogisticsSettingRepository(tenant);
            return sharedLogisticsSettingRepository.GetSingle(tenant.ToString(), tenant);
        }


        private List<QueryColumnPM> GetQueryColumnsByQueryCode(string queryCode)
        {
            QueryColumnQuery queryColumnQuery = new QueryColumnQuery(tenant);
            return queryColumnQuery.GetQueryColumnsByQueryCode(tenantZero, queryCode).OrderBy(q => q.IndexOrder).ToList();
        }


        private void FilterQueryColumnsBasedOnSharedLogisticsSetting()
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


   
    }

    public class ExportReportResult
    {
        public string FileName { get; set; }
    }
}