import {NgModule} from '@angular/core';
import {InfrastructureModule} from '../../Infrastructure/Module_INFR';
import {Components, ModuleDeclarations} from './ModuleDeclarations';

@NgModule({
    imports: [InfrastructureModule],
    declarations: [...Components],
    entryComponents: [...Components],
})

export class ModuleQuoteCharges {
    public static GetComponent(name: string) {
        return ModuleDeclarations.Get(name);
    }
}