import {Component} from '@angular/core';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {APInvoicePM} from '../../EntityPMs/APInvoicePM';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    moduleId: module.id,
    templateUrl: "./APInvoiceShortTitleComponent.html",
})

export class APInvoiceShortTitleComponent {
    public EntityPM: APInvoicePM;
    public isRTL: boolean = false;

    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;

        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");       

        if (this.EntityPM != null) {
            this.BuildComponent();
        }
    }

    public EntityNumber: string = null;
    private BuildComponent() {
        if (this.EntityPM.InvoiceNumber) {
            this.EntityNumber = this.EntityPM.InvoiceNumber + ", ";
        }
    }
}