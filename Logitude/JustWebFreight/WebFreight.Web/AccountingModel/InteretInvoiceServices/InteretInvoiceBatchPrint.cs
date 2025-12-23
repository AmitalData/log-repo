using Atp.Pdf;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.InterestService.HelperClasses;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.Tools;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.StorageService;
using Logitude.Server.Tools.Utils;
using Microsoft.Azure.Pipelines.WebApi;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; 
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Enums;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.IdentityModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Transactions;
using WebFreight.Web.WebServices;


namespace Logitude.Accounting.BL.InterestService
{
    public class InteretInvoiceBatchPrint
    {
        public Stream _Stream;
        byte[] datainByte;

        const string INTEREST_REPORT = "ITDT";
        public PdfDocument CheckValidCopiesForInvoicesAndPrint(InterestReportArguments interestReportArgs,int tenant,string email)
        {
           
            NetCommonHelper.Logger.DevLog.Instance.WriteInfo("Starting document validation and printing process.");
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
                NetCommonHelper.Logger.DevLog.Instance.WriteInfo($"Total invoices selected for processing: {interestReportArgs.SelectedIds.Count}");

                for (int i = 0; i < interestReportArgs.SelectedIds.Count; i++)
                {
                    string[] EntitiesId = interestReportArgs.SelectedIds[i].Split(',');
                    string InterestReportId = EntitiesId[0];
                    string ARInvoieId = EntitiesId[1];
                    bool IsPrintARInvoice = PrintInvoicesPDF(email, tenant, ARInvoieId, pdfDoc, "999G",false, commonContext, invoiceContext);
                    if (IsPrintARInvoice && interestReportArgs.AttachReportWithEachInvoice)
                    {
                        bool IsPrintInterestReport = PrintInvoicesPDF(email, tenant, InterestReportId, pdfDoc, INTEREST_REPORT, false, commonContext, invoiceContext);
                    }

                }
                scope.Complete();  // Commit the transaction
                NetCommonHelper.Logger.DevLog.Instance.WriteInfo("All documents processed successfully and transaction completed.");
            }
            return pdfDoc;
        }
        public bool PrintDocuments(InterestReportArguments interestReportArgs, int tenant, string email)
        {
            NetCommonHelper.Logger.DevLog.Instance.WriteInfo("Starting PrintDocuments");
            bool isPrintSuccess = false;
            using (var scope = TransactionFactory.GetTransaction())
            {
                try
                {
                    if (ProcessInterestReports(interestReportArgs, tenant, email))
                    {
                        scope.Complete();  // Commit the transaction
                        isPrintSuccess = true;
                        NetCommonHelper.Logger.DevLog.Instance.WriteInfo("All documents processed successfully and transaction completed.");
                    }
                }
                catch (Exception ex)
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteError($"Error processing documents: {ex}");
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
                if (!PrintSingleDocumentInv(email, tenant, ARInvoieId, "999G", commonContext, invoiceContext))
                {
                    return false;
                }
                if (args.AttachReportWithEachInvoice)
                {
                    if (!PrintSingleDocumentITDT(email, tenant, InterestReportId, INTEREST_REPORT, commonContext, invoiceContext))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool PrintSingleDocumentITDT(string email, int tenant, string invoiceId, string documentCode, ICommonDataContext commonContext, IInvoiceContext invoiceContext)
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
                    NetCommonHelper.Logger.DevLog.Instance.WriteError("No document found for printing.");
                    return false;
                }

                return UpdateAndPrintDocument(documentOut, documentCode, invoiceId, tenant, email, commonContext, invoiceContext);
            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError($"Failed to print document {invoiceId}: {ex.Message}");
                return false;
            }
        }


        private bool PrintSingleDocumentInv(string email, int tenant, string invoiceId, string documentCode, ICommonDataContext commonContext, IInvoiceContext invoiceContext)
        {
            try
            {

                ARInvoiceRepository repository = new ARInvoiceRepository();
                ARInvoice invoice = repository.GetSingleARInvoice(invoiceId, tenant);

                ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
                DocumentRepository documentRepository = new DocumentRepository(commoncontext);
                DocumentsFilingRepository myDocumentsFilingRepository = new DocumentsFilingRepository(commoncontext);
                DocumentsFilingQuery myDocumentsFilingQuery = new DocumentsFilingQuery(myDocumentsFilingRepository);
                DocumentOutCopyQuery documentOutCopyQuery = new DocumentOutCopyQuery(tenant);

                DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(tenant);
                string documentTypeId = documentTypeQuery.GetDocumentTypeListIdByCodeAndTenant(documentCode, tenant);
                DocumentOutQuery documentOutQuery = new DocumentOutQuery(tenant);
                bool isPrinted = invoice.IsPrinted;
                if (invoice.IsPrinted)
                {
                    DocumentHelper documentHelper = new DocumentHelper();
                    bool isPDFExist = documentHelper.CheckPDFInvoiceInStorage_Outer(invoice, tenant, repository);
                    if (!isPDFExist)
                    {
                        isPrinted = false;
                    }
                }
                if (invoice.IsPrinted)
                {
                    var documentOut = commonContext.DocumentOuts
                    .Include("DocumentsFiling")
                    .Include("DocumentsFiling.DocumentType")
                    .FirstOrDefault(doc => doc.DocumentsFiling.EntityId == invoiceId && doc.DocumentsFiling.DocumentTypeId == documentTypeId && doc.Tenant == tenant);

                    if (documentOut == null)
                    {
                        isPrinted = false;
                    }
                }

                if (!isPrinted)
                {
                    PrintInterestInvoice(tenant, invoice, invoiceContext);
                }

                else if (invoice.IsSigned == ARInvoiceSignedStatusValues.NotSigned || invoice.IsSigned == ARInvoiceSignedStatusValues.SigningFailed)
                {
                    SignInterestInvoice(tenant, invoice, invoiceContext);
                }
                else if (invoice.IsSigned == ARInvoiceSignedStatusValues.SignedButNotYetSent || invoice.IsSigned == ARInvoiceSignedStatusValues.SignedButSendingByEmailFailed)
                {
                    DocumentsFilingPM myDocumentFilings = myDocumentsFilingQuery.GetDocumentsFilingPMsByEntityId(invoice?.Id, tenant).FirstOrDefault();
                    DocumentOutCopyPM documentOutCopyPM = documentOutCopyQuery.GetDocumentOutCopiesForDocumentOutAndType(myDocumentFilings?.Id, tenant, "999G");
                    Document document = documentRepository.GetSingleDocument(tenant, documentOutCopyPM?.DocumentId);
                    string contactEmail = String.Empty;
                    bool isInterestReport = false;
                    (contactEmail, isInterestReport) = this.IsSignatureHtmlPresentByBillToId(invoice?.BillToId, tenant);
                    if (!string.IsNullOrEmpty(contactEmail))
                    {
                        DocumentHelper documentHelper = new DocumentHelper();
                        documentHelper.SendToEmailContactOuter(contactEmail, invoice, document, myDocumentFilings?.Id, repository, tenant, isInterestReport);
                    }
                }

                return true; // UpdateAndPrintDocument(documentOut, documentCode, invoiceId, tenant, email, commonContext, invoiceContext);
            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError($"Failed to print document {invoiceId}: {ex.Message}");
                return false;
            }
        }


        private (string email, bool isInterestReport) IsSignatureHtmlPresentByBillToId(string Billto, int tenant)
        {
            string email = String.Empty;
            bool isInterestReport = false;    
            ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);

            if (string.IsNullOrEmpty(Billto)) return (email, isInterestReport);
            Simplog.Data.CommonDataModel.EntityPOCOs.Card myCard = (from card in objectContext.Cards
                                                                    where card.Id == Billto
                                                                    select card).FirstOrDefault();

            if (myCard != null && !string.IsNullOrEmpty(myCard.EmailForSendingSingArinvoice))
            {
                email = objectContext.Contacts.Where(contact => contact.Id == myCard.EmailForSendingSingArinvoice).FirstOrDefault()?.Email;
                if (!string.IsNullOrEmpty(email))
                {
                    return (email, myCard.SendingInterestReport);
                }

            }
            return (email, isInterestReport);
        }



        public void PrintInterestInvoice(int tenant, ARInvoice aRInvoice, IInvoiceContext invoiceContext)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                try
                {
                    ARInvoiceQuery aRInvoiceQuery = new ARInvoiceQuery(tenant);
                    InterestReport interestReport = aRInvoiceQuery.GetInterestReport(aRInvoice);
                    ARInvoicePM aRInvoicePM = aRInvoiceQuery.GetSinglePM(aRInvoice.Id, tenant);
                    if (aRInvoicePM == null || interestReport == null) throw new ArgumentNullException("There is no ARInvoice or InterestReport");

                    ARInvoiceService invoiceService = new ARInvoiceService(invoiceContext, tenant);

                    invoiceService.PrintOrSendInvoice(aRInvoicePM.Id, aRInvoicePM.InvoiceNumber, tenant, aRInvoicePM.CreatedByUserId);
                    NetCommonHelper.Logger.DevLog.Instance.WriteTrace($"PrintInterestInvoice aRInvoicePM.Id={aRInvoicePM.Id}");
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex, "Error in PrintInterestInvoice aRInvoice.Id=" + aRInvoice.Id);
                }

            }
        }


        public void SignInterestInvoice(int tenant, ARInvoice aRInvoice, IInvoiceContext invoiceContext)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                try
                {
                    ARInvoiceQuery aRInvoiceQuery = new ARInvoiceQuery(tenant);
                    ARInvoicePM aRInvoicePM = aRInvoiceQuery.GetSinglePM(aRInvoice.Id, tenant);
                    if (aRInvoicePM == null) throw new ArgumentNullException("There is no ARInvoice");

                    ARInvoiceService invoiceService = new ARInvoiceService(invoiceContext, tenant);
                    invoiceService.SignInvoice(aRInvoicePM, tenant);
                    NetCommonHelper.Logger.DevLog.Instance.WriteTrace("PrintInterestInvoice aRInvoicePM.Id=" + aRInvoicePM.Id);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex, "Error in PrintInterestInvoice aRInvoice.Id=" + aRInvoice.Id);
                }

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
                bool print999G1copy = true;

                ARInvoiceQuery aRInvoiceQuery = new ARInvoiceQuery((int)tenant);
                var aRInvoice = aRInvoiceQuery.GetSingleARInvoice(invoiceId, (int)tenant);
                if (aRInvoice != null)
                {
                    string signStatus = aRInvoice.IsSigned;
                    if (signStatus == "1" || signStatus == "3" || signStatus == "4")
                    {
                        print999G1copy = false;  // print current 'copy' value, i. e. with documentCode
                    }
                }
                if (print999G1copy)
                {
                    copy = GetDocumentCopyByCode(commonContext, (int)tenant, documentOutId, "999G1");
                }
            }
            if (copy == null)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError("No valid document copy found for printing.");
                return true;
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
            NetCommonHelper.Logger.DevLog.Instance.WriteInfo($"Printing document {documentOut.Id} for tenant {tenant}.");
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
            var ids = interestReportQueryService.GetInterestReprtsWithInvocies(tenant, args.SelectedIds);
            return ids;  // Adjust according to the actual requirement
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

            var ids = interestReportQueryService.GetInterestReprtsWithInvocies(tenant, interestReportArgs.SelectedIds);
            List<string> ARInvoiceIdsNotPrinted = new List<string>();
            List<string> InterestReportIdsNotPrinted = new List<string>();

            for (int i = 0; i < ids.Count; i++)
            {
                string[] EntitiesId = ids[i].Split(',');
                string InterestReportId = EntitiesId[0];
                string ARInvoieId = EntitiesId[1];
                bool IsPrintARInvoice = PrintInvoicesPDF(email, tenant, ARInvoieId, pdfDoc, "999G", true);
                if (!IsPrintARInvoice)
                {
                    ARInvoiceIdsNotPrinted.Add(ARInvoieId);

                }
                if (interestReportArgs.AttachReportWithEachInvoice)
                {
                    bool IsPrintInterestReport = PrintInvoicesPDF(email, tenant, InterestReportId, pdfDoc, INTEREST_REPORT, true);
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
                ARInvoice aRInvoice = null;
                DocumentOut doucmentOut = null;
                if (commonContext == null)
                {
                    commonContext = CommonDataContext.GetContext((tenant != null ? (int)tenant : 0));
                }
                if (invoiceContext == null)
                {
                    invoiceContext = InvoiceContext.GetContext((tenant != null ? (int)tenant : 0));
                }
                
                DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery((int)tenant);
                string documentTypeId = documentTypeQuery.GetDocumentTypeListIdByCodeAndTenant(DocumentCode, (int)tenant);


                DocumentOutQuery documentOutQuery = new DocumentOutQuery((int)tenant);
                ARInvoiceQuery aRInvoiceQuery = new ARInvoiceQuery((int)tenant);

                aRInvoice = aRInvoiceQuery.GetSingleARInvoice(SelectId, (int)tenant);

                doucmentOut = (from a in commonContext.DocumentOuts
                               join docFile in commonContext.DocumentsFilings on a.Id equals docFile.Id
                               where docFile.EntityId == SelectId && docFile.DocumentTypeId == documentTypeId && a.Tenant == tenant
                               select a).Include("DocumentsFiling").Include("DocumentsFiling.DocumentType").FirstOrDefault();

                if (doucmentOut != null)
                {
                    bool includeInPrint = true;

                    documentOutId = doucmentOut.Id;
                    tenant = doucmentOut.Tenant;
                    bool notGetCopy = false;

                    DocumentOutCopy copy = GetDocumentCopyByCode(commonContext, (int)tenant, documentOutId, DocumentCode);
                    if (aRInvoice != null)
                    {
                        string signStatus = aRInvoice.IsSigned;
                        if (signStatus == ARInvoiceSignedStatusValues.SignedButNotYetSent
                                || signStatus == ARInvoiceSignedStatusValues.SigningFailed
                                || signStatus == ARInvoiceSignedStatusValues.SignedAndSentByEmail
                                || signStatus == ARInvoiceSignedStatusValues.SignedButSendingByEmailFailed)
                            notGetCopy = true; 
                    }

                    if (copy != null && doucmentOut.DocumentsFiling.DocumentType.LimitedPrintCopyId == copy.DocumentTypeCopyId && !string.IsNullOrEmpty(copy.LastPrintedByUserId) && ((DocumentCode != "999G" && doucmentOut.DocumentsFiling.DocumentType.IsDocumentOneTimePrintLimited) || (DocumentCode == "999G" && !notGetCopy)))
                    {
                        copy = GetDocumentCopyByCode(commonContext, (int)tenant, documentOutId, "999G1");
                     }
                    if (copy == null)
                    {
                        NetCommonHelper.Logger.DevLog.Instance.WriteError("No valid document copy found for printing.");
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
                            NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"Attempting to download and merge document ID={documentId} with extension={documentExtension}.");
 
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
                                    NetCommonHelper.Logger.DevLog.Instance.WriteError("Failed to download file.");
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
