using System;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Accounting.BL.CoreBL;
using System.Web.Http;
using Logitude.Infrastructure.BL.EntityPMs;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Web.Script.Serialization;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using WebFreight.Web.DataContracts;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.DataContract;
using Logitude.BL.CommonDataModel.EntityQueries;
using System.Text;
using Logitude.Accounting.BL.CoreBL.Testers;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.BL.EntityUpdateServices;
using Simplog.Server.Infrastructure;

namespace WebFreight.Web.Controllers.AccountingModel
{


    public class TaxReportOpController : ApiController
    {
        //public HttpResponseMessage PostDownloadPNC874File(TaxReportPM entityPM)
        //{
        //    try
        //    {
        //        string token = HttpContext.Current.Request.Headers["Token"];
        //        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
        //        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
        //        SecurityUtility.CheckContactFeature("TaxReport", "NEW", authToken.Tenant);
        //        int tenant = authToken.Tenant;

        //        DocumentsFilingPM docOut = TaxReportService.CreatePNC874File(entityPM.Id, tenant);


        //        return Request.CreateResponse(HttpStatusCode.OK, docOut);
        //    }

        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
        //    }

        //}

        public HttpResponseMessage PostDownloadPNC874FileInBatch(TaxReportPM entityPM)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnEntityTenant("TaxReport", entityPM.Tenant,authToken.Tenant);
                SecurityUtility.CheckContactFeature("TaxReport", "NEW", authToken.Tenant);
                int tenant = authToken.Tenant;               
               TaxReportHelper.CheckWithoutTransmitLines(authToken,entityPM);
                TaxReportHelper.CheckErrorsInLines(authToken, entityPM);
                BatchTaskExecutionPM btePM = TaxReportService.CreatePNCFileInBatch(entityPM.Id, tenant);


