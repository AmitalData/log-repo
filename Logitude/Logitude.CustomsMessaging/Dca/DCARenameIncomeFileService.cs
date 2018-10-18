using Logitude.Server.Tools.Helpers;
using Logitude.SystemLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logitude.CustomsMessaging.Dca
{
    class DCARenameIncomeFileService : IDisposable
    {
        private Server.Tools.ExternalServices.DcaManager _DcaManager;

        public DCARenameIncomeFileService(Server.Tools.ExternalServices.DcaManager _DcaManager)
        {
            // TODO: Complete member initialization
            this._DcaManager = _DcaManager;
        }

        public void Dispose()
        {
            this._DcaManager = null;
        }

        internal bool RenameIt(Customs.Def.EntityPMs.InterfaceTenantDefinitionManagementPM messageDCA, Customs.BL.Utils.DCAFileModel dcaFile, string _AppendToDownloadFolderName)
        {
            try
            {

                //1.      לטבלת ניהול מסרים יש להוסיף סימון "בדיקת קובץ יזום" ושדה להזנת קידומת לקובץ המוחזר.
                string renameFilePrefix = //messageDCA.InterfaceManagement.RenameFilePrefixFake;
                    messageDCA.DcaRenameFilePrefix;
                
                var test = false;
                if (test)
                {
                    renameFilePrefix = "itzikTest." + messageDCA.InterfaceManagement.DcaPrefixName;
                }
                if (String.IsNullOrWhiteSpace(renameFilePrefix))
                {
                    return false;
                }
                LogMessagingUtil.Instance.AppendLine("DcaRenameFilePrefix=" + renameFilePrefix);
                var SelectedFileDownload=dcaFile.SelectedFileDownload ;
                
                
                string newFileName = null;
                newFileName = newFileName ?? GetName(messageDCA.InterfaceManagement.DcaPrefixName, renameFilePrefix, SelectedFileDownload);
                newFileName = newFileName ?? GetName(messageDCA.InterfaceManagement.DcaPrefixName2, renameFilePrefix, SelectedFileDownload);
                newFileName = newFileName ?? GetName(messageDCA.InterfaceManagement.DcaPrefixName3, renameFilePrefix, SelectedFileDownload);
                newFileName = newFileName ?? GetName(messageDCA.InterfaceManagement.DcaPrefixName4, renameFilePrefix, SelectedFileDownload);
                if (String.IsNullOrWhiteSpace(newFileName))
                {

                    return false;
                }

                LogMessagingUtil.Instance.AppendLine("newFileName=" + newFileName);
                bool errorOccurred=false;string MessageLog="";
                string MoreParams = "";
                _DcaManager.RenameIncomeFile(dcaFile.SelectedFileDownload, newFileName, _AppendToDownloadFolderName, ref MoreParams,
                    out errorOccurred, out MessageLog);
                if (errorOccurred)
                {
                    throw new Exception("_DcaManager.RenameIncomeFile errorOccurred !!! :MessageLog=" + MessageLog);
                    
                }
                LogMessagingUtil.Instance.AppendLine("_DcaManager.RenameIncomeFile");
                return true;
            }
            catch (Exception  e)
            {
                var selectedFileDownload = dcaFile != null ? dcaFile.SelectedFileDownload : "SelectedFileDownload";
                ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole CustomsCommandSendDCAWR", "CustomsCommandSendDCAWR : RenameIt(" + selectedFileDownload + ") ", null);
                return false;
            }
            
        }

        private string GetName(string currPrefixName, string newPrefix, string SelectedFileDownload)
        {
            if (!string.IsNullOrWhiteSpace(currPrefixName))
            {
                if (SelectedFileDownload.StartsWith(currPrefixName))
                {

                    return SelectedFileDownload.Replace(currPrefixName, newPrefix);
                }
            }
            return null;
        }
    }
}
