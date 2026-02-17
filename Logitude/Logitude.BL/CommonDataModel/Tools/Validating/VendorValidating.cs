using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;

namespace Logitude.BL.CommonDataModel.Tools.Validating
{
    public class VendorValidating
    {
        public static void Validate(EntityPMs.VendorPM entityPM)
        {
            foreach (AddressPM itemPM in entityPM.Addresses)
            {
                AddressValidating.Validate(itemPM);
            }
        }
    }
}