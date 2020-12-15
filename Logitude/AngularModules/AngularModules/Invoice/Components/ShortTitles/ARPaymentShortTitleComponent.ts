import { Component, OnDestroy } from '@angular/core';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {ARPaymentPM} from '../../EntityPMs/ARPaymentPM';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';
import { AppTool } from '../../../Infrastructure/Tools';

@Component({    
    templateUrl: "./ARPaymentShortTitleComponent.html",
})

export class ARPaymentShortTitleComponent implements OnDestroy {
    public EntityPM: ARPaymentPM;
    public DisplaySATSettings: boolean = false;
    public isRTL: boolean = false;
    public showLocals: boolean = false;
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;

        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.showLocals = !(SessionLocator.LoggedUserPM.DontShowLocal);

        if (this.EntityPM != null) {
            this.BuildComponent();
        }

        if (SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            this.DisplaySATSettings = true;
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
