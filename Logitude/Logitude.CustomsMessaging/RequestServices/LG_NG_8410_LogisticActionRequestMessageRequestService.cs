using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.CargoTracking;
using Logitude.CustomsMessaging.RequestServices;
using UnifreightIIG.Common.LogisticActionRequestMessageDecision;
using UnifreightIIG.Common.MessageLib.LogisticActionRequestMessage;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class LG_NG_8410_LogisticActionRequestMessageRequestService : RequestServiceBase<LG_NG_8410_LogisticActionRequestMessage, LogisticActionRequestRequestParams>
    {
        public override LG_NG_8410_LogisticActionRequestMessage GetRequest(LogisticActionRequestRequestParams p)
        {
            var myMsg = new LG_NG_8410_LogisticActionRequestMessage()
            {
                GeneralDetails = new LG_NG_8410_LogisticActionRequestMessageGeneralDetails()
                {
                    ExporterIdentifierType = p.ExporterIdentifierType,
                    ExporterNumber = p.ExporterNumber,
                    PassportCountry = p.PassportCountry,
                    PassportNumber = p.PassportNumber,
                    RequestType = p.RequestType,
                    RequestReason = p.RequestReason,
                    DeliverySiteID = p.DeliverySiteID,
                    CargoIdentifier =  new UnifreightIIG.Common.MessageLib.LogisticActionRequestMessage.cargoIdentifier() 
                    {
                        cargoIdentifierKey1 = p.CargoIdentifierKey1,
                        cargoIdentifierKey2 = p.CargoIdentifierKey2,
                        cargoIdentifierKey3 = p.CargoIdentifierKey3,
                        cargoIdentifierType = p.CargoIdentifierType,
                    },
                    PackingDetails = new LG_NG_8410_LogisticActionRequestMessageGeneralDetailsPackingDetails() 
                    {
                        PackagingTypeCode = p.PackagingTypeCode,
                        Quantity = p.Quantity,
                    }
                },
                Attachments = new Attachment[] { }   // need add
            };

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.LogisticActionRequest");
            this.MyRequestSheetParam.EntityId1 = p.LogisticActionRequestId;
            this.MyRequestSheetParam.CustomFileNo = p.CustomsFile;
            this.MyRequestSheetParam.RequestDescription = "בקשת ביטול יצוא";

            return myMsg;
        }

    }
}
