using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.VehicleInServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class VP_NG_2691_MSG101_VehicleInResponseService : ResponseServiceBase
        <INF_MSG_GenericResponseData, VP_NG_2691_MSG101_VehicleInResponse, UpdateDeleteVehicleRequestParams>
    {
        public VehiclePM _MyVehicle { get; set; }

        public override void Update(VP_NG_2691_MSG101_VehicleInResponse customResponse, UpdateDeleteVehicleRequestParams requestParams)
        {
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            var myVehicleQueryService = new VehicleQueryService(dbContext);
            var myVehicleUpdateService = new VehicleUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            string exceptionMessage = "";

            this.MyResponseData = new INF_MSG_GenericResponseData();
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.ApplicationID = requestParams.VehicleId;

            if (customResponse.ResponseContentHeader.Exception != null)
            {
                foreach (var exceptionItem in customResponse.ResponseContentHeader.Exception)
                {
                    exceptionMessage = string.Concat(exceptionMessage, "\n", exceptionItem.ExeptionDescription);
                }
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = exceptionMessage;
                LogMessagingUtil.Instance.AppendLine("VehicleInResponse has exception:\n" + exceptionMessage);
                return;
            }

            if (customResponse.VehicleDetails != null)
            {
                if (string.IsNullOrWhiteSpace(requestParams.VehicleId))
                {
                    requestParams.VehicleId = myVehicleQueryService.GetVehicleIdByChassisNumber(customResponse.VehicleDetails.FirstOrDefault().vehicleChassisNumber, requestParams.Tenant);
                }
                _MyVehicle = myVehicleQueryService.GetSingle(requestParams.VehicleId, true, false);
                if (_MyVehicle == null)
                {
                    this.MyResponseData.HasException = true;
                    this.MyResponseData.UserMessage = "לא נמצאה שלדה במערכת " + customResponse.VehicleDetails.FirstOrDefault().vehicleChassisNumber;
                    LogMessagingUtil.Instance.AppendLine("Can not find vehicle, ChassisNumber: " + customResponse.VehicleDetails.FirstOrDefault().vehicleChassisNumber);
                    return;
                }

                if (customResponse.VehicleDetails.FirstOrDefault().richbitFileNumber != null && customResponse.VehicleDetails.FirstOrDefault().richbitFileNumber !=0)
                {
                    _MyVehicle.RichbitFileNumber = customResponse.VehicleDetails.FirstOrDefault().richbitFileNumber.ToString();
                }
                _MyVehicle.StatusCode = "2";
            }
            else if (customResponse.VehicleToDelete != null)
            {
                if (string.IsNullOrWhiteSpace(requestParams.VehicleId))
                {
                    requestParams.VehicleId = myVehicleQueryService.GetVehicleIdByRichbitFileNumber(customResponse.VehicleToDelete.FirstOrDefault().richbitFileNumberToDelete.ToString(), requestParams.Tenant);
                }
                _MyVehicle = myVehicleQueryService.GetSingle(requestParams.VehicleId, true, false);
                if (_MyVehicle == null)
                {
                    this.MyResponseData.HasException = true;
                    this.MyResponseData.UserMessage = "לא נמצאה שלדה במערכת " + customResponse.VehicleToDelete.FirstOrDefault().richbitFileNumberToDelete;
                    LogMessagingUtil.Instance.AppendLine("Can not find vehicle, RichbitFileNumber: " + customResponse.VehicleToDelete.FirstOrDefault().richbitFileNumberToDelete);
                    return;
                }
                _MyVehicle.StatusCode = "4";
            }

            _MyVehicle.ChangeSetOp = ChangeSetOperation.Update;
            myVehicleUpdateService.Update(this._MyVehicle, true);

            this.MyResponseData.HasException = false;
            this.MyResponseData.ApplicationID = requestParams.VehicleId;
            this.MyResponseData.UserMessage = "עודכנו נתוני רכב" + @"
מספר שלדה: " + _MyVehicle.VehicleChassisNumber;
            if (!string.IsNullOrWhiteSpace(_MyVehicle.RichbitFileNumber))
            {
                this.MyResponseData.UserMessage = this.MyResponseData.UserMessage + @"
מס' תיק ריכבית: " + _MyVehicle.RichbitFileNumber;
            }
        }

        public override INF_MSG_GenericResponseData GetResponse(VP_NG_2691_MSG101_VehicleInResponse customResponse, UpdateDeleteVehicleRequestParams requestParams)
        {
            return this.MyResponseData;
        }
    }
}
