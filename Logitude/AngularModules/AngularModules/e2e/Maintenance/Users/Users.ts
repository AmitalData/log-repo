import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';

export class NewUser {

    private helper: FieldsHelper;
    private logitudeTab: GeneralFunctions;
    private UserName: string;

    constructor() {
        this.helper = new FieldsHelper();
        this.UserName = 'RaghadUser-' + Math.random();
    }

    QuickSearch() {


        this.helper.WaitBusyIndicator();
        this.helper.WaitByIdAndClick('General.MH.Maintenance');
        this.helper.waitByCss('#null_Search');
    }

    SearchUserTab() {

        this.helper.WaitByIdAndFill('null_Search', "User");
        this.helper.WaitByIdAndClick('MaintenanceItemMTUS');

    }

    CreateNewUser() {
        this.helper.WaitByIdAndClick('NewUserId');
        this.helper.WaitByIdAndFill('User_Email', this.UserName+"@mail.com");
        this.helper.WaitByIdAndFill('PasswordId', "123");
        this.helper.WaitByIdAndFill('retypePass',"123")
        //retypepassword
        //  this.helper.WaitByIdAndFill('', "Poland");
        this.helper.WaitByIdAndFill('User_EnglishName', this.UserName);
        this.helper.WaitByIdAndFill('User_DepartmentId', "Management");
        this.helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.helper.WaitByIdAndFill('User_BranchId', "main");
        this.helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.helper.WaitByIdAndFill('User_Notes', "This is Test ")
        this.helper.WaitByIdAndClick('row11');
         this.helper.WaitByIdAndClick('CheckBox_0_0_LBL');
        this.helper.WaitByIdAndClick('OKIdButton');
    }


    SearchUser() {
        this.helper.WaitBusyIndicator();
        this.helper.WaitWindowClosed();
        this.helper.WaitByIdAndFill('User_Search', this.UserName);
        this.helper.WaitByCssAndClick_FromTagInsideList('.ListBoxItem', 0);
        this.helper.WaitByIdAndFill('User_LocalName', "Raghad");
        this.helper.WaitByIdAndFill('User_Notes', "This Is Test For Protractor")
        this.helper.WaitByIdAndClick('User-Save');

    }

}
