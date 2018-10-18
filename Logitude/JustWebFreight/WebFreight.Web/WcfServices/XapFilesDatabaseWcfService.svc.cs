using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Web;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.WcfServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "XapFilesDatabaseWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select XapFilesDatabaseWcfService.svc or XapFilesDatabaseWcfService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class XapFilesDatabaseWcfService : IXapFilesDatabaseWcfService
    {

        public void UpsertXapFile(string fileName, byte[] fileData, bool isStaging)
        {

            string ipstring = System.Configuration.ConfigurationManager.AppSettings.Get("CustomerCareIP");
            string[] authenticatedIPs = ipstring.Split(',');

            //if (!authenticatedIPs.Contains(HttpContext.Current.Request.UserHostAddress))
            //{
            //    throw new Exception("You are not authorized to do this operation");
                
            //}

            UpdateXapFileData updateXapFileData = new UpdateXapFileData();
            updateXapFileData.UpdateXapFile(fileName, fileData, isStaging);
        }

        public Helpers.XapFileInfo GetXapFile(string fileName, bool isStaging)
        {

           
            string ipstring = System.Configuration.ConfigurationManager.AppSettings.Get("CustomerCareIP");
            string[] authenticatedIPs = ipstring.Split(',');

            //if (!authenticatedIPs.Contains(HttpContext.Current.Request.UserHostAddress))
            //{
            //    throw new Exception("You are not authorized to do this operation");
            //}

            GetXapFileData getXapFileData = new GetXapFileData();

            return getXapFileData.GetXapFile(fileName, isStaging);

        }
    }
}
