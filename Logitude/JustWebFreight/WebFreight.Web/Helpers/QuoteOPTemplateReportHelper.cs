using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using EvoPdf;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
#if false
    using Logitude.BL.QuoteModel.EntityPMs;
    using Logitude.BL.QuoteModel.EntityQueries;
#endif
using Logitude.Server.Tools.Helpers;
using Microsoft.WindowsAzure.Storage.Blob;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using HtmlAgilityPack;
using Microsoft.Practices.Unity;
using System.Globalization;
using WebFreight.Web.Helpers;
using Logitude.BL.Interfaces;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using WebFreight.Web.Security;
using Logitude.Server.Tools.StorageService;
using Logitude.Server.Tools;
using Document = Simplog.Data.CommonDataModel.EntityPOCOs.Document;
using Logitude.Server.Tools.Counters;

using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityLists;
using System.Text.RegularExpressions;

using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Amital.QuoteOPM.Def.EntityPMs;

namespace Logitude.BL.Helpers
{
    public class QuoteOPTemplateReportHelper : IQuoteOPTemplateReportHelper
    {

    }
#if false
    public class QuoteOPTemplateReportHelper : IQuoteOPTemplateReportHelper
    {
        private IQuotesContext context { get; set; }
        public bool ViewFixedPrice = false;
        int TdCount = 0;
        bool isRightToLeft;
        public int Tenant;
        string LocalCurrencyCode;
        bool IsShowlanguage = new bool();
        string NameTextCode;
        TenantPM tenantPm = null;

        public byte[] BuildQuoteOPTemplatePdfReport(string quoteId, string QuoteOPTemplateId, string userId, int tenant, List<QuoteOPTemplateSectionPM> templateSections, int? userTenant = null, QuoteOPPM QuoteOPPM = null, int? versionNumber = null)
        {


            QuoteOPTemplateBuildArges QuoteOPTemplateBuildArges = new QuoteOPTemplateBuildArges();
            IQuotesContext context = QuotesContext.GetContext(tenant);
            QuoteQuery quoteQuery = new QuoteQuery(new QuoteRepository(context));
            QuoteOPTemplateQuery QuoteOPTemplateQuery = new QuoteOPTemplateQuery(new QuoteOPTemplateRepository(context));
            QuoteOPTemplateSettingQuery QuoteOPTemplateSettingQuery = new QuoteOPTemplateSettingQuery(new QuoteOPTemplateSettingRepository(context));
            QuoteOPTemplateSectionQuery sectionsQuery = new QuoteOPTemplateSectionQuery(new QuoteOPTemplateSectionRepository(context));
            QuoteOPTemplateTextDesignQuery QuoteOPTemplateTextDesignQuery = new QuoteOPTemplateTextDesignQuery(new QuoteOPTemplateTextDesignRepository(context));
            QuoteOPTemplateTableDesignQuery QuoteOPTemplateTableDesignQuery = new QuoteOPTemplateTableDesignQuery(new QuoteOPTemplateTableDesignRepository(context));
            QuoteOPTemplateTextCodeQuery QuoteOPTemplateTextCodeQuery = new QuoteOPTemplateTextCodeQuery(tenant);
            HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();
            byte[] pdfData = null;
            string headerHtmlString = null;
            string footerHtmlString = null;
            string bodyHtmlString = null;
            string RequestArea = "Maintenance";
            QuoteOPTemplatePM template = QuoteOPTemplateQuery.GetSinglePM(QuoteOPTemplateId, tenant);
            ObjectTableRepository objectTabelRepository = null;
            ObjectTable objectTable = null;
            int correctTenant = userTenant != null ? (int)userTenant : tenant;

            if (!string.IsNullOrEmpty(quoteId) && QuoteOPPM == null)
            {
                QuoteOPPM = quoteQuery.GetSinglePM(quoteId, correctTenant);
            }

            if (QuoteOPPM == null) QuoteOPPM = BuildingQuoteOPPM();

            if (templateSections == null)
            {
                if (QuoteOPPM.QuoteTemplateId == QuoteOPTemplateId)
                {
                    if (!string.IsNullOrEmpty(QuoteOPPM.QuotationSections))
                    {
                        templateSections = sectionsQuery.GetQuoteOPTemplateSectionPMsByIds(QuoteOPPM.QuotationSections.Split(',').ToList(), tenant);
                    }
                }

                if (templateSections == null)
                {
                    templateSections = sectionsQuery.GetQuoteOPTemplateSectionPMsByTemplateId(QuoteOPTemplateId, tenant);
                }

                QuoteOPTemplateSectionRepository QuoteOPTemplateSectionsRepository = new QuoteOPTemplateSectionRepository(context);
                List<QuoteOPTemplateSectionModification> modifications = QuoteOPTemplateSectionsRepository.GetAllQuoteOPTemplateSectionModifications(QuoteOPPM.Id, tenant).ToList();
                foreach (QuoteOPTemplateSectionModification mod in modifications)
                {
                    QuoteOPTemplateSectionPM section = templateSections.Where(s => s.Id == mod.QuoteOPTemplateSectionId).FirstOrDefault();
                    if (section != null)
                    {
                        section.SectionDocId = mod.SectionDocId;
                    }
                }

                QuoteOPTemplateExcludedSectionRepository excludedSectionRepository = new QuoteOPTemplateExcludedSectionRepository(tenant);
                List<QuoteOPTemplateExcludedSection> excludedsection = excludedSectionRepository.GetAllQuoteOPTemplateExcludedSection(quoteId, QuoteOPTemplateId, tenant).ToList();

                if (excludedsection != null)
                {
                    foreach (QuoteOPTemplateExcludedSection mod2 in excludedsection)
                    {
                        QuoteOPTemplateSectionPM section = templateSections.Where(s => s.Id == mod2.QuoteOPTemplateSectionId).FirstOrDefault();
                        if (section != null)
                        {
                            section.IsExcluded = true;
                        }
                    }
                }
            }


            QuoteOPTemplateSettingPM setting = QuoteOPTemplateSettingQuery.GetSinglePM(template.QuoteOPTemplateSettingId, tenant);
            List<QuoteOPTemplateTextDesignPM> QuoteOPTemplateTextDesignsList = QuoteOPTemplateTextDesignQuery.GetQuoteOPTemplateTextDesignPMsByTenant(tenant).ToList();
            List<QuoteOPTemplateTableDesignPM> QuoteOPTemplateTableDesignsList = QuoteOPTemplateTableDesignQuery.GetQuoteOPTemplateTableDesignPMsByTenant(tenant).ToList();
            List<QuoteOPTemplateTextCodePM> textcodes = QuoteOPTemplateTextCodeQuery.GetQuoteOPTemplateTextCodePMsByQuoteOPTemplateId(template.Tenant, template.Id).ToList();



            if (!string.IsNullOrEmpty(quoteId))
            {
                RequestArea = "Quote";
                if (QuoteOPPM != null)
                {
                    if (QuoteOPPM.TransportModeId == "A" || QuoteOPPM.ShipmentTypeId == "LCL" || QuoteOPPM.ShipmentTypeId == "LCLD" || QuoteOPPM.ShipmentTypeId == "LTL")
                    {
                        templateSections = templateSections.Where(t => t.QuoteOPTemplateSectionTypeCode != "PC").ToList();

                    }
                    else if (QuoteOPPM.ShipmentTypeId == "FCL" || QuoteOPPM.ShipmentTypeId == "FCLD" || QuoteOPPM.ShipmentTypeId == "FTL")
                    {
                        templateSections = templateSections.Where(t => t.QuoteOPTemplateSectionTypeCode != "PP").ToList(); ;
                    }
                }
            }



            QuoteOPTemplateBuildArges.Tenant = tenant;
            QuoteOPTemplateBuildArges.IsResultPDF = true;
            QuoteOPTemplateBuildArges.QuoteOPPM = QuoteOPPM;
            QuoteOPTemplateBuildArges.QuoteOPTemplateSettingPM = setting;
            QuoteOPTemplateBuildArges.QuoteOPTemplateTextDesignPMLists = QuoteOPTemplateTextDesignsList;
            QuoteOPTemplateBuildArges.QuoteOPTemplateTableDesignsLists = QuoteOPTemplateTableDesignsList;
            QuoteOPTemplateBuildArges.QuoteOPTemplateTextCodePMLists = textcodes;
            QuoteOPTemplateBuildArges.UserTenant = correctTenant;
            QuoteOPTemplateBuildArges.UserId = userId;
            QuoteOPTemplateBuildArges.QuoteOPTemplateSectionPMLists = templateSections;
            QuoteOPTemplateBuildArges.QuoteOPTemplatePM = template;
            QuoteOPTemplateBuildArges.VersionNumber = versionNumber;


            HtmlToPdfConverter pdfConverter = new HtmlToPdfConverter();
            pdfConverter.LicenseKey = "fvDj8eTh8eDg4vHk/+Hx4uD/4OP/6Ojo6A==";
            pdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.A4;
            pdfConverter.HtmlViewerWidth = 800;
            pdfConverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Normal;
            pdfConverter.PdfDocumentOptions.EnhancedGraphicsQuality = true;
            pdfConverter.PdfDocumentOptions.PdfPageOrientation = PdfPageOrientation.Portrait;
            pdfConverter.TriggeringMode = TriggeringMode.Auto;
            SetPdfMargins(setting, pdfConverter.PdfDocumentOptions);


            // set the header HTML area
            QuoteOPTemplateSectionPM headerSection = templateSections.Where(s => s.QuoteOPTemplateSectionTypeCode == "PH").FirstOrDefault();
            if (!headerSection.IsExcluded)
            {
                pdfConverter.PdfDocumentOptions.ShowHeader = true;
                if (setting.PageHeaderArea1Type == "Quote Header" || setting.PageHeaderArea2Type == "Quote Header" || setting.PageHeaderArea3Type == "Quote Header") QuoteOPTemplateBuildArges.HideQuoteHeaderFromPdf = true;
                SetSectionTypeCode(QuoteOPTemplateBuildArges, "PH");
                byte[] headerdata = GetQuoteOPTemplatePageHeaderFooter(QuoteOPTemplateBuildArges);
                headerHtmlString += GetBodyString(headerdata);



                headerHtmlString = ResolveHtmlData(correctTenant, htmlEditorHelper, headerHtmlString, QuoteOPPM, template, userId, ref objectTabelRepository, ref objectTable);
                HtmlToPdfElement headerHtml = new HtmlToPdfElement(0, 0, 0, 0, headerHtmlString, null, 2040, 0);
                pdfConverter.PdfHeaderOptions.AddElement(headerHtml);
                pdfConverter.PdfHeaderOptions.HeaderHeight = 1;
                pdfConverter.PdfHeaderOptions.HeaderHeight = setting.PageHeaderAreaHeight * 29;
            

            }


            // set the Footer HTML area
            QuoteOPTemplateSectionPM footerSection = templateSections.Where(s => s.QuoteOPTemplateSectionTypeCode == "PF").FirstOrDefault();
            if (!footerSection.IsExcluded)
            {
                pdfConverter.PdfDocumentOptions.ShowFooter = true;
                SetSectionTypeCode(QuoteOPTemplateBuildArges, "PF");
                byte[] footerdata = GetQuoteOPTemplatePageHeaderFooter(QuoteOPTemplateBuildArges);
                footerHtmlString += GetBodyString(footerdata);

                double footerTopMargin = (double)setting.SpaceLinesBeforeFooters * 21;
                float heightFooter = (setting.PageFooterAreaHeight * 29) + (float)footerTopMargin + 5;

                footerHtmlString = ResolveHtmlData(correctTenant, htmlEditorHelper, footerHtmlString, QuoteOPPM, template, userId, ref objectTabelRepository, ref objectTable);
                HtmlToPdfElement footerHtml = new HtmlToPdfElement(0, 0, 0, 0, footerHtmlString, null, 2040, 0);
                pdfConverter.PdfFooterOptions.AddElement(footerHtml);
                pdfConverter.PdfFooterOptions.FooterHeight = (heightFooter + 10);

                if (!setting.HidePageNumber)
                {
                    QuoteOPTemplateTextDesignPM QuoteOPTemplateTextDesignPMPageNumbering = QuoteOPTemplateTextDesignsList.Where(t => t.Id == setting.PageNumberingTextDesignId).FirstOrDefault();
                    TextElement footerTextElement = null;
                    if (QuoteOPTemplateTextDesignPMPageNumbering != null)
                    {
                        footerTextElement = GetTextElementProperitiesForQuoteOPTemplateTextDesign(QuoteOPTemplateTextDesignPMPageNumbering, heightFooter);
                        pdfConverter.PdfFooterOptions.FooterHeight += Convert.ToSingle(QuoteOPTemplateTextDesignPMPageNumbering.FontSize) - 7;
                    }
                    else 
                    {
                        footerTextElement = new TextElement(0, heightFooter, "page &p; of &P;  ", new Font(new System.Drawing.FontFamily("Times New Roman"), 7, GraphicsUnit.Point));
                        footerTextElement.TextAlign = HorizontalTextAlign.Right;
                    }
                    pdfConverter.PdfFooterOptions.AddElement(footerTextElement);
                }


            }
            else pdfConverter.PdfFooterOptions.FooterHeight = 1;


            

            //Body

            SetSectionTypeCode(QuoteOPTemplateBuildArges, null);
            byte[] bodyData = GetQuoteOPTemplateHtmlReport(QuoteOPTemplateBuildArges, false, RequestArea);
            bodyHtmlString = GetBodyString(bodyData);

            HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
            htmlDocument.LoadHtml(bodyHtmlString);
            var nodes = htmlDocument.DocumentNode.Elements("p");
            foreach (HtmlAgilityPack.HtmlNode node in nodes)
            {
                if (node.Attributes["dir"] != null && node.Attributes["dir"].Value.ToString() == "RTL")
                {
                    int childnodescount = node.ChildNodes.Count;
                    if (childnodescount > 1)
                    {
                        List<HtmlNode> deletedNodes = new List<HtmlNode>();
                        for (int i = 1; i < childnodescount; i++)
                        {
                            node.ChildNodes[0].InnerHtml = node.ChildNodes[0].InnerHtml + node.ChildNodes[i].InnerText;
                            deletedNodes.Add(node.ChildNodes[i]);
                        }

                        foreach (HtmlNode dnode in deletedNodes) node.ChildNodes.Remove(dnode);

                    }
                }
            }
            bodyHtmlString = htmlDocument.DocumentNode.InnerHtml;
            bodyHtmlString = ResolveHtmlData(correctTenant, htmlEditorHelper, bodyHtmlString, QuoteOPPM, template, userId, ref objectTabelRepository, ref objectTable);

            pdfData = pdfConverter.ConvertHtml(bodyHtmlString, null);
            if (QuoteOPPM.Id != "10697")
            {
                string fullHtml = headerHtmlString + bodyHtmlString + footerHtmlString;
                CreateHtmlQuotationDocument(tenant, QuoteOPPM, fullHtml);
            }

            return pdfData;
        }


        private TextElement GetTextElementProperitiesForQuoteOPTemplateTextDesign(QuoteOPTemplateTextDesignPM QuoteOPTemplateTextDesignPMPageNumbering, float heightFooter)
        {
            FontFamily family = new FontFamily(QuoteOPTemplateTextDesignPMPageNumbering.FontFamily);
            bool isBold = QuoteOPTemplateTextDesignPMPageNumbering.FontWeight.ToLower() == "bold";
            bool isItalic = QuoteOPTemplateTextDesignPMPageNumbering.Italic;
            bool isUnderline = QuoteOPTemplateTextDesignPMPageNumbering.UnDerLine;
            Color backGroundColor = System.Drawing.ColorTranslator.FromHtml(QuoteOPTemplateTextDesignPMPageNumbering.BackgroundColor);
            Color ForeColor = System.Drawing.ColorTranslator.FromHtml(QuoteOPTemplateTextDesignPMPageNumbering.TextColor);
            HorizontalTextAlign textAlign = QuoteOPTemplateTextDesignPMPageNumbering.Alignment.ToLower() == "right" ? HorizontalTextAlign.Right : QuoteOPTemplateTextDesignPMPageNumbering.Alignment.ToLower() == "left" ? HorizontalTextAlign.Left : HorizontalTextAlign.Center;
            float size = Convert.ToSingle(QuoteOPTemplateTextDesignPMPageNumbering.FontSize);
            
            Font font = new Font(family, size, (isBold ? FontStyle.Bold : FontStyle.Regular) | (isItalic ? FontStyle.Italic : FontStyle.Regular) | (isUnderline ? FontStyle.Underline : FontStyle.Regular), GraphicsUnit.Point);

            TextElement footerTextElement = new TextElement(0, heightFooter, "page &p; of &P;  ", font);
            footerTextElement.TextAlign = textAlign;
            footerTextElement.BackColor = backGroundColor;
            footerTextElement.ForeColor = ForeColor;

            return footerTextElement;
        }
        private  void SetPdfMargins(QuoteOPTemplateSettingPM setting, PdfDocumentOptions PdfDocumentOptions)
        {
            if (setting != null)
            {
                int marginleft = (setting.QuoteTemplatePDFMarginLeft * 72) / 96;
                int marginRight = (setting.QuoteTemplatePDFMarginRight * 72) / 96;
                int marginTop = (setting.QuoteTemplatePDFMarginTop * 72) / 96;
                int marginBottom = (setting.QuoteTemplatePDFMarginBottom * 72) / 96;


                PdfDocumentOptions.LeftMargin = marginleft;
                PdfDocumentOptions.RightMargin = marginRight;
                PdfDocumentOptions.TopMargin = marginTop;
                PdfDocumentOptions.BottomMargin = marginBottom;

            }
        }

        private static void CreateHtmlQuotationDocument(int tenant, QuoteOPPM QuoteOPPM, string fullHtml)
        {
            if (!string.IsNullOrEmpty(fullHtml))
            {
                var fullQuotationHtmlByte = Encoding.UTF8.GetBytes(fullHtml);
                DocumentRepository documentRep = new DocumentRepository(tenant);
                Document document = new Document()
                {
                    FileName = "QuotationHtml-" + QuoteOPPM.QuoteNumber,
                    CalculatedFileName = "QuotationHtml-" + QuoteOPPM.QuoteNumber,
                    CreateDate = DateTime.Now,
                    Extension = "html",
                    FileSize = fullQuotationHtmlByte.Length,
                    Tenant = tenant,
                    Id = IdCounter.GetNumber("Document", tenant),
                    HasFile = true,
                    Folder = "others",
                    IsEncrypted = true,
                };
                documentRep.Add(document);


                string filename = document.Id + "." + document.Extension;
                string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(filename.ToLower(), document.Folder);
                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = document.Id,
                    FolderName = document.Folder,
                    Extension = document.Extension,
                    Tenant = tenant,
                    FileSize = fullQuotationHtmlByte.Length,
                    IsEncrypted = true,
                };

                storageservice.Write(fullQuotationHtmlByte, fileInfo);
                documentRep.SubmitChanges();
                QuoteOPPM.QuoteHTMLDocumentId = document.Id;
            }
        }

        private static void SetSectionTypeCode(QuoteOPTemplateBuildArges QuoteOPTemplateBuildArges, string sectionTypeCode)
        {
            QuoteOPTemplateBuildArges.SectionTypeCode = sectionTypeCode;
        }

        private string ResolveHtmlData(int tenant, HtmlEditorHelper htmlEditorHelper, string htmlString, QuoteOPPM QuoteOPPM, QuoteOPTemplatePM template, string userId, ref ObjectTableRepository objectTabelRepository, ref ObjectTable objectTable)
        {

            if (!string.IsNullOrEmpty(htmlString) && (htmlString.Contains("[") || htmlString.Contains("]")))
            {
                if (objectTable == null)
                {
                    objectTabelRepository = new ObjectTableRepository(tenant);
                    objectTable = objectTabelRepository.GetObjectTableByName("Quote", tenant, true);
                }

                HtmlEditorResolveArgs htmlEditorResolveArgs = new HtmlEditorResolveArgs()
                {
                   
                    UserId = userId,
                    ObjectTableId = objectTable.Id,
                    DocumentTemplateId = template.Id,
                    Tenant = tenant,
                    HtmlString = htmlString,
                };


                var htmlEditorResolveResult = htmlEditorHelper.ResolveHtmlData(htmlEditorResolveArgs, QuoteOPPM);
                htmlString = htmlEditorResolveResult.HtmlString;
            }
            return htmlString;
        }



    #region Quote Template Page Header And Footer

        public byte[] GetQuoteOPTemplatePageHeaderFooter(QuoteOPTemplateBuildArges QuoteOPTemplateBuildArges)
        {

            QuoteOPTemplatePM template = QuoteOPTemplateBuildArges.QuoteOPTemplatePM;
            QuoteOPTemplateSettingPM setting = QuoteOPTemplateBuildArges.QuoteOPTemplateSettingPM;
            List<QuoteOPTemplateTextDesignPM> QuoteOPTemplateTextDesignsList = QuoteOPTemplateBuildArges.QuoteOPTemplateTextDesignPMLists;
            int tenant = QuoteOPTemplateBuildArges.Tenant;
            bool isPdf = QuoteOPTemplateBuildArges.IsResultPDF;
            string sessiontype = QuoteOPTemplateBuildArges.SectionTypeCode == "PH" ? "Header" : "Footer";

            string type = sessiontype;
            int NumberOfTd = 0;
            int CountArea = 0;
            double heightAreaNumber = 0;
            string widthArea1 = type == "Header" ? setting.PageHeaderArea1Width + "px" : setting.PageFooterArea1Width + "px";
            string widthArea2 = type == "Header" ? setting.PageHeaderArea2Width + "px" : setting.PageFooterArea2Width + "px";
            string widthArea3 = type == "Header" ? setting.PageHeaderArea3Width + "px" : setting.PageFooterArea3Width + "px";
            string HeightArea = "";

            StringBuilder HtmlTemplate = new StringBuilder();

            int Centimeter = type == "Header" ? setting.PageHeaderAreaHeight : setting.PageFooterAreaHeight;

            var per = isPdf ? 2.4 : 1;
            heightAreaNumber = CmToPx(Centimeter) * per;
            if (Centimeter == 1 || Centimeter == 2) heightAreaNumber -= 5;

            HeightArea = heightAreaNumber.ToString() + "px";

            QuoteOPTemplateTableDesignPM TableDesign = new QuoteOPTemplateTableDesignPM()
            {
                BorderColor = type == "Header" ? setting.PageHeaderBorderColor : setting.PageFooterBorderColor,
                BorderThickness = type == "Header" ? setting.PageHeaderBorderThickness : setting.PageFooterBorderThickness,
                BorderTypeCode = type == "Header" ? setting.PageHeaderBorderTypeCode : setting.PageFooterBorderTypeCode,
            };

            HtmlTemplate.Append("<!DOCTYPE html>");
            HtmlTemplate.Append("<html  lang='ar'>");
            HtmlTemplate.Append("<head>");
            HtmlTemplate.Append("<title></title>");
            HtmlTemplate.Append("<meta charset='utf-8'>");
            HtmlTemplate.Append("</head>");
            HtmlTemplate.Append("<body>");

            if (type == "Footer")
            {
                var perc = isPdf ? 2.2 : 1;
                var spaceLinesBefore = GetHtmlStringLine(setting.SpaceLinesBeforeFooters, false, perc);
                HtmlTemplate.Append(spaceLinesBefore);
            }

            string dir = "dir='RTL'";
            if (!setting.RightToLeft) dir = "";
            HtmlTemplate.Append("<div  style='width:100%; height:" + HeightArea + "'>");
            string styleTable = GetStyleTableHeaderFooter(TableDesign, HeightArea);
            HtmlTemplate.Append("<table " + dir + " width='100%' " + styleTable + " >");
            string styleTr = "style='" + "Height:" + HeightArea + "'";
            HtmlTemplate.Append("<tr>");

    #region prop
            string pageArea1Type = type == "Header" ? setting.PageHeaderArea1Type : setting.PageFooterArea1Type;
            string area1ImageDetailId = type == "Header" ? setting.PageHeaderArea1ImageDetailId : setting.PageFooterArea1ImageDetailId;
            double? area1Width = type == "Header" ? setting.PageHeaderArea1Width : setting.PageFooterArea1Width;
            int area1Height = type == "Header" ? setting.PageHeaderArea1Height : setting.PageFooterArea1Height;
            int image1Width = type == "Header" ? setting.PageHeaderImage1Width : setting.PageFooterImage1Width;
            string area1ImageAlignment = type == "Header" ? setting.PageHeaderArea1ImageAlignment : setting.PageFooterArea1ImageAlignment;
            string area1FreeText = type == "Header" ? setting.PageHeaderArea1FreeText : setting.PageFooterArea1FreeText;
            string area1FreeTextDesignId = type == "Header" ? setting.PageHeaderArea1FreeTextDesignId : setting.PageFooterArea1FreeTextDesignId;

            string pageArea2Type = type == "Header" ? setting.PageHeaderArea2Type : setting.PageFooterArea2Type;
            string area2ImageDetailId = type == "Header" ? setting.PageHeaderArea2ImageDetailId : setting.PageFooterArea2ImageDetailId;
            double? area2Width = type == "Header" ? setting.PageHeaderArea2Width : setting.PageFooterArea2Width;
            int area2Height = type == "Header" ? setting.PageHeaderArea2Height : setting.PageFooterArea2Height;
            int image2Width = type == "Header" ? setting.PageHeaderImage2Width : setting.PageFooterImage2Width;
            string area2ImageAlignment = type == "Header" ? setting.PageHeaderArea2ImageAlignment : setting.PageFooterArea2ImageAlignment;
            string area2FreeText = type == "Header" ? setting.PageHeaderArea2FreeText : setting.PageFooterArea2FreeText;
            string area2FreeTextDesignId = type == "Header" ? setting.PageHeaderArea2FreeTextDesignId : setting.PageFooterArea2FreeTextDesignId;

            string pageArea3Type = type == "Header" ? setting.PageHeaderArea3Type : setting.PageFooterArea3Type;
            string area3ImageDetailId = type == "Header" ? setting.PageHeaderArea3ImageDetailId : setting.PageFooterArea3ImageDetailId;
            double? area3Width = type == "Header" ? setting.PageHeaderArea3Width : setting.PageFooterArea3Width;
            int area3Height = type == "Header" ? setting.PageHeaderArea3Height : setting.PageFooterArea3Height;
            int image3Width = type == "Header" ? setting.PageHeaderImage3Width : setting.PageFooterImage3Width;
            string area3ImageAlignment = type == "Header" ? setting.PageHeaderArea3ImageAlignment : setting.PageFooterArea3ImageAlignment;
            string area3FreeText = type == "Header" ? setting.PageHeaderArea3FreeText : setting.PageFooterArea3FreeText;
            string area3FreeTextDesignId = type == "Header" ? setting.PageHeaderArea3FreeTextDesignId : setting.PageFooterArea3FreeTextDesignId;
            string borderTypeCode = type == "Header" ? setting.PageHeaderBorderTypeCode : setting.PageFooterBorderTypeCode;

    #endregion


            if (pageArea1Type == "Logo" && area1Width > 0)
            {
                AppendHeaderFooterAreaImage(borderTypeCode, area1Width, area1Height, image1Width, area1ImageAlignment, area1ImageDetailId, tenant, ref NumberOfTd, ref CountArea, HeightArea, HtmlTemplate, TableDesign, isPdf, (int)heightAreaNumber);
            }
            else if (pageArea1Type == "Text" && area1Width > 0)
            {
                AppendHeaderFooterAreaText(borderTypeCode, area1Width, area1FreeText, area1FreeTextDesignId, QuoteOPTemplateTextDesignsList, ref NumberOfTd, ref CountArea, widthArea1, HeightArea, HtmlTemplate, TableDesign, isPdf);
            }
            else if (pageArea1Type == "Quote Header" && type == "Header" && area1Width > 0)
            {
                AppendHeaderFooterAreaQuoteHeaderText(borderTypeCode, area1Width, ref NumberOfTd, ref CountArea, widthArea1, HeightArea, HtmlTemplate, TableDesign, isPdf, QuoteOPTemplateBuildArges);
            }



            if (pageArea2Type == "Logo" && area2Width > 0)
            {
                AppendHeaderFooterAreaImage(borderTypeCode, area2Width, area2Height, image2Width, area2ImageAlignment, area2ImageDetailId, tenant, ref NumberOfTd, ref CountArea, HeightArea, HtmlTemplate, TableDesign, isPdf, (int)heightAreaNumber);
            }
            else if (pageArea2Type == "Text" && area2Width > 0)
            {
                AppendHeaderFooterAreaText(borderTypeCode, area2Width, area2FreeText, area2FreeTextDesignId, QuoteOPTemplateTextDesignsList, ref NumberOfTd, ref CountArea, widthArea2, HeightArea, HtmlTemplate, TableDesign, isPdf);
            }
            else if (pageArea2Type == "Quote Header" && type == "Header" && area2Width > 0)
            {
                AppendHeaderFooterAreaQuoteHeaderText(borderTypeCode, area2Width, ref NumberOfTd, ref CountArea, widthArea2, HeightArea, HtmlTemplate, TableDesign, isPdf, QuoteOPTemplateBuildArges);
            }

            if (pageArea3Type == "Logo" && area3Width > 0)
            {
                AppendHeaderFooterAreaImage(borderTypeCode, area3Width, area3Height, image3Width, area3ImageAlignment, area3ImageDetailId, tenant, ref NumberOfTd, ref CountArea, HeightArea, HtmlTemplate, TableDesign, isPdf, (int)heightAreaNumber);
            }

            else if (pageArea3Type == "Text" && area3Width > 0)
            {
                AppendHeaderFooterAreaText(borderTypeCode, area3Width, area3FreeText, area3FreeTextDesignId, QuoteOPTemplateTextDesignsList, ref NumberOfTd, ref CountArea, widthArea3, HeightArea, HtmlTemplate, TableDesign, isPdf);
            }

            else if (pageArea3Type == "Quote Header" && type == "Header" && area3Width > 0)
            {
                AppendHeaderFooterAreaQuoteHeaderText(borderTypeCode, area3Width, ref NumberOfTd, ref CountArea, widthArea3, HeightArea, HtmlTemplate, TableDesign, isPdf, QuoteOPTemplateBuildArges);
            }

            HtmlTemplate.Append("</tr>");
            HtmlTemplate.Append("</table>");
            HtmlTemplate.Append("</div>");
            HtmlTemplate.Append("</body>");
            HtmlTemplate.Append("</html>");

            return Encoding.UTF8.GetBytes(HtmlTemplate.ToString());
        }

