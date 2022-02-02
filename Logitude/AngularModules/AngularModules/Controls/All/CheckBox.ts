
import {Component, Output, EventEmitter, OnInit, AfterViewInit, ChangeDetectorRef, ChangeDetectionStrategy} from '@angular/core';
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';
import { IdGeneratorPipe } from '../Pipes/IdGeneratorPipe';



@Component({
    selector: "CheckBox",
    inputs: ['IsChecked', 'IsEnabled', 'Text', 'Top', 'ZIndex', 'DataCy'],
    changeDetection: ChangeDetectionStrategy.OnPush,

    template:
    `
    <table>
        <tr [style.height.px]="Top" *ngIf="Top">
            <td>
                <div></div>
            </td>
        </tr>

        <tr>
            <td style="width: 16px; min-width: 16px; padding:0 !important;">
                <div [attr.data-cy]="DataCy" class="LogitudeCheckBox" [style.zIndex]="ZIndex">
                    <input  [attr.id]="ControlId" type="checkbox" [disabled]="!IsEnabled" [checked]="IsChecked" (click)="OnClick()" (blur)="OnLostFocus()" />
                    <label [attr.id]="ControlId2" [attr.for]="ControlId"></label>
                </div>
            </td>

            <td class="Label" *ngIf="Text" style="width: 1px; padding-left: 3px; padding-right: 3px; font-size: 11px !important; color: #6E7172 !important;">{{Text}}</td>

            <td>
                <div></div>
            </td>
        </tr>
    </table>
    `,

    styles:
    [`
    .LogitudeCheckBox input:focus + label {
        border: 1px solid #3BB3E2;
    }
    .LogitudeCheckBox input:disabled + label {
        opacity: 0.5;
        pointer-events: none;
    }
    .LogitudeCheckBox input:checked + label {
        background: url('./Images/CheckBoxIcon.png') center center no-repeat white;
    }
    .LogitudeCheckBox label {
        border-radius: 2px;
        -webkit-border-radius: 2px;
        -moz-border-radius: 2px;
    }
    .LogitudeCheckBox label {
        width: 16px;
        height: 16px;
        cursor: pointer;
        display: block;
        background: white;
        border: 1px solid #AAAAAA;
        user-select: none;
        -ms-user-select: none;
        -moz-user-select: none;
        -webkit-user-select: none;
        -moz-box-shadow: inset 0 0 3px #AAAAAA;
        -webkit-box-shadow: inset 0 0 3px #AAAAAA;
        box-shadow: inset 0 0 3px #AAAAAA;
        line-height: 15px;
        text-indent: 20px;
        font-size: 11px !important;
        color: #6E7172 !important;
        position: absolute;
    }
    .LogitudeCheckBox input {    
        opacity: 0;
        display: none;
        position: absolute;
    }
    .LogitudeCheckBox {
        position: relative;
        display: block;
        height: 16px;
    }
    `],
})

export class CheckBox{
    public ControlId: string = null;
    public ControlId2: string = null;
    public DataCy: string = null;

    public Top: number = null;
    public ZIndex: number = 0;
    @Output() Checked: EventEmitter<boolean> = new EventEmitter<boolean>();
    @Output() LostFocus: EventEmitter<boolean> = new EventEmitter<boolean>();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        var pipe: IdGeneratorPipe = new IdGeneratorPipe();
        this.ControlId = pipe.transform("CheckBox_" + this.CurrentSession.GetNewId("CheckBox"));
        this.ControlId2 = this.ControlId + "_LBL";
    }

    private isChecked: boolean = false;
    get IsChecked() { return this.isChecked; }
    set IsChecked(newValue: boolean) {
        if (this.isChecked != newValue) {
            this.isChecked = newValue;
        }
    }

    private isEnabled: boolean = true;
    get IsEnabled() { return this.isEnabled; }
    set IsEnabled(newValue: boolean) {
        if (this.isEnabled != newValue) {
            this.isEnabled = newValue;
        }
    }

    private text: string;
    get Text() { return this.text; }
    set Text(newValue: string) {
        if (this.text != newValue) {
            this.text = newValue;
        }
    }

    OnClick() {
        this.IsChecked = !this.IsChecked;
        this.Checked.emit(this.IsChecked);
    }

    OnLostFocus() {
        this.LostFocus.emit(true);
    }
}
