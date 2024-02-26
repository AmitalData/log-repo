import { NgModule } from '@angular/core';
import { InfrastructureModule } from '../../../Infrastructure/Module_INFR';
import { Components, ModuleDeclarations } from './ModuleDeclarations';
import { ModuleCustomsControls } from '../../CustomsControls/ModuleCustomsControls';
import { LogtuideTableDataService } from '../../../QuoteOPM/Components/NewEntity/components/autocomplate-table/logtuide-table-data.service';
import { PdfViewerModule } from 'ng2-pdf-viewer';


@NgModule({
  imports: [InfrastructureModule, ModuleCustomsControls, PdfViewerModule],
  exports: [ModuleCustomsControls],
   
    declarations: [...Components],
    entryComponents: [...Components],
    providers: [
        LogtuideTableDataService,
    ]
})

export class ModuleDeclarationOthers {
    public static GetComponent(name: string) {
        return ModuleDeclarations.Get(name);
    }
}
