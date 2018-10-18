declare var window: any;
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';
import { AppTool, DateTool } from '../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../Infrastructure/Utilities/TextCodeTranslator';
import { Validator } from '../../Infrastructure/Validators/Validator';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';

import { DeclarationCargoSplitPM } from '../EntityPMs/DeclarationCargoSplitPM';

export class DeclarationCargoSplitValidator {
    private _DeclarationCargoSplitPM: DeclarationCargoSplitPM;
    private FIELD_IS_REQUIERD: string;
    public ValidationErrorMessageCodes: string[];
    OriginalValidationErrorMessageCodes: any;

    constructor() {
        this.ValidationErrorMessageCodes = [];
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
    }

    public SetEntityPM(DeclarationCargoSplitPM: DeclarationCargoSplitPM) {
        this._DeclarationCargoSplitPM = DeclarationCargoSplitPM;
    }


    public SubmitDateTimeCheck() {
        var errorMessage: string = "";

        if (this._DeclarationCargoSplitPM != null) {

            if (!AppTool.IsNullOrEmpty(errorMessage)) {
                this.ValidationErrorMessageCodes.push(errorMessage);
            }
        }
    }

    public Validate(entityPM: DeclarationCargoSplitPM) {

        var result = [];
        this._DeclarationCargoSplitPM = entityPM;

        return this.OriginalValidationErrorMessageCodes;
    }

    GetRequierdFieldErrorText(fieldName) {
        return this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate(fieldName));
    }

}
