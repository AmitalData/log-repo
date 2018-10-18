using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.BL.EntityQueryServices;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.BL.Validators;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class JournalActionTypeUpdateService : EntityUpdateService<JournalActionType, JournalActionTypePM, EntityPM>
    {

        protected override void OnCreating(JournalActionTypePM entityPM, EntityPM entityParentPM)
        {
            if (entityPM.Id == null || entityPM.Id == "") entityPM.Id = IdCounter.GetNumber("JournalActionType", entityPM.Tenant);
            entityPM.SearchFields = entityPM.Code + "," + entityPM.EnglishName + "," + entityPM.LocalName;
        }

        protected override void OnUpdating(JournalActionTypePM entityPM)
        {
            entityPM.SearchFields = entityPM.Code + "," + entityPM.EnglishName + "," + entityPM.LocalName;
        }

        protected override void Validate(JournalActionTypePM entityPM)
        {
            ValidationResult result = JournalActionTypeValidator.IsJournalActionTypeValid(entityPM);
            if (result != null)
            {
                throw new ApplicationException(result.ErrorMessage);
            }
            base.Validate(entityPM);
        }
    }
}
