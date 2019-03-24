using System;
using System.Linq;
using System.Transactions;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.WebServices;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using WebFreight.Web.WcfApi;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Xml.XPath;
using System.Xml;
using System.Xml.Xsl;
using WebFreight.Web.Security;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.SystemLogs;
using Simplog.Server.Infrastructure;
using Simplog.Data.CommonDataModel;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.WebPages
{
    public partial class Downloadpage : System.Web.UI.Page
    {
        public byte[] _DatainByte;

        public bool CheckAvailablityTenantsForEmail(string email, int tenant)
        {
            UserRepository userRep = new UserRepository(0);
            Simplog.Data.CommonDataModel.EntityPOCOs.User user = userRep.GetSingleUserByEmail(email, 0, false);

            bool available = true;
            if (user != null)
            {
                TenantManagementRepository tenantManagementRep = new TenantManagementRepository();
                bool isDistributorToCurrentTenant = tenantManagementRep.CheckDistributor(user.DistributorCode, tenant);
                if (user.IsDistributor)
                {
                    if (isDistributorToCurrentTenant)
                    {
                        available = true;
                    }
                    else
                    {
                        available = false;
                    }
                }
                else
                {
                    available = true;
                }
            }
            else
            {
                ContactRepository contactRep = new ContactRepository(tenant);
                available = contactRep.CheckEmailAvailabilityForTenant(email, tenant);
            }
            return available;
        }


        int? tenant = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            var xml2html = false;
            try
            {
                string documentOutCopyId = null;

                string documentExtension = null;
                string filename = null;

                string securityKey = Request["securityId"] ?? "";
                string token = Request["tempId"] ?? "";
                string securityId = "";
                string CustomName = "";

                SecurityDocumentResult securityDocumentResult = SecurityDocumentHelper.ValidationDocumentToken(token);
                bool isValid = securityDocumentResult.IsValid;
                string email = securityDocumentResult.Email;
                string exceptionMessage = securityDocumentResult.ExceptionResult;
                tenant = securityDocumentResult.Tenant;


                bool overrideSecDueIsConnectedToUniFreight = false;

                if (isValid)
                {
                    if (LogitudeSettings.IsCostomsDeploy)
                    {
                        var setting = CustomsSettingQueryService.GetSettingByTenant((int)tenant) ?? new CustomsSettingPM();
                        overrideSecDueIsConnectedToUniFreight = setting.IsConnectedToUniFreight;
                        if (!overrideSecDueIsConnectedToUniFreight)//semi a like Connected  == not cloud !!
                        {
                            if (!String.IsNullOrWhiteSpace(setting.OnPremiseFillingService)) overrideSecDueIsConnectedToUniFreight = true;
                        }
                    }

                    if (overrideSecDueIsConnectedToUniFreight || CheckAvailablityTenantsForEmail(email, (int)tenant) || tenant == 0)
                    {
                        if (!overrideSecDueIsConnectedToUniFreight && !IsUser(email, (int)tenant)) isValid = false;
                    }
                    else isValid = false;
                }

                if (isValid)
                {
                    #region securityKey
                    if (!string.IsNullOrEmpty(securityKey))
                    {
                        Uploader up = new Uploader();

                        var securityArray = securityKey.Split('~');
                        if (securityArray != null)
                        {
                            if (securityArray.Length > 0) securityId = securityArray[0];
                            if (securityArray.Length > 1) filename = documentOutCopyId = securityArray[1];

                            Document document = up.GetFileExtensionBySecurityIdAndCopyId(securityId, filename, (int)tenant);
                            if (document != null)
                            {
                                documentExtension = document.Extension;
                                CustomName = document.CalculatedFileName;
                                filename = document.Id;
                            }
                            else isValid = false;
                        }
                    }
                    #endregion

                    string[] filestrings = null;
                    string entityName = null;

                    string headerRequest = Request["id"] ?? "";

                    #region CustomName

                    if (headerRequest.Contains("*"))
                    {
                        var Data = headerRequest.Split('*');
                        CustomName = Data[1];
                        headerRequest = Data[0];
                        CustomName = CustomName.Replace('-', '_');
                    }
                    #endregion


                    if (string.IsNullOrEmpty(documentOutCopyId))
                    {
                        documentOutCopyId = Request["documentOutCopyId"] ?? "";
                    }

                    #region ShowReceivedCustomResponse

                    if (!String.IsNullOrWhiteSpace(Request["ShowReceivedCustomResponse"]))
                    {

                        //ShowFormatedResponseAction
                        var list = new List<string>();
                        //http://localhost:62619//WebPages/Downloadpage.aspx?id=1_1-20998_xml2html
                        list.Remove("xml2html");
                        string mRequestComminicationId = Request["RequestComminicationId"];
                        string mTenant = Request["Tenant"];
                        var mDocumentId = GetReceivedCustomResponseCorrelationDocumentId(mRequestComminicationId, mTenant);
                        if (String.IsNullOrWhiteSpace(mDocumentId))
                        {
                            Response.Write("Bad Params");
                            return;
                        }
                        xml2html = true;
                        list.Clear(); list.Add(mDocumentId);
                        filestrings = list.ToArray();
                    }

                    #endregion
                    else
                    {
                        filestrings = headerRequest.Split('_');
                    }



                    if (string.IsNullOrEmpty(securityKey))
                    {
                        filename = filestrings[0].ToString();
                    }

                    if (filestrings.Length > 1) entityName = filestrings[1].ToString();


                    if (headerRequest.Contains("QuestionnaireAnswers"))
                    {
                        if (filestrings.Length > 3) entityName = filestrings[3].ToString();

                    }
                    string documentId = filename;
                    if (!string.IsNullOrEmpty(documentId))
                    {
                        documentId = documentId.Split('.')[0].ToString();
                    }


                    if (entityName == "analyzeQueue")
                    {
                        AnalyzeQueueRepository analyzeQueueRep = new AnalyzeQueueRepository();
                        AnalyzeQueue analyzeQueue = analyzeQueueRep.GetSingleAnalyzeQueue(filename, (int)tenant);
                        _DatainByte = analyzeQueue.MessageBody;
                        documentExtension = "xml";

                    }
                    else
                        if (entityName == "QuestionnaireAnswers")
                    {
                        //Convert.ToInt32(filestrings[1])
                        CustomerWcfService customerWcf = new CustomerWcfService();
                        string html = customerWcf.GetActivationQuestionnaireAnswers(filestrings[0].ToString(), (int)tenant, filestrings[1].ToString(), filestrings[2].ToString(), filestrings[4].ToString());

                        _DatainByte = Encoding.UTF8.GetBytes(html);
                        documentExtension = "html";
                    }

                    else
                    {
                        Uploader up = new Uploader();

                        if (filestrings.Count() > 1)
                        {
                            documentExtension = "pdf";
                            filename += ".pdf";
                            string containername = filestrings[1].ToString();
                            _DatainByte = up.DownloadStaticFile(filename, containername);

                        }
                        else
                        {
                            bool isTenantZero = (int)tenant == 0 ? true : false;
                            if (string.IsNullOrEmpty(securityKey))
                            {
                                documentExtension = up.GetFileExtension(documentId, (int)tenant, isTenantZero);

                                if (!string.IsNullOrEmpty(documentExtension))
                                {
                                    _DatainByte = up.DownloadFile(filename, documentExtension, "", (int)tenant, isTenantZero);
                                }
                                else isValid = false;

                            }
                            else
                            {

                                if (!string.IsNullOrEmpty(documentExtension) && !string.IsNullOrEmpty(filename))
                                {
                                    _DatainByte = up.DownloadFile(filename, documentExtension, "", (int)tenant, isTenantZero);
                                }
                                else isValid = false;
                            }
                        }
                    }


                }


                if (isValid)
                {
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
                        string documentName = (!string.IsNullOrEmpty(CustomName) ? CustomName : filename) + "." + documentExtension;

                        if (!string.IsNullOrEmpty(documentName)) documentName = documentName.Replace(" ", "");

                        // _DatainByte = sender as byte[];
                        HttpContext.Current.Response.Clear();
                        HttpContext.Current.Response.AddHeader("Content-Length", _DatainByte.Length.ToString());
                        //HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + DocumentName);

                        // Get content type
                        // FileExtension = filename.Split('.')[1];

                        var browser = HttpContext.Current.Request.Browser;
                        //Page.Title = "Abed";
                        string ShowType = "attachment";
                        switch (documentExtension)
                        {
                            case "pdf":
                                // HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;filename=" + documentName);
                                HttpContext.Current.Response.ContentType = "application/" + "pdf";
                                ShowType = "inline";
                                break;

                            case "doc":
                                // HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + documentName);
                                HttpContext.Current.Response.ContentType = "application/" + "msword";
                                break;

                            case "docx":
                                //HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + documentName);
                                HttpContext.Current.Response.ContentType = "application/" + "vnd.openxmlformats-officedocument.wordprocessingml.document";
                                break;

                            case "xls":
                                //HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + documentName);
                                HttpContext.Current.Response.ContentType = "application/" + "vnd.ms-excel";
                                break;

                            case "xlsx":
                                //HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + documentName);
                                HttpContext.Current.Response.ContentType = "application/" + "vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                break;

                            case "ppt":
                                // HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + documentName);
                                HttpContext.Current.Response.ContentType = "application/" + "vnd.ms-powerpoint";
                                break;

                            case "pptx":
                                // HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + documentName);
                                HttpContext.Current.Response.ContentType = "application/" + "vnd.openxmlformats-officedocument.presentationml.presentation";
                                break;

                            case "jpg":
                                //HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + documentName);
                                HttpContext.Current.Response.ContentType = "image/jpeg";
                                break;

                            case "zip":
                                // HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + documentName);
                                HttpContext.Current.Response.ContentType = "application/zip";
                                break;

                            case "xml":
                                //HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;filename=" + filename);
                                HttpContext.Current.Response.ContentType = "application/xml";
                                ShowType = "inline";
                                break;

                            case "html":
                                //HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;filename=" + documentName);
                                HttpContext.Current.Response.ContentType = "application/html";
                                ShowType = "inline";
                                break;
                            default:
                                // HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + documentName);
                                HttpContext.Current.Response.ContentType = "application/octet-stream";
                                break;

                        }


                        if (browser != null && browser.Browser.Equals("ie", StringComparison.OrdinalIgnoreCase))
                        {

                            HttpContext.Current.Response.AppendHeader("Content-Disposition", ShowType + "; filename*=UTF-8''" + HttpUtility.UrlPathEncode(documentName) + "\"");
                        }
                        else
                        {
                            HttpContext.Current.Response.AppendHeader("Content-Disposition", ShowType + "; filename=\"" + HttpUtility.UrlPathEncode(documentName) + "\"");
                        }

                        if (!string.IsNullOrEmpty(documentOutCopyId))
                        {
                            UserRepository userRep = new UserRepository((int)tenant);
                            User printedBy = null;
                            printedBy = userRep.GetSingleUserByEmail(email, (int)tenant, false);

                            DocumentOutCopyRepository myRep = new DocumentOutCopyRepository((int)tenant);
                            DocumentOutCopy documentoutCopy = myRep.GetSingleDocumentOutCopyByTenant(documentOutCopyId, (int)tenant);
                            documentoutCopy.LastPrintDate = TenantServerConfigration.GetCurrentDateTime((int)tenant);
                            documentoutCopy.LastPrintedByUserId = printedBy != null ? printedBy.Id : "";
                            myRep.Update(documentoutCopy);
                            myRep.SubmitChanges();
                        }

                        HttpContext.Current.Response.BinaryWrite(_DatainByte);

                        if (HttpContext.Current.Response.IsClientConnected)
                        {
                            HttpContext.Current.Response.Flush();
                            HttpContext.Current.Response.Close();
                            HttpContext.Current.ApplicationInstance.CompleteRequest();

                        }

                    }

                    else
                    {
                        Response.Output.Write("Document file is empty.");
                        // throw new ApplicationException("Document file is empty.");
                    }
                }
                else
                {
                    var message = exceptionMessage;
                    if (string.IsNullOrEmpty(exceptionMessage)) message = "Sorry you’re not authenticated to view this document.";
                    Response.Output.Write(message);
                    //  throw new ApplicationException(message);
                }

            }
            catch (ExceptionInErrorLog ExceptionInErrorLog)
            {
                Response.Clear();
                Response.Output.Write(
//                    String.Format(
//@"An unhandled exception has been caught (our ref :{0})  
//{1}",ExceptionInErrorLog.ErrorlogId, ExceptionInErrorLog.Message)
ExceptionInErrorLog.ToString()
    );
            }
            catch (Exception errorInfo)
            {
                string ErrorMessage = errorInfo.Message;
                if (!ErrorMessage.Contains("Sorry you’re not authenticated to view this document") && !ErrorMessage.Contains("Sorry, your download link has expired.") && !ErrorMessage.Contains("Document file is empty."))
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
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, (int)tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "DownloadPage : PageLoad Method", null);
                }
                //AzureLog.SaveLogsInStorage(ErrorMessage, "E", 0, User.Identity.Name, User.Identity.Name);
            }

        }

        private static bool IsUser(string email, int tenant)
        {
            bool isUser = false;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                IGlobalContext globalContext = GlobalContext.GetContext();
                isUser = globalContext.GlobalContacts.Where(c => c.Email == email && (c.GlobalTenantId == tenant || c.GlobalTenantId == 0) && c.IsUser == true).Any();
            }
            return isUser;
        }

        private string GetReceivedCustomResponseCorrelationDocumentId(string requestComminicationId, string mtenant)
        {

            int tenant = 0;
            if (!int.TryParse(mtenant, out tenant))
            {
                return null;
            }
            SecurityUtility.AuthenticationOnTenant(tenant);

            var communicationLogStepQuery = new CommunicationLogStepQuery(tenant);
            var myCommunicationLogStepList = communicationLogStepQuery.GetCommunicationLogStepListsByLogId(requestComminicationId, tenant);
            if (myCommunicationLogStepList == null)
            {
                return null;
            }
            var receivedCustomResponseCorrelationStep = myCommunicationLogStepList
                    .FirstOrDefault(rec => rec.Name == "ReceivedCustomResponseCorrelation");
            if (receivedCustomResponseCorrelationStep == null)
            {
                return null;
            }
            return receivedCustomResponseCorrelationStep.DocumentId;
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
                    //var inx = xml.IndexOf("<html", StringComparison.CurrentCulture);
                    //if (inx > 0)
                    //{
                    //    inx += 5;
                    //    //xml = string.Concat(@"<html encoding=""windows-1255""  ", xml.Substring(inx));
                    //    xml = string.Concat(@"<html encoding=""uft-8""  ", xml.Substring(inx));
                    //}
                    var byteArray = System.Text.UTF8Encoding.UTF8.GetBytes(xml);
                    return byteArray;
                }
            }
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
