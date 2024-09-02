import { ChangeDetectorRef, Component, EventEmitter, Input, Output, SimpleChanges } from "@angular/core";
import { UIProperties } from "Infrastructure/Components/LogitudeComponents/UIProperties";
import { FieldType } from "./LogTexBoxFormComponent";
import { AmitalAPIAddWindowService } from "../WindowsComponent/AmitalAPIAddWindowService";

@Component({
    selector: 'wrapper-log-field',
    template: `        
        <div *ngIf='show' [ngSwitch]='_type' [ngClass]='{"disabled": _disabled}'>
            <div *ngSwitchCase='"boolean"'>
                <CheckBox [IsEnabled]='!_disabled' [IsChecked]="_DataContext[name]" (Checked)="_DataContext[name] = $event; change.emit($event)" [ngClass]='{"error": error}'></CheckBox>            
            </div>

            <div *ngSwitchCase='"selectCustom"'>
                <select [disabled]='_disabled' #selectedData (change)='_DataContext[name] = selectedData.value; change.emit(selectedData.value)' [value]='_DataContext[name]' [ngClass]='{"error": error}' >
                    <option *ngFor='let x of values'>{{x}}</option>
                </select>
            </div>
            
            <LogDatePicker [IsDisabled]='_disabled' *ngSwitchCase='"date"' [ObjectFieldName]="name" [DataContext]="_DataContext" [SelectedDateValue]='date' [ForceSubscribe]='true' (ValueChanged)='change.emit(_DataContext[name])'></LogDatePicker>
            
            <LogLov *ngSwitchCase='"logLov"' [IsDisabled]='_disabled' [SelectedValue]='_DataContext[name]' [ObjectFieldName]="name" [DataContext]="_DataContext" 
            [LookUpTableName]="params?.LookUpTableName" [HideColumns]="true" (SelectedItemChanged)='changeEvent.emit($event)' (ValueChanged)='change.emit(_DataContext[name])'></LogLov>

            <LogTextBox *ngSwitchDefault [InputType]='_type || "text"' [DataContext]="_DataContext" [ObjectFieldName]='name' [dir]="dir" (changed)='change.emit(_DataContext[name])'></LogTextBox>
        </div>    
    `,
    styleUrls: ['../fields.scss'],
    styles: [`
            .disabled {
                pointer-events: none;
                opacity: 0.5 !important;
            }
        `],
})
export class WrapperLogFieldComponent {
    amitalAPIAddWindowService: AmitalAPIAddWindowService = new AmitalAPIAddWindowService();
    show: boolean = true;
    date!: Date;
    _disabled: boolean = false;
    @Input() set disabled(v: boolean) {
        (this._DataContext.UIProperties as UIProperties).SetEnabled(this.name, null, !v);
        this._disabled = !!v;
    }
    @Input() dir: string = 'rtl';
    @Input() name: string = 'why_you_dont_fill_value_name';
    @Input() params: any = {};
    _type!: FieldType;
    @Input() set type (t: FieldType) {
        if(this._type)
            this._DataContext[this.name] = null;
        
        this._type = t || 'text';
    };
    _DataContext: any = { UIProperties: new UIProperties() };
    @Input() set DataContext(d: any) {  
        this._DataContext = d || { UIProperties: new UIProperties() };        
        this.refreshTextbox();
    }
    @Input() values?: any[];
    @Input() error?: boolean;
    @Input() set value (val: any) {
        this._DataContext[this.name] = val;
        this.refreshTextbox();
    }
    @Output() change: EventEmitter<any> = new EventEmitter<any>();
    @Output() changeEvent: EventEmitter<any> = new EventEmitter<any>();

    private refreshTextbox() {
        this.show = false;
        this.cd.detectChanges();
        this.show = true;
        this.cd.detectChanges();
    }

    ngOnChanges(changes: SimpleChanges) {
        if(changes.DataContext) 
            this.refreshTextbox();
    }

    constructor(private cd: ChangeDetectorRef) {}

    public get Value(): any {
        return this.type === 'boolean' ? ('' + this._DataContext[this.name]).toLowerCase() === 'true' : this._DataContext[this.name];
    }

    public get valid(): boolean {
        return this.amitalAPIAddWindowService.chekFormValidation([{name: this.name, error: this.error, label: '', type: this._type}], this._DataContext);
    }
}