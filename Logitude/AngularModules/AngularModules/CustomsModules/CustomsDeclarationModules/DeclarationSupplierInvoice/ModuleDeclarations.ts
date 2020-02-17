


import { EditSupplierInvoiceItem } from './Components/SupplierInvoices/SupplierInvoiceItem/EditSupplierInvoiceItem';
import { SupplierInvoiceItemCertificatesComponent } from './Components/SupplierInvoices/SupplierInvoiceItem/SupplierInvoiceItemCertificatesComponent';
import { DeclarationSupplierInvoiceTabComponent } from './Components/SupplierInvoices/DeclarationSupplierInvoiceTabComponent';
import { VendorExtendedSearchComponent } from './Components/SupplierInvoices/VendorExtendedSearchComponent';
import { UpdateProcessCodeComponent } from './Components/SupplierInvoices/UpdateProcessCodeComponent';
import { UpdateCountryOfOriginComponent } from './Components/SupplierInvoices/UpdateCountryOfOriginComponent';
import { AddEditSupplierInvoiceComponent } from './Components/SupplierInvoices/AddEditSupplierInvoiceComponent';
import { SupplierInvoiceGeneralTabComponent } from './Components/SupplierInvoices/SupplierInvoiceGeneralTabComponent';
import { SupplierInvoiceMoreTabComponent } from './Components/SupplierInvoices/SupplierInvoiceMoreTabComponent';
import { PartnersItemsSelectionComponent } from './Components/SupplierInvoices/PartnersItemsSelectionComponent';
import { MultiCertificateUpdateComponent } from './Components/SupplierInvoices/MultiCertificateUpdate/MultiCertificateUpdateComponent';
import { SupplierInvoiceItemVehicleComponent } from './Components/SupplierInvoices/SupplierInvoiceItem/SupplierInvoiceItemVehicleComponent';
import { VehiclesSearchComponent } from './Components/SupplierInvoices/SupplierInvoiceItem/VehiclesSearchComponent';
import { AddEditActualLinesComponent } from './Components/SupplierInvoices/SupplierInvoiceItem/AddEditActualLinesComponent';





export const Components =
  [
    
    EditSupplierInvoiceItem,
    DeclarationSupplierInvoiceTabComponent,
    SupplierInvoiceItemCertificatesComponent,
    VendorExtendedSearchComponent,
    UpdateProcessCodeComponent,
    UpdateCountryOfOriginComponent,
    AddEditSupplierInvoiceComponent,
    SupplierInvoiceGeneralTabComponent,
    SupplierInvoiceMoreTabComponent,
    PartnersItemsSelectionComponent,
    MultiCertificateUpdateComponent,
    SupplierInvoiceItemVehicleComponent,
    VehiclesSearchComponent,
    AddEditActualLinesComponent,

  ];

export class ModuleDeclarations {
  public static Get(name: string) {

    var myResult: any = null;

    switch (name) {
      case "DeclarationSupplierInvoiceTabComponent": { myResult = DeclarationSupplierInvoiceTabComponent; break; }
      case "EditSupplierInvoiceItem": { myResult = EditSupplierInvoiceItem; break; }
      case "SupplierInvoiceItemCertificatesComponent": { myResult = SupplierInvoiceItemCertificatesComponent; break; }
      case "VendorExtendedSearchComponent": { myResult = VendorExtendedSearchComponent; break; }
      case "UpdateProcessCodeComponent": { myResult = UpdateProcessCodeComponent; break; }
      case "UpdateCountryOfOriginComponent": { myResult = UpdateCountryOfOriginComponent; break; }
      case "AddEditSupplierInvoiceComponent": { myResult = AddEditSupplierInvoiceComponent; break; }
      case "SupplierInvoiceGeneralTabComponent": { myResult = SupplierInvoiceGeneralTabComponent; break; }
      case "SupplierInvoiceMoreTabComponent": { myResult = SupplierInvoiceMoreTabComponent; break; }
      case "PartnersItemsSelectionComponent": { myResult = PartnersItemsSelectionComponent; break; }
      case "SupplierInvoiceItemVehicleComponent": {
        myResult = SupplierInvoiceItemVehicleComponent; break;
      }
      case "VehiclesSearchComponent": {
        myResult = VehiclesSearchComponent; break;
      }

      case "AddEditActualLinesComponent": { myResult = AddEditActualLinesComponent; break; }
      case "MultiCertificateUpdateComponent": { myResult = MultiCertificateUpdateComponent; break; }
    }

    return myResult;
  }
}
