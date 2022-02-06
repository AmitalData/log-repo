using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class HTSCodeMapping
    {
        public static void MapEntity(HTSCodePM entityPM, HTSCode poco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                poco.Id = entityPM.Id;
                poco.Tenant = entityPM.Tenant;
            }

            poco.ItemId = entityPM.ItemId;
            poco.DestinationCountryId = entityPM.DestinationCountryId;
            poco.Code = entityPM.Code;
            poco.ApprovedByCustomer = entityPM.ApprovedByCustomer;
            poco.InActive = entityPM.InActive;
            poco.LineNumber = entityPM.LineNumber;
            poco.VATPercentage = entityPM.VATPercentage;
            poco.DutiesPercentage = entityPM.DutiesPercentage;
            poco.OtherDuties = entityPM.OtherDuties;
            poco.Remarks = entityPM.Remarks;
        }
    }
}
