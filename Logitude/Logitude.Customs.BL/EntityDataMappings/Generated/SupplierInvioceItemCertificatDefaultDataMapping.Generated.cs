
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
   
   public partial class SupplierInvioceItemCertificatDefaultDataMapping: IMapping<SupplierInvioceItemCertificatDefaultPM, SupplierInvioceItemCertificatDefault>,IMappingEncodeBase64NVARCHARFields<SupplierInvioceItemCertificatDefaultPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         SupplierInvioceExportDefaultId, 
	         CertificateNumber, 
	         Tenant, 
	         ReqConfirmationTypeCode, 
	         CertificateExemptionTypeCode, 
	         AttachmentTypeCode, 
	         ResConfirmationTypeCode, 
	         CustomsAttachmentID, 
	         SequenceNumeric,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         SupplierInvioceExportDefaultId, 
	         CertificateNumber, 
	         Tenant, 
	         ReqConfirmationTypeCode, 
	         CertificateExemptionTypeCode, 
	         AttachmentTypeCode, 
	         ResConfirmationTypeCode, 
	         CustomsAttachmentID, 
	         SequenceNumeric,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(SupplierInvioceItemCertificatDefaultPM entityPM, SupplierInvioceItemCertificatDefault entityPOCO)
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
			}

		public void POCOToPM(SupplierInvioceItemCertificatDefaultPM entityPM, SupplierInvioceItemCertificatDefault entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SupplierInvioceExportDefaultId))
            {
					entityPM.SupplierInvioceExportDefaultId = entityPOCO.SupplierInvioceExportDefaultId;
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

		}

		public void PMToOldPM(SupplierInvioceItemCertificatDefaultPM entityPM, SupplierInvioceItemCertificatDefaultPM oldEntityPM)
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
			
		}

	    public void EncodeBase64NVARCHARFields(SupplierInvioceItemCertificatDefaultPM entityPM)
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
	 