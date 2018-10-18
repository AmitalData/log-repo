using Logitude.CRM.BL.EntityPMs;
using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.EntityUpdateServices
{
    public partial class EmployeeGroupLineUpdateService
    {
        protected override void OnCreating(EmployeeGroupLinePM entityPM, EmployeeGroupPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("EmployeeGroupLine", entityPM.Tenant);
            }

            entityPM.EmployeeGroupId = entityParentPM.Id;
        }
    }
}
