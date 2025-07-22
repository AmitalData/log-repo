using Logitude.Server.Tools.ExternalServices;
using Logitude.Server.Tools.Helpers;
using Logitude.SystemLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Dca
{
    class DCAMoveIncomeFileToDirService : IDisposable
    {
        private IDcaManagerShim _dca;

        public DCAMoveIncomeFileToDirService(IDcaManagerShim dca)
        {
            _dca = dca;
        }

        public void Dispose()
        {
            this._dca = null;
        }

        internal bool MoveItToDir(string SelectedFileDownload, string _AppendToDownloadFolderName,
            string MoveUnUseDCAFilesToDIr)
        {
            try
            {

                
                LogMessagingUtil.Instance.AppendLine("DCAMoveIncomeFileToDirService=" + SelectedFileDownload);
                
                
                
                bool errorOccurred = false; string MessageLog = "";
                string MoreParams = "";
                _dca.MoveIncomeFileToDir(SelectedFileDownload, SelectedFileDownload, _AppendToDownloadFolderName,MoveUnUseDCAFilesToDIr, ref MoreParams,
                    out errorOccurred, out MessageLog);
                if (errorOccurred)
                {
                    throw new Exception("_DcaManager.MoveItToDir errorOccurred !!! :MessageLog=" + MessageLog);

                }
                LogMessagingUtil.Instance.AppendLine("_DcaManager.MoveItToDir");
                return true;
            }
            catch (Exception e)
            {
                var selectedFileDownload = SelectedFileDownload ;
                ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole CustomsCommandSendDCAWR", "CustomsCommandSendDCAWR : DCAMoveIncomeFileToDirService(" + selectedFileDownload + ") ", null);
                return false;
            }

        }

    }
}