        private void AppendHeaderFooterAreaQuoteHeaderText(string borderTypeCode, double? areaWidth, ref int NumberOfTd, ref int CountArea, string widthArea, string HeightArea, StringBuilder HtmlTemplate, QuoteOPTemplateTableDesignPM TableDesign, bool isPdf, QuoteOPTemplateBuildArges QuoteOPTemplateBuildArges)
        {
            if (QuoteOPTemplateBuildArges != null)
            {
                string styleTd = GetStyleRowTablePageHeaderFooter(borderTypeCode, HeightArea, areaWidth, "Left", ++NumberOfTd, TableDesign, "#ffffff");
                styleTd += "^";
                styleTd = styleTd.Replace("'^", "");
                string padding = isPdf ? "10px" : "5px";
                styleTd += ";padding:" + padding + "'";


                int tenant = QuoteOPTemplateBuildArges.Tenant;
                QuoteOPTemplateBuildArges.RequestArea = "Header";
                var quoteHeaderHtmlByte = GetQuoteOPTemplateHeader(QuoteOPTemplateBuildArges);

                string quoteHeaderHtml = "<td " + styleTd + ">" + Encoding.UTF8.GetString(quoteHeaderHtmlByte) + "</td>";

                if (isPdf)
                {
                    HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();
                    quoteHeaderHtml = htmlEditorHelper.ConvertNormalHtmlToEvoHtml(quoteHeaderHtml);
                }

                HtmlTemplate.Append(quoteHeaderHtml);

            }
        }

        private void AppendHeaderFooterAreaText(string borderTypeCode, double? areaWidth, string freeText, string freeTextDesignId, List<QuoteOPTemplateTextDesignPM> QuoteOPTemplateTextDesignsList, ref int NumberOfTd, ref int CountArea, string widthArea, string HeightArea, StringBuilder HtmlTemplate, QuoteOPTemplateTableDesignPM TableDesign, bool isPdf)
        {
            if (string.IsNullOrEmpty(freeText)) freeText = "";

            freeText = freeText.Replace('\n', '\r');

            QuoteOPTemplateTextDesignPM freeTextDesign = QuoteOPTemplateTextDesignsList.Where(t => t.Id == freeTextDesignId).FirstOrDefault();

            string styleTd = GetStyleRowTablePageHeaderFooter(borderTypeCode, HeightArea, areaWidth, freeTextDesign.Alignment, ++NumberOfTd, TableDesign, freeTextDesign.BackgroundColor);
            HtmlTemplate.Append("<td " + styleTd + ">");
            //  HtmlTemplate.Append("<td style='max-Height:" + HeightArea + " + styleTd + " > ");




            var per = isPdf ? 2.5 : 1;

            string StyleSpan = GetSpanRowStyle(freeTextDesign, "", widthArea, per);
            if (freeTextDesign.Italic)
            {
                HtmlTemplate.Append("<span " + StyleSpan + ">" + "<i>" + freeText.Replace("\r", "<br />") + "</i>" + "</span> " + "</td>");
            }
            else
            {
                HtmlTemplate.Append("<span " + StyleSpan + ">" + freeText.Replace("\r", "<br />") + "</span> " + "</td>");
            }
            ++CountArea;
        }

        private void AppendHeaderFooterAreaImage(string borderTypeCode, double? areaWidth, int areaHeight, int imageWidth, string areaImageAlignment, string imageDetailId, int tenant, ref int NumberOfTd, ref int CountArea, string HeightArea, StringBuilder HtmlTemplate, QuoteOPTemplateTableDesignPM TableDesign, bool isPdf, int heightAreaNumber)
        {
            string styleTd = GetStyleRowTablePageHeaderFooter(borderTypeCode, HeightArea, areaWidth, areaImageAlignment, ++NumberOfTd, TableDesign, "#ffffff");

            var per = isPdf ? 2.5 : 1;
            string Height = ((double)areaHeight * per) > heightAreaNumber ? "100%" : ((double)areaHeight * per).ToString() + "px";
            string Width = ((int)imageWidth * per) > 820 ? "100%" : ((double)imageWidth * per).ToString() + "px";
            // heightAreaNumber
            string margin = isPdf ? ";margin:2px" : "";
            string styleimage = "style='" + "width:" + Width + margin + ";Height:" + Height + ";max-Height:" + HeightArea + ";text-align:" + areaImageAlignment + "'";

            HtmlTemplate.Append("<td " + styleTd + ">");
            HtmlTemplate.Append("<span " + styleimage + ">");
            if (!string.IsNullOrEmpty(imageDetailId)) HtmlTemplate.Append(GetImageHtmlString(imageDetailId, tenant, styleimage));
            HtmlTemplate.Append("</span>");
            HtmlTemplate.Append("</td>");
            ++CountArea;
        }

        private int CmToPx(int cm)
        {
            return cm * 38;
        }
    #endregion


        public string GetHtmlStringLine(int line, bool firstSectionInBody  , double per=1)
        {
           string result = string.Empty;
            string firstSectionInBodyId = string.Empty;
            if (line > 0)
            {
                double lineheight = (double)line * (double)30;
                lineheight = per * (double)lineheight;
                if (firstSectionInBody)
                {
                    lineheight = lineheight - 10;
                    firstSectionInBodyId = "FirstSectionInBody";
                }

                result = "<div id='" + firstSectionInBodyId + "'  style='height:" + lineheight.ToString() + "px;'>" + " &nbsp;  &nbsp; &nbsp; &nbsp; &nbsp;  &nbsp;   &nbsp;" + "</div>";
            }
            return result;
        }

        public byte[] GetQuoteOPTemplateHtmlReport(QuoteOPTemplateBuildArges QuoteOPTemplateBuildArges, bool includeHeaderFooter, string requestArea )
        {

            
            QuoteOPPM QuoteOPPM = QuoteOPTemplateBuildArges.QuoteOPPM;
            QuoteOPTemplatePM template = QuoteOPTemplateBuildArges.QuoteOPTemplatePM;
            QuoteOPTemplateSettingPM setting = QuoteOPTemplateBuildArges.QuoteOPTemplateSettingPM;
            List<QuoteOPTemplateTextDesignPM> QuoteOPTemplateTextDesignsList = QuoteOPTemplateBuildArges.QuoteOPTemplateTextDesignPMLists;
            List<QuoteOPTemplateTableDesignPM> QuoteOPTemplateTableDesignsList = QuoteOPTemplateBuildArges.QuoteOPTemplateTableDesignsLists;
            List<QuoteOPTemplateTextCodePM> textcodes = QuoteOPTemplateBuildArges.QuoteOPTemplateTextCodePMLists;
            List<QuoteOPTemplateSectionPM> templateSections = QuoteOPTemplateBuildArges.QuoteOPTemplateSectionPMLists;
            string userId = QuoteOPTemplateBuildArges.UserId;
            int tenant = QuoteOPTemplateBuildArges.Tenant;
            IQuotesContext context = QuotesContext.GetContext(tenant);
            QuoteQuery quoteQuery = new QuoteQuery(new QuoteRepository(context));

            string htmlString = "";
            if (setting.ShowIncludedChargesPackages)
            {
                List<QuoteOPSaleChargePM> emptySaleCharges = quoteQuery.AddEmptySaleCharge(QuoteOPPM.QuoteCharges, QuoteOPPM.TransportModeId, QuoteOPPM.ShipmentTypeId);
                QuoteOPPM.QuoteSaleCharges.AddRange(emptySaleCharges);
            }

            if (requestArea == "Quote")
            {
                if (QuoteOPPM.TransportModeId == "A" || QuoteOPPM.ShipmentTypeId == "LCL" || QuoteOPPM.ShipmentTypeId == "LCLD" || QuoteOPPM.ShipmentTypeId == "LTL")
                {
                    templateSections = templateSections.Where(t => t.QuoteOPTemplateSectionTypeCode != "PC").ToList();

                }
                else if (QuoteOPPM.ShipmentTypeId == "FCL" || QuoteOPPM.ShipmentTypeId == "FCLD" || QuoteOPPM.ShipmentTypeId == "FTL")
                {
                    templateSections = templateSections.Where(t => t.QuoteOPTemplateSectionTypeCode != "PP").ToList(); ;
                }
            }


            if (includeHeaderFooter)
            {
                QuoteOPTemplateSectionPM headerSection = templateSections.Where(s => s.QuoteOPTemplateSectionTypeCode == "PH" && !s.IsExcluded).FirstOrDefault();
                SetSectionTypeCode(QuoteOPTemplateBuildArges,"PH");
                byte[] headerdata = GetQuoteOPTemplatePageHeaderFooter(QuoteOPTemplateBuildArges);
                if (headerdata != null)
                {
                    htmlString += GetBodyString(headerdata);

                }
            }

            QuoteOPTemplateBuildArges.FirstSectionInBody =true;
            foreach (QuoteOPTemplateSectionPM section in templateSections.Where(s => s.QuoteOPTemplateSectionTypeCode != "PF" && s.QuoteOPTemplateSectionTypeCode != "PH" && !s.IsExcluded).OrderBy(x => x.Order).ToList())
            {
             
                if (section.QuoteOPTemplateSectionTypeCode == "PP" || section.QuoteOPTemplateSectionTypeCode == "PC")
                {
                    SetSectionTypeCode(QuoteOPTemplateBuildArges, section.QuoteOPTemplateSectionTypeCode);
                    byte[] pricingdata = GetQuoteOPTemplatePricingHtmlData(QuoteOPTemplateBuildArges);

                    if (pricingdata != null && pricingdata.Length>0)
                    {
                        htmlString += GetBodyString(pricingdata);
                    }
                    
                }
                else if (section.QuoteOPTemplateSectionTypeCode == "QD" || section.QuoteOPTemplateSectionTypeCode == "QH")
                {
                    SetSectionTypeCode(QuoteOPTemplateBuildArges, section.QuoteOPTemplateSectionTypeCode);

                    if (section.QuoteOPTemplateSectionTypeCode == "QH")
                    {
                        if (!QuoteOPTemplateBuildArges.HideQuoteHeaderFromPdf)
                        {
                            byte[] quoteHeaderdata = GetQuoteOPTemplateHeader(QuoteOPTemplateBuildArges);
                            if (quoteHeaderdata != null)
                            {
                                htmlString += GetBodyString(quoteHeaderdata);
                            }
                        }

                    }
                    else if (section.QuoteOPTemplateSectionTypeCode == "QD")
                    {
                        byte[] quoteDetailrdata = GetQuoteOPTemplateDetails(QuoteOPTemplateBuildArges);
                        if (quoteDetailrdata != null)
                        {
                            htmlString += GetBodyString(quoteDetailrdata);

                        }


                    }

                }
                else if (section.QuoteOPTemplateSectionTypeCode == "PB") htmlString += "<p style='page-break-after:always;'> <span style=visibility:collapse>Page Break</span></p>";
                else
                {
                    byte[] sectiondata = DownloadQuoteOPTemplateSectionDataFile(section.SectionDocId, tenant);
                    if (sectiondata != null)
                    {
                        htmlString += GetBodyString(sectiondata);
                    }
                }

                if (QuoteOPTemplateBuildArges.FirstSectionInBody == true && (htmlString.Contains("FirstSectionInBody") || section.QuoteOPTemplateSectionTypeCode == "S" || section.QuoteOPTemplateSectionTypeCode == "PB")) QuoteOPTemplateBuildArges.FirstSectionInBody = false;

            }


            if (includeHeaderFooter)
            {
                SetSectionTypeCode(QuoteOPTemplateBuildArges, "PF");
                QuoteOPTemplateSectionPM footerSection = templateSections.Where(s => s.QuoteOPTemplateSectionTypeCode == "PF" && !s.IsExcluded).FirstOrDefault();
                byte[] footerdata = GetQuoteOPTemplatePageHeaderFooter(QuoteOPTemplateBuildArges);
                htmlString +=  GetBodyString(footerdata);
            }

            return Encoding.UTF8.GetBytes(htmlString);
        }

        public byte[] GetQuoteOPTemplatePricingHtmlData(QuoteOPTemplateBuildArges QuoteOPTemplateBuildArges )
        {


            QuoteOPPM QuoteOPPM = QuoteOPTemplateBuildArges.QuoteOPPM;
            QuoteOPTemplatePM template = QuoteOPTemplateBuildArges.QuoteOPTemplatePM;
            QuoteOPTemplateSettingPM setting = QuoteOPTemplateBuildArges.QuoteOPTemplateSettingPM;
            List<QuoteOPTemplateTextDesignPM> QuoteOPTemplateTextDesignsList = QuoteOPTemplateBuildArges.QuoteOPTemplateTextDesignPMLists;
            List<QuoteOPTemplateTableDesignPM> QuoteOPTemplateTableDesignsList = QuoteOPTemplateBuildArges.QuoteOPTemplateTableDesignsLists;
            List<QuoteOPTemplateTextCodePM> textcodes = QuoteOPTemplateBuildArges.QuoteOPTemplateTextCodePMLists;
            int tenant = QuoteOPTemplateBuildArges.Tenant;
            int? userTenant = QuoteOPTemplateBuildArges.UserTenant;
            string pricingSectionType = QuoteOPTemplateBuildArges.SectionTypeCode;
            string email = HttpContext.Current.User.Identity.Name;
            int tenantNumber = userTenant != null ? (int)userTenant : tenant;
            LocalCurrencyCode = GetLocalCurrencyCode(tenantNumber, email);

            if (QuoteOPPM == null) QuoteOPPM = BuildingQuoteOPPM();

            if (textcodes == null || textcodes.Count == 0)
            {
                QuoteOPTemplateTextCodeQuery QuoteOPTemplateTextCodeQuery = new QuoteOPTemplateTextCodeQuery(tenant);
                textcodes = QuoteOPTemplateTextCodeQuery.GetQuoteOPTemplateTextCodePMsByQuoteOPTemplateId(template.Tenant, template.Id).ToList();
                QuoteOPTemplateBuildArges.QuoteOPTemplateTextCodePMLists = textcodes;
            }


            ChargesGroupQuery chargesGroupQuery = new ChargesGroupQuery(tenant);
            List<ChargesGroupList> chargesGroupLists = chargesGroupQuery.GetChargesGroupListsByTenant(tenant).ToList();

            StringBuilder HtmlTemplate = new StringBuilder();



            IsShowlanguage = setting.ShowLocalLanguage;
            var isSplitChargesbyGroups = (pricingSectionType == "PP" && setting.SplitChargesbyGroupsPackages) ? true : (pricingSectionType == "PC" && setting.SplitChargesbyGroupsContainers) ? true : false;
            var isRoutingRates = template != null ? template.TemplateTypeCode == "P" ? true : false : false;
            bool isShowPerContainers = pricingSectionType == "PC" && QuoteOPPM.TotalPerContainer && !string.IsNullOrEmpty(setting.TotalPerContainersTableDesignId) ? true : false;
            if (setting != null && isRoutingRates) setting.ShowUnitsPackages = false;


            List<QuoteOPSaleChargePM> QuoteSaleChargePricingTableLists = QuoteOPPM.QuoteSaleCharges;
            List<QuoteOPSaleChargePM> QuoteSaleChargePerContainersLists = QuoteOPPM.QuoteSaleCharges;

            if (IsShowIncludedChargesPricingTable(pricingSectionType, setting)) QuoteSaleChargePricingTableLists = QuoteSaleChargePricingTableLists.Concat(QuoteOPPM.QuotationSaleCharges).ToList();
            if (setting.ShowIncludedChargesPerContainers) QuoteSaleChargePerContainersLists = QuoteSaleChargePerContainersLists.Concat(QuoteOPPM.QuotationSaleCharges).ToList();
            if (setting.ShowFixedPriceContainers) ViewFixedPrice = IsShowFixedPriceContainer(QuoteSaleChargePricingTableLists);
            SetQuoteOPTemplateSettingShowSaleMaxMinAmount(setting, pricingSectionType, QuoteSaleChargePricingTableLists);
            GetCountHeader(setting, QuoteOPPM, pricingSectionType);


    #region Set Quote Sale Charge Related Order Fields
            //Set Quote Sale Charge Related Order Fields
            if (isSplitChargesbyGroups || isShowPerContainers)
            {
                ChargesTypeQuery chargesTypeQuery = new ChargesTypeQuery(tenant);
                List<ChargesTypeList> chargesTypeLists = chargesTypeQuery.GetChargesTypeListsByTenant(tenant).ToList();
                if (isSplitChargesbyGroups) SetQuoteSaleChargeRelatedOrderFields(QuoteSaleChargePricingTableLists, chargesGroupLists, chargesTypeLists);
                if (isShowPerContainers) SetQuoteSaleChargeRelatedOrderFields(QuoteSaleChargePerContainersLists, chargesGroupLists, chargesTypeLists);
            }
    #endregion

    #region PricingTable


            if (TdCount != 0)
            {
                // Space Line

                string tableDesignId = pricingSectionType == "PP" ? setting.PackagesTableDesignId : setting.ContainserTableDesignId;
                string titleDesignId = pricingSectionType == "PP" ? setting.PricingPackagesTitleDesignId : setting.PricingContainsersTitleDesignId;
               
                QuoteOPTemplateTableDesignPM QuoteOPTemplateTableDesignPM = QuoteOPTemplateTableDesignsList.Where(t => t.Id == tableDesignId).FirstOrDefault(); 
                QuoteOPTemplateTextDesignPM QuoteOPTemplateTextDesignPMPricingTitle = QuoteOPTemplateTextDesignsList.Where(t => t.Id == titleDesignId).FirstOrDefault();
                QuoteOPTemplateTextDesignPM QuoteOPTemplateTextDesignPMHeader = QuoteOPTemplateTextDesignsList.Where(t => t.Id == QuoteOPTemplateTableDesignPM.HeaderDesignId).FirstOrDefault();
                QuoteOPTemplateTextDesignPM QuoteOPTemplateTextDesignLines = QuoteOPTemplateTextDesignsList.Where(t => t.Id == QuoteOPTemplateTableDesignPM.LinesDesignId).FirstOrDefault();
                isRightToLeft = setting.RightToLeft;
                int line = pricingSectionType == "PP" ? setting.SpaceLinesBeforePackages : setting.SpaceLinesBeforeContainers;
                HtmlTemplate.Append(GetHtmlStringLine(line, QuoteOPTemplateBuildArges.FirstSectionInBody));


                if ((pricingSectionType == "PP" && setting.ShowTitlePricingPackages) || (pricingSectionType == "PC" && setting.ShowTitlePricingContainsers))
                {
                    BuildPricingTitle(HtmlTemplate, QuoteOPTemplateTextDesignPMPricingTitle, pricingSectionType, textcodes, setting.RightToLeft);

                    HtmlTemplate.Append("<div  style='height:8px;'>" + " &nbsp;  &nbsp; &nbsp; &nbsp; &nbsp;  &nbsp;   &nbsp;" + "</div>");
                }

                string StyleTable = GetStyleTable(QuoteOPTemplateTableDesignPM);
                string dir = "";
                if (setting.RightToLeft) dir = "dir='RTL'";

                if (QuoteOPTemplateTableDesignPM.BorderTypeCode == "NONE")
                {
                    HtmlTemplate.Append("<div  align='center'>");
                    HtmlTemplate.Append("<table style='border-collapse: collapse;'  width='100%' " + dir + " >");
                }
                else
                {
                    HtmlTemplate.Append("<div>");

                    HtmlTemplate.Append("<table " + dir + " width='100%' " + StyleTable + " >");
                }


                IEnumerable<IGrouping<string, QuoteOPSaleChargePM>> chargegroups = QuoteSaleChargePricingTableLists.GroupBy(q => q.ChargesGroupCode).OrderBy(d => d.ToList().FirstOrDefault().ChargesGroupViewOrder).ThenBy(n => n.ToList().FirstOrDefault().ChargesTypeViewOrder).ThenBy(d => d.ToList().FirstOrDefault().ChargesGroupName).ToList();

                bool showHeaderLabels = pricingSectionType == "PC" ? setting.ShowHeaderLabelsContainers : setting.ShowHeaderLabelsPackages;

                if (showHeaderLabels)
                {
                    buildHeadercolumn(HtmlTemplate, setting, QuoteOPTemplateTextDesignPMHeader, QuoteOPTemplateTableDesignPM, QuoteOPPM, pricingSectionType, setting.RightToLeft, textcodes);
                }

    #region  Table

                if ((pricingSectionType == "PP" && setting.SplitChargesbyGroupsPackages) || (pricingSectionType == "PC" && setting.SplitChargesbyGroupsContainers))
                {

                    QuoteOPTemplateTextDesignPM QuoteOPTemplateTextDesignPMGroupByHeader = pricingSectionType == "PP" ? QuoteOPTemplateTextDesignsList.Where(t => t.Id == setting.GroupByPackagesValueDesignId).FirstOrDefault() : QuoteOPTemplateTextDesignsList.Where(t => t.Id == setting.GroupByContainsersValueDesignId).FirstOrDefault();
                    string groupByHeaderStyle = GetSpanRowStyle(QuoteOPTemplateTextDesignPMGroupByHeader, "Total");

                    string StyleTrHeaderGroupBy = GetStyleGroupByTd("", QuoteOPTemplateTableDesignPM.BorderColor, QuoteOPTemplateTableDesignPM.BorderThickness, QuoteOPTemplateTableDesignPM.BorderTypeCode, QuoteOPTemplateTextDesignPMGroupByHeader, setting.RightToLeft);

                    foreach (IGrouping<string, QuoteOPSaleChargePM> chargegroup in chargegroups)
                    {
                        ChargesGroupList group = chargesGroupLists.Where(d => d.Code == chargegroup.Key).FirstOrDefault();

                        string chargeGroupName = setting.ShowLocalLanguage ? group.LocalName : group.Name;
                        string textValue = QuoteOPTemplateTextDesignPMGroupByHeader.Italic ? "</i>" + "<span " + groupByHeaderStyle + " > " + chargeGroupName + "&nbsp" + "</span>" + "</i>" : "<span " + groupByHeaderStyle + "> " + chargeGroupName + "&nbsp" + "</span>";

                        HtmlTemplate.Append("<tr  style= 'height:auto; width:auto;vertical-align:central'>" + "<td " + StyleTrHeaderGroupBy + "colspan=' " + TdCount.ToString() + "';" + ">" + textValue + " </td>" + "</tr>");

                        List<QuoteOPSaleChargePM> charges = chargegroup.ToList();
                        BuildTableRows(charges, HtmlTemplate, QuoteOPTemplateTextDesignLines, QuoteOPTemplateTableDesignPM, QuoteOPTemplateBuildArges);
                        bool showTotalPerChargeGroup = pricingSectionType == "PP" ? setting.ShowTotalPerChargeGroupPackages : setting.ShowTotalPerChargeGroupContainers;
                        if (showTotalPerChargeGroup)
                        {
                            BuildTotalPerChargeGroup(QuoteOPTemplateBuildArges, HtmlTemplate, charges);

                        }
                    }

                }
                else
                {
                    BuildTableRows(QuoteSaleChargePricingTableLists, HtmlTemplate, QuoteOPTemplateTextDesignLines, QuoteOPTemplateTableDesignPM, QuoteOPTemplateBuildArges);
                    HtmlTemplate.Append("</tr>");
                }
                
                HtmlTemplate.Append("</table>");

    #endregion

                if (!isShowPerContainers && !isRoutingRates)
                {
                    BuilQuoteTotalCurrency(QuoteOPTemplateBuildArges, HtmlTemplate);
                }

            }
            else
            {
                HtmlTemplate.Append("");

            }

    #endregion

    #region TotalPerContainers Table
            if (isShowPerContainers)
            {
                BuildTotalPerContainers(QuoteOPTemplateBuildArges,  chargesGroupLists, HtmlTemplate, QuoteSaleChargePerContainersLists);
            }

    #endregion


            return Encoding.UTF8.GetBytes(HtmlTemplate.ToString());
        }

