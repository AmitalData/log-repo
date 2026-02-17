
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
   
   public partial class SupplierInvioceItemCertificatDataMapping: IMapping<SupplierInvioceItemCertificatPM, SupplierInvioceItemCertificat>,IMappingEncodeBase64NVARCHARFields<SupplierInvioceItemCertificatPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         DeclarationId, 
	         InvoiceCounterKey, 
	         LineNumber, 
	         ItemCertificateCounterKey, 
	         CertificateNumber, 
	         Tenant, 
	         ReqConfirmationTypeCode, 
	         CertificateExemptionTypeCode, 
	         AttachmentTypeCode, 
	         ResConfirmationTypeCode, 
	         CustomsAttachmentID, 
	         SequenceNumeric, 
	         ExternalCertificatCode, 
	         ExternalRequestTypeCode, 
	         ApprovalRequestNumber,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         DeclarationId, 
	         InvoiceCounterKey, 
	         LineNumber, 
	         ItemCertificateCounterKey, 
	         CertificateNumber, 
	         Tenant, 
	         ReqConfirmationTypeCode, 
	         CertificateExemptionTypeCode, 
	         AttachmentTypeCode, 
	         ResConfirmationTypeCode, 
	         CustomsAttachmentID, 
	         ReqConfirmationTypeName, 
	         CertificateExemptionTypeName, 
	         AttachmentTypeName, 
	         ResConfirmationTypeName, 
	         SequenceNumeric, 
	         ExternalCertificatCode, 
	         ExternalRequestTypeCode, 
	         ApprovalRequestNumber,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(SupplierInvioceItemCertificatPM entityPM, SupplierInvioceItemCertificat entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CertificateNumber))
            {
				entityPOCO.CertificateNumber = entityPM.CertificateNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReqConfirmationTypeCode))
            {
				entityPOCO.ReqConfirmationTypeCode = entityPM.ReqConfirmationTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CertificateExemptionTypeCode))
            {
				entityPOCO.CertificateExemptionTypeCode = entityPM.CertificateExemptionTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AttachmentTypeCode))
            {
				entityPOCO.AttachmentTypeCode = entityPM.AttachmentTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ResConfirmationTypeCode))
            {
				entityPOCO.ResConfirmationTypeCode = entityPM.ResConfirmationTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsAttachmentID))
            {
				entityPOCO.CustomsAttachmentID = entityPM.CustomsAttachmentID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SequenceNumeric))
            {
				entityPOCO.SequenceNumeric = entityPM.SequenceNumeric;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExternalCertificatCode))
            {
				entityPOCO.ExternalCertificatCode = entityPM.ExternalCertificatCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExternalRequestTypeCode))
            {
				entityPOCO.ExternalRequestTypeCode = entityPM.ExternalRequestTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ApprovalRequestNumber))
            {
				entityPOCO.ApprovalRequestNumber = entityPM.ApprovalRequestNumber;
			}
			}

		public void POCOToPM(SupplierInvioceItemCertificatPM entityPM, SupplierInvioceItemCertificat entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationId))
            {
					entityPM.DeclarationId = entityPOCO.DeclarationId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InvoiceCounterKey))
            {
					entityPM.InvoiceCounterKey = entityPOCO.InvoiceCounterKey;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LineNumber))
            {
					entityPM.LineNumber = entityPOCO.LineNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ItemCertificateCounterKey))
            {
					entityPM.ItemCertificateCounterKey = entityPOCO.ItemCertificateCounterKey;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CertificateNumber))
            {
					entityPM.CertificateNumber = entityPOCO.CertificateNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ReqConfirmationTypeCode))
            {
					entityPM.ReqConfirmationTypeCode = entityPOCO.ReqConfirmationTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CertificateExemptionTypeCode))
            {
					entityPM.CertificateExemptionTypeCode = entityPOCO.CertificateExemptionTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AttachmentTypeCode))
            {
					entityPM.AttachmentTypeCode = entityPOCO.AttachmentTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ResConfirmationTypeCode))
            {
					entityPM.ResConfirmationTypeCode = entityPOCO.ResConfirmationTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsAttachmentID))
            {
					entityPM.CustomsAttachmentID = entityPOCO.CustomsAttachmentID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SequenceNumeric))
            {
					entityPM.SequenceNumeric = entityPOCO.SequenceNumeric;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExternalCertificatCode))
            {
					entityPM.ExternalCertificatCode = entityPOCO.ExternalCertificatCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExternalRequestTypeCode))
            {
					entityPM.ExternalRequestTypeCode = entityPOCO.ExternalRequestTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ApprovalRequestNumber))
            {
					entityPM.ApprovalRequestNumber = entityPOCO.ApprovalRequestNumber;
            }

		}

		public void PMToOldPM(SupplierInvioceItemCertificatPM entityPM, SupplierInvioceItemCertificatPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CertificateNumber))
            {
                oldEntityPM.CertificateNumber = entityPM.CertificateNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReqConfirmationTypeCode))
            {
                oldEntityPM.ReqConfirmationTypeCode = entityPM.ReqConfirmationTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CertificateExemptionTypeCode))
            {
                oldEntityPM.CertificateExemptionTypeCode = entityPM.CertificateExemptionTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AttachmentTypeCode))
            {
                oldEntityPM.AttachmentTypeCode = entityPM.AttachmentTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ResConfirmationTypeCode))
            {
                oldEntityPM.ResConfirmationTypeCode = entityPM.ResConfirmationTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsAttachmentID))
            {
                oldEntityPM.CustomsAttachmentID = entityPM.CustomsAttachmentID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SequenceNumeric))
            {
                oldEntityPM.SequenceNumeric = entityPM.SequenceNumeric;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExternalCertificatCode))
            {
                oldEntityPM.ExternalCertificatCode = entityPM.ExternalCertificatCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExternalRequestTypeCode))
            {
                oldEntityPM.ExternalRequestTypeCode = entityPM.ExternalRequestTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ApprovalRequestNumber))
            {
                oldEntityPM.ApprovalRequestNumber = entityPM.ApprovalRequestNumber;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(SupplierInvioceItemCertificatPM entityPM)
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
	 