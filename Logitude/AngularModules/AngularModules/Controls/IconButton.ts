import {Component, OnInit, ChangeDetectionStrategy} from '@angular/core';
import {ObjectsLocator} from '../Infrastructure/Locators/ObjectsLocator';

@Component({
    selector: 'IconButton',
    inputs: ['Name', 'IsSmall', 'Width', 'Height', 'IsEnabled', 'disabled', 'IsIconOnly', 'TopIndent', 'Left', 'Top', 'Title', 'ExternalId', 'DataCy'],    
    changeDetection: ChangeDetectionStrategy.OnPush,
    template:
    `
    <button [attr.data-cy]="DataCy" id="{{ Name+ExternalId | IdGeneratorPipe}}"   title="{{Title}}" *ngIf="!IsIconOnly" 
            class="LogitudeIconButton" 
            [style.width.px]="Width" 
            [style.height.px]="Height" 
            [style.margin-left.px]="Left" 
            [disabled]="!IsEnabled" 
            [ngStyle]="{top: Top}"
            (mouseenter)="OnMouseEnter()" 
            (mouseleave)="OnMouseLeave()" 
            tabindex="-1">

                <img [class.FlipImgHoriz]="LayoutDirection == 'rtl'" 
                    [attr.src]="Source" 
                    style="visibility:inherit;" 
                    [style.width]="Name=='return' ? '13px' : 'initial'"
                    [ngStyle]="LayoutDirection == 'rtl' ? {top: TopIndent,'right': LeftIndent , 'left' : 0} : {top: TopIndent,'left': LeftIndent, 'right':0}" />

    </button>

    <div *ngIf="IsIconOnly"  title="{{Title}}"
            style="width: 20px; height: 20px; cursor:pointer; position: relative;" 
            (mouseenter)="OnMouseEnter()" 
            (mouseleave)="OnMouseLeave()">

                <img [class.FlipImgHoriz]="LayoutDirection == 'rtl'"
                    [attr.src]="Source" 
                    [style.width]="Name=='return' ? '13px' : 'initial'"
                    style="visibility:inherit; vertical-align: middle; position: absolute; top:0; bottom:0; right:0; margin: auto; transform:none;" />

    </div>

    `,

    styles:
    [`

    .LogitudeIconButton img {
        position: absolute;
        top: 1px;
        bottom: 0;
        margin: auto;
    }

    .LogitudeIconButton:focus:not(:disabled) {
        border: 1px solid #3BB3E2;
        -webkit-box-shadow: 0px 0px 6px 0px #3BB3E2;
        -moz-box-shadow: 0px 0px 6px 0px #3BB3E2;
        box-shadow: 0px 0px 6px 0px #3BB3E2;
    }
    .LogitudeIconButton:hover:not(:disabled) {
        cursor: pointer;
        background: -moz-linear-gradient(50% 0% -90deg,rgba(255, 255, 255, 1) 0%,rgba(221, 232, 245, 1) 100%);
        background: -webkit-linear-gradient(-90deg, rgba(255, 255, 255, 1) 0%, rgba(221, 232, 245, 1) 100%);
        background: -webkit-gradient(linear,50% 0%,50% 100%,color-stop(0,rgba(255, 255, 255, 1) ),color-stop(1,rgba(221, 232, 245, 1) ));
        background: -o-linear-gradient(-90deg, rgba(255, 255, 255, 1) 0%, rgba(221, 232, 245, 1) 100%);
        background: linear-gradient(180deg, rgba(255, 255, 255, 1) 0%, rgba(221, 232, 245, 1) 100%);
    }

    .LogitudeIconButton:disabled {
        opacity: 0.5;
        pointer-events: none !important; 
        cursor: default !important;;
    }

    .LogitudeIconButton {
        display: block !important;
        float: left !important;
        width: 21px;
        height: 21px;
        outline: none;
        border: 1px solid #6A8299;
        border-radius: 3px;
        -moz-border-radius: 3px;
        -webkit-border-radius: 3px;
        position: relative;     
        background: -moz-linear-gradient(50% 0% -90deg,rgba(255, 255, 255, 1) 0%,rgba(186, 206, 227, 1) 100%);
        background: -webkit-linear-gradient(-90deg, rgba(255, 255, 255, 1) 0%, rgba(186, 206, 227, 1) 100%);
        background: -webkit-gradient(linear,50% 0%,50% 100%,color-stop(0,rgba(255, 255, 255, 1) ),color-stop(1,rgba(186, 206, 227, 1) ));
        background: -o-linear-gradient(-90deg, rgba(255, 255, 255, 1) 0%, rgba(186, 206, 227, 1) 100%);
        background: linear-gradient(180deg, rgba(255, 255, 255, 1) 0%, rgba(186, 206, 227, 1) 100%);
    }
    .FlipImgHoriz{
        -moz-transform: scaleX(-1);
        -o-transform: scaleX(-1);
        -webkit-transform: scaleX(-1);
        transform: scaleX(-1);
        filter: FlipH;
        -ms-filter: "FlipH";
    }
    `],
    
})

