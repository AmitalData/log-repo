using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.BookingLib.BL.EntityPMs;
using Logitude.Server.Tools.Counters;

namespace Logitude.BookingLib.BL.EntityUpdateServices
{
    public partial class BookingAnswerUpdateService
    {
        protected override void OnCreating(BookingAnswerPM entityPM, BookingPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("BookingAnswer", entityPM.Tenant);
            }

            entityPM.BookingId = entityParentPM.Id;
        }
    }
}
