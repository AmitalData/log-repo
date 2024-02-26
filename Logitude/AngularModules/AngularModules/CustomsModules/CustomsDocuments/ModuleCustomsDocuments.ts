import { NgModule } from '@angular/core';
import { InfrastructureModule } from '../../Infrastructure/Module_INFR';
//import {CustomsModule} from '../../Customs/Module_CUST';
import { Components, ModuleDeclarations } from './ModuleDeclarations';
import { ModuleCustomsControls } from '../CustomsControls/ModuleCustomsControls';
import { PdfViewerModule } from 'ng2-pdf-viewer';

@NgModule({
  imports: [InfrastructureModule, ModuleCustomsControls, PdfViewerModule],
  exports: [...Components, ModuleCustomsControls],
  declarations: [...Components],
  entryComponents: [...Components],
})

export class ModuleCustomsDocuments {
  public static GetComponent(name: string) {
    return ModuleDeclarations.Get(name);
  }
}
