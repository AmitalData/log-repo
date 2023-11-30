import {Component, Output, EventEmitter, ChangeDetectionStrategy,OnInit} from '@angular/core';
import {FontTool} from '../../Infrastructure/Tools';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {IdGeneratorPipe} from '../Pipes/IdGeneratorPipe';
@Component({
    selector: "RadioButton",
    inputs: ['IsChecked', 'IsEnabled', 'Text', 'Top', 'Name', 'IsGreenText','IsComboBoxWithCheck', 'DataCy'],
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
            <td style="width: 16px; min-width: 16px;">
                <div class="LogitudeRadioButton">
                    <input [attr.data-cy]="DataCy" [attr.id]="ControlId" type="radio" [attr.name]="Name" [disabled]="!IsEnabled" [checked]="IsChecked" (click)="OnClick()" />
                    <label  [attr.id]="ControlId2" [attr.for]="ControlId"></label>
                </div>
            </td>

            <td  *ngIf="Text" style="width:3px;min-width:3px">
                <div></div>
            </td>

  <td  *ngIf="IsComboBoxWithCheck" style="width:10px;min-width:10px">
                <div></div>
            </td>



            <td class="Label" *ngIf="Text" style="width: 1px;" [ngStyle]="{'color': TextColor}">{{Text}}</td>

            <td>
                <div></div>
            </td>
        </tr>
    </table>
    `,

    styles:
    [`
    .LogitudeRadioButton input:focus + label {
        border: 1px solid #3BB3E2;
    }

    .LogitudeRadioButton input:disabled + label {
        opacity: 0.5;
        pointer-events: none;    
    }

    .LogitudeRadioButton input:checked + label {
        background-image: url('./Images/RadioButtonChecked.png');
        background-repeat: no-repeat;
        background-position: 3px 3px;
        background-color: white;
        background-size: 8.2px 8px;
        cursor: default;
    }

    .LogitudeRadioButton label {
        border-radius: 10px;
        -webkit-border-radius: 10px;
        -moz-border-radius: 10px;
    }

    .LogitudeRadioButton label {
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
        box-sizing: border-box;
        -moz-box-sizing: border-box;
        -webkit-box-sizing: border-box;
        -moz-box-shadow: inset 0 0 3px #AAAAAA;
        -webkit-box-shadow: inset 0 0 3px #AAAAAA;
        box-shadow: inset 0 0 3px #AAAAAA;
        line-height: 15px;
        text-indent: 20px;
        font-size: 11px;
        color: #6E7172;
        position: absolute;
    }

    .LogitudeRadioButton input {
        opacity:0;
        display: none;
        position: absolute;
    }

    .LogitudeRadioButton {
        position: relative;
        display: block;
        height: 16px;
    }

    `],
})

export class RadioButton implements OnInit {
    public ControlId: string = null;
    public ControlId2: string = null;



    
    public Top: number = null;
    public TextColor: string = FontTool.Gray;
    public IsComboBoxWithCheck: boolean = false;
    public DataCy: string;
    @Output() Checked: EventEmitter<boolean> = new EventEmitter<boolean>();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

    }

    ngOnInit() {
        var pipe: IdGeneratorPipe = new IdGeneratorPipe();

        this.ControlId = pipe.transform(this.Text + "_" + this.Name);
        this.ControlId2 = this.ControlId + "_LBL";
        this.Name += this.CurrentSession.GetNewId("RadioButton");
    }

    private name: string;
    get Name() { return this.name; }
    set Name(value: string) {
        if (this.name != value) {
            this.name = value;
        }
    }

    private isChecked: boolean = false;
    get IsChecked() { return this.isChecked; }
    set IsChecked(newValue: boolean) {
        if (this.isChecked != newValue) {
            this.isChecked = newValue;
            this.UpdateTextColor();
        }
    }

    private isGreenText: boolean = false;
    get IsGreenText() { return this.isGreenText; }
    set IsGreenText(newValue: boolean) {
        if (this.isGreenText != newValue) {
            this.isGreenText = newValue;
            this.UpdateTextColor();
        }
    }

    UpdateTextColor() {
        if (this.IsChecked && this.IsGreenText) {
            this.TextColor = FontTool.Green;
        }

        else {
            this.TextColor = FontTool.Gray;
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
        if (!this.IsChecked) {
            this.IsChecked = true;
            this.Checked.emit(this.IsChecked);
        }
    }
}
