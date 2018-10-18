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
    public partial class DecCargoSplitConsPackDetUpdateService : EntityUpdateService<DecCargoSplitConsPackDet, DecCargoSplitConsPackDetPM, DecCargoSplitConsItemPM>
    {
        protected override void OnCreating(DecCargoSplitConsPackDetPM entityPM, DecCargoSplitConsItemPM entityParentPM)
        {
            entityPM.DeclarationCargoSplitId = entityParentPM.DeclarationCargoSplitId;
            entityPM.DecCargoSplitConsLineNo = entityParentPM.DecCargoSplitConsLineNo;
            entityPM.DecCargoSplitConsItemLine = entityParentPM.ItemLine;
            entityParentPM.DecCargoSplitConsPackDetLastLineNumber += 1;
            entityPM.PackageLine = entityParentPM.DecCargoSplitConsPackDetLastLineNumber;
        }

        protected override void AfterUpdating(DecCargoSplitConsPackDetPM entityPM, DecCargoSplitConsItemPM entityParentPM)
        {
            
        }

        public void FastDeleteComposition(Logitude.Customs.Data.EntityKeys.DecCargoSplitConsItemKeys entityKeyFields)
        {
            (Repository as Logitude.Customs.Data.Repsitories.DecCargoSplitConsPackDetRepository).FastDeleteMulti(entityKeyFields);
        }
    }
}
