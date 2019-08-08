
import { NewPaymentOrderComponent } from './Components/NewEntity/NewPaymentOrderComponent';
import { PaymentOrdersGeneralTabComponent } from './Components/EditTabs/General/PaymentOrdersGeneralTabComponent';
import { AccountingCustomFilesComponent } from './Components/EditTabs/General/AccountingCustomFilesComponent';
import { SendPaymentOrderComponent } from './Components/SendPaymentOrder/SendPaymentOrderComponent';
import { PaymentOrderDepositDataComponent } from './Components/EditTabs/Tapag/Deposit/PaymentOrderDepositDataComponent';
import { PaymentOrderDeficitComponent } from './Components/EditTabs/Tapag/Deficit/PaymentOrderDeficitComponent';
import { BankAccountToRefundComponent } from './Components/EditTabs/Tapag/Deposit/BankAccountToRefundComponent';
import { DeficitDecisionComponent } from './Components/EditTabs/Tapag/Deficit/DeficitDecisionComponent';

export const Components =
  [
        NewPaymentOrderComponent,
        PaymentOrdersGeneralTabComponent,
        AccountingCustomFilesComponent,
        SendPaymentOrderComponent,
        PaymentOrderDepositDataComponent,
        PaymentOrderDeficitComponent,
        BankAccountToRefundComponent,
        DeficitDecisionComponent,

  ];

export class ModuleDeclarations {
  public static Get(name: string) {

    var myResult: any = null;

    switch (name) {
      case "NewPaymentOrderComponent": { myResult = NewPaymentOrderComponent; break; }
      case "PaymentOrdersGeneralTabComponent": { myResult = PaymentOrdersGeneralTabComponent; break; }
      case "AccountingCustomFilesComponent": { myResult = AccountingCustomFilesComponent; break; }
      case "SendPaymentOrderComponent": { myResult = SendPaymentOrderComponent; break; }

      case "PaymentOrderDepositDataComponent": { myResult = PaymentOrderDepositDataComponent; break; }
      case "PaymentOrderDeficitComponent": { myResult = PaymentOrderDeficitComponent; break; }
      case "BankAccountToRefundComponent": { myResult = BankAccountToRefundComponent; break; }
        case "DeficitDecisionComponent": { myResult = DeficitDecisionComponent; break; }
    }

    return myResult;
  }
}
