
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

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class OriginCriterionDataMapping: IMapping<OriginCriterionPM, OriginCriterion>
   {

        public void CustomPMToPOCO(OriginCriterionPM entityPM, OriginCriterion entityPOCO)
        {

            CustomMappedPOCOProperties.Add(POCOPropertyNames.Code);
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.Code = entityPM.Code;
                entityPOCO.EnglishName = entityPM.EnglishName;
                entityPOCO.LocalName = entityPM.LocalName;
                entityPOCO.Inactive = entityPM.Inactive;
                entityPOCO.SearchFields = entityPM.SearchFields;
                entityPOCO.CertificateOfOriginTypeCodeID = entityPM.CertificateOfOriginTypeCodeID;
                entityPOCO.OriginCriterionCode = entityPM.OriginCriterionCode;

            }
                


        }

        public void CustomPOCOToPM(OriginCriterionPM entityPM, OriginCriterion entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   