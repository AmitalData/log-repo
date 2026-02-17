import {Component} from '@angular/core';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {ARPaymentPM} from '../../EntityPMs/ARPaymentPM';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    moduleId: module.id,
    templateUrl: "./ARPaymentShortTitleComponent.html",
})

export class ARPaymentShortTitleComponent {
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
