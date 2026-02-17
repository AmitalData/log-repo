declare var System: any;
declare var window: any;
import {OnInit, Output, EventEmitter} from '@angular/core';
import {TextCodeTranslator} from '../../Utilities/TextCodeTranslator';
import {ObjectFieldPM} from '../../EntityPMs/ObjectFieldPM';
import {ObjectTablePM} from '../../EntityPMs/ObjectTablePM';
import {RulesValidator} from '../../Validators/RulesValidator';
export class UIProperty implements OnInit {

    private isEnabled: boolean;
    public get IsEnabled() {
        if (this.isEnabled == undefined) {
            this.isEnabled = true;
        }
        return this.isEnabled;
    }
    public set IsEnabled(newValue: boolean) {
        this.isEnabled = newValue;
    }

    private isRequired: boolean;
    public get IsRequired() {
        if (this.isRequired == undefined) {
            this.isRequired = false;
        }
        return this.isRequired;
    }
    public set IsRequired(newValue: boolean) {
        this.isRequired = newValue;
    }

    private isVisible: boolean;
    public get IsVisible() {
        if (this.isVisible == undefined) {
            this.isVisible = true;
        }
        return this.isVisible;
    }
    public set IsVisible(newValue: boolean) {
        this.isVisible = newValue;
    }

    private validValue: boolean;
    public get ValidValue() {
        if (this.validValue == undefined) {
            this.validValue = true;
        }
        return this.validValue;
    }
    public set ValidValue(newValue: boolean) {
        this.validValue = newValue;
    }

    private validationError: string;
    public get ValidationError() {
        return this.validationError;
    }
    public set ValidationError(newValue: string) {
        this.validationError = newValue;
    }

    private manualValidationError: string;
    public get ManualValidationError() {
        return this.manualValidationError;
    }
    public set ManualValidationError(newValue: string) {
        this.manualValidationError = newValue;
    }

    private isValidManually: boolean;
    public get IsValidManually() {
        return this.isValidManually;
    }
    public set IsValidManually(newValue: boolean) {
        this.isValidManually = newValue;
    }

    private hasWarning: boolean;
    public get HasWarning() {
        if (this.hasWarning == undefined) {
            this.hasWarning = false;
        }
        return this.hasWarning;
    }
    public set HasWarning(newValue: boolean) {
        this.hasWarning = newValue;
    }

    @Output() UIPropertyChanged: EventEmitter<any> = new EventEmitter();
    constructor(public FieldName: string, public ObjectTableName: string) {

        this.IsEnabled = true;
        this.IsRequired = false;
        this.IsVisible = true;
        this.ValidValue = true;
        this.HasWarning = false;
    }

    ngOnInit() {

    }
}


export class UIProperties {
    public UIPropertyList: UIProperty[];
    _RulesValidator: RulesValidator;
    // public EntityPM: any;
    constructor(entity: any = null) {
        this.UIPropertyList = new Array<UIProperty>();

        //this.EntityPM = entity;
    }

    public GetUIProperty(fieldName: string, objectTableName: string, dataContext: any, applyRules: boolean = true) {
        if (!this._RulesValidator) {
            this._RulesValidator = new RulesValidator();
        }
      var entityPM = dataContext;
      if (dataContext) {
        if (dataContext.EntityPM)
          entityPM = dataContext.EntityPM;

            var isNewEntity = (entityPM.OldEntityPM === null || entityPM.OldEntityPM === undefined);
            if (this._RulesValidator.IsNewEntity != isNewEntity) {
                this._RulesValidator.IsNewEntity = isNewEntity;
                this._RulesValidator.Initizialize();
            }
        }

        var uiProperty: UIProperty;


        uiProperty = this.UIPropertyList.filter(d => d.FieldName == fieldName && d.ObjectTableName == objectTableName)[0];
        if (!uiProperty) {

            uiProperty = new UIProperty(fieldName, objectTableName);

            var objectTable: ObjectTablePM = window.ObjectTables.filter(d => d.Name == objectTableName)[0];
            if (objectTable != null && objectTable != undefined) {
                var objectField: ObjectFieldPM = window.ObjectFields.filter(d => d.FieldName == fieldName && d.ObjectTableId == objectTable.Id)[0];
                if (objectField != null && objectField != undefined) {
                    if (objectField.DisplayOnly || objectField.AutomaticField) {
                        uiProperty.IsEnabled = false;
                    }
                }
            }

            if (applyRules && entityPM) {
                this._RulesValidator.ApplyUnConditionalSetFieldRules(fieldName, entityPM, objectTableName);
                this._RulesValidator.ApplyConditionalBlockFieldRules(fieldName, entityPM, objectTableName, false, uiProperty);
                this._RulesValidator.ApplyUnConditionalBlockFieldRules(fieldName, entityPM, objectTableName, false, uiProperty);
                this._RulesValidator.ApplyRequiredFieldRules(fieldName, entityPM, objectTableName, this);
            }

            this.UIPropertyList.push(uiProperty);
        }

        if (uiProperty.IsValidManually == undefined) {
            uiProperty = this.RefreshUIProperty(fieldName, objectTableName);
        }




        return uiProperty;
    }



