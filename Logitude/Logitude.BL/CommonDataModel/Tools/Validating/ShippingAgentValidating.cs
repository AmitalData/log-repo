using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;

namespace Logitude.BL.CommonDataModel.Tools.Validating
{
    public class ShippingAgentValidating
    {
        public static void Validate(EntityPMs.ShippingAgentPM entityPM)
        {
            foreach (AddressPM itemPM in entityPM.Addresses)
            {
                AddressValidating.Validate(itemPM);
            }
        }
    }
}