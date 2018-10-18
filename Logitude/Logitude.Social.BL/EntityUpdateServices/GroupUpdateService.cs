using Logitude.Server.Tools.Counters;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Social.BL.EntityUpdateServices
{
   public partial class  GroupUpdateService
    {
       protected override void OnCreating(EntityPMs.GroupPM entityPM, Server.Tools.EntityPM entityParentPM)
       {

           if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
           {
               entityPM.Id = IdCounter.GetNumber("Group", entityPM.Tenant);
               entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
           }

       }


    }
}
