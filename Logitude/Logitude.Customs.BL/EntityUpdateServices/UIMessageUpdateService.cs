using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Server.Tools;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class UIMessageUpdateService : ICanUpdateClosedTable<UIMessagePM>
    {

        protected override void OnUpdating(UIMessagePM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            UIMessageAdditionalRepository additionalRep = new UIMessageAdditionalRepository(context);
            UIMessageAdditional additional = additionalRep.GetSingleAdditionalByCode(entityPM.Code, entityPM.Tenant); 
            if (additional != null)
            {
                additional.Sort = entityPM.Sort;
                additionalRep.Update(additional);
            }
            else
            {
                additional = new UIMessageAdditional();
                additional.Id = IdCounter.GetNumber("Customs.UIMessageAdditional", entityPM.Tenant);
                additional.Code = entityPM.Code;
                additional.Tenant = entityPM.Tenant;
                additional.Sort = entityPM.Sort;
                //additional.SearchFields = entityPM.Code + ',' + entityPM.EnglishName + ',' + entityPM.LocalName;
                additional.SearchFields = entityPM.Code + ',' + entityPM.Sort;
                additionalRep.Add(additional);
            }

        }
    }
}
