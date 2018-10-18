declare var window: any;
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';
import { AppTool, DateTool } from '../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../Infrastructure/Utilities/TextCodeTranslator';
import { Validator } from '../../Infrastructure/Validators/Validator';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';

import { VehiclePM } from '../EntityPMs/VehiclePM';

export class VehicleValidator {
    private _VehiclePM: VehiclePM;
    private FIELD_IS_REQUIERD: string;
    public ValidationErrorMessageCodes: string[];
    OriginalValidationErrorMessageCodes: any;

    constructor() {
        this.ValidationErrorMessageCodes = [];
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
    }

    public SetEntityPM(VehiclePM: VehiclePM) {
        this._VehiclePM = VehiclePM;
    }


    public SubmitDateTimeCheck() {
        var errorMessage: string = "";

        if (this._VehiclePM != null) {

            if (!AppTool.IsNullOrEmpty(errorMessage)) {
                this.ValidationErrorMessageCodes.push(errorMessage);
            }
        }
    }

    public Validate(entityPM: VehiclePM) {

        var result = [];
        this._VehiclePM = entityPM;
        this.MandatoryFieldsCheck();
        return this.ValidationErrorMessageCodes;
    }

    //Check if there is an empty Importer Declarations
    public MandatoryFieldsCheck() {
        var errorMessage: string = "";

        if (this._VehiclePM != null) {
            if (AppTool.IsNullOrEmpty(this._VehiclePM.ImporterIdentityId) && (AppTool.IsNullOrEmpty(this._VehiclePM.ImporterPassportNumber) || AppTool.IsNullOrEmpty(this._VehiclePM.ImporterPassCountryCode) || AppTool.IsNullOrEmpty(this._VehiclePM.ImporterPassportTypeCode))) {
                errorMessage = "חובה להזין מס' יבואן או נתוני דרכון(מס' דרכון, מדינת הדרכון וסוג תעודת מסע) לרכב";
                if (!AppTool.IsNullOrEmpty(errorMessage)) {
                    this.ValidationErrorMessageCodes.push(errorMessage);
                }
            }
            var newValue = this._VehiclePM.VehiclePowerKW;
            if (!AppTool.IsNullOrEmpty(newValue)) {
                var strValue = newValue.toString();
                if (strValue.indexOf(".") > -1) strValue = newValue.toString().substring(0, newValue.toString().indexOf("."));
                if (!AppTool.IsNullOrEmpty(strValue)) {
                    if (strValue.length > 5) {
                        errorMessage = "הספק מנוע לא יכול להיות ארוך מחמישה תווים";
                        if (!AppTool.IsNullOrEmpty(errorMessage)) {
                            this.ValidationErrorMessageCodes.push(errorMessage);
                        }
                    }
                }
            }
            var newValue = this._VehiclePM.VehicleMaxPowerKW;
            if (!AppTool.IsNullOrEmpty(newValue)) {
                var strValue = newValue.toString();
                if (strValue.indexOf(".") > -1) strValue = newValue.toString().substring(0, newValue.toString().indexOf("."));
                if (!AppTool.IsNullOrEmpty(strValue)) {
                    if (strValue.length > 5) {
                        errorMessage = "הספק מנוע מירבי לא יכול להיות ארוך מחמישה תווים";
                        if (!AppTool.IsNullOrEmpty(errorMessage)) {
                            this.ValidationErrorMessageCodes.push(errorMessage);
                        }
                    }
                }
            }
            if (this._VehiclePM.VehicleOwners != null && this._VehiclePM.VehicleOwners.length > 0) {
                for (let item of this._VehiclePM.VehicleOwners) {
                    if (AppTool.IsNullOrEmpty(item.ClientId) && (AppTool.IsNullOrEmpty(item.PassportNumber) || AppTool.IsNullOrEmpty(item.PassCountryCode) || AppTool.IsNullOrEmpty(item.ImporterPassportTypeCode))) {
                        errorMessage = "חובה להזין זיהוי בעל רכב או נתוני דרכון(מס' דרכון, מדינת דרכון יבואן וסוג תעודת מסע) לבעל רכב";
                        if (!AppTool.IsNullOrEmpty(errorMessage)) {
                            this.ValidationErrorMessageCodes.push(errorMessage);
                        }
                    }
                }
                
            }
        }
    }

    GetRequierdFieldErrorText(fieldName) {
        return this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate(fieldName));
    }

}
