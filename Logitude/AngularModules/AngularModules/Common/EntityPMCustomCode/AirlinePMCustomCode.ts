import { AirlinePM } from '../EntityPMs/AirlinePM';
export class AirlinePMCustomCode {
    public static ApplyEntityChanged(propertyName: string, entityPM: AirlinePM) {
        if (propertyName == "LimitedLength") {
            if (entityPM.LimitedLength == false) {
                entityPM.CheckDigit = false;
                entityPM.UIProperties.SetEnabled("CheckDigit", "Airline", false);
            }
            else {
                entityPM.UIProperties.SetEnabled("CheckDigit", "Airline", true);
            }
        }
    }
}