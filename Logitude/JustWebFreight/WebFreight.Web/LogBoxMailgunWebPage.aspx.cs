using HtmlAgilityPack;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.SystemLogs;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.TicketAnalyzer;
using WebFreight.Web.Security;


namespace WebFreight.Web
{
    public partial class LogBoxMailgunWebPage : System.Web.UI.Page
    {
        EmailUpload emailDetails;
        InboundEmailGeneralHelperMethods helper;

        private string LogForOrit()
        {
            var retval = string.Empty;
            try
            {
                using (var reader = new StreamReader(Request.InputStream))
                {
                    retval = reader.ReadToEnd();
                }

                string folderpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");
                if (!Directory.Exists(folderpath))
                {
                    Directory.CreateDirectory(folderpath);
                }


                var sb = new StringBuilder();
                Request.Headers.AllKeys.ToList().ForEach(k => sb.AppendLine($"{k} : {Request.Headers[k]}"));
                sb.AppendLine("Request.Files count : " + Request.Files?.Count.ToString());

                string filename = Guid.NewGuid().ToString();
                File.WriteAllText($"{folderpath}\\LogboxMilgun_BODY_{filename}.log", retval);
                File.WriteAllText($"{folderpath}\\LogboxMilgun_HEADERS_{filename}.log", sb.ToString());
                File.WriteAllText($"{folderpath}\\LogboxMilgun_QUERYSTR_{filename}.log", Request.Url.OriginalString);

            }

            catch (Exception ex)
            {

            }

            return retval;

        }

        protected void Page_Load(object sender, EventArgs e)
        {

          string reqStreamString = LogForOrit();

            try
            {
                string values = reqStreamString;
                //using (var reader = new StreamReader(Request.InputStream))
                //{
                //    values = reader.ReadToEnd();
                //}

                if (!string.IsNullOrEmpty(values))
                {
                    helper = new InboundEmailGeneralHelperMethods(Request);
                    emailDetails = new EmailUpload();
                    string subject = helper.GetValue("subject");
                    emailDetails.Subject = helper.getSubject(subject);

                    emailDetails.Sender = helper.GetValue("sender");
                    emailDetails.RecipientEmail = helper.GetValue("recipient");

                    string toEmails = helper.GetValue("To");
                    if (!string.IsNullOrEmpty(toEmails) && toEmails.Contains(','))
                    {
                        toEmails = toEmails.Replace(',', ';');
                    }
                    toEmails = helper.TruncateCc(toEmails, 4000);
                    emailDetails.To = toEmails;

                    emailDetails.StrippedBodyPlain = helper.GetValue("body-plain");
                    emailDetails.FullBodyPlain = helper.Truncate(helper.GetValue("body-plain"), 4000);
                    string cc = helper.GetValue("Cc");
                    if (!string.IsNullOrEmpty(cc) && cc.Contains(','))
                    {
                        cc = cc.Replace(',', ';');
                    }

                    cc = helper.TruncateCc(cc, 4000);
                    emailDetails.CCs = cc;

                    emailDetails.StrippedHtml = helper.GetValue("stripped-html");
                    emailDetails.BodyHtml = helper.GetValue("body-html");

                    if (emailDetails.StrippedBodyPlain == emailDetails.BodyHtml)
                    {
                        string noHTML = Regex.Replace(emailDetails.BodyHtml, @"<[^>]+>|&nbsp;", "").Trim();
                        string noHTMLNormalised = Regex.Replace(noHTML, @"\s{2,}", " ");
                        string result = stripTags(emailDetails.BodyHtml);
                        emailDetails.FullBodyPlain = result;
                        emailDetails.StrippedBodyPlain = result;
                    }

                    if (string.IsNullOrWhiteSpace(emailDetails.StrippedBodyPlain))
                    {
                        emailDetails.StrippedBodyPlain = "Empty Body";
                    }

                    if (string.IsNullOrWhiteSpace(emailDetails.FullBodyPlain))
                    {
                        emailDetails.FullBodyPlain = "Empty Body";
                    }
                    emailDetails.AttachmentsFiles = helper.FillAttachments();


                    InsertNewAnalyzeQueue(emailDetails);
                }
            }

            catch (Exception errorInfo)
            {
                string errorMessage = errorInfo.Message;
                AzureLog.SaveLogsInStorage("LogBox MailGun Page error  " + Environment.NewLine + errorMessage, "E", DateTime.Now, errorInfo.Message, errorInfo.StackTrace, 0, null, null, null);
                throw;
            }
        }

        private void InsertNewAnalyzeQueue(EmailUpload emailDetails)
        {
            try
            {
                Type myType = emailDetails.GetType();
                MemoryStream myMemoryStream = new MemoryStream();
                XmlSerializer ser = new XmlSerializer(myType);
                ser.Serialize(myMemoryStream, emailDetails);
                myMemoryStream.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(myMemoryStream);
                string content = reader.ReadToEnd();
                byte[] bytearray = myMemoryStream.ToArray();

                User user = this.GetUser();
                int tenant = user != null ? user.Tenant : 0;

                AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();
                AnalyzeQueue analyzeQueue = new AnalyzeQueue()
                {
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(0),
                    From = "DocumentFilingEmail",
                    Subject = "Filing Emails",
                    Id = IdCounter.GetNumber("AnalyzeQueue", 0),
                    MessageBody = bytearray,
                    Status = "W",
                    Retries = 0,
                    ConnectedToEntity = true,
                    ConnectedToTenant = true,
                    FileSize = bytearray.Length,
                    Tenant = tenant,
                };

                analyzeQueue.SearchFields = analyzeQueue.From + ',' + analyzeQueue.Status;
                analyzeQueueReposiory.Add(analyzeQueue);
                analyzeQueueReposiory.SubmitChanges();
            }

            catch (Exception errorInfo)
            {
                string errorMessage = errorInfo.Message;
                AzureLog.SaveLogsInStorage("LogBox MailGun Page error  " + Environment.NewLine + errorMessage, "E", DateTime.Now, errorInfo.Message, errorInfo.StackTrace, 0, null, null, null);
                throw;
            }
        }
        private User GetUser()
        {
            User user = helper.GetTenant_LogBox(emailDetails.RecipientEmail);
            return user;
        }
        private string stripTags(string html)
        {
            html = html.Replace("\r", "").Replace("\n", " ");
            html = Regex.Replace(html, @"&nbsp;", "").Trim();
            html = Regex.Replace(html, @"\s{2,}", " ");

            var output = new StringBuilder();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            if (doc.DocumentNode != null && doc.DocumentNode.SelectNodes("//*") != null)
            {
                foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//*"))
                {
                    output.AppendLine(node.InnerText.ToString());

                    node.ParentNode.ReplaceChild(HtmlNode.CreateNode(node.InnerText.ToString() + "\n"), node);
                }
                return doc.DocumentNode.InnerText.Trim();
            }
            return "";
        }
    }
}