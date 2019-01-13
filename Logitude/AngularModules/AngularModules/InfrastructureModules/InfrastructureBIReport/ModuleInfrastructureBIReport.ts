import { NgModule } from '@angular/core';
import { InfrastructureModule } from '../../Infrastructure/Module_INFR';
import { Components, ModuleDeclarations } from './ModuleDeclarations';
import { AgGridModule } from "ag-grid-angular";
@NgModule({
    imports: [InfrastructureModule,AgGridModule.withComponents(null)],
    declarations: [...Components],
    entryComponents: [...Components],
})

export class ModuleInfrastructureBIReport {
    public static GetComponent(name: string) {
        return ModuleDeclarations.Get(name);
    }
}
