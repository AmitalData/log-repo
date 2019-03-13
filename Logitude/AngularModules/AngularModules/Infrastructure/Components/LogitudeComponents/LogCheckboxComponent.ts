declare var window: any;
import {Directive, ElementRef, Renderer, Input, Output, Component, EventEmitter, OnInit, OnChanges, OnDestroy} from '@angular/core';
import {BaseComponent} from './BaseComponent';
import {UIProperty, UIProperties, UIPropertyArgs} from './UIProperties';
import {ObjectFieldPM} from '../../EntityPMs/ObjectFieldPM';
import {SessionLocator} from '../../Utilities/SessionLocator';
import {AppTool} from '../../Tools';
import {TextCodeTranslator} from '../../Utilities/TextCodeTranslator';
import {FormGroup, FormControl, Validators} from '@angular/forms';
import {ControlsIdCounter} from '../../Utilities/ControlsIdCounter';
import {CustomFieldClass} from '../../DataContracts/CustomFieldClass';

@Component({
    selector: 'LogCheckBox',
    template:
    `
    <table *ngIf="uiProperty.IsVisible">
        <tr>
            <td style="width: 16px;">
                <div class="CheckBox">
                    <input [attr.id]="ControlId" type="checkbox" [disabled]="IsDisabled" [(ngModel)]="BoolValue" (focus)="onFocus()" (blur)="onBlur()" />
                    <label [attr.for]="ControlId">{{Text}}</label>
                </div>
            </td>

            <td style="width: 18px;" *ngIf="!HideColumns">                
                <HelpIcon *ngIf="ShowHelp" [HideHeader]="true" [Text]="ObjectFieldHelp" [IconSize]="15"></HelpIcon>
            </td>

            <td>
                <div></div>
            </td>
        </tr>
    </table>
    `,

    inputs: ['ObjectFieldName', 'ObjectTableName', 'DataContext', 'HideColumns', 'IsDisabled','Text'],
})

export class LogCheckboxComponent implements OnInit, OnDestroy {
    public ControlId: string = null;
    public ShowHelp: boolean = false;
    public ObjectField: ObjectFieldPM;
    public ObjectFieldName: string = null;
    public ObjectFieldHelp: string = null;
    public ObjectTableName: string = null;
    public HideColumns: boolean = false;
    public DataContext: any;
    private dataContext: BaseComponent;
    public uiProperty: UIProperty;
    CopyValueSubs: any;
    private show: boolean;

    private isDisabled: boolean;
    public get IsDisabled() {
        return this.isDisabled;
    }

    public set IsDisabled(value: boolean) {
        this.isDisabled = value;
    }

    
    Text: string;
    @Input() NoObjectField: boolean = false;
    //value: boolean;
    @Input() public get Value() {
        return this.boolValue;
    }
    public set Value(newValue: boolean) {
        if (this.BoolValue != newValue) {
            //this.value = newValue;
            this.BoolValue = newValue;
        }
    }

    boolValue: boolean;
    public get BoolValue() {
        return this.boolValue;
    }
    public set BoolValue(newValue: boolean) {

        var dataContextValue = this.DataContext[this.ObjectFieldName];

        if (!this.ObjectField) {
            var table = window.ObjectTables.filter(d => d.Name === this.ObjectTableName)[0];
            if (table) {
                this.ObjectField = window.ObjectFields.filter(d => d.ObjectTableId === table.Id && d.FieldName === this.ObjectFieldName)[0];
            }
        }

        if (this.ObjectField && this.ObjectField.IsCustom) {
            var customFieldClass: CustomFieldClass = this.DataContext[this.ObjectFieldName];
            if (customFieldClass != null && customFieldClass != undefined) {
                dataContextValue = customFieldClass.GetFieldDataTypeValue(this.ObjectField, customFieldClass.Value);
            }
            else {
                console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
            }
        }
        if (this.boolValue != newValue) {
            if (typeof (newValue) == 'boolean') {
                this.boolValue = newValue;
                if (typeof (this.boolValue) == 'boolean') {
                    if (dataContextValue != this.boolValue) {

                        if (this.ObjectField && this.ObjectField.IsCustom) {
                            var customFieldClass: CustomFieldClass = this.DataContext[this.ObjectFieldName];
                            if (customFieldClass != null && customFieldClass != undefined) {
                                customFieldClass.Value = this.boolValue + "";
                                this.DataContext[this.ObjectFieldName] = customFieldClass;
                            }
                            else {
                                console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
                            }
                            
                           
                        }
                        else {
                            this.DataContext[this.ObjectFieldName] = this.boolValue;
                        }
                        this.ValueChanged.emit(this.boolValue);
                    }
                }
            }
        }
    }

