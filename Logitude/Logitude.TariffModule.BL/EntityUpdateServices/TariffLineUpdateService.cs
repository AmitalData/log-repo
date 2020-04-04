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
                    string loggedContactId = null;
                    ContactRepository contactRepository = new ContactRepository(commonContext);
                    Contact contact = contactRepository.GetSingleContactByEmail(AuthenticationUtil.GetAuthenticatedUser(), entityPM.Tenant);

                    if (contact != null)
                    {
                        loggedContactId = contact.Id;
                    }

                    if (tariff != null && !string.IsNullOrEmpty(loggedContactId))
                    {
                        TariffCarrierTranslationRepository tariffCarrierTranslationRepository = new TariffCarrierTranslationRepository(commonContext);

                        if (string.IsNullOrEmpty(entityPOCO.OriginPortId) && !string.IsNullOrEmpty(entityPM.OriginPortId))
                        {
                            TariffCarrierTranslation carrierTranslation = tariffCarrierTranslationRepository.GetTranslationByPortAndCarrierAndPartnerCode(entityPM.OriginPortText, entityPM.OriginPortId, tariff.SellerId, entityPM.Tenant);
                            if (carrierTranslation == null)
                            {
                                this.CreateCarrierTranslation(loggedContactId, entityPM.OriginPortText, entityPM.OriginPortId, tariff.SellerId, entityPM.Tenant, tariffCarrierTranslationRepository);                                
                            }
                        }

                        if (string.IsNullOrEmpty(entityPOCO.DestinationPortId) && !string.IsNullOrEmpty(entityPM.DestinationPortId))
                        {
                            TariffCarrierTranslation carrierTranslation = tariffCarrierTranslationRepository.GetTranslationByPortAndCarrierAndPartnerCode(entityPM.DestinationPortText, entityPM.DestinationPortId, tariff.SellerId, entityPM.Tenant);
                            if (carrierTranslation == null)
                            {
                                this.CreateCarrierTranslation(loggedContactId, entityPM.DestinationPortText, entityPM.DestinationPortId, tariff.SellerId, entityPM.Tenant, tariffCarrierTranslationRepository);
                            }
                        }

                        commonContext.SaveChanges();
                    }
                }
            }
        }

        private void CreateCarrierTranslation(string loggedContactId, string code, string portId, string carrierId, int tenant, TariffCarrierTranslationRepository tariffCarrierTranslationRepository)
        {
            TariffCarrierTranslation carrierTranslation = new TariffCarrierTranslation()
            {
                Id = IdCounter.GetNumber("TariffCarrierTranslation", tenant),
                Tenant = tenant,
                PortId = portId,
                CarrierId = carrierId,
                PartnerCode = code,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                CreatedByUserId = loggedContactId,
                UpdatedByUserId = loggedContactId,
                SearchFields = code,
            };

            tariffCarrierTranslationRepository.Add(carrierTranslation);
        }

        protected override void UpdateComposition(TariffLinePM entityPM)
        {
            TariffLinesContainersPriceUpdateService tariffLinesContainersPriceUpdateService = new TariffLinesContainersPriceUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            tariffLinesContainersPriceUpdateService.UpdateMulti(entityPM.ContainersPrices, entityPM.DeletedContainersPrices, entityPM, false);
        }
    }
}
