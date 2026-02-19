import { NgModule } from '@angular/core';
import { InfrastructureModule } from '../../Infrastructure/Module_INFR';
import { Components, ModuleDeclarations } from './ModuleDeclarations';
import { ModuleCustomsControls } from '../CustomsControls/ModuleCustomsControls';
import { ControlsModule } from '../../Controls/Module_CTRL';

@NgModule({
    imports: [InfrastructureModule, ModuleCustomsControls, ControlsModule],
    exports: [...Components, ModuleCustomsControls],
    declarations: [...Components],
    entryComponents: [...Components],
})

export class ModuleInvoiceQueue {
    public static GetComponent(name: string) {
        return ModuleDeclarations.Get(name);
    }
}
