using Atp.Pdf;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.BL.InterestService;
using Logitude.Accounting.BL.InterestService.HelperClasses;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
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
        public Stream _Stream;
        byte[] datainByte;

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

        public HttpResponseMessage PutBatchPrint(InterestReportArguments interestReportArgs)
        {
            try
            {
                int tenant = AuthinticateTenant();
                string email = HttpContext.Current.User.Identity.Name;
                PdfDocument pdfDoc = new PdfDocument();
                InterestReportQueryService interestReportQueryService = new InterestReportQueryService(tenant);
                ARInvoiceQuery aRInvoiceQuery = new ARInvoiceQuery(tenant);

                if (interestReportArgs.AllSelected)
                {
                    IQueryable<ARInvoice> ARInvoices = aRInvoiceQuery.GetAllInterestInvoices(interestReportArgs.FromDate, interestReportArgs.ToDate, interestReportArgs.ShowPrintedInvoice, tenant);
                    if (interestReportArgs.ExcludedIds != null)
                    {
                        interestReportArgs.SelectedIds = (from a in ARInvoices
                                                          where !interestReportArgs.ExcludedIds.Contains(a.Id)
                                                          select a.Id).ToList();

                    }

                }
 
                interestReportArgs.SelectedIds = interestReportQueryService.GetInterestReprtsWithInvocies(tenant, interestReportArgs.SelectedIds);

                for (int i = 0; i < interestReportArgs.SelectedIds.Count; i++)
                {
                    string [] EntitiesId = interestReportArgs.SelectedIds[i].Split(',') ;
                    string InterestReportId = EntitiesId[0];
                    string ARInvoieId  = EntitiesId[1];
                    bool IsPrintARInvoice = PrintInvoicesPDF(email, tenant, ARInvoieId, pdfDoc,"999G",false);
                    if (IsPrintARInvoice)
                    {
                        bool IsPrintInterestReport = PrintInvoicesPDF(email, tenant, InterestReportId, pdfDoc, "ITDT", false);
                    }
 
                 }

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


        public HttpResponseMessage PutNumberOfDocumentNotPrinted(InterestReportArguments interestReportArgs)
        {
            try
            {
                int tenant = AuthinticateTenant();
                string email = HttpContext.Current.User.Identity.Name;
                PdfDocument pdfDoc = new PdfDocument();
                InterestReportQueryService interestReportQueryService = new InterestReportQueryService(tenant);
                ARInvoiceQuery aRInvoiceQuery = new ARInvoiceQuery(tenant);

                if (interestReportArgs.AllSelected)
                {
                    IQueryable<ARInvoice> ARInvoices = aRInvoiceQuery.GetAllInterestInvoices(interestReportArgs.FromDate, interestReportArgs.ToDate, interestReportArgs.ShowPrintedInvoice, tenant);
                    if (interestReportArgs.ExcludedIds != null)
                    {
                        interestReportArgs.SelectedIds = (from a in ARInvoices
                                                          where !interestReportArgs.ExcludedIds.Contains(a.Id)
                                                          select a.Id).ToList();

                    }

                }

                interestReportArgs.SelectedIds = interestReportQueryService.GetInterestReprtsWithInvocies(tenant, interestReportArgs.SelectedIds);
                List<string> ARInvoiceIdsNotPrinted = new List<string>();
                List<string> InterestReportIdsNotPrinted = new List<string>();

                for (int i = 0; i < interestReportArgs.SelectedIds.Count; i++)
                {
                    string[] EntitiesId = interestReportArgs.SelectedIds[i].Split(',');
                    string InterestReportId = EntitiesId[0];
                    string ARInvoieId = EntitiesId[1];
                    bool IsPrintARInvoice = PrintInvoicesPDF(email, tenant, ARInvoieId, pdfDoc, "999G",true);
                    if (!IsPrintARInvoice)
                    {
                        ARInvoiceIdsNotPrinted.Add(ARInvoieId);

                    }
                    bool IsPrintInterestReport = PrintInvoicesPDF(email, tenant, InterestReportId, pdfDoc, "ITDT", true);
                    if (!IsPrintInterestReport)
                    {
                        InterestReportIdsNotPrinted.Add(InterestReportId);
                    }
      
                }

 
                List<string> ARInvoiceNumbersNotPrinted = null;
                List<string> InterestReportNumbersNotPrinted = null;
                if (ARInvoiceIdsNotPrinted != null && ARInvoiceIdsNotPrinted.Count() > 0)
                {
                    ARInvoiceNumbersNotPrinted = aRInvoiceQuery.GetInterestInvoiceNumbersByIds(ARInvoiceIdsNotPrinted, tenant);

                }
                if (InterestReportIdsNotPrinted != null && InterestReportIdsNotPrinted.Count() > 0)
                {
                    InterestReportNumbersNotPrinted = interestReportQueryService.GetInterestReportNumbersByIds(InterestReportIdsNotPrinted, tenant);

                }
 
                PDFDocumentInvoices pDFDocumentInvoices = new PDFDocumentInvoices();
                pDFDocumentInvoices.ARInvoiceNumbersNotPrinted = ARInvoiceNumbersNotPrinted;
                pDFDocumentInvoices.InterestReportNumbersNotPrinted = InterestReportNumbersNotPrinted;
 
 
                return Request.CreateResponse(HttpStatusCode.OK, pDFDocumentInvoices);
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

        private bool PrintInvoicesPDF(string Email, int? tenant, string SelectId, PdfDocument pdfDoc, string DocumentCode, bool IsForChecked)
        {
            try
            {


                string userId = "";
                string documentOutId = null;
                string email = Email;
                bool IsPrinted = false;

                DocumentOut doucmentOut = null;
                ICommonDataContext commonContext = CommonDataContext.GetContext((tenant != null ? (int)tenant : 0));

                DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery((int)tenant);
                string documentTypeId = documentTypeQuery.GetDocumentTypeListIdByCodeAndTenant(DocumentCode, (int)tenant);


                DocumentOutQuery documentOutQuery = new DocumentOutQuery((int)tenant);


                doucmentOut = (from a in commonContext.DocumentOuts
                               join docFile in commonContext.DocumentsFilings on a.Id equals docFile.Id
                               where docFile.EntityId == SelectId && docFile.DocumentTypeId == documentTypeId && a.Tenant == tenant
                               select a).Include("DocumentsFiling").Include("DocumentsFiling.DocumentType").FirstOrDefault();

                if (doucmentOut != null)
                {
                    bool includeInPrint = true;

                    documentOutId = doucmentOut.Id;
                    tenant = doucmentOut.Tenant;

                    List<DocumentOutCopy> copies = (from a in commonContext.DocumentOutCopies
                                                    where a.DocumentOutId == documentOutId && tenant == (int)tenant
                                                    select a).OrderBy(d => d.DocumentTypeCopy.IndexOrder).ToList();

                    Uploader up = new Uploader();

                    foreach (DocumentOutCopy copy in copies)
                    {
                        includeInPrint = true;
                        if (doucmentOut.DocumentsFiling.DocumentType.IsDocumentOneTimePrintLimited && doucmentOut.DocumentsFiling.DocumentType.LimitedPrintCopyId == copy.DocumentTypeCopyId && !string.IsNullOrEmpty(copy.LastPrintedByUserId))
                        {
                            includeInPrint = false;
                        }

                        if (includeInPrint)
                        {
                            IsPrinted = true;
                            if (!IsForChecked)
                            {

                                string documentExtension = up.GetFileExtension(copy.DocumentId, (int)tenant);
                                string documentId = copy.DocumentId;
                                if (!string.IsNullOrEmpty(documentExtension))
                                {
                                    _Stream = DownloadFile(documentId, documentExtension, "", (int)tenant);
                                    if (_Stream != null)
                                    {
                                        PdfDocumentBase.Merge(pdfDoc, _Stream);
                                        if ((pdfDoc.Pages.Count % 2 == 1) && doucmentOut.DocumentsFiling.DocumentType.Code == "740")
                                        {
                                            pdfDoc.Pages.Add();
                                        }

                                        UserRepository userRep = new UserRepository((int)tenant);
                                        User printedBy = null;
                                        if (!string.IsNullOrEmpty(userId)) printedBy = userRep.GetSingleUser(userId, (int)tenant);
                                        else printedBy = userRep.GetSingleUserByCodeOrEmailForTenant(null, email, (int)tenant, false);
                                        if (printedBy == null)
                                        {
                                            printedBy = userRep.GetSingleUserByCodeOrEmailForTenant(null, email, 0, false);
                                        }

                                        DocumentOutCopyRepository myRep = new DocumentOutCopyRepository((int)tenant);
                                        DocumentOutCopy documentoutCopy = myRep.GetSingleDocumentOutCopyByTenant(copy.Id, (int)tenant);
                                        documentoutCopy.LastPrintDate = TenantServerConfigration.GetCurrentDateTime((int)tenant);
                                        documentoutCopy.LastPrintedByUserId = printedBy.Id;
                                        myRep.Update(documentoutCopy);
                                        myRep.SubmitChanges();

                                        if (DocumentCode == "999G")
                                        {
                                            ARInvoiceRepository aRInvoiceRepository = new ARInvoiceRepository((int)tenant);
                                            ARInvoice aRInvoice = aRInvoiceRepository.GetInvoices().Where(s => s.Id == SelectId).FirstOrDefault();
                                            aRInvoice.IsPrinted = true;
                                            aRInvoiceRepository.Update(aRInvoice);
                                            aRInvoiceRepository.SubmitChanges();
                                        }

                                        break;

                                    }
                                }
                            }



                        }

                    }

                }




                return IsPrinted;



            }
            catch (Exception errorInfo)
            {
                string errorMessage = errorInfo.Message;

                if (errorInfo.InnerException != null)
                {
                    errorMessage += Environment.NewLine + errorInfo.InnerException.Message;
                }
                errorMessage += Environment.NewLine + errorInfo.ToString();
                if (!string.IsNullOrEmpty(errorInfo.StackTrace))
                {
                    errorMessage += Environment.NewLine + errorInfo.StackTrace;
                }
                throw new ApplicationException(errorMessage);
            }
        }
        public Stream DownloadFile(string documentId, string documentExtension, string fileLocation, int tenant)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);

            Document document = (from doc in commonContext.Documents
                                 where doc.Id == documentId
                                 select doc).FirstOrDefault();
            if (document != null)
            {
                //string filelocation = GetFileLocation(fileLocation);
                try
                {

                    //else // In Azure
                    //{

                    //string filename = document.Id + "." + document.Extension;

                    //blobContainer = StorageAcountDetails.GetCurrentContainer(tenant);

                    BlobFileInfo fileInfo = new BlobFileInfo()
                    {
                        FileName = document.Id,
                        FolderName = document.Folder,
                        Extension = document.Extension,
                        Tenant = document.Tenant,
                        FileSize = document.FileSize,
                    };
                    IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                    datainByte = storageservice.Read(fileInfo);


                    if (datainByte != null)
                    {

                        MemoryStream stream = new MemoryStream(datainByte);
                        return stream;
                    }
                    else
                        return null;

                }
                catch (Exception e)
                {
                    string ip = "";
                    if (HttpContext.Current != null && HttpContext.Current.Request != null)
                    {
                        string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                        if (string.IsNullOrEmpty(currentIP))
                        {
                            currentIP = HttpContext.Current.Request.UserHostAddress;
                        }
                        ip = currentIP;
                    }
                    ExceptionHandler.HandleException(e, DateTime.Now, tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "MergeAllPage : DownloadFile Method ", ip);
                    return null;
                }
            }
            return null;
        }


    }



    public class PDFDocumentInvoices
    {
        public StreamContent Document { set; get; }
        public List<string> ARInvoiceNumbersNotPrinted { set; get; }
        public  List<string> InterestReportNumbersNotPrinted { set; get; }
    }
}