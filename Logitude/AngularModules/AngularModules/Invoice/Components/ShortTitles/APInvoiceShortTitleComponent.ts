import { SessionLocator } from './../../../Infrastructure/Utilities/SessionLocator';
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
    private CurrentSession = SessionLocator.SelectedSession;
    public showLocal = !SessionLocator.LoggedUserPM.DontShowLocal;

    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;

        this.Listen();

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


    private Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    this.BuildComponent();
                }
            });

            this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    this.BuildComponent();
                }
            });
        }
    }
}
