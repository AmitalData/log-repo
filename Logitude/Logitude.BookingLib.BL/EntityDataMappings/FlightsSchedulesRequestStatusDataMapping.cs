
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.BL.EntityPMs; 
using Logitude.BookingLib.Data;

namespace Logitude.BookingLib.BL.EntityDataMappings
{
   
   public partial class FlightsSchedulesRequestStatusDataMapping: IMapping<FlightsSchedulesRequestStatusPM, FlightsSchedulesRequestStatus>
   {

        public void CustomPMToPOCO(FlightsSchedulesRequestStatusPM entityPM, FlightsSchedulesRequestStatus entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(FlightsSchedulesRequestStatusPM entityPM, FlightsSchedulesRequestStatus entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   