import { Component, AfterViewInit, ViewChild } from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {APInvoicePM} from '../../../../Invoice/EntityPMs/APInvoicePM';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import { ChildDirective } from '../../../../Infrastructure/Directives/ChildDirective';

@Component({
    template:
    `
        <table>
            <tr>
                <td>
                    <div class="MediaFill">
                        <div ChildDirective></div>
                    </div>
                </td>
            </tr>
        </table>
    `,
})

export class APInvoiceDetailsTabComponent implements AfterViewInit {
    public EntityPM: APInvoicePM = null;
  public ObjectTableName = "APInvoice";

  @ViewChild(ChildDirective) Child: ChildDirective;

    //@ViewChild("Child", { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;
    public isRTL: boolean = false;

    constructor(private entityArgs: EntityArgs, private entityResourceService: EntityResourceService) {
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");      
        this.EntityPM = entityArgs.EntityPM;
    }

  ngAfterViewInit() {
        this.entityResourceService.getEntityResourceByTableName("APInvoice").subscribe((res: any) => {
            this.entityResourceService.getEntityResourceByTableName("APInvoiceLine").subscribe((res: any) => {

              if (this.EntityPM.IsMultipleEntities) {
                SessionLocator.DynamicLoader.Load("./InvoiceModules/APInvoice/Components/EditTabs/APInvoiceMultipleDetailsTabComponent", this.Child.Location)
                        .then(cmpRef => {
                            //cmpRef.instance 66
                          var d = 9;
                        });
                }
              else if (this.EntityPM.IsGeneralInvoice) {
                SessionLocator.DynamicLoader.Load("./InvoiceModules/APInvoice/Components/EditTabs/APInvoiceDetailsTabGeneral", this.Child.Location)
                        .then(cmpRef => {
                            //cmpRef.instance
                        });
                }
              else {
                SessionLocator.DynamicLoader.Load("./InvoiceModules/APInvoice/Components/EditTabs/APInvoiceDetailsTabNormal", this.Child.Location)
                        .then(cmpRef => {
                            //cmpRef.instance
                        });
                }
            });
        });
    }
}
