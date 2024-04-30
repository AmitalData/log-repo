import { NgModule } from '@angular/core';
import { InfrastructureModule } from '../../Infrastructure/Module_INFR';
//import {CustomsModule} from '../../Customs/Module_CUST';
import { Components, ModuleDeclarations } from './ModuleDeclarations';
import { ModuleCustomsControls } from '../CustomsControls/ModuleCustomsControls';
// import { LogtuideTableDataService } from 'QuoteOPM/Components/NewEntity/components/autocomplate-table/logtuide-table-data.service';
import { LogisticActionRequestPMService } from 'Customs/Services/StandardPMs/LogisticActionRequestPMService';
import { LogisticActionRequestWebService } from 'Customs/Services/WebServices/LogisticActionRequestWebService';
import { loggerService } from 'Infrastructure/Utilities/logger.service';
import { LogtuideTableDataService } from 'Infrastructure/Services/logtuide-table-data.service';
@NgModule({
  imports: [InfrastructureModule, ModuleCustomsControls],
  exports: [...Components, ModuleCustomsControls],
  declarations: [...Components],
  providers: [
   LogtuideTableDataService,
    LogisticActionRequestPMService,
    LogisticActionRequestWebService,
    loggerService,
  ],
  entryComponents: [...Components],
})

export class ModuleCustomsLogisticActionRequest {
  public static GetComponent(name: string) {
    return ModuleDeclarations.Get(name);
  }
}
