
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

    public partial class ReferantExceptionDataMapping : IMapping<ReferantExceptionPM, ReferantException>
    {

        public void CustomPMToPOCO(ReferantExceptionPM entityPM, ReferantException entityPOCO)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
                this.CustomMappedPOCOProperties.Add(POCOPropertyNames.DeclarationId);
                this.CustomMappedPOCOProperties.Add(POCOPropertyNames.ExceptionReasonsCode);
                entityPOCO.DeclarationId = entityPM.DeclarationId;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.ExceptionReasonsCode = entityPM.ExceptionReasonsCode;
            }


        }

        public void CustomPOCOToPM(ReferantExceptionPM entityPM, ReferantException entityPOCO)
        {
            //throw new NotImplementedException();
        }
    }


}
