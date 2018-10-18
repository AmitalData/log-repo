using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ImportDeclarationCancellationReplyMsgServiceReference;
using UnifreightIIG.Common.MessageLib.Fault;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class DF_5018_ImportExportDeclarationCancellationResponseService : ResponseServiceBase
        <INF_MSG_GenericResponseData,
        DF_NG_5018_MSG14004_ImportDeclarationCancellationReplyMsg,
        GenericRequestParams>
    {
        public DeclarationPM _MyDeclarationPM;

        public override void Update(DF_NG_5018_MSG14004_ImportDeclarationCancellationReplyMsg customResponse, GenericRequestParams requestParams)
        {
            throw new NotImplementedException();
        }
#if false
        לפי גלעד , שם השירות של מסר 5018 הוחלף בחדש ל5118 .
         public override void Update(DF_NG_5018_MSG14004_ImportDeclarationCancellationReplyMsg customResponse, GenericRequestParams requestParams)
        {
            //Analyze message 5018- Import Declaration Cancellation Reply (DCA)
            ICustomContext customContext = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(customContext);
            var myDeclarationUpdateService = new DeclarationUpdateService(customContext, new Dictionary<string, IContext>(), requestParams.Tenant);

            string declarationId = myDeclarationQueryService.GetIdByDeclarationNumber(customResponse.Response.Declaration.ID.Value, requestParams.Tenant);
            if (string.IsNullOrWhiteSpace(declarationId))
            {
                LogMessagingUtil.Instance.AppendLine("Can not find declaration" + customResponse.Response.Declaration.ID.Value);
                this.MyResponseData = new INF_MSG_GenericResponseData();
                this.MyResponseData.ApplicationID = declarationId;
                this.MyResponseData.Succeeded = false;
                this.MyResponseData.UserMessage = "Can not find declaration" + customResponse.Response.Declaration.ID.Value;
                return;
            }

            if (customResponse.Response.FunctionCode.Value != "2")
            {
                this.MyResponseData = new INF_MSG_GenericResponseData();
                this.MyResponseData.ApplicationID = declarationId;
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.UserMessage = "Not Cancellation Message- FunctionCode value must be 2";
            }

            this._MyDeclarationPM = myDeclarationQueryService.GetSingle(declarationId, true, false);
            this._MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
            this._MyDeclarationPM.DeclarationStatusTypeCode = customResponse.Response.Status.NameCode.Value;

            EventContextTagModel myInsertEventContextTagModel = new EventContextTagModel();
            myInsertEventContextTagModel.CallProccessID = EventContextTagModel.ProccessEnum.DF_NG_5018_MSG14004_ImportDeclarationCancellation;
            myInsertEventContextTagModel.EventCode = "DCN";
            myInsertEventContextTagModel.EventRemarks = "Declaration Cancellation";
            this._MyDeclarationPM.CurrentContextTag = myInsertEventContextTagModel;

            myDeclarationUpdateService.Update(this._MyDeclarationPM, true);

            if (customResponse.ProceduralFaultMsg != null)
            {
                var headerXml = XmlGenericUtil<UnifreightIIG.Common.ImportDeclarationCancellationReplyMsgServiceReference.ResponseContentHeader>.SerializeObject(customResponse.ResponseContentHeader);
                var header = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Fault.RequestContentHeader>.DeSerializeObject(headerXml);
                var requestXml = XmlGenericUtil<UnifreightIIG.Common.ImportDeclarationCancellationReplyMsgServiceReference.ProceduralFaultDetails[]>.SerializeObject(customResponse.ProceduralFaultMsg);
                var proceduralFaulxml = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Fault.ProceduralFaultDetails[]>.DeSerializeObject(requestXml);

                EV_NG_8218_MSG14100_ProceduralFaultMsg myEV_NG_8218_MSG14100_ProceduralFaultMsg = new EV_NG_8218_MSG14100_ProceduralFaultMsg();
                myEV_NG_8218_MSG14100_ProceduralFaultMsg.RequestContentHeader = header;
                myEV_NG_8218_MSG14100_ProceduralFaultMsg.ProceduralFaultDetails = proceduralFaulxml;

                var xml = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Fault.EV_NG_8218_MSG14100_ProceduralFaultMsg>.SerializeObject(myEV_NG_8218_MSG14100_ProceduralFaultMsg);
                var ser = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Fault.EV_NG_8218_MSG14100_ProceduralFaultMsg>.DeSerializeObject(xml);
                var proceduralFaultMsgResponseService = new EV_NG_8218_MSG14100_ProceduralFaultMsgResponseService();
                proceduralFaultMsgResponseService.Update(ser, requestParams);
            }

            this.MyResponseData = new INF_MSG_GenericResponseData();
            this.MyResponseData.ApplicationID = this._MyDeclarationPM.Id;
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTabelRepository.GetObjectTableByName("Customs.Declaration");
            this.MyRequestSheetParam.EntityId1 = this._MyDeclarationPM.Id;
            this.MyRequestSheetParam.RequestDescription = "תיקון / ביטול הצהרה" + this._MyDeclarationPM.DeclarationNumber;
        }
        לפי גלעד , שם השירות של מסר 5018 הוחלף בחדש ל5118 .
#endif
        public override INF_MSG_GenericResponseData GetResponse(DF_NG_5018_MSG14004_ImportDeclarationCancellationReplyMsg customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }
    }
}
