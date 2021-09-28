using EvoPdf;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.WorkerRole.PODImage
{
    public class PODImageConverterService
    {
        private StorageDataArgs storageDataArgs;
        private int tenant;
        private DocumentRepository documentRepository;
        private Simplog.Data.CommonDataModel.EntityPOCOs.Document document;

        public PODImageConverterService(string documnetId, int tenant)
        {
            this.tenant = tenant;
            documentRepository = new DocumentRepository(tenant);
            document = documentRepository.GetSingleDocument(tenant, documnetId);
        }



        public void Convert(IPODImageConverter podImageConverter)
        {
            if (IsAllowConvert(podImageConverter))
            {
                byte[] convertedFileData = podImageConverter.Convert(ReadFileFromStorage());
                if (convertedFileData != null)
                {
                    UpdateDocument(convertedFileData, podImageConverter.Extention);
                    UpdateFileOnStorage(convertedFileData);
                }
            }
        }


        private bool IsAllowConvert(IPODImageConverter podImageConverter)
        {
            return document != null && document.Extension.ToLower() != podImageConverter.Extention.ToLower();
        }

        private byte[] ReadFileFromStorage()
        {
            storageDataArgs = BuildStorageDataArgs();
            return StorageDataService.ReadFileFromStorage(storageDataArgs);
        }


        private void UpdateDocument(byte[] fileData, string extention)
        {
            document.Extension = extention;
            document.FileSize = fileData.Length;
            documentRepository.Update(document);
            documentRepository.SubmitChanges();
        }


        private void UpdateFileOnStorage(byte[] fileData)
        {
            storageDataArgs = BuildStorageDataArgs();
            storageDataArgs.FileData = fileData;
            StorageDataService.WriteFileOnStorage(storageDataArgs);
        }




        private StorageDataArgs BuildStorageDataArgs()
        {
           return new StorageDataArgs()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Tenant = document.Tenant,
                Extension = document.Extension,
              
            };

        }
    }
}