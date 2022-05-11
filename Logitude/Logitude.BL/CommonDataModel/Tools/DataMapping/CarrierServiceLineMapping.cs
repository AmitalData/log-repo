using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class CarrierServiceLineMapping
    {
        public static void MapEntity(CarrierServiceLinePM entityPM, CarrierServiceLine poco, bool isNewEntity)
        {
            poco.Id = entityPM.Id;
            poco.Tenant = entityPM.Tenant;
            poco.CardId = entityPM.CardId;
            poco.PartnerTypeId = entityPM.PartnerTypeId;
            poco.Name = entityPM.Name;
            poco.Description = entityPM.Description;
            poco.SearchFields = entityPM.Name;            
            poco.Inactive = entityPM.Inactive;
        }
    }
}
