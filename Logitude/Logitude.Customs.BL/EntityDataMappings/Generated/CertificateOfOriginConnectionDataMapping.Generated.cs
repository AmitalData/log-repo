
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
   
   public partial class CertificateOfOriginConnectionDataMapping: IMapping<CertificateOfOriginConnectionPM, CertificateOfOriginConnection>,IMappingEncodeBase64NVARCHARFields<CertificateOfOriginConnectionPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         SearchFields, 
	         CooStatus, 
	         CooReason, 
	         Active,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         SearchFields, 
	         CooStatus, 
	         CooReason, 
	         Active,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CertificateOfOriginConnectionPM entityPM, CertificateOfOriginConnection entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CooStatus))
            {
				entityPOCO.CooStatus = entityPM.CooStatus;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CooReason))
            {
				entityPOCO.CooReason = entityPM.CooReason;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Active))
            {
				entityPOCO.Active = entityPM.Active;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(CertificateOfOriginConnectionPM entityPM, CertificateOfOriginConnection entityPOCO)
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CooStatus))
            {
					entityPM.CooStatus = entityPOCO.CooStatus;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CooReason))
            {
					entityPM.CooReason = entityPOCO.CooReason;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Active))
            {
					entityPM.Active = entityPOCO.Active;
            }

		}

		public void PMToOldPM(CertificateOfOriginConnectionPM entityPM, CertificateOfOriginConnectionPM oldEntityPM)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CooStatus))
            {
                oldEntityPM.CooStatus = entityPM.CooStatus;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CooReason))
            {
                oldEntityPM.CooReason = entityPM.CooReason;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Active))
            {
                oldEntityPM.Active = entityPM.Active;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CertificateOfOriginConnectionPM entityPM)
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
		
		private void BuildSearchFieldsGenerated(CertificateOfOriginConnectionPM entityPM, CertificateOfOriginConnection entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 