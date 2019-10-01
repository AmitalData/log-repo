using EvoPdf;
using Logitude.Server.Tools;
using HtmlAgilityPack;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Services;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Linq;
using WebFreight.Web.DataProviders;
using WebFreight.Web.Helpers;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.QuoteModel.EntityOtherServices;
using Logitude.BL.Helpers;
using Logitude.BL.QuoteModel.EntityQueries;
using Logitude.BL.QuoteModel.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;


namespace WebFreight.Web.WebServices
{
    /// <summary>
    /// Summary description for QuoteTemplateWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class QuoteTemplateWebService : System.Web.Services.WebService
    {

        string subject = "";
        string from = "";
        string replyTo = "";
        string cc = "";
        string bcc = "";

        [WebMethod]
        public byte[] GetQuoteTemplatePdfReport(string quoteId, string quoteTemplateId, string userId, int tenant)
        {
            QuoteTemplateEntityService QuoteTemplateService = new QuoteTemplateEntityService();
            byte[] data = QuoteTemplateService.GetQuoteTemplatePdfReport(quoteId, quoteTemplateId, userId, tenant, null);

            return data;
        }


        [WebMethod]
        public byte[] DownloadQuoteTemplateSectionDataFile(string documentId, int tenant)
        {

            QuoteTemplateReportHelper helper = new QuoteTemplateReportHelper();

            return helper.DownloadQuoteTemplateSectionDataFile(documentId, tenant);
        }



        [WebMethod]
        public byte[] DownloadQuoteTemplateSectionPdfFile(string sectionTypeCode, string sectionDocId, string quoteTemplateId, int tenant, string settingId, string quoteId)
        {
            QuoteTemplateReportHelper helper = new QuoteTemplateReportHelper();


            IQuotesContext context = QuotesContext.GetContext(tenant);
            QuoteQuery quoteQuery = new QuoteQuery(new QuoteRepository(context));
            QuoteTemplateQuery quoteTemplateQuery = new QuoteTemplateQuery(new QuoteTemplateRepository(context));
            QuoteTemplateSettingQuery quoteTemplateSettingQuery = new QuoteTemplateSettingQuery(new QuoteTemplateSettingRepository(context));
            QuoteTemplateSectionQuery sectionsQuery = new QuoteTemplateSectionQuery(new QuoteTemplateSectionRepository(context));
            QuoteTemplateTextDesignQuery quotetemplateTextDesignQuery = new QuoteTemplateTextDesignQuery(new QuoteTemplateTextDesignRepository(context));
            QuoteTemplateTableDesignQuery quotetemplateTableDesignQuery = new QuoteTemplateTableDesignQuery(new QuoteTemplateTableDesignRepository(context));
            QuoteTemplateTextCodeQuery quoteTemplateTextCodeQuery = new QuoteTemplateTextCodeQuery(tenant);

            QuoteTemplatePM template = quoteTemplateQuery.GetSinglePM(quoteTemplateId, tenant);
            List<QuoteTemplateSectionPM> templateSections = sectionsQuery.GetQuoteTemplateSectionPMsByTemplateId(quoteTemplateId, tenant);
            QuoteTemplateExcludedSectionRepository excludedSectionRepository = new QuoteTemplateExcludedSectionRepository(tenant);
            List<QuoteTemplateExcludedSection> excludedsection = excludedSectionRepository.GetAllQuoteTemplateExcludedSection(quoteId, quoteTemplateId, tenant).ToList();
            QuoteTemplateSettingPM setting = quoteTemplateSettingQuery.GetSinglePM(template.QuoteTemplateSettingId, tenant);
            List<QuoteTemplateTextDesignPM> quoteTemplateTextDesignsList = quotetemplateTextDesignQuery.GetQuoteTemplateTextDesignPMsByTenant(tenant).ToList();
            List<QuoteTemplateTableDesignPM> quoteTemplateTableDesignsList = quotetemplateTableDesignQuery.GetQuoteTemplateTableDesignPMsByTenant(tenant).ToList();
            List<QuoteTemplateTextCodePM> textcodes = quoteTemplateTextCodeQuery.GetQuoteTemplateTextCodePMsByQuoteTemplateId(template.Tenant, template.Id).ToList();

            QuotePM quotePM = null;
      
            if (!string.IsNullOrEmpty(quoteId))
            {
                quotePM = quoteQuery.GetSinglePM(quoteId, tenant);

            }

            if(quotePM == null)
            {
                quotePM = helper.BuildingQuotePM();
            }


            QuoteTemplateBuildArges quoteTemplateBuildArges = new QuoteTemplateBuildArges();
            quoteTemplateBuildArges.Tenant = tenant;
            quoteTemplateBuildArges.SectionTypeCode = sectionTypeCode;
            quoteTemplateBuildArges.QuotePM = quotePM;
            quoteTemplateBuildArges.QuoteTemplatePM = template;
            quoteTemplateBuildArges.QuoteTemplateSettingPM = setting;
            quoteTemplateBuildArges.QuoteTemplateTextCodePMLists = textcodes;
            quoteTemplateBuildArges.QuoteTemplateTableDesignsLists = quoteTemplateTableDesignsList;
            quoteTemplateBuildArges.QuoteTemplateTextDesignPMLists = quoteTemplateTextDesignsList;
            quoteTemplateBuildArges.QuotePM = quotePM;
            quoteTemplateBuildArges.QuoteTemplateSectionPMLists = templateSections;
            quoteTemplateBuildArges.Tenant = tenant;


            byte[] bodyData = null;

            switch (sectionTypeCode)
            {
                case "S":
                    bodyData = helper.DownloadQuoteTemplateSectionDataFile(sectionDocId, tenant);
                    break;

                case "PH":
                    bodyData = helper.GetQuoteTemplatePageHeaderFooter(quoteTemplateBuildArges);//GetQuoteTemplatePageHeader(quoteTemplateId, tenant);
                    break;


                case "QH":
                    bodyData = helper.GetQuoteTemplateHeader(quoteTemplateBuildArges);//GetQuoteTemplateHeader(quoteTemplateId, tenant, settingId, quoteId);
                    break;

                case "QD":
                    bodyData = helper.GetQuoteTemplateDetails(quoteTemplateBuildArges);//GetQuoteTemplateDetails(quoteTemplateId, tenant, settingId, quoteId);
                    break;

                case "PP":
                case "PC":
                    bodyData = helper.GetQuoteTemplatePricingHtmlData(quoteTemplateBuildArges);//GetQuoteTemplatePricingHtmlData(sectionTypeCode, quoteId, quoteTemplateId, tenant);
                    break;

                case "PF":
                    bodyData = helper.GetQuoteTemplatePageHeaderFooter(quoteTemplateBuildArges);
                    break;

                default:
                    bodyData = helper.DownloadQuoteTemplateSectionDataFile(sectionDocId, tenant);
                    break;


            }

            string bodyHtmlString = helper.GetBodyString(bodyData);

            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Quote", tenant, true);

            HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();
            bodyHtmlString = htmlEditorHelper.ResolveHtmlData("", objectTable.Id, template.CreatedByUserId, tenant, bodyHtmlString, ref subject, ref from, ref replyTo,ref cc, ref bcc, quotePM);

            return helper.HtmlToPdf(bodyHtmlString, setting);
        }


        [WebMethod]
        public string UploadQuoteTemplateSectionDataFile(byte[] data, string documentId, int tenant)
        {
            QuoteTemplateEntityService QuoteTemplateService = new QuoteTemplateEntityService();
            string dataString = QuoteTemplateService.UploadQuoteTemplateSectionDataFile(data, documentId, tenant);

            return dataString;
        }


        [WebMethod]
        public byte[] UpdateQuoteDocumentVersion(string quoteId, int versionNumber, string quoteTemplateId, string updatedByUserId, int tenant)
        {

            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            DocumentRepository documentRep = new DocumentRepository(commonContext);
            ObjectTableRepository tableRepository = new ObjectTableRepository(tenant);
            DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(commonContext);
            DocumentOutRepository documentOutRepository = new DocumentOutRepository(commonContext);

            IQuotesContext objectContext = QuotesContext.GetContext(tenant);
            QuoteDocumentVersionRepository quoteDocumentVersionRep = new QuoteDocumentVersionRepository(objectContext);
            QuoteRepository quoteRep = new QuoteRepository(objectContext);
            DocumentType documentType = documentTypeRepository.GetSingleDocumentTypeByCode("QUOTE", tenant);
            byte[] pdfData = null;
            if (documentType != null)
            {
                pdfData = this.GetQuoteTemplatePdfReport(quoteId, quoteTemplateId, updatedByUserId, tenant);

                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {

                    QuoteDocumentVersion version = quoteDocumentVersionRep.GetSingleQuoteDocumentVersion(quoteId, tenant, versionNumber);
                    Quote quote = quoteRep.GetSingleQuote(quoteId, tenant);

                    quote.LastVersionNumber = version.VersionNumber;
                    quote.QuoteTemplateId = version.QuoteTemplateId;

                    Simplog.Data.CommonDataModel.EntityPOCOs.Document document = documentRep.GetSingleDocument(tenant, version.DocumentId);
                
                    document.FileSize = Convert.ToInt32(pdfData.Length);
                    document.Extension = "pdf";
                    document.IsEncrypted = true;
                    version.VersionType = "G";
                    version.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    version.UpdatedByUserId = updatedByUserId;
                    version.QuoteTemplateId = quoteTemplateId;

                    DocumentOut documentout = documentOutRepository.GetDocumentOutByDocumentTypeAndEntity(quoteId, documentType.Id, tenant);
                    documentout.IsBlobExist = true;
                    documentout.Issued = true;
                    documentout.DocumentsFiling.UpdatedByUserId = updatedByUserId;
                    documentout.DocumentsFiling.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    documentOutRepository.Update(documentout);

                    commonContext.SaveChanges();

                    quoteRep.Update(quote);
                    quoteDocumentVersionRep.Update(version);

                    objectContext.SaveChanges();


                    string filename = document.Id + "." + document.Extension;
                    string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(filename.ToLower(), document.Folder);
                    IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                    BlobFileInfo fileInfo = new BlobFileInfo()
                    {
                        FileName = document.Id,
                        FolderName = document.Folder,
                        Extension = document.Extension,
                        Tenant = tenant,
                        FileSize = pdfData.Length,

                    };
                    storageservice.Write(pdfData, fileInfo);
                     

                    scope.Complete();


                }
            }
            else
            {
                throw new Exception("Document Type with code 'QUOTE' is not found!");
            }

            return pdfData;


        }

        [WebMethod]
        public void UpLoadQuoteDocumentVersionFile(string quoteId, int versionNumber, byte[] fileData, string fileExtension, string updatedByUserId, int tenant)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            DocumentRepository documentRep = new DocumentRepository(commonContext);
            ObjectTableRepository tableRepository = new ObjectTableRepository(tenant);
            DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(commonContext);
            DocumentOutRepository documentOutRepository = new DocumentOutRepository(commonContext);

            IQuotesContext objectContext = QuotesContext.GetContext(tenant);
            QuoteDocumentVersionRepository quoteDocumentVersionRep = new QuoteDocumentVersionRepository(objectContext);
            QuoteRepository quoteRep = new QuoteRepository(objectContext);
            DocumentType documentType = documentTypeRepository.GetSingleDocumentTypeByCode("QUOTE", tenant);
            if (documentType != null)
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    QuoteDocumentVersion version = quoteDocumentVersionRep.GetSingleQuoteDocumentVersion(quoteId, tenant, versionNumber);


                    Quote quote = quoteRep.GetSingleQuote(quoteId, tenant);
                    quote.LastVersionNumber = version.VersionNumber;
                    quote.QuoteTemplateId = version.QuoteTemplateId;

                    Simplog.Data.CommonDataModel.EntityPOCOs.Document document = documentRep.GetSingleDocument(tenant, version.DocumentId);
                    version.VersionType = "U";
                    version.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    version.UpdatedByUserId = updatedByUserId;

                    //string oldlocalPath = "quotetemplatesectionfiles/" + document.Id + "." + document.Extension;
                    //var blobContainer = StorageAcountDetails.GetCurrentContainer(tenant);
                    //var oldblobfile = blobContainer.GetBlockBlobReference(oldlocalPath);
                    //oldblobfile.Delete();


                    document.Extension = fileExtension;
                    document.FileSize = Convert.ToInt32(fileData.Length);
                    document.IsEncrypted = true;


                    DocumentOut documentout = documentOutRepository.GetDocumentOutByDocumentTypeAndEntity(quoteId, documentType.Id, tenant);
                    documentout.IsBlobExist = true;
                    documentout.Issued = true;
                    documentout.DocumentsFiling.UpdatedByUserId = updatedByUserId;
                    documentout.DocumentsFiling.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    documentOutRepository.Update(documentout);

                    commonContext.SaveChanges();

                    quoteDocumentVersionRep.Update(version);
                    quoteDocumentVersionRep.SubmitChanges();


                    string filename = document.Id + "." + document.Extension;
                    string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(filename.ToLower(), document.Folder);
                    IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;

                    BlobFileInfo fileInfo = new BlobFileInfo()
                    {
                        FileName = document.Id,
                        FolderName = document.Folder,
                        Extension = document.Extension,
                        Tenant = tenant,
                        FileSize = fileData.Length,

                    };

                   storageservice.Delete(fileInfo);
                   storageservice.Write(fileData, fileInfo);
                 
                  

                    scope.Complete();
                }

            }
            else
            {
                throw new Exception("Document Type with code 'QUOTE' is not found!");
            }
        }

    }
}