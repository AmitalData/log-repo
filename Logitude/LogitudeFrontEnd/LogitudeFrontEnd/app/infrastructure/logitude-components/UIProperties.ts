import {OnInit, Output, EventEmitter} from 'angular2/core';
import {TextcodeTranslationPipe} from './../pipes/textcode-translation/textcode-translation.pipe';
export class UIProperty implements OnInit {

    public IsEnabled: boolean;
    public IsRequired: boolean;
    public IsVisible: boolean;
    public ValidValue: boolean;
    public ValidationError: string;
    @Output UIPropertyChanged: EventEmitter<any> = new EventEmitter();

    constructor(public FieldName: string,
        public ObjectTableName: string) {

        this.IsEnabled = true;
        this.IsRequired = false;
        this.IsVisible = true;
        this.ValidValue = true;
    }
    
    ngOnInit() {

       
    }
}


export class UIProperties {


    constructor() {
        
        this.UIPropertyList = new Array<UIProperty>();
        //console.log("i'm in uipropertiees constructor");
    }
    public UIPropertyList: UIProperty[];

    public GetUIProperty(fieldName: string, objectTableName: string) {
        //console.log("i'm in getuiproprty of" + fieldName +"  " +objectTableName);
        var uiProperty: UIProperty;
        if (this.UIPropertyList.length > 0) {
            uiProperty = this.UIPropertyList.filter(d=> d.FieldName == fieldName && d.ObjectTableName == objectTableName)[0];
            if (!uiProperty) {

                uiProperty = new UIProperty(fieldName, objectTableName);
                this.UIPropertyList.push(uiProperty);
            }
        }
        else
        {
            uiProperty = new UIProperty(fieldName, objectTableName);
            this.UIPropertyList.push(uiProperty);
        }

        uiProperty = this.RefreshUIProperty(fieldName, objectTableName);

        //console.log(uiProperty.IsRequired);
        return uiProperty;
    }

    public SetValidity(fieldName: string, objectTableName: string, value: boolean, errorMessage: string) {
        var uiProperty: UIProperty;
        uiProperty = this.UIPropertyList.filter(d=> d.FieldName == fieldName && d.ObjectTableName == objectTableName)[0];
        uiProperty.ValidValue = value;
        uiProperty.ValidationError = errorMessage;
    }

    public RefreshUIProperty(fieldName: string, objectTableName: string) {
        var uiProperty: UIProperty;
        uiProperty = this.UIPropertyList.filter(d=> d.FieldName == fieldName && d.ObjectTableName == objectTableName)[0];
        var field: any;
        var table: any;
        table = window.ObjectTables.filter(d => d.Name === objectTableName)[0];
        field = window.ObjectFields.filter(d => d.ObjectTableId === table.Id && d.FieldName === fieldName)[0];
       // console.log(field);
        uiProperty.IsRequired = field.IsRequiered;
        //uiProperty.IsRequired = true;
        //if (uiProperty.IsRequired && (entityPM[fieldName] == null || entityPM[fieldName] =="")) {
        //    uiProperty.ValidValue = false;
        //    uiProperty.ValidationError = fieldName + " " + "is required";
        //    console.log(fieldName + " :" + uiProperty.ValidationError);
        //}
        return uiProperty;
    }

    public SetVisibility(fieldName: string, objectTableName: string, value: boolean)
    {
        //console.log("setting visibilisy");
        //console.log(fieldName, objectTableName, value);
        var uiProperty: UIProperty;
        uiProperty = this.UIPropertyList.filter(d=> d.FieldName == fieldName && d.ObjectTableName == objectTableName)[0];
        //console.log(uiProperty);
        uiProperty.IsVisible = value;
        uiProperty.UIPropertyChanged.next(uiProperty);
    }

    public SetEnabled(fieldName: string, objectTableName: string, value: boolean) {
        var uiProperty: UIProperty;
        uiProperty = this.UIPropertyList.filter(d=> d.FieldName == fieldName && d.ObjectTableName == objectTableName)[0];
        uiProperty.IsEnabled = value;
        uiProperty.UIPropertyChanged.next(uiProperty);
    }

    public SetRequired(fieldName: string, objectTableName: string, value: boolean) {
        var uiProperty: UIProperty;
        uiProperty = this.UIPropertyList.filter(d=> d.FieldName == fieldName && d.ObjectTableName == objectTableName)[0];
        uiProperty.IsRequired = value;
        uiProperty.UIPropertyChanged.next(uiProperty);
    }


  
}