import {NgModule} from '@angular/core';
import {InfrastructureModule} from '../Infrastructure/Module_INFR';
import {Components, ModuleDeclarations} from './ModuleDeclarations';
import {ModuleProviders} from './ModuleProviders';

@NgModule({
    imports: [InfrastructureModule],
    declarations: [...Components],
    entryComponents: [...Components],
})

export class LogitudeCommonModule {
    public static Components = Components;

    public static GetComponent(name: string) {
        return ModuleDeclarations.Get(name);
    }

    public static GetInstance(name: string) {
        return ModuleProviders.GetInstance(name);
    }
}