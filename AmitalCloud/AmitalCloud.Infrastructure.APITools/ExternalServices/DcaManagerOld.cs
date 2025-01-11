
//using AmitalCloud.Infrastructure.Data;
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.IO;
//using System.IO.Compression;
//using System.Linq;
//using System.ServiceModel;
//using System.Text;
//using System.Text.RegularExpressions;
//using UnifreightIIG.Common.BLClient;
//using UnifreightIIG.Common.Extensions;
//using UnifreightIIG.Common.Faults;
//using UnifreightIIG.Common.Utils;
//using UnifreightIIG.DCA;
//using ICSharpCode.SharpZipLib.BZip2;

//namespace AmitalCloud.Infrastructure.APITools.ExternalServices
//{
//    public partial class DcaManagerOld
//       : ManagerProxyBase
//    {
//        public DcaManagerOld(string myDCAServiceAddress, string partnerID, int tenant)
//            : base(ErrorHandlerUtil.CreateNew())
//        {
//            var uri = new Uri(AmitalCloudSettings.LogitudeURL);
//            var branchEnv = uri.LocalPath.Trim(@"\"[0]).Trim(@"/"[0]);
//            DCAServiceAddress = @"http://xxx-7-new:5050/Unifreight/DCAService/basic";
//            DCAServiceAddress = myDCAServiceAddress;
//            PartnerVaultID = partnerID;
//            Tenant = tenant;
//            UnifreightEnvironmentID =
//                branchEnv + "_" + Tenant.ToString();
//            if (String.IsNullOrWhiteSpace(this.DCAServiceAddress))
//            {
//                throw new ArgumentNullException("this.DCAServiceAddress");
//            }
//            if (String.IsNullOrWhiteSpace(this.UnifreightEnvironmentID))
//            {
//                throw new ArgumentNullException("this.UnifreightEnvironmentID");
//            }
//            if (string.IsNullOrWhiteSpace(PartnerVaultID))
//            {
//                throw new ArgumentNullException("PartnerVaultID");
//            }
//        }
//        public string GetState()
//        {
//            var state = "";
//            using (var myClient = GetDCAClient()) 
//            {
//                state = myClient.GetChannel<IDCAService>().GetState();
//            }
//            return state;
//        }
//        public void GetOutgoingQueueStatus(
//            string ComAmitalID, string Filename,
//            out string UnifreightQueueOutStatus, out string MessageOut)
//        {
//            UnifreightQueueOutStatus = MessageOut = "";
//            string MessageLog = "";
//            string MoreParams = "";
//            try
//            {
//                if (String.IsNullOrWhiteSpace(ComAmitalID))
//                {
//                    throw new ArgumentNullException("ComAmitalID");
//                }
//                if (string.IsNullOrWhiteSpace(Filename))
//                {
//                    throw new ArgumentNullException("Filename");
//                }
//                MoreParams = "IIGServer=1";
//                using (var myClient = GetDCAClient()) 
//                {
//                    myClient.GetChannel<IDCAService>().GetOutgoingQueueStatus(
//                        this.UnifreightEnvironmentID, ComAmitalID,
//                        this.PartnerVaultID, Filename,
//                        out UnifreightQueueOutStatus,
//                        ref MoreParams, out MessageLog);
//                }
//               NetCommonHelper.Logger.DevLog.Instance.WriteDebug("UnifreightQueueOutStatus=" + UnifreightQueueOutStatus);
//                MessageOut = MessageLog;

