import {AccountingSystemsSettingListService} from './Services/StandardLists/AccountingSystemsSettingListService';
import {AccountingSystemsSyncStatusListService} from './Services/StandardLists/AccountingSystemsSyncStatusListService';
import {AccountingTransferHeaderListService} from './Services/StandardLists/AccountingTransferHeaderListService';
import {AccountListService} from './Services/StandardLists/AccountListService';
import {AccountTypeListService} from './Services/StandardLists/AccountTypeListService';
import {APInvoiceListService} from './Services/StandardLists/APInvoiceListService';
import {APInvoiceStatusListService} from './Services/StandardLists/APInvoiceStatusListService';
import {APInvoiceTransferStatusListService} from './Services/StandardLists/APInvoiceTransferStatusListService';
import {APInvoiceTypeListService} from './Services/StandardLists/APInvoiceTypeListService';
import {APPaymentListService} from './Services/StandardLists/APPaymentListService';
import {APPaymentStatusListService} from './Services/StandardLists/APPaymentStatusListService';
import {ARInvoiceListService} from './Services/StandardLists/ARInvoiceListService';
import {ARInvoiceStatusListService} from './Services/StandardLists/ARInvoiceStatusListService';
import {ARInvoicesSignedStatusListService} from './Services/StandardLists/ARInvoicesSignedStatusListService';
import {ARInvoiceTransferStatusListService} from './Services/StandardLists/ARInvoiceTransferStatusListService';
import {ARInvoiceTypeListService} from './Services/StandardLists/ARInvoiceTypeListService';
import {ARPaymentListService} from './Services/StandardLists/ARPaymentListService';
import {ARPaymentStatusListService} from './Services/StandardLists/ARPaymentStatusListService';
import {CreditCardTypeListService} from './Services/StandardLists/CreditCardTypeListService';
import {ExternalSystemsTablesCodeListService} from './Services/StandardLists/ExternalSystemsTablesCodeListService';
import { ARPaymentTransferStatusListService } from './Services/StandardLists/ARPaymentTransferStatusListService';
import { APPaymentTransferStatusListService } from './Services/StandardLists/APPaymentTransferStatusListService';
import {SATInterfaceListService} from './Services/StandardLists/SATInterfaceListService';
import {SATPaymentMethodListService} from './Services/StandardLists/SATPaymentMethodListService';
import {SATTransferStatusListService} from './Services/StandardLists/SATTransferStatusListService';
import {SATInvoiceStatusListService} from './Services/StandardLists/SATInvoiceStatusListService';
import {AccountingPaymentMethodPMService} from './Services/StandardPMs/AccountingPaymentMethodPMService';
import {AccountingPaymentMethodListService} from './Services/StandardLists/AccountingPaymentMethodListService';
import { ARInvoiceStockPMService } from './Services/StandardPMs/ARInvoiceStockPMService';
import {AccountingTransferHeaderPMService} from './Services/StandardPMs/AccountingTransferHeaderPMService';
import {APInvoicePMService} from './Services/StandardPMs/APInvoicePMService';
import {APPaymentPMService} from './Services/StandardPMs/APPaymentPMService';
import {ARInvoicePMService} from './Services/StandardPMs/ARInvoicePMService';
import {ARPaymentPMService} from './Services/StandardPMs/ARPaymentPMService';
import {CreditCardTypePMService} from './Services/StandardPMs/CreditCardTypePMService';
import {SATInterfaceSettingPMService} from './Services/StandardPMs/SATInterfaceSettingPMService';
import {BankAccountLitePMService} from './Services/StandardPMs/BankAccountLitePMService';
import {BankAccountLiteListService} from './Services/StandardLists/BankAccountLiteListService';

import {APInvoiceMenuButtonsHandler} from './Components/MenuButtons/APInvoiceMenuButtonsHandler';
import {APPaymentMenuButtonsHandler} from './Components/MenuButtons/APPaymentMenuButtonsHandler';
import {ARInvoiceMenuButtonsHandler} from './Components/MenuButtons/ARInvoiceMenuButtonsHandler';
import { ARPaymentMenuButtonsHandler } from './Components/MenuButtons/ARPaymentMenuButtonsHandler';
import { ARInvoiceStockMenuButtonsHandler } from './Components/MenuButtons/ARInvoiceStockMenuButtonsHandler';
import { ARInvoiceExtendedService } from './Services/ExtendedPMs/ARInvoiceExtendedService';
import { QBOGlobalTaxCalculationListService } from './Services/StandardLists/QBOGlobalTaxCalculationListService';
import { ConfirmationNumberDefaultListService } from './Services/StandardLists/ConfirmationNumberDefaultListService';
import { ConfirmationNumberDefaultPMService } from './Services/StandardPMs/ConfirmationNumberDefaultPMService';
import { ConfirmationNumberStatusListService } from './Services/StandardLists/ConfirmationNumberStatusListService';
import { ConfirmationNumberDefaultExtendedService } from './Services/ExtendedPMs/ConfirmationNumberDefaultExtendedService';
import { MasavInterfaceListService } from './Services/StandardLists/MasavInterfaceListService';
import { MasavInterfaceStatusListService } from './Services/StandardLists/MasavInterfaceStatusListService';
import { MasavInterfacePMService } from './Services/StandardPMs/MasavInterfacePMService';
import { MasavInterfaceMenuButtonsHandler } from './Components/MenuButtons/MasavInterfaceMenuButtonsHandler';
import { APPaymentExtendedService } from './Services/ExtendedPMs/APPaymentExtendedService';

