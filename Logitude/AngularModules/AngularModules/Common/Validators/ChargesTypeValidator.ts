import {ChargesTypePM} from '../EntityPMs/ChargesTypePM';

export class ChargesTypeValidator {
    public Validate(entityPM: ChargesTypePM) {
        var errors = [];

        if (entityPM.IsAutoDisplayInQuote || entityPM.IsAutoDisplayInShipment || entityPM.IsAutoDisplayInConsolidation || entityPM.IsAutoDisplayInCustoms) {
            if (!entityPM.IsExport && !entityPM.IsImport && !entityPM.IsDomestic && !entityPM.IsDrop) {
                errors.push("Please select at least one direction (export, import, domestic or drop)");
            }
        }
       
        return errors;
    }
}
