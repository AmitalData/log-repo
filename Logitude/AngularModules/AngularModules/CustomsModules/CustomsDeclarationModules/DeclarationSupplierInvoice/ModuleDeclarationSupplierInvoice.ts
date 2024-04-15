import { NgModule } from '@angular/core';
import { InfrastructureModule } from '../../../Infrastructure/Module_INFR';
import { Components, ModuleDeclarations } from './ModuleDeclarations';
import { ModuleCustomsControls } from '../../CustomsControls/ModuleCustomsControls';
// import { LogtuideTableDataService } from 'QuoteOPM/Components/NewEntity/components/autocomplate-table/logtuide-table-data.service';
import { SupplierInvoiceSharedService } from './Components/SupplierInvoices/Services/SupplierInvoiceSharedService';


@NgModule({
  imports: [InfrastructureModule, ModuleCustomsControls],
  exports: [ModuleCustomsControls],
  declarations: [...Components],
  entryComponents: [...Components],
  providers: [
    // LogtuideTableDataService,
    SupplierInvoiceSharedService
  ]
})

export class ModuleDeclarationSupplierInvoice {
  public static GetComponent(name: string) {
    return ModuleDeclarations.Get(name);
  }
}
