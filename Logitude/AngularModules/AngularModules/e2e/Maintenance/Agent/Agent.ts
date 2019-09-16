import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';

export class NewAgent {

    private helper: FieldsHelper;
    private logitudeTab: GeneralFunctions;
    private agentName: string;

    constructor() {
        this.helper = new FieldsHelper();
        this.agentName = 'Raghad Company-' + Math.random();
    }


    QuickSearch() {

        this.helper.WaitBusyIndicator();
        this.helper.WaitByIdAndClick('General.MH.Maintenance');
        this.helper.waitByCss('#null_Search');
        this.helper.WaitBusyIndicator();

    }

    SearchAgentTab() {
        browser.sleep(1000)
        this.helper.WaitByIdAndFill('null_Search', "Agent");
        this.helper.WaitByIdAndClick('MaintenanceItemMTAG');


    }

    CreateNewAgent() {
        this.helper.WaitByIdAndClick('NewButton_Agent');
        this.helper.WaitByIdAndFill('Address_Name', this.agentName);
        this.helper.WaitByIdAndFill('Address_CountryId', "Poland");
        this.helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.helper.WaitBusyIndicator();
        this.helper.WaitByIdAndFill('Address_City', "Poznan");
        browser.sleep(1000)
        this.helper.WaitByIdAndClick('OkButtonId');
    }


    SearchAgent() {

        this.helper.WaitBusyIndicator();
        this.helper.WaitWindowClosed();
        this.helper.WaitByIdAndFill('SearchFieldsId_0_0', this.agentName);
        this.helper.WaitByIdAndClick('LogGrid_0_0row0');
        this.helper.WaitByIdAndFill('Agent_Notes', "This Is Test For Protractor")
        this.helper.WaitByIdAndClick('Agent-Save');

    }
    SaveAgent() {
        this.helper.ItemsVisibility('Agent-Save')
        this.helper.WaitByIdAndClick('Agent-Save');


    }


}
