using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using WebFreight.Web.Helpers;

namespace Logitude.BL.CommonDataModel.LogitudeGridExportToExcel
{
    public class LogitudeGridExportToExcelHelper
    {

        public string ExportDataToExcel(LogitudeGridExportToExcelArguments logitudeGridExportToExcelArguments)
        {
            QueryOperations queryOperations = GetQueryOperationsByLogitudeGridArguments(logitudeGridExportToExcelArguments);
            SetFiltersForQueryOperations(queryOperations, logitudeGridExportToExcelArguments);
            byte[] ExcelDataByte = GetExportDateToExcel(queryOperations, logitudeGridExportToExcelArguments);
            string FileName= WriteDataOnStorageServiceByBlobFileInfo(logitudeGridExportToExcelArguments, ExcelDataByte);
            return FileName;
        }
 
        private string WriteDataOnStorageServiceByBlobFileInfo(LogitudeGridExportToExcelArguments logitudeGridExportToExcelArguments, byte[] ExcelDataByte)
        {
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = logitudeGridExportToExcelArguments.ObjectTableName + DateTime.Now.ToShortDateString(),
                FolderName = "others",
                Extension = logitudeGridExportToExcelArguments.IsXslxFormat ? "xlsx" : "xls",//fileparams[1],
                Tenant = logitudeGridExportToExcelArguments.Tenant,
                FileSize = ExcelDataByte.Length,

            };
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            storageservice.Write(ExcelDataByte, fileInfo);

            return fileInfo.FileName;
        }
        private QueryOperations GetQueryOperationsByLogitudeGridArguments(LogitudeGridExportToExcelArguments logitudeGridExportToExcelArguments)
        {
            QueryOperations queryOperations = new QueryOperations()
            {
                ObjectTableName = logitudeGridExportToExcelArguments.ObjectTableName,
                PageIndex = logitudeGridExportToExcelArguments.PageIndex,
                PageSize = logitudeGridExportToExcelArguments.PageSize,
                SortByColumnName = logitudeGridExportToExcelArguments.SortBy,
                SortDirectin = logitudeGridExportToExcelArguments.SortDirection,
                QuerySection = logitudeGridExportToExcelArguments.QuerySection,
                GetAll = true,

            };

            return queryOperations;
        }

        private byte[] GetExportDateToExcel(QueryOperations queryOperations, LogitudeGridExportToExcelArguments logitudeGridExportToExcelArguments)
        {
            FilterSerializer serializer = new FilterSerializer();
            byte[] arrayOfBytes = serializer.SerializeFilterItems(queryOperations);
            QueryPM queryPM = new QueryPM()
            {
                QuerySection =
                    logitudeGridExportToExcelArguments.ObjectTableName,
                ObjectTableName =
                    logitudeGridExportToExcelArguments.ObjectTableName,
                DisplayText =
                    logitudeGridExportToExcelArguments.QueryName
            };
            ExportToExcelArgs exportToExcelArgs = new ExportToExcelArgs()
            {

                XmlFilters = arrayOfBytes,
                QueryCode = null,
                Tenant = logitudeGridExportToExcelArguments.Tenant,
                UserId = logitudeGridExportToExcelArguments.UserId,
                TypeName = null,
                QueryColumns =
                logitudeGridExportToExcelArguments.QueryColumns,
                QueryPM = queryPM,
                IsXslxFormat = logitudeGridExportToExcelArguments.IsXslxFormat,
            };
            var data = new ExportToExcelHelper().ExportQueryToExcel(exportToExcelArgs);

            return data;

        }

        private void SetFiltersForQueryOperations(QueryOperations queryOperations, LogitudeGridExportToExcelArguments logitudeGridExportToExcelArguments)
        {
        


            List<ObjectField> ObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName(queryOperations.ObjectTableName, logitudeGridExportToExcelArguments.Tenant);
            foreach (QueryFilterItem filter in logitudeGridExportToExcelArguments.AdditionalFilters)
            {
                ObjectField field = ObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);
                if (field != null)
                {
                    string valuestring1 = filter.FieldValue != null ? filter.FieldValue.ToString() : null;
                    valuestring1 = HttpUtility.UrlDecode(valuestring1);
                    object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                    string valuestring2 = filter.FieldValue2 != null ? filter.FieldValue2.ToString() : null;
                    valuestring2 = HttpUtility.UrlDecode(valuestring2);
                    object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);
                    queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList, field.IsCustom, field.DataTypeCode);
                }
                else
                {
                    queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                }
            }

        }
    }


}
