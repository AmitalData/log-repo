
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.Testers.Messages;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.SystemTableServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class UniCourierBatchSend1170_MsgResponseService : ResponseServiceBase
        //<TResponseData, TCustomResponse, TRequestParams>
        <INF_MSG_GenericResponseData, DCAInUCB1170WithResponseContentHeader, GenericRequestParams>
    {
        private Customs.Def.EntityPMs.DeclarationPM _MyDeclarationPM;
        public override void Update(DCAInUCB1170WithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {

            var mess = new StringBuilder();
            var context = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(context);
            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            this.MyResponseData = new INF_MSG_GenericResponseData();

            var objectTableId =
    //ObjectTableRepository.GetObjectTableByName("Customs.CustomsClosedTable");
    ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            var objectTableIdCourierMaster = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");

            var qs = new DeclarationCourierStatusQueryService(context);
            List<DeclarationCourierStatusPM> listPM = new List<DeclarationCourierStatusPM>();
            if (customResponse.DeclarationsList != null && customResponse.DeclarationsList.Count > 0)
            {
                listPM = qs.GetDeclarationsByIds(customResponse.DeclarationsList, requestParams.Tenant);
            }
            else
            {
                listPM = qs.GetByMasterIDCourierManifestStatusCode(requestParams.Tenant, requestParams.AppicationId, "R",
                   customResponse.SelectedBOLValue,
                   customResponse.SelectedStatusValue,
                   customResponse.SelectedTotalInvoiceValue,
                   customResponse.SelectedFastIndividualProcessValue);
                if (customResponse.CourierDeclarationStatusCode == "RV")
                {
                    var listPM2 = qs.GetByMasterIDCourierManifestStatusCode(requestParams.Tenant, requestParams.AppicationId, "V", customResponse.SelectedBOLValue,
                    customResponse.SelectedStatusValue,
                    customResponse.SelectedTotalInvoiceValue,
                    customResponse.SelectedFastIndividualProcessValue);
                    listPM = listPM.Concat(listPM2).ToList();
                }
            }
            if (listPM.Count == 0)
            {
                mess.AppendLine($"There ARE  NOT any Declarations 'R'eady to (Manifest) send for master {requestParams.AppicationId} ");
            }
            else
            {

                listPM = listPM.Where(r => (r.CourierPaymentStatusCode == "R" || string.IsNullOrWhiteSpace(r.CourierPaymentStatusCode))).ToList();
                if (listPM.Count == 0)
                {
                    mess.AppendLine($"יש להוסיף בדיקה לשדר מצהר תקינים ושדר הצהרה תקינים שרק הצהרות שלא שולמו ישלחו  {requestParams.AppicationId} ");
                }
                else
                {
                    //string inList= String.Join(",", listPM.Select(r => $"'{r.DeclarationId}'").ToArray());
                    //string updateSql = $"Update DeclarationCourierStatuses set COURIERMANIFESTSTATUSCODE='I' where DECLARATIONID in ({inList}) ";

                    //(context as CustomContext).CommandExecuteNonQuery(requestParams.Tenant,updateSql);



                    listPM.ChunkBy(100)
.ForEach(list100 =>
{
string inList = String.Join(",", list100.Select(r => $"'{r.DeclarationId}'").ToArray());
string updateSql = $"Update DeclarationCourierStatuses set COURIERMANIFESTSTATUSCODE='I' where DECLARATIONID in ({inList}) ";

(context as CustomContext).CommandExecuteNonQuery(requestParams.Tenant, updateSql);
});

                }
            }

            foreach (var itemPM in listPM)
            {
                try
                {
                    var requestParams1170 = new MANIFESTRequestRequestParams()
                    {
                        Tenant = requestParams.Tenant,
                        //IsFakeResponse = true,
                        //RequestName = requestName,
                        //ResponseName = responseName,
                        LoggingEnabled = true,
                        LoggingObjectTableId = objectTableId,
                        LoggingEntityId = itemPM.DeclarationId,
                        LoggingObjectTableId2 = requestParams.LoggingObjectTableId,
                        LoggingEntityId2 = objectTableIdCourierMaster,
                        //AppicationId = itemPM.DeclarationId,
                        InterfaceTypeCode = "1170",

                        //LoggingEntityReference = declarationNumber,
                        LoggingUserId = requestParams.LoggingUserId,
                        RequestVIA = SendRequestVIA.WebServiceBatch,


                        DeclarationId = itemPM.DeclarationId,
                        LoggingEntityReference = itemPM.DeclarationId,
                        //ImportManifest =""

                    };

                    SBQMessageService.CreateSheetSBQMessage<MANIFESTRequestRequestParams>(requestParams1170, false);
                    LogMessagingUtil.Instance.AppendLine($" CreateSheetSBQMessage({itemPM.DeclarationId})");
                    mess.AppendLine($" CreateSheetSBQMessage({itemPM.DeclarationId})");
                }
                catch (System.Exception ee1)
                {

                    LogMessagingUtil.Instance.AppendLine($"Exception!!!CreateSheetSBQMessage({itemPM.DeclarationId}) : {ee1.Message}");
                    mess.AppendLine($"Exception!!!CreateSheetSBQMessage({itemPM.DeclarationId}) : {ee1.Message}");
                }
            }

            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();

            //this.MyRequestSheetParam.RequestDescription = "Build Custom Zip File";
            ///LogitudeSettings.HandleBuildObjectTablesZipFilesData_Inject(false, true);

            this.MyRequestSheetParam.RequestDescription = requestParams.RequestName;
            this.MyResponseData.UserMessage = mess.ToString();
            this.MyResponseData.Succeeded = true;
        }

        public override INF_MSG_GenericResponseData GetResponse(DCAInUCB1170WithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }
        

    }
}
