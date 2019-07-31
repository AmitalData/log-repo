import { Component, OnInit, ViewChild, ViewContainerRef, OnDestroy} from '@angular/core';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {QuotePM} from '../../../Quote/EntityPMs/QuotePM';
import {QuoteUtilities} from '../../../Quote/Utilities/QuoteUtilities';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import { AppTool } from '../../../Infrastructure/Tools';

@Component({
    selector: 'ChargesTabComponent',

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

export class ChargesTabComponent implements OnInit, OnDestroy {
    public EntityPM: QuotePM = null;
    public ObjectTableName: string = "Quote";
    @ViewChild('Child', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
    constructor(public entityArgs: EntityArgs, private entityResourceService: EntityResourceService) {
        this.EntityPM = entityArgs.EntityPM;

        this.Listen();
    }

    private isLCL: boolean = false;
    private LoadCompletedEvent: any = null;
    Listen() {
        if (this.entityArgs.EditComponent) {            
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.isLCL = QuoteUtilities.IsLCLQuote(this.EntityPM);

                    this.SelectTab();
                }
            });            
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    ngOnInit() {
        this.entityResourceService.getEntityResourceByTableName("QuoteCharge").subscribe((res1: any) => {
            this.entityResourceService.getEntityResourceByTableName("QuotePriceSteps").subscribe((res2: any) => {
                this.entityResourceService.getEntityResourceByTableName("TarrifHeader").subscribe((res3: any) => {

                    this.isLCL = QuoteUtilities.IsLCLQuote(this.EntityPM);
                    this.SelectTab();                   
                });
            });
        });
    }

    private SelectTab() {
        this.viewContainerRef.clear();

        if (this.isLCL) {
            SessionLocator.DynamicLoader.Load('./QuoteModules/QuoteCharges/Components/LCLChargesComponent', this.viewContainerRef)
                .then(cmpRef => {                    
                });
        }

        else {
            SessionLocator.DynamicLoader.Load('./QuoteModules/QuoteCharges/Components/FCLChargesComponent', this.viewContainerRef)
                .then(cmpRef => {                   
                });
        }
    }
}
