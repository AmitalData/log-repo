using AmitalCloud.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using UnifreightIIG.Common.BLClient;
using UnifreightIIG.Common.Extensions;
using UnifreightIIG.Common.Faults;
using UnifreightIIG.Common.Utils;
using UnifreightIIG.DCA;
using ICSharpCode.SharpZipLib.BZip2;

namespace AmitalCloud.Infrastructure.APITools.ExternalServices
{
    public partial class DcaManager : ManagerProxyBase
    {
        public DcaManager(string myDCAServiceAddress, string partnerID, int tenant)
            : base(ErrorHandlerUtil.CreateNew())
        {
            var uri = new Uri(AmitalCloudSettings.LogitudeURL);
            var branchEnv = uri.LocalPath.Trim('/'); // Simplified trimming
            DCAServiceAddress = myDCAServiceAddress ?? throw new ArgumentNullException(nameof(myDCAServiceAddress));
            PartnerVaultID = partnerID ?? throw new ArgumentNullException(nameof(partnerID));
            Tenant = tenant;
            UnifreightEnvironmentID = $"{branchEnv}_{Tenant}";

            if (string.IsNullOrWhiteSpace(UnifreightEnvironmentID))
            {
                throw new ArgumentNullException(nameof(UnifreightEnvironmentID));
            }
        }

        public string GetState()
        {
            using (var myClient = GetDCAClient())
            {
                return myClient.GetChannel<IDCAService>().GetState();
            }
        }

        public void GetOutgoingQueueStatus(string ComAmitalID, string Filename, out string UnifreightQueueOutStatus, out string MessageOut)
        {
            UnifreightQueueOutStatus = MessageOut = string.Empty;
            string MessageLog = string.Empty;
            string MoreParams = "IIGServer=1";

            if (string.IsNullOrWhiteSpace(ComAmitalID))
            {
                throw new ArgumentNullException(nameof(ComAmitalID));
            }
            if (string.IsNullOrWhiteSpace(Filename))
            {
                throw new ArgumentNullException(nameof(Filename));
            }

            try
            {
                using (var myClient = GetDCAClient())
                {
                    myClient.GetChannel<IDCAService>().GetOutgoingQueueStatus(
                        UnifreightEnvironmentID, ComAmitalID, PartnerVaultID, Filename,
                        out UnifreightQueueOutStatus, ref MoreParams, out MessageLog);
                }
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"UnifreightQueueOutStatus={UnifreightQueueOutStatus}");
                MessageOut = MessageLog;
            }
            catch (Exception ex)
            {
                MessageLog = _ErrorHandler.ToFormattedMessage(ex);
            }
        }

        public string SubmitFileOutgoingQueue(string ComAmitalID, string fileContentsBASE64, string filename, out string MessageOut)
        {
            string ServerJobID = string.Empty;
            string MoreParams = "IIGServer=1";
            MessageOut = string.Empty;
            string MessageLog = string.Empty;

            if (string.IsNullOrWhiteSpace(ComAmitalID))
            {
                throw new ArgumentNullException(nameof(ComAmitalID));
            }
            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentNullException(nameof(filename));
            }

            try
            {
                var uriBuilder = new UriBuilder(AmitalCloudSettings.LogitudeURL)
                {
                    Path = "/CustomWebServices/DCAServerUploadStatusWebForm.Aspx"
                };
                MoreParams = $"IIGServerUrl={uriBuilder.Uri}";

                bool useChunk = true;
                if (useChunk)
                {
                    SubmitFileOutgoingQueueChunks(PartnerVaultID, fileContentsBASE64, filename, UnifreightEnvironmentID, ComAmitalID, out ServerJobID, ref MoreParams, out MessageLog);
                }
                else
                {
                    using (var myClient = GetDCAClient())
                    {
                        myClient.GetChannel<IDCAService>().SubmitFileOutgoingQueue(PartnerVaultID, fileContentsBASE64, filename, UnifreightEnvironmentID, ComAmitalID, out ServerJobID, ref MoreParams, out MessageLog);
                    }
                }
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"SubmitFileOutgoingQueue():ServerJobID={ServerJobID}");
                MessageOut = MessageLog;
            }
            catch (Exception ex)
            {
                MessageLog = _ErrorHandler.ToFormattedMessage(ex);
            }
            return ServerJobID;
        }

        public DateTime? LogitudeCalcDirLastChangeAt(string appendToDownloadFolderName, ref string MoreParams, out bool ErrorOccurred, out string MessageLog)
        {
            DateTime? LastDateTime = null;
            MessageLog = string.Empty;
            ErrorOccurred = true;
            string sErrorOccurred = string.Empty;

            try
            {
                using (var myClient = GetDCAClient())
                {
                    myClient.GetChannel<IDCAServiceLogitude>().LogitudeCalcDirLastChangeAt(PartnerVaultID, UnifreightEnvironmentID, appendToDownloadFolderName, out LastDateTime, out sErrorOccurred, ref MoreParams, out MessageLog);
                }
                if (sErrorOccurred.ToBoolAmitalFormart())
                {
                    ErrorOccurred = true;
                    return null;
                }
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"LastDateTime={LastDateTime}");
                ErrorOccurred = false;
            }
            catch (Exception ex)
            {
                ErrorOccurred = true;
                MessageLog += _ErrorHandler.ToFormattedMessage(ex);
            }
            return LastDateTime;
        }

        public List<string> FileListing(string SearchPattern, string appendToDownloadFolderName, ref string MoreParams, out bool ErrorOccurred, out string MessageLog)
        {
            MessageLog = string.Empty;
            ErrorOccurred = true;
            string sErrorOccurred = string.Empty;
            string listFileInLines = string.Empty;

            if (string.IsNullOrEmpty(SearchPattern))
            {
                throw new ArgumentNullException(nameof(SearchPattern));
            }
            if (SearchPattern.Equals("*"))
            {
                throw new ArgumentException("SearchPattern must be different than *");
            }

            try
            {
                using (var myClient = GetDCAClient())
                {
                    myClient.GetChannel<IDCAServiceLogitude>().FileListingLogitude(PartnerVaultID, UnifreightEnvironmentID, SearchPattern, appendToDownloadFolderName, out listFileInLines, out sErrorOccurred, ref MoreParams, out MessageLog);
                }
                if (sErrorOccurred.ToBoolAmitalFormart())
                {
                    ErrorOccurred = true;
                    return null;
                }
                var myList = listFileInLines.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"listFileInLines={myList.Count}");
                ErrorOccurred = false;
                return myList;
            }
            catch (Exception ex)
            {
                ErrorOccurred = true;
                MessageLog += _ErrorHandler.ToFormattedMessage(ex);
                return null;
            }
        }

        private ServiceWrapper<IDCAService> GetDCAClient()
        {
            return GenericFluentFactory<UnifreightIIG.DCA.UnifreightDCAFactory>
                .Init(new UnifreightIIG.DCA.UnifreightDCAFactory())
                .AddPropertyValue(x => x.UnifreightDCAServiceAddress, DCAServiceAddress)
                .AddPropertyValue(x => x.UnifreightDCAServiceTimeout, 300)
                .Get().CreateDCAClient();
        }
        public string GetContentsBASE64OfDownloadIncomeFile(string Filename, string appendToDownloadFolderName, ref string MoreParams, out bool errorOccurred, out string MessageLog)
        {
            string sErrorOccurred = string.Empty;
            string fileContentsBASE64 = string.Empty;
            string searchPattern = Filename;
            MessageLog = string.Empty;
            errorOccurred = true;

            if (string.IsNullOrWhiteSpace(UnifreightEnvironmentID))
            {
                throw new ArgumentNullException(nameof(UnifreightEnvironmentID));
            }
            if (string.IsNullOrWhiteSpace(PartnerVaultID))
            {
                throw new ArgumentNullException(nameof(PartnerVaultID));
            }
            if (string.IsNullOrWhiteSpace(Filename))
            {
                throw new ArgumentNullException(nameof(Filename));
            }
            if (Filename.Contains("*"))
            {
                throw new ArgumentException("Bad file name");
            }

            try
            {
                bool useChunk = true;
                if (useChunk)
                {
                    DownloadIncomeFileLogitudeChunks(PartnerVaultID, UnifreightEnvironmentID, searchPattern, appendToDownloadFolderName, true, out Filename, out fileContentsBASE64, out sErrorOccurred, ref MoreParams, out MessageLog);
                }
                else
                {
                    using (var myClient = GetDCAClient())
                    {
                        myClient.GetChannel<IDCAServiceLogitude>().DownloadIncomeFileLogitude(PartnerVaultID, UnifreightEnvironmentID, searchPattern, appendToDownloadFolderName, true, out Filename, out fileContentsBASE64, out sErrorOccurred, ref MoreParams, out MessageLog);
                    }
                }

                if (!sErrorOccurred.ToBoolAmitalFormart())
                {
                    if (!searchPattern.Equals(Filename, StringComparison.OrdinalIgnoreCase))
                    {
                        var mess = $"SearchPattern {searchPattern} <> Filename {Filename}";
                        AmitalCloudSettings.HandleLogMe(mess, true, "DCAError", DateTime.MaxValue);
                        throw new Exception(mess);
                    }
                }
                else
                {
                    throw new FaultException<UnifreightIIGFault>(UnifreightIIGFault.GetUnifreightValidation(MessageLog));
                }

                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["DCAZIPResponse"]))
                {
                    AmitalCloudSettings.HandleLogMe($"DCAZIPResponse!!!\nb4UnZip:fileContentsBASE64.Length={fileContentsBASE64.Length}", false, "DCA", DateTime.MaxValue);
                    fileContentsBASE64 = UnZipFileFromBase64StringAsBase64String(fileContentsBASE64);
                    AmitalCloudSettings.HandleLogMe($"AfterUnZip:fileContentsBASE64.Length={fileContentsBASE64.Length}", false, "DCA", DateTime.MaxValue);
                }
                else
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteDebug("SuppressDCAZIPResponse");
                }
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"fileContentsBASE64.Length={fileContentsBASE64.Length}");
                errorOccurred = false;
                return fileContentsBASE64;
            }
            catch (Exception ex)
            {
                errorOccurred = true;
                MessageLog = _ErrorHandler.ToFormattedMessage(ex);
                return null;
            }
        }


        public void MoveIncomeFileTorDir(
string Filename, string RenameFilename,
string appendToDownloadFolderName,
string MoveUnUseDCAFilesToDIr,
ref string MoreParams,
out bool errorOccurred, out string MessageLog)
        {

            string sErrorOccurred = "";
            MessageLog = "";
            errorOccurred = true;
            try
            {
                if (string.IsNullOrWhiteSpace(Filename))
                {
                    throw new ArgumentNullException("Filename");
                }
                if (Filename.IndexOf("*") >= 0)
                {
                    throw new ArgumentException("Bad file name");
                }
                using (var myClient = GetDCAClient())
                {
                    myClient.GetChannel<IDCAServiceLogitude>()
                        .MoveFileToDir(PartnerVaultID, this.UnifreightEnvironmentID, Filename, MoveUnUseDCAFilesToDIr, appendToDownloadFolderName,
                        out sErrorOccurred,
                        ref MoreParams, out MessageLog);
                }
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("RenameIncomeFile=" + (!sErrorOccurred.ToBoolAmitalFormart()).ToString());
                if (sErrorOccurred.ToBoolAmitalFormart())
                {
                    throw new FaultException<UnifreightIIGFault>(UnifreightIIGFault.GetUnifreightValidation(MessageLog));
                }
                errorOccurred = false;
            }
            catch (System.Exception ex)
            {
                errorOccurred = true;
                MessageLog = _ErrorHandler.ToFormattedMessage(ex);
            }
        }
        public void RenameIncomeFile(string Filename, string RenameFilename, string appendToDownloadFolderName,
        ref string MoreParams, out bool errorOccurred, out string MessageLog)
        {
            string sErrorOccurred = "";
            MessageLog = "";
            errorOccurred = true;
            try
            {
                if (string.IsNullOrWhiteSpace(Filename))
                {
                    throw new ArgumentNullException("Filename");
                }
                if (Filename.IndexOf("*") >= 0)
                {
                    throw new ArgumentException("Bad file name");
                }
                using (var myClient = GetDCAClient())
                {
                    myClient.GetChannel<IDCAServiceLogitude>()
                        .RenameIncomeFile(PartnerVaultID, this.UnifreightEnvironmentID, Filename, RenameFilename, appendToDownloadFolderName,
                        out sErrorOccurred,
                        ref MoreParams, out MessageLog);
                }
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("RenameIncomeFile=" + (!sErrorOccurred.ToBoolAmitalFormart()).ToString());
                if (sErrorOccurred.ToBoolAmitalFormart())
                {
                    throw new FaultException<UnifreightIIGFault>(UnifreightIIGFault.GetUnifreightValidation(MessageLog));
                }
                errorOccurred = false;
            }
            catch (System.Exception ex)
            {
                errorOccurred = true;
                MessageLog = _ErrorHandler.ToFormattedMessage(ex);
            }
        }
        public void DeleteIncomeFile(
            string Filename,
            string appendToDownloadFolderName,
            ref string MoreParams,
            out bool errorOccurred, out string MessageLog)
        {
            string sErrorOccurred = "";
            MessageLog = "";
            errorOccurred = true;
            try
            {
                if (string.IsNullOrWhiteSpace(Filename))
                {
                    throw new ArgumentNullException("Filename");
                }
                if (Filename.IndexOf("*") >= 0)
                {
                    throw new ArgumentException("Bad file name");
                }
                using (var myClient = GetDCAClient())
                {
                    myClient.GetChannel<IDCAServiceLogitude>()
                        .DeleteIncomeFileLogitude(PartnerVaultID, this.UnifreightEnvironmentID, Filename, appendToDownloadFolderName,
                        out sErrorOccurred,
                        ref MoreParams, out MessageLog);
                }
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("DeleteIncomeFile=" + (!sErrorOccurred.ToBoolAmitalFormart()).ToString());
                if (sErrorOccurred.ToBoolAmitalFormart())
                {
                    throw new FaultException<UnifreightIIGFault>(UnifreightIIGFault.GetUnifreightValidation(MessageLog));
                }
                errorOccurred = false;
            }
            catch (System.Exception ex)
            {
                errorOccurred = true;
                MessageLog = _ErrorHandler.ToFormattedMessage(ex);
            }
        }
        public static string GetDCAOutFileName(string p_ServiceName, string p_ExternalId, bool p_taskyam, DCAParams p_EnviorentParams)
        {
            if (String.IsNullOrWhiteSpace(p_EnviorentParams.Sufix))
            {
                throw new Exception(" p_EnviorentParams.Sufix is null ");
            }
            string v_customer_list = "";
            v_customer_list = "IL" + p_EnviorentParams.consumerId;
            if (p_taskyam) v_customer_list += ".IL" + p_EnviorentParams.mehesCustomerId;
            if (!String.IsNullOrWhiteSpace(p_EnviorentParams.consumerId2)) v_customer_list += ".IL" + p_EnviorentParams.consumerId2;
            if (!String.IsNullOrWhiteSpace(p_EnviorentParams.consumerId3)) v_customer_list += ".IL" + p_EnviorentParams.consumerId3;
            if (!String.IsNullOrWhiteSpace(p_EnviorentParams.consumerId4)) v_customer_list += ".IL" + p_EnviorentParams.consumerId4;
            string v_env = "";// p_EnviorentParams.env_id;
            v_env = p_EnviorentParams.Sufix;
            return string.Format("{0}{1}.{2}.{3}.{4}{5}",
                             p_ServiceName, "_In",
                             v_customer_list,
                             DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"),
                             p_ExternalId, v_env);
        }
        void UnifaceRequest(string MessageID, string MessageXML, out string ResponseXML, out string ErrorOccurred, ref string MoreParams, out string ErrorMess)
        {
            throw new NotImplementedException();
        }
        public string DCAServiceAddress { get; private set; }
        public int Tenant { get; private set; }
        public static Stream Decompress(Stream compressed)
        {
            var decompressed = new MemoryStream();
            using (var zip = new GZipStream(compressed, CompressionMode.Decompress, true))
            {
                zip.CopyTo(decompressed);
            }
            decompressed.Seek(0, SeekOrigin.Begin);
            return decompressed;
        }
        public static bool IsBase64String(string s)
        {
            s = s.Trim();
            return (s.Length % 4 == 0) && Regex.IsMatch(s, @"^[a-zA-Z0-9\+/]*={0,3}$");
        }

        string UnZipFileFromBase64StringAsBase64String(string fileAsBase64String)
        {
            using (var m_MyMemoryStream = new MemoryStream(Convert.FromBase64String(fileAsBase64String)))
            using (var MyZipInputStream = new BZip2InputStream(m_MyMemoryStream))
            using (var m_MyMemoryStream2 = new MemoryStream())
            {
                MyZipInputStream.CopyTo(m_MyMemoryStream2);
                return Convert.ToBase64String(m_MyMemoryStream2.ToArray());
            }
        }
        public string PartnerVaultID { get; private set; }
        public string UnifreightEnvironmentID { get; private set; }
    }
}
