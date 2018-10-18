
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
using Simplog.Server.Infrastructure;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class Category3DataMapping: IMapping<Category3PM, Category3>
   {

        public void CustomPMToPOCO(Category3PM entityPM, Category3 entityPOCO)
       {
           CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
           CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);

           if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
           {
               entityPOCO.Id = entityPM.Id;
               entityPOCO.Tenant = entityPM.Tenant;

           }
       }

        public void CustomPOCOToPM(Category3PM entityPM, Category3 entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   