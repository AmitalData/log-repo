declare var window: any;
import {Component, Input, OnInit} from '@angular/core';
import {BaseComponent} from './BaseComponent';
import {SessionLocator} from '../../Utilities/SessionLocator';
import {TextCodeTranslator} from '../../Utilities/TextCodeTranslator';
import {UIProperty, UIProperties, UIPropertyArgs} from './UIProperties';
import {AppTool} from '../../Tools';
import {ObjectsLocator} from '../../Locators/ObjectsLocator';
import { EntityResourceService } from '../../Services/EntityResourceService';

@Component({
    selector: 'LogLabel',

    inputs:
    [
        'ObjectFieldName',
        'ObjectTableName',
        'DataContext',
        'Text',
        'TextCode',
        "HideColumns",
        "ShowWarning",
        "NoValidation",
        "IsSmallLabel",
        "Replace",
        "ReplaceWith",
    ],

    template:
    `
    <table *ngIf="uiProperty!=null && uiProperty.IsVisible" style="table-layout: fixed;">
        <tr [ngStyle]="{opacity: LabelOpacity}">

            <td style="width: 10px; vertical-align: middle;" *ngIf="!HideColumns">
                <div style="width: 10px;">
                    <div style="width: 7px; height: 7px; background: #E45A26; -webkit-border-radius: 25px; -moz-border-radius: 25px; border-radius: 25px;" *ngIf="!IsFieldValid"></div>
                    <div style="width: 7px; height: 7px; background: #FFCB00; -webkit-border-radius: 25px; -moz-border-radius: 25px; border-radius: 25px;" *ngIf="IsFieldValid && ShowWarning"></div>
                </div>
            </td>

            <td class="TextTrimming" style="vertical-align:middle;"  [style.text-align]="LayoutDirection=='rtl' ? 'right' : 'left'" *ngIf="!IsSmallLabel">
                <label class="Label" [ngStyle]="{color: LabelColor}">{{LabelText}}</label>
            </td>

            <td class="TextTrimming" style="vertical-align:middle;" [style.text-align]="LayoutDirection=='rtl' ? 'right' : 'left'" *ngIf="IsSmallLabel">
                <label class="SmallLabel" [ngStyle]="{color: LabelColor}">{{LabelText}}</label>
            </td>
        </tr>
    </table>
    `,       
})

export class LogLabelComponent implements OnInit {
    public DataContext: any;
    public ObjectFieldName: string;
    private _ObjectTableName: string;  
    public get ObjectTableName(): string {
        return this._ObjectTableName;
    }
    public set ObjectTableName(value: string) {
        this._ObjectTableName = value;
        //if (!AppTool.IsNullOrEmpty(this._ObjectTableName)) {
        //    if (this._ObjectTableName.toLowerCase().startsWith("quoteop")) {
        //        this.LayoutDirection = 'ltr';
        //    }
        //}
    }
    public LabelText: string;
    public HideColumns: boolean = false;
    public IsSmallLabel: boolean = false;
    public LabelColor: string = "#6E7172";
    public LabelOpacity: number = 1;
    public ShowWarning: boolean = false;
    public NoValidation: boolean = false;
    objectfield: any;
    uiProperty: UIProperty;   
    @Input() NoObjectField: boolean = false; 
    LayoutDirection: string = 'ltr';

    constructor(private _entityResourceService: EntityResourceService) {
        this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
        this._entityResourceService.getEntityResourceByTableName("User", 0).subscribe((response: any) => {
        
            });
    }

    private isFieldValid: boolean = true;
    public get IsFieldValid() { return this.isFieldValid; }
    public set IsFieldValid(newValue: boolean) {
        if (this.isFieldValid != newValue) {
            var hehehehe = this.ObjectFieldName;
            this.isFieldValid = newValue;
        }
    }

    private text: string;
    public get Text() { return this.text; }
    public set Text(newValue: string) {
        if (this.text != newValue) {
            this.text = newValue;
            this.SetLabel();
        }
    }

    private textCode: string;
    public get TextCode() { return this.textCode; }
    public set TextCode(newValue: string) {
        if (this.textCode != newValue) {
            this.textCode = newValue;
            this.SetLabel();
        }
    }

    private replace: string;
    public get Replace() { return this.replace; }
    public set Replace(newValue: string) {
        if (this.replace != newValue) {
            this.replace = newValue;
            this.SetLabel();
        }
    }

    private replaceWith: string;
    public get ReplaceWith() { return this.replaceWith; }
    public set ReplaceWith(newValue: string) {
        if (this.replaceWith != newValue) {
            this.replaceWith = newValue;
            this.SetLabel();
        }
    }

