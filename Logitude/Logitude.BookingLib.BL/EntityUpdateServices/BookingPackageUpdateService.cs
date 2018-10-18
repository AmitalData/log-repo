using Logitude.BookingLib.BL.EntityPMs;
using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BookingLib.BL.EntityUpdateServices
{
    public partial class BookingPackageUpdateService
    {
        protected override void OnCreating(BookingPackagePM entityPM, BookingPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("BookingPackage", entityPM.Tenant);
            }

            entityPM.BookingId = entityParentPM.Id;
        }
    }
}
