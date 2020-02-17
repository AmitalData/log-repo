using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.SealUpdateServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class SE_6001_SealUpdateRequestService : RequestServiceBase<SE_NG_6001_MSG01_SealUpdateMessage, CargoSealsRequestParams>
    {
        public override SE_NG_6001_MSG01_SealUpdateMessage GetRequest(CargoSealsRequestParams requestParams)
        {
            var mySE_NG_6001_MSG01_SealUpdateMessage = new SE_NG_6001_MSG01_SealUpdateMessage();
            mySE_NG_6001_MSG01_SealUpdateMessage.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };

            mySE_NG_6001_MSG01_SealUpdateMessage.General = new SE_NG_6001_MSG01_SealUpdateMessageGeneral();
            int cargoRowNumber = 0;
            int.TryParse(requestParams.CargoRowNumber, out cargoRowNumber);
            mySE_NG_6001_MSG01_SealUpdateMessage.General.cargoRowNumber = cargoRowNumber;
            mySE_NG_6001_MSG01_SealUpdateMessage.General.containerNumber = requestParams.ContainerNumber;
            mySE_NG_6001_MSG01_SealUpdateMessage.General.updateDate = requestParams.UpdateDate;
            int customerExternalId = 0;
            int.TryParse(requestParams.ImporterNumber, out customerExternalId);
            mySE_NG_6001_MSG01_SealUpdateMessage.General.customerExternalId = customerExternalId;
            if(customerExternalId!= 0) mySE_NG_6001_MSG01_SealUpdateMessage.General.customerExternalIdSpecified = true;
            // customerExternalIdSpecified
            mySE_NG_6001_MSG01_SealUpdateMessage.General.customerActivityType = 4;
            mySE_NG_6001_MSG01_SealUpdateMessage.General.cargoIdentifier = new cargoIdentifier();
            int cargoIdentifier = 0;
            int.TryParse(requestParams.CargoIdentifierTypeCode, out cargoIdentifier);
            mySE_NG_6001_MSG01_SealUpdateMessage.General.cargoIdentifier.cargoIdentifierType = cargoIdentifier;
            mySE_NG_6001_MSG01_SealUpdateMessage.General.cargoIdentifier.cargoIdentifierKey1 = requestParams.CargoIdentifierKey1;
            mySE_NG_6001_MSG01_SealUpdateMessage.General.cargoIdentifier.cargoIdentifierKey2 = requestParams.CargoIdentifierKey2;
            mySE_NG_6001_MSG01_SealUpdateMessage.General.cargoIdentifier.cargoIdentifierKey3 = requestParams.CargoIdentifierKey3;

            mySE_NG_6001_MSG01_SealUpdateMessage.UpdatedSeals = GetUpdatedSealsList(requestParams);

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            this.MyRequestSheetParam.EntityId1 = requestParams.DeclarationID;
            this.MyRequestSheetParam.CustomFileNo = requestParams.CustomFileNo;
            this.MyRequestSheetParam.RequestDescription = "בקשה לעדכון סגרים";

            return mySE_NG_6001_MSG01_SealUpdateMessage;
        }

        private SE_NG_6001_MSG01_SealUpdateMessageUpdatedSeals[] GetUpdatedSealsList(CargoSealsRequestParams requestParams)
        {
            List<SE_NG_6001_MSG01_SealUpdateMessageUpdatedSeals> myCargoSealDetailsList = new List<SE_NG_6001_MSG01_SealUpdateMessageUpdatedSeals>();
            foreach (var sealItem in requestParams.CargoSealList)
            {
                SE_NG_6001_MSG01_SealUpdateMessageUpdatedSeals myCargoSealDetails = new SE_NG_6001_MSG01_SealUpdateMessageUpdatedSeals();
                myCargoSealDetails.remarks = sealItem.Remarks;
                int sealCompletenessState = 0;
                int.TryParse(sealItem.SealCompletenessStateCode, out sealCompletenessState);
                myCargoSealDetails.sealCompletenessState = sealCompletenessState;
                myCargoSealDetails.sealNumber = sealItem.SealNumber;
                int sealType = 0;
                int.TryParse(sealItem.SealTypeCode, out sealType);
                myCargoSealDetails.sealType = sealType;
                int updateReason = 0;
                int.TryParse(sealItem.UpdateReasonCode, out updateReason);
                myCargoSealDetails.updateReason = updateReason;
                int updateType = 0;
                int.TryParse(sealItem.UpdateTypeCode, out updateType);
                myCargoSealDetails.updateType = updateType;
                myCargoSealDetailsList.Add(myCargoSealDetails);
            }
            return myCargoSealDetailsList.ToArray();
        }
    }
}
