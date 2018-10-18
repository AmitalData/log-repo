using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class ClientSearchByIDResponseData : ResponseDataBase
    {
        public string ExternalID { get; set; }
        public string DunsNumber { get; set; }
        public string MerkavahNumber { get; set; }
        public bool IsActive { get; set; }
        public string LocalFirstName { get; set; }
        public string LocalLastName { get; set; }
        public string LocalCorporationName { get; set; }
        public string EnglishFirstName { get; set; }
        public string EnglishLastName { get; set; }
        public string EnglishCorporationName { get; set; }

        public List<AddressContactPhone> AddressContactPhoneList { get; set; }
        public List<Authorized> AuthorizedList { get; set; }
        public List<Authorized> AuthorizerList { get; set; }
        public List<CustomerActivity> CustomerActivityList { get; set; }
        public List<ExportRequest> ExportRequestList { get; set; }
        public List<IndicationPerClassification> IndicationPerClassificationList { get; set; }
    }

    public class AddressContactPhone
    {
        public string Address { get; set; }
        public string AddressPurposeID { get; set; }
        public string AddressPurposeName { get; set; }
        public string AddressTypeID { get; set; }
        public string AddressTypeName { get; set; }

        public List<ContactPhone> ContactPhoneList { get; set; }
    }

    public class ContactPhone
    {
        public string CommunicationAddress { get; set; }
        public string CommunicationTypeID { get; set; }
        public string CommunicationTypeName { get; set; }
    }

    public class Authorized
    {
        public string AuthorizedName { get; set; }
        public string CustomerActivityTypeID { get; set; }
        public string CustomerActivityTypeName { get; set; }
        public string EndDate { get; set; }
        public string PoaAuthorizationTypeID { get; set; }
        public string PoaAuthorizationTypeName { get; set; }
        public string PoaID { get; set; }
        public string PoaStatus { get; set; }
        public string PoaStatusName { get; set; }
        public string StartDate { get; set; }
    }

    public class CustomerActivity
    {
        public string CustomerActivityTypeID { get; set; }
        public string CustomerActivityTypeName { get; set; }
        public bool IsActive { get; set; }
        public string LogisticIdentification { get; set; }
        public string StartDate { get; set; }

        public List<CustomerIndication> CustomerIndicationList { get; set; }
    }

    public class CustomerIndication
    {
        public string CustomerIndicationTypeID { get; set; }
        public string CustomerIndicationTypeName { get; set; }
        public string EndDate { get; set; }
        public bool IsActive { get; set; }
        public string StartDate { get; set; }
    }

    public class ExportRequest
    {
        public string ApprovementEndDate { get; set; }
        public string ApprovementStartDate { get; set; }
        public string CreateDate { get; set; }
        public string CustomerRequestStatusID { get; set; }
        public string CustomerRequestStatusName { get; set; }
        public string OrganizationialUnitID { get; set; }
        public string RequestID { get; set; }
        public string RequestTypeID { get; set; }
        public string RequestTypeName { get; set; }
        public string StationName { get; set; }
    }

    public class IndicationPerClassification
    {
        public string ClassificationID { get; set; }
        public string EndDate { get; set; }
        public string GoodsItemDescription { get; set; }
        public string IndicationPerClassificationTypeID { get; set; }
        public string IndicationPerClassificationTypeName { get; set; }
        public string StartDate { get; set; }
    }
}
