
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.BL.EntityPMs; 
using Logitude.Accounting.Data;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class ReconcileCurrencyTypeDataMapping: IMapping<ReconcileCurrencyTypePM, ReconcileCurrencyType>
   {

        public void CustomPMToPOCO(ReconcileCurrencyTypePM entityPM, ReconcileCurrencyType entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(ReconcileCurrencyTypePM entityPM, ReconcileCurrencyType entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   