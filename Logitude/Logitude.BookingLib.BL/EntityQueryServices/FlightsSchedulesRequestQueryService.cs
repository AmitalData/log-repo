using Logitude.BookingLib.BL.EntityPMs;
using Logitude.BookingLib.Data;
using Logitude.BookingLib.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BookingLib.BL.EntityQueryServices
{
    public partial class FlightsSchedulesRequestQueryService
    {
        public override void GetComposition(EntityKeyFields entityKeys, FlightsSchedulesRequestPM entityPM)
        {
            IBookingContext context = MainContext as IBookingContext;
            FlightsSchedulesRequestKeys requestKeys = entityKeys as FlightsSchedulesRequestKeys;

            FlightsSchedulesResponseQueryService queryService = new FlightsSchedulesResponseQueryService(context);
            entityPM.Responses = queryService.GetMulti(requestKeys, true);
        }
    }
}
