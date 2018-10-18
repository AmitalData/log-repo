using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.Testers.Messages;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class UniDebug01_MsgResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, UniDebug01_Msg, GenericRequestParams>
    {
        private Customs.Def.EntityPMs.DeclarationPM _MyDeclarationPM;
        public override void Update(UniDebug01_Msg customResponse, GenericRequestParams requestParams)
        {
            
            var context = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(context);
            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            this.MyResponseData = new INF_MSG_GenericResponseData();

            if (customResponse.ToFailAnalyze)
            {
                throw new Exception("customResponse.ToFailAnalyze !!!");
            }

            requestParams.AppicationId = myDeclarationQueryService.GetIdByDeclarationNumber(customResponse.DeclarationID, requestParams.Tenant);
            
            
            this._MyDeclarationPM = myDeclarationQueryService.GetSingle(requestParams.AppicationId, true, false);
            _MyDeclarationPM.ChangeSetOp=  ChangeSetOperation.Update;
            _MyDeclarationPM.UserNotes =  _MyDeclarationPM.UserNotes ??"";
            var dt=TenantServerConfigration.GetCurrentDateTime(requestParams.Tenant);
            var myRemarks = "T" + customResponse.Remarks + "A" + dt.ToString("yyyy-MM-dd HH:mm:ss.fff",
                                            CultureInfo.InvariantCulture);
                
            //_MyDeclarationPM.UserNotes += myRemarks;



            this.MyResponseData.UserMessage += Environment.NewLine + myRemarks;
            //_MyDeclarationPM.Consignments[0].CargoDescription += myRemarks;
            //_MyDeclarationPM.Consignments[0].ChangeSetOp = ChangeSetOperation.Update;

            myDeclarationUpdateService.Update(_MyDeclarationPM, true);

            this.MyRequestSheetParam = this.MyRequestSheetParam ??new RequestSheetParam();
            this.MyRequestSheetParam.CustomFileNo = _MyDeclarationPM. CustomFileNo;
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            this.MyRequestSheetParam.EntityId1 = _MyDeclarationPM.Id;
            this.MyRequestSheetParam.RequestDescription = myRemarks;


            this.MyResponseData.Succeeded = true;
        }

        public override INF_MSG_GenericResponseData GetResponse(UniDebug01_Msg customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        
    }
}
