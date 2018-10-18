using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class CustomItemLegalDemandsResponseData : ResponseDataBase
    {
        public List<CustomsLegalDemandsResult> CustomsLegalDemandsList { get; set; }
        public List<CountriesExclusionResult> CountriesExclusionList { get; set; }
    }

    public class CustomsLegalDemandsResult
    {
        public string AuthoritiyID { get; set; }
        public string AuthoritiyName { get; set; }
        public string CertificateTypeId { get; set; }
        public string CertificateTypeName { get; set; }
        public string FullClassification { get; set; }
        public string InterConditionsRelationshipName { get; set; }
        public bool? IsAuthorityConformationDetailsExist { get; set; }
        public string IsCarnetIncluded { get; set; }
        public string IsPersonalImportIncluded { get; set; }
        public string RegularityPublicationName { get; set; }
        public string RegularityRequirementId { get; set; }
        public string RequirementGoodsDescription { get; set; }
        public string RequirementSourceName { get; set; }
        public string TextualCondition { get; set; }
        public string TrNumber { get; set; }
        public List<ConfirmationWebAddressResult> ConfirmationWebAddressList { get; set; }
    }

    public class ConfirmationWebAddressResult
    {
        public string ConfirmationTypeWebAddress { get; set; }
        public string FormTypeForConfirmation { get; set; }
    }

    public class CountriesExclusionResult
    {
        public string CountryId { get; set; }
        public string CountryName { get; set; }
        public string RegularityRequirementId { get; set; }
    }
}
