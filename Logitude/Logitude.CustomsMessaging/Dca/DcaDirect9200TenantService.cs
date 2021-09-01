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

namespace Logitude.CustomsMessaging.Dca
{
    public class DcaDirect9200TenantService
    {
        private CustomsSettingPM _CustomsSettingPM;
        private List<string> _AllDcaPreFixWithoutInOutUpper;
        private List<InterfaceTenantDefinitionManagementPM> _InterfaceListDCA;
        private List<InterfaceTenantDefinitionManagementPM> _AllInterface;
        StringBuilder _SBInfoLog;
        StringBuilder _SBErrorLog;
        Stopwatch sw;
        int NumOfMessages;
        private bool _SaveError;

        public DcaDirect9200TenantService(CustomsSettingPM costomSetting, List<string> allDcaPreFixWithoutInOutUpper, List<InterfaceTenantDefinitionManagementPM> interfaceListDCA, 
            List<InterfaceTenantDefinitionManagementPM> allInterface)
        {
            this._CustomsSettingPM = costomSetting;
            this._AllDcaPreFixWithoutInOutUpper = allDcaPreFixWithoutInOutUpper;
            this._InterfaceListDCA = interfaceListDCA;
            this._AllInterface = allInterface;
        }

        public Action SetLastActivity { get; set; }
        public Action LogDoneItemInMemoryAction { get; set; }



        public void DownloadAll(/*string debugIIGMessageId*/)
        {

            _SBInfoLog = new StringBuilder($"Tenant:{_CustomsSettingPM.Tenant} Start At {DateTime.Now}");
            _SBErrorLog= new StringBuilder($"Tenant:{_CustomsSettingPM.Tenant} Start At {DateTime.Now}");
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

                while (Send9100Take100Messages_HaveMore().GetValueOrDefault() > 0)
                {
                    if (sw.Elapsed > TimeSpan.FromMinutes(1))
                    {
                        break;//do next tenant
                    }
                }
            }
            catch (System.Exception e)
            {
                _SaveError = true;
                _SBErrorLog.AppendLine("Error: " + e.ToString());

            }
            finally
            {
                if (_SaveError)
                {
                    Logger.LogMe(_SBErrorLog.ToString(), true, "DcaDirect9200TenantService");
                }
                
            }
        }




        public int? Send9100Take100Messages_HaveMore()
        {
            var correlationIdsCanClear = new List<NG_9200_OutgoingMessageDeliveryApprovalListOfCorrelationIDs>();

            string RequestsSheetExternalId = Guid.NewGuid().ToString();
            NG_9101_MSG_OutgoingMessageResponse response = null;
            try
            {
                var request = new UnifreightIIG.Common.OutgoingMessageRequestServiceReference.NG_9100_MSG_OutgoingMessageRequest()
                {
                    PeekWay = new NG_9100_MSG_OutgoingMessageRequestPeekWay()
                    {
                        Peek_Way = 2,
                        Take = _CustomsSettingPM.QtyFeedbackInPendingMessage??20,
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
                var files = new List<String>();
                _SBInfoLog.AppendLine($"HowManyOtherWaitingMessages {response.Result.HowManyOtherWaitingMessages}  took:{sw.Elapsed}");
                _SBInfoLog.AppendLine($"RowNumbers {response.Result.RowNumbers}");
                var sbFilename = new StringBuilder();
                sbFilename.AppendLine($"Start Tenant {_CustomsSettingPM.Tenant}");
                if (response.OutgoingMessage != null)
                {
                    foreach (var itemOutgoingMessage in response.OutgoingMessage)
                    {
                        SaveInDB(correlationIdsCanClear, sbFilename, itemOutgoingMessage);
                    }

                }

                

                if (correlationIdsCanClear != null && correlationIdsCanClear.Count() > 0)
                {
                    UpdateIIGCorralationAreDone(correlationIdsCanClear);

                    

                    Logger.LogMe(sbFilename.ToString(), false, "TenantDownloaderFilename");
                    string haveMore = response.Result.HowManyOtherWaitingMessages > 0 ? "Have more .." : "";
                    _SBInfoLog.AppendLine("All messages received  " + haveMore);
                    
                }

               
            }
            catch (System.Exception ex)
            {
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

                if (messageDCA!=null && _AllDcaPreFixWithoutInOutUpper.Any(prefix=> myFileName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)))
                {
                    try
                    {
                        

                        SaveRequestSheet(messageDCA, dcaFile, itemOutgoingMessage.MSG);

                        correlationIDs.Add(new NG_9200_OutgoingMessageDeliveryApprovalListOfCorrelationIDs { CorrelationIDs = itemOutgoingMessage.CorrelationId });
                        sbFilename.AppendLine(myFileName);
                        Debug.WriteLine($"SaveInDB({myFileName}) -Done");
                        NumOfMessages++;
                    }
                    catch (System.Exception EE)
                    {
                        Debug.WriteLine($"SaveInDB({myFileName}) -{EE.ToString()}");
                        _SaveError = true;
                        _SBErrorLog.AppendLine($"Error while save message in DCA : {EE.ToString()}");
                        //throw;
                    }
                }
                else
                {

                    sbFilename.AppendLine($"NOT NEEDED!!!! {myFileName}");
                    Debug.WriteLine($"NOT NEEDED!!!! needed in our tenant =SaveInDB({myFileName})");
                    NumOfMessages++;
                    correlationIDs.Add(new NG_9200_OutgoingMessageDeliveryApprovalListOfCorrelationIDs { CorrelationIDs = itemOutgoingMessage.CorrelationId });
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

                Debug.WriteLine("DCA MessagingSheetWR: SaveMessageToAnalyzeQueueN():!ContainerAccessor.Container.IsRegistered :analyzeClass = " + currMessagingService);
                Debug.WriteLine("Due infinite errors i cancel writing log"); return;
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
    }
}