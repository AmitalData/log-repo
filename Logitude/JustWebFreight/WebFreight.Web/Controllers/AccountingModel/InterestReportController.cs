using Atp.Pdf;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.BL.InterestService;
using Logitude.Accounting.BL.InterestService.HelperClasses;
using Logitude.Accounting.BL.Validators;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.Resolvers;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityQueryServices;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.Controllers.AccountingModel
{
    public class InterestReportController : ApiController
    {
     

        [HttpGet]
        public HttpResponseMessage GetInterestReportsByFilters([FromUri] ApiQueryFilters filters)
        {
            try
            {
                int tenant = AuthinticateTenant();
                var accountingContext = AccountingContext.GetContext(tenant);

                ServiceResponse response = new ServiceResponse();
                InterestReportListQueryService interestReportListQueryService = new InterestReportListQueryService(accountingContext);
                QueryOperations queryOperations = CreateQueryOperations(filters, tenant);
                List<InterestReportList> interestReports = interestReportListQueryService.GetList(queryOperations, tenant);
                if (filters.GetCount)
                {
                    int count = interestReportListQueryService.GetListCount(queryOperations, tenant);
                    response.Count = count;
                }
                response.Result = interestReports;
                return Request.CreateResponse(HttpStatusCode.OK, response);


            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage PutCreateInterestReportInvoiceBatch(InterestReportArguments interestReportArgs)  
        {
            try
            {
                int tenant = AuthinticateTenant();
                string email = HttpContext.Current.User.Identity.Name;
                InterestReportService interestReportService = new InterestReportService();
                string BatchId = interestReportService.CheckLastBatchAndCreateInvoiceBatch(interestReportArgs, tenant, email);  
                return Request.CreateResponse(HttpStatusCode.OK, BatchId);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage PutIsCreateInvoicedValid(InterestReportPM InterestReportPM)
        {
            try
            {
                int tenant = AuthinticateTenant();
                string email = HttpContext.Current.User.Identity.Name;
                bool InterestReportStatus = InterestReportValidator.IsCreateInvoicedValid(InterestReportPM, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, InterestReportStatus);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage PutBatchPrint(InterestReportArguments interestReportArgs)
        {
            try
            {
                int tenant = AuthinticateTenant();
                string email = HttpContext.Current.User.Identity.Name;
                InteretInvoiceBatchPrint interetInvoiceBatchPrint = new InteretInvoiceBatchPrint();
                PdfDocument pdfDoc = interetInvoiceBatchPrint.CheckValidCopiesForInvoicesAndPrint(interestReportArgs, tenant, email);
                MemoryStream memoryStream = new MemoryStream();
                pdfDoc.Save(memoryStream);
                var dataBytes = memoryStream.ToArray();
                var dataStream = new MemoryStream(dataBytes);
                var response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StreamContent(dataStream),
                };
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "InterestInvoices.pdf"
                };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
  
                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        [HttpPut]
        public HttpResponseMessage PrintDocuments(InterestReportArguments interestReportArgs)
        {
            try
            {
                int tenant = AuthinticateTenant();
                string email = HttpContext.Current.User.Identity.Name;
                InteretInvoiceBatchPrint interetInvoiceBatchPrint = new InteretInvoiceBatchPrint();
                bool success = interetInvoiceBatchPrint.PrintDocuments(interestReportArgs, tenant, email);
                return Request.CreateResponse(HttpStatusCode.OK,success);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }


        public HttpResponseMessage PutNumberOfDocumentNotPrinted(InterestReportArguments interestReportArgs)
        {
            try
            {
                int tenant = AuthinticateTenant();
                string email = HttpContext.Current.User.Identity.Name;
                InteretInvoiceBatchPrint interetInvoiceBatchPrint = new InteretInvoiceBatchPrint();
                PDFDocumentInvoices pDFDocumentInvoices=interetInvoiceBatchPrint.GetNumberOfDocumentNotPrinted(interestReportArgs,tenant,email);
                return Request.CreateResponse(HttpStatusCode.OK, pDFDocumentInvoices);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage PutSendSignedInvoices(string[] selectList)
        {
            try
            {
                 int tenant = AuthinticateTenant();
                 DocumentHelper documentHelper= new DocumentHelper();
                 documentHelper.SendSignInterestInvoices(selectList, tenant);
                  
                  return Request.CreateResponse(HttpStatusCode.OK, true);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }


        public HttpResponseMessage GetInterestLastBatchServiceByTenant()
        {
            try
            {
                int tenant = AuthinticateTenant();
                InterestReportService interestReportService = new InterestReportService();
                InterestLastBatchServicePM InterestLastBatchService = interestReportService.GetInterestLastBatchServiceByTenant(tenant);

                return Request.CreateResponse(HttpStatusCode.OK, InterestLastBatchService);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage PutCheckNumberOfInterestReportInvoicingWithoutInvoice(InterestReportArguments interestReportArgs)
        {
            try
            {
                int tenant = AuthinticateTenant();
                string email = HttpContext.Current.User.Identity.Name;
                InterestReportService interestReportService = new InterestReportService();
                int NumberOfInterestReportsWithoutInvoice = interestReportService.CheckNumberOfInterestReportInvoicingWithoutInvoice(interestReportArgs, tenant, email);

                return Request.CreateResponse(HttpStatusCode.OK, NumberOfInterestReportsWithoutInvoice);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
   
        private static int AuthinticateTenant()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.CheckContactFeature("InterestReport", "READ", authToken.Tenant);
            int tenant = authToken.Tenant;
            return tenant;
        }

        private QueryOperations CreateQueryOperations(ApiQueryFilters filters, int tenant)
        {
            QueryOperations queryOperations = new QueryOperations()
            {
                ObjectTableName = "InterestReport",
                PageIndex = filters.PageIndex,
                PageSize = filters.PageSize,
                QuerySection = "InterestReport",
                SortByColumnName = filters.SortBy,
                SortDirectin = filters.SortDirection,
                GetAll = filters.GetAll,
            };
            List<ObjectField> interestReportObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("InterestReport", tenant);
            if (!string.IsNullOrEmpty(filters.AdditionalFilters))
            {
                JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
                var filters_list = JsonConvert.Deserialize<List<QueryFilterItem>>(filters.AdditionalFilters);

                foreach (QueryFilterItem filter in filters_list)
                {

                    ObjectField field = interestReportObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);

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

            return queryOperations;
        }
 
    }


}