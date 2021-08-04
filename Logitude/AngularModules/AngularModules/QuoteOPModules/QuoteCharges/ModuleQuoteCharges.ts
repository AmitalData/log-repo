import {NgModule} from '@angular/core';
import { QuoteModule } from '../../Quote/Module_QUOT';
import { InfrastructureModule } from '../../Infrastructure/Module_INFR';
import {Components, ModuleDeclarations} from './ModuleDeclarations';

@NgModule({
    imports: [QuoteModule, InfrastructureModule],
    declarations: [...Components],
    entryComponents: [...Components],
})

export class ModuleQuoteCharges {
    public static GetComponent(name: string) {
        return ModuleDeclarations.Get(name);
    }
}
