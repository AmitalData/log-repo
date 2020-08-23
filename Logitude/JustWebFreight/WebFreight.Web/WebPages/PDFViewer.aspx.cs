using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

using Atp.Pdf;

//using Microsoft.WindowsAzure.Storage;

using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;

using WebFreight.Web.Azure;
using WebFreight.Web.CommonDataModel;
using WebFreight.Web.Testing;
using WebFreight.Web.WebServices;
using Microsoft.WindowsAzure.Storage.Blob;
using Logitude.SystemLogs;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel.Repositories;
using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using System.Transactions;
using Simplog.Global.Data.GlobalModel;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.WebPages
{
    public partial class PDFViewer : System.Web.UI.Page
    { 

 
        protected void Page_Load(object sender, EventArgs e)
        {
            string data = Request.QueryString["Args"];

            PdfDocument pdfDoc = new PdfDocument();

            if   (pdfDoc != null)
                {

                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;filename=PDFViewer.pdf");
                    HttpContext.Current.Response.ContentType = "application/" + "pdf";

                    MemoryStream memoryStream = new MemoryStream();
                    pdfDoc.Save(memoryStream);
                    HttpContext.Current.Response.BinaryWrite(memoryStream.ToArray());

                    if (HttpContext.Current.Response.IsClientConnected)
                    {
                        HttpContext.Current.Response.Flush();
                        //HttpContext.Current.Response.Close();
                        HttpContext.Current.ApplicationInstance.CompleteRequest();

                    }

                }
 
 
        
        }
    }
}
