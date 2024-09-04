import { Component, Input } from "@angular/core";
import { FieldData } from "../amitalApiTypes";
import { UIProperties } from "Infrastructure/Components/LogitudeComponents/UIProperties";
import { AmitalAPIAddWindowService } from "../WindowsComponent/AmitalAPIAddWindowService";

@Component({
    selector: 'log-text-box-form',
    template: `
        <div class='form-data-field' [style.direction]='dir' [style.justifyContent]="'flex-' + (dir === 'rtl' ? 'end' : 'start')">
            <ng-content></ng-content>
            
            <div *ngFor='let field of fields' class='form-data-field-field'>
                <LogLabel [DataContext]="DataContext" [Text]="field.label" [LayoutDirection]="dir"></LogLabel>

                <ng-container [ngSwitch]='field.type'>
                    <div *ngSwitchCase='"boolean"'>
                        <select #selectedData (change)='DataContext[field.name] = selectedData.value' [value]='DataContext[field.name]' [ngClass]='{"error": field.error}' >
                            <option *ngFor='let x of ["true", "false"]'>{{x}}</option>
                        </select>
                    </div>

                    <div *ngSwitchCase='"selectCustom"'>
                        <select #selectedData (change)='DataContext[field.name] = selectedData.value' [value]='DataContext[field.name]' [ngClass]='{"error": field.error}' >
                            <option *ngFor='let x of field.values'>{{x}}</option>
                        </select>
                    </div>
                    
                    <LogDatePicker *ngSwitchCase='"date"' [ObjectFieldName]="field.name" [DataContext]="DataContext" [SelectedDateValue]='DataContext[field.name]' [ForceSubscribe]='true'></LogDatePicker>

                    <LogTextBox *ngSwitchDefault [InputType]='field.type || "text"' [DataContext]="DataContext" [ObjectFieldName]='field.name' [dir]="dir"></LogTextBox>
                </ng-container>
            </div>
        </div>
    `,
    styleUrls: ['../fields.scss'],
    styles: [``],
})
export class LogTexBoxFormComponent {
    amitalAPIAddWindowService: AmitalAPIAddWindowService = new AmitalAPIAddWindowService();
    @Input() fields: TextBoxField[] = [];
    @Input() DataContext?: any = { UIProperties: new UIProperties() };
    @Input() dir: string = 'rtl';
    
    public get values(): any {
        return this.fields.reduce((acc, field) => {
            acc[field.name] = this.DataContext[field.name];
            return acc;
        }, {});    
    }

    public isValid(): boolean {
        const fields = this.fields.filter(field => (field.required && !this.DataContext[field.name]) || field.error);
        return this.amitalAPIAddWindowService.chekFormValidation(fields, this.DataContext);
    }
}

export type TextBoxField = FieldData & {
    type?: 'boolean' | 'selectCustom' | 'date' | 'integer';
    values?: any[];
    error?: boolean;
    required?: boolean;
    value?: any;
};