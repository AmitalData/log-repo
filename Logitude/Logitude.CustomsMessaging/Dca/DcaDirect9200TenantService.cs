using Logitude.BL.Security;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.OutgoingMessageDeliveryApprovalServiceReference;
using UnifreightIIG.Common.OutgoingMessageRequestServiceReference;
using UnifreightIIG.Common.TheGateway;
using Microsoft.Practices.Unity;
using Logitude.Customs.BL.Utils;
using System.Threading;
using Logitude.Server.Tools.Utils;
using System.IO;
using System.Collections.Concurrent;
using Logitude.CustomsMessaging.Dca.Utilities;
using Logitude.Server.Tools.ExternalServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.CustomsMessaging.FakeMessagingServices;

namespace Logitude.CustomsMessaging.Dca
{
    public class DcaDirect9200TenantService
    {
        private CustomsSettingPM _CustomsSettingPM;
        private List<string> _AllDcaPreFixWithoutInOutUpper;
        private List<InterfaceTenantDefinitionManagementPM> _InterfaceListDCA;
        private List<InterfaceTenantDefinitionManagementPM> _AllInterface;
        private readonly DedicatedCourierDCAModel _DedicatedCourierDCAModel;
        private readonly FTPDetail _uploadFtpDetail;
        StringBuilder _SBInfoLog;
        StringBuilder _SBErrorLog;
        Stopwatch sw;
        int NumOfMessages;
        private bool _SaveError;
        private static DateTime _LastErrordateTime;
        private List<string> _AllDcaPreFixByEnvironment;
        public static bool SkipCorrelationClearForTests { get; set; } = false;
        internal static NG_9101_MSG_OutgoingMessageResponse testResponse;

        public DcaDirect9200TenantService(
            CustomsSettingPM costomSetting, List<string> allDcaPreFixWithoutInOutUpper, List<InterfaceTenantDefinitionManagementPM> interfaceListDCA,
            List<InterfaceTenantDefinitionManagementPM> allInterface, DedicatedCourierDCAModel dedicatedCourierDCAModel, List<string> allDcaPreFixByEnvironment, PartnerSftpConfig uploadCfg)
        {
            this._CustomsSettingPM = costomSetting;
            this._AllDcaPreFixWithoutInOutUpper = allDcaPreFixWithoutInOutUpper;
            this._InterfaceListDCA = interfaceListDCA;
            this._AllInterface = allInterface;
            this._DedicatedCourierDCAModel = dedicatedCourierDCAModel;
            this._AllDcaPreFixByEnvironment = allDcaPreFixByEnvironment;
            _uploadFtpDetail = ToFtpDetail(uploadCfg);
        }

        public Action SetLastActivity { get; set; }
        public Action LogDoneItemInMemoryAction { get; set; }