        private void BuildTotalPerContainers(QuoteOPTemplateBuildArges QuoteOPTemplateBuildArges, List<ChargesGroupList> chargesGroupLists, StringBuilder HtmlTemplate, List<QuoteOPSaleChargePM> QuoteSaleChargePerContainersLists)
        {
            int tenant = QuoteOPTemplateBuildArges.Tenant;
            bool exist = SecurityUtility.CheckFeature("Quote", "TOTALPERCONTAINER", tenant);
            if (exist)
            {
                QuoteOPPM QuoteOPPM = QuoteOPTemplateBuildArges.QuoteOPPM;
                QuoteOPTemplateSettingPM setting = QuoteOPTemplateBuildArges.QuoteOPTemplateSettingPM;
                List<QuoteOPTemplateTextDesignPM> QuoteOPTemplateTextDesignsList = QuoteOPTemplateBuildArges.QuoteOPTemplateTextDesignPMLists;
                List<QuoteOPTemplateTextCodePM> textcodes = QuoteOPTemplateBuildArges.QuoteOPTemplateTextCodePMLists;
                List<QuoteOPTemplateTableDesignPM> QuoteOPTemplateTableDesignsList = QuoteOPTemplateBuildArges.QuoteOPTemplateTableDesignsLists;
                string pricingSectionType = QuoteOPTemplateBuildArges.SectionTypeCode;


                bool showIncludedChargesPerContainers = setting.ShowIncludedChargesPerContainers;
                List<OPTotalPerContainerClass> OPTotalPerContainerClassLists = new List<OPTotalPerContainerClass>();
                BuildOPTotalPerContainerClassLists(QuoteOPPM, QuoteSaleChargePerContainersLists, HtmlTemplate, setting, OPTotalPerContainerClassLists);


                IEnumerable<IGrouping<string, OPTotalPerContainerClass>> totalPerContainerGroupingListsByGroupCode = OPTotalPerContainerClassLists.GroupBy(q => q.ChargeGroupCode).OrderBy(d => d.ToList().FirstOrDefault().ChargesGroupViewOrder).ThenBy(n => n.ToList().FirstOrDefault().ChargesTypeViewOrder).ThenBy(d => d.ToList().FirstOrDefault().ChargesGroupName).ToList();

                IEnumerable<IGrouping<string, OPTotalPerContainerClass>> totalPerContainerGroupingListsByFieldCode = OPTotalPerContainerClassLists.GroupBy(q => q.FieldCode);
                QuoteOPTemplateTextDesignPM totalPerContainersAdditionalTextDesign = QuoteOPTemplateTextDesignsList.Where(t => t.Id == setting.TotalPerContainersAdditionalTextDesignId).FirstOrDefault();
                QuoteOPTemplateTableDesignPM totalPerContainersTableDesign = QuoteOPTemplateTableDesignsList.Where(t => t.Id == setting.TotalPerContainersTableDesignId).FirstOrDefault();
                QuoteOPTemplateTextDesignPM totalPerContainersTableHeader = QuoteOPTemplateTextDesignsList.Where(t => t.Id == totalPerContainersTableDesign.HeaderDesignId).FirstOrDefault();
                QuoteOPTemplateTextDesignPM totalPerContainersTableLines = QuoteOPTemplateTextDesignsList.Where(t => t.Id == totalPerContainersTableDesign.LinesDesignId).FirstOrDefault();

                if (totalPerContainerGroupingListsByGroupCode.Count() > 0)
                {
                    string translateInclueLable = GetTranslateInclueLable(QuoteOPTemplateBuildArges.QuoteOPTemplateTextCodePMLists, "TotalPerContainers");


                    if (setting.ShowPageBreakBeforeTotalPerContainersTable)
                    {
                        HtmlTemplate.Append("<p style='page-break-after:always;'> <span style=visibility:collapse>Page Break</span></p>");
                    }

                    HtmlTemplate.Append(GetHtmlStringLine(setting.SpaceLinesBeforePerContainers, QuoteOPTemplateBuildArges.FirstSectionInBody));

                    //HtmlTemplate.Append("<div  style='height:10px;'>" + " &nbsp;  &nbsp; &nbsp; &nbsp; &nbsp;  &nbsp;   &nbsp;" + "</div>");

                    if (setting.ShowTitleTotalPerContainersTable)
                    {
                        BuildPricingTitle(HtmlTemplate, totalPerContainersAdditionalTextDesign, "TotalPerContainers", textcodes, setting.RightToLeft);
                        HtmlTemplate.Append("<div  style='height:5px;'>" + " &nbsp;  &nbsp; &nbsp; &nbsp; &nbsp;  &nbsp;   &nbsp;" + "</div>");

                    }

                    string styleTotalPerContainerTable = GetStyleTable(totalPerContainersTableDesign);


                    string dir = "";
                    if (setting.RightToLeft) dir = "dir='RTL'";


                    if (totalPerContainersTableDesign.BorderTypeCode == "NONE")
                    {
                        HtmlTemplate.Append("<div " + dir + ">");

                        HtmlTemplate.Append("<table " + dir + "style='border-collapse: collapse;'  width='100%' " + dir + " >");
                    }
                    else
                    {
                        HtmlTemplate.Append("<div " + dir + ">");

                        HtmlTemplate.Append("<table " + dir + " width='width' " + styleTotalPerContainerTable + " >");
                    }
                    HtmlTemplate.Append("<tr style= 'height:auto; width:auto;vertical-align:central'>");

                    string fieldName = GetNameColum("CHARGEGROUP", textcodes, "TotalPerContainers");

                    HtmlTemplate.Append(BuildTableColumn(fieldName, totalPerContainersTableHeader, totalPerContainersTableDesign, "Header", null, setting.RightToLeft));
                    foreach (IGrouping<string, OPTotalPerContainerClass> totalPerContainer in totalPerContainerGroupingListsByFieldCode)
                    {
                        OPTotalPerContainerClass OPTotalPerContainerClass = totalPerContainer.FirstOrDefault();
                        if (OPTotalPerContainerClass != null)
                        {
                            HtmlTemplate.Append(BuildTableColumn(OPTotalPerContainerClass.Name, totalPerContainersTableHeader, totalPerContainersTableDesign, "Header", null, setting.RightToLeft, true));
                        }

                    }
                    HtmlTemplate.Append("</tr>");

                    List<OPTotalPerContainerClass> totals = new List<OPTotalPerContainerClass>();
                    foreach (IGrouping<string, OPTotalPerContainerClass> totalPerContainer in totalPerContainerGroupingListsByGroupCode)
                    {
                        bool includedChargesPerContainers = !(totalPerContainer.Where(d => !d.Included).Any());
                        HtmlTemplate.Append("<tr style= 'height:auto; width:auto;vertical-align:central'>");
                        IEnumerable<IGrouping<string, OPTotalPerContainerClass>> totalPerContainerGroupByFieldCode = totalPerContainer.GroupBy(q => q.FieldCode);

                        ChargesGroupList group = chargesGroupLists.Where(d => d.Code == totalPerContainer.Key).FirstOrDefault();
                        string chargeGroupName = setting.ShowLocalLanguage ? group.LocalName : group.Name;
                        HtmlTemplate.Append(BuildTableColumn(chargeGroupName, totalPerContainersTableLines, totalPerContainersTableDesign, "Field", null, setting.RightToLeft));
                        foreach (IGrouping<string, OPTotalPerContainerClass> totalPerContainerGroup in totalPerContainerGroupByFieldCode)
                        {
                            if (!includedChargesPerContainers)
                            {
                                List<OPTotalPerContainerClass> totalPerContainersLists = totalPerContainerGroup.ToList();
                                double value = 0;
                                foreach (OPTotalPerContainerClass item in totalPerContainersLists)
                                {
                                    value += item.Value;

                                }

                                if (setting.TotalPerContainersCurrencyType == "LOCAL")
                                {
                                    value = value * (double)QuoteOPPM.ExchangeRate;
                                }
                                totals.Add(new OPTotalPerContainerClass() { Value = value, FieldCode = totalPerContainersLists[0].FieldCode });
                                HtmlTemplate.Append(BuildTableColumn(value.ToString("N"), totalPerContainersTableLines, totalPerContainersTableDesign, "Field", null, true));
                            }
                            else
                            {
                                HtmlTemplate.Append(BuildTableColumn(translateInclueLable, totalPerContainersTableLines, totalPerContainersTableDesign, "Field", null, true));
                            }
                        }
                        HtmlTemplate.Append("</tr>");
                    }

                    if (setting.TotalPerContainersCurrencyType != "MULTIPLE")
                    {
                        AppendPerContainerTotalBySaleCurrency(QuoteOPPM, setting, HtmlTemplate, totalPerContainersTableDesign, totalPerContainersTableLines, totals);
                    }
                    if (setting.TotalPerContainersCurrencyType == "MULTIPLE")
                    {
                        HtmlTemplate.Append("<tr style= 'height:auto; width:auto;vertical-align:central'>");
                        HtmlTemplate.Append(BuildTableColumn("Total", totalPerContainersTableLines, totalPerContainersTableDesign, "Field", null, setting.RightToLeft));
                        foreach (IGrouping<string, OPTotalPerContainerClass> totalPerContainer in totalPerContainerGroupingListsByFieldCode)
                        {

                            List<OPTotalPerContainerClass> items = new List<OPTotalPerContainerClass>();
                            IEnumerable<IGrouping<string, OPTotalPerContainerClass>> totalPerContainersGroupByCurrencyCodeLists = totalPerContainer.ToList().GroupBy(d => d.CurrencyCode).ToList();
                            foreach (IGrouping<string, OPTotalPerContainerClass> totalPerContainersGroupByCurrencyCodeList in totalPerContainersGroupByCurrencyCodeLists)
                            {

                                var totalPerContainersGroupByCurrencyCode = totalPerContainersGroupByCurrencyCodeList.ToList();
                                var OPTotalPerContainerClass = new OPTotalPerContainerClass() { CurrencyCode = totalPerContainersGroupByCurrencyCode[0].CurrencyCode };
                                double orginalValue = 0;
                                foreach (OPTotalPerContainerClass item in totalPerContainersGroupByCurrencyCode)
                                {
                                    orginalValue += ((double)item.OrginalValue);
                                }
                                OPTotalPerContainerClass.OrginalValue = orginalValue;
                                items.Add(OPTotalPerContainerClass);
                            }


                            string value = string.Empty;
                            foreach (OPTotalPerContainerClass item in items.OrderBy(d => d.Name).ToList())
                            {
                                var x = (double)item.OrginalValue;
                                value += ((x.ToString("N") + " " + item.CurrencyCode) + "<br/>");
                            }
                            HtmlTemplate.Append(BuildTableColumn(value, totalPerContainersTableLines, totalPerContainersTableDesign, "Field", null, true));
                        }
                        HtmlTemplate.Append("</tr>");
                    }

                    HtmlTemplate.Append("</table>");


                }
            }
        }

        private void BuilQuoteTotalCurrency(QuoteOPTemplateBuildArges QuoteOPTemplateBuildArges , StringBuilder HtmlTemplate)
        {
           
       
            QuoteOPPM QuoteOPPM = QuoteOPTemplateBuildArges.QuoteOPPM;
            QuoteOPTemplateSettingPM setting = QuoteOPTemplateBuildArges.QuoteOPTemplateSettingPM;
            List<QuoteOPTemplateTextDesignPM> QuoteOPTemplateTextDesignsList = QuoteOPTemplateBuildArges.QuoteOPTemplateTextDesignPMLists;
            List<QuoteOPTemplateTextCodePM> textcodes = QuoteOPTemplateBuildArges.QuoteOPTemplateTextCodePMLists;
            int tenant = QuoteOPTemplateBuildArges.Tenant;
            string pricingSectionType = QuoteOPTemplateBuildArges.SectionTypeCode;

            QuoteOPTemplateTextDesignPM QuoteOPTemplateTextDesignTotalsLabel = null;
            QuoteOPTemplateTextDesignPM QuoteOPTemplateTextDesignTotalsValue = null;

            if (pricingSectionType == "PC")
            {
                QuoteOPTemplateTextDesignTotalsLabel = QuoteOPTemplateTextDesignsList.Where(t => t.Id == setting.TotalsContainsersLabelDesignId).FirstOrDefault();//QuoteOPTemplateTextDesignQuery.GetSinglePM(setting.TotalsContainsersLabelDesignId, tenant);
                QuoteOPTemplateTextDesignTotalsValue = QuoteOPTemplateTextDesignsList.Where(t => t.Id == setting.TotalsContainsersValueDesignId).FirstOrDefault();//QuoteOPTemplateTextDesignQuery.GetSinglePM(setting.TotalsContainsersValueDesignId, tenant);
            }
            else
            {
                QuoteOPTemplateTextDesignTotalsLabel = QuoteOPTemplateTextDesignsList.Where(t => t.Id == setting.TotalsPackagesLabelDesignId).FirstOrDefault();//QuoteOPTemplateTextDesignQuery.GetSinglePM(setting.TotalsPackagesLabelDesignId, tenant);
                QuoteOPTemplateTextDesignTotalsValue = QuoteOPTemplateTextDesignsList.Where(t => t.Id == setting.TotalsPackagesValueDesignId).FirstOrDefault();//QuoteOPTemplateTextDesignQuery.GetSinglePM(setting.TotalsPackagesValueDesignId, tenant);
            }
            string Name = GetNameColum("TOTALAMOUNTS", textcodes, pricingSectionType);
            if (QuoteOPPM.SaleCurrencyCode == null && QuoteOPPM.SaleCurrencyId != null)
            {
                SetSaleCurrencyCode(QuoteOPPM, tenant);
            }

            string SaleTotalAmountInSaleCurrency =  GetNumberValueFormate(!QuoteOPPM.IsChargesByVAT ?QuoteOPPM.SaleTotalAmountInSaleCurrency: QuoteOPPM.TotalSaleIncludingVATAmountInSaleCurrency);
            string SaleTotalAmountInLocalCurrency = GetNumberValueFormate(!QuoteOPPM.IsChargesByVAT ? QuoteOPPM.SaleTotalAmountInLocalCurrency: QuoteOPPM.TotalSaleIncludingVATAmountInLocalCurrency);
           
    #region Total Currency Containers
            bool ShowTotalInSaleCurrency = pricingSectionType == "PC" ? setting.ShowTotalInSaleCurrencyContainers : setting.ShowTotalInSaleCurrencyPackages;
            bool ShowTotalInLocalCurrency = pricingSectionType == "PC" ? setting.ShowTotalInLocalCurrencyContainers : setting.ShowTotalInLocalCurrencyPackages;


            if (ShowTotalInSaleCurrency || ShowTotalInLocalCurrency)
            {
                HtmlTemplate.Append("<div  style='height:5px;'>" + " &nbsp;  &nbsp; &nbsp; &nbsp; &nbsp;  &nbsp;   &nbsp;" + "</div>");
            }
            AppendTotalCurrencyHtml(QuoteOPPM, setting, HtmlTemplate, ShowTotalInSaleCurrency, ShowTotalInLocalCurrency, QuoteOPTemplateTextDesignTotalsLabel, QuoteOPTemplateTextDesignTotalsValue,
              Name, SaleTotalAmountInSaleCurrency, SaleTotalAmountInLocalCurrency);
    #endregion


            HtmlTemplate.Append("</div>");

    
        }

        private void BuildTotalPerChargeGroup(QuoteOPTemplateBuildArges QuoteOPTemplateBuildArges, StringBuilder HtmlTemplate ,  List<QuoteOPSaleChargePM> charges)
        {


            string pricingSectionType = QuoteOPTemplateBuildArges.SectionTypeCode;
            IEnumerable<IGrouping<string, QuoteOPSaleChargePM>> QuoteSaleChargeGroup = charges.GroupBy(q => q.CurrencyCode);
            string currencyCode = "";
            double? amount = 0;

            QuoteOPTemplateTextDesignPM QuoteOPTemplateTextDesignPMGroupByTotal = pricingSectionType == "PP" ? QuoteOPTemplateBuildArges.QuoteOPTemplateTextDesignPMLists.Where(t => t.Id == QuoteOPTemplateBuildArges.QuoteOPTemplateSettingPM.GroupByPackagesLabelDesignId).FirstOrDefault() : QuoteOPTemplateBuildArges.QuoteOPTemplateTextDesignPMLists.Where(t => t.Id == QuoteOPTemplateBuildArges.QuoteOPTemplateSettingPM.GroupByContainsersLabelDesignId).FirstOrDefault();
            string groupByTotalStyle = GetSpanRowStyle(QuoteOPTemplateTextDesignPMGroupByTotal, "Lable");
       
            string tableDesignId = pricingSectionType == "PP" ? QuoteOPTemplateBuildArges.QuoteOPTemplateSettingPM.PackagesTableDesignId : QuoteOPTemplateBuildArges.QuoteOPTemplateSettingPM.ContainserTableDesignId;
            QuoteOPTemplateTableDesignPM QuoteOPTemplateTableDesignPM = QuoteOPTemplateBuildArges.QuoteOPTemplateTableDesignsLists.Where(d => d.Id == tableDesignId).FirstOrDefault() ;

            string StyleTrGroupByTotal = GetStyleGroupByTd("Empty", QuoteOPTemplateTableDesignPM.BorderColor, QuoteOPTemplateTableDesignPM.BorderThickness, QuoteOPTemplateTableDesignPM.BorderTypeCode, QuoteOPTemplateTextDesignPMGroupByTotal, QuoteOPTemplateBuildArges.QuoteOPTemplateSettingPM.RightToLeft);

            string translateInclueLable = GetTranslateInclueLable(QuoteOPTemplateBuildArges.QuoteOPTemplateTextCodePMLists, pricingSectionType);


            HtmlTemplate.Append("<tr  style= 'height:auto; width:auto;vertical-align:central'>");

            if (ShowSaleCurrencyColumnColumnPosition > 1)
            {
                HtmlTemplate.Append("<td " + StyleTrGroupByTotal + "colspan=' " + (ShowSaleCurrencyColumnColumnPosition - 1).ToString() + "';" + ">"); HtmlTemplate.Append("<div " + groupByTotalStyle + " >" + "<div>"); HtmlTemplate.Append("</td>");
    
            HtmlTemplate.Append("<td " + StyleTrGroupByTotal + ">");
        }

            foreach (IGrouping<string, QuoteOPSaleChargePM> quoteSaleCharge in QuoteSaleChargeGroup)
            {
                currencyCode = "";
                amount = 0;
                List<QuoteOPSaleChargePM> quoteSaleCharges = quoteSaleCharge.ToList();

                bool allChargeIncluded = !(quoteSaleCharges.Where(d => d.IsAllIN == null || (d.IsAllIN != null && d.IsAllIN.ToLower() == "false")).Any());
                foreach (QuoteOPSaleChargePM QuoteOPSaleChargePM in quoteSaleCharges)
                {
                    if (QuoteOPSaleChargePM.IsAllIN == null || (QuoteOPSaleChargePM.IsAllIN != null && QuoteOPSaleChargePM.IsAllIN.ToLower() == "false"))
                    {
                        currencyCode = QuoteOPSaleChargePM.CurrencyCode;
                        amount += QuoteOPSaleChargePM.SaleTotalAmount;
                    }

                }

                string amountValue = " ";
                if (amount != null)
                {
                    double value = (double)amount;
                    amountValue = value.ToString("N"); // 1,234.512
                }
                string displayValue = amountValue.ToString() + " " + currencyCode;
                if (allChargeIncluded) displayValue = translateInclueLable;

                HtmlTemplate.Append("<div " + groupByTotalStyle + " >" + displayValue + "<div>");

            }

            HtmlTemplate.Append("</td>");
            if (TdCount != ShowSaleCurrencyColumnColumnPosition)
            {
                HtmlTemplate.Append("<td " + StyleTrGroupByTotal + "colspan=' " + (TdCount - ShowSaleCurrencyColumnColumnPosition).ToString() + "';" + ">"); HtmlTemplate.Append("<div " + groupByTotalStyle + " >" + "<div>"); HtmlTemplate.Append("</td>");
            }
            HtmlTemplate.Append("</tr>");
        }

        private string GetTranslateInclueLable(List<QuoteOPTemplateTextCodePM> QuoteOPTemplateTextCodePMLists, string pricingSectionType)
        {
           return  GetNameColum("INCLUDED", QuoteOPTemplateTextCodePMLists, pricingSectionType);
        }

        private  void SetSaleCurrencyCode(QuoteOPPM QuoteOPPM, int tenant)
        {
            CurrencyRepository currencyRepository = new CurrencyRepository(tenant);
            var currency = currencyRepository.GetSingleCurrency(QuoteOPPM.SaleCurrencyId, tenant);
            if (currency != null)
            {
                QuoteOPPM.SaleCurrencyCode = currency.Code;
            }
        }

        private string GetLocalCurrencyCode(int tenantNumber , string email)
        {
            string localCurrencyCode = string.Empty;


            ContactRepository contactrep = new ContactRepository(tenantNumber);
            Contact contact = contactrep.GetSingleContactByEmail(email, tenantNumber);
            CurrencyRepository currencyRepository = new CurrencyRepository(tenantNumber);

            if (contact != null)
            {
                SystemDataQuery systemDataQuery = new SystemDataQuery(tenantNumber);
                SystemDataPM systemEntity = systemDataQuery.GetSinglePM(contact.Id, tenantNumber);

                if (systemEntity != null)
                {

                    if (!string.IsNullOrEmpty(systemEntity.LocalCurrencyId))
                    {
                        Currency currency = currencyRepository.GetSingleCurrency(systemEntity.LocalCurrencyId, tenantNumber);
                        if (currency != null) localCurrencyCode = currency.Code;
                    }
                }
            }

            return localCurrencyCode;
        }

        private static void SetQuoteOPTemplateSettingShowSaleMaxMinAmount(QuoteOPTemplateSettingPM setting, string pricingSectionType, List<QuoteOPSaleChargePM> QuoteSaleChargePricingTableLists)
        {
            bool isHaveMaxMinValue = QuoteSaleChargePricingTableLists.Where(d => d.SaleMaxAmount != null || d.SaleMinAmount != null).Any();
            if (!isHaveMaxMinValue)
            {
                if (pricingSectionType == "PP") setting.ShowSaleMaxMinAmountPackages = false;
                else if (pricingSectionType == "PC") setting.ShowSaleMaxMinAmountContainers = false;
            }
        }

        private static void SetQuoteSaleChargeRelatedOrderFields( List<QuoteOPSaleChargePM> QuoteSaleChargePerContainersLists, List<ChargesGroupList> chargesGroupLists, List<ChargesTypeList> chargesTypeLists)
        {

            foreach (QuoteOPSaleChargePM item in QuoteSaleChargePerContainersLists)
            {
                var group = chargesGroupLists.Where(d => d.Code == item.ChargesGroupCode).FirstOrDefault();
                if (group != null)
                {
                    item.ChargesGroupViewOrder = item.ChargesTypeViewOrder = group.ViewOrder;
                    item.ChargesGroupName = group.Name;
                    var chargesTypeList = chargesTypeLists.Where(d => d.Id == item.ChargesTypeId).FirstOrDefault();
                    if (chargesTypeList != null) item.ChargesTypeViewOrder = chargesTypeList.ViewOrder;
                }
            }
        }

        private List<OPChargeGroupOrderList> BuildOPChargeGroupOrderList(List<ChargesGroupList> chargesGroups , int tenant)
        {
            ChargesTypeQuery chargesTypeQuery = new ChargesTypeQuery(tenant);
            var chargesTypeLists = chargesTypeQuery.GetChargesTypeListsByTenant(tenant);

            List<OPChargeGroupOrderList> items = new List<OPChargeGroupOrderList>();
            foreach (ChargesGroupList item in chargesGroups)
            {
                var chargesTypeList = chargesTypeLists.Where(d => d.ChargesGroupId == item.Id).FirstOrDefault();
                items.Add(new OPChargeGroupOrderList() { GroupOrder = item.ViewOrder,Name = item.Name, ChargeTypeOrder= chargesTypeList!=null? chargesTypeList.ViewOrder: item.ViewOrder,Code = item.Code });
            }

            int i = 1;
            foreach (var s in items.OrderBy(c => c.GroupOrder).ThenBy(n => n.ChargeTypeOrder).ThenBy(d => d.Name).ToList())
            {
                s.Order = i;
                i += 1;
            }
           
            return items;
        }

        private bool IsShowIncludedChargesPricingTable(string pricingSectionType, QuoteOPTemplateSettingPM setting)
        {
            return pricingSectionType == "PP" ? setting.ShowIncludedChargesPackages: setting.ShowIncludedChargesContainers;

        }

        private void AppendPerContainerTotalBySaleCurrency(QuoteOPPM QuoteOPPM, QuoteOPTemplateSettingPM setting,  StringBuilder HtmlTemplate, QuoteOPTemplateTableDesignPM totalPerContainersTableDesign, QuoteOPTemplateTextDesignPM totalPerContainersTableLines, List<OPTotalPerContainerClass> totals)
        {
            HtmlTemplate.Append("<tr style= 'height:auto; width:auto;vertical-align:central'>");

            int tenant = setting.Tenant;
            string totalPerContainersCurrencyLable = setting.TotalPerContainersCurrencyType != "LOCAL" ? "Total By Sale Currency" : "Total By Local Currency";

            HtmlTemplate.Append(BuildTableColumn(totalPerContainersCurrencyLable, totalPerContainersTableLines, totalPerContainersTableDesign, "Field", null, setting.RightToLeft));

            TenantPM tenantPM = TenantQuery.GetSingleTenantPM(tenant, true);
            IEnumerable<IGrouping<string, OPTotalPerContainerClass>> totalPerList = totals.GroupBy(q => q.FieldCode);
            foreach (IGrouping<string, OPTotalPerContainerClass> totalPer in totalPerList)
            {
                List<OPTotalPerContainerClass> totalPerContainersLists = totalPer.ToList();
                double value = 0;
                foreach (OPTotalPerContainerClass item in totalPerContainersLists)
                {
                    value += item.Value;

                }

                string currencyCode = setting.TotalPerContainersCurrencyType == "LOCAL" ? tenantPM.CurrencyCode : QuoteOPPM.SaleCurrencyCode;

                HtmlTemplate.Append(BuildTableColumn(value.ToString("N") + " " + currencyCode, totalPerContainersTableLines, totalPerContainersTableDesign, "Field", null, setting.RightToLeft, true));
            }
            HtmlTemplate.Append("</tr>");
        }

