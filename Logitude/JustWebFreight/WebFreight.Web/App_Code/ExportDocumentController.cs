using iTextSharp.text;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Export;
using Stimulsoft.Report.Units;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Xml;
using System.Xml.Serialization;
using WebFreight.Web.CommonDataModel.DomainServices;
using WebFreight.Web.DataContracts;
using WebFreight.Web.DataProviders;
using WebFreight.Web.Helpers;
using WebFreight.Web.ReportsWebServices;
using WebFreight.Web.Security;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.App_Code
{
    public class ExportDocumentController : ApiController
    {

        // GET api/<controller>/5


        public HttpResponseMessage GetDocumentPdfFile(string documentTypeId, string entityId, string entityObjectTableId, string childEntityId, string childObjectTableId, string documentOutId, int tenant, string documentTypeCopyId , string userId)
        {
            ExportDocumentHelper exportDocumentHelper = new ExportDocumentHelper();
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                if (tenant != authToken.Tenant)
                {
                    throw new Exception("Sorry you’re not authenticated");
                }


                var reslut = exportDocumentHelper.ExportDocument2Pdf(documentTypeId, entityId, entityObjectTableId, childEntityId, childObjectTableId, documentOutId, tenant, documentTypeCopyId, userId);
                return Request.CreateResponse(HttpStatusCode.OK, reslut);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetDownloadFileFromServer(string documentId, int tenant)
        {
            List<object> htmlResult = new List<object>();

            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                if (tenant != authToken.Tenant)
                {
                    throw new Exception("Sorry you’re not authenticated");
                }
                
                    ExportDocumentHelper ExportDocumentHelper = new ExportDocumentHelper();
                    HtmlEditorHelper htmlEditorHelper = new Helpers.HtmlEditorHelper();
                    htmlResult = ExportDocumentHelper.GetDownloadFileFromServer(documentId, tenant);
                
                return Request.CreateResponse(HttpStatusCode.OK, htmlResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetResetEditableFields(string documentoOutId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                DocumentOutRepository documentOutRepository = new DocumentOutRepository(authToken.Tenant);
                DocumentOut documentOut = documentOutRepository.GetSingleDocumentOut(documentoOutId, authToken.Tenant);
                string reslutXml = "<?xml version='1.0' encoding='utf-8' standalone='yes'?> <StiSerializer version='1.02' type='Silverlight' application='Editable Fields of Rendered Report'>   <Items isList='true' count='0' /> </StiSerializer>  ";

                System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
                Byte[]  bytedata = enc.GetBytes(reslutXml);
                documentOut.EditableFields = bytedata;
                documentOutRepository.Update(documentOut);
                documentOutRepository.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, bytedata);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        private byte[] GetBytes(string str)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);

            return bytes;
        }

        int PageNumber = 1;
        int PageCount = 0;
        string ReportKey = "";
        int Tenant = 0;
        bool IsDisplayOnly = false;


        public HttpResponseMessage PostReportStimulsoftViewer(ExportDocumentArgs filter)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                if(filter.Tenant != authToken.Tenant)
                {
                   throw new Exception("Sorry you’re not authenticated");
                }

                long theT1 = new long();
                long theT2 = new long();
                long theA1 = new long();
                long theA2 = new long();

                List<EditableFieldPosition> editableFieldPositionList = new List<EditableFieldPosition>();
                List<string> result = new List<string>();


                ICommonDataContext context = CommonDataContext.GetContext(filter.Tenant);
                CommonDataDomainService commonService = new CommonDataDomainService();
                Tenant currentTenant = context.Tenants.Where(t => t.Id == filter.Tenant).FirstOrDefault();
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                string datetimeformat = @"dd\/MM\/yyyy";
                if (!string.IsNullOrEmpty(currentTenant.DateTimeFormat))
                {
                    datetimeformat = currentTenant.DateTimeFormat;
                }

                Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = datetimeformat;
                Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortTimePattern = "HH:mm";

                Tenant = filter.Tenant;
                ReportKey = filter.ReportKey;
                IsDisplayOnly = filter.IsDisplayOnly;
                PageNumber = filter.PageNumber;
               
                DocumentOutRepository documentOutRepository = new DocumentOutRepository(filter.Tenant);
                DocumentOut documentOut = documentOutRepository.GetSingleDocumentOut(filter.CurrentDocumentOutId, filter.Tenant);

                if (string.IsNullOrEmpty(filter.ReportKey) || filter.RequestMethodType == "GenerateReport")
                {

                    Byte[] templatedata = null;
                    DocumentTypeRepository repository = new DocumentTypeRepository(filter.Tenant);
                  
                    DocumentRepository docRepository = new DocumentRepository(filter.Tenant);
                    DocumentTypeCopyRepository documentTypeCopyRep = new DocumentTypeCopyRepository(filter.Tenant);
                    DocumentTypeTemplateRepository documentTypeTemplaterep = new DocumentTypeTemplateRepository(filter.Tenant);
                    DocumentOutCopyRepository documentOutCopyRep = new DocumentOutCopyRepository(filter.Tenant);
                   
                    DocumentTypeTemplate template = documentTypeTemplaterep.GetSingleDocumentTypeTemplate(filter.DocumentTypeTemplateId);

                    DocumentTypeCopy documentTypeCopy = documentTypeCopyRep.GetSingleDocumentTypeCopy(filter.DocumentTypeCopyId);
                    DocumentType documentType = repository.GetSingleDocumentTypes(template.DocumentTypeId, filter.Tenant);
                    DocumentsFilingRepository documentsFilingRepository = new DocumentsFilingRepository(filter.Tenant);
                    ExportDocumentHelper exportDocumentHelper = new ExportDocumentHelper();


                    if (template != null) templatedata = template.TemplateBody;

                    if (templatedata != null)
                    {
                        if (templatedata.Length != 0)
                        {
                            StiReport report = new StiReport();

                            report = exportDocumentHelper.GetReportDocument(documentType, filter.EntityId, filter.ObjectTableId, filter.ChildEntityId, filter.ChildObjectTableId, documentTypeCopy, report, templatedata, template, filter.Tenant, theT1, theT2, theA1, theA2, filter.LoggedContactId);

                            string mdc = report.SaveDocumentToString();


                            result = GetReportImage(report, mdc, documentOut, filter.PageNumber);

                            #region Write Report To Storage

                            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

                            byte[] reportData = encoding.GetBytes(mdc);
                            if (reportData != null)
                            {
                                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                                ReportKey = Guid.NewGuid().ToString();
                                BlobFileInfo fileInfo = GetNewBlobFileInfo(ReportKey, Tenant);
                                fileInfo.FileSize = reportData.Length;
                                storageservice.Write(reportData, fileInfo);
                            }


                            #endregion


                        }
                    }

                    editableFieldPositionList = BulidEditableFieldPositionList(filter.IsDisplayOnly, result, ReportKey, filter.PageNumber);

                }
                else
                {
                    result = GetReportAsImageFromStorage(filter.ReportKey, filter.Tenant, filter.PageNumber, documentOut);

                    editableFieldPositionList = BulidEditableFieldPositionList(filter.IsDisplayOnly, result, ReportKey, filter.PageNumber);

                }


                return Request.CreateResponse(HttpStatusCode.OK, editableFieldPositionList);

            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        private List<EditableFieldPosition> BulidEditableFieldPositionList(bool isDisplayOnly, List<string> result ,string reportKey , int pagenumber)
        {

            List<EditableFieldPosition> editableFieldPositionList = new List<EditableFieldPosition>();
            if (isDisplayOnly)
            {
                if (result.Count > 0)
                {
                    editableFieldPositionList.Add(new EditableFieldPosition() { FieldName = "Image", FieldValue = result[0], PageCount = PageCount, ReportKey = reportKey });
                }
            }

            else
            {
                if (result != null && result.Count > 1)
                {
                    string xmal = "";
                    string reportunit = "";
                    string editablefield = "";

                    if (result.Count > 1) xmal = result[1];
                    if (result.Count > 2) reportunit = result[2];
                    if (result.Count > 3) editablefield = result[3];


                    XmlNodeList childFieldNodes = null;

     

                    if (!string.IsNullOrEmpty(editablefield))
                    {
                        try
                        {
                            XmlReader readerfield = XmlReader.Create(new StringReader(editablefield));
                            XmlDataDocument messageFieldDoc = new XmlDataDocument();
                            messageFieldDoc.Load(readerfield);
                            XmlNodeList ItemsFieldList = messageFieldDoc.GetElementsByTagName("Items");
                            if (ItemsFieldList != null  && ItemsFieldList.Count>0 ) childFieldNodes = ItemsFieldList[0].ChildNodes;
                       
                        }
                        catch (Exception ex)
                        {

                        }

                    }

                    int  numberOfEditedField = 0;

                    editableFieldPositionList = GetEditableFieldPosition(xmal, reportunit, childFieldNodes, pagenumber, ref numberOfEditedField);

                    editableFieldPositionList.Add(new EditableFieldPosition() { FieldName = "NumberOfEditedField", FieldValue = numberOfEditedField.ToString(), PageCount = PageCount, ReportKey = reportKey });

                    if (result.Count > 0)
                    {
                        editableFieldPositionList.Add(new EditableFieldPosition() { FieldName = "Image", FieldValue = result[0], PageCount = PageCount, ReportKey = reportKey });
                    }
                }
                
            }
            return editableFieldPositionList;
        }

        private List<string> GetReportAsImageFromStorage(string reportKey, int tenant, int pagenumber, DocumentOut documentOut)
        {
         
            List<string> results = new List<string>();
           
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;

            BlobFileInfo fileInfo = GetNewBlobFileInfo(reportKey, tenant);
            byte[] result = storageservice.Read(fileInfo);

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            string mdc = encoding.GetString(result);

            StiReport stiReport = new StiReport();
            stiReport.LoadDocumentFromString(mdc);
            results = GetReportImage(stiReport, mdc, documentOut, pagenumber);
            return results;

        }

        private BlobFileInfo GetNewBlobFileInfo(string reportKey, int tenant)
        {
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = reportKey,
                FolderName = "others",
                Extension = "mdc",
                Tenant = tenant,
            };
            return fileInfo;
        }



        private string ResolveSpecialCharacters(string editableFields)
        {
            editableFields = editableFields.Replace("<", "&lt;");
            editableFields = editableFields.Replace(">" , "&gt;");
            editableFields = editableFields.Replace("\"" , "&quot;");
            editableFields = editableFields.Replace("\'" , "&apos;");
            editableFields = editableFields.Replace("&" , "&amp;");
            return editableFields;
        }

        private List<string> GetReportImage(StiReport stiReport, string mdc, DocumentOut documentOut, int pagenumber)
        {

            string editableFields = "";

            string units = stiReport.ReportUnit.ToString();

            if (documentOut != null && documentOut.EditableFields != null)
            {

                editableFields = System.Text.Encoding.UTF8.GetString(documentOut.EditableFields);
               // if (!string.IsNullOrEmpty(editableFields)) editableFields = ResolveSpecialCharacters(editableFields);

                Stream editableFieldsStream;
                using (editableFieldsStream = new MemoryStream())
                {
                    editableFieldsStream.Write(documentOut.EditableFields, 0, documentOut.EditableFields.Length);

                    editableFieldsStream.Seek(0, SeekOrigin.Begin);
                    stiReport.LoadEditableFields(editableFieldsStream);
                    editableFieldsStream.Dispose();
                }
            }


            MemoryStream memoryStream = new MemoryStream();
            List<string> results = new List<string>();
            stiReport.ExportDocument(StiExportFormat.ImagePng, memoryStream, new StiPngExportSettings() { PageRange = new StiPagesRange(StiRangeType.Pages, pagenumber.ToString(), pagenumber), ImageResolution = 200, ImageFormat = StiImageFormat.Color });



            PageCount = stiReport.RenderedPages.Count;
        
            string base64String = System.Convert.ToBase64String(memoryStream.ToArray(), 0, memoryStream.ToArray().Length);

            string uri = "data:image/jpg;base64," + base64String;
            results.Add(uri);
            results.Add(mdc);
            results.Add(units);
            results.Add(editableFields);
            memoryStream.Dispose();
            return results;
        }



        private List<EditableFieldPosition> GetEditableFieldPosition(string xmal, string reportUnit, XmlNodeList childFieldNodes, int pagenumber ,ref int numberOfEditedField)
        {
            double pageWidth  = 0;
            double pageHeight = 0;
            double margeleft = 0;
            double margeRight = 0;
            XmlNode componentXmlNode = null;
    
            EditableFieldPosition editableFieldPosition;

        
            List<EditableFieldPosition> editableFieldPositionLists = new List<EditableFieldPosition>();
            if (!string.IsNullOrEmpty(xmal))
            {
                XmlReader reader = XmlReader.Create(new StringReader(xmal));
                XmlDataDocument messageDoc = new XmlDataDocument();
                messageDoc.Load(reader);

                GetPropertiesValueFromXmlDataDocumen(pagenumber, messageDoc,  ref pageWidth , ref pageHeight , ref margeleft ,ref margeRight, ref componentXmlNode);  
      
                if (componentXmlNode != null)
                {
                    List<XmlNode> childNodes = (new System.Collections.Generic.List<XmlNode>(Shim<XmlNode>(componentXmlNode.ChildNodes))).Where(d => d.Attributes["ed"]!=null || (d.Attributes["type"] != null && d.Attributes["type"].Value!=null && d.Attributes["type"].Value.ToLower() == "checkbox")).ToList();
                    foreach (XmlNode item in childNodes)
                    {
                        bool isEditable = false;

                        string controltype = "text";
                        var type = item.Attributes["type"];
                        if (type != null && !string.IsNullOrEmpty(type.Value))
                        {
                            controltype = type.Value.ToLower();
                        }

                        var ed = item.Attributes["ed"];

                        #region Check checkbox is iseditable
                        if (controltype == "checkbox")
                        {
                            XmlNode editableChexkBoxNode = (new System.Collections.Generic.List<XmlNode>(Shim<XmlNode>(item.ChildNodes))).Where(d => d.Name == "Editable").FirstOrDefault();
                            if (editableChexkBoxNode != null)
                            {
                                var value = editableChexkBoxNode.InnerText != null ? editableChexkBoxNode.InnerText : "";
                                if (value.ToLower() == "true") isEditable = true;
                            }

                        }
                        #endregion


                        if (ed != null || (controltype == "checkbox" && isEditable))
                        {
                    


                            string fontFamily = "Arial";
                            double fontSize = 17;
                            string textColor = "Black";
                            string floatText = "left";
                            string textAligh = "left";
                            string verticalAlign = "top";
                            string fontweight = string.Empty;
                            double left = 0;
                            double top = 0;
                            double widht = 0;
                            double height = 0;
                            double customPageWidth = 0;
                            double customPageHeight = 0;
                            string fieldName = "";
                            string fieldValue = "";
                            string oldValue = "";
                            int fieldPosition = 1;
                            bool isEditedField = false;
                           
                            string[] recInfo = new string[] { "0", "0", "0", "0" };
                            string[] fontInfo = new string[] { "Arial", "17", "" };

                            if (controltype == "text")
                            {
                                fieldName = item.Attributes["name"].Value;
                                fieldValue = item.Attributes["text"].Value;
                                oldValue = item.Attributes["text"].Value;
                            }
                            else if (controltype == "checkbox")
                            {
                                fieldValue = "false";
                  
                                List<XmlNode> itemchildNodes = (new System.Collections.Generic.List<XmlNode>(Shim<XmlNode>(item.ChildNodes))).Where(d =>d.Name == "ClientRectangle" || d.Name == "Name").ToList();
                                foreach (XmlNode node in itemchildNodes)
                                {
                                    if (node.Name == "ClientRectangle") recInfo = node.InnerText != null ? node.InnerText.Split(',') : recInfo;

                                    else if (node.Name == "Name")
                                    {
                                        fieldName = node.InnerText != null ? node.InnerText : "";
                                        oldValue = "false";
                                    }

                                }
                            }


                          EditableFieldPosition editableField = editableFieldPositionLists.Where(d => d.FieldName == fieldName && d.PageFieldIndex == (PageNumber - 1)).FirstOrDefault();
                            if (editableField != null)
                            {
                                fieldPosition = editableFieldPositionLists.Where(d => d.FieldName == fieldName && d.PageFieldIndex == (PageNumber - 1)).Count() + 1;

                            }

                            //  Replace FieldValue width  EditableFields

                            if (childFieldNodes != null)
                            {
                                XmlNode field = (new System.Collections.Generic.List<XmlNode>(Shim<XmlNode>(childFieldNodes))).Where(d => (d["ComponentName"] != null ? d["ComponentName"].InnerText : "").ToLower() == fieldName.ToLower() && (d["PageIndex"] != null ? d["PageIndex"].InnerText : "") == (pagenumber - 1).ToString() && (d["Position"] != null ? d["Position"].InnerText : "").ToLower() == fieldPosition.ToString()).FirstOrDefault();

                                if (field != null)
                                {
                                    var value = field["TextValue"] != null ? field["TextValue"].InnerText : "";
                                    if (fieldValue != value)
                                    {
                                        fieldValue = value;
                                        isEditedField = true;
                                        numberOfEditedField += 1;
                                    }
                                }
                            }

                                // Rec Info Postion


                                if (controltype == "text")
                                {

                                    if (item.Attributes["rc"] != null)
                                    {
                                        recInfo = !string.IsNullOrEmpty(item.Attributes["rc"].Value) ? item.Attributes["rc"].Value.Split(',') : recInfo;
                                    }

                                    // Font Info

                                    if (item.Attributes["fn"] != null)
                                    {
                                        fontInfo = !string.IsNullOrEmpty(item.Attributes["fn"].Value) ? item.Attributes["fn"].Value.Split(',') : fontInfo;
                                    }

                                    if (fontInfo.Length > 0) fontFamily = !string.IsNullOrEmpty(fontInfo[0]) ? fontInfo[0] : "Arial";
                                    if (fontInfo.Length > 1) fontSize = !string.IsNullOrEmpty(fontInfo[1]) ? ConvertFromPointToPixel(Double.Parse(fontInfo[1])) : 17;
                                    if (fontInfo.Length > 2) fontweight = !string.IsNullOrEmpty(fontInfo[2]) ? fontInfo[2] : "";


                                    //RightToLeft
                                    if (item.Attributes["RightToLeft"] != null)
                                    {
                                        string RTL = item.Attributes["RightToLeft"].Value;
                                        floatText = !string.IsNullOrEmpty(RTL) ? RTL.ToLower() == "true" ? "right" : "left" : "left";

                                    }


                                //Text Color
                                if (item.Attributes["tb"] != null)
                                {
                                    textColor = !string.IsNullOrEmpty(item.Attributes["tb"].Value) ? item.Attributes["tb"].Value : "Black";
                                    if (textColor.Contains(':'))
                                    {
                                        string[] RGBColor = textColor.Replace("[", "").Replace("]", "").Split(':');
                                        if (RGBColor != null && RGBColor.Length == 3)
                                        {
                                            textColor = ConvertFromRGBToColor(RGBColor);
                                        }
                                    }
                                }


                                    //Text Align
                                    if (item.Attributes["ha"] != null)
                                    {
                                        textAligh = !string.IsNullOrEmpty(item.Attributes["ha"].Value) ? item.Attributes["ha"].Value : "left";
                                    }


                                    //Vertical Align
                                    if (item.Attributes["va"] != null)
                                    {
                                        verticalAlign = !string.IsNullOrEmpty(item.Attributes["va"].Value) ? item.Attributes["va"].Value : "top";
                                    }

                                }
                              




                            if (reportUnit == "Millimeters")
                            {
                                left = !string.IsNullOrEmpty(recInfo[0]) ? (Double.Parse(recInfo[0]) / 10) + (margeleft / 10) : 0;
                                top = !string.IsNullOrEmpty(recInfo[1]) ? (Double.Parse(recInfo[1]) / 10) + (margeRight / 10) : 0;
                                widht = !string.IsNullOrEmpty(recInfo[2]) ? Double.Parse(recInfo[2]) / 10 : 0;
                                height = !string.IsNullOrEmpty(recInfo[3]) ? Double.Parse(recInfo[3]) / 10 : 0;
                                customPageWidth = pageWidth / 10;
                                customPageHeight = pageHeight / 10;

                            }
                            else if (reportUnit == "Inches" || reportUnit == "HundredthsOfInch")
                            {
                                left = !string.IsNullOrEmpty(recInfo[0]) ? (Double.Parse(recInfo[0]) * 2.54) + (margeleft * 2.54) : 0;
                                top = !string.IsNullOrEmpty(recInfo[1]) ? (Double.Parse(recInfo[1]) * 2.54) + (margeRight * 2.54) : 0;
                                widht = !string.IsNullOrEmpty(recInfo[2]) ? Double.Parse(recInfo[2]) * 2.54 : 0;
                                height = !string.IsNullOrEmpty(recInfo[3]) ? Double.Parse(recInfo[3]) * 2.54 : 0;

                                customPageWidth = pageWidth * 2.54;
                                customPageHeight = pageHeight * 2.54;
                            }


                            else if (reportUnit == "Centimeters")
                            {
                                left = !string.IsNullOrEmpty(recInfo[0]) ? Double.Parse(recInfo[0]) + margeleft : 0;
                                top = !string.IsNullOrEmpty(recInfo[1]) ? Double.Parse(recInfo[1]) + margeRight : 0;
                                widht = !string.IsNullOrEmpty(recInfo[2]) ? Double.Parse(recInfo[2]) : 0;
                                height = !string.IsNullOrEmpty(recInfo[3]) ? Double.Parse(recInfo[3]) : 0;

                                customPageWidth = pageWidth;
                                customPageHeight = pageHeight;
                            }

                            //  image 200 / 2.54 dp
                            double perc = 200 / 2.54;


                            editableFieldPosition = new EditableFieldPosition()
                            {
                                Left = left * perc,
                                Top = top * perc,
                                Width = widht * perc,
                                Height = height * perc,
                                FieldName = fieldName,
                                FieldValue = fieldValue,
                                FontFamily = fontFamily,
                                FontSize = fontSize,
                                Fontweight = fontweight,
                                TextColor = textColor,
                                TextAligh = textAligh,
                                VerticalAlign = verticalAlign,
                                Float = floatText,
                                BorderBrach = "",
                                WidthPagePrecentage = customPageWidth * perc,
                                HeightPagePrecentage = customPageHeight * perc,
                                PageFieldIndex = (PageNumber - 1),
                                FieldPosition = fieldPosition.ToString(),
                                IsEditedField = isEditedField,
                                ControlType = controltype,
                                OldValue = oldValue,
                            };

                            editableFieldPositionLists.Add(editableFieldPosition);
                        }
                   
                       

                    }
                }
           
            }
            return editableFieldPositionLists;


        }


        private static IEnumerable<T> Shim<T>(System.Collections.IEnumerable enumerable)
        {
            foreach (object current in enumerable)
            {
                yield return (T)current;
            }
        }
        private  void GetPropertiesValueFromXmlDataDocumen(int pageNumber, XmlDataDocument messageDoc, ref double pageWidth, ref double pageHeight, ref double margeleft, ref double margeRight, ref XmlNode componentXmlNode )
        {
            int pageIndex = pageNumber - 1;
               
            List<string> types = new List<string>() { "Margins", "PageWidth", "PageHeight", "Components" };
            foreach (string type in types)
            {

                XmlNodeList xmlNodeList = messageDoc.GetElementsByTagName(type);

                if (xmlNodeList != null && xmlNodeList.Count > 0)
                {
                    List<XmlNode> xmlNodeLists = (new System.Collections.Generic.List<XmlNode>(Shim<XmlNode>(xmlNodeList))).Where(d => d.ParentNode != null && !string.IsNullOrEmpty(d.ParentNode.Name) && d.ParentNode.Name.ToLower().Contains("page")).ToList();

                    if (xmlNodeLists != null && xmlNodeLists.Count > pageIndex)
                    {
                        XmlNode xmlNode = xmlNodeLists[pageIndex];
                        if (xmlNode != null)
                        {
                            if (!string.IsNullOrEmpty(xmlNode.InnerXml))
                            {
                                if (type == "Margins")
                                {
                                    string mleft = xmlNode.InnerXml.Split(',')[0];
                                    string mRight = xmlNode.InnerXml.Split(',')[2];
                                    if (!string.IsNullOrEmpty(mleft)) margeleft = Double.Parse(mleft);
                                    if (!string.IsNullOrEmpty(mRight)) margeRight = Double.Parse(mRight);
                                }
                                else if (type == "PageWidth") pageWidth = Double.Parse(xmlNode.InnerXml);
                                else if (type == "PageHeight") pageHeight = Double.Parse(xmlNode.InnerXml);

                            }

                            if (type == "Components") componentXmlNode = xmlNode;

                        }
                    }


                }
            }




           
     



        }

        public string ConvertFromRGBToColor(string[] rgbColor)
        {
            string R = "";
            string G = "";
            string B = "";

            try
            {
                if (rgbColor != null)
                {
                    R = rgbColor[0];
                    G = rgbColor[1];
                    B = rgbColor[2];
                }

                Color myColor = Color.FromArgb(Int32.Parse(R), Int32.Parse(B), Int32.Parse(G));
                string hex = myColor.R.ToString("X2") + myColor.G.ToString("X2") + myColor.B.ToString("X2");
                return "#" + hex;
            }

            catch(Exception ex)
            {
                return "";
            }
         
        }


        public HttpResponseMessage GetUsedSpaceForTenant(int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                if (authToken.Tenant == tenant)
                {
                    DocumentRepository documentRepository = new DocumentRepository(tenant);
                    double? usedSpace = documentRepository.GetUsedSpaceForTenant(tenant);

                    string result = GetUsedSpaceAndUnit((int?)usedSpace);
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
             else throw new Exception("Sorry you’re not authenticated to view system info.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private string GetUsedSpaceAndUnit(int? usedSpace)
        {
            double Byte = 1024;
            string FileSize = "";
            double usedSpaceInDouble;
            if (usedSpace == null)
            {
                usedSpace = 0;
            }

            double.TryParse(usedSpace.Value.ToString(), out usedSpaceInDouble);
            if (usedSpace < Byte)
            {
                FileSize = string.Format("{0:0.00}", usedSpaceInDouble) + " B";
            }
            else if (usedSpace >= Byte && usedSpace < Byte * Byte)
            {
                double result = usedSpaceInDouble / Byte;
                FileSize = string.Format("{0:0.00}", result) + " KB";
            }
            else if (usedSpace >= Byte * Byte && usedSpace < Byte * Byte * Byte)
            {
                double result = usedSpaceInDouble / (Byte * Byte);
                FileSize = string.Format("{0:0.00}", result) + " MB";
            }
            else if (usedSpace >= Byte * Byte * Byte && usedSpace < Byte * Byte * Byte * Byte)
            {
                double result = usedSpaceInDouble / (Byte * Byte * Byte);
                FileSize = string.Format("{0:0.00}", result) + " GB";
            }

            return FileSize;
        }

        private double   ConvertFromPointToPixel(double points)
        {
            points = points * 2;
            double pixels = points / 0.75;
            return pixels;
        }


    }
}