export class IconButton implements OnInit {
    public Name: string;
    public Width: number;
    public Height: number;
    public Left: number = 0;
    public Top: string = "0px";
    public Source: string;
    public IsSmall: boolean;
    public IsIconOnly: boolean = false;
    public LeftIndent: string = "1px";
    private mySource: string;
    private mySourceOver: string;
    public TopIndent: string = "0px";
    LayoutDirection: string = 'ltr';
    public Title: string;
    public ExternalId: string = "";
    constructor() {

        this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
    }

    ngOnInit() {

        if (this.Name != null) {

            if (this.Width == null || this.Width === undefined || this.Width == 0) {
                this.Width = this.IsSmall ? 18 : 21;
            }

            if (this.Height == null || this.Height === undefined || this.Height == 0) {
                this.Height = this.IsSmall ? 18 : 21;
            }

            if (this.IsSmall) {
                this.LeftIndent = "2px";
            }

            switch (this.Name.toLowerCase()) {
                case "settings": {
                    this.mySource = './Images/Icons/Settings.png';
                    this.mySourceOver = './Images/Icons/Settings_Blue.png';
                    break;
                }

                case "actions": {
                    this.mySource = './Images/Icons/ActionsGray.png';
                    this.mySourceOver = './Images/Icons/Actions.png';
                    break;
                }

                case "payments": {
                    this.mySource = './Images/Icons/payments.png';
                    this.mySourceOver = './Images/Icons/payments_Blue.png';
                    break;
                }

                case "refresh": {
                    this.mySource = './Images/Buttons/Refresh.png';
                    break;
                }

                case "add": {
                    this.mySource = "./Images/Buttons/Add.png";
                    this.mySourceOver = "./Images/Buttons/Add.over.png";
                    break;
                }

                case "edit": {
                    this.mySource = "./Images/Buttons/Edit.png";
                    this.mySourceOver = "./Images/Buttons/Edit.over.png";
                    break;
                }
                case "editorange": {
                    this.mySource = "./Images/Buttons/Edit.over.png";
                    this.mySourceOver = "./Images/Buttons/Edit.png";
                    break;
                }
                case "additional": {
                    this.mySource = "./Images/ThickTick.png";
                    this.mySourceOver = "./Images/ThickTick.png";
                    break;
                }

                case "delete": {
                    this.mySource = "./Images/Buttons/Delete.png";
                    this.mySourceOver = "./Images/Buttons/Delete.over.png";
                    break;
                }

                case "connect": {
                    this.mySource = "./Images/Buttons/Connect.png";
                    this.mySourceOver = "./Images/Buttons/Connect.png";
                    break;
                }

                case "disconnect": {
                    this.mySource = "./Images/Buttons/Disconnect.png";
                    this.mySourceOver = "./Images/Buttons/Disconnect.png";
                    break;
                }

                case 'help': {
                    this.mySource = "./Images/Icons/Help.png";
                    this.mySourceOver = "./Images/Icons/Help_Blue.png";
                    break;
                }

                case 'signout': {
                    this.mySource = "./Images/Icons/Signout.png";
                    this.mySourceOver = "./Images/Icons/Signout_Blue.png";
                    break;
                }

                case "call": {
                    this.mySource = "./Images/Buttons/Call.png";
                    break;
                }

                case "task": {
                    this.mySource = "./Images/Buttons/Task.png";
                    break;
                }

                case "appoint": {
                    this.mySource = "./Images/Buttons/Appointment.png";
                    break;
                }

                case "email": {
                    this.mySource = "./Images/Buttons/Email.png";
                    break;
                }

                case "excel": {
                    this.mySource = "./Images/Buttons/excelicon.png";
                    break;
                }

                case "watch": {
                    this.mySource = "./Images/Watch.png";
                    break;
                }

                case "disabledwatch": {
                    this.mySource = "./Images/DisabledWatch.png";
                    break;
                }

                case "bell": {
                    this.mySource = "./Images/Bell.png";
                    break;
                }
                       
                case "deletefollowup": {
                    this.mySource = "./_Resources/Images/Icons/Followups/DeleteFollowup.png";
                    this.mySourceOver = "./_Resources/Images/Icons/Followups/DeleteFollowup_Red.png";
                    break;
                }

                case "donefollowup": {
                    this.mySource = "./_Resources/Images/Icons/Followups/DoneButton.png";
                    this.mySourceOver = "./_Resources/Images/Icons/Followups/DoneButton_Green.png";
                    break;
                }

                case "search": {
                    this.mySource = "./Images/LOVSearch.png";  
                    this.LeftIndent = "0px";                
                    break;
                }
                case "copy": {
                    this.mySource = "./Images/Buttons/copy.png";
                    break;
                }
                case "return": {
                    this.mySource = "./Images/return.png";
                    break;
                }


            }

            this.Source = this.mySource;

            if (this.mySourceOver == null) {
                this.mySourceOver = this.mySource;
            }
        }
    }

    private isdisabled: boolean = false;
    get disabled() { return this.isdisabled }
    set disabled(value: boolean) {
        if (this.isdisabled != value) {
            this.isdisabled = value;
            this.IsEnabled = !value;
        }
    }

    private isEnabled: boolean = true;
    get IsEnabled() { return this.isEnabled; }
    set IsEnabled(value: boolean) {
        if (this.isEnabled != value) {
            this.isEnabled = value;
        }
    }

    OnMouseEnter() {
        this.Source = this.mySourceOver;
    }

    OnMouseLeave() {
        this.Source = this.mySource;
    }
}
