
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
   
   public partial class CourierPendingReasonDataMapping: IMapping<CourierPendingReasonPM, CourierPendingReason>,IMappingEncodeBase64NVARCHARFields<CourierPendingReasonPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Code, 
	         LocalName, 
	         SearchFields, 
	         EnglishName, 
	         Inactive, 
	         ErrorPlace, 
	         Tenant, 
	         UnifreightStatusCode, 
	         MamanSuspendedCode, 
	         SwissportSuspendedCode, 
	         RequiresApproval, 
	         RequiresPayment, 
	         OverseasSuspendedCode,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Code, 
	         LocalName, 
	         SearchFields, 
	         EnglishName, 
	         Inactive, 
	         ErrorPlace, 
	         Tenant, 
	         UnifreightStatusCode, 
	         ErrorPlaceName, 
	         MamanSuspendedCode, 
	         SwissportSuspendedCode, 
	         RequiresApproval, 
	         RequiresPayment, 
	         OverseasSuspendedCode,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CourierPendingReasonPM entityPM, CourierPendingReason entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Id))
            {
				entityPOCO.Id = entityPM.Id;
			}
			
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ErrorPlace))
            {
				entityPOCO.ErrorPlace = entityPM.ErrorPlace;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UnifreightStatusCode))
            {
				entityPOCO.UnifreightStatusCode = entityPM.UnifreightStatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MamanSuspendedCode))
            {
				entityPOCO.MamanSuspendedCode = entityPM.MamanSuspendedCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SwissportSuspendedCode))
            {
				entityPOCO.SwissportSuspendedCode = entityPM.SwissportSuspendedCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequiresApproval))
            {
				entityPOCO.RequiresApproval = entityPM.RequiresApproval;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequiresPayment))
            {
				entityPOCO.RequiresPayment = entityPM.RequiresPayment;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OverseasSuspendedCode))
            {
				entityPOCO.OverseasSuspendedCode = entityPM.OverseasSuspendedCode;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(CourierPendingReasonPM entityPM, CourierPendingReason entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ErrorPlace))
            {
					entityPM.ErrorPlace = entityPOCO.ErrorPlace;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UnifreightStatusCode))
            {
					entityPM.UnifreightStatusCode = entityPOCO.UnifreightStatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MamanSuspendedCode))
            {
					entityPM.MamanSuspendedCode = entityPOCO.MamanSuspendedCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SwissportSuspendedCode))
            {
					entityPM.SwissportSuspendedCode = entityPOCO.SwissportSuspendedCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RequiresApproval))
            {
					entityPM.RequiresApproval = entityPOCO.RequiresApproval;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RequiresPayment))
            {
					entityPM.RequiresPayment = entityPOCO.RequiresPayment;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OverseasSuspendedCode))
            {
					entityPM.OverseasSuspendedCode = entityPOCO.OverseasSuspendedCode;
            }

		}

		public void PMToOldPM(CourierPendingReasonPM entityPM, CourierPendingReasonPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Id))
            {
                oldEntityPM.Id = entityPM.Id;
            }
			
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ErrorPlace))
            {
                oldEntityPM.ErrorPlace = entityPM.ErrorPlace;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UnifreightStatusCode))
            {
                oldEntityPM.UnifreightStatusCode = entityPM.UnifreightStatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MamanSuspendedCode))
            {
                oldEntityPM.MamanSuspendedCode = entityPM.MamanSuspendedCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SwissportSuspendedCode))
            {
                oldEntityPM.SwissportSuspendedCode = entityPM.SwissportSuspendedCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequiresApproval))
            {
                oldEntityPM.RequiresApproval = entityPM.RequiresApproval;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequiresPayment))
            {
                oldEntityPM.RequiresPayment = entityPM.RequiresPayment;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OverseasSuspendedCode))
            {
                oldEntityPM.OverseasSuspendedCode = entityPM.OverseasSuspendedCode;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CourierPendingReasonPM entityPM)
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
		
		private void BuildSearchFieldsGenerated(CourierPendingReasonPM entityPM, CourierPendingReason entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 