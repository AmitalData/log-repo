import {Component, ChangeDetectionStrategy} from '@angular/core';

@Component({
    selector: 'SectionBox',
    inputs: ['Top', 'Bottom', 'Scrolling', 'HeaderHeight', 'Border'],
    changeDetection: ChangeDetectionStrategy.OnPush,

    template:
    `
    <div class="MediaFill">
        <table>
            <tr [style.height.px]="Top" *ngIf="Top">
                <td>
                    <div></div>
                </td>
            </tr>

            <tr [style.height.px]="HeaderHeight">
                <td [style.height.px]="HeaderHeight">
                    <ng-content select="SectionHead"></ng-content>
                </td>
            </tr>

            <tr style="height: 3px;" *ngIf="!Border">
                <td>
                    <div></div>
                </td>
            </tr>

            <tr>
                <td>
                    <div class="MediaFill">
                        <div class="MediaFixed">
                            <div class="LogitudeSectionBody LogitudeSmallScrollViewer" [ngStyle]="{'overflow-y': Scrolling ? 'auto' : 'hidden', 'border': Border, 'border-top': '0px solid transparent'}">                                 
                                <ng-content select="SectionBody"></ng-content>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>

            <tr [style.height.px]="Bottom" *ngIf="Bottom">
                <td>
                    <div></div>
                </td>
            </tr>
        </table>
    </div>
    `,

    styles:
    [`
    .LogitudeSectionBody {
        height:100%;        
        width:100%;
        min-width: 100%;
        min-height: 100%;
        max-height: 100%;
        max-width: 100%;
        overflow-x: hidden;
        position: relative;
        border-radius: 0px 0px 4px 4px;
        -moz-border-radius: 0px 0px 4px 4px;
        -webkit-border-radius: 0px 0px 4px 4px;
        padding-right: 1px;
    }
    `],
})

export class SectionBox {
    public Top: number = null;
    public Bottom: number = null;
    public Scrolling: boolean = false;
    public SectionHead: SectionHead;
    constructor() {

    }

    private headerHeight: number = 27;
    get HeaderHeight() { return this.headerHeight; }
    set HeaderHeight(value: number) {
        this.headerHeight = value;
        if (this.SectionHead) {
            this.SectionHead.Height = value;
        }
    }

    private border: string = null;
    get Border() { return this.border; }
    set Border(value: string) {
        this.border = value;
        if (this.SectionHead) {
            this.SectionHead.Border = value;
        }
    }
}

