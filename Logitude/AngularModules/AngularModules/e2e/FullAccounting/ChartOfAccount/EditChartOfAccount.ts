import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { browser, by, element, protractor } from 'protractor';



export class EditChartOfAccount {
    private Helper = new FieldsHelper();
    constructor() {


    }

    EditChartOfAccount( Code: string) {
        this.Helper.WaitByIdAndFill('SearchFieldsId_0_0', Code);
        this.Helper.ItemsPresent('ListDataLoaded');
        this.Helper.WaitByIdAndClick('row0col0');
        this.Helper.WaitBusyIndicator();
        //this.Helper.WaitByIdAndFill('ChartOfAccount_Code',ChartOfAccountNo);
        this.Helper.WaitByIdAndFill('ChartOfAccount_EnglishName','English Name Modified');
        this.Helper.WaitByIdAndFill('ChartOfAccount_LocalName','Local Name Modified');

//this.EditAppointmentGeneralTab(appointmentNo);
this.Helper.ItemsPresent('ChartOfAccount-SaveClose');
    this.Helper.WaitByIdAndClick('ChartOfAccount-SaveClose');
this.Helper.WaitBusyIndicator();


//var EC = protractor.ExpectedConditions;
//browser.wait(EC.invisibilityOf(element(by.id('Activity.B.MarkAsComplete'))), 100000).then(a => {
//});

    }


}
