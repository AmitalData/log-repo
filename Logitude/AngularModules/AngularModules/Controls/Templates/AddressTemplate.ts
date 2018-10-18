import {Component} from '@angular/core';
import {AppTool} from '../../Infrastructure/Tools';
import {AddressList} from '../../Common/EntityLists/AddressList';

@Component({
    selector: 'AddressTemplate',
    inputs: ['Address', 'Height', 'Right', 'Border'],    

    template:
    `
    <table [style.height.px]="Height">
        <tr>
            <td>
                <div class="MediaFill">
                    <div class="LogitudeAddressTemplate" [ngStyle]= "{border:Border}">
                        <div *ngIf="Address" class="MediaFillAbsolute">
                            <div class="TextTrimming" *ngIf="Address.Address1">{{Address.Address1}}</div>
                            <div class="TextTrimming" *ngIf="Address.Address2" style="padding-right: 30px;">{{Address.Address2}}</div>
                            <div class="TextTrimming" *ngIf="CityLineText">{{CityLineText}}</div>
                            <div class="TextTrimming" *ngIf="Address.CountryName">{{Address.CountryName}}</div>

                            <img style="position: absolute; right:2px; bottom:-2px; width: 32px; height: 32px;" [attr.src]="Address.CountryCode | CountryFlagPipe" />
                            <div style="width: 20px; height: 15px; position:absolute; right: 10px; bottom: 30px;">
                                <GoogleMapsButton [AddressList]="Address"></GoogleMapsButton>
                            </div>
                        </div>
                    </div>
                </div>
            </td>

            <td [style.width.px]="Right" *ngIf="Right">
                <div></div>
            </td>
        </tr>
    </table>  
    `,

    styles:
    [`
    .LogitudeAddressTemplate {
        position: relative
        width: 100%;
        height: 100%;
        background: #EBEBEB;
        border: 1px solid #AAAAAA;
        border-radius: 3px;
        -moz-border-radius: 3px;
        -webkit-border-radius: 3px;
        overflow: hidden;        
    }

    .LogitudeAddressTemplate div {
        font-size: 10px;
        color: #282E30;
        text-indent: 5px;
        line-height: 17px;
    }
    `],
})

export class AddressTemplate {
    public Height: number = 70;
    public Right: number = null;
    public Border: string = "1px solid #AAAAAA";

    constructor() {

    }

    private address: AddressList = null;
    get Address() { return this.address; }
    set Address(value: AddressList) {
        if (this.address != value) {
            this.address = value;
            this.GetCityLineText();            
        }
    }

    public CityLineText: string = null;
    GetCityLineText() {
        var myResult = null;

        if (this.Address != null) {
            myResult = this.Address.City;

            if (!AppTool.IsNullOrEmpty(this.Address.StateName)) {
                myResult += ", " + this.Address.StateName;
            }

            if (!AppTool.IsNullOrEmpty(this.Address.ZipCode)) {
                myResult += ", " + this.Address.ZipCode;
            }
        }

        this.CityLineText = myResult;
    }
}