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

namespace WebFreight.Web.WebServices
{
    /// <summary>
    /// Summary description for DocOutBuildWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class DocOutBuildWebService : System.Web.Services.WebService
    {

        [WebMethod]

        public string HelloWorld(string stiBusinessObjectJsonString,  string documentTypeTemplateId , string otherstiBusinessObjectJsonString)
        {
            int tenant = 1;
            StiReport stiReport = new StiReport();

            if (!string.IsNullOrEmpty(stiBusinessObjectJsonString))
            {
                StiBusinessObject stiBusinessObject = LogitudeXmlSerializer.DeserializeObject<StiBusinessObject>(stiBusinessObjectJsonString);
                StiBusinessObject otherstiBusinessObject = !string.IsNullOrEmpty(otherstiBusinessObjectJsonString)? JsonConvert.DeserializeObject<StiBusinessObject>(stiBusinessObjectJsonString, new JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore }):null;
                DocumentTypeTemplateRepository documentTypeTemplaterep = new DocumentTypeTemplateRepository(tenant);
                DocumentTypeTemplate defaulttemplate = documentTypeTemplaterep.GetSingleDocumentTypeTemplate(documentTypeTemplateId);

                ExportDocumentHelper exportDocumentHelper = new ExportDocumentHelper();
                stiReport = exportDocumentHelper.LoadandRenderStiReport(defaulttemplate, stiBusinessObject, tenant, otherstiBusinessObject);

            }

            return JsonConvert.SerializeObject(stiReport, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore }); 

        }

    }
}
