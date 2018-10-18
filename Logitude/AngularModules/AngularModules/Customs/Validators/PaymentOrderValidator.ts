declare var window: any;
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';
import { AppTool, DateTool } from '../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../Infrastructure/Utilities/TextCodeTranslator';
import { Validator } from '../../Infrastructure/Validators/Validator';

import { PaymentOrderPM } from '../EntityPMs/PaymentOrderPM';

export class PaymentOrderValidator {
    private _PaymentOrderPM: PaymentOrderPM;
    private FIELD_IS_REQUIERD: string;
    public ValidationErrorMessageCodes: string[];

    constructor() {
        this.ValidationErrorMessageCodes = [];
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
    }

    public SetEntityPM(paymentOrderPM: PaymentOrderPM) {
        this._PaymentOrderPM = paymentOrderPM;
    }


    public SubmitDateTimeCheck() {
        var errorMessage: string = "";

        if (this._PaymentOrderPM != null) {
            //if (this._PaymentOrderPM.SubmitDate == null) {
            //    errorMessage = errorMessage = "Customs.General.O.TaxationDateTimeNotToday";
            //}
            //else{
            //    //if ((this._PaymentOrderPM.SubmitDate.getFullYear != Date.) ||
            //    //    (this._PaymentOrderPM.SubmitDate.Value.Date.Month != DateTime.Now.Date.Month) ||
            //    //    (this._PaymentOrderPM.SubmitDate.Value.Date.Day != DateTime.Now.Date.Day)) {
            //    //    errorMessage = "Customs.General.O.TaxationDateTimeNotToday";
            //    //}
            //}

            if (!AppTool.IsNullOrEmpty(errorMessage)) {
                this.ValidationErrorMessageCodes.push(errorMessage);
            }
        }
    }

    public Validate(entityPM: PaymentOrderPM) {

        var result = [];
        this._PaymentOrderPM = entityPM;
        return this.ValidationErrorMessageCodes;
    }

    GetRequierdFieldErrorText(fieldName) {
        return this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate(fieldName));
    }
}
