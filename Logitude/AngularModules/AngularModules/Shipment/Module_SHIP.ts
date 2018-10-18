import {NgModule} from '@angular/core';
import {InfrastructureModule} from '../Infrastructure/Module_INFR';
import {Components, ControlsComponents, ModuleDeclarations} from './ModuleDeclarations';
import {ModuleProviders} from './ModuleProviders';

@NgModule({
    imports: [InfrastructureModule],
    declarations: [...Components, ControlsComponents],
    entryComponents: [...Components, ControlsComponents],
})

export class Shipment_Module {
    public static GetComponent(name: string) {
        return ModuleDeclarations.Get(name);
    }

    public static GetInstance(name: string) {
        return ModuleProviders.GetInstance(name);
    }
}
