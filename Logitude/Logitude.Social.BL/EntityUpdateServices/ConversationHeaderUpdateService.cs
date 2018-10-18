using Logitude.Server.Tools.Counters;
using Logitude.Social.BL.EntityPMs;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Social.BL.EntityUpdateServices
{
  public partial  class ConversationHeaderUpdateService
    {


      protected override void OnCreating(ConversationHeaderPM entityPM, Server.Tools.EntityPM entityParentPM)
      {
          if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
          {
              entityPM.Id = IdCounter.GetNumber("ConversationHeader", entityPM.Tenant);
              entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
       
          }
      }
    }
}
