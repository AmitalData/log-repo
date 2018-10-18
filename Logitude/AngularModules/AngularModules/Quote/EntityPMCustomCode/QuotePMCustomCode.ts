import {QuotePM} from '../EntityPMs/QuotePM';
import {QuoteTool} from '../Tools';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';

export class QuotePMCustomCode {

    // Ayman:
    // this Class is not applied (not working)
    // need to applied in the lxml file
    public static ApplyEntityChanged(propertyName: string, entityPM: QuotePM) {
        if (SessionLocator.CurrentSession.CurrentEditComponent) {
            if (SessionLocator.CurrentSession.CurrentEditComponent.IsEntityLoaded) {
                switch (propertyName) {
                    case "GrossWeight":
                    case "ChargeableWeight":
                    case "Volume":
                    case "TEU":
                    case "ValueOfGoods":
                        {
                            QuoteTool.OnQuoteQuantitiesChanged(entityPM);
                            break;
                        }
                }
            }
        }
    }
}