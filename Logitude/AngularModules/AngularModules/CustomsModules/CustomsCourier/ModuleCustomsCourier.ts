import {NgModule} from '@angular/core';
import {InfrastructureModule} from '../../Infrastructure/Module_INFR';
//import {CustomsModule} from '../../Customs/Module_CUST';
import {Components, ModuleDeclarations} from './ModuleDeclarations';
import { ModuleCustomsControls } from '../CustomsControls/ModuleCustomsControls';
import { ModuleSharedPrimeNG } from './ModuleSharedPrimeNG';
import { PendingWebService } from 'Customs/Services/WebServices/PendingWebService';
import { LogtuideTableDataService } from 'QuoteOPM/Components/NewEntity/components/autocomplate-table/logtuide-table-data.service';
import { CourierWorksheetSharedDataService } from 'Customs/Services/DataChange/CourierWorksheetSharedDataService';
//import { BrowserAnimationsModule } from '@angular/platform-browser/animations';


//import { TableModule } from 'primeng/table';
////import { SelectItem } from 'primeng/api';
////import { MessageService } from 'primeng/api';
//import { ToastModule } from 'primeng/toast';
//import { CalendarModule } from 'primeng/calendar';
//import { SliderModule } from 'primeng/slider';
//import { MultiSelectModule } from 'primeng/multiselect';
//import { ContextMenuModule } from 'primeng/contextmenu';
//import { DialogModule } from 'primeng/dialog';
//import { ButtonModule } from 'primeng/button';
//import { DropdownModule } from 'primeng/dropdown';
//import { ProgressBarModule } from 'primeng/progressbar';
//import { InputTextModule } from 'primeng/inputtext';

/*import { FilterUtils } from 'primeng/utils';*/
//import { LazyLoadEvent } from 'primeng/api';
//import { PrimeNGConfig } from 'primeng/api';


@NgModule({
    imports: [InfrastructureModule, ModuleCustomsControls,
        ModuleSharedPrimeNG
        //TableModule, /*MessageService, */ToastModule, CalendarModule, SliderModule, MultiSelectModule, ContextMenuModule,
        //DialogModule, ButtonModule, DropdownModule, ProgressBarModule,

        ///BrowserAnimationsModule, // npm i @angular/animations@latest --save

    ],
    exports: [...Components, ModuleCustomsControls, ModuleSharedPrimeNG],
    declarations: [...Components],
    entryComponents: [...Components],
    providers: [
        PendingWebService,
        LogtuideTableDataService,
        CourierWorksheetSharedDataService,
    ]
})

export class ModuleCustomsCourier {
    public static GetComponent(name: string) {
        return ModuleDeclarations.Get(name);
    }
}
