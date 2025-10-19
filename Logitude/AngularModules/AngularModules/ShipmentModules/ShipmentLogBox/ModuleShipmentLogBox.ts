import {NgModule} from '@angular/core';
import {InfrastructureModule} from '../../Infrastructure/Module_INFR';
import {Components, ModuleDeclarations} from './ModuleDeclarations';
import {ControlsModule} from '../../Controls/Module_CTRL';

@NgModule({
    imports: [InfrastructureModule, ControlsModule],
    declarations: [...Components],
    entryComponents: [...Components],
})

export class ModuleShipmentLogBox {
    public static GetComponent(name: string) {
        return ModuleDeclarations.Get(name);
    }
}
