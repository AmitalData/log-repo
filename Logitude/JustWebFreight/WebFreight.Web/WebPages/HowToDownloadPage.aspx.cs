using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Web;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using WebFreight.Web.Helpers;

using WebFreight.Web.WebServices;

namespace WebFreight.Web.WebPages
{
    public partial class HowToDownloadPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string headerRequest = Request["Code"];
            string id = Request["id"];
            int? tenant = null;
            string token = Request["Token"] ?? "";
            SecurityDocumentResult securityDocumentResult = SecurityDocumentHelper.ValidationDocumentToken(token);
            bool isValid = securityDocumentResult.IsValid;
            string email = securityDocumentResult.Email;
            string exceptionMessage = securityDocumentResult.ExceptionResult;
            tenant = securityDocumentResult.Tenant;
            if (!string.IsNullOrEmpty(email))
            {
                int tenant1 = tenant == null ? 0 : tenant.Value;

                ContactRepository contactRepository = new ContactRepository(tenant1);
                Contact contact = contactRepository.GetSingleContactByEmail(email, tenant1);
                if (contact == null)
                {
                    this.Context.Response.Redirect("../Login.aspx?HowToDownloadPage="+id);
                }
            }
            else
            {
                this.Context.Response.Redirect("../Login.aspx?HowToDownloadPage="+id);
            }

            Uploader manager = new Uploader();
            byte[] data = null;
            string documentName="";

            HelpResourceRepository helpResourceRepository = new HelpResourceRepository();
            HelpResource helpResource = helpResourceRepository.GetSingleHelpResource(headerRequest, 0);

            if(helpResource != null)
            {
                documentName = helpResource.FileName;
                data = manager.DownloadStaticFile(documentName, "how-to");
            }
            
            if (data != null)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.AddHeader("Content-Length", data.Length.ToString());

                HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;filename=" + documentName);
                if (documentName.Split('.')[1].ToString() == "pdf")
                {
                    HttpContext.Current.Response.ContentType = "application/" + "pdf";
                }
                else
                {
                    HttpContext.Current.Response.ContentType = "application/" + "html";
                }

                HttpContext.Current.Response.BinaryWrite(data);

                if (HttpContext.Current.Response.IsClientConnected)
                {
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.Close();
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }

            else
            {
                Response.Output.Write("Document is not available ! ");
            }
        }
    }
}