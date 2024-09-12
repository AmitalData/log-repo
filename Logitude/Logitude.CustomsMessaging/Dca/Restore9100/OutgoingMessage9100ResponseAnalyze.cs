using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.Utils;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Utils;
using Microsoft.Practices.Unity;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.OutgoingMessageDeliveryApprovalServiceReference;
using UnifreightIIG.Common.OutgoingMessageRequestServiceReference;

namespace Logitude.CustomsMessaging.Dca.Restore9100
{
    internal class OutgoingMessage9100ResponseAnalyze
    {

        private static HashSet<string> _BadMessagingServiceCode = new HashSet<string>();
        private readonly CustomsSettingPM _CustomsSettingPM;
        private List<InterfaceTenantDefinitionManagementPM> _InterfaceListDCA;
        
        public StringBuilder MyStringBuilder { get; }
        public ConcurrentQueue<string> sbFilenameQueue { get; }
        public ConcurrentBag<string> exceptionBag { get; }
        public ConcurrentBag<NG_9200_OutgoingMessageDeliveryApprovalListOfCorrelationIDs> correlationIdsCanClear { get; }

        private static DateTime _LastErrordateTime;

        public OutgoingMessage9100ResponseAnalyze(CustomsSettingPM customsSettingPM, List<InterfaceTenantDefinitionManagementPM> interfaceListDCA)
        {
            if (customsSettingPM is null)
            {
                throw new System.ArgumentNullException("customsSettingPM");
            }
            if (interfaceListDCA is null)
            {
                throw new System.ArgumentNullException("interfaceListDCA");
            }



            _CustomsSettingPM = customsSettingPM;
            _InterfaceListDCA = interfaceListDCA;
            _InterfaceListDCA = (new InterfaceTenantDefinitionQueryService(customsSettingPM.Tenant)).GetInterfaceListDCA(_InterfaceListDCA, customsSettingPM.CompanyType);


            MyStringBuilder = new StringBuilder();
            sbFilenameQueue = new ConcurrentQueue<String>();
            exceptionBag = new ConcurrentBag<String>();
            correlationIdsCanClear = new ConcurrentBag<NG_9200_OutgoingMessageDeliveryApprovalListOfCorrelationIDs>();
        }

        internal void SaveInDB(List<NG_9101_MSG_OutgoingMessageResponseOutgoingMessage> OutgoingMessageList)
        {

            var sw = Stopwatch.StartNew();
            


            try
            {

                var OutgoingMessageInterfaceList = _InterfaceListDCA.GetOutgoingMessageInterfaceList(OutgoingMessageList);

                LogIt($"OutgoingMessage9100ResponseAnalyze-OutgoingMessageInterfaceList== {OutgoingMessageInterfaceList.Count}");
                if (OutgoingMessageInterfaceList.Count == 0)
                {
                    LogIt($"OutgoingMessage9100ResponseAnalyze-nothing to do");
                    return;
                }

                sw = Stopwatch.StartNew();
                Parallel.ForEach(OutgoingMessageInterfaceList,
                            new ParallelOptions { MaxDegreeOfParallelism = 4 },//cpu
                            currMessage =>
                                TPL_SaveInDB(currMessage.response, currMessage.messageDCA, currMessage.dcaFile,
                                    correlationIdsCanClear, sbFilenameQueue, exceptionBag)
                            );
                sw.Stop();
                LogIt($"OutgoingMessage9100ResponseAnalyze-Parallel Save took {sw.Elapsed}");
            }
            catch (System.Exception e)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(e, $"RestoreWaitingImport_BIGError_T{_CustomsSettingPM.Tenant}");
                throw;
            }
        }


        private void LogIt(string mess)
        {
            Debug.WriteLine(mess);
            MyStringBuilder.AppendLine(mess);
        }

