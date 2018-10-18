import {PackageTypePM} from '../EntityPMs/PackageTypePM';
import {TextCodeTranslator} from '../../Infrastructure/Utilities/TextCodeTranslator';

export class PackageTypeValidator {
    public Validate(entityPM: PackageTypePM) {
        var errors = [];

        var valid = ((entityPM.IsAir) || (entityPM.IsOcean) || (entityPM.IsInland));

        if (!valid) {
            errors.push(TextCodeTranslator.Translate("PackageType.M.ChoosePackageTypeTransportation"));
        }
        return errors;
    }
}