        private void AppendTotalCurrencyHtml(QuoteOPPM QuoteOPPM, QuoteOPTemplateSettingPM setting, StringBuilder HtmlTemplate, bool showTotalInSaleCurrency, bool showTotalInLocalCurrency, QuoteOPTemplateTextDesignPM QuoteOPTemplateTextDesignTotalsLabel, QuoteOPTemplateTextDesignPM QuoteOPTemplateTextDesignTotalsValue, string Name, string SaleTotalAmountInSaleCurrency, string SaleTotalAmountInLocalCurrency)
        {

            string dir = "";
            string alignContent = QuoteOPTemplateTextDesignTotalsLabel.Alignment;
            if (setting.RightToLeft) dir = "dir='RTL'";
            string totalInSaleCurrency = BuildTotalInSale(Name + " : ", QuoteOPTemplateTextDesignTotalsLabel, false) + BuildTotalInSale(SaleTotalAmountInSaleCurrency + " " + QuoteOPPM.SaleCurrencyCode, QuoteOPTemplateTextDesignTotalsValue, false, true);
            string totalInLocalCurrency = BuildTotalInSale(Name + " : ", QuoteOPTemplateTextDesignTotalsLabel, true) + BuildTotalInSale(SaleTotalAmountInLocalCurrency + " " + LocalCurrencyCode, QuoteOPTemplateTextDesignTotalsValue, false, true);

            if (!QuoteOPPM.IsSaleCurrencySameAsCost)
            {
                if (showTotalInSaleCurrency && showTotalInLocalCurrency)
                {
                    HtmlTemplate.Append("<div " + dir + " style='display:block;text-align:"+ alignContent + ";'>" + totalInSaleCurrency + "</div>");
                    if (QuoteOPPM.SaleCurrencyCode != LocalCurrencyCode) HtmlTemplate.Append("<div " + dir + " style='display:block;text-align:" + alignContent + ";'>" + totalInLocalCurrency + "</div>");
                }
                else if (showTotalInSaleCurrency)
                {
                    totalInSaleCurrency = BuildTotalInSale(Name + " : ", QuoteOPTemplateTextDesignTotalsLabel, false) + BuildTotalInSale(SaleTotalAmountInSaleCurrency + " " + QuoteOPPM.SaleCurrencyCode, QuoteOPTemplateTextDesignTotalsValue, false, true);
                    HtmlTemplate.Append("<div " + dir + " style='display:block;text-align:" + alignContent + ";'>" + totalInSaleCurrency + "</div>");
                }
                else if (showTotalInLocalCurrency)
                {
                    totalInLocalCurrency = BuildTotalInSale(Name + " : ", QuoteOPTemplateTextDesignTotalsLabel, false) + BuildTotalInSale(SaleTotalAmountInLocalCurrency + " " + LocalCurrencyCode, QuoteOPTemplateTextDesignTotalsValue, false, true);
                    HtmlTemplate.Append("<div " + dir + " style='display:block;text-align:" + alignContent + ";'>" + totalInLocalCurrency + "</div>");
                }
            }

            else
            {
                string LocalCurrencyName = Name;
                if (showTotalInSaleCurrency)
                {
                    List<QuoteSalesTotalPM> QuoteSalesTotals = ComputeQuoteSalesTotals(QuoteOPPM);
                    bool isHideTitle = false;
                    foreach (QuoteSalesTotalPM item in QuoteSalesTotals.OrderByDescending(d => d.Amount))
                    {

                        string amountValue = " ";
                        if (item.Amount != null)
                        {
                            double value = (double)item.Amount;
                            amountValue = value.ToString("N"); // 1,234.512
                        }

                        string total = BuildTotalInSale(Name + " : ", QuoteOPTemplateTextDesignTotalsLabel, isHideTitle) + BuildTotalInSale(amountValue.ToString() + " " + item.CurrencyCode, QuoteOPTemplateTextDesignTotalsValue, false, true);
                        HtmlTemplate.Append("<div " + dir + " style='display:block;text-align:" + alignContent + ";'>" + total + " </div>");
                        isHideTitle = true;
                    }

                    LocalCurrencyName = "Estimated total (" + LocalCurrencyCode + ")";

                    if (showTotalInLocalCurrency)
                    {

                        totalInLocalCurrency = BuildTotalInSale(LocalCurrencyName, QuoteOPTemplateTextDesignTotalsLabel, false) + BuildTotalInSale(" : " + SaleTotalAmountInLocalCurrency, QuoteOPTemplateTextDesignTotalsValue, false);
                        HtmlTemplate.Append("<div " + dir + " style='display:block;text-align:" + alignContent + ";'>" + totalInLocalCurrency + "</div>");
                    }

                }
                else
                {

                    if (showTotalInLocalCurrency)
                    {
                        totalInLocalCurrency = BuildTotalInSale(LocalCurrencyName + " : ", QuoteOPTemplateTextDesignTotalsLabel, false) + BuildTotalInSale(SaleTotalAmountInLocalCurrency + " " + LocalCurrencyCode, QuoteOPTemplateTextDesignTotalsValue, false, true);
                        HtmlTemplate.Append("<div " + dir + " style='display:block;text-align:" + alignContent + ";'>" + totalInLocalCurrency + "</div>");

                    }




                }


            }
        }

        private List<QuoteSalesTotalPM> ComputeQuoteSalesTotals(QuoteOPPM QuoteOPPM)
        {
            IEnumerable<IGrouping<string, QuoteOPSaleChargePM>> quoteSaleChargesGroupLists = QuoteOPPM.QuoteSaleCharges.GroupBy(q => q.CurrencyCode);
            List<QuoteSalesTotalPM> QuoteSalesTotals = new List<QuoteSalesTotalPM>();
            foreach (IGrouping<string, QuoteOPSaleChargePM> quoteSaleChargesGrop in quoteSaleChargesGroupLists)
            {
                string currencyCode = "";
                double? amount = 0;
                List<QuoteOPSaleChargePM> quoteSaleCharges = quoteSaleChargesGrop.ToList();
                foreach (QuoteOPSaleChargePM QuoteOPSaleChargePM in quoteSaleCharges)
                {
                    currencyCode = QuoteOPSaleChargePM.CurrencyCode;

                    double vatAmount = QuoteOPSaleChargePM.VatAmount != null ? (double)QuoteOPSaleChargePM.VatAmount : 0;
                    amount += ((QuoteOPSaleChargePM.SaleTotalAmount != null ? QuoteOPSaleChargePM.SaleTotalAmount : 0) + (QuoteOPPM.IsChargesByVAT ? vatAmount : 0 ));

                }
                QuoteSalesTotals.Add(new QuoteSalesTotalPM() { CurrencyCode = currencyCode, Amount = amount });

            }

            return QuoteSalesTotals;
        }

    #region QuoteH eader and Details Table

        public byte[] GetQuoteOPTemplateHeader(QuoteOPTemplateBuildArges QuoteOPTemplateBuildArges)
        {


            QuoteOPPM QuoteOPPM = QuoteOPTemplateBuildArges.QuoteOPPM;
            QuoteOPTemplatePM template = QuoteOPTemplateBuildArges.QuoteOPTemplatePM;
            QuoteOPTemplateSettingPM setting = QuoteOPTemplateBuildArges.QuoteOPTemplateSettingPM;
            List<QuoteOPTemplateTextDesignPM> QuoteOPTemplateTextDesignsList = QuoteOPTemplateBuildArges.QuoteOPTemplateTextDesignPMLists;
            List<QuoteOPTemplateTableDesignPM> QuoteOPTemplateTableDesignsList = QuoteOPTemplateBuildArges.QuoteOPTemplateTableDesignsLists;
            List<QuoteOPTemplateTextCodePM> textcodes = QuoteOPTemplateBuildArges.QuoteOPTemplateTextCodePMLists;
            int tenant = QuoteOPTemplateBuildArges.Tenant;

            string sessiontype = QuoteOPTemplateBuildArges.SectionTypeCode == "PH" ? "Header" : "Footer";


            List<ObjectField> customFields = ObjectFieldRepository.GetCustomObjectFieldsByObjectTableName("Quote", tenant).ToList();

            if (QuoteOPPM == null)
            {
                QuoteOPPM = BuildingQuoteOPPM();
            }

            if (QuoteOPTemplateBuildArges.VersionNumber != null)
            {
                QuoteOPPM.QuoteVersion =  QuoteOPPM.QuoteNumber + "-" + (int)QuoteOPTemplateBuildArges.VersionNumber;   
            }


            Tenant = QuoteOPPM.Tenant;
            string FieldValue = "";
            string FieldName = "";
            string dir = "";
            isRightToLeft = setting.RightToLeft;
            if (setting.RightToLeft) dir = "dir='RTL'";

            QuoteOPTemplateHeaderFieldQuery QuoteOPTemplateHeaderFieldQuery = new QuoteOPTemplateHeaderFieldQuery(tenant);
            IQueryable<QuoteOPTemplateHeaderFieldPM> QuoteTemplaetHeaderFieldList = QuoteOPTemplateHeaderFieldQuery.GetQuoteOPTemplateHeaderFieldPMsByQuoteOPTemplateId(tenant, template.Id);
            StringBuilder HtmlTemplate = new StringBuilder();

            HtmlTemplate.Append("<!DOCTYPE html>");
            HtmlTemplate.Append("<html  lang='ar'>");
            HtmlTemplate.Append("<head>");
            HtmlTemplate.Append("<title></title>");
            HtmlTemplate.Append("<meta charset='utf-8'>");
            HtmlTemplate.Append("</head>");
            HtmlTemplate.Append("<body>"); 


            if (QuoteTemplaetHeaderFieldList.Count() > 0)
            {
                if (QuoteOPTemplateBuildArges.RequestArea != "Header")
                {
                    HtmlTemplate.Append(GetHtmlStringLine(setting.SpaceLinesBeforeQuoteHeaders, QuoteOPTemplateBuildArges.FirstSectionInBody));
                }


                List<QuoteOPTemplateHeaderFieldPM> QuoteTemplaetHeaderColum0;
                List<QuoteOPTemplateHeaderFieldPM> QuoteTemplaetHeaderColum1;

                QuoteTemplaetHeaderColum0 = QuoteTemplaetHeaderFieldList.Where(d => d.Column == 0).OrderBy(d => d.Row).ToList();
                QuoteTemplaetHeaderColum1 = QuoteTemplaetHeaderFieldList.Where(d => d.Column == 1).OrderBy(d => d.Row).ToList();

                IsShowlanguage = setting.ShowLocalLanguage;

                QuoteOPTemplateTableDesignPM QuoteOPTemplateTableDesignPM = QuoteOPTemplateTableDesignsList.Where(t => t.Id == setting.HeaderTableDesignId).FirstOrDefault();
                QuoteOPTemplateTextDesignPM QuoteOPTemplateTextDesignPMHeaderLable = QuoteOPTemplateTextDesignsList.Where(t => t.Id == QuoteOPTemplateTableDesignPM.HeaderDesignId).FirstOrDefault();
                QuoteOPTemplateTextDesignPM QuoteOPTemplateTextDesignPMHeaderValue = QuoteOPTemplateTextDesignsList.Where(t => t.Id == QuoteOPTemplateTableDesignPM.LinesDesignId).FirstOrDefault();

                string styleTable = GetStyleTable(QuoteOPTemplateTableDesignPM);

                string StyleQuoteHeaderLable = GetsyleSpanQuoteHeaderDetails(QuoteOPTemplateTextDesignPMHeaderLable);
                string styleTd = "";
                string widthtable = "";
                bool IsHeaderTableAuto = setting.HeaderTableColumWidthType == "AUTO" ? true : false;

                string StyleQuoteHeaderValue = GetsyleSpanQuoteHeaderDetails(QuoteOPTemplateTextDesignPMHeaderValue);

                string HeaderTableColumn1LabelWidth = IsHeaderTableAuto ? "Auto" : (setting.HeaderTableColumn1LabelWidth.ToString() + "%");
                string HeaderTableColumn1ValueWidth = IsHeaderTableAuto ? "Auto" : (setting.HeaderTableColumn1ValueWidth.ToString() + "%");
                string HeaderTableColumn2LabelWidth = IsHeaderTableAuto ? "Auto" : (setting.HeaderTableColumn2LabelWidth.ToString() + "%");
                string HeaderTableColumn2ValueWidth = IsHeaderTableAuto ? "Auto" : (setting.HeaderTableColumn2ValueWidth.ToString() + "%");

                double? W = setting.HeaderTableColumn1LabelWidth + setting.HeaderTableColumn1ValueWidth;
                if (setting.HeaderSectionHasTwoColumns) W += (setting.HeaderTableColumn2LabelWidth + setting.HeaderTableColumn2ValueWidth);
                widthtable = W.ToString() + "%";
                HtmlTemplate.Append("<div " + dir + " style ='width=100%' >");

                if (QuoteOPTemplateTableDesignPM.BorderTypeCode == "NONE") styleTable = "";
                if (IsHeaderTableAuto) HtmlTemplate.Append("<table   " + dir + " width='Auto' " + styleTable + " >");
                else HtmlTemplate.Append("<table " + dir + " width='" + widthtable + "'" + styleTable + " >");




                if (!setting.HeaderSectionHasTwoColumns)
                {
                    foreach (QuoteOPTemplateHeaderFieldPM Feild in QuoteTemplaetHeaderColum0)
                    {
                        HtmlTemplate.Append("<tr>");
                        FieldName = "";
                        FieldName = GetNameColum(Feild.FieldCode, textcodes, "QH");
                        if (string.IsNullOrEmpty(FieldName)) FieldName = TranslateTextsClass.Translate(Feild.FieldCode, Feild.Tenant);
                        if ((QuoteOPTemplateTableDesignPM.BorderTypeCode == "NONE"))
                        {
                            if (!string.IsNullOrEmpty(FieldName)) FieldName += " :";
                        }

                        FieldValue = GetQuoteOPTemplateHeaderFieldValue(Feild.FieldCode, QuoteOPPM);
                        if (FieldValue == "CustomField") FieldValue = GetCustomFelidValue(Feild.FieldCode, customFields, QuoteOPPM);

                        //Lable
                        styleTd = GetStyleRowTable(QuoteOPTemplateTextDesignPMHeaderLable, QuoteOPTemplateTableDesignPM, HeaderTableColumn1LabelWidth);
                        AppendNewQuoteTableoRow(FieldName, HtmlTemplate, QuoteOPTemplateTextDesignPMHeaderLable, StyleQuoteHeaderLable, styleTd);

                        //Value
                        styleTd = GetStyleRowTable(QuoteOPTemplateTextDesignPMHeaderValue, QuoteOPTemplateTableDesignPM, HeaderTableColumn1ValueWidth);
                        AppendNewQuoteTableoRow(FieldValue, HtmlTemplate, QuoteOPTemplateTextDesignPMHeaderValue, StyleQuoteHeaderValue, styleTd);

                        HtmlTemplate.Append("</tr>");
                    }
                }
                else
                {
                    if (QuoteTemplaetHeaderColum0.Count() == QuoteTemplaetHeaderColum1.Count() || QuoteTemplaetHeaderColum0.Count() > QuoteTemplaetHeaderColum1.Count())
                    {
                        int J = 0;
                        TdCount = 4;
                        int Count = QuoteTemplaetHeaderColum1.Count();
                        foreach (QuoteOPTemplateHeaderFieldPM Feild in QuoteTemplaetHeaderColum0)
                        {
                            HtmlTemplate.Append("<tr>");


                            FieldName = "";
                            FieldName = GetNameColum(Feild.FieldCode, textcodes, "QH");

                            if (string.IsNullOrEmpty(FieldName)) FieldName = TranslateTextsClass.Translate(Feild.FieldCode, Feild.Tenant);

                            if ((QuoteOPTemplateTableDesignPM.BorderTypeCode == "NONE"))
                            {
                                if (!string.IsNullOrEmpty(FieldName)) FieldName += " :";
                            }

                            FieldValue = GetQuoteOPTemplateHeaderFieldValue(Feild.FieldCode, QuoteOPPM);

                            if (FieldValue == "CustomField")
                            {
                                FieldValue = GetCustomFelidValue(Feild.FieldCode, customFields, QuoteOPPM);
                            }

                            //TextFeild
                            styleTd = GetStyleRowTable(QuoteOPTemplateTextDesignPMHeaderLable, QuoteOPTemplateTableDesignPM, HeaderTableColumn1LabelWidth);
                            AppendNewQuoteTableoRow(FieldName, HtmlTemplate, QuoteOPTemplateTextDesignPMHeaderLable, StyleQuoteHeaderLable, styleTd);


                            //Text Value
                            styleTd = GetStyleRowTable(QuoteOPTemplateTextDesignPMHeaderValue, QuoteOPTemplateTableDesignPM, HeaderTableColumn1ValueWidth);
                            AppendNewQuoteTableoRow(FieldValue, HtmlTemplate, QuoteOPTemplateTextDesignPMHeaderValue, StyleQuoteHeaderValue, styleTd);



                            if (Count > 0 && J <= Count - 1)
                            {
                                if (QuoteTemplaetHeaderColum1[J] != null)
                                {
                                    FieldName = "";
                                    FieldName = GetNameColum(QuoteTemplaetHeaderColum1[J].FieldCode, textcodes, "QH");

                                    if (string.IsNullOrEmpty(FieldName))
                                    {
                                        FieldName = TranslateTextsClass.Translate(QuoteTemplaetHeaderColum1[J].FieldCode, Feild.Tenant);
                                    }

                                    if ((QuoteOPTemplateTableDesignPM.BorderTypeCode == "NONE"))
                                    {
                                        if (!string.IsNullOrEmpty(FieldName)) FieldName += " :";
                                    }

                                    FieldValue = GetQuoteOPTemplateHeaderFieldValue(QuoteTemplaetHeaderColum1[J].FieldCode, QuoteOPPM);
                                    if (FieldValue == "CustomField")
                                    {
                                        FieldValue = GetCustomFelidValue(Feild.FieldCode, customFields, QuoteOPPM);
                                    }


                                    //TextFeild
                                    styleTd = GetStyleRowTable(QuoteOPTemplateTextDesignPMHeaderLable, QuoteOPTemplateTableDesignPM, HeaderTableColumn2LabelWidth);
                                    AppendNewQuoteTableoRow(FieldName, HtmlTemplate, QuoteOPTemplateTextDesignPMHeaderLable, StyleQuoteHeaderLable, styleTd);

                                    //TextValue
                                    styleTd = GetStyleRowTable(QuoteOPTemplateTextDesignPMHeaderValue, QuoteOPTemplateTableDesignPM, HeaderTableColumn2ValueWidth);
                                    AppendNewQuoteTableoRow(FieldValue, HtmlTemplate, QuoteOPTemplateTextDesignPMHeaderValue, StyleQuoteHeaderValue, styleTd);

                                }
                            }

                            else
                            {
                                styleTd = GetStyleRowTable(QuoteOPTemplateTextDesignPMHeaderLable, QuoteOPTemplateTableDesignPM, null);
                                HtmlTemplate.Append("<td " + styleTd + ">" + "<Div style='width:70px;'" + ">" + "" + "</Div>" + "</td>");

                                styleTd = GetStyleRowTable(QuoteOPTemplateTextDesignPMHeaderValue, QuoteOPTemplateTableDesignPM, null);
                                HtmlTemplate.Append("<td " + styleTd + ">" + "<Div style='width:70px;'" + ">" + "" + "</Div>" + "</td>");
                            }

                            HtmlTemplate.Append("</tr>");
                            ++J;
                        }
                    }

                    else
                    {
                        int J = 0;
                        int Count = QuoteTemplaetHeaderColum0.Count();
                        TdCount = 4;
                        foreach (QuoteOPTemplateHeaderFieldPM Feild in QuoteTemplaetHeaderColum1)
                        {
                            HtmlTemplate.Append("<tr>");


                            if (Count > 0 && J <= Count - 1)
                            {
                                if (QuoteTemplaetHeaderColum0[J] != null)
                                {
                                    FieldName = "";
                                    FieldName = GetNameColum(QuoteTemplaetHeaderColum0[J].FieldCode, textcodes, "QH");
                                    if (string.IsNullOrEmpty(FieldName))
                                    {
                                        FieldName = TranslateTextsClass.Translate(QuoteTemplaetHeaderColum0[J].FieldCode, Feild.Tenant);
                                    }

                                    if ((QuoteOPTemplateTableDesignPM.BorderTypeCode == "NONE"))
                                    {
                                        if (!string.IsNullOrEmpty(FieldName)) FieldName += " :";

                                    }

                                    FieldValue = GetQuoteOPTemplateHeaderFieldValue(QuoteTemplaetHeaderColum0[J].FieldCode, QuoteOPPM);

                                    if (FieldValue == "CustomField")
                                    {
                                        FieldValue = GetCustomFelidValue(QuoteTemplaetHeaderColum0[J].FieldCode, customFields, QuoteOPPM);
                                    }
                                    //FieldName
                                    styleTd = GetStyleRowTable(QuoteOPTemplateTextDesignPMHeaderLable, QuoteOPTemplateTableDesignPM, HeaderTableColumn1LabelWidth);
                                    AppendNewQuoteTableoRow(FieldName, HtmlTemplate, QuoteOPTemplateTextDesignPMHeaderLable, StyleQuoteHeaderLable, styleTd);

                                    //FieldValue
                                    styleTd = GetStyleRowTable(QuoteOPTemplateTextDesignPMHeaderValue, QuoteOPTemplateTableDesignPM, HeaderTableColumn1ValueWidth);
                                    AppendNewQuoteTableoRow(FieldValue, HtmlTemplate, QuoteOPTemplateTextDesignPMHeaderValue, StyleQuoteHeaderValue, styleTd);


                                    // HtmlTemplate.Append("<td  style = 'width = 20px'>" + "</td>");
                                }
                            }
                            else
                            {
                                styleTd = GetStyleRowTable(QuoteOPTemplateTextDesignPMHeaderLable, QuoteOPTemplateTableDesignPM, null);
                                HtmlTemplate.Append("<td " + styleTd + ">" + "<Div style='width:70px;'" + ">" + "" + "</Div>" + "</td>");

                                styleTd = GetStyleRowTable(QuoteOPTemplateTextDesignPMHeaderValue, QuoteOPTemplateTableDesignPM, null);
                                HtmlTemplate.Append("<td " + styleTd + ">" + "<Div style='width:70px;'" + ">" + "" + "</Div>" + "</td>");
                            }

                            FieldName = "";
                            FieldName = GetNameColum(Feild.FieldCode, textcodes, "QH");
                            if (string.IsNullOrEmpty(FieldName))
                            {
                                FieldName = TranslateTextsClass.Translate(Feild.FieldCode, Feild.Tenant);
                            }

                            if ((QuoteOPTemplateTableDesignPM.BorderTypeCode == "NONE"))
                            {
                                if (!string.IsNullOrEmpty(FieldName)) FieldName += " :";

                            }

                            FieldValue = GetQuoteOPTemplateHeaderFieldValue(Feild.FieldCode, QuoteOPPM);
                            if (FieldValue == "CustomField")
                            {
                                FieldValue = GetCustomFelidValue(Feild.FieldCode, customFields, QuoteOPPM);
                            }
                            //FieldName
                            styleTd = GetStyleRowTable(QuoteOPTemplateTextDesignPMHeaderLable, QuoteOPTemplateTableDesignPM, HeaderTableColumn2LabelWidth);
                            AppendNewQuoteTableoRow(FieldName, HtmlTemplate, QuoteOPTemplateTextDesignPMHeaderLable, StyleQuoteHeaderLable, styleTd);

                            //FieldValue
                            styleTd = GetStyleRowTable(QuoteOPTemplateTextDesignPMHeaderLable, QuoteOPTemplateTableDesignPM, HeaderTableColumn2LabelWidth);
                            AppendNewQuoteTableoRow(FieldValue, HtmlTemplate, QuoteOPTemplateTextDesignPMHeaderValue, StyleQuoteHeaderValue, styleTd);


                            HtmlTemplate.Append("</tr>");
                            ++J;
                        }
                    }
                }

                HtmlTemplate.Append("</table>");
                HtmlTemplate.Append("</div>");
            }


 
            HtmlTemplate.Append("</body>");
            HtmlTemplate.Append("</html>");

            return Encoding.UTF8.GetBytes(HtmlTemplate.ToString());
        }
        private void AppendNewQuoteTableoRow(string text, StringBuilder HtmlTemplate, QuoteOPTemplateTextDesignPM QuoteOPTemplateTextDesignPM, string styleLable, string styleTd)
        {
            if (QuoteOPTemplateTextDesignPM.Italic)
            {
                HtmlTemplate.Append("<td  " + styleTd + ">" + "<i>" + "<Div " + styleLable + ">" + text + "</Div>" + "</i>" + "</td>");
            }
            else
            {
                HtmlTemplate.Append("<td " + styleTd + ">" + "<Div " + styleLable + ">" + text + "</Div>" + "</td>");
            }
        }
        public byte[] GetQuoteOPTemplateDetails(QuoteOPTemplateBuildArges QuoteOPTemplateBuildArges )
        {

            QuoteOPPM QuoteOPPM = QuoteOPTemplateBuildArges.QuoteOPPM;
            QuoteOPTemplatePM template = QuoteOPTemplateBuildArges.QuoteOPTemplatePM;
            QuoteOPTemplateSettingPM setting = QuoteOPTemplateBuildArges.QuoteOPTemplateSettingPM;
            List<QuoteOPTemplateTextDesignPM> QuoteOPTemplateTextDesignsList = QuoteOPTemplateBuildArges.QuoteOPTemplateTextDesignPMLists;
            List<QuoteOPTemplateTableDesignPM> QuoteOPTemplateTableDesignsList = QuoteOPTemplateBuildArges.QuoteOPTemplateTableDesignsLists;
            List<QuoteOPTemplateTextCodePM> textcodes = QuoteOPTemplateBuildArges.QuoteOPTemplateTextCodePMLists;
            int tenant = QuoteOPTemplateBuildArges.Tenant;
            List<ObjectField> customFields = ObjectFieldRepository.GetCustomObjectFieldsByObjectTableName("Quote", tenant).ToList();
            if (QuoteOPPM == null) QuoteOPPM = BuildingQuoteOPPM();
            if (QuoteOPTemplateBuildArges.VersionNumber != null) QuoteOPPM.QuoteVersion = QuoteOPPM.QuoteNumber + "-" + (int)QuoteOPTemplateBuildArges.VersionNumber;
            Tenant = QuoteOPPM.Tenant;
            string FieldValue = "";
            string FieldName = "";
            string dir = "";
            if (setting.RightToLeft) dir = "dir='RTL'";
            isRightToLeft = setting.RightToLeft;
            QuoteOPTemplateDetailsFieldQuery QuoteOPTemplateDetailsFieldQuery = new QuoteOPTemplateDetailsFieldQuery(tenant);
            IQueryable<QuoteOPTemplateDetailsFieldPM> QuoteTemplaetDetailsFieldList = QuoteOPTemplateDetailsFieldQuery.GetQuoteOPTemplateDetailsFieldPMsByQuoteOPTemplateId(tenant, template.Id);
            StringBuilder HtmlTemplate = new StringBuilder();

            HtmlTemplate.Append("<!DOCTYPE html>");
            HtmlTemplate.Append("<html  lang='ar'>");
            HtmlTemplate.Append("<head>");
            HtmlTemplate.Append("<title></title>");
            HtmlTemplate.Append("<meta charset='utf-8'>");
            HtmlTemplate.Append("</head>");
            HtmlTemplate.Append("<body>");


            if (QuoteTemplaetDetailsFieldList.Count() > 0)
            {

                HtmlTemplate.Append(GetHtmlStringLine(setting.SpaceLinesBeforeQuoteDetails, QuoteOPTemplateBuildArges.FirstSectionInBody));
                List<QuoteOPTemplateDetailsFieldPM> QuoteTemplaetDetailsColum0;
                List<QuoteOPTemplateDetailsFieldPM> QuoteTemplaetDetailsColum1;

                QuoteTemplaetDetailsColum0 = QuoteTemplaetDetailsFieldList.Where(d => d.Column == 0).OrderBy(d => d.Row).ToList();
                QuoteTemplaetDetailsColum1 = QuoteTemplaetDetailsFieldList.Where(d => d.Column == 1).OrderBy(d => d.Row).ToList();

                IsShowlanguage = setting.ShowLocalLanguage;

                QuoteOPTemplateTableDesignPM QuoteOPTemplateTableDesignPM = QuoteOPTemplateTableDesignsList.Where(t => t.Id == setting.DetailsTableDesignId).FirstOrDefault();
                QuoteOPTemplateTextDesignPM QuoteOPTemplateTextDesignPMDetailsLable = QuoteOPTemplateTextDesignsList.Where(t => t.Id == QuoteOPTemplateTableDesignPM.HeaderDesignId).FirstOrDefault();
                QuoteOPTemplateTextDesignPM QuoteOPTemplateTextDesignPMDetailsValue = QuoteOPTemplateTextDesignsList.Where(t => t.Id == QuoteOPTemplateTableDesignPM.LinesDesignId).FirstOrDefault();

                string styleTable = GetStyleTable(QuoteOPTemplateTableDesignPM);

                string StyleQuoteDetailsLable = GetsyleSpanQuoteHeaderDetails(QuoteOPTemplateTextDesignPMDetailsLable);
                string styleTd = "";
                string widthtable = "";
                bool IsDetailsTableAuto = setting.DetailsTableColumWidthType == "AUTO" ? true : false;

                string StyleQuoteDetailsValue = GetsyleSpanQuoteHeaderDetails(QuoteOPTemplateTextDesignPMDetailsValue);

                string DetailsTableColumn1LabelWidth = IsDetailsTableAuto ? "Auto" : (setting.DetailsTableColumn1LabelWidth.ToString() + "%");
                string DetailsTableColumn1ValueWidth = IsDetailsTableAuto ? "Auto" : (setting.DetailsTableColumn1ValueWidth.ToString() + "%");
                string DetailsTableColumn2LabelWidth = IsDetailsTableAuto ? "Auto" : (setting.DetailsTableColumn2LabelWidth.ToString() + "%");
                string DetailsTableColumn2ValueWidth = IsDetailsTableAuto ? "Auto" : (setting.DetailsTableColumn2ValueWidth.ToString() + "%");

                double? W = setting.DetailsTableColumn1LabelWidth + setting.DetailsTableColumn1ValueWidth;
                if (setting.DetailsSectionHasTwoColumns) W += (setting.DetailsTableColumn2LabelWidth + setting.DetailsTableColumn2ValueWidth);
                widthtable = W.ToString() + "%";
                HtmlTemplate.Append("<div " + dir + " style ='width=100%' >");

                if (QuoteOPTemplateTableDesignPM.BorderTypeCode == "NONE") styleTable = "";
                if (IsDetailsTableAuto) HtmlTemplate.Append("<table   " + dir + " width='Auto' " + styleTable + " >");
                else HtmlTemplate.Append("<table " + dir + " width='" + widthtable + "'" + styleTable + " >");

                //  ShowTitleQuoteDetails
                if (setting.ShowTitleQuoteDetails)
                {
                    QuoteOPTemplateTextDesignPM QuoteOPTemplateTextDesignPMDetailsTitle = QuoteOPTemplateTextDesignsList.Where(t => t.Id == setting.DetailsTitleDesignId).FirstOrDefault();//QuoteOPTemplateTextDesignQuery.GetSinglePM(setting.DetailsTitleDesignId, tenant);

                    if (setting.RightToLeft)
                    {
                        QuoteOPTemplateTextDesignPMDetailsTitle.Alignment = "Right";
                    }
                    string styledetailstitle = GetSpanRowStyle(QuoteOPTemplateTextDesignPMDetailsTitle, "Header", "auto");

                    FieldName = "";
                    FieldName = GetNameColum("GENERALDETAILS", textcodes, "QD");

                    if ((QuoteOPTemplateTableDesignPM.BorderTypeCode == "NONE"))
                    {
                        if (!string.IsNullOrEmpty(FieldName))
                        {
                            FieldName += " :";
                        }
                    }
                    if (QuoteOPTemplateTextDesignPMDetailsTitle.Italic)
                    {
                        HtmlTemplate.Append("<div " + styledetailstitle + ">" + "<i " + styledetailstitle + ">" + FieldName + "</i>" + "</div>");
                    }
                    else
                    {
                        HtmlTemplate.Append("<div " + styledetailstitle + ">" + FieldName + "</div>");
                    }

                    HtmlTemplate.Append("<div  style='height:8px;'>" + " &nbsp;  &nbsp; &nbsp; &nbsp; &nbsp;  &nbsp;   &nbsp;" + "</div>");
                }



                if (!setting.DetailsSectionHasTwoColumns)
                {
                    foreach (QuoteOPTemplateDetailsFieldPM Feild in QuoteTemplaetDetailsColum0)
                    {
                        HtmlTemplate.Append("<tr>");
                        FieldName = "";
                        FieldName = GetNameColum(Feild.FieldCode, textcodes, "QD");
                        if (string.IsNullOrEmpty(FieldName)) FieldName = TranslateTextsClass.Translate(Feild.FieldCode, Feild.Tenant);
                        if ((QuoteOPTemplateTableDesignPM.BorderTypeCode == "NONE"))
                        {
                            if (!string.IsNullOrEmpty(FieldName)) FieldName += " :";
                        }

                        FieldValue = GetQuoteOPTemplateDetailsFieldValue(Feild.FieldCode, QuoteOPPM);
                        if (FieldValue == "CustomField") FieldValue = GetCustomFelidValue(Feild.FieldCode, customFields, QuoteOPPM);

                        //Lable
                        styleTd = GetStyleRowTable(QuoteOPTemplateTextDesignPMDetailsLable, QuoteOPTemplateTableDesignPM, DetailsTableColumn1LabelWidth);
                        AppendNewQuoteTableoRow(FieldName, HtmlTemplate, QuoteOPTemplateTextDesignPMDetailsLable, StyleQuoteDetailsLable, styleTd);

                        //Value
                        styleTd = GetStyleRowTable(QuoteOPTemplateTextDesignPMDetailsValue, QuoteOPTemplateTableDesignPM, DetailsTableColumn1ValueWidth);
                        AppendNewQuoteTableoRow(FieldValue, HtmlTemplate, QuoteOPTemplateTextDesignPMDetailsValue, StyleQuoteDetailsValue, styleTd);

                        HtmlTemplate.Append("</tr>");
                    }
                }
                else
                {
                    if (QuoteTemplaetDetailsColum0.Count() == QuoteTemplaetDetailsColum1.Count() || QuoteTemplaetDetailsColum0.Count() > QuoteTemplaetDetailsColum1.Count())
                    {
                        int J = 0;
                        TdCount = 4;
                        int Count = QuoteTemplaetDetailsColum1.Count();
                        foreach (QuoteOPTemplateDetailsFieldPM Feild in QuoteTemplaetDetailsColum0)
                        {
                            HtmlTemplate.Append("<tr>");


                            FieldName = "";
                            FieldName = GetNameColum(Feild.FieldCode, textcodes, "QD");

                            if (string.IsNullOrEmpty(FieldName)) FieldName = TranslateTextsClass.Translate(Feild.FieldCode, Feild.Tenant);

                            if ((QuoteOPTemplateTableDesignPM.BorderTypeCode == "NONE"))
                            {
                                if (!string.IsNullOrEmpty(FieldName)) FieldName += " :";
                            }

                            FieldValue = GetQuoteOPTemplateDetailsFieldValue(Feild.FieldCode, QuoteOPPM);

                            if (FieldValue == "CustomField")
                            {
                                FieldValue = GetCustomFelidValue(Feild.FieldCode, customFields, QuoteOPPM);
                            }

                            //TextFeild
                            styleTd = GetStyleRowTable(QuoteOPTemplateTextDesignPMDetailsLable, QuoteOPTemplateTableDesignPM, DetailsTableColumn1LabelWidth);
                            AppendNewQuoteTableoRow(FieldName, HtmlTemplate, QuoteOPTemplateTextDesignPMDetailsLable, StyleQuoteDetailsLable, styleTd);


                            //Text Value
                            styleTd = GetStyleRowTable(QuoteOPTemplateTextDesignPMDetailsValue, QuoteOPTemplateTableDesignPM, DetailsTableColumn1ValueWidth);
                            AppendNewQuoteTableoRow(FieldValue, HtmlTemplate, QuoteOPTemplateTextDesignPMDetailsValue, StyleQuoteDetailsValue, styleTd);



                            if (Count > 0 && J <= Count - 1)
                            {
                                if (QuoteTemplaetDetailsColum1[J] != null)
                                {
                                    FieldName = "";
                                    FieldName = GetNameColum(QuoteTemplaetDetailsColum1[J].FieldCode, textcodes, "QD");

                                    if (string.IsNullOrEmpty(FieldName))
                                    {
                                        FieldName = TranslateTextsClass.Translate(QuoteTemplaetDetailsColum1[J].FieldCode, Feild.Tenant);
                                    }

                                    if ((QuoteOPTemplateTableDesignPM.BorderTypeCode == "NONE"))
                                    {
                                        if (!string.IsNullOrEmpty(FieldName)) FieldName += " :";
                                    }

                                    FieldValue = GetQuoteOPTemplateDetailsFieldValue(QuoteTemplaetDetailsColum1[J].FieldCode, QuoteOPPM);
                                    if (FieldValue == "CustomField")
                                    {
                                        FieldValue = GetCustomFelidValue(Feild.FieldCode, customFields, QuoteOPPM);
                                    }


                                    //TextFeild
                                    styleTd = GetStyleRowTable(QuoteOPTemplateTextDesignPMDetailsLable, QuoteOPTemplateTableDesignPM, DetailsTableColumn2LabelWidth);
                                    AppendNewQuoteTableoRow(FieldName, HtmlTemplate, QuoteOPTemplateTextDesignPMDetailsLable, StyleQuoteDetailsLable, styleTd);

                                    //TextValue
                                    styleTd = GetStyleRowTable(QuoteOPTemplateTextDesignPMDetailsValue, QuoteOPTemplateTableDesignPM, DetailsTableColumn2ValueWidth);
                                    AppendNewQuoteTableoRow(FieldValue, HtmlTemplate, QuoteOPTemplateTextDesignPMDetailsValue, StyleQuoteDetailsValue, styleTd);

                                }
                            }

                            else
                            {
                                styleTd = GetStyleRowTable(QuoteOPTemplateTextDesignPMDetailsLable, QuoteOPTemplateTableDesignPM, null);
                                HtmlTemplate.Append("<td " + styleTd + ">" + "<Div style='width:70px;'" + ">" + "" + "</Div>" + "</td>");

                                styleTd = GetStyleRowTable(QuoteOPTemplateTextDesignPMDetailsValue, QuoteOPTemplateTableDesignPM, null);
                                HtmlTemplate.Append("<td " + styleTd + ">" + "<Div style='width:70px;'" + ">" + "" + "</Div>" + "</td>");
                            }

                            HtmlTemplate.Append("</tr>");
                            ++J;
                        }
                    }

                    else
                    {
                        int J = 0;
                        int Count = QuoteTemplaetDetailsColum0.Count();
                        TdCount = 4;
                        foreach (QuoteOPTemplateDetailsFieldPM Feild in QuoteTemplaetDetailsColum1)
                        {
                            HtmlTemplate.Append("<tr>");


                            if (Count > 0 && J <= Count - 1)
                            {
                                if (QuoteTemplaetDetailsColum0[J] != null)
                                {
                                    FieldName = "";
                                    FieldName = GetNameColum(QuoteTemplaetDetailsColum0[J].FieldCode, textcodes, "QD");
                                    if (string.IsNullOrEmpty(FieldName))
                                    {
                                        FieldName = TranslateTextsClass.Translate(QuoteTemplaetDetailsColum0[J].FieldCode, Feild.Tenant);
                                    }

                                    if ((QuoteOPTemplateTableDesignPM.BorderTypeCode == "NONE"))
                                    {
                                        if (!string.IsNullOrEmpty(FieldName)) FieldName += " :";

                                    }

                                    FieldValue = GetQuoteOPTemplateDetailsFieldValue(QuoteTemplaetDetailsColum0[J].FieldCode, QuoteOPPM);

                                    if (FieldValue == "CustomField")
                                    {
                                        FieldValue = GetCustomFelidValue(QuoteTemplaetDetailsColum0[J].FieldCode, customFields, QuoteOPPM);
                                    }
                                    //FieldName
                                    styleTd = GetStyleRowTable(QuoteOPTemplateTextDesignPMDetailsLable, QuoteOPTemplateTableDesignPM, DetailsTableColumn1LabelWidth);
                                    AppendNewQuoteTableoRow(FieldName, HtmlTemplate, QuoteOPTemplateTextDesignPMDetailsLable, StyleQuoteDetailsLable, styleTd);

                                    //FieldValue
                                    styleTd = GetStyleRowTable(QuoteOPTemplateTextDesignPMDetailsValue, QuoteOPTemplateTableDesignPM, DetailsTableColumn1ValueWidth);
                                    AppendNewQuoteTableoRow(FieldValue, HtmlTemplate, QuoteOPTemplateTextDesignPMDetailsValue, StyleQuoteDetailsValue, styleTd);


                                    // HtmlTemplate.Append("<td  style = 'width = 20px'>" + "</td>");
                                }
                            }
                            else
                            {
                                styleTd = GetStyleRowTable(QuoteOPTemplateTextDesignPMDetailsLable, QuoteOPTemplateTableDesignPM, null);
                                HtmlTemplate.Append("<td " + styleTd + ">" + "<Div style='width:70px;'" + ">" + "" + "</Div>" + "</td>");

                                styleTd = GetStyleRowTable(QuoteOPTemplateTextDesignPMDetailsValue, QuoteOPTemplateTableDesignPM, null);
                                HtmlTemplate.Append("<td " + styleTd + ">" + "<Div style='width:70px;'" + ">" + "" + "</Div>" + "</td>");
                            }

                            FieldName = "";
                            FieldName = GetNameColum(Feild.FieldCode, textcodes, "QD");
                            if (string.IsNullOrEmpty(FieldName))
                            {
                                FieldName = TranslateTextsClass.Translate(Feild.FieldCode, Feild.Tenant);
                            }

                            if ((QuoteOPTemplateTableDesignPM.BorderTypeCode == "NONE"))
                            {
                                if (!string.IsNullOrEmpty(FieldName)) FieldName += " :";

                            }

                            FieldValue = GetQuoteOPTemplateDetailsFieldValue(Feild.FieldCode, QuoteOPPM);
                            if (FieldValue == "CustomField")
                            {
                                FieldValue = GetCustomFelidValue(Feild.FieldCode, customFields, QuoteOPPM);
                            }
                            //FieldName
                            styleTd = GetStyleRowTable(QuoteOPTemplateTextDesignPMDetailsLable, QuoteOPTemplateTableDesignPM, DetailsTableColumn2LabelWidth);
                            AppendNewQuoteTableoRow(FieldName, HtmlTemplate, QuoteOPTemplateTextDesignPMDetailsLable, StyleQuoteDetailsLable, styleTd);

                            //FieldValue
                            styleTd = GetStyleRowTable(QuoteOPTemplateTextDesignPMDetailsLable, QuoteOPTemplateTableDesignPM, DetailsTableColumn2ValueWidth);
                            AppendNewQuoteTableoRow(FieldValue, HtmlTemplate, QuoteOPTemplateTextDesignPMDetailsValue, StyleQuoteDetailsValue, styleTd);


                            HtmlTemplate.Append("</tr>");
                            ++J;
                        }
                    }
                }

                HtmlTemplate.Append("</table>");
                HtmlTemplate.Append("</div>");
            }

            HtmlTemplate.Append("</body>");
            HtmlTemplate.Append("</html>");

            return Encoding.UTF8.GetBytes(HtmlTemplate.ToString());
        }