//            }
//            catch (System.Exception ex)
//            {
//                MessageLog = _ErrorHandler.ToFormattedMessage(ex);
//            }
//        }
//        public string SubmitFileOutgoingQueue(
//            string ComAmitalID,
//            string fileContentsBASE64, string filename,
//            out string MessageOut)
//        {
//            string ServerJobID;
//            string MoreParams;
//            ServerJobID = MessageOut = "";
//            string MessageLog = "";
//            try
//            {
//                if (String.IsNullOrWhiteSpace(ComAmitalID))
//                {
//                    throw new ArgumentNullException("ComAmitalID");
//                }
//                if (string.IsNullOrWhiteSpace(filename))
//                {
//                    throw new ArgumentNullException("Filename");
//                }
//                MoreParams = "IIGServer=1";
//                System.UriBuilder uriBuilder = new System.UriBuilder(AmitalCloudSettings.LogitudeURL);
//                uriBuilder.Path += @"/CustomWebServices/DCAServerUploadStatusWebForm.Aspx";
//                MoreParams = "IIGServerUrl=" + uriBuilder.Uri.ToString(); ;
//                bool useChunk = true;
//                if (useChunk)
//                {
//                    this.SubmitFileOutgoingQueueChunks(PartnerVaultID,
//                            fileContentsBASE64, filename,
//                            this.UnifreightEnvironmentID, ComAmitalID,
//                            out ServerJobID,
//                            ref MoreParams, out MessageLog);
//                }
//                else
//                {
//                    using (var myClient = GetDCAClient())
//                    {
//                        myClient.GetChannel<IDCAService>().SubmitFileOutgoingQueue(
//                            PartnerVaultID,
//                            fileContentsBASE64, filename,
//                            this.UnifreightEnvironmentID, ComAmitalID,
//                            out ServerJobID,
//                            ref MoreParams, out MessageLog);
//                    }
//                }
//               NetCommonHelper.Logger.DevLog.Instance.WriteDebug("SubmitFileOutgoingQueue():ServerJobID=" + ServerJobID);
//                MessageOut = MessageLog;
//            }
//            catch (System.Exception ex)
//            {
//                MessageLog = _ErrorHandler.ToFormattedMessage(ex);
//            }
//            return ServerJobID;
//        }
//        public DateTime? LogitudeCalcDirLastChangeAt(
//        string appendToDownloadFolderName, ref string MoreParams, out bool ErrorOccurred, out string MessageLog)
//        {
//            DateTime? LastDateTime = null;
//            MessageLog = "";
//            ErrorOccurred = true;
//            string sErrorOccurred = "";
//            string listFileInLines = "";
//            try
//            {
//                using (var myClient = GetDCAClient())
//                {
//                    myClient.GetChannel<IDCAServiceLogitude>().LogitudeCalcDirLastChangeAt(PartnerVaultID, UnifreightEnvironmentID, appendToDownloadFolderName,
//                        out LastDateTime, out sErrorOccurred, ref MoreParams, out MessageLog);
//                }
//                if (sErrorOccurred.ToBoolAmitalFormart())
//                {
//                    ErrorOccurred = true;
//                    return null;
//                }
//               NetCommonHelper.Logger.DevLog.Instance.WriteDebug("LastDateTime =" + LastDateTime.ToString());
//                ErrorOccurred = false;
//            }
//            catch (System.Exception ex)
//            {
//                ErrorOccurred = true;
//                MessageLog += _ErrorHandler.ToFormattedMessage(ex);
//           }
//            return LastDateTime;
//        }
//        public List<string> FileListing(
//            string SearchPattern, string appendToDownloadFolderName, ref string MoreParams, out bool ErrorOccurred, out string MessageLog)
//        {
//            MessageLog = "";
//            ErrorOccurred = true;
//            string sErrorOccurred = "";
//            string listFileInLines = "";
//            try
//            {
//                if (string.IsNullOrEmpty(SearchPattern))
//                {
//                    throw new ArgumentNullException("SearchPattern");
//                }
//                if (SearchPattern.Equals("*"))
//                {
//                    throw new ArgumentException("SearchPattern must be different than *");
//                }
//                using (var myClient = GetDCAClient())
//                {
//                    myClient.GetChannel<IDCAServiceLogitude>().FileListingLogitude(PartnerVaultID, UnifreightEnvironmentID, SearchPattern,
//                      appendToDownloadFolderName, out listFileInLines, out sErrorOccurred, ref MoreParams, out MessageLog);
//                }
//                if (sErrorOccurred.ToBoolAmitalFormart())
//                {
//                    ErrorOccurred = true;
//                    return null;
//                }
//                var myList = listFileInLines
//                    .Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
//                    .ToList();
//               NetCommonHelper.Logger.DevLog.Instance.WriteDebug("listFileInLines=" + myList.Count());
//                ErrorOccurred = false;
//                return myList;
//            }
//            catch (System.Exception ex)
//            {
//                ErrorOccurred = true;
//                MessageLog += _ErrorHandler.ToFormattedMessage(ex);
//                return null;
//            }
//        }
//        private ServiceWrapper<IDCAService> GetDCAClient()
//        {
//            return GenericFluentFactory<UnifreightIIG.DCA.UnifreightDCAFactory>
//                                .Init(new UnifreightIIG.DCA.UnifreightDCAFactory())
//                 .AddPropertyValue(x => x.UnifreightDCAServiceAddress, this.DCAServiceAddress)
//                 .AddPropertyValue(x => x.UnifreightDCAServiceTimeout, 300)
//                 .Get().CreateDCAClient();
//        }
//        public string GetContentsBASE64OfDownloadIncomeFile(
//            string Filename,
//            string appendToDownloadFolderName,
//            ref string MoreParams,
//            out bool errorOccurred, out string MessageLog)
//        {
//            string sErrorOccurred = "";
//            string fileContentsBASE64 = "";
//            string searchPattern;
//            MessageLog = "";
//            errorOccurred = true;
//            try
//            {
//                if (String.IsNullOrWhiteSpace(this.UnifreightEnvironmentID))
//                {
//                    throw new ArgumentNullException("this.UnifreightEnvironmentID");
//                }
//                if (string.IsNullOrWhiteSpace(PartnerVaultID))
//                {
//                    throw new ArgumentNullException("PartnerVaultID");
//                }
//                if (string.IsNullOrWhiteSpace(Filename))
//                {
//                    throw new ArgumentNullException("Filename");
//                }
//                if (Filename.IndexOf("*") >= 0)
//                {
//                    throw new ArgumentException("Bad file name");
//                }
//                searchPattern = Filename;
//#if (useCommpressStream)
//                {
//                    Stream compressStream = null;
//                    using (var myClient = GetDCAClient()) // GetDCAClient()) //new UnifreightSdkDCA())
//                    {

