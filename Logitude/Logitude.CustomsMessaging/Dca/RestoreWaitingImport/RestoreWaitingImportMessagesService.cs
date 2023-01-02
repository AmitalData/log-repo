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
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.OutgoingMessageDeliveryApprovalServiceReference;
using UnifreightIIG.Common.OutgoingMessageRequestServiceReference;
using UnifreightIIG.Common.TheGateway;

namespace Logitude.CustomsMessaging.Dca.RestoreWaitingImport
{
    public class RestoreWaitingImportMessagesService
    {
        private static List<MessageCorrelationSavedInDB> _MessageCorrelationSavedInDBList = new List<MessageCorrelationSavedInDB>();
        private static HashSet<string> _BadMessagingServiceCode = new HashSet<string>();
        const int Minutes2Retrieve= 15;
        StringBuilder _StringBuilder = new StringBuilder();
        DateTime? _LastRetrive = null;
        private readonly CustomsSettingPM _CustomsSettingPM;
        private List<InterfaceTenantDefinitionManagementPM> _InterfaceListDCA;
        private DateTime _LastErrordateTime;

        public RestoreWaitingImportMessagesService(CustomsSettingPM customsSettingPM, List<InterfaceTenantDefinitionManagementPM> interfaceListDCA)
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
        }

        public static void TestMe()
        {
            CustomsSettingPM customsSettingPM = (new CustomsSettingQueryService(6)).GetSingle("6", false,false);
            var interfaceTypeQueryService = new InterfaceTenantDefinitionQueryService(6);
            var _AllInterface = interfaceTypeQueryService.GetWithInterfaceManagementDefinition(customsSettingPM.Tenant);


            var interfaceListDCA = 
                   _AllInterface
                   .Where(r => r.OverrideActive == true)

                    //להתייחס לשדה Active מרמת ניהול מסרים
                    .Where(r => r.InterfaceManagement.Active == true)



                    .Where(
                       r =>
                    //INTERFACETYPE
                    //ערכים NULL== הכל, C == רק עמילות, B == רק בלדרות
                    string.IsNullOrWhiteSpace(r.InterfaceManagement.InterfaceType)//All

                    ||
                    (
                    !string.IsNullOrWhiteSpace(r.InterfaceManagement.InterfaceType)
                    &&
                     //COMPANYTYPE שם שדה ערכים C -דיפולטיבי(בסקריפט), או B == בלדרות - אסור ריק יאותחל עם הפצה ראשונה + DEFAULT == C
                     r.InterfaceManagement.InterfaceType == customsSettingPM.CompanyType
                     )
                     )

                   .Where(rec =>
                       //rec.InterfaceManagement.INOUT ==  Logitude.Customs.BL.ClosedTable.InOutType.In  &&
                       //!string.IsNullOrWhiteSpace(rec.InterfaceManagement.DcaPrefixName) && 
                       //!rec.OverrideInActive &&
                       ///////rec.Interactive == Logitude.Customs.BL.ClosedTable.InteractiveMode.DCABatchIn &&
                       !String.IsNullOrWhiteSpace(
                       rec.InterfaceManagement.DcaPrefixName +
                       rec.InterfaceManagement.DcaPrefixName2 +
                       rec.InterfaceManagement.DcaPrefixName3 +
                       rec.InterfaceManagement.DcaPrefixName4)
                       ).ToList();

            var restoreWaitingImportService = new RestoreWaitingImportMessagesService(customsSettingPM, interfaceListDCA);
            restoreWaitingImportService.RestoreWaitingImportSaveInDB();

        }

        public void RestoreWaitingImportSaveInDB()
        {

            if (!String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["20230102.SuppressRestoreWaitingImportSaveInDB"]))
            {
                Debug.WriteLine($"Suppress RestoreWaitingImportSaveInDB");
                return;
            }

            var sbFilenameQueue = new ConcurrentQueue<String>();
            var exceptionBag = new ConcurrentBag<String>();
            var correlationIdsCanClear = new ConcurrentBag<NG_9200_OutgoingMessageDeliveryApprovalListOfCorrelationIDs>();
            Debug.WriteLine($"RestoreWaitingImportService.RestoreWaitingImportSaveInDB()");

            var dcaFilterByEnvironmentService = new DcaFilterByEnvironmentService();
            if (dcaFilterByEnvironmentService.GetDCAEnvPerTenant(_CustomsSettingPM.Tenant) != DcaFilterByEnvironment.Export)
            {
                Debug.WriteLine($"RestoreWaitingImportSaveInDB-only in export cloud");
                return;
            }
            try
            {


                _LastRetrive = _LastRetrive ?? DateTime.Now.AddMinutes(-1* Minutes2Retrieve);
                if (DateTime.Now.Date.Equals(new DateTime(2023,01,2)))
                {
                    //_LastRetrive = DateTime.Now.AddDays(-31);
            }
                var request = GetRequest(_LastRetrive.Value);
                var sw = Stopwatch.StartNew();
                Debug.WriteLine($"RestoreWaitingImportSaveInDB-Send 9100 restore fromDate {request.GetOptions.fromDate}");
                var response = SendOutgoingMessageRequest(request);
                Debug.WriteLine($"RestoreWaitingImportSaveInDB-Send 9100 took {sw.Elapsed}");
                if (response?.OutgoingMessage?.Length == null || response?.OutgoingMessage?.Length == 0)
                {
                    Debug.WriteLine($"RestoreWaitingImportSaveInDB-OutgoingMessage == 0 - nothing todo");
                    return;
                }
                Debug.WriteLine($"RestoreWaitingImportSaveInDB-OutgoingMessage == {response?.OutgoingMessage?.Length}");

                List<OutgoingMessageInterFace> newImportMessagesWithIncludeDcaPrefixName = FilterOnlyNewImportMessagges(response);

                Debug.WriteLine($"RestoreWaitingImportSaveInDB-newImportMessagesWithIncludeDcaPrefixName== {newImportMessagesWithIncludeDcaPrefixName.Count}");
                if (newImportMessagesWithIncludeDcaPrefixName.Count==0)
                {
                    Debug.WriteLine($"RestoreWaitingImportSaveInDB-nothing to do");
                    return;
                }

                sw= Stopwatch.StartNew();
                Parallel.ForEach(newImportMessagesWithIncludeDcaPrefixName,
                            new ParallelOptions { MaxDegreeOfParallelism = 4 },//cpu
                            currMessage =>
                                TPL_SaveInDB(currMessage.response, currMessage.messageDCA, currMessage.dcaFile,
                                    correlationIdsCanClear, sbFilenameQueue, exceptionBag)
                            );
                sw.Stop();
                Debug.WriteLine($"RestoreWaitingImportSaveInDB-Parallel Save took {sw.Elapsed}");

                correlationIdsCanClear.ToList().
                    ForEach(r =>
                    _MessageCorrelationSavedInDBList.Add(
                        new MessageCorrelationSavedInDB() {
                        CorrelationId = r.CorrelationIDs,
                        DownloadAt = DateTime.Now,
                        Tenant = _CustomsSettingPM.Tenant
                        })
                    );
                if (exceptionBag.Count>0)
                {
                    Logger.LogMe(String.Join(Environment.NewLine, exceptionBag.ToList()), true, $"RestoreWaitingImport_Error_T{_CustomsSettingPM.Tenant}");
                }
                if (sbFilenameQueue.Count>0)
                {
                    Logger.LogMe(String.Join(Environment.NewLine, sbFilenameQueue.ToList()), false, $"RestoreWaitingImport_Files_T{_CustomsSettingPM.Tenant}");
                }
                

            }
            catch (System.Exception e)
            {
                Logger.LogMe(e.ToString(), true, $"RestoreWaitingImport_BIGError_T{_CustomsSettingPM.Tenant}");
                throw;
            }
            finally
            {
                int min2delete = Minutes2Retrieve + 2;
                var oldFiles = _MessageCorrelationSavedInDBList
                    .Where(r => r.Tenant == _CustomsSettingPM.Tenant)
                    .Where(r => DateTime.Now.Subtract(r.DownloadAt) > TimeSpan.FromMinutes(min2delete)).ToList();
                _MessageCorrelationSavedInDBList.RemoveAll(r => oldFiles.Contains(r));

            }

        }

        private List<OutgoingMessageInterFace> FilterOnlyNewImportMessagges(NG_9101_MSG_OutgoingMessageResponse response)
        {
            var onlyImportMessages = response.OutgoingMessage.Where(r => !r.Filename.Contains("_EX_")).ToList();

            var listLast15MinImportCorrelationId = _MessageCorrelationSavedInDBList
                .Where(r => r.Tenant == _CustomsSettingPM.Tenant)
                .Select(r => r.CorrelationId).ToList();
            var newImportMessages = onlyImportMessages.Where(r => !listLast15MinImportCorrelationId.Contains(r.CorrelationId)).ToList();

            var newImportMessagesWithIncludeDcaPrefixName = new List<OutgoingMessageInterFace>();
            newImportMessages.ForEach(r =>
            {

                var res = _InterfaceListDCA.GetInterfaceTenantDefinitionManagementPM(r.Filename);
                if (res.messageDCA != null)
                {
                    newImportMessagesWithIncludeDcaPrefixName.Add(new OutgoingMessageInterFace
                    {
                        response = r,
                        messageDCA = res.messageDCA,
                        dcaFile = res.dcaFile


                    });
                }
            });
            return newImportMessagesWithIncludeDcaPrefixName;
        }


        private void TPL_SaveInDB(
            NG_9101_MSG_OutgoingMessageResponseOutgoingMessage itemOutgoingMessage,
            InterfaceTenantDefinitionManagementPM messageDCA,
            DCAFileModel dcaFile,
            ConcurrentBag<NG_9200_OutgoingMessageDeliveryApprovalListOfCorrelationIDs> correlationIdsCanClear, ConcurrentQueue<string> sbFilenameQueue, ConcurrentBag<string> exceptionBag)
        {
            try
            {
                Debug.WriteLine($"SaveRequestSheet... {itemOutgoingMessage.CorrelationId}");
                
                SaveRequestSheet(messageDCA, dcaFile, itemOutgoingMessage.MSG);

                Debug.WriteLine($"SaveRequestSheet!Done! {itemOutgoingMessage.CorrelationId}");

                correlationIdsCanClear.Add(new NG_9200_OutgoingMessageDeliveryApprovalListOfCorrelationIDs { CorrelationIDs = itemOutgoingMessage.CorrelationId });
                sbFilenameQueue.Enqueue(dcaFile.SelectedFileDownload);
                Debug.WriteLine($"SaveInDB({dcaFile.SelectedFileDownload}) -Done");
                //NumOfMessages++;
            }
            catch (System.Exception EE)
            {
                Debug.WriteLine($"SaveInDB({dcaFile.SelectedFileDownload}) -{EE.ToString()}");
                //_SaveError = true;
                exceptionBag.Add($"Error while save message in DCA : {EE.ToString()}");
                //throw;
            }
        }

        private void SaveRequestSheet(InterfaceTenantDefinitionManagementPM messageDCA, DCAFileModel dcaFile, string fileContents
        //byte[] messageBytes
        )
        {

            string currMessagingServiceCode = _InterfaceListDCA.GetMainMessagingServiceCode(messageDCA);
            if (_BadMessagingServiceCode.Contains(currMessagingServiceCode))
            {
                Debug.WriteLine($"{currMessagingServiceCode} in _BadMessagingServiceCode {dcaFile.SelectedFileDownload} ");
                return;
            }
            if (!ContainerAccessor.Container.IsRegistered<IMessagingServiceInterfaceType>(currMessagingServiceCode))
            {
                try
                {
                    _BadMessagingServiceCode.Add(currMessagingServiceCode);
                    Debug.WriteLine("DCA MessagingSheetWR: SaveMessageToAnalyzeQueueN():!ContainerAccessor.Container.IsRegistered :analyzeClass = " + currMessagingServiceCode);
                    Debug.WriteLine("Due infinite errors i cancel writing log");

                    if (DateTime.Now.Subtract(_LastErrordateTime) > TimeSpan.FromMinutes(10))
                    {
                        _LastErrordateTime = DateTime.Now;
                        Logger.LogMe("NO MAIN Code (response 2754 of 2750 !!!) - currMessagingService : " + currMessagingServiceCode + " Is not Registered in ContainerAccessor.Container,    Due infinite errors i cancel writing log", true, "DCANotIsRegistered");
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
            Debug.WriteLine($"DcaReceivedCustomResponseCorrelation.... {currMessagingServiceCode}  {dcaFile.SelectedFileDownload}");
            
            AmitalDebuggerUtil.Break();

            var anaO = ContainerAccessor.Container.Resolve<IMessagingServiceInterfaceType>(currMessagingServiceCode);
            //var fileContents = UnifreightIIG.Common.Utils.Base64Util.FromBase64_Decode(fileContents, false);

            using (var scope = TransactionFactory.GetTransaction())
            {
                anaO.DcaReceivedCustomResponseCorrelation(messageDCA.InterfaceManagement, _CustomsSettingPM.Tenant, dcaFile, fileContents);
                scope.Complete();
            }
        }


        private NG_9101_MSG_OutgoingMessageResponse SendOutgoingMessageRequest(NG_9100_MSG_OutgoingMessageRequest request)
        {
            string RequestsSheetExternalId = Guid.NewGuid().ToString();
            NG_9101_MSG_OutgoingMessageResponse response = null;

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
            return response;
        }

        private static NG_9100_MSG_OutgoingMessageRequest GetRequest(DateTime lastRetrive)
        {
            const int MAX_TAKE = 999;


            return new NG_9100_MSG_OutgoingMessageRequest()
            {


                PeekWay = new NG_9100_MSG_OutgoingMessageRequestPeekWay()
                {
                    Peek_Way = 3,
                    Take = MAX_TAKE  /*1000*/,
                },
                GetOptions = new NG_9100_MSG_OutgoingMessageRequestGetOptions()
                {
                    fromDate = lastRetrive,
                    toDate = DateTime.Now,
                    ServiceName = null,//all 

                },

                RequestContentHeader = new UnifreightIIG.Common.OutgoingMessageRequestServiceReference.RequestContentHeader()
                {
                    SenderID = 1,
                    TransmitionDateTime = DateTime.Now,
                    RecieverID = new int[] { 11 }

                }
            };
        }
    }
    class OutgoingMessageInterFace
    {
        internal NG_9101_MSG_OutgoingMessageResponseOutgoingMessage response;
        internal InterfaceTenantDefinitionManagementPM messageDCA;
        internal DCAFileModel dcaFile;
    }
    class MessageCorrelationSavedInDB
    {
        public int Tenant { get; set; }
        public string CorrelationId { get; set; }
        public DateTime DownloadAt { get; set; }

    }
}
