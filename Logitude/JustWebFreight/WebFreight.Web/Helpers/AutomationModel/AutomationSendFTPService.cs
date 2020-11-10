using Logitude.Server.Tools;
using Logitude.Server.Tools.FTP;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TextManager.Interop;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.AutomationModel
{
    public class AutomationSendFTPService
    {
        public void SendAutomationFTP(FTPAutomationDetails fTPDetails, string documentId , int tenant)
        {
            DocumentRepository documentRepository = new DocumentRepository(tenant);
            var document =   documentRepository.GetSingleDocument(tenant , documentId);
            if (document != null)
            {
                byte[] fileData = GetFileDataFromStorageByDocument(document);
                var fileName = document.FileName + "." + document.Extension;
                string p_message = "";
                string p_status = "";
                FTPServiceMod ftpService = new FTPServiceMod(fTPDetails.Host, fTPDetails.UserName, fTPDetails.Password);
                ftpService.Upload(fileName, fTPDetails.Folder, fileData, out p_message, out p_status, true, true);
            }

        }

        private  byte[] GetFileDataFromStorageByDocument(Simplog.Data.CommonDataModel.EntityPOCOs.Document document)
        {
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            BlobFileInfo fileInfo = new BlobFileInfo() { FileName = document.Id, FolderName = document.Folder, Tenant = document.Tenant, Extension = document.Extension };
            byte[] fileData = storageservice.Read(fileInfo);
            return fileData;
        }
    }
}