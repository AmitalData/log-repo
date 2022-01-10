using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Security;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.WebPages
{
    public partial class CorrespondenceDownloadpage : System.Web.UI.Page
    {
        int tenant;
        public byte[] _DatainByte;
     


        protected void Page_Load(object sender, EventArgs e)
        {
            var xml2html = false;
            try
            {
                string documentExtension = "";
                string filename = "";
                //tenant = 0;
                string AllHeaderRequest = Request["DA"];
                if (AllHeaderRequest == "1")
                {
                    string headerRequest = Request["securitykey"];
                    string[] filestrings = headerRequest.Split(':');
                    string securityId = filestrings[0].ToString();
                    string EntityId = filestrings[1].ToString();
                    string partnertype = filestrings[2].ToString();
                    string forwardingShipmentEntityId = null;
                    string domainName = "";
                    int tenant = int.Parse(filestrings[3] + "");
                    // cargo forwarding shipment
                    if (filestrings.Length >= 5)
                    {
                        forwardingShipmentEntityId = filestrings[4].ToString();
                    }

                    if (filestrings.Length == 6)
                    {
                        domainName = filestrings[5].ToString();
                    }


                    if (!string.IsNullOrEmpty(securityId) && !string.IsNullOrEmpty(EntityId))
                        DownloadAll(securityId, EntityId, tenant, partnertype, forwardingShipmentEntityId, domainName);
                    else if (!string.IsNullOrEmpty(securityId) && string.IsNullOrEmpty(EntityId))
                        DownloadAllBySecurityKey(securityId, tenant, partnertype, forwardingShipmentEntityId, domainName);

                }
                else
                {
                    string headerRequest = Request["id"];
                    string[] filestrings = headerRequest.Split('~');
                    string securityId = filestrings[0].ToString();
                    int tenant = Convert.ToInt32(filestrings[1]);

                    Uploader up = new Uploader();

                    Document myDoc = up.GetFileExtensionBySecurityId(securityId, tenant);
                    documentExtension = myDoc.Extension;
                    filename = filestrings[3] != null ? filestrings[3]: myDoc.FileName;

                    if (!string.IsNullOrEmpty(documentExtension))
                    {
                        _DatainByte = up.DownloadFile(myDoc.Id, documentExtension, "", myDoc.Tenant);

                        if (_DatainByte != null)
                        {
                            if (xml2html && documentExtension == "xml")
                            {
                                _DatainByte = TransformXml2Html(_DatainByte);
                                var suppressDownload = true;
                                if (suppressDownload)
                                {
                                    var xmlF = System.Text.UTF8Encoding.UTF8.GetString(_DatainByte);
                                    Response.Clear();
                                    Response.Write(xmlF);
                                    return;
                                }

                                documentExtension = "html";
                            }

                            string documentName = filename + "." + documentExtension;
                            // _DatainByte = sender as byte[];
                            HttpContext.Current.Response.Clear();
                            HttpContext.Current.Response.AddHeader("Content-Length", _DatainByte.Length.ToString());

                            switch (documentExtension)
                            {
                                case "pdf":
                                    HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;filename=" + documentName);
                                    HttpContext.Current.Response.ContentType = "application/" + "pdf";
                                    break;

                                case "doc":
                                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + documentName);
                                    HttpContext.Current.Response.ContentType = "application/" + "msword";
                                    break;

                                case "docx":
                                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + documentName);
                                    HttpContext.Current.Response.ContentType = "application/" + "vnd.openxmlformats-officedocument.wordprocessingml.document";
                                    break;

                                case "xls":
                                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + documentName);
                                    HttpContext.Current.Response.ContentType = "application/" + "vnd.ms-excel";
                                    break;

                                case "xlsx":
                                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + documentName);
                                    HttpContext.Current.Response.ContentType = "application/" + "vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                    break;

                                case "ppt":
                                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + documentName);
                                    HttpContext.Current.Response.ContentType = "application/" + "vnd.ms-powerpoint";
                                    break;

                                case "pptx":
                                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + documentName);
                                    HttpContext.Current.Response.ContentType = "application/" + "vnd.openxmlformats-officedocument.presentationml.presentation";
                                    break;

                                case "jpg":
                                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + documentName);
                                    HttpContext.Current.Response.ContentType = "image/jpeg";
                                    break;

                                case "zip":
                                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + documentName);
                                    HttpContext.Current.Response.ContentType = "application/zip";
                                    break;

                                case "xml":
                                    HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;filename=" + filename);
                                    HttpContext.Current.Response.ContentType = "application/xml";
                                    break;

                                case "html":
                                    HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;filename=" + documentName);
                                    HttpContext.Current.Response.ContentType = "application/html";
                                    break;
                                default:
                                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + documentName);
                                    HttpContext.Current.Response.ContentType = "application/octet-stream";
                                    break;

                            }

                            HttpContext.Current.Response.BinaryWrite(_DatainByte);

                            if (HttpContext.Current.Response.IsClientConnected)
                            {
                                HttpContext.Current.Response.Flush();
                                HttpContext.Current.Response.Close();
                                HttpContext.Current.ApplicationInstance.CompleteRequest();

                            }
                        }
                    }
                }
            }

            catch (Exception errorInfo)
            {
                string ErrorMessage = errorInfo.Message;
                if (!ErrorMessage.Contains("Sorry you’re not authenticated to view this document") && !ErrorMessage.Contains("Sorry, your download link has expired."))
                {
                    if (errorInfo.InnerException != null)
                    {
                        ErrorMessage += Environment.NewLine + errorInfo.InnerException.Message;
                    }
                    ErrorMessage += Environment.NewLine + errorInfo.ToString();
                    if (!string.IsNullOrEmpty(errorInfo.StackTrace))
                    {
                        ErrorMessage += Environment.NewLine + errorInfo.StackTrace;
                    }

                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "CorrespondenceDownloadPage : PageLoad Method", null);
                }

                if (ErrorMessage.Contains("Sorry you’re not authenticated to view this document"))
                {
                    HttpContext.Current.Response.Write("Sorry you’re not authenticated to view this document");
                }
             else  if (ErrorMessage.Contains("Sorry, your download link has expired."))
                {
                    HttpContext.Current.Response.Write("Sorry, your download link has expired.");
                }
                else
                {
                    HttpContext.Current.Response.Write("Invalid Document Security Id!");
                }

                if (HttpContext.Current.Response.IsClientConnected)
                {
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.Close();
                    HttpContext.Current.ApplicationInstance.CompleteRequest();

                }

            }

        }

        private byte[] TransformXml2Html(byte[] myByteArray)
        {
            // Open books.xml as an XPathDocument.
            using (var stream = new MemoryStream())
            {

                stream.Write(myByteArray, 0, myByteArray.Length);
                stream.Position = 0;
                XPathDocument doc = new XPathDocument(stream);

                using (var sw = new StringWriter())
                {
                    using (var xw = XmlWriter.Create(sw))
                    {
                        // Build Xml with xw.
                        XslCompiledTransform transform = new XslCompiledTransform();
                        XsltSettings settings = new XsltSettings();
                        settings.EnableScript = true;
                        transform.Load(new XmlTextReader(new StringReader(xsltXml2Html)), settings, null);

                        // Execute the transformation.
                        transform.Transform(doc, xw);

                    }
                    var xml = sw.ToString();

                    var byteArray = System.Text.UTF8Encoding.UTF8.GetBytes(xml);
                    return byteArray;
                }
            }
        }


        private void DownloadAll(string SecurityKey, string EntityId, int tenant, string partnerType, string forwardingShipmentEntityId, string domainName)
        {
            try
            {
                ShipmentRepository rep = new ShipmentRepository(tenant);                
                    Uploader up = new Uploader();
                Shipment shipment = rep.getSingleShipmentBySecurityIdAndId(EntityId,SecurityKey, tenant);
                string compressedFileName = "Documents";
                string shipmentNumber = null;
                if (domainName == "cargo") {
                    compressedFileName = $"{shipment.ShipmentNumber}_Documents";
                    shipmentNumber = shipment.ShipmentNumber;
                }
                if (shipment != null)
                {
                    List<DocumentsFilingPM> documents = up.GetDocumentByEntityAndTenant(EntityId, tenant);
                    if (!string.IsNullOrWhiteSpace(forwardingShipmentEntityId))
                    {
                        List<DocumentsFilingPM> customsForwardingShipmentdocuments = up.GetDocumentByEntityAndTenant(forwardingShipmentEntityId, tenant);
                        documents.AddRange(customsForwardingShipmentdocuments);
                    }

                    if (partnerType == "AG")
                    {
                        documents = documents.Where(d => d.IsAgentView).ToList();
                    }

                    else if (partnerType == "CS")
                    {
                        documents = documents.Where(d => d.IsCustomerView).ToList();
                    }
                    Dictionary<string, byte[]> CompressedArray = new Dictionary<string, byte[]>();
                    bool DocumentsExistance = false;
                    var ItemNum = 0;
                    foreach (DocumentsFilingPM document in documents)
                    {
                        document.CalculatedFileName = domainName == "cargo" ? shipmentNumber + '_' + document.DocumentTypeName : document.CalculatedFileName;
                        if (document.DirectionCode == "O" && document.DoucmentTypeTemplateFormatCode == "M")
                        {
                            continue;
                        }


                        else if (!string.IsNullOrEmpty(document.FileExtension))
                        {
                            DocumentsExistance = true;
                            string fileName = !string.IsNullOrEmpty(document.CalculatedFileName) ? document.CalculatedFileName : document.FileName;
                            fileName = fileName.Replace('/', ' ');
                            fileName += ("." + document.FileExtension);
                            
                            while (CompressedArray.ContainsKey(document.FileExtension + "@"+fileName))
                            {
                                ItemNum += 1;
                                fileName = !string.IsNullOrEmpty(document.CalculatedFileName) ? document.CalculatedFileName + " (" + ItemNum + ")" : document.FileName+" ("+ItemNum+")";
                                fileName = fileName.Replace('/', ' ');
                                fileName += ("." + document.FileExtension);
                            }
                            ItemNum = 0;
                            CompressedArray.Add(document.FileExtension + "@" + fileName, up.DownloadFile(document.DocumentId, document.FileExtension, "", tenant));
                        }

                    }
                    if (DocumentsExistance)
                    {

                        byte[] CompressedData = CompressionData("Documents", CompressedArray, false);
                        HttpContext.Current.Response.Clear();
                        HttpContext.Current.Response.AddHeader("Content-Length", CompressedData.Length.ToString());
                        HttpContext.Current.Response.AddHeader("Content-Disposition", $"attachment;filename={compressedFileName}.zip");
                        HttpContext.Current.Response.ContentType = "application/zip";
                        HttpContext.Current.Response.BinaryWrite(CompressedData);

                        if (HttpContext.Current.Response.IsClientConnected)
                        {
                            HttpContext.Current.Response.Flush();
                            HttpContext.Current.Response.Close();
                            HttpContext.Current.ApplicationInstance.CompleteRequest();

                        }
                    }
                }
                else
                {
                    HttpContext.Current.Response.Write("Invalid Document Security Id!");
                }



            }
            catch (Exception e)
            {

            }
        }
        private void DownloadAllBySecurityKey(string SecurityKey, int tenant, string partnerType, string forwardingShipmentEntityId, string domainName)
        {
            Shipment shipment = GetShipmentBySecurityKey(SecurityKey, tenant);

            DownloadAll(SecurityKey, shipment?.Id, tenant, partnerType, forwardingShipmentEntityId, domainName);
        }

        private static Shipment GetShipmentBySecurityKey(string SecurityKey, int tenant)
        {
            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
            Shipment shipment = shipmentRepository.getSingleShipmentBySecurityId(SecurityKey, tenant);
            return shipment;
        }

        public byte[] CompressionData(string listKey, Dictionary<string, byte[]> dataBackList, bool saveetodisk = false)
        {
            MemoryStream outputMemStream = new MemoryStream();
            ZipOutputStream zipStream = new ZipOutputStream(outputMemStream);

            zipStream.SetLevel(3);
            byte[] bytes = null;
            foreach (string key in dataBackList.Keys)
            {

                string fileName = key.Split('@')[1];
                var newEntry = new ZipEntry(fileName);
                newEntry.DateTime = DateTime.Now;

                zipStream.PutNextEntry(newEntry);

                bytes = dataBackList[key];

                MemoryStream inStream = new MemoryStream(bytes);
                long inStreamLength = inStream.Length;
                if (inStreamLength < 200)
                {
                    inStreamLength = 200;
                }

                StreamUtils.Copy(inStream, zipStream, new byte[inStreamLength]);
                inStream.Close();
                zipStream.CloseEntry();

            }

            zipStream.IsStreamOwner = false;
            zipStream.Close();
            outputMemStream.Position = 0;
            if (saveetodisk)
            {

                string appPath = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\ZipFiles\"; // <---
                if (Directory.Exists(appPath) == false)                                              // <---
                {                                                                                    // <---
                    Directory.CreateDirectory(appPath);                                              // <---
                }                                                                                    // <---

                appPath += listKey + ".zip";

                System.IO.File.WriteAllBytes(appPath, outputMemStream.ToArray());
            }
            return outputMemStream.ToArray();

        }


        private bool CheckAvailablityTenantsForEmail(string email, int tenant)
        {
            ContactRepository contactRep = new ContactRepository(tenant);
            return contactRep.CheckEmailAvailabilityForTenant(email, tenant);
        }

        private bool CheckSharedContactAuthenticationForShipment(string agentId, string customerId, int tenant)
        {
            if (tenant != 0)
            {

                bool exists = false;
                if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {//using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    //{
                    //}
                    ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
                    string email = HttpContext.Current.User.Identity.Name;

                    ContactRepository contactrep = new ContactRepository(commonDataContext);
                    Contact contact = contactrep.GetSingleContactByEmail(email, tenant);

                    if (contact != null)
                    {
                        CardContact cardContact = commonDataContext.CardContacts.Where(d => d.ContactId == contact.Id && (d.CardId == customerId || d.CardId == agentId)).FirstOrDefault();
                        if (cardContact != null)
                        {
                            exists = true;

                        }
                    }


                }
                if (!exists)
                {
                    throw new AutenticationException("Sorry! you are not authorized to read data!");
                }
                return exists;
            }
            return true;


        }


        const string xsltXml2Html =
           @"<?xml version=""1.0""?>
