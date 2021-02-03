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
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Microsoft.WindowsAzure.Storage.Blob;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
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
using Logitude.BL.QuoteModel.Tools.EntityService;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityLists;
using System.Text.RegularExpressions;

namespace Logitude.BL.Helpers
{
    public class QuoteTemplateReportHelper : IQuoteTemplateReportHelper
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

        public byte[] BuildQuoteTemplatePdfReport(string quoteId, string quoteTemplateId, string userId, int tenant, List<QuoteTemplateSectionPM> templateSections, int? userTenant = null, QuotePM quotePM = null, int? versionNumber = null)
        {


            QuoteTemplateBuildArges quoteTemplateBuildArges = new QuoteTemplateBuildArges();
            IQuotesContext context = QuotesContext.GetContext(tenant);
            QuoteQuery quoteQuery = new QuoteQuery(new QuoteRepository(context));
            QuoteTemplateQuery quoteTemplateQuery = new QuoteTemplateQuery(new QuoteTemplateRepository(context));
            QuoteTemplateSettingQuery quoteTemplateSettingQuery = new QuoteTemplateSettingQuery(new QuoteTemplateSettingRepository(context));
            QuoteTemplateSectionQuery sectionsQuery = new QuoteTemplateSectionQuery(new QuoteTemplateSectionRepository(context));
            QuoteTemplateTextDesignQuery quotetemplateTextDesignQuery = new QuoteTemplateTextDesignQuery(new QuoteTemplateTextDesignRepository(context));
            QuoteTemplateTableDesignQuery quotetemplateTableDesignQuery = new QuoteTemplateTableDesignQuery(new QuoteTemplateTableDesignRepository(context));
            QuoteTemplateTextCodeQuery quoteTemplateTextCodeQuery = new QuoteTemplateTextCodeQuery(tenant);
            HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();
            byte[] pdfData = null;
            string headerHtmlString = null;
            string footerHtmlString = null;
            string bodyHtmlString = null;
            string RequestArea = "Maintenance";
            QuoteTemplatePM template = quoteTemplateQuery.GetSinglePM(quoteTemplateId, tenant);
            ObjectTableRepository objectTabelRepository = null;
            ObjectTable objectTable = null;

            if (!string.IsNullOrEmpty(quoteId) && quotePM == null)
            {
                int tenantNumber = userTenant != null ? (int)userTenant : tenant;
                quotePM = quoteQuery.GetSinglePM(quoteId, tenantNumber);
            }

            if (quotePM == null) quotePM = BuildingQuotePM();

            if (templateSections == null)
            {
                if (quotePM.QuoteTemplateId == quoteTemplateId)
                {
                    if (!string.IsNullOrEmpty(quotePM.QuotationSections))
                    {
                        templateSections = sectionsQuery.GetQuoteTemplateSectionPMsByIds(quotePM.QuotationSections.Split(',').ToList(), tenant);
                    }
                }

                if (templateSections == null)
                {
                    templateSections = sectionsQuery.GetQuoteTemplateSectionPMsByTemplateId(quoteTemplateId, tenant);
                }

                QuoteTemplateSectionRepository quoteTemplateSectionsRepository = new QuoteTemplateSectionRepository(context);
                List<QuoteTemplateSectionModification> modifications = quoteTemplateSectionsRepository.GetAllQuoteTemplateSectionModifications(quotePM.Id, tenant).ToList();
                foreach (QuoteTemplateSectionModification mod in modifications)
                {
                    QuoteTemplateSectionPM section = templateSections.Where(s => s.Id == mod.QuoteTemplateSectionId).FirstOrDefault();
                    if (section != null)
                    {
                        section.SectionDocId = mod.SectionDocId;
                    }
                }

                QuoteTemplateExcludedSectionRepository excludedSectionRepository = new QuoteTemplateExcludedSectionRepository(tenant);
                List<QuoteTemplateExcludedSection> excludedsection = excludedSectionRepository.GetAllQuoteTemplateExcludedSection(quoteId, quoteTemplateId, tenant).ToList();

                if (excludedsection != null)
                {
                    foreach (QuoteTemplateExcludedSection mod2 in excludedsection)
                    {
                        QuoteTemplateSectionPM section = templateSections.Where(s => s.Id == mod2.QuoteTemplateSectionId).FirstOrDefault();
                        if (section != null)
                        {
                            section.IsExcluded = true;
                        }
                    }
                }
            }


            QuoteTemplateSettingPM setting = quoteTemplateSettingQuery.GetSinglePM(template.QuoteTemplateSettingId, tenant);
            List<QuoteTemplateTextDesignPM> quoteTemplateTextDesignsList = quotetemplateTextDesignQuery.GetQuoteTemplateTextDesignPMsByTenant(tenant).ToList();
            List<QuoteTemplateTableDesignPM> quoteTemplateTableDesignsList = quotetemplateTableDesignQuery.GetQuoteTemplateTableDesignPMsByTenant(tenant).ToList();
            List<QuoteTemplateTextCodePM> textcodes = quoteTemplateTextCodeQuery.GetQuoteTemplateTextCodePMsByQuoteTemplateId(template.Tenant, template.Id).ToList();



            if (!string.IsNullOrEmpty(quoteId))
            {
                RequestArea = "Quote";
                if (quotePM != null)
                {
                    if (quotePM.TransportModeId == "A" || quotePM.ShipmentTypeId == "LCL" || quotePM.ShipmentTypeId == "LCLD" || quotePM.ShipmentTypeId == "LTL")
                    {
                        templateSections = templateSections.Where(t => t.QuoteTemplateSectionTypeCode != "PC").ToList();

                    }
                    else if (quotePM.ShipmentTypeId == "FCL" || quotePM.ShipmentTypeId == "FCLD" || quotePM.ShipmentTypeId == "FTL")
                    {
                        templateSections = templateSections.Where(t => t.QuoteTemplateSectionTypeCode != "PP").ToList(); ;
                    }
                }
            }



            quoteTemplateBuildArges.Tenant = tenant;
            quoteTemplateBuildArges.IsResultPDF = true;
            quoteTemplateBuildArges.QuotePM = quotePM;
            quoteTemplateBuildArges.QuoteTemplateSettingPM = setting;
            quoteTemplateBuildArges.QuoteTemplateTextDesignPMLists = quoteTemplateTextDesignsList;
            quoteTemplateBuildArges.QuoteTemplateTableDesignsLists = quoteTemplateTableDesignsList;
            quoteTemplateBuildArges.QuoteTemplateTextCodePMLists = textcodes;
            quoteTemplateBuildArges.UserTenant = userTenant != null ? (int)userTenant : tenant;
            quoteTemplateBuildArges.UserId = userId;
            quoteTemplateBuildArges.QuoteTemplateSectionPMLists = templateSections;
            quoteTemplateBuildArges.QuoteTemplatePM = template;
            quoteTemplateBuildArges.VersionNumber = versionNumber;


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
            QuoteTemplateSectionPM headerSection = templateSections.Where(s => s.QuoteTemplateSectionTypeCode == "PH").FirstOrDefault();
            if (!headerSection.IsExcluded)
            {
                pdfConverter.PdfDocumentOptions.ShowHeader = true;
                if (setting.PageHeaderArea1Type == "Quote Header" || setting.PageHeaderArea2Type == "Quote Header" || setting.PageHeaderArea3Type == "Quote Header") quoteTemplateBuildArges.HideQuoteHeaderFromPdf = true;
                SetSectionTypeCode(quoteTemplateBuildArges, "PH");
                byte[] headerdata = GetQuoteTemplatePageHeaderFooter(quoteTemplateBuildArges);
                headerHtmlString += GetBodyString(headerdata);



                headerHtmlString = ResolveHtmlData(tenant, htmlEditorHelper, headerHtmlString, quotePM, template, userId, ref objectTabelRepository, ref objectTable);
                HtmlToPdfElement headerHtml = new HtmlToPdfElement(0, 0, 0, 0, headerHtmlString, null, 2040, 0);
                pdfConverter.PdfHeaderOptions.AddElement(headerHtml);
                pdfConverter.PdfHeaderOptions.HeaderHeight = 1;
                pdfConverter.PdfHeaderOptions.HeaderHeight = setting.PageHeaderAreaHeight * 29;
            

            }


            // set the Footer HTML area
            QuoteTemplateSectionPM footerSection = templateSections.Where(s => s.QuoteTemplateSectionTypeCode == "PF").FirstOrDefault();
            if (!footerSection.IsExcluded)
            {
                pdfConverter.PdfDocumentOptions.ShowFooter = true;
                SetSectionTypeCode(quoteTemplateBuildArges, "PF");
                byte[] footerdata = GetQuoteTemplatePageHeaderFooter(quoteTemplateBuildArges);
                footerHtmlString += GetBodyString(footerdata);

                double footerTopMargin = (double)setting.SpaceLinesBeforeFooters * 21;
                float heightFooter = (setting.PageFooterAreaHeight * 29) + (float)footerTopMargin;

                footerHtmlString = ResolveHtmlData(tenant, htmlEditorHelper, footerHtmlString, quotePM, template, userId, ref objectTabelRepository, ref objectTable);
                HtmlToPdfElement footerHtml = new HtmlToPdfElement(0, 0, 0, 0, footerHtmlString, null, 2040, 0);
                pdfConverter.PdfFooterOptions.AddElement(footerHtml);
                pdfConverter.PdfFooterOptions.FooterHeight = (heightFooter + 10);

                if (!setting.HidePageNumber)
                {
                    QuoteTemplateTextDesignPM quotetemplateTextDesignPMPageNumbering = quoteTemplateTextDesignsList.Where(t => t.Id == setting.PageNumberingTextDesignId).FirstOrDefault();
                    TextElement footerTextElement = null;
                    if (quotetemplateTextDesignPMPageNumbering != null)
                    {
                        footerTextElement = GetTextElementProperitiesForQuotetemplateTextDesign(quotetemplateTextDesignPMPageNumbering, heightFooter);
                        pdfConverter.PdfFooterOptions.FooterHeight += Convert.ToSingle(quotetemplateTextDesignPMPageNumbering.FontSize) - 7;
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

            SetSectionTypeCode(quoteTemplateBuildArges, null);
            byte[] bodyData = GetQuoteTemplateHtmlReport(quoteTemplateBuildArges, false, RequestArea);
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
            bodyHtmlString = ResolveHtmlData(tenant, htmlEditorHelper, bodyHtmlString, quotePM, template, userId, ref objectTabelRepository, ref objectTable);

            pdfData = pdfConverter.ConvertHtml(bodyHtmlString, null);
            if (quotePM.Id != "10697")
            {
                string fullHtml = headerHtmlString + bodyHtmlString + footerHtmlString;
                CreateHtmlQuotationDocument(tenant, quotePM, fullHtml);
            }

            return pdfData;
        }


        private TextElement GetTextElementProperitiesForQuotetemplateTextDesign(QuoteTemplateTextDesignPM quotetemplateTextDesignPMPageNumbering, float heightFooter)
        {
            FontFamily family = new FontFamily(quotetemplateTextDesignPMPageNumbering.FontFamily);
            bool isBold = quotetemplateTextDesignPMPageNumbering.FontWeight.ToLower() == "bold";
            bool isItalic = quotetemplateTextDesignPMPageNumbering.Italic;
            bool isUnderline = quotetemplateTextDesignPMPageNumbering.UnDerLine;
            Color backGroundColor = System.Drawing.ColorTranslator.FromHtml(quotetemplateTextDesignPMPageNumbering.BackgroundColor);
            Color ForeColor = System.Drawing.ColorTranslator.FromHtml(quotetemplateTextDesignPMPageNumbering.TextColor);
            HorizontalTextAlign textAlign = quotetemplateTextDesignPMPageNumbering.Alignment.ToLower() == "right" ? HorizontalTextAlign.Right : quotetemplateTextDesignPMPageNumbering.Alignment.ToLower() == "left" ? HorizontalTextAlign.Left : HorizontalTextAlign.Center;
            float size = Convert.ToSingle(quotetemplateTextDesignPMPageNumbering.FontSize);
            
            Font font = new Font(family, size, (isBold ? FontStyle.Bold : FontStyle.Regular) | (isItalic ? FontStyle.Italic : FontStyle.Regular) | (isUnderline ? FontStyle.Underline : FontStyle.Regular), GraphicsUnit.Point);

            TextElement footerTextElement = new TextElement(0, heightFooter, "page &p; of &P;  ", font);
            footerTextElement.TextAlign = textAlign;
            footerTextElement.BackColor = backGroundColor;
            footerTextElement.ForeColor = ForeColor;

            return footerTextElement;
        }
        private  void SetPdfMargins(QuoteTemplateSettingPM setting, PdfDocumentOptions PdfDocumentOptions)
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

        private static void CreateHtmlQuotationDocument(int tenant, QuotePM quotePM, string fullHtml)
        {
            if (!string.IsNullOrEmpty(fullHtml))
            {
                var fullQuotationHtmlByte = Encoding.UTF8.GetBytes(fullHtml);
                DocumentRepository documentRep = new DocumentRepository(tenant);
                Document document = new Document()
                {
                    FileName = "QuotationHtml-" + quotePM.QuoteNumber,
                    CalculatedFileName = "QuotationHtml-" + quotePM.QuoteNumber,
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
                quotePM.QuoteHTMLDocumentId = document.Id;
            }
        }

        private static void SetSectionTypeCode(QuoteTemplateBuildArges quoteTemplateBuildArges, string sectionTypeCode)
        {
            quoteTemplateBuildArges.SectionTypeCode = sectionTypeCode;
        }

        private string ResolveHtmlData(int tenant, HtmlEditorHelper htmlEditorHelper, string htmlString, QuotePM quotePM, QuoteTemplatePM template, string userId, ref ObjectTableRepository objectTabelRepository, ref ObjectTable objectTable)
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


                var htmlEditorResolveResult = htmlEditorHelper.ResolveHtmlData(htmlEditorResolveArgs, quotePM);
                htmlString = htmlEditorResolveResult.HtmlString;
            }
            return htmlString;
        }



        #region Quote Template Page Header And Footer

        public byte[] GetQuoteTemplatePageHeaderFooter(QuoteTemplateBuildArges quoteTemplateBuildArges)
        {

            QuoteTemplatePM template = quoteTemplateBuildArges.QuoteTemplatePM;
            QuoteTemplateSettingPM setting = quoteTemplateBuildArges.QuoteTemplateSettingPM;
            List<QuoteTemplateTextDesignPM> quoteTemplateTextDesignsList = quoteTemplateBuildArges.QuoteTemplateTextDesignPMLists;
            int tenant = quoteTemplateBuildArges.Tenant;
            bool isPdf = quoteTemplateBuildArges.IsResultPDF;
            string sessiontype = quoteTemplateBuildArges.SectionTypeCode == "PH" ? "Header" : "Footer";

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

            QuoteTemplateTableDesignPM TableDesign = new QuoteTemplateTableDesignPM()
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
                AppendHeaderFooterAreaText(borderTypeCode, area1Width, area1FreeText, area1FreeTextDesignId, quoteTemplateTextDesignsList, ref NumberOfTd, ref CountArea, widthArea1, HeightArea, HtmlTemplate, TableDesign, isPdf);
            }
            else if (pageArea1Type == "Quote Header" && type == "Header" && area1Width > 0)
            {
                AppendHeaderFooterAreaQuoteHeaderText(borderTypeCode, area1Width, ref NumberOfTd, ref CountArea, widthArea1, HeightArea, HtmlTemplate, TableDesign, isPdf, quoteTemplateBuildArges);
            }



            if (pageArea2Type == "Logo" && area2Width > 0)
            {
                AppendHeaderFooterAreaImage(borderTypeCode, area2Width, area2Height, image2Width, area2ImageAlignment, area2ImageDetailId, tenant, ref NumberOfTd, ref CountArea, HeightArea, HtmlTemplate, TableDesign, isPdf, (int)heightAreaNumber);
            }
            else if (pageArea2Type == "Text" && area2Width > 0)
            {
                AppendHeaderFooterAreaText(borderTypeCode, area2Width, area2FreeText, area2FreeTextDesignId, quoteTemplateTextDesignsList, ref NumberOfTd, ref CountArea, widthArea2, HeightArea, HtmlTemplate, TableDesign, isPdf);
            }
            else if (pageArea2Type == "Quote Header" && type == "Header" && area2Width > 0)
            {
                AppendHeaderFooterAreaQuoteHeaderText(borderTypeCode, area2Width, ref NumberOfTd, ref CountArea, widthArea2, HeightArea, HtmlTemplate, TableDesign, isPdf, quoteTemplateBuildArges);
            }

            if (pageArea3Type == "Logo" && area3Width > 0)
            {
                AppendHeaderFooterAreaImage(borderTypeCode, area3Width, area3Height, image3Width, area3ImageAlignment, area3ImageDetailId, tenant, ref NumberOfTd, ref CountArea, HeightArea, HtmlTemplate, TableDesign, isPdf, (int)heightAreaNumber);
            }

            else if (pageArea3Type == "Text" && area3Width > 0)
            {
                AppendHeaderFooterAreaText(borderTypeCode, area3Width, area3FreeText, area3FreeTextDesignId, quoteTemplateTextDesignsList, ref NumberOfTd, ref CountArea, widthArea3, HeightArea, HtmlTemplate, TableDesign, isPdf);
            }

            else if (pageArea3Type == "Quote Header" && type == "Header" && area3Width > 0)
            {
                AppendHeaderFooterAreaQuoteHeaderText(borderTypeCode, area3Width, ref NumberOfTd, ref CountArea, widthArea3, HeightArea, HtmlTemplate, TableDesign, isPdf, quoteTemplateBuildArges);
            }

            HtmlTemplate.Append("</tr>");
            HtmlTemplate.Append("</table>");
            HtmlTemplate.Append("</div>");
            HtmlTemplate.Append("</body>");
            HtmlTemplate.Append("</html>");

            return Encoding.UTF8.GetBytes(HtmlTemplate.ToString());
        }

        private void AppendHeaderFooterAreaQuoteHeaderText(string borderTypeCode, double? areaWidth, ref int NumberOfTd, ref int CountArea, string widthArea, string HeightArea, StringBuilder HtmlTemplate, QuoteTemplateTableDesignPM TableDesign, bool isPdf, QuoteTemplateBuildArges quoteTemplateBuildArges)
        {
            if (quoteTemplateBuildArges != null)
            {
                string styleTd = GetStyleRowTablePageHeaderFooter(borderTypeCode, HeightArea, areaWidth, "Left", ++NumberOfTd, TableDesign, "#ffffff");
                styleTd += "^";
                styleTd = styleTd.Replace("'^", "");
                string padding = isPdf ? "10px" : "5px";
                styleTd += ";padding:" + padding + "'";


                int tenant = quoteTemplateBuildArges.Tenant;
                quoteTemplateBuildArges.RequestArea = "Header";
                var quoteHeaderHtmlByte = GetQuoteTemplateHeader(quoteTemplateBuildArges);

                string quoteHeaderHtml = "<td " + styleTd + ">" + Encoding.UTF8.GetString(quoteHeaderHtmlByte) + "</td>";

                if (isPdf)
                {
                    HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();
                    quoteHeaderHtml = htmlEditorHelper.ConvertNormalHtmlToEvoHtml(quoteHeaderHtml);
                }

                HtmlTemplate.Append(quoteHeaderHtml);

            }
        }

        private void AppendHeaderFooterAreaText(string borderTypeCode, double? areaWidth, string freeText, string freeTextDesignId, List<QuoteTemplateTextDesignPM> quoteTemplateTextDesignsList, ref int NumberOfTd, ref int CountArea, string widthArea, string HeightArea, StringBuilder HtmlTemplate, QuoteTemplateTableDesignPM TableDesign, bool isPdf)
        {
            if (string.IsNullOrEmpty(freeText)) freeText = "";

            freeText = freeText.Replace('\n', '\r');

            QuoteTemplateTextDesignPM freeTextDesign = quoteTemplateTextDesignsList.Where(t => t.Id == freeTextDesignId).FirstOrDefault();

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

        private void AppendHeaderFooterAreaImage(string borderTypeCode, double? areaWidth, int areaHeight, int imageWidth, string areaImageAlignment, string imageDetailId, int tenant, ref int NumberOfTd, ref int CountArea, string HeightArea, StringBuilder HtmlTemplate, QuoteTemplateTableDesignPM TableDesign, bool isPdf, int heightAreaNumber)
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

        public byte[] GetQuoteTemplateHtmlReport(QuoteTemplateBuildArges quoteTemplateBuildArges, bool includeHeaderFooter, string requestArea )
        {

            
            QuotePM quotePM = quoteTemplateBuildArges.QuotePM;
            QuoteTemplatePM template = quoteTemplateBuildArges.QuoteTemplatePM;
            QuoteTemplateSettingPM setting = quoteTemplateBuildArges.QuoteTemplateSettingPM;
            List<QuoteTemplateTextDesignPM> quoteTemplateTextDesignsList = quoteTemplateBuildArges.QuoteTemplateTextDesignPMLists;
            List<QuoteTemplateTableDesignPM> quoteTemplateTableDesignsList = quoteTemplateBuildArges.QuoteTemplateTableDesignsLists;
            List<QuoteTemplateTextCodePM> textcodes = quoteTemplateBuildArges.QuoteTemplateTextCodePMLists;
            List<QuoteTemplateSectionPM> templateSections = quoteTemplateBuildArges.QuoteTemplateSectionPMLists;
            string userId = quoteTemplateBuildArges.UserId;
            int tenant = quoteTemplateBuildArges.Tenant;
            IQuotesContext context = QuotesContext.GetContext(tenant);
            QuoteQuery quoteQuery = new QuoteQuery(new QuoteRepository(context));

            string htmlString = "";
            if (setting.ShowIncludedChargesPackages)
            {
                List<QuoteSaleChargePM> emptySaleCharges = quoteQuery.AddEmptySaleCharge(quotePM.QuoteCharges, quotePM.TransportModeId, quotePM.ShipmentTypeId);
                quotePM.QuoteSaleCharges.AddRange(emptySaleCharges);
            }

            if (requestArea == "Quote")
            {
                if (quotePM.TransportModeId == "A" || quotePM.ShipmentTypeId == "LCL" || quotePM.ShipmentTypeId == "LCLD" || quotePM.ShipmentTypeId == "LTL")
                {
                    templateSections = templateSections.Where(t => t.QuoteTemplateSectionTypeCode != "PC").ToList();

                }
                else if (quotePM.ShipmentTypeId == "FCL" || quotePM.ShipmentTypeId == "FCLD" || quotePM.ShipmentTypeId == "FTL")
                {
                    templateSections = templateSections.Where(t => t.QuoteTemplateSectionTypeCode != "PP").ToList(); ;
                }
            }


            if (includeHeaderFooter)
            {
                QuoteTemplateSectionPM headerSection = templateSections.Where(s => s.QuoteTemplateSectionTypeCode == "PH" && !s.IsExcluded).FirstOrDefault();
                SetSectionTypeCode(quoteTemplateBuildArges,"PH");
                byte[] headerdata = GetQuoteTemplatePageHeaderFooter(quoteTemplateBuildArges);
                if (headerdata != null)
                {
                    htmlString += GetBodyString(headerdata);

                }
            }

            quoteTemplateBuildArges.FirstSectionInBody =true;
            foreach (QuoteTemplateSectionPM section in templateSections.Where(s => s.QuoteTemplateSectionTypeCode != "PF" && s.QuoteTemplateSectionTypeCode != "PH" && !s.IsExcluded).OrderBy(x => x.Order).ToList())
            {
             
                if (section.QuoteTemplateSectionTypeCode == "PP" || section.QuoteTemplateSectionTypeCode == "PC")
                {
                    SetSectionTypeCode(quoteTemplateBuildArges, section.QuoteTemplateSectionTypeCode);
                    byte[] pricingdata = GetQuoteTemplatePricingHtmlData(quoteTemplateBuildArges);

                    if (pricingdata != null && pricingdata.Length>0)
                    {
                        htmlString += GetBodyString(pricingdata);
                    }
                    
                }
                else if (section.QuoteTemplateSectionTypeCode == "QD" || section.QuoteTemplateSectionTypeCode == "QH")
                {
                    SetSectionTypeCode(quoteTemplateBuildArges, section.QuoteTemplateSectionTypeCode);

                    if (section.QuoteTemplateSectionTypeCode == "QH")
                    {
                        if (!quoteTemplateBuildArges.HideQuoteHeaderFromPdf)
                        {
                            byte[] quoteHeaderdata = GetQuoteTemplateHeader(quoteTemplateBuildArges);
                            if (quoteHeaderdata != null)
                            {
                                htmlString += GetBodyString(quoteHeaderdata);
                            }
                        }

                    }
                    else if (section.QuoteTemplateSectionTypeCode == "QD")
                    {
                        byte[] quoteDetailrdata = GetQuoteTemplateDetails(quoteTemplateBuildArges);
                        if (quoteDetailrdata != null)
                        {
                            htmlString += GetBodyString(quoteDetailrdata);

                        }


                    }

                }
                else if (section.QuoteTemplateSectionTypeCode == "PB") htmlString += "<p style='page-break-after:always;'> <span style=visibility:collapse>Page Break</span></p>";
                else
                {
                    byte[] sectiondata = DownloadQuoteTemplateSectionDataFile(section.SectionDocId, tenant);
                    if (sectiondata != null)
                    {
                        htmlString += GetBodyString(sectiondata);
                    }
                }

                if (quoteTemplateBuildArges.FirstSectionInBody == true && (htmlString.Contains("FirstSectionInBody") || section.QuoteTemplateSectionTypeCode == "S" || section.QuoteTemplateSectionTypeCode == "PB")) quoteTemplateBuildArges.FirstSectionInBody = false;

            }


            if (includeHeaderFooter)
            {
                SetSectionTypeCode(quoteTemplateBuildArges, "PF");
                QuoteTemplateSectionPM footerSection = templateSections.Where(s => s.QuoteTemplateSectionTypeCode == "PF" && !s.IsExcluded).FirstOrDefault();
                byte[] footerdata = GetQuoteTemplatePageHeaderFooter(quoteTemplateBuildArges);
                htmlString +=  GetBodyString(footerdata);
            }

            return Encoding.UTF8.GetBytes(htmlString);
        }

        public byte[] GetQuoteTemplatePricingHtmlData(QuoteTemplateBuildArges quoteTemplateBuildArges )
        {


            QuotePM quotePM = quoteTemplateBuildArges.QuotePM;
            QuoteTemplatePM template = quoteTemplateBuildArges.QuoteTemplatePM;
            QuoteTemplateSettingPM setting = quoteTemplateBuildArges.QuoteTemplateSettingPM;
            List<QuoteTemplateTextDesignPM> quoteTemplateTextDesignsList = quoteTemplateBuildArges.QuoteTemplateTextDesignPMLists;
            List<QuoteTemplateTableDesignPM> quoteTemplateTableDesignsList = quoteTemplateBuildArges.QuoteTemplateTableDesignsLists;
            List<QuoteTemplateTextCodePM> textcodes = quoteTemplateBuildArges.QuoteTemplateTextCodePMLists;
            int tenant = quoteTemplateBuildArges.Tenant;
            int? userTenant = quoteTemplateBuildArges.UserTenant;
            string pricingSectionType = quoteTemplateBuildArges.SectionTypeCode;
            string email = HttpContext.Current.User.Identity.Name;
            int tenantNumber = userTenant != null ? (int)userTenant : tenant;
            LocalCurrencyCode = GetLocalCurrencyCode(tenantNumber, email);

            if (quotePM == null) quotePM = BuildingQuotePM();

            if (textcodes == null || textcodes.Count == 0)
            {
                QuoteTemplateTextCodeQuery quoteTemplateTextCodeQuery = new QuoteTemplateTextCodeQuery(tenant);
                textcodes = quoteTemplateTextCodeQuery.GetQuoteTemplateTextCodePMsByQuoteTemplateId(template.Tenant, template.Id).ToList();
                quoteTemplateBuildArges.QuoteTemplateTextCodePMLists = textcodes;
            }


            ChargesGroupQuery chargesGroupQuery = new ChargesGroupQuery(tenant);
            List<ChargesGroupList> chargesGroupLists = chargesGroupQuery.GetChargesGroupListsByTenant(tenant).ToList();

            StringBuilder HtmlTemplate = new StringBuilder();



            IsShowlanguage = setting.ShowLocalLanguage;
            var isSplitChargesbyGroups = (pricingSectionType == "PP" && setting.SplitChargesbyGroupsPackages) ? true : (pricingSectionType == "PC" && setting.SplitChargesbyGroupsContainers) ? true : false;
            var isRoutingRates = template != null ? template.TemplateTypeCode == "P" ? true : false : false;
            bool isShowPerContainers = pricingSectionType == "PC" && quotePM.TotalPerContainer && !string.IsNullOrEmpty(setting.TotalPerContainersTableDesignId) ? true : false;
            if (setting != null && isRoutingRates) setting.ShowUnitsPackages = false;


            List<QuoteSaleChargePM> QuoteSaleChargePricingTableLists = quotePM.QuoteSaleCharges;
            List<QuoteSaleChargePM> QuoteSaleChargePerContainersLists = quotePM.QuoteSaleCharges;

            if (IsShowIncludedChargesPricingTable(pricingSectionType, setting)) QuoteSaleChargePricingTableLists = QuoteSaleChargePricingTableLists.Concat(quotePM.QuotationSaleCharges).ToList();
            if (setting.ShowIncludedChargesPerContainers) QuoteSaleChargePerContainersLists = QuoteSaleChargePerContainersLists.Concat(quotePM.QuotationSaleCharges).ToList();
            if (setting.ShowFixedPriceContainers) ViewFixedPrice = IsShowFixedPriceContainer(QuoteSaleChargePricingTableLists);
            SetQuoteTemplateSettingShowSaleMaxMinAmount(setting, pricingSectionType, QuoteSaleChargePricingTableLists);
            GetCountHeader(setting, quotePM, pricingSectionType);


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
               
                QuoteTemplateTableDesignPM quotetemplatetableDesignPM = quoteTemplateTableDesignsList.Where(t => t.Id == tableDesignId).FirstOrDefault(); 
                QuoteTemplateTextDesignPM quotetemplateTextDesignPMPricingTitle = quoteTemplateTextDesignsList.Where(t => t.Id == titleDesignId).FirstOrDefault();
                QuoteTemplateTextDesignPM quotetemplateTextDesignPMHeader = quoteTemplateTextDesignsList.Where(t => t.Id == quotetemplatetableDesignPM.HeaderDesignId).FirstOrDefault();
                QuoteTemplateTextDesignPM quoteTemplateTextDesignLines = quoteTemplateTextDesignsList.Where(t => t.Id == quotetemplatetableDesignPM.LinesDesignId).FirstOrDefault();
                isRightToLeft = setting.RightToLeft;
                int line = pricingSectionType == "PP" ? setting.SpaceLinesBeforePackages : setting.SpaceLinesBeforeContainers;
                HtmlTemplate.Append(GetHtmlStringLine(line, quoteTemplateBuildArges.FirstSectionInBody));


                if ((pricingSectionType == "PP" && setting.ShowTitlePricingPackages) || (pricingSectionType == "PC" && setting.ShowTitlePricingContainsers))
                {
                    BuildPricingTitle(HtmlTemplate, quotetemplateTextDesignPMPricingTitle, pricingSectionType, textcodes, setting.RightToLeft);

                    HtmlTemplate.Append("<div  style='height:8px;'>" + " &nbsp;  &nbsp; &nbsp; &nbsp; &nbsp;  &nbsp;   &nbsp;" + "</div>");
                }

                string StyleTable = GetStyleTable(quotetemplatetableDesignPM);
                string dir = "";
                if (setting.RightToLeft) dir = "dir='RTL'";

                if (quotetemplatetableDesignPM.BorderTypeCode == "NONE")
                {
                    HtmlTemplate.Append("<div  align='center'>");
                    HtmlTemplate.Append("<table style='border-collapse: collapse;'  width='100%' " + dir + " >");
                }
                else
                {
                    HtmlTemplate.Append("<div>");

                    HtmlTemplate.Append("<table " + dir + " width='100%' " + StyleTable + " >");
                }


                IEnumerable<IGrouping<string, QuoteSaleChargePM>> chargegroups = QuoteSaleChargePricingTableLists.GroupBy(q => q.ChargesGroupCode).OrderBy(d => d.ToList().FirstOrDefault().ChargesGroupViewOrder).ThenBy(n => n.ToList().FirstOrDefault().ChargesTypeViewOrder).ThenBy(d => d.ToList().FirstOrDefault().ChargesGroupName).ToList();

                bool showHeaderLabels = pricingSectionType == "PC" ? setting.ShowHeaderLabelsContainers : setting.ShowHeaderLabelsPackages;

                if (showHeaderLabels)
                {
                    buildHeadercolumn(HtmlTemplate, setting, quotetemplateTextDesignPMHeader, quotetemplatetableDesignPM, quotePM, pricingSectionType, setting.RightToLeft, textcodes);
                }

                #region  Table

                if ((pricingSectionType == "PP" && setting.SplitChargesbyGroupsPackages) || (pricingSectionType == "PC" && setting.SplitChargesbyGroupsContainers))
                {

                    QuoteTemplateTextDesignPM quoteTemplateTextDesignPMGroupByHeader = pricingSectionType == "PP" ? quoteTemplateTextDesignsList.Where(t => t.Id == setting.GroupByPackagesValueDesignId).FirstOrDefault() : quoteTemplateTextDesignsList.Where(t => t.Id == setting.GroupByContainsersValueDesignId).FirstOrDefault();
                    string groupByHeaderStyle = GetSpanRowStyle(quoteTemplateTextDesignPMGroupByHeader, "Total");

                    string StyleTrHeaderGroupBy = GetStyleGroupByTd("", quotetemplatetableDesignPM.BorderColor, quotetemplatetableDesignPM.BorderThickness, quotetemplatetableDesignPM.BorderTypeCode, quoteTemplateTextDesignPMGroupByHeader, setting.RightToLeft);

                    foreach (IGrouping<string, QuoteSaleChargePM> chargegroup in chargegroups)
                    {
                        ChargesGroupList group = chargesGroupLists.Where(d => d.Code == chargegroup.Key).FirstOrDefault();

                        string chargeGroupName = setting.ShowLocalLanguage ? group.LocalName : group.Name;
                        string textValue = quoteTemplateTextDesignPMGroupByHeader.Italic ? "</i>" + "<span " + groupByHeaderStyle + " > " + chargeGroupName + "&nbsp" + "</span>" + "</i>" : "<span " + groupByHeaderStyle + "> " + chargeGroupName + "&nbsp" + "</span>";

                        HtmlTemplate.Append("<tr  style= 'height:auto; width:auto;vertical-align:central'>" + "<td " + StyleTrHeaderGroupBy + "colspan=' " + TdCount.ToString() + "';" + ">" + textValue + " </td>" + "</tr>");

                        List<QuoteSaleChargePM> charges = chargegroup.ToList();
                        BuildTableRows(charges, HtmlTemplate, quoteTemplateTextDesignLines, quotetemplatetableDesignPM, quoteTemplateBuildArges);
                        bool showTotalPerChargeGroup = pricingSectionType == "PP" ? setting.ShowTotalPerChargeGroupPackages : setting.ShowTotalPerChargeGroupContainers;
                        if (showTotalPerChargeGroup)
                        {
                            BuildTotalPerChargeGroup(quoteTemplateBuildArges, HtmlTemplate, charges);

                        }
                    }

                }
                else
                {
                    BuildTableRows(QuoteSaleChargePricingTableLists, HtmlTemplate, quoteTemplateTextDesignLines, quotetemplatetableDesignPM, quoteTemplateBuildArges);
                    HtmlTemplate.Append("</tr>");
                }
                
                HtmlTemplate.Append("</table>");

                #endregion

                if (!isShowPerContainers && !isRoutingRates)
                {
                    BuilQuoteTotalCurrency(quoteTemplateBuildArges, HtmlTemplate);
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
                BuildTotalPerContainers(quoteTemplateBuildArges,  chargesGroupLists, HtmlTemplate, QuoteSaleChargePerContainersLists);
            }

            #endregion


            return Encoding.UTF8.GetBytes(HtmlTemplate.ToString());
        }

        private void BuildTotalPerContainers(QuoteTemplateBuildArges quoteTemplateBuildArges, List<ChargesGroupList> chargesGroupLists, StringBuilder HtmlTemplate, List<QuoteSaleChargePM> QuoteSaleChargePerContainersLists)
        {
            int tenant = quoteTemplateBuildArges.Tenant;
            bool exist = SecurityUtility.CheckFeature("Quote", "TOTALPERCONTAINER", tenant);
            if (exist)
            {
                QuotePM quotePM = quoteTemplateBuildArges.QuotePM;
                QuoteTemplateSettingPM setting = quoteTemplateBuildArges.QuoteTemplateSettingPM;
                List<QuoteTemplateTextDesignPM> quoteTemplateTextDesignsList = quoteTemplateBuildArges.QuoteTemplateTextDesignPMLists;
                List<QuoteTemplateTextCodePM> textcodes = quoteTemplateBuildArges.QuoteTemplateTextCodePMLists;
                List<QuoteTemplateTableDesignPM> quoteTemplateTableDesignsList = quoteTemplateBuildArges.QuoteTemplateTableDesignsLists;
                string pricingSectionType = quoteTemplateBuildArges.SectionTypeCode;


                bool showIncludedChargesPerContainers = setting.ShowIncludedChargesPerContainers;
                List<TotalPerContainerClass> totalPerContainerClassLists = new List<TotalPerContainerClass>();
                BuildTotalPerContainerClassLists(quotePM, QuoteSaleChargePerContainersLists, HtmlTemplate, setting, totalPerContainerClassLists);


                IEnumerable<IGrouping<string, TotalPerContainerClass>> totalPerContainerGroupingListsByGroupCode = totalPerContainerClassLists.GroupBy(q => q.ChargeGroupCode).OrderBy(d => d.ToList().FirstOrDefault().ChargesGroupViewOrder).ThenBy(n => n.ToList().FirstOrDefault().ChargesTypeViewOrder).ThenBy(d => d.ToList().FirstOrDefault().ChargesGroupName).ToList();

                IEnumerable<IGrouping<string, TotalPerContainerClass>> totalPerContainerGroupingListsByFieldCode = totalPerContainerClassLists.GroupBy(q => q.FieldCode);
                QuoteTemplateTextDesignPM totalPerContainersAdditionalTextDesign = quoteTemplateTextDesignsList.Where(t => t.Id == setting.TotalPerContainersAdditionalTextDesignId).FirstOrDefault();
                QuoteTemplateTableDesignPM totalPerContainersTableDesign = quoteTemplateTableDesignsList.Where(t => t.Id == setting.TotalPerContainersTableDesignId).FirstOrDefault();
                QuoteTemplateTextDesignPM totalPerContainersTableHeader = quoteTemplateTextDesignsList.Where(t => t.Id == totalPerContainersTableDesign.HeaderDesignId).FirstOrDefault();
                QuoteTemplateTextDesignPM totalPerContainersTableLines = quoteTemplateTextDesignsList.Where(t => t.Id == totalPerContainersTableDesign.LinesDesignId).FirstOrDefault();

                if (totalPerContainerGroupingListsByGroupCode.Count() > 0)
                {
                    string translateInclueLable = GetTranslateInclueLable(quoteTemplateBuildArges.QuoteTemplateTextCodePMLists, "TotalPerContainers");


                    if (setting.ShowPageBreakBeforeTotalPerContainersTable)
                    {
                        HtmlTemplate.Append("<p style='page-break-after:always;'> <span style=visibility:collapse>Page Break</span></p>");
                    }

                    HtmlTemplate.Append(GetHtmlStringLine(setting.SpaceLinesBeforePerContainers, quoteTemplateBuildArges.FirstSectionInBody));

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
                    foreach (IGrouping<string, TotalPerContainerClass> totalPerContainer in totalPerContainerGroupingListsByFieldCode)
                    {
                        TotalPerContainerClass totalPerContainerClass = totalPerContainer.FirstOrDefault();
                        if (totalPerContainerClass != null)
                        {
                            HtmlTemplate.Append(BuildTableColumn(totalPerContainerClass.Name, totalPerContainersTableHeader, totalPerContainersTableDesign, "Header", null, setting.RightToLeft, true));
                        }

                    }
                    HtmlTemplate.Append("</tr>");

                    List<TotalPerContainerClass> totals = new List<TotalPerContainerClass>();
                    foreach (IGrouping<string, TotalPerContainerClass> totalPerContainer in totalPerContainerGroupingListsByGroupCode)
                    {
                        bool includedChargesPerContainers = !(totalPerContainer.Where(d => !d.Included).Any());
                        HtmlTemplate.Append("<tr style= 'height:auto; width:auto;vertical-align:central'>");
                        IEnumerable<IGrouping<string, TotalPerContainerClass>> totalPerContainerGroupByFieldCode = totalPerContainer.GroupBy(q => q.FieldCode);

                        ChargesGroupList group = chargesGroupLists.Where(d => d.Code == totalPerContainer.Key).FirstOrDefault();
                        string chargeGroupName = setting.ShowLocalLanguage ? group.LocalName : group.Name;
                        HtmlTemplate.Append(BuildTableColumn(chargeGroupName, totalPerContainersTableLines, totalPerContainersTableDesign, "Field", null, setting.RightToLeft));
                        foreach (IGrouping<string, TotalPerContainerClass> totalPerContainerGroup in totalPerContainerGroupByFieldCode)
                        {
                            if (!includedChargesPerContainers)
                            {
                                List<TotalPerContainerClass> totalPerContainersLists = totalPerContainerGroup.ToList();
                                double value = 0;
                                foreach (TotalPerContainerClass item in totalPerContainersLists)
                                {
                                    value += item.Value;

                                }

                                if (setting.TotalPerContainersCurrencyType == "LOCAL")
                                {
                                    value = value * (double)totalPerContainersLists[0].SaleExchangeRate;
                                }
                                totals.Add(new TotalPerContainerClass() { Value = value, FieldCode = totalPerContainersLists[0].FieldCode });
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
                        AppendPerContainerTotalBySaleCurrency(quotePM, setting, HtmlTemplate, totalPerContainersTableDesign, totalPerContainersTableLines, totals);
                    }
                    if (setting.TotalPerContainersCurrencyType == "MULTIPLE")
                    {
                        HtmlTemplate.Append("<tr style= 'height:auto; width:auto;vertical-align:central'>");
                        HtmlTemplate.Append(BuildTableColumn("Total", totalPerContainersTableLines, totalPerContainersTableDesign, "Field", null, setting.RightToLeft));
                        foreach (IGrouping<string, TotalPerContainerClass> totalPerContainer in totalPerContainerGroupingListsByFieldCode)
                        {

                            List<TotalPerContainerClass> items = new List<TotalPerContainerClass>();
                            IEnumerable<IGrouping<string, TotalPerContainerClass>> totalPerContainersGroupByCurrencyCodeLists = totalPerContainer.ToList().GroupBy(d => d.CurrencyCode).ToList();
                            foreach (IGrouping<string, TotalPerContainerClass> totalPerContainersGroupByCurrencyCodeList in totalPerContainersGroupByCurrencyCodeLists)
                            {

                                var totalPerContainersGroupByCurrencyCode = totalPerContainersGroupByCurrencyCodeList.ToList();
                                var totalPerContainerClass = new TotalPerContainerClass() { CurrencyCode = totalPerContainersGroupByCurrencyCode[0].CurrencyCode };
                                double orginalValue = 0;
                                foreach (TotalPerContainerClass item in totalPerContainersGroupByCurrencyCode)
                                {
                                    orginalValue += ((double)item.OrginalValue);
                                }
                                totalPerContainerClass.OrginalValue = orginalValue;
                                items.Add(totalPerContainerClass);
                            }


                            string value = string.Empty;
                            foreach (TotalPerContainerClass item in items.OrderBy(d => d.Name).ToList())
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

        private void BuilQuoteTotalCurrency(QuoteTemplateBuildArges quoteTemplateBuildArges , StringBuilder HtmlTemplate)
        {
           
       
            QuotePM quotePM = quoteTemplateBuildArges.QuotePM;
            QuoteTemplateSettingPM setting = quoteTemplateBuildArges.QuoteTemplateSettingPM;
            List<QuoteTemplateTextDesignPM> quoteTemplateTextDesignsList = quoteTemplateBuildArges.QuoteTemplateTextDesignPMLists;
            List<QuoteTemplateTextCodePM> textcodes = quoteTemplateBuildArges.QuoteTemplateTextCodePMLists;
            int tenant = quoteTemplateBuildArges.Tenant;
            string pricingSectionType = quoteTemplateBuildArges.SectionTypeCode;

            QuoteTemplateTextDesignPM quoteTemplateTextDesignTotalsLabel = null;
            QuoteTemplateTextDesignPM quoteTemplateTextDesignTotalsValue = null;

            if (pricingSectionType == "PC")
            {
                quoteTemplateTextDesignTotalsLabel = quoteTemplateTextDesignsList.Where(t => t.Id == setting.TotalsContainsersLabelDesignId).FirstOrDefault();//quotetemplateTextDesignQuery.GetSinglePM(setting.TotalsContainsersLabelDesignId, tenant);
                quoteTemplateTextDesignTotalsValue = quoteTemplateTextDesignsList.Where(t => t.Id == setting.TotalsContainsersValueDesignId).FirstOrDefault();//quotetemplateTextDesignQuery.GetSinglePM(setting.TotalsContainsersValueDesignId, tenant);
            }
            else
            {
                quoteTemplateTextDesignTotalsLabel = quoteTemplateTextDesignsList.Where(t => t.Id == setting.TotalsPackagesLabelDesignId).FirstOrDefault();//quotetemplateTextDesignQuery.GetSinglePM(setting.TotalsPackagesLabelDesignId, tenant);
                quoteTemplateTextDesignTotalsValue = quoteTemplateTextDesignsList.Where(t => t.Id == setting.TotalsPackagesValueDesignId).FirstOrDefault();//quotetemplateTextDesignQuery.GetSinglePM(setting.TotalsPackagesValueDesignId, tenant);
            }
            string Name = GetNameColum("TOTALAMOUNTS", textcodes, pricingSectionType);
            if (quotePM.SaleCurrencyCode == null && quotePM.SaleCurrencyId != null)
            {
                SetSaleCurrencyCode(quotePM, tenant);
            }

            string SaleTotalAmountInSaleCurrency =  GetNumberValueFormate(!quotePM.IsChargesByVAT ?quotePM.SaleTotalAmountInSaleCurrency: quotePM.TotalSaleIncludingVATAmountInSaleCurrency);
            string SaleTotalAmountInLocalCurrency = GetNumberValueFormate(!quotePM.IsChargesByVAT ? quotePM.SaleTotalAmountInLocalCurrency: quotePM.TotalSaleIncludingVATAmountInLocalCurrency);
           
            #region Total Currency Containers
            bool ShowTotalInSaleCurrency = pricingSectionType == "PC" ? setting.ShowTotalInSaleCurrencyContainers : setting.ShowTotalInSaleCurrencyPackages;
            bool ShowTotalInLocalCurrency = pricingSectionType == "PC" ? setting.ShowTotalInLocalCurrencyContainers : setting.ShowTotalInLocalCurrencyPackages;


            if (ShowTotalInSaleCurrency || ShowTotalInLocalCurrency)
            {
                HtmlTemplate.Append("<div  style='height:5px;'>" + " &nbsp;  &nbsp; &nbsp; &nbsp; &nbsp;  &nbsp;   &nbsp;" + "</div>");
            }
            AppendTotalCurrencyHtml(quotePM, setting, HtmlTemplate, ShowTotalInSaleCurrency, ShowTotalInLocalCurrency, quoteTemplateTextDesignTotalsLabel, quoteTemplateTextDesignTotalsValue,
              Name, SaleTotalAmountInSaleCurrency, SaleTotalAmountInLocalCurrency);
            #endregion


            HtmlTemplate.Append("</div>");

    
        }

        private void BuildTotalPerChargeGroup(QuoteTemplateBuildArges quoteTemplateBuildArges, StringBuilder HtmlTemplate ,  List<QuoteSaleChargePM> charges)
        {


            string pricingSectionType = quoteTemplateBuildArges.SectionTypeCode;
            IEnumerable<IGrouping<string, QuoteSaleChargePM>> QuoteSaleChargeGroup = charges.GroupBy(q => q.CurrencyCode);
            string currencyCode = "";
            double? amount = 0;

            QuoteTemplateTextDesignPM quoteTemplateTextDesignPMGroupByTotal = pricingSectionType == "PP" ? quoteTemplateBuildArges.QuoteTemplateTextDesignPMLists.Where(t => t.Id == quoteTemplateBuildArges.QuoteTemplateSettingPM.GroupByPackagesLabelDesignId).FirstOrDefault() : quoteTemplateBuildArges.QuoteTemplateTextDesignPMLists.Where(t => t.Id == quoteTemplateBuildArges.QuoteTemplateSettingPM.GroupByContainsersLabelDesignId).FirstOrDefault();
            string groupByTotalStyle = GetSpanRowStyle(quoteTemplateTextDesignPMGroupByTotal, "Lable");
       
            string tableDesignId = pricingSectionType == "PP" ? quoteTemplateBuildArges.QuoteTemplateSettingPM.PackagesTableDesignId : quoteTemplateBuildArges.QuoteTemplateSettingPM.ContainserTableDesignId;
            QuoteTemplateTableDesignPM quotetemplatetableDesignPM = quoteTemplateBuildArges.QuoteTemplateTableDesignsLists.Where(d => d.Id == tableDesignId).FirstOrDefault() ;

            string StyleTrGroupByTotal = GetStyleGroupByTd("Empty", quotetemplatetableDesignPM.BorderColor, quotetemplatetableDesignPM.BorderThickness, quotetemplatetableDesignPM.BorderTypeCode, quoteTemplateTextDesignPMGroupByTotal, quoteTemplateBuildArges.QuoteTemplateSettingPM.RightToLeft);

            string translateInclueLable = GetTranslateInclueLable(quoteTemplateBuildArges.QuoteTemplateTextCodePMLists, pricingSectionType);


            HtmlTemplate.Append("<tr  style= 'height:auto; width:auto;vertical-align:central'>");

            if (ShowSaleCurrencyColumnColumnPosition > 1)
            {
                HtmlTemplate.Append("<td " + StyleTrGroupByTotal + "colspan=' " + (ShowSaleCurrencyColumnColumnPosition - 1).ToString() + "';" + ">"); HtmlTemplate.Append("<div " + groupByTotalStyle + " >" + "<div>"); HtmlTemplate.Append("</td>");
    
            HtmlTemplate.Append("<td " + StyleTrGroupByTotal + ">");
        }

            foreach (IGrouping<string, QuoteSaleChargePM> quoteSaleCharge in QuoteSaleChargeGroup)
            {
                currencyCode = "";
                amount = 0;
                List<QuoteSaleChargePM> quoteSaleCharges = quoteSaleCharge.ToList();

                bool allChargeIncluded = !(quoteSaleCharges.Where(d => d.IsAllIN == null || (d.IsAllIN != null && d.IsAllIN.ToLower() == "false")).Any());
                foreach (QuoteSaleChargePM quoteSaleChargePM in quoteSaleCharges)
                {
                    if (quoteSaleChargePM.IsAllIN == null || (quoteSaleChargePM.IsAllIN != null && quoteSaleChargePM.IsAllIN.ToLower() == "false"))
                    {
                        currencyCode = quoteSaleChargePM.CurrencyCode;
                        amount += quoteSaleChargePM.SaleTotalAmount;
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

        private string GetTranslateInclueLable(List<QuoteTemplateTextCodePM> quoteTemplateTextCodePMLists, string pricingSectionType)
        {
           return  GetNameColum("INCLUDED", quoteTemplateTextCodePMLists, pricingSectionType);
        }

        private  void SetSaleCurrencyCode(QuotePM quotePM, int tenant)
        {
            CurrencyRepository currencyRepository = new CurrencyRepository(tenant);
            var currency = currencyRepository.GetSingleCurrency(quotePM.SaleCurrencyId, tenant);
            if (currency != null)
            {
                quotePM.SaleCurrencyCode = currency.Code;
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

        private static void SetQuoteTemplateSettingShowSaleMaxMinAmount(QuoteTemplateSettingPM setting, string pricingSectionType, List<QuoteSaleChargePM> QuoteSaleChargePricingTableLists)
        {
            bool isHaveMaxMinValue = QuoteSaleChargePricingTableLists.Where(d => d.SaleMaxAmount != null || d.SaleMinAmount != null).Any();
            if (!isHaveMaxMinValue)
            {
                if (pricingSectionType == "PP") setting.ShowSaleMaxMinAmountPackages = false;
                else if (pricingSectionType == "PC") setting.ShowSaleMaxMinAmountContainers = false;
            }
        }

        private static void SetQuoteSaleChargeRelatedOrderFields( List<QuoteSaleChargePM> QuoteSaleChargePerContainersLists, List<ChargesGroupList> chargesGroupLists, List<ChargesTypeList> chargesTypeLists)
        {

            foreach (QuoteSaleChargePM item in QuoteSaleChargePerContainersLists)
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

        private List<ChargeGroupOrderList> BuildChargeGroupOrderList(List<ChargesGroupList> chargesGroups , int tenant)
        {
            ChargesTypeQuery chargesTypeQuery = new ChargesTypeQuery(tenant);
            var chargesTypeLists = chargesTypeQuery.GetChargesTypeListsByTenant(tenant);

            List<ChargeGroupOrderList> items = new List<ChargeGroupOrderList>();
            foreach (ChargesGroupList item in chargesGroups)
            {
                var chargesTypeList = chargesTypeLists.Where(d => d.ChargesGroupId == item.Id).FirstOrDefault();
                items.Add(new ChargeGroupOrderList() { GroupOrder = item.ViewOrder,Name = item.Name, ChargeTypeOrder= chargesTypeList!=null? chargesTypeList.ViewOrder: item.ViewOrder,Code = item.Code });
            }

            int i = 1;
            foreach (var s in items.OrderBy(c => c.GroupOrder).ThenBy(n => n.ChargeTypeOrder).ThenBy(d => d.Name).ToList())
            {
                s.Order = i;
                i += 1;
            }
           
            return items;
        }

        private bool IsShowIncludedChargesPricingTable(string pricingSectionType, QuoteTemplateSettingPM setting)
        {
            return pricingSectionType == "PP" ? setting.ShowIncludedChargesPackages: setting.ShowIncludedChargesContainers;

        }

        private void AppendPerContainerTotalBySaleCurrency(QuotePM quotePM, QuoteTemplateSettingPM setting,  StringBuilder HtmlTemplate, QuoteTemplateTableDesignPM totalPerContainersTableDesign, QuoteTemplateTextDesignPM totalPerContainersTableLines, List<TotalPerContainerClass> totals)
        {
            HtmlTemplate.Append("<tr style= 'height:auto; width:auto;vertical-align:central'>");

            int tenant = setting.Tenant;
            string totalPerContainersCurrencyLable = setting.TotalPerContainersCurrencyType != "LOCAL" ? "Total By Sale Currency" : "Total By Local Currency";

            HtmlTemplate.Append(BuildTableColumn(totalPerContainersCurrencyLable, totalPerContainersTableLines, totalPerContainersTableDesign, "Field", null, setting.RightToLeft));

            TenantPM tenantPM = TenantQuery.GetSingleTenantPM(tenant, true);
            IEnumerable<IGrouping<string, TotalPerContainerClass>> totalPerList = totals.GroupBy(q => q.FieldCode);
            foreach (IGrouping<string, TotalPerContainerClass> totalPer in totalPerList)
            {
                List<TotalPerContainerClass> totalPerContainersLists = totalPer.ToList();
                double value = 0;
                foreach (TotalPerContainerClass item in totalPerContainersLists)
                {
                    value += item.Value;

                }

                string currencyCode = setting.TotalPerContainersCurrencyType == "LOCAL" ? tenantPM.CurrencyCode : quotePM.SaleCurrencyCode;

                HtmlTemplate.Append(BuildTableColumn(value.ToString("N") + " " + currencyCode, totalPerContainersTableLines, totalPerContainersTableDesign, "Field", null, setting.RightToLeft, true));
            }
            HtmlTemplate.Append("</tr>");
        }

        private void AppendTotalCurrencyHtml(QuotePM quotePM, QuoteTemplateSettingPM setting, StringBuilder HtmlTemplate, bool showTotalInSaleCurrency, bool showTotalInLocalCurrency, QuoteTemplateTextDesignPM quoteTemplateTextDesignTotalsLabel, QuoteTemplateTextDesignPM quoteTemplateTextDesignTotalsValue, string Name, string SaleTotalAmountInSaleCurrency, string SaleTotalAmountInLocalCurrency)
        {

            string dir = "";
            string alignContent = quoteTemplateTextDesignTotalsLabel.Alignment;
            if (setting.RightToLeft) dir = "dir='RTL'";
            string totalInSaleCurrency = BuildTotalInSale(Name + " : ", quoteTemplateTextDesignTotalsLabel, false) + BuildTotalInSale(SaleTotalAmountInSaleCurrency + " " + quotePM.SaleCurrencyCode, quoteTemplateTextDesignTotalsValue, false, true);
            string totalInLocalCurrency = BuildTotalInSale(Name + " : ", quoteTemplateTextDesignTotalsLabel, true) + BuildTotalInSale(SaleTotalAmountInLocalCurrency + " " + LocalCurrencyCode, quoteTemplateTextDesignTotalsValue, false, true);

            if (!quotePM.IsSaleCurrencySameAsCost)
            {
                if (showTotalInSaleCurrency && showTotalInLocalCurrency)
                {
                    HtmlTemplate.Append("<div " + dir + " style='display:block;text-align:"+ alignContent + ";'>" + totalInSaleCurrency + "</div>");
                    if (quotePM.SaleCurrencyCode != LocalCurrencyCode) HtmlTemplate.Append("<div " + dir + " style='display:block;text-align:" + alignContent + ";'>" + totalInLocalCurrency + "</div>");
                }
                else if (showTotalInSaleCurrency)
                {
                    totalInSaleCurrency = BuildTotalInSale(Name + " : ", quoteTemplateTextDesignTotalsLabel, false) + BuildTotalInSale(SaleTotalAmountInSaleCurrency + " " + quotePM.SaleCurrencyCode, quoteTemplateTextDesignTotalsValue, false, true);
                    HtmlTemplate.Append("<div " + dir + " style='display:block;text-align:" + alignContent + ";'>" + totalInSaleCurrency + "</div>");
                }
                else if (showTotalInLocalCurrency)
                {
                    totalInLocalCurrency = BuildTotalInSale(Name + " : ", quoteTemplateTextDesignTotalsLabel, false) + BuildTotalInSale(SaleTotalAmountInLocalCurrency + " " + LocalCurrencyCode, quoteTemplateTextDesignTotalsValue, false, true);
                    HtmlTemplate.Append("<div " + dir + " style='display:block;text-align:" + alignContent + ";'>" + totalInLocalCurrency + "</div>");
                }
            }

            else
            {
                string LocalCurrencyName = Name;
                if (showTotalInSaleCurrency)
                {
                    List<QuoteSalesTotalPM> QuoteSalesTotals = ComputeQuoteSalesTotals(quotePM);
                    bool isHideTitle = false;
                    foreach (QuoteSalesTotalPM item in QuoteSalesTotals.OrderByDescending(d => d.Amount))
                    {

                        string amountValue = " ";
                        if (item.Amount != null)
                        {
                            double value = (double)item.Amount;
                            amountValue = value.ToString("N"); // 1,234.512
                        }

                        string total = BuildTotalInSale(Name + " : ", quoteTemplateTextDesignTotalsLabel, isHideTitle) + BuildTotalInSale(amountValue.ToString() + " " + item.CurrencyCode, quoteTemplateTextDesignTotalsValue, false, true);
                        HtmlTemplate.Append("<div " + dir + " style='display:block;text-align:" + alignContent + ";'>" + total + " </div>");
                        isHideTitle = true;
                    }

                    LocalCurrencyName = "Estimated total (" + LocalCurrencyCode + ")";

                    if (showTotalInLocalCurrency)
                    {

                        totalInLocalCurrency = BuildTotalInSale(LocalCurrencyName, quoteTemplateTextDesignTotalsLabel, false) + BuildTotalInSale(" : " + SaleTotalAmountInLocalCurrency, quoteTemplateTextDesignTotalsValue, false);
                        HtmlTemplate.Append("<div " + dir + " style='display:block;text-align:" + alignContent + ";'>" + totalInLocalCurrency + "</div>");
                    }

                }
                else
                {

                    if (showTotalInLocalCurrency)
                    {
                        totalInLocalCurrency = BuildTotalInSale(LocalCurrencyName + " : ", quoteTemplateTextDesignTotalsLabel, false) + BuildTotalInSale(SaleTotalAmountInLocalCurrency + " " + LocalCurrencyCode, quoteTemplateTextDesignTotalsValue, false, true);
                        HtmlTemplate.Append("<div " + dir + " style='display:block;text-align:" + alignContent + ";'>" + totalInLocalCurrency + "</div>");

                    }




                }


            }
        }

        private List<QuoteSalesTotalPM> ComputeQuoteSalesTotals(QuotePM quotePM)
        {
            IEnumerable<IGrouping<string, QuoteSaleChargePM>> quoteSaleChargesGroupLists = quotePM.QuoteSaleCharges.GroupBy(q => q.CurrencyCode);
            List<QuoteSalesTotalPM> QuoteSalesTotals = new List<QuoteSalesTotalPM>();
            foreach (IGrouping<string, QuoteSaleChargePM> quoteSaleChargesGrop in quoteSaleChargesGroupLists)
            {
                string currencyCode = "";
                double? amount = 0;
                List<QuoteSaleChargePM> quoteSaleCharges = quoteSaleChargesGrop.ToList();
                foreach (QuoteSaleChargePM quoteSaleChargePM in quoteSaleCharges)
                {
                    currencyCode = quoteSaleChargePM.CurrencyCode;

                    double vatAmount = quoteSaleChargePM.VatAmount != null ? (double)quoteSaleChargePM.VatAmount : 0;
                    amount += ((quoteSaleChargePM.SaleTotalAmount != null ? quoteSaleChargePM.SaleTotalAmount : 0) + (quotePM.IsChargesByVAT ? vatAmount : 0 ));

                }
                QuoteSalesTotals.Add(new QuoteSalesTotalPM() { CurrencyCode = currencyCode, Amount = amount });

            }

            return QuoteSalesTotals;
        }

        #region QuoteH eader and Details Table

        public byte[] GetQuoteTemplateHeader(QuoteTemplateBuildArges quoteTemplateBuildArges)
        {


            QuotePM quotePM = quoteTemplateBuildArges.QuotePM;
            QuoteTemplatePM template = quoteTemplateBuildArges.QuoteTemplatePM;
            QuoteTemplateSettingPM setting = quoteTemplateBuildArges.QuoteTemplateSettingPM;
            List<QuoteTemplateTextDesignPM> quoteTemplateTextDesignsList = quoteTemplateBuildArges.QuoteTemplateTextDesignPMLists;
            List<QuoteTemplateTableDesignPM> quoteTemplateTableDesignsList = quoteTemplateBuildArges.QuoteTemplateTableDesignsLists;
            List<QuoteTemplateTextCodePM> textcodes = quoteTemplateBuildArges.QuoteTemplateTextCodePMLists;
            int tenant = quoteTemplateBuildArges.Tenant;

            string sessiontype = quoteTemplateBuildArges.SectionTypeCode == "PH" ? "Header" : "Footer";


            List<ObjectField> customFields = ObjectFieldRepository.GetCustomObjectFieldsByObjectTableName("Quote", tenant).ToList();

            if (quotePM == null)
            {
                quotePM = BuildingQuotePM();
            }

            if (quoteTemplateBuildArges.VersionNumber != null)
            {
                quotePM.QuoteVersion =  quotePM.QuoteNumber + "-" + (int)quoteTemplateBuildArges.VersionNumber;   
            }


            Tenant = quotePM.Tenant;
            string FieldValue = "";
            string FieldName = "";
            string dir = "";
            isRightToLeft = setting.RightToLeft;
            if (setting.RightToLeft) dir = "dir='RTL'";

            QuoteTemplateHeaderFieldQuery quoteTemplateHeaderFieldQuery = new QuoteTemplateHeaderFieldQuery(tenant);
            IQueryable<QuoteTemplateHeaderFieldPM> QuoteTemplaetHeaderFieldList = quoteTemplateHeaderFieldQuery.GetQuoteTemplateHeaderFieldPMsByQuotetemplateId(tenant, template.Id);
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
                if (quoteTemplateBuildArges.RequestArea != "Header")
                {
                    HtmlTemplate.Append(GetHtmlStringLine(setting.SpaceLinesBeforeQuoteHeaders, quoteTemplateBuildArges.FirstSectionInBody));
                }


                List<QuoteTemplateHeaderFieldPM> QuoteTemplaetHeaderColum0;
                List<QuoteTemplateHeaderFieldPM> QuoteTemplaetHeaderColum1;

                QuoteTemplaetHeaderColum0 = QuoteTemplaetHeaderFieldList.Where(d => d.Column == 0).OrderBy(d => d.Row).ToList();
                QuoteTemplaetHeaderColum1 = QuoteTemplaetHeaderFieldList.Where(d => d.Column == 1).OrderBy(d => d.Row).ToList();

                IsShowlanguage = setting.ShowLocalLanguage;

                QuoteTemplateTableDesignPM quotetemplatetableDesignPM = quoteTemplateTableDesignsList.Where(t => t.Id == setting.HeaderTableDesignId).FirstOrDefault();
                QuoteTemplateTextDesignPM quotetemplateTextDesignPMHeaderLable = quoteTemplateTextDesignsList.Where(t => t.Id == quotetemplatetableDesignPM.HeaderDesignId).FirstOrDefault();
                QuoteTemplateTextDesignPM quotetemplateTextDesignPMHeaderValue = quoteTemplateTextDesignsList.Where(t => t.Id == quotetemplatetableDesignPM.LinesDesignId).FirstOrDefault();

                string styleTable = GetStyleTable(quotetemplatetableDesignPM);

                string StyleQuoteHeaderLable = GetsyleSpanQuoteHeaderDetails(quotetemplateTextDesignPMHeaderLable);
                string styleTd = "";
                string widthtable = "";
                bool IsHeaderTableAuto = setting.HeaderTableColumWidthType == "AUTO" ? true : false;

                string StyleQuoteHeaderValue = GetsyleSpanQuoteHeaderDetails(quotetemplateTextDesignPMHeaderValue);

                string HeaderTableColumn1LabelWidth = IsHeaderTableAuto ? "Auto" : (setting.HeaderTableColumn1LabelWidth.ToString() + "%");
                string HeaderTableColumn1ValueWidth = IsHeaderTableAuto ? "Auto" : (setting.HeaderTableColumn1ValueWidth.ToString() + "%");
                string HeaderTableColumn2LabelWidth = IsHeaderTableAuto ? "Auto" : (setting.HeaderTableColumn2LabelWidth.ToString() + "%");
                string HeaderTableColumn2ValueWidth = IsHeaderTableAuto ? "Auto" : (setting.HeaderTableColumn2ValueWidth.ToString() + "%");

                double? W = setting.HeaderTableColumn1LabelWidth + setting.HeaderTableColumn1ValueWidth;
                if (setting.HeaderSectionHasTwoColumns) W += (setting.HeaderTableColumn2LabelWidth + setting.HeaderTableColumn2ValueWidth);
                widthtable = W.ToString() + "%";
                HtmlTemplate.Append("<div " + dir + " style ='width=100%' >");

                if (quotetemplatetableDesignPM.BorderTypeCode == "NONE") styleTable = "";
                if (IsHeaderTableAuto) HtmlTemplate.Append("<table   " + dir + " width='Auto' " + styleTable + " >");
                else HtmlTemplate.Append("<table " + dir + " width='" + widthtable + "'" + styleTable + " >");




                if (!setting.HeaderSectionHasTwoColumns)
                {
                    foreach (QuoteTemplateHeaderFieldPM Feild in QuoteTemplaetHeaderColum0)
                    {
                        HtmlTemplate.Append("<tr>");
                        FieldName = "";
                        FieldName = GetNameColum(Feild.FieldCode, textcodes, "QH");
                        if (string.IsNullOrEmpty(FieldName)) FieldName = TranslateTextsClass.Translate(Feild.FieldCode, Feild.Tenant);
                        if ((quotetemplatetableDesignPM.BorderTypeCode == "NONE"))
                        {
                            if (!string.IsNullOrEmpty(FieldName)) FieldName += " :";
                        }

                        FieldValue = GetQuoteTemplateHeaderFieldValue(Feild.FieldCode, quotePM);
                        if (FieldValue == "CustomField") FieldValue = GetCustomFelidValue(Feild.FieldCode, customFields, quotePM);

                        //Lable
                        styleTd = GetStyleRowTable(quotetemplateTextDesignPMHeaderLable, quotetemplatetableDesignPM, HeaderTableColumn1LabelWidth);
                        AppendNewQuoteTableoRow(FieldName, HtmlTemplate, quotetemplateTextDesignPMHeaderLable, StyleQuoteHeaderLable, styleTd);

                        //Value
                        styleTd = GetStyleRowTable(quotetemplateTextDesignPMHeaderValue, quotetemplatetableDesignPM, HeaderTableColumn1ValueWidth);
                        AppendNewQuoteTableoRow(FieldValue, HtmlTemplate, quotetemplateTextDesignPMHeaderValue, StyleQuoteHeaderValue, styleTd);

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
                        foreach (QuoteTemplateHeaderFieldPM Feild in QuoteTemplaetHeaderColum0)
                        {
                            HtmlTemplate.Append("<tr>");


                            FieldName = "";
                            FieldName = GetNameColum(Feild.FieldCode, textcodes, "QH");

                            if (string.IsNullOrEmpty(FieldName)) FieldName = TranslateTextsClass.Translate(Feild.FieldCode, Feild.Tenant);

                            if ((quotetemplatetableDesignPM.BorderTypeCode == "NONE"))
                            {
                                if (!string.IsNullOrEmpty(FieldName)) FieldName += " :";
                            }

                            FieldValue = GetQuoteTemplateHeaderFieldValue(Feild.FieldCode, quotePM);

                            if (FieldValue == "CustomField")
                            {
                                FieldValue = GetCustomFelidValue(Feild.FieldCode, customFields, quotePM);
                            }

                            //TextFeild
                            styleTd = GetStyleRowTable(quotetemplateTextDesignPMHeaderLable, quotetemplatetableDesignPM, HeaderTableColumn1LabelWidth);
                            AppendNewQuoteTableoRow(FieldName, HtmlTemplate, quotetemplateTextDesignPMHeaderLable, StyleQuoteHeaderLable, styleTd);


                            //Text Value
                            styleTd = GetStyleRowTable(quotetemplateTextDesignPMHeaderValue, quotetemplatetableDesignPM, HeaderTableColumn1ValueWidth);
                            AppendNewQuoteTableoRow(FieldValue, HtmlTemplate, quotetemplateTextDesignPMHeaderValue, StyleQuoteHeaderValue, styleTd);



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

                                    if ((quotetemplatetableDesignPM.BorderTypeCode == "NONE"))
                                    {
                                        if (!string.IsNullOrEmpty(FieldName)) FieldName += " :";
                                    }

                                    FieldValue = GetQuoteTemplateHeaderFieldValue(QuoteTemplaetHeaderColum1[J].FieldCode, quotePM);
                                    if (FieldValue == "CustomField")
                                    {
                                        FieldValue = GetCustomFelidValue(Feild.FieldCode, customFields, quotePM);
                                    }


                                    //TextFeild
                                    styleTd = GetStyleRowTable(quotetemplateTextDesignPMHeaderLable, quotetemplatetableDesignPM, HeaderTableColumn2LabelWidth);
                                    AppendNewQuoteTableoRow(FieldName, HtmlTemplate, quotetemplateTextDesignPMHeaderLable, StyleQuoteHeaderLable, styleTd);

                                    //TextValue
                                    styleTd = GetStyleRowTable(quotetemplateTextDesignPMHeaderValue, quotetemplatetableDesignPM, HeaderTableColumn2ValueWidth);
                                    AppendNewQuoteTableoRow(FieldValue, HtmlTemplate, quotetemplateTextDesignPMHeaderValue, StyleQuoteHeaderValue, styleTd);

                                }
                            }

                            else
                            {
                                styleTd = GetStyleRowTable(quotetemplateTextDesignPMHeaderLable, quotetemplatetableDesignPM, null);
                                HtmlTemplate.Append("<td " + styleTd + ">" + "<Div style='width:70px;'" + ">" + "" + "</Div>" + "</td>");

                                styleTd = GetStyleRowTable(quotetemplateTextDesignPMHeaderValue, quotetemplatetableDesignPM, null);
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
                        foreach (QuoteTemplateHeaderFieldPM Feild in QuoteTemplaetHeaderColum1)
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

                                    if ((quotetemplatetableDesignPM.BorderTypeCode == "NONE"))
                                    {
                                        if (!string.IsNullOrEmpty(FieldName)) FieldName += " :";

                                    }

                                    FieldValue = GetQuoteTemplateHeaderFieldValue(QuoteTemplaetHeaderColum0[J].FieldCode, quotePM);

                                    if (FieldValue == "CustomField")
                                    {
                                        FieldValue = GetCustomFelidValue(QuoteTemplaetHeaderColum0[J].FieldCode, customFields, quotePM);
                                    }
                                    //FieldName
                                    styleTd = GetStyleRowTable(quotetemplateTextDesignPMHeaderLable, quotetemplatetableDesignPM, HeaderTableColumn1LabelWidth);
                                    AppendNewQuoteTableoRow(FieldName, HtmlTemplate, quotetemplateTextDesignPMHeaderLable, StyleQuoteHeaderLable, styleTd);

                                    //FieldValue
                                    styleTd = GetStyleRowTable(quotetemplateTextDesignPMHeaderValue, quotetemplatetableDesignPM, HeaderTableColumn1ValueWidth);
                                    AppendNewQuoteTableoRow(FieldValue, HtmlTemplate, quotetemplateTextDesignPMHeaderValue, StyleQuoteHeaderValue, styleTd);


                                    // HtmlTemplate.Append("<td  style = 'width = 20px'>" + "</td>");
                                }
                            }
                            else
                            {
                                styleTd = GetStyleRowTable(quotetemplateTextDesignPMHeaderLable, quotetemplatetableDesignPM, null);
                                HtmlTemplate.Append("<td " + styleTd + ">" + "<Div style='width:70px;'" + ">" + "" + "</Div>" + "</td>");

                                styleTd = GetStyleRowTable(quotetemplateTextDesignPMHeaderValue, quotetemplatetableDesignPM, null);
                                HtmlTemplate.Append("<td " + styleTd + ">" + "<Div style='width:70px;'" + ">" + "" + "</Div>" + "</td>");
                            }

                            FieldName = "";
                            FieldName = GetNameColum(Feild.FieldCode, textcodes, "QH");
                            if (string.IsNullOrEmpty(FieldName))
                            {
                                FieldName = TranslateTextsClass.Translate(Feild.FieldCode, Feild.Tenant);
                            }

                            if ((quotetemplatetableDesignPM.BorderTypeCode == "NONE"))
                            {
                                if (!string.IsNullOrEmpty(FieldName)) FieldName += " :";

                            }

                            FieldValue = GetQuoteTemplateHeaderFieldValue(Feild.FieldCode, quotePM);
                            if (FieldValue == "CustomField")
                            {
                                FieldValue = GetCustomFelidValue(Feild.FieldCode, customFields, quotePM);
                            }
                            //FieldName
                            styleTd = GetStyleRowTable(quotetemplateTextDesignPMHeaderLable, quotetemplatetableDesignPM, HeaderTableColumn2LabelWidth);
                            AppendNewQuoteTableoRow(FieldName, HtmlTemplate, quotetemplateTextDesignPMHeaderLable, StyleQuoteHeaderLable, styleTd);

                            //FieldValue
                            styleTd = GetStyleRowTable(quotetemplateTextDesignPMHeaderLable, quotetemplatetableDesignPM, HeaderTableColumn2LabelWidth);
                            AppendNewQuoteTableoRow(FieldValue, HtmlTemplate, quotetemplateTextDesignPMHeaderValue, StyleQuoteHeaderValue, styleTd);


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
        private void AppendNewQuoteTableoRow(string text, StringBuilder HtmlTemplate, QuoteTemplateTextDesignPM quoteTemplateTextDesignPM, string styleLable, string styleTd)
        {
            if (quoteTemplateTextDesignPM.Italic)
            {
                HtmlTemplate.Append("<td  " + styleTd + ">" + "<i>" + "<Div " + styleLable + ">" + text + "</Div>" + "</i>" + "</td>");
            }
            else
            {
                HtmlTemplate.Append("<td " + styleTd + ">" + "<Div " + styleLable + ">" + text + "</Div>" + "</td>");
            }
        }
        public byte[] GetQuoteTemplateDetails(QuoteTemplateBuildArges quoteTemplateBuildArges )
        {

            QuotePM quotePM = quoteTemplateBuildArges.QuotePM;
            QuoteTemplatePM template = quoteTemplateBuildArges.QuoteTemplatePM;
            QuoteTemplateSettingPM setting = quoteTemplateBuildArges.QuoteTemplateSettingPM;
            List<QuoteTemplateTextDesignPM> quoteTemplateTextDesignsList = quoteTemplateBuildArges.QuoteTemplateTextDesignPMLists;
            List<QuoteTemplateTableDesignPM> quoteTemplateTableDesignsList = quoteTemplateBuildArges.QuoteTemplateTableDesignsLists;
            List<QuoteTemplateTextCodePM> textcodes = quoteTemplateBuildArges.QuoteTemplateTextCodePMLists;
            int tenant = quoteTemplateBuildArges.Tenant;
            List<ObjectField> customFields = ObjectFieldRepository.GetCustomObjectFieldsByObjectTableName("Quote", tenant).ToList();
            if (quotePM == null) quotePM = BuildingQuotePM();
            if (quoteTemplateBuildArges.VersionNumber != null) quotePM.QuoteVersion = quotePM.QuoteNumber + "-" + (int)quoteTemplateBuildArges.VersionNumber;
            Tenant = quotePM.Tenant;
            string FieldValue = "";
            string FieldName = "";
            string dir = "";
            if (setting.RightToLeft) dir = "dir='RTL'";
            isRightToLeft = setting.RightToLeft;
            QuoteTemplateDetailsFieldQuery quoteTemplateDetailsFieldQuery = new QuoteTemplateDetailsFieldQuery(tenant);
            IQueryable<QuoteTemplateDetailsFieldPM> QuoteTemplaetDetailsFieldList = quoteTemplateDetailsFieldQuery.GetQuoteTemplateDetailsFieldPMsByQuotetemplateId(tenant, template.Id);
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

                HtmlTemplate.Append(GetHtmlStringLine(setting.SpaceLinesBeforeQuoteDetails, quoteTemplateBuildArges.FirstSectionInBody));
                List<QuoteTemplateDetailsFieldPM> QuoteTemplaetDetailsColum0;
                List<QuoteTemplateDetailsFieldPM> QuoteTemplaetDetailsColum1;

                QuoteTemplaetDetailsColum0 = QuoteTemplaetDetailsFieldList.Where(d => d.Column == 0).OrderBy(d => d.Row).ToList();
                QuoteTemplaetDetailsColum1 = QuoteTemplaetDetailsFieldList.Where(d => d.Column == 1).OrderBy(d => d.Row).ToList();

                IsShowlanguage = setting.ShowLocalLanguage;

                QuoteTemplateTableDesignPM quotetemplatetableDesignPM = quoteTemplateTableDesignsList.Where(t => t.Id == setting.DetailsTableDesignId).FirstOrDefault();
                QuoteTemplateTextDesignPM quotetemplateTextDesignPMDetailsLable = quoteTemplateTextDesignsList.Where(t => t.Id == quotetemplatetableDesignPM.HeaderDesignId).FirstOrDefault();
                QuoteTemplateTextDesignPM quotetemplateTextDesignPMDetailsValue = quoteTemplateTextDesignsList.Where(t => t.Id == quotetemplatetableDesignPM.LinesDesignId).FirstOrDefault();

                string styleTable = GetStyleTable(quotetemplatetableDesignPM);

                string StyleQuoteDetailsLable = GetsyleSpanQuoteHeaderDetails(quotetemplateTextDesignPMDetailsLable);
                string styleTd = "";
                string widthtable = "";
                bool IsDetailsTableAuto = setting.DetailsTableColumWidthType == "AUTO" ? true : false;

                string StyleQuoteDetailsValue = GetsyleSpanQuoteHeaderDetails(quotetemplateTextDesignPMDetailsValue);

                string DetailsTableColumn1LabelWidth = IsDetailsTableAuto ? "Auto" : (setting.DetailsTableColumn1LabelWidth.ToString() + "%");
                string DetailsTableColumn1ValueWidth = IsDetailsTableAuto ? "Auto" : (setting.DetailsTableColumn1ValueWidth.ToString() + "%");
                string DetailsTableColumn2LabelWidth = IsDetailsTableAuto ? "Auto" : (setting.DetailsTableColumn2LabelWidth.ToString() + "%");
                string DetailsTableColumn2ValueWidth = IsDetailsTableAuto ? "Auto" : (setting.DetailsTableColumn2ValueWidth.ToString() + "%");

                double? W = setting.DetailsTableColumn1LabelWidth + setting.DetailsTableColumn1ValueWidth;
                if (setting.DetailsSectionHasTwoColumns) W += (setting.DetailsTableColumn2LabelWidth + setting.DetailsTableColumn2ValueWidth);
                widthtable = W.ToString() + "%";
                HtmlTemplate.Append("<div " + dir + " style ='width=100%' >");

                if (quotetemplatetableDesignPM.BorderTypeCode == "NONE") styleTable = "";
                if (IsDetailsTableAuto) HtmlTemplate.Append("<table   " + dir + " width='Auto' " + styleTable + " >");
                else HtmlTemplate.Append("<table " + dir + " width='" + widthtable + "'" + styleTable + " >");

                //  ShowTitleQuoteDetails
                if (setting.ShowTitleQuoteDetails)
                {
                    QuoteTemplateTextDesignPM quotetemplateTextDesignPMDetailsTitle = quoteTemplateTextDesignsList.Where(t => t.Id == setting.DetailsTitleDesignId).FirstOrDefault();//quotetemplateTextDesignQuery.GetSinglePM(setting.DetailsTitleDesignId, tenant);

                    if (setting.RightToLeft)
                    {
                        quotetemplateTextDesignPMDetailsTitle.Alignment = "Right";
                    }
                    string styledetailstitle = GetSpanRowStyle(quotetemplateTextDesignPMDetailsTitle, "Header", "auto");

                    FieldName = "";
                    FieldName = GetNameColum("GENERALDETAILS", textcodes, "QD");

                    if ((quotetemplatetableDesignPM.BorderTypeCode == "NONE"))
                    {
                        if (!string.IsNullOrEmpty(FieldName))
                        {
                            FieldName += " :";
                        }
                    }
                    if (quotetemplateTextDesignPMDetailsTitle.Italic)
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
                    foreach (QuoteTemplateDetailsFieldPM Feild in QuoteTemplaetDetailsColum0)
                    {
                        HtmlTemplate.Append("<tr>");
                        FieldName = "";
                        FieldName = GetNameColum(Feild.FieldCode, textcodes, "QD");
                        if (string.IsNullOrEmpty(FieldName)) FieldName = TranslateTextsClass.Translate(Feild.FieldCode, Feild.Tenant);
                        if ((quotetemplatetableDesignPM.BorderTypeCode == "NONE"))
                        {
                            if (!string.IsNullOrEmpty(FieldName)) FieldName += " :";
                        }

                        FieldValue = GetQuoteTemplateDetailsFieldValue(Feild.FieldCode, quotePM);
                        if (FieldValue == "CustomField") FieldValue = GetCustomFelidValue(Feild.FieldCode, customFields, quotePM);

                        //Lable
                        styleTd = GetStyleRowTable(quotetemplateTextDesignPMDetailsLable, quotetemplatetableDesignPM, DetailsTableColumn1LabelWidth);
                        AppendNewQuoteTableoRow(FieldName, HtmlTemplate, quotetemplateTextDesignPMDetailsLable, StyleQuoteDetailsLable, styleTd);

                        //Value
                        styleTd = GetStyleRowTable(quotetemplateTextDesignPMDetailsValue, quotetemplatetableDesignPM, DetailsTableColumn1ValueWidth);
                        AppendNewQuoteTableoRow(FieldValue, HtmlTemplate, quotetemplateTextDesignPMDetailsValue, StyleQuoteDetailsValue, styleTd);

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
                        foreach (QuoteTemplateDetailsFieldPM Feild in QuoteTemplaetDetailsColum0)
                        {
                            HtmlTemplate.Append("<tr>");


                            FieldName = "";
                            FieldName = GetNameColum(Feild.FieldCode, textcodes, "QD");

                            if (string.IsNullOrEmpty(FieldName)) FieldName = TranslateTextsClass.Translate(Feild.FieldCode, Feild.Tenant);

                            if ((quotetemplatetableDesignPM.BorderTypeCode == "NONE"))
                            {
                                if (!string.IsNullOrEmpty(FieldName)) FieldName += " :";
                            }

                            FieldValue = GetQuoteTemplateDetailsFieldValue(Feild.FieldCode, quotePM);

                            if (FieldValue == "CustomField")
                            {
                                FieldValue = GetCustomFelidValue(Feild.FieldCode, customFields, quotePM);
                            }

                            //TextFeild
                            styleTd = GetStyleRowTable(quotetemplateTextDesignPMDetailsLable, quotetemplatetableDesignPM, DetailsTableColumn1LabelWidth);
                            AppendNewQuoteTableoRow(FieldName, HtmlTemplate, quotetemplateTextDesignPMDetailsLable, StyleQuoteDetailsLable, styleTd);


                            //Text Value
                            styleTd = GetStyleRowTable(quotetemplateTextDesignPMDetailsValue, quotetemplatetableDesignPM, DetailsTableColumn1ValueWidth);
                            AppendNewQuoteTableoRow(FieldValue, HtmlTemplate, quotetemplateTextDesignPMDetailsValue, StyleQuoteDetailsValue, styleTd);



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

                                    if ((quotetemplatetableDesignPM.BorderTypeCode == "NONE"))
                                    {
                                        if (!string.IsNullOrEmpty(FieldName)) FieldName += " :";
                                    }

                                    FieldValue = GetQuoteTemplateDetailsFieldValue(QuoteTemplaetDetailsColum1[J].FieldCode, quotePM);
                                    if (FieldValue == "CustomField")
                                    {
                                        FieldValue = GetCustomFelidValue(Feild.FieldCode, customFields, quotePM);
                                    }


                                    //TextFeild
                                    styleTd = GetStyleRowTable(quotetemplateTextDesignPMDetailsLable, quotetemplatetableDesignPM, DetailsTableColumn2LabelWidth);
                                    AppendNewQuoteTableoRow(FieldName, HtmlTemplate, quotetemplateTextDesignPMDetailsLable, StyleQuoteDetailsLable, styleTd);

                                    //TextValue
                                    styleTd = GetStyleRowTable(quotetemplateTextDesignPMDetailsValue, quotetemplatetableDesignPM, DetailsTableColumn2ValueWidth);
                                    AppendNewQuoteTableoRow(FieldValue, HtmlTemplate, quotetemplateTextDesignPMDetailsValue, StyleQuoteDetailsValue, styleTd);

                                }
                            }

                            else
                            {
                                styleTd = GetStyleRowTable(quotetemplateTextDesignPMDetailsLable, quotetemplatetableDesignPM, null);
                                HtmlTemplate.Append("<td " + styleTd + ">" + "<Div style='width:70px;'" + ">" + "" + "</Div>" + "</td>");

                                styleTd = GetStyleRowTable(quotetemplateTextDesignPMDetailsValue, quotetemplatetableDesignPM, null);
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
                        foreach (QuoteTemplateDetailsFieldPM Feild in QuoteTemplaetDetailsColum1)
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

                                    if ((quotetemplatetableDesignPM.BorderTypeCode == "NONE"))
                                    {
                                        if (!string.IsNullOrEmpty(FieldName)) FieldName += " :";

                                    }

                                    FieldValue = GetQuoteTemplateDetailsFieldValue(QuoteTemplaetDetailsColum0[J].FieldCode, quotePM);

                                    if (FieldValue == "CustomField")
                                    {
                                        FieldValue = GetCustomFelidValue(QuoteTemplaetDetailsColum0[J].FieldCode, customFields, quotePM);
                                    }
                                    //FieldName
                                    styleTd = GetStyleRowTable(quotetemplateTextDesignPMDetailsLable, quotetemplatetableDesignPM, DetailsTableColumn1LabelWidth);
                                    AppendNewQuoteTableoRow(FieldName, HtmlTemplate, quotetemplateTextDesignPMDetailsLable, StyleQuoteDetailsLable, styleTd);

                                    //FieldValue
                                    styleTd = GetStyleRowTable(quotetemplateTextDesignPMDetailsValue, quotetemplatetableDesignPM, DetailsTableColumn1ValueWidth);
                                    AppendNewQuoteTableoRow(FieldValue, HtmlTemplate, quotetemplateTextDesignPMDetailsValue, StyleQuoteDetailsValue, styleTd);


                                    // HtmlTemplate.Append("<td  style = 'width = 20px'>" + "</td>");
                                }
                            }
                            else
                            {
                                styleTd = GetStyleRowTable(quotetemplateTextDesignPMDetailsLable, quotetemplatetableDesignPM, null);
                                HtmlTemplate.Append("<td " + styleTd + ">" + "<Div style='width:70px;'" + ">" + "" + "</Div>" + "</td>");

                                styleTd = GetStyleRowTable(quotetemplateTextDesignPMDetailsValue, quotetemplatetableDesignPM, null);
                                HtmlTemplate.Append("<td " + styleTd + ">" + "<Div style='width:70px;'" + ">" + "" + "</Div>" + "</td>");
                            }

                            FieldName = "";
                            FieldName = GetNameColum(Feild.FieldCode, textcodes, "QD");
                            if (string.IsNullOrEmpty(FieldName))
                            {
                                FieldName = TranslateTextsClass.Translate(Feild.FieldCode, Feild.Tenant);
                            }

                            if ((quotetemplatetableDesignPM.BorderTypeCode == "NONE"))
                            {
                                if (!string.IsNullOrEmpty(FieldName)) FieldName += " :";

                            }

                            FieldValue = GetQuoteTemplateDetailsFieldValue(Feild.FieldCode, quotePM);
                            if (FieldValue == "CustomField")
                            {
                                FieldValue = GetCustomFelidValue(Feild.FieldCode, customFields, quotePM);
                            }
                            //FieldName
                            styleTd = GetStyleRowTable(quotetemplateTextDesignPMDetailsLable, quotetemplatetableDesignPM, DetailsTableColumn2LabelWidth);
                            AppendNewQuoteTableoRow(FieldName, HtmlTemplate, quotetemplateTextDesignPMDetailsLable, StyleQuoteDetailsLable, styleTd);

                            //FieldValue
                            styleTd = GetStyleRowTable(quotetemplateTextDesignPMDetailsLable, quotetemplatetableDesignPM, DetailsTableColumn2ValueWidth);
                            AppendNewQuoteTableoRow(FieldValue, HtmlTemplate, quotetemplateTextDesignPMDetailsValue, StyleQuoteDetailsValue, styleTd);


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

        public QuotePM BuildingQuotePM()
        {
            QuotePM quotePM = new QuotePM()
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

            quotePM.QuoteSaleCharges.Add(new QuoteSaleChargePM() { VatTypeName ="vat1",VatPercentage =50 ,ChargesTypeCode = "AFT", Notes = "test", ChargesGroupCode = "FRT", ChargesTypeName = "Air Freight", SaleQuantity = 500, SaleUnitPrice = 1, SaleMeasurementShortName = "Ch Weight", SaleTotalAmount = 500, SaleTotalAmountLocal = 600, CurrencyCode = "USD", ChargesTypeDescription = "Air Freight Description", SaleMaxAmount = 50, SaleMinAmount = 20 });
            quotePM.QuoteSaleCharges.Add(new QuoteSaleChargePM() { VatTypeName = "vat2", VatPercentage = 40, ChargesTypeCode = "AFT", Notes = "test2", ChargesGroupCode = "FRT", ChargesTypeName = "Air Freight", SaleQuantity = 500, SaleUnitPrice = 1, SaleMeasurementShortName = "Ch Weight", SaleTotalAmount = 500, SaleTotalAmountLocal = 600, CurrencyCode = "USD", ChargesTypeDescription = "Air Freight Description1", SaleMaxAmount = 40, SaleMinAmount = 25 });
            quotePM.QuoteSaleCharges.Add(new QuoteSaleChargePM() { VatTypeName = "vat3", VatPercentage = 30, ChargesTypeCode = "DEMU", Notes = "test3", ChargesGroupCode = "HNDCH", ChargesTypeName = "Demmurage", SaleQuantity = 500, SaleUnitPrice = 1, SaleMeasurementShortName = "Ch Weight", SaleTotalAmount = 500, SaleTotalAmountLocal = 600, CurrencyCode = "USD", ChargesTypeDescription = "Air Freight Description2", SaleMaxAmount = 45, SaleMinAmount = 33 });
            quotePM.QuoteSaleCharges.Add(new QuoteSaleChargePM() { VatTypeName = "vat4", VatPercentage = 10, ChargesTypeCode = "DU", Notes = "test4", ChargesGroupCode = "FRT", ChargesTypeName = "Duties", SaleQuantity = 500, SaleUnitPrice = 1, SaleMeasurementShortName = "Ch Weight", SaleTotalAmount = 500, SaleTotalAmountLocal = 600, CurrencyCode = "USD", ChargesTypeDescription = "Air Freight Description3", SaleMaxAmount = 22, SaleMinAmount = 15 });

            return quotePM;
        }

        public string GetCustomFelidValue(string fullNameTextCode, List<ObjectField> customFields, QuotePM quotePM)
        {
            string FieldValue = "";

            ObjectField field = customFields.Where(d => d.FullNameTextCode.Code == fullNameTextCode).FirstOrDefault();
            if (field != null)
            {
                string FullNameTextCode = field.FullNameTextCode.Code;
                CustomFieldResolver customFieldResolver = new CustomFieldResolver();

                PropertyInfo propInfo = typeof(QuotePM).GetProperty(field.FieldName);
                object newValue = customFieldResolver.GetFieldValue(quotePM, field, quotePM.Tenant);
                if (newValue != null)
                {
                    if (field.DataTypeCode.ToLower() == "boolean")
                    {
                        FieldValue = newValue.ToString().ToLower() == "false" ? TranslateTextsClass.Translate("General.O.No", quotePM.Tenant) : TranslateTextsClass.Translate("General.O.Yes", quotePM.Tenant);
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

        private string GetsyleSpanQuoteHeaderDetails(QuoteTemplateTextDesignPM headerDesign)
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

        private string GetQuoteTemplateDetailsFieldValue(string fieldname, QuotePM quotePM)
        {
            string FieldValue = "";
            if (fieldname == "EXPIRATIONDAYS")
            {
                FieldValue = quotePM.ExpirationDays != null ? quotePM.ExpirationDays.ToString() : "";
            }
            else if (fieldname == "EXPIRATIONDATE")
            {
                FieldValue = quotePM.ExpirationDate != null ? ConvertToShortDate((DateTime)quotePM.ExpirationDate, quotePM.Tenant) : "";
            }
            else if (fieldname == "SHIPPERNAME")
            {
                FieldValue = quotePM.ShipperName;
            }
            else if (fieldname == "SHIPPERADDRESS")
            {
                if (!string.IsNullOrEmpty(quotePM.ShipperMainAddressId))
                {
                    AddressRepository addressRep = new AddressRepository(quotePM.Tenant);
                    Address address = addressRep.GetSingleAddress(quotePM.ShipperMainAddressId, quotePM.Tenant);

                    if (address != null)
                    {
                        FieldValue = General.GetAddress(address);
                    }
                }
            }
            else if (fieldname == "SHIPPERCONTACT")
            {
                if (!string.IsNullOrEmpty(quotePM.ShipperContactId))
                {
                    ContactRepository contactRep = new ContactRepository(quotePM.Tenant);
                    FieldValue = contactRep.GetContactNameById(quotePM.ShipperContactId, quotePM.Tenant);
                }
            }
            else if (fieldname == "SHIPPERREFERENCES")
            {

                FieldValue = CombinedReferences(quotePM.ShipperReference1, quotePM.ShipperReference2);
            }
            // Consignee
            else if (fieldname == "CONSIGNEENAME")
            {
                FieldValue = quotePM.ConsigneeName;
            }
            else if (fieldname == "CONSIGNEEADDRESS")
            {
                if (!string.IsNullOrEmpty(quotePM.ConsigneeMainAddressId))
                {
                    AddressRepository addressRep = new AddressRepository(quotePM.Tenant);
                    Address address = addressRep.GetSingleAddress(quotePM.ConsigneeMainAddressId, quotePM.Tenant);

                    if (address != null)
                    {
                        FieldValue = General.GetAddress(address);
                    }
                }
            }
            else if (fieldname == "CONSIGNEECONTACT")
            {
                if (!string.IsNullOrEmpty(quotePM.ConsigneeContactId))
                {
                    ContactRepository contactRep = new ContactRepository(quotePM.Tenant);
                    FieldValue = contactRep.GetContactNameById(quotePM.ConsigneeContactId, quotePM.Tenant);

                }
            }
            else if (fieldname == "CONSIGNEEREFERENCES")
            {

                FieldValue = CombinedReferences(quotePM.ConsigneeReference1, quotePM.ConsigneeReference2);
            }
            else if (fieldname == "CUSTOMERNAME")
            {
                FieldValue = quotePM.CustomerName;
            }
            else if (fieldname == "CUSTOMERADDRESS")
            {
                FieldValue = "";
            }
            else if (fieldname == "CUSTOMERCONTACT")
            {
                if (!string.IsNullOrEmpty(quotePM.CustomerContactId))
                {
                    ContactRepository contactRep = new ContactRepository(quotePM.Tenant);
                    FieldValue = contactRep.GetContactNameById(quotePM.CustomerContactId, quotePM.Tenant);
                }
            }
            else if (fieldname == "CUSTOMERREFERENCES")
            {

                FieldValue = CombinedReferences(quotePM.CustomerReference1, quotePM.CustomerReference2);
            }
            else if (fieldname == "QUOTENUMBER")
            {
                FieldValue = !string.IsNullOrEmpty(quotePM.QuoteVersion) ? quotePM.QuoteVersion : quotePM.QuoteNumber;
            }
            else if (fieldname == "PICKUPFROM")
            {
                FieldValue = quotePM.PickupLocation;
            }
            else if (fieldname == "DELIVERYTO")
            {
                FieldValue = quotePM.DeliveryLocation;
            }
            else if (fieldname == "FROMPORT" || fieldname == "FROMLOCATION")
            {
                FieldValue = quotePM.FromPortName;
            }
            else if (fieldname == "TOPORT" || fieldname == "TOLOCATION")
            {
                FieldValue = quotePM.ToPortName;
            }
            else if (fieldname == "INCOTERMS")
            {
                FieldValue = quotePM.IncotermName;
            }
            else if (fieldname == "SERVICE")
            {
                // FieldValue = quotePM.
                if (!string.IsNullOrEmpty(quotePM.ShipmentTypeId))
                {
                    ShipmentTypeRepository typeRep = new ShipmentTypeRepository(quotePM.Tenant);
                    ShipmentType shiptype = typeRep.GetSingleShipmentType(quotePM.ShipmentTypeId);
                    if (shiptype != null)
                    {
                        FieldValue = quotePM.TransportModeName + shiptype.Name;
                    }
                }
            }
            else if (fieldname == "TRANSITTIME")
            {
                FieldValue = quotePM.TransitTime;
            }
            else if (fieldname == "MOVETYPE")
            {
                if (!string.IsNullOrEmpty(quotePM.MoveTypeId))
                {
                    MoveTypeQuery moveTypeQuery = new MoveTypeQuery(quotePM.Tenant);
                    FieldValue = moveTypeQuery.GetMoveTypeNameById(quotePM.MoveTypeId, quotePM.Tenant);
                }
            }
            else if (fieldname == "DESCRIPTIONOFGOODS")
            {
                FieldValue = quotePM.DescriptionOfGoods;
            }
            else if (fieldname == "DEPARTUREFREQUENCY")
            {
                FieldValue = quotePM.DepartureFrequency;
            }
            else if (fieldname == "DANGEROUSGOODS")
            {
                FieldValue = !quotePM.IsDangerous ? TranslateTextsClass.Translate("General.O.No", quotePM.Tenant) : TranslateTextsClass.Translate("General.O.Yes", quotePM.Tenant);
            }
            else if (fieldname == "TRUCKER" || fieldname == "SHIPINGLINE" || fieldname == "AIRLINE")
            {
                if (!string.IsNullOrEmpty(quotePM.MainCarriageCarrierId))
                {
                    CardRepository cardRep = new CardRepository(quotePM.Tenant);
                    FieldValue = cardRep.GetEnglishNameCardById(quotePM.MainCarriageCarrierId, quotePM.Tenant);
                }
            }
            else if (fieldname == "CHARGEABLEWEIGHT")
            {
                if (quotePM.ChargeableWeight != null)
                {
                    FieldValue = quotePM.ChargeableWeight.ToString() + " " + quotePM.ChargeableWeightUnitCode.ToString();
                }
            }
            else if (fieldname == "GROSSWEIGHT")
            {
                if (quotePM.GrossWeight != null)
                {
                    FieldValue = quotePM.GrossWeight.ToString() + " " + quotePM.GrossWeightUnitCode.ToString();
                }
            }
            else if (fieldname == "VOLUME")
            {
                if (quotePM.Volume != null)
                {
                    FieldValue = quotePM.Volume.ToString() + " " + quotePM.VolumeUnitCode.ToString();
                }
            }
            else if (fieldname == "VOLUMETRICWEIGHT")
            {
                if (quotePM.VolumetricWeight != null)
                {
                    FieldValue = quotePM.VolumetricWeight.ToString() + " " + quotePM.VolumeUnitCode.ToString();
                }
            }
            else if (fieldname == "NUMBEROFPACKAGES")
            {
                if (quotePM.NumberOfPackages != null)
                {
                    FieldValue = quotePM.NumberOfPackages.ToString();
                }
            }
            else if (fieldname == "NUMBEROFCONTAINERS")
            {
                if (quotePM.NumberOfContainers != null)
                {
                    FieldValue = quotePM.NumberOfContainers.ToString();
                }
            }
            else if (fieldname == "NOTIFYNAME")
            {
                FieldValue = quotePM.NotifyName;
            }
            else if (fieldname == "NOTIFYADDRESS")
            {
                if (!string.IsNullOrEmpty(quotePM.NotifyAddressId))
                {
                    AddressRepository addressRep = new AddressRepository(quotePM.Tenant);
                    Address address = addressRep.GetSingleAddress(quotePM.NotifyAddressId, quotePM.Tenant);

                    if (address != null)
                    {
                        FieldValue = General.GetAddress(address);
                    }
                }
            }
            else if (fieldname == "NOTIFYCONTACT")
            {
                if (!string.IsNullOrEmpty(quotePM.NotifyContactId))
                {
                    ContactRepository contactRep = new ContactRepository(quotePM.Tenant);
                    FieldValue = contactRep.GetContactNameById(quotePM.NotifyContactId, quotePM.Tenant);
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

        public string GetQuoteTemplateHeaderFieldValue(string fieldname, QuotePM quotePM)
        {
            string FieldValue = "";

            if (fieldname == "QUOTENUMBER")
            {
                FieldValue =!string.IsNullOrEmpty(quotePM.QuoteVersion)? quotePM.QuoteVersion : quotePM.QuoteNumber;
            }

            else if (fieldname == "EXPIRATIONDATE" && quotePM.ExpirationDate != null)
            {
                FieldValue = ConvertToShortDate((DateTime)quotePM.ExpirationDate, quotePM.Tenant);
            }
            else
                if (fieldname == "CUSTOMER")
            {
                FieldValue = quotePM.CustomerName;
            }
            else if (fieldname == "QUOTEDATE")
            {
                FieldValue = ConvertToShortDate(DateTime.Now, quotePM.Tenant);
            }

            else if (fieldname == "ATTN")
            {
                if (quotePM != null)
                {
                    ContactRepository contactrrep = new ContactRepository(quotePM.Tenant);
                    Contact contact = contactrrep.GetSingleContact(quotePM.CustomerContactId, quotePM.Tenant);

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

        private string GetStyleGroupByTd(string type, string BorderColorr, int BorderThicknesss, string borderTypeCode, QuoteTemplateTextDesignPM QuoteTemplateTextDesigGroupBy, bool RightToLeft)
        {

            string BorderColor = BorderColorr.Remove(1, 2);
            string BorderThickness = BorderThicknesss.ToString() + "px";
            string BackgroundColor = QuoteTemplateTextDesigGroupBy.BackgroundColor;
            if (QuoteTemplateTextDesigGroupBy.BackgroundColor.Length > 7)
            {
                BackgroundColor = QuoteTemplateTextDesigGroupBy.BackgroundColor.Remove(1, 2);
            }


            var alignment = ";text-align:" + QuoteTemplateTextDesigGroupBy.Alignment;// RightToLeft ? ";text-align:right" : ";text-align:" + QuoteTemplateTextDesigGroupBy.Alignment;


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

        private void buildHeadercolumn(StringBuilder HtmlTemplate, QuoteTemplateSettingPM setting, QuoteTemplateTextDesignPM quotetemplateTextDesignPMHeader, QuoteTemplateTableDesignPM quoteTemplateTableDesignPM, QuotePM quotePM, string pricingSectionType, bool IsRightToLeft, List<QuoteTemplateTextCodePM> textcodes)
        {
            HtmlTemplate.Append("<tr style= 'height:auto; width:auto;vertical-align:central'>");



            IsShowlanguage = setting.ShowLocalLanguage;
            if (pricingSectionType == "PP")
            {
                GetCountHeader(setting, quotePM, "PP");

                if (setting.ShowChargeCodePackages)
                {
                    AppendHeaderColumn("CHARGECODEPACKAGES", HtmlTemplate, setting, quotetemplateTextDesignPMHeader, quoteTemplateTableDesignPM, pricingSectionType, textcodes);
                }

                if (setting.ShowChargeNamePackages)
                {
                    AppendHeaderColumn("CHARGEPACKAGES", HtmlTemplate, setting, quotetemplateTextDesignPMHeader, quoteTemplateTableDesignPM, pricingSectionType, textcodes);
                }

                if (setting.ShowUnitsPackages)
                {
                    AppendHeaderColumn("UNITSPACKAGES", HtmlTemplate, setting, quotetemplateTextDesignPMHeader, quoteTemplateTableDesignPM, pricingSectionType, textcodes);
                }

                if (setting.ShowUnitPricePackages)
                {
                    AppendHeaderColumn("UNITPRICEPACKAGES", HtmlTemplate, setting, quotetemplateTextDesignPMHeader, quoteTemplateTableDesignPM, pricingSectionType, textcodes);

                }

                if (setting.ShowMeasurementPackages)
                {
                    AppendHeaderColumn("MEASUREMENTPACKAGES", HtmlTemplate, setting, quotetemplateTextDesignPMHeader, quoteTemplateTableDesignPM, pricingSectionType, textcodes);
                }

                if (setting.ShowSaleCurrencyColumnPackages)
                {
                    AppendHeaderColumn("TOTALPACKAGES", HtmlTemplate, setting, quotetemplateTextDesignPMHeader, quoteTemplateTableDesignPM, pricingSectionType, textcodes);
                }

                if (setting.ShowLocalCurrencyColumnPackages)
                {
                    AppendHeaderColumn("LOCALAMOUNTPACKAGES", HtmlTemplate, setting, quotetemplateTextDesignPMHeader, quoteTemplateTableDesignPM, pricingSectionType, textcodes);
                }

                if (setting.ShowChargeDescriptionPackages)
                {
                    AppendHeaderColumn("CHARGEDESCRIPTIONPACKAGES", HtmlTemplate, setting, quotetemplateTextDesignPMHeader, quoteTemplateTableDesignPM, pricingSectionType, textcodes);

                }

                if (setting.ShowChargeNotePackages)
                {
                    AppendHeaderColumn("CHARGENOTEPACKAGES", HtmlTemplate, setting, quotetemplateTextDesignPMHeader, quoteTemplateTableDesignPM, pricingSectionType, textcodes);
                }


                if (setting.ShowSaleMaxMinAmountPackages)
                {
                    AppendHeaderColumn("SALEMINMAXPACKAGES", HtmlTemplate, setting, quotetemplateTextDesignPMHeader, quoteTemplateTableDesignPM, pricingSectionType, textcodes);
                }

                if (setting.ShowRegionalTAXPackages)
                {
                    AppendHeaderColumn("ISREGIONALTAXPACKAGES", HtmlTemplate, setting, quotetemplateTextDesignPMHeader, quoteTemplateTableDesignPM, pricingSectionType, textcodes);
                }

                if (quotePM.IsChargesByVAT)
                {
                    if (setting.ShowVATTypePackages)
                    {
                        AppendHeaderColumn("VATTYPEPACKAGES", HtmlTemplate, setting, quotetemplateTextDesignPMHeader, quoteTemplateTableDesignPM, pricingSectionType, textcodes);
                    }
                    if (setting.ShowVATPercentagePackages)
                    {
                        AppendHeaderColumn("VATPERCENTAGEPACKAGES", HtmlTemplate, setting, quotetemplateTextDesignPMHeader, quoteTemplateTableDesignPM, pricingSectionType, textcodes);
                    }
                }

                HtmlTemplate.Append("</tr>");
            }
            else if (pricingSectionType == "PC")
            {
                GetCountHeader(setting, quotePM, "PC");
                if (setting.ShowChargeCodeContainers)
                {
                    AppendHeaderColumn("CHARGECODECONTAINERS", HtmlTemplate, setting, quotetemplateTextDesignPMHeader, quoteTemplateTableDesignPM, pricingSectionType, textcodes);

                }

                if (setting.ShowChargeNameContainers)
                {
                    AppendHeaderColumn("CHARGECONTAINERS", HtmlTemplate, setting, quotetemplateTextDesignPMHeader, quoteTemplateTableDesignPM, pricingSectionType, textcodes);

                }

                if (setting.ShowMeasurementContainers)
                {
                    AppendHeaderColumn("MEASUREMENTCONTAINERS", HtmlTemplate, setting, quotetemplateTextDesignPMHeader, quoteTemplateTableDesignPM, pricingSectionType, textcodes);
                }

                if (setting.ShowFixedPriceContainers)
                {
                    if (ViewFixedPrice)
                    {
                        AppendHeaderColumn("FIXEDPRICECONTAINERS", HtmlTemplate, setting, quotetemplateTextDesignPMHeader, quoteTemplateTableDesignPM, pricingSectionType, textcodes);
                    }
                }

                //=============================== islam ==================================
                if (setting.ShowPriceByContainerColumn)
                {
                    PackageType packageType;
                    if (quotePM.PackageType1Id != null)
                    {
                        packageType = PackageTypeRepository.GetSinglePackageType(quotePM.PackageType1Id, quotePM.Tenant, true);
                        string containerTypePrintAs = packageType.PrintAs;
                        string Name = quotePM.PackageType1Quantity + " x " + containerTypePrintAs;
                        HtmlTemplate.Append(BuildTableColumn(Name, quotetemplateTextDesignPMHeader, quoteTemplateTableDesignPM, "Header", null, setting.RightToLeft, true));
                    }

                    if (quotePM.PackageType2Id != null)
                    {
                        packageType = PackageTypeRepository.GetSinglePackageType(quotePM.PackageType2Id, quotePM.Tenant, true);
                        string containerTypePrintAs = packageType.PrintAs;
                        string Name = quotePM.PackageType2Quantity + " x " + containerTypePrintAs;
                        HtmlTemplate.Append(BuildTableColumn(Name, quotetemplateTextDesignPMHeader, quoteTemplateTableDesignPM, "Header", null, setting.RightToLeft, true));
                    }

                    if (quotePM.PackageType3Id != null)
                    {
                        packageType = PackageTypeRepository.GetSinglePackageType(quotePM.PackageType3Id, quotePM.Tenant, true);
                        string containerTypePrintAs = packageType.PrintAs;
                        string Name = quotePM.PackageType3Quantity + " x " + containerTypePrintAs;
                        HtmlTemplate.Append(BuildTableColumn(Name, quotetemplateTextDesignPMHeader, quoteTemplateTableDesignPM, "Header", null, setting.RightToLeft, true));
                    }

                    if (quotePM.PackageType4Id != null)
                    {
                        packageType = PackageTypeRepository.GetSinglePackageType(quotePM.PackageType4Id, quotePM.Tenant, true);
                        string containerTypePrintAs = packageType.PrintAs;
                        string Name = quotePM.PackageType4Quantity + " x " + containerTypePrintAs;
                        HtmlTemplate.Append(BuildTableColumn(Name, quotetemplateTextDesignPMHeader, quoteTemplateTableDesignPM, "Header", null, setting.RightToLeft, true));
                    }

                    if (quotePM.PackageType5Id != null)
                    {
                        packageType = PackageTypeRepository.GetSinglePackageType(quotePM.PackageType5Id, quotePM.Tenant, true);
                        string containerTypePrintAs = packageType.PrintAs;
                        string Name = quotePM.PackageType5Quantity + " x " + containerTypePrintAs;
                        HtmlTemplate.Append(BuildTableColumn(Name, quotetemplateTextDesignPMHeader, quoteTemplateTableDesignPM, "Header", null, setting.RightToLeft, true));
                    }
                }

                if (setting.ShowSaleCurrencyColumnContainers)
                {
                    AppendHeaderColumn("TOTALCONTAINERS", HtmlTemplate, setting, quotetemplateTextDesignPMHeader, quoteTemplateTableDesignPM, pricingSectionType, textcodes);
                }

                if (setting.ShowLocalCurrencyColumnContainers)
                {
                    AppendHeaderColumn("LOCALAMOUNTCONTAINERS", HtmlTemplate, setting, quotetemplateTextDesignPMHeader, quoteTemplateTableDesignPM, pricingSectionType, textcodes);
                }

                if (setting.ShowChargeDescriptionContainers)
                {
                    AppendHeaderColumn("CHARGEDESCRIPTIONCONTAINERS", HtmlTemplate, setting, quotetemplateTextDesignPMHeader, quoteTemplateTableDesignPM, pricingSectionType, textcodes);
                }

                if (setting.ShowChargeNoteContainers)
                {
                    AppendHeaderColumn("CHARGENOTECONTAINERS", HtmlTemplate, setting, quotetemplateTextDesignPMHeader, quoteTemplateTableDesignPM, pricingSectionType, textcodes);

                }

                if (setting.ShowSaleMaxMinAmountContainers)
                {
                    AppendHeaderColumn("SALEMINMAXCONTAINERS", HtmlTemplate, setting, quotetemplateTextDesignPMHeader, quoteTemplateTableDesignPM, pricingSectionType, textcodes);
                }

                if (setting.ShowRegionalTAXContainers)
                {
                    AppendHeaderColumn("ISREGIONALTAXCONTAINERS", HtmlTemplate, setting, quotetemplateTextDesignPMHeader, quoteTemplateTableDesignPM, pricingSectionType, textcodes);
                }

                if (quotePM.IsChargesByVAT)
                {
                    if (setting.ShowVATTypeContainers)
                    {
                        AppendHeaderColumn("VATTYPECONTAINERS", HtmlTemplate, setting, quotetemplateTextDesignPMHeader, quoteTemplateTableDesignPM, pricingSectionType, textcodes);
                    }
                    if (setting.ShowVATPercentageContainers)
                    {
                        AppendHeaderColumn("VATPERCENTAGECONTAINERS", HtmlTemplate, setting, quotetemplateTextDesignPMHeader, quoteTemplateTableDesignPM, pricingSectionType, textcodes);
                    }
                }

                //===========================================================================

                HtmlTemplate.Append("</tr>");
            }


        }

        private void AppendHeaderColumn(string textCode, StringBuilder HtmlTemplate, QuoteTemplateSettingPM setting, QuoteTemplateTextDesignPM quotetemplateTextDesignPMHeader, QuoteTemplateTableDesignPM quoteTemplateTableDesignPM, string pricingSectionType, List<QuoteTemplateTextCodePM> textcodes)
        {
            string Name = GetNameColum(textCode, textcodes, pricingSectionType);
            int width = GetWidthColumHeaderForPricingTable(textCode, pricingSectionType, setting);
            HtmlTemplate.Append(BuildTableColumn(Name, quotetemplateTextDesignPMHeader, quoteTemplateTableDesignPM, "Header" , (width.ToString() +"%"), setting.RightToLeft));
        }

        private int GetWidthColumHeaderForPricingTable(string textCode, string pricingSectionType, QuoteTemplateSettingPM setting)
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

        public void BuildPricingTitle(StringBuilder HtmlTemplate, QuoteTemplateTextDesignPM quotetemplateTextDesignPMPricingTitle, string typepricing, List<QuoteTemplateTextCodePM> textcodes, bool RightToLeft)
        {
            string FieldName = "";
            string type = RightToLeft ? "PricingTableTitle" : "PricingTitle";
            string stylePricingtitle = GetSpanRowStyle(quotetemplateTextDesignPMPricingTitle, type, "100%");
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

            if (quotetemplateTextDesignPMPricingTitle.Italic)
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

        private string GetStyleTableHeaderFooter(QuoteTemplateTableDesignPM tableDesign, string height)
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

        private string GetStyleTable(QuoteTemplateTableDesignPM quotetemplatetableDesignPM)
        {
            string BorderColor = quotetemplatetableDesignPM.BorderColor;
            if (BorderColor.Length > 7)
            {
                BorderColor = quotetemplatetableDesignPM.BorderColor.Remove(1, 2);
            }

            string BorderThickness = quotetemplatetableDesignPM.BorderThickness.ToString() + "px";
            string style = "";
            style = "style='" + "border-top:" + BorderThickness + " solid " + BorderColor + ";border-bottom:" + BorderThickness + " solid " + BorderColor + ";border-left:" + BorderThickness + " solid " + BorderColor + ";border-right:" + BorderThickness + " solid " + BorderColor + ";table-layout: auto" + ";border-collapse: collapse" +
             "'";
            return style;
        }

        int ShowSaleCurrencyColumnColumnPosition = 0;
       
        private void GetCountHeader(QuoteTemplateSettingPM setting, QuotePM quotePM, string sectionType)
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
                if (quotePM.IsChargesByVAT)
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
                    if (quotePM.PackageType1Id != null) ++TdCount;
                    if (quotePM.PackageType2Id != null) ++TdCount;
                    if (quotePM.PackageType3Id != null) ++TdCount;
                    if (quotePM.PackageType4Id != null) ++TdCount;
                    if (quotePM.PackageType5Id != null) ++TdCount;

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

                if (quotePM.IsChargesByVAT)
                {
                    if (setting.ShowVATTypeContainers) ++TdCount;
                    if (setting.ShowVATPercentageContainers) ++TdCount;
                }



            }
        }

        private string BuildTotalInSale(string columncontent, QuoteTemplateTextDesignPM headerDesign, bool IsHiddenTotal, bool leftToRight = false)
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

        private string BuildTableColumn(string columncontent, QuoteTemplateTextDesignPM headerDesign, QuoteTemplateTableDesignPM tableDesign, string CodeTypeTd, string width, bool bodyRightToLeft, bool headeLeftToRight = false)
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


        private string GetSpanRowStyle(QuoteTemplateTextDesignPM Design, string type, string width = null, double per = 1)
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

        private string GetStyleRowTable(QuoteTemplateTextDesignPM design, QuoteTemplateTableDesignPM quotetemplatetableDesignPM, string width)
        {

            string BorderColor = quotetemplatetableDesignPM.BorderColor;
            if (BorderColor.Length > 7)
            {
                BorderColor = quotetemplatetableDesignPM.BorderColor.Remove(1, 2);
            }

            string style = "";
            string BorderThickness = quotetemplatetableDesignPM.BorderThickness.ToString() + "px";
            string BackgroundColor = design.BackgroundColor;
            if (BackgroundColor.Length > 7)
            {
                BackgroundColor = design.BackgroundColor.Remove(1, 2);
            }

            if (quotetemplatetableDesignPM.BorderTypeCode == "ALL")
            {
                style = "style='" + "border-top:" + BorderThickness + " solid " + BorderColor + ";border-bottom:" + BorderThickness + " solid " + BorderColor + ";border-left:" + BorderThickness + " solid " + BorderColor + ";border-right:" + BorderThickness + " solid " + BorderColor + ";table-layout: auto"
                  + "; text-indent:4px" + "; vertical-align:central" + ";background-color:" + BackgroundColor + "'";
            }

            else if (quotetemplatetableDesignPM.BorderTypeCode == "HORIZONTALLINES")
            {
                style = "style='" + "border-top:" + BorderThickness + " solid " + BorderColor + ";border-bottom:" + BorderThickness + " solid " + BorderColor + ";table-layout: auto"
               + "; text-indent:4px" + "; vertical-align:central" + ";background-color:" + BackgroundColor + "'";
            }
            else if (quotetemplatetableDesignPM.BorderTypeCode == "BOX")
            {
                style = "style='" + ";table-layout: auto"
               + ";text-indent:4px" + "; vertical-align:central" + ";background-color:" + BackgroundColor + "'";
            }
            else if (quotetemplatetableDesignPM.BorderTypeCode == "VERTICALLINES")
            {
                style = "style='" + "border-left:" + BorderThickness + " solid " + BorderColor + ";border-right:" + BorderThickness + " solid " + BorderColor + ";table-layout: auto"
                                    + "; text-indent:4px" + "; vertical-align:central" + ";background-color:" + BackgroundColor + "'";
            }

            else if (quotetemplatetableDesignPM.BorderTypeCode == "NONE")
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

        private string GetTextStyleSpanhidden(QuoteTemplateTextDesignPM headerDesign)
        {

            string style = "style='" + ";vertical-align:central" + ";height:auto" + ";width:auto" + ";margin-top:35px" + ";visibility:hidden" + " '";
            return style;
        }

        private string GetNameColum(string TextCode, List<QuoteTemplateTextCodePM> textcodes, string typeSetting)
        {
            NameTextCode = "";
            List<QuoteTemplateTextCodePM> qoutetemplatetextcodeList = new List<QuoteTemplateTextCodePM>();
            if (typeSetting == "PP") qoutetemplatetextcodeList = textcodes.Where(d => d.Area == "Packages").ToList();
            else if (typeSetting == "PC") qoutetemplatetextcodeList = textcodes.Where(d => d.Area == "Containers").ToList();
            else if (typeSetting == "QD") qoutetemplatetextcodeList = textcodes.Where(d => d.Area == "QuoteDetails").ToList();

            else if (typeSetting == "QH") qoutetemplatetextcodeList = textcodes.Where(d => d.Area == "QuoteHeader").ToList();
            else if (typeSetting == "TotalPerContainers") qoutetemplatetextcodeList = textcodes.Where(d => d.Area == "TotalPerContainers").ToList();
            foreach (QuoteTemplateTextCodePM qoutetemplatetextcode in qoutetemplatetextcodeList)
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

        
        private string AddTableRows(PricingTableRowDetailsArgs pricingTableRowDetailsArgs)
        {
            string width = GetWidthColumHeaderForPricingTable(pricingTableRowDetailsArgs.FieldCode , pricingTableRowDetailsArgs.PricingSectionType, pricingTableRowDetailsArgs.QuoteTemplateSettingPM).ToString() + "%"; ;
            return BuildTableColumn(pricingTableRowDetailsArgs.Value, pricingTableRowDetailsArgs.HeaderDesign, pricingTableRowDetailsArgs.TableDesign, pricingTableRowDetailsArgs.RowDataType, width, pricingTableRowDetailsArgs.QuoteTemplateSettingPM.RightToLeft, pricingTableRowDetailsArgs.RowDataType == "FieldPrice" ? true:false);
        }

        private void BuildTableRows(List<QuoteSaleChargePM> quoteSaleCharges, StringBuilder HtmlTemplate, QuoteTemplateTextDesignPM quoteTemplateTextDesignLines, QuoteTemplateTableDesignPM quotetemplatetableDesignPM, QuoteTemplateBuildArges quoteTemplateBuildArges )
        {
            QuotePM quotePM = quoteTemplateBuildArges.QuotePM;
            string pricingSectionType = quoteTemplateBuildArges.SectionTypeCode;
            QuoteTemplateSettingPM setting = quoteTemplateBuildArges.QuoteTemplateSettingPM;
            string translateInclueLable = GetTranslateInclueLable(quoteTemplateBuildArges.QuoteTemplateTextCodePMLists, pricingSectionType);

            PricingTableRowDetailsArgs pricingTableRowDetailsArgs = new PricingTableRowDetailsArgs() {QuoteTemplateSettingPM = setting, HeaderDesign = quoteTemplateTextDesignLines, TableDesign = quotetemplatetableDesignPM , PricingSectionType = pricingSectionType};


            int i = -1;
            foreach (QuoteSaleChargePM chargePM in quoteSaleCharges)
            {
                bool included = (chargePM.IsAllIN != null && chargePM.IsAllIN.ToLower() == "true" ? true : false);
                ++i;
                HtmlTemplate.Append("<tr style= 'height:auto; width:auto;vertical-align:central'>");

                if (pricingSectionType == "PP")
                {
                    if (setting.ShowChargeCodePackages)
                    {

                        HtmlTemplate.Append(AddTableRows(new PricingTableRowDetailsArgs() { Value = chargePM.ChargesTypeCode, RowDataType = "Field", QuoteTemplateSettingPM = setting, HeaderDesign = quoteTemplateTextDesignLines, TableDesign = quotetemplatetableDesignPM, PricingSectionType = pricingSectionType }));

                    }

                        if (setting.ShowChargeNamePackages)
                    {
                        string text = setting.ShowLocalLanguage && chargePM.ChargesTypeLocalName != null ? chargePM.ChargesTypeLocalName : chargePM.ChargesTypeName;
                        HtmlTemplate.Append(AddTableRows(new PricingTableRowDetailsArgs() {FieldCode = "CHARGEPACKAGES", Value = text, RowDataType = "Field" , QuoteTemplateSettingPM = setting, HeaderDesign = quoteTemplateTextDesignLines, TableDesign = quotetemplatetableDesignPM, PricingSectionType = pricingSectionType }));
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
                        HtmlTemplate.Append(AddTableRows(new PricingTableRowDetailsArgs() { Value = text, RowDataType = "FieldPrice", QuoteTemplateSettingPM = setting, HeaderDesign = quoteTemplateTextDesignLines, TableDesign = quotetemplatetableDesignPM, PricingSectionType = pricingSectionType }));

                    }

                    if (setting.ShowUnitPricePackages)
                    {
                        string saleUnitPriceValues = GetUnitPricePackagesValue(quoteTemplateBuildArges, chargePM, included);

                        HtmlTemplate.Append(AddTableRows(new PricingTableRowDetailsArgs() {Value = saleUnitPriceValues, RowDataType = "FieldPrice", QuoteTemplateSettingPM = setting, HeaderDesign = quoteTemplateTextDesignLines, TableDesign = quotetemplatetableDesignPM, PricingSectionType = pricingSectionType }));


                    }

                    if (setting.ShowMeasurementPackages)
                    {
                        var text = setting.RightToLeft ? chargePM.SaleMeasurementLocalName : chargePM.SaleMeasurementShortName;
                        HtmlTemplate.Append(AddTableRows(new PricingTableRowDetailsArgs() { Value = text, RowDataType = "FieldP", QuoteTemplateSettingPM = setting, HeaderDesign = quoteTemplateTextDesignLines, TableDesign = quotetemplatetableDesignPM, PricingSectionType = pricingSectionType }));

                    }



                    if (setting.ShowSaleCurrencyColumnPackages)
                    {
                        string text = " ";
                        if (chargePM.SaleTotalAmount != null)
                        {
                            double value = (double)chargePM.SaleTotalAmount;
                            text = value.ToString("N") + " " + GetChargeCurrencyCode(quotePM, chargePM);
                        }
                        if (included) text = translateInclueLable;
                        HtmlTemplate.Append(AddTableRows(new PricingTableRowDetailsArgs() { Value = text, RowDataType = "FieldPrice", QuoteTemplateSettingPM = setting, HeaderDesign = quoteTemplateTextDesignLines, TableDesign = quotetemplatetableDesignPM, PricingSectionType = pricingSectionType }));



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
                        HtmlTemplate.Append(AddTableRows(new PricingTableRowDetailsArgs() { Value = text, RowDataType = "FieldPrice", QuoteTemplateSettingPM = setting, HeaderDesign = quoteTemplateTextDesignLines, TableDesign = quotetemplatetableDesignPM, PricingSectionType = pricingSectionType }));

                    }

                        if (setting.ShowChargeDescriptionPackages)
                    {
                        HtmlTemplate.Append(AddTableRows(new PricingTableRowDetailsArgs() { FieldCode = "CHARGEDESCRIPTIONPACKAGES", Value = chargePM.ChargesTypeDescription, RowDataType = "Field", QuoteTemplateSettingPM = setting, HeaderDesign = quoteTemplateTextDesignLines, TableDesign = quotetemplatetableDesignPM, PricingSectionType = pricingSectionType }));

                    }

                    if (setting.ShowChargeNotePackages)
                    {
                        HtmlTemplate.Append(AddTableRows(new PricingTableRowDetailsArgs() { Value = chargePM.Notes, RowDataType = "Field", QuoteTemplateSettingPM = setting, HeaderDesign = quoteTemplateTextDesignLines, TableDesign = quotetemplatetableDesignPM, PricingSectionType = pricingSectionType }));


                    }
                    if (setting.ShowSaleMaxMinAmountPackages)
                    {
                        string text = included ? translateInclueLable : GetSaleMaxMinAmountValue(chargePM);
                        HtmlTemplate.Append(AddTableRows(new PricingTableRowDetailsArgs() { Value = text, RowDataType = "Field", QuoteTemplateSettingPM = setting, HeaderDesign = quoteTemplateTextDesignLines, TableDesign = quotetemplatetableDesignPM, PricingSectionType = pricingSectionType }));

                    }
                    if (setting.ShowRegionalTAXPackages)
                    {
                        string isRegionalTax = chargePM.IsRegionalTax ? "Yes" : "No";
                        HtmlTemplate.Append(AddTableRows(new PricingTableRowDetailsArgs() { Value = isRegionalTax, RowDataType = "Field", QuoteTemplateSettingPM = setting, HeaderDesign = quoteTemplateTextDesignLines, TableDesign = quotetemplatetableDesignPM, PricingSectionType = pricingSectionType }));

                    }

                    if (quotePM.IsChargesByVAT)
                    {
                        if (setting.ShowVATTypePackages)
                        {
                            HtmlTemplate.Append(AddTableRows(new PricingTableRowDetailsArgs() { FieldCode = "VATTYPEPACKAGES", Value = chargePM.VatTypeName, RowDataType = "Field", QuoteTemplateSettingPM = setting, HeaderDesign = quoteTemplateTextDesignLines, TableDesign = quotetemplatetableDesignPM, PricingSectionType = pricingSectionType }));
                        }
                        if (setting.ShowVATPercentagePackages)
                        {
                            string value = chargePM.VatPercentage != null ? (chargePM.VatPercentage + "%") : "";
                            HtmlTemplate.Append(AddTableRows(new PricingTableRowDetailsArgs() { FieldCode = "VATTYPEPACKAGES", Value = value, RowDataType = "Field", QuoteTemplateSettingPM = setting, HeaderDesign = quoteTemplateTextDesignLines, TableDesign = quotetemplatetableDesignPM, PricingSectionType = pricingSectionType }));
                        }
                    }


                }
                else if (pricingSectionType == "PC")
                {
                    int row = 0;
               

                    if (setting.ShowChargeCodeContainers)
                    {
                        HtmlTemplate.Append(AddTableRows(new PricingTableRowDetailsArgs() { Value = chargePM.ChargesTypeCode, RowDataType = "Field", QuoteTemplateSettingPM = setting, HeaderDesign = quoteTemplateTextDesignLines, TableDesign = quotetemplatetableDesignPM, PricingSectionType = pricingSectionType }));

                        row += 1;
                    }

                        if (setting.ShowChargeNameContainers)
                    {
                        if (setting.ShowLocalLanguage && chargePM.ChargesTypeLocalName != null)
                        {
                            HtmlTemplate.Append(AddTableRows(new PricingTableRowDetailsArgs() { FieldCode = "CHARGECONTAINERS", Value = chargePM.ChargesTypeLocalName, RowDataType = "Field", QuoteTemplateSettingPM = setting, HeaderDesign = quoteTemplateTextDesignLines, TableDesign = quotetemplatetableDesignPM, PricingSectionType = pricingSectionType }));

                        }
                        else
                        {
                            HtmlTemplate.Append(AddTableRows(new PricingTableRowDetailsArgs() { Value = chargePM.ChargesTypeName, RowDataType = "Field", QuoteTemplateSettingPM = setting, HeaderDesign = quoteTemplateTextDesignLines, TableDesign = quotetemplatetableDesignPM, PricingSectionType = pricingSectionType }));

                        }
                        row += 1;
                    }

                    if (setting.ShowMeasurementContainers)
                    {
                        var value = setting.RightToLeft ? chargePM.SaleMeasurementLocalName : chargePM.SaleMeasurementShortName;
                        HtmlTemplate.Append(AddTableRows(new PricingTableRowDetailsArgs() { Value = value, RowDataType = "Field", QuoteTemplateSettingPM = setting, HeaderDesign = quoteTemplateTextDesignLines, TableDesign = quotetemplatetableDesignPM, PricingSectionType = pricingSectionType }));




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
                                    fixedPrice = value.ToString("N") + " " + GetChargeCurrencyCode(quotePM, chargePM);
                                }
                            }
                            if (included) fixedPrice = translateInclueLable;

                            HtmlTemplate.Append(AddTableRows(new PricingTableRowDetailsArgs() { Value = fixedPrice, RowDataType = "FieldPrice", QuoteTemplateSettingPM = setting, HeaderDesign = quoteTemplateTextDesignLines, TableDesign = quotetemplatetableDesignPM, PricingSectionType = pricingSectionType }));

                            row += 1;
                        }

                    }

                    if (setting.ShowPriceByContainerColumn)
                    {
                        if (quotePM.PackageType1Id != null)
                        {
                            if (setting.ShowPriceByContainerColumn)
                            {
                                string saleContainerType1UnitPrice = "";
                                double value = 0;

                                row += 1;
                                if (chargePM.SaleContainerType1UnitPrice != null)
                                {
                                    value = (double)chargePM.SaleContainerType1UnitPrice;
                                    saleContainerType1UnitPrice = value.ToString("N") + " " + GetChargeCurrencyCode(quotePM, chargePM); ;
                                }

                                if(included) saleContainerType1UnitPrice = translateInclueLable;

                                HtmlTemplate.Append(AddTableRows(new PricingTableRowDetailsArgs() { Value = saleContainerType1UnitPrice, RowDataType = "FieldPrice", QuoteTemplateSettingPM = setting, HeaderDesign = quoteTemplateTextDesignLines, TableDesign = quotetemplatetableDesignPM, PricingSectionType = pricingSectionType }));
                            }

                        }

                        if (quotePM.PackageType2Id != null)
                        {
                            if (setting.ShowPriceByContainerColumn)
                            {
                                string saleContainerType2UnitPrice = "";
                                double value = 0;
                                row += 1;
                                if (chargePM.SaleContainerType2UnitPrice != null)
                                {

                                    value = (double)chargePM.SaleContainerType2UnitPrice;
                                    saleContainerType2UnitPrice = value.ToString("N") + " " + GetChargeCurrencyCode(quotePM, chargePM);
                                }
                                if (included) saleContainerType2UnitPrice = translateInclueLable;

                                HtmlTemplate.Append(AddTableRows(new PricingTableRowDetailsArgs() { Value = saleContainerType2UnitPrice, RowDataType = "FieldPrice", QuoteTemplateSettingPM = setting, HeaderDesign = quoteTemplateTextDesignLines, TableDesign = quotetemplatetableDesignPM, PricingSectionType = pricingSectionType }));

                            }
                        }

                        if (quotePM.PackageType3Id != null)
                        {
                            if (setting.ShowPriceByContainerColumn)
                            {
                                string saleContainerType3UnitPrice = "";
                                double value = 0;
                                row += 1;
                                if (chargePM.SaleContainerType3UnitPrice != null)
                                {
                                    value = (double)chargePM.SaleContainerType3UnitPrice;
                                    saleContainerType3UnitPrice = value.ToString("N") + " " + GetChargeCurrencyCode(quotePM, chargePM);

                                }
                                if (included) saleContainerType3UnitPrice = translateInclueLable;

                                HtmlTemplate.Append(AddTableRows(new PricingTableRowDetailsArgs() { Value = saleContainerType3UnitPrice, RowDataType = "FieldPrice", QuoteTemplateSettingPM = setting, HeaderDesign = quoteTemplateTextDesignLines, TableDesign = quotetemplatetableDesignPM, PricingSectionType = pricingSectionType }));

                            }
                        }

                        if (quotePM.PackageType4Id != null)
                        {
                            if (setting.ShowPriceByContainerColumn)
                            {
                                string saleContainerType4UnitPrice = "";
                                double value = 0;
                                row += 1;
                                if (chargePM.SaleContainerType4UnitPrice != null)
                                {
                                    value = (double)chargePM.SaleContainerType4UnitPrice;
                                    saleContainerType4UnitPrice = value.ToString("N") + " " + GetChargeCurrencyCode(quotePM, chargePM);
                                }
                                if (included) saleContainerType4UnitPrice = translateInclueLable;
                                HtmlTemplate.Append(AddTableRows(new PricingTableRowDetailsArgs() { Value = saleContainerType4UnitPrice, RowDataType = "FieldPrice", QuoteTemplateSettingPM = setting, HeaderDesign = quoteTemplateTextDesignLines, TableDesign = quotetemplatetableDesignPM, PricingSectionType = pricingSectionType }));

                            }
                        }

                        if (quotePM.PackageType5Id != null)
                        {
                            if (setting.ShowPriceByContainerColumn)
                            {
                                string saleContainerType5UnitPrice = "";
                                double value = 0;
                                row += 1;
                                if (chargePM.SaleContainerType5UnitPrice != null)
                                {
                                    value = (double)chargePM.SaleContainerType5UnitPrice;
                                    saleContainerType5UnitPrice = value.ToString("N") + " " + GetChargeCurrencyCode(quotePM, chargePM);
                                }
                                if (included) saleContainerType5UnitPrice = translateInclueLable;
                                HtmlTemplate.Append(AddTableRows(new PricingTableRowDetailsArgs() { Value = saleContainerType5UnitPrice, RowDataType = "FieldPrice", QuoteTemplateSettingPM = setting, HeaderDesign = quoteTemplateTextDesignLines, TableDesign = quotetemplatetableDesignPM, PricingSectionType = pricingSectionType }));

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
                            saleTotalAmount = value.ToString("N") + " " + GetChargeCurrencyCode(quotePM, chargePM);
                        }
                        if (included) saleTotalAmount = translateInclueLable;

                        HtmlTemplate.Append(AddTableRows(new PricingTableRowDetailsArgs() { Value = saleTotalAmount, RowDataType = "FieldPrice", QuoteTemplateSettingPM = setting, HeaderDesign = quoteTemplateTextDesignLines, TableDesign = quotetemplatetableDesignPM, PricingSectionType = pricingSectionType }));

                    }

                    if (setting.ShowLocalCurrencyColumnContainers)
                    {
                        row += 1;
                  
                        string value = GetNumberValueFormate(chargePM.SaleTotalAmountLocal);

                        string localTotalAmount = value + " " + LocalCurrencyCode;

                        if (included) localTotalAmount = translateInclueLable;

                        HtmlTemplate.Append(AddTableRows(new PricingTableRowDetailsArgs() { Value = localTotalAmount, RowDataType = "FieldPrice", QuoteTemplateSettingPM = setting, HeaderDesign = quoteTemplateTextDesignLines, TableDesign = quotetemplatetableDesignPM, PricingSectionType = pricingSectionType }));

                    }


                    if (setting.ShowChargeDescriptionContainers)
                    {
                        row += 1;

                        HtmlTemplate.Append(AddTableRows(new PricingTableRowDetailsArgs() {FieldCode = "CHARGEDESCRIPTIONCONTAINERS", Value = chargePM.ChargesTypeDescription, RowDataType = "Field", QuoteTemplateSettingPM = setting, HeaderDesign = quoteTemplateTextDesignLines, TableDesign = quotetemplatetableDesignPM, PricingSectionType = pricingSectionType }));


                    }

                    if (setting.ShowChargeNoteContainers)
                    {
                        HtmlTemplate.Append(AddTableRows(new PricingTableRowDetailsArgs() { Value = chargePM.Notes, RowDataType = "Field", QuoteTemplateSettingPM = setting, HeaderDesign = quoteTemplateTextDesignLines, TableDesign = quotetemplatetableDesignPM, PricingSectionType = pricingSectionType }));


                    }

                    if (setting.ShowSaleMaxMinAmountContainers)
                    {
                        string saleMaxMinAmount = included ? translateInclueLable : GetSaleMaxMinAmountValue(chargePM);
                        HtmlTemplate.Append(AddTableRows(new PricingTableRowDetailsArgs() { Value = saleMaxMinAmount, RowDataType = "Field", QuoteTemplateSettingPM = setting, HeaderDesign = quoteTemplateTextDesignLines, TableDesign = quotetemplatetableDesignPM, PricingSectionType = pricingSectionType }));
                    }

                    if (setting.ShowRegionalTAXContainers)
                    {
                        string isRegionalTax = chargePM.IsRegionalTax ? "Yes" : "No";
                        HtmlTemplate.Append(AddTableRows(new PricingTableRowDetailsArgs() { Value = isRegionalTax, RowDataType = "Field", QuoteTemplateSettingPM = setting, HeaderDesign = quoteTemplateTextDesignLines, TableDesign = quotetemplatetableDesignPM, PricingSectionType = pricingSectionType }));
                    }

                    if (quotePM.IsChargesByVAT)
                    {
                        if (setting.ShowVATTypeContainers)
                        {
                            HtmlTemplate.Append(AddTableRows(new PricingTableRowDetailsArgs() { FieldCode = "VATTYPECONTAINERS", Value = chargePM.VatTypeName, RowDataType = "Field", QuoteTemplateSettingPM = setting, HeaderDesign = quoteTemplateTextDesignLines, TableDesign = quotetemplatetableDesignPM, PricingSectionType = pricingSectionType }));
                        }
                        if (setting.ShowVATPercentageContainers)
                        {
                            string value = chargePM.VatPercentage!=null? (chargePM.VatPercentage + "%"):"";
                            HtmlTemplate.Append(AddTableRows(new PricingTableRowDetailsArgs() { FieldCode = "VATPERCENTAGECONTAINERS", Value = value, RowDataType = "Field", QuoteTemplateSettingPM = setting, HeaderDesign = quoteTemplateTextDesignLines, TableDesign = quotetemplatetableDesignPM, PricingSectionType = pricingSectionType }));
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

        private string GetUnitPricePackagesValue(QuoteTemplateBuildArges quoteTemplateBuildArges, QuoteSaleChargePM chargePM, bool included)
        {
            string saleUnitPriceValues = "";
            QuotePM quotePM = quoteTemplateBuildArges.QuotePM;

            if (chargePM.IsChargeBySteps && quotePM.QuoteTypeCode == "P")
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
                        if (i > 1 || quoteTemplateBuildArges.QuoteTemplateSettingPM.RightToLeft) priceBreaks += "&nbsp;";
                        priceBreaks += ((priceBreak + " " + GetChargeCurrencyCode(quotePM, chargePM)) + "<br>");


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
                    saleUnitPriceValues = GetNameColum("INCLUDED", quoteTemplateBuildArges.QuoteTemplateTextCodePMLists, quoteTemplateBuildArges.SectionTypeCode);
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


        private static string GetChargeCurrencyCode(QuotePM quotePM, QuoteSaleChargePM chargePM)
        {
            return (quotePM.IsSaleCurrencySameAsCost ? chargePM.CurrencyCode : quotePM.SaleCurrencyCode);
        }

        private static string GetFormatDisplayAmountWithCurrencyCode(string saleTotalAmount, string currencyCode, QuoteTemplateSettingPM setting)
        {
            string result = saleTotalAmount + " " + currencyCode;
         
            return result;
        }



        private string GetSaleMaxMinAmountValue(QuoteSaleChargePM chargePM)
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
        private void BuildTotalPerContainerClassLists(QuotePM quotePM, List<QuoteSaleChargePM> quoteSaleCharges, StringBuilder HtmlTemplate, QuoteTemplateSettingPM setting, List<TotalPerContainerClass> totalPerContainerClassLists)
        {
            QuoteSaleChargePM freightQuoteSaleChargePM = quoteSaleCharges.Where(d => d.ChargesGroupCode == "FRT").FirstOrDefault();
            foreach (QuoteSaleChargePM quoteSaleChargePM in quoteSaleCharges)
            {

                if (quotePM.PackageType1Id != null)
                {
                    double? saleUnitPrice1InSaleCurrency = GetSaleUnitPriceInSaleCurrency("SaleUnitPrice1InSaleCurrency", quoteSaleChargePM, freightQuoteSaleChargePM);
                    ComputedTotalPerContainer(totalPerContainerClassLists, quotePM.PackageType1Id, saleUnitPrice1InSaleCurrency, quotePM.PackageType1Quantity, "PackageType1Id", quoteSaleChargePM);
                }

                if (quotePM.PackageType2Id != null)
                {
                    double? saleUnitPrice2InSaleCurrency = GetSaleUnitPriceInSaleCurrency("SaleUnitPrice2InSaleCurrency", quoteSaleChargePM, freightQuoteSaleChargePM);

                    ComputedTotalPerContainer(totalPerContainerClassLists, quotePM.PackageType2Id, saleUnitPrice2InSaleCurrency, quotePM.PackageType2Quantity, "PackageType2Id", quoteSaleChargePM);
                }

                if (quotePM.PackageType3Id != null)
                {
                    double? saleUnitPrice3InSaleCurrency = GetSaleUnitPriceInSaleCurrency("SaleUnitPrice3InSaleCurrency", quoteSaleChargePM, freightQuoteSaleChargePM);

                    ComputedTotalPerContainer(totalPerContainerClassLists, quotePM.PackageType3Id, saleUnitPrice3InSaleCurrency, quotePM.PackageType3Quantity, "PackageType3Id", quoteSaleChargePM);
                }

                if (quotePM.PackageType4Id != null)
                {
                    double? saleUnitPrice4InSaleCurrency = GetSaleUnitPriceInSaleCurrency("SaleUnitPrice4InSaleCurrency", quoteSaleChargePM, freightQuoteSaleChargePM);

                    ComputedTotalPerContainer(totalPerContainerClassLists, quotePM.PackageType4Id, saleUnitPrice4InSaleCurrency, quotePM.PackageType4Quantity, "PackageType4Id", quoteSaleChargePM);
                }

                if (quotePM.PackageType5Id != null)
                {
                    double? saleUnitPrice5InSaleCurrency = GetSaleUnitPriceInSaleCurrency("SaleUnitPrice5InSaleCurrency", quoteSaleChargePM, freightQuoteSaleChargePM);

                    ComputedTotalPerContainer(totalPerContainerClassLists, quotePM.PackageType5Id, saleUnitPrice5InSaleCurrency, quotePM.PackageType5Quantity, "PackageType5Id", quoteSaleChargePM);
                }

            }
        }

        private double? GetSaleUnitPriceInSaleCurrency(string fieldName, QuoteSaleChargePM quoteSaleChargePM, QuoteSaleChargePM fRTQuoteSaleChargePM)
        {
            double? result =0;
            var entityPM = quoteSaleChargePM.SaleMeasurementCode != "PRFR" ? quoteSaleChargePM : fRTQuoteSaleChargePM;
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

        private void ComputedTotalPerContainer(List<TotalPerContainerClass> totalPerContainerClassLists, string PackageTypeId, double? saleUnitPriceInSaleCurrency, int? packageTypeQuantity, string fieldCode, QuoteSaleChargePM chargePM)
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
                value = (double)(saleUnitPriceInSaleCurrency * chargePM.SaleUnitPrice) / 100;
            }
            else
            {
                if (chargePM.SaleAmountInSaleCurrency != null)
                {
                    value = (double)chargePM.SaleAmountInSaleCurrency;
                }
            }
            AddPerContainerClassToLists(totalPerContainerClassLists, value, name, fieldCode, chargePM, (double)orginalValue);

        }
        private static void AddPerContainerClassToLists(List<TotalPerContainerClass> totalPerContainerClassLists, double value, string name, string fieldCode, QuoteSaleChargePM chargePM , double orginalValue)
        {
            if (totalPerContainerClassLists != null)
            {
                TotalPerContainerClass totalPerContainerClass = new TotalPerContainerClass()
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
                    totalPerContainerClass.Included = true;
                    totalPerContainerClass.OrginalValue = 0;
                    totalPerContainerClass.Value = 0;
                }


                totalPerContainerClassLists.Add(totalPerContainerClass);
            }
        }

        #endregion

        private bool IsShowFixedPriceContainer(List<QuoteSaleChargePM> quoteSaleCharges)
        {
            bool viewFixedPriceContainer = false;

            foreach (QuoteSaleChargePM chargePM in quoteSaleCharges)
            {
                if (chargePM.SaleMeasurementCode == "FIXD")
                {
                    viewFixedPriceContainer = true;
                }
            };

            return viewFixedPriceContainer;
        }

        private string GetStyleRowTablePageHeaderFooter(string typetable, string height, double? width, string alignment, int loactionTd, QuoteTemplateTableDesignPM tabledesign, string BackgroundColorArea)
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

        public byte[] DownloadQuoteTemplateSectionDataFile(string documentId, int tenant)
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

        public byte[] HtmlToPdf(string bodyHtmlString, QuoteTemplateSettingPM setting)
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


    public class PricingTableRowDetailsArgs
    {
        public string Value { get; set; }
        public QuoteTemplateSettingPM QuoteTemplateSettingPM { get; set; }
        public string FieldCode { get; set; }
        public QuoteTemplateTextDesignPM HeaderDesign { get; set; }
        public QuoteTemplateTableDesignPM TableDesign { get; set; }
        public string PricingSectionType { get; set; }        
        public string RowDataType { get; set; }
        

    }



    public class TotalPerContainerClass
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

    public class QuoteTemplateBuildArges
    {
        public List<QuoteTemplateTableDesignPM> QuoteTemplateTableDesignsLists { get; set; }
        public List<QuoteTemplateTextCodePM> QuoteTemplateTextCodePMLists { get; set; }
        public QuotePM QuotePM { get; set; }
        public QuoteTemplatePM QuoteTemplatePM { get; set; }
        public QuoteTemplateSettingPM QuoteTemplateSettingPM { get; set; }
        public List<QuoteTemplateTextDesignPM> QuoteTemplateTextDesignPMLists { get; set; }
        public string SectionTypeCode { get; set; }
        public bool IsResultPDF { get; set; }
        public int Tenant { get; set; }
        public int? UserTenant { get; set; }
        public string UserId { get; set; }
        public List<QuoteTemplateSectionPM> QuoteTemplateSectionPMLists { get; set; }
        public bool HideQuoteHeaderFromPdf { get; set; }
        public string RequestArea { get; set; }
        public bool FirstSectionInBody { get; set; }
        public int? VersionNumber { get; set; }


    }


    public class ChargeGroupOrderList
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int GroupOrder { get; set; }
        public int ChargeTypeOrder { get; set; }
        public int Order { get; set; }
        public int IsOrder { get; set; }
    }


}

