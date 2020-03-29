import { NgModule } from '@angular/core';
import { InfrastructureModule } from '../../Infrastructure/Module_INFR';
import { Components, ModuleDeclarations } from './ModuleDeclarations';
import { ModuleCustomsControls } from '../CustomsControls/ModuleCustomsControls';
@NgModule({
    imports: [InfrastructureModule, ModuleCustomsControls],
    exports: [...Components, ModuleCustomsControls],
    declarations: [...Components],
    entryComponents: [...Components],
})

export class ModuleCustomsReferant {
    public static GetComponent(name: string) {
        return ModuleDeclarations.Get(name);
    }
}
