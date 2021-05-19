import {NgModule} from '@angular/core';
import {InfrastructureModule} from '../../Infrastructure/Module_INFR';
//import {CustomsModule} from '../../Customs/Module_CUST';
import {Components, ModuleDeclarations} from './ModuleDeclarations';
import {ModuleCustomsControls} from '../CustomsControls/ModuleCustomsControls';
@NgModule({
    imports: [InfrastructureModule, ModuleCustomsControls],
    exports: [...Components,ModuleCustomsControls],
    declarations: [...Components],
    entryComponents: [...Components],
})

export class ModuleCustomsContainerization {
    public static GetComponent(name: string) {
        return ModuleDeclarations.Get(name);
    }
}
