import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';

import { NewChartOfAccount } from './NewEntity/NewChartOfAccount';
import { EditChartOfAccount } from './EditEntity/EditChartOfAccount';




export class ChartOFAccountModule {
    private Helper: FieldsHelper;
    private GeneralFun: GeneralFunctions;

    private addChart: NewChartOfAccount;
    private editChart: EditChartOfAccount;

 

    constructor() {
        this.Helper = new FieldsHelper();
        this.GeneralFun = new GeneralFunctions();


        this.addChart = new NewChartOfAccount();
        this.editChart = new EditChartOfAccount();

    }

    public CreateAndEditChartOfAccount() {
        var chartOfAccountNo = this.GeneralFun.RandomNum();
        this.addChart.CreateNewChartOFAccount(chartOfAccountNo);
        this.GeneralFun.QuickSearchTextBox('SearchFieldsId_0_0',  chartOfAccountNo);
       // this.editChart.EditChartOfAccount(chartOfAccountNo);
        // browser.driver.sleep(6000);

    }
    


}



