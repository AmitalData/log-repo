import { NgModule } from '@angular/core';
import { InfrastructureModule } from '../../Infrastructure/Module_INFR';
import { Components, ModuleDeclarations } from './ModuleDeclarations';
import { ModuleCustomsControls } from '../CustomsControls/ModuleCustomsControls';
import { ControlsModule } from '../../Controls/Module_CTRL';
import { CustomsReportsComponent } from './Components/CustomsReportsComponent';
@NgModule({
    imports: [InfrastructureModule, ModuleCustomsControls, ControlsModule],
    exports: [...Components,ModuleCustomsControls, ControlsModule],
    declarations: [...Components],
    entryComponents: [...Components],
})

export class ModuleCustomsReports {
    public static GetComponent(name: string) {
        return ModuleDeclarations.Get(name);
    }
}
