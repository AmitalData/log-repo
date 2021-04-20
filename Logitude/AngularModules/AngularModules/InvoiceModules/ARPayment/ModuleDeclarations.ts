import {NewARPaymentComponent} from './Components/NewEntity/NewARPaymentComponent';
import {ARPaymentGeneralTabComponent} from './Components/EditTabs/ARPaymentGeneralTabComponent';
import {ARPaymentDetailsTabComponent} from './Components/EditTabs/ARPaymentDetailsTabComponent';
import {ARPaymentDocsInTabComponent} from './Components/EditTabs/ARPaymentDocsInTabComponent';
import {ARPaymentDocsOutTabComponent} from './Components/EditTabs/ARPaymentDocsOutTabComponent';
import {ARPaymentTransferTabComponent} from './Components/EditTabs/ARPaymentTransferTabComponent';
import {ARPaymentTransferTemplate} from './Components/NewEntity/ARPaymentTransferTemplate';
import {EditMultiCurrency} from './Components/EditTabs/EditMultiCurrency';
import {ARPaymentDetailsFullAccountingTab} from './Components/EditTabs/ARPaymentDetailsFullAccountingTab';
import {CancelARPaymentComponent} from './Components/Other/CancelARPaymentComponent';
import { ARPaymentMultiChequesComponent } from './Components/Other/ARPaymentMultiChequesComponent';
export const Components =
    [
        NewARPaymentComponent,
        ARPaymentDetailsTabComponent,
        ARPaymentDocsInTabComponent,
        ARPaymentDocsOutTabComponent,
        ARPaymentTransferTabComponent,
        ARPaymentTransferTemplate,
        EditMultiCurrency,
        ARPaymentGeneralTabComponent,
        ARPaymentDetailsFullAccountingTab,
        CancelARPaymentComponent,
        ARPaymentMultiChequesComponent
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "NewARPaymentComponent": { myResult = NewARPaymentComponent; break; }
            case "ARPaymentDetailsTabComponent": { myResult = ARPaymentDetailsTabComponent; break; }
            case "ARPaymentDocsInTabComponent": { myResult = ARPaymentDocsInTabComponent; break; }
            case "ARPaymentDocsOutTabComponent": { myResult = ARPaymentDocsOutTabComponent; break; }
            case "ARPaymentTransferTabComponent": { myResult = ARPaymentTransferTabComponent; break; }
            case "ARPaymentTransferTemplate": { myResult = ARPaymentTransferTemplate; break; }
            case "EditMultiCurrency": { myResult = EditMultiCurrency; break; }
            case "ARPaymentGeneralTabComponent": { myResult = ARPaymentGeneralTabComponent; break; }
            case "CancelARPaymentComponent": { myResult = CancelARPaymentComponent; break; }
            case "ARPaymentDetailsFullAccountingTab": { myResult = ARPaymentDetailsFullAccountingTab; break; }
            case "ARPaymentMultiChequesComponent": { myResult = ARPaymentMultiChequesComponent; break; }
        }

        return myResult;
    }
}
