import {Component, OnInit, ChangeDetectionStrategy} from '@angular/core';
import {AppTool} from '../../Infrastructure/Tools';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../Infrastructure/Utilities/TextCodeTranslator';
import {ObjectsLocator} from '../../Infrastructure/Locators/ObjectsLocator';
@Component({
    selector: 'ValidationSummary',
    inputs: ['ItemsSource', 'SingleLine'],
    changeDetection: ChangeDetectionStrategy.OnPush,

    template:
    `
    <table style="min-height: 25px;">
        <tr>
            <td>
                <div class="ValidationSummary">
                    <table>
                        <tr>
                            <td [style.text-align]="LayoutDirection=='rtl' ? 'right' : 'left'"
                                [ngStyle]="LayoutDirection == 'rtl' ? {'padding-right': '8px'} : {'padding-left': '8px'}"
                                 style="width: 110px; vertical-align: top; padding-top: 3px; padding-bottom: 3px; color: #B02020; font-size: 12px;">
                                {{ErrorsCount}} {{ErrorsFoundText}}:
                            </td>

                            <td style="position: relative; vertical-align:top; padding-top: 2px; padding-bottom: 2px;" >

                                <div *ngIf="isSingleError" class="SingleError" [ngStyle]="LayoutDirection == 'rtl' ? {'padding-right': '25px'} : {'padding-left': '25px'}"><img [className]="LayoutDirection == 'rtl' ? 'RightCenter' : 'LeftCenter'" src="./Images/ValidationError.png" />{{ItemsSource[0]}}</div>

                                <ul *ngIf="!isSingleError">
                                    <li class="ValidationItem" [ngStyle]="{width: ItemWidth}" *ngFor="let item of ItemsSource" [style.float]="LayoutDirection=='rtl' ? 'right' : 'left'">
                                        <img [className]="LayoutDirection == 'rtl' ? 'RightCenter' : 'LeftCenter'" src="./Images/ValidationError.png" />
                                        <span [className]="LayoutDirection == 'rtl' ? 'RightCenter' : 'LeftCenter'" [style.text-align]="LayoutDirection=='rtl' ? 'right' : 'left'"
                                                [ngStyle]="LayoutDirection == 'rtl' ? {'margin-right': '20px'} : {'margin-left': '20px'}">{{item}}</span>
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
    .ValidationSummary span {
        text-align: left;
        width: calc(100% - 25px);
        overflow: hidden;
        white-space: nowrap;
        text-overflow: ellipsis;
    }

    .ValidationSummary .ValidationItem {
        display: block;
        float: left;
        width: 50%;
        height: 18px;
        line-height: 20px;
        position: relative;
    }

    .ValidationSummary ul {
        max-height: 80px;
        box-sizing: border-box;
        -moz-box-sizing: border-box;
        -webkit-box-sizing: border-box;
        overflow-x: hidden;
        overflow-y: auto;
    }

    .ValidationSummary {
        height: 100%;
        width: 100%;
        border: 1px solid #DA6F6F;
        border-radius: 5px;
        background: rgba(247, 227, 227, 1);
        box-sizing: border-box;
        -moz-box-sizing: border-box;
        -webkit-box-sizing: border-box;
        overflow: hidden;
    }    

    .SingleError{
        white-space: normal;
        max-height: 60px;
        padding-top: 2px;
    }
    `],
})

export class ValidationSummary implements OnInit {
    public ErrorsCount: number = 0;
    public ItemWidth: string = "50%";
    isSingleError: boolean = false;
    LayoutDirection: string = 'ltr';
    ErrorsFoundText: string;

    ngOnInit() {

        this.ErrorsFoundText = TextCodeTranslator.Translate('General.O.ErrorsFound');
        this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
        this.UpdateItemWidth();
    }

    private singleLine: boolean = false;
    get SingleLine() { return this.singleLine; }
    set SingleLine(newValue: boolean) {
        if (this.singleLine != newValue) {
            this.singleLine = newValue;
            this.UpdateItemWidth();
        }
    }

    private itemsSource: string[] = [];
    get ItemsSource() { return this.itemsSource; }
    set ItemsSource(newValue: string[]) {
        if (this.itemsSource != newValue) {
            this.itemsSource = [];

            if (newValue != null) {
                newValue.forEach((item: string) => {

                    if (!AppTool.IsNullOrEmpty(item) && typeof (item) == "string") {
                        if (item.startsWith("!!")) {
                            item = item.substr(1);
                        }

                        if (item.indexOf(';') > -1) {
                            var itemParts = item.split(';');
                            itemParts.forEach(itemPart => {

                                itemPart = AppTool.Replace(itemPart, "(ᵜ)", ",");

                                if (this.itemsSource.indexOf(itemPart) == -1) {
                                    this.itemsSource.push(itemPart);
                                }
                            });
                        }

                        else {
                            item = AppTool.Replace(item, "(ᵜ)", ",");

                            if (this.itemsSource.indexOf(item) == -1) {
                                this.itemsSource.push(item);
                            }
                        }
                    }
                });
            }

            if (this.itemsSource.length == 1) {
                this.isSingleError = true;
            }

            else {
                this.isSingleError = false;
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
