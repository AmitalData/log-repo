using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.AmitalMessaging.Infrastructure;
using Logitude.AmitalMessaging.Utils;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.U2L.Sivug;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class Unifreight_LOGISIVUGResponseService
        : ResponseServiceBase<
         Unifreight_L2US01ResponseData, LOGISIVUGWithResponseContentHeader, Unifreight_L2US01RequestParam>

    {
        private Logitude.CustomsMessaging.U2L.Sivug.SivugUpsertService _SivugUpsertService;

        public override void Update(LOGISIVUGWithResponseContentHeader UnifreightResponse, Unifreight_L2US01RequestParam requestParams)
        {
            if (DateTime.Now < new DateTime(2017, 01, 15))
            {
                System.Threading.Thread.Sleep(5000);
            }
            //throw new NotImplementedException();
            this.MyResponseData = new Unifreight_L2US01ResponseData();
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = true;

            _SivugUpsertService = new SivugUpsertService();
            _SivugUpsertService.RequestParams = requestParams;
            string moreParams= UnifreightResponse.MyMoreParams;
            string messageOut="";
            _SivugUpsertService.ProccessGenericRequestObj(UnifreightResponse.MyLOGISIVUG,
                ref  moreParams,
                out messageOut);

            this.MyResponseData.HasException = false;
            this.MyResponseData.UserMessage = messageOut;
            if (this.MyRequestSheetParam == null)this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = "מסר סיווג פריטים " + _SivugUpsertService.RequestParams.CFIFILEMFileNo;
            if (!String.IsNullOrWhiteSpace(moreParams))
            {
                var unifreightListsParams = UnifreightListsUtil.Deserialize(moreParams);
                var mode = UnifreightListsUtil.GetValue(ref unifreightListsParams, "MODE");
                if (mode == "UMS2L")
                {
                    this.MyRequestSheetParam.RequestDescription = "עדכון נתונים ממערכת סיווג " + _SivugUpsertService.RequestParams.CFIFILEMFileNo;
                }
            }
            this.MyRequestSheetParam.EntityId1 = _SivugUpsertService.RequestParams.DeclarationId;
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
        }



        public override Unifreight_L2US01ResponseData GetResponse(LOGISIVUGWithResponseContentHeader customResponse, Unifreight_L2US01RequestParam requestParams)
        {
            return this.MyResponseData;
        }


        /*
        public override Unifreight_L2US01ResponseData GetResponse(LOGISIVUGWithResponseContentHeader customResponse, Unifreight_L2US01RequestParam requestParams)
        {

            var amitalCustomFileCommunicationModel = new Logitude.Customs.BL.Messaging.Amital.AmitalCommunicationModelBase(
                Logitude.Server.Tools.Models.AmitalStandardCommunicationModel.OperationMethod.DataAccess,
                //"CWSFLOGIFILE"
                //"CWSFLOGISIVUG"
                "CFIUSIVUG"
                ,
                //"DeclarationUpsertPut"
                //"DeclarationSetL2US01"
                "SivugUpsertBatchPut"
                )
            {
                Tenant = requestParams.Tenant,
                objectTableName = "Customs.Declaration",
                CommunicationLoggingEntityReference = requestParams.CFIFILEMFileNo,
                EntityId = requestParams.CFIFILEMFileNo,
                UserId = requestParams.LoggingUserId,
                CommunicationSubject = "Unifreight_LOGISIVUGResponseService",

            };

            var myUServerCommunicationService = new Logitude.Customs.BL.Messaging.Amital.UServerCommunicationService
                <Logitude.Customs.BL.Messaging.Amital.AmitalCommunicationModelBase, GenericResponseObj>(
                amitalCustomFileCommunicationModel, _SivugUpsertService.MyGenericResponseObj);
            bool syn = true;
            if (DateTime.Now < new DateTime(2014, 04, 01))
            {
                syn = false;// due uroter 54 use 9501 instead 96 
            }

            var info = myUServerCommunicationService.Send(syn);

            LogMessagingUtil.Instance.AppendLine("UServerCommunicationCustomFileService CommunicationMessage =" + info.ImmediatelyMessage ?? "NULL");
            LogMessagingUtil.Instance.AppendLine("UServerCommunicationCustomFileService CommunicationLogId =" + info.CommunicationLogId ?? "NULL");

            this.MyRequestSheetParam = new RequestSheetParam()
            {
                RequestDescription = "Sivug upsert " + requestParams.CFIFILEMFileNo ,
                 CustomFileNo = requestParams.CFIFILEMFileNo,
            };
            
            return new Unifreight_L2US01ResponseData()
            {
                MyGenricResponse = 
                //_SivugUpsertService.MyGenericResponseObj
                info.GenericResponseObj.ResponseXml
            };
        }*/
    }
}
