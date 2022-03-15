import { PaymentTermPM } from '../EntityPMs/PaymentTermPM';
export class PaymentTermPMCustomCode {
    public static ApplyEntityChanged(propertyName: string, entityPM: PaymentTermPM) {
        if (propertyName == "EndOfMonth") {
            var isEndOfMonthTrue = entityPM.EndOfMonth;
            entityPM.UIProperties.SetEnabled("NumberOfMonths", "PaymentTerm", isEndOfMonthTrue);
        }
    }
}