    #endregion

    #region Sections

        public QuoteOPPM BuildingQuoteOPPM()
        {
            QuoteOPPM QuoteOPPM = new QuoteOPPM()
            {
                Id = "10697",
                QuoteNumber = "EX1009",
                DirectionId = "D",
                ExpirationDate = new DateTime(2010, 1, 18),
                CustomerName = "Notify1",
                ExpirationDays = 30,
                FromPort = "Fortaleza",
                ToPort = "Matupa",
                ShipperName = "Shipper",
                ShipperContactId = "1-150",
                ShipperReference1 = "SHI-R1",
                ShipperReference2 = "SHI-R2",
                ShipperMainAddressId = "",
                ConsigneeName = "Abed",
                ConsigneeContactId = "1-150",
                ConsigneeReference1 = "CON-R1",
                ConsigneeReference2 = "CON-R2",
                ConsigneeMainAddressId = "",
                CustomerContactId = "1-161",
                CustomerReference1 = "CUS-R1",
                CustomerReference2 = "CUSR2",
                PickupLocation = "",
                DeliveryLocation = "",
                IncotermName = "",
                ShipmentTypeId = "",
                TransportModeName = "",
                SalesmanName = "Ahamd",
                DescriptionOfGoods = "GOOOOOOOOOOOOOOOOOOOOOOOOODS ....",
                IsDangerous = true,
                MainCarriageCarrierId = "1-714",
                TransportModeId = "A",
                SaleTotalAmountInLocalCurrency = 4000,
                SaleTotalAmountInSaleCurrency = 1750,
                SaleCurrencyCode = "USD",
                TotalPerContainer = true,
                IsChargesByVAT = true,
            };

            QuoteOPPM.QuoteSaleCharges.Add(new QuoteOPSaleChargePM() { VatTypeName ="vat1",VatPercentage =50 ,ChargesTypeCode = "AFT", Notes = "test", ChargesGroupCode = "FRT", ChargesTypeName = "Air Freight", SaleQuantity = 500, SaleUnitPrice = 1, SaleMeasurementShortName = "Ch Weight", SaleTotalAmount = 500, SaleTotalAmountLocal = 600, CurrencyCode = "USD", ChargesTypeDescription = "Air Freight Description", SaleMaxAmount = 50, SaleMinAmount = 20 });
            QuoteOPPM.QuoteSaleCharges.Add(new QuoteOPSaleChargePM() { VatTypeName = "vat2", VatPercentage = 40, ChargesTypeCode = "AFT", Notes = "test2", ChargesGroupCode = "FRT", ChargesTypeName = "Air Freight", SaleQuantity = 500, SaleUnitPrice = 1, SaleMeasurementShortName = "Ch Weight", SaleTotalAmount = 500, SaleTotalAmountLocal = 600, CurrencyCode = "USD", ChargesTypeDescription = "Air Freight Description1", SaleMaxAmount = 40, SaleMinAmount = 25 });
            QuoteOPPM.QuoteSaleCharges.Add(new QuoteOPSaleChargePM() { VatTypeName = "vat3", VatPercentage = 30, ChargesTypeCode = "DEMU", Notes = "test3", ChargesGroupCode = "HNDCH", ChargesTypeName = "Demmurage", SaleQuantity = 500, SaleUnitPrice = 1, SaleMeasurementShortName = "Ch Weight", SaleTotalAmount = 500, SaleTotalAmountLocal = 600, CurrencyCode = "USD", ChargesTypeDescription = "Air Freight Description2", SaleMaxAmount = 45, SaleMinAmount = 33 });
            QuoteOPPM.QuoteSaleCharges.Add(new QuoteOPSaleChargePM() { VatTypeName = "vat4", VatPercentage = 10, ChargesTypeCode = "DU", Notes = "test4", ChargesGroupCode = "FRT", ChargesTypeName = "Duties", SaleQuantity = 500, SaleUnitPrice = 1, SaleMeasurementShortName = "Ch Weight", SaleTotalAmount = 500, SaleTotalAmountLocal = 600, CurrencyCode = "USD", ChargesTypeDescription = "Air Freight Description3", SaleMaxAmount = 22, SaleMinAmount = 15 });

            return QuoteOPPM;
        }

        public string GetCustomFelidValue(string fullNameTextCode, List<ObjectField> customFields, QuoteOPPM QuoteOPPM)
        {
            string FieldValue = "";

            ObjectField field = customFields.Where(d => d.FullNameTextCode.Code == fullNameTextCode).FirstOrDefault();
            if (field != null)
            {
                string FullNameTextCode = field.FullNameTextCode.Code;
                CustomFieldResolver customFieldResolver = new CustomFieldResolver();

                PropertyInfo propInfo = typeof(QuoteOPPM).GetProperty(field.FieldName);
                object newValue = customFieldResolver.GetFieldValue(QuoteOPPM, field, QuoteOPPM.Tenant);
                if (newValue != null)
                {
                    if (field.DataTypeCode.ToLower() == "boolean")
                    {
                        FieldValue = newValue.ToString().ToLower() == "false" ? TranslateTextsClass.Translate("General.O.No", QuoteOPPM.Tenant) : TranslateTextsClass.Translate("General.O.Yes", QuoteOPPM.Tenant);
                    }
                    else FieldValue = newValue.ToString();

                }
            }
            return FieldValue;
        }

        public string GetBodyString(byte[] data)
        {
            string htmlString = "";
            if (data != null)
            {
                htmlString = Encoding.UTF8.GetString(data);
                if (htmlString.Contains("<body>"))
                {
                    int startIndex = htmlString.IndexOf("<body>") + 6;
                    int length = htmlString.IndexOf("</body>") - startIndex;
                    string bodyString = htmlString.Substring(startIndex, length);
                }

            }
            return htmlString;
        }

        private string GetImageHtmlString(string imageDetailId, int tenant, string styleimage)
        {
            string imagHtml;
            ImageDetailRepository imageDetailsRepository = new ImageDetailRepository(tenant);
            ImageDetail imageDetail = imageDetailsRepository.GetSingleImageDetail(imageDetailId, tenant);
            string rawData = "";
            byte[] imageData = this.GetFile(imageDetail.Id, imageDetail.Extension, "images", tenant);
            string extension = "png";
            if (imageData != null)
            {
                extension = imageDetail.Extension;
                char[] base64Data = new char[(int)(Math.Ceiling((double)imageData.Length / 3) * 4)];
                Convert.ToBase64CharArray(imageData, 0, imageData.Length, base64Data, 0);
                System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
                rawData = new String(base64Data);

                MemoryStream ms = new MemoryStream(imageData, 0, imageData.Length);          // Convert byte[] to Image    
                ms.Write(imageData, 0, imageData.Length);
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
            }
            imagHtml = "<img " + styleimage + " src='data:image/" + extension + ";base64," + rawData + "'/>";

            return imagHtml;
        }

        private double GetFontSizeInPixles(object points, double pre)
        {
            double pixels = (double)points * pre; //((double)96 / (double)72);
            return pixels;
        }

        private string GetsyleSpanQuoteHeaderDetails(QuoteOPTemplateTextDesignPM headerDesign)
        {
            string FontSize = headerDesign.FontSize + "px";
            string TextColor = headerDesign.TextColor;
            if (headerDesign.TextColor.Length > 7)
            {
                TextColor = headerDesign.TextColor.Remove(1, 2);
            }

            string alignment = headerDesign.Alignment;
            string BackgroundColor = headerDesign.BackgroundColor;
            if (headerDesign.BackgroundColor.Length > 7)
            {
                BackgroundColor = headerDesign.BackgroundColor.Remove(1, 2);
            }

            string style = "";
            string unDerLine = headerDesign.UnDerLine ? ";text-decoration: underline" : "";

            // alignment = isRightToLeft ? "right" : alignment;

            style = "style='" + "font-weight:" + headerDesign.FontWeight + ";font-family:" + headerDesign.FontFamily +
              ";font-size:" + FontSize + ";color:" + TextColor + ";vertical-align:central" + ";height:auto" + unDerLine + ";text-align:" + alignment + " '";

            return style;
        }

