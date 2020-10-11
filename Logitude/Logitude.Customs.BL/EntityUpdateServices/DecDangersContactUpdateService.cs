using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
using Simplog.Server.Infrastructure;
using Logitude.Customs.Data.EntityKeys;
using System.ComponentModel;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class DecDangersContactUpdateService : EntityUpdateService<DecDangersContact, DecDangersContactPM, DeclarationPM >
    {
        protected override void OnCreating(DecDangersContactPM entityPM, DeclarationPM entityParentPM)
        {
            entityPM.DeclarationId = entityParentPM.Id;
  
         }
 

    }
}
