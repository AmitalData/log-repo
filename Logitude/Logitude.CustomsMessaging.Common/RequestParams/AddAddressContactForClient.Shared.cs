using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class AddAddressContactForClient: RequestParamsBase
    {
        public enum OperationTypes
        {
            Add = 1,
            Update = 2,
            Delete = 3,
        }

        public string ClientId { get; set; }
        public string ExternalId { get; set; }
        public string PassportNumber { get; set; }
        public string PassportTypeCode { get; set; }
        public string PassportCountryCode { get; set; }
        public OperationTypes OperationType { get; set; }
        public ClientAddress AddressCode { get; set; }

        public class ClientAddress
        {
            //Address Details
            public string AddressId { get; set; }
            public string AddressContactState { get; set; }
            public string AddressTypeCode { get; set; }
            public string AddressPurposeCode { get; set; }
            public bool IsPalestinianCity { get; set; }
            public bool IsHebrewAddress { get; set; }
            public string BranchName { get; set; }
            public string ContactIdentifier { get; set; }
            public string ContactFirstName { get; set; }
            public string ContactLastName { get; set; }
            public string ContactRoleTypeCode { get; set; }
            public string AuthorizedSignerPermit1 { get; set; }
            public string AuthorizedSignerPermit2 { get; set; }
            public string AuthorizedSignerPermit3 { get; set; }
            public string LocalCityCode { get; set; }
            public string LocalSecondLine { get; set; }
            public string LocalStreetName { get; set; }
            public string LocalHouseLetter { get; set; }
            public string LocalEntrance { get; set; }
            public string EnglishCountryCode { get; set; }
            public string EnglishSubCountryCode { get; set; }
            public string EnglishCityName { get; set; }
            public string EnglishMainAddressLine { get; set; }
            public string EnglishPostalCode { get; set; }
            public string LocalApartment { get; set; }
            public string LocalPOBox { get; set; }
            public string LocalPostalCode { get; set; }
            public string LocalHouseNumber { get; set; }
            public string CustomAddressCode { get; set; }

            //Communication Details
            public List<ClientsAddressCommunicationResult> ClientsAddressCommunication { get; set; }
            public class ClientsAddressCommunicationResult
            {
                public string CommunicationAddress { get; set; }
                public string CommunicationType { get; set; }
                public string CommunicationTypeName { get; set; }
            }
        }
    }
}
