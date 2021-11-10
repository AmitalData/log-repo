using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
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
    public class SaveCH_MSG_195_SearchResultsRequestService : RequestServiceBase<CH_NG_195_MSG6_SearchResults, GenericRequestParams>
    {

        public override void OnRequestFail(GenericRequestParams requestParams)
        {
            base.OnRequestFail(requestParams);
        }

        public override CH_NG_195_MSG6_SearchResults GetRequest(GenericRequestParams requestParams)
        {
            var cH_NG_195_MSG6_SearchResults = new CH_NG_195_MSG6_SearchResults();
            cH_NG_195_MSG6_SearchResults.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };

            PhysicalCheckQueryService physicalCheckQueryService = new PhysicalCheckQueryService(requestParams.Tenant);

            var physicalCheck = physicalCheckQueryService.GetSingle(requestParams.LoggingEntityId, false, false);

            cH_NG_195_MSG6_SearchResults.GeneralDetails = new CH_NG_195_MSG6_SearchResultsGeneralDetails();
            CustomsSettingQueryService customsSettingQuery = new CustomsSettingQueryService(requestParams.Tenant);
            CustomsSettingPM CustomsSetting = customsSettingQuery.GetSingleByTenant(requestParams.Tenant);

            cH_NG_195_MSG6_SearchResults.GeneralDetails.customsAgent = Convert.ToInt32(CustomsSetting.CustomsAgentId);
            cH_NG_195_MSG6_SearchResults.GeneralDetails.customsAgentSpecified = true;
            cH_NG_195_MSG6_SearchResults.GeneralDetails.searchReasult = Convert.ToInt32( physicalCheck.SearchResult);
            cH_NG_195_MSG6_SearchResults.GeneralDetails.SealNumber = physicalCheck.SealNumber;
            if(string.IsNullOrEmpty(physicalCheck.CheckAuthorityAttenderTypeID))
            {
                cH_NG_195_MSG6_SearchResults.GeneralDetails.CheckAuthorityAttenderTypeID = Convert.ToInt32(physicalCheck.CheckAuthorityAttenderTypeID);
                cH_NG_195_MSG6_SearchResults.GeneralDetails.CheckAuthorityAttenderTypeIDSpecified = true;
            }

            cH_NG_195_MSG6_SearchResults.GeneralDetails.CheckAuthorityAttenderTypeName = physicalCheck.CheckAuthorityAttenderTypeName;
            cH_NG_195_MSG6_SearchResults.GeneralDetails.declarationID = physicalCheck.DeclarationNo;
            cH_NG_195_MSG6_SearchResults.GeneralDetails.checkId = Convert.ToInt32(physicalCheck.CheckId);
            cH_NG_195_MSG6_SearchResults.GeneralDetails.checkIdSpecified = true;
            cH_NG_195_MSG6_SearchResults.GeneralDetails.storageSiteNumber = physicalCheck.StorageSiteCode;
            cH_NG_195_MSG6_SearchResults.GeneralDetails.rowNumber = Convert.ToInt32(physicalCheck.RowNumber);
            if(!string.IsNullOrEmpty(physicalCheck.CargoIdentifierKey1))
            {
                cH_NG_195_MSG6_SearchResults.GeneralDetails.CargoIdentifier = new cargoIdentifier();
                cH_NG_195_MSG6_SearchResults.GeneralDetails.CargoIdentifier.cargoIdentifierKey1 = physicalCheck.CargoIdentifierKey1;
                cH_NG_195_MSG6_SearchResults.GeneralDetails.CargoIdentifier.cargoIdentifierKey2 = physicalCheck.CargoIdentifierKey2;
                cH_NG_195_MSG6_SearchResults.GeneralDetails.CargoIdentifier.cargoIdentifierKey3 = physicalCheck.CargoIdentifierKey3;
                cH_NG_195_MSG6_SearchResults.GeneralDetails.CargoIdentifier.cargoIdentifierType = Convert.ToInt32(physicalCheck.CargoIdentifierTypeCode);

            }

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.PhysicalCheck");
            this.MyRequestSheetParam.EntityId1 = requestParams.AppicationId;
            this.MyRequestSheetParam.CustomFileNo = physicalCheck.CustomFileNo;
            this.MyRequestSheetParam.RequestDescription = "תשובה לבדיקה פיזית";
             return cH_NG_195_MSG6_SearchResults;
        }

     }
}