//                        //IPhysicalCheck myPC = myUnifreightSdkGateway.Channel;
//                        //myUnifreightSdkGateway.Channel.PhysicalCheckChangingTime(
//                        DownloadIncomeCompressLogitudeResult res=
//                        myClient.GetChannel<IDCAServiceLogitude>()
//                            .DownloadIncomeCompressLogitude(PartnerVaultID, UnifreightEnvironmentID, searchPattern, appendToDownloadFolderName, true,
//                            MoreParams);
//                        compressStream = res.compressStream;
//                    }
//                      if (compressStream!=null)
//                    {

//                        var decompressStream = Decompress(compressStream);

//                        using (StreamReader reader = new StreamReader(decompressStream))
//                        {

//                            byte[] bytedata = System.Text.UTF8Encoding.Default.GetBytes(reader.ReadToEnd());

//                            fileContentsBASE64 = Convert.ToBase64String(bytedata);
//                        }
//                    }
//                }
//#endif
//                {
//                    bool useChunk = true;
//                    if (useChunk)
//                    {
//                        this.DownloadIncomeFileLogitudeChunks(
//                            PartnerVaultID, UnifreightEnvironmentID, searchPattern, appendToDownloadFolderName, true,
//                                 out Filename, out fileContentsBASE64,
//                                out sErrorOccurred, ref MoreParams, out MessageLog);
//                    }
//                    else
//                    {
//                        using (var myClient = GetDCAClient()) 
//                        {
//                            myClient.GetChannel<IDCAServiceLogitude>()
//                                .DownloadIncomeFileLogitude(PartnerVaultID, UnifreightEnvironmentID, searchPattern, appendToDownloadFolderName, true,
//                                 out Filename, out fileContentsBASE64,
//                                out sErrorOccurred, ref MoreParams, out MessageLog);
//                        }
//                    }
//                }

