
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
   
   public partial class AnalyticsFactsFieldsMetaDataDataMapping: IMapping<AnalyticsFactsFieldsMetaDataPM, AnalyticsFactsFieldsMetaData>
   {

        public void CustomPMToPOCO(AnalyticsFactsFieldsMetaDataPM entityPM, AnalyticsFactsFieldsMetaData entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(AnalyticsFactsFieldsMetaDataPM entityPM, AnalyticsFactsFieldsMetaData entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   