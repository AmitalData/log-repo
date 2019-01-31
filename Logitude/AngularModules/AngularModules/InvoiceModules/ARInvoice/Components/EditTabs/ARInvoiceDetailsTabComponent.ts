import {Component, OnInit, ViewChild, ViewContainerRef}  from '@angular/core';
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

export class ARInvoiceDetailsTabComponent implements OnInit {
    public EntityPM: ARInvoicePM = null;
    public ObjectTableName = "ARInvoice";
    @ViewChild("Child", { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
    constructor(private entityArgs: EntityArgs, private entityResourceService: EntityResourceService) {
        this.EntityPM = entityArgs.EntityPM;
    }

    ngOnInit() {
        this.entityResourceService.getEntityResourceByTableName("ARInvoice").subscribe((res1: any) => {
            this.entityResourceService.getEntityResourceByTableName("ARInvoiceLine").subscribe((res2: any) => {
                if (this.EntityPM.IsConsolidationInvoice) {
                    SessionLocator.DynamicLoader.Load("./InvoiceModules/ARInvoice/Components/EditTabs/ARInvoiceDetailsTabConsolidation", this.viewContainerRef)
                        .then(cmpRef => {
                            //cmpRef.instance
                        });
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
            });
        });
    }
}