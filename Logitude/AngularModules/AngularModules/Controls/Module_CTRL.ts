import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Components, Pipes, Directives, ModuleDeclarations } from './ModuleDeclarations';
import {ModuleProviders} from './ModuleProviders';

@NgModule({
    imports: [CommonModule, FormsModule, ReactiveFormsModule],
    declarations: [...Components, ...Pipes, ...Directives],
    exports: [...Components, ...Pipes, ...Directives, CommonModule, FormsModule, ReactiveFormsModule],
    entryComponents: [...Components],
})

export class ControlsModule {
    public static GetComponent(name: string) {
        return ModuleDeclarations.Get(name);
    }

    public static GetInstance(name: string) {
        return ModuleProviders.GetInstance(name);
    }
}
