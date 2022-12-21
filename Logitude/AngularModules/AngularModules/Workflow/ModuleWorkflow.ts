import { NgModule } from '@angular/core';
import { InfrastructureModule } from '../Infrastructure/Module_INFR';
import { Components, ModuleDeclarations, Pipes } from './ModuleDeclarations';
import { ModuleProviders } from './ModuleProviders';

import { NzTreeSelectModule } from 'ng-zorro-antd/tree-select';

import { FormsModule } from '@angular/forms';
import { MentionModule } from 'angular-mentions';

@NgModule({
    imports: [InfrastructureModule, NzTreeSelectModule, FormsModule, MentionModule],
    declarations: [...Components, ...Pipes],
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