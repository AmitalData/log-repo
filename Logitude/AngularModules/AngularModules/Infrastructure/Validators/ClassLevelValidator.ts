declare var window: any;
declare var System: any;
import {TextCodeTranslator} from '../Utilities/TextCodeTranslator';
import {AppTool} from '../Tools';
import {RulesValidator} from './RulesValidator';
import { FieldValidator } from './FieldValidator';
import { time } from 'console';

export class ClassLevelValidator {
    public ErrorsArray: any[];
    private _RulesValidator: RulesValidator;
    constructor() {
        this.ErrorsArray = [];
        if (this._RulesValidator == null) {
            this._RulesValidator = new RulesValidator();
        }
    }

    public Validate(objectTableName, entityPM) {

        var errorsArray = [];
        var objectTableId;
        var requiredErrorCode: string = "General.M.FieldIsRequired";
        var minmaxErrorCode: string = "General.M.MinMax";
        var translatedRequiredError: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var translatedMinMaxError: string = TextCodeTranslator.Translate("General.M.MinMax");
        var translatedMaxError: string = TextCodeTranslator.Translate("General.M.Max");

        var objectTable = window.ObjectTables.filter(x => x.Name === objectTableName)[0];

        if (objectTable) {
            objectTableId = objectTable.Id;
        }

        var objectFields = window.ObjectFields.filter(x => x.ObjectTableId === objectTable.Id);

        if (objectTableName == "Shipment" && entityPM.ShipmentLevelCode == "C") {
            var masterTable = window.ObjectTables.filter(x => x.Name === "Master")[0];
            objectFields = objectFields.filter(x => x.IsCustom === false || (x.IsCustom === true && x.ObjectTableId === masterTable.Id));
        }

        var isNewEntity = (entityPM.OldEntityPM === null || entityPM.OldEntityPM === undefined);
        if (this._RulesValidator.IsNewEntity != isNewEntity) {
            this._RulesValidator.IsNewEntity = isNewEntity;
            this._RulesValidator.Initizialize();
        }
        this._RulesValidator.ValidateAllTableRules(entityPM, objectTable.Id,errorsArray);

        objectFields.forEach((objectfield, key) => {
            if (objectfield.ObjectTableId === objectTableId) {
                if (objectfield.IsRequiered === true) {

                    //Boolean
                    //Constant
                    //Date
                    //DateTime
                    //Decimal
                    //Double
                    //Emails
                    //Integer
                    //List
                    //LookUp
                    //nText
                    //PickList
                    //SigDouble
                    //Text
                    //UnsDecimal
                    //UnsInteger

                    switch (objectfield.DataTypeCode) {

                        case "Integer":
                        case "Double":
                        case "Decimal":
                        case "SigDouble":
                        case "UnsDecimal":
                        case "UnsInteger":
                        case "Text":
                        case "nText":
                        case "LookUp":
                        case "DateTime":
                            {
                                if (this.IsNullFieldValue(entityPM, objectfield)) {
                                    errorsArray.push(this.GetTranslatedRequiredError(objectfield, translatedRequiredError));
                                }
                                break;
                            }
                        case "Date":
                        case "PickList":
                        case "Time":
                            {
                                if (objectfield.IsCustom && this.IsNullCustomFieldValue(entityPM, objectfield)) {
                                    errorsArray.push(this.GetTranslatedRequiredError(objectfield, translatedRequiredError));
                                }

                                break;
                            }
                    }
                }


                this.validateTextAndNumberDataTypes(entityPM, objectfield, errorsArray);
            }
        });

        return errorsArray;
    }

    private GetTranslatedRequiredError(objectfield: any, translatedRequiredError: string) {
        let fieldName: string = TextCodeTranslator.Translate(objectfield.FullNameTextCodeCode);
        let fieldError: string = translatedRequiredError.replace("%FieldName", fieldName);

        return fieldError;
    }

