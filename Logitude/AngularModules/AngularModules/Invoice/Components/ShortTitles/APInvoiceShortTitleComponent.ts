import { Component, OnDestroy} from '@angular/core';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {APInvoicePM} from '../../EntityPMs/APInvoicePM';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';
import { SessionLocator } from './../../../Infrastructure/Utilities/SessionLocator';
import { AppTool } from './../../../Infrastructure/Tools';

@Component({    
    templateUrl: "./APInvoiceShortTitleComponent.html",
})

export class APInvoiceShortTitleComponent implements OnDestroy {
    public EntityPM: APInvoicePM;
    public isRTL: boolean = false;
    public showLocal = !SessionLocator.LoggedUserPM.DontShowLocal;
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;

        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

        if (this.EntityPM) {
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

    public EntityNumber: string = null;
    private BuildComponent() {
        if (this.EntityPM.InvoiceNumber) {
            this.EntityNumber = this.EntityPM.InvoiceNumber + ", ";
        }
    }
}
