
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
   
   public partial class DeficitConnFileParagraphTypeDataMapping: IMapping<DeficitConnFileParagraphTypePM, DeficitConnFileParagraphType>
   {

        public void CustomPMToPOCO(DeficitConnFileParagraphTypePM entityPM, DeficitConnFileParagraphType entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.DeclarationId);
            AddPOCOPropertyName(POCOPropertyNames.DeficitId);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            AddPOCOPropertyName(POCOPropertyNames.ParagraphTypeCode);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.DeclarationId = entityPM.DeclarationId;
                entityPOCO.DeficitId = entityPM.DeficitId;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.ParagraphTypeCode = entityPM.ParagraphTypeCode;
            }
        }

        public void CustomPOCOToPM(DeficitConnFileParagraphTypePM entityPM, DeficitConnFileParagraphType entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.ParagraphTypeName);
            if (entityPOCO.ParagraphTypeCode != null)
            {
                ParagraphTypeQueryService paragraphTypeQueryService = new ParagraphTypeQueryService(entityPOCO.Tenant);
                ParagraphTypePM paragraphType = paragraphTypeQueryService.GetSingle(entityPOCO.ParagraphTypeCode, false, true);
                entityPM.ParagraphTypeName = paragraphType.LocalName;
            }
        }
   }


}
   