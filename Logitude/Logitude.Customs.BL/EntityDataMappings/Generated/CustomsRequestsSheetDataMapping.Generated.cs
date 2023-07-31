
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
   
   public partial class CustomsRequestsSheetDataMapping: IMapping<CustomsRequestsSheetPM, CustomsRequestsSheet>,IMappingEncodeBase64NVARCHARFields<CustomsRequestsSheetPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         ObjectTableId1, 
	         EntityId1, 
	         ObjectTableId2, 
	         EntityId2, 
	         RequestStatusCode, 
	         RequestCreateDate, 
	         AnswerCreateDate, 
	         RequestOwnerId, 
	         RequestComminicationId, 
	         RequestDescription, 
	         InterfaceTypeCode, 
	         EntityReference, 
	         CustomFileNo, 
	         CorrelationId, 
	         IsDCA, 
	         SearchFields, 
	         IsRestored, 
	         AnalyzeDcaAggregateKey, 
	         TenantPriority, 
	         IsHSM,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         ObjectTableId1, 
	         EntityId1, 
	         ObjectTableId2, 
	         EntityId2, 
	         RequestStatusCode, 
	         RequestCreateDate, 
	         AnswerCreateDate, 
	         RequestOwnerId, 
	         RequestComminicationId, 
	         RequestDescription, 
	         InterfaceTypeCode, 
	         EntityReference, 
	         CustomFileNo, 
	         CorrelationId, 
	         IsDCA, 
	         SearchFields, 
	         InterfaceTypeName, 
	         RequestOwnerName, 
	         RequestStatusName, 
	         IsRestored, 
	         AnalyzeDcaAggregateKey, 
	         FutureSendDateTime, 
	         TenantPriority, 
	         IsHSM,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CustomsRequestsSheetPM entityPM, CustomsRequestsSheet entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ObjectTableId1))
            {
				entityPOCO.ObjectTableId1 = entityPM.ObjectTableId1;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityId1))
            {
				entityPOCO.EntityId1 = entityPM.EntityId1;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ObjectTableId2))
            {
				entityPOCO.ObjectTableId2 = entityPM.ObjectTableId2;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityId2))
            {
				entityPOCO.EntityId2 = entityPM.EntityId2;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequestStatusCode))
            {
				entityPOCO.RequestStatusCode = entityPM.RequestStatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequestCreateDate))
            {
				entityPOCO.RequestCreateDate = entityPM.RequestCreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AnswerCreateDate))
            {
				entityPOCO.AnswerCreateDate = entityPM.AnswerCreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequestOwnerId))
            {
				entityPOCO.RequestOwnerId = entityPM.RequestOwnerId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequestComminicationId))
            {
				entityPOCO.RequestComminicationId = entityPM.RequestComminicationId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequestDescription))
            {
				entityPOCO.RequestDescription = entityPM.RequestDescription;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InterfaceTypeCode))
            {
				entityPOCO.InterfaceTypeCode = entityPM.InterfaceTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityReference))
            {
				entityPOCO.EntityReference = entityPM.EntityReference;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomFileNo))
            {
				entityPOCO.CustomFileNo = entityPM.CustomFileNo;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CorrelationId))
            {
				entityPOCO.CorrelationId = entityPM.CorrelationId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsDCA))
            {
				entityPOCO.IsDCA = entityPM.IsDCA;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsRestored))
            {
				entityPOCO.IsRestored = entityPM.IsRestored;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AnalyzeDcaAggregateKey))
            {
				entityPOCO.AnalyzeDcaAggregateKey = entityPM.AnalyzeDcaAggregateKey;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TenantPriority))
            {
				entityPOCO.TenantPriority = entityPM.TenantPriority;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsHSM))
            {
				entityPOCO.IsHSM = entityPM.IsHSM;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(CustomsRequestsSheetPM entityPM, CustomsRequestsSheet entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ObjectTableId1))
            {
					entityPM.ObjectTableId1 = entityPOCO.ObjectTableId1;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EntityId1))
            {
					entityPM.EntityId1 = entityPOCO.EntityId1;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ObjectTableId2))
            {
					entityPM.ObjectTableId2 = entityPOCO.ObjectTableId2;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EntityId2))
            {
					entityPM.EntityId2 = entityPOCO.EntityId2;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RequestStatusCode))
            {
					entityPM.RequestStatusCode = entityPOCO.RequestStatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RequestCreateDate))
            {
					entityPM.RequestCreateDate = entityPOCO.RequestCreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AnswerCreateDate))
            {
					entityPM.AnswerCreateDate = entityPOCO.AnswerCreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RequestOwnerId))
            {
					entityPM.RequestOwnerId = entityPOCO.RequestOwnerId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RequestComminicationId))
            {
					entityPM.RequestComminicationId = entityPOCO.RequestComminicationId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RequestDescription))
            {
					entityPM.RequestDescription = entityPOCO.RequestDescription;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InterfaceTypeCode))
            {
					entityPM.InterfaceTypeCode = entityPOCO.InterfaceTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EntityReference))
            {
					entityPM.EntityReference = entityPOCO.EntityReference;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomFileNo))
            {
					entityPM.CustomFileNo = entityPOCO.CustomFileNo;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CorrelationId))
            {
					entityPM.CorrelationId = entityPOCO.CorrelationId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsDCA))
            {
					entityPM.IsDCA = entityPOCO.IsDCA;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsRestored))
            {
					entityPM.IsRestored = entityPOCO.IsRestored;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AnalyzeDcaAggregateKey))
            {
					entityPM.AnalyzeDcaAggregateKey = entityPOCO.AnalyzeDcaAggregateKey;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TenantPriority))
            {
					entityPM.TenantPriority = entityPOCO.TenantPriority;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsHSM))
            {
					entityPM.IsHSM = entityPOCO.IsHSM;
            }

		}

		public void PMToOldPM(CustomsRequestsSheetPM entityPM, CustomsRequestsSheetPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ObjectTableId1))
            {
                oldEntityPM.ObjectTableId1 = entityPM.ObjectTableId1;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityId1))
            {
                oldEntityPM.EntityId1 = entityPM.EntityId1;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ObjectTableId2))
            {
                oldEntityPM.ObjectTableId2 = entityPM.ObjectTableId2;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityId2))
            {
                oldEntityPM.EntityId2 = entityPM.EntityId2;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequestStatusCode))
            {
                oldEntityPM.RequestStatusCode = entityPM.RequestStatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequestCreateDate))
            {
                oldEntityPM.RequestCreateDate = entityPM.RequestCreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AnswerCreateDate))
            {
                oldEntityPM.AnswerCreateDate = entityPM.AnswerCreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequestOwnerId))
            {
                oldEntityPM.RequestOwnerId = entityPM.RequestOwnerId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequestComminicationId))
            {
                oldEntityPM.RequestComminicationId = entityPM.RequestComminicationId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequestDescription))
            {
                oldEntityPM.RequestDescription = entityPM.RequestDescription;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InterfaceTypeCode))
            {
                oldEntityPM.InterfaceTypeCode = entityPM.InterfaceTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityReference))
            {
                oldEntityPM.EntityReference = entityPM.EntityReference;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomFileNo))
            {
                oldEntityPM.CustomFileNo = entityPM.CustomFileNo;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CorrelationId))
            {
                oldEntityPM.CorrelationId = entityPM.CorrelationId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsDCA))
            {
                oldEntityPM.IsDCA = entityPM.IsDCA;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsRestored))
            {
                oldEntityPM.IsRestored = entityPM.IsRestored;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AnalyzeDcaAggregateKey))
            {
                oldEntityPM.AnalyzeDcaAggregateKey = entityPM.AnalyzeDcaAggregateKey;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TenantPriority))
            {
                oldEntityPM.TenantPriority = entityPM.TenantPriority;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsHSM))
            {
                oldEntityPM.IsHSM = entityPM.IsHSM;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CustomsRequestsSheetPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.RequestDescription)) //T4 find type == nText 
            {
                entityPM.RequestDescription = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.RequestDescription));
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
		
		private void BuildSearchFieldsGenerated(CustomsRequestsSheetPM entityPM, CustomsRequestsSheet entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 