
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
   
   public partial class CertificateOfOriginMandatoryFieldsDataMapping: IMapping<CertificateOfOriginMandatoryFieldsPM, CertificateOfOriginMandatoryFields>,IMappingEncodeBase64NVARCHARFields<CertificateOfOriginMandatoryFieldsPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Code, 
	         LocalName, 
	         SearchFields, 
	         EnglishName, 
	         Inactive, 
	         IsMandatory, 
	         Location, 
	         LastUpdatedDate, 
	         MappedCertificateFields, 
	         CertificateOfOriginTypeCodeID, 
	         CertificateOfOriginTypeName,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Code, 
	         LocalName, 
	         SearchFields, 
	         EnglishName, 
	         Inactive, 
	         IsMandatory, 
	         Location, 
	         LastUpdatedDate, 
	         MappedCertificateFieldsName, 
	         MappedCertificateFields, 
	         CertificateOfOriginTypeCodeID, 
	         CertificateOfOriginTypeName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CertificateOfOriginMandatoryFieldsPM entityPM, CertificateOfOriginMandatoryFields entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalName))
            {
				entityPOCO.LocalName = entityPM.LocalName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishName))
            {
				entityPOCO.EnglishName = entityPM.EnglishName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Inactive))
            {
				entityPOCO.Inactive = entityPM.Inactive;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsMandatory))
            {
				entityPOCO.IsMandatory = entityPM.IsMandatory;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Location))
            {
				entityPOCO.Location = entityPM.Location;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastUpdatedDate))
            {
				entityPOCO.LastUpdatedDate = entityPM.LastUpdatedDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MappedCertificateFields))
            {
				entityPOCO.MappedCertificateFields = entityPM.MappedCertificateFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CertificateOfOriginTypeCodeID))
            {
				entityPOCO.CertificateOfOriginTypeCodeID = entityPM.CertificateOfOriginTypeCodeID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CertificateOfOriginTypeName))
            {
				entityPOCO.CertificateOfOriginTypeName = entityPM.CertificateOfOriginTypeName;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(CertificateOfOriginMandatoryFieldsPM entityPM, CertificateOfOriginMandatoryFields entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Code))
            {
					entityPM.Code = entityPOCO.Code;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LocalName))
            {
					entityPM.LocalName = entityPOCO.LocalName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EnglishName))
            {
					entityPM.EnglishName = entityPOCO.EnglishName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Inactive))
            {
					entityPM.Inactive = entityPOCO.Inactive;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsMandatory))
            {
					entityPM.IsMandatory = entityPOCO.IsMandatory;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Location))
            {
					entityPM.Location = entityPOCO.Location;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastUpdatedDate))
            {
					entityPM.LastUpdatedDate = entityPOCO.LastUpdatedDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MappedCertificateFields))
            {
					entityPM.MappedCertificateFields = entityPOCO.MappedCertificateFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CertificateOfOriginTypeCodeID))
            {
					entityPM.CertificateOfOriginTypeCodeID = entityPOCO.CertificateOfOriginTypeCodeID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CertificateOfOriginTypeName))
            {
					entityPM.CertificateOfOriginTypeName = entityPOCO.CertificateOfOriginTypeName;
            }

		}

		public void PMToOldPM(CertificateOfOriginMandatoryFieldsPM entityPM, CertificateOfOriginMandatoryFieldsPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalName))
            {
                oldEntityPM.LocalName = entityPM.LocalName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishName))
            {
                oldEntityPM.EnglishName = entityPM.EnglishName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Inactive))
            {
                oldEntityPM.Inactive = entityPM.Inactive;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsMandatory))
            {
                oldEntityPM.IsMandatory = entityPM.IsMandatory;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Location))
            {
                oldEntityPM.Location = entityPM.Location;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastUpdatedDate))
            {
                oldEntityPM.LastUpdatedDate = entityPM.LastUpdatedDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MappedCertificateFields))
            {
                oldEntityPM.MappedCertificateFields = entityPM.MappedCertificateFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CertificateOfOriginTypeCodeID))
            {
                oldEntityPM.CertificateOfOriginTypeCodeID = entityPM.CertificateOfOriginTypeCodeID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CertificateOfOriginTypeName))
            {
                oldEntityPM.CertificateOfOriginTypeName = entityPM.CertificateOfOriginTypeName;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CertificateOfOriginMandatoryFieldsPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.LocalName)) //T4 find type == nText 
            {
                entityPM.LocalName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.LocalName));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.CertificateOfOriginTypeName)) //T4 find type == nText 
            {
                entityPM.CertificateOfOriginTypeName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.CertificateOfOriginTypeName));
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
		
		private void BuildSearchFieldsGenerated(CertificateOfOriginMandatoryFieldsPM entityPM, CertificateOfOriginMandatoryFields entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 