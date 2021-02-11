import { Component, OnDestroy} from '@angular/core';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {APPaymentPM} from '../../EntityPMs/APPaymentPM';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';
import { AppTool } from './../../../Infrastructure/Tools';

@Component({    
    templateUrl: "./APPaymentShortTitleComponent.html",
})

export class APPaymentShortTitleComponent implements OnDestroy {
    public EntityPM: APPaymentPM;
    public isRTL: boolean = false;
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;

        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

        if (this.EntityPM != null) {
            this.BuildComponent();
        }

        this.Listen();
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    private Listen() {
        if (this.entityArgs.EditComponent) {

            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.BuildComponent();
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.BuildComponent();
                }
            });
        }
    }

    VoidedByJournalNumber: string;
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
