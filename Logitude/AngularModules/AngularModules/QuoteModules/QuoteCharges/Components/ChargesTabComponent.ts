import {Component, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {QuotePM} from '../../../Quote/EntityPMs/QuotePM';
import {QuoteUtilities} from '../../../Quote/Utilities/QuoteUtilities';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';

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

export class ChargesTabComponent implements OnInit {
    public EntityPM: QuotePM = null;
    public ObjectTableName: string = "Quote";
    @ViewChild('Child', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
    constructor(entityArgs: EntityArgs, private entityResourceService: EntityResourceService) {
        this.EntityPM = entityArgs.EntityPM;
    }

    ngOnInit() {
        this.entityResourceService.getEntityResourceByTableName("QuoteCharge").subscribe((res1: any) => {
            this.entityResourceService.getEntityResourceByTableName("QuotePriceSteps").subscribe((res2: any) => {
                this.entityResourceService.getEntityResourceByTableName("TarrifHeader").subscribe((res3: any) => {

                    var isLCL = QuoteUtilities.IsLCLQuote(this.EntityPM);

                    if (isLCL) {
                        SessionLocator.DynamicLoader.Load('./QuoteModules/QuoteCharges/Components/LCLChargesComponent', this.viewContainerRef)
                            .then(cmpRef => {
                                //cmpRef.instance
                            });
                    }

                    else {
                        SessionLocator.DynamicLoader.Load('./QuoteModules/QuoteCharges/Components/FCLChargesComponent', this.viewContainerRef)
                            .then(cmpRef => {
                                //cmpRef.instance
                            });
                    }
                });
            });
        });
    }
}