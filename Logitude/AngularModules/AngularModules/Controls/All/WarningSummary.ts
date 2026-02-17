import {Component, OnInit, ChangeDetectionStrategy} from '@angular/core';

@Component({
    selector: 'WarningSummary',
    inputs: ['ItemsSource', 'SingleLine', 'HideHeader'],
    changeDetection: ChangeDetectionStrategy.OnPush,

    template:
    `
    <table style="min-height: 25px;">
        <tr>
            <td>
                <div class="WarningSummary">
                    <table>
                        <tr>
                            <td *ngIf="!HideHeader" style="width: 120px; vertical-align: top; text-align: left; padding-left: 8px; padding-top: 4px; color: #D8770E; font-size: 12px;">
                                {{ErrorsCount}} Warnings Found:
                            </td>

                            <td style="position: relative; vertical-align:top; padding-top: 2px;">
                                <ul>
                                    <li class="ValidationItem" [ngStyle]="{width: ItemWidth}" *ngFor="let item of ItemsSource">
                                        <img class="LeftCenter" src="./Images/WarningIcon.png" />
                                        <span class="LeftCenter" style="margin-left: 20px;">{{item}}</span>
                                    </li>
                                </ul>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>

        <tr style="height: 5px;">
            <td>
                <div></div>
            </td>
        </tr>
    </table>
    `,

    styles:
    [`
    .WarningSummary span {
        text-align: left;
        width: calc(100% - 25px);
        overflow: hidden;
        white-space: nowrap;
        text-overflow: ellipsis;
    }

    .WarningSummary .ValidationItem {
        display: block;
        float: left;
        width: 50%;
        height: 18px;
        line-height: 20px;
        position: relative;
    }

    .WarningSummary ul {
        max-height: 80px;
        box-sizing: border-box;
        -moz-box-sizing: border-box;
        -webkit-box-sizing: border-box;
        overflow-x: hidden;
        overflow-y: auto;
    }

    .WarningSummary {
        height: 100%;
        width: 100%;
        border: 1px solid Orange;
        border-radius: 5px;
        background: #FFFBDA;
        box-sizing: border-box;
        -moz-box-sizing: border-box;
        -webkit-box-sizing: border-box;
        overflow: hidden;
    }
    `],
})

export class WarningSummary implements OnInit {
    public SingleLine: boolean = false;
    public HideHeader: boolean = false;
    public ErrorsCount: number;
    public ItemWidth: string = "50%";

    ngOnInit() {
        if (this.SingleLine) {
            this.ItemWidth = "100%";
        }
    }

    private itemsSource: string[] = [];
    get ItemsSource() { return this.itemsSource; }
    set ItemsSource(newValue: string[]) {
        if (this.itemsSource != newValue) {
            this.itemsSource = [];

            if (newValue != null) {
                newValue.forEach((item) => {

                    if (item != null) {
                        if (item.startsWith("!!")) {
                            item = item.substr(1);
                        }

                        if (this.itemsSource.indexOf(item) == -1) {
                            this.itemsSource.push(item);
                        }
                    }

                    //if (item != null) {
                    //    var fixedItem = item;
                    //    while (fixedItem.charAt(0) === '!') {
                    //        fixedItem = fixedItem.substr(1);
                    //    }

                    //    if (this.itemsSource.indexOf(fixedItem) == -1) {
                    //        this.itemsSource.push(fixedItem);
                    //    }
                    //}

                });
            }

            this.ErrorsCount = this.itemsSource.length;
            this.UpdateItemWidth();
        }
    }

    private UpdateItemWidth() {
        if (this.ErrorsCount <= 1) {
            this.ItemWidth = "100%";
        }

        else {
            if (this.SingleLine) {
                this.ItemWidth = "100%";
            }

            else {
                this.ItemWidth = "50%";
            }
        }
    }
}