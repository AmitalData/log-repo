
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.BL.EntityPMs; 
using Logitude.Infrastructure.Data;

namespace Logitude.Infrastructure.BL.EntityDataMappings
{
   
   public partial class DashboardDataMapping: IMapping<DashboardPM, Dashboard>
   {

        public void CustomPMToPOCO(DashboardPM entityPM, Dashboard entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(DashboardPM entityPM, Dashboard entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   