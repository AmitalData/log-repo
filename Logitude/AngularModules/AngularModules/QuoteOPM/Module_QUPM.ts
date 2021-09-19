import {NgModule} from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { InfrastructureModule } from '../Infrastructure/Module_INFR';
import { Components, SharedComponents, ModuleDeclarations } from './ModuleDeclarations';
import {ModuleProviders} from './ModuleProviders';
import {AutoCompleteModule} from 'primeng/autocomplete';
import {RadioButtonModule} from 'primeng/radiobutton';
import { InputTextModule } from 'primeng/inputtext';
import {CalendarModule} from 'primeng/calendar';
import {InputNumberModule} from 'primeng/inputnumber';
import {CheckboxModule} from 'primeng/checkbox';
import {ButtonModule} from 'primeng/button';
import {TableModule} from 'primeng/table';

@NgModule({
    imports: [InfrastructureModule, 
        FormsModule,
    ReactiveFormsModule,    
    AutoCompleteModule,
    RadioButtonModule,
    InputTextModule,
    CalendarModule,
    InputNumberModule,
    CheckboxModule,
    ButtonModule,
    TableModule,
    ],
    declarations: [...Components,...SharedComponents],
    entryComponents: [...Components, ...SharedComponents],
    exports: [...SharedComponents],
})
    /// change to QuoteModule
export class QuoteModule {
    public static GetComponent(name: string) {
        return ModuleDeclarations.Get(name);
    }

    public static GetInstance(name: string) {
        return ModuleProviders.GetInstance(name);
    }
}
