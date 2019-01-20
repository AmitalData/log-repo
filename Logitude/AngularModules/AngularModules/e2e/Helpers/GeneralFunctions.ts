import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from './FieldsHelper';
export class GeneralFunctions {
    private Helper: FieldsHelper;

    constructor() {
        this.Helper = new FieldsHelper();
    }
    public GoToMainMenu(menuid: string) {
        this.Helper.WaitByIdAndClick('PAR');
        var selectMenu = this.Helper.WaitByIdAndClick(menuid);
    }
    SelectMenuWorkSpaceTabs(id: string) {
        var selectTab = this.Helper.WaitByIdAndClick(id);
    }
    public RandomNum() {
        var randomNumber = Math.floor(Math.random() * 1000000).toString();
        return randomNumber;
    }
} 