//                if (!sErrorOccurred.ToBoolAmitalFormart())
//                {
//                    if (!searchPattern.Equals(Filename, StringComparison.OrdinalIgnoreCase))
//                    {
//                        var mess = $"SearchPattern {searchPattern} <> Filename {Filename}";
//                        AmitalCloudSettings.HandleLogMe(mess, true, "DCAError", DateTime.MaxValue);
//                        throw new Exception(mess);
//                    }
//                }
//                else
//                {
//                    throw new FaultException<UnifreightIIGFault>(UnifreightIIGFault.GetUnifreightValidation(MessageLog));
//                }
//                if (!String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["DCAZIPResponse"]))
//                {
//                    AmitalCloudSettings.HandleLogMe(@"DCAZIPResponse!!!
//b4UnZip:fileContentsBASE64.Length" + fileContentsBASE64.Length, false, "DCA", DateTime.MaxValue);
//                    fileContentsBASE64 = UnZipFileFromBase64StringAsBase64String(fileContentsBASE64);
//                    AmitalCloudSettings.HandleLogMe("AfterUnZip:fileContentsBASE64.Length" + fileContentsBASE64.Length, false, "DCA", DateTime.MaxValue);
//                }
//                else
//                {
//                   NetCommonHelper.Logger.DevLog.Instance.WriteDebug("SuppressDCAZIPResponse");
//                }
//               NetCommonHelper.Logger.DevLog.Instance.WriteDebug("fileContentsBASE64.Length=" + fileContentsBASE64.Length);
//                errorOccurred = false;
//                return fileContentsBASE64;
//            }
//            catch (System.Exception ex)
//            {
//                errorOccurred = true;
//                MessageLog = _ErrorHandler.ToFormattedMessage(ex);
//                return null;
//            }
//        }
//        public static bool IsBase64String(string s)
//        {
//            s = s.Trim();
//            return (s.Length % 4 == 0) && Regex.IsMatch(s, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
//        }
//        string UnZipFileFromBase64StringAsBase64String(string fileAsBase64String)
//        {
//            MemoryStream m_MyMemoryStream = null;
//            MemoryStream m_MyMemoryStream2 = new MemoryStream();
//            BZip2InputStream MyZipInputStream = null;
//            string m_Dataout = "";
//            try
//            {
//                m_MyMemoryStream = new MemoryStream(Convert.FromBase64String(fileAsBase64String));
//                MyZipInputStream = new BZip2InputStream(m_MyMemoryStream);
//                StringBuilder MyUncompressMessage = new StringBuilder();
//                int MySize = 1024;
//                byte[] BytesUncompressed = new byte[MySize];
//                while (true)
//                {
//                    MySize = MyZipInputStream.Read(BytesUncompressed, 0, MySize);
//                    if (MySize > 0)
//                        m_MyMemoryStream2.Write(BytesUncompressed, 0, MySize);
//                    else
//                        break;
//                }
//                m_MyMemoryStream2.Position = 0;
//                m_Dataout = Convert.ToBase64String(ReadFully(m_MyMemoryStream2));
//                BytesUncompressed = null;
//                MyUncompressMessage.Remove(0, MyUncompressMessage.Length);
//                return (m_Dataout);
//            }
//            catch
//            {
//                throw;
//            }

//        }
//        static byte[] ReadFully(Stream input)
//        {
//            if (input == null)
//                return (null);
//            input.Position = 0;
//            using (MemoryStream ms = new MemoryStream())
//            {
//                input.CopyTo(ms);
//                return ms.ToArray();
//            }
//        }
//        public void MoveIncomeFileTorDir(
//        string Filename, string RenameFilename,
//        string appendToDownloadFolderName,
//        string MoveUnUseDCAFilesToDIr,
//        ref string MoreParams,
//        out bool errorOccurred, out string MessageLog)
//        {

