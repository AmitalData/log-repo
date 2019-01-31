using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class DeclarationMamanSpecialActionUpdateService : EntityUpdateService<DeclarationMamanSpecialAction, DeclarationMamanSpecialActionPM, EntityPM>
    {
        protected override void OnCreating(DeclarationMamanSpecialActionPM entityPM, EntityPM entityParentPM)
        {
            base.OnCreating(entityPM, entityParentPM);
        }
    }
}
