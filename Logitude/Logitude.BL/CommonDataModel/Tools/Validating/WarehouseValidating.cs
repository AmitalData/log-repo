using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;

namespace Logitude.BL.CommonDataModel.Tools.Validating
{
    public class WarehouseValidating
    {
        public static void Validate(EntityPMs.WarehousePM entityPM)
        {
            foreach (AddressPM itemPM in entityPM.Addresses)
            {
                AddressValidating.Validate(itemPM);
            }
        }

    }
}