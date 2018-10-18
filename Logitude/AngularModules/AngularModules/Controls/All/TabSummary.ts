import {Component, ChangeDetectionStrategy} from '@angular/core';

@Component({
    selector: 'TabSummary',
    inputs: ['Height'],
    changeDetection: ChangeDetectionStrategy.OnPush,

    template:

    `
    <table [style.height.px]="Height" [style.min-height.px]="Height" [style.max-height.px]="Height">
        <tr>
            <td>
                <div class="MediaFill">
                    <div class="LogitudeTabSummary">
                        <div class="MarginAbsolute5">
                            <ng-content></ng-content>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>    
    `,

    styles:
    [`
    .LogitudeTabSummary {
        width: 100%;
        height: 100%;
        position: relative;
        overflow: hidden;
        
        border: 0px;
        border-top-width: 1px;
        box-shadow: 0px -1px 7px #AAAAAA;
        -moz-box-shadow: 0px -1px 7px #AAAAAA;
        -webkit-box-shadow: 0px -1px 7px #AAAAAA;
        border-radius: 10px 10px 8px 0px;
        -moz-border-radius: 10px 10px 8px 0px;
        -webkit-border-radius: 10px 10px 8px 0px;
    }
    `],
})

export class TabSummary {
    constructor() {

    }

    private height: number = 100;
    get Height() { return this.height; }
    set Height(value: number) {
        if (this.height != value) {
            this.height = value;
        }
    }

}