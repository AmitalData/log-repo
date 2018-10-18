import {Component, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {QuotePM} from '../../../../Quote/EntityPMs/QuotePM';
import {QuoteUtilities} from '../../../../Quote/Utilities/QuoteUtilities';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';

@Component({
    selector: 'RoutingsTabComponent',

    template:
    `
        <div class="TabHolder">
            <table>
                <tr>
                    <td class="CellStretch">
                        <div class="CellContent">
                            <div #Child></div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    `,
})

export class RoutingsTabComponent implements OnInit {
    public EntityPM: QuotePM = null;
    public ObjectTableName: string = null;
    @ViewChild('Child', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef; 
    constructor(private entityArgs: EntityArgs) {
        this.EntityPM = entityArgs.EntityPM;
        this.ObjectTableName = entityArgs.ObjectTableName;
    }
    
    ngOnInit() {
        if (this.EntityPM != null) {
            var isInlandDomestic = QuoteUtilities.IsInlandDomestic(this.EntityPM);

            if (isInlandDomestic) {
                SessionLocator.DynamicLoader.Load('./QuoteModules/QuoteTabs/Components/Routings/InlandDomesticRoutingsComponent', this.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.InitTab(this.EntityPM, this.ObjectTableName);
                    });
            }

            else {
                SessionLocator.DynamicLoader.Load('./QuoteModules/QuoteTabs/Components/Routings/OrdinaryRoutingsComponent', this.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.InitTab(this.EntityPM, this.ObjectTableName);
                    });
            }
        }
    }    
}