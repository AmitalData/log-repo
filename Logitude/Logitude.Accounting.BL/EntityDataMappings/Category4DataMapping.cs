
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
   
   public partial class Category4DataMapping: IMapping<Category4PM, Category4>
   {

        public void CustomPMToPOCO(Category4PM entityPM, Category4 entityPOCO)
       {
           CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
           CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);

           if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
           {
               entityPOCO.Id = entityPM.Id;
               entityPOCO.Tenant = entityPM.Tenant;

           }
       }

        public void CustomPOCOToPM(Category4PM entityPM, Category4 entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   