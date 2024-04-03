import { NgModule } from '@angular/core';
import { InfrastructureModule } from '../Infrastructure/Module_INFR';
import { Components, ModuleDeclarations, Directives } from './ModuleDeclarations';
import { ModuleProviders } from './ModuleProviders';

@NgModule({
    imports: [InfrastructureModule],
    declarations: [...Directives, ...Components],
    entryComponents: [...Components],
    exports: [...Directives],
})

export class DashboardModule {
    public static GetComponent(name: string) {
        return ModuleDeclarations.Get(name);
    }

    public static GetInstance(name: string) {
        return ModuleProviders.GetInstance(name);
    }
}