export class ModuleProviders {
    public static GetInstance(name: string) {

        var myResult: any = null;

        switch (name) {
            case "AccountingSystemsSettingListService": { myResult = new AccountingSystemsSettingListService(); break; }
            case "AccountingSystemsSyncStatusListService": { myResult = new AccountingSystemsSyncStatusListService(); break; }
            case "AccountingTransferHeaderListService": { myResult = new AccountingTransferHeaderListService(); break; }
            case "AccountListService": { myResult = new AccountListService(); break; }
            case "AccountTypeListService": { myResult = new AccountTypeListService(); break; }
            case "APInvoiceListService": { myResult = new APInvoiceListService(); break; }
            case "APInvoiceStatusListService": { myResult = new APInvoiceStatusListService(); break; }
            case "APInvoiceTransferStatusListService": { myResult = new APInvoiceTransferStatusListService(); break; }
            case "ARPaymentTransferStatusListService": { myResult = new ARPaymentTransferStatusListService(); break; }               
            case "APInvoiceTypeListService": { myResult = new APInvoiceTypeListService(); break; }
            case "APPaymentListService": { myResult = new APPaymentListService(); break; }
            case "APPaymentStatusListService": { myResult = new APPaymentStatusListService(); break; }
            case "ARInvoiceListService": { myResult = new ARInvoiceListService(); break; }
            case "ARInvoiceStatusListService": { myResult = new ARInvoiceStatusListService(); break; }
            case "ARInvoicesSignedStatusListService": { myResult = new ARInvoicesSignedStatusListService(); break; }
            case "ARInvoiceTransferStatusListService": { myResult = new ARInvoiceTransferStatusListService(); break; }
            case "ARInvoiceTypeListService": { myResult = new ARInvoiceTypeListService(); break; }
            case "ARPaymentListService": { myResult = new ARPaymentListService(); break; }
            case "ARPaymentStatusListService": { myResult = new ARPaymentStatusListService(); break; }
            case "CreditCardTypeListService": { myResult = new CreditCardTypeListService(); break; }
            case "ExternalSystemsTablesCodeListService": { myResult = new ExternalSystemsTablesCodeListService(); break; }           
            case "AccountingTransferHeaderPMService": { myResult = new AccountingTransferHeaderPMService(); break; }
            case "APInvoicePMService": { myResult = new APInvoicePMService(); break; }
            case "APPaymentPMService": { myResult = new APPaymentPMService(); break; }
            case "ARInvoicePMService": { myResult = new ARInvoicePMService(); break; }
            case "ARPaymentPMService": { myResult = new ARPaymentPMService(); break; }
            case "CreditCardTypePMService": { myResult = new CreditCardTypePMService(); break; }
            case "SATInterfaceSettingPMService": { myResult = new SATInterfaceSettingPMService(); break; }
            case "SATInterfaceListService": { myResult = new SATInterfaceListService(); break; }
            case "SATPaymentMethodListService": { myResult = new SATPaymentMethodListService(); break; }
            case "SATTransferStatusListService": { myResult = new SATTransferStatusListService(); break; }
            case "BankAccountLiteListService": { myResult = new BankAccountLiteListService(); break; }
            case "BankAccountLitePMService": { myResult = new BankAccountLitePMService(); break; }
            case "SATInvoiceStatusListService": { myResult = new SATInvoiceStatusListService(); break; }
            case "AccountingPaymentMethodPMService": { myResult = new AccountingPaymentMethodPMService(); break; }
            case "AccountingPaymentMethodListService": { myResult = new AccountingPaymentMethodListService(); break; }
            case "ConfirmationNumberDefaultListService": { myResult = new ConfirmationNumberDefaultListService(); break; }
            case "ConfirmationNumberDefaultPMService": { myResult = new ConfirmationNumberDefaultPMService(); break; }
            case "ConfirmationNumberStatusListService": { myResult = new ConfirmationNumberStatusListService(); break; }

            case "APPaymentTransferStatusListService": { myResult = new APPaymentTransferStatusListService(); break; }
            case "ARInvoiceStockPMService": { myResult = new ARInvoiceStockPMService(); break; }
            case "ARInvoiceExtendedService": { myResult = new ARInvoiceExtendedService(); break; }
            case "ConfirmationNumberDefaultExtendedService": { myResult = new ConfirmationNumberDefaultExtendedService(); break; }

            case "APInvoiceMenuButtonsHandler": { myResult = new APInvoiceMenuButtonsHandler(); break; }
            case "APPaymentMenuButtonsHandler": { myResult = new APPaymentMenuButtonsHandler(); break; }
            case "ARInvoiceMenuButtonsHandler": { myResult = new ARInvoiceMenuButtonsHandler(); break; }
            case "ARPaymentMenuButtonsHandler": { myResult = new ARPaymentMenuButtonsHandler(); break; }
            case "ARInvoiceStockMenuButtonsHandler": { myResult = new ARInvoiceStockMenuButtonsHandler(); break; } 
            case "QBOGlobalTaxCalculationListService": { myResult = new QBOGlobalTaxCalculationListService(); break; } 
            case "MasavInterfaceListService": { myResult = new MasavInterfaceListService(); break; }           
            case "MasavInterfaceStatusListService": { myResult = new MasavInterfaceStatusListService(); break; }           
            case "MasavInterfacePMService": { myResult = new MasavInterfacePMService(); break; }           
            case "MasavInterfaceMenuButtonsHandler": { myResult = new MasavInterfaceMenuButtonsHandler(); break; }           
            case "APPaymentExtendedService": { myResult = new APPaymentExtendedService(); break; }
        }

        return myResult;
    }
}
