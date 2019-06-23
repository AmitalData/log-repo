
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityPMs; 
using Logitude.CRM.Data;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;

namespace Logitude.CRM.BL.EntityDataMappings
{
   
   public partial class OccasionTypeDataMapping: IMapping<OccasionTypePM, OccasionType>
   {

        public void CustomPMToPOCO(OccasionTypePM entityPM, OccasionType entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            entityPOCO.Id = entityPM.Id;

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
        }

        public void CustomPOCOToPM(OccasionTypePM entityPM, OccasionType entityPOCO)
        {
            //throw new NotImplementedException();
        }

        private void BuildSearchFields(OccasionTypePM entityPM, OccasionType entityPOCO, bool p)
        {
            string mySearchFields = "";

            if (!string.IsNullOrEmpty(entityPM.Code))
            {
                MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Code);
            }

            if (!string.IsNullOrEmpty(entityPM.Name))
            {
                MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Name);
            }

            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }


}
   