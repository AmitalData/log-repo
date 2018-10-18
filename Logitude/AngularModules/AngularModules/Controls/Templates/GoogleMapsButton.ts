import {Component, OnInit} from '@angular/core';
import {AppTool} from '../../Infrastructure/Tools';
import {AddressList} from '../../Common/EntityLists/AddressList';
import {ServiceLocator} from '../../Infrastructure/Locators/ServiceLocator';

@Component({
    selector: "GoogleMapsButton",
    inputs: ['AddressList', 'AddressString'],
    template:
    `
    <div *ngIf="IsVisible" style="width: 20px; height: 15px; position: relative; cursor:pointer;" title="Google Maps" (click)="OnClick()">
        <img src="./Images/GooglePin.png" style="width: 20px; height: 15px;"/>
    </div> 
    `,
})

export class GoogleMapsButton {
    public IsVisible: boolean = false;
    constructor() {

    }

    private addressList: AddressList = null;
    get AddressList() { return this.addressList; }
    set AddressList(value: AddressList) {
        if (this.addressList != value) {
            this.addressList = value;
            this.SetIsVisible();
        }
    }

    private addressString: string = null;
    get AddressString() { return this.addressString; }
    set AddressString(value: string) {
        if (this.addressString != value) {
            this.addressString = value;
            this.SetIsVisible();
        }
    }

    SetIsVisible() {
        var isVisible = false;

        if (this.AddressList != null) {
            if (!AppTool.IsNullOrEmpty(this.AddressList.CountryCode)) {
                isVisible = true;
            }
        }

        else if (!AppTool.IsNullOrEmpty(this.AddressString)) {
            isVisible = true;
        }

        this.IsVisible = isVisible;
    }

    OnClick() {
        if (this.AddressList != null || !AppTool.IsNullOrEmpty(this.AddressString)) {
            ServiceLocator.SendTotangoUserActivity("GoogleMaps", "Google maps search click");

            var myString = this.AddressString;

            if (this.AddressList != null) {
                myString = this.AddressList.Address1 + " " + this.AddressList.City + " " + (this.AddressList.StateName != null ? (this.AddressList.StateName + " ") : "") + this.AddressList.CountryName;
            }           

            var link = AppTool.GetLogitudeURL() + "/WebPages/GoogleMap.aspx?address=" + myString;

            var win = window.open(link, '_blank');
            win.focus();
        }
    }
}