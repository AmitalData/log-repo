
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
   
   public partial class DeclarationCasualDetailsDataMapping: IMapping<DeclarationCasualDetailsPM, DeclarationCasualDetails>,IMappingEncodeBase64NVARCHARFields<DeclarationCasualDetailsPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         DeclarationId, 
	         Tenant, 
	         CasualSupplierName, 
	         CasualSupplierAddress, 
	         CasualImporterAddress1, 
	         CasualImporterAddress2, 
	         CasualImporterCity, 
	         CasualImporterZipCode, 
	         CasualImporterFax, 
	         CasualImporterEmail, 
	         CasualImporterTel, 
	         CasualImporterContact,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         DeclarationId, 
	         Tenant, 
	         CasualSupplierName, 
	         CasualSupplierAddress, 
	         CasualImporterAddress1, 
	         CasualImporterAddress2, 
	         CasualImporterCity, 
	         CasualImporterZipCode, 
	         CasualImporterFax, 
	         CasualImporterEmail, 
	         CasualImporterTel, 
	         CasualImporterContact,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(DeclarationCasualDetailsPM entityPM, DeclarationCasualDetails entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualSupplierName))
            {
				entityPOCO.CasualSupplierName = entityPM.CasualSupplierName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualSupplierAddress))
            {
				entityPOCO.CasualSupplierAddress = entityPM.CasualSupplierAddress;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualImporterAddress1))
            {
				entityPOCO.CasualImporterAddress1 = entityPM.CasualImporterAddress1;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualImporterAddress2))
            {
				entityPOCO.CasualImporterAddress2 = entityPM.CasualImporterAddress2;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualImporterCity))
            {
				entityPOCO.CasualImporterCity = entityPM.CasualImporterCity;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualImporterZipCode))
            {
				entityPOCO.CasualImporterZipCode = entityPM.CasualImporterZipCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualImporterFax))
            {
				entityPOCO.CasualImporterFax = entityPM.CasualImporterFax;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualImporterEmail))
            {
				entityPOCO.CasualImporterEmail = entityPM.CasualImporterEmail;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualImporterTel))
            {
				entityPOCO.CasualImporterTel = entityPM.CasualImporterTel;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualImporterContact))
            {
				entityPOCO.CasualImporterContact = entityPM.CasualImporterContact;
			}
			}

		public void POCOToPM(DeclarationCasualDetailsPM entityPM, DeclarationCasualDetails entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationId))
            {
					entityPM.DeclarationId = entityPOCO.DeclarationId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CasualSupplierName))
            {
					entityPM.CasualSupplierName = entityPOCO.CasualSupplierName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CasualSupplierAddress))
            {
					entityPM.CasualSupplierAddress = entityPOCO.CasualSupplierAddress;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CasualImporterAddress1))
            {
					entityPM.CasualImporterAddress1 = entityPOCO.CasualImporterAddress1;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CasualImporterAddress2))
            {
					entityPM.CasualImporterAddress2 = entityPOCO.CasualImporterAddress2;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CasualImporterCity))
            {
					entityPM.CasualImporterCity = entityPOCO.CasualImporterCity;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CasualImporterZipCode))
            {
					entityPM.CasualImporterZipCode = entityPOCO.CasualImporterZipCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CasualImporterFax))
            {
					entityPM.CasualImporterFax = entityPOCO.CasualImporterFax;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CasualImporterEmail))
            {
					entityPM.CasualImporterEmail = entityPOCO.CasualImporterEmail;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CasualImporterTel))
            {
					entityPM.CasualImporterTel = entityPOCO.CasualImporterTel;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CasualImporterContact))
            {
					entityPM.CasualImporterContact = entityPOCO.CasualImporterContact;
            }

		}

		public void PMToOldPM(DeclarationCasualDetailsPM entityPM, DeclarationCasualDetailsPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualSupplierName))
            {
                oldEntityPM.CasualSupplierName = entityPM.CasualSupplierName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualSupplierAddress))
            {
                oldEntityPM.CasualSupplierAddress = entityPM.CasualSupplierAddress;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualImporterAddress1))
            {
                oldEntityPM.CasualImporterAddress1 = entityPM.CasualImporterAddress1;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualImporterAddress2))
            {
                oldEntityPM.CasualImporterAddress2 = entityPM.CasualImporterAddress2;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualImporterCity))
            {
                oldEntityPM.CasualImporterCity = entityPM.CasualImporterCity;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualImporterZipCode))
            {
                oldEntityPM.CasualImporterZipCode = entityPM.CasualImporterZipCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualImporterFax))
            {
                oldEntityPM.CasualImporterFax = entityPM.CasualImporterFax;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualImporterEmail))
            {
                oldEntityPM.CasualImporterEmail = entityPM.CasualImporterEmail;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualImporterTel))
            {
                oldEntityPM.CasualImporterTel = entityPM.CasualImporterTel;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualImporterContact))
            {
                oldEntityPM.CasualImporterContact = entityPM.CasualImporterContact;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(DeclarationCasualDetailsPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.CasualSupplierName)) //T4 find type == nText 
            {
                entityPM.CasualSupplierName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.CasualSupplierName));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.CasualSupplierAddress)) //T4 find type == nText 
            {
                entityPM.CasualSupplierAddress = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.CasualSupplierAddress));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.CasualImporterAddress1)) //T4 find type == nText 
            {
                entityPM.CasualImporterAddress1 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.CasualImporterAddress1));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.CasualImporterAddress2)) //T4 find type == nText 
            {
                entityPM.CasualImporterAddress2 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.CasualImporterAddress2));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.CasualImporterCity)) //T4 find type == nText 
            {
                entityPM.CasualImporterCity = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.CasualImporterCity));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.CasualImporterContact)) //T4 find type == nText 
            {
                entityPM.CasualImporterContact = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.CasualImporterContact));
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
	 