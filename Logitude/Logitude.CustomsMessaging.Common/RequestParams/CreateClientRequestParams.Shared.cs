using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class CreateClientRequestParams : RequestParamsBase
    {
        public bool IsExternalId { get; set; }

        public string ClientTypeSpecificCode { get; set; }

        public bool IsActive { get; set; }

        public string LocalFirstName { get; set; }




        public string LocalLastName { get; set; }
        public string LocalCorporationName { get; set; }


        public string EnglishFirstName { get; set; }



        public string EnglishLastName { get; set; }


        public string EnglishCorporationName { get; set; }


        public DateTime? BirthDate { get; set; }


        public string GenderCode { get; set; }

        public string DunsNumber { get; set; }


        public string PassportNumber { get; set; }


        public string FullName { get; set; }

        public string PassportCountryCode { get; set; }


        public string PassportTypeCode { get; set; }

        public string PassportFirstName { get; set; }


        public string PassportLastName { get; set; }


        public string EnglishBirthPlace { get; set; }

        public string EnglishFatherName { get; set; }


        public DateTime? PassportExpirationDate { get; set; }


        public DateTime? PassportIssueDate { get; set; }



        List<ClientAdressParams> clientAddresses;
        public List<ClientAdressParams> ClientAddresses
        {
            get
            {
                if (clientAddresses == null)
                {
                    clientAddresses = new List<ClientAdressParams>();
                }
                return clientAddresses;
            }
            set { clientAddresses = value; }
        }



        public string ClientTypeSpecificName { get; set; }

        public string PassportCountryName { get; set; }

        public string GenderName { get; set; }


        public string PassportTypeName { get; set; }
        public bool IsImporter { get; set; }


        public bool IsExporter { get; set; }

        public string ConcurrencyGUID { get; set; }


        public string NewConcurrencyGUID { get; set; }

        public string FacilitationTypeCode { get; set; }


        List<ClientDrivingLicenseParams> clientDrivingLicenses;
        public List<ClientDrivingLicenseParams> ClientDrivingLicenses
        {
            get
            {
                if (clientDrivingLicenses == null)
                {
                    clientDrivingLicenses = new List<ClientDrivingLicenseParams>();
                }
                return clientDrivingLicenses;
            }
            set { clientDrivingLicenses = value; }
        }


    }

    public class ClientAdressParams
    {

        public string ContactStateCode { get; set; }

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

        private string localSecondLine { get; set; }

        public string LocalSecondLine { get; set; }


        public string LocalStreetName { get; set; }

        public string LocalHouseLetter { get; set; }


        public string LocalEntrance { get; set; }

        public string EnglishCountryCode { get; set; }

        public string EnglishSubCountryCode { get; set; }


        public string EnglishCityName { get; set; }


        public string EnglishMainAddressLine { get; set; }


        public string EnglishPostalCode { get; set; }


        public decimal? LocalApartment { get; set; }

        public string LocalPOBox { get; set; }

        public string LocalPostalCode { get; set; }


        public string LocalHouseNumber { get; set; }


        private List<ClientAddressCommunicationType> clientsAddressCommTypes;

        public List<ClientAddressCommunicationType> ClientAddressCommunicationType
        {
            get
            {
                if (clientsAddressCommTypes == null)
                {
                    clientsAddressCommTypes = new List<ClientAddressCommunicationType>();
                }
                return clientsAddressCommTypes;
            }
            set { clientsAddressCommTypes = value; }
        }

        public string ContactStateName { get; set; }

        public string AddressTypeName { get; set; }

        public string AddressPurposeName { get; set; }


        public string ContactRoleTypeName { get; set; }

        private string authorizedSignerPermit1Name { get; set; }


        public string AuthorizedSignerPermit1Name { get; set; }


        public string AuthorizedSignerPermit2Name { get; set; }

        public string AuthorizedSignerPermit3Name { get; set; }


        public string LocalCityName { get; set; }

        public string EnglishCountryName { get; set; }

        public string EnglishSubCountryName { get; set; }

        public string CustomAddressCode { get; set; }


        public string AddressId { get; set; }
    }

   public class ClientAddressCommunicationType
   {

        public string CommunicationTypeCode { get; set; }

        public string CommunicationAddress { get; set; }

       public string CommunicationTypeName { get; set; }
       
   }

    public class ClientDrivingLicenseParams
    {
        public string DrivingLicenseNumber { get; set; }
        public DateTime DriverLicenseValidityDate { get; set; }
        public string DrivingLicenseCountryID { get; set; }

        private List<ClientDrivingLicenseTypeParams> clientDrivingLicenseTypes;
        public List<ClientDrivingLicenseTypeParams> ClientDrivingLicenseTypes
        {
            get
            {
                if (clientDrivingLicenseTypes == null)
                {
                    clientDrivingLicenseTypes = new List<ClientDrivingLicenseTypeParams>();
                }
                return clientDrivingLicenseTypes;
            }
            set { clientDrivingLicenseTypes = value; }
        }
    }

    public class ClientDrivingLicenseTypeParams
    {
        public string DriversLicenseTypeCode { get; set; }

    }
}
