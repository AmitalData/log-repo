import {Component} from '@angular/core';
import {AppTool} from '../../../Infrastructure/Tools';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {ARInvoicePM} from '../../EntityPMs/ARInvoicePM';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    moduleId: module.id,
    templateUrl: "./ARInvoiceShortTitleComponent.html",
})

export class ARInvoiceShortTitleComponent {
    public EntityPM: ARInvoicePM;
    public DisplaySATSettings: boolean = false;
    public isRTL: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");   
        this.BuildComponent();
        this.Listen();

        if (SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            this.DisplaySATSettings = true;
        }
    }

    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
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

    public IsConnectedToConsolidation: boolean = false;
    private BuildComponent() {
        if (this.EntityPM != null) {
            if (this.EntityPM.IsConstituentInvoice && this.EntityPM.ConsolidationInvoiceId != null) {
                this.IsConnectedToConsolidation = true;
                
            }

            this.GetEntityNumber();
        }
    }

    public EntityNumber: string = null;
    GetEntityNumber() {
        if (this.EntityPM.StatusCode == "DR") {
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
                });
        }
    }
}
