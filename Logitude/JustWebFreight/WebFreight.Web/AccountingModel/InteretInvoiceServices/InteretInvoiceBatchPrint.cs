using Atp.Pdf;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.InterestService.HelperClasses;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Logitude.Server.Tools.Utils;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using WebFreight.Web.WebServices;


namespace Logitude.Accounting.BL.InterestService
{
    public class InteretInvoiceBatchPrint
    {
        public Stream _Stream;
        byte[] datainByte;


        public PdfDocument CheckValidCopiesForInvoicesAndPrint(InterestReportArguments interestReportArgs,int tenant,string email)
        {
            Logger.LogInfo("Starting document validation and printing process.");
            PdfDocument pdfDoc = new PdfDocument();
            using (var scope = TransactionFactory.GetTransaction())
            {
                ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
                IInvoiceContext invoiceContext = InvoiceContext.GetContext(tenant);
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
                Logger.LogInfo($"Total invoices selected for processing: {interestReportArgs.SelectedIds.Count}");

                for (int i = 0; i < interestReportArgs.SelectedIds.Count; i++)
                {
                    string[] EntitiesId = interestReportArgs.SelectedIds[i].Split(',');
                    string InterestReportId = EntitiesId[0];
                    string ARInvoieId = EntitiesId[1];
                    bool IsPrintARInvoice = PrintInvoicesPDF(email, tenant, ARInvoieId, pdfDoc, "999G",false, commonContext, invoiceContext);
                    if (IsPrintARInvoice && interestReportArgs.AttachReportWithEachInvoice)
                    {
                        bool IsPrintInterestReport = PrintInvoicesPDF(email, tenant, InterestReportId, pdfDoc, "ITDT", false, commonContext, invoiceContext);
                    }

                }
                scope.Complete();  // Commit the transaction
                Logger.LogInfo("All documents processed successfully and transaction completed.");
            }
            return pdfDoc;
        }
        public bool PrintDocuments(InterestReportArguments interestReportArgs, int tenant, string email)
        {
            Logger.LogInfo("Starting PrintDocuments");
            bool isPrintSuccess = false;
            using (var scope = TransactionFactory.GetTransaction())
            {
                try
                {
                    if (ProcessInterestReports(interestReportArgs, tenant, email))
                    {
                        scope.Complete();  // Commit the transaction
                        isPrintSuccess = true;
                        Logger.LogInfo("All documents processed successfully and transaction completed.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Error processing documents: {ex}");
                }

            }
            return isPrintSuccess;
        }
        private bool ProcessInterestReports(InterestReportArguments args, int tenant, string email)
        {
            var invoiceContext = InvoiceContext.GetContext(tenant);
            var commonContext = CommonDataContext.GetContext(tenant);
            var interestInvoiceIds = GetInterestInvoiceIds(args, tenant);

            foreach (string invoiceId in interestInvoiceIds)
            {
                string[] EntitiesId = invoiceId.Split(',');
                string InterestReportId = EntitiesId[0];
                string ARInvoieId = EntitiesId[1];
                if (!PrintSingleDocument(email, tenant, ARInvoieId, "999G", commonContext, invoiceContext))
                {
                    return false;
                }
                if (args.AttachReportWithEachInvoice)
                {
                    if (!PrintSingleDocument(email, tenant, InterestReportId, "ITDT", commonContext, invoiceContext))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private bool PrintSingleDocument(string email, int tenant, string invoiceId, string documentCode, ICommonDataContext commonContext, IInvoiceContext invoiceContext)
        {
            try
            {
                DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(tenant);
                string documentTypeId = documentTypeQuery.GetDocumentTypeListIdByCodeAndTenant(documentCode, tenant);
                DocumentOutQuery documentOutQuery = new DocumentOutQuery(tenant);

                var documentOut = commonContext.DocumentOuts
                    .Include("DocumentsFiling")
                    .Include("DocumentsFiling.DocumentType")
                    .FirstOrDefault(doc => doc.DocumentsFiling.EntityId == invoiceId && doc.DocumentsFiling.DocumentTypeId == documentTypeId && doc.Tenant == tenant);

                if (documentOut == null)
                {
                    Logger.LogError("No document found for printing.");
                    return false;
                }

                return UpdateAndPrintDocument(documentOut, documentCode, invoiceId, tenant, email, commonContext, invoiceContext);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to print document {invoiceId}: {ex.Message}");
                return false;
            }
        }
        private bool UpdateAndPrintDocument(DocumentOut documentOut, string documentCode,string invoiceId, int tenant, string email, ICommonDataContext commonContext, IInvoiceContext invoiceContext)
        {
            string userId = "";
            string documentOutId = documentOut.Id;
            tenant = documentOut.Tenant;

            DocumentOutCopy copy = GetDocumentCopyByCode(commonContext, (int)tenant, documentOutId, documentCode);
            if (copy != null && documentOut.DocumentsFiling.DocumentType.IsDocumentOneTimePrintLimited && documentOut.DocumentsFiling.DocumentType.LimitedPrintCopyId == copy.DocumentTypeCopyId && !string.IsNullOrEmpty(copy.LastPrintedByUserId))
            {
                copy = GetDocumentCopyByCode(commonContext, (int)tenant, documentOutId, "999G1");
            }
            if (copy == null)
            {
                Logger.LogError("No valid document copy found for printing.");
                return false;
            }
            UserRepository userRep = new UserRepository((int)tenant);
            User printedBy = null;
            if (!string.IsNullOrEmpty(userId)) printedBy = userRep.GetSingleUser(userId, (int)tenant);
            else printedBy = userRep.GetSingleUserByCodeOrEmailForTenant(null, email, (int)tenant, false);
            if (printedBy == null)
            {
                printedBy = userRep.GetSingleUserByCodeOrEmailForTenant(null, email, 0, false);
            }

            DocumentOutCopyRepository myRep = new DocumentOutCopyRepository(commonContext);
            DocumentOutCopy documentoutCopy = myRep.GetSingleDocumentOutCopyByTenant(copy.Id, (int)tenant);
            documentoutCopy.LastPrintDate = TenantServerConfigration.GetCurrentDateTime((int)tenant);
            documentoutCopy.LastPrintedByUserId = printedBy.Id;
            myRep.Update(documentoutCopy);
            myRep.SubmitChanges();

            if (documentCode == "999G")
            {
                ARInvoiceRepository aRInvoiceRepository = new ARInvoiceRepository(invoiceContext);
                ARInvoice aRInvoice = aRInvoiceRepository.GetInvoices().Where(s => s.Id == invoiceId).FirstOrDefault();
                aRInvoice.IsPrinted = true;
                aRInvoiceRepository.Update(aRInvoice);
                aRInvoiceRepository.SubmitChanges();
            }
            Logger.LogInfo($"Printing document {documentOut.Id} for tenant {tenant}.");
            return true; // Replace with actual printing and updating logic
        }
        private List<string> GetInterestInvoiceIds(InterestReportArguments args, int tenant)
        {
            ARInvoiceQuery aRInvoiceQuery = new ARInvoiceQuery(tenant);
            InterestReportQueryService interestReportQueryService = new InterestReportQueryService(tenant);
            if (args.AllSelected)
            {
                IQueryable<ARInvoice> ARInvoices = aRInvoiceQuery.GetAllInterestInvoices(args.FromDate, args.ToDate, args.ShowPrintedInvoice, tenant);
                if (args.ExcludedIds != null)
                {
                    args.SelectedIds = (from a in ARInvoices
                                                      where !args.ExcludedIds.Contains(a.Id)
                                                      select a.Id).ToList();

                }
            }
            args.SelectedIds = interestReportQueryService.GetInterestReprtsWithInvocies(tenant, args.SelectedIds);
            return args.SelectedIds;  // Adjust according to the actual requirement
        }
        public PDFDocumentInvoices GetNumberOfDocumentNotPrinted(InterestReportArguments interestReportArgs, int tenant,string email)
        {
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
                bool IsPrintARInvoice = PrintInvoicesPDF(email, tenant, ARInvoieId, pdfDoc, "999G", true);
                if (!IsPrintARInvoice)
                {
                    ARInvoiceIdsNotPrinted.Add(ARInvoieId);

                }
                if (interestReportArgs.AttachReportWithEachInvoice)
                {
                    bool IsPrintInterestReport = PrintInvoicesPDF(email, tenant, InterestReportId, pdfDoc, "ITDT", true);
                    if (!IsPrintInterestReport)
                    {
                        InterestReportIdsNotPrinted.Add(InterestReportId);
                    }

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

            return pDFDocumentInvoices;

        }

        private DocumentOutCopy GetDocumentCopyByCode(ICommonDataContext commonContext, int tenant, string documentOutId, string DocumentCode )
        {
            DocumentOutCopy copy = (from a in commonContext.DocumentOutCopies
                                    join Copy in commonContext.DocumentTypeCopies on a.DocumentTypeCopyId equals Copy.Id
                                    where a.DocumentOutId == documentOutId
                                          && tenant == (int)tenant
                                          && Copy.Code == DocumentCode
                                    select a).OrderBy(d => d.DocumentTypeCopy.IndexOrder).FirstOrDefault();


            return copy;
        }
        private bool PrintInvoicesPDF(string Email, int? tenant, string SelectId, PdfDocument pdfDoc, string DocumentCode, bool IsForChecked, ICommonDataContext commonContext=null, IInvoiceContext invoiceContext=null)
        {
            try
            {

                string userId = "";
                string documentOutId = null;
                string email = Email;
                bool IsPrinted = false;

                DocumentOut doucmentOut = null;
                if (commonContext == null)
                {
                    commonContext = CommonDataContext.GetContext((tenant != null ? (int)tenant : 0));
                }

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

                    DocumentOutCopy copy = GetDocumentCopyByCode(commonContext, (int)tenant, documentOutId, DocumentCode);


                    if (copy!=null && doucmentOut.DocumentsFiling.DocumentType.IsDocumentOneTimePrintLimited && doucmentOut.DocumentsFiling.DocumentType.LimitedPrintCopyId == copy.DocumentTypeCopyId && !string.IsNullOrEmpty(copy.LastPrintedByUserId))
                    {
                        copy = GetDocumentCopyByCode(commonContext,(int)tenant, documentOutId, "999G1");
                    }
                    if (copy == null)
                    {
                        Logger.LogError("No valid document copy found for printing.");
                        return false;
                    }

                        Uploader up = new Uploader();

                    //foreach (DocumentOutCopy copy in copies)
                    //{
                        includeInPrint = true;
                        //if (doucmentOut.DocumentsFiling.DocumentType.IsDocumentOneTimePrintLimited && doucmentOut.DocumentsFiling.DocumentType.LimitedPrintCopyId == copy.DocumentTypeCopyId && !string.IsNullOrEmpty(copy.LastPrintedByUserId))
                        //{
                        //    includeInPrint = false;
                        //}

                        if (includeInPrint)
                        {
                        IsPrinted = true;
                        if (!IsForChecked)
                        {

                            string documentExtension = up.GetFileExtension(copy.DocumentId, (int)tenant);
                            string documentId = copy.DocumentId;
                            Logger.LogDebug($"Attempting to download and merge document ID={documentId} with extension={documentExtension}.");

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
                                    /* moved to different function
                                    UserRepository userRep = new UserRepository((int)tenant);
                                    User printedBy = null;
                                    if (!string.IsNullOrEmpty(userId)) printedBy = userRep.GetSingleUser(userId, (int)tenant);
                                    else printedBy = userRep.GetSingleUserByCodeOrEmailForTenant(null, email, (int)tenant, false);
                                    if (printedBy == null)
                                    {
                                        printedBy = userRep.GetSingleUserByCodeOrEmailForTenant(null, email, 0, false);
                                    }

                                    DocumentOutCopyRepository myRep = new DocumentOutCopyRepository(commonContext);
                                    DocumentOutCopy documentoutCopy = myRep.GetSingleDocumentOutCopyByTenant(copy.Id, (int)tenant);
                                    documentoutCopy.LastPrintDate = TenantServerConfigration.GetCurrentDateTime((int)tenant);
                                    documentoutCopy.LastPrintedByUserId = printedBy.Id;
                                    myRep.Update(documentoutCopy);
                                    myRep.SubmitChanges();

                                    if (DocumentCode == "999G")
                                    {
                                        ARInvoiceRepository aRInvoiceRepository = new ARInvoiceRepository(invoiceContext);
                                        ARInvoice aRInvoice = aRInvoiceRepository.GetInvoices().Where(s => s.Id == SelectId).FirstOrDefault();
                                        aRInvoice.IsPrinted = true;
                                        aRInvoiceRepository.Update(aRInvoice);
                                        aRInvoiceRepository.SubmitChanges();
                                    }
                                    */
                                    //break;

                                }
                                else
                                {
                                    Logger.LogError("Failed to download file.");
                                }
                            }
                            }



                        }

                    //}

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
                try
                {

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
                    throw new Exception(e.Message);
                }
            }
            return null;
        }
    }



    public class PDFDocumentInvoices
    {
        public StreamContent Document { set; get; }
        public List<string> ARInvoiceNumbersNotPrinted { set; get; }
        public List<string> InterestReportNumbersNotPrinted { set; get; }
    }
}
