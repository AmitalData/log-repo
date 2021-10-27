import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { InfrastructureModule } from '../Infrastructure/Module_INFR';
import { Components, SharedComponents, ModuleDeclarations } from './ModuleDeclarations';
import { ModuleProviders } from './ModuleProviders';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { RadioButtonModule } from 'primeng/radiobutton';
import { InputTextModule } from 'primeng/inputtext';
import { CalendarModule } from 'primeng/calendar';
import { InputNumberModule } from 'primeng/inputnumber';
import { CheckboxModule } from 'primeng/checkbox';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { DialogModule } from 'primeng/dialog';
import { NewQuoteDataService } from './Components/NewEntity/Services/new-quote-data/new-quote-data.service';
import { NewQuoteOPWebService } from 'Customs/Services/WebServices/NewQuoteOPWebService';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { GenericTableModule } from 'Customs/Components/generic-table/generic-table.module';
import {TabViewModule} from 'primeng/tabview';
import {SplitButtonModule} from 'primeng/splitbutton';
import { MenuModule } from 'primeng/menu';
import { DropdownModule } from 'primeng/dropdown'
import { DialogsService } from './Components/NewEntity/Services/dialogs/dialogs.service';
import { NewQuoteValidateEntityService } from './Components/NewEntity/Services/new-quote-validate-entity/new-quote-validate-entity.service';
import { CommonModule } from '@angular/common';

@NgModule({
    imports: [InfrastructureModule,
        CommonModule,
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
        DialogModule,
        ToastModule,
        GenericTableModule,
        TabViewModule,
        SplitButtonModule,
        MenuModule,
        DropdownModule
    ],
    declarations: [...Components, ...SharedComponents],
    providers: [
        NewQuoteDataService,
        NewQuoteOPWebService,
        MessageService,
        DialogsService,
        NewQuoteValidateEntityService,
    ],
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
