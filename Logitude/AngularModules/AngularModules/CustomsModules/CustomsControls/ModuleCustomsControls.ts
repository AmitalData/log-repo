import {NgModule} from '@angular/core';
import {InfrastructureModule} from '../../Infrastructure/Module_INFR';
//import {CustomsModule} from '../../Customs/Module_CUST';
import {Components, ModuleDeclarations} from './ModuleDeclarations';

@NgModule({
    imports: [InfrastructureModule],
    declarations: [...Components],
    entryComponents: [...Components],
    exports:  [...Components],
})

export class ModuleCustomsControls {
    public static GetComponent(name: string) {
        return ModuleDeclarations.Get(name);
    }
}
