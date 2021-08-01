using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Def.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amital.QuoteOPM.BL.EntityUpdateServices
{
    public partial class QuoteOPUpdateService : EntityUpdateService<QuoteOP, QuoteOPPM, EntityPM>
    {
        //C:\C21R01\Logitude\Logitude.BL\QuoteModel\Tools\EntityService\QuoteService.cs
        xxxx
        protected override void OnCreating(QuoteOPPM entityPM, EntityPM entityParentPM)
        {
            base.OnCreating(entityPM, entityParentPM);
        }
    }
}
