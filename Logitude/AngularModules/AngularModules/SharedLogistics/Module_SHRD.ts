import {NgModule} from '@angular/core';
import {InfrastructureModule} from '../Infrastructure/Module_INFR';
import {Components, ControlsComponents, ModuleDeclarations} from './ModuleDeclarations';
import {ModuleProviders} from './ModuleProviders';
import { MonacoEditorModule,NgxMonacoEditorConfig } from 'ngx-monaco-editor';
const monacoConfig: NgxMonacoEditorConfig = { 
  defaultOptions: { theme: 'vs-dark', language: 'html'},
  baseUrl: './_Resources', // configure base path cotaining monaco-editor directory after build default: './assets'
 };

@NgModule({
    imports: [InfrastructureModule,MonacoEditorModule.forRoot(monacoConfig)],
    declarations: [...Components, ControlsComponents],
    entryComponents: [...Components, ControlsComponents],
})

export class SharedLogisticsModule {
    public static GetComponent(name: string) {
        return ModuleDeclarations.Get(name);
    }

    public static GetInstance(name: string) {
        return ModuleProviders.GetInstance(name);
    }
}