                return Request.CreateResponse(HttpStatusCode.OK, btePM);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage CancelTaxReportInBatch(TaxReportPM entityPM)
        {
            try
            {
                int tenant = GetAuthinticatedTenant();
                SecurityUtility.AuthenticationOnEntityTenant("TaxReport", entityPM.Tenant, tenant);

                BatchTaskExecutionPM batchTaskPM = TaxReportService.CancelTaxReportInBatch(entityPM.Id, tenant);


                return Request.CreateResponse(HttpStatusCode.OK, batchTaskPM);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        private static int GetAuthinticatedTenant()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.CheckContactFeature("TaxReport", "UPDATE", authToken.Tenant);
            int tenant = authToken.Tenant;
            return tenant;
        }

        [HttpGet]
        public HttpResponseMessage GetLinesByFilters([FromUri] ApiQueryFilters filters)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("TaxReport", "READ", authToken.Tenant);

                int tenant = authToken.Tenant;
                if (filters.Tenant != null)
                    tenant = tenant;

                QueryOperations queryOperations = new QueryOperations()
                {
                    ObjectTableName = "TaxReportLine",
                    PageIndex = filters.PageIndex,
                    PageSize = filters.PageSize,
                    QuerySection = "TaxReportLines",
                    SortByColumnName = filters.SortBy,
                    SortDirectin = filters.SortDirection,
                    GetAll = filters.GetAll,
                };


                List<ObjectField> TaxReportLineObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("TaxReportLine", tenant);
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
                        ObjectField field = TaxReportLineObjectFields.FirstOrDefault(f => f.FieldName == filterName);
                        if (field != null)
                        {
                            string valuestring1 = filterValue1 != null ? filterValue1.ToString() : null;
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = filterValue2 != null ? filterValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            queryOperations.SetFilter(filterName, value1, field.IsCustomFilter, filterOperator, value2, field.DisplayInList);
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
                        ObjectField field = TaxReportLineObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);
                        if (field != null)
                        {


                            string valuestring1 = filter.FieldValue != null ? filter.FieldValue.ToString() : null;
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = filter.FieldValue2 != null ? filter.FieldValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList);
                        }
                        else
                        {
                            queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                        }
                    }
                }

                IAccountingContext MyContext = AccountingContext.GetContext(tenant);
                TaxReportLineListQueryService trLineQS = new TaxReportLineListQueryService(MyContext);

                List<TaxReportLineList> entityLists = trLineQS.GetList(queryOperations, tenant);

                ServiceResponse response = new ServiceResponse();
                if (filters.GetCount)
                {
                    int count = trLineQS.GetListCount(queryOperations, tenant);
                    response.Count = count;
                }

                response.Result = entityLists;
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage GetLinesCounters(string taxReportId)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("TaxReport", "READ", authToken.Tenant);
                int tenant = authToken.Tenant;
                

                IAccountingContext MyContext = AccountingContext.GetContext(tenant);
                TaxReportQueryService reportService = new TaxReportQueryService(MyContext);
                TaxReportLinesCounter reportCounter = reportService.GetReportLinesCounter(taxReportId, tenant);

                ServiceResponse response = new ServiceResponse();
              
                response.Result = reportCounter;
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetReturnToDraftButtonStatus(DateTime createDate)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                AuthenticationToken authToken = GetAuthenticationToken();
                SecurityUtility.CheckContactFeature("TaxReport", "READ", authToken.Tenant);
                int tenant = authToken.Tenant;
                bool FutureReportExist = CheckIfActiveFutureReportsExist(createDate,tenant);              
                ServiceResponse response = new ServiceResponse();
                response.Result = FutureReportExist;
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        private bool CheckIfActiveFutureReportsExist(DateTime createDate, int tenant)
        {
            IAccountingContext MyContext = AccountingContext.GetContext(tenant);
            TaxReportQueryService taxReportQueryService = new TaxReportQueryService(MyContext);
           return taxReportQueryService.GetFutureActiveReports(createDate, tenant).Any();

        }
        private AuthenticationToken GetAuthenticationToken()
        {

            string token = HttpContext.Current.Request.Headers["Token"];

            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            return authToken;
        }
        public HttpResponseMessage GetErrorsCount(string reportId)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("TaxReport", "READ", authToken.Tenant);
                int tenant = authToken.Tenant;


                IAccountingContext MyContext = AccountingContext.GetContext(tenant);
                TaxReportQueryService reportService = new TaxReportQueryService(MyContext);
                int count = reportService.GetReportLinesWithErrors(reportId, tenant);

                ServiceResponse response = new ServiceResponse();

                response.Result = count;
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage PutCreateTaxReportLine(TaxReportPM entityPM)
        {

            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnEntityTenant("TaxReport", entityPM.Tenant, authToken.Tenant);

                SecurityUtility.CheckContactFeature("TaxReport", "UPDATE", authToken.Tenant);
                int tenant = authToken.Tenant;             
            //    entityPM = TaxReportService.CreatetTaxReportLine(entityPM);


                return Request.CreateResponse(HttpStatusCode.OK,entityPM );
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage PostCreateTaxReportInBatch(TaxReportPM entityPM)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnEntityTenant("TaxReport", entityPM.Tenant, authToken.Tenant);

                SecurityUtility.CheckContactFeature("TaxReport", "NEW", authToken.Tenant);
                int tenant = authToken.Tenant;

                BatchTaskExecutionPM btePM = TaxReportService.CreateTaxReportFileInBatch(entityPM.Id, tenant);


                return Request.CreateResponse(HttpStatusCode.OK, btePM);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetTenantTransmittedTaxReports()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("TaxReport", "NEW", authToken.Tenant);
                int tenant = authToken.Tenant;
                TaxReportQueryService taxReportQueryService = new TaxReportQueryService(tenant);
                List<TaxReportPM> reports= taxReportQueryService.GetTransmittedTaxReports(tenant);
              

                return Request.CreateResponse(HttpStatusCode.OK, reports);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }


        public HttpResponseMessage PostCreateTaxReportLines(ImageParameter fileUploadParamerter)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnEntityTenant("ImageParameter", fileUploadParamerter.Tenant, authToken.Tenant);

                if (fileUploadParamerter != null && !string.IsNullOrEmpty(fileUploadParamerter.Base64String))
                {
                    string winHebrewString = EncodeStringFromImageParameter(fileUploadParamerter);

                    var myConsolidatedTaxReportFlatFileAnalyser = new ConsolidatedTaxReportFlatFileAnalyser();
                    myConsolidatedTaxReportFlatFileAnalyser.Analyse(authToken.Tenant, fileUploadParamerter.EntityId, winHebrewString);

                    return Request.CreateResponse(HttpStatusCode.OK, new { Message = "Done" });
                }
                else
                {
                    throw new Exception("fileUploadParamerter is empty");
                }



            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetTaxReportClosingJournalAbility(string taxReportId)
        {
            try
            {

                int tenant = GetAuthinticatedTenant();

                IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
                TaxReportQueryService reportService = new TaxReportQueryService(accountingContext);
                var canHaveClosingJournal = reportService.CheckIfTaxReportCanHaveClosingJournal(taxReportId, tenant);

                ServiceResponse response = new ServiceResponse
                {
                    Result = canHaveClosingJournal
                };

                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        private string EncodeStringFromImageParameter(ImageParameter fileUploadParamerter)
        {
            byte[] dataBytes = Convert.FromBase64String(fileUploadParamerter.Base64String);
            var dosEnc = System.Text.Encoding.GetEncoding("DOS-862"); // ms-dos codepage ( US English )
            var winHebrewEncoding = Encoding.GetEncoding("Windows-1255");
            var hebBytes = Encoding.Convert(dosEnc, winHebrewEncoding, dataBytes);
            string winHebrewString = winHebrewEncoding.GetString(hebBytes);
            return winHebrewString.Replace("\r\n", "\n").Replace("\n", Environment.NewLine);

        }

    }
}
	 