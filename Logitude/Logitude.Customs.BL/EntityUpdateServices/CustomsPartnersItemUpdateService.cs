using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
  public partial  class CustomsPartnersItemUpdateService
    {

      protected override void OnCreating(CustomsPartnersItemPM entityPM, EntityPM entityParentPM)
      {
          entityPM.Id = IdCounter.GetNumber("Customs.CustomsPartnersItem", entityPM.Tenant);
      }


    }
}
