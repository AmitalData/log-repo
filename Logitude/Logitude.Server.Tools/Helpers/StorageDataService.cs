using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Helpers
{
 public static class StorageDataService
    {

        public static void WriteFileOnStorage(StorageDataArgs storageDataArgs)
        {
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = storageDataArgs.FileName,
                FolderName = storageDataArgs.FolderName,
                Tenant = storageDataArgs.Tenant,
                FileSize = storageDataArgs.FileData.Length,
                Extension = storageDataArgs.Extension ,
                
            };

            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            storageservice.Write(storageDataArgs.FileData, fileInfo);
        }

        public static byte[] ReadFileFromStorage(StorageDataArgs storageDataArgs)
        {
            byte[] fileData = null;
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = storageDataArgs.FileName,
                FolderName = storageDataArgs.FolderName,
                Tenant = storageDataArgs.Tenant,
                Extension = storageDataArgs.Extension,

            };
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            fileData =  storageservice.Read( fileInfo);
            return fileData;
        }
    }

    public class StorageDataArgs
    {
        public string FileName { get; set; }
        public string FolderName { get; set; }
        public int Tenant { get; set; }
        public byte[] FileData { get; set; }
        public string Extension { get; set; }

    }
}
