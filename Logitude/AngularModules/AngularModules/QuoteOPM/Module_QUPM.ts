import {NgModule} from '@angular/core';
import { InfrastructureModule } from '../Infrastructure/Module_INFR';
import { Components, SharedComponents, ModuleDeclarations } from './ModuleDeclarations';
import {ModuleProviders} from './ModuleProviders';

@NgModule({
    imports: [InfrastructureModule],
    declarations: [...Components,...SharedComponents],
    entryComponents: [...Components, ...SharedComponents],
    exports: [...SharedComponents],
})

export class QuoteModule {
    public static GetComponent(name: string) {
        return ModuleDeclarations.Get(name);
    }

    public static GetInstance(name: string) {
        return ModuleProviders.GetInstance(name);
    }
}
