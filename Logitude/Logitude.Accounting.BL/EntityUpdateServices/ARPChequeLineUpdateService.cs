using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Web;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.BL.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class ARPChequeLineUpdateService : EntityUpdateService<ARPChequeLine, ARPChequeLinePM, EntityPM>
    {
        protected override void OnCreating(ARPChequeLinePM entityPM, EntityPM entityParentPM)
        {
            if (entityPM.Id == null || entityPM.Id == "") entityPM.Id = IdCounter.GetNumber("ARPChequeLine", entityPM.Tenant);
            entityPM.SearchFields = entityPM.ChequeNumber + "," + entityPM.BankAccount;
            base.OnCreating(entityPM, entityParentPM);
        }

        protected override void OnUpdating(ARPChequeLinePM entityPM)
        {
            entityPM.SearchFields = entityPM.ChequeNumber + "," + entityPM.BankAccount;
            base.OnUpdating(entityPM);
        }

    }
}
