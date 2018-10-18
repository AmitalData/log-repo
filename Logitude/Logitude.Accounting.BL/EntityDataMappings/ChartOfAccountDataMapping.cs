
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
using Logitude.Accounting.Data;
using Logitude.Accounting.BL.EntityQueryServices;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class ChartOfAccountDataMapping: IMapping<ChartOfAccountPM, ChartOfAccount>
   {
       public void CustomPMToPOCO(ChartOfAccountPM entityPM, ChartOfAccount entityPOCO)
       {
           AddPOCOPropertyName(POCOPropertyNames.Id);
           AddPOCOPropertyName(POCOPropertyNames.Tenant);
           if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
           {
               entityPOCO.Id = entityPM.Id;
               entityPOCO.Tenant = entityPM.Tenant;
           }
       }

        public void CustomPOCOToPM(ChartOfAccountPM entityPM, ChartOfAccount entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Code);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.EnglishName);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Inactive);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.ParentId);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.TypeCode);

            if (entityPOCO.ParentId != null)
            {
                ChartOfAccountQueryService chartOfAccountQueryService = new ChartOfAccountQueryService(entityPOCO.Tenant);
                ChartOfAccountPM parent = chartOfAccountQueryService.GetSingle(entityPOCO.ParentId, false, false);
                entityPM.ParentName = parent.EnglishName;
            }

            if (entityPOCO.TypeCode != null)
            {
                ChartOfAccountsTypeQueryService chartOfAccountsTypeQueryService = new ChartOfAccountsTypeQueryService(entityPOCO.Tenant);
                ChartOfAccountsTypePM type = chartOfAccountsTypeQueryService.GetSingle(entityPOCO.TypeCode, false, true);
                entityPM.TypeName = type.EnglishName;
            }

        }

   }


}
   