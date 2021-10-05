using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class ChargesGroupMapping
    {

        public static void MapEntity(ChargesGroupPM entityPM, ChargesGroup entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }

            entityPOCO.LocalName = entityPM.LocalName;
            entityPOCO.Name = entityPM.Name;
            entityPOCO.Code = entityPM.Code;
            entityPOCO.ViewOrder = entityPM.ViewOrder;
            entityPOCO.QuoteGroupSectionID = entityPM.QuoteGroupSectionID;
        }


    }
}
