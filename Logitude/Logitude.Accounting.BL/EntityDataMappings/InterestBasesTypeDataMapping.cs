
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
using Logitude.Server.Tools.Helpers;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class InterestBasesTypeDataMapping: IMapping<InterestBasesTypePM, InterestBasesType>
   {

        public void CustomPMToPOCO(InterestBasesTypePM entityPM, InterestBasesType entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            entityPOCO.Id = entityPM.Id;
            CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO);
        }

        public void CustomPOCOToPM(InterestBasesTypePM entityPM, InterestBasesType entityPOCO)
        {

        }

        private void BuildSearchFields(InterestBasesTypePM entityPM, InterestBasesType entityPOCO)
        {
            string mySearchFields = "";
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.LocalName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.EnglishName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Code);
            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }


}
   