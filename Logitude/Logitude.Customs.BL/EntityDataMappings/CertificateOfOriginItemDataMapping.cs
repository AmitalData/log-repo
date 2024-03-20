
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
   
   public partial class CertificateOfOriginItemDataMapping: IMapping<CertificateOfOriginItemPM, CertificateOfOriginItem>
   {

        public void CustomPMToPOCO(CertificateOfOriginItemPM entityPM, CertificateOfOriginItem entityPOCO)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Id))
                {
                    entityPOCO.Id = entityPM.Id;

                }
            }

        }

        public void CustomPOCOToPM(CertificateOfOriginItemPM entityPM, CertificateOfOriginItem entityPOCO)
        {

            var originCriterionQueryService = new OriginCriterionQueryService(entityPOCO.Tenant);
            var originCriterion = originCriterionQueryService.GetSingle(entityPOCO.OriginCriterionCode, false, true);
            if (originCriterion != null)
            {
                entityPM.OriginCriterionCodeName = originCriterion.OriginCriterionCode;
            }
        }
   }


}
   