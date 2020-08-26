using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.TraceEvents
{
    public class HorseTracing
    {
        public static void Trace(HorsePM entityPM, Horse entityPOCO, bool isNewEntity, string loggedContactId)
        {
            if (isNewEntity)
            {
                // CREV
            }

            else
            {
                //UPEV

                //inactive: HRIN
                //active: HRRC
            }
        }
    }
}
