using Logitude.BookingLib.BL.EntityPMs;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BookingLib.BL.EntityUpdateServices
{
    public partial class BookingProductUpdateService
    {
        protected override void OnCreating(BookingProductPM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("BookingProduct", 0);

                if (!string.IsNullOrEmpty(entityPM.AirlineId))
                {
                    AirlineRepository rep = new AirlineRepository(0);
                    Airline airline = rep.GetSingleAirline(entityPM.AirlineId, 0);
                    if (airline != null)
                    {
                        airline.HasAdaptations = true;

                        rep.Update(airline);
                        rep.SubmitChanges();
                    }
                }
            }
        }
    }
}
