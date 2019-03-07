using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.GatepassFeedbackMServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class GP_1030_GatepassRequestMessageRequestService : RequestServiceBase<GP_NG_1030_MSG1_GatepassRequestMessage, GatepassRequestMessageRequestParams>
    {
        private CourierMasterPM _CourierMasterPM;

        public override GP_NG_1030_MSG1_GatepassRequestMessage GetRequest(GatepassRequestMessageRequestParams requestParams)
        {
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            CourierMasterQueryService myCourierMasterQueryService = new CourierMasterQueryService(dbContext);
            GatepassRequestQueryService myGatepassRequestQueryService = new GatepassRequestQueryService(dbContext);
            _CourierMasterPM = myCourierMasterQueryService.GetSingle(requestParams.MasterCourierId, true, false);

            var myGP_NG_1030_MSG1_GatepassRequestMessage = new GP_NG_1030_MSG1_GatepassRequestMessage();
            myGP_NG_1030_MSG1_GatepassRequestMessage.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };

            List<GP_NG_1030_MSG1_GatepassRequestMessageGatepassRequestMessage> myGatepassRequestMessageList = new List<GP_NG_1030_MSG1_GatepassRequestMessageGatepassRequestMessage>();
            GP_NG_1030_MSG1_GatepassRequestMessageGatepassRequestMessage myGatepassRequestMessage = new GP_NG_1030_MSG1_GatepassRequestMessageGatepassRequestMessage();
            myGatepassRequestMessage.CargoIdentifier = new cargoIdentifier();
            myGatepassRequestMessage.CargoIdentifier.cargoIdentifierType = 1;
            myGatepassRequestMessage.CargoIdentifier.cargoIdentifierKey1 = _CourierMasterPM.DepartureDate.Value.Year.ToString();
            myGatepassRequestMessage.CargoIdentifier.cargoIdentifierKey2 = _CourierMasterPM.MAWB;
            myGatepassRequestMessage.CargoIdentifier.cargoIdentifierKey3 = _CourierMasterPM.HAWB;
            myGatepassRequestMessage.exportFromDifferentPortIndication = false;

            GatepassRequestPM myGatepassRequest = myGatepassRequestQueryService.GetSingle(requestParams.MasterCourierId, false, false);
            myGatepassRequestMessage.gatepassNumber = myGatepassRequest.GatepassNumber;

            CustomsSettingQueryService customsSettingQuery = new CustomsSettingQueryService(dbContext);
            CustomsSettingPM CustomsSetting = customsSettingQuery.GetSingleByTenant(_CourierMasterPM.Tenant);
            myGatepassRequestMessage.ExternalID = CustomsSetting.CustomsAgentId;

            myGatepassRequestMessage.originSiteCode = myGatepassRequest.OriginSiteCode;
            myGatepassRequestMessage.processTypeCode = 1;
            myGatepassRequestMessage.requestDate = DateTime.Now;
            myGatepassRequestMessage.customerActivityType = 7;
            int updateCode;
            int.TryParse(requestParams.UpdateCode, out updateCode);
            myGatepassRequestMessage.updateCode = updateCode;

            List<GP_NG_1030_MSG1_GatepassRequestMessageGatepassRequestMessageGatepassDestinationSite> myGatepassDestinationSiteList = new List<GP_NG_1030_MSG1_GatepassRequestMessageGatepassRequestMessageGatepassDestinationSite>();
            GP_NG_1030_MSG1_GatepassRequestMessageGatepassRequestMessageGatepassDestinationSite myGatepassDestinationSite = new GP_NG_1030_MSG1_GatepassRequestMessageGatepassRequestMessageGatepassDestinationSite();
            myGatepassDestinationSite.designateSiteCode = myGatepassRequest.DesignateSiteCode;
            int transportationTypeCode;
            int.TryParse(myGatepassRequest.TransportationTypeCode, out transportationTypeCode);
            myGatepassDestinationSite.transportationTypeCode = transportationTypeCode;
            myGatepassDestinationSite.isFinalDestination = false;

            myGatepassDestinationSiteList.Add(myGatepassDestinationSite);
            myGatepassRequestMessage.GatepassDestinationSite = myGatepassDestinationSiteList.ToArray(); ;


            myGatepassRequestMessageList.Add(myGatepassRequestMessage);
            myGP_NG_1030_MSG1_GatepassRequestMessage.GatepassRequestMessage = myGatepassRequestMessageList.ToArray();

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = "בקשת העברה ";

            return myGP_NG_1030_MSG1_GatepassRequestMessage;
        }
    }
}
