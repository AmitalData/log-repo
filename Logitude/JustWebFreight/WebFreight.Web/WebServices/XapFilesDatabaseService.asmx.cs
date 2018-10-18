using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.WebServices
{
    /// <summary>
    /// Summary description for XapFilesDatabaseService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class XapFilesDatabaseService : System.Web.Services.WebService
    {

        [WebMethod]
        public void UpsertXapFile(string fileName ,byte[] fileData , bool isStaging)
        {
            UpdateXapFileData updateXapFileData = new UpdateXapFileData();
            updateXapFileData.UpdateXapFile(fileName, fileData, isStaging);

        }


        [WebMethod]
        public XapFileInfo GetXapFile(string fileName , bool isstaging)
        {
            GetXapFileData getXapFileData = new GetXapFileData();

            return getXapFileData.GetXapFile(fileName, isstaging);

        }



  

    }
}
