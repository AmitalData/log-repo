import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';

export class NewUser {

    private helper: FieldsHelper;
    private logitudeTab: GeneralFunctions;
    private UserName: string;

    constructor() {
        this.helper = new FieldsHelper();
        this.UserName = 'Raghad Company-' + Math.random();
    }

    QuickSearch() {


        this.helper.WaitBusyIndicator();
        this.helper.WaitByIdAndClick('General.MH.Maintenance');
    }

    SearchUserTab() {

        this.helper.WaitByIdAndFill('null_Search', "Agent");
        this.helper.WaitByIdAndClick('MaintenanceItemMTUS');

    }

    CreateNewUser() {
        this.helper.WaitByIdAndClick('NewButton_Agent');
        this.helper.WaitByIdAndFill('Address_Name', this.UserName);
        this.helper.WaitByIdAndFill('Address_CountryId', "Poland");
        this.helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.helper.WaitBusyIndicator();
        this.helper.WaitByIdAndFill('Address_City', "Poznan");
        this.helper.WaitByIdAndClick('OkButtonId');
    }


    SearchUser() {
        this.helper.WaitBusyIndicator();
        this.helper.WaitByIdAndFill('SearchFieldsId_0_0', this.UserName);
        //click on row 
        this.helper.WaitByIdAndClick('LogGrid_0_0row0');
        //edit 
        this.helper.WaitByIdAndFill('Agent_Notes', "This Is Test For Protractor")
        this.helper.WaitByIdAndClick('Agent-Save');

    }

}