@Component({
    selector: 'SectionHead',
    inputs: ['Left', 'Right', 'IsLight', 'IsBlue'],
    template:
    `
    <div
        [class.LogitudeSectionHead]="!IsLight && !IsBlue"
        [class.LogitudeSectionHead2]="IsLight"
        [class.LogitudeSectionBlueHeader]="IsBlue"
        [style.height.px]="Height"
        [style.line-height.px]="Height"
        [style.padding-left.px]="Left"
        [style.padding-right.px]="Right"
        [ngStyle]="{border: Border, 'border-bottom': '0px solid transparent'}"
    > 
        <ng-content></ng-content>
    </div>
    `,

    styles:
    [`
    .LogitudeSectionHead {
        color: #45494A;
        font-size: 14px;
        text-align: left;
        border-radius: 4px 4px 0px 0px;
        -moz-border-radius: 4px 4px 0px 0px;
        -webkit-border-radius: 4px 4px 0px 0px;
        background: -moz-linear-gradient(50% 0% -90deg,rgba(235, 235, 235, 1) 0%,rgba(233, 233, 233, 1) 48.14%,rgba(226, 226, 226, 1) 67.05%,rgba(214, 214, 214, 1) 80.81%,rgba(197, 197, 197, 1) 92.01%,rgba(179, 179, 179, 1) 100%);
        background: -webkit-linear-gradient(-90deg, rgba(235, 235, 235, 1) 0%, rgba(233, 233, 233, 1) 48.14%, rgba(226, 226, 226, 1) 67.05%, rgba(214, 214, 214, 1) 80.81%, rgba(197, 197, 197, 1) 92.01%, rgba(179, 179, 179, 1) 100%);
        background: -webkit-gradient(linear,50% 0%,50% 100%,color-stop(0,rgba(235, 235, 235, 1) ),color-stop(0.4814,rgba(233, 233, 233, 1) ),color-stop(0.6705,rgba(226, 226, 226, 1) ),color-stop(0.8081,rgba(214, 214, 214, 1) ),color-stop(0.9201,rgba(197, 197, 197, 1) ),color-stop(1,rgba(179, 179, 179, 1) ));
        background: -o-linear-gradient(-90deg, rgba(235, 235, 235, 1) 0%, rgba(233, 233, 233, 1) 48.14%, rgba(226, 226, 226, 1) 67.05%, rgba(214, 214, 214, 1) 80.81%, rgba(197, 197, 197, 1) 92.01%, rgba(179, 179, 179, 1) 100%);
        background: linear-gradient(180deg, rgba(235, 235, 235, 1) 0%, rgba(233, 233, 233, 1) 48.14%, rgba(226, 226, 226, 1) 67.05%, rgba(214, 214, 214, 1) 80.81%, rgba(197, 197, 197, 1) 92.01%, rgba(179, 179, 179, 1) 100%);
    }

    .LogitudeSectionHead2 {
        color: #45494A;
        font-size: 14px;
        text-align: left;
        border-radius: 4px 4px 0px 0px;
        -moz-border-radius: 4px 4px 0px 0px;
        -webkit-border-radius: 4px 4px 0px 0px;
        background: -moz-linear-gradient(50% 0% -90deg,rgba(245, 245, 245, 1) 0%,rgba(243, 243, 243, 1) 50.24%,rgba(236, 236, 236, 1) 69.98%,rgba(224, 224, 224, 1) 84.35%,rgba(207, 207, 207, 1) 96.04%,rgba(199, 199, 199, 1) 100%);
        background: -webkit-linear-gradient(-90deg, rgba(245, 245, 245, 1) 0%, rgba(243, 243, 243, 1) 50.24%, rgba(236, 236, 236, 1) 69.98%, rgba(224, 224, 224, 1) 84.35%, rgba(207, 207, 207, 1) 96.04%, rgba(199, 199, 199, 1) 100%);
        background: -webkit-gradient(linear,50% 0%,50% 100%,color-stop(0,rgba(245, 245, 245, 1) ),color-stop(0.5024,rgba(243, 243, 243, 1) ),color-stop(0.6998,rgba(236, 236, 236, 1) ),color-stop(0.8435,rgba(224, 224, 224, 1) ),color-stop(0.9604,rgba(207, 207, 207, 1) ),color-stop(1,rgba(199, 199, 199, 1) ));
        background: -o-linear-gradient(-90deg, rgba(245, 245, 245, 1) 0%, rgba(243, 243, 243, 1) 50.24%, rgba(236, 236, 236, 1) 69.98%, rgba(224, 224, 224, 1) 84.35%, rgba(207, 207, 207, 1) 96.04%, rgba(199, 199, 199, 1) 100%);
        background: linear-gradient(180deg, rgba(245, 245, 245, 1) 0%, rgba(243, 243, 243, 1) 50.24%, rgba(236, 236, 236, 1) 69.98%, rgba(224, 224, 224, 1) 84.35%, rgba(207, 207, 207, 1) 96.04%, rgba(199, 199, 199, 1) 100%);
    }

    .LogitudeSectionBlueHeader {
        color: #282E30;
        font-size: 12px;
        text-align: left;
        border-radius: 4px 4px 0px 0px;
        -moz-border-radius: 4px 4px 0px 0px;
        -webkit-border-radius: 4px 4px 0px 0px;
        background: -moz-linear-gradient(50% 0% -90deg,rgba(235, 243, 255, 1) 0%,rgba(198, 223, 255, 1) 42%,rgba(171, 201, 238, 1) 43%,rgba(208, 232, 255, 1) 100%);
        background: -webkit-linear-gradient(-90deg, rgba(235, 243, 255, 1) 0%, rgba(198, 223, 255, 1) 42%, rgba(171, 201, 238, 1) 43%, rgba(208, 232, 255, 1) 100%);
        background: -o-linear-gradient(-90deg, rgba(235, 243, 255, 1) 0%, rgba(198, 223, 255, 1) 42%, rgba(171, 201, 238, 1) 43%, rgba(208, 232, 255, 1) 100%);
        background: linear-gradient(180deg, rgba(235, 243, 255, 1) 0%, rgba(198, 223, 255, 1) 42%, rgba(171, 201, 238, 1) 43%, rgba(208, 232, 255, 1) 100%);
    }
    `],
})
export class SectionHead {
    public Left: number = 10;
    public Right: number = 5;
    public IsLight: boolean = false;
    public IsBlue: boolean = false;
    constructor(Parent: SectionBox) {
        if (Parent) {
            Parent.SectionHead = this;
        }
    }

    private height: number = 27;
    get Height() { return this.height; }
    set Height(value: number) {
        this.height = value;
    }

    private border: string = null;
    get Border() { return this.border; }
    set Border(value: string) {
        this.border = value;        
    }
}

@Component({
    selector: 'SectionBody',
    template: '<ng-content></ng-content>',
})
export class SectionBody { }
