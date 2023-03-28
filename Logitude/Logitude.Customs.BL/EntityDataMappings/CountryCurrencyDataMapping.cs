
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
   
   public partial class CountryCurrencyDataMapping: IMapping<CountryCurrencyPM, CountryCurrency>
   {

        public void CustomPMToPOCO(CountryCurrencyPM entityPM, CountryCurrency entityPOCO)
        {
            if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CountryId))
            {
                entityPOCO.CountryId = entityPM.CountryId;
            }

            if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Currency))
            {
                entityPOCO.Currency = entityPM.Currency;
            }
        }

        public void CustomPOCOToPM(CountryCurrencyPM entityPM, CountryCurrency entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   