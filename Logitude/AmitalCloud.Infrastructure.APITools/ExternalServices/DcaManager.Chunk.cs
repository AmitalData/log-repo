using Amital.UpDown.Common;
using Amital.UpDown.Common.Client;
using Amital.UpDown.Common.ModelShared.DCA;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.APITools.ExternalServices
{
    public partial class DcaManager
    {

        private void SubmitFileOutgoingQueueChunks(string PartnerVault, string FileContentsBASE64, string Filename, string Unique_Environment_ID, string ComAmitalID, out string ServerJobID, ref string MoreParams, out string MessageOut)
        {

            int sendTimeoutInSec = 30;
            var chunkSizeInKB = 1024;
            var bytsArry = Convert.FromBase64String(FileContentsBASE64);
            SubmitFileOutgoingQueueResponseM_V1 res = null;
            var client = new UploadClient<SubmitFileOutgoingQueueRequestM_V1, SubmitFileOutgoingQueueResponseM_V1>();
            try
            {

                var sync = true;
                if (sync)
                {
                    res = client.Upload(this.DCAServiceAddress, sendTimeoutInSec, new SubmitFileOutgoingQueueRequestM_V1()
                    {
                        ComAmitalID = ComAmitalID,
                        FileName = Filename,
                        MoreParams = MoreParams,
                        PartnerVaultID = this.PartnerVaultID,
                        UnifreightEnvironmentID = this.UnifreightEnvironmentID

                    }, bytsArry, chunkSizeInKB);
                }
                else
                {
                    var task = client.UploadAsync(
                    this.DCAServiceAddress , sendTimeoutInSec, new SubmitFileOutgoingQueueRequestM_V1()
                    {
                        ComAmitalID = ComAmitalID,
                        FileName = Filename,
                        MoreParams = MoreParams,
                        PartnerVaultID = this.PartnerVaultID,
                        UnifreightEnvironmentID = this.UnifreightEnvironmentID

                    }, bytsArry, chunkSizeInKB);
                    Wait4Finsh("SubmitFileOutgoingQueueChunks", task, 6);
                    res = task.Result;
                }
                MessageOut = res.MessageLog;
                ServerJobID = res.ServerJobID;
                MoreParams = res.MoreParams;
            }
            catch (AggregateException ae)
            {
                throw ae.Flatten();
            }
        }
        private void DownloadIncomeFileLogitudeChunks(string PartnerVault, string Unique_Environment_ID, string SearchPattern, string appendToDownloadFolderName, bool useUTF8, out string Filename, out string fileContentsBASE64, out string sErrorOccurred, ref string MoreParams, out string MessageLog)
        {
            int sendTimeoutInSec = 30;
            var chunkSizeInKB = 1024;//0.25MB
            sendTimeoutInSec = 240;
            fileContentsBASE64 = "";
            //var bytsArry = Convert.FromBase64String(fileContentsBASE64);
            DownloadServiceResponseData resDownloadServiceResponseData = null;
            try
            {
                var client = new DownloadClient<DownloadIncomeFileLogitudeRequestM_V1, DownloadIncomeFileLogitudeResponseM_V1>();
                var sync = true;
                if (sync)
                {
                    resDownloadServiceResponseData = client.Download(
                    this.DCAServiceAddress
                    , new DownloadIncomeFileLogitudeRequestM_V1()
                    {
                        PartnerVault = PartnerVaultID,
                        appendToDownloadFolderName = appendToDownloadFolderName,
                        SearchPattern = SearchPattern,
                        Unique_Environment_ID = Unique_Environment_ID,
                        useUTF8 = useUTF8
                    }, sendTimeoutInSec, chunkSizeInKB);
                }
                else
                {
                    Task<DownloadServiceResponseData> task =
                    client.DownloadAsync(this.DCAServiceAddress, new DownloadIncomeFileLogitudeRequestM_V1()
                    {
                        PartnerVault = PartnerVaultID,
                        appendToDownloadFolderName = appendToDownloadFolderName,
                        SearchPattern = SearchPattern,
                        Unique_Environment_ID = Unique_Environment_ID,
                        useUTF8 = useUTF8
                    }, sendTimeoutInSec, chunkSizeInKB);
                    Wait4Finsh(SearchPattern, task, 6);
                    if (task.Exception != null)
                    {
                        throw task.Exception;
                    }
                    resDownloadServiceResponseData = task.Result;
                }
                if (resDownloadServiceResponseData == null)
                {
                    throw new Exception("Result is null");
                }
                switch (resDownloadServiceResponseData.ParamOutType)
                {
                    case "DownloadIncomeFileLogitudeResponseM_V1":
                        var DownloadIncomeFileLogitudeResponseM_V1 = XmlGenericUtil<DownloadIncomeFileLogitudeResponseM_V1>.DeSerializeObject(resDownloadServiceResponseData.ParamOutXml);
                        Filename = DownloadIncomeFileLogitudeResponseM_V1.Filename;
                        sErrorOccurred = DownloadIncomeFileLogitudeResponseM_V1.ErrorOccurred;
                        MessageLog = DownloadIncomeFileLogitudeResponseM_V1.MessageOut;
                        MoreParams = DownloadIncomeFileLogitudeResponseM_V1.MoreParams;
                        break;
                    default:
                        throw new Exception("ParamOutType available DownloadIncomeFileLogitudeResponseM_V1");
                        break;
                }
                if (resDownloadServiceResponseData.AllDataCalcOnClient != null)
                {
                    fileContentsBASE64 = Convert.ToBase64String(resDownloadServiceResponseData.AllDataCalcOnClient);
                }
            }
            catch (AggregateException ae)
            {
                throw ae.Flatten();
            }
        }
        private static void Wait4Finsh(string ExeptionData, Task task, int TimeOutInMin)
        {
            var ts = Stopwatch.StartNew();
            while (ts.Elapsed < TimeSpan.FromMinutes(TimeOutInMin))
            {
                task.Wait(TimeSpan.FromSeconds(1));
                if (task.IsCompleted)
                {
                    break;
                }
                else
                {
                    continue;
                }
            }
            if (!task.IsCompleted)
            {
                task.Dispose();
                throw new Exception("Timeout DownloadIncomeFileLogitudeAsync 6Min " + ExeptionData);
            }
        }
    }
}
