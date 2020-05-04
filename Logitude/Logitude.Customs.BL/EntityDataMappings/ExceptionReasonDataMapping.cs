
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

    public partial class ExceptionReasonDataMapping : IMapping<ExceptionReasonPM, ExceptionReason>
    {

        public void CustomPMToPOCO(ExceptionReasonPM entityPM, ExceptionReason entityPOCO)
        {

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Code);
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.Code = entityPM.Code;
            }
            entityPM.SearchFields = entityPM.EnglishName + "," + entityPM.LocalName + "," + entityPM.Code;

        }

        public void CustomPOCOToPM(ExceptionReasonPM entityPM, ExceptionReason entityPOCO)
        {

        }
    }


}
