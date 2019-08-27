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

        this.helper.WaitByIdAndFill('null_Search', "User");
        this.helper.WaitByIdAndClick('MaintenanceItemMTUS');

    }

    CreateNewUser() {
        this.helper.WaitByIdAndClick('NewUserId');
        this.helper.WaitByIdAndFill('User_Email', "test2@mail.com");
        this.helper.WaitByIdAndFill('', "123");
        //retypepassword
        //  this.helper.WaitByIdAndFill('', "Poland");
        this.helper.WaitByIdAndFill('', this.UserName);
        this.helper.WaitByIdAndFill('User_DepartmentId', "Management");
        this.helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.helper.WaitByIdAndFill('User_BranchId', "main");
        this.helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        this.helper.WaitBusyIndicator();
        this.helper.WaitByIdAndClick('CheckBox_0_151_LBL');
        this.helper.WaitByIdAndClick('OkIdButton');
    }


    SearchUser() {
        this.helper.WaitBusyIndicator();
        this.helper.WaitByIdAndFill('User_Search', this.UserName);
        //TO CHOOSE WICH USER YOU WANT TO EDI 
        this.helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        //click on row 
        this.helper.WaitByIdAndFill('User_LocalName',"Raghad");
        this.helper.WaitByIdAndFill('User_Notes', "This Is Test For Protractor")
        this.helper.WaitByIdAndClick('User-Save');

    }

}