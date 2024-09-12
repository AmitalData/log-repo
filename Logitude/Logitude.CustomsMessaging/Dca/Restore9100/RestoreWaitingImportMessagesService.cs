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

namespace Logitude.CustomsMessaging.Dca.Restore9100
{
    public class RestoreWaitingImportMessagesService
    {
        private static List<MessageCorrelationSavedInDB> _MessageCorrelationSavedInDBList = new List<MessageCorrelationSavedInDB>();
        private static HashSet<string> _BadMessagingServiceCode = new HashSet<string>();

        StringBuilder _StringBuilder = new StringBuilder();
        DateTime? _LastRetrive = null;
        private readonly CustomsSettingPM _CustomsSettingPM;
        private List<InterfaceTenantDefinitionManagementPM> _InterfaceListDCA;


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
            CustomsSettingPM customsSettingPM = (new CustomsSettingQueryService(6)).GetSingle("6", false, false);
            var interfaceTypeQueryService = new InterfaceTenantDefinitionQueryService(6);
            var _AllInterface = interfaceTypeQueryService.GetWithInterfaceManagementDefinition(customsSettingPM.Tenant);


            var _InterfaceListDCA = interfaceTypeQueryService.GetInterfaceListDCA(_AllInterface, customsSettingPM.CompanyType);


            var restoreWaitingImportService = new RestoreWaitingImportMessagesService(customsSettingPM, _InterfaceListDCA);
            restoreWaitingImportService.RestoreWaitingImportSaveInDB();

        }
        
        int Minutes2Retrieve()
        {
            const int C_Minutes2Retrieve = 15;
            string s=ConfigurationManager.AppSettings.Get("RestoreWaitingImportMessages:Minutes2Retrieve");
            int iMinutes2Retrieve = C_Minutes2Retrieve;
            if (int.TryParse(s,out iMinutes2Retrieve))
            {
                if (iMinutes2Retrieve>15 && iMinutes2Retrieve< 180)
                {
                    return iMinutes2Retrieve;
                }
            }
            return C_Minutes2Retrieve;
        }
        public void RestoreWaitingImportSaveInDB()
        {

            if (!String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["20230102.SuppressRestoreWaitingImportSaveInDB"]))
            {
                Debug.WriteLine($"Suppress RestoreWaitingImportSaveInDB");
                return;
            }
            Debug.WriteLine($"RestoreWaitingImportService.RestoreWaitingImportSaveInDB()");

            var dcaFilterByEnvironmentService = new DcaFilterByEnvironmentService();
            if (dcaFilterByEnvironmentService.GetDCAEnvPerTenant(_CustomsSettingPM.Tenant) != DcaFilterByEnvironment.Export)
            {
                Debug.WriteLine($"RestoreWaitingImportSaveInDB-only in export cloud");
                return;
            }
            try
            {


                _LastRetrive = _LastRetrive ?? DateTime.Now.AddMinutes(-1* Minutes2Retrieve());
                if (DateTime.Now.Date.Equals(new DateTime(2023,01,3)))
                {
                    ///_LastRetrive = DateTime.Now.AddDays(-31);
            }
                var request = GetRequest(_LastRetrive.Value);
                var sw = Stopwatch.StartNew();
                Debug.WriteLine($"RestoreWaitingImportSaveInDB-Send 9100 restore fromDate {request.GetOptions.fromDate}");
                var response = SendOutgoingMessageRequest(request);
                Debug.WriteLine($"RestoreWaitingImportSaveInDB-Send 9100 took {sw.Elapsed}");
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"t{_CustomsSettingPM.Tenant};fromDate {request.GetOptions.fromDate}; OutgoingMessage={response?.OutgoingMessage?.Length}");
                if (response?.OutgoingMessage?.Length == null || response?.OutgoingMessage?.Length == 0)
                {
                    Debug.WriteLine($"RestoreWaitingImportSaveInDB-OutgoingMessage == 0 - nothing todo");
                    return;
                }
                Debug.WriteLine($"RestoreWaitingImportSaveInDB-OutgoingMessage == {response?.OutgoingMessage?.Length}");

