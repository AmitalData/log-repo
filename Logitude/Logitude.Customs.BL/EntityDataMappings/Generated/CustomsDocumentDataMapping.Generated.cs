
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
   
   public partial class CustomsDocumentDataMapping: IMapping<CustomsDocumentPM, CustomsDocument>,IMappingEncodeBase64NVARCHARFields<CustomsDocumentPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Tenant, 
	         DocumentsFilingId, 
	         CustomsDocId, 
	         DocumentStatusCode, 
	         DocumentRemarks, 
	         DocumentTypeCode, 
	         IsMetaDataReady, 
	         CustomRecievedDate, 
	         DocumentVersion, 
	         ExternalAttachmentId, 
	         IsPartOfDeclaration,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Tenant, 
	         DocumentsFilingId, 
	         CustomsDocId, 
	         DocumentStatusCode, 
	         DocumentRemarks, 
	         DocumentStatusName, 
	         Extension, 
	         FileSize, 
	         Name, 
	         DocumentTypeCode, 
	         DocumentTypeName, 
	         IsMetaDataReady, 
	         CustomRecievedDate, 
	         DocumentVersion, 
	         ExternalAttachmentId, 
	         IsPartOfDeclaration, 
	         DeclarationId, 
	         ClaimId, 
	         CurrentEntityId, 
	         CurrentTableName, 
	         CurrentCustomsDocumentsTicketId, 
	         ExternalEntityName, 
	         ExternalEntityReference, 
	         DocumentId, 
	         DocumentsFilingCode, 
	         IsSendToQueue, 
	         ForceRemoveCustomsDocId, 
	         CollateralId, 
	         IsDigitallySigned, 
	         SignersList, 
	         IsCustomSendTime, 
	         ParentRequestId, 
	         OcrScore, 
	         OcrStatusCode,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CustomsDocumentPM entityPM, CustomsDocument entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsDocId))
            {
				entityPOCO.CustomsDocId = entityPM.CustomsDocId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DocumentStatusCode))
            {
				entityPOCO.DocumentStatusCode = entityPM.DocumentStatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DocumentRemarks))
            {
				entityPOCO.DocumentRemarks = entityPM.DocumentRemarks;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DocumentTypeCode))
            {
				entityPOCO.DocumentTypeCode = entityPM.DocumentTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsMetaDataReady))
            {
				entityPOCO.IsMetaDataReady = entityPM.IsMetaDataReady;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomRecievedDate))
            {
				entityPOCO.CustomRecievedDate = entityPM.CustomRecievedDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DocumentVersion))
            {
				entityPOCO.DocumentVersion = entityPM.DocumentVersion;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExternalAttachmentId))
            {
				entityPOCO.ExternalAttachmentId = entityPM.ExternalAttachmentId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsPartOfDeclaration))
            {
				entityPOCO.IsPartOfDeclaration = entityPM.IsPartOfDeclaration;
			}
			}

		public void POCOToPM(CustomsDocumentPM entityPM, CustomsDocument entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DocumentsFilingId))
            {
					entityPM.DocumentsFilingId = entityPOCO.DocumentsFilingId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsDocId))
            {
					entityPM.CustomsDocId = entityPOCO.CustomsDocId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DocumentStatusCode))
            {
					entityPM.DocumentStatusCode = entityPOCO.DocumentStatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DocumentRemarks))
            {
					entityPM.DocumentRemarks = entityPOCO.DocumentRemarks;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DocumentTypeCode))
            {
					entityPM.DocumentTypeCode = entityPOCO.DocumentTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsMetaDataReady))
            {
					entityPM.IsMetaDataReady = entityPOCO.IsMetaDataReady;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomRecievedDate))
            {
					entityPM.CustomRecievedDate = entityPOCO.CustomRecievedDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DocumentVersion))
            {
					entityPM.DocumentVersion = entityPOCO.DocumentVersion;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExternalAttachmentId))
            {
					entityPM.ExternalAttachmentId = entityPOCO.ExternalAttachmentId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsPartOfDeclaration))
            {
					entityPM.IsPartOfDeclaration = entityPOCO.IsPartOfDeclaration;
            }

		}

		public void PMToOldPM(CustomsDocumentPM entityPM, CustomsDocumentPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsDocId))
            {
                oldEntityPM.CustomsDocId = entityPM.CustomsDocId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DocumentStatusCode))
            {
                oldEntityPM.DocumentStatusCode = entityPM.DocumentStatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DocumentRemarks))
            {
                oldEntityPM.DocumentRemarks = entityPM.DocumentRemarks;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DocumentTypeCode))
            {
                oldEntityPM.DocumentTypeCode = entityPM.DocumentTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsMetaDataReady))
            {
                oldEntityPM.IsMetaDataReady = entityPM.IsMetaDataReady;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomRecievedDate))
            {
                oldEntityPM.CustomRecievedDate = entityPM.CustomRecievedDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DocumentVersion))
            {
                oldEntityPM.DocumentVersion = entityPM.DocumentVersion;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExternalAttachmentId))
            {
                oldEntityPM.ExternalAttachmentId = entityPM.ExternalAttachmentId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsPartOfDeclaration))
            {
                oldEntityPM.IsPartOfDeclaration = entityPM.IsPartOfDeclaration;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CustomsDocumentPM entityPM)
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
	 