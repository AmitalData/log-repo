
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
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.EntityQueryServices;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class CustomsDocumentPointerDataMapping: IMapping<CustomsDocumentPointerPM, CustomsDocumentPointer>
   {

        public void CustomPMToPOCO(CustomsDocumentPointerPM entityPM, CustomsDocumentPointer entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(CustomsDocumentPointerPM entityPM, CustomsDocumentPointer entityPOCO)
        {
            //this.CustomMappedPMProperties.Add(PMPropertyNames.DocumentTypeName);

            //if (entityPOCO.DocumentTypeCode != null)
            //{
            //    CustomDocumentTypeQueryService customDocumentTypeQueryService = new CustomDocumentTypeQueryService(entityPOCO.Tenant);
            //    CustomDocumentTypePM customDocumentType = customDocumentTypeQueryService.GetSingle(entityPOCO.DocumentTypeCode, false, false);
            //    entityPM.DocumentTypeName = customDocumentType.LocalName;
            //}
        }
   }


}
   