using Logitude.Server.Tools;
using Logitude.Server.Tools.FTP;
using Logitude.Server.Tools.Helpers;
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
                byte[] fileData = StorageDataService.ReadFileFromStorage(new StorageDataArgs() { FileName = document.Id, Extension = document.Extension, FolderName = document.Folder,  Tenant = document.Tenant });
                var fileName = document.FileName + "." + document.Extension;
                string p_message = "";
                string p_status = "";
                FTPServiceMod ftpService = new FTPServiceMod(fTPDetails.Host, fTPDetails.UserName, fTPDetails.Password);
                ftpService.Upload(fileName, fTPDetails.Folder, fileData, out p_message, out p_status, true, true);
            }

        }
    }
}