
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class CurrencyTypeTenantDataMapping: IMapping<CurrencyTypeTenantPM, CurrencyTypeTenant>
   {

        public void CustomPMToPOCO(CurrencyTypeTenantPM entityPM, CurrencyTypeTenant entityPOCO)
        {
            
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                this.CustomMappedPOCOProperties.Add(POCOPropertyNames.UpdateDate);
                this.CustomMappedPOCOProperties.Add(POCOPropertyNames.UpdatedByUserId);
                this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
                this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
                this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Code);


                entityPOCO.UpdateDate = entityPM.UpdateDate;
                entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.Code = entityPM.Code;
            }

        }

        public void CustomPOCOToPM(CurrencyTypeTenantPM entityPM, CurrencyTypeTenant entityPOCO)
        {
            //throw new NotImplementedException();


            

        }
    }


}
   