<xsl:stylesheet version=""1.0"" 
xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" 
xmlns:x1=""http://malam.com/customs/EAICommon.xsd"" 
xmlns:msxsl=""urn:schemas-microsoft-com:xslt""
xmlns:x2=""http://malam.com/customs/INF_MSG_Generic"">
  <xsl:output method=""html"" encoding=""windows-1255"" indent=""yes""/>
  <xsl:template match=""/"">
    <html>
        <head>
            <meta charset=""utf-8"" />
        </head>
      <body>
        <xsl:variable name=""tbls_"">
          <xsl:apply-templates/>
        </xsl:variable>
        <xsl:variable name=""tbls"" select=""$tbls_""/>
        <xsl:call-template name=""tbl"">
          <xsl:with-param name=""t"" select=""$tbls""/>
        </xsl:call-template>
      </body>
    </html>
  </xsl:template>
  <xsl:template name=""tbl"">
    <xsl:param name=""t""/>
    <xsl:if test=""th!=''"">
      <table border=""1"" bordercolor=""black"" cellpadding=""0"" cellspacing=""0"" width=""50%"">
        <th colspan=""2"" style=""color:red"">
          <xsl:value-of select=""th""/>
        </th>
        <xsl:for-each select=""tr"">
          <tr style=""color:black"">
            <xsl:for-each select=""td"">
              <td width=""50%"">
                <xsl:value-of select="".""/>
              </td>
            </xsl:for-each>
          </tr>
        </xsl:for-each>
      </table>
      <br/>
    </xsl:if>
    <xsl:for-each select=""msxsl:node-set($t)/table"">
      <xsl:if test=""name()='table'"">
        <xsl:call-template name=""tbl"">
          <xsl:with-param name=""t"" select="".""/>
        </xsl:call-template>
      </xsl:if>
    </xsl:for-each>
  </xsl:template>
  <xsl:template match=""*"">
    <xsl:choose>
      <xsl:when test=""count(child::*)=0"">
        <tr>
          <td>
            <xsl:value-of select=""local-name()""/>
          </td>
          <td>
            <xsl:value-of select=""text()""/>
            <xsl:value-of select=""'&#160;&#160;'""/>
          </td>
        </tr>
      </xsl:when>
      <xsl:otherwise>
        <table border=""1"" bordercolor=""black"" cellpadding=""0"" cellspacing=""0"">
          <!--<xsl:if test=""count(ancestor::*) &gt; 0"">-->
          <th colspan=""2"">
            <xsl:value-of select=""local-name()""/>
          </th>
          <!--</xsl:if>-->
          <xsl:apply-templates select=""*""/>
        </table>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet><!-- Stylus Studio meta-information - (c)1998-2002 eXcelon Corp.
<metaInformation>
<scenarios ><scenario default=""yes"" name=""Scenario1"" userelativepaths=""yes"" externalpreview=""no"" url=""data.xml"" htmlbaseurl="""" processortype=""msxml"" commandline="""" additionalpath="""" additionalclasspath="""" postprocessortype=""none"" postprocesscommandline="""" postprocessadditionalpath="""" postprocessgeneratedext=""""/></scenarios><MapperInfo srcSchemaPath="""" srcSchemaRoot="""" srcSchemaPathIsRelative=""yes"" srcSchemaInterpretAsXML=""no"" destSchemaPath="""" destSchemaRoot="""" destSchemaPathIsRelative=""yes"" destSchemaInterpretAsXML=""no""/>
</metaInformation>
-->";

    }
}