//Yuval Chalup 25.06.2015 TASK-8907
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class SpecialActivityRequestParams : RequestParamsBase 
    {
        public GeneralDetails GeneralDetailsData { get; set; }
        public GoodsDetails GoodsDetailsData { get; set; }
        public RePackingApprovalDetails RePackingApprovalDetailsData { get; set; }
        public List<SampleRequestDetails> SampleRequestDetailsDataList { get; set; }
        public List<CurrentPackingDetails> CurrentPackingDetailsDataList { get; set; }
        public List<DesiredPackingDetails> DesiredPackingDetailsDataList { get; set; }

        //////////////////////////////          General         //////////////////////////////
        public class GeneralDetails
        {
            public DateTime? ActivityRequestStartDate { get; set; }
            public DateTime? ActivityRequestStartTime { get; set; }
            public DateTime? ActivityRequestEndDate { get; set; }
            public DateTime? ActivityRequestEndTime { get; set; }
            public int ApplicantAgentNumber { get; set; }
            public string AuthorityCode { get; set; }
            public bool AuthorityCodeSpecified { get; set; }
            public string CargoRowNumber { get; set; }
            public bool CargoRowNumberSpecified { get; set; }
            public string CheckSite { get; set; }
            public string ClientFullName { get; set; }
            public string ContainerNumber { get; set; }
            public string ImporterName { get; set; }
            public string ImporterNumber { get; set; }
            public bool ImporterNumberSpecified { get; set; }
            public bool? IsContainer { get; set; }
            public bool IsContainerSpecified { get; set; }
            public string SealNumber { get; set; }
            public string SiteNumber { get; set; }
            public string SpecialActivityRequestNumber { get; set; }
            public int SpecialActivityType { get; set; }
            public int? WarehouseBlockNumber { get; set; }
            public CargoIdentifier CargoIdentifier { get; set; }
            public string SpecialActivityTypeEssence { get; set; }

            public string CustomFileNo { get; set; }
            public string DeclarationId { get; set; }

        }

        public class CargoIdentifier
        {
            public string CargoIdentifierKey1 { get; set; }
            public string CargoIdentifierKey2 { get; set; }
            public string CargoIdentifierKey3 { get; set; }
            public int CargoIdentifierType { get; set; }
        }

        ///////////////////////                 GoodsDetail         //////////////////////////////
        public class GoodsDetails
        {
            public string GoodsDescription { get; set; }
            public int? IdemanderType { get; set; }
            public bool IdemanderTypeSpecified { get; set; }
            public string OtherDescription { get; set; }
            public int? SpecialActionsCode { get; set; }
            public bool SpecialActionsCodeSpecified { get; set; }
            public List<RepresentativeDetails> RepresentativeList { get; set; }
        }

        public class RepresentativeDetails
        {
            public string RepresentativeID { get; set; }
            public bool RepresentativeIDSpecified { get; set; }
            public string RepresentativeName { get; set; }
            public int? RepresentativeNumber { get; set; }
            public bool RepresentativeNumberSpecified { get; set; }
        }

        ///////////////////////             SampleRequest              ///////////////////////
        public class SampleRequestDetails
        {
            public string CurrencyTypeCode { get; set; }
            public string CurrencyTypeName { get; set; }
            public string CustomsItem { get; set; }
            public int? CustomsItemQuantity { get; set; }
            public bool CustomsItemQuantitySpecified { get; set; }
            public string SampleDescription { get; set; }
            public DateTime? SampleReturnDate { get; set; }
            public int? SampleRowNumber { get; set; }
            public bool SampleRowNumberSpecified { get; set; }
            public int? SampleValue { get; set; }
            public PackingDetails SamplePackingDetails { get; set; }
        }

        ///////////////////////             StorageRePackingApproval              ///////////////////////
        public class RePackingApprovalDetails
        {
            public DateTime? ApprovalDate { get; set; }
            public bool ApprovalDateSpecified { get; set; }
            public string ApprovalName { get; set; }
            public string SiteNumber { get; set; }
        }

        public class CurrentPackingDetails
        {
            public string PresentPackingStateContent { get; set; }
            public int? RePackingOldLineNumber { get; set; }
            public bool RePackingOldLineNumberSpecified { get; set; }
            public PackingDetails PackingDetails { get; set; }
        }

        public class DesiredPackingDetails
        {
            public int? RePackingNewLineNumber { get; set; }
            public bool RePackingNewLineNumberSpecified { get; set; }
            public int? RePackingOldLineNumber { get; set; }
            public bool RePackingOldLineNumberSpecified { get; set; }
            public PackingDetails PackingDetails { get; set; }
        }

        public class PackingDetails
        {
            public string PackageId { get; set; }
            public string PackageType { get; set; }
            public string PackageTypeName { get; set; }
            public string Quantity { get; set; }
            //public int SampleValue { get; set; }
            public int? Weight { get; set; }
            public bool WeightSpecified { get; set; }
        }
    }
}
