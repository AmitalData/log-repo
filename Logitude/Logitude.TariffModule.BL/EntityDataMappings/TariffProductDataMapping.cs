
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.BL.EntityPMs; 
using Logitude.TariffModule.Data;
using Logitude.Server.Tools.Helpers;

namespace Logitude.TariffModule.BL.EntityDataMappings
{
   
   public partial class TariffProductDataMapping: IMapping<TariffProductPM, TariffProduct>
   {

        public void CustomPMToPOCO(TariffProductPM entityPM, TariffProduct entityPOCO)
        {
            entityPOCO.Id = entityPM.Id;
            BuildSearchFields(entityPM, entityPOCO);
        }

        public void CustomPOCOToPM(TariffProductPM entityPM, TariffProduct entityPOCO)
        {
            //throw new NotImplementedException();
        }

        private void BuildSearchFields(TariffProductPM entityPM, TariffProduct entityPOCO)
        {
            string mySearchFields = "";
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Name);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Code);
            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }


}
   