        public void DownloadAll(NG_9101_MSG_OutgoingMessageResponse testResponse = null)
        {
            if (testResponse == null && DcaDirect9200TenantService.testResponse != null)
            {
                testResponse = DcaDirect9200TenantService.testResponse;
                DcaDirect9200TenantService.testResponse = null;
            }

            _SBInfoLog = new StringBuilder($"Tenant:{_CustomsSettingPM.Tenant} Start At {DateTime.Now}");
            _SBErrorLog = new StringBuilder($"Tenant:{_CustomsSettingPM.Tenant} Start At {DateTime.Now}");
            sw = Stopwatch.StartNew();
            try
            {
                if (string.IsNullOrEmpty(this._CustomsSettingPM.CustomsAgentId))
                {
                    _SaveError = true;
                    _SBErrorLog.AppendLine("Error: CustomsAgentId is null ");
                    return;
                }

                if (string.IsNullOrEmpty(this._CustomsSettingPM.IIGServiceAddress))
                {
                    _SaveError = true;
                    _SBErrorLog.AppendLine("Error: IIGServiceAddress is null ");
                    return;
                }

                NumOfMessages = 0;
                bool? useTPL = this._DedicatedCourierDCAModel?.UseTPL;
                while (true)//(Send9100Take100Messages_HaveMore().GetValueOrDefault() > 0)
                {
                    int dcaMessagesLeftInIIGServer = 0;
                    if (useTPL.GetValueOrDefault())
                    {
                        dcaMessagesLeftInIIGServer = TPL_Send9100Take100Messages_HaveMore(testResponse).GetValueOrDefault();
                    }
                    else
                    {
                        dcaMessagesLeftInIIGServer = Send9100Take100Messages_HaveMore(testResponse).GetValueOrDefault();
                    }
                    if (dcaMessagesLeftInIIGServer > 0)
                    {

                        if (sw.Elapsed > TimeSpan.FromMinutes(1))
                        {
                            if (this._DedicatedCourierDCAModel != null)// _DedicatedCourierDCAModel== fast very fast
                            {
                                continue;//continue work on the same tenant! Do not stop! 
                            }
                            break;//do next tenant
                        }

                    }
                    else
                    {
                        break;
                    }

                }
            }
            catch (System.Exception e)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(e);
                _SaveError = true;
                _SBErrorLog.AppendLine("Error: " + e.ToString());

            }
            finally
            {
                if (_SaveError)
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteError(_SBErrorLog.ToString() + ":" + "DcaDirect9200TenantService");
                }

            }
        }


        public int? TPL_Send9100Take100Messages_HaveMore(NG_9101_MSG_OutgoingMessageResponse testResponse = null)
        {
            var correlationIdsCanClear = new ConcurrentBag<NG_9200_OutgoingMessageDeliveryApprovalListOfCorrelationIDs>();
            var exceptionBag = new ConcurrentBag<String>();
            var sbFilenameQueue = new ConcurrentQueue<String>();
            string RequestsSheetExternalId = Guid.NewGuid().ToString();
            NG_9101_MSG_OutgoingMessageResponse response = null;
            try
            {
                if (testResponse != null)
                {
                    response = testResponse;
                }
                else
                {
                    var request = new UnifreightIIG.Common.OutgoingMessageRequestServiceReference.NG_9100_MSG_OutgoingMessageRequest()
                    {
                        PeekWay = new NG_9100_MSG_OutgoingMessageRequestPeekWay()
                        {
                            Peek_Way = 2,
                            Take = _CustomsSettingPM.QtyFeedbackInPendingMessage ?? 20,
                        },
                        GetOptions = null,

                        RequestContentHeader = new UnifreightIIG.Common.OutgoingMessageRequestServiceReference.RequestContentHeader()
                        {
                            SenderID = 1,
                            TransmitionDateTime = DateTime.Now,
                            RecieverID = new int[] { 11 }

                        }
                    };

                    var myIIGGatewayMoreParams = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };



                    using (var uifreightSdkGateway = new UnifreightSdkGateway(_CustomsSettingPM.IIGServiceAddress))
                    {
                        var _ResponseHeader = uifreightSdkGateway.GetChannel<IOutgoingMessageRequestOperation>()
                            .OutgoingMessageRequest(
                            RequestsSheetExternalId,
                            _CustomsSettingPM.CustomsAgentId,
                            request,
                            ref myIIGGatewayMoreParams,
                            out response);

                    }
                }

                var files = new List<String>();
                _SBInfoLog.AppendLine($"HowManyOtherWaitingMessages {response.Result.HowManyOtherWaitingMessages}  took:{sw.Elapsed}");
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"HowManyOtherWaitingMessages={response.Result.HowManyOtherWaitingMessages} ");

                _SBInfoLog.AppendLine($"RowNumbers {response.Result.RowNumbers}");
                //var sbFilename = new StringBuilder();
                sbFilenameQueue.Enqueue($"Start Tenant {_CustomsSettingPM.Tenant}");
                if (response.OutgoingMessage != null)
                {

                    var dcaUtil = new DcaFilterByEnvironmentService();
                    var res = dcaUtil
                        .FilterByEnvironmentOutGoing(_CustomsSettingPM.Tenant, response.OutgoingMessage.ToList(), _AllDcaPreFixByEnvironment);
                    var outgoingMessageFilterByEnvironment = res.OutgoingMessage;
                    sbFilenameQueue.Enqueue(res.SbLocal.ToString());

                    Parallel.ForEach(
                        outgoingMessageFilterByEnvironment,
                        new ParallelOptions { MaxDegreeOfParallelism = 4 },//cpu
                        itemOutgoingMessage =>
                        {
                            TPL_SaveInDB(correlationIdsCanClear, sbFilenameQueue, itemOutgoingMessage, exceptionBag);
                        }
                        );

                }
                exceptionBag.ToList().ForEach(err => _SBErrorLog.AppendLine(err));

                if (!SkipCorrelationClearForTests && correlationIdsCanClear != null && correlationIdsCanClear.Count() > 0)
                {
                    UpdateIIGCorralationAreDone(correlationIdsCanClear.ToList());



                    var l = sbFilenameQueue.ToList();
                    var sb1 = new StringBuilder();
                    l.ForEach(line => { sb1.AppendLine(line); });
                    NetCommonHelper.Logger.DevLog.Instance.WriteInfo(sb1.ToString() + "TenantDownloaderFilename");
                    string haveMore = response.Result.HowManyOtherWaitingMessages > 0 ? "Have more .." : "";
                    _SBInfoLog.AppendLine("All messages received  " + haveMore);

                }


            }
            catch (System.Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex);
                _SaveError = true;
                _SBErrorLog.AppendLine($"Error while send 9100 error : {ex.ToString()}");
            }
            return response?.Result?.HowManyOtherWaitingMessages;
        }

        public int? Send9100Take100Messages_HaveMore(NG_9101_MSG_OutgoingMessageResponse testResponse = null)
        {
            var correlationIdsCanClear = new List<NG_9200_OutgoingMessageDeliveryApprovalListOfCorrelationIDs>();

            string RequestsSheetExternalId = Guid.NewGuid().ToString();
            NG_9101_MSG_OutgoingMessageResponse response = null;
            try
            {
                if (testResponse != null)
                {
                    response = testResponse;
                }
                else
                {
                    var request = new UnifreightIIG.Common.OutgoingMessageRequestServiceReference.NG_9100_MSG_OutgoingMessageRequest()
                    {
                        PeekWay = new NG_9100_MSG_OutgoingMessageRequestPeekWay()
                        {
                            Peek_Way = 2,
                            Take = _CustomsSettingPM.QtyFeedbackInPendingMessage ?? 20,
                        },
                        GetOptions = null,

                        RequestContentHeader = new UnifreightIIG.Common.OutgoingMessageRequestServiceReference.RequestContentHeader()
                        {
                            SenderID = 1,
                            TransmitionDateTime = DateTime.Now,
                            RecieverID = new int[] { 11 }

                        }
                    };

                    var myIIGGatewayMoreParams = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };



                    using (var uifreightSdkGateway = new UnifreightSdkGateway(_CustomsSettingPM.IIGServiceAddress))
                    {
                        var _ResponseHeader = uifreightSdkGateway.GetChannel<IOutgoingMessageRequestOperation>()
                            .OutgoingMessageRequest(
                            RequestsSheetExternalId,
                            _CustomsSettingPM.CustomsAgentId,
                            request,
                            ref myIIGGatewayMoreParams,
                            out response);

                    }
                }
                var files = new List<String>();
                _SBInfoLog.AppendLine($"HowManyOtherWaitingMessages {response.Result.HowManyOtherWaitingMessages}  took:{sw.Elapsed}");
                _SBInfoLog.AppendLine($"RowNumbers {response.Result.RowNumbers}");
                var sbFilename = new StringBuilder();
                sbFilename.AppendLine($"Start Tenant {_CustomsSettingPM.Tenant}");
                if (response.OutgoingMessage != null)
                {

                    var dcaUtil = new DcaFilterByEnvironmentService();
                    var resFilterByEnvironmentOutGoing = dcaUtil
                        .FilterByEnvironmentOutGoing(_CustomsSettingPM.Tenant, response.OutgoingMessage.ToList(), _AllDcaPreFixByEnvironment);
                    var outgoingMessageFilterByEnvironment = resFilterByEnvironmentOutGoing.OutgoingMessage;
                    sbFilename.AppendLine(resFilterByEnvironmentOutGoing.SbLocal.ToString());

                    foreach (var itemOutgoingMessage in outgoingMessageFilterByEnvironment /*response.OutgoingMessage*/)
                    {
                        SaveInDB(correlationIdsCanClear, sbFilename, itemOutgoingMessage);
                    }

                }



                if (!SkipCorrelationClearForTests && correlationIdsCanClear != null && correlationIdsCanClear.Count() > 0)
                {
                    UpdateIIGCorralationAreDone(correlationIdsCanClear);
                    NetCommonHelper.Logger.DevLog.Instance.WriteInfo(sbFilename.ToString() + "TenantDownloaderFilename");
                    string haveMore = response.Result.HowManyOtherWaitingMessages > 0 ? "Have more .." : "";
                    _SBInfoLog.AppendLine("All messages received  " + haveMore);
                }


            }
            catch (System.Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex);
                _SaveError = true;
                _SBErrorLog.AppendLine($"Error while send 9100 error : {ex.ToString()}");
            }
            return response?.Result?.HowManyOtherWaitingMessages;
        }

        private void UpdateIIGCorralationAreDone(List<NG_9200_OutgoingMessageDeliveryApprovalListOfCorrelationIDs> correlationIDs)
        {
            MoreParams myIIGGatewayMoreParams = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };
            var request9200 = new UnifreightIIG.Common.OutgoingMessageDeliveryApprovalServiceReference.NG_9200_OutgoingMessageDeliveryApproval()
            {

                ListOfCorrelationIDs = correlationIDs.ToArray(),


                RequestContentHeader = new UnifreightIIG.Common.OutgoingMessageDeliveryApprovalServiceReference.RequestContentHeader()
                {
                    SenderID = 1,
                    TransmitionDateTime = DateTime.Now,
                    RecieverID = new int[] { 11 }

                }
            };

            var RequestsSheetExternalId9200 = Guid.NewGuid().ToString();
            INF_MSG_Generic response9200 = null;

            try
            {
                using (var uifreightSdkGateway = new UnifreightSdkGateway(this._CustomsSettingPM.IIGServiceAddress))
                {
                    var _ResponseHeader = uifreightSdkGateway.GetChannel<IOutgoingMessageDeliveryApprovalOperation>()
                        .OutgoingMessageDeliveryApproval(
                        RequestsSheetExternalId9200,
                        this._CustomsSettingPM.CustomsAgentId,
                        request9200,
                        ref myIIGGatewayMoreParams,
                        out response9200);

                }
            }
            catch (System.Exception ee)
            {
                _SaveError = true;
                this._SBErrorLog.AppendLine("Error while UpdateIIGCorralationAreDone" + ee.ToString());
            }



        }
        private void TPL_SaveInDB(
            ConcurrentBag<NG_9200_OutgoingMessageDeliveryApprovalListOfCorrelationIDs> correlationIDs,
            ConcurrentQueue<string> sbFilename, NG_9101_MSG_OutgoingMessageResponseOutgoingMessage itemOutgoingMessage,
            ConcurrentBag<string> exceptionBag)
        {
            try
            {
                var myFileName = System.IO.Path.GetFileName(itemOutgoingMessage.Filename);
                var dcaFile = DCAFilePraser.GetDCAFileModel(myFileName);
                InterfaceTenantDefinitionManagementPM messageDCA = _InterfaceListDCA.FirstOrDefault(
                    rec => IsStart(rec.InterfaceManagement.DcaPrefixName, myFileName) ||
                    IsStart(rec.InterfaceManagement.DcaPrefixName2, myFileName) ||
                    IsStart(rec.InterfaceManagement.DcaPrefixName3, myFileName) ||
                    IsStart(rec.InterfaceManagement.DcaPrefixName4, myFileName) ||
                    (rec.InterfaceManagement.DcaPrefixName == null && rec.InterfaceManagement.DcaPrefixName2 == null
                    && rec.InterfaceManagement.DcaPrefixName3 == null && rec.InterfaceManagement.DcaPrefixName4 == null));
                var IsUnifreight = messageDCA.IsUnifreight == true ? messageDCA.IsUnifreight : messageDCA.InterfaceManagement.IsUnifreight;
                var IsCustomsFile = messageDCA.IsCustomsFile == true ? messageDCA.IsCustomsFile : messageDCA.InterfaceManagement.IsCustomsFile;


                if (IsUnifreight == true && IsCustomsFile == false)
                {
                    UploadViaSftp(myFileName, itemOutgoingMessage.MSG, s => sbFilename.Enqueue(s));
                }
                else
                {

                    if (messageDCA != null && _AllDcaPreFixWithoutInOutUpper.Any(prefix => myFileName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)))
                    {
                        try
                        {


                            SaveRequestSheet(messageDCA, dcaFile, itemOutgoingMessage.MSG);

                            correlationIDs.Add(new NG_9200_OutgoingMessageDeliveryApprovalListOfCorrelationIDs { CorrelationIDs = itemOutgoingMessage.CorrelationId });
                            sbFilename.Enqueue(myFileName);
                            NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"SaveInDB({myFileName}) -Done");
                            if (IsUnifreight == true)
                            {
                                UploadViaSftp(myFileName, itemOutgoingMessage.MSG, s => sbFilename.Enqueue(s));
                            }
                        }
                        catch (System.Exception EE)
                        {
                            NetCommonHelper.Logger.DevLog.Instance.WriteFatal(EE, $"SaveInDB({myFileName})");
                            _SaveError = true;
                            exceptionBag.Add($"Error while save message in DCA : {EE.ToString()}");
                            //throw;
                        }
                    }
                    else
                    {
                        HandleNotNeededMessage(myFileName, itemOutgoingMessage.MSG, s => sbFilename.Enqueue(s));
                        correlationIDs.Add(new NG_9200_OutgoingMessageDeliveryApprovalListOfCorrelationIDs
                        { CorrelationIDs = itemOutgoingMessage.CorrelationId });
                    }
                }

            }
            catch (System.Exception e)
            {
                _SaveError = true;
                exceptionBag.Add($"Error while save message in DCA : {e.Message}");

            }
        }

        private void SaveInDB(List<NG_9200_OutgoingMessageDeliveryApprovalListOfCorrelationIDs> correlationIDs, StringBuilder sbFilename, NG_9101_MSG_OutgoingMessageResponseOutgoingMessage itemOutgoingMessage)
        {
            try
            {
                var myFileName = System.IO.Path.GetFileName(itemOutgoingMessage.Filename);
                var dcaFile = DCAFilePraser.GetDCAFileModel(myFileName);
                InterfaceTenantDefinitionManagementPM messageDCA = _InterfaceListDCA.FirstOrDefault(
rec => IsStart(rec.InterfaceManagement.DcaPrefixName, myFileName) ||
IsStart(rec.InterfaceManagement.DcaPrefixName2, myFileName) ||
IsStart(rec.InterfaceManagement.DcaPrefixName3, myFileName) ||
IsStart(rec.InterfaceManagement.DcaPrefixName4, myFileName)
);

                if (messageDCA != null && _AllDcaPreFixWithoutInOutUpper.Any(prefix => myFileName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)))
                {
                    try
                    {


                        SaveRequestSheet(messageDCA, dcaFile, itemOutgoingMessage.MSG);
                        correlationIDs.Add(new NG_9200_OutgoingMessageDeliveryApprovalListOfCorrelationIDs { CorrelationIDs = itemOutgoingMessage.CorrelationId });
                        sbFilename.AppendLine(myFileName);
                        NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"SaveInDB({myFileName}) -Done");
                        NumOfMessages++;
                        if (messageDCA.InterfaceManagement.IsUnifreight.GetValueOrDefault())
                        {
                            UploadViaSftp(myFileName, itemOutgoingMessage.MSG, s => sbFilename.AppendLine(s));
                        }
                    }
                    catch (System.Exception EE)
                    {
                        NetCommonHelper.Logger.DevLog.Instance.WriteFatal(EE, $"SaveInDB({myFileName})");
                        _SaveError = true;
                        _SBErrorLog.AppendLine($"Error while save message in DCA : {EE.ToString()}");
                        //throw;
                    }
                }
                else
                {
                    HandleNotNeededMessage(myFileName, itemOutgoingMessage.MSG, s => sbFilename.AppendLine(s));
                    NumOfMessages++;
                    correlationIDs.Add(new NG_9200_OutgoingMessageDeliveryApprovalListOfCorrelationIDs
                    { CorrelationIDs = itemOutgoingMessage.CorrelationId });
                }

            }
            catch (System.Exception e)
            {
                _SaveError = true;
                _SBErrorLog.AppendLine($"Error while save message in DCA : {e.Message}");

            }
        }

        private static bool IsStart(string curDcaPrefixName, string dcaFile)
        {
            if (!String.IsNullOrWhiteSpace(curDcaPrefixName))
            {
                if (dcaFile.StartsWith(curDcaPrefixName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;

        }

        private void SaveRequestSheet(InterfaceTenantDefinitionManagementPM messageDCA, DCAFileModel dcaFile, string fileContents
                //byte[] messageBytes
                )
        {

            string currMessagingService = GetMainMessagingService(messageDCA);
            if (!ContainerAccessor.Container.IsRegistered<IMessagingServiceInterfaceType>(currMessagingService))
            {
                try
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteDebug("DCA MessagingSheetWR: SaveMessageToAnalyzeQueueN():!ContainerAccessor.Container.IsRegistered :analyzeClass = " + currMessagingService);
                    NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Due infinite errors i cancel writing log");

                    if (DateTime.Now.Subtract(_LastErrordateTime) > TimeSpan.FromMinutes(10))
                    {
                        _LastErrordateTime = DateTime.Now;
                        NetCommonHelper.Logger.DevLog.Instance.WriteError("NO MAIN Code (response 2754 of 2750 !!!) - currMessagingService : " + currMessagingService + " Is not Registered in ContainerAccessor.Container,    Due infinite errors i cancel writing log" + ":" + "DCANotIsRegistered");
                    }
                }
                catch
                {

                }
                return;
                //ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "DCA MessagingSheetWR: SaveMessageToAnalyzeQueueN():!ContainerAccessor.Container.IsRegistered :analyzeClass=" + currMessagingService);
                throw new System.Exception("DCA MessagingSheetWR: SaveMessageToAnalyzeQueueN():!ContainerAccessor.Container.IsRegistered :analyzeClass=" + currMessagingService);
                //return;
            }


            AmitalDebuggerUtil.Break();

            var anaO = ContainerAccessor.Container.Resolve<IMessagingServiceInterfaceType>(currMessagingService);
            //var fileContents = UnifreightIIG.Common.Utils.Base64Util.FromBase64_Decode(fileContents, false);

            using (var scope = TransactionFactory.GetTransaction())
            {
                anaO.DcaReceivedCustomResponseCorrelation(messageDCA.InterfaceManagement, _CustomsSettingPM.Tenant, dcaFile, fileContents);
                scope.Complete();
            }
        }
        private string GetMainMessagingService(InterfaceTenantDefinitionManagementPM messageDCA)
        {
            var currMessagingService = messageDCA.Code;


            var mainOutInterface = _AllInterface.FirstOrDefault(rec =>
                rec.InterfaceManagement.ResponseInterfaceCode == messageDCA.InterfaceManagement.Code);
            if (mainOutInterface != null)
            {
                currMessagingService = mainOutInterface.Code;
            }
            else
            {

            }
            return currMessagingService;
        }
        private static FTPDetail ToFtpDetail(PartnerSftpConfig cfg)
        {
            return cfg == null ? null : new FTPDetail
            {
                Host = cfg.Host,
                UserName = cfg.Username,
                Password = cfg.Password,
                Folder = cfg.RemotePath
            };
        }
        private void UploadViaSftp(string fileName, string fileContents, Action<string> logLineOut)
        {
            if (_uploadFtpDetail == null) return;         

            try
            {
                var uploader = new PartnerSftpUploader(_uploadFtpDetail);
                uploader.UploadBytes(fileName, Encoding.UTF8.GetBytes(fileContents));

                logLineOut?.Invoke($"SFTP upload OK → {fileName}");
                NetCommonHelper.Logger.DevLog.Instance
                    .WriteDebug($"SFTP upload OK → {fileName}");
            }
            catch (System.Exception ex)
            {
                logLineOut?.Invoke($"SFTP upload error!!!! {ex.Message} for {fileName}");
                NetCommonHelper.Logger.DevLog.Instance
                    .WriteError($"SFTP‑Upload: {ex}");
                _SaveError = true;               
            }
        }
        private void HandleNotNeededMessage(string fileName, string fileContents, Action<string> logLineOut)
        {
            logLineOut?.Invoke($"NOT NEEDED!!!! {fileName}");
            NetCommonHelper.Logger.DevLog.Instance.WriteDebug(
                $"NOT NEEDED!!!! SaveInDB({fileName})");

            UploadViaSftp(fileName, fileContents, logLineOut);

            if (_DedicatedCourierDCAModel != null)
            {
                try
                {
                    string backupPath = Path.Combine(_DedicatedCourierDCAModel.BackupPath, fileName);
                    File.WriteAllText(backupPath, fileContents);
                }
                catch (System.Exception ex)
                {
                    logLineOut?.Invoke($"WriteAllText error!!!! {ex.Message}");
                    NetCommonHelper.Logger.DevLog.Instance
                        .WriteError($"Backup‑NotNeeded: {ex}");
                }
            }
        }
    }
}