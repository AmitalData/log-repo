import { NgModule } from '@angular/core';
import { InfrastructureModule } from '../../Infrastructure/Module_INFR';
//import {CustomsModule} from '../../Customs/Module_CUST';
import { Components, ModuleDeclarations } from './ModuleDeclarations';
import { ModuleCustomsControls } from '../CustomsControls/ModuleCustomsControls';
import { ModuleCustomsCourier } from '../CustomsCourier/ModuleCustomsCourier';
@NgModule({
    imports: [InfrastructureModule, ModuleCustomsControls, ModuleCustomsCourier],
    exports: [...Components, ModuleCustomsControls, ModuleCustomsCourier],
  declarations: [...Components],
  entryComponents: [...Components],
})

export class ModuleCustomsListTemplates {
  public static GetComponent(name: string) {
    return ModuleDeclarations.Get(name);
  }
}
