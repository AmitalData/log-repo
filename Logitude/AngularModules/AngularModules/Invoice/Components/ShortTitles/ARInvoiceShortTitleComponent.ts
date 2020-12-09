import { Component, OnDestroy } from '@angular/core';
import {AppTool} from '../../../Infrastructure/Tools';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {ARInvoicePM} from '../../EntityPMs/ARInvoicePM';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';

@Component({    
    templateUrl: "./ARInvoiceShortTitleComponent.html",
})

export class ARInvoiceShortTitleComponent implements OnDestroy {
    public EntityPM: ARInvoicePM;
    public DisplaySATSettings: boolean = false;
    public isRTL: boolean = false;
    public showLocal: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;

        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.showLocal = !SessionLocator.LoggedUserPM.DontShowLocal;

        this.BuildComponent();

        this.Listen();

        if (SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            this.DisplaySATSettings = true;
        }
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

    public IsConnectedToConsolidation: boolean = false;
    private BuildComponent() {
        var isConnectedToConsolidation: boolean = false;

        if (this.EntityPM != null) {
            if (this.EntityPM.IsConstituentInvoice && this.EntityPM.ConsolidationInvoiceId != null) {
                isConnectedToConsolidation = true;

            }

            this.GetEntityNumber();
        }

        this.IsConnectedToConsolidation = isConnectedToConsolidation;
    }

    public EntityNumber: string = null;
    GetEntityNumber() {

        if (this.EntityPM.StatusCode == "DR" || this.EntityPM.StatusCode == "LL") {
            if (this.EntityPM.DraftNumber) {
                this.EntityNumber = this.EntityPM.DraftNumber + ", ";
            }
        }

        else {
            if (this.EntityPM.InvoiceNumber) {
                this.EntityNumber = this.EntityPM.InvoiceNumber + ", ";
            }
        }
    }

    ViewEntityClicked(invoiceId: string) {
        if (!AppTool.IsNullOrEmpty(invoiceId)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: invoiceId, ObjectTableName: 'ARInvoice', BackButtonLabel: "A/R Invoice: " + this.EntityPM.InvoiceNumber });

                    let isEditComponentSaved = false;

                    cmpRef.instance.BackCompleted.subscribe(bk => {
                        if (isEditComponentSaved) {
                            this.entityArgs.EditComponent.ReloadEntityPM();
                        }

                        else if (cmpRef.instance.NeedRefresh) {
                            this.entityArgs.EditComponent.NeedRefresh = true;
                            this.entityArgs.EditComponent.ReloadEntityPM();
                        }
                    });

                    cmpRef.instance.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                        if (isSaveSuccess) {
                            isEditComponentSaved = true;
                        }
                    });

                    cmpRef.instance.SaveAndCloseCompleted.subscribe((isSaveSuccess: boolean) => {
                        if (isSaveSuccess) {
                            isEditComponentSaved = true;
                        }
                    });
                });
        }
    }
}
