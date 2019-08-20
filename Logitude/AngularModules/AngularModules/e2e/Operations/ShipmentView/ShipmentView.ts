import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../Helpers/FieldsHelper';

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
        // this.helper.WaitByIdAndClick('ChooseNewView');
        //fill view name
        this.helper.WaitByIdAndFill("ViewNameId",'Raghads View')
        this.helper.WaitByIdAndFill('NewViewSearchFields_0_0', "Agent");
        this.helper.WaitByCssAndClick_FromTagInsideList('.ListBoxItem', 0);
        this.helper.WaitByIdAndClick("NewButton.View.Add");
        this.helper.WaitByIdAndFill('NewViewSearchFields_0_0', "shipper");
        this.helper.WaitByCssAndClick_FromTagInsideList('.ListBoxItem', 0);
        this.helper.WaitByIdAndClick("NewButton.View.Add");
       // this.helper.WaitByIdAndClick("NewButton.View.Create");
       this.helper.WaitByIdAndClick("NewButton.View.Create");
       this.helper.WaitWindowClosed();
       browser.driver.sleep(2000);


        //to fill filters 
     //   this.helper.WaitByIdAndClick("NewTab.View.Filters");
       // this.helper.WaitByIdAndFill("NewViewFiltersSearchFieldsId_0_0", 'Agent');
       // this.helper.WaitByIdAndClick("CheckBox_0_631_LBL");
      //  this.helper.WaitByIdAndFill("Shipment_TextValue", 'Raghad');
      //  this.helper.waitByCss('#LogLovDropDown-Shipment_TextValue');
      //  browser.driver.sleep(1000);
       // this.helper.WaitByCssAndClick_FromTagInsideList(".DropDownList", 0);
      //  this.helper.WaitByIdAndClick("NewButton.View.Create");
       // this.helper.WaitWindowClosed();
      //  browser.driver.sleep(2000);

      //  this.helper.WaitByIdAndClick('QueryList_0_0');
      //  this.helper.ItemsVisibility('ChooseNewView')


    }

    EditNewView(){
       
        this.helper.WaitByIdAndClick('QueryList_0_0');
        // this.helper.waitByCss('.ActionButtonsParent TextTrimming ComboBoxItem Selected')
        this.helper.WaitActionButtonAndClick('ActionButtonsParent', true);
        this.helper.WaitByIdAndFill('NewViewSearchFields_0_1', "Branch");
        this.helper.WaitByCssAndClick_FromTagInsideList('.ListBoxItem', 0);
        this.helper.WaitByIdAndClick("NewButton.View.Add");
        this.helper.WaitByIdAndFill('NewViewSearchFields_0_1', "AWB");
        this.helper.WaitByCssAndClick_FromTagInsideList('.ListBoxItem', 0);
        this.helper.WaitByIdAndClick("NewButton.View.Add");
        this.helper.WaitByIdAndClick("NewButton.View.Create");
        this.helper.WaitWindowClosed();


    }

    DeleteNewView() {
        this.helper.WaitByIdAndClick('QueryList_0_0');
        this.helper.WaitActionButtonAndClick('ActionButtonsParent', false);
        this.helper.WaitByIdAndClick('ConfirmWindow_Yes_0');
    }

    DisplayView() {
        this.helper.WaitByIdAndClick('QueryList_0_0');
        this.helper.WaitByIdAndClick('ChooseNewView');
    }

}