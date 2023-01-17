using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.CustomItemDetailsServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class Get_CB_MSG_8314_8888_CustomItemDetailsHeaderRequestService: RequestServiceBase<CB_NG_8314_CustomItemDetailsHeaderIn, GenericRequestParams>
    {
        public override CB_NG_8314_CustomItemDetailsHeaderIn GetRequest(GenericRequestParams requestParams)
        {
           var myMsg=new CB_NG_8314_CustomItemDetailsHeaderIn();

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.CustomsItem");
            this.MyRequestSheetParam.EntityId1 = requestParams.AppicationId;
            //this.MyRequestSheetParam.CustomFileNo= requestParams.c
            this.MyRequestSheetParam.RequestDescription = "נתוני פרט מכס";
            return myMsg;
           
        }
    }
}
