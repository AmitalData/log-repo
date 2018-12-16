/// <reference path="../../infrastructure/utilities/amitalgatewayutil.ts" />
import {Component, ChangeDetectionStrategy} from '@angular/core';
import {AmitalGatewayUtil} from '../../Infrastructure/Utilities/AmitalGatewayUtil';

@Component({
    selector: 'BusyIndicator',
    inputs: ['Text', 'IsBusy', 'Width', 'Height', 'ImageWidth', 'ImageHeight'],
    ///changeDetection: ChangeDetectionStrategy.OnPush,

    //border: 0;height: 10px;border-radius: 5px;

    template:
    `
    <div [hidden]="!IsBusy" class="BusyIndicatorControlLayout" tabindex="-1" contenteditable="false"></div>
    <div [hidden]="!IsBusy" class="BusyIndicatorControl" Id="BusyIndecator">
        <div class="BusyIndicatorControlOuter" [style.width.px]="Width" [style.height.px]="Height">
            <div class="BusyIndicatorControlInner" [style.width.px]="Width" [style.height.px]="Height">
                <div style="margin: auto; margin-top: 20px;" [style.width.px]="ImageWidth" [style.height.px]="ImageHeight">
                    <img *ngIf="!IsAmitalVer" src="./Images/BusyIndicator.gif" alt="Loading..." [style.width.px]="ImageWidth" [style.height.px]="ImageHeight"/> 
                    <progress  *ngIf="IsAmitalVer" value="{{progressValue}}" max="100" style="margin-right: -35px;" ></progress>
                      

                </div>

                <div style="margin-top: 15px; height: 20px; width: 100%; text-align: center;">
                    <label>{{Text}}</label>
                </div>
            </div>
        </div>
    </div>
    `,

    styles:
    [`
    .BusyIndicatorControlLayout {
        position: absolute;
        top: 0;
        bottom: 0;
        left: 0;
        right: 0;
        margin: auto;
        opacity: 0.5;
        background: white;
        z-index: 99;
    }

    .BusyIndicatorControl {
        position: absolute;
        top: 0;
        bottom: 0;
        left: 0;
        right: 0;
        margin: auto;
        z-index: 100;
    }

    .BusyIndicatorControlOuter {
        border: 1px solid #DADADA;
        border-radius: 2px;
        -moz-border-radius: 2px;
        -webkit-border-radius: 2px;
        background: white;
        position: absolute;
        top: 0;
        bottom: 0;
        left: 0;
        right: 0;
        margin: auto;
    }

    .BusyIndicatorControlInner {
        border-radius: 2px;
        -moz-border-radius: 2px;
        -webkit-border-radius: 2px;
        background: linear-gradient(rgba(240, 240, 240, 0.5), rgba(198, 198, 198, 0.5));
        position: absolute;
        top: 0;
        bottom: 0;
        left: 0;
        right: 0;
        margin: auto;
    }
    `],
})

export class BusyIndicator {
    public Text: string;
    public Width: number = 200;
    public Height: number = 120;
    public ImageWidth: number = 50;
    public ImageHeight: number = 50;

    
    IsAmitalVer: boolean = false;
    constructor() {
        this.IsAmitalVer = AmitalGatewayUtil.Instance.AmitalBrowserInUse;
        //this.IsAmitalVer = false;
    }

    private isBusy: boolean = false;
    get IsBusy() { return this.isBusy; }
    set IsBusy(value: boolean) {
        if (this.isBusy != value) {
            this.isBusy = value;
        }
        if (!this.IsAmitalVer) {
            return;
        }
        if (this.isBusy) {
            
            this._TimerToken =
                setTimeout(() => {
                    this.AnimateIt();
            }, this._TimeSpan);
        } else {
            clearTimeout(this._TimerToken)
        }
    }
    _TimeSpan: number = 500;
    _TimerToken: any;
    _Delta: number = 20;
    progressValue: number = 0;
    AnimateIt() {
        //if (this.progressValue > 100) {
        //    this._Delta = -10;
        //} else if (this.progressValue < 1) {
        //    this._Delta = 10;
        //}
        if (this.progressValue > 100) {
            this.progressValue= 0;
        } 
        this.progressValue = this.progressValue + this._Delta;
        if (this.isBusy) {
            this._TimerToken =
                setTimeout(() => {
                    this.AnimateIt();
            }, this._TimeSpan);
        }
    }


    
    
   
}
