using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;

namespace WebFreight.Web.CustomWebServices
{
    /// <summary>
    /// Summary description for DeclarationFormsWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class DeclarationFormsWebService : System.Web.Services.WebService
    {
        public DeclarationPM declarationPM;
        [WebMethod]
        public byte[] LoadDataToTzrufa(string declarationId, int tenant, string documentTypeCopyId)
        {
            return StartLoadingDataToTzrufa(declarationId, tenant, documentTypeCopyId, false);
        }

        public byte[] StartLoadingDataToTzrufa(string declarationId, int tenant, string documentTypeCopyId, bool isPrint)
        {
            DeclarationFormsDataProvider frombDp = LoadTzrufaDataProvider(declarationId, tenant, documentTypeCopyId, isPrint);
            XmlSerializer serializer = new XmlSerializer(typeof(DeclarationFormsDataProvider));
            using (MemoryStream memstream = new MemoryStream())
            {
                serializer.Serialize(memstream, frombDp);
                memstream.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(memstream);
                string content = reader.ReadToEnd();
                byte[] bytearray = memstream.ToArray();
                return bytearray;
            }
        }

        private DeclarationFormsDataProvider LoadTzrufaDataProvider(string declarationId, int tenant, string documentTypeCopyId, bool isPrint)
        {
            DeclarationFormsDataProvider formbDp = new DeclarationFormsDataProvider();
            ICustomContext customsContext = CustomContext.GetContext(tenant);
       
            DeclarationQueryService declarationQuery = new DeclarationQueryService(customsContext);

            declarationPM = declarationQuery.GetSingle(declarationId, false, false);
        

            if (declarationPM != null)
            {

                formbDp.CustomFileNumber = declarationPM.CustomFileNo;
            }


            return formbDp;
        }
    }
}
