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
    public partial class DecCargoSplitConsItemUpdateService : EntityUpdateService<DecCargoSplitConsItem, DecCargoSplitConsItemPM, DecCargoSplitConPM>
    {
        protected override void OnCreating(DecCargoSplitConsItemPM entityPM, DecCargoSplitConPM entityParentPM)
        {
            entityPM.DeclarationCargoSplitId = entityParentPM.DeclarationCargoSplitId;
            entityPM.DecCargoSplitConsLineNo = entityParentPM.LineNumber;
            entityParentPM.DecCargoSplitConsItemLastLineNumber += 1;
            entityPM.ItemLine = entityParentPM.DecCargoSplitConsItemLastLineNumber;
        }

        protected override void UpdateComposition(DecCargoSplitConsItemPM entityPM)
        {
            DecCargoSplitConsPackDetUpdateService decCargoSplitConsPackDetUpdateServiceUpdateService = new DecCargoSplitConsPackDetUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            decCargoSplitConsPackDetUpdateServiceUpdateService.UpdateMulti(entityPM.DecCargoSplitConsPackDets, entityPM.DeletedDecCargoSplitConsPackDets, entityPM, false);
        }

        protected override void AfterUpdating(DecCargoSplitConsItemPM entityPM, DecCargoSplitConPM entityParentPM)
        {
            
        }

        public void FastDeleteComposition(Logitude.Customs.Data.EntityKeys.DecCargoSplitConKeys entityKeyFields)
        {
            (Repository as Logitude.Customs.Data.Repsitories.DecCargoSplitConsItemRepository).FastDeleteMulti(entityKeyFields);
        }
    }
}
