using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.BL.Validators;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class AutomaticReconcileMethodUpdateService : EntityUpdateService<AutomaticReconcileMethod, AutomaticReconcileMethodPM, EntityPM>
    {
        protected override void OnCreating(AutomaticReconcileMethodPM entityPM, EntityPM entityParentPM)
        {
            if (entityPM.Id == null || entityPM.Id == "") entityPM.Id = IdCounter.GetNumber("AutomaticReconcileMethod", entityPM.Tenant);
        }

        protected override void OnUpdating(AutomaticReconcileMethodPM entityPM)
        {
        }

        protected override void Validate(AutomaticReconcileMethodPM entityPM)
        {
            ValidationResult result = AutomaticReconcileMethodValidator.IsAutomaticReconcileMethodValid(entityPM);
            if (result != null)
            {
                throw new ApplicationException(result.ErrorMessage);
            }
            base.Validate(entityPM);
        }

    }
}
