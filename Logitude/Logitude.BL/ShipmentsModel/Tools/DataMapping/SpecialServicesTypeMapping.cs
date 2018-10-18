using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.ShipmentsModel.EntityPMs;

namespace Logitude.BL.ShipmentsModel.Tools.DataMapping
{
    public class SpecialServicesTypeMapping
    {
        public static void MapEntity(SpecialServicesTypePM itemPM, SpecialServicesType itemPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.Id = itemPM.Id;
            }

            itemPoco.LocalName = itemPM.LocalName;
            itemPoco.EnglishName = itemPM.EnglishName;
            itemPoco.Code = itemPM.Code;
            itemPoco.InActive = itemPM.InActive;
            itemPoco.SearchFields = itemPM.Code + "," + itemPM.EnglishName + "," + itemPM.LocalName;
        }
    }
}