import { Component, Input } from "@angular/core";
import { FieldData } from "../amitalApiTypes";
import { UIProperties } from "Infrastructure/Components/LogitudeComponents/UIProperties";

@Component({
    selector: 'log-text-box-form',
    template: `
        <div class='form-data-field'>
            <ng-content></ng-content>
            
            <div *ngFor='let field of fields' class='form-data-field-field'>
                <LogLabel [DataContext]="DataContext" [Text]="field.label" [LayoutDirection]="dir"></LogLabel>

                <ng-container [ngSwitch]='field.type'>
                    <div *ngSwitchCase='"boolean"'>
                        <select #selectedData (change)='DataContext[field.name] = selectedData.value' [value]='DataContext[field.name]' [ngClass]='{"error": field.error}' >
                            <option *ngFor='let x of ["true", "false"]'>{{x}}</option>
                        </select>
                    </div>
                    
                    <LogDatePicker *ngSwitchCase='"date"' [ObjectFieldName]="field.name" [DataContext]="DataContext" [SelectedDateValue]='DataContext[field.name]'></LogDatePicker>

                    <LogTextBox *ngSwitchDefault [DataContext]="DataContext" [ObjectFieldName]='field.name' [dir]="dir"></LogTextBox>
                </ng-container>
            </div>
        </div>
    `,
    styleUrls: ['../fields.scss'],
    styles: [``],
})
export class LogTexBoxFormComponent {
    @Input() fields: TextBoxField[] = [];
    @Input() DataContext: any = { UIProperties: new UIProperties() };
    @Input() dir: string = 'rtl';
}

export type TextBoxField = FieldData & {
    type?: 'boolean' | 'date';
    error?: boolean;
};