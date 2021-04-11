import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { InfrastructureModule } from '../Infrastructure/Module_INFR';
import { Components, ModuleDeclarations } from './ModuleDeclarations';
@NgModule({
    imports: [InfrastructureModule],
    declarations: [...Components],
    entryComponents: [...Components],
    schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class ModuleTasksApp {
    public static GetComponent(name: string) {
        return ModuleDeclarations.Get(name);
    }
}
