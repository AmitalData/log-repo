

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
import { ExportDeclarationClosingDataComponent } from './Components/CloseDeclaration/ExportDeclarationClosingDataComponent';



export const Components =
    [
        DeclarationPaymentComponent,
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
        ExportDeclarationClosingDataComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "DeclarationPaymentComponent": { myResult = DeclarationPaymentComponent; break; }
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
            case "ExportDeclarationClosingDataComponent": { myResult = ExportDeclarationClosingDataComponent; break; }

        }

        return myResult;
    }
}
