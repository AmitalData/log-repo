
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

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class ContinuousRequestTypeDataMapping: IMapping<ContinuousRequestTypePM, ContinuousRequestType>
   {

        public void CustomPMToPOCO(ContinuousRequestTypePM entityPM, ContinuousRequestType entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(ContinuousRequestTypePM entityPM, ContinuousRequestType entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   