import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { browser } from 'protractor';
import { createThisTypeNode } from 'typescript';

export class ShipmentView {
    private helper: FieldsHelper;

    constructor() {
        this.helper = new FieldsHelper();
    }

    OpenShipmentView() {

        this.helper.WaitByIdAndClick('General.MH.Operations');
        this.helper.WaitByIdAndClick('SHIP');
        this.helper.WaitBusyIndicator();
        this.helper.WaitByIdAndClick('Shipments-O-Q');
        this.helper.WaitBusyIndicator();
    }


    CreatNewView() {

        this.helper.WaitByIdAndClick('QueryList_0_0');
        this.helper.WaitByIdAndClick('NewViewId_0_0');
        this.helper.WaitByIdAndClick('NewViewTabchoose');
        this.helper.WaitByIdAndFill("ViewNameId", 'Raghads View');
        this.helper.waitByCss('.ListBoxItem');
        this.helper.WaitByIdAndFill('NewViewSearchFields_0_0', "first");
        this.helper.WaitByCssAndClick_FromTagInsideList('.ListBoxItem', 0);
        this.helper.WaitByIdAndClick("NewButton.View.Add");
        this.helper.WaitBusyIndicator();
        this.helper.WaitByIdAndFill('NewViewSearchFields_0_0', "shipper");
        this.helper.WaitByCssAndClick_FromTagInsideList('.ListBoxItem', 0);
        this.helper.WaitByIdAndClick("NewButton.View.Add");
        this.helper.WaitByIdAndClick("NewButton.View.Create");
        this.helper.WaitWindowClosed();

    }

    EditNewView() {
        browser.sleep(1000)
        this.helper.WaitBusyIndicator()
        this.helper.WaitByIdAndClick('QueryList_0_0');
        this.helper.WaitActionButtonAndClick('ActionButtonsParent', true);
        this.helper.waitByCss('.ListBoxItem');
        this.helper.WaitByIdAndFill('NewViewSearchFields_0_1', "account");
        this.helper.waitByCss('.ListBoxItem');
        this.helper.WaitByCssAndClick_FromTagInsideList('.ListBoxItem', 0);
        this.helper.WaitByIdAndClick("NewButton.View.Add");
        this.helper.WaitByIdAndClick("NewButton.View.Create");
        this.helper.WaitBusyIndicator();
        this.helper.WaitWindowClosed();
    }

    DeleteNewView() {
     
        this.helper.WaitByIdAndClick('QueryList_0_0');
        this.helper.WaitActionButtonAndClick('ActionButtonsParent', false);
        this.helper.waitByCss('.ConfirmWindow');
        this.helper.WaitByIdAndClick('ConfirmWindow_Yes_0');
    }

}
