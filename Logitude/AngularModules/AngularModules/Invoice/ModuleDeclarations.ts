import {FieldTemplateComponent} from './Components/Templates/FieldTemplateComponent';
import {InvoiceComponent} from './Components/Workspaces/InvoiceComponent';
import {AccountReceivablesComponent} from './Components/Workspaces/AccountReceivablesComponent';
import {AccountPayablesComponent} from './Components/Workspaces/AccountPayablesComponent';
import {AccountingTransferComponent} from './Components/Workspaces/AccountingTransferComponent';
import {RecalculateExternalsComponent} from './Components/Workspaces/Windows/RecalculateExternalsComponent';
import {NewTransferComponent} from './Components/Workspaces/Windows/NewTransferComponent';
import {ExportTransferComponent} from './Components/Workspaces/Windows/ExportTransferComponent';
import {TransferSettingsComponent} from './Components/Workspaces/Windows/TransferSettingsComponent';
import {TransferStartDateComponent} from './Components/Workspaces/Windows/TransferStartDateComponent';
import {PrintTaxComponent} from './Components/Workspaces/Windows/PrintTaxComponent';
import {ExternalAccountingSystemComponent} from './Components/Workspaces/ExternalAccountingSystemComponent';
import {QuickBooksLogin} from './Components/Workspaces/QuickBooksLogin';

import {AccountingTab_AccountingPaymentMethod} from './Components/AccountingTab/AccountingTab_AccountingPaymentMethod'; 
import {AccountingTab_APPaymentMethod} from './Components/AccountingTab/AccountingTab_APPaymentMethod';

// BankAccountLite
import {NewBankAccountLiteComponent} from './Components/NewEntity/NewBankAccountLiteComponent';

// TransferHeader
import {TransferHeaderDetailsTabComponent} from './Components/EditTabs/TransferHeader/TransferHeaderDetailsTabComponent';

// ShortTitle
import {APInvoiceShortTitleComponent} from './Components/ShortTitles/APInvoiceShortTitleComponent';
import {APPaymentShortTitleComponent} from './Components/ShortTitles/APPaymentShortTitleComponent';
import {ARInvoiceShortTitleComponent} from './Components/ShortTitles/ARInvoiceShortTitleComponent';
import {ARPaymentShortTitleComponent} from './Components/ShortTitles/ARPaymentShortTitleComponent';

// Helper
import {APInvoiceHelperComponent} from './Components/Helpers/APInvoiceHelperComponent';
import {APPaymentHelperComponent} from './Components/Helpers/APPaymentHelperComponent';
import {ARInvoiceHelperComponent} from './Components/Helpers/ARInvoiceHelperComponent';
import {ARPaymentHelperComponent} from './Components/Helpers/ARPaymentHelperComponent';
import {AccountingTransferHeaderHelperComponent} from './Components/Helpers/AccountingTransferHeaderHelperComponent';

// Templates
import {ARInvoiceIsPrintedHeaderTemplate} from './Components/ListHeaderTemplates/ARInvoiceIsPrintedHeaderTemplate';
import {ARInvoiceSentHeaderTemplate} from './Components/ListHeaderTemplates/ARInvoiceSentHeaderTemplate';
import {ARInvoiceMenuButtonsComponent} from './Components/MenuButtonsComponents/ARInvoiceMenuButtonsComponent';
import {CreditLimitPopupComponent} from './Components/NewEntity/CreditLimitPopupComponent';
import {SettingsComponent} from './Components/Workspaces/SettingsComponent';
import {SATInterfaceSettingsComponent} from './Components/Workspaces/SATInterfaceSettingsComponent'; 
import {SendPaymentWindowComponent} from './Components/SAT/SendPaymentWindowComponent';

