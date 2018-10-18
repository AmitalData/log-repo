using Logitude.Accounting.BL.EntityPMs;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class ReconcileCurrencyTypeUpdateService : EntityUpdateService<ReconcileCurrencyType, ReconcileCurrencyTypePM, EntityPM>
    {
        protected override void OnCreating(ReconcileCurrencyTypePM entityPM, EntityPM entityParentPM)
        {
            // entityPM.Id = IdCounter.GetNumber("Accounting.ReconcileCurrencyType", entityPM.Tenant);
        }
    }
}
