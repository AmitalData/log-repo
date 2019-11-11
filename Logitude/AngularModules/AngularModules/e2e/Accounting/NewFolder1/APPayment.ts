import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';
import { PayablesTabComponent } from '../../Operations/Shipments/EditEntity/PayablesTab';
import { OperationsComp } from '../../Operations/Shipments/NewEntity/Operations.po'

export class APPaymentComponent {
    private Helper: FieldsHelper;
    private AccountingTab: GeneralFunctions = new GeneralFunctions();
    private payablesComponent: PayablesTabComponent = new PayablesTabComponent();
    private opp: OperationsComp = new OperationsComp();
    constructor() {
        this.Helper = new FieldsHelper();
    }
    //public shipNumber: string;
    Shipment() {
        var shipNumber;
        shipNumber = this.opp.DoOperations('D', 'Export', 'A', '');
        this.opp.SearchForShipment(shipNumber);
        this.opp.EditShipment(shipNumber);
        this.opp.SaveShip();
        //this.Helper.WaitByIdAndClick('Shipment.TH.Overview');
        //this.Helper.WaitEditComponentBusyIndicator();
        //this.Helper.WaitByIdAndClick('Shipment.TH.Payables');
        //this.Helper.WaitEditComponentBusyIndicator();
        //this.payablesComponent.AddPayableLines(shipNumber, 'D');
        //this.payablesComponent.CreatAPInvoicewithVoid(shipNumber + '1L1', false);
        //this.payablesComponent.EditAPInvoice(shipNumber + '1L1', false);
        
        //this.Helper.WaitByIdAndClick('EditBackbutton_1');

        this.Helper.WaitByIdAndClick('BackButton');
    }
    accounting() {
        this.AccountingTab.GoToMainMenu('General.MH.Accounting');
        this.Helper.WaitByIdAndClick('PAYABLEAccounting');
        this.Helper.WaitByIdAndClick('NewAPPayment');

        
        this.Helper.WaitByIdAndFill('APPayment_VendorId', 'TestAgentExport1');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'APPayment_VendorId', 'TestAgentExport1');

        this.Helper.WaitByIdAndFill('APPayment_AccountingPaymentMethodId', 'cash');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'APPayment_AccountingPaymentMethodId', 'cash');

        this.Helper.WaitByIdAndFill('APPayment_PaymentCurrencyId', 'EU');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'APPayment_PaymentCurrencyId', 'EU');

        this.Helper.WaitByIdAndFill('APPayment_AmountInPaymentCurrency', '500');
        browser.driver.sleep(5000)
    }

}
