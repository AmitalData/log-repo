import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';

export class NewAgent {

    private helper: FieldsHelper;
    private logitudeTab: GeneralFunctions;

    constructor() {
        this.helper = new FieldsHelper();


    }


    QuickSearch() {


        this.helper.WaitBusyIndicator();
        this.helper.WaitByIdAndClick('General.MH.Maintenance');
    }

    SearchAgentTab() {

        this.helper.WaitByIdAndFill('null_Search', "Agent");
        this.helper.WaitByIdAndClick('MaintenanceItemMTAG');

    }

    CreateNewAgent() {
        this.helper.WaitByIdAndClick('NewButton_Agent');
        this.helper.WaitByIdAndFill('Address_Name', "Raghad Company");
        this.helper.WaitByIdAndFill('Address_CountryId', "Poland");
        this.helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.helper.WaitBusyIndicator();
        this.helper.WaitByIdAndFill('Address_City', "Poznań");
        //Ok Button 
        this.helper.WaitByIdAndClick('');




    }


    SearchAgent() {

        this.helper.WaitByIdAndFill('SearchFieldsId_0_1', "Raghad Company")
        //click on row 
        //edit 
        this.helper.WaitByIdAndFill('Agent_Notes', "This Is Test For Protractor")
        this.helper.WaitByIdAndClick('Agent-Save');

    }

}