    ngOnInit() {
        if (this.DataContext != null) {
            this.uiProperty = this.DataContext.UIProperties.GetUIProperty(this.ObjectFieldName, this.ObjectTableName, this.DataContext);
            this.objectfield = window.ObjectFields.filter(d=> d.FieldName == this.ObjectFieldName && d.ObjectTableName == this.ObjectTableName)[0];

            if (this.uiProperty != null) {
                this.uiProperty.UIPropertyChanged.subscribe(value => {
                    if (value instanceof UIPropertyArgs) {
                        var uiPropertyArgs: UIPropertyArgs = value as UIPropertyArgs;
                        var uiProperty: UIProperty = uiPropertyArgs.uiProperty as UIProperty;

                        if (uiProperty.FieldName == this.ObjectFieldName && uiProperty.ObjectTableName == this.ObjectTableName) {

                            if (uiPropertyArgs.property == "IsRequired" || uiPropertyArgs.property == "IsValid") {
                                if (!this.NoValidation) {
                                    this.uiProperty.IsRequired = uiProperty.IsRequired;
                                }
                            }

                            if (uiPropertyArgs.property == "HasWarning") {
                                if (uiProperty.HasWarning) {
                                    if (this.DataContext[this.ObjectFieldName]) {
                                        this.ShowWarning = false;
                                    }
                                    else {
                                        this.ShowWarning = true;
                                    }
                                }
                                else {
                                    this.ShowWarning = false;
                                }
                            }
                        }
                    }
                    //if (uiProperty != "valuechanges") 
                    //    if (!this.NoValidation) {
                    //        this.uiProperty.IsRequired = uiProperty.IsRequired;
                    //    }
                    //    this.uiProperty.IsEnabled = uiProperty.IsEnabled;
                    //}
                    if (!this.NoValidation) {
                        this.Validate();
                    }
                });
                
                this.SetLabel();
                if (!this.NoValidation) {
                    this.Validate();
                }
            }


            if (!this.objectfield && !this.NoObjectField) {
                console.warn(this.ObjectFieldName + " LABEL has no object field!");
            }
        }
    }

    SetLabel() {
         if (this.DataContext != null) {
            var labelText = "";

            if (this.Text != null) {
                labelText = this.Text + ":";
            }

            else if (this.TextCode != null) {
                labelText = TextCodeTranslator.Translate(this.TextCode) + ":";
            }

            else {
                if (this.objectfield != null) {
                    var textcodecode = this.objectfield.FullNameTextCodeCode;

                    if (AppTool.IsNullOrEmpty(this.Replace)) {
                        if (this.objectfield.ShortNameTextCodeCode) {
                            textcodecode = this.objectfield.ShortNameTextCodeCode;
                        }
                    }
                    var translatedText = TextCodeTranslator.Translate(textcodecode);
                    if (AppTool.IsNullOrEmpty(translatedText)) {
                        textcodecode = this.objectfield.FullNameTextCodeCode;
                        translatedText = TextCodeTranslator.Translate(textcodecode);
                    }
                    labelText = translatedText + ":";
                }

                if (!AppTool.IsNullOrEmpty(this.Replace)) {

                    if (AppTool.IsNullOrEmpty(this.ReplaceWith)) {
                        labelText = labelText.replace(this.Replace, "");
                    }

                    else {
                        labelText = labelText.replace(this.Replace, this.ReplaceWith);
                    }
                }
            }

            this.LabelText = labelText;
        }
    }

    Validate() {

        var isValid: boolean = true;

        if (this.uiProperty != null) {
            if (this.DataContext != null) {
                if (this.uiProperty.IsValidManually == false) {
                    isValid = false;
                }
                else if (!this.uiProperty.ValidValue) {
                    isValid = false;
                }
                else if (this.uiProperty.IsRequired) {
                    if (this.objectfield){
                        if (this.objectfield.DataTypeCode.toLowerCase() != 'boolean') {
                            //if (this.DataContext[this.ObjectFieldName] == null || this.DataContext[this.ObjectFieldName] == "") {
                            if (AppTool.IsNullOrEmpty(this.DataContext[this.ObjectFieldName])){
                                isValid = false;
                            }
                        }
                    }
                    else { 
                        isValid = false;
                    }
                }
                //else if (!this.uiProperty.ValidValue) {
                //    isValid = false;
                //}
                else if (this.uiProperty.HasWarning) {
                    if (this.DataContext[this.ObjectFieldName]) {
                        this.ShowWarning = false;
                    }
                    else {
                        this.ShowWarning = true;
                    }
                }
                else {
                    if (this.objectfield != null) {
                        if (this.objectfield.DataTypeCode != null) {
                            switch (this.objectfield.DataTypeCode.toLowerCase()) {
                                case "text":
                                case "ntext": {
                                    if (!this.objectfield.IsMaxLength) {

                                        var fieldValueLength: number = 0;

                                        if (!AppTool.IsNullOrEmpty(this.DataContext[this.ObjectFieldName])) {
                                            fieldValueLength = this.DataContext[this.ObjectFieldName].length;
                                        }

                                        if (fieldValueLength > this.objectfield.MaxLength) {
                                            isValid = false;
                                        }

                                        else {
                                            if (this.objectfield.MinLength != 0) {
                                                if (fieldValueLength > 0) {
                                                    if (fieldValueLength < this.objectfield.MinLength) {
                                                        isValid = false;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        this.IsFieldValid = isValid;
        this.SetLabelStyle();
    }

    SetLabelStyle() {
        var myColor: string = "#6E7172";
        var myOpacity: number = 1;

        if (this.uiProperty != null) {

            if (this.IsFieldValid) {
                myColor = "#6E7172";
            }

            else {
                myColor = "#E53030";
            }

            if (this.uiProperty.IsEnabled) {
                myOpacity = 1;
            }

            else {
                myOpacity = 0.5;
            }
        }

        this.LabelColor = myColor;
        //this.LabelOpacity = myOpacity;
        this.LabelOpacity = 1;
    }
}
