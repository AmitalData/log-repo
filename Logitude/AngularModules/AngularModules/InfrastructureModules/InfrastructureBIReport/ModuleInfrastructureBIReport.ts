import { NgModule } from '@angular/core';
import { InfrastructureModule } from '../../Infrastructure/Module_INFR';
import { Components, ModuleDeclarations } from './ModuleDeclarations';
import { AgGridModule } from "ag-grid-angular";
import { AGGridCustomHeader } from "./Components/TemplateRenderer/AGGridCustomHeader";

@NgModule({
    imports: [InfrastructureModule, AgGridModule.withComponents([AGGridCustomHeader])],
    declarations: [...Components, AGGridCustomHeader],
    entryComponents: [...Components],
})

export class ModuleInfrastructureBIReport {
    public static GetComponent(name: string) {
        return ModuleDeclarations.Get(name);
    }
}
