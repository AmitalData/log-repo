using System;
using System.Web.Services;
using WebFreight.Web.Helpers;


namespace WebFreight.Web.WebServices
{
    /// <summary>
    /// Summary description for HtmlEditorWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.

 
    // [System.Web.Script.Services.ScriptService]
         

    public class HtmlEditorWebService : System.Web.Services.WebService
    {
        HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();

        [WebMethod]
        public byte[] GetDocumentHtmlTemplate(string docOutId, string entityId, string objectTableId, string childEntityId, string childEntityObjectTableId, int tenant, string userId, bool theIsSendMail, string documentTemplateId, ref string subject, ref string from, ref string replyTo) 
        {
            return htmlEditorHelper.GetEditorXamlData(docOutId, entityId, objectTableId, childEntityId, childEntityObjectTableId, tenant, userId, theIsSendMail, documentTemplateId, ref subject, ref from, ref replyTo);
        }

        [WebMethod]
        public byte[] GetDocumenTemplateByteHtml(string docOutId, string entityId, string objectTableId, string childEntityId, string childEntityObjectTableId, int tenant, string userId, bool theIsSendMail, string documentTemplateId, ref string subject) 
        {
             string from="";
             string replyTo="";
             byte[] ByteHtml = htmlEditorHelper.GetEditorXamlData(docOutId, entityId, objectTableId, childEntityId, childEntityObjectTableId, tenant, userId, theIsSendMail, documentTemplateId, ref subject, ref  from, ref replyTo);
            return ByteHtml;
        }


        [WebMethod]
        public byte[] GetSentMessageHtmlBody(string documentId, int tenant)
        {
            return htmlEditorHelper.GetSentMessageHtmlBody(documentId, tenant);
        }

   
        [WebMethod]
        public string SendHtmlDocument(string documentTypeId, byte[] htmlData, string internalDocumentId, string externalDocumentId, int tenant, string toEmail, string subject, string cc, string bcc, string userId, string entityId, string objectTableId, string attachments, string entityReference , string from , string replyTo)
        {

            return htmlEditorHelper.SendHtmlDocument(htmlData, internalDocumentId, externalDocumentId, tenant, toEmail, subject, cc, bcc, userId, entityId, objectTableId, attachments, entityReference, from, replyTo);

        }


        [WebMethod]
        public string SendEmailOutActivityForEntity(byte[] htmlData, byte[] textData, int tenant, string toEmail, string subject, string cc, string bcc, string userId, string myEntityId, string customerId, string objectTableId, string attachments, string entityReference, string documentTypeCode, string eventTypeCode)
        {
            return htmlEditorHelper.SendEmailOutActivityForEntity(htmlData, textData, tenant, toEmail, subject, cc, bcc, userId, myEntityId, customerId, objectTableId, attachments, entityReference, documentTypeCode, eventTypeCode);

        }

       
    }
}