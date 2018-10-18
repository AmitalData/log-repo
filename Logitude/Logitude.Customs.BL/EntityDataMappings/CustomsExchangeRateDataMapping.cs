
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
using Logitude.Customs.BL.EntityQueryServices;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class CustomsExchangeRateDataMapping: IMapping<CustomsExchangeRatePM, CustomsExchangeRate>
   {

        public void CustomPMToPOCO(CustomsExchangeRatePM entityPM, CustomsExchangeRate entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            entityPOCO.Id = entityPM.Id;

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            entityPOCO.Tenant = entityPM.Tenant;
        }

        public void CustomPOCOToPM(CustomsExchangeRatePM entityPM, CustomsExchangeRate entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.CurrencyTypeName);


            if (entityPOCO.CurrencyTypeCode != null)
            {
                CurrencyTypeQueryService currencyTypeQueryService = new CurrencyTypeQueryService(entityPOCO.Tenant);
                CurrencyTypePM CurrencyType = currencyTypeQueryService.GetSingle(entityPOCO.CurrencyTypeCode, false, true);
                entityPM.CurrencyTypeName = CurrencyType.LocalName;
            }
        }
   }


}
   