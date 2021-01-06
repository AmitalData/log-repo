using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    partial class ConsignmentInternalTransitionUpdateService : EntityUpdateService<ConsignmentInternalTransition, ConsignmentInternalTransitionPM, ConsignmentPM>
    {
        protected override void OnCreating(ConsignmentInternalTransitionPM entityPM, ConsignmentPM entityParentPM)
        {
            entityPM.DeclarationId = entityParentPM.DeclarationId;
            entityPM.ConsignmentNumber = entityParentPM.ConsignmentNumber;

            entityParentPM.ConsignmentInternalTransitionLastLineNumber += 1;
            entityPM.LineNumber = entityParentPM.ConsignmentInternalTransitionLastLineNumber;
            LogMessagingUtil.Instance.AppendLine("SiteCode at ConsignmentInternalTransitionUpdateService.OnCreating - " + entityPM.SiteCode);
        }


        public void FastDeleteComposition(Logitude.Customs.Data.EntityKeys.DeclarationKeys entityKeyFields)
        {
            (Repository as Logitude.Customs.Data.Repsitories.ConsignmentInternalTransitionRepository).FastDeleteMulti(entityKeyFields);
        }
    }
}
