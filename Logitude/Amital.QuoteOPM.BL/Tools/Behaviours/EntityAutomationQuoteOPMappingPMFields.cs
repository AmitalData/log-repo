using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.BL.ExternalService;

using Logitude.BL.QuoteModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;


namespace Amital.QuoteOPM.BL.Tools.Behaviours
{
    class EntityAutomationQuoteOPMappingPMFields : IEntityAutomationMappingPMFields
    {
        public void Map<T1, T2>(T1 entityPM, T2 oldEntityPM)
        {
             
            QuotePM quoteOldEntityPM = oldEntityPM as QuotePM; 

            PortRepository quoteRepository = new PortRepository(quoteOldEntityPM.Tenant);
            Port fromPort = quoteRepository.GetSinglePort(quoteOldEntityPM.FromPortId, quoteOldEntityPM.Tenant);
             
            quoteOldEntityPM.FromPortCountry = fromPort != null ? fromPort.CountryName : null;
             
            Port toPort = quoteRepository.GetSinglePort(quoteOldEntityPM.ToPortId, quoteOldEntityPM.Tenant);
             
            quoteOldEntityPM.ToPortCountry = toPort != null ? toPort.CountryName : null; 

        }
    }
}
