import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { GenericTableModule } from "Infrastructure/Components/generic-table/generic-table.module";
import { NewQuoteOPWebService } from "Customs/Services/WebServices/NewQuoteOPWebService";
import { AccordionModule } from "Infrastructure/Components/accordion/accordion.module";
import { InfrastructureModule } from "Infrastructure/Module_INFR";
import { MessageService } from "primeng/api";
import { AutoCompleteModule } from "primeng/autocomplete";
import { ButtonModule } from "primeng/button";
import { CalendarModule } from "primeng/calendar";
import { CheckboxModule } from "primeng/checkbox";
import { DialogModule } from "primeng/dialog";
import { DropdownModule } from "primeng/dropdown";
import { InputNumberModule } from "primeng/inputnumber";
import { InputTextModule } from "primeng/inputtext";
import { MenuModule } from "primeng/menu";
import { RadioButtonModule } from "primeng/radiobutton";
import { SplitButtonModule } from "primeng/splitbutton";
import { TableModule } from "primeng/table";
import { TabViewModule } from "primeng/tabview";
import { ToastModule } from "primeng/toast";
import { AddressTextareaComponent } from "./components/address-textarea/address-textarea.component";
import { AutocomplateTableComponent } from "./components/autocomplate-table/autocomplate-table.component";
import { BtnMenuPlusComponent } from "./components/btn-menu-plus/btn-menu-plus.component";
import { NewQuoteAddressComponent } from "./components/new-quote-address/new-quote-address.component";
import { NewQuoteExpectedOrderComponent } from "./components/new-quote-expected-order/new-quote-expected-order.component";
import { NewQuoteGeneralComponent } from "./components/new-quote-general/new-quote-general.component";
import { NewQuoteLeftSideComponent } from "./components/new-quote-left-side/new-quote-left-side.component";
import { NewQuoteMyCustomersComponent } from "./components/new-quote-my-customers/new-quote-my-customers.component";
import { NewQuotePartnerComponent } from "./components/new-quote-partner/new-quote-partner.component";
import { NewQuotePropertiesComponent } from "./components/new-quote-properties/new-quote-properties.component";
import { DialogsService } from "./Services/dialogs/dialogs.service";
import { NewQuoteDataService } from "./Services/new-quote-data/new-quote-data.service";
import { NewQuoteValidateEntityService } from "./Services/new-quote-validate-entity/new-quote-validate-entity.service";
import { LogtuideTableDataService } from "./components/autocomplate-table/logtuide-table-data.service";
import { HighlightPipeModule } from "Infrastructure/Pipes/highlight-pipe/highlight-pipe.module";
import { NewQuoteHandleLinkedDataService } from "./Services/new-quote-handle-linked-data/new-quote-handle-linked-data.service";
import { NewQuoteFocusErrorService } from "./Services/new-quote-focus-error/new-quote-focus-error.service";
import { NewQuoteDataShareService } from "./Services/new-quote-data-share/new-quote-data-share.service";

@NgModule({
    imports: [  
        InfrastructureModule,
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
        DropdownModule,
        AccordionModule, 
        HighlightPipeModule,
    ],
    declarations: 
    [
        NewQuoteLeftSideComponent,
        NewQuoteMyCustomersComponent,
        NewQuotePropertiesComponent,
        NewQuoteGeneralComponent,
        NewQuoteExpectedOrderComponent,
        AddressTextareaComponent,
        AutocomplateTableComponent,        
        NewQuoteAddressComponent,
        BtnMenuPlusComponent,
        NewQuotePartnerComponent,
    ],
    entryComponents: [],
    providers: 
    [
        LogtuideTableDataService,
        NewQuoteDataService,
        NewQuoteOPWebService,
        MessageService,
        DialogsService,
        NewQuoteValidateEntityService,
        NewQuoteDataShareService,
        NewQuoteFocusErrorService,
        NewQuoteHandleLinkedDataService,
    ],
    exports:
    [
        NewQuoteLeftSideComponent,
        NewQuoteMyCustomersComponent,
        NewQuotePropertiesComponent,
        NewQuoteGeneralComponent,
        NewQuoteExpectedOrderComponent,
        AddressTextareaComponent,
        AutocomplateTableComponent,
        NewQuoteAddressComponent,
        BtnMenuPlusComponent,
        NewQuotePartnerComponent,
    ],
})

export class quoteOP {

}