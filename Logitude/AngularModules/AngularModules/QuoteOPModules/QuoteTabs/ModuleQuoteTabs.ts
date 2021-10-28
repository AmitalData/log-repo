import {NgModule} from '@angular/core';
import { quoteOP } from 'QuoteOPM/Components/NewEntity/quoteOP.module';
import {InfrastructureModule} from '../../Infrastructure/Module_INFR';
import {Components, ModuleDeclarations} from './ModuleDeclarations';

@NgModule({
    imports: [InfrastructureModule,quoteOP],
    declarations: [...Components],
    entryComponents: [...Components],
})

export class ModuleQuoteTabs {
    public static GetComponent(name: string) {
        return ModuleDeclarations.Get(name);
    }
}