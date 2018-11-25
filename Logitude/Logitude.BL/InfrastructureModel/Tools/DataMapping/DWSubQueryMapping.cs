using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
   public class DWSubQueryMapping
    {
        public static void MapEntity(DWSubQueryPM entityPM, DWSubQuery entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }

            entityPOCO.DWFactTableCode = entityPM.DWFactTableCode;
            entityPOCO.DWQueryId = entityPM.DWQueryId;
            entityPOCO.FiltersXML = entityPM.FiltersXML;
            entityPOCO.ColumnsXML = entityPM.ColumnsXML;
            entityPOCO.SQLString = entityPM.SQLString;
            //entityPOCO.DWObjectTableCode = entityPM.DWObjectTableCode;


        }
    }
}
