
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

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class Aur_ItemDataMapping: IMapping<Aur_ItemPM, Aur_Item>
   {

        public void CustomPMToPOCO(Aur_ItemPM entityPM, Aur_Item entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.PaymentId);
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.PaymentId = entityPM.PaymentId;
                entityPOCO.Line = entityPM.Line;
            }
        }

        public void CustomPOCOToPM(Aur_ItemPM entityPM, Aur_Item entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   