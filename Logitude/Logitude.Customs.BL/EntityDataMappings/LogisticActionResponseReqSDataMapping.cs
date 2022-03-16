
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
   
   public partial class LogisticActionResponseReqSDataMapping: IMapping<LogisticActionResponseReqSPM, LogisticActionResponseReqS>
   {

        public void CustomPMToPOCO(LogisticActionResponseReqSPM entityPM, LogisticActionResponseReqS entityPOCO)
        {

        }

        public void CustomPOCOToPM(LogisticActionResponseReqSPM entityPM, LogisticActionResponseReqS entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   