export const Components =
    [
        FieldTemplateComponent,
        InvoiceComponent,
        AccountReceivablesComponent,
        AccountPayablesComponent,
        AccountingTransferComponent,
        RecalculateExternalsComponent,
        NewTransferComponent,
        ExportTransferComponent,
        TransferSettingsComponent,
        TransferStartDateComponent,
        PrintTaxComponent,
        
        NewBankAccountLiteComponent,
        
        APInvoiceShortTitleComponent,
        APPaymentShortTitleComponent,
        ARInvoiceShortTitleComponent,
        ARPaymentShortTitleComponent,
        APInvoiceHelperComponent,
        APPaymentHelperComponent,
        ARInvoiceHelperComponent,
        ARPaymentHelperComponent,
        AccountingTransferHeaderHelperComponent,

        ARInvoiceIsPrintedHeaderTemplate,
        ARInvoiceSentHeaderTemplate,
        ARInvoiceMenuButtonsComponent,
        ExternalAccountingSystemComponent,
        QuickBooksLogin,
        TransferHeaderDetailsTabComponent,
        
        CreditLimitPopupComponent,
        SettingsComponent,
        SATInterfaceSettingsComponent,
        SendPaymentWindowComponent,
        AccountingTab_AccountingPaymentMethod,
        AccountingTab_APPaymentMethod,        
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "FieldTemplateComponent": { myResult = FieldTemplateComponent; break; }
            case "InvoiceComponent": { myResult = InvoiceComponent; break; }
            case "AccountReceivablesComponent": { myResult = AccountReceivablesComponent; break; }
            case "AccountPayablesComponent": { myResult = AccountPayablesComponent; break; }
            case "AccountingTransferComponent": { myResult = AccountingTransferComponent; break; }
            case "RecalculateExternalsComponent": { myResult = RecalculateExternalsComponent; break; }
            case "NewTransferComponent": { myResult = NewTransferComponent; break; }
            case "ExportTransferComponent": { myResult = ExportTransferComponent; break; }
            case "TransferSettingsComponent": { myResult = TransferSettingsComponent; break; }               
            case "TransferStartDateComponent": { myResult = TransferStartDateComponent; break; }
            case "PrintTaxComponent": { myResult = PrintTaxComponent; break; }

            case "NewBankAccountLiteComponent": { myResult = NewBankAccountLiteComponent; break; }
             
            case "APInvoiceShortTitleComponent": { myResult = APInvoiceShortTitleComponent; break; }
            case "APPaymentShortTitleComponent": { myResult = APPaymentShortTitleComponent; break; }
            case "ARInvoiceShortTitleComponent": { myResult = ARInvoiceShortTitleComponent; break; }
            case "ARPaymentShortTitleComponent": { myResult = ARPaymentShortTitleComponent; break; }
            case "APInvoiceHelperComponent": { myResult = APInvoiceHelperComponent; break; }
            case "APPaymentHelperComponent": { myResult = APPaymentHelperComponent; break; }
            case "ARInvoiceHelperComponent": { myResult = ARInvoiceHelperComponent; break; }
            case "ARPaymentHelperComponent": { myResult = ARPaymentHelperComponent; break; }
            case "AccountingTransferHeaderHelperComponent": { myResult = AccountingTransferHeaderHelperComponent; break; }
                
            case "ARInvoiceIsPrintedHeaderTemplate": { myResult = ARInvoiceIsPrintedHeaderTemplate; break; }
            case "ARInvoiceSentHeaderTemplate": { myResult = ARInvoiceSentHeaderTemplate; break; }
            case "ARInvoiceMenuButtonsComponent": { myResult = ARInvoiceMenuButtonsComponent; break; }
            case "ExternalAccountingSystemComponent": { myResult = ExternalAccountingSystemComponent; break; }
            case "QuickBooksLogin": { myResult = QuickBooksLogin; break; }
            case "TransferHeaderDetailsTabComponent": { myResult = TransferHeaderDetailsTabComponent; break; }
                
            case "CreditLimitPopupComponent": { myResult = CreditLimitPopupComponent; break; } 
            case "SettingsComponent": { myResult = SettingsComponent; break; }    
            case "SATInterfaceSettingsComponent": { myResult = SATInterfaceSettingsComponent; break; }
            case "SendPaymentWindowComponent": { myResult = SendPaymentWindowComponent; break; }     
            case "AccountingTab_AccountingPaymentMethod": { myResult = AccountingTab_AccountingPaymentMethod; break; }     
            case "AccountingTab_APPaymentMethod": { myResult = AccountingTab_APPaymentMethod; break; }    
        }

        return myResult;
    }
}