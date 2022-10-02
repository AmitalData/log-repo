declare var window: any;
declare var System: any;
import {TextCodeTranslator} from '../Utilities/TextCodeTranslator';
import {AppTool} from '../Tools';
import {RulesValidator} from './RulesValidator';
import { FieldValidator } from './FieldValidator';

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
                            {
                                if (AppTool.IsNullOrEmpty(entityPM[objectfield.FieldName]))
                                {
                                    var fieldName: string = TextCodeTranslator.Translate(objectfield.FullNameTextCodeCode);
                                    var fieldError: string = translatedRequiredError.replace("%FieldName", fieldName);
                                    errorsArray.push(fieldError);
                                }

                                break;
                            }

                        case "Text":
                        case "nText":
                        case "LookUp":
                        case "DateTime":
                            {
                                if (AppTool.IsNullOrEmpty(entityPM[objectfield.FieldName]))
                                {
                                    var fieldName: string = TextCodeTranslator.Translate(objectfield.FullNameTextCodeCode);
                                    var fieldError: string = translatedRequiredError.replace("%FieldName", fieldName);
                                    errorsArray.push(fieldError);
                                }

                                break;
                            }
                    }
                }

                if (entityPM[objectfield.FieldName]) {
                    if (objectfield.DataTypeCode === "Text" || objectfield.DataTypeCode === "nText") {
                        if (objectfield.MaxLength !== 0 || objectfield.MinLength !== 0) {
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

                                var fieldValidator: FieldValidator = new FieldValidator();
                                if (!fieldValidator.IsValidTextValue(objectfield, fieldvalue)) {
                                    errorsArray.push(fieldValidator.GetMinMaxErrorMessage(objectfield, fieldvalue));
                                }
                            }
                        }
                    }
                }
            }
        });
            
        return errorsArray;
    }

    public IsValid(entityPM) {
        var isValid: boolean;

        return isValid;
    }    
}
