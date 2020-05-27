
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
   
   public partial class InterestReportLineDataMapping: IMapping<InterestReportLinePM, InterestReportLine>
   {

        public void CustomPMToPOCO(InterestReportLinePM entityPM, InterestReportLine entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.InterestTransactionId);
            AddPOCOPropertyName(POCOPropertyNames.InterestReportId);
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.InterestReportId = entityPM.InterestReportId;
                entityPOCO.InterestTransactionId = entityPM.InterestTransactionId;
            }
           
        }

        public void CustomPOCOToPM(InterestReportLinePM entityPM, InterestReportLine entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   