    public RefreshUIProperty(fieldName: string, objectTableName: string) {
        var uiProperty: UIProperty;
        uiProperty = this.UIPropertyList.filter(d => d.FieldName == fieldName && d.ObjectTableName == objectTableName)[0];
        var objectFieldAvailable: boolean = true;
        var field: any;
        var table: any;
        table = window.ObjectTables.filter(d => d.Name === objectTableName)[0];
        if (table) {
            field = window.ObjectFields.filter(d => d.ObjectTableId === table.Id && d.FieldName === fieldName)[0];
            if (!field) {
                objectFieldAvailable = false;
            }
        }
        else {
            objectFieldAvailable = false;
        }
        if (objectFieldAvailable) {
            uiProperty.IsRequired = field.IsRequiered;
        }

        return uiProperty;
    }

    public SetVisibility(fieldName: string, objectTableName: string, value: boolean = false) {
        var uiProperty: UIProperty = this.GetUIProperty(fieldName, objectTableName, null, false);

        uiProperty.IsVisible = value;
        uiProperty.UIPropertyChanged.emit(new UIPropertyArgs(uiProperty, "IsVisible", value));
    }

    public SetEnabled(fieldName: string, objectTableName: string, value: boolean = false) {

        var uiProperty: UIProperty = this.GetUIProperty(fieldName, objectTableName, null, false);

        uiProperty.IsEnabled = value;
        uiProperty.UIPropertyChanged.emit(new UIPropertyArgs(uiProperty, "IsEnabled", value));
    }

    public SetRequired(fieldName: string, objectTableName: string, value: boolean = false) {
        var translatedRequiredError: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var table = window.ObjectTables.filter(d => d.Name === objectTableName)[0];
        var translatedFieldName = null;
        if (table) {
            var field: ObjectFieldPM = window.ObjectFields.filter(d => d.ObjectTableId === table.Id && d.FieldName === fieldName)[0];
            if (field) {
                translatedFieldName = TextCodeTranslator.Translate(field.FullNameTextCodeCode);
            }
        }
        if (!translatedFieldName) {
            translatedFieldName = fieldName;
        }

        var fieldError: string = translatedRequiredError.replace("%FieldName", translatedFieldName);

        var uiProperty: UIProperty = this.GetUIProperty(fieldName, objectTableName, null, false);

        uiProperty.IsRequired = value;
        if (uiProperty.IsRequired) {
            uiProperty.IsValidManually = false;
            uiProperty.ManualValidationError = fieldError;
            uiProperty.ValidValue = false;
            uiProperty.ValidationError = fieldError;
        }
        else {
            uiProperty.IsValidManually = true;
            uiProperty.ManualValidationError = null;
            uiProperty.ValidValue = true;
            uiProperty.ValidationError = null;

        }
        uiProperty.UIPropertyChanged.emit(new UIPropertyArgs(uiProperty, "IsRequired", value));
    }

    public SetValidity(fieldName: string, objectTableName: string, value: boolean = false, errorMessage: string) {
        var uiProperty: UIProperty = this.GetUIProperty(fieldName, objectTableName, null, false);

        uiProperty.IsValidManually = value;
        uiProperty.ManualValidationError = errorMessage;
        uiProperty.ValidValue = value;
        uiProperty.ValidationError = errorMessage;
        uiProperty.UIPropertyChanged.emit(new UIPropertyArgs(uiProperty, "IsValid", value));
    }

    public SetWarning(fieldName: string, objectTableName: string, value: boolean = false) {
        var uiProperty: UIProperty = this.GetUIProperty(fieldName, objectTableName, false);
        uiProperty.HasWarning = value;
        uiProperty.UIPropertyChanged.emit(new UIPropertyArgs(uiProperty, "HasWarning", value));
    }

}

export class UIPropertyArgs {
    constructor(public uiProperty: UIProperty, public property: string, public newValue: any) {
    }
}
