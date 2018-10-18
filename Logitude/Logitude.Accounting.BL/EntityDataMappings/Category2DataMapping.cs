
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
   
   public partial class Category2DataMapping: IMapping<Category2PM, Category2>
   {

        public void CustomPMToPOCO(Category2PM entityPM, Category2 entityPOCO)
       {
           CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
           CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);

           if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
           {
               entityPOCO.Id = entityPM.Id;
               entityPOCO.Tenant = entityPM.Tenant;

           }
       }

        public void CustomPOCOToPM(Category2PM entityPM, Category2 entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   