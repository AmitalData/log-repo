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
    public partial class DecCargoSplitCargoIdentifierUpdateService : EntityUpdateService<DecCargoSplitCargoIdentifier, DecCargoSplitCargoIdentifierPM, DeclarationCargoSplitPM>
    {
        protected override void OnCreating(DecCargoSplitCargoIdentifierPM entityPM, DeclarationCargoSplitPM entityParentPM)
        {
            entityPM.DeclarationCargoSplitId = entityParentPM.Id;
            entityParentPM.DecCargoSplitCargoIdentifierLastLineNumber += 1;
            entityPM.LineNumber = entityParentPM.DecCargoSplitCargoIdentifierLastLineNumber;
        }

        protected override void AfterUpdating(DecCargoSplitCargoIdentifierPM entityPM, DeclarationCargoSplitPM entityParentPM)
        {
            
        }

        public void FastDeleteComposition(Logitude.Customs.Data.EntityKeys.DeclarationCargoSplitKeys entityKeyFields)
        {
            (Repository as Logitude.Customs.Data.Repsitories.DecCargoSplitCargoIdentifierRepository).FastDeleteMulti(entityKeyFields);
        }
    }
}
