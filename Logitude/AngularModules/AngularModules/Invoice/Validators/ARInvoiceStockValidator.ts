import { ARInvoiceStockPM } from '../EntityPMs/ARInvoiceStockPM';
import { Validator } from '../../Infrastructure/Validators/Validator';
import { DateTool } from '../../Infrastructure/Tools';

export class ARInvoiceStockValidator {
    public Validate(entityPM: ARInvoiceStockPM) {
        var errors: string[] = [];

        Validator.TryValidateObject(entityPM, "ARInvoiceStock", errors);

        if (entityPM.StartDate != null  && entityPM.EndDate != null) {
            if (DateTool.GetDateParts(entityPM.StartDate).DateTicks > DateTool.GetDateParts(entityPM.EndDate).DateTicks) {
                errors.push("Start Date cannot be greater than End Date");
            }
        }

        return errors;
    }
}
