import { CustomsRequestsSheetsListTemplate } from './Components/CustomsRequestsSheetsListTemplate';
import { DeclarationSupplierInvoiceListTemplate } from './Components/DeclarationSupplierInvoiceListTemplate';
import { CustomsClosedTablesListTemplate } from './Components/CustomsClosedTablesListTemplate';
import { CertificateCheckBoxComponent } from './Components/CertificateCheckBoxComponent';
import { CertificateTextBoxComponent } from './Components/CertificateTextBoxComponent';
import { NotificationListTemplate } from './Components/NotificationListTemplate';
import { SupplierInvoiceItemsTaxListTemplate } from './Components/SupplierInvoiceItemsTaxListTemplate';
import { InterfaceManagementsListTemplate } from './Components/InterfaceManagementsListTemplate';
import { CourierConnectedDeclarationListTemplate } from './Components/CourierConnectedDeclarationListTemplate';
import { SignStationListTemplate } from './Components/SignStationListTemplate';
import { CourierWorksheetListTemplate } from './Components/CourierWorksheetListTemplate';
import { DeclarationQueryListTemplate } from './Components/DeclarationQueryListTemplate';
 
import { CourierDeclarationWorkspaceListTemplate } from './Components/CourierDeclarationWorkspaceListTemplate'
 import { CustomsCollateralListTemplate } from './Components/CustomsCollateralListTemplate';
import { DeclarationAmendmentListTemplate } from './Components/DeclarationAmendmentDeclarationListTemplate';
 
export const Components =
  [
    CustomsRequestsSheetsListTemplate,
    CustomsClosedTablesListTemplate,
    CertificateCheckBoxComponent,
    CertificateTextBoxComponent,
    InterfaceManagementsListTemplate,
    SignStationListTemplate,
    CourierConnectedDeclarationListTemplate,
    CourierWorksheetListTemplate,
    DeclarationSupplierInvoiceListTemplate,
    DeclarationQueryListTemplate,
    NotificationListTemplate,
    SupplierInvoiceItemsTaxListTemplate,
    CustomsCollateralListTemplate,
    CourierDeclarationWorkspaceListTemplate,
    DeclarationAmendmentListTemplate
  ];

export class ModuleDeclarations {
  public static Get(name: string) {

    var myResult: any = null;

      switch (name) {
          case "CustomsRequestsSheetsListTemplate": { myResult = CustomsRequestsSheetsListTemplate; break; }
          case "DeclarationSupplierInvoiceListTemplate": { myResult = DeclarationSupplierInvoiceListTemplate; break; }
          case "CustomsClosedTablesListTemplate": { myResult = CustomsClosedTablesListTemplate; break; }
          case "CertificateCheckBoxComponent": { myResult = CertificateCheckBoxComponent; break; }
          case "CertificateTextBoxComponent": { myResult = CertificateTextBoxComponent; break; }
          case "NotificationListTemplate": { myResult = NotificationListTemplate; break; }
          case "SupplierInvoiceItemsTaxListTemplate": { myResult = SupplierInvoiceItemsTaxListTemplate; break; }
          case "InterfaceManagementsListTemplate": { myResult = InterfaceManagementsListTemplate; break; }
          case "SignStationListTemplate": { myResult = SignStationListTemplate; break; }
          case "CourierWorksheetListTemplate": { myResult = CourierWorksheetListTemplate; break; }
          case "CourierConnectedDeclarationListTemplate": { myResult = CourierConnectedDeclarationListTemplate; break; }
          case "DeclarationQueryListTemplate": { myResult = DeclarationQueryListTemplate; break; }
          case "CustomsCollateralListTemplate": { myResult = CustomsCollateralListTemplate; break; } 
          case "CourierDeclarationWorkspaceListTemplate": { myResult = CourierDeclarationWorkspaceListTemplate; break; }
          case "DeclarationAmendmentListTemplate": { myResult = DeclarationAmendmentListTemplate; break; } 
      }

    return myResult;
  }
}
