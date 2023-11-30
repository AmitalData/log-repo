
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
using Logitude.DashboardModule.Data.Repositories;

namespace Logitude.DashboardModule.BL.EntityDataMappings
{
   
   public partial class AnalyticsFactsFieldsMetaDataDataMapping: IMapping<AnalyticsFactsFieldsMetaDataPM, AnalyticsFactsFieldsMetaData>
   {
        public void CustomPMToPOCO(AnalyticsFactsFieldsMetaDataPM entityPM, AnalyticsFactsFieldsMetaData entityPOCO)
        {
            BuildSearchFields(entityPM, entityPOCO);
        }

        public void CustomPOCOToPM(AnalyticsFactsFieldsMetaDataPM entityPM, AnalyticsFactsFieldsMetaData entityPOCO)
        {
            entityPM.ObjectTableName = new AnalyticsFactsMetaDataRepository(entityPM.Tenant).GetSingle(entityPM.AnalyticsFactsMetaDataId, entityPM.Tenant)?.ObjectTableName;
        }
        private void BuildSearchFields(AnalyticsFactsFieldsMetaDataPM entityPM, AnalyticsFactsFieldsMetaData entityPOCO)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.FieldCode);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.DisplayName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.DisplayNamePlural);

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }
}
   