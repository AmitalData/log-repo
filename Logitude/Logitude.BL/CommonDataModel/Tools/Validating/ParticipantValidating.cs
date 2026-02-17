using Logitude.BL.CommonDataModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.Validating
{
    public class ParticipantValidating
    {
        public static void Validate(EntityPMs.ParticipantPM entityPM)
        {
            foreach (AddressPM itemPM in entityPM.Addresses)
            {
                AddressValidating.Validate(itemPM);
            }
        }
    }
}
