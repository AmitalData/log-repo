
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.BL.EntityPMs; 
using Logitude.DashboardModule.Data;

namespace Logitude.DashboardModule.BL.EntityDataMappings
{
   
   public partial class AnalyticsFactsMetaDataDataMapping: IMapping<AnalyticsFactsMetaDataPM, AnalyticsFactsMetaData>
   {

        public void CustomPMToPOCO(AnalyticsFactsMetaDataPM entityPM, AnalyticsFactsMetaData entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(AnalyticsFactsMetaDataPM entityPM, AnalyticsFactsMetaData entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   