    private IsNullFieldValue(entityPM: any, objectfield: any) {
        if (objectfield.IsCustom) {
            return this.IsNullCustomFieldValue(entityPM, objectfield);
        }

        return AppTool.IsNullOrEmpty(entityPM[objectfield.FieldName]);

    }

    private IsNullCustomFieldValue(entityPM: any, objectfield: any) {
        let customFieldObject = entityPM[objectfield.FieldName];
        return !customFieldObject || AppTool.IsNullOrEmpty(customFieldObject.Value);

    }


    public ValidateCustomEntity(objectTableName, entityPM) {

        var errorsArray = [];
        var translatedRequiredError: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        var objectTable = window.ObjectTables.filter(x => x.Name === objectTableName)[0];
        var objectFields = window.ObjectFields.filter(x => x.ObjectTableId === objectTable.Id);

        objectFields.filter(d => d.IsRequiered).forEach((objectfield, key) => {
            let fieldError = this.ValidateCustomField(entityPM, objectfield, translatedRequiredError);
            if (fieldError) errorsArray.push(fieldError);
        });

        return errorsArray;
    }


    private ValidateCustomField(entityPM: any, objectfield: any, translatedRequiredError: string) {
        let fieldValue = this.GetFieldValue(entityPM, objectfield);
        if (!AppTool.IsNullOrEmpty(fieldValue)) return;
        var fieldName: string = TextCodeTranslator.Translate(objectfield.FullNameTextCodeCode);
        return translatedRequiredError.replace("%FieldName", fieldName);

    }

    private validateTextAndNumberDataTypes(entityPM: any, objectfield: any, errorsArray: any[]) {
        if (!entityPM[objectfield.FieldName]) return;

        this.validateTextDataType(objectfield, entityPM, errorsArray);
        this.validateNumberDataType(objectfield, entityPM, errorsArray);

    }

    private validateNumberDataType(objectfield: any, entityPM: any, errorsArray: any[]) {
        if (objectfield.DataTypeCode !== "Decimal") return;
        var fieldValidator: FieldValidator = new FieldValidator();
        var fieldvalue = this.GetFieldValue(entityPM, objectfield);
        let isNotValid = this.GetIsNotValidValue(entityPM, objectfield);
        if (!fieldValidator.IsValidValue(objectfield, fieldvalue, isNotValid)) {
            errorsArray.push(fieldValidator.GetValidationErrorMessage(objectfield, fieldvalue));
        }
    }

    private validateTextDataType(objectfield: any, entityPM: any, errorsArray: any[]) {
        if (objectfield.DataTypeCode !== "Text" && objectfield.DataTypeCode !== "nText") return;
        if (objectfield.MaxLength === 0 && objectfield.MinLength === 0) return;
        var fieldValidator: FieldValidator = new FieldValidator();
        var fieldvalue = this.GetFieldValue(entityPM, objectfield);
        if (!fieldValidator.IsValidValue(objectfield, fieldvalue, false)) {
            errorsArray.push(fieldValidator.GetValidationErrorMessage(objectfield, fieldvalue));
        }

    }

    private GetIsNotValidValue(entityPM: any, objectfield: any) {
        if (entityPM[objectfield.FieldName] && objectfield.IsCustom) {
            return entityPM[objectfield.FieldName].IsNotValid;
        }
        return false;
    }

    private GetFieldValue(entityPM: any, objectfield: any) {
        if (entityPM[objectfield.FieldName]) {
            var fieldvalue = "";
            if (objectfield.IsCustom === true) {
                fieldvalue = entityPM[objectfield.FieldName].Value;
            }

            else
                fieldvalue = entityPM[objectfield.FieldName];

            if (fieldvalue == null || fieldvalue == undefined) {
                fieldvalue = "";
            }

        }
        return fieldvalue;
    }

    public IsValid(entityPM) {
        var isValid: boolean;

        return isValid;
    }
}
