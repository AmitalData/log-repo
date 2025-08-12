using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.SystemLogs;
using NPOI.SS.Formula.Functions;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; 
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;
using WebFreight.Web.Controllers.DigitalPortal.Helpers;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class CorrespondenceDownloadController : ApiController
    {
        int _Tenant;
        HttpResponseMessage _response = null;

        [HttpGet]
        [Route("CorrespondenceDownload/DownloadDocument")]

        public HttpResponseMessage DownloadDocumentLimited(string DA = "", string securitykey = "", string id = "")
        { 
            return DownloadDocumentInner(DA, securitykey, id, true);
        }

        private HttpResponseMessage DownloadDocumentInner(string DA = "", string securitykey = "" , string id = "", bool limitedDateRange = true)
        {
            
            byte[] _DatainByte;
            bool xml2html = false;
            try
            {
                string documentExtension = "";
                string filename = "";

                string AllHeaderRequest = DA;
                if (AllHeaderRequest == "1")
                {
                    string headerRequest = securitykey;
                    string[] filestrings = headerRequest.Split(':');
                    string securityId = filestrings[0].ToString();
                    string EntityId = filestrings[1].ToString();
                    string partnertype = filestrings[2].ToString();
                    string forwardingShipmentEntityId = null;
                    string domainName = "";

                    int tenant = int.Parse(filestrings[3] + "");
                    _Tenant = tenant;
                    // cargo forwarding shipment
                    if (filestrings.Length >= 5 && filestrings[4].ToString() == "DigitalPortal")
                    {
                        domainName = filestrings[4].ToString();
                    }
                    else if (filestrings.Length >= 5 && filestrings[4].ToString() != "DigitalPortal")
                    {
                        forwardingShipmentEntityId = filestrings[4].ToString();
                    }

                    if (filestrings.Length == 6)
                    {
                        domainName = filestrings[5].ToString();
                    }


                    if (!string.IsNullOrEmpty(securityId) && !string.IsNullOrEmpty(EntityId))
                        return DownloadAll(securityId, EntityId, tenant, partnertype, forwardingShipmentEntityId, domainName, limitedDateRange);
                    else if (!string.IsNullOrEmpty(securityId) && string.IsNullOrEmpty(EntityId))
                        return DownloadAllBySecurityKey(securityId, tenant, partnertype, forwardingShipmentEntityId, domainName, limitedDateRange);

                }
                else
                {
                    string headerRequest = id;
                    string[] filestrings = headerRequest.Split('~');
                    string securityId = filestrings[0].ToString();
                    int tenant = Convert.ToInt32(filestrings[1]);
                    string copyId = filestrings.Length > 2 ? filestrings[2].ToString() : null;

                    bool isDigitalPortal = false;
                    if (filestrings.Length > 3)
                    {
                        Boolean.TryParse(filestrings[3].ToString(), out isDigitalPortal);
                    }

                    Uploader up = new Uploader();

                    Document myDoc = up.GetFileExtensionBySecurityIdAndCopyId(securityId, copyId, tenant);
                    if ((string.IsNullOrEmpty(copyId) || copyId == "null") && myDoc == null)
                    {
                        myDoc = up.GetFileExtensionBySecurityId(securityId, tenant);
                    }
                    if (limitedDateRange)
                    {
                        DateTime dateTime = new DateTime(2022, 01, 01);
                        //DateTime dateTime = DateTime.Now.AddDays(-183); // half a year 
                        if (myDoc?.CreateDate != null && myDoc?.CreateDate.Value.Date < dateTime.Date)
                        {
                            throw new Exception("The document is not allowed.");
                        }
                    }
                    documentExtension = myDoc.Extension;
                    filename = myDoc.FileName;
                    var documentType = string.Empty;

                    if (isDigitalPortal)
                    {
                        var query = new DocumentsFilingQuery(tenant);
                        var documentsFilingPM = query.GetDocumentsFilingByDocumentId(myDoc.Id, tenant);

                        if (documentsFilingPM != null)
                        {
                            documentType = documentsFilingPM.DocumentTypeName;
                        }
                    }

                    if (filestrings.Length >= 4 && !isDigitalPortal)
                    {
                        filename = filestrings[3] != null ? filestrings[3] : filename;
                    }

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
                                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, new StringContent(xmlF, System.Text.Encoding.UTF8, "text/xml"));

                                }

                                documentExtension = "html";
                            }

                            string documentName = $"{filename}.{documentExtension}";

                            if (isDigitalPortal)
                            {
                                var digitalFileName = !string.IsNullOrEmpty(myDoc.CalculatedFileName) ? myDoc.CalculatedFileName : myDoc.FileName;
                                documentName = $"{documentType} - {digitalFileName}.{documentExtension}";
                            }



                            _response = Request.CreateResponse(HttpStatusCode.OK);
                            _response.Content = new StreamContent(new MemoryStream(_DatainByte));
                            _response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                            _response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                            _response.Content.Headers.ContentDisposition.FileName = documentName;
                            _response.Content.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");



                        }
                        else
                        {
                             string documentName = $"{filename}.{documentExtension}";
                             _response = Request.CreateResponse(HttpStatusCode.OK);
                            _response.Content.Headers.ContentLength = _DatainByte.LongLength;
                            _response.Content = new StreamContent(new MemoryStream(_DatainByte));
                             _response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                             _response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                             _response.Content.Headers.ContentDisposition.FileName = documentName;
                          


                        }

                    }
                    else
                    {
                        string documentName = $"{filename}.{documentExtension}";
                        _response = Request.CreateResponse(HttpStatusCode.OK);
                        _response.Content.Headers.ContentLength = 0;
                        _response.Content = new StreamContent(new MemoryStream());
                        _response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        _response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        _response.Content.Headers.ContentDisposition.FileName = documentName;
                    }

                } 
                return _response;
            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception errorInfo)
            {
                string ErrorMessage = errorInfo.Message;
                if (!ErrorMessage.Contains("Sorry you’re not authenticated to view this document") && !ErrorMessage.Contains("Sorry, your download link has expired.") && !ErrorMessage.Contains("The document is not allowed."))
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

                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, _Tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "CorrespondenceDownloadPage : PageLoad Method", null);
                }

                if (ErrorMessage.Contains("Sorry you’re not authenticated to view this document"))
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, new StringContent("Sorry you’re not authenticated to view this document", System.Text.Encoding.UTF8, "text/plain") );
                }
                else if (ErrorMessage.Contains("Sorry, your download link has expired."))
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, new StringContent("Sorry, your download link has expired.", System.Text.Encoding.UTF8, "text/plain") );
                }
                else if (ErrorMessage.Contains("The document is not allowed."))
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, new StringContent("The document is not allowed.", System.Text.Encoding.UTF8, "text/plain") );
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, new StringContent("Invalid Document Security Id!", System.Text.Encoding.UTF8, "text/plain") );
                }



            }


        }

        private static HttpResponseMessage GetResponse(string responseDataDocumentId, XElement myXml)
        {
            HttpResponseMessage httpResponse;
            var data = System.Text.UTF8Encoding.UTF8.GetBytes(myXml.ToString());

            httpResponse = new HttpResponseMessage(HttpStatusCode.OK);

            httpResponse.Content = new StreamContent(new MemoryStream(data));
            httpResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            httpResponse.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            httpResponse.Content.Headers.ContentDisposition.FileName = responseDataDocumentId + ".xml";
            return httpResponse;
        }

        [HttpGet]
        [Route("CorrespondenceDownload/ValidateAndDownloadDocument")]
        public HttpResponseMessage ValidateAndDownloadDocument(string DA = "", string securitykey = "", string id = "")
        {
            try
            {


                if (!HttpContext.Current.Request.IsAuthenticated)
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteError("Not Authenticated DA= " + DA + ", securitykey= " + securitykey + ", id= " + id);

                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Redirect);
                    response.Headers.Location = new Uri("../login", UriKind.Relative);
                    return response;

                }
                else
                {
                    try
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        int authTenant = authToken.Tenant;
                        int tenant = authTenant;

                        string AllHeaderRequest = DA;
                        if (AllHeaderRequest == "1")
                        {
                            string headerRequest = securitykey;
                            string[] filestrings = headerRequest.Split(':');
                            string securityId = filestrings[0].ToString();
                            string EntityId = filestrings[1].ToString();
                            string partnertype = filestrings[2].ToString();
                            string forwardingShipmentEntityId = null;
                            string domainName = "";

                            tenant = int.Parse(filestrings[3] + "");


                        }
                        else
                        {
                            string headerRequest = id;
                            string[] filestrings = headerRequest.Split('~');
                            string securityId = filestrings[0].ToString();
                            tenant = Convert.ToInt32(filestrings[1]);
                        }
                        if (authTenant != tenant)
                        {
                            NetCommonHelper.Logger.DevLog.Instance.WriteError("Authenticated Tenant " + authTenant.ToString() + ", Document link tenant " + tenant.ToString() + ", DA= " + DA + ", securitykey= " + securitykey + ", id= " + id);
                            return Request.CreateResponse(HttpStatusCode.Unauthorized, new StringContent("Invalid Security Id!", System.Text.Encoding.UTF8, "text/plain"));
                        }

                        return DownloadDocumentInner(DA, securitykey, id, false);

                    }
                    catch (Exception ex)
                    {
                        NetCommonHelper.Logger.DevLog.Instance.WriteError("ValidateAndDownloadDocument (1)  Exception= " + ex.ToString());

                        return Request.CreateResponse(HttpStatusCode.Unauthorized, ApiExceptionBuilder.BuildException(ex)); ;
                    }


                }
            }
            catch (Exception errorInfo)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError("ValidateAndDownloadDocument (2)  Exception= " + errorInfo.ToString());
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Redirect);
                response.Headers.Location = new Uri("../login", UriKind.Relative);
                return response;
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


        private HttpResponseMessage DownloadAll(string SecurityKey, string EntityId, int tenant, string partnerType, string forwardingShipmentEntityId, string domainName, bool limitedDateRange)
        {
            HttpResponseMessage response = null;
            try
            {
                ShipmentRepository rep = new ShipmentRepository(tenant);
                Uploader up = new Uploader();
                Shipment shipment = rep.getSingleShipmentBySecurityIdAndId(EntityId, SecurityKey, tenant);
                string compressedFileName = "Documents";
                string shipmentNumber = null;

                

                if (shipment != null)
                {
                    if (domainName == "cargo" || domainName == "DigitalPortal")
                    {
                        compressedFileName = $"{shipment.ShipmentNumber}_Documents";
                        shipmentNumber = shipment.ShipmentNumber;
                    }

                    var documents = up.GetDocumentByEntityAndTenant(EntityId, tenant);

                    if (!string.IsNullOrWhiteSpace(forwardingShipmentEntityId))
                    {
                        var customsForwardingShipmentdocuments = up.GetDocumentByEntityAndTenant(forwardingShipmentEntityId, tenant);
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

                    if (limitedDateRange)
                    {
                        DateTime dateTime = DateTime.Now.AddDays(-183); // half a year //was: new DateTime(2022, 01, 01);
                        documents = documents.Where(d => d.CreateDate.Date >= dateTime.Date).ToList();
                    }

                    var CompressedArray = new Dictionary<string, byte[]>();
                    bool DocumentsExistance = false;
                    var ItemNum = 0;
                    var documentsListWithoutDuplications = new List<DocumentsFilingPM>();

                    foreach (DocumentsFilingPM document in documents)
                    {
                        if (domainName == "cargo"
                            && documentsListWithoutDuplications.Any(x => x.DocumentTypeCode == document.DocumentTypeCode
                                                                         && x.CalculatedFileName == document.CalculatedFileName
                                                                         && x.FileSize == document.FileSize))
                        {
                            continue;
                        }

                        documentsListWithoutDuplications.Add(new DocumentsFilingPM
                        {
                            CalculatedFileName = document.CalculatedFileName,
                            DocumentTypeCode = document.DocumentTypeCode,
                            FileSize = document.FileSize,
                        });

                        var digitalFileName = !string.IsNullOrEmpty(document.CalculatedFileName) ? document.CalculatedFileName : document.FileName;

                        document.CalculatedFileName = domainName == "cargo"
                                                      ? shipmentNumber + '_' + document.DocumentTypeName
                                                      : domainName == "DigitalPortal"
                                                        ? $"{document.DocumentTypeName} - {digitalFileName}"
                                                        : document.CalculatedFileName;

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

                            while (CompressedArray.ContainsKey(document.FileExtension + "@" + fileName))
                            {
                                ItemNum += 1;
                                fileName = !string.IsNullOrEmpty(document.CalculatedFileName)
                                           ? document.CalculatedFileName + " (" + ItemNum + ")"
                                           : document.FileName + " (" + ItemNum + ")";
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
                        response = Request.CreateResponse(HttpStatusCode.OK);
                        response.Content.Headers.ContentLength = CompressedData.LongLength;
                        response.Content = new StreamContent(new MemoryStream(CompressedData));
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/zip");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = compressedFileName + ".zip";
                        return response;

                    }
                    else 
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, new StringContent("No Documents Found", System.Text.Encoding.UTF8, "text/plain"));
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, new StringContent("Invalid Document Security Id!", System.Text.Encoding.UTF8, "text/plain"));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, ApiExceptionBuilder.BuildException(ex));
            }
        }
        private HttpResponseMessage DownloadAllBySecurityKey(string SecurityKey, int tenant, string partnerType, string forwardingShipmentEntityId, string domainName, bool limitedDateRange)
        {
            Shipment shipment = GetShipmentBySecurityKey(SecurityKey, tenant);

            return DownloadAll(SecurityKey, shipment?.Id, tenant, partnerType, forwardingShipmentEntityId, domainName, limitedDateRange);
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
                {
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