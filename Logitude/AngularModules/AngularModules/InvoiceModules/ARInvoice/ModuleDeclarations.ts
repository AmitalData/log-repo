import {NewARInvoiceComponent} from './Components/NewEntity/NewARInvoiceComponent';
import {NewConsolidationComponent} from './Components/NewEntity/NewConsolidationComponent';
import {NewGeneralARInvoiceComponent} from './Components/NewEntity/NewGeneralARInvoiceComponent';
import {ARInvoiceDetailsTabComponent} from './Components/EditTabs/ARInvoiceDetailsTabComponent';
import {ARInvoiceDetailsTabNormal} from './Components/EditTabs/ARInvoiceDetailsTabNormal';
import {ARInvoiceDetailsTabConsolidation} from './Components/EditTabs/ARInvoiceDetailsTabConsolidation';
import {ARInvoiceDocsInTabComponent} from './Components/EditTabs/ARInvoiceDocsInTabComponent';
import {ARInvoiceDocsOutTabComponent} from './Components/EditTabs/ARInvoiceDocsOutTabComponent';
import {ARInvoicePaymentsTabComponent} from './Components/EditTabs/ARInvoicePaymentsTabComponent';
import {ARInvoiceTransferTabComponent} from './Components/EditTabs/ARInvoiceTransferTabComponent';
import {AddEditARInvoiceLineComponent} from './Components/EditTabs/AddEditARInvoiceLineComponent';
import {ARInvoiceDetailsTabGeneral} from './Components/EditTabs/ARInvoiceDetailsTabGeneral';
import {AddEditARGeneralInvoiceLineComponent} from './Components/EditTabs/AddEditARGeneralInvoiceLineComponent';
import {ARInvoiceTransferTemplate} from './Components/NewEntity/ARInvoiceTransferTemplate';
import {ARInvoiceGeneralTabComponent} from './Components/EditTabs/ARInvoiceGeneralTabComponent';

export const Components =
    [
        NewARInvoiceComponent,
        NewConsolidationComponent,
        NewGeneralARInvoiceComponent,
        ARInvoiceDetailsTabComponent,
        ARInvoiceDetailsTabNormal,
        ARInvoiceDetailsTabConsolidation,
        ARInvoiceDocsInTabComponent,
        ARInvoiceDocsOutTabComponent,
        ARInvoicePaymentsTabComponent,
        ARInvoiceTransferTabComponent,
        AddEditARInvoiceLineComponent,
        ARInvoiceDetailsTabGeneral,
        AddEditARGeneralInvoiceLineComponent,
        ARInvoiceTransferTemplate,
        ARInvoiceGeneralTabComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "NewARInvoiceComponent": { myResult = NewARInvoiceComponent; break; }
            case "NewConsolidationComponent": { myResult = NewConsolidationComponent; break; }
            case "NewGeneralARInvoiceComponent": { myResult = NewGeneralARInvoiceComponent; break; }
            case "ARInvoiceDetailsTabComponent": { myResult = ARInvoiceDetailsTabComponent; break; }
            case "ARInvoiceDetailsTabNormal": { myResult = ARInvoiceDetailsTabNormal; break; }
            case "ARInvoiceDetailsTabConsolidation": { myResult = ARInvoiceDetailsTabConsolidation; break; }
            case "ARInvoiceDocsInTabComponent": { myResult = ARInvoiceDocsInTabComponent; break; }
            case "ARInvoiceDocsOutTabComponent": { myResult = ARInvoiceDocsOutTabComponent; break; }
            case "ARInvoicePaymentsTabComponent": { myResult = ARInvoicePaymentsTabComponent; break; }
            case "ARInvoiceTransferTabComponent": { myResult = ARInvoiceTransferTabComponent; break; }
            case "AddEditARInvoiceLineComponent": { myResult = AddEditARInvoiceLineComponent; break; }
            case "ARInvoiceDetailsTabGeneral": { myResult = ARInvoiceDetailsTabGeneral; break; }
            case "AddEditARGeneralInvoiceLineComponent": { myResult = AddEditARGeneralInvoiceLineComponent; break; }
            case "ARInvoiceTransferTemplate": { myResult = ARInvoiceTransferTemplate; break; } 
            case "ARInvoiceGeneralTabComponent": { myResult = ARInvoiceGeneralTabComponent; break; }  
        }

        return myResult;
    }
}