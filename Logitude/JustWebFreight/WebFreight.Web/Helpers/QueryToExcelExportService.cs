using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
 
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Script.Serialization;
using WebFreight.Web.Security;

namespace WebFreight.Web.Helpers
{
    public class QueryToExcelExportService
    {
        public ExportResult ExportQueryDataToExcel(CustomApiQueryFilters queryFilters)
        {
            var runViaWorkerRole = FeatureToggleHelper.HasFeatureToggle("EQW", (int)queryFilters.Tenant);
            if (runViaWorkerRole)
            {
                return AddQueryExportExecutionLog(queryFilters);
            }
            else
            {
                return ExportQueryDataToStorage(new ExportQueryToExcelArgs() { QueryFilters = queryFilters });
            }
        }

        private ExportResult AddQueryExportExecutionLog(CustomApiQueryFilters queryFilters)
        {
            int tenant = (int)queryFilters.Tenant;
            //var queryOperations = this.GetQueryOperations(queryFilters);
            QueryExportExecutionLogRepository reportExecutionLogRepository = new QueryExportExecutionLogRepository(tenant);
            var logId = IdCounter.GetNumber("QueryExportExecutionLog", tenant);// Guid.NewGuid().ToString();//

            QueryExportExecutionLog executionLog = new QueryExportExecutionLog()
            {
                Id = logId,
                CreateDate = DateTime.Now,
                CreatedByUserId = queryFilters.userid,
                QueryFilterXML = LogitudeXmlSerializer.SerializeObjectToXmlString(queryFilters),
                Tenant = tenant,
                StatusCode = "W",
                QueryCode = queryFilters.queryCode,

            };


            reportExecutionLogRepository.Add(executionLog);
            reportExecutionLogRepository.SubmitChanges();

            string ObjectTableName = queryFilters.ObjectTableName.Replace("Customs.", "");
            string fileName = GetOutpuFileName(queryFilters, ObjectTableName);

            var loggedEmail = SecurityUtility.GetAuthenticatedUser();

            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("QueryExportExecutionLogQueue", executionLog.Tenant);
            queueservice.Send(new Dictionary<string, string>() {
                    { "LogId", executionLog.Id },
                    { "Tenant", executionLog.Tenant.ToString() },
                    { "FileName", fileName },
                    { "LoggedUserEmail", loggedEmail },
                }, tenant, null, null, null, null);



            return new ExportResult() { ExecutionLogId = logId, FileName = fileName, IsWorkerRole = true };
        }

        private static string GetOutpuFileName(CustomApiQueryFilters queryFilters, string ObjectTableName)
        {
            return ObjectTableName + "_" + queryFilters.queryCode + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss");
        }

        public ExportResult ExportQueryDataToStorage(ExportQueryToExcelArgs args)
        {
            var queryFilters = args.QueryFilters;
            var queryOperations = this.GetQueryOperations(queryFilters);
            var IsXslxFormatFilter = queryOperations.QueryFilterItems.FirstOrDefault(x => x.FieldName == "IsXslxFormat");
            FilterSerializer serializer = new FilterSerializer();
            byte[] arrayOfBytes = serializer.SerializeFilterItems(queryOperations);


            string queryCode = queryFilters.queryCode;
            int tenant = (int)queryFilters.Tenant;
            string userid = queryFilters.userid;
            string ObjectTableName = queryFilters.ObjectTableName.Replace("Customs.", "");

            SecurityUtility.IsWorkerRoleCall = Logitude.BL.Security.SecurityUtility.IsWorkerRoleCall = args.IsWorkerRoleCall;
            Logitude.CRM.Data.Helpers.Tools.AuthenticatedUserEmail = args.LoggedUserEmail;

            var data = new ExportToExcelHelper().ExportQueryToExcel(new ExportToExcelArgs()
            {
                XmlFilters = arrayOfBytes,
                QueryCode = queryCode,
                Tenant = tenant,
                UserId = userid,
                TypeName = null,
                IsXslxFormat = IsXslxFormatFilter != null ? true : false,
            });
            //Uploader uploaderService = new Uploader();
            //string[] blockIdlist = { Convert.ToBase64String(Guid.NewGuid().ToByteArray()) };
            //string result = uploaderService.UploadFile(ObjectTableName + DateTime.Now.ToShortDateString() + ".xls", data, data.Length, data.Length, blockIdlist, 0, null, tenant, "others", null);
            //if (ObjectTableName.Contains("Customs."))
            //{
            //    ObjectTableName = ObjectTableName.Replace("Customs.", "");
            //}

            string fileName = !string.IsNullOrEmpty(args.OutputFileName) ? args.OutputFileName : GetOutpuFileName(queryFilters, ObjectTableName);
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = fileName,//ObjectTableName + DateTime.Now.ToShortDateString(),//fileparams[0],
                FolderName = "others",
                Extension = "xls",//fileparams[1],
                Tenant = tenant,
                FileSize = data.Length,

            };
            if (Logitude.Server.Tools.Helpers.FeatureToggleHelper.HasFeatureToggle("NXL", tenant))
                fileInfo.Extension = "xlsx";
            //string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(fileNameAndExtension.ToLower(), fileLocation);
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            storageservice.Write(data, fileInfo);

