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
    public RandomNumAcc() {
        var randomNumber = Math.floor(Math.random() * 1000).toString();
        return randomNumber;
    }
    UseSearchBox(searchFeildId: string, searchByRef: string) {

        this.Helper.WaitByIdAndFill(searchFeildId, searchByRef);
        this.Helper.WaitByCssAndClick_FromTagInsideList('.ListBoxItem', 0);
    }
    QuickSearchTextBox(searchFeildId: string, searchByRef: string) {
        this.Helper.WaitByIdAndFill(searchFeildId, searchByRef);
        this.Helper.WaitByCssAndClick_FromTagInsideList('.LogitudeQuickSearchItem', 0);
    }
    OpenViews(viewId: string, viewSearchFeildId: string, searchBy: string) {
        this.Helper.WaitByIdAndClick(viewId);
        this.Helper.WaitByIdAndFill(viewSearchFeildId, searchBy);
    }

} 