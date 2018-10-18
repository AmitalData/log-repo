
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
   
   public partial class RequiredGuaranteeTypeDataMapping: IMapping<RequiredGuaranteeTypePM, RequiredGuaranteeType>
   {

        public void CustomPMToPOCO(RequiredGuaranteeTypePM entityPM, RequiredGuaranteeType entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.GuaranteeId = entityPM.GuaranteeId;
            }
        }

        public void CustomPOCOToPM(RequiredGuaranteeTypePM entityPM, RequiredGuaranteeType entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.GuaranteeTypeName);

            if (entityPOCO.GuaranteeTypeCode != null)
            {
                GuaranteeCertificateTypeQueryService entityQuery = new GuaranteeCertificateTypeQueryService(entityPM.Tenant);
                GuaranteeCertificateTypePM entity = entityQuery.GetSingle(entityPOCO.GuaranteeTypeCode, false, false);
                if (entity != null)
                    entityPM.GuaranteeTypeName = entity.LocalName;
            }
        }
   }


}
   