
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
   
   public partial class BIReportFolderDataMapping: IMapping<BIReportFolderPM, BIReportFolder>
   {

        public void CustomPMToPOCO(BIReportFolderPM entityPM, BIReportFolder entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(BIReportFolderPM entityPM, BIReportFolder entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   