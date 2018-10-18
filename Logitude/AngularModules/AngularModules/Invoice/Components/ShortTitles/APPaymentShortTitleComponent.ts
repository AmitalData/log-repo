import {Component} from '@angular/core';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {APPaymentPM} from '../../EntityPMs/APPaymentPM';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    moduleId: module.id,
    templateUrl: "./APPaymentShortTitleComponent.html",
})

export class APPaymentShortTitleComponent {
    public EntityPM: APPaymentPM;
    public isRTL: boolean = false;

    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;

        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");       

        if (this.EntityPM != null) {
            this.BuildComponent();
        }
    }

    private BuildComponent() {
        
    }

    get EntityNumber() {
        var myResult = "";

        if (this.EntityPM) {
            if (this.EntityPM.PaymentNo) {
                myResult = this.EntityPM.PaymentNo + ", ";
            }
        }

        return myResult;
    }
}