import {NgModule} from '@angular/core';
import {InfrastructureModule} from '../Infrastructure/Module_INFR';
import {ControlsModule} from '../Controls/Module_CTRL';
import {Components, ModuleDeclarations} from './ModuleDeclarations';
import {ModuleProviders} from './ModuleProviders';

@NgModule({
    imports: [InfrastructureModule, ControlsModule],
    declarations: [...Components],
    entryComponents: [...Components],
})

export class AccountingModule {
    public static GetComponent(name: string) {
        return ModuleDeclarations.Get(name);
    }

    public static GetInstance(name: string) {
        return ModuleProviders.GetInstance(name);
    }
}