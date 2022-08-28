
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
   
   public partial class DashboardDataMapping: IMapping<DashboardPM, Dashboard>
   {
        public void CustomPMToPOCO(DashboardPM entityPM, Dashboard entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            entityPOCO.Id = entityPM.Id;

            BuildSearchFields(entityPM, entityPOCO);
        }

        public void CustomPOCOToPM(DashboardPM entityPM, Dashboard entityPOCO)
        {

        }

        private void BuildSearchFields(DashboardPM entityPM, Dashboard entityPOCO)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Name);

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }

}
   