using System;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Stimulsoft.Report;
using System.IO;
using Atp.Pdf;
using Logitude.Server.Tools.StorageService;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Logitude.Server.Tools.Counters;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Simplog.Data.Helpers;
using WebFreight.Web.WebServices;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.Tools.EntityService;

namespace WebFreight.Web.Helpers.BatchPrint
{
    public abstract class BatchPrinter
    {
        public BatchPrinterArgs _batchPrinterArgs;
        public DocumentType documentType;
        public DocumentTypeCopy documentTypeCopy;
        public DocumentTypeTemplatePM template;
        public User printedBy;
        public BatchTaskExecutionUpdateService batchTaskExecutionUpdateService;
        public string documentOutId = null;
        public BatchPrinter(BatchPrinterArgs batchPrinterArgs)
        {
            _batchPrinterArgs = batchPrinterArgs;
            documentType = GetDecumentType();
            documentTypeCopy = GetDocumentTypeCopy();
            template = GetDocumentTypeTemplate();
            printedBy = GetUser();
            batchTaskExecutionUpdateService = GetBatchTaskExecutionUpdateService();
        }

        private BatchTaskExecutionUpdateService GetBatchTaskExecutionUpdateService()
        {
            IInfrastructureContext context = Logitude.Infrastructure.Data.InfrastructureContext.GetContext(_batchPrinterArgs.Tenant);
            var batchTaskExecutionUpdateService = new BatchTaskExecutionUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), _batchPrinterArgs.Tenant);
            return batchTaskExecutionUpdateService;
        }

        public abstract void CustomeValidation(PrintEntityKeys item);
        public abstract void AfterPrint(PrintEntityKeys item);

        public PrintingResult PrintDocuments()
        {
            List<ItemPrintingResult> itemPrintingResults = new List<ItemPrintingResult>();
            _batchPrinterArgs.BatchTaskExecution.ProgressPercentage = 0;
            foreach (var item in _batchPrinterArgs.EntityIds)
            {
                itemPrintingResults.Add(Print(item));
                IncresePrograse();
            }
            Document document = null;
            DocumentsFiling documentFiling = null;
            if (itemPrintingResults.Any(e => e.IsSuccessfullyPrinted))
            {
                PdfDocument pdfDoc = new PdfDocument();
                MargePdfs(pdfDoc, itemPrintingResults);
                MemoryStream memoryStream = GetMemoryStream(pdfDoc);
                document = UploadPDFToStorage(memoryStream, _batchPrinterArgs.Tenant, "multiprint");
                //documentFiling = BuildDcoumentFiling();
                documentFiling = GetDcoumentFiling();
            }

            PrintingResult printingResults = CreatePrintResult(document, itemPrintingResults, documentFiling);

            return printingResults;
        }
        private DocumentsFiling GetDcoumentFiling()
        {
            if (string.IsNullOrEmpty(documentOutId)) return null;
            DocumentsFilingRepository documentsFilingRepository = new DocumentsFilingRepository(_batchPrinterArgs.Tenant);
            return documentsFilingRepository.GetSingleDocumentsFiling(documentOutId, _batchPrinterArgs.Tenant);
        }

        //private DocumentsFiling BuildDcoumentFiling()
        //{
        //    ICommonDataContext objectContext = CommonDataContext.GetContext(_batchPrinterArgs.Tenant);
        //    DocumentsFilingRepository documentsFilingRepository = new DocumentsFilingRepository(objectContext);
        //    DocumentOutRepository documentOutRepository = new DocumentOutRepository(objectContext);
        //    var newDocumentFiling = CreateDcoumentFiling();
        //    documentsFilingRepository.Add(newDocumentFiling);
        //    DocumentOut newDocument = CreateDocumentOutInstance(printedBy.Id, template.Id, documentType.DocumentTypeDefaultHTMLTemplateId);
        //    newDocument.Id = newDocumentFiling.Id;
        //    documentOutRepository.Add(newDocument);
        //    objectContext.SaveChanges();
        //    return newDocumentFiling;

        //}

        //private DocumentsFiling CreateDcoumentFiling()
        //{
        //    DocumentsFiling newDocumentFiling = new DocumentsFiling() { DocumentTypeId = documentType.Id, EntityId = null, Tenant = _batchPrinterArgs.Tenant, ObjectTableId = null, ChildEntityId = null, ChildEntityReference = null, DirectionCode = "O" };
        //    newDocumentFiling.Id = IdCounter.GetNumber("Document", _batchPrinterArgs.Tenant).ToString();
        //    newDocumentFiling.SecurityId = newDocumentFiling.Id + RandomString(10);
        //    newDocumentFiling.Code = CodeCounter.GetNumber("DocumentsFiling", _batchPrinterArgs.Tenant).ToString();
        //    newDocumentFiling.CreatedByUserId = printedBy.Id;
        //    newDocumentFiling.OwnerId = printedBy.Id;
        //    newDocumentFiling.UpdatedByUserId = printedBy.Id;
        //    newDocumentFiling.UpdateDate = TenantServerConfigration.GetCurrentDateTime(_batchPrinterArgs.Tenant);
        //    newDocumentFiling.CreateDate = TenantServerConfigration.GetCurrentDateTime(_batchPrinterArgs.Tenant);
        //    newDocumentFiling.SearchFields = newDocumentFiling.Code + "," + newDocumentFiling.DirectionCode;
        //    return newDocumentFiling;
        //}

        //private DocumentOut CreateDocumentOutInstance(string userId, string documentTemplateId, string emailTemplateId)
        //{
        //    DocumentOut documentOut = new DocumentOut() { EmailTemplateId = emailTemplateId, DocumentTemplateId = documentTemplateId, Tenant = _batchPrinterArgs.Tenant, Issued = false };
        //    documentOut.Issued = true;
        //    documentOut.IssuedDate = TenantServerConfigration.GetCurrentDateTime(_batchPrinterArgs.Tenant);
        //    documentOut.IssuedByUserId = userId;
        //    documentOut.NeedsRebuild = false;
        //    return documentOut;
        //}
        private void IncresePrograse()
        {
            _batchPrinterArgs.BatchTaskExecution.ProgressPercentage++;
            batchTaskExecutionUpdateService.Update(_batchPrinterArgs.BatchTaskExecution, true);
        }

        private PrintingResult CreatePrintResult(Document document, List<ItemPrintingResult> itemPrintingResults, DocumentsFiling documentFiling)
        {
            var result = new PrintingResult();
            if (document != null)
            {
                result.DocumentId = document.Id;
                result.FileName = document.FileName;
            }
            if(documentFiling != null)
            {
                result.SecurityId = documentFiling.SecurityId;
            }
            
            result.NotValidRows = itemPrintingResults.Where(e => !e.IsSuccessfullyPrinted).Select(e => new PrintingRow() 
            { 
                EntityId = e.EntityId,
                EntityNumber = e.EntityNumber,
                Error = e.Error 
            }).ToList();
            return result;
        }

        private MemoryStream GetMemoryStream(PdfDocument pdfDoc)
        {
            MemoryStream memoryStream = new MemoryStream();
            pdfDoc.Save(memoryStream);
            memoryStream.Position = 0;
            return memoryStream;
        }

        private void MargePdfs(PdfDocument pdfDoc, List<ItemPrintingResult> printingResults)
        {
            foreach (var item in printingResults)
            {
                if (item.IsSuccessfullyPrinted)
                    PdfDocumentBase.Merge(pdfDoc, item.DocumentStream);
            }
        }
        private string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private Document UploadPDFToStorage(MemoryStream memoryStream, int tenant,string folderName)
        {
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            byte[] ByteData = memoryStream.ToArray();
            DocumentRepository documentRepository = new DocumentRepository(tenant);
            string fileName = "Documents_MultiPrint" + DateTime.Now.ToString("dd-MM-yyy");
            Document document = new Document()
            {
                FileName = fileName,
                CreateDate = DateTime.Now,
                Extension = "pdf",
                FileSize = ByteData.Length,
                Tenant = tenant,
                Id = IdCounter.GetNumber("Document", tenant),
                HasFile = true,
                Folder = folderName,
            };

            documentRepository.Add(document);
            documentRepository.SubmitChanges();

            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = folderName,
                Extension = document.Extension,
                Tenant = tenant,
                FileSize = document.FileSize,
            };

            storageservice.Write(ByteData, fileInfo);
            return document;
        }
        private ItemPrintingResult Print(PrintEntityKeys item)
        {
            var result = new ItemPrintingResult(item.EntityId, item.EntityNumber);

            if (!CheckValidation(item, result))
                return result;
            try
            {

                var stream = BuildReportStream(item);
                
                result.DocumentStream = stream;
                result.IsSuccessfullyPrinted = true;

                

                AfterPrint(item);
                return result;
            }

            catch (Exception e)
            {
                result.IsSuccessfullyPrinted = false;
                result.Error = e.Message;
                return result;
            }
        }
        private void UpdateDocumentOut(DocumentOutPM documentOutPM)
        {

            documentOutPM.Issued = true;
            documentOutPM.NeedsRebuild = false;
            documentOutPM.Issued = true;
            documentOutPM.IssuedByUserId = printedBy.Id;
            documentOutPM.IsChangeIssuedDate = true;
           
            ICommonDataContext objectContext = CommonDataContext.GetContext(documentOutPM.Tenant);
            DocumentOutService service = new DocumentOutService(objectContext, documentOutPM.Tenant);
            service.Update(documentOutPM, documentOutPM.DocumentOutCopies);

        }
        private DocumentOutPM BuildDocumentOut(PrintEntityKeys item)
        {
            var documentOut = GetDocumentOutPM(item);
            if (documentOut != null)
                return documentOut;
            documentOut = CreateDcoumentOutPM(item);
            return documentOut;
        }
        private DocumentOutPM GetDocumentOutPM(PrintEntityKeys item)
        {
            DocumentOutQuery documentOutQuery = new DocumentOutQuery(_batchPrinterArgs.Tenant);
            DocumentOutPM documentOutPM = documentOutQuery.GetDocumentOutByDocumentTypeEntityAndChild(item.EntityId, item.ChildEntityId, _batchPrinterArgs.DocumentTypeId, _batchPrinterArgs.Tenant);
            return documentOutPM;
        }
        private DocumentOutPM CreateDcoumentOutPM(PrintEntityKeys item)
        {
            DocumentHelper documentHelper = new DocumentHelper();
            return documentHelper.CreateDocumentOut(_batchPrinterArgs.DocumentTypeId, item.EntityId, item.ChildEntityId, null, item.ObjectTableId, _batchPrinterArgs.Tenant, printedBy.Id);
        }

       // public bool isPrintedPreviously = false;
        private MemoryStream BuildReportStream(PrintEntityKeys item)
        {
            var printedCopy = GetPrintedCopy(item);
            if (printedCopy != null)
            {
                item.IsAlreadyPrinted = true;
                var stream = GetReportStreamFromCopy(printedCopy);
                if(stream != null) return stream;
            }
              
            var documentOut = BuildDocumentOut(item);
            documentOutId = documentOut?.Id;
            var reportStream = CreateReportStream(item , documentOut);
            var document = UploadPDFToStorage(reportStream, _batchPrinterArgs.Tenant, "docsout");
            AddDocumentOutCopy(document, item, documentOut);
            UpdateDocumentOut(documentOut);
            return reportStream;

        }

        private void AddDocumentOutCopy(Document document, PrintEntityKeys item, DocumentOutPM documentOut)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(_batchPrinterArgs.Tenant);
            DocumentOutCopyRepository documentOutCopyRepository = new DocumentOutCopyRepository(commonContext);

            DocumentOutCopy documentOutCopy = new DocumentOutCopy()
            {
                Id = document.Id,
                DocumentId = document.Id,
                DocumentOutId = documentOut.Id,
                DocumentTypeCopyId = documentTypeCopy.Id,
                Tenant = _batchPrinterArgs.Tenant,               
                LastPrintDate = DateTime.Now,
                LastPrintedByUserId = printedBy.Id,
            };

            documentOutCopyRepository.Add(documentOutCopy);
            documentOutCopyRepository.SubmitChanges();
        }

        private MemoryStream CreateReportStream(PrintEntityKeys item, DocumentOutPM documentOut)
        {
            ExportDocumentHelper exportDocumentHelper = new ExportDocumentHelper();
            try
            {
                if (template != null) template.DocumentOutId = documentOut?.Id;
                StiReport report = exportDocumentHelper.GetReportDocument(template, item.EntityId, item.ObjectTableId, item.ChildEntityId,
                item.ObjectTableId, documentTypeCopy, template.TemplateBody, _batchPrinterArgs.Tenant, new long(), new long(), new long(), new long(), printedBy.Id);
                var stream = new MemoryStream();
                report.ExportDocument(StiExportFormat.Pdf, stream);
                return stream;
            }
            catch (Exception e)
            {
                throw new Exception("Stimulsoft Error: " + e.Message);
            }
        }

        private MemoryStream GetReportStreamFromCopy(DocumentOutCopy printedCopy)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(printedCopy.Tenant);

            Document document = (from doc in commonContext.Documents
                                 where doc.Id == printedCopy.DocumentId
                                 select doc).FirstOrDefault();
            if (document == null)
                return null;

            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = document.Tenant,
                FileSize = document.FileSize,
            };
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            var datainByte = storageservice.Read(fileInfo);

            if (datainByte == null)
                return null;

            MemoryStream stream = new MemoryStream(datainByte);
            
            return stream;
        }
        private bool CheckValidation(PrintEntityKeys item, ItemPrintingResult result)
        {
            try
            {
                GeneralValidatoin(item);
                CustomeValidation(item);
                return true;
            }
            catch (Exception e)
            {
                result.Error = e.Message;
                return false;

            }
        }

        private void GeneralValidatoin(PrintEntityKeys item)
        {
            if (template.TemplateBody == null)
                throw new Exception("Template Body is empty");
            if (documentType.IsDocumentOneTimePrintLimited && IsAlreadyPrinted(item))
                throw new Exception("This document is already printed");
        }

        private bool IsAlreadyPrinted(PrintEntityKeys item)
        {
            var printedCopy = GetPrintedCopy(item);
            if (printedCopy == null)
                return false;
            if (printedCopy.DocumentTypeCopyId != documentType.LimitedPrintCopyId)
                return false;
            return true;
        }

        private DocumentOutCopy GetPrintedCopy(PrintEntityKeys item)
        {
            DocumentOutCopyRepository documentsFilingRepository = new DocumentOutCopyRepository(_batchPrinterArgs.Tenant);
            var args = new DocumentOutCopyArgs()
            {
                Tenant = _batchPrinterArgs.Tenant,
                EntityId = item.EntityId,
                ObjectTableId = item.ObjectTableId,
                DocumentTypeCopyId = _batchPrinterArgs.CopyId,
                DocumentTypeId = _batchPrinterArgs.DocumentTypeId,
                DocumentTemplateId = _batchPrinterArgs.TemplateId,
                ChildEntityId = item.ChildEntityId,
            };

            return documentsFilingRepository.GetDocumentOutCopy(args);
        }

        private User GetUser()
        {
            UserRepository userRep = new UserRepository(_batchPrinterArgs.Tenant);
            User printedBy = userRep.GetSingleUserByCodeOrEmailForTenant(null, _batchPrinterArgs.Email, _batchPrinterArgs.Tenant, false);
            if(printedBy != null)
                return printedBy;
            printedBy = userRep.GetSingleUserByCodeOrEmailForTenant(null, _batchPrinterArgs.Email, 0, false);
            if (printedBy == null)
                throw new Exception($"the user {_batchPrinterArgs.Email} not found");
            if(!printedBy.IsDistributor)
                return printedBy;
            throw new Exception($"the user {_batchPrinterArgs.Email} not found");
        }

        private DocumentTypeTemplatePM GetDocumentTypeTemplate()
        {
            DocumentTypeTemplateQuery documentTypeTemplateQuery = new DocumentTypeTemplateQuery(_batchPrinterArgs.Tenant);
            return  documentTypeTemplateQuery.GetById(_batchPrinterArgs.TemplateId , _batchPrinterArgs.Tenant);
        }

        private DocumentTypeCopy GetDocumentTypeCopy()
        {
            DocumentTypeCopyRepository documentTypeCopyRep = new DocumentTypeCopyRepository(_batchPrinterArgs.Tenant);
            DocumentTypeCopy documentTypeCopy = documentTypeCopyRep.GetSingleDocumentTypeCopy(_batchPrinterArgs.CopyId);
            return documentTypeCopy;
        }

        private DocumentType GetDecumentType()
        {
            DocumentTypeRepository repository = new DocumentTypeRepository(_batchPrinterArgs.Tenant);
            return repository.GetSingleDocumentTypes(_batchPrinterArgs.DocumentTypeId, _batchPrinterArgs.Tenant);
        }
    }
}