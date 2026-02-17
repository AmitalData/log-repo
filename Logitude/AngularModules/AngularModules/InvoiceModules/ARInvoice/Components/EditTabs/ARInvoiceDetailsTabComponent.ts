import { Component, OnInit, OnDestroy, ViewChild, ViewContainerRef } from '@angular/core';
import { AppTool } from '../../../../Infrastructure/Tools';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ARInvoicePM} from '../../../../Invoice/EntityPMs/ARInvoicePM';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';

@Component({
    template:
    `
        <table>
            <tr>
                <td>
                    <div class="MediaFill">
                        <div #Child></div>
                    </div>
                </td>
            </tr>
        </table>
    `,
})

export class ARInvoiceDetailsTabComponent implements OnInit, OnDestroy {
    public EntityPM: ARInvoicePM = null;
    public ObjectTableName = "ARInvoice";
    @ViewChild("Child", { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs, private entityResourceService: EntityResourceService) {
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    }

    private SessionEvent: any = null;
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent) {

            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "ResetARInvoiceBaseDeailsTab") {
                    this.InitBaseTabComponent();
                }
            });

            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                }
            });
        }
    }

    ngOnInit() {
        this.entityResourceService.getEntityResourceByTableName("ARInvoice").subscribe((res1: any) => {
            this.entityResourceService.getEntityResourceByTableName("ARInvoiceLine").subscribe((res2: any) => {
                this.InitBaseTabComponent();                
            });
        });
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SessionEvent);
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    InitBaseTabComponent() {

        this.viewContainerRef.clear();

        if (this.EntityPM.IsConsolidationInvoice) {

            if (this.EntityPM.StatusCode == "AC" || this.EntityPM.StatusCode == "AR") {
                SessionLocator.DynamicLoader.Load("./InvoiceModules/ARInvoice/Components/EditTabs/ARInvoiceDetailsTabNormal", this.viewContainerRef)
                    .then(cmpRef => {
                        //cmpRef.instance
                    });
            }

            else {
                SessionLocator.DynamicLoader.Load("./InvoiceModules/ARInvoice/Components/EditTabs/ARInvoiceDetailsTabConsolidation", this.viewContainerRef)
                    .then(cmpRef => {
                        //cmpRef.instance
                    });
            }
        }
        else if (this.EntityPM.IsGeneralInvoice) {
            SessionLocator.DynamicLoader.Load("./InvoiceModules/ARInvoice/Components/EditTabs/ARInvoiceDetailsTabGeneral", this.viewContainerRef)
                .then(cmpRef => {
                    //cmpRef.instance
                });
        }
        else {
            SessionLocator.DynamicLoader.Load("./InvoiceModules/ARInvoice/Components/EditTabs/ARInvoiceDetailsTabNormal", this.viewContainerRef)
                .then(cmpRef => {
                    //cmpRef.instance
                });
        }
    }
}
