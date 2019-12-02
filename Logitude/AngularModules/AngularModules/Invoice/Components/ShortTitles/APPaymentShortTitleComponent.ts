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
        //this.Listen();
        if (this.EntityPM != null) {
            this.BuildComponent();
        }
    }
    VoidedByJournalNumber:string;
    private BuildComponent() {
        
    }
    // private Listen() {
    //    if (this.CurrentSession.CurrentEditComponent != null) {
    //        this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
    //            if (isSaveSuccess) {
    //                this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;

    //            }
    //        });

    //        this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
    //            if (isLoadSuccess) {
    //                this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;

    //            }
    //        });
    //    }
    //}
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
