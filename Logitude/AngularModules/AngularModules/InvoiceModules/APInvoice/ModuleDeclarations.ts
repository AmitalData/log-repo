import {NewAPInvoiceComponent} from './Components/NewEntity/NewAPInvoiceComponent';
import {APInvoiceDetailsTabComponent} from './Components/EditTabs/APInvoiceDetailsTabComponent';
import {APInvoiceDocsInTabComponent} from './Components/EditTabs/APInvoiceDocsInTabComponent';
import {APInvoiceDocsOutTabComponent} from './Components/EditTabs/APInvoiceDocsOutTabComponent';
import {APInvoicePaymentsTabComponent} from './Components/EditTabs/APInvoicePaymentsTabComponent';
import {APInvoiceTransferTabComponent} from './Components/EditTabs/APInvoiceTransferTabComponent';
import {APInvoiceDetailsTabNormal} from     './Components/EditTabs/APInvoiceDetailsTabNormal';
import {AddEditAPInvoiceLineComponent} from './Components/EditTabs/AddEditAPInvoiceLineComponent';
import {APInvoiceMultipleDetailsTabComponent} from './Components/EditTabs/APInvoiceMultipleDetailsTabComponent';
import {EditMultipleShipmentComponent} from './Components/EditTabs/EditMultipleShipmentComponent';
import {AddEditMultipleAPInvoiceLineComponent} from './Components/EditTabs/AddEditMultipleAPInvoiceLineComponent';
import {APInvoiceTransferTemplate} from './Components/NewEntity/APInvoiceTransferTemplate';
import {NewGeneralAPInvoiceComponent} from './Components/NewEntity/NewGeneralAPInvoiceComponent';
import {APInvoiceDetailsTabGeneral} from './Components/EditTabs/APInvoiceDetailsTabGeneral';
import {AddEditAPGeneralInvoiceLineComponent } from './Components/EditTabs/AddEditAPGeneralInvoiceLineComponent';
import {APInvoiceTotalVATOnlyComponent} from './Components/Others/APInvoiceTotalVATOnlyComponent';
import {APInvoiceAuditTabComponent} from './Components/EditTabs/APInvoiceAuditTabComponent';
import { APInvoiceGeneralTabComponent } from './Components/EditTabs/APInvoiceGeneralTabComponent';

export const Components =
    [
        NewAPInvoiceComponent,
        APInvoiceDetailsTabComponent,
        APInvoiceMultipleDetailsTabComponent,
        EditMultipleShipmentComponent,
        AddEditMultipleAPInvoiceLineComponent,
        APInvoiceDocsInTabComponent,
        APInvoiceDocsOutTabComponent,
        APInvoicePaymentsTabComponent,
        APInvoiceTransferTabComponent,
        APInvoiceDetailsTabNormal,
        AddEditAPInvoiceLineComponent,
        APInvoiceTransferTemplate,
        NewGeneralAPInvoiceComponent,
        APInvoiceDetailsTabGeneral,
        AddEditAPGeneralInvoiceLineComponent,
        APInvoiceTotalVATOnlyComponent,
        APInvoiceAuditTabComponent,
        APInvoiceGeneralTabComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "NewAPInvoiceComponent": { myResult = NewAPInvoiceComponent; break; }
            case "APInvoiceDetailsTabComponent": { myResult = APInvoiceDetailsTabComponent; break; }
            case "APInvoiceMultipleDetailsTabComponent": { myResult = APInvoiceMultipleDetailsTabComponent; break; }
            case "EditMultipleShipmentComponent": { myResult = EditMultipleShipmentComponent; break; }
            case "AddEditMultipleAPInvoiceLineComponent": { myResult = AddEditMultipleAPInvoiceLineComponent; break; }
            case "APInvoiceDocsInTabComponent": { myResult = APInvoiceDocsInTabComponent; break; }
            case "APInvoiceDocsOutTabComponent": { myResult = APInvoiceDocsOutTabComponent; break; }
            case "APInvoicePaymentsTabComponent": { myResult = APInvoicePaymentsTabComponent; break; }
            case "APInvoiceTransferTabComponent": { myResult = APInvoiceTransferTabComponent; break; }
            case "APInvoiceDetailsTabNormal": { myResult = APInvoiceDetailsTabNormal; break; }
            case "AddEditAPInvoiceLineComponent": { myResult = AddEditAPInvoiceLineComponent; break; }
            case "APInvoiceTransferTemplate": { myResult = APInvoiceTransferTemplate; break; }
            case "NewGeneralAPInvoiceComponent": { myResult = NewGeneralAPInvoiceComponent; break; }
            case "APInvoiceDetailsTabGeneral": { myResult = APInvoiceDetailsTabGeneral; break; }
            case "AddEditAPGeneralInvoiceLineComponent": { myResult = AddEditAPGeneralInvoiceLineComponent; break; }
            case "APInvoiceTotalVATOnlyComponent": { myResult = APInvoiceTotalVATOnlyComponent; break; }
            case "APInvoiceAuditTabComponent": { myResult = APInvoiceAuditTabComponent; break; }
            case "APInvoiceGeneralTabComponent": { myResult = APInvoiceGeneralTabComponent; break; } 
        }

        return myResult;
    }
}
