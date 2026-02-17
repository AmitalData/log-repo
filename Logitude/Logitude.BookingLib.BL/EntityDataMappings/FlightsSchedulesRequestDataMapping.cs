
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
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.BookingLib.BL.EntityDataMappings
{
   
   public partial class FlightsSchedulesRequestDataMapping: IMapping<FlightsSchedulesRequestPM, FlightsSchedulesRequest>
   {
        public void CustomPMToPOCO(FlightsSchedulesRequestPM entityPM, FlightsSchedulesRequest entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(FlightsSchedulesRequestPM entityPM, FlightsSchedulesRequest entityPOCO)
        {
            if (!string.IsNullOrEmpty(entityPOCO.AirlineId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPOCO.AirlineId, entityPOCO.Tenant, true);
                if (myCard != null)
                {
                    entityPM.AirlineCode = myCard.Code;
                    entityPM.AirlineName = myCard.EnglishName;
                }
            }
        }
   }
}
   