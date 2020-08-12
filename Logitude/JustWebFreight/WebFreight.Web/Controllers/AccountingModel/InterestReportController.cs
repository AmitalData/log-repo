using Atp.Pdf;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
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

        public HttpResponseMessage PutInterestReportStatus(InterestReportArguments interestReportArgs)
        {
            try
            {
                int tenant = AuthinticateTenant();
                string email = HttpContext.Current.User.Identity.Name;
                string BatchId = null;

                BatchId = CheckLastBatchAndCreateInvoiceBatch(interestReportArgs, tenant, email);
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
                    if (IsPrintARInvoice)
                    {
                        bool IsPrintInterestReport = PrintInvoicesPDF(email, tenant, InterestReportId, pdfDoc, "ITDT", true);
                        if (!IsPrintInterestReport)
                        {
                            InterestReportIdsNotPrinted.Add(InterestReportId);
                        }
                    }
                    else
                    {
                        ARInvoiceIdsNotPrinted.Add(ARInvoieId);

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

        private string CheckLastBatchAndCreateInvoiceBatch(InterestReportArguments interestReportArgs, int tenant, string email)
        {
            string BatchId = null;
            InterestLastBatchServiceQueryService interestLastBatchServiceQueryService = new InterestLastBatchServiceQueryService(tenant);
            InterestLastBatchServicePM InterestLastBatchService = interestLastBatchServiceQueryService.CheckInterestLastBatchServicesByTenant(tenant);
            IAccountingContext MyContext = AccountingContext.GetContext(tenant);
            InterestLastBatchServiceUpdateService interestLastBatchServiceUpdateService = new InterestLastBatchServiceUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);

            if (InterestLastBatchService != null && !string.IsNullOrEmpty(InterestLastBatchService.CreateInvoicesBatchId))
            {
                BatchTaskExecutionQueryService batchTaskExecutionQueryService = new BatchTaskExecutionQueryService(tenant);
                BatchTaskExecutionPM batchTaskExecutionPM = batchTaskExecutionQueryService.GetSingle(InterestLastBatchService.CreateInvoicesBatchId, false, false);

                if (batchTaskExecutionPM.StatusCode == "D" || batchTaskExecutionPM.StatusCode == "F")
                {
                    BatchId = CreateBatchInvoice(interestReportArgs, tenant, email);
                    InterestLastBatchService.CreateInvoicesBatchId = BatchId;
                    InterestLastBatchService.ChangeSetOp = ChangeSetOperation.Update;
                    interestLastBatchServiceUpdateService.Update(InterestLastBatchService, true);
                }
                else
                {
                    ContactPM contact = GetLoggedContact(tenant);
                    bool showLocals = !contact.DontShowLocal;
                    throw new Exception(TextCodesTranslator.TranslateText("InterestReport.O.AnotherBatchInvoiceStillInProgress", tenant, showLocals));
                }
            }
            else if (InterestLastBatchService != null && string.IsNullOrEmpty(InterestLastBatchService.CreateInvoicesBatchId))
            {
                BatchId = CreateBatchInvoice(interestReportArgs, tenant, email);
                InterestLastBatchService.CreateInvoicesBatchId = BatchId;
                InterestLastBatchService.ChangeSetOp = ChangeSetOperation.Update;
                interestLastBatchServiceUpdateService.Update(InterestLastBatchService, true);

            }

            else if (InterestLastBatchService == null)
            {
                BatchId = CreateBatchInvoice(interestReportArgs, tenant, email);
                InterestLastBatchService = new InterestLastBatchServicePM();
                InterestLastBatchService.Tenant = tenant;
                InterestLastBatchService.CreateInvoicesBatchId = BatchId;
                InterestLastBatchService.ChangeSetOp = ChangeSetOperation.Insert;
                interestLastBatchServiceUpdateService.Update(InterestLastBatchService, true);

            }

            return BatchId;

        }


        private static bool IsUser(string email, int tenant)
        {
            bool isUser = false;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                IGlobalContext globalContext = GlobalContext.GetContext();
                isUser = globalContext.GlobalContacts.Where(c => c.Email == email && (c.GlobalTenantId == tenant || c.GlobalTenantId == 0) && c.IsUser == true).Any();
            }
            return isUser;
        }

        public bool CheckAvailablityTenantsForEmail(string email, int tenant)
        {
            UserRepository userRep = new UserRepository(0);
            Simplog.Data.CommonDataModel.EntityPOCOs.User user = userRep.GetSingleUserByCodeOrEmail(null, email, 0, false);

            bool available = true;
            if (user != null)
            {
                TenantManagementRepository tenantManagementRep = new TenantManagementRepository();
                bool isDistributorToCurrentTenant = tenantManagementRep.CheckDistributor(user.DistributorCode, tenant);
                if (user.IsDistributor)
                {
                    if (isDistributorToCurrentTenant)
                    {
                        available = true;
                    }
                    else
                    {
                        available = false;
                    }
                }
                else
                {
                    available = true;
                }
            }
            else
            {
                ContactRepository contactRep = new ContactRepository(tenant);
                available = contactRep.CheckEmailAvailabilityForTenant(email, tenant);
            }
            return available;
        }

        private bool PrintInvoicesPDF(string Email, int? tenant, string SelectId, PdfDocument pdfDoc,string DocumentCode,bool IsForChecked)
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

                        if (doucmentOut.DocumentsFiling.DocumentType.IsDocumentOneTimePrintLimited && doucmentOut.DocumentsFiling.DocumentType.LimitedPrintCopyId == copy.DocumentTypeCopyId && !string.IsNullOrEmpty(copy.LastPrintedByUserId))
                        {
                            includeInPrint = false;
                        }
                        else if (doucmentOut.DocumentsFiling.DocumentType.IsDocumentOneTimePrintLimited && doucmentOut.DocumentsFiling.DocumentType.LimitedPrintCopyId == copy.DocumentTypeCopyId)
                        {
                            UserRepository userRep = new UserRepository((int)tenant);

                            User printedBy = null;
                            if (!string.IsNullOrEmpty(userId)) printedBy = userRep.GetSingleUser(userId, (int)tenant);
                            else printedBy = userRep.GetSingleUserByCodeOrEmailForTenant(null, email, (int)tenant, false);


                            DocumentOutCopyRepository myRep = new DocumentOutCopyRepository((int)tenant);
                            DocumentOutCopy documentoutCopy = myRep.GetSingleDocumentOutCopyByTenant(copy.Id, (int)tenant);
                            documentoutCopy.LastPrintDate = TenantServerConfigration.GetCurrentDateTime((int)tenant);
                            documentoutCopy.LastPrintedByUserId = printedBy.Id;
                            myRep.Update(documentoutCopy);
                            myRep.SubmitChanges();
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
                                    }
                                }
                            }
                         

                            break;
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
        public string CreateBatchInvoice(InterestReportArguments interestReportArgs, int tenant, string email)
        {
            string BatchId = null;
            interestReportArgs.Tenant = tenant;
            interestReportArgs.Email = email;
            BatchId = CreateBatchTaskExecution(interestReportArgs, "Create Batch Invoice", "Logitude.Accounting.BL.CoreBL.Batch.BatchInterestReportInvoiceService,Logitude.Accounting.BL");

            return BatchId;
        }
        public HttpResponseMessage GetInterestLastBatchServiceByTenant()
        {
            try
            {
                int tenant = AuthinticateTenant();
                InterestLastBatchServiceQueryService interestLastBatchServiceQueryService = new InterestLastBatchServiceQueryService(tenant);
                InterestLastBatchServicePM InterestLastBatchService = interestLastBatchServiceQueryService.CheckInterestLastBatchServicesByTenant(tenant);

                if (InterestLastBatchService != null && !string.IsNullOrEmpty(InterestLastBatchService.CreateInvoicesBatchId))
                {
                    BatchTaskExecutionQueryService batchTaskExecutionQueryService = new BatchTaskExecutionQueryService(tenant);
                    BatchTaskExecutionPM batchTaskExecutionPM = batchTaskExecutionQueryService.GetSingle(InterestLastBatchService.CreateInvoicesBatchId, false, false);

                    if (batchTaskExecutionPM.StatusCode != "D" && batchTaskExecutionPM.StatusCode != "F")
                    {
                        ContactPM contact = GetLoggedContact(tenant);
                        bool showLocals = !contact.DontShowLocal;
                        throw new Exception(TextCodesTranslator.TranslateText("InterestReport.O.AnotherBatchInvoiceStillInProgress", tenant, showLocals));
                    }
                }

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


                InterestLastBatchServiceQueryService interestLastBatchServiceQueryService = new InterestLastBatchServiceQueryService(tenant);
                InterestLastBatchServicePM InterestLastBatchService = interestLastBatchServiceQueryService.CheckInterestLastBatchServicesByTenant(tenant);

                if (InterestLastBatchService != null && !string.IsNullOrEmpty(InterestLastBatchService.CreateInvoicesBatchId))
                {
                    BatchTaskExecutionQueryService batchTaskExecutionQueryService = new BatchTaskExecutionQueryService(tenant);
                    BatchTaskExecutionPM batchTaskExecutionPM = batchTaskExecutionQueryService.GetSingle(InterestLastBatchService.CreateInvoicesBatchId, false, false);

                    if (batchTaskExecutionPM.StatusCode != "D" && batchTaskExecutionPM.StatusCode != "F")
                    {
                        ContactPM contact = GetLoggedContact(tenant);
                        bool showLocals = !contact.DontShowLocal;
                        throw new Exception(TextCodesTranslator.TranslateText("InterestReport.O.AnotherBatchInvoiceStillInProgress", tenant, showLocals));
                    }
                }
                interestReportArgs.Tenant = tenant;
                interestReportArgs.Email = email;

                InterestReportQueryService interestReportQueryService = new InterestReportQueryService(tenant);
                int NumberOfInterestReportsWithoutInvoice = interestReportQueryService.GetInterestReportsBySelectedIds(interestReportArgs).Count();

                return Request.CreateResponse(HttpStatusCode.OK, NumberOfInterestReportsWithoutInvoice);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        private void UpdateStatusForALLNotInvoicedInterestReports(InterestReportArguments interestReportArgs, int tenant)
        {
            InterestReportQueryService interestReportQueryService = new InterestReportQueryService(tenant);

            List<InterestReportPM> interestReports = interestReportQueryService.GetNotInvoicedInterestReportsByDates(interestReportArgs.FromDate, interestReportArgs.ToDate, tenant);

            if (interestReportArgs.ExcludedIds != null)
            {
                interestReports = (from a in interestReports
                                   where !interestReportArgs.ExcludedIds.Contains(a.Id)
                                   select a).ToList();
            }
            UpdateInterestReports(interestReports, tenant);
        }
        private void UpdateStatusForSelectedInterestReport(InterestReportArguments interestReportArgs, int tenant)
        {
            InterestReportQueryService interestReportQueryService = new InterestReportQueryService(tenant);
            List<InterestReportPM> interestReports = interestReportQueryService.GetInterestReportsByIds(interestReportArgs.SelectedIds, tenant);
            UpdateInterestReports(interestReports, tenant);

        }
        private void UpdateInterestReports(List<InterestReportPM> interestReports, int tenant)
        {
            var accountingContext = AccountingContext.GetContext(tenant);
            foreach (InterestReportPM report in interestReports)
            {
                report.InterestReportStatusCode = "8";
                report.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                InterestReportUpdateService service = new InterestReportUpdateService(accountingContext, new Dictionary<string, IContext>(), tenant);
                service.Update(report, true);
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

        private string CreateBatchTaskExecution(InterestReportArguments args, string Subject, string ClassName)
        {
            // 1- create BTE record
            BatchTaskExecutionPM taskExe;

            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(typeof(InterestReportArguments));
            serializer.Serialize(stringwriter, args);
            string xmlParameters = stringwriter.ToString();


            taskExe = new BatchTaskExecutionPM()
            {
                Subject = Subject,
                Tenant = args.Tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ClassName = ClassName,
                CreateDate = DateTime.Now,
                PrametersXml = xmlParameters,
                StatusCode = "C",

            };


            IInfrastructureContext MyContext = InfrastructureContext.GetContext(args.Tenant);
            BatchTaskExecutionUpdateService bteUpdateService = new BatchTaskExecutionUpdateService(MyContext, new Dictionary<string, IContext>(), args.Tenant);
            bteUpdateService.Update(taskExe, true);

            // 2- Send to queue
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("batchtaskexecutionqueue", 0);
            queueservice.Send(new Dictionary<string, string>()
                {
                    { "BatchTaskExecutionId", taskExe.Id },
                    { "Tenant",  args.Tenant.ToString() }
                }, args.Tenant);


            return taskExe.Id;
        }

        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }
        public static ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }


    }


    public class PDFDocumentInvoices
    {
        public StreamContent Document { set; get; }
        public List<string> ARInvoiceNumbersNotPrinted { set; get; }
        public  List<string> InterestReportNumbersNotPrinted { set; get; }
    }
}