
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
   
   public partial class DeclarationCargoSplitDataMapping: IMapping<DeclarationCargoSplitPM, DeclarationCargoSplit>,IMappingEncodeBase64NVARCHARFields<DeclarationCargoSplitPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         RequestDate, 
	         SearchFields, 
	         ActionTypeCode, 
	         RequestReason, 
	         RequestNumber, 
	         RequestRemarks, 
	         CargoTypeCode, 
	         ManifestNumber, 
	         SecondCargoID, 
	         ThirdCargoID, 
	         DeclarationId, 
	         IsClosed, 
	         ResponseStatusCode,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         RequestDate, 
	         SearchFields, 
	         ActionTypeCode, 
	         RequestReason, 
	         RequestNumber, 
	         ActionTypeName, 
	         RequestReasonName, 
	         RequestRemarks, 
	         CargoTypeCode, 
	         CargoTypeName, 
	         ManifestNumber, 
	         SecondCargoID, 
	         ThirdCargoID, 
	         DeclarationId, 
	         IsClosed, 
	         ResponseStatusCode, 
	         ResponseStatusName, 
	         CustomFileNo,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(DeclarationCargoSplitPM entityPM, DeclarationCargoSplit entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequestDate))
            {
				entityPOCO.RequestDate = entityPM.RequestDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActionTypeCode))
            {
				entityPOCO.ActionTypeCode = entityPM.ActionTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequestReason))
            {
				entityPOCO.RequestReason = entityPM.RequestReason;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequestNumber))
            {
				entityPOCO.RequestNumber = entityPM.RequestNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequestRemarks))
            {
				entityPOCO.RequestRemarks = entityPM.RequestRemarks;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CargoTypeCode))
            {
				entityPOCO.CargoTypeCode = entityPM.CargoTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ManifestNumber))
            {
				entityPOCO.ManifestNumber = entityPM.ManifestNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SecondCargoID))
            {
				entityPOCO.SecondCargoID = entityPM.SecondCargoID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ThirdCargoID))
            {
				entityPOCO.ThirdCargoID = entityPM.ThirdCargoID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeclarationId))
            {
				entityPOCO.DeclarationId = entityPM.DeclarationId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsClosed))
            {
				entityPOCO.IsClosed = entityPM.IsClosed;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ResponseStatusCode))
            {
				entityPOCO.ResponseStatusCode = entityPM.ResponseStatusCode;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(DeclarationCargoSplitPM entityPM, DeclarationCargoSplit entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RequestDate))
            {
					entityPM.RequestDate = entityPOCO.RequestDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ActionTypeCode))
            {
					entityPM.ActionTypeCode = entityPOCO.ActionTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RequestReason))
            {
					entityPM.RequestReason = entityPOCO.RequestReason;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RequestNumber))
            {
					entityPM.RequestNumber = entityPOCO.RequestNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RequestRemarks))
            {
					entityPM.RequestRemarks = entityPOCO.RequestRemarks;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CargoTypeCode))
            {
					entityPM.CargoTypeCode = entityPOCO.CargoTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ManifestNumber))
            {
					entityPM.ManifestNumber = entityPOCO.ManifestNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SecondCargoID))
            {
					entityPM.SecondCargoID = entityPOCO.SecondCargoID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ThirdCargoID))
            {
					entityPM.ThirdCargoID = entityPOCO.ThirdCargoID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationId))
            {
					entityPM.DeclarationId = entityPOCO.DeclarationId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsClosed))
            {
					entityPM.IsClosed = entityPOCO.IsClosed;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ResponseStatusCode))
            {
					entityPM.ResponseStatusCode = entityPOCO.ResponseStatusCode;
            }

		}

		public void PMToOldPM(DeclarationCargoSplitPM entityPM, DeclarationCargoSplitPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequestDate))
            {
                oldEntityPM.RequestDate = entityPM.RequestDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActionTypeCode))
            {
                oldEntityPM.ActionTypeCode = entityPM.ActionTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequestReason))
            {
                oldEntityPM.RequestReason = entityPM.RequestReason;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequestNumber))
            {
                oldEntityPM.RequestNumber = entityPM.RequestNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequestRemarks))
            {
                oldEntityPM.RequestRemarks = entityPM.RequestRemarks;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CargoTypeCode))
            {
                oldEntityPM.CargoTypeCode = entityPM.CargoTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ManifestNumber))
            {
                oldEntityPM.ManifestNumber = entityPM.ManifestNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SecondCargoID))
            {
                oldEntityPM.SecondCargoID = entityPM.SecondCargoID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ThirdCargoID))
            {
                oldEntityPM.ThirdCargoID = entityPM.ThirdCargoID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeclarationId))
            {
                oldEntityPM.DeclarationId = entityPM.DeclarationId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsClosed))
            {
                oldEntityPM.IsClosed = entityPM.IsClosed;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ResponseStatusCode))
            {
                oldEntityPM.ResponseStatusCode = entityPM.ResponseStatusCode;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(DeclarationCargoSplitPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.RequestRemarks)) //T4 find type == nText 
            {
                entityPM.RequestRemarks = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.RequestRemarks));
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
		
		private void BuildSearchFieldsGenerated(DeclarationCargoSplitPM entityPM, DeclarationCargoSplit entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 