        private  void TPL_SaveInDB(
            NG_9101_MSG_OutgoingMessageResponseOutgoingMessage itemOutgoingMessage,
            InterfaceTenantDefinitionManagementPM messageDCA,
            DCAFileModel dcaFile,
            ConcurrentBag<NG_9200_OutgoingMessageDeliveryApprovalListOfCorrelationIDs> correlationIdsCanClear, ConcurrentQueue<string> sbFilenameQueue, ConcurrentBag<string> exceptionBag)
        {
            try
            {
                LogIt($"SaveRequestSheet... {itemOutgoingMessage.CorrelationId}");

                SaveRequestSheet(messageDCA, dcaFile, itemOutgoingMessage.MSG);

                LogIt($"SaveRequestSheet!Done! {itemOutgoingMessage.CorrelationId}");

                correlationIdsCanClear.Add(new NG_9200_OutgoingMessageDeliveryApprovalListOfCorrelationIDs { CorrelationIDs = itemOutgoingMessage.CorrelationId });
                sbFilenameQueue.Enqueue(dcaFile.SelectedFileDownload);
                LogIt($"SaveInDB({dcaFile.SelectedFileDownload}) -Done");
                //NumOfMessages++;
            }
            catch (System.Exception EE)
            {
                LogIt($"SaveInDB({dcaFile.SelectedFileDownload}) -{EE.ToString()}");
                //_SaveError = true;
                exceptionBag.Add($"Error while save message in DCA : {EE.ToString()}");
                //throw;
            }
        }

        private  void SaveRequestSheet(InterfaceTenantDefinitionManagementPM messageDCA, DCAFileModel dcaFile, string fileContents
        //byte[] messageBytes
        )
        {

            string currMessagingServiceCode = _InterfaceListDCA.GetMainMessagingServiceCode(messageDCA);
            if (_BadMessagingServiceCode.Contains(currMessagingServiceCode))
            {
                LogIt($"{currMessagingServiceCode} in _BadMessagingServiceCode {dcaFile.SelectedFileDownload} ");
                return;
            }
            if (!ContainerAccessor.Container.IsRegistered<IMessagingServiceInterfaceType>(currMessagingServiceCode))
            {
                try
                {
                    _BadMessagingServiceCode.Add(currMessagingServiceCode);
                    LogIt("DCA MessagingSheetWR: SaveMessageToAnalyzeQueueN():!ContainerAccessor.Container.IsRegistered :analyzeClass = " + currMessagingServiceCode);
                    LogIt("Due infinite errors i cancel writing log");

                    if (DateTime.Now.Subtract(_LastErrordateTime) > TimeSpan.FromMinutes(10))
                    {
                        _LastErrordateTime = DateTime.Now;
                        NetCommonHelper.Logger.DevLog.Instance.WriteError("NO MAIN Code (response 2754 of 2750 !!!) - currMessagingService : " + currMessagingServiceCode + " Is not Registered in ContainerAccessor.Container,    Due infinite errors i cancel writing log");
                    }
                }
                catch
                {

                }
                return;
                //ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "DCA MessagingSheetWR: SaveMessageToAnalyzeQueueN():!ContainerAccessor.Container.IsRegistered :analyzeClass=" + currMessagingService);
                throw new System.Exception("DCA MessagingSheetWR: SaveMessageToAnalyzeQueueN():!ContainerAccessor.Container.IsRegistered :analyzeClass=" + currMessagingServiceCode);
                //return;
            }
            ///Task.Delay(TimeSpan.FromSeconds(5)).Wait();
            LogIt($"DcaReceivedCustomResponseCorrelation.... {currMessagingServiceCode}  {dcaFile.SelectedFileDownload}");

            AmitalDebuggerUtil.Break();

            var anaO = ContainerAccessor.Container.Resolve<IMessagingServiceInterfaceType>(currMessagingServiceCode);
            //var fileContents = UnifreightIIG.Common.Utils.Base64Util.FromBase64_Decode(fileContents, false);

            using (var scope = TransactionFactory.GetTransaction())
            {
                anaO.DcaReceivedCustomResponseCorrelation(messageDCA.InterfaceManagement, _CustomsSettingPM.Tenant, dcaFile, fileContents);
                scope.Complete();
            }
        }

    }
}