            return new ExportResult() { FileName = fileName };
        }

        private QueryOperations GetQueryOperations(CustomApiQueryFilters filters)
        {
            string queryId = filters.queryId;
            string queryCode = filters.queryCode;

            int tenant = (int)filters.Tenant;
            string userid = filters.userid;
            string ObjectTableName = filters.ObjectTableName;

            QueryRepository queryRep = new QueryRepository(tenant);
            QueryQuery queryQuery = new QueryQuery(queryRep);
            QueryPM query = queryQuery.GetSingleQueryPM(queryCode, tenant);
            QueryOperations queryOperations = new QueryOperations()
            {
                ObjectTableName = ObjectTableName,
                PageIndex = filters.PageIndex,
                PageSize = filters.PageSize,
                QuerySection = query.QuerySection,
                SortByColumnName = filters.SortBy,
                SortDirectin = filters.SortDirection,

            };

            List<ObjectField> ObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName(ObjectTableName, tenant);
            List<PropertyInfo> filterProperties = filters.GetType().GetProperties().ToList();
            for (int i = 1; i <= 10; i++)
            {
                object filterNameProp = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Name")).GetValue(filters);
                object filterValue1 = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Value")).GetValue(filters);
                object filterOperatorProp = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Operator")).GetValue(filters);
                object filterValue2 = null;

                if (filterNameProp != null)
                {
                    string filterName = filterNameProp.ToString();
                    string filterOperator = filterOperatorProp != null ? filterOperatorProp.ToString() : "Equals";
                    //if (filterValue1 != null && filterValue1.GetType() == typeof(string))
                    //{
                    //string[] values = filterValue1.ToString().Split(',');
                    //if (values.Count() > 1)
                    //{
                    //filterValue1 = values[0];
                    //filterValue2 = values[1];
                    //}
                    //}
                    //ToDo: Get object field by name and set the remained filter properties
                    ObjectField field = ObjectFields.FirstOrDefault(f => f.FieldName == filterName);
                    if (field != null)
                    {
                        string valuestring1 = filterValue1 != null ? filterValue1.ToString() : null;
                        object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                        string valuestring2 = filterValue2 != null ? filterValue2.ToString() : null;
                        object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                        //queryOperations.SetFilter(filterName, value1, field.IsCustomFilter, filterOperator, value2, field.DisplayInList);
                        queryOperations.SetFilter(filterName, value1, field.IsCustomFilter, filterOperator, value2, field.DisplayInList, field.IsCustom, field.DataTypeCode);
                    }
                    else
                        queryOperations.SetFilter(filterName, filterValue1, false, filterOperator, filterValue2, true);
                }



            }

            if (!string.IsNullOrEmpty(filters.AdditionalFilters))
            {
                JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
                var filters_list = JsonConvert.Deserialize<List<QueryFilterItem>>(filters.AdditionalFilters);

                foreach (QueryFilterItem filter in filters_list)
                {
                    ObjectField field = ObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);
                    if (field != null)
                    {


                        string valuestring1 = filter.FieldValue != null ? filter.FieldValue.ToString() : null;
                        object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                        string valuestring2 = filter.FieldValue2 != null ? filter.FieldValue2.ToString() : null;
                        object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);
                        queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList, field.IsCustom, field.DataTypeCode);
                        //queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList);
                    }
                    else
                    {
                        queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                    }
                }
            }

            return queryOperations;
        }



    }

    public class ExportQueryToExcelArgs {
        public CustomApiQueryFilters QueryFilters { get; set; }
        public bool IsWorkerRoleCall { get; set; }
        public string OutputFileName { get; set; }

        public string LoggedUserEmail { get; set; }
    }
    public class ExportResult
    {
        public string FileName { get; set; }
        public bool IsWorkerRole { get; set; }
        public string ExecutionLogId { get; set; }
    }
}