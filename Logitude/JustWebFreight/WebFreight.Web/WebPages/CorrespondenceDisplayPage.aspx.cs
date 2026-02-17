using HtmlAgilityPack;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityQueryServices;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.WebPages
{
    public partial class CorrespondenceDisplayPage : System.Web.UI.Page
    {
        string headerRequest = " ";
        string Id, EntityId;
        int? tenant = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            string request = Request["id"];
            string token = Request["tempId"] ?? "";
            Id = request.Split('_')[0];


            SecurityDocumentResult securityDocumentResult = SecurityDocumentHelper.ValidationDocumentToken(token);
            bool isValid = securityDocumentResult.IsValid;
            string email = securityDocumentResult.Email;
            string exceptionMessage = securityDocumentResult.ExceptionResult;
            tenant = securityDocumentResult.Tenant;

            if (isValid)
            {
                HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new System.Security.Principal.GenericIdentity(email), new string[0]);
                SecurityUtility.AuthenticationOnTenant((int)tenant);
                SecurityUtility.CheckContactFeature("Ticket", "READ", (int)tenant);

                CorrespondenceRepository repository = new CorrespondenceRepository((int)tenant);
                Correspondence line = repository.GetSingle(Id, (int)tenant);
                EntityId = line.EntityId;

                if (line != null)
                {
                    headerRequest = line.HTMLFullBody;

                    if (!string.IsNullOrEmpty(headerRequest))
                    {
                        headerRequest = Regex.Replace(headerRequest, "style\\s*=\\s*\"[^\"]*\\bfont-family:.*?'.*?(;|\")", m => m.Value.Replace("'", ""));
                        headerRequest = Regex.Replace(headerRequest, @"\t|\n|\r", "");
                        headerRequest = headerRequest.Replace('"', '\'');
                    }
                }

                string stripImag = this.StripImagTags();
                if (!string.IsNullOrEmpty(stripImag))
                {
                    headerRequest = stripImag;
                }

                Response.Write(headerRequest);
            }
            else
            {
                var message = exceptionMessage;
                if (string.IsNullOrEmpty(exceptionMessage)) message = "Sorry you’re not authenticated to view this excel.";
                Response.Output.Write(message);
  
            }
        }

        private string StripImagTags()
        {
            var output = new StringBuilder();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(headerRequest);
            var sb = new StringBuilder();

            if (doc.DocumentNode.SelectSingleNode("//img[@src and (@width or @height)]") != null)
            {
                ICRMContext context = CRMContext.GetContext((int)tenant);
                CorrespondencesAttachmentQueryService attachQuery = new CorrespondencesAttachmentQueryService(context);
                List<CorrespondencesAttachmentPM> myCorrespondencesAttachments = attachQuery.GetCorrespondencesAttachmentsListByCorrespondenceIdAndTenant(Id, (int)tenant);
                List<DocumentsFilingPM> DocumentsFilings = new List<DocumentsFilingPM>();
                DocumentsFilingQuery docQuery = new DocumentsFilingQuery((int)tenant);
                ObjectTableRepository objectTableRepository = new ObjectTableRepository((int)tenant);
                ObjectTable objectTable = objectTableRepository.GetObjectTableByName("Ticket", 0, true);
                DocumentsFilings = docQuery.GetDocumentsFilingPMsByEntityId((int)tenant, EntityId, objectTable.Id);

                foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//img[@src and (@width or @height)]"))
                {
                    //var src = node.Attributes["src"].Value.Split('?');
                    //var width = node.Attributes["width"].Value.Replace("px", "");
                    //var height = node.Attributes["height"].Value.Replace("px", "");

                    HtmlAttribute src = node.Attributes["src"];
                    string srctext = src.Value;
                    string filename = "";
                    if (!string.IsNullOrEmpty(srctext))
                    {
                        filename = srctext.Split('@')[0].Split(':')[1].Split('.')[0].Trim();
                    }

                    string securityId = DocumentsFilings.Where(a => a.FileName == filename).Select(a=>a.SecurityId).FirstOrDefault();
                    string uri = "";
                    if (string.IsNullOrEmpty(securityId))
                    {
                        uri = srctext;
                    }
                    else
                    {
                        uri = LogitudeSettings.LogitudeURL + "/WebPages/CorrespondenceDownloadpage.aspx?id=" + securityId + "~" + tenant;
                    }

                    node.SetAttributeValue("src", uri);
                }

                using (var writer = new StringWriter(sb))
                {
                    doc.Save(writer);
                }
            }

            return sb.ToString();
        }
    }
}