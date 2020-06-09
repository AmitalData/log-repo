using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Web;

using WebFreight.Web.WebServices;

namespace WebFreight.Web.WebPages
{
    public partial class HowToDownloadPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string headerRequest = Request["id"];

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