import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ExportStoragePMService } from 'Customs/Services/StandardPMs/ExportStoragePMService';
import { ExportStorageGeneralTabComponent } from './components/edit/ExportStorageGeneralTabComponent/ExportStorageGeneralTabComponent';
import { LogtuideTableDataService } from 'QuoteOPM/Components/NewEntity/components/autocomplate-table/logtuide-table-data.service';
import { InfrastructureModule } from 'Infrastructure/Module_INFR';
import { ModuleCustomsControls } from 'CustomsModules/CustomsControls/ModuleCustomsControls';



@NgModule({
  declarations: [
    ExportStorageGeneralTabComponent
  ],
  imports: [
    CommonModule,
    InfrastructureModule, 
    ModuleCustomsControls,
  ],
  exports:[
    ExportStorageGeneralTabComponent,
    ModuleCustomsControls
  ],
  entryComponents:[
    ExportStorageGeneralTabComponent
  ],
  providers: [
    ExportStoragePMService,
    LogtuideTableDataService,
  ]
})
export class ExportStorageModule {
  public static GetComponent(name: string) {
    let myResult: any = null;

    switch (name) {
      case "ExportStorageGeneralTabComponent": { myResult = ExportStorageGeneralTabComponent; break; }
    }

    return myResult;
  }
}
