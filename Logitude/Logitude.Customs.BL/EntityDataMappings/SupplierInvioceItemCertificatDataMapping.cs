
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;
using Logitude.Customs.BL.EntityQueryServices;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class SupplierInvioceItemCertificatDataMapping: IMapping<SupplierInvioceItemCertificatPM, SupplierInvioceItemCertificat>
   {

        public void CustomPMToPOCO(SupplierInvioceItemCertificatPM entityPM, SupplierInvioceItemCertificat entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.DeclarationId);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.InvoiceCounterKey);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.ItemCertificateCounterKey);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.LineNumber);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);


            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
               
                entityPOCO.DeclarationId = entityPM.DeclarationId;             
                entityPOCO.InvoiceCounterKey = entityPM.InvoiceCounterKey;                
                entityPOCO.ItemCertificateCounterKey = entityPM.ItemCertificateCounterKey;               
                entityPOCO.LineNumber = entityPM.LineNumber;               
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(SupplierInvioceItemCertificatPM entityPM, SupplierInvioceItemCertificat entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.AttachmentTypeName);
            CustomMappedPMProperties.Add(PMPropertyNames.CertificateExemptionTypeName);
            CustomMappedPMProperties.Add(PMPropertyNames.ReqConfirmationTypeName);

            if (entityPOCO.AttachmentTypeCode != null)
            {
                AttachmentTypeQueryService attachementTypeQueryService = new AttachmentTypeQueryService(entityPOCO.Tenant);
                AttachmentTypePM attachementType = attachementTypeQueryService.GetSingle(entityPOCO.AttachmentTypeCode, false, true);
                entityPM.AttachmentTypeName = attachementType.LocalName;
            }

            if (entityPOCO.CertificateExemptionTypeCode != null)
            {
                CertificateExemptionTypeQueryService certificateExemptionTypeQueryService = new CertificateExemptionTypeQueryService(entityPOCO.Tenant);
                CertificateExemptionTypePM certificateExemptionType = certificateExemptionTypeQueryService.GetSingle(entityPOCO.CertificateExemptionTypeCode, false, true);
                entityPM.CertificateExemptionTypeName = certificateExemptionType.LocalName;
            }

            if (entityPOCO.ReqConfirmationTypeCode != null)
            {
                ConfirmationTypeQueryService confirmationTypeQueryService = new ConfirmationTypeQueryService(entityPOCO.Tenant);
                ConfirmationTypePM confirmationType = confirmationTypeQueryService.GetSingle(entityPOCO.ReqConfirmationTypeCode, false, true);
                entityPM.ReqConfirmationTypeName = confirmationType.LocalName;
            }

            if (entityPOCO.ResConfirmationTypeCode != null)
            {
                ConfirmationTypeQueryService confirmationTypeQueryService = new ConfirmationTypeQueryService(entityPOCO.Tenant);
                ConfirmationTypePM confirmationType = confirmationTypeQueryService.GetSingle(entityPOCO.ResConfirmationTypeCode, false, true);
                entityPM.ResConfirmationTypeName = confirmationType.LocalName;
            }
        }
   }


}
   