using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using System.Diagnostics;
using System;
using Logitude.AmitalMessaging.Utils;
using System.Collections.Generic;
using System.IO;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Server.Tools.Counters;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.ExternalServices;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.EntityKeys;
using Logitude.CustomsMessaging.Common.DCAParams;
using System.Threading;
using Logitude.Customs.Def.ClosedTable;
using Logitude.CustomsMessaging.Common.RequestParams;


namespace Logitude.CustomsMessaging.MessagingServices
{
    public abstract partial class MessagingServiceBase<TRequestParams, TResponseData, TCustomsRequest, TCustomsResponse, TRequestService, TResponseService, TRequestHeader>
: Logitude.CustomsMessaging.MessagingServices.IMessagingServiceBase<TRequestParams, TResponseData, TCustomsRequest, TCustomsResponse, TRequestHeader>
    {
        public Nullable<CustomsCommandEnum> CurrentCustomsCommandWR {get;set;}
        private TResponseData SendSheet(CustomsCommandEnum currentWR)
        {
            if (_CustomsRequestsSheetService == null)
            {
                throw new Exception("SendSheetStateMachine():(_CustomsRequestsSheetService == null)");
            }
            CurrentCustomsCommandWR = currentWR;

            _CustomsRequestsSheetService.CurrentWR = CustomsCommandEnum.CustomsCommandGetCustomRequestWR;


            return null;
        }

    }
}
