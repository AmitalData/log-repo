import { ChangeDetectorRef, Component, EventEmitter, Input, Output, SimpleChanges } from "@angular/core";
import { UIProperties } from "Infrastructure/Components/LogitudeComponents/UIProperties";
import { FieldType } from "./LogTexBoxFormComponent";
import { AmitalAPIAddWindowService } from "../WindowsComponent/AmitalAPIAddWindowService";

@Component({
    selector: 'wrapper-log-field',
    template: `
        <ng-container *ngIf='show'>
        <ng-container [ngSwitch]='_type'>
            <div *ngSwitchCase='"boolean"'>
                <CheckBox [IsChecked]="_DataContext[name]" (Checked)="_DataContext[name] = $event; change.emit($event)" [ngClass]='{"error": error}'></CheckBox>            
            </div>

            <div *ngSwitchCase='"selectCustom"'>
                <select #selectedData (change)='_DataContext[name] = selectedData.value; change.emit(selectedData.value)' [value]='_DataContext[name]' [ngClass]='{"error": error}' >
                    <option *ngFor='let x of values'>{{x}}</option>
                </select>
            </div>
            
            <LogDatePicker *ngSwitchCase='"date"' [ObjectFieldName]="name" [DataContext]="_DataContext" [SelectedDateValue]='_DataContext[name]' [ForceSubscribe]='true' (ValueChanged)='change.emit(_DataContext[name])'></LogDatePicker>

            <LogTextBox *ngSwitchDefault [InputType]='_type || "text"' [DataContext]="_DataContext" [ObjectFieldName]='name' [dir]="dir" (changed)='change.emit(_DataContext[name])'></LogTextBox>
        </ng-container>    
    `,
    styleUrls: ['../fields.scss'],
    styles: [``],
})
export class WrapperLogFieldComponent {
    amitalAPIAddWindowService: AmitalAPIAddWindowService = new AmitalAPIAddWindowService();
    show: boolean = true;
    _type!: FieldType;
    _DataContext: any = { UIProperties: new UIProperties() };
    @Input() set DataContext(d: any) {  
        this._DataContext = d || { UIProperties: new UIProperties() };
    }
    @Input() dir: string = 'rtl';
    @Input() name: string = 'aa_why_you_dont_fill_value_name';
    @Input() set type (t: FieldType) {
        if(this._type)
            this._DataContext[this.name] = null;

        this._type = t || 'text';
        this.refreshTextbox();
    };
    @Input() values?: any[];
    @Input() error?: boolean;
    @Output() change: EventEmitter<any> = new EventEmitter<any>();

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

    public get value(): any {
        return this.type === 'boolean' ? !!this._DataContext[this.name] : this._DataContext[this.name];
    }

    public get valid(): boolean {
        return this.amitalAPIAddWindowService.chekFormValidation([{name: this.name, error: this.error, label: ''}], this._DataContext);
    }
}