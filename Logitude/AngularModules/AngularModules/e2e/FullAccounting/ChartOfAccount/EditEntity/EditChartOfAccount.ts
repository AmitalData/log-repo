import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { browser, by, element, protractor } from 'protractor';



export class EditChartOfAccount {
    private Helper = new FieldsHelper();
    constructor() {


    }

    EditChartOfAccount( DisplayNumber: string) {

        //this.Helper.WaitByIdAndFill('ChartOfAccount_Code',ChartOfAccountNo);
        this.Helper.WaitByIdAndFill('ChartOfAccount_EnglishName','customer english modified');
        this.Helper.WaitByIdAndFill('ChartOfAccount_LocalName','customer local modified');

//this.EditAppointmentGeneralTab(appointmentNo);
    this.Helper.WaitByIdAndClick('ChartOfAccount-SaveClose');
this.Helper.WaitBusyIndicator();


var EC = protractor.ExpectedConditions;
//browser.wait(EC.invisibilityOf(element(by.id('Activity.B.MarkAsComplete'))), 100000).then(a => {
//});

    }


}