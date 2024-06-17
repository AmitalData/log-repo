import { ChangeDetectorRef, Component, EventEmitter, Input, Output } from "@angular/core";
import { UIProperties } from "Infrastructure/Components/LogitudeComponents/UIProperties";
import { FieldType } from "./LogTexBoxFormComponent";
import { AmitalAPIAddWindowService } from "../WindowsComponent/AmitalAPIAddWindowService";

@Component({
    selector: 'wrapper-log-field',
    template: `
        <ng-container *ngIf='show'>
        <ng-container [ngSwitch]='_type'>
            <div *ngSwitchCase='"boolean"'>
                <CheckBox [IsChecked]="DataContext[name]" (Checked)="DataContext[name] = $event; change.emit($event)" [ngClass]='{"error": error}'></CheckBox>            
            </div>

            <div *ngSwitchCase='"selectCustom"'>
                <select #selectedData (change)='DataContext[name] = selectedData.value; change.emit(selectedData.value)' [value]='DataContext[name]' [ngClass]='{"error": error}' >
                    <option *ngFor='let x of values'>{{x}}</option>
                </select>
            </div>
            
            <LogDatePicker *ngSwitchCase='"date"' [ObjectFieldName]="name" [DataContext]="DataContext" [SelectedDateValue]='DataContext[name]' [ForceSubscribe]='true' (ValueChanged)='change.emit(DataContext[name])'></LogDatePicker>

            <LogTextBox *ngSwitchDefault [InputType]='_type || "text"' [DataContext]="DataContext" [ObjectFieldName]='name' [dir]="dir" (changed)='change.emit(DataContext[name])'></LogTextBox>
        </ng-container>    
    `,
    styleUrls: ['../fields.scss'],
    styles: [``],
})
export class WrapperLogFieldComponent {
    amitalAPIAddWindowService: AmitalAPIAddWindowService = new AmitalAPIAddWindowService();
    show: boolean = true;
    _type!: FieldType;
    @Input() DataContext?: any = { UIProperties: new UIProperties() };
    @Input() dir: string = 'rtl';
    @Input() name: string = 'izikf';
    @Input() set type (t: FieldType) {
        if(this._type)
            this.DataContext[this.name] = null;

        this._type = t || 'text';
        this.show = false;
        this.cd.detectChanges();
        this.show = true;
        this.cd.detectChanges();
    };
    @Input() values?: any[];
    @Input() error?: boolean;
    @Output() change: EventEmitter<any> = new EventEmitter<any>();

    constructor(private cd: ChangeDetectorRef) {}

    public get value(): any {
        return this.type === 'boolean' ? !!this.DataContext[this.name] : this.DataContext[this.name];
    }

    public get valid(): boolean {
        return this.amitalAPIAddWindowService.chekFormValidation([{name: this.name, error: this.error, label: ''}], this.DataContext);
    }
}