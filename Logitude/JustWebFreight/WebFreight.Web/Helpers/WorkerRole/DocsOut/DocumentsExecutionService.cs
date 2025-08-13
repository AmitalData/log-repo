using Logitude.Server.Tools;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.SystemLogs;
using WebFreight.Web.DataContracts;
using Logitude.Server.Tools.Helpers;
using System.Threading.Tasks;
using Logitude.BL.CommonDataModel.EntityQueries;
using System.Threading;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.DataContracts;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.Server.Tools.EntityChanges;
using WebFreight.Web.Helpers.CallBack;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using System.Transactions;
using Newtonsoft.Json;
using Logitude.BL.QuoteModel.Tools.EntityService;
using Simplog.Data.QuoteModel;
using Microsoft.Practices.Unity;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel.APIDataContract.ApiV1;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using System.Collections.Concurrent;


namespace WebFreight.Web.Helpers.WorkerRole.DocsOut
{
    public class DocumentsExecutionService
    {
        private DbQueueService queueService = null;
        private QueueResponse queueResponse = null;
        private int? tenant =null;
        private string documentsExecutionLogId = string.Empty;
        private string communicationLogId = string.Empty;
        private string versionNumber = string.Empty;
        private string quoteTemplateId = string.Empty;
        private string updatedByUserId = string.Empty;
        private string isGenerate = string.Empty;
        
        private string callBackDetailsXml = string.Empty;
        private DocumentsExecutionLogRepository documentsExecutionLogRepository = null;
        private DocumentsExecutionLog documentsExecutionLog = null;
        private DateTime startDate = DateTime.Now;
        private string childObjectTableName = string.Empty;
        private const string AR_INVOICE = "ARInvoice";
        private const string AP_INVOICE = "APInvoice";  
        public DocumentsExecutionService(DbQueueService queueService, QueueResponse queueResponse)
        {
            this.queueService = queueService;
            this.queueResponse = queueResponse;
            if (queueService != null && queueResponse != null)
            {
                documentsExecutionLogId = queueResponse.MessageValues != null && queueResponse.MessageValues.Keys.Contains("DocumentsExecutionLogId") ? queueResponse.MessageValues["DocumentsExecutionLogId"].ToString() : "";
                tenant = GetTenantValueFromQueueResponse(queueResponse);
                callBackDetailsXml = queueResponse.MessageValues != null && queueResponse.MessageValues.Keys.Contains("CallBackDetailsXml") ? queueResponse.MessageValues["CallBackDetailsXml"].ToString() : "";
                communicationLogId = queueResponse.MessageValues != null && queueResponse.MessageValues.Keys.Contains("communicationLogId") ? queueResponse.MessageValues["communicationLogId"].ToString() : "";
                versionNumber = queueResponse.MessageValues != null && queueResponse.MessageValues.Keys.Contains("versionNumber") ? queueResponse.MessageValues["versionNumber"].ToString() : "";
                quoteTemplateId = queueResponse.MessageValues != null && queueResponse.MessageValues.Keys.Contains("quoteTemplateId") ? queueResponse.MessageValues["quoteTemplateId"].ToString() : "";
                updatedByUserId = queueResponse.MessageValues != null && queueResponse.MessageValues.Keys.Contains("updatedByUserId") ? queueResponse.MessageValues["updatedByUserId"].ToString() : "";
                isGenerate = queueResponse.MessageValues != null && queueResponse.MessageValues.Keys.Contains("isGenerate") ? queueResponse.MessageValues["isGenerate"].ToString() : "";
            }
        }

        public void ExecuteDocumentsExecutionQueue()
        {
            try
            {
                if (!string.IsNullOrEmpty(communicationLogId)) {
                    UpdateCommunicationLogsDocuments();
                }
                else if (queueService != null && queueResponse!=null)
                {
                    documentsExecutionLog = GetDocumentsExecutionLog();
                    if (documentsExecutionLog != null && documentsExecutionLog.RetryNumber < 2 && documentsExecutionLog.CreateDate > DateTime.Now.AddMinutes(-5) &&  (documentsExecutionLog.StatusCode == "W" || documentsExecutionLog.StatusCode == "P"))
                    {
                        UpdateDocumentsExecutionLog(new DocumentsExecutionLogArgs() { StartDate = startDate, StatusCode = "P" });
                        ExportStimulDocumentToPDF();
                        if (!string.IsNullOrEmpty(callBackDetailsXml)) CallBackService.Notifiy(callBackDetailsXml, null);
                      
                    }
                    else
                    {
                        ExceptionHandler.HandleException(new Exception("Document build failed after 3 retries or it reaches the time out.Please try again.If the issue is persistent then please kindly contact our Customer Support"), DateTime.Now, 0, null, "WorkerRole Monitor", null,  System.Environment.MachineName);
                        string error = "";
                        if (documentsExecutionLog == null)
                        {
                            error += "documentsExecutionLog is null";
                        }
                        else
                        {
                            error += "documentsExecutionLog.RetryNumber = " + documentsExecutionLog.RetryNumber + ", documentsExecutionLog.CreateDate = " + documentsExecutionLog.CreateDate + ", documentsExecutionLog.StatusCode = " + documentsExecutionLog.StatusCode;
                        }
                        UpdateDocumentsExecutionLog(new DocumentsExecutionLogArgs() {Exception= new Exception("Document build failed after 3 retries or it reaches the time out.Please try again.If the issue is persistent then please kindly contact our Customer Support" + error), DoneDate = DateTime.Now, StartDate = startDate, StatusCode = "F" });                      
                        queueService.Complete();
                    }
                }
            }
            catch (AggregateException aggregateException)
            {
                var excep = new Exception("aggregateException exception");
                foreach (var exception in aggregateException.Flatten().InnerExceptions)
                {
                    excep = exception;
                    ExceptionHandler.HandleException(exception, DateTime.Now, 0, null, "Document execution queue worker role start", null, null);
                    ExceptionHandler.HandleException(exception, DateTime.Now, 0, null, "Doc WorkerRole Monitor|" + "Aggr Catch ExecuteDocumentsExecutionQueue", null, System.Environment.MachineName);
                }
                try
                {
                    UpdateDocumentsExecutionLog(new DocumentsExecutionLogArgs() { Exception = excep, DoneDate = DateTime.Now, StartDate = startDate, StatusCode = "F" });
                    queueService.CompleteAsFailed();
                }
                catch (Exception ex)
                {

                    ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Doc WorkerRole Monitor|" + "Catch ExecuteDocumentsExecutionQueue", " inside Aggr catch exception while running ", System.Environment.MachineName);
                }
                Thread.Sleep(new TimeSpan(0, 0, 0, 0, 250));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception, DateTime.Now, 0, null, "Document execution queue worker role start", null, null);
                ExceptionHandler.HandleException(exception, DateTime.Now, 0, null, "Doc WorkerRole Monitor|" + "Catch ExecuteDocumentsExecutionQueue", null, System.Environment.MachineName);
                try
                {
                    UpdateDocumentsExecutionLog(new DocumentsExecutionLogArgs() { Exception = exception, DoneDate = DateTime.Now, StartDate = startDate, StatusCode = "F" });
                    queueService.CompleteAsFailed();
                }
                catch (Exception ex)
                {

                    ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Doc WorkerRole Monitor|" + "Catch ExecuteDocumentsExecutionQueue", " inside catch exception while running UpdateDocumentsExecutionLog", System.Environment.MachineName);
                }

                Thread.Sleep(new TimeSpan(0, 0, 0, 0, 250));
            }
        }

