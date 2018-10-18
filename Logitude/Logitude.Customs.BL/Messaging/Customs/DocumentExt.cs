using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Server.Tools.Models;
namespace Logitude.Customs.BL.Messaging.Customs
{
    public static class DocumentExt
    {
        public static string GetBlobUrl(this Document document, string documentSufix)
        {

            try
            {
                string filename = document.Id + documentSufix + "." + //document.Extension
                document.Extension.ToLower()
                ;

            string filePath = "tenant" + document.Tenant + "/" + StorageAcountDetails.GetBlobNameByLocation(filename, document.Folder);
                return filePath
                    .ToLower(); //itzik ask ihab : ok 
            }
            catch (Exception e)
            {
                var mess = "";
                if (document == null)
                {
                    e.ChangeExceptionMessage("GetBlobUrl():document is null");
                    throw e;
                }
                if (string.IsNullOrWhiteSpace(document.Id))
                {
                    e.ChangeExceptionMessage("GetBlobUrl():document id  is null");
                    throw e;
                }
                e.ChangeExceptionMessage("GetBlobUrl():document id  =" + document.Id);
                throw e;
            }
            
        }



    }
}
