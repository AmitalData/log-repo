using System.IO;
using System.Web.Services;
//using Microsoft.WindowsAzure.Storage;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Azure;

using WebFreight.Web.Azure;
using Microsoft.WindowsAzure.Storage.Blob;
using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;

namespace WebFreight.Web.WebServices
{
    /// <summary>
    /// Summary description for SharedLogisticsUpdateWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SharedLogisticsUpdateWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public byte[] GetSharedLogisticsUpdates(int tenant,string id)
        {
            byte[] datainByte;
            SharedLogisticsUpdateRepository sharedLogisticsUpdateRep = new SharedLogisticsUpdateRepository(tenant);


            DocumentRepository documentrepository = new DocumentRepository(tenant);

            SharedLogisticsUpdate update = sharedLogisticsUpdateRep.GetSingleSharedLogisticUpdate(id);
            if (update != null)
            {
                Document document = documentrepository.GetSingleDocument(tenant, update.DocumentId);
                //string containername = "tenant" + tenant.ToString();

                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = document.Id,
                    FolderName = document.Folder,
                    Extension = document.Extension,
                    Tenant = document.Tenant,
                    FileSize = document.FileSize,
                };
                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                datainByte = storageservice.Read(fileInfo);


                 
                    return datainByte;
                


                //CloudBlobContainer blobContainer = StorageAcountDetails.BlobClient.GetContainerReference(containername);
                //if (blobContainer != null)
                //{
                //    string filename = document.Id + "." + document.Extension;

                //    blobContainer = StorageAcountDetails.GetCurrentContainer(tenant);


                //    var blobfile = blobContainer.GetBlockBlobReference(StorageAcountDetails.GetBlobNameByLocation(filename, document.Folder));

                //    if (blobfile.Exists())
                //    {
                //        using (MemoryStream memstream = new MemoryStream())
                //        {

                //            blobfile.DownloadToStream(memstream);
                //            datainByte = memstream.ToArray();

                //        }
                         
                //        return datainByte;
                //    }

                //    else
                //        return null;


                //}
            }
            return null;
        }
    }
}
