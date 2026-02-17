
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
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class CustomsDocumentDataMapping: IMapping<CustomsDocumentPM, CustomsDocument>
   {

        public void CustomPMToPOCO(CustomsDocumentPM entityPM, CustomsDocument entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.DocumentsFilingId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {

                entityPOCO.DocumentsFilingId = entityPM.DocumentsFilingId;             
                entityPOCO.Tenant = entityPM.Tenant;



            }

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.CustomsDocId);
            if (!string.IsNullOrEmpty(entityPM.CustomsDocId))
            {
                entityPOCO.CustomsDocId = entityPM.CustomsDocId;
            }
            else if (entityPM.ForceRemoveCustomsDocId)//is null or empty !!!
            {
                entityPOCO.CustomsDocId = entityPM.CustomsDocId;
            }

        }

        public void CustomPOCOToPM(CustomsDocumentPM entityPM, CustomsDocument entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.DocumentStatusName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.DocumentTypeName);
            

            if (entityPOCO.DocumentStatusCode != null)
            {
                CustomsDocumentStatusTypeQueryService customsDocumentStatusTypeQueryService = new CustomsDocumentStatusTypeQueryService(entityPOCO.Tenant);
                CustomsDocumentStatusTypePM customsDocumentStatusType = customsDocumentStatusTypeQueryService.GetSingle(entityPOCO.DocumentStatusCode, false, true);
                entityPM.DocumentStatusName = customsDocumentStatusType.LocalName;
            }


            if (entityPOCO.DocumentTypeCode != null)
            {
                CustomDocumentTypeQueryService customDocumentTypeQueryService = new CustomDocumentTypeQueryService(entityPOCO.Tenant);
                CustomDocumentTypePM customDocumentType = customDocumentTypeQueryService.GetSingle(entityPOCO.DocumentTypeCode, false, true);
                entityPM.DocumentTypeName = customDocumentType.LocalName;
            }


            this.AddPMPropertyName(PMPropertyNames.Extension);
            this.AddPMPropertyName(PMPropertyNames.FileSize);
            this.AddPMPropertyName(PMPropertyNames.Name);
            if (entityPOCO.DocumentsFilingId != null)
            {
                DocumentsFilingRepository documentInRep = new DocumentsFilingRepository(entityPOCO.Tenant);
                DocumentsFiling documentsFiling = documentInRep.GetSingleDocumentsFiling(entityPOCO.DocumentsFilingId, entityPOCO.Tenant);
                if (documentsFiling != null)
                {
                    entityPM.CurrentEntityId = documentsFiling.EntityId;
                    if (documentsFiling.Document != null)
                    {
                        entityPM.DocumentId = documentsFiling.Document.Id;
                        entityPM.DocumentsFilingCode = documentsFiling.Code;
                        entityPM.Extension = documentsFiling.Document.Extension;
                        entityPM.FileSize =    documentsFiling.Document.FileSize;
                        entityPM.IsDigitallySigned = documentsFiling.IsDigitallySigned;
                        entityPM.SignersList = documentsFiling.SignersList;
                    }
                    if (documentsFiling.DocumentType != null)
                    {
                        entityPM.Name = documentsFiling.DocumentType.Name;
                    }


                }


            }

           

        }
   }


}
   