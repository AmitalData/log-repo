import {Component} from '@angular/core';

@Component({
    selector: 'BusyIndicator',
    inputs: ['Text', 'IsBusy', 'Width', 'Height', 'ImageWidth', 'ImageHeight'],

    template:
    `
    <div   [hidden]="!IsBusy" class="BusyIndicatorControlLayout" tabindex="-1" contenteditable="false"></div>
    <div   [hidden]="!IsBusy" class="BusyIndicatorControl">
        <div class="BusyIndicatorControlOuter" [style.width.px]="Width" [style.height.px]="Height">
            <div class="BusyIndicatorControlInner" [style.width.px]="Width" [style.height.px]="Height">
                <div style="margin: auto; margin-top: 20px;" [style.width.px]="ImageWidth" [style.height.px]="ImageHeight">
                   <div class="lds-ring"><div></div><div></div><div></div><div></div></div>
                <!--    <img src="./assets/images/Gif/BusyIndicator2.gif" alt="Loading..." [style.width.px]="ImageWidth" [style.height.px]="ImageHeight"/>  -->                    
                </div>

                <div style="margin-top: 0px; height: 20px; width: 100%; text-align: center;">
                    <label style="font-weight: bold; color: #222; font-size: 13px;">{{Text}}</label>
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
        z-index: 9999;
    }

    .BusyIndicatorControl {
        position: absolute;
        top: 0;
        bottom: 0;
        left: 0;
        right: 0;
        margin: auto;
        z-index: 10000;
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
        border: none;
        background: none;
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
        background: none;
    }
    .lds-ring {
        display: inline-block;
        position: relative;
        width: 80px;
        height: 80px;
      }
      .lds-ring div {
        box-sizing: border-box;
        display: block;
        position: absolute;
        width: 64px;
        height: 64px;
        margin: 8px;
        border: 8px solid #fff;
        border-radius: 50%;
        animation: lds-ring 1.2s cubic-bezier(0.5, 0, 0.5, 1) infinite;
        border-color: #fff transparent transparent transparent;
      }
      .lds-ring div:nth-child(1) {
        animation-delay: -0.45s;
      }
      .lds-ring div:nth-child(2) {
        animation-delay: -0.3s;
      }
      .lds-ring div:nth-child(3) {
        animation-delay: -0.15s;
      }
      @keyframes lds-ring {
        0% {
          transform: rotate(0deg);
        }
        100% {
          transform: rotate(360deg);
        }
      }
    `],
})

export class BusyIndicator {
    public Text: string;
    public Width: number = 80;
    public Height: number = 80;
    public ImageWidth: number = 80;
    public ImageHeight: number = 80;
    constructor() {

    }

    private isBusy: boolean = false;
    get IsBusy() { return this.isBusy; }
    set IsBusy(value: boolean) {
        if (this.isBusy != value) {
            this.isBusy = value;
        }
    }
}
