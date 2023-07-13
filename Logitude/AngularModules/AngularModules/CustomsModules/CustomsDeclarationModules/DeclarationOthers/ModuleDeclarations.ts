

import { DeclarationPaymentComponent } from './Components/DeclarationPayment/DeclarationPaymentComponent';
import { DeclarationSplitComponent } from './Components/DeclarationSplitComponent';
import { DocumentsPanelComponent } from './Components/DocumentsPanel/DocumentsPanelComponent';
import { VehicleModificationsComponent } from './Components/VehicleModificationsComponent';
import { NewExportDeclarationComponent } from './Components/NewEntity/NewExportDeclarationComponent';
import { NewDeclarationComponent } from './Components/NewEntity/NewDeclarationComponent';
import { SendManifestComponent } from './Components/SendDeclaration/SendManifestComponent';
import { SendDeclarationComponent } from './Components/SendDeclaration/SendDeclarationComponent';
import { SupplierInvoiceSelectionComponent } from './Components/DeclarationPayment/SupplierInvoiceSelectionComponent';
import { PointersFromInvoicesSelectionComponent } from './Components/Documents/PointersFromInvoicesSelectionComponent';
import { DeclarationQueryComponent } from './Components/DeclarationQueryComponent';
import { DeclarationCancellationComponent } from './Components/DeclarationCancellation/DeclarationCancellationComponent';
import { LoadExcelSupplierInvoicesComponent } from './Components/LoadExcelSupplierInvoice/LoadExcelSupplierInvoicesComponent';
import { ExportDeclarationClosingDataComponent } from './Components/CloseDeclaration/ExportDeclarationClosingDataComponent';
import { DeclarationPaymentExportComponent } from './Components/DeclarationPayment/DeclarationPaymentExportComponent';
import { DeclarationFiltersMenuComponent } from './Components/FiltersMenu/DeclarationFiltersMenuComponent';
import { ExportStorageDeclerationComponent } from './Components/ExportStorageDecleration/ExportStorageDeclerationComponent';
import { ExportFilterMenuComponent } from './Components/FiltersMenu/ExportFilterMenuComponent';



export const Components =
    [
        DeclarationPaymentComponent,
        DeclarationPaymentExportComponent,
        DeclarationSplitComponent,
        DocumentsPanelComponent,
        VehicleModificationsComponent,
        NewDeclarationComponent,
        NewExportDeclarationComponent,
        SendDeclarationComponent,
        SupplierInvoiceSelectionComponent,
        PointersFromInvoicesSelectionComponent,
        DeclarationQueryComponent,
        SendManifestComponent,
        DeclarationCancellationComponent,
        LoadExcelSupplierInvoicesComponent,
        ExportDeclarationClosingDataComponent,
        DeclarationFiltersMenuComponent,
        ExportFilterMenuComponent,
        ExportStorageDeclerationComponent
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "DeclarationPaymentComponent": { myResult = DeclarationPaymentComponent; break; }
            case "DeclarationPaymentExportComponent": { myResult = DeclarationPaymentExportComponent; break; }
            case "DeclarationSplitComponent": { myResult = DeclarationSplitComponent; break; }
            case "DocumentsPanelComponent": { myResult = DocumentsPanelComponent; break; }
            case "VehicleModificationsComponent": { myResult = VehicleModificationsComponent; break; }
            case "NewDeclarationComponent": { myResult = NewDeclarationComponent; break; }
            case "NewExportDeclarationComponent": { myResult = NewExportDeclarationComponent; break; }
            case "SendDeclarationComponent": { myResult = SendDeclarationComponent; break; }
            case "SupplierInvoiceSelectionComponent": { myResult = SupplierInvoiceSelectionComponent; break; }
            case "PointersFromInvoicesSelectionComponent": { myResult = PointersFromInvoicesSelectionComponent; break; }
            case "DeclarationQueryComponent": { myResult = DeclarationQueryComponent; break; }
            case "SendManifestComponent": { myResult = SendManifestComponent; break; }
            case "DeclarationCancellationComponent": { myResult = DeclarationCancellationComponent; break; }
            case "LoadExcelSupplierInvoicesComponent": { myResult = LoadExcelSupplierInvoicesComponent; break; }
            case "ExportDeclarationClosingDataComponent": { myResult = ExportDeclarationClosingDataComponent; break; }
            case "DeclarationFiltersMenuComponent": { myResult = DeclarationFiltersMenuComponent; break; }
            case "ExportStorageDeclerationComponent": { myResult = ExportStorageDeclerationComponent; break; }
            case "ExportFilterMenuComponent": { myResult = ExportFilterMenuComponent; break; }

        }

        return myResult;
    }
}
