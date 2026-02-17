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
using Logitude.Customs.Data.Repsitories;

namespace Logitude.Customs.BL.EntityDataMappings
{

   public partial class DeclarationCourierStatusDataMapping : IMapping<DeclarationCourierStatusPM, DeclarationCourierStatus>
    {

        public void CustomPMToPOCO(DeclarationCourierStatusPM entityPM, DeclarationCourierStatus entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.DeclarationId);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            //CustomMappedPOCOProperties.Add(POCOPropertyNames.IsClosedForFollowUp);
            //CustomMappedPOCOProperties.Add(POCOPropertyNames.IsCourierMissingClassification);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.DeclarationId = entityPM.DeclarationId;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.IsClosedForFollowUp = entityPM.IsClosedForFollowUp;
                entityPOCO.IsCourierMissingClassification = entityPM.IsCourierMissingClassification;
            }
        }

        public void CustomPOCOToPM(DeclarationCourierStatusPM entityPM, DeclarationCourierStatus entityPOCO)
        {

            DeclarationQueryService declarationQueryService = new DeclarationQueryService(entityPM.Tenant);
            DeclarationPM declarationPM = declarationQueryService.GetSingle(entityPM.DeclarationId, false,false);
            if (declarationPM != null)
            {
                entityPM.CourierHawb = declarationPM.CourierHAWB;
                entityPM.ProcedureCurrentCode = declarationPM.ProcedureCurrentCode;
                entityPM.ImporterCode = declarationPM.ImporterCode;
            }
        }
    }


}
