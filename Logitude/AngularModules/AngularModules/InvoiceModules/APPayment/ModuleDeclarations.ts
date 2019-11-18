import {APPaymentDetailsTabComponent} from './Components/EditTabs/APPaymentDetailsTabComponent';
import {APPaymentDocsInTabComponent} from './Components/EditTabs/APPaymentDocsInTabComponent';
import {APPaymentDocsOutTabComponent} from './Components/EditTabs/APPaymentDocsOutTabComponent';
import {APPaymentTransferTabComponent} from './Components/EditTabs/APPaymentTransferTabComponent';
import {APPaymentTransferTemplate} from './Components/NewEntity/APPaymentTransferTemplate';
import {APEditMultiCurrency} from './Components/EditTabs/APEditMultiCurrency';
import { EditPaymentChequeComponent } from './Components/EditTabs/EditPaymentChequeComponent';
import { APPaymentGeneralTabComponent } from './Components/EditTabs/APPaymentGeneralTabComponent';

export const Components =
    [
        APPaymentTransferTabComponent,
        APPaymentTransferTemplate,
        APPaymentDetailsTabComponent,
        APPaymentDocsInTabComponent,
        APPaymentDocsOutTabComponent,
        APEditMultiCurrency,
        EditPaymentChequeComponent,
        APPaymentGeneralTabComponent
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "APPaymentDetailsTabComponent": { myResult = APPaymentDetailsTabComponent; break; }
            case "APPaymentDocsInTabComponent": { myResult = APPaymentDocsInTabComponent; break; }
            case "APPaymentDocsOutTabComponent": { myResult = APPaymentDocsOutTabComponent; break; }
            case "APPaymentTransferTabComponent": { myResult = APPaymentTransferTabComponent; break; }
            case "APPaymentTransferTemplate": { myResult = APPaymentTransferTemplate; break; }
            case "APEditMultiCurrency": { myResult = APEditMultiCurrency; break; }
            case "EditPaymentChequeComponent": { myResult = EditPaymentChequeComponent; break; }
            case "APPaymentGeneralTabComponent": { myResult = APPaymentGeneralTabComponent; break; }

                
        }

        return myResult;
    }
}
