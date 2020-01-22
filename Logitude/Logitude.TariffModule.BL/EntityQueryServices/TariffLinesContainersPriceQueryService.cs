using Logitude.TariffModule.BL.EntityPMs;
using Logitude.TariffModule.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TariffModule.BL.EntityQueryServices
{
    public partial class TariffLinesContainersPriceQueryService
    {
        public List<TariffLinesContainersPricePM> GetContainerPricesByVersionAndPorts(int version, string fromPortId, string toPortId, int tenant)
        {
            TariffLine tariffLine = (from a in context.TariffLines
                                     where a.Version == version && a.Tenant == tenant && a.OriginPortId == fromPortId && a.DestinationPortId == toPortId
                                     select a).FirstOrDefault();

            List<TariffLinesContainersPricePM> tariffLinesContainersPricePMs = new List<TariffLinesContainersPricePM>();

            if (tariffLine != null)
            {
                List<TariffLinesContainersPrice> containerPrices = (from a in context.TariffLinesContainersPrices
                                                                    where a.TariffLineId == tariffLine .Id && a.Tenant == tenant
                                                                    select a).ToList();
                                
                foreach (TariffLinesContainersPrice entityPOCO in containerPrices)
                {
                    TariffLinesContainersPricePM entityPM = new TariffLinesContainersPricePM();
                    mapping.CustomPOCOToPM(entityPM, entityPOCO);
                    mapping.POCOToPM(entityPM, entityPOCO);
                    tariffLinesContainersPricePMs.Add(entityPM);
                }
            }

            return tariffLinesContainersPricePMs;
        }
    }
}
