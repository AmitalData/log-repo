
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
using UnifreightIIG.Common.SystemTableServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class UniCustomTableZip_MsgResponseService : ResponseServiceBase
        //<TResponseData, TCustomResponse, TRequestParams>
        <INF_MSG_GenericResponseData, SYSTBL_NG_9001_MSG_SystemTablesResponse, GenericRequestParams>
    {
        private Customs.Def.EntityPMs.DeclarationPM _MyDeclarationPM;
        public override void Update(SYSTBL_NG_9001_MSG_SystemTablesResponse customResponse, GenericRequestParams requestParams)
        {

            var context = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(context);
            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            this.MyResponseData = new INF_MSG_GenericResponseData();

            
            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();
            //this.MyRequestSheetParam.CustomFileNo = _MyDeclarationPM.CustomFileNo;
            //this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            //this.MyRequestSheetParam.EntityId1 = _MyDeclarationPM.Id;
            this.MyRequestSheetParam.RequestDescription = "Build Custom Zip File";
            LogitudeSettings.HandleBuildObjectTablesZipFilesData_Inject(false, true);
            

            this.MyResponseData.Succeeded = true;
        }

        public override INF_MSG_GenericResponseData GetResponse(SYSTBL_NG_9001_MSG_SystemTablesResponse customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }


    }
}
