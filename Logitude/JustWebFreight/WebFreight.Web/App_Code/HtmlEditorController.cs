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

        public HttpResponseMessage PostGetEditorHtmlData(HtmlEditorResolveArgs htmlEditorResolveArgs)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnEntityTenant("Automation", htmlEditorResolveArgs.Tenant, authToken.Tenant);

                if (htmlEditorResolveArgs.Tenant != authToken.Tenant)
                {
                    throw new Exception("Sorry you’re not authenticated");
                }

                HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();

                var htmlEditorResult = htmlEditorHelper.GetEditorHtmlData(htmlEditorResolveArgs);
                SendHtmlFilter reslutFilter = new SendHtmlFilter();

                if (!string.IsNullOrEmpty(htmlEditorResult.HtmlString) && htmlEditorResolveArgs.Mode == "Edit")
                {

                    string header = htmlEditorHelper.getBetween(htmlEditorResult.HtmlString, "<header>", "</header>");
                    string headerheight = htmlEditorHelper.getBetween(header, "<height>", "</height>");

                    string footer = htmlEditorHelper.getBetween(htmlEditorResult.HtmlString, "<footer>", "</footer>");
                    string footerheight = htmlEditorHelper.getBetween(footer, "<height>", "</height>");

                    if (htmlEditorResolveArgs.Mode == "Edit")
                    {
                        htmlEditorResult.HtmlString = htmlEditorResult.HtmlString.Replace("<header>" + header + "</header>", "").Replace("<footer>" + footer + "</footer>", "");
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





                reslutFilter.Htmlstring = htmlEditorResult.HtmlString;
                reslutFilter.Subject = htmlEditorResult.Subject;
                reslutFilter.From = htmlEditorResult.From;
                reslutFilter.ReplyTo = htmlEditorResult.ReplyTo;
                reslutFilter.Cc = htmlEditorResult.Cc;
                reslutFilter.Bcc = htmlEditorResult.Bcc;
                reslutFilter.ToEmail = htmlEditorResult.To;
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
                SecurityUtility.AuthenticationOnTenant(tenant);

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
                SecurityUtility.AuthenticationOnEntityTenant("SendHtml", filter.Tenant, authToken.Tenant);




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
                        List<string> attachmentDocumentId = filter.Attachments.Split(',').Where(d => !string.IsNullOrEmpty(d)).ToList();
                        DocumentRepository documentRepository = new DocumentRepository(authToken.Tenant);

                        if (!documentRepository.CheckIfDocumentsExistOnTenant(attachmentDocumentId, authToken.Tenant))
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


                    DocumentPopulateAutomaticDateUpdateService documentPopulateAutomaticDateUpdateService = new DocumentPopulateAutomaticDateUpdateService();
                    documentPopulateAutomaticDateUpdateService.Update(new DocumentPopulateAutomaticDateArgs() { EntityId = filter.EntityId, ObjectTableName = filter.ObjectTableName, ChildObjectTableId = filter.ChildObjectTableId, ChildEntityId = filter.ChildEntityId, DocumentTypeCode = filter.DocumentTypeCode, ProcessType = "Send", Tenant = filter.Tenant });


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
            try
            {

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnEntityTenant("FroalaEditor", filter.Tenant, authToken.Tenant);

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
                reslut = exportDocumentHelper.SaveEditedReportToServer(filter.DocumentOutId, pdfDataFile, htmlDataFile, filter.Tenant, filter.DocumentTypeCopyId, filter.DocumentTypeId, filter.EntityId, filter.ChildEntityId);

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