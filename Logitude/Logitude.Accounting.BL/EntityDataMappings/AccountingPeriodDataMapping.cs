using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Accounting.Data;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class AccountingPeriodDataMapping: IMapping<AccountingPeriodPM, AccountingPeriod>
   {

        public void CustomPMToPOCO(AccountingPeriodPM entityPM, AccountingPeriod entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(AccountingPeriodPM entityPM, AccountingPeriod entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.PeriodTypeName);


            if (entityPOCO.PeriodTypeCode != null)
            {
                PeriodTypeQueryService periodTypeQueryService = new PeriodTypeQueryService(entityPOCO.Tenant);
                PeriodTypePM periodType = periodTypeQueryService.GetSingle(entityPOCO.PeriodTypeCode, false, true);
                entityPM.PeriodTypeName = periodType.EnglishName;
            }

        }
   }


}
   