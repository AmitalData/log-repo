




using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class CustomerDepositionMapping
    {

        public static void MapEntity(CustomerDepositionPM entityPM, CustomerDeposition entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;

            }
            entityPOCO.CustomsShipperId = entityPM.CustomsShipperId;
            entityPOCO.DepositionNumber = entityPM.DepositionNumber;
            entityPOCO.ValidityStartDate = entityPM.ValidityStartDate;
            entityPOCO.ValidityEndDate = entityPM.ValidityEndDate;
            entityPOCO.CreateDate = entityPM.CreateDate;
            

        }

    }
}