        private void UpdateCommunicationLogsDocuments() {
            ICommonDataContext context = CommonDataContext.GetContext(tenant.Value);
            CommunicationLogRepository communicationLogRep = new CommunicationLogRepository(context);
            CommunicationLog commLog = communicationLogRep.GetSingleCommunicationLog(communicationLogId, tenant.Value);
            var email = "system@tenant" + tenant.Value + ".com";
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = commLog.Document.Id,
                FolderName = commLog.Document.Folder,
                Extension = commLog.Document.Extension,
                Tenant = commLog.Document.Tenant,
                FileSize = commLog.Document.FileSize,
            };
            Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
            if (commLog != null && commLog.Subject == "Update Quote Document")
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        
                        byte[] objectData = storageservice.Read(fileInfo);
                        var jsonObject = System.Text.Encoding.Default.GetString(objectData);
                        var quotePM = JsonConvert.DeserializeObject<QuotePM>(jsonObject);
                        IQuotesContext MyContext = QuotesContext.GetContext(tenant.Value);
                        QuoteService service = new QuoteService(MyContext, tenant.Value, email);

                        service.UpdateQuoteDocuments(quotePM);
                        commLog.CommunicationStatusTypeCode = "D";
                        commLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(commLog.Tenant);
                        commLog.DoneDateUTC = DateTime.UtcNow;
                        commLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(commLog.Tenant);
                        commLog.LastStatusDateUTC = DateTime.UtcNow;
                        communicationLogRep.Update(commLog);
                        communicationLogRep.SubmitChanges();
                        scope.Complete();
                    }
                    queueService.Complete();
                }
                catch (Exception ex)
                {
                    Communications.UpdateCommunicationLogStatus(commLog.Id, tenant.Value, null, "F", null + DateTime.Now.ToString(), ex.Message);
                    queueService.Complete();
                }
            }
            else if (commLog != null && commLog.Subject == "Update Quote Document2") {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        IQuotesContext MyContext = QuotesContext.GetContext(tenant.Value);
                        QuoteService service = new QuoteService(MyContext, tenant.Value, email);
                        var pdfData = service.BuildQuoteTemplatePdfDocument(commLog.EntityId, int.Parse(versionNumber), quoteTemplateId, updatedByUserId, tenant.Value, Boolean.Parse(isGenerate));
                        storageservice.Write(pdfData, fileInfo);
                        commLog.CommunicationStatusTypeCode = "D";
                        commLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(commLog.Tenant);
                        commLog.DoneDateUTC = DateTime.UtcNow;
                        commLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(commLog.Tenant);
                        commLog.LastStatusDateUTC = DateTime.UtcNow;
                        communicationLogRep.Update(commLog);
                        communicationLogRep.SubmitChanges();
                        scope.Complete();
                    }
                    queueService.Complete();
                }
                catch (Exception ex)
                {
                    Communications.UpdateCommunicationLogStatus(commLog.Id, tenant.Value, null, "F", null + DateTime.Now.ToString(), ex.Message);
                    queueService.Complete();
                }
            }
            else if (commLog != null && commLog.Subject == "Preview Quote Document")
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        QuoteTemplateReportHelper quoteTemplateReportHelper = new QuoteTemplateReportHelper();
                        var pdfData = quoteTemplateReportHelper.BuildQuoteTemplatePdfReport(commLog.EntityId, quoteTemplateId, updatedByUserId, tenant.Value, null, tenant.Value);

                        storageservice.Write(pdfData, fileInfo);
                        commLog.CommunicationStatusTypeCode = "D";
                        commLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(commLog.Tenant);
                        commLog.DoneDateUTC = DateTime.UtcNow;
                        commLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(commLog.Tenant);
                        commLog.LastStatusDateUTC = DateTime.UtcNow;
                        communicationLogRep.Update(commLog);
                        communicationLogRep.SubmitChanges();
                        scope.Complete();
                    }
                    queueService.Complete();
                }
                catch (Exception ex)
                {
                    Communications.UpdateCommunicationLogStatus(commLog.Id, tenant.Value, null, "F", null + DateTime.Now.ToString(), ex.Message);
                    queueService.Complete();
                }
            }
        }
        public void ExecuteDocumentsV2ExecutionQueue()
        {
            try
            {
                documentsExecutionLog = GetDocumentsExecutionLog();
                if (HaveDocumentExecutionErrorMessage())
                {
                    throw new ApplicationException(GetDocumentExecutionErrorMessage());
                }
                UpdateDocumentsExecutionLog(new DocumentsExecutionLogArgs() { StartDate = startDate, StatusCode = "P" });
                ExportStimulDocumentToPDF(true);
                if (!string.IsNullOrEmpty(callBackDetailsXml)) CallBackService.Notifiy(callBackDetailsXml, null);
            }
            catch (AggregateException aggregateException)
            {
                Exception lastAggregateException = new Exception("aggregateException exception");
                foreach (var exception in aggregateException.Flatten().InnerExceptions)
                {
                    lastAggregateException = exception;
                    ExceptionHandler.HandleException(exception, DateTime.Now, 0, null, "Document Execution V2 WorkerRole Monitor|" + "Aggr Catch ExecuteDocumentsExecutionQueue", null, System.Environment.MachineName);
                }
                HandleDocumentExecutionException(lastAggregateException);
            }
            catch (Exception exception)
            {
                HandleDocumentExecutionException(exception);
            }
        }

        private bool HaveDocumentExecutionErrorMessage()
        {
            if (documentsExecutionLog == null) return true;
            if(documentsExecutionLog.RetryNumber >= 2) return true;
            if(documentsExecutionLog.CreateDate <= DateTime.Now.AddMinutes(-5)) return true;
            if(documentsExecutionLog.StatusCode != "W" && documentsExecutionLog.StatusCode != "P") return true;
            return false;
        }

        private void HandleDocumentExecutionException(Exception exception)
        {           
            try
            {
                UpdateDocumentsExecutionLog(new DocumentsExecutionLogArgs() { Exception = exception, DoneDate = DateTime.Now, StartDate = startDate });
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Document Execution V2 WorkerRole Monitor|" + "Catch ExecuteDocumentsExecutionQueue", " inside Aggr catch exception while running ", System.Environment.MachineName);
            }
            throw new ApplicationException(exception.Message, exception.InnerException);
        }

        private string GetDocumentExecutionErrorMessage()
        {
            if (documentsExecutionLog == null)
                return "Cannot Find Documents Execution Log with Id = " + documentsExecutionLogId + " and Tenant = " + tenant;

            if (documentsExecutionLog.RetryNumber >= 2)
                return "Document build failed after 3 retries. Please try again.";

            if (documentsExecutionLog.CreateDate <= DateTime.Now.AddMinutes(-5)) 
                return "Document build failed since it reached the time out.";

            if (documentsExecutionLog.StatusCode != "W" && documentsExecutionLog.StatusCode != "P")
                return "Document build failed since execution log status code is " + documentsExecutionLog.StatusCode + ".";

            return "Document build failed: Unhandled Error";
        }

        private readonly object _locker = new object();
        private void ExportStimulDocumentToPDF(bool isVersion2 = false)
        {
            ExportDocumentArgs exportDocumentArgs = !string.IsNullOrEmpty(documentsExecutionLog.RequestXML) ? LogitudeXmlSerializer.DeserializeObject<ExportDocumentArgs>(documentsExecutionLog.RequestXML) : null;
            if (exportDocumentArgs != null)
            {
                bool skip = false;
                string objectTableName = !string.IsNullOrEmpty(exportDocumentArgs.ObjectTableId) ? ObjectTableRepository.GetNameById(exportDocumentArgs.ObjectTableId, exportDocumentArgs.Tenant) : "";
                if (objectTableName == AR_INVOICE && !string.IsNullOrEmpty(exportDocumentArgs.EntityId))
                {
                    ARInvoiceQuery aRInvoiceQuery = new ARInvoiceQuery(exportDocumentArgs.Tenant);
                    var aRInvoice = aRInvoiceQuery.GetSingleARInvoice(exportDocumentArgs.EntityId, exportDocumentArgs.Tenant);
                    if (aRInvoice != null)
                    {
                        string signStatus = aRInvoice.IsSigned;
                        if (signStatus == "1" || signStatus == "3" || signStatus == "4")
                        {
                            skip = true;
                            NetCommonHelper.Logger.DevLog.Instance.WriteTrace("Skip Printing Signed ARInvoice [{aRInvoice.InvoiceNumber}] Tenant=[{aRInvoice.Tenant}] IsSigned=[{signStatus}]" +
                                JsonConvert.SerializeObject(exportDocumentArgs));
                        }
                    }
                }
                if (!skip)
                {
                    childObjectTableName = !string.IsNullOrEmpty(exportDocumentArgs.ChildObjectTableId) ? ObjectTableRepository.GetNameById(exportDocumentArgs.ChildObjectTableId, exportDocumentArgs.Tenant) : "";
                    List<DocumentTypeCopiesDetails> documentTypeCopiesDetails = new List<DocumentTypeCopiesDetails>();
                    string authenticatedUserEmail = GetContactEmailByContactId(exportDocumentArgs.LoggedContactId, exportDocumentArgs.Tenant);

                    if (childObjectTableName == AR_INVOICE || exportDocumentArgs.ObjectTableName == AR_INVOICE)
                    {
                        DocumentHelper DocumentHelper = new DocumentHelper();
                        IFullAccountingSettingQueryServiceExt query = ContainerAccessor.Container.Resolve(typeof(IFullAccountingSettingQueryServiceExt), "FullAccountingSettingQueryServiceExt", new ParameterOverride("", 1)) as IFullAccountingSettingQueryServiceExt;
                        FullAccountingSettingPM accountingSettings = query.GetFullAccountingSettingByTenant(exportDocumentArgs.Tenant);



                        DocumentTypeCopyQuery documentTypeCopyQuery = new DocumentTypeCopyQuery(exportDocumentArgs.Tenant);
                        List<DocumentTypeCopyPM> typeCopies = documentTypeCopyQuery.GetDocumentTypeCopiesByTenant(exportDocumentArgs.Tenant);
                        if (typeCopies != null && typeCopies.Count > 0)
                        {
                            DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(exportDocumentArgs.Tenant);
                            List<string> documentTypes = new List<string>();
                            List<string> entityIds = new List<string>();
                            foreach (string typeCopyId in exportDocumentArgs.DocumentTypeCopyIdsList)
                            {
                                DocumentTypeCopyPM tcPM = typeCopies.Where(tcy => tcy.Id == typeCopyId).FirstOrDefault();
                                if (tcPM != null)
                                {
                                    string docType = tcPM.DocumentTypeId;
                                    if (!string.IsNullOrEmpty(docType))
                                    {
                                        documentTypes.Add(docType);
                                    }
                                    if (objectTableName == AR_INVOICE && !string.IsNullOrEmpty(exportDocumentArgs.EntityId) && !entityIds.Contains(exportDocumentArgs.EntityId))
                                    {
                                        entityIds.Add(exportDocumentArgs.EntityId);
                                    }
                                    if (childObjectTableName == AR_INVOICE && !string.IsNullOrEmpty(exportDocumentArgs.ChildEntityId) && !entityIds.Contains(exportDocumentArgs.ChildEntityId))
                                    {
                                        entityIds.Add(exportDocumentArgs.ChildEntityId);
                                    }

                                }
                            }
                            if (documentTypes.Count > 0)
                            {
                                List<DocumentsFilingList> documentsFilings = documentsFilingQuery.GetDocumentsFilingListsByDocumentTypeIdsAndEntityId(documentTypes, entityIds, exportDocumentArgs.Tenant);
                                if (documentsFilings != null && documentsFilings.Count > 0)
                                {
                                    ConcurrentBag<string> toDelete = new ConcurrentBag<string>();


                                    Parallel.ForEach(documentsFilings, (documentsFiling) =>
                                    { 
                                        string docId = documentsFiling.DocumentId;
                                        if (!string.IsNullOrEmpty(docId))
                                        {
                                            bool okGotFromStorage = false;
                                            okGotFromStorage = DocumentHelper.CheckPDFInvoiceInStorage(documentsFiling.Id, exportDocumentArgs.Tenant, accountingSettings, exportDocumentArgs.LoggedContactId);
                                            if (okGotFromStorage)
                                            {
                                                lock (_locker)
                                                {
                                                    string typeCopyToDelete = typeCopies.Where(tc => tc.DocumentTypeId == documentsFiling.DocumentTypeId).FirstOrDefault().Id;
                                                    toDelete.Add(typeCopyToDelete);
                                                }
                                            }
                                        }
                                    });
                                    if (toDelete.Count > 0)
                                    {
                                        foreach (string copyId in toDelete)
                                        {
                                            exportDocumentArgs.DocumentTypeCopyIdsList.Remove(copyId);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    ConcurrentBag<DocumentTypeCopiesDetails> localDocumentDetails = new ConcurrentBag<DocumentTypeCopiesDetails>();
                    Parallel.ForEach(exportDocumentArgs.DocumentTypeCopyIdsList, (documentTypeCopyId) =>
                    {
                        AuthenticationUtil.AuthenticatedUserEmail = authenticatedUserEmail;
                        ExportDocumentHelper exportDocumentHelper = new ExportDocumentHelper();
                        string result = exportDocumentHelper.ExportDocument2Pdf(exportDocumentArgs, documentTypeCopyId);


                        localDocumentDetails.Add(new DocumentTypeCopiesDetails
                        {
                            DocumentTypeCopyId = documentTypeCopyId,
                            DocumentId = result
                        });
                    });
                    lock (_locker)
                    {
                        documentTypeCopiesDetails.AddRange(localDocumentDetails);
                    }


                    if (exportDocumentArgs.DocumentTypeCopyIdsList != null && exportDocumentArgs.DocumentTypeCopyIdsList.Count > 0)
                    {

                        UpdateDocumentOut(exportDocumentArgs);
                        if (childObjectTableName == AR_INVOICE || exportDocumentArgs.ObjectTableName == AR_INVOICE) UpdateARInvoicePrintingDetails(exportDocumentArgs, authenticatedUserEmail);

                        DocumentPopulateAutomaticDateUpdateService documentPopulateAutomaticDateUpdateService = new DocumentPopulateAutomaticDateUpdateService();
                        documentPopulateAutomaticDateUpdateService.Update(new DocumentPopulateAutomaticDateArgs() { EntityId = exportDocumentArgs.EntityId, ObjectTableName = exportDocumentArgs.ObjectTableName, ChildObjectTableId = exportDocumentArgs.ChildObjectTableId, ChildEntityId = exportDocumentArgs.ChildEntityId, DocumentTypeCode = exportDocumentArgs.CurrentDocumentTypeCode, ProcessType = "Print", Tenant = exportDocumentArgs.Tenant });
                        if (!(childObjectTableName == AP_INVOICE && string.IsNullOrWhiteSpace(exportDocumentArgs.EntityId)))
                        {
                            RunAutomation(exportDocumentArgs, "OnDocumentUpdate", documentTypeCopiesDetails);
                        }
                    }
                }
                UpdateDocumentsExecutionLog(new DocumentsExecutionLogArgs() {StatusCode = "D", DoneDate = DateTime.Now });
                if(!isVersion2) queueService.Complete();
            }
            else
            {
                UpdateDocumentsExecutionLog(new DocumentsExecutionLogArgs() { Exception = new Exception("RequestXML is null"), DoneDate = DateTime.Now, StartDate = startDate, StatusCode = "F" });
                if (!isVersion2) queueService.Complete();
            }
        }

        private void UpdateARInvoicePrintingDetails(ExportDocumentArgs exportDocumentArgs ,  string loggedUserEmail)
        {
            ARInvoicePrintDetailsService aRInvoicePrintDetailsService = new ARInvoicePrintDetailsService(exportDocumentArgs.Tenant, !string.IsNullOrWhiteSpace(exportDocumentArgs.ChildEntityId) ? exportDocumentArgs.ChildEntityId : exportDocumentArgs.EntityId , loggedUserEmail);
            aRInvoicePrintDetailsService.Update(exportDocumentArgs.CurrentDocumentOutId);

        }

        private void RunAutomation(ExportDocumentArgs exportDocumentArgs, string automationType, List<DocumentTypeCopiesDetails> documentTypeCopiesDetails)
        {
            GeneralEntityChangeService generalEntityChangeService = new GeneralEntityChangeService();
            EntityDetails entityDetails = generalEntityChangeService.GetEntityDetails(exportDocumentArgs.EntityId, exportDocumentArgs.ObjectTableName, exportDocumentArgs.Tenant);

            bool isHaveAutomation = generalEntityChangeService.CheckIfEntityHaveAutomation(entityDetails.CombinedObjectTableName, automationType, exportDocumentArgs.Tenant);
            if (!isHaveAutomation) return;

            MainEntityChangeService mainEntityChangeService = new MainEntityChangeService(new EntityChangeArgs() { EntityPM = entityDetails.EntityPM, ProcessType = automationType, ObjectTableName = entityDetails.ObjectTableName, EntityId = exportDocumentArgs.EntityId, Tenant = exportDocumentArgs.Tenant, StartDate = DateTime.Now, ExtraDetails = new OnUpdateDocumentDetails { Type = "Print", DocumentTypeCopiesDetails = documentTypeCopiesDetails }, OtherObjectTableName = entityDetails.OtherObjectTableName });
            mainEntityChangeService.AddEntityChange();
        }

        private void UpdateDocumentOut(ExportDocumentArgs exportDocumentArgs)
        {
            DocumentOutQuery documentOutQuery = new DocumentOutQuery(exportDocumentArgs.Tenant);
            DocumentOutPM documentOutPM = documentOutQuery.GetSinglePM(exportDocumentArgs.CurrentDocumentOutId, exportDocumentArgs.Tenant);
            if (documentOutPM != null)
            {
                documentOutPM.Issued = true;
                documentOutPM.NeedsRebuild = false;
                documentOutPM.Issued = true;
                documentOutPM.IssuedByUserId = exportDocumentArgs.LoggedContactId;
                documentOutPM.IsChangeIssuedDate = true;
                documentOutPM.DocumentTemplateId = exportDocumentArgs.DocumentTypeTemplateId;
                documentOutPM.DocumentTemplateEditorTool = exportDocumentArgs.DocumentTemplateEditorTool;
                ICommonDataContext objectContext = CommonDataContext.GetContext(documentOutPM.Tenant);
                DocumentOutService service = new DocumentOutService(objectContext, documentOutPM.Tenant);
                service.Update(documentOutPM, documentOutPM.DocumentOutCopies);
            }
        }

        private void UpdateDocumentsExecutionLog(DocumentsExecutionLogArgs documentsExecutionLogArgs)
        {
            if (documentsExecutionLog != null)
            {
                documentsExecutionLog.StatusCode = !string.IsNullOrEmpty(documentsExecutionLogArgs.StatusCode) ? documentsExecutionLogArgs.StatusCode : documentsExecutionLog.StatusCode;
                documentsExecutionLog.RetryNumber = queueResponse != null ? queueResponse.RetryNumber : documentsExecutionLog.RetryNumber;
                documentsExecutionLog.StartDate = documentsExecutionLogArgs.StartDate != null ? documentsExecutionLogArgs.StartDate : documentsExecutionLog.StartDate;
                documentsExecutionLog.ExceptionMessage = documentsExecutionLogArgs.Exception != null ? GetFullExceptionMessageFromException(documentsExecutionLogArgs.Exception) : documentsExecutionLog.ExceptionMessage;
                documentsExecutionLog.DoneDate = documentsExecutionLogArgs.DoneDate != null ? documentsExecutionLogArgs.DoneDate : documentsExecutionLog.DoneDate;
                documentsExecutionLog.ExecutedByServerName =!string.IsNullOrEmpty(System.Environment.MachineName) ? System.Environment.MachineName : documentsExecutionLog.ExecutedByServerName;
                if (documentsExecutionLog.RetryNumber >= 2 && documentsExecutionLog.StatusCode != "D" && documentsExecutionLogArgs.Exception != null)
                {
                    documentsExecutionLog.StatusCode = "F";
                    documentsExecutionLog.DoneDate = DateTime.Now;
                    queueService.CompleteAsFailed();
                }
                documentsExecutionLogRepository.Update(documentsExecutionLog);
                documentsExecutionLogRepository.SubmitChanges();

                if (!string.IsNullOrEmpty(callBackDetailsXml) && documentsExecutionLog.StatusCode == "F") CallBackService.Notifiy(callBackDetailsXml, null);
            }
        }



        private DocumentsExecutionLog GetDocumentsExecutionLog()
        {
            DocumentsExecutionLog documentsExecutionLog = null;
            if (!string.IsNullOrEmpty(documentsExecutionLogId) && tenant!=null)
            {
                documentsExecutionLogRepository = new DocumentsExecutionLogRepository((int)tenant);
                documentsExecutionLog = documentsExecutionLogRepository.GetSingleDocumentsExecutionLog(documentsExecutionLogId, (int)tenant);
            }

            return documentsExecutionLog;
        }

        private string GetContactEmailByContactId(string loggedContactId, int tenant)
        {
            string contactEmail = string.Empty;
            if (!string.IsNullOrEmpty(loggedContactId))
            {
                ContactQuery contactQuery = new ContactQuery(tenant);
                contactEmail = contactQuery.GetContactEmailById(loggedContactId, tenant);
                if (contactEmail == null && tenant != 0) contactEmail = contactQuery.GetContactEmailById(loggedContactId, 0);
            }
            return contactEmail;
        }

        private string GetFullExceptionMessageFromException(Exception exception)
        {
            var exceptionMessage = string.Empty;

            if (exception != null)
            {
                exceptionMessage = exception.Message;
                if (exception.InnerException != null)
                {
                    exceptionMessage = exceptionMessage + Environment.NewLine + exception.InnerException;
                }
                if (exception.StackTrace != null)
                {
                    exceptionMessage = exceptionMessage + Environment.NewLine + "Stack trace: " + exception.StackTrace;
                }
            }
            return exceptionMessage;
        }

        private int? GetTenantValueFromQueueResponse(QueueResponse queueResponse)
        {
            int? tenant = null;
            if (queueResponse.MessageValues != null && queueResponse.MessageValues.Keys.Contains("Tenant"))
            {
                string tenantString = queueResponse.MessageValues["Tenant"].ToString();
                if (!string.IsNullOrEmpty(tenantString)) tenant = int.Parse(tenantString);
            }
            return tenant;
        }

    }


    public class DocumentsExecutionLogArgs
    {
        public string ExceptionMessage { get; set; }
        public string StatusCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DoneDate { get; set; }
        public Exception Exception { get; set; }
        
    }
}