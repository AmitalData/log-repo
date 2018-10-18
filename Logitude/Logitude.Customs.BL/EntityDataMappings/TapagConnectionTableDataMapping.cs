
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
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class TapagConnectionTableDataMapping: IMapping<TapagConnectionTablePM, TapagConnectionTable>
   {

        public void CustomPMToPOCO(TapagConnectionTablePM entityPM, TapagConnectionTable entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.DeclarationId);
            AddPOCOPropertyName(POCOPropertyNames.TapagId);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.DeclarationId = entityPM.DeclarationId;
                entityPOCO.TapagId = entityPM.TapagId;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(TapagConnectionTablePM entityPM, TapagConnectionTable entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   