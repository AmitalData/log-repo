import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';
import { protractor, WebDriver, browser, by, element, $ } from 'protractor';

export class NewARInvoice {
    private Helper = new FieldsHelper();
    private gn = new GeneralFunctions();
    constructor() {
    }

    CreateNewARInvoice(BillToName: string) {
        browser.ignoreSynchronization = true;
        this.Helper.WaitByIdAndClick('NewInvoice');
        //  this.Helper.WaitByIdAndClick('NewGeneralARInvoice');
        //this.Helper.WaitByCssButtonClick('.ToggleButtonMenu','New Invoice');
        this.Helper.WaitByIdAndClick('NewGeneralInvoice');
        this.Helper.WaitByIdAndFill('ARInvoice_BillToId', BillToName);
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitBusyIndicator();
        //browser.sleep(5000);
        console.log("Rabaia 1");
        this.Helper.WaitByIdAndFill('ARInvoice_InvoiceCurrencyId', 'NIS');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitBusyIndicator();
        console.log("Rabaia 2");
        this.Helper.WaitByIdAndFill('ARInvoice_VatNumber', '1');
        //this.Helper.WaitBusyIndicator();
       // this.Helper.WaitBusyIndicator(); // closed month
        // this.Helper.WaitBusyIndicator(); // currency rates
        this.Helper.WaitByIdAndClickRabaia('ok-addArInvoice').then(z => {
            console.log("Rabaia 3");
            element(by.id('ok-addArInvoice')).click();
            this.Helper.WaitWindowClosedRabaia().then(zz => {
                this.Helper.WaitBusyIndicator().then(a => {
                    var EC = protractor.ExpectedConditions;
                    // Waits for the element with id 'abc' to be clickable.
                    console.log("Rabaia 4");
                    browser.wait(EC.elementToBeClickable($('#Add')), 1000000).then(az => {
                        console.log("Rabaia 5");
                        this.Helper.WaitByIdAndClick('Add');
                        //this.Helper.WaitBusyIndicator();
                        console.log("Rabaia 6");
                        this.Helper.WaitByIdAndFill('ARInvoiceLine_ChargesTypeId', 'Air Freight');
                        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
                        this.Helper.WaitByIdAndFill('ARInvoiceLine_VatTypeId', 'Zero');
                        console.log("Rabaia 6");
                        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
                        this.Helper.WaitByIdAndFill('ARInvoiceLine_Quantity', '5');
                        this.Helper.WaitByIdAndFill('ARInvoiceLine_UnitPrice', '10');
                        console.log("Rabaia 7");
                        this.Helper.WaitByIdAndClick('ok-addArInvoiceline');
                        console.log("Rabaia 8");
                        this.Helper.WaitBusyIndicator().then(aadd => {
                            console.log("Rabaia 9");
                            // this.Helper.WaitBusyIndicator().then(aa => {
                            this.Helper.WaitByIdAndClickRabaia('ARInvoice.B.Approve').then(ax => {
                                console.log("Rabaia 10");
                                element(by.id('ARInvoice.B.Approve')).click().then(c => {
                                    console.log("Rabaia 11");
                                    this.Helper.WaitBusyIndicator()
                                        .then(aaa => {
                                            console.log("Rabaia 12");
                                            //  browser.sleep(5000);

                                            this.Helper.WaitByIdAndCheckText('ARInvoiceHeaderStatusName', 'Unpaid');

                                            //console.log('Invoice Is Approved and Unpdaid Now');
                                            //  browser.sleep(9000);

                                        });
                                });

                            });
                            //  });
                        });

                    });

                });
            });
        });


        //element(by.id(''))




        //this.Helper.WaitBusyIndicator();
        // this.Helper.WaitByIdAndClick('ARInvoice.B.Approve');
        //browser.driver.sleep(5000);
        // this.Helper.ItemsVisibility('Add');

        // this.Helper.WaitBusyIndicator();
        // this.Helper.WaitByIdAndCheckText('ARInvoiceHeaderStatusName', 'Draft');
        // console.log('Invoice Is Draft Now');
        //browser.sleep(10000);

        //



        // browser.sleep(9000);
        // this.Helper.WaitByIdAndCheckText('ARInvoiceHeaderStatusName', 'Unpaid');

        // console.log('Invoice Is Approved and Unpdaid Now');


        // this.Helper.WaitByIdAndClick('EditBackbutton');

        browser.sleep(5000);
    }

}