
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
   
   public partial class ClientsPoaDataMapping: IMapping<ClientsPoaPM, ClientsPoa>,IMappingEncodeBase64NVARCHARFields<ClientsPoaPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         SearchFields, 
	         ClientId, 
	         poaID, 
	         AuthorizedExternalId, 
	         AuthorizerExternalId, 
	         AuthorizerPassportNumber, 
	         AuthorizerPassportCountry, 
	         AuthorizerPassportType, 
	         StartDate, 
	         EndDate, 
	         PoaStatus, 
	         PoaAuthorizationType,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         SearchFields, 
	         ClientId, 
	         poaID, 
	         AuthorizedExternalId, 
	         AuthorizerExternalId, 
	         AuthorizerPassportNumber, 
	         AuthorizerPassportCountry, 
	         AuthorizerPassportType, 
	         StartDate, 
	         EndDate, 
	         PoaStatus, 
	         PoaAuthorizationType,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ClientsPoaPM entityPM, ClientsPoa entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClientId))
            {
				entityPOCO.ClientId = entityPM.ClientId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.poaID))
            {
				entityPOCO.poaID = entityPM.PoaID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AuthorizedExternalId))
            {
				entityPOCO.AuthorizedExternalId = entityPM.AuthorizedExternalId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AuthorizerExternalId))
            {
				entityPOCO.AuthorizerExternalId = entityPM.AuthorizerExternalId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AuthorizerPassportNumber))
            {
				entityPOCO.AuthorizerPassportNumber = entityPM.AuthorizerPassportNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AuthorizerPassportCountry))
            {
				entityPOCO.AuthorizerPassportCountry = entityPM.AuthorizerPassportCountry;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AuthorizerPassportType))
            {
				entityPOCO.AuthorizerPassportType = entityPM.AuthorizerPassportType;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartDate))
            {
				entityPOCO.StartDate = entityPM.StartDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EndDate))
            {
				entityPOCO.EndDate = entityPM.EndDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PoaStatus))
            {
				entityPOCO.PoaStatus = entityPM.PoaStatus;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PoaAuthorizationType))
            {
				entityPOCO.PoaAuthorizationType = entityPM.PoaAuthorizationType;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(ClientsPoaPM entityPM, ClientsPoa entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ClientId))
            {
					entityPM.ClientId = entityPOCO.ClientId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.poaID))
            {
					entityPM.PoaID = entityPOCO.poaID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AuthorizedExternalId))
            {
					entityPM.AuthorizedExternalId = entityPOCO.AuthorizedExternalId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AuthorizerExternalId))
            {
					entityPM.AuthorizerExternalId = entityPOCO.AuthorizerExternalId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AuthorizerPassportNumber))
            {
					entityPM.AuthorizerPassportNumber = entityPOCO.AuthorizerPassportNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AuthorizerPassportCountry))
            {
					entityPM.AuthorizerPassportCountry = entityPOCO.AuthorizerPassportCountry;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AuthorizerPassportType))
            {
					entityPM.AuthorizerPassportType = entityPOCO.AuthorizerPassportType;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StartDate))
            {
					entityPM.StartDate = entityPOCO.StartDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EndDate))
            {
					entityPM.EndDate = entityPOCO.EndDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PoaStatus))
            {
					entityPM.PoaStatus = entityPOCO.PoaStatus;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PoaAuthorizationType))
            {
					entityPM.PoaAuthorizationType = entityPOCO.PoaAuthorizationType;
            }

		}

		public void PMToOldPM(ClientsPoaPM entityPM, ClientsPoaPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClientId))
            {
                oldEntityPM.ClientId = entityPM.ClientId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.poaID))
            {
                oldEntityPM.PoaID = entityPM.PoaID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AuthorizedExternalId))
            {
                oldEntityPM.AuthorizedExternalId = entityPM.AuthorizedExternalId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AuthorizerExternalId))
            {
                oldEntityPM.AuthorizerExternalId = entityPM.AuthorizerExternalId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AuthorizerPassportNumber))
            {
                oldEntityPM.AuthorizerPassportNumber = entityPM.AuthorizerPassportNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AuthorizerPassportCountry))
            {
                oldEntityPM.AuthorizerPassportCountry = entityPM.AuthorizerPassportCountry;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AuthorizerPassportType))
            {
                oldEntityPM.AuthorizerPassportType = entityPM.AuthorizerPassportType;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartDate))
            {
                oldEntityPM.StartDate = entityPM.StartDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EndDate))
            {
                oldEntityPM.EndDate = entityPM.EndDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PoaStatus))
            {
                oldEntityPM.PoaStatus = entityPM.PoaStatus;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PoaAuthorizationType))
            {
                oldEntityPM.PoaAuthorizationType = entityPM.PoaAuthorizationType;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(ClientsPoaPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
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
		
		private void BuildSearchFieldsGenerated(ClientsPoaPM entityPM, ClientsPoa entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 