                var dOnlyNewImportMessagges = FilterOnlyNewImportMessagges(response);


                var outgoingMessage9100ResponseAnalyze = new OutgoingMessage9100ResponseAnalyze(_CustomsSettingPM, _InterfaceListDCA);
                outgoingMessage9100ResponseAnalyze.SaveInDB(dOnlyNewImportMessagges);


                outgoingMessage9100ResponseAnalyze.
                correlationIdsCanClear.ToList().
                    ForEach(r =>
                    _MessageCorrelationSavedInDBList.Add(
                        new MessageCorrelationSavedInDB() {
                        CorrelationId = r.CorrelationIDs,
                        DownloadAt = DateTime.Now,
                        Tenant = _CustomsSettingPM.Tenant
                        })
                    );
                if (outgoingMessage9100ResponseAnalyze.exceptionBag.Count>0)
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteError(String.Join(Environment.NewLine, outgoingMessage9100ResponseAnalyze.exceptionBag.ToList())+ $"RestoreWaitingImport_Error_T{_CustomsSettingPM.Tenant}");
                }
                if (outgoingMessage9100ResponseAnalyze.sbFilenameQueue.Count>0)
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteDebug(String.Join(Environment.NewLine, outgoingMessage9100ResponseAnalyze.sbFilenameQueue.ToList())+ $"RestoreWaitingImport_Files_T{_CustomsSettingPM.Tenant}");
                }
                

            }
            catch (System.Exception e)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(e, $"RestoreWaitingImport_BIGError_T{_CustomsSettingPM.Tenant}");
                throw;
            }
            finally
            {
                int min2delete = Minutes2Retrieve() + 2;
                var oldFiles = _MessageCorrelationSavedInDBList
                    .Where(r => r.Tenant == _CustomsSettingPM.Tenant)
                    .Where(r => DateTime.Now.Subtract(r.DownloadAt) > TimeSpan.FromMinutes(min2delete)).ToList();
                _MessageCorrelationSavedInDBList.RemoveAll(r => oldFiles.Contains(r));

            }

        }

        private List<NG_9101_MSG_OutgoingMessageResponseOutgoingMessage> FilterOnlyNewImportMessagges(NG_9101_MSG_OutgoingMessageResponse response)
        {
            var onlyImportMessages = response.OutgoingMessage.Where(r => !r.Filename.Contains("_EX_")).ToList();

            var listLast15MinImportCorrelationId = _MessageCorrelationSavedInDBList
                .Where(r => r.Tenant == _CustomsSettingPM.Tenant)
                .Select(r => r.CorrelationId).ToList();
            var newImportMessages = onlyImportMessages.Where(r => !listLast15MinImportCorrelationId.Contains(r.CorrelationId)).ToList();
            return newImportMessages;
            
        }



        private NG_9101_MSG_OutgoingMessageResponse SendOutgoingMessageRequest(NG_9100_MSG_OutgoingMessageRequest customRequest)
        {
            string RequestsSheetExternalId = Guid.NewGuid().ToString();
            NG_9101_MSG_OutgoingMessageResponse response = null;

            var myIIGGatewayMoreParams = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };



            var sendNG_9100_MSG_OutgoingMessageRequestService = new SendNG_9100_MSG_OutgoingMessageRequestService();
            var result = sendNG_9100_MSG_OutgoingMessageRequestService.CallWS(customRequest, this._CustomsSettingPM, myIIGGatewayMoreParams, RequestsSheetExternalId);
            
            return result.response;
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
    
    class MessageCorrelationSavedInDB
    {
        public int Tenant { get; set; }
        public string CorrelationId { get; set; }
        public DateTime DownloadAt { get; set; }

    }
}
