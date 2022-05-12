import { NgModule } from '@angular/core';
import { InfrastructureModule } from '../../Infrastructure/Module_INFR';
//import {CustomsModule} from '../../Customs/Module_CUST';
import { Components, ModuleDeclarations } from './ModuleDeclarations';
import { ModuleCustomsControls } from '../CustomsControls/ModuleCustomsControls';
import { CustomDocumentNewVersionService } from './services/CustomDocumentNewVersion.service';
@NgModule({
  imports: [InfrastructureModule, ModuleCustomsControls],
  exports: [...Components, ModuleCustomsControls],
  declarations: [...Components],
  entryComponents: [...Components],
  providers:[CustomDocumentNewVersionService],
})

export class ModuleCustomsDocuments {
  public static GetComponent(name: string) {
    return ModuleDeclarations.Get(name);
  }
}