        private string GetQuoteOPTemplateDetailsFieldValue(string fieldname, QuoteOPPM QuoteOPPM)
        {
            string FieldValue = "";
            if (fieldname == "EXPIRATIONDAYS")
            {
                FieldValue = QuoteOPPM.ExpirationDays != null ? QuoteOPPM.ExpirationDays.ToString() : "";
            }
            else if (fieldname == "EXPIRATIONDATE")
            {
                FieldValue = QuoteOPPM.ExpirationDate != null ? ConvertToShortDate((DateTime)QuoteOPPM.ExpirationDate, QuoteOPPM.Tenant) : "";
            }
            else if (fieldname == "SHIPPERNAME")
            {
                FieldValue = QuoteOPPM.ShipperName;
            }
            else if (fieldname == "SHIPPERADDRESS")
            {
                if (!string.IsNullOrEmpty(QuoteOPPM.ShipperMainAddressId))
                {
                    AddressRepository addressRep = new AddressRepository(QuoteOPPM.Tenant);
                    Address address = addressRep.GetSingleAddress(QuoteOPPM.ShipperMainAddressId, QuoteOPPM.Tenant);

                    if (address != null)
                    {
                        FieldValue = General.GetAddress(address);
                    }
                }
            }
            else if (fieldname == "SHIPPERCONTACT")
            {
                if (!string.IsNullOrEmpty(QuoteOPPM.ShipperContactId))
                {
                    ContactRepository contactRep = new ContactRepository(QuoteOPPM.Tenant);
                    FieldValue = contactRep.GetContactNameById(QuoteOPPM.ShipperContactId, QuoteOPPM.Tenant);
                }
            }
            else if (fieldname == "SHIPPERREFERENCES")
            {

                FieldValue = CombinedReferences(QuoteOPPM.ShipperReference1, QuoteOPPM.ShipperReference2);
            }
            // Consignee
            else if (fieldname == "CONSIGNEENAME")
            {
                FieldValue = QuoteOPPM.ConsigneeName;
            }
            else if (fieldname == "CONSIGNEEADDRESS")
            {
                if (!string.IsNullOrEmpty(QuoteOPPM.ConsigneeMainAddressId))
                {
                    AddressRepository addressRep = new AddressRepository(QuoteOPPM.Tenant);
                    Address address = addressRep.GetSingleAddress(QuoteOPPM.ConsigneeMainAddressId, QuoteOPPM.Tenant);

                    if (address != null)
                    {
                        FieldValue = General.GetAddress(address);
                    }
                }
            }
            else if (fieldname == "CONSIGNEECONTACT")
            {
                if (!string.IsNullOrEmpty(QuoteOPPM.ConsigneeContactId))
                {
                    ContactRepository contactRep = new ContactRepository(QuoteOPPM.Tenant);
                    FieldValue = contactRep.GetContactNameById(QuoteOPPM.ConsigneeContactId, QuoteOPPM.Tenant);

                }
            }
            else if (fieldname == "CONSIGNEEREFERENCES")
            {

                FieldValue = CombinedReferences(QuoteOPPM.ConsigneeReference1, QuoteOPPM.ConsigneeReference2);
            }
            else if (fieldname == "CUSTOMERNAME")
            {
                FieldValue = QuoteOPPM.CustomerName;
            }
            else if (fieldname == "CUSTOMERADDRESS")
            {
                FieldValue = "";
            }
            else if (fieldname == "CUSTOMERCONTACT")
            {
                if (!string.IsNullOrEmpty(QuoteOPPM.CustomerContactId))
                {
                    ContactRepository contactRep = new ContactRepository(QuoteOPPM.Tenant);
                    FieldValue = contactRep.GetContactNameById(QuoteOPPM.CustomerContactId, QuoteOPPM.Tenant);
                }
            }
            else if (fieldname == "CUSTOMERREFERENCES")
            {

                FieldValue = CombinedReferences(QuoteOPPM.CustomerReference1, QuoteOPPM.CustomerReference2);
            }
            else if (fieldname == "QUOTENUMBER")
            {
                FieldValue = !string.IsNullOrEmpty(QuoteOPPM.QuoteVersion) ? QuoteOPPM.QuoteVersion : QuoteOPPM.QuoteNumber;
            }
            else if (fieldname == "PICKUPFROM")
            {
                FieldValue = QuoteOPPM.PickupLocation;
            }
            else if (fieldname == "DELIVERYTO")
            {
                FieldValue = QuoteOPPM.DeliveryLocation;
            }
            else if (fieldname == "FROMPORT" || fieldname == "FROMLOCATION")
            {
                FieldValue = QuoteOPPM.FromPortName;
            }
            else if (fieldname == "TOPORT" || fieldname == "TOLOCATION")
            {
                FieldValue = QuoteOPPM.ToPortName;
            }
            else if (fieldname == "INCOTERMS")
            {
                FieldValue = QuoteOPPM.IncotermName;
            }
            else if (fieldname == "SERVICE")
            {
#if false


                // FieldValue = QuoteOPPM.
                if (!string.IsNullOrEmpty(QuoteOPPM.ShipmentTypeId))
                {
                    ShipmentTypeRepository typeRep = new ShipmentTypeRepository(QuoteOPPM.Tenant);
                    ShipmentType shiptype = typeRep.GetSingleShipmentType(QuoteOPPM.ShipmentTypeId);
                    if (shiptype != null)
                    {
                        FieldValue = QuoteOPPM.TransportModeName + shiptype.Name;
                    }
                }
#endif
            }
            else if (fieldname == "TRANSITTIME")
            {
                FieldValue = QuoteOPPM.TransitTime;
            }
            else if (fieldname == "MOVETYPE")
            {
                if (!string.IsNullOrEmpty(QuoteOPPM.MoveTypeId))
                {
                    MoveTypeQuery moveTypeQuery = new MoveTypeQuery(QuoteOPPM.Tenant);
                    FieldValue = moveTypeQuery.GetMoveTypeNameById(QuoteOPPM.MoveTypeId, QuoteOPPM.Tenant);
                }
            }
            else if (fieldname == "DESCRIPTIONOFGOODS")
            {
                FieldValue = QuoteOPPM.DescriptionOfGoods;
            }
            else if (fieldname == "DEPARTUREFREQUENCY")
            {
                FieldValue = QuoteOPPM.DepartureFrequency;
            }
            else if (fieldname == "DANGEROUSGOODS")
            {
                FieldValue = !QuoteOPPM.IsDangerous ? TranslateTextsClass.Translate("General.O.No", QuoteOPPM.Tenant) : TranslateTextsClass.Translate("General.O.Yes", QuoteOPPM.Tenant);
            }
            else if (fieldname == "TRUCKER" || fieldname == "SHIPINGLINE" || fieldname == "AIRLINE")
            {
                if (!string.IsNullOrEmpty(QuoteOPPM.MainCarriageCarrierId))
                {
                    CardRepository cardRep = new CardRepository(QuoteOPPM.Tenant);
                    FieldValue = cardRep.GetEnglishNameCardById(QuoteOPPM.MainCarriageCarrierId, QuoteOPPM.Tenant);
                }
            }
            else if (fieldname == "CHARGEABLEWEIGHT")
            {
                if (QuoteOPPM.ChargeableWeight != null)
                {
                    FieldValue = QuoteOPPM.ChargeableWeight.ToString() + " " + QuoteOPPM.ChargeableWeightUnitCode.ToString();
                }
            }
            else if (fieldname == "GROSSWEIGHT")
            {
                if (QuoteOPPM.GrossWeight != null)
                {
                    FieldValue = QuoteOPPM.GrossWeight.ToString() + " " + QuoteOPPM.GrossWeightUnitCode.ToString();
                }
            }
            else if (fieldname == "VOLUME")
            {
                if (QuoteOPPM.Volume != null)
                {
                    FieldValue = QuoteOPPM.Volume.ToString() + " " + QuoteOPPM.VolumeUnitCode.ToString();
                }
            }
            else if (fieldname == "VOLUMETRICWEIGHT")
            {
                if (QuoteOPPM.VolumetricWeight != null)
                {
                    FieldValue = QuoteOPPM.VolumetricWeight.ToString() + " " + QuoteOPPM.VolumeUnitCode.ToString();
                }
            }
            else if (fieldname == "NUMBEROFPACKAGES")
            {
                if (QuoteOPPM.NumberOfPackages != null)
                {
                    FieldValue = QuoteOPPM.NumberOfPackages.ToString();
                }
            }
            else if (fieldname == "NUMBEROFCONTAINERS")
            {
                if (QuoteOPPM.NumberOfContainers != null)
                {
                    FieldValue = QuoteOPPM.NumberOfContainers.ToString();
                }
            }
            else if (fieldname == "NOTIFYNAME")
            {
                FieldValue = QuoteOPPM.NotifyName;
            }
            else if (fieldname == "NOTIFYADDRESS")
            {
                if (!string.IsNullOrEmpty(QuoteOPPM.NotifyAddressId))
                {
                    AddressRepository addressRep = new AddressRepository(QuoteOPPM.Tenant);
                    Address address = addressRep.GetSingleAddress(QuoteOPPM.NotifyAddressId, QuoteOPPM.Tenant);

                    if (address != null)
                    {
                        FieldValue = General.GetAddress(address);
                    }
                }
            }
            else if (fieldname == "NOTIFYCONTACT")
            {
                if (!string.IsNullOrEmpty(QuoteOPPM.NotifyContactId))
                {
                    ContactRepository contactRep = new ContactRepository(QuoteOPPM.Tenant);
                    FieldValue = contactRep.GetContactNameById(QuoteOPPM.NotifyContactId, QuoteOPPM.Tenant);
                }
            }
            else FieldValue = "CustomField";
            return FieldValue;
        }

        private static string CombinedReferences(string ref1, string ref2)
        {
            string result = ref1;
            if (!string.IsNullOrEmpty(ref1) && !string.IsNullOrEmpty(ref2)) result += ",";
            result += ref2;

            return result;
        }

        public string GetQuoteOPTemplateHeaderFieldValue(string fieldname, QuoteOPPM QuoteOPPM)
        {
            string FieldValue = "";

            if (fieldname == "QUOTENUMBER")
            {
                FieldValue =!string.IsNullOrEmpty(QuoteOPPM.QuoteVersion)? QuoteOPPM.QuoteVersion : QuoteOPPM.QuoteNumber;
            }

            else if (fieldname == "EXPIRATIONDATE" && QuoteOPPM.ExpirationDate != null)
            {
                FieldValue = ConvertToShortDate((DateTime)QuoteOPPM.ExpirationDate, QuoteOPPM.Tenant);
            }
            else
                if (fieldname == "CUSTOMER")
            {
                FieldValue = QuoteOPPM.CustomerName;
            }
            else if (fieldname == "QUOTEDATE")
            {
                FieldValue = ConvertToShortDate(DateTime.Now, QuoteOPPM.Tenant);
            }

            else if (fieldname == "ATTN")
            {
                if (QuoteOPPM != null)
                {
                    ContactRepository contactrrep = new ContactRepository(QuoteOPPM.Tenant);
                    Contact contact = contactrrep.GetSingleContact(QuoteOPPM.CustomerContactId, QuoteOPPM.Tenant);

                    if (contact != null)
                    {
                        FieldValue = contact.EnglishName;
                    }
                }
            }
            else
            {
                FieldValue = "CustomField";
            }

            return FieldValue;
        }

        public string ConvertToShortDate(DateTime date, int tenant)
        {
            string dateString = "";

            if (tenantPm == null)
            {

                tenantPm = TenantQuery.GetSingleTenantPM(tenant, true);
            }
            if (tenantPm.DateTimeFormat != null)
            {

                dateString = date.ToString(tenantPm.DateTimeFormat, CultureInfo.CurrentCulture);
            }

            else
            {
                dateString = date.ToString("d", CultureInfo.CurrentCulture);

            }

            //if (!string.IsNullOrEmpty(date))
            //{
            //    date = date.Replace(":", "A");

            //    int index = date.IndexOf("A");
            //    FieldValue = date.Remove(index - 2);
            //}

            return dateString;
        }

        private string GetStyleGroupByTd(string type, string BorderColorr, int BorderThicknesss, string borderTypeCode, QuoteOPTemplateTextDesignPM QuoteOPTemplateTextDesigGroupBy, bool RightToLeft)
        {

            string BorderColor = BorderColorr.Remove(1, 2);
            string BorderThickness = BorderThicknesss.ToString() + "px";
            string BackgroundColor = QuoteOPTemplateTextDesigGroupBy.BackgroundColor;
            if (QuoteOPTemplateTextDesigGroupBy.BackgroundColor.Length > 7)
            {
                BackgroundColor = QuoteOPTemplateTextDesigGroupBy.BackgroundColor.Remove(1, 2);
            }


            var alignment = ";text-align:" + QuoteOPTemplateTextDesigGroupBy.Alignment;// RightToLeft ? ";text-align:right" : ";text-align:" + QuoteOPTemplateTextDesigGroupBy.Alignment;


            string style = "";

            if (type == "Empty")
            {
                style = "style='" + "border-collapse: collapse; " + "table-layout: auto" + alignment + ";background-color:" + BackgroundColor + "'";
            }
            else
            {


                if (borderTypeCode == "ALL")
                {
                    style = "style='" + "border-top:" + BorderThickness + " solid " + BorderColor + ";border-bottom:" + BorderThickness + " solid " + BorderColor + ";border-left:" + BorderThickness + " solid " + BorderColor + ";border-right:" + BorderThickness + " solid " + BorderColor + ";table-layout: auto" +
                                ";margin-right:10px" + alignment + ";background-color:" + BackgroundColor + "'";
                }

                else if (borderTypeCode == "HORIZONTALLINES" || borderTypeCode == "VERTICALLINES")
                {


                    style = "style='" + "border-top:" + BorderThickness + " solid " + BorderColor + ";border-bottom:" + BorderThickness + " solid " + BorderColor + ";table-layout: auto" +
                      ";margin-right:10px" + alignment + ";background-color:" + BackgroundColor + "'";

                }
                else if (borderTypeCode == "BOX")
                {

                    style = "style='" + ";table-layout: auto" + ";margin-right:10px" + alignment + ";background-color:" + BackgroundColor + "'";

                }


                else if (borderTypeCode == "NONE")
                {
                    style = "style='" + "border-collapse: collapse; " + "table-layout: auto" + ";margin-right:10px" + alignment + ";background-color:" + BackgroundColor + "'";


                }
            }



            return style;
        }

        private void buildHeadercolumn(StringBuilder HtmlTemplate, QuoteOPTemplateSettingPM setting, QuoteOPTemplateTextDesignPM QuoteOPTemplateTextDesignPMHeader, QuoteOPTemplateTableDesignPM QuoteOPTemplateTableDesignPM, QuoteOPPM QuoteOPPM, string pricingSectionType, bool IsRightToLeft, List<QuoteOPTemplateTextCodePM> textcodes)
        {
            HtmlTemplate.Append("<tr style= 'height:auto; width:auto;vertical-align:central'>");



            IsShowlanguage = setting.ShowLocalLanguage;
            if (pricingSectionType == "PP")
            {
                GetCountHeader(setting, QuoteOPPM, "PP");

                if (setting.ShowChargeCodePackages)
                {
                    AppendHeaderColumn("CHARGECODEPACKAGES", HtmlTemplate, setting, QuoteOPTemplateTextDesignPMHeader, QuoteOPTemplateTableDesignPM, pricingSectionType, textcodes);
                }

                if (setting.ShowChargeNamePackages)
                {
                    AppendHeaderColumn("CHARGEPACKAGES", HtmlTemplate, setting, QuoteOPTemplateTextDesignPMHeader, QuoteOPTemplateTableDesignPM, pricingSectionType, textcodes);
                }

                if (setting.ShowUnitsPackages)
                {
                    AppendHeaderColumn("UNITSPACKAGES", HtmlTemplate, setting, QuoteOPTemplateTextDesignPMHeader, QuoteOPTemplateTableDesignPM, pricingSectionType, textcodes);
                }

                if (setting.ShowUnitPricePackages)
                {
                    AppendHeaderColumn("UNITPRICEPACKAGES", HtmlTemplate, setting, QuoteOPTemplateTextDesignPMHeader, QuoteOPTemplateTableDesignPM, pricingSectionType, textcodes);

                }

                if (setting.ShowMeasurementPackages)
                {
                    AppendHeaderColumn("MEASUREMENTPACKAGES", HtmlTemplate, setting, QuoteOPTemplateTextDesignPMHeader, QuoteOPTemplateTableDesignPM, pricingSectionType, textcodes);
                }

                if (setting.ShowSaleCurrencyColumnPackages)
                {
                    AppendHeaderColumn("TOTALPACKAGES", HtmlTemplate, setting, QuoteOPTemplateTextDesignPMHeader, QuoteOPTemplateTableDesignPM, pricingSectionType, textcodes);
                }

                if (setting.ShowLocalCurrencyColumnPackages)
                {
                    AppendHeaderColumn("LOCALAMOUNTPACKAGES", HtmlTemplate, setting, QuoteOPTemplateTextDesignPMHeader, QuoteOPTemplateTableDesignPM, pricingSectionType, textcodes);
                }

                if (setting.ShowChargeDescriptionPackages)
                {
                    AppendHeaderColumn("CHARGEDESCRIPTIONPACKAGES", HtmlTemplate, setting, QuoteOPTemplateTextDesignPMHeader, QuoteOPTemplateTableDesignPM, pricingSectionType, textcodes);

                }

                if (setting.ShowChargeNotePackages)
                {
                    AppendHeaderColumn("CHARGENOTEPACKAGES", HtmlTemplate, setting, QuoteOPTemplateTextDesignPMHeader, QuoteOPTemplateTableDesignPM, pricingSectionType, textcodes);
                }


                if (setting.ShowSaleMaxMinAmountPackages)
                {
                    AppendHeaderColumn("SALEMINMAXPACKAGES", HtmlTemplate, setting, QuoteOPTemplateTextDesignPMHeader, QuoteOPTemplateTableDesignPM, pricingSectionType, textcodes);
                }

                if (setting.ShowRegionalTAXPackages)
                {
                    AppendHeaderColumn("ISREGIONALTAXPACKAGES", HtmlTemplate, setting, QuoteOPTemplateTextDesignPMHeader, QuoteOPTemplateTableDesignPM, pricingSectionType, textcodes);
                }

                if (QuoteOPPM.IsChargesByVAT)
                {
                    if (setting.ShowVATTypePackages)
                    {
                        AppendHeaderColumn("VATTYPEPACKAGES", HtmlTemplate, setting, QuoteOPTemplateTextDesignPMHeader, QuoteOPTemplateTableDesignPM, pricingSectionType, textcodes);
                    }
                    if (setting.ShowVATPercentagePackages)
                    {
                        AppendHeaderColumn("VATPERCENTAGEPACKAGES", HtmlTemplate, setting, QuoteOPTemplateTextDesignPMHeader, QuoteOPTemplateTableDesignPM, pricingSectionType, textcodes);
                    }
                }

                HtmlTemplate.Append("</tr>");
            }
            else if (pricingSectionType == "PC")
            {
                GetCountHeader(setting, QuoteOPPM, "PC");
                if (setting.ShowChargeCodeContainers)
                {
                    AppendHeaderColumn("CHARGECODECONTAINERS", HtmlTemplate, setting, QuoteOPTemplateTextDesignPMHeader, QuoteOPTemplateTableDesignPM, pricingSectionType, textcodes);

                }

                if (setting.ShowChargeNameContainers)
                {
                    AppendHeaderColumn("CHARGECONTAINERS", HtmlTemplate, setting, QuoteOPTemplateTextDesignPMHeader, QuoteOPTemplateTableDesignPM, pricingSectionType, textcodes);

                }

                if (setting.ShowMeasurementContainers)
                {
                    AppendHeaderColumn("MEASUREMENTCONTAINERS", HtmlTemplate, setting, QuoteOPTemplateTextDesignPMHeader, QuoteOPTemplateTableDesignPM, pricingSectionType, textcodes);
                }

                if (setting.ShowFixedPriceContainers)
                {
                    if (ViewFixedPrice)
                    {
                        AppendHeaderColumn("FIXEDPRICECONTAINERS", HtmlTemplate, setting, QuoteOPTemplateTextDesignPMHeader, QuoteOPTemplateTableDesignPM, pricingSectionType, textcodes);
                    }
                }

                //=============================== islam ==================================
                if (setting.ShowPriceByContainerColumn)
                {
                    PackageType packageType;
                    if (QuoteOPPM.PackageType1Id != null)
                    {
                        packageType = PackageTypeRepository.GetSinglePackageType(QuoteOPPM.PackageType1Id, QuoteOPPM.Tenant, true);
                        string containerTypePrintAs = packageType.PrintAs;
                        string Name = QuoteOPPM.PackageType1Quantity + " x " + containerTypePrintAs;
                        HtmlTemplate.Append(BuildTableColumn(Name, QuoteOPTemplateTextDesignPMHeader, QuoteOPTemplateTableDesignPM, "Header", null, setting.RightToLeft, true));
                    }

                    if (QuoteOPPM.PackageType2Id != null)
                    {
                        packageType = PackageTypeRepository.GetSinglePackageType(QuoteOPPM.PackageType2Id, QuoteOPPM.Tenant, true);
                        string containerTypePrintAs = packageType.PrintAs;
                        string Name = QuoteOPPM.PackageType2Quantity + " x " + containerTypePrintAs;
                        HtmlTemplate.Append(BuildTableColumn(Name, QuoteOPTemplateTextDesignPMHeader, QuoteOPTemplateTableDesignPM, "Header", null, setting.RightToLeft, true));
                    }

                    if (QuoteOPPM.PackageType3Id != null)
                    {
                        packageType = PackageTypeRepository.GetSinglePackageType(QuoteOPPM.PackageType3Id, QuoteOPPM.Tenant, true);
                        string containerTypePrintAs = packageType.PrintAs;
                        string Name = QuoteOPPM.PackageType3Quantity + " x " + containerTypePrintAs;
                        HtmlTemplate.Append(BuildTableColumn(Name, QuoteOPTemplateTextDesignPMHeader, QuoteOPTemplateTableDesignPM, "Header", null, setting.RightToLeft, true));
                    }

                    if (QuoteOPPM.PackageType4Id != null)
                    {
                        packageType = PackageTypeRepository.GetSinglePackageType(QuoteOPPM.PackageType4Id, QuoteOPPM.Tenant, true);
                        string containerTypePrintAs = packageType.PrintAs;
                        string Name = QuoteOPPM.PackageType4Quantity + " x " + containerTypePrintAs;
                        HtmlTemplate.Append(BuildTableColumn(Name, QuoteOPTemplateTextDesignPMHeader, QuoteOPTemplateTableDesignPM, "Header", null, setting.RightToLeft, true));
                    }

                    if (QuoteOPPM.PackageType5Id != null)
                    {
                        packageType = PackageTypeRepository.GetSinglePackageType(QuoteOPPM.PackageType5Id, QuoteOPPM.Tenant, true);
                        string containerTypePrintAs = packageType.PrintAs;
                        string Name = QuoteOPPM.PackageType5Quantity + " x " + containerTypePrintAs;
                        HtmlTemplate.Append(BuildTableColumn(Name, QuoteOPTemplateTextDesignPMHeader, QuoteOPTemplateTableDesignPM, "Header", null, setting.RightToLeft, true));
                    }
                }

                if (setting.ShowSaleCurrencyColumnContainers)
                {
                    AppendHeaderColumn("TOTALCONTAINERS", HtmlTemplate, setting, QuoteOPTemplateTextDesignPMHeader, QuoteOPTemplateTableDesignPM, pricingSectionType, textcodes);
                }

                if (setting.ShowLocalCurrencyColumnContainers)
                {
                    AppendHeaderColumn("LOCALAMOUNTCONTAINERS", HtmlTemplate, setting, QuoteOPTemplateTextDesignPMHeader, QuoteOPTemplateTableDesignPM, pricingSectionType, textcodes);
                }

                if (setting.ShowChargeDescriptionContainers)
                {
                    AppendHeaderColumn("CHARGEDESCRIPTIONCONTAINERS", HtmlTemplate, setting, QuoteOPTemplateTextDesignPMHeader, QuoteOPTemplateTableDesignPM, pricingSectionType, textcodes);
                }

                if (setting.ShowChargeNoteContainers)
                {
                    AppendHeaderColumn("CHARGENOTECONTAINERS", HtmlTemplate, setting, QuoteOPTemplateTextDesignPMHeader, QuoteOPTemplateTableDesignPM, pricingSectionType, textcodes);

                }

                if (setting.ShowSaleMaxMinAmountContainers)
                {
                    AppendHeaderColumn("SALEMINMAXCONTAINERS", HtmlTemplate, setting, QuoteOPTemplateTextDesignPMHeader, QuoteOPTemplateTableDesignPM, pricingSectionType, textcodes);
                }

                if (setting.ShowRegionalTAXContainers)
                {
                    AppendHeaderColumn("ISREGIONALTAXCONTAINERS", HtmlTemplate, setting, QuoteOPTemplateTextDesignPMHeader, QuoteOPTemplateTableDesignPM, pricingSectionType, textcodes);
                }

                if (QuoteOPPM.IsChargesByVAT)
                {
                    if (setting.ShowVATTypeContainers)
                    {
                        AppendHeaderColumn("VATTYPECONTAINERS", HtmlTemplate, setting, QuoteOPTemplateTextDesignPMHeader, QuoteOPTemplateTableDesignPM, pricingSectionType, textcodes);
                    }
                    if (setting.ShowVATPercentageContainers)
                    {
                        AppendHeaderColumn("VATPERCENTAGECONTAINERS", HtmlTemplate, setting, QuoteOPTemplateTextDesignPMHeader, QuoteOPTemplateTableDesignPM, pricingSectionType, textcodes);
                    }
                }

                //===========================================================================

                HtmlTemplate.Append("</tr>");
            }


        }

        private void AppendHeaderColumn(string textCode, StringBuilder HtmlTemplate, QuoteOPTemplateSettingPM setting, QuoteOPTemplateTextDesignPM QuoteOPTemplateTextDesignPMHeader, QuoteOPTemplateTableDesignPM QuoteOPTemplateTableDesignPM, string pricingSectionType, List<QuoteOPTemplateTextCodePM> textcodes)
        {
            string Name = GetNameColum(textCode, textcodes, pricingSectionType);
            int width = GetWidthColumHeaderForPricingTable(textCode, pricingSectionType, setting);
            HtmlTemplate.Append(BuildTableColumn(Name, QuoteOPTemplateTextDesignPMHeader, QuoteOPTemplateTableDesignPM, "Header" , (width.ToString() +"%"), setting.RightToLeft));
        }

        private int GetWidthColumHeaderForPricingTable(string textCode, string pricingSectionType, QuoteOPTemplateSettingPM setting)
        {
          
            int coulmnCount = TdCount;
            bool isShowShowChargeDescription = pricingSectionType == "PP" ? setting.ShowChargeDescriptionPackages : setting.ShowChargeDescriptionContainers;
            bool isShowShowChargeName = pricingSectionType == "PP" ? setting.ShowChargeNamePackages : setting.ShowChargeNameContainers;
            if (isShowShowChargeDescription) coulmnCount += 1;
            if (isShowShowChargeName) coulmnCount += 1;
            int result = (100 / coulmnCount);
            if (textCode == "CHARGEPACKAGES" || textCode == "CHARGEDESCRIPTIONPACKAGES" || textCode == "CHARGECONTAINERS" || textCode == "CHARGEDESCRIPTIONCONTAINERS")
            {
                result = result * 2;
            }

            return result;

        }

        public void BuildPricingTitle(StringBuilder HtmlTemplate, QuoteOPTemplateTextDesignPM QuoteOPTemplateTextDesignPMPricingTitle, string typepricing, List<QuoteOPTemplateTextCodePM> textcodes, bool RightToLeft)
        {
            string FieldName = "";
            string type = RightToLeft ? "PricingTableTitle" : "PricingTitle";
            string stylePricingtitle = GetSpanRowStyle(QuoteOPTemplateTextDesignPMPricingTitle, type, "100%");
            if (typepricing == "PP")
            {
                FieldName = GetNameColum("PRICINGPACKAGES", textcodes, "PP");
            }
            else if (typepricing == "PC")
            {
                FieldName = GetNameColum("PRICINGCONTAINERS", textcodes, "PC");
            }
            else if (typepricing == "TotalPerContainers")
            {
                FieldName = GetNameColum("TOTALPERCONTAINERS", textcodes, "TotalPerContainers");
            }

            if (QuoteOPTemplateTextDesignPMPricingTitle.Italic)
            {
                if (RightToLeft)
                {
                    HtmlTemplate.Append("<div   dir='rtl' " + stylePricingtitle + ">" + "<i " + stylePricingtitle + " >" + FieldName + "</i> " + "</div>");
                }
                else
                {
                    HtmlTemplate.Append("<div " + stylePricingtitle + ">" + "<i " + stylePricingtitle + " >" + FieldName + "</i> " + "</div>");
                }
            }

            else
            {
                if (RightToLeft)
                {
                    HtmlTemplate.Append("<div  dir='rtl'" + stylePricingtitle + ">" + FieldName + "</div>");
                }
                else
                {
                    HtmlTemplate.Append("<div " + stylePricingtitle + ">" + FieldName + "</div>");
                }
            }

            //HtmlTemplate.Append("<div  style='height:10px;'>" + " &nbsp;  &nbsp; &nbsp; &nbsp; &nbsp;  &nbsp;   &nbsp;" + "</div>");
        }

        private string GetStyleTableHeaderFooter(QuoteOPTemplateTableDesignPM tableDesign, string height)
        {
            string BorderColor = tableDesign.BorderColor;
            if (BorderColor.Length > 7) BorderColor = BorderColor.Remove(1, 2);
            string heightText = !string.IsNullOrEmpty(height) ? ";height:" + height : "";
            string borderThickness = tableDesign.BorderThickness.ToString() + "px";
            string style = "";

            if (tableDesign.BorderTypeCode != "NONE")
            {
                style = "style='" + "border-top:" + borderThickness + " solid " + BorderColor + ";border-bottom:" + borderThickness + " solid " + BorderColor + ";border-left:" + borderThickness + " solid " + BorderColor + ";border-right:" + borderThickness + " solid " + BorderColor + ";table-layout: auto" + ";border-collapse: collapse" +
                  ";width:100%" + ";margin-right:10px" + heightText + "'";
            }
            else
            {
                style = "style='" + "border-top:" + "0" + " none " + "#ffffff" + ";border-bottom:" + "0" + " none " + "#ffffff" + ";border-left:" + "0" + " none " + "#ffffff" + ";border-right:" + "0" + " none " + "#ffffff" + ";table-layout: auto" + ";border-collapse: collapse" +
                  ";width:100%" + ";margin-right:10px" + heightText + "'";

            }
            return style;
        }

        private string GetStyleTable(QuoteOPTemplateTableDesignPM QuoteOPTemplateTableDesignPM)
        {
            string BorderColor = QuoteOPTemplateTableDesignPM.BorderColor;
            if (BorderColor.Length > 7)
            {
                BorderColor = QuoteOPTemplateTableDesignPM.BorderColor.Remove(1, 2);
            }

            string BorderThickness = QuoteOPTemplateTableDesignPM.BorderThickness.ToString() + "px";
            string style = "";
            style = "style='" + "border-top:" + BorderThickness + " solid " + BorderColor + ";border-bottom:" + BorderThickness + " solid " + BorderColor + ";border-left:" + BorderThickness + " solid " + BorderColor + ";border-right:" + BorderThickness + " solid " + BorderColor + ";table-layout: auto" + ";border-collapse: collapse" +
             "'";
            return style;
        }

        int ShowSaleCurrencyColumnColumnPosition = 0;
       
