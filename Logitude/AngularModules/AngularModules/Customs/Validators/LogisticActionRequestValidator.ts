declare var window: any;
import { Injectable } from "@angular/core";
import { TextCodeTranslator } from '../../Infrastructure/Utilities/TextCodeTranslator';
import { ServiceResponse } from '../../Infrastructure/DataContracts/ServiceResponse';
import { LogisticActionRequestPM } from '../EntityPMs/LogisticActionRequestPM';
import { ServiceHelper } from '../../Infrastructure/Utilities/ServiceHelper';
import { defer, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { LogisticActionRequestService } from 'Customs/Services/Others/LogisticActionRequestService';
import { AppTool } from "../../Infrastructure/Tools";
@Injectable()


export class LogisticActionRequestValidator {

    private _LogisticActionRequestPM: LogisticActionRequestPM;
    private FIELD_IS_REQUIERD: string;
    public ValidationErrorMessageCodes: string[];
    OriginalValidationErrorMessageCodes: any;
    LogisticActionRequestService: LogisticActionRequestService = new LogisticActionRequestService();
    constructor() {
        this.ValidationErrorMessageCodes = [];
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
    }

    public SetEntityPM(LogisticActionRequestPM: LogisticActionRequestPM) {
        this._LogisticActionRequestPM = LogisticActionRequestPM;
    }

    public Validate(entityPM: any) {
        this._LogisticActionRequestPM = entityPM;
        // this.CheckIfLogisticActionRequestExist();
        return this.OriginalValidationErrorMessageCodes;
    }

    GetRequierdFieldErrorText(fieldName) {
        return this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate(fieldName));
    }

    CheckIfLogisticActionRequestExist() {
        this.LogisticActionRequestService.GetIfLogisticActionRequestExists(this._LogisticActionRequestPM.Id, this._LogisticActionRequestPM.CargoIdentifierKey1, this._LogisticActionRequestPM.CargoIdentifierKey2, this._LogisticActionRequestPM.CargoIdentifierKey3, this._LogisticActionRequestPM.CargoIdentifierType ).subscribe((Result: any) => {
            var mm: ServiceResponse = Result;
            if (!mm.HasError) {
                if (mm.Result) {
                    var errorMsg: string = TextCodeTranslator.Translate("Customs.General.O.LogisticActionRequestAlreadyExist");
                    if (AppTool.IsNullOrEmpty(errorMsg)) errorMsg = "קיימת בקשה לביטול יצוא עם אותם מזהי מטען";
                    this.ValidationErrorMessageCodes.push(errorMsg);
                }
            }
        });
    }
}
