
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
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.TariffModule.BL.EntityDataMappings
{
   
   public partial class TariffLineDataMapping: IMapping<TariffLinePM, TariffLine>
   {

        public void CustomPMToPOCO(TariffLinePM entityPM, TariffLine entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.TariffId);

            entityPOCO.Id = entityPM.Id;
            entityPOCO.Tenant = entityPM.Tenant;
            entityPOCO.TariffId = entityPM.TariffId;
        }

        public void CustomPOCOToPM(TariffLinePM entityPM, TariffLine entityPOCO)
        {
            PortRepository portRepository = new PortRepository(entityPOCO.Tenant);
            CurrencyRepository currencyRepository = new CurrencyRepository(entityPOCO.Tenant);

            Port fromPort = portRepository.GetSinglePort(entityPOCO.OriginPortId, entityPOCO.Tenant);
            Port toPort = portRepository.GetSinglePort(entityPOCO.DestinationPortId, entityPOCO.Tenant);

            if (fromPort != null)
            {
                entityPM.OriginPortCode = fromPort.Code;
                entityPM.OriginPortCombinedCode = fromPort.CombinedCode;
                entityPM.OriginPortName = fromPort.EnglishName;
            }
            if (toPort != null)
            {
                entityPM.DestinationPortCode = toPort.Code;
                entityPM.DestinationPortCombinedCode = toPort.CombinedCode;
                entityPM.DestinationPortName = toPort.EnglishName;
            }
            if (!string.IsNullOrEmpty(entityPOCO.CurrencyId))
            {
                Currency currency = currencyRepository.GetSingleCurrency(entityPOCO.CurrencyId, entityPOCO.Tenant);
                if (currency != null)
                {
                    entityPM.CurrencyCode = currency.Code;
                }
            }
            
        }
    }


}
   