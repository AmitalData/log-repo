
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
   
   public partial class ClientDataMapping: IMapping<ClientPM, Client>,IMappingEncodeBase64NVARCHARFields<ClientPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         Code, 
	         SearchFields, 
	         FullName, 
	         ClientTypeSpecificCode, 
	         IsActive, 
	         LocalFirstName, 
	         LocalLastName, 
	         LocalCorporationName, 
	         EnglishFirstName, 
	         EnglishLastName, 
	         EnglishCorporationName, 
	         BirthDate, 
	         GenderCode, 
	         DunsNumber, 
	         PassportNumber, 
	         PassportCountryCode, 
	         PassportTypeCode, 
	         PassportFirstName, 
	         PassportLastName, 
	         EnglishBirthPlace, 
	         EnglishFatherName, 
	         PassportExpirationDate, 
	         PassportIssueDate, 
	         IsImporter, 
	         IsExporter, 
	         ConcurrencyGUID, 
	         FacilitationTypeCode, 
	         NationalIdentificationNumber,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         Code, 
	         SearchFields, 
	         FullName, 
	         ClientTypeSpecificCode, 
	         IsActive, 
	         LocalFirstName, 
	         LocalLastName, 
	         LocalCorporationName, 
	         EnglishFirstName, 
	         EnglishLastName, 
	         EnglishCorporationName, 
	         BirthDate, 
	         GenderCode, 
	         DunsNumber, 
	         PassportNumber, 
	         PassportCountryCode, 
	         PassportTypeCode, 
	         PassportFirstName, 
	         PassportLastName, 
	         EnglishBirthPlace, 
	         EnglishFatherName, 
	         PassportExpirationDate, 
	         PassportIssueDate, 
	         ClientTypeSpecificName, 
	         PassportCountryName, 
	         GenderName, 
	         PassportTypeName, 
	         IsImporter, 
	         IsExporter, 
	         ConcurrencyGUID, 
	         NewConcurrencyGUID, 
	         FacilitationTypeCode, 
	         NationalIdentificationNumber,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ClientPM entityPM, Client entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Code))
            {
				entityPOCO.Code = entityPM.Code;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FullName))
            {
				entityPOCO.FullName = entityPM.FullName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClientTypeSpecificCode))
            {
				entityPOCO.ClientTypeSpecificCode = entityPM.ClientTypeSpecificCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsActive))
            {
				entityPOCO.IsActive = entityPM.IsActive;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalFirstName))
            {
				entityPOCO.LocalFirstName = entityPM.LocalFirstName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalLastName))
            {
				entityPOCO.LocalLastName = entityPM.LocalLastName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalCorporationName))
            {
				entityPOCO.LocalCorporationName = entityPM.LocalCorporationName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishFirstName))
            {
				entityPOCO.EnglishFirstName = entityPM.EnglishFirstName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishLastName))
            {
				entityPOCO.EnglishLastName = entityPM.EnglishLastName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishCorporationName))
            {
				entityPOCO.EnglishCorporationName = entityPM.EnglishCorporationName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BirthDate))
            {
				entityPOCO.BirthDate = entityPM.BirthDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GenderCode))
            {
				entityPOCO.GenderCode = entityPM.GenderCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DunsNumber))
            {
				entityPOCO.DunsNumber = entityPM.DunsNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PassportNumber))
            {
				entityPOCO.PassportNumber = entityPM.PassportNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PassportCountryCode))
            {
				entityPOCO.PassportCountryCode = entityPM.PassportCountryCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PassportTypeCode))
            {
				entityPOCO.PassportTypeCode = entityPM.PassportTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PassportFirstName))
            {
				entityPOCO.PassportFirstName = entityPM.PassportFirstName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PassportLastName))
            {
				entityPOCO.PassportLastName = entityPM.PassportLastName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishBirthPlace))
            {
				entityPOCO.EnglishBirthPlace = entityPM.EnglishBirthPlace;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishFatherName))
            {
				entityPOCO.EnglishFatherName = entityPM.EnglishFatherName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PassportExpirationDate))
            {
				entityPOCO.PassportExpirationDate = entityPM.PassportExpirationDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PassportIssueDate))
            {
				entityPOCO.PassportIssueDate = entityPM.PassportIssueDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsImporter))
            {
				entityPOCO.IsImporter = entityPM.IsImporter;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsExporter))
            {
				entityPOCO.IsExporter = entityPM.IsExporter;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConcurrencyGUID))
            {
				entityPOCO.ConcurrencyGUID = entityPM.ConcurrencyGUID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FacilitationTypeCode))
            {
				entityPOCO.FacilitationTypeCode = entityPM.FacilitationTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NationalIdentificationNumber))
            {
				entityPOCO.NationalIdentificationNumber = entityPM.NationalIdentificationNumber;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(ClientPM entityPM, Client entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Code))
            {
					entityPM.Code = entityPOCO.Code;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FullName))
            {
					entityPM.FullName = entityPOCO.FullName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ClientTypeSpecificCode))
            {
					entityPM.ClientTypeSpecificCode = entityPOCO.ClientTypeSpecificCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsActive))
            {
					entityPM.IsActive = entityPOCO.IsActive;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LocalFirstName))
            {
					entityPM.LocalFirstName = entityPOCO.LocalFirstName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LocalLastName))
            {
					entityPM.LocalLastName = entityPOCO.LocalLastName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LocalCorporationName))
            {
					entityPM.LocalCorporationName = entityPOCO.LocalCorporationName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EnglishFirstName))
            {
					entityPM.EnglishFirstName = entityPOCO.EnglishFirstName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EnglishLastName))
            {
					entityPM.EnglishLastName = entityPOCO.EnglishLastName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EnglishCorporationName))
            {
					entityPM.EnglishCorporationName = entityPOCO.EnglishCorporationName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BirthDate))
            {
					entityPM.BirthDate = entityPOCO.BirthDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GenderCode))
            {
					entityPM.GenderCode = entityPOCO.GenderCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DunsNumber))
            {
					entityPM.DunsNumber = entityPOCO.DunsNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PassportNumber))
            {
					entityPM.PassportNumber = entityPOCO.PassportNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PassportCountryCode))
            {
					entityPM.PassportCountryCode = entityPOCO.PassportCountryCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PassportTypeCode))
            {
					entityPM.PassportTypeCode = entityPOCO.PassportTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PassportFirstName))
            {
					entityPM.PassportFirstName = entityPOCO.PassportFirstName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PassportLastName))
            {
					entityPM.PassportLastName = entityPOCO.PassportLastName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EnglishBirthPlace))
            {
					entityPM.EnglishBirthPlace = entityPOCO.EnglishBirthPlace;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EnglishFatherName))
            {
					entityPM.EnglishFatherName = entityPOCO.EnglishFatherName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PassportExpirationDate))
            {
					entityPM.PassportExpirationDate = entityPOCO.PassportExpirationDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PassportIssueDate))
            {
					entityPM.PassportIssueDate = entityPOCO.PassportIssueDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsImporter))
            {
					entityPM.IsImporter = entityPOCO.IsImporter;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsExporter))
            {
					entityPM.IsExporter = entityPOCO.IsExporter;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConcurrencyGUID))
            {
					entityPM.ConcurrencyGUID = entityPOCO.ConcurrencyGUID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FacilitationTypeCode))
            {
					entityPM.FacilitationTypeCode = entityPOCO.FacilitationTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NationalIdentificationNumber))
            {
					entityPM.NationalIdentificationNumber = entityPOCO.NationalIdentificationNumber;
            }

		}

		public void PMToOldPM(ClientPM entityPM, ClientPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Code))
            {
                oldEntityPM.Code = entityPM.Code;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FullName))
            {
                oldEntityPM.FullName = entityPM.FullName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClientTypeSpecificCode))
            {
                oldEntityPM.ClientTypeSpecificCode = entityPM.ClientTypeSpecificCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsActive))
            {
                oldEntityPM.IsActive = entityPM.IsActive;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalFirstName))
            {
                oldEntityPM.LocalFirstName = entityPM.LocalFirstName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalLastName))
            {
                oldEntityPM.LocalLastName = entityPM.LocalLastName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalCorporationName))
            {
                oldEntityPM.LocalCorporationName = entityPM.LocalCorporationName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishFirstName))
            {
                oldEntityPM.EnglishFirstName = entityPM.EnglishFirstName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishLastName))
            {
                oldEntityPM.EnglishLastName = entityPM.EnglishLastName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishCorporationName))
            {
                oldEntityPM.EnglishCorporationName = entityPM.EnglishCorporationName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BirthDate))
            {
                oldEntityPM.BirthDate = entityPM.BirthDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GenderCode))
            {
                oldEntityPM.GenderCode = entityPM.GenderCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DunsNumber))
            {
                oldEntityPM.DunsNumber = entityPM.DunsNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PassportNumber))
            {
                oldEntityPM.PassportNumber = entityPM.PassportNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PassportCountryCode))
            {
                oldEntityPM.PassportCountryCode = entityPM.PassportCountryCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PassportTypeCode))
            {
                oldEntityPM.PassportTypeCode = entityPM.PassportTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PassportFirstName))
            {
                oldEntityPM.PassportFirstName = entityPM.PassportFirstName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PassportLastName))
            {
                oldEntityPM.PassportLastName = entityPM.PassportLastName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishBirthPlace))
            {
                oldEntityPM.EnglishBirthPlace = entityPM.EnglishBirthPlace;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishFatherName))
            {
                oldEntityPM.EnglishFatherName = entityPM.EnglishFatherName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PassportExpirationDate))
            {
                oldEntityPM.PassportExpirationDate = entityPM.PassportExpirationDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PassportIssueDate))
            {
                oldEntityPM.PassportIssueDate = entityPM.PassportIssueDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsImporter))
            {
                oldEntityPM.IsImporter = entityPM.IsImporter;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsExporter))
            {
                oldEntityPM.IsExporter = entityPM.IsExporter;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConcurrencyGUID))
            {
                oldEntityPM.ConcurrencyGUID = entityPM.ConcurrencyGUID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FacilitationTypeCode))
            {
                oldEntityPM.FacilitationTypeCode = entityPM.FacilitationTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NationalIdentificationNumber))
            {
                oldEntityPM.NationalIdentificationNumber = entityPM.NationalIdentificationNumber;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(ClientPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.FullName)) //T4 find type == nText 
            {
                entityPM.FullName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.FullName));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.LocalFirstName)) //T4 find type == nText 
            {
                entityPM.LocalFirstName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.LocalFirstName));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.LocalLastName)) //T4 find type == nText 
            {
                entityPM.LocalLastName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.LocalLastName));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.LocalCorporationName)) //T4 find type == nText 
            {
                entityPM.LocalCorporationName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.LocalCorporationName));
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
		
		private void BuildSearchFieldsGenerated(ClientPM entityPM, Client entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 