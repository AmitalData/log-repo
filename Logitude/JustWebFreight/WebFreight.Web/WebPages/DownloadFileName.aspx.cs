using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Simplog.Data.CommonDataModel.Repositories;
using WebFreight.Web.WebServices;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.WebPages
{
    public partial class DownloadFileName : System.Web.UI.Page
    {
        public bool CheckAvailablityTenantsForEmail(string email, int tenant)
        {
            ContactRepository contactRep = new ContactRepository(tenant);
            return (contactRep.CheckEmailAvailabilityForTenant(email, tenant) || contactRep.CheckEmailAvailabilityForTenant0(email));
        }
        int? tenant = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
           
                string token = Request["tempId"] ?? "";
                string filename = Request["id"] ?? "";

                SecurityDocumentResult securityDocumentResult = SecurityDocumentHelper.ValidationDocumentToken(token);
                bool isValid = securityDocumentResult.IsValid;
                string email = securityDocumentResult.Email;
                string exceptionMessage = securityDocumentResult.ExceptionResult;
                tenant = securityDocumentResult.Tenant;

                if (isValid)
                {
                    if (!CheckAvailablityTenantsForEmail(email, (int)tenant)) isValid = false;
                }


                if (isValid)
                {
                    if (!string.IsNullOrEmpty(filename))
                    {
                        Uploader up = new Uploader();
                        var containername = StorageAcountDetails.GetCurrentContainer((int)tenant);
                        byte[] _DatainByte = up.DownloadStaticFile(filename, containername.Name,(int)tenant);

                        if (_DatainByte != null)
                        {
                            HttpContext.Current.Response.Clear();
                            HttpContext.Current.Response.AddHeader("Content-Length", _DatainByte.Length.ToString());

                            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                            HttpContext.Current.Response.ContentType = "application/octet-stream";

                            HttpContext.Current.Response.BinaryWrite(_DatainByte);

                            if (HttpContext.Current.Response.IsClientConnected)
                            {
                                HttpContext.Current.Response.Flush();
                                HttpContext.Current.Response.Close();
                                HttpContext.Current.ApplicationInstance.CompleteRequest();
                            }
                        }
                    }
                }
                else
                {
                    var message = exceptionMessage;
                    if (string.IsNullOrEmpty(exceptionMessage)) message = "Sorry you’re not authenticated to view this document.";
                    Response.Output.Write(message);
                 //   throw new ApplicationException(message);

                }


            }

            catch
            {
                //AzureLog.SaveLogsInStorage(ErrorMessage, "E",0,User.Identity.Name,User.Identity.Name);
            }
        }
    }
}