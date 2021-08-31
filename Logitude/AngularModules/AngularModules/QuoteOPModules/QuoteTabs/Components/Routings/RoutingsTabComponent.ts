import { Component, AfterViewInit, ViewChild } from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {QuoteOPPM} from '../../../../QuoteOPM/EntityPMs/QuoteOPPM';
import {QuoteUtilities} from '../../../../QuoteOPM/Utilities/QuoteUtilities';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import { ChildDirective } from '../../../../Infrastructure/Directives/ChildDirective';

@Component({
  selector: 'RoutingsTabComponent',

  template:
    `
        <div class="TabHolder">
            <table>
                <tr>
                    <td>
                        <div class="MediaFill">
                            <div ChildDirective></div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    `,
})

export class RoutingsTabComponent implements AfterViewInit {
  public EntityPM: QuoteOPPM = null;
  public ObjectTableName: string = null;
  @ViewChild(ChildDirective) Child: ChildDirective;

  constructor(private entityArgs: EntityArgs) {
    this.EntityPM = entityArgs.EntityPM;
    this.ObjectTableName = entityArgs.ObjectTableName;
  }

  ngAfterViewInit() {
    if (this.EntityPM != null) {
      var isInlandDomestic = QuoteUtilities.IsInlandDomestic(this.EntityPM);

      if (isInlandDomestic) {
        SessionLocator.DynamicLoader.Load('./QuoteOPModules/QuoteTabs/Components/Routings/InlandDomesticRoutingsComponent', this.Child.Location)
          .then(cmpRef => {
            cmpRef.instance.InitTab(this.EntityPM, this.ObjectTableName);
          });
      }

      else {
        SessionLocator.DynamicLoader.Load('./QuoteOPModules/QuoteTabs/Components/Routings/OrdinaryRoutingsComponent', this.Child.Location)
          .then(cmpRef => {
            cmpRef.instance.InitTab(this.EntityPM, this.ObjectTableName);
          });
      }
    }
  }
}
