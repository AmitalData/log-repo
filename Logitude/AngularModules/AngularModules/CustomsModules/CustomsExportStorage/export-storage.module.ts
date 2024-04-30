import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ExportStoragePMService } from 'Customs/Services/StandardPMs/ExportStoragePMService';
import { ExportStorageGeneralTabComponent } from './components/edit/ExportStorageGeneralTabComponent/ExportStorageGeneralTabComponent';
// import { LogtuideTableDataService } from 'QuoteOPM/Components/NewEntity/components/autocomplate-table/logtuide-table-data.service';
import { InfrastructureModule } from 'Infrastructure/Module_INFR';
import { ModuleCustomsControls } from 'CustomsModules/CustomsControls/ModuleCustomsControls';
import { FeedbackToStorageTabComponent } from './components/edit/FeedbackToStorageTabComponent/FeedbackToStorageTabComponent.component';
import { Xml2jsonService } from 'Infrastructure/Services/xml2json/xml2json.service';
import { LogtuideTableDataService } from 'Infrastructure/Services/logtuide-table-data.service';



@NgModule({
  declarations: [
    ExportStorageGeneralTabComponent,
    FeedbackToStorageTabComponent,
  ],
  imports: [
    CommonModule,
    InfrastructureModule, 
    ModuleCustomsControls,
  ],
  exports:[
    ExportStorageGeneralTabComponent,
    FeedbackToStorageTabComponent,
    ModuleCustomsControls,
  ],
  entryComponents:[
    ExportStorageGeneralTabComponent,
    FeedbackToStorageTabComponent,
  ],
  providers: [
    ExportStoragePMService,
     LogtuideTableDataService,
    Xml2jsonService,
  ]
})
export class ExportStorageModule {
  public static GetComponent(name: string) {
    let myResult: any = null;

    switch (name) {
      case "ExportStorageGeneralTabComponent": { myResult = ExportStorageGeneralTabComponent; break; }
      case "FeedbackToStorageTabComponent": { myResult = FeedbackToStorageTabComponent; break; }
    }

    return myResult;
  }
}
