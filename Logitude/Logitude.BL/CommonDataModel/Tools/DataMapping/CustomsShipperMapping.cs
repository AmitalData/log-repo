


using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class CustomsShipperMapping
    {

        public static void MapEntity(CustomsShipperPM entityPM, CustomsShipper entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            
            }

            entityPOCO.ValidDepositionNumber = entityPM.ValidDepositionNumber;
            entityPOCO.CustomsShipperCode = entityPM.CustomsShipperCode;

            entityPOCO.FutureDepositionExist = entityPM.FutureDepositionExist;
            entityPOCO.ValidityStartDate = entityPM.ValidityStartDate;
            entityPOCO.ValidityEndDate = entityPM.ValidityEndDate;

        }

    }
}

