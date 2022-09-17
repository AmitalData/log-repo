import { NgModule } from '@angular/core';
import { InfrastructureModule } from '../Infrastructure/Module_INFR';
import { Components, ModuleDeclarations } from './ModuleDeclarations';
import { ModuleProviders } from './ModuleProviders';
import { AgGridModule } from 'ag-grid-angular';

@NgModule({
    imports: [InfrastructureModule, AgGridModule],
    declarations: [...Components],
    entryComponents: [...Components],
})

export class DashboardModule {
    public static GetComponent(name: string) {
        return ModuleDeclarations.Get(name);
    }

    public static GetInstance(name: string) {
        return ModuleProviders.GetInstance(name);
    }
}
