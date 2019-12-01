import {APPaymentDetailsTabComponent} from './Components/EditTabs/APPaymentDetailsTabComponent';
import {APPaymentDocsInTabComponent} from './Components/EditTabs/APPaymentDocsInTabComponent';
import {APPaymentDocsOutTabComponent} from './Components/EditTabs/APPaymentDocsOutTabComponent';
import {APPaymentTransferTabComponent} from './Components/EditTabs/APPaymentTransferTabComponent';
import {APPaymentTransferTemplate} from './Components/NewEntity/APPaymentTransferTemplate';
import {APEditMultiCurrency} from './Components/EditTabs/APEditMultiCurrency';
import { EditPaymentChequeComponent } from './Components/EditTabs/EditPaymentChequeComponent';
import { APPaymentGeneralTabComponent } from './Components/EditTabs/APPaymentGeneralTabComponent';
import { CancelAPPaymentComponent } from './Components/Other/CancelAPPaymentComponent';
import {APPaymentCancelationDetailsComponent} from './Components/EditTabs/APPaymentCancelationDetailsComponent';
export const Components =
    [
        APPaymentTransferTabComponent,
        APPaymentTransferTemplate,
        APPaymentDetailsTabComponent,
        APPaymentDocsInTabComponent,
        APPaymentDocsOutTabComponent,
        APEditMultiCurrency,
        EditPaymentChequeComponent,
        APPaymentGeneralTabComponent,
        CancelAPPaymentComponent,
        APPaymentCancelationDetailsComponent
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
            case "CancelAPPaymentComponent": { myResult = CancelAPPaymentComponent; break; }
            case "APPaymentCancelationDetailsComponent" :{myResult = APPaymentCancelationDetailsComponent; break;}
        }

        return myResult;
    }
}
