import { TariffPM } from '../EntityPMs/TariffPM';
import { AppTool } from '../../Infrastructure/Tools';
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';

export class TariffPMInitService {

    public static InitValues(entityPM: TariffPM, isNew: boolean) {
        if (isNew) {
            entityPM.Tenant = SessionLocator.Tenant;
            entityPM.CreatedByUserId = SessionLocator.LoggedUserId;
            entityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
            entityPM.NewConcurrencyGUID = AppTool.GetNewGuid();
            entityPM.ConcurrencyGUID = entityPM.NewConcurrencyGUID;
        }
    }

    public static ApplyUIPoperties(entityPM: TariffPM, isNew: boolean) {
        if (entityPM.InActive) {
            entityPM.UIProperties.SetEnabled("Name", "Tariff", false);
            entityPM.UIProperties.SetEnabled("Contract", "Tariff", false);
            entityPM.UIProperties.SetEnabled("SellerId", "Tariff", false);
            entityPM.UIProperties.SetEnabled("CurrencyId", "Tariff", false);
            entityPM.UIProperties.SetEnabled("StartDate", "Tariff", false);
            entityPM.UIProperties.SetEnabled("ExpirationDate", "Tariff", false); 
            entityPM.UIProperties.SetEnabled("Description", "Tariff", false);
        }
        else {
            entityPM.UIProperties.SetEnabled("Name", "Tariff", true);
            entityPM.UIProperties.SetEnabled("Contract", "Tariff", true);
            entityPM.UIProperties.SetEnabled("SellerId", "Tariff", true);
            entityPM.UIProperties.SetEnabled("CurrencyId", "Tariff", true);
            entityPM.UIProperties.SetEnabled("StartDate", "Tariff", true);
            entityPM.UIProperties.SetEnabled("ExpirationDate", "Tariff", true);
            entityPM.UIProperties.SetEnabled("Description", "Tariff", true);
        }
    }
}
