import {NgModule} from '@angular/core';
import {InfrastructureModule} from '../Infrastructure/Module_INFR';
import {Components, ModuleDeclarations} from './ModuleDeclarations';
import {ModuleProviders} from './ModuleProviders';
import {ModuleCustomsControls} from '../CustomsModules/CustomsControls/ModuleCustomsControls';
import {ModuleCustomsCourier} from '../CustomsModules/CustomsCourier/ModuleCustomsCourier';
import {ModulePhysicalCheck} from '../CustomsModules/CustomsPhysicalCheck/ModulePhysicalCheck';
@NgModule({
    imports: [InfrastructureModule, ModuleCustomsControls, ModuleCustomsCourier, ModulePhysicalCheck],
    exports: [ModuleCustomsControls, ModuleCustomsCourier, ModulePhysicalCheck],
    declarations: [...Components],
    entryComponents: [...Components],
})

export class CustomsModule {
    public static Components = Components;

    public static GetComponent(name: string) {
        return ModuleDeclarations.Get(name);
    }

    public static GetInstance(name: string) {
        return ModuleProviders.GetInstance(name);
    }
}