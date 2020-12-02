using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.TariffModule.BL.EntityPMs;
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.Data.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TariffModule.BL.EntityUpdateServices
{
    public partial class TariffLineUpdateService
    {
        protected override void OnCreating(TariffLinePM entityPM, TariffVersionPM entityParentPM)
        {
            entityPM.TariffId = entityParentPM.TariffId;

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("TariffLine", entityPM.Tenant);                
            }
        }

        protected override void OnUpdating(TariffLinePM entityPM, TariffLine entityPOCO)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                if ((string.IsNullOrEmpty(entityPOCO.OriginPortId) && !string.IsNullOrEmpty(entityPM.OriginPortId))
                    ||
                    (string.IsNullOrEmpty(entityPOCO.DestinationPortId) && !string.IsNullOrEmpty(entityPM.DestinationPortId)))
                {
                    TariffRepository tariffRepository = new TariffRepository(entityPM.Tenant);
                    Tariff tariff = tariffRepository.GetSingle(entityPM.TariffId, entityPM.Tenant);

                    ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
                    TariffCarrierTranslationService myService = new TariffCarrierTranslationService(commonContext, entityPM.Tenant);                    

                    if (tariff != null)
                    {
                        TariffCarrierTranslationRepository tariffCarrierTranslationRepository = new TariffCarrierTranslationRepository(commonContext);

                        if (string.IsNullOrEmpty(entityPOCO.OriginPortId) && !string.IsNullOrEmpty(entityPM.OriginPortId))
                        {
                            TariffCarrierTranslation carrierTranslation = tariffCarrierTranslationRepository.GetTranslationByPortAndCarrierAndPartnerCode(entityPM.OriginPortText, entityPM.OriginPortId, tariff.SellerId, entityPM.Tenant);
                            if (carrierTranslation == null)
                            {
                                this.CreateCarrierTranslation(entityPM.OriginPortText, entityPM.OriginPortId, tariff.SellerId, entityPM.Tenant, myService);                                
                            }
                        }

                        if (string.IsNullOrEmpty(entityPOCO.DestinationPortId) && !string.IsNullOrEmpty(entityPM.DestinationPortId))
                        {
                            TariffCarrierTranslation carrierTranslation = tariffCarrierTranslationRepository.GetTranslationByPortAndCarrierAndPartnerCode(entityPM.DestinationPortText, entityPM.DestinationPortId, tariff.SellerId, entityPM.Tenant);
                            if (carrierTranslation == null)
                            {
                                this.CreateCarrierTranslation(entityPM.DestinationPortText, entityPM.DestinationPortId, tariff.SellerId, entityPM.Tenant, myService);
                            }
                        }
                    }
                }
            }
        }

        private void CreateCarrierTranslation(string code, string portId, string carrierId, int tenant, TariffCarrierTranslationService myService)
        {
            TariffCarrierTranslationPM carrierTranslation = new TariffCarrierTranslationPM()
            {
                Tenant = tenant,
                PortId = portId,
                CarrierId = carrierId,
                PartnerCode = code,
            };

            myService.Create(carrierTranslation);
        }

        protected override void UpdateComposition(TariffLinePM entityPM)
        {
            var linePrices = (from d in entityPM.ContainersPrices
                              group d by d.SurchargeId into g
                              select new
                              {
                                  Id = g.Key,
                                  Count = g.Count()
                              }).ToList();

            if (linePrices.Where(d => d.Count > 1).Any())
            {
                throw new ApplicationException("Tariff line has duplicated charges");
            }

            TariffLinesContainersPriceUpdateService tariffLinesContainersPriceUpdateService = new TariffLinesContainersPriceUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            tariffLinesContainersPriceUpdateService.UpdateMulti(entityPM.ContainersPrices, entityPM.DeletedContainersPrices, entityPM, false);
        }
    }
}
