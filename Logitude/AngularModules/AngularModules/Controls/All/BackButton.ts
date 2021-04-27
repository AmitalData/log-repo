import {Component, OnInit, ChangeDetectionStrategy} from '@angular/core';
import {ObjectsLocator} from '../../Infrastructure/Locators/ObjectsLocator';
import { AppTool } from '../../Infrastructure/Tools';

@Component({
    selector: 'BackButton',
    inputs: ['Text', 'IsEnabled', 'LayoutDirection'],
    changeDetection: ChangeDetectionStrategy.OnPush,

    template:
    `
    <div id="{{ 'EditBackbutton' | IdGeneratorPipe}}" class="BackBottun" (mouseover)="IsHover = true" (mouseleave)="IsHover = false"
        [ngStyle]="LayoutDirection == 'rtl' ? {'padding-right': '10px'} : {'padding-left': '10px'}">
        <div class="BackBottonBody" [ngStyle]="LayoutDirection == 'rtl' ? {'border-right': 'none'} : {'border-left': 'none'}">{{Text}}</div>

        <div class="BackBottonHead" [className]="LayoutDirection == 'rtl' ? 'BackBottonHead FlipImgHoriz' : 'BackBottonHead'" [ngStyle]="LayoutDirection == 'rtl' ? {'right': '0'} : {'left': '0'}">
            <img src="./Images/Buttons/BackBottonHead.png" style="width: 15px; height: 24px;" [hidden]="IsHover" />
            <img src="./Images/Buttons/BackBottonHeadHover.png" style="width: 15px; height: 24px;" [hidden]="!IsHover" />
        </div>
    </div>
    `,

    styles:
    [`
    .disabled{
        pointer-events: none;
        opacity: 0.7;
        cursor: default;
    }
    .BackBottun:hover .BackBottonBody {
        border-color: #EDC093;
        background: -moz-linear-gradient(50% 0% -90deg,rgba(255, 255, 255, 1) 0%,rgba(237, 192, 147, 1) 100%);
        background: -webkit-linear-gradient(-90deg, rgba(255, 255, 255, 1) 0%, rgba(237, 192, 147, 1) 100%);
        background: -webkit-gradient(linear,50% 0%,50% 100%,color-stop(0,rgba(255, 255, 255, 1) ),color-stop(1,rgba(237, 192, 147, 1) ));
        background: -o-linear-gradient(-90deg, rgba(255, 255, 255, 1) 0%, rgba(237, 192, 147, 1) 100%);
        background: linear-gradient(180deg, rgba(255, 255, 255, 1) 0%, rgba(237, 192, 147, 1) 100%);
        -webkit-box-shadow: 0px 0px 6px 0px #EDC093;
        -moz-box-shadow: 0px 0px 6px 0px #EDC093;
        box-shadow: 0px 0px 6px 0px #EDC093;
        text-shadow: 1px 1px white;
        color: #EE8F55;
    }

    .BackBottonBody {
        height: 24px;
        width: 100%;
        min-width: 40px;
        border: 1px solid #6B8399;
        border-radius: 5px;     
        font-size: 12px;
        font-family: Arial;           
        background: -moz-linear-gradient(50% 0% -90deg,rgba(255, 255, 255, 1) 0%,rgba(236, 192, 147, 1) 100%);
        background: -webkit-linear-gradient(-90deg, rgba(255, 255, 255, 1) 0%, rgba(236, 192, 147, 1) 100%);
        background: -webkit-gradient(linear,50% 0%,50% 100%,color-stop(0,rgba(255, 255, 255, 1) ),color-stop(1,rgba(236, 192, 147, 1) ));
        background: -o-linear-gradient(-90deg, rgba(255, 255, 255, 1) 0%, rgba(236, 192, 147, 1) 100%);
        background: linear-gradient(180deg, rgba(255, 255, 255, 1) 0%, rgba(236, 192, 147, 1) 100%);
        -moz-box-shadow: inset 0 0 3px #AAAAAA;
        -webkit-box-shadow: inset 0 0 3px #AAAAAA;
        box-shadow: inset 0 0 3px #AAAAAA;
        padding-left: 5px;
        padding-right: 5px;
        padding-top: 4px;
    }

    @media all and (-ms-high-contrast: none), (-ms-high-contrast: active) {
        .BackBottonBody {
            padding-top: 4.5px;
        }
    }
    .BackBottonHead {
        height: 24px;
        width: 15px;
        min-width: 15px;
        position: absolute;
        top: 0;
    }

    .BackBottun {
        height: 24px;        
        width: 100%;
        cursor: pointer;
        position: relative;
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

export class BackButton implements OnInit {
    public Text: string = "Back";
    public IsHover: boolean = false;
    LayoutDirection: string = '';

    private isEnabled: boolean = true;
    get IsEnabled() { return this.isEnabled; }
    set IsEnabled(value: boolean) {
        if (this.isEnabled != value) {
            this.isEnabled = value;
        }
    }
    constructor() {
        if (AppTool.IsNullOrEmpty(this.LayoutDirection)) this.LayoutDirection = 'ltr';
        this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
    }

    ngOnInit() {
        if (!this.Text) {
            this.Text = "Back";
        }
    }
}
