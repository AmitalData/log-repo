import { NgModule } from '@angular/core';
import { InfrastructureModule } from '../../../Infrastructure/Module_INFR';
import { Components, ModuleDeclarations } from './ModuleDeclarations';
import { ModuleCustomsControls } from '../../CustomsControls/ModuleCustomsControls';
import { SupplierInvoiceSharedService } from './Components/SupplierInvoices/Services/SupplierInvoiceSharedService';
import { LogtuideTableDataService } from 'Infrastructure/Services/logtuide-table-data.service';


@NgModule({
  imports: [InfrastructureModule, ModuleCustomsControls],
  exports: [ModuleCustomsControls],
  declarations: [...Components],
  entryComponents: [...Components],
  providers: [
     LogtuideTableDataService,
    SupplierInvoiceSharedService
  ]
})

export class ModuleDeclarationSupplierInvoice {
  public static GetComponent(name: string) {
    return ModuleDeclarations.Get(name);
  }
}