    checked: boolean;
    @Input() public get Checked() {
        return this.checked;
    }
     public set Checked(newValue: boolean) {
        if (this.checked != newValue) {
            this.checked = newValue;
            this.BoolValue = this.checked;
        }
    }

   
    @Input() LogitudeForm: FormGroup;
    @Output() ValueChanged = new EventEmitter();
    constructor() {
        this.show = false;
    }


    counterId: number;
    CheckIfExists(IdCom: string) {
        var element = document.getElementById(IdCom);
        if (element != null && element != undefined) {
            return true;
        }
        return false;
    }

    SetControlIds(baseIdCombination: string) {
        this.ControlId = baseIdCombination;
    }

    ngOnInit() {
        //if (this.ValueChangedEvent) {
        //    this.ValueChangedEvent.subscribe((res) => {
        //        this.BoolValue = res;
        //    });
        //}
        var objectFieldAvailable: boolean = true;

        this.counterId = null;
        var baseIdCombination = null;
        if (this.ObjectTableName) {
            baseIdCombination = this.ObjectTableName + "_" + this.ObjectFieldName;
        }
        else {
            baseIdCombination = this.ObjectFieldName;
        }
        if (this.CheckIfExists(baseIdCombination)) {
            this.counterId = ControlsIdCounter.GetNextControlIdCounter(baseIdCombination);
        }

        if (this.counterId != null) {
            baseIdCombination = baseIdCombination + '_' + this.counterId.toString();
        }

        this.SetControlIds(baseIdCombination);
        //if (this.FocusOnMe) {// it means it is inside a grid.
        if (SessionLocator.CurrentSession) {
            this.CopyValueSubs = SessionLocator.CurrentSession.CopyCellIntoMemory.subscribe((id) => {
                if (id == this.ControlId) {
                    SessionLocator.CurrentSession.CopiedCell = this.DataContext[this.ObjectFieldName];
                }
            });

            if (SessionLocator.CurrentSession.CopiedCell) {
                this.DataContext[this.ObjectFieldName] = SessionLocator.CurrentSession.CopiedCell;
                SessionLocator.CurrentSession.CopiedCell = null;
            }
        }
        //}
        var table = window.ObjectTables.filter(d => d.Name === this.ObjectTableName)[0];

        if (table) {
            this.ObjectField = window.ObjectFields.filter(d => d.ObjectTableId === table.Id && d.FieldName === this.ObjectFieldName)[0];
            if (!this.ObjectField) {
                objectFieldAvailable = false;
            }

            else if (this.ObjectField.HelpTextCodeId != null) {                
                this.ObjectFieldHelp = TextCodeTranslator.Translate(this.ObjectField.HelpTextTextCodeCode);

                if (!AppTool.IsNullOrEmpty(this.ObjectFieldHelp)) {
                    if (this.ObjectFieldHelp.length > 1) {
                        this.ShowHelp = true;
                    }
                }
            }
        }

        else {
            objectFieldAvailable = false;
        }

        this.uiProperty = this.DataContext.UIProperties.GetUIProperty(this.ObjectFieldName, this.ObjectTableName, this.DataContext);
        this.IsDisabled = !this.uiProperty.IsEnabled;
        if (objectFieldAvailable) {
            

            this.uiProperty.UIPropertyChanged.subscribe(value => {
                if (value instanceof UIPropertyArgs) {
                    var uiPropertyArgs: UIPropertyArgs = value as UIPropertyArgs;
                    var uiProperty: UIProperty = uiPropertyArgs.uiProperty as UIProperty;

                    if (uiProperty.FieldName == this.ObjectFieldName && uiProperty.ObjectTableName == this.ObjectTableName) {
                        if (uiPropertyArgs.property == "IsEnabled") {
                            var isEnabled = uiPropertyArgs.newValue;
                            this.IsDisabled = !isEnabled;
                            this.uiProperty.IsEnabled = isEnabled;
                        }
                    }
                }
            });
        }


        if (!objectFieldAvailable && !this.NoObjectField) {
            console.warn(this.ObjectFieldName + " CHECKBOX has no object field!");
        }
        if (this.ObjectField && this.ObjectField.IsCustom) {
            var customFieldClass: CustomFieldClass = this.DataContext[this.ObjectFieldName];
            if (customFieldClass != null && customFieldClass != undefined) {
                this.BoolValue = customFieldClass.GetFieldDataTypeValue(this.ObjectField, customFieldClass.Value);
            }
            else {
                console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
            }
        }
        else {
            this.BoolValue = this.DataContext[this.ObjectFieldName];
        }
        
    }


    onFocus() {

    }

    onBlur() {
    }

    ngOnDestroy() {
        
        if (this.CopyValueSubs) {
            this.CopyValueSubs.unsubscribe();
            this.CopyValueSubs = null;
        }
    }

}
