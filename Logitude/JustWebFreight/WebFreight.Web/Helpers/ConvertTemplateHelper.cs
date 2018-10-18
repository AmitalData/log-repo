
using HtmlAgilityPack;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using Telerik.Windows.Documents.FormatProviders.Html;
using Telerik.Windows.Documents.FormatProviders.Xaml;
using Telerik.Windows.Documents.Model;

namespace WebFreight.Web.Helpers
{
    public class ConvertTemplateHelper
    {
        public void ConvertContactSignatureXMLToHtml()
        {
            try
            {
                ContactRepository contactRepository = new ContactRepository(0);
                List<Contact> contactList = contactRepository.GetAllContactsThatHaveSignature().ToList();
                int count = 0;
                int total = contactList.Count;
                int loopCount = 0;
                foreach (Contact contact in contactList)
                {
                    loopCount += 1;

                    if (contact.Signature != null)
                    {
                        try
                        {
                            HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();
                            contact.SignatureHtml = htmlEditorHelper.ConvertXmlByteToHtmlByte(contact.Signature);
                            contactRepository.Update(contact);

                            count += 1;

                            if (count == 30 || total == loopCount)
                            {
                                contactRepository.SubmitChanges();
                                count = 0;
                           
                            }

                        }
                        catch (Exception ex)
                        {
                            string data = "Signature _________" + "ContactName : " + contact.EnglishName + " @ Id : " + contact.Id + "@ Tenant : " + contact.Tenant + "@ Exception : " + ex.Message;
                            AzureLog.SaveLogsInStorage(data, "P", DateTime.Now, "", "", 0, "", "ConvertContactSignatureXMLToHtml", null);


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string data = "Signature Convert _________ Exception : " + ex.Message;
                AzureLog.SaveLogsInStorage(data, "P", DateTime.Now, "", "", 0, "", "ConvertContactSignatureXMLToHtml", null);


            }

        }

        public void ConvertDocumentTypeTemplateXMLToHtml(string type)
        {

            try
            {
                TenantQuery tenantQuery = new TenantQuery(0);
                DocumentTypeTemplateRepository documentTypeTemplateRepository = new DocumentTypeTemplateRepository(0);
                List<TenantList> tenantList = new List<TenantList>();

                if (type == "All") tenantList = tenantQuery.GetTenantLists();
                else tenantList = tenantQuery.GetSomeTenant();

                foreach (TenantList tenant in tenantList)
                {
                    try
                    {
                        List<DocumentTypeTemplate> documentTypeTemplateList = documentTypeTemplateRepository.GetHtmDocumentTypeTemplates(tenant.Id).ToList();
                        int total = documentTypeTemplateList.ToList().Count;
                        int loopCount = 0;
                        int count = 0;
                        foreach (DocumentTypeTemplate documentTypeTemplate in documentTypeTemplateList)
                        {
                            loopCount += 1;

                            try
                            {
                                if (documentTypeTemplate.TemplateBody != null && documentTypeTemplate.TemplateTechnologyCode != "AG")
                                {
                                    HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();
                                    var template = htmlEditorHelper.UpdateHeaderAndFooterAndBodyHtml(documentTypeTemplate);
                                    documentTypeTemplateRepository.Update(template);
                                    count += 1;
                                }
                            }
                            catch (Exception ex)
                            {
                                string data = "Template _________" + "Name : " + documentTypeTemplate.Description + " @ Id : " + documentTypeTemplate.Id + "@ Tenant : " + documentTypeTemplate.Tenant + "@ Exception : " + ex.Message;
                                AzureLog.SaveLogsInStorage(data, "P", DateTime.Now, "", "", 0, "", "ConvertDocumentTypeTemplateXMLToHtml", null);
                            }

                            if ((count == 100 || total == loopCount) && count > 0)
                            {
                                documentTypeTemplateRepository.SubmitChanges();
                                count = 0; 
                            }

                        }

                    }

                    catch (Exception ex)
                    {
                        string data =  "Document Type Template Convert Tenant : (" + tenant.Id +  ") _________ Exception : " + ex.Message;
                        AzureLog.SaveLogsInStorage(data, "P", DateTime.Now, "", "", 0, "", "ConvertDocumentTypeTemplateXMLToHtml", null);

                    }


                }

            }


            catch (Exception ex)
            {
                string data = "Document Type Template Convert _________ Exception : " + ex.Message;
                AzureLog.SaveLogsInStorage(data, "P", DateTime.Now, "", "", 0, "", "ConvertDocumentTypeTemplateXMLToHtml", null);


            }

        }


        #region GetTableColor

        System.IO.StreamWriter file = null;
        private void WriteToFile(string message)
        {
            try
            {
                file = new System.IO.StreamWriter(@"C:\Users\Public\TestFolder\WriteLines2.txt" ,true);

                using (file)
                {
                    file.WriteLine(message);
                    file.Close();
                }
            }

            catch (Exception ex)
            {

            }
        }

        public void GetTableColor()
        {

            try
            {
                TenantQuery tenantQuery = new TenantQuery(0);
                DocumentTypeTemplateRepository documentTypeTemplateRepository = new DocumentTypeTemplateRepository(0);
                List<TenantList> tenantList = new List<TenantList>();

                tenantList = tenantQuery.GetTenantLists();

                foreach (TenantList tenant in tenantList)
                {
                    List<DocumentTypeTemplate> documentTypeTemplateList = documentTypeTemplateRepository.GetDocumentTypeTemplatesByTenant(tenant.Id).ToList();

                    foreach (DocumentTypeTemplate documentTypeTemplate in documentTypeTemplateList)
                    {
                        if (documentTypeTemplate.TemplateBody != null)
                        {
                            HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();
                            try
                            {
                               UpdateHeaderAndFooterAndBodyHtml(documentTypeTemplate);
                
                         
                            }
                            catch (Exception ex)
                            {
   

                            }
                        }

                    }
                }

            }


            catch (Exception ex)
            {



            }

        }

    
        public void UpdateHeaderAndFooterAndBodyHtml(DocumentTypeTemplate documentTypeTemplate)
        {

            XmlDataDocument messageDoc = new XmlDataDocument();
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            string xamlString = enc.GetString(documentTypeTemplate.TemplateBody);

            if (!string.IsNullOrEmpty(xamlString))
            {
                XmlReader reader = XmlReader.Create(new StringReader(xamlString));
                messageDoc = new XmlDataDocument();
                messageDoc.Load(reader);
                XmlDocument doc2 = (XmlDocument)messageDoc.Clone();

                XmlDataDocument xmlDataDocumentHeader = new XmlDataDocument();
                XmlDataDocument xmlDataDocumentFooter = new XmlDataDocument();

                XmlNodeList spansListHeader = messageDoc.GetElementsByTagName("t:Header.Body");
                XmlNodeList spansListFooter = messageDoc.GetElementsByTagName("t:Footer.Body");

                byte[] bytedata = null;
                //Header
                if (spansListHeader != null && spansListHeader.Count > 0)
                {
                    xmlDataDocumentHeader.LoadXml(spansListHeader[0].InnerXml);
                    bytedata = enc.GetBytes(xmlDataDocumentHeader.InnerXml);
                    ConvertXmalToHtml(bytedata, documentTypeTemplate);

                }

                //Footer
                if (spansListFooter != null && spansListFooter.Count > 0)
                {
                    xmlDataDocumentFooter.LoadXml(spansListFooter[0].InnerXml);
                    bytedata = enc.GetBytes(xmlDataDocumentFooter.InnerXml);
                    ConvertXmalToHtml(bytedata, documentTypeTemplate);
                }


                //Body
                bytedata = enc.GetBytes(messageDoc.InnerXml);
                ConvertXmalToHtml(bytedata, documentTypeTemplate);



            }



        }

        private string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }


        string frOriginalClass = "";
        bool HideBorderTable = true;
        string tableColor = "";
        private void BuildTableHtml(string htmlstring, DocumentTypeTemplate documentTypeTemplate)
        {

        
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlstring);

            foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//table"))
            {
                double widthtotal = 0;
                HideBorderTable = true;
                tableColor = "";
                foreach (HtmlNode row in table.SelectNodes("tr"))
                {
                    if(!string.IsNullOrEmpty(tableColor)) break;
                  
                    foreach (HtmlNode cell in row.SelectNodes("th|td"))
                    {
                        if (cell.Attributes["style"] != null)
                        {
                            var style = cell.Attributes["style"].Value;
                            if (!string.IsNullOrEmpty(style))
                            {
                                if (string.IsNullOrEmpty(tableColor)) GetColor(style, "border-left");
                                if (string.IsNullOrEmpty(tableColor)) GetColor(style, "border-right");
                                if (string.IsNullOrEmpty(tableColor)) GetColor(style, "border-top");
                                if (string.IsNullOrEmpty(tableColor)) GetColor(style, "border-bottom");
                            }
                        }
                    }
                }

               
                if (!string.IsNullOrEmpty(tableColor))
                {
                   
                 WriteToFile("Tenant : " + documentTypeTemplate.Tenant.ToString() + " @ " + "DocumentTypeCode : " + documentTypeTemplate.DocumentType.Code + " @ " + "Description : " + documentTypeTemplate.Description + " @ " + "Contact Id : " + documentTypeTemplate.LastUpdatedByUserId + " @ " + "TableColor : " + tableColor + " @ ");
                   
                }

            }
           
        }

        private void GetColor(string style , string propName)
        {
            string borderstyle = getBetween(style, propName +":", ";");

            if (!borderstyle.Contains("none") && !borderstyle.Replace(" ", "").Contains("0px"))
            {
                borderstyle += ";";
                string color = getBetween(borderstyle, "#", ";").ToLower();


                if (!string.IsNullOrEmpty(color) && color != "ffffff" && color != "000000" && color != "ff0000" && color != "ffff00" && color != "00b050" && color != "c00000" && color != "74a6e2" && color != "537db1" && color != "376092" && color != "4f81bd" && color != "1f497d" && color != "808080" && color != "9f2936")
                {
                   if(string.IsNullOrEmpty(tableColor)) tableColor = "#" + color;
                }
            }
        }

        private static string GetHtmlSrtingFromByte(Byte[] bytedata)
        {
            string html = "";

            XamlFormatProvider provider = new XamlFormatProvider();
            RadDocument document = provider.Import(bytedata);
            Telerik.Windows.Documents.FormatProviders.Html.HtmlFormatProvider htmlFormatProvider = new HtmlFormatProvider();
            Telerik.Windows.Documents.FormatProviders.Html.HtmlExportSettings htmlExportSettings = new HtmlExportSettings();
            htmlExportSettings.DocumentExportLevel = Telerik.Windows.Documents.FormatProviders.Html.DocumentExportLevel.Fragment;
            htmlExportSettings.StylesExportMode = Telerik.Windows.Documents.FormatProviders.Html.StylesExportMode.Inline;
            htmlExportSettings.StyleRepositoryExportMode = StyleRepositoryExportMode.DontExportStyles;
            htmlExportSettings.ExportFontStylesAsTags = true;
            htmlExportSettings.DocumentExportLevel = DocumentExportLevel.Fragment;
            htmlExportSettings.ImageExportMode = Telerik.Windows.Documents.FormatProviders.Html.ImageExportMode.Base64Encoded;

            htmlFormatProvider.ExportSettings = htmlExportSettings;

            html = htmlFormatProvider.Export(document);
            return html;
        }

        public void ConvertXmalToHtml(Byte[] bytedata , DocumentTypeTemplate documentTypeTemplate)
        {
            byte[] result = null;
            string html = "";

            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();


            if (bytedata != null)
            {
                html = GetHtmlSrtingFromByte(bytedata);

                if (!string.IsNullOrEmpty(html))
                {
                    if (html.Contains("table"))
                    {
                        BuildTableHtml(html , documentTypeTemplate);
               

                    }
                }
            }

     

        }

        #endregion

    }
}