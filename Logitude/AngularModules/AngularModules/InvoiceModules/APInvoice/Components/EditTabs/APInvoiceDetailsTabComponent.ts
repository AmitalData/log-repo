import {Component, OnInit, ViewChild, ViewContainerRef}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {APInvoicePM} from '../../../../Invoice/EntityPMs/APInvoicePM';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';

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

export class APInvoiceDetailsTabComponent implements OnInit {
    public EntityPM: APInvoicePM = null;
    public ObjectTableName = "APInvoice";
    @ViewChild("Child", { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
    public isRTL: boolean = false;

    constructor(private entityArgs: EntityArgs, private entityResourceService: EntityResourceService) {
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");      
        this.EntityPM = entityArgs.EntityPM;
    }

    ngOnInit() {
        this.entityResourceService.getEntityResourceByTableName("APInvoice").subscribe((res: any) => {
            this.entityResourceService.getEntityResourceByTableName("APInvoiceLine").subscribe((res: any) => {

                if (this.EntityPM.IsMultipleEntities) {
                    SessionLocator.DynamicLoader.Load("./InvoiceModules/APInvoice/Components/EditTabs/APInvoiceMultipleDetailsTabComponent", this.viewContainerRef)
                        .then(cmpRef => {
                            //cmpRef.instance
                        });
                }
                else if (this.EntityPM.IsGeneralInvoice) {
                    SessionLocator.DynamicLoader.Load("./InvoiceModules/APInvoice/Components/EditTabs/APInvoiceDetailsTabGeneral", this.viewContainerRef)
                        .then(cmpRef => {
                            //cmpRef.instance
                        });
                }
                else {
                    SessionLocator.DynamicLoader.Load("./InvoiceModules/APInvoice/Components/EditTabs/APInvoiceDetailsTabNormal", this.viewContainerRef)
                        .then(cmpRef => {
                            //cmpRef.instance
                        });
                }
            });
        });
    }
}