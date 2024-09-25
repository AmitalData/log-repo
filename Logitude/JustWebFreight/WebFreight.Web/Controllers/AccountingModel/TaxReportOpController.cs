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
using Simplog.Server.Infrastructure.Helpers;
using System.Transactions;
using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.CoreBL.Batch;
using System.IO;
using System.Net.Http.Headers;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using static Dropbox.Api.Sharing.ListFileMembersIndividualResult;

namespace WebFreight.Web.Controllers.AccountingModel
{

    public class GetTaxReportLinesResponse
    {
        public int Count { get; set; }
        public List<TaxReportLineList> Lines { get; set; }
        public string FileName { get; set; }

    }
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
                SecurityUtility.AuthenticationOnEntityTenant("TaxReport", entityPM.Tenant, authToken.Tenant);
                SecurityUtility.CheckContactFeature("TaxReport", "NEW", authToken.Tenant);
                int tenant = authToken.Tenant;
                TaxReportHelper.CheckWithoutTransmitLines(authToken, entityPM);
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
        public HttpResponseMessage CancelTaxReportByTester(string taxReportId)
        {
            try
            {
                int tenant = GetAuthinticatedTenant();
                TaxReportQueryService taxReportQueryService = new TaxReportQueryService(tenant);
                TaxReportPM taxReportPM = taxReportQueryService.GetSingle(taxReportId, false, false);
                taxReportPM.IsCancelled = true;
                taxReportPM.StatusCode = VatReportStatusValues.Cancelled;
                IAccountingContext accountingContext = AccountingContext.GetContext(taxReportPM.Tenant);
                TaxReportUpdateService taxReportUpdateService = new TaxReportUpdateService(accountingContext, new Dictionary<string, IContext>(), taxReportPM.Tenant);
                taxReportPM.ChangeSetOp = ChangeSetOperation.Update;
                try
                {
                    taxReportUpdateService.Update(taxReportPM, true);
                }
                catch (Exception ex)
                {
                    taxReportPM.IsCancelled = false;
                    taxReportPM.StatusCode = VatReportStatusValues.CancelationFailed;
                    taxReportPM.ChangeSetOp = ChangeSetOperation.Update;
                    taxReportUpdateService.Update(taxReportPM, true);
                }

                return Request.CreateResponse(HttpStatusCode.OK, taxReportPM);
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

        private GetTaxReportLinesResponse GetTaxReportLines([FromUri] ApiQueryFilters filters, int tenant)
        {
            GetTaxReportLinesResponse response = new GetTaxReportLinesResponse();
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
                        if (field.FieldName == "TaxReportId")
                        {
                            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
                            TaxReportQueryService taxReportQuery = new TaxReportQueryService(accountingContext);
                            TaxReportPM taxReportPM = taxReportQuery.GetSingle(valuestring1, true, false);
                            var tenantQuery = new TenantQuery(tenant);
                            var tenantPM = tenantQuery.GetSinglePM(tenant);
                            response.FileName = tenantPM.Company + "." + taxReportPM.VatNumber + "-" + taxReportPM.TaxReportNumber + ".txt";
                        }
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
            if (filters.GetCount)
            {
                int count = trLineQS.GetListCount(queryOperations, tenant);
                response.Count = count;
            }
            response.Lines = trLineQS.GetList(queryOperations, tenant);
            return response;
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


                var resp = GetTaxReportLines(filters, tenant);

                ServiceResponse response = new ServiceResponse();
                if (filters.GetCount)
                {
                    response.Count = resp.Count;
                }

                response.Result = resp.Lines;
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage PostDownloadPaFile([FromUri] ApiQueryFilters filters)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("TaxReport", "READ", authToken.Tenant);

                int tenant = authToken.Tenant;
                if (filters.Tenant != null)
                    tenant = tenant;


                var resp = GetTaxReportLines(filters, tenant);
                var pLines = resp.Lines;
                for (int i = 0; i < pLines.Count; i++)
                {
                    string vatNumber = pLines[i].VatNumber.Substring(0,9);
                    string referenceDate = pLines[i].ReferenceDate.Value.ToString("ddMMyyyy");
                    string referecneGroup = pLines[i].ReferecneGroup.Substring(0, 1);
                    string reference = pLines[i].Reference.PadRight(14, ' ').Substring(0,14);
                    string vatAmount = (pLines[i].VatAmount < 0 ? "-" + pLines[i].VatAmount.ToString().Replace("-", "").TrimEnd('0', '.').PadLeft(8, '0') : pLines[i].VatAmount.ToString().TrimEnd('0', '.').PadLeft(9, '0')).Substring(0,9);
                    string row = string.Format("{0}{1}{2}{3}{4}", vatNumber, referenceDate, referecneGroup, reference, vatAmount);
                    if (i == pLines.Count - 1)
                    {
                        sb.Append(row);
                    }
                    else
                    {
                        sb.AppendLine(row);
                    }
                }
                // Generate content of the text file
                string fileContent = sb.ToString();

                // Convert content to byte array
                byte[] contentBytes = Encoding.UTF8.GetBytes(fileContent);

                // Create a MemoryStream to hold the file content
                MemoryStream ms = new MemoryStream(contentBytes);


                var response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StreamContent(ms),
                };
                // Set content type header
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                response.Content.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");

                // Set file name header
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = resp.FileName
                };

                return response;
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
                bool FutureReportExist = CheckIfActiveFutureReportsExist(createDate, tenant);
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


                return Request.CreateResponse(HttpStatusCode.OK, entityPM);
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
                bool batchIt = true;
                if (batchIt)
                {
                    BatchTaskExecutionPM btePM = TaxReportService.CreateTaxReportFileInBatch(entityPM.Id, tenant,entityPM.RecalculateData);


                    return Request.CreateResponse(HttpStatusCode.OK, btePM);
                }
                else
                {
                    DirectRun(entityPM, tenant);
                    var res1 = new { Success = true, Message = "" };
                    return Request.CreateResponse(HttpStatusCode.OK, res1);

                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }



        private void DirectRun(TaxReportPM taxReportPM, int tenant)
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(taxReportPM.Tenant);

            TaxReportUpdateService taxReportUpdateService = new TaxReportUpdateService(accountingContext, new Dictionary<string, IContext>(), taxReportPM.Tenant);

            // Call the service
            List<TaxReportLinePM> lines = TaxReportService.CreateTaxReportLines(taxReportPM, tenant);
            TaxReportService.CalculateReportTotals(taxReportPM, lines);
            taxReportPM.ChangeSetOp = ChangeSetOperation.Update;
            taxReportUpdateService.Update(taxReportPM, true);
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
                List<TaxReportPM> reports = taxReportQueryService.GetTransmittedTaxReports(tenant);


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

        public HttpResponseMessage GetTaxReportReconciledLines(string taxReportId)
        {
            try
            {

                int tenant = GetAuthinticatedTenant();

                IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
                TaxReportQueryService reportService = new TaxReportQueryService(accountingContext);
                var reconciledLines = reportService.GetTaxReportReconciledLines(taxReportId, tenant);

                if (reconciledLines != null && reconciledLines.Count() > 0)
                {
                    string linesNumbersCS = string.Join(",", reconciledLines.Select(d => d.Line));

                    return Request.CreateResponse(HttpStatusCode.OK, new ServiceResponse { Result = linesNumbersCS });
                }


                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage PostClosingTaxReportJournal(string taxReportId)
        {
            try
            {
                int tenant = GetAuthinticatedTenant();
                string batchId = new BatchClosingTaxReportJournalTask(null).CreateQBatchTaskExecution<BatchClosingTaxReportJournalTaskArgs>(
                    new BatchClosingTaxReportJournalTaskArgs()
                    {
                        TaxReportId = taxReportId,
                        Tenant = tenant
                    }, tenant, "Closing Tax Report Journal", false);

                return Request.CreateResponse(HttpStatusCode.OK, batchId);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostCancelClosingJournal(string taxReportId)
        {
            try
            {
                int tenant = GetAuthinticatedTenant();
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    TaxReportClosingService closingService = new TaxReportClosingService(tenant, taxReportId, true);
                    closingService.CancelClosingJournal();

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, closingService.journalPM);
                }

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }


        public HttpResponseMessage PostCancelClosingJournalInBatch(string taxReportId)
        {
            try
            {
                int tenant = GetAuthinticatedTenant();
                string batchId = new BatchCancelClosingJournalTask(null).CreateQBatchTaskExecution<BatchCancelClosingJournalTaskArgs>(
                    new BatchCancelClosingJournalTaskArgs()
                    {
                        TaxReportId = taxReportId,
                        Tenant = tenant
                    }, tenant, "Cancel Closing Tax Report Journal", false);

                return Request.CreateResponse(HttpStatusCode.OK, batchId);
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
