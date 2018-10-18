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
    public partial class DecCargoSplitConUpdateService : EntityUpdateService<DecCargoSplitCon, DecCargoSplitConPM, DeclarationCargoSplitPM>
    {
        protected override void OnCreating(DecCargoSplitConPM entityPM, DeclarationCargoSplitPM entityParentPM)
        {
            entityPM.DeclarationCargoSplitId = entityParentPM.Id;

            //int line = 0;
            //if (entityParentPM.DecCargoSplitCons.Count > 0)
            //{
            //    line = entityParentPM.DecCargoSplitCons.Max(d => d.LineNumber);
            //}

            //entityPM.LineNumber = line + 1;

            entityParentPM.DecCargoSplitConLastLineNumber += 1;
            entityPM.LineNumber = entityParentPM.DecCargoSplitConLastLineNumber;
        }

        protected override void UpdateComposition(DecCargoSplitConPM entityPM)
        {
            DecCargoSplitConsItemUpdateService decCargoSplitConsItemUpdateServiceUpdateService = new DecCargoSplitConsItemUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            decCargoSplitConsItemUpdateServiceUpdateService.UpdateMulti(entityPM.DecCargoSplitConsItems, entityPM.DeletedDecCargoSplitConsItems, entityPM, false);
        }

        protected override void AfterUpdating(DecCargoSplitConPM entityPM, DeclarationCargoSplitPM entityParentPM)
        {
            
        }

        public void FastDeleteComposition(Logitude.Customs.Data.EntityKeys.DeclarationCargoSplitKeys entityKeyFields)
        {
            (Repository as Logitude.Customs.Data.Repsitories.DecCargoSplitConRepository).FastDeleteMulti(entityKeyFields);
        }
    }
}
