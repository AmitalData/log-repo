using Logitude.Customs.BL.EntityQueryServices;
using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using UnifreightIIG.Common.SearchResultsServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class SaveCH_MSG_195_SearchResultsRequestService : RequestServiceBase<CH_NG_195_MSG6_SearchResults, RequestParamsBase>
    {

        public override void OnRequestFail(RequestParamsBase requestParams)
        {
            base.OnRequestFail(requestParams);
        }

        public override CH_NG_195_MSG6_SearchResults GetRequest(RequestParamsBase requestParams)
        {
            var cH_NG_195_MSG6_SearchResults = new CH_NG_195_MSG6_SearchResults();
            cH_NG_195_MSG6_SearchResults.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };

            PhysicalCheckQueryService physicalCheckQueryService = new PhysicalCheckQueryService(requestParams.Tenant);

            var physicalCheck = physicalCheckQueryService.GetSingle(requestParams.LoggingEntityId, false, false);

            cH_NG_195_MSG6_SearchResults.GeneralDetails = new CH_NG_195_MSG6_SearchResultsGeneralDetails();

            cH_NG_195_MSG6_SearchResults.GeneralDetails.searchReasult = physicalCheck.

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            this.MyRequestSheetParam.EntityId1 = requestParams.DeclarationID;
            this.MyRequestSheetParam.CustomFileNo = requestParams.CustomFileNo;
            this.MyRequestSheetParam.RequestDescription = "בקשה לעדכון סגרים";

            return cH_NG_195_MSG6_SearchResults;
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
