
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;  
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class ClientAddressDataMapping: IMapping<ClientAddressPM, ClientAddress>,IMappingEncodeBase64NVARCHARFields<ClientAddressPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         ClientId, 
	         AddressId, 
	         Tenant, 
	         ContactStateCode, 
	         AddressTypeCode, 
	         AddressPurposeCode, 
	         IsPalestinianCity, 
	         IsHebrewAddress, 
	         BranchName, 
	         ContactIdentifier, 
	         ContactFirstName, 
	         ContactLastName, 
	         ContactRoleTypeCode, 
	         AuthorizedSignerPermit1, 
	         AuthorizedSignerPermit2, 
	         AuthorizedSignerPermit3, 
	         LocalCityCode, 
	         LocalSecondLine, 
	         LocalStreetName, 
	         LocalHouseLetter, 
	         LocalEntrance, 
	         EnglishCountryCode, 
	         EnglishSubCountryCode, 
	         EnglishCityName, 
	         EnglishMainAddressLine, 
	         EnglishPostalCode, 
	         LocalApartment, 
	         LocalPOBox, 
	         LocalPostalCode, 
	         LocalHouseNumber, 
	         CustomAddressCode,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         ClientId, 
	         AddressId, 
	         Tenant, 
	         ContactStateCode, 
	         AddressTypeCode, 
	         AddressPurposeCode, 
	         IsPalestinianCity, 
	         IsHebrewAddress, 
	         BranchName, 
	         ContactIdentifier, 
	         ContactFirstName, 
	         ContactLastName, 
	         ContactRoleTypeCode, 
	         AuthorizedSignerPermit1, 
	         AuthorizedSignerPermit2, 
	         AuthorizedSignerPermit3, 
	         LocalCityCode, 
	         LocalSecondLine, 
	         LocalStreetName, 
	         LocalHouseLetter, 
	         LocalEntrance, 
	         EnglishCountryCode, 
	         EnglishSubCountryCode, 
	         EnglishCityName, 
	         EnglishMainAddressLine, 
	         EnglishPostalCode, 
	         LocalApartment, 
	         LocalPOBox, 
	         LocalPostalCode, 
	         LocalHouseNumber, 
	         ContactStateName, 
	         AddressTypeName, 
	         AddressPurposeName, 
	         ContactRoleTypeName, 
	         AuthorizedSignerPermit1Name, 
	         AuthorizedSignerPermit2Name, 
	         AuthorizedSignerPermit3Name, 
	         LocalCityName, 
	         EnglishCountryName, 
	         EnglishSubCountryName, 
	         CustomAddressCode,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ClientAddressPM entityPM, ClientAddress entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContactStateCode))
            {
				entityPOCO.ContactStateCode = entityPM.ContactStateCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AddressTypeCode))
            {
				entityPOCO.AddressTypeCode = entityPM.AddressTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AddressPurposeCode))
            {
				entityPOCO.AddressPurposeCode = entityPM.AddressPurposeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsPalestinianCity))
            {
				entityPOCO.IsPalestinianCity = entityPM.IsPalestinianCity;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsHebrewAddress))
            {
				entityPOCO.IsHebrewAddress = entityPM.IsHebrewAddress;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BranchName))
            {
				entityPOCO.BranchName = entityPM.BranchName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContactIdentifier))
            {
				entityPOCO.ContactIdentifier = entityPM.ContactIdentifier;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContactFirstName))
            {
				entityPOCO.ContactFirstName = entityPM.ContactFirstName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContactLastName))
            {
				entityPOCO.ContactLastName = entityPM.ContactLastName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContactRoleTypeCode))
            {
				entityPOCO.ContactRoleTypeCode = entityPM.ContactRoleTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AuthorizedSignerPermit1))
            {
				entityPOCO.AuthorizedSignerPermit1 = entityPM.AuthorizedSignerPermit1;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AuthorizedSignerPermit2))
            {
				entityPOCO.AuthorizedSignerPermit2 = entityPM.AuthorizedSignerPermit2;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AuthorizedSignerPermit3))
            {
				entityPOCO.AuthorizedSignerPermit3 = entityPM.AuthorizedSignerPermit3;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalCityCode))
            {
				entityPOCO.LocalCityCode = entityPM.LocalCityCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalSecondLine))
            {
				entityPOCO.LocalSecondLine = entityPM.LocalSecondLine;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalStreetName))
            {
				entityPOCO.LocalStreetName = entityPM.LocalStreetName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalHouseLetter))
            {
				entityPOCO.LocalHouseLetter = entityPM.LocalHouseLetter;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalEntrance))
            {
				entityPOCO.LocalEntrance = entityPM.LocalEntrance;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishCountryCode))
            {
				entityPOCO.EnglishCountryCode = entityPM.EnglishCountryCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishSubCountryCode))
            {
				entityPOCO.EnglishSubCountryCode = entityPM.EnglishSubCountryCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishCityName))
            {
				entityPOCO.EnglishCityName = entityPM.EnglishCityName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishMainAddressLine))
            {
				entityPOCO.EnglishMainAddressLine = entityPM.EnglishMainAddressLine;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishPostalCode))
            {
				entityPOCO.EnglishPostalCode = entityPM.EnglishPostalCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalApartment))
            {
				entityPOCO.LocalApartment = entityPM.LocalApartment;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalPOBox))
            {
				entityPOCO.LocalPOBox = entityPM.LocalPOBox;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalPostalCode))
            {
				entityPOCO.LocalPostalCode = entityPM.LocalPostalCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalHouseNumber))
            {
				entityPOCO.LocalHouseNumber = entityPM.LocalHouseNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomAddressCode))
            {
				entityPOCO.CustomAddressCode = entityPM.CustomAddressCode;
			}
			}

		public void POCOToPM(ClientAddressPM entityPM, ClientAddress entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ClientId))
            {
					entityPM.ClientId = entityPOCO.ClientId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AddressId))
            {
					entityPM.AddressId = entityPOCO.AddressId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ContactStateCode))
            {
					entityPM.ContactStateCode = entityPOCO.ContactStateCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AddressTypeCode))
            {
					entityPM.AddressTypeCode = entityPOCO.AddressTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AddressPurposeCode))
            {
					entityPM.AddressPurposeCode = entityPOCO.AddressPurposeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsPalestinianCity))
            {
					entityPM.IsPalestinianCity = entityPOCO.IsPalestinianCity;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsHebrewAddress))
            {
					entityPM.IsHebrewAddress = entityPOCO.IsHebrewAddress;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BranchName))
            {
					entityPM.BranchName = entityPOCO.BranchName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ContactIdentifier))
            {
					entityPM.ContactIdentifier = entityPOCO.ContactIdentifier;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ContactFirstName))
            {
					entityPM.ContactFirstName = entityPOCO.ContactFirstName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ContactLastName))
            {
					entityPM.ContactLastName = entityPOCO.ContactLastName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ContactRoleTypeCode))
            {
					entityPM.ContactRoleTypeCode = entityPOCO.ContactRoleTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AuthorizedSignerPermit1))
            {
					entityPM.AuthorizedSignerPermit1 = entityPOCO.AuthorizedSignerPermit1;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AuthorizedSignerPermit2))
            {
					entityPM.AuthorizedSignerPermit2 = entityPOCO.AuthorizedSignerPermit2;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AuthorizedSignerPermit3))
            {
					entityPM.AuthorizedSignerPermit3 = entityPOCO.AuthorizedSignerPermit3;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LocalCityCode))
            {
					entityPM.LocalCityCode = entityPOCO.LocalCityCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LocalSecondLine))
            {
					entityPM.LocalSecondLine = entityPOCO.LocalSecondLine;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LocalStreetName))
            {
					entityPM.LocalStreetName = entityPOCO.LocalStreetName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LocalHouseLetter))
            {
					entityPM.LocalHouseLetter = entityPOCO.LocalHouseLetter;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LocalEntrance))
            {
					entityPM.LocalEntrance = entityPOCO.LocalEntrance;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EnglishCountryCode))
            {
					entityPM.EnglishCountryCode = entityPOCO.EnglishCountryCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EnglishSubCountryCode))
            {
					entityPM.EnglishSubCountryCode = entityPOCO.EnglishSubCountryCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EnglishCityName))
            {
					entityPM.EnglishCityName = entityPOCO.EnglishCityName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EnglishMainAddressLine))
            {
					entityPM.EnglishMainAddressLine = entityPOCO.EnglishMainAddressLine;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EnglishPostalCode))
            {
					entityPM.EnglishPostalCode = entityPOCO.EnglishPostalCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LocalApartment))
            {
					entityPM.LocalApartment = entityPOCO.LocalApartment;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LocalPOBox))
            {
					entityPM.LocalPOBox = entityPOCO.LocalPOBox;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LocalPostalCode))
            {
					entityPM.LocalPostalCode = entityPOCO.LocalPostalCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LocalHouseNumber))
            {
					entityPM.LocalHouseNumber = entityPOCO.LocalHouseNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomAddressCode))
            {
					entityPM.CustomAddressCode = entityPOCO.CustomAddressCode;
            }

		}

		public void PMToOldPM(ClientAddressPM entityPM, ClientAddressPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContactStateCode))
            {
                oldEntityPM.ContactStateCode = entityPM.ContactStateCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AddressTypeCode))
            {
                oldEntityPM.AddressTypeCode = entityPM.AddressTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AddressPurposeCode))
            {
                oldEntityPM.AddressPurposeCode = entityPM.AddressPurposeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsPalestinianCity))
            {
                oldEntityPM.IsPalestinianCity = entityPM.IsPalestinianCity;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsHebrewAddress))
            {
                oldEntityPM.IsHebrewAddress = entityPM.IsHebrewAddress;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BranchName))
            {
                oldEntityPM.BranchName = entityPM.BranchName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContactIdentifier))
            {
                oldEntityPM.ContactIdentifier = entityPM.ContactIdentifier;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContactFirstName))
            {
                oldEntityPM.ContactFirstName = entityPM.ContactFirstName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContactLastName))
            {
                oldEntityPM.ContactLastName = entityPM.ContactLastName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContactRoleTypeCode))
            {
                oldEntityPM.ContactRoleTypeCode = entityPM.ContactRoleTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AuthorizedSignerPermit1))
            {
                oldEntityPM.AuthorizedSignerPermit1 = entityPM.AuthorizedSignerPermit1;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AuthorizedSignerPermit2))
            {
                oldEntityPM.AuthorizedSignerPermit2 = entityPM.AuthorizedSignerPermit2;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AuthorizedSignerPermit3))
            {
                oldEntityPM.AuthorizedSignerPermit3 = entityPM.AuthorizedSignerPermit3;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalCityCode))
            {
                oldEntityPM.LocalCityCode = entityPM.LocalCityCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalSecondLine))
            {
                oldEntityPM.LocalSecondLine = entityPM.LocalSecondLine;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalStreetName))
            {
                oldEntityPM.LocalStreetName = entityPM.LocalStreetName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalHouseLetter))
            {
                oldEntityPM.LocalHouseLetter = entityPM.LocalHouseLetter;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalEntrance))
            {
                oldEntityPM.LocalEntrance = entityPM.LocalEntrance;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishCountryCode))
            {
                oldEntityPM.EnglishCountryCode = entityPM.EnglishCountryCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishSubCountryCode))
            {
                oldEntityPM.EnglishSubCountryCode = entityPM.EnglishSubCountryCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishCityName))
            {
                oldEntityPM.EnglishCityName = entityPM.EnglishCityName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishMainAddressLine))
            {
                oldEntityPM.EnglishMainAddressLine = entityPM.EnglishMainAddressLine;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishPostalCode))
            {
                oldEntityPM.EnglishPostalCode = entityPM.EnglishPostalCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalApartment))
            {
                oldEntityPM.LocalApartment = entityPM.LocalApartment;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalPOBox))
            {
                oldEntityPM.LocalPOBox = entityPM.LocalPOBox;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalPostalCode))
            {
                oldEntityPM.LocalPostalCode = entityPM.LocalPostalCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalHouseNumber))
            {
                oldEntityPM.LocalHouseNumber = entityPM.LocalHouseNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomAddressCode))
            {
                oldEntityPM.CustomAddressCode = entityPM.CustomAddressCode;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(ClientAddressPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.BranchName)) //T4 find type == nText 
            {
                entityPM.BranchName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.BranchName));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ContactFirstName)) //T4 find type == nText 
            {
                entityPM.ContactFirstName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ContactFirstName));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ContactLastName)) //T4 find type == nText 
            {
                entityPM.ContactLastName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ContactLastName));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.LocalSecondLine)) //T4 find type == nText 
            {
                entityPM.LocalSecondLine = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.LocalSecondLine));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.LocalStreetName)) //T4 find type == nText 
            {
                entityPM.LocalStreetName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.LocalStreetName));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.LocalHouseLetter)) //T4 find type == nText 
            {
                entityPM.LocalHouseLetter = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.LocalHouseLetter));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.LocalEntrance)) //T4 find type == nText 
            {
                entityPM.LocalEntrance = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.LocalEntrance));
            }
            entityPM.EncodeBase64NVARCHARFieldsBy=null;
		}


	    public void AddPOCOPropertyName(POCOPropertyNames pocoPropertyName)
        {
            CustomMappedPOCOProperties.Add(pocoPropertyName);
        }

        public void AddPMPropertyName(PMPropertyNames pocoPropertyName)
        {
            CustomMappedPMProperties.Add(pocoPropertyName);
        }
			  
   }
}
	 