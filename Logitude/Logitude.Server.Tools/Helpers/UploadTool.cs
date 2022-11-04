
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Helpers
{
   public class UploadTool
    {
        private string fileName;
        byte[] datainByte;
        private long receivedBytes = 0;
        public long ReceivedBytes
        {
            get { return receivedBytes; }
            set { receivedBytes = value; }
        }
        string fileNameAndExtension;
        string documentIdAndExtension;
        public UploadTool()
        {

        }

        public byte[] DownloadFile(string documentId, string documentExtension, string fileLocation, int tenant, bool withOutTenant = false)
        {
            try
            {


                fileName = documentId + "." + documentExtension;
                string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(fileName.ToLower(), fileLocation);
                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = documentId,
                    FolderName = fileLocation,
                    Extension = documentExtension,
                    Tenant = tenant,


                };
                datainByte = storageservice.Read(fileInfo);



                return datainByte;

            }
            catch (Exception e)
            {
                return null;
            }
        }

     
        public string UploadImage(string filename, byte[] buffer, long fileSize, long sentBytes, string[] blockIdsList, int bufferNumber, int tenant, string extension, string cardId, string contactId, string imageDetalId)
        {
            var x = 0;
            var y = 10 / x;
            string filelocation = "images";
            fileName = filename.ToLower();
            string filePath = "tenant" + tenant.ToString() + "/";
            string imagedetailid = null;
            try
            {


                ImageDetailRepository imageDetailRep = new ImageDetailRepository(tenant);
                ImageDetail imagedetail = null;
                if (!string.IsNullOrEmpty(imagedetailid))
                {
                    imagedetail = imageDetailRep.GetSingleImageDetail(imagedetailid, tenant);
                }
                if (imagedetail == null)
                {
                    imagedetail = new ImageDetail() { Id = IdCounter.GetNumber("ImageDetail", tenant), Tenant = tenant, Extension = extension, Size = fileSize };
                    imageDetailRep.Add(imagedetail);
                    imageDetailRep.SubmitChanges();
                }
                imagedetailid = imagedetail.Id;



                fileName = imagedetailid;

                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;


                ReceivedBytes += buffer.Length;
                fileNameAndExtension = fileName + "." + extension;
                //if (sentBytes < fileSize)
                //{
                MemoryStream memorystream = new MemoryStream(buffer);
                //tempcloudBlockBlob.PutBlock(blockIdsList[bufferNumber], memorystream, null);
                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = fileName,
                    FolderName = filelocation,
                    Extension = extension,
                    Tenant = tenant,
                    FileSize = fileSize,

                };
                storageservice.WriteBlock(buffer, sentBytes, blockIdsList, bufferNumber, fileInfo);


                if (sentBytes == fileSize)
                    fileNameAndExtension = fileName + "." + extension;



                documentIdAndExtension = fileNameAndExtension;


            }
            //}
            catch (Exception e)
            {
            }
            return imagedetailid;
        }

    }
}
