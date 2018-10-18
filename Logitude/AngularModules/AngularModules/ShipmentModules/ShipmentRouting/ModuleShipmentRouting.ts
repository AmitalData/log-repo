import {NgModule} from '@angular/core';
import {InfrastructureModule} from '../../Infrastructure/Module_INFR';
import {Components, ControlsComponents, ModuleDeclarations} from './ModuleDeclarations';

@NgModule({
    imports: [InfrastructureModule],
    declarations: [...Components, ...ControlsComponents],
    entryComponents: [...Components, ...ControlsComponents],
})

export class ModuleShipmentRouting {
    public static GetComponent(name: string) {
        return ModuleDeclarations.Get(name);
    }
}
