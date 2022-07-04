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

namespace WebFreight.Web.Helpers.BatchPrint
{
    public abstract class BatchPrinter
    {
        public BatchPrinterArgs _batchPrinterArgs;
        public DocumentType documentType;
        public DocumentTypeCopy documentTypeCopy;
        public DocumentTypeTemplate template;
        public User printedBy;
        public BatchTaskExecutionUpdateService batchTaskExecutionUpdateService;
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
            string documentId = null;
            if (itemPrintingResults.Any(e => e.IsSuccessfullyPrinted))
            {
                PdfDocument pdfDoc = new PdfDocument();
                MargePdfs(pdfDoc, itemPrintingResults);
                MemoryStream memoryStream = GetMemoryStream(pdfDoc);
                documentId = UploadPDFToStorage(memoryStream, _batchPrinterArgs.Tenant);
            }
            PrintingResult printingResults = CreatePrintResult(documentId, itemPrintingResults);

            return printingResults;
        }

        private void IncresePrograse()
        {
            _batchPrinterArgs.BatchTaskExecution.ProgressPercentage++;
            batchTaskExecutionUpdateService.Update(_batchPrinterArgs.BatchTaskExecution, true);
        }

        private PrintingResult CreatePrintResult(string documentId, List<ItemPrintingResult> itemPrintingResults)
        {
            var result = new PrintingResult();
            result.DocumentId = documentId;
            result.NotValidRows = itemPrintingResults.Where(e=>!e.IsSuccessfullyPrinted).Select(e => new PrintingRow() { EntityId = e.EntityId, Error = e.Error }).ToList();
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



        private string UploadPDFToStorage(MemoryStream memoryStream, int tenant)
        {
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            byte[] ByteData = memoryStream.ToArray();
            DocumentRepository documentRepository = new DocumentRepository(tenant);
            var document = new Document()
            {
                FileName = "Documents" + DateTime.Now.ToString("dd-MM-yyy"),
                CreateDate = DateTime.Now,
                Extension = "pdf",
                FileSize = ByteData.Length,
                Tenant = tenant,
                Id = IdCounter.GetNumber("Document", tenant),
                HasFile = true,
                Folder = "MultiPrint",
            };

            documentRepository.Add(document);
            documentRepository.SubmitChanges();

            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = "tariff",
                Extension = document.Extension,
                Tenant = tenant,
                FileSize = document.FileSize,
            };

            storageservice.Write(ByteData, fileInfo);


            return document.Id;
        }
        private ItemPrintingResult Print(PrintEntityKeys item)
        {
            var result = new ItemPrintingResult(item.EntityId);

            if (!CheckValidation(item, result))
                return result;
            try
            {
                var stream = GetReportStream(item);
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

        private MemoryStream GetReportStream(PrintEntityKeys item)
        {
            var printedCopy = GetPrintedCopy(item);
            if (printedCopy != null)
                return GetReportStreamFromCopy(printedCopy);

            return CreateReportStream(item);

        }

        private MemoryStream CreateReportStream(PrintEntityKeys item)
        {
            ExportDocumentHelper exportDocumentHelper = new ExportDocumentHelper();
            try
            {
                StiReport report = exportDocumentHelper.GetReportDocument(documentType, item.EntityId, _batchPrinterArgs.ObjectTableId, item.ChildEntityId,
                _batchPrinterArgs.ChildObjectTableId, documentTypeCopy, template.TemplateBody, template, _batchPrinterArgs.Tenant, new long(), new long(), new long(), new long(), printedBy.Id);
                var stream = new MemoryStream();
                report.ExportDocument(StiExportFormat.Pdf, stream);
                return stream;
            }
            catch (Exception e)
            {
                throw new Exception("Stimulsoft Error: "+ e.Message);
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
                ObjectTableId = _batchPrinterArgs.ObjectTableId,
                DocumentTypeCopyId = _batchPrinterArgs.CopyId,
                DocumentTypeId = _batchPrinterArgs.DocumentTypeId,
            };
            return documentsFilingRepository.GetDocumentOutCopy(args);
        }

        private User GetUser()
        {
            UserRepository userRep = new UserRepository(_batchPrinterArgs.Tenant);
            User printedBy = userRep.GetSingleUserByCodeOrEmailForTenant(null, _batchPrinterArgs.Email, _batchPrinterArgs.Tenant, false);
            return printedBy;
        }

        private DocumentTypeTemplate GetDocumentTypeTemplate()
        {
            DocumentTypeTemplateRepository documentTypeTemplaterep = new DocumentTypeTemplateRepository(_batchPrinterArgs.Tenant);
            DocumentTypeTemplate template = documentTypeTemplaterep.GetSingleDocumentTypeTemplate(_batchPrinterArgs.TemplateId);
            return template;
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