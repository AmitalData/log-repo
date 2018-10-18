using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
namespace Logitude.BL.ShipmentsModel.Tools.DataMapping
{
    public class FBLStockMapping
    {
        public static void MapEntity(FBLStockPM entityPM, FBLStock poco, bool isNewState)
        {
            poco.Number = entityPM.Number;
            
            poco.Tenant = entityPM.Tenant;
            poco.InsertionDate = entityPM.InsertionDate;
            poco.Notes = entityPM.Notes;
             
            poco.IsUsed = entityPM.IsUsed;

        }
    }
}