        private void GetCountHeader(QuoteOPTemplateSettingPM setting, QuoteOPPM QuoteOPPM, string sectionType)
        {

            TdCount = 0;
            ShowSaleCurrencyColumnColumnPosition = 0;
            if (sectionType == "PP" && setting.ShowPricesTablePackages)
            {
                if (setting.ShowChargeCodePackages) ++TdCount;
                if (setting.ShowChargeNamePackages) ++TdCount;
                if (setting.ShowUnitsPackages) ++TdCount;
                if (setting.ShowUnitPricePackages) ++TdCount;
                if (setting.ShowMeasurementPackages) ++TdCount;
                if (setting.ShowSaleCurrencyColumnPackages)
                {
                    ++TdCount;
                    ShowSaleCurrencyColumnColumnPosition = TdCount;
                }

                if (setting.ShowLocalCurrencyColumnPackages) ++TdCount;
                if (setting.ShowChargeDescriptionPackages) ++TdCount;
                if (setting.ShowChargeNotePackages) ++TdCount;
                if (setting.ShowSaleMaxMinAmountPackages) ++TdCount;
                if (setting.ShowRegionalTAXPackages) ++TdCount;
                if (QuoteOPPM.IsChargesByVAT)
                {
                    if (setting.ShowVATTypePackages) ++TdCount;
                    if (setting.ShowVATPercentagePackages) ++TdCount;
                }


            }
            else if (sectionType == "PC" && setting.ShowPricesTableContainers)
            {
                TdCount = 0;
                if (setting.ShowChargeCodeContainers) ++TdCount;
                if (setting.ShowChargeNameContainers) ++TdCount;
                if (setting.ShowMeasurementContainers) ++TdCount;
                if (setting.ShowFixedPriceContainers)
                {
                    if (ViewFixedPrice) ++TdCount;
                }

                if (setting.ShowPriceByContainerColumn)
                {
                    if (QuoteOPPM.PackageType1Id != null) ++TdCount;
                    if (QuoteOPPM.PackageType2Id != null) ++TdCount;
                    if (QuoteOPPM.PackageType3Id != null) ++TdCount;
                    if (QuoteOPPM.PackageType4Id != null) ++TdCount;
                    if (QuoteOPPM.PackageType5Id != null) ++TdCount;

                }

                if (setting.ShowSaleCurrencyColumnContainers)
                {
                    ++TdCount;
                    ShowSaleCurrencyColumnColumnPosition = TdCount;
                }
                if (setting.ShowLocalCurrencyColumnContainers) ++TdCount;
   
                if (setting.ShowChargeDescriptionContainers) ++TdCount;
                if (setting.ShowChargeNoteContainers) ++TdCount;
                if (setting.ShowSaleMaxMinAmountContainers) ++TdCount;
                if (setting.ShowRegionalTAXContainers) ++TdCount;

                if (QuoteOPPM.IsChargesByVAT)
                {
                    if (setting.ShowVATTypeContainers) ++TdCount;
                    if (setting.ShowVATPercentageContainers) ++TdCount;
                }



            }
        }

        private string BuildTotalInSale(string columncontent, QuoteOPTemplateTextDesignPM headerDesign, bool IsHiddenTotal, bool leftToRight = false)
        {
            string result = "";
            string style = "";
            string type = IsHiddenTotal ? "TotalHidden" : "Total";

            style = GetSpanRowStyle(headerDesign, type);

            string styleleftToRight = leftToRight ? "dir='LTR' " : "";
            if (headerDesign.Italic)
            {
                result = "<i " + style + " >" + "<span " + styleleftToRight + style + ">" + columncontent + "</span>" + "</i>";
            }
            else
            {
                result = "<span  " + style + styleleftToRight + ">" + columncontent + "</span>";
            }



            return result;
        }

        private string BuildTableColumn(string columncontent, QuoteOPTemplateTextDesignPM headerDesign, QuoteOPTemplateTableDesignPM tableDesign, string CodeTypeTd, string width, bool bodyRightToLeft, bool headeLeftToRight = false)
        {
            string styleAlgiment = "";
            string style = GetStyleRowTable(headerDesign, tableDesign, width);
            string stylespan = GetSpanRowStyle(headerDesign, "");
            string result = "";

            var alignment = ";text-align:" + headerDesign.Alignment;//CodeTypeTd == "FieldPrice" ? ";text-align:right" : ";text-align:" + headerDesign.Alignment;
            if (CodeTypeTd == "Field") alignment = bodyRightToLeft ? ";text-align:right" : ";text-align:left";
            styleAlgiment = "style='" + "height:auto" + ";width:auto" + alignment + " '";

            string headerLeftToRight = headeLeftToRight ? "dir='LTR' " : "";


            if (headerDesign.Italic)
            {
                result = "<td " + style + ">" + "<i>" + "<div  " + styleAlgiment + ">" + "<span " + headerLeftToRight + stylespan + ">" + columncontent + "</span>" + "</div>" + "</i>" + "</td>";
            }
            else
            {
                result = "<td " + style + ">" + "<div  " + styleAlgiment + ">" + "<span " + headerLeftToRight + stylespan + ">" + columncontent + "</span>" + "</div>" + "</td>";
            }

            return result;
        }


        private string GetSpanRowStyle(QuoteOPTemplateTextDesignPM Design, string type, string width = null, double per = 1)
        {

            string alignment = Design.Alignment;//type == "Lable" ? "right" : Design.Alignment;

            if (type == "PricingTableTitle") alignment = "";

            string fontSize = GetFontSizeInPixles(Design.FontSize, per).ToString() + "px";
            string textColor = Design.TextColor;
            if (Design.TextColor.Length > 7) textColor = Design.TextColor.Remove(1, 2);
            string backgroundColor = Design.BackgroundColor;
            if (Design.BackgroundColor.Length > 7) backgroundColor = Design.BackgroundColor.Remove(1, 2);

            string widht = !string.IsNullOrEmpty(width) ? (";width:" + width + ";max-width:" + width + ";word-wrap: break-word") : ";width:auto;height:auto";

            string unDerLine = Design.UnDerLine ? ";text-decoration: underline" : "";

            string visible = type == "TotalHidden" ? ";visibility:hidden" : "";
            string margin = type == "" ? ";margin-top:35px" : "";



            string style = "style='" + "font-weight:" + Design.FontWeight + ";font-family:" + Design.FontFamily +
                ";font-size:" + fontSize + ";color:" + textColor + ";vertical-align:central" + unDerLine + ";text-align:" + alignment + widht + visible + margin + ";background-color:" + backgroundColor + " '";

            if (string.IsNullOrEmpty(alignment))
            {
                style = "style='" + "font-weight:" + Design.FontWeight + ";font-family:" + Design.FontFamily +
                 ";font-size:" + fontSize + ";color:" + textColor + ";vertical-align:central" + unDerLine + widht + visible + margin + ";background-color:" + backgroundColor + " '";

            }

            return style;
        }

        private string GetStyleRowTable(QuoteOPTemplateTextDesignPM design, QuoteOPTemplateTableDesignPM QuoteOPTemplateTableDesignPM, string width)
        {

            string BorderColor = QuoteOPTemplateTableDesignPM.BorderColor;
            if (BorderColor.Length > 7)
            {
                BorderColor = QuoteOPTemplateTableDesignPM.BorderColor.Remove(1, 2);
            }

            string style = "";
            string BorderThickness = QuoteOPTemplateTableDesignPM.BorderThickness.ToString() + "px";
            string BackgroundColor = design.BackgroundColor;
            if (BackgroundColor.Length > 7)
            {
                BackgroundColor = design.BackgroundColor.Remove(1, 2);
            }

            if (QuoteOPTemplateTableDesignPM.BorderTypeCode == "ALL")
            {
                style = "style='" + "border-top:" + BorderThickness + " solid " + BorderColor + ";border-bottom:" + BorderThickness + " solid " + BorderColor + ";border-left:" + BorderThickness + " solid " + BorderColor + ";border-right:" + BorderThickness + " solid " + BorderColor + ";table-layout: auto"
                  + "; text-indent:4px" + "; vertical-align:central" + ";background-color:" + BackgroundColor + "'";
            }

            else if (QuoteOPTemplateTableDesignPM.BorderTypeCode == "HORIZONTALLINES")
            {
                style = "style='" + "border-top:" + BorderThickness + " solid " + BorderColor + ";border-bottom:" + BorderThickness + " solid " + BorderColor + ";table-layout: auto"
               + "; text-indent:4px" + "; vertical-align:central" + ";background-color:" + BackgroundColor + "'";
            }
            else if (QuoteOPTemplateTableDesignPM.BorderTypeCode == "BOX")
            {
                style = "style='" + ";table-layout: auto"
               + ";text-indent:4px" + "; vertical-align:central" + ";background-color:" + BackgroundColor + "'";
            }
            else if (QuoteOPTemplateTableDesignPM.BorderTypeCode == "VERTICALLINES")
            {
                style = "style='" + "border-left:" + BorderThickness + " solid " + BorderColor + ";border-right:" + BorderThickness + " solid " + BorderColor + ";table-layout: auto"
                                    + "; text-indent:4px" + "; vertical-align:central" + ";background-color:" + BackgroundColor + "'";
            }

            else if (QuoteOPTemplateTableDesignPM.BorderTypeCode == "NONE")
            {
                style = "style='" + "border-collapse: collapse; " + "table-layout: auto"
                                             + "; text-indent:4px" + "; vertical-align:central" + ";background-color:" + BackgroundColor + "'";
            }

            if (width != null && width != "Auto")
            {
                if (!string.IsNullOrEmpty(style))
                {
                    int xx = style.Length;
                    style = style.Remove(xx - 1);
                    style += ";width:" + width + "'";
                }
            }

            return style;
        }

        private string GetTextStyleSpanhidden(QuoteOPTemplateTextDesignPM headerDesign)
        {

            string style = "style='" + ";vertical-align:central" + ";height:auto" + ";width:auto" + ";margin-top:35px" + ";visibility:hidden" + " '";
            return style;
        }

        private string GetNameColum(string TextCode, List<QuoteOPTemplateTextCodePM> textcodes, string typeSetting)
        {
            NameTextCode = "";
            List<QuoteOPTemplateTextCodePM> qoutetemplatetextcodeList = new List<QuoteOPTemplateTextCodePM>();
            if (typeSetting == "PP") qoutetemplatetextcodeList = textcodes.Where(d => d.Area == "Packages").ToList();
            else if (typeSetting == "PC") qoutetemplatetextcodeList = textcodes.Where(d => d.Area == "Containers").ToList();
            else if (typeSetting == "QD") qoutetemplatetextcodeList = textcodes.Where(d => d.Area == "QuoteDetails").ToList();

            else if (typeSetting == "QH") qoutetemplatetextcodeList = textcodes.Where(d => d.Area == "QuoteHeader").ToList();
            else if (typeSetting == "TotalPerContainers") qoutetemplatetextcodeList = textcodes.Where(d => d.Area == "TotalPerContainers").ToList();
            foreach (QuoteOPTemplateTextCodePM qoutetemplatetextcode in qoutetemplatetextcodeList)
            {
                if (qoutetemplatetextcode.TextCode == TextCode)
                {
                    if (IsShowlanguage) NameTextCode = qoutetemplatetextcode.LocalName;
                    else NameTextCode = qoutetemplatetextcode.EnglishName;

                    break;
                }
            }

            return NameTextCode;
        }

        
        private string AddTableRows(OPPricingTableRowDetailsArgs OPPricingTableRowDetailsArgs)
        {
            string width = GetWidthColumHeaderForPricingTable(OPPricingTableRowDetailsArgs.FieldCode , OPPricingTableRowDetailsArgs.PricingSectionType, OPPricingTableRowDetailsArgs.QuoteOPTemplateSettingPM).ToString() + "%"; ;
            return BuildTableColumn(OPPricingTableRowDetailsArgs.Value, OPPricingTableRowDetailsArgs.HeaderDesign, OPPricingTableRowDetailsArgs.TableDesign, OPPricingTableRowDetailsArgs.RowDataType, width, OPPricingTableRowDetailsArgs.QuoteOPTemplateSettingPM.RightToLeft, OPPricingTableRowDetailsArgs.RowDataType == "FieldPrice" ? true:false);
        }

        private void BuildTableRows(List<QuoteOPSaleChargePM> quoteSaleCharges, StringBuilder HtmlTemplate, QuoteOPTemplateTextDesignPM QuoteOPTemplateTextDesignLines, QuoteOPTemplateTableDesignPM QuoteOPTemplateTableDesignPM, QuoteOPTemplateBuildArges QuoteOPTemplateBuildArges )
        {
            QuoteOPPM QuoteOPPM = QuoteOPTemplateBuildArges.QuoteOPPM;
            string pricingSectionType = QuoteOPTemplateBuildArges.SectionTypeCode;
            QuoteOPTemplateSettingPM setting = QuoteOPTemplateBuildArges.QuoteOPTemplateSettingPM;
            string translateInclueLable = GetTranslateInclueLable(QuoteOPTemplateBuildArges.QuoteOPTemplateTextCodePMLists, pricingSectionType);

            OPPricingTableRowDetailsArgs OPPricingTableRowDetailsArgs = new OPPricingTableRowDetailsArgs() {QuoteOPTemplateSettingPM = setting, HeaderDesign = QuoteOPTemplateTextDesignLines, TableDesign = QuoteOPTemplateTableDesignPM , PricingSectionType = pricingSectionType};


            int i = -1;
            foreach (QuoteOPSaleChargePM chargePM in quoteSaleCharges)
            {
                bool included = (chargePM.IsAllIN != null && chargePM.IsAllIN.ToLower() == "true" ? true : false);
                ++i;
                HtmlTemplate.Append("<tr style= 'height:auto; width:auto;vertical-align:central'>");

                if (pricingSectionType == "PP")
                {
                    if (setting.ShowChargeCodePackages)
                    {

                        HtmlTemplate.Append(AddTableRows(new OPPricingTableRowDetailsArgs() { Value = chargePM.ChargesTypeCode, RowDataType = "Field", QuoteOPTemplateSettingPM = setting, HeaderDesign = QuoteOPTemplateTextDesignLines, TableDesign = QuoteOPTemplateTableDesignPM, PricingSectionType = pricingSectionType }));

                    }

                        if (setting.ShowChargeNamePackages)
                    {
                        string text = setting.ShowLocalLanguage && chargePM.ChargesTypeLocalName != null ? chargePM.ChargesTypeLocalName : chargePM.ChargesTypeName;
                        HtmlTemplate.Append(AddTableRows(new OPPricingTableRowDetailsArgs() {FieldCode = "CHARGEPACKAGES", Value = text, RowDataType = "Field" , QuoteOPTemplateSettingPM = setting, HeaderDesign = QuoteOPTemplateTextDesignLines, TableDesign = QuoteOPTemplateTableDesignPM, PricingSectionType = pricingSectionType }));
                    }


                    if (setting.ShowUnitsPackages)
                    {
                        string text = " ";
                        if (chargePM.SaleQuantity != null)
                        {
                            double value = (double)chargePM.SaleQuantity;
                            text = value.ToString("N"); // 1,234.512
                        }

                        if (included) text = translateInclueLable;
                        HtmlTemplate.Append(AddTableRows(new OPPricingTableRowDetailsArgs() { Value = text, RowDataType = "FieldPrice", QuoteOPTemplateSettingPM = setting, HeaderDesign = QuoteOPTemplateTextDesignLines, TableDesign = QuoteOPTemplateTableDesignPM, PricingSectionType = pricingSectionType }));

                    }

                    if (setting.ShowUnitPricePackages)
                    {
                        string saleUnitPriceValues = GetUnitPricePackagesValue(QuoteOPTemplateBuildArges, chargePM, included);

                        HtmlTemplate.Append(AddTableRows(new OPPricingTableRowDetailsArgs() {Value = saleUnitPriceValues, RowDataType = "FieldPrice", QuoteOPTemplateSettingPM = setting, HeaderDesign = QuoteOPTemplateTextDesignLines, TableDesign = QuoteOPTemplateTableDesignPM, PricingSectionType = pricingSectionType }));


                    }

                    if (setting.ShowMeasurementPackages)
                    {
                        var text = setting.RightToLeft ? chargePM.SaleMeasurementLocalName : chargePM.SaleMeasurementShortName;
                        HtmlTemplate.Append(AddTableRows(new OPPricingTableRowDetailsArgs() { Value = text, RowDataType = "FieldP", QuoteOPTemplateSettingPM = setting, HeaderDesign = QuoteOPTemplateTextDesignLines, TableDesign = QuoteOPTemplateTableDesignPM, PricingSectionType = pricingSectionType }));

                    }



                    if (setting.ShowSaleCurrencyColumnPackages)
                    {
                        string text = " ";
                        if (chargePM.SaleTotalAmount != null)
                        {
                            double value = (double)chargePM.SaleTotalAmount;
                            text = value.ToString("N") + " " + GetChargeCurrencyCode(QuoteOPPM, chargePM);
                        }
                        if (included) text = translateInclueLable;
                        HtmlTemplate.Append(AddTableRows(new OPPricingTableRowDetailsArgs() { Value = text, RowDataType = "FieldPrice", QuoteOPTemplateSettingPM = setting, HeaderDesign = QuoteOPTemplateTextDesignLines, TableDesign = QuoteOPTemplateTableDesignPM, PricingSectionType = pricingSectionType }));



                    }

                    if (setting.ShowLocalCurrencyColumnPackages)
                    {
                        string text = " ";
                        if (chargePM.SaleTotalAmountLocal != null)
                        {
                            double value = (double)chargePM.SaleTotalAmountLocal;
                            text = value.ToString("N") + " " + LocalCurrencyCode;
                        }
                        if (included) text = translateInclueLable;
                        HtmlTemplate.Append(AddTableRows(new OPPricingTableRowDetailsArgs() { Value = text, RowDataType = "FieldPrice", QuoteOPTemplateSettingPM = setting, HeaderDesign = QuoteOPTemplateTextDesignLines, TableDesign = QuoteOPTemplateTableDesignPM, PricingSectionType = pricingSectionType }));

                    }

                        if (setting.ShowChargeDescriptionPackages)
                    {
                        HtmlTemplate.Append(AddTableRows(new OPPricingTableRowDetailsArgs() { FieldCode = "CHARGEDESCRIPTIONPACKAGES", Value = chargePM.ChargesTypeDescription, RowDataType = "Field", QuoteOPTemplateSettingPM = setting, HeaderDesign = QuoteOPTemplateTextDesignLines, TableDesign = QuoteOPTemplateTableDesignPM, PricingSectionType = pricingSectionType }));

                    }

                    if (setting.ShowChargeNotePackages)
                    {
                        HtmlTemplate.Append(AddTableRows(new OPPricingTableRowDetailsArgs() { Value = chargePM.Notes, RowDataType = "Field", QuoteOPTemplateSettingPM = setting, HeaderDesign = QuoteOPTemplateTextDesignLines, TableDesign = QuoteOPTemplateTableDesignPM, PricingSectionType = pricingSectionType }));


                    }
                    if (setting.ShowSaleMaxMinAmountPackages)
                    {
                        string text = included ? translateInclueLable : GetSaleMaxMinAmountValue(chargePM);
                        HtmlTemplate.Append(AddTableRows(new OPPricingTableRowDetailsArgs() { Value = text, RowDataType = "Field", QuoteOPTemplateSettingPM = setting, HeaderDesign = QuoteOPTemplateTextDesignLines, TableDesign = QuoteOPTemplateTableDesignPM, PricingSectionType = pricingSectionType }));

                    }
                    if (setting.ShowRegionalTAXPackages)
                    {
                        string isRegionalTax = chargePM.IsRegionalTax ? "Yes" : "No";
                        HtmlTemplate.Append(AddTableRows(new OPPricingTableRowDetailsArgs() { Value = isRegionalTax, RowDataType = "Field", QuoteOPTemplateSettingPM = setting, HeaderDesign = QuoteOPTemplateTextDesignLines, TableDesign = QuoteOPTemplateTableDesignPM, PricingSectionType = pricingSectionType }));

                    }

                    if (QuoteOPPM.IsChargesByVAT)
                    {
                        if (setting.ShowVATTypePackages)
                        {
                            HtmlTemplate.Append(AddTableRows(new OPPricingTableRowDetailsArgs() { FieldCode = "VATTYPEPACKAGES", Value = chargePM.VatTypeName, RowDataType = "Field", QuoteOPTemplateSettingPM = setting, HeaderDesign = QuoteOPTemplateTextDesignLines, TableDesign = QuoteOPTemplateTableDesignPM, PricingSectionType = pricingSectionType }));
                        }
                        if (setting.ShowVATPercentagePackages)
                        {
                            string value = chargePM.VatPercentage != null ? (chargePM.VatPercentage + "%") : "";
                            HtmlTemplate.Append(AddTableRows(new OPPricingTableRowDetailsArgs() { FieldCode = "VATTYPEPACKAGES", Value = value, RowDataType = "Field", QuoteOPTemplateSettingPM = setting, HeaderDesign = QuoteOPTemplateTextDesignLines, TableDesign = QuoteOPTemplateTableDesignPM, PricingSectionType = pricingSectionType }));
                        }
                    }


                }
                else if (pricingSectionType == "PC")
                {
                    int row = 0;
               

                    if (setting.ShowChargeCodeContainers)
                    {
                        HtmlTemplate.Append(AddTableRows(new OPPricingTableRowDetailsArgs() { Value = chargePM.ChargesTypeCode, RowDataType = "Field", QuoteOPTemplateSettingPM = setting, HeaderDesign = QuoteOPTemplateTextDesignLines, TableDesign = QuoteOPTemplateTableDesignPM, PricingSectionType = pricingSectionType }));

                        row += 1;
                    }

                        if (setting.ShowChargeNameContainers)
                    {
                        if (setting.ShowLocalLanguage && chargePM.ChargesTypeLocalName != null)
                        {
                            HtmlTemplate.Append(AddTableRows(new OPPricingTableRowDetailsArgs() { FieldCode = "CHARGECONTAINERS", Value = chargePM.ChargesTypeLocalName, RowDataType = "Field", QuoteOPTemplateSettingPM = setting, HeaderDesign = QuoteOPTemplateTextDesignLines, TableDesign = QuoteOPTemplateTableDesignPM, PricingSectionType = pricingSectionType }));

                        }
                        else
                        {
                            HtmlTemplate.Append(AddTableRows(new OPPricingTableRowDetailsArgs() { Value = chargePM.ChargesTypeName, RowDataType = "Field", QuoteOPTemplateSettingPM = setting, HeaderDesign = QuoteOPTemplateTextDesignLines, TableDesign = QuoteOPTemplateTableDesignPM, PricingSectionType = pricingSectionType }));

                        }
                        row += 1;
                    }

                    if (setting.ShowMeasurementContainers)
                    {
                        var value = setting.RightToLeft ? chargePM.SaleMeasurementLocalName : chargePM.SaleMeasurementShortName;
                        HtmlTemplate.Append(AddTableRows(new OPPricingTableRowDetailsArgs() { Value = value, RowDataType = "Field", QuoteOPTemplateSettingPM = setting, HeaderDesign = QuoteOPTemplateTextDesignLines, TableDesign = QuoteOPTemplateTableDesignPM, PricingSectionType = pricingSectionType }));




                        row += 1;
                    }

                    if (setting.ShowFixedPriceContainers)
                    {
                        if (ViewFixedPrice)
                        {
                            string fixedPrice = " ";
                            if (chargePM.SaleMeasurementCode != "BCNT")
                            {
                                if (chargePM.SaleTotalAmount != null)
                                {
                                    double value = (double)chargePM.SaleTotalAmount;
                                    fixedPrice = value.ToString("N") + " " + GetChargeCurrencyCode(QuoteOPPM, chargePM);
                                }
                            }
                            if (included) fixedPrice = translateInclueLable;

                            HtmlTemplate.Append(AddTableRows(new OPPricingTableRowDetailsArgs() { Value = fixedPrice, RowDataType = "FieldPrice", QuoteOPTemplateSettingPM = setting, HeaderDesign = QuoteOPTemplateTextDesignLines, TableDesign = QuoteOPTemplateTableDesignPM, PricingSectionType = pricingSectionType }));

                            row += 1;
                        }

                    }

                    if (setting.ShowPriceByContainerColumn)
                    {
                        if (QuoteOPPM.PackageType1Id != null)
                        {
                            if (setting.ShowPriceByContainerColumn)
                            {
                                string saleContainerType1UnitPrice = "";
                                double value = 0;

                                row += 1;
                                if (chargePM.SaleContainerType1UnitPrice != null)
                                {
                                    value = (double)chargePM.SaleContainerType1UnitPrice;
                                    saleContainerType1UnitPrice = value.ToString("N") + " " + GetChargeCurrencyCode(QuoteOPPM, chargePM); ;
                                }

                                if(included) saleContainerType1UnitPrice = translateInclueLable;

                                HtmlTemplate.Append(AddTableRows(new OPPricingTableRowDetailsArgs() { Value = saleContainerType1UnitPrice, RowDataType = "FieldPrice", QuoteOPTemplateSettingPM = setting, HeaderDesign = QuoteOPTemplateTextDesignLines, TableDesign = QuoteOPTemplateTableDesignPM, PricingSectionType = pricingSectionType }));
                            }

                        }

                        if (QuoteOPPM.PackageType2Id != null)
                        {
                            if (setting.ShowPriceByContainerColumn)
                            {
                                string saleContainerType2UnitPrice = "";
                                double value = 0;
                                row += 1;
                                if (chargePM.SaleContainerType2UnitPrice != null)
                                {

                                    value = (double)chargePM.SaleContainerType2UnitPrice;
                                    saleContainerType2UnitPrice = value.ToString("N") + " " + GetChargeCurrencyCode(QuoteOPPM, chargePM);
                                }
                                if (included) saleContainerType2UnitPrice = translateInclueLable;

                                HtmlTemplate.Append(AddTableRows(new OPPricingTableRowDetailsArgs() { Value = saleContainerType2UnitPrice, RowDataType = "FieldPrice", QuoteOPTemplateSettingPM = setting, HeaderDesign = QuoteOPTemplateTextDesignLines, TableDesign = QuoteOPTemplateTableDesignPM, PricingSectionType = pricingSectionType }));

                            }
                        }

                        if (QuoteOPPM.PackageType3Id != null)
                        {
                            if (setting.ShowPriceByContainerColumn)
                            {
                                string saleContainerType3UnitPrice = "";
                                double value = 0;
                                row += 1;
                                if (chargePM.SaleContainerType3UnitPrice != null)
                                {
                                    value = (double)chargePM.SaleContainerType3UnitPrice;
                                    saleContainerType3UnitPrice = value.ToString("N") + " " + GetChargeCurrencyCode(QuoteOPPM, chargePM);

                                }
                                if (included) saleContainerType3UnitPrice = translateInclueLable;

                                HtmlTemplate.Append(AddTableRows(new OPPricingTableRowDetailsArgs() { Value = saleContainerType3UnitPrice, RowDataType = "FieldPrice", QuoteOPTemplateSettingPM = setting, HeaderDesign = QuoteOPTemplateTextDesignLines, TableDesign = QuoteOPTemplateTableDesignPM, PricingSectionType = pricingSectionType }));

                            }
                        }

                        if (QuoteOPPM.PackageType4Id != null)
                        {
                            if (setting.ShowPriceByContainerColumn)
                            {
                                string saleContainerType4UnitPrice = "";
                                double value = 0;
                                row += 1;
                                if (chargePM.SaleContainerType4UnitPrice != null)
                                {
                                    value = (double)chargePM.SaleContainerType4UnitPrice;
                                    saleContainerType4UnitPrice = value.ToString("N") + " " + GetChargeCurrencyCode(QuoteOPPM, chargePM);
                                }
                                if (included) saleContainerType4UnitPrice = translateInclueLable;
                                HtmlTemplate.Append(AddTableRows(new OPPricingTableRowDetailsArgs() { Value = saleContainerType4UnitPrice, RowDataType = "FieldPrice", QuoteOPTemplateSettingPM = setting, HeaderDesign = QuoteOPTemplateTextDesignLines, TableDesign = QuoteOPTemplateTableDesignPM, PricingSectionType = pricingSectionType }));

                            }
                        }

                        if (QuoteOPPM.PackageType5Id != null)
                        {
                            if (setting.ShowPriceByContainerColumn)
                            {
                                string saleContainerType5UnitPrice = "";
                                double value = 0;
                                row += 1;
                                if (chargePM.SaleContainerType5UnitPrice != null)
                                {
                                    value = (double)chargePM.SaleContainerType5UnitPrice;
                                    saleContainerType5UnitPrice = value.ToString("N") + " " + GetChargeCurrencyCode(QuoteOPPM, chargePM);
                                }
                                if (included) saleContainerType5UnitPrice = translateInclueLable;
                                HtmlTemplate.Append(AddTableRows(new OPPricingTableRowDetailsArgs() { Value = saleContainerType5UnitPrice, RowDataType = "FieldPrice", QuoteOPTemplateSettingPM = setting, HeaderDesign = QuoteOPTemplateTextDesignLines, TableDesign = QuoteOPTemplateTableDesignPM, PricingSectionType = pricingSectionType }));

                            }
                        }
                    }


                    if (setting.ShowSaleCurrencyColumnContainers)
                    {
                        string saleTotalAmount = " ";
                        row += 1;
                        if (chargePM.SaleTotalAmount != null)
                        {
                            double value = (double)chargePM.SaleTotalAmount;
                            saleTotalAmount = value.ToString("N") + " " + GetChargeCurrencyCode(QuoteOPPM, chargePM);
                        }
                        if (included) saleTotalAmount = translateInclueLable;

                        HtmlTemplate.Append(AddTableRows(new OPPricingTableRowDetailsArgs() { Value = saleTotalAmount, RowDataType = "FieldPrice", QuoteOPTemplateSettingPM = setting, HeaderDesign = QuoteOPTemplateTextDesignLines, TableDesign = QuoteOPTemplateTableDesignPM, PricingSectionType = pricingSectionType }));

                    }

                    if (setting.ShowLocalCurrencyColumnContainers)
                    {
                        row += 1;
                  
                        string value = GetNumberValueFormate(chargePM.SaleTotalAmountLocal);

                        string localTotalAmount = value + " " + LocalCurrencyCode;

                        if (included) localTotalAmount = translateInclueLable;

                        HtmlTemplate.Append(AddTableRows(new OPPricingTableRowDetailsArgs() { Value = localTotalAmount, RowDataType = "FieldPrice", QuoteOPTemplateSettingPM = setting, HeaderDesign = QuoteOPTemplateTextDesignLines, TableDesign = QuoteOPTemplateTableDesignPM, PricingSectionType = pricingSectionType }));

                    }


                    if (setting.ShowChargeDescriptionContainers)
                    {
                        row += 1;

                        HtmlTemplate.Append(AddTableRows(new OPPricingTableRowDetailsArgs() {FieldCode = "CHARGEDESCRIPTIONCONTAINERS", Value = chargePM.ChargesTypeDescription, RowDataType = "Field", QuoteOPTemplateSettingPM = setting, HeaderDesign = QuoteOPTemplateTextDesignLines, TableDesign = QuoteOPTemplateTableDesignPM, PricingSectionType = pricingSectionType }));


                    }

                    if (setting.ShowChargeNoteContainers)
                    {
                        HtmlTemplate.Append(AddTableRows(new OPPricingTableRowDetailsArgs() { Value = chargePM.Notes, RowDataType = "Field", QuoteOPTemplateSettingPM = setting, HeaderDesign = QuoteOPTemplateTextDesignLines, TableDesign = QuoteOPTemplateTableDesignPM, PricingSectionType = pricingSectionType }));


                    }

                    if (setting.ShowSaleMaxMinAmountContainers)
                    {
                        string saleMaxMinAmount = included ? translateInclueLable : GetSaleMaxMinAmountValue(chargePM);
                        HtmlTemplate.Append(AddTableRows(new OPPricingTableRowDetailsArgs() { Value = saleMaxMinAmount, RowDataType = "Field", QuoteOPTemplateSettingPM = setting, HeaderDesign = QuoteOPTemplateTextDesignLines, TableDesign = QuoteOPTemplateTableDesignPM, PricingSectionType = pricingSectionType }));
                    }

                    if (setting.ShowRegionalTAXContainers)
                    {
                        string isRegionalTax = chargePM.IsRegionalTax ? "Yes" : "No";
                        HtmlTemplate.Append(AddTableRows(new OPPricingTableRowDetailsArgs() { Value = isRegionalTax, RowDataType = "Field", QuoteOPTemplateSettingPM = setting, HeaderDesign = QuoteOPTemplateTextDesignLines, TableDesign = QuoteOPTemplateTableDesignPM, PricingSectionType = pricingSectionType }));
                    }

                    if (QuoteOPPM.IsChargesByVAT)
                    {
                        if (setting.ShowVATTypeContainers)
                        {
                            HtmlTemplate.Append(AddTableRows(new OPPricingTableRowDetailsArgs() { FieldCode = "VATTYPECONTAINERS", Value = chargePM.VatTypeName, RowDataType = "Field", QuoteOPTemplateSettingPM = setting, HeaderDesign = QuoteOPTemplateTextDesignLines, TableDesign = QuoteOPTemplateTableDesignPM, PricingSectionType = pricingSectionType }));
                        }
                        if (setting.ShowVATPercentageContainers)
                        {
                            string value = chargePM.VatPercentage!=null? (chargePM.VatPercentage + "%"):"";
                            HtmlTemplate.Append(AddTableRows(new OPPricingTableRowDetailsArgs() { FieldCode = "VATPERCENTAGECONTAINERS", Value = value, RowDataType = "Field", QuoteOPTemplateSettingPM = setting, HeaderDesign = QuoteOPTemplateTextDesignLines, TableDesign = QuoteOPTemplateTableDesignPM, PricingSectionType = pricingSectionType }));
                        }
                    }




                }

                HtmlTemplate.Append("</tr>");
            }
        }

        private  string GetNumberValueFormate(double? value)
        {
            string result = string.Empty;
            if (value != null)
            {
                result = ((double)value).ToString("N"); // 1,234.512
            }

            return result;
        }

        private string GetUnitPricePackagesValue(QuoteOPTemplateBuildArges QuoteOPTemplateBuildArges, QuoteOPSaleChargePM chargePM, bool included)
        {
            string saleUnitPriceValues = "";
            QuoteOPPM QuoteOPPM = QuoteOPTemplateBuildArges.QuoteOPPM;

            if (chargePM.IsChargeBySteps && QuoteOPPM.QuoteTypeCode == "P")
            {
                if (!string.IsNullOrEmpty(chargePM.PriceBreaks))
                {
                    var priceBreaks = chargePM.PriceBreaks;
                    priceBreaks = priceBreaks.Replace("\r", "^");
                    var priceBreaksArrays = priceBreaks.Split('^');
                    priceBreaks = null;
                    int i = 1;
                    foreach (var item in priceBreaksArrays)
                    {
                        var priceBreak = FormatPriceBreaksWithTwoDecimalDigits(item);
                        if (i > 1 || QuoteOPTemplateBuildArges.QuoteOPTemplateSettingPM.RightToLeft) priceBreaks += "&nbsp;";
                        priceBreaks += ((priceBreak + " " + GetChargeCurrencyCode(QuoteOPPM, chargePM)) + "<br>");


                        i += 1;
                    }
                    saleUnitPriceValues = priceBreaks;

                }
            }
            else
            {
                if (chargePM.SaleUnitPrice != null)
                {
                    double value = (double)chargePM.SaleUnitPrice;
                    saleUnitPriceValues = value.ToString("N"); // 1,234.512
                    string saleUnitPriceCurrency = chargePM.SaleMeasurementCode == "PRFR" ? "%" : chargePM.CurrencyCode;
                    saleUnitPriceValues += " " + saleUnitPriceCurrency;
                }
                if (included)
                {
                    saleUnitPriceValues = GetNameColum("INCLUDED", QuoteOPTemplateBuildArges.QuoteOPTemplateTextCodePMLists, QuoteOPTemplateBuildArges.SectionTypeCode);
                }
            }

            return saleUnitPriceValues;
        }



        private string FormatPriceBreaksWithTwoDecimalDigits(string priceBreak)
        {
            string priceBreakFormatted = priceBreak;
            if (!string.IsNullOrEmpty(priceBreak) && priceBreak.Contains(":"))
            {
                string priceBreakPartDigitNumber = priceBreak.Split(':')[1];
                if (!string.IsNullOrEmpty(priceBreakPartDigitNumber))
                {
                    string stringDigitNumber = Regex.Replace(priceBreakPartDigitNumber, @"\D", "");
                    if (!string.IsNullOrEmpty(stringDigitNumber))
                    {
                        string priceBreakPartDigitNumberFormated = priceBreakPartDigitNumber.Replace(stringDigitNumber, Double.Parse(stringDigitNumber).ToString("N"));
                        priceBreakFormatted = priceBreakFormatted.Replace(priceBreakPartDigitNumber, priceBreakPartDigitNumberFormated);
                    }
                }
            }
            return priceBreakFormatted;
        }


        private static string GetChargeCurrencyCode(QuoteOPPM QuoteOPPM, QuoteOPSaleChargePM chargePM)
        {
            return (QuoteOPPM.IsSaleCurrencySameAsCost || QuoteOPPM.IsMultiCurrency) ? chargePM.CurrencyCode : QuoteOPPM.SaleCurrencyCode;
        }

        private static string GetFormatDisplayAmountWithCurrencyCode(string saleTotalAmount, string currencyCode, QuoteOPTemplateSettingPM setting)
        {
            string result = saleTotalAmount + " " + currencyCode;
         
            return result;
        }



        private string GetSaleMaxMinAmountValue(QuoteOPSaleChargePM chargePM)
        {
            var saleMinAmount = string.Empty;
            var saleMaxAmount = string.Empty;
            string saleMaxMinAmount = string.Empty;

            if (chargePM.SaleMinAmount != null && chargePM.SaleMinAmount > 0)
            {
                string AA = " ";
                double value = (double)chargePM.SaleMinAmount;
                AA = value.ToString("N");
                saleMinAmount = "min " + AA;
                saleMaxMinAmount = saleMinAmount;
            }

            if (chargePM.SaleMaxAmount != null && chargePM.SaleMaxAmount > 0)
            {
                string AA = " ";
                double value = (double)chargePM.SaleMaxAmount;
                AA = value.ToString("N");
                saleMaxAmount = "max " + AA;
                if (!string.IsNullOrEmpty(saleMaxMinAmount)) saleMaxMinAmount += " , ";
                saleMaxMinAmount += saleMaxAmount;
            }
            return saleMaxMinAmount;
        }


    #region TotalPerContainer
        private void BuildOPTotalPerContainerClassLists(QuoteOPPM QuoteOPPM, List<QuoteOPSaleChargePM> quoteSaleCharges, StringBuilder HtmlTemplate, QuoteOPTemplateSettingPM setting, List<OPTotalPerContainerClass> OPTotalPerContainerClassLists)
        {
            QuoteOPSaleChargePM freightQuoteOPSaleChargePM = quoteSaleCharges.Where(d => d.ChargesGroupCode == "FRT").FirstOrDefault();
            foreach (QuoteOPSaleChargePM QuoteOPSaleChargePM in quoteSaleCharges)
            {

                if (QuoteOPPM.PackageType1Id != null)
                {
                    double? saleUnitPrice1InSaleCurrency = GetSaleUnitPriceInSaleCurrency("SaleUnitPrice1InSaleCurrency", QuoteOPSaleChargePM, freightQuoteOPSaleChargePM);
                    ComputedTotalPerContainer(OPTotalPerContainerClassLists, QuoteOPPM.PackageType1Id, saleUnitPrice1InSaleCurrency, QuoteOPPM.PackageType1Quantity, "PackageType1Id", QuoteOPSaleChargePM);
                }

                if (QuoteOPPM.PackageType2Id != null)
                {
                    double? saleUnitPrice2InSaleCurrency = GetSaleUnitPriceInSaleCurrency("SaleUnitPrice2InSaleCurrency", QuoteOPSaleChargePM, freightQuoteOPSaleChargePM);

                    ComputedTotalPerContainer(OPTotalPerContainerClassLists, QuoteOPPM.PackageType2Id, saleUnitPrice2InSaleCurrency, QuoteOPPM.PackageType2Quantity, "PackageType2Id", QuoteOPSaleChargePM);
                }

                if (QuoteOPPM.PackageType3Id != null)
                {
                    double? saleUnitPrice3InSaleCurrency = GetSaleUnitPriceInSaleCurrency("SaleUnitPrice3InSaleCurrency", QuoteOPSaleChargePM, freightQuoteOPSaleChargePM);

                    ComputedTotalPerContainer(OPTotalPerContainerClassLists, QuoteOPPM.PackageType3Id, saleUnitPrice3InSaleCurrency, QuoteOPPM.PackageType3Quantity, "PackageType3Id", QuoteOPSaleChargePM);
                }

                if (QuoteOPPM.PackageType4Id != null)
                {
                    double? saleUnitPrice4InSaleCurrency = GetSaleUnitPriceInSaleCurrency("SaleUnitPrice4InSaleCurrency", QuoteOPSaleChargePM, freightQuoteOPSaleChargePM);

                    ComputedTotalPerContainer(OPTotalPerContainerClassLists, QuoteOPPM.PackageType4Id, saleUnitPrice4InSaleCurrency, QuoteOPPM.PackageType4Quantity, "PackageType4Id", QuoteOPSaleChargePM);
                }

                if (QuoteOPPM.PackageType5Id != null)
                {
                    double? saleUnitPrice5InSaleCurrency = GetSaleUnitPriceInSaleCurrency("SaleUnitPrice5InSaleCurrency", QuoteOPSaleChargePM, freightQuoteOPSaleChargePM);

                    ComputedTotalPerContainer(OPTotalPerContainerClassLists, QuoteOPPM.PackageType5Id, saleUnitPrice5InSaleCurrency, QuoteOPPM.PackageType5Quantity, "PackageType5Id", QuoteOPSaleChargePM);
                }

            }
        }

        private double? GetSaleUnitPriceInSaleCurrency(string fieldName, QuoteOPSaleChargePM QuoteOPSaleChargePM, QuoteOPSaleChargePM fRTQuoteOPSaleChargePM)
        {
            double? result =0;
            var entityPM = QuoteOPSaleChargePM.SaleMeasurementCode != "PRFR" ? QuoteOPSaleChargePM : fRTQuoteOPSaleChargePM;
            if (entityPM != null)
            {
                PropertyInfo propInfo = entityPM.GetType().GetProperty(fieldName);
                object propertValue = propInfo.GetValue(entityPM);
                if (propertValue != null && !string.IsNullOrEmpty(propertValue.ToString()))
                {
                    result = double.Parse(propertValue.ToString());
                }
            }

            return result;

        }

        private void ComputedTotalPerContainer(List<OPTotalPerContainerClass> OPTotalPerContainerClassLists, string PackageTypeId, double? saleUnitPriceInSaleCurrency, int? packageTypeQuantity, string fieldCode, QuoteOPSaleChargePM chargePM)
        {
            double value = 0;
            double orginalValue = 0;
            if (fieldCode == "PackageType1Id") orginalValue = chargePM.SaleContainerType1UnitPrice!=null ? (double)chargePM.SaleContainerType1UnitPrice:0;
            else if (fieldCode == "PackageType2Id") orginalValue = chargePM.SaleContainerType2UnitPrice != null ? (double)chargePM.SaleContainerType2UnitPrice : 0;
            else if (fieldCode == "PackageType3Id") orginalValue = chargePM.SaleContainerType3UnitPrice != null ? (double)chargePM.SaleContainerType3UnitPrice : 0;
            else if (fieldCode == "PackageType4Id") orginalValue = chargePM.SaleContainerType4UnitPrice != null ? (double)chargePM.SaleContainerType4UnitPrice : 0;
            else if (fieldCode == "PackageType5Id") orginalValue = chargePM.SaleContainerType5UnitPrice != null ? (double)chargePM.SaleContainerType5UnitPrice : 0;

            if (chargePM.SaleMeasurementCode == "FIXD" && orginalValue == 0)
            {
                orginalValue = chargePM.SaleTotalAmount != null ? (double)chargePM.SaleTotalAmount : 0;
            }

            PackageType packageType = PackageTypeRepository.GetSinglePackageType(PackageTypeId, chargePM.Tenant, true);
            string printAs = packageType.PrintAs;
            string name = packageTypeQuantity != null ? packageTypeQuantity + " x " + printAs : " x " + printAs;

            if (chargePM.SaleMeasurementCode == "BCNT" && packageTypeQuantity != null && saleUnitPriceInSaleCurrency != null)
            {
                value = (double)packageTypeQuantity * (double)saleUnitPriceInSaleCurrency;
                orginalValue = (double)packageTypeQuantity * (double)orginalValue;

            }
            else if (chargePM.SaleMeasurementCode == "BTEU" && chargePM.SaleUnitPriceInSaleCurrency != null && packageTypeQuantity != null)
            {
                value = (double)chargePM.SaleUnitPriceInSaleCurrency * (double)packageTypeQuantity * packageType.TEU;
                orginalValue = (double)packageTypeQuantity * (double)orginalValue * packageType.TEU; 
            }
            else if (chargePM.SaleMeasurementCode == "PRFR")
            {
                value = GetTotalPerContainerForPercentofFreight(saleUnitPriceInSaleCurrency, packageTypeQuantity, chargePM);
            }
            else
            {
                if (chargePM.SaleAmountInSaleCurrency != null)
                {
                    value = (double)chargePM.SaleAmountInSaleCurrency;
                }
            }
            AddPerContainerClassToLists(OPTotalPerContainerClassLists, value, name, fieldCode, chargePM, (double)orginalValue);

        }

        private static double GetTotalPerContainerForPercentofFreight(double? saleUnitPriceInSaleCurrency, int? packageTypeQuantity, QuoteOPSaleChargePM chargePM)
        {
            return (double)((saleUnitPriceInSaleCurrency * (double)packageTypeQuantity) * chargePM.SaleUnitPrice) / 100;
        }

        private static void AddPerContainerClassToLists(List<OPTotalPerContainerClass> OPTotalPerContainerClassLists, double value, string name, string fieldCode, QuoteOPSaleChargePM chargePM , double orginalValue)
        {
            if (OPTotalPerContainerClassLists != null)
            {
                OPTotalPerContainerClass OPTotalPerContainerClass = new OPTotalPerContainerClass()
                {
                    Value = value,
                    ChargeGroupCode = chargePM.ChargesGroupCode,
                    Name = name,// "PackageType1Id",
                    FieldCode = fieldCode,
                    SaleExchangeRate = chargePM.SaleExchangeRate,
                    CurrencyCode = chargePM.CurrencyCode,
                    OrginalValue = orginalValue,
                    ChargesGroupName = chargePM.ChargesGroupName,
                    ChargesTypeViewOrder = chargePM.ChargesTypeViewOrder,
                    ChargesGroupViewOrder = chargePM.ChargesGroupViewOrder,
                };

                if (chargePM.IsAllIN != null && chargePM.IsAllIN.ToLower() == "true")
                {
                    OPTotalPerContainerClass.Included = true;
                    OPTotalPerContainerClass.OrginalValue = 0;
                    OPTotalPerContainerClass.Value = 0;
                }


                OPTotalPerContainerClassLists.Add(OPTotalPerContainerClass);
            }
        }

    #endregion

        private bool IsShowFixedPriceContainer(List<QuoteOPSaleChargePM> quoteSaleCharges)
        {
            bool viewFixedPriceContainer = false;

            foreach (QuoteOPSaleChargePM chargePM in quoteSaleCharges)
            {
                if (chargePM.SaleMeasurementCode == "FIXD")
                {
                    viewFixedPriceContainer = true;
                }
            };

            return viewFixedPriceContainer;
        }

        private string GetStyleRowTablePageHeaderFooter(string typetable, string height, double? width, string alignment, int loactionTd, QuoteOPTemplateTableDesignPM tabledesign, string BackgroundColorArea)
        {
            string style = "";
            string BackgroundColor = "";
            string BorderColor = tabledesign.BorderColor;
            if (BorderColor.Length > 7)
            {
                BorderColor = tabledesign.BorderColor.Remove(1, 2);
            }

            BackgroundColor = BackgroundColorArea;

            if (BackgroundColor.Length > 7)
            {
                BackgroundColor = BackgroundColorArea.Remove(1, 2);
            }

            string BorderThickness = tabledesign.BorderThickness.ToString() + "px";
            string Widthtd = width.ToString() + "%";

            if (typetable == "ALL" || typetable == "VERTICALLINES")
            {

                style = "style='" + "border-top:" + BorderThickness + " solid " + BorderColor + ";border-bottom:" + BorderThickness + " solid " + BorderColor + ";border-left:" + BorderThickness + " solid " + BorderColor + ";border-right:" + BorderThickness + " solid " + BorderColor + ";table-layout: auto" +
                              ";background-color:" + BackgroundColor + ";vertical-align: top" + ";Width:" + Widthtd + ";Height:" + height + ";text-align:" + alignment + ";max-width:" + Widthtd + ";word-wrap:break-word" + "'";


            }

            else if (typetable == "BOX" || typetable == "HORIZONTALLINES")
            {
                style = "style='" + ";background-color:" + BackgroundColor + ";vertical-align: top" + ";Width:" + Widthtd + ";Height:" + height + ";text-align:" + alignment + ";max-width:" + Widthtd + ";word-wrap:break-word" + "'";

            }

            else if (typetable == "NONE")
            {
                style = "style='" + "border-collapse: collapse; " + "table-layout: auto" + "; vertical-align:central" + ";background-color:" + BackgroundColor + " ;text-align:" + alignment + ";Width:" + Widthtd + ";Height:" + height + ";max-width:" + Widthtd + ";word-wrap:break-word" + ";vertical-align: top" + "'";
            }

            return style;
        }

    #endregion

        public byte[] DownloadQuoteOPTemplateSectionDataFile(string documentId, int tenant)
        {

            ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);
            DocumentRepository docRepository = new DocumentRepository(objectContext);

            Simplog.Data.CommonDataModel.EntityPOCOs.Document document = docRepository.GetSingleDocument(tenant, documentId);

            if (document != null)
            {
                Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
                {
                    FileName = document.Id,
                    FolderName = document.Folder,
                    Extension = document.Extension,
                    Tenant = document.Tenant,
                    FileSize = document.FileSize,
                };
                Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
                return storageservice.Read(fileInfo);

            }
            else return null;
        }

        public byte[] HtmlToPdf(string bodyHtmlString, QuoteOPTemplateSettingPM setting)
        {
            PdfConverter pdfConverter = new PdfConverter();
            pdfConverter.LicenseKey = "fvDj8eTh8eDg4vHk/+Hx4uD/4OP/6Ojo6A==";
            pdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.A4;
            SetPdfMargins(setting, pdfConverter.PdfDocumentOptions);

            pdfConverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Normal;
            pdfConverter.PdfDocumentOptions.PdfPageOrientation = PdfPageOrientation.Portrait;
            pdfConverter.PdfDocumentOptions.ShowHeader = true;
            pdfConverter.PdfDocumentOptions.ShowFooter = true;


            byte[] data = pdfConverter.GetPdfBytesFromHtmlString(bodyHtmlString);
            return data;
        }

        public byte[] GetFile(string fileid, string extention, string location, int tenant)
        {
            try
            {
                byte[] datainByte;

                Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
                {
                    FileName = fileid,
                    FolderName = location,
                    Extension = extention,
                    Tenant = tenant,

                };
                Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;

                return storageservice.Read(fileInfo);



                //string filename = fileid + "." + extention;

                //CloudBlobContainer blobContainer = StorageAcountDetails.GetCurrentContainer(tenant);

                //var blobfile = blobContainer.GetBlockBlobReference(StorageAcountDetails.GetBlobNameByLocation(filename, location));

                //if (blobfile.Exists())
                //{
                //    using (MemoryStream memstream = new MemoryStream())
                //    {
                //        blobfile.DownloadToStream(memstream);
                //        datainByte = memstream.ToArray();
                //    }

                //    return datainByte;
                //}
                //else
                //    return null;
            }
            catch (Exception e)
            {
                // ExceptionHandler.HandleException(e, DateTime.Now, tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "Uploader : DownloadFile Method");
                return null;
            }
        }
    }


#endif

    public class OPPricingTableRowDetailsArgs
    {
        public string Value { get; set; }
        public QuoteOPTemplateSettingPM QuoteOPTemplateSettingPM { get; set; }
        public string FieldCode { get; set; }
        public QuoteOPTemplateTextDesignPM HeaderDesign { get; set; }
        public QuoteOPTemplateTableDesignPM TableDesign { get; set; }
        public string PricingSectionType { get; set; }        
        public string RowDataType { get; set; }
        

    }



    public class OPTotalPerContainerClass
    {
        public double Value { get; set; }
        public string Name { get; set; }
        public string FieldCode { get; set; }
        public string ChargeGroupCode { get; set; }
        public double? SaleExchangeRate { get; set; }
        public string CurrencyCode { get; set; }
        public double? OrginalValue { get; set; }
        public bool Included { get; set; }
        public string ChargesGroupName { get; set; }
        public int ChargesTypeViewOrder { get; set; }
        public int ChargesGroupViewOrder { get; set; }

    }

    public class QuoteOPTemplateBuildArges
    {
        public List<QuoteOPTemplateTableDesignPM> QuoteOPTemplateTableDesignsLists { get; set; }
        public List<QuoteOPTemplateTextCodePM> QuoteOPTemplateTextCodePMLists { get; set; }
        public QuoteOPPM QuoteOPPM { get; set; }
        public QuoteOPTemplatePM QuoteOPTemplatePM { get; set; }
        public QuoteOPTemplateSettingPM QuoteOPTemplateSettingPM { get; set; }
        public List<QuoteOPTemplateTextDesignPM> QuoteOPTemplateTextDesignPMLists { get; set; }
        public string SectionTypeCode { get; set; }
        public bool IsResultPDF { get; set; }
        public int Tenant { get; set; }
        public int? UserTenant { get; set; }
        public string UserId { get; set; }
        public List<QuoteOPTemplateSectionPM> QuoteOPTemplateSectionPMLists { get; set; }
        public bool HideQuoteHeaderFromPdf { get; set; }
        public string RequestArea { get; set; }
        public bool FirstSectionInBody { get; set; }
        public int? VersionNumber { get; set; }


    }


    public class OPChargeGroupOrderList
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int GroupOrder { get; set; }
        public int ChargeTypeOrder { get; set; }
        public int Order { get; set; }
        public int IsOrder { get; set; }
    }

    public interface IQuoteOPTemplateReportHelper { }

}

