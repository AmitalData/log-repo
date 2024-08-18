import { ChangeDetectorRef, Component, ViewChild } from "@angular/core";
import { UIProperties } from "Infrastructure/Components/LogitudeComponents/UIProperties";
import { ValueChange, TextBoxField } from "InfrastructureModules/InfrastructureOthers/AmitalAPI/components/LogTexBoxFormComponent";
import { FieldByTypeComponent } from "./FieldByTypeComponent";

@Component({
    selector: 'LogDefaultAndConfigurationValueTab',
    template: `
        <div>
            <log-text-box-form *ngIf='dataInit' [fields]='[field]' [DataContext]='DataContext' [dir]="'ltr'"> 
                <ng-container end>
                    <div class='form-data-field-field' [ngClass]='{"is-array": typeIsArray}'>
                        <LogLabel [DataContext]="DataContext" [Text]="'Value ' + valueNumber" [LayoutDirection]="'ltr'"></LogLabel>
                        <app-field-by-type #value [type]='field.value' [dir]='"ltr"' [name]='"Value" + valueNumber' [DataContext]='DataContext'></app-field-by-type>
                    </div>
                </ng-container>
            </log-text-box-form>
        </div>
    `,
    styles: [`
        :host ::ng-deep .is-array app-field-by-type .values-container {
            overflow-y: auto;
            overflow-x: hidden;
            max-width: initial;
            max-height: 90px;
        }

        :host ::ng-deep .is-array app-field-by-type LogTextBox,
        :host ::ng-deep .is-array app-field-by-type LogDatePicker,
        :host ::ng-deep .is-array app-field-by-type .InputDiv,
        :host ::ng-deep .is-array app-field-by-type .DatePickerInputDiv,
        :host ::ng-deep .is-array app-field-by-type select {
            width: 260px !important;
        }
    `],
})
export class DefaultAndConfigurationValueTabComponent {
    @ViewChild('value') value!: FieldByTypeComponent;
    dataInit: boolean = false;
    valueNumber: number = 0;
    DataContext = { UIProperties: new UIProperties() };
    typeIsArray: boolean = false;
    field: TextBoxField = { name: '', label: 'Set Value Type', type: 'text', value: 'System.String', disabled: true };

    constructor(private readonly cd: ChangeDetectorRef) { }

    SetTabArgs({ EntityPM }) {
        this.DataContext = EntityPM.dataContext;
        this.valueNumber = EntityPM.valueNumber;
        this.field.name = 'SetValueType' + this.valueNumber;
        EntityPM.$setKeyChange.subscribe((value) => this.setKeyChange(value));
        this.dataInit = true;
        this.cd.detectChanges();

        EntityPM.forms['value' + this.valueNumber] = this.value;
    }

    setKeyChange(valueChange: ValueChange) {
        if(valueChange.field === 'SetKey') {
            this.field.value = valueChange.value['SetType' + this.valueNumber];
            this.typeIsArray = this.field.value.endsWith('[]');            

        } else if(valueChange.field === '') {
            this.field.value = valueChange.value[this.field.name];
            this.typeIsArray = this.field.value.endsWith('[]');
            
            if(this.field.value === 'System.Boolean')
                valueChange.value['Value' + this.valueNumber] = valueChange.value['Value' + this.valueNumber].toLowerCase() === 'true';

            this.DataContext = valueChange.value;
        }
    }
}