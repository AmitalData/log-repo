
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
using Logitude.Server.Tools.Helpers;

namespace Logitude.DashboardModule.BL.EntityDataMappings
{
   
   public partial class AnalyticsFactsMetaDataDataMapping: IMapping<AnalyticsFactsMetaDataPM, AnalyticsFactsMetaData>
   {
        public void CustomPMToPOCO(AnalyticsFactsMetaDataPM entityPM, AnalyticsFactsMetaData entityPOCO)
        {
            BuildSearchFields(entityPM, entityPOCO);
        }

        public void CustomPOCOToPM(AnalyticsFactsMetaDataPM entityPM, AnalyticsFactsMetaData entityPOCO)
        {
            //throw new NotImplementedException();
        }

        private void BuildSearchFields(AnalyticsFactsMetaDataPM entityPM, AnalyticsFactsMetaData entityPOCO)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Name);

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }
}
   