//            string sErrorOccurred = "";
//            MessageLog = "";
//            errorOccurred = true;
//            try
//            {
//                if (string.IsNullOrWhiteSpace(Filename))
//                {
//                    throw new ArgumentNullException("Filename");
//                }
//                if (Filename.IndexOf("*") >= 0)
//                {
//                    throw new ArgumentException("Bad file name");
//                }
//                using (var myClient = GetDCAClient())
//                {
//                    myClient.GetChannel<IDCAServiceLogitude>()
//                        .MoveFileToDir(PartnerVaultID, this.UnifreightEnvironmentID, Filename, MoveUnUseDCAFilesToDIr, appendToDownloadFolderName,
//                        out sErrorOccurred,
//                        ref MoreParams, out MessageLog);
//                }
//               NetCommonHelper.Logger.DevLog.Instance.WriteDebug("RenameIncomeFile=" + (!sErrorOccurred.ToBoolAmitalFormart()).ToString());
//                if (sErrorOccurred.ToBoolAmitalFormart())
//                {
//                    throw new FaultException<UnifreightIIGFault>(UnifreightIIGFault.GetUnifreightValidation(MessageLog));
//                }
//                errorOccurred = false;
//            }
//            catch (System.Exception ex)
//            {
//                errorOccurred = true;
//                MessageLog = _ErrorHandler.ToFormattedMessage(ex);
//            }
//        }
//        public void RenameIncomeFile( string Filename, string RenameFilename,string appendToDownloadFolderName,
//        ref string MoreParams, out bool errorOccurred, out string MessageLog)
//        {
//            string sErrorOccurred = "";
//            MessageLog = "";
//            errorOccurred = true;
//            try
//            {
//                if (string.IsNullOrWhiteSpace(Filename))
//                {
//                    throw new ArgumentNullException("Filename");
//                }
//                if (Filename.IndexOf("*") >= 0)
//                {
//                    throw new ArgumentException("Bad file name");
//                }
//                using (var myClient = GetDCAClient())
//                {
//                    myClient.GetChannel<IDCAServiceLogitude>()
//                        .RenameIncomeFile(PartnerVaultID, this.UnifreightEnvironmentID, Filename, RenameFilename, appendToDownloadFolderName,
//                        out sErrorOccurred,
//                        ref MoreParams, out MessageLog);
//                }
//               NetCommonHelper.Logger.DevLog.Instance.WriteDebug("RenameIncomeFile=" + (!sErrorOccurred.ToBoolAmitalFormart()).ToString());
//                if (sErrorOccurred.ToBoolAmitalFormart())
//                {
//                    throw new FaultException<UnifreightIIGFault>(UnifreightIIGFault.GetUnifreightValidation(MessageLog));
//                }
//                errorOccurred = false;
//            }
//            catch (System.Exception ex)
//            {
//                errorOccurred = true;
//                MessageLog = _ErrorHandler.ToFormattedMessage(ex);
//            }
//        }
//        public void DeleteIncomeFile(
//            string Filename,
//            string appendToDownloadFolderName,
//            ref string MoreParams,
//            out bool errorOccurred, out string MessageLog)
//        {
//            string sErrorOccurred = "";
//            MessageLog = "";
//            errorOccurred = true;
//            try
//            {
//                if (string.IsNullOrWhiteSpace(Filename))
//                {
//                    throw new ArgumentNullException("Filename");
//                }
//                if (Filename.IndexOf("*") >= 0)
//                {
//                    throw new ArgumentException("Bad file name");
//                }
//                using (var myClient = GetDCAClient())
//                {
//                    myClient.GetChannel<IDCAServiceLogitude>()
//                        .DeleteIncomeFileLogitude(PartnerVaultID, this.UnifreightEnvironmentID, Filename, appendToDownloadFolderName,
//                        out sErrorOccurred,
//                        ref MoreParams, out MessageLog);
//                }
//               NetCommonHelper.Logger.DevLog.Instance.WriteDebug("DeleteIncomeFile=" + (!sErrorOccurred.ToBoolAmitalFormart()).ToString());
//                if (sErrorOccurred.ToBoolAmitalFormart())
//                {
//                    throw new FaultException<UnifreightIIGFault>(UnifreightIIGFault.GetUnifreightValidation(MessageLog));
//                }
//                errorOccurred = false;
//            }
//            catch (System.Exception ex)
//            {
//                errorOccurred = true;
//                MessageLog = _ErrorHandler.ToFormattedMessage(ex);
//            }
//        }
//        public static string GetDCAOutFileName(string p_ServiceName, string p_ExternalId, bool p_taskyam, DCAParams p_EnviorentParams)
//        {
//            if (String.IsNullOrWhiteSpace(p_EnviorentParams.Sufix))
//            {
//                throw new Exception(" p_EnviorentParams.Sufix is null ");
//            }
//            string v_customer_list = "";
//            v_customer_list = "IL" + p_EnviorentParams.consumerId;
//            if (p_taskyam) v_customer_list += ".IL" + p_EnviorentParams.mehesCustomerId;
//            if (!String.IsNullOrWhiteSpace(p_EnviorentParams.consumerId2)) v_customer_list += ".IL" + p_EnviorentParams.consumerId2;
//            if (!String.IsNullOrWhiteSpace(p_EnviorentParams.consumerId3)) v_customer_list += ".IL" + p_EnviorentParams.consumerId3;
//            if (!String.IsNullOrWhiteSpace(p_EnviorentParams.consumerId4)) v_customer_list += ".IL" + p_EnviorentParams.consumerId4;
//            string v_env = "";// p_EnviorentParams.env_id;
//            v_env = p_EnviorentParams.Sufix;
//            return string.Format("{0}{1}.{2}.{3}.{4}{5}",
//                             p_ServiceName, "_In",
//                             v_customer_list,
//                             DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"),
//                             p_ExternalId, v_env );
//        }
//        void UnifaceRequest(string MessageID, string MessageXML, out string ResponseXML, out string ErrorOccurred, ref string MoreParams, out string ErrorMess)
//        {
//            throw new NotImplementedException();
//        }
//        public string DCAServiceAddress { get; private set; }
//        public string PartnerVaultID { get; private set; }
//        public string UnifreightEnvironmentID { get; private set; }
//        public int Tenant { get; private set; }
//        public static Stream Decompress(Stream compressed)
//        {
//            var decompressed = new MemoryStream();
//            using (var zip = new GZipStream(compressed, CompressionMode.Decompress, true))
//            {
//                zip.CopyTo(decompressed);
//            }

//            decompressed.Seek(0, SeekOrigin.Begin);
//            return decompressed;
//        }
//    }
//}
