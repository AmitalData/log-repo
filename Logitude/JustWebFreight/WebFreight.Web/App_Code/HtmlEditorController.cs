using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.QuoteModel.APIDataContract.ApiV1;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text; 
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.App_Code
{
    public class HtmlEditorController : ApiController
    {

        public HttpResponseMessage GetEditorHtmlData(string docOutId, string entityId, string objectTableId, string childEntityId, string childEntityObjectTableId, int tenant, string userId, bool theIsSendMail, string documentTemplateId, string subject , string Mode=null ,string from = null, string replyTo = null, string cc = null)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                if (tenant != authToken.Tenant)
                {
                    throw new Exception("Sorry you’re not authenticated");
                }

                HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();

            string htmlstring = htmlEditorHelper.GetEditorHtmlData(docOutId, entityId, objectTableId, childEntityId, childEntityObjectTableId, tenant, userId, theIsSendMail, documentTemplateId, ref subject, ref from, ref replyTo,ref cc, Mode);
            SendHtmlFilter reslutFilter = new SendHtmlFilter();

            if (!string.IsNullOrEmpty(htmlstring) && Mode == "Edit")
            {

                string header = htmlEditorHelper.getBetween(htmlstring, "<header>", "</header>");
                string headerheight = htmlEditorHelper.getBetween(header, "<height>", "</height>");

                string footer = htmlEditorHelper.getBetween(htmlstring, "<footer>", "</footer>");
                string footerheight = htmlEditorHelper.getBetween(footer, "<height>", "</height>");

                if (Mode == "Edit")
                {
                    htmlstring = htmlstring.Replace("<header>" + header + "</header>", "").Replace("<footer>" + footer + "</footer>", "");
                    header = header.Replace("<height>" + headerheight + "</height>", "");


                    footer += "</footer>";
                    footer = footer.Replace("<div style = 'bottom:0;'>", "").Replace("</div></footer>", "");

                    footer = footer.Replace("<height>" + footerheight + "</height>", "");


                    reslutFilter.HeaderHtml = header;
                    reslutFilter.FooterHtml = footer;



                    headerheight = htmlEditorHelper.getBetween(headerheight, "<div style='display:none'>", "</div>");
                    footerheight = htmlEditorHelper.getBetween(footerheight, "<div style='display:none'>", "</div>");

                    if (!string.IsNullOrEmpty(headerheight)) reslutFilter.HeaderHeight = Int32.Parse(headerheight);
                    if (!string.IsNullOrEmpty(footerheight)) reslutFilter.FooterHeight = Int32.Parse(footerheight);


                }
            }


     

 
            reslutFilter.Htmlstring = htmlstring;
            reslutFilter.Subject = subject;
            reslutFilter.From = from;
            reslutFilter.ReplyTo =replyTo;
            reslutFilter.Cc = cc;

            return Request.CreateResponse(HttpStatusCode.OK, reslutFilter);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetSentMessageHtmlBody(string documentId, int tenant)
        {

            try
            {
                string html = "";

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                if (tenant != authToken.Tenant)
                {
                    throw new Exception("Sorry you’re not authenticated");
                }

                HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();
               System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
              byte[] htmlbyte = htmlEditorHelper.GetSentMessageHtmlBody(documentId, tenant);

             if (htmlbyte != null)
             {
                 html = enc.GetString(htmlbyte);
             }
             return Request.CreateResponse(HttpStatusCode.OK, html);


            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

 
        //  SendHtmlFilter
        public HttpResponseMessage PostSendHtmlDocument(SendHtmlFilter filter)
        {
            try
            {
                string reslut = "";


                if (!string.IsNullOrEmpty(filter.Subject) && filter.Subject.ToLower() == "exceptiontest") throw new Exception("Exception");

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);




                if (filter.Tenant == authToken.Tenant)
                {
                    
                    UserQuery userQuery = new UserQuery(authToken.Tenant);
                    bool isExist = userQuery.CheckIfUserExistInTenant(filter.UserId, authToken.Tenant);
                    if (!isExist)
                    {
                        throw new Exception("Sorry you’re not authenticated to send this email");
                    }
                    if (!string.IsNullOrEmpty(filter.Attachments))
                    {
                        List<string> attachmentDocumentId = filter.Attachments.Split(',').Where(d=>!string.IsNullOrEmpty(d)).ToList();
                        DocumentRepository documentRepository = new DocumentRepository(authToken.Tenant);

                        if(!documentRepository.CheckIfDocumentsExistOnTenant(attachmentDocumentId, authToken.Tenant))
                        {
                            throw new Exception("Sorry you’re not authenticated to send these attachments");
                        }
                    }
             

                    System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
                    HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();
                    EncodedHtmlHelper encodedHtmlHelper = new EncodedHtmlHelper();
                    string htmlstring = "";
                    if (!string.IsNullOrEmpty(filter.Htmlstring))
                    {
                        htmlstring = filter.Htmlstring;
                        htmlstring = htmlEditorHelper.GetLogoHtmlString(htmlstring);
                        htmlstring = encodedHtmlHelper.EncodedHtmlScript(htmlstring);
                    }

                   
                    byte[] bytedata = enc.GetBytes(htmlstring);
                    if (!filter.IsCRM)
                    {
                        reslut = htmlEditorHelper.SendHtmlDocument(bytedata, filter.InternalDocumentId, filter.ExternalDocumentId, filter.Tenant, filter.ToEmail, filter.Subject, filter.Cc, filter.Bcc, filter.UserId, filter.EntityId, filter.ObjectTableId, filter.Attachments, filter.EntityReference, filter.From, filter.ReplyTo);
                    }
                    else
                    {
                        
                        string htmlPlainString = "";
                        if (!string.IsNullOrEmpty(filter.HtmlPlainString))
                        {
                            htmlPlainString = htmlEditorHelper.GetLogoHtmlString(filter.HtmlPlainString);
                            htmlPlainString = encodedHtmlHelper.EncodedHtmlScript(htmlPlainString);
                        }

                        byte[] bytePlainTextdata = enc.GetBytes(htmlPlainString);

                        reslut = htmlEditorHelper.SendEmailOutActivityForEntity(bytedata, bytePlainTextdata, filter.Tenant, filter.ToEmail, filter.Subject, filter.Cc, filter.Bcc, filter.UserId, filter.EntityId, filter.CustomerId, filter.ObjectTableId, filter.Attachments, filter.EntityReference, filter.DocumentTypeCode, filter.EventTypeCode);
                    }

                    if (filter.ExportQuotationsToIntegratedSystem && filter.ObjectTableName == "Quote")
                    {
                        QuoteQueryService quoteQueryService = new QuoteQueryService(filter.Tenant);
                        Logitude.BL.QuoteModel.APIDataContract.ApiV1.Quote quote = quoteQueryService.GetQuoteById(filter.EntityId, filter.Tenant);
                        string xmlstring = "";

                        if (quote != null)
                        {
                            xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(quote);
                        }
                        CommunicationsParams tasklogParams = new CommunicationsParams()
                        {
                            Tenant = filter.Tenant,
                            CommunicationLogTypeCode = "Q",
                            QueueName = "externaltasksqueue" + filter.Tenant + 1,
                            Priority = 1,
                            InOut = "O",
                            Status = "W",
                            LoggingUserId = filter.UserId,
                            LoggingObjectTableId = filter.ObjectTableId,
                            LoggingEntityId = filter.EntityId,
                            Subject = "Quotation Document",
                            FolderName = "ExternalTasksQueue",
                        };

                        List<QueueTask> queue2Tasks = new List<QueueTask>();
                        queue2Tasks.Add(
                            new QueueTask()
                            {
                                Action = "ExportQuotationsToIntegratedSystem",
                                Parameters = new List<Parameter>()
                            {
                                         new Parameter{ Name = "QuoteMetaData", Order = 1,Value =  xmlstring},

                            }
                            });

                        tasklogParams.ByteData = LogitudeXmlSerializer.SerializeObject(queue2Tasks);
                        Communications.AddCommunicationLog(tasklogParams);

                    }
                }
                else throw new Exception("Sorry you’re not authenticated to send this email");
                return Request.CreateResponse(HttpStatusCode.OK, reslut);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        //Pdf Document Template Html
        public HttpResponseMessage PutSaveEditedReportToServer(FroalaEditorFilters filter)
        {
            try {

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                if (filter.Tenant != authToken.Tenant)
                {
                    throw new Exception("Sorry you’re not authenticated");
                }

                HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();
           
            string reslut = "";
            string htmlString = filter.HtmlString;
            if (string.IsNullOrEmpty(htmlString)) htmlString = "";
            htmlString = htmlString.Replace("\"", "'");

            byte[] htmlDataFile = GetBytes(htmlString);
            byte[] pdfDataFile = htmlEditorHelper.BuildPdfDocumentHtml(htmlString);
            ExportDocumentHelper exportDocumentHelper = new ExportDocumentHelper();
            reslut = exportDocumentHelper.SaveEditedReportToServer(filter.DocumentOutId, pdfDataFile, htmlDataFile, filter.Tenant, filter.DocumentTypeCopyId, filter.DocumentTypeId, filter.EntityId , filter.ChildEntityId);

            return Request.CreateResponse(HttpStatusCode.OK, reslut);
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

    }



}