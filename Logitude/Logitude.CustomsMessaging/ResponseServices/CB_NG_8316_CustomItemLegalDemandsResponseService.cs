using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.CustomItemLegalDemandsServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class CB_NG_8316_CustomItemLegalDemandsResponseService : ResponseServiceBase<CustomItemLegalDemandsResponseData, CB_NG_8316_CustomItemLegalDemandsOut, CustomItemLegalDemandsRequestParams>
    {
        public override void Update(CB_NG_8316_CustomItemLegalDemandsOut customResponse, CustomItemLegalDemandsRequestParams requestParams)
        {
            this.MyResponseData = new CustomItemLegalDemandsResponseData();
            this.MyResponseData.Succeeded = true;

            if (customResponse.ResponseContentHeader.Exception != null)
            {
                LogMessagingUtil.Instance.AppendLine(customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription);
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription;
                return;
            }

            if (customResponse.CILegalDemandsOut == null || (customResponse.CILegalDemandsOut != null && customResponse.CILegalDemandsOut.Count() == 0))
            {
                LogMessagingUtil.Instance.AppendLine("CILegalDemandsOut is empty");
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = "לא התקבלו נתונים מהמכס";
                return;
            }

            List<CustomsLegalDemandsResult> customsLegalDemandsList = new List<CustomsLegalDemandsResult>();
            List<CountriesExclusionResult> countriesExclusionList = new List<CountriesExclusionResult>();

            foreach (var customResponseItem in customResponse.CILegalDemandsOut)
            {
                //Get Customs Legal Demands Details
                if (customResponseItem.LegalDemandsOut != null)
                {
                    foreach (var legalDemandsItem in customResponseItem.LegalDemandsOut)
                    {
                        CustomsLegalDemandsResult countriesExclusionResult = new CustomsLegalDemandsResult();
                        countriesExclusionResult.AuthoritiyID = legalDemandsItem.authoritiyID.ToString();
                        countriesExclusionResult.AuthoritiyName = legalDemandsItem.authoritiyName;
                        countriesExclusionResult.CertificateTypeId = legalDemandsItem.certificateTypeId.ToString();
                        countriesExclusionResult.CertificateTypeName = legalDemandsItem.certificateTypeName;
                        countriesExclusionResult.FullClassification = legalDemandsItem.fullClassification;
                        countriesExclusionResult.InterConditionsRelationshipName = legalDemandsItem.interConditionsRelationshipName;
                        countriesExclusionResult.IsAuthorityConformationDetailsExist = legalDemandsItem.isAuthorityConformationDetailsExist;
                        if (legalDemandsItem.isCarnetIncluded != null && legalDemandsItem.isCarnetIncluded == true)
                        {
                            countriesExclusionResult.IsCarnetIncluded = "Visible";
                        }
                        else
                        {
                            countriesExclusionResult.IsCarnetIncluded = "Collapsed";
                        }

                        if (legalDemandsItem.isPersonalImportIncluded != null && legalDemandsItem.isPersonalImportIncluded == true)
                        {
                            countriesExclusionResult.IsPersonalImportIncluded = "Visible";
                        }
                        else
                        {
                            countriesExclusionResult.IsPersonalImportIncluded = "Collapsed";
                        }

                        countriesExclusionResult.RegularityPublicationName = legalDemandsItem.regularityPublicationName;
                        countriesExclusionResult.RegularityRequirementId = legalDemandsItem.regularityRequirementId.ToString();
                        countriesExclusionResult.RequirementGoodsDescription = legalDemandsItem.requirementGoodsDescription;
                        countriesExclusionResult.RequirementSourceName = legalDemandsItem.requirementSourceName;
                        countriesExclusionResult.TextualCondition = legalDemandsItem.textualCondition;
                        countriesExclusionResult.TrNumber = legalDemandsItem.trNumber.ToString();

                        customsLegalDemandsList.Add(countriesExclusionResult);
                    }
                }

                //Get Countries Exclusion Details
                if (customResponseItem.CountriesExclusion != null)
                {
                    foreach (var countriesExclusionItem in customResponseItem.CountriesExclusion)
                    {
                        CountriesExclusionResult countriesExclusionResult = new CountriesExclusionResult()
                        {
                            CountryId = countriesExclusionItem.countryId.ToString(),
                            CountryName = countriesExclusionItem.countryName,
                            RegularityRequirementId = countriesExclusionItem.regularityRequirementId.ToString(),
                        };
                        countriesExclusionList.Add(countriesExclusionResult);
                    }
                }
            }

            this.MyResponseData.UserMessage = "שאילתא לדרישת חוקיות לפרט מכס בוצעה בהצלחה";
            this.MyResponseData.CustomsLegalDemandsList = customsLegalDemandsList;
            this.MyResponseData.CountriesExclusionList = countriesExclusionList;
        }

        public override CustomItemLegalDemandsResponseData GetResponse(CB_NG_8316_CustomItemLegalDemandsOut customResponse, CustomItemLegalDemandsRequestParams requestParams)
        {
            return this.MyResponseData;
        }
    }
}
