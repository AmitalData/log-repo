import { NgModule } from '@angular/core';
import { InfrastructureModule } from '../Infrastructure/Module_INFR';
import { Components, SharedComponents, ModuleDeclarations } from './ModuleDeclarations';
import { ModuleProviders } from './ModuleProviders';

import { ToastModule } from 'primeng/toast';
import { quoteOP } from './Components/NewEntity/quoteOP.module';
import { ButtonModule } from 'primeng/button';
import { AccordionModule } from 'Infrastructure/Components/accordion/accordion.module';

@NgModule({
    imports: [
        InfrastructureModule,
        quoteOP,
        ToastModule,
        ButtonModule,
        AccordionModule,
    ],
    declarations: [...Components, ...SharedComponents],

    entryComponents: [...Components, ...SharedComponents],
    exports: [...SharedComponents],
})
/// change to QuoteModule
export class QuoteModule {
    public static GetComponent(name: string) {
        return ModuleDeclarations.Get(name);
    }

    public static GetInstance(name: string) {
        return ModuleProviders.GetInstance(name);
    }
}
