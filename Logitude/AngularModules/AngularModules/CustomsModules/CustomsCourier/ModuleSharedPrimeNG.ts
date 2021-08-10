
import { TableModule } from 'primeng/table';
//import { SelectItem } from 'primeng/api';
//import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { CalendarModule } from 'primeng/calendar';
import { SliderModule } from 'primeng/slider';
import { MultiSelectModule } from 'primeng/multiselect';
import { ContextMenuModule } from 'primeng/contextmenu';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { ProgressBarModule } from 'primeng/progressbar';
import { InputTextModule } from 'primeng/inputtext';
import { NgModule } from '@angular/core';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { SplitButtonModule } from 'primeng/splitbutton';
//import { BrowserAnimationsModule } from '@angular/platform-browser/animations';


import { TieredMenuModule } from 'primeng/tieredmenu';
import { MenuModule } from 'primeng/menu';
   

@NgModule({
    imports: [TableModule, /*MessageService, */ToastModule, CalendarModule, SliderModule, MultiSelectModule, ContextMenuModule,
        DialogModule, ButtonModule, DropdownModule, ProgressBarModule, AutoCompleteModule, 
        SplitButtonModule, /*BrowserAnimationsModule, */

        TieredMenuModule, MenuModule
    ],

    exports: [TableModule, /*MessageService, */ToastModule, CalendarModule, SliderModule, MultiSelectModule, ContextMenuModule,
        DialogModule, ButtonModule, DropdownModule, ProgressBarModule,
        SplitButtonModule, /*BrowserAnimationsModule, */
        TieredMenuModule, MenuModule, AutoCompleteModule
]
})
export class ModuleSharedPrimeNG { }
