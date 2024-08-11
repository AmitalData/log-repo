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
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.ShipmentsModel.Extended
{
    public class LogboxShipmentExportExcelController : ApiController
    {
        public HttpResponseMessage PostGetQueryToExcelData(LogboxShipmentExportExcelArgs logboxShipmentExportExcelArgs)
        {
            int tenant = (int)logboxShipmentExportExcelArgs.Tenant;
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);
            string userid = logboxShipmentExportExcelArgs.UserId;
            string ObjectTableName = logboxShipmentExportExcelArgs.ObjectTableName;

            QueryOperations queryOperations = new QueryOperations()
            {
                ObjectTableName = ObjectTableName,
                PageIndex = logboxShipmentExportExcelArgs.PageIndex,
                PageSize = logboxShipmentExportExcelArgs.PageSize,
                SortByColumnName = logboxShipmentExportExcelArgs.SortBy,
                SortDirectin = logboxShipmentExportExcelArgs.SortDirection,
                QuerySection = logboxShipmentExportExcelArgs.QuerySection,

            };

            List<ObjectField> ObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName(ObjectTableName, tenant);


            JavaScriptSerializer JsonConvert = new JavaScriptSerializer();

            foreach (QueryFilterItem filter in logboxShipmentExportExcelArgs.AdditionalFilters)
            {
                ObjectField field = ObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);
                if (field != null)
                {
                    string valuestring1 = filter.FieldValue != null ? filter.FieldValue.ToString() : null;
                    object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                    string valuestring2 = filter.FieldValue2 != null ? filter.FieldValue2.ToString() : null;
                    object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);
                    queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList, field.IsCustom, field.DataTypeCode);
                }
                else
                {
                    queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                }
            }

            FilterSerializer serializer = new FilterSerializer();
            byte[] arrayOfBytes = serializer.SerializeFilterItems(queryOperations);

            var data = new ExportToExcelHelper().ExportQueryToExcel(new ExportToExcelArgs() { XmlFilters = arrayOfBytes, QueryCode = null, Tenant = tenant, UserId = userid, TypeName = null, QueryColumns = logboxShipmentExportExcelArgs.QueryColumns, QueryPM = new QueryPM() { QuerySection  =logboxShipmentExportExcelArgs.QuerySection, ObjectTableName = logboxShipmentExportExcelArgs.ObjectTableName, DisplayText = logboxShipmentExportExcelArgs.QueryName , EditWizardName = "LogBoxMainComponent" } });

            

            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = "LogBox" + ObjectTableName + DateTime.Now.ToShortDateString(),
                FolderName = "others",
                Extension = "xls",//fileparams[1],
                Tenant = tenant,
                FileSize = data.Length,

            };
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            storageservice.Write(data, fileInfo);


            return Request.CreateResponse(HttpStatusCode.OK, fileInfo.FileName);




        }





    }

    public class LogboxShipmentExportExcelArgs
    {

        public List<QueryColumnPM> QueryColumns { get; set; }
        public int Tenant { get; set; }
        public string UserId { get; set; }
        public string ObjectTableName { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string QuerySection { get; set; }
        public string QueryName { get; set; }

        
        public string SortBy { get; set; }
        public string SortDirection { get; set; }
        public List<QueryFilterItem> AdditionalFilters { get; set; }


    }


}