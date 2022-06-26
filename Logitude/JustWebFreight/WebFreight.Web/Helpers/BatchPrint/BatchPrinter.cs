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

namespace WebFreight.Web.Helpers.BatchPrint
{
    public abstract class BatchPrinter
    {
        public BatchPrintManagerArgs _batchPrintManagerArgs;
        public DocumentType documentType;
        public DocumentTypeCopy documentTypeCopy;
        public DocumentTypeTemplate template;
        public User printedBy;
        public BatchPrinter(BatchPrintManagerArgs batchPrintManagerArgs)
        {
            _batchPrintManagerArgs = batchPrintManagerArgs;
            documentType = GetDecumentType();
            documentTypeCopy = GetDocumentTypeCopy();
            template = GetDocumentTypeTemplate();
            printedBy = GetUser();
        }

        public abstract void CustomeValidation();

        public PrintingResult PrintDocuments()
        {
            var itemPrintingResults = new List<ItemPrintingResult>();
            foreach (var item in _batchPrintManagerArgs.EntityIds)
            {
                itemPrintingResults.Add(Print(item));
            }
            PdfDocument pdfDoc = new PdfDocument();
            MargePdfs(pdfDoc, itemPrintingResults);
            MemoryStream memoryStream = GetMemoryStream(pdfDoc);
            var documentId = UploadPDFToStorage(memoryStream, _batchPrintManagerArgs.Tenant);
            var printingResults = CreatePrintResult(documentId, itemPrintingResults);

            return printingResults;
        }

        private PrintingResult CreatePrintResult(string documentId, List<ItemPrintingResult> itemPrintingResults)
        {
            var result = new PrintingResult();
            result.DocumentId = documentId;
            result.NotValidRows = itemPrintingResults.Select(e=> new PrintingRow() { EntityId = e.EntityId,Error = e.Error}).ToList();
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
            var result = new ItemPrintingResult();

            if (!CheckValidation(item,result))
                return result;

            ExportDocumentHelper exportDocumentHelper = new ExportDocumentHelper();
            StiReport report = exportDocumentHelper.GetReportDocument(documentType, item.EntityId, _batchPrintManagerArgs.ObjectTableId, item.ChildEntityId,
                _batchPrintManagerArgs.ChildObjectTableId, documentTypeCopy, template.TemplateBody, template, _batchPrintManagerArgs.Tenant, new long(), new long(), new long(), new long(), printedBy.Id);
            var stream = new MemoryStream();
            report.ExportDocument(StiExportFormat.Pdf, stream);


            result.DocumentStream = stream;
            result.IsSuccessfullyPrinted = true;
            return result;
        }

        private bool CheckValidation(PrintEntityKeys item, ItemPrintingResult result)
        {
            try
            {
                GeneralValidatoin(item);
                CustomeValidation();
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
            if(documentType.IsDocumentOneTimePrintLimited && IsAlreadyPrinted(item))
                throw new Exception("This document is already printed");

        }

        private bool IsAlreadyPrinted(PrintEntityKeys item)
        {
            DocumentsFilingRepository documentsFilingRepository = new DocumentsFilingRepository(_batchPrintManagerArgs.Tenant);
            return documentsFilingRepository.CheckIfDocumentTypeHasDocumentFilling(documentType.Id, _batchPrintManagerArgs.ObjectTableId, item.EntityId, _batchPrintManagerArgs.Tenant);
        }

        private User GetUser()
        {
            UserRepository userRep = new UserRepository(_batchPrintManagerArgs.Tenant);
            User printedBy = userRep.GetSingleUserByCodeOrEmailForTenant(null, _batchPrintManagerArgs.Email, _batchPrintManagerArgs.Tenant, false);
            return printedBy;
        }

        private DocumentTypeTemplate GetDocumentTypeTemplate()
        {
            DocumentTypeTemplateRepository documentTypeTemplaterep = new DocumentTypeTemplateRepository(_batchPrintManagerArgs.Tenant);
            DocumentTypeTemplate template = documentTypeTemplaterep.GetSingleDocumentTypeTemplate(_batchPrintManagerArgs.TemplateId);
            return template;
        }

        private DocumentTypeCopy GetDocumentTypeCopy()
        {
            DocumentTypeCopyRepository documentTypeCopyRep = new DocumentTypeCopyRepository(_batchPrintManagerArgs.Tenant);
            DocumentTypeCopy documentTypeCopy = documentTypeCopyRep.GetSingleDocumentTypeCopy(_batchPrintManagerArgs.CopyId);
            return documentTypeCopy;
        }

        private DocumentType GetDecumentType()
        {
            DocumentTypeRepository repository = new DocumentTypeRepository(_batchPrintManagerArgs.Tenant);
            return repository.GetSingleDocumentTypes(_batchPrintManagerArgs.DocumentId, _batchPrintManagerArgs.Tenant);
        }
    }
}