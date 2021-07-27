
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
   
   public partial class CustomsDocumentPointerDataMapping: IMapping<CustomsDocumentPointerPM, CustomsDocumentPointer>,IMappingEncodeBase64NVARCHARFields<CustomsDocumentPointerPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         ParentEntityCode, 
	         ParentEntityId, 
	         Child1EntityCode, 
	         Child1EntityId, 
	         Child2EntityCode, 
	         Child2EntityId, 
	         Child3EntityCode, 
	         Child3EntityId, 
	         CustomsDocumentsTicketId, 
	         OriginEntity,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         ParentEntityCode, 
	         ParentEntityId, 
	         Child1EntityCode, 
	         Child1EntityId, 
	         Child2EntityCode, 
	         Child2EntityId, 
	         Child3EntityCode, 
	         Child3EntityId, 
	         CustomsDocId, 
	         DocumentStatusCode, 
	         DocumentRemarks, 
	         DocumentStatusName, 
	         Extension, 
	         FileSize, 
	         Name, 
	         IsMetaDataReady, 
	         CustomsDocumentsTicketId, 
	         CustomsRequestsSheetId, 
	         DocumentTypeCode, 
	         OriginEntity,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CustomsDocumentPointerPM entityPM, CustomsDocumentPointer entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ParentEntityCode))
            {
				entityPOCO.ParentEntityCode = entityPM.ParentEntityCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ParentEntityId))
            {
				entityPOCO.ParentEntityId = entityPM.ParentEntityId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Child1EntityCode))
            {
				entityPOCO.Child1EntityCode = entityPM.Child1EntityCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Child1EntityId))
            {
				entityPOCO.Child1EntityId = entityPM.Child1EntityId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Child2EntityCode))
            {
				entityPOCO.Child2EntityCode = entityPM.Child2EntityCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Child2EntityId))
            {
				entityPOCO.Child2EntityId = entityPM.Child2EntityId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Child3EntityCode))
            {
				entityPOCO.Child3EntityCode = entityPM.Child3EntityCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Child3EntityId))
            {
				entityPOCO.Child3EntityId = entityPM.Child3EntityId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsDocumentsTicketId))
            {
				entityPOCO.CustomsDocumentsTicketId = entityPM.CustomsDocumentsTicketId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginEntity))
            {
				entityPOCO.OriginEntity = entityPM.OriginEntity;
			}
			}

		public void POCOToPM(CustomsDocumentPointerPM entityPM, CustomsDocumentPointer entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ParentEntityCode))
            {
					entityPM.ParentEntityCode = entityPOCO.ParentEntityCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ParentEntityId))
            {
					entityPM.ParentEntityId = entityPOCO.ParentEntityId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Child1EntityCode))
            {
					entityPM.Child1EntityCode = entityPOCO.Child1EntityCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Child1EntityId))
            {
					entityPM.Child1EntityId = entityPOCO.Child1EntityId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Child2EntityCode))
            {
					entityPM.Child2EntityCode = entityPOCO.Child2EntityCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Child2EntityId))
            {
					entityPM.Child2EntityId = entityPOCO.Child2EntityId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Child3EntityCode))
            {
					entityPM.Child3EntityCode = entityPOCO.Child3EntityCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Child3EntityId))
            {
					entityPM.Child3EntityId = entityPOCO.Child3EntityId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsDocumentsTicketId))
            {
					entityPM.CustomsDocumentsTicketId = entityPOCO.CustomsDocumentsTicketId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OriginEntity))
            {
					entityPM.OriginEntity = entityPOCO.OriginEntity;
            }

		}

		public void PMToOldPM(CustomsDocumentPointerPM entityPM, CustomsDocumentPointerPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ParentEntityCode))
            {
                oldEntityPM.ParentEntityCode = entityPM.ParentEntityCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ParentEntityId))
            {
                oldEntityPM.ParentEntityId = entityPM.ParentEntityId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Child1EntityCode))
            {
                oldEntityPM.Child1EntityCode = entityPM.Child1EntityCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Child1EntityId))
            {
                oldEntityPM.Child1EntityId = entityPM.Child1EntityId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Child2EntityCode))
            {
                oldEntityPM.Child2EntityCode = entityPM.Child2EntityCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Child2EntityId))
            {
                oldEntityPM.Child2EntityId = entityPM.Child2EntityId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Child3EntityCode))
            {
                oldEntityPM.Child3EntityCode = entityPM.Child3EntityCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Child3EntityId))
            {
                oldEntityPM.Child3EntityId = entityPM.Child3EntityId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsDocumentsTicketId))
            {
                oldEntityPM.CustomsDocumentsTicketId = entityPM.CustomsDocumentsTicketId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginEntity))
            {
                oldEntityPM.OriginEntity = entityPM.OriginEntity;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CustomsDocumentPointerPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

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
	 