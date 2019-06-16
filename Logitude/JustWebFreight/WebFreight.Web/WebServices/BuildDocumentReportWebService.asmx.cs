using Logitude.Server.Tools;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.WebServices
{
    /// <summary>
    /// Summary description for BuildDocumentReportWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class BuildDocumentReportWebService : System.Web.Services.WebService
    {
        [WebMethod]
        public string BuildDocumentReport(string buildDocumentParameterxml)
        {


            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

            StiReport stiReport = new StiReport();

            if (!string.IsNullOrEmpty(buildDocumentParameterxml))
            {
                buildDocumentParameterxml = buildDocumentParameterxml.Replace("@TagOpen", "<");
                BuildDocumentParameter buildDocumentParameter = LogitudeXmlSerializer.DeserializeObject<BuildDocumentParameter>(buildDocumentParameterxml);
                ExportDocumentHelper exportDocumentHelper = new ExportDocumentHelper();
                stiReport = exportDocumentHelper.BuildReport(buildDocumentParameter.DocumentTypeCode, buildDocumentParameter.DocumentTypeId, buildDocumentParameter.EntityId, buildDocumentParameter.EntityObjectTableId, buildDocumentParameter.ChildEntityId, buildDocumentParameter.ChildObjectTableId, buildDocumentParameter.DefaulttemplateId, tenant, buildDocumentParameter.DocumentTypeCopyId, buildDocumentParameter.UserId);

            }

            string result = stiReport.SaveDocumentToString();
            return result;

        }

        [WebMethod]
        public string ExportDocument2Pdf(string exportDocument2PdfParameterxml)
        {
            string result = "";
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

            if (!string.IsNullOrEmpty(exportDocument2PdfParameterxml))
            {
                exportDocument2PdfParameterxml = exportDocument2PdfParameterxml.Replace("@TagOpen", "<");
                BuildDocumentParameter buildDocumentParameter = LogitudeXmlSerializer.DeserializeObject<BuildDocumentParameter>(exportDocument2PdfParameterxml);
                ExportDocumentHelper exportDocumentHelper = new ExportDocumentHelper();
                result = exportDocumentHelper.ExportDocument2Pdf(buildDocumentParameter.DocumentTypeId, buildDocumentParameter.EntityId, buildDocumentParameter.EntityObjectTableId, buildDocumentParameter.ChildEntityId, buildDocumentParameter.ChildObjectTableId, buildDocumentParameter.DocumentOutId, authToken.Tenant, buildDocumentParameter.DocumentTypeCopyId, buildDocumentParameter.UserId,false);

            }

            return result;

        }


    }
}
