import {NgModule} from '@angular/core';
import {InfrastructureModule} from '../../Infrastructure/Module_INFR';
import {Components, ModuleDeclarations} from './ModuleDeclarations';
import { ControlsModule } from '../../Controls/Module_CTRL';
import { FormsModule } from '@angular/forms';

@NgModule({
    imports: [InfrastructureModule, ControlsModule, FormsModule],
    declarations: [...Components],
    entryComponents: [...Components],
})

export class ModuleInfrastructureCommunications {
    public static GetComponent(name: string) {
        return ModuleDeclarations.Get(name);
    }
}
