using EvoPdf;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.WorkerRole
{
    public class ConvertImageToEvoPdfService
    {
        private string documentId = string.Empty;
        private int tenant;
        private byte[] fileData = null;

        public ConvertImageToEvoPdfService(string documnetId, int tenant)
        {
            this.documentId = documnetId;
            this.tenant = tenant;
            StorageDataArgs storageDataArgs = BuildStorageDataArgs();
            fileData = StorageDataService.ReadFileFromStorage(storageDataArgs);
        }


        public void Convert()
        {
            

        }

    
        private StorageDataArgs BuildStorageDataArgs()
        {
           var document = new DocumentRepository(tenant).GetSingleDocument(tenant , documentId); 

           return new StorageDataArgs()
            {
                FileName = document.FileName,
                FolderName = document.Folder,
                Tenant = document.Tenant,
                Extension = document.Extension,
            };

        }
    }
}