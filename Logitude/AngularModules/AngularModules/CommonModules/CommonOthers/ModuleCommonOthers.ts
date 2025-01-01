import {NgModule} from '@angular/core';
import {InfrastructureModule} from '../../Infrastructure/Module_INFR';
import {Components, ModuleDeclarations} from './ModuleDeclarations';
import { PdfViewerModule } from 'ng2-pdf-viewer';

@NgModule({
    imports: [InfrastructureModule, PdfViewerModule],
    declarations: [...Components],
    entryComponents: [...Components],
})

export class ModuleCommonOthers {
    public static GetComponent(name: string) {
        return ModuleDeclarations.Get(name);
    }
}