import { ChangeDetectorRef, Component, EventEmitter, Input, Output } from "@angular/core";
import { FieldData } from "../amitalApiTypes";
import { UIProperties } from "Infrastructure/Components/LogitudeComponents/UIProperties";
import { AmitalAPIAddWindowService } from "../WindowsComponent/AmitalAPIAddWindowService";

@Component({
    selector: 'log-text-box-form',
    template: `
        <div class='form-data-field' [style.direction]='dir' [style.justifyContent]="'flex-' + (dir === 'rtl' ? 'end' : 'start')">
            <ng-content></ng-content>

            <ng-container *ngIf='showFields'>
                <div *ngFor='let field of _fields' class='form-data-field-field'>
                    <LogLabel [DataContext]="DataContext" [Text]="field.label" [LayoutDirection]="dir"></LogLabel>
                    <wrapper-log-field [DataContext]='DataContext' [dir]='dir' [name]='field.name' [type]='field.type' [error]='field.error' [values]='field.values' [disabled]='field.disabled' [params]='field.params' [value]='field.value' 
                    (change)='valueChange.emit({field: field.name, value: DataContext[field.name]})' (changeEvent)='changeEvent.emit({field: field.name, value: $event})'></wrapper-log-field>
                </div>
            </ng-container>

            <ng-content select='[end]'></ng-content>
        </div>
    `,
    styleUrls: ['../fields.scss'],
    styles: [``],
})
export class LogTexBoxFormComponent {
    amitalAPIAddWindowService: AmitalAPIAddWindowService = new AmitalAPIAddWindowService();
    showFields: boolean = true;
    @Input() set fields(f: TextBoxField[])  {
        this._fields = f;
        f.filter(field => field != undefined).forEach(field => this.DataContext[field.name] = field.value);
    };
    @Input() DataContext?: any = { UIProperties: new UIProperties() };
    @Input() dir: string = 'rtl';
    @Output() valueChange: EventEmitter<ValueChange> = new EventEmitter<ValueChange>();
    @Output() changeEvent: EventEmitter<ValueChange> = new EventEmitter<ValueChange>();
    _fields: TextBoxField[] = [];    

    public Set(field: string, value: any) {
        this._fields.find(f => f.name === field).value = value;
    }

    public get values(): any {
        return this._fields.reduce((acc, field) => {
            acc[field.name] = field.type === 'boolean' ? !!this.DataContext[field.name] : this.DataContext[field.name];
            return acc;
        }, {});    
    }

    public get valid(): boolean {
        const fields = this._fields.filter(field => (field.required && !this.DataContext[field.name]) || field.error);
        return this.amitalAPIAddWindowService.chekFormValidation(fields, this.DataContext);
    }

    constructor(private cd: ChangeDetectorRef) {}
}

export type ValueChange = { field: string, value: any };
export type FieldType = 'boolean' | 'selectCustom' | 'date' | 'number' | 'text' | 'logLov';

export type TextBoxField = FieldData & {
    type?: FieldType;
    values?: any[];
    error?: boolean;
    required?: boolean;
    value?: any;
    params?: any;
    disabled?: boolean;
};