import { NgModule } from '@angular/core';
import { InfrastructureModule } from '../Infrastructure/Module_INFR';
import { Components, ModuleDeclarations } from './ModuleDeclarations';
import { ModuleProviders } from './ModuleProviders';

import { NzTreeSelectModule } from 'ng-zorro-antd/tree-select';

@NgModule({
    imports: [InfrastructureModule, NzTreeSelectModule],
    declarations: [...Components],
    entryComponents: [...Components],
})

export class ModuleWorkflow {
    public static GetComponent(name: string) {
        return ModuleDeclarations.Get(name);
    }

    public static GetInstance(name: string) {
        return ModuleProviders.GetInstance(name);
    }
}