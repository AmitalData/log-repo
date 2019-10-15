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
        var result = '';
        var characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
        var charactersLength = characters.length;
        for (var i = 0; i < 2; i++) {
            result += characters.charAt(Math.floor(Math.random() * charactersLength));
        }
        result += Math.floor(Math.random() * 1000000).toString();
        for (var i = 0; i < 2; i++) {
            result += characters.charAt(Math.floor(Math.random() * charactersLength));
        }

        return result;
    }
    public RandomNumAcc() {
        var randomNumber = Math.floor(Math.random() * 10000).toString();
        return randomNumber;
    }
    public RandomNumACCWithChars() {
        var result = '';
        var characters = '0A1BC2DE3F4G5HI6J7KL8M9NOP1Q0R3S9T6U4V7W5X8Y2Z';
        var charactersLength = characters.length;
        for (var i = 0; i < 5; i++) {
            result += characters.charAt(Math.floor(Math.random() * charactersLength));
        }
        return result;
    }
    public StockNumbers() {
        var randomNumber = Math.floor(Math.random() * 10000000).toString();
        return randomNumber;
    }
    UseSearchBox(searchFeildId: string, searchByRef: string, listItemCss: string) {
        this.Helper.WaitByIdAndFill(searchFeildId, searchByRef);
        this.Helper.WaitByCssAndClick_FromTagInsideList('.'+listItemCss, 0);
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
