import {VatTypePM} from '../EntityPMs/VatTypePM';
import {Validator} from '../../Infrastructure/Validators/Validator';
import {TextCodeTranslator} from '../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool} from '../../Infrastructure/Tools';

export class VatTypeValidator {
    private Errors: string[] = [];
    private EntityPM: VatTypePM;
    private message: string;
    constructor() {
        this.Errors = [];
        this.message = TextCodeTranslator.Translate("General.M.FieldIsRequired");
    }

    public Validate(entityPM: any) {
        this.Errors = [];
        this.EntityPM = entityPM;

        Validator.TryValidateObject(this.EntityPM, "VatType", this.Errors);

        if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            if (!this.EntityPM.IsMultiPercentage) {

                if (AppTool.IsNullOrEmpty(this.EntityPM.NewEntityPercentage)) {
                    var field = TextCodeTranslator.Translate("VatType.F.NewEntityPercentage");
                    this.Errors.push(this.message.replace("%FieldName", field));
                }

                if (AppTool.IsNullOrEmpty(this.EntityPM.NewEntityPercentageDate)) {
                    var field = TextCodeTranslator.Translate("VatType.F.NewEntityPercentageDate");
                    this.Errors.push(this.message.replace("%FieldName", field));
                }
            }
        }

        return this.Errors;
    }
}