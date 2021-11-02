import { NgModule } from '@angular/core';
import { InfrastructureModule } from '../../Infrastructure/Module_INFR';
import { Components, ModuleDeclarations } from './ModuleDeclarations';
import { ModuleCustomsControls } from '../CustomsControls/ModuleCustomsControls';
import { CustomsReportsComponent } from './Components/CustomsReportsComponent';
@NgModule({
    imports: [InfrastructureModule, ModuleCustomsControls],
    exports:  [ModuleCustomsControls],
    declarations: [CustomsReportsComponent],
})

export class ModuleCustomsReports {
    public static GetComponent(name: string) {
        return ModuleDeclarations.Get(name);
    }
}
