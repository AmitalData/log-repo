import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from './../../Helpers/FieldsHelper';
import { GeneralFunctions } from './../../Helpers/GeneralFunctions';


export class ShipmentHelper {
    private Helper: FieldsHelper;
    private generalFun: GeneralFunctions = new GeneralFunctions();

    constructor() {
        this.Helper = new FieldsHelper();

    }

    SelectDicrctionTransportMode(Direction: string, TransportMode: string, ShipmentType: string) {
        var directionBtn: any;
        var transportModeBtn: any;
        var shipmentTypeBtn: any;
        var EC = protractor.ExpectedConditions;
        var directionID: string;
        var shipmentTypeID: string;

        if (TransportMode == 'A' && ShipmentType == '') {

            var EC = protractor.ExpectedConditions;
            browser.wait(EC.elementToBeClickable(element(by.css('.RadioButton'))), 20000).then(a => {
                if (Direction == 'Export') {
                    directionBtn = element(by.id('DirectionRadio_0E'));

                }
                else if (Direction == 'Import') {
                    directionBtn = element(by.id('DirectionRadio_0I'));

                }
                else if (Direction == 'Domestic') {
                    directionBtn = element(by.id('DirectionRadio_0D'));

                }
                else if (Direction == 'Drop') {
                    directionBtn = element(by.id('DirectionRadio_0R'));
                }
                browser.executeScript("arguments[0].click();", directionBtn.getWebElement());

                transportModeBtn = element(by.id('TransportModeRadio_0A'));
                browser.executeScript("arguments[0].click();", transportModeBtn.getWebElement());
            });
        }
        else if (TransportMode == 'O' && ShipmentType != '') {

            var EC = protractor.ExpectedConditions;
            browser.wait(EC.elementToBeClickable(element(by.css('.RadioButton'))), 20000).then(a => {
                if (Direction == 'Export') {
                    directionBtn = element(by.id('DirectionRadio_0E'));

                }
                else if (Direction == 'Import') {
                    directionBtn = element(by.id('DirectionRadio_0I'));

                }
                else if (Direction == 'Domestic') {
                    directionBtn = element(by.id('DirectionRadio_0D'));

                }
                else if (Direction == 'Drop') {
                    directionBtn = element(by.id('DirectionRadio_0R'));
                }
                browser.executeScript("arguments[0].click();", directionBtn.getWebElement());

                transportModeBtn = element(by.id('TransportModeRadio_0O'));
                browser.executeScript("arguments[0].click();", transportModeBtn.getWebElement());

                if (ShipmentType == 'FCL') {
                    shipmentTypeBtn = element(by.id('ShipmentTypeRadio_0FCLD'));
                }
                else if (ShipmentType == 'LCL') {
                    shipmentTypeBtn = element(by.id('ShipmentTypeRadio_0LCLD'));
                }
                else if (ShipmentType == 'OG')
                    shipmentTypeBtn = element(by.id('ShipmentTypeRadio_0MyGO'));

                browser.executeScript("arguments[0].click();", shipmentTypeBtn.getWebElement());

            });
        }
        else if (TransportMode == 'I' && ShipmentType != '') {

            var EC = protractor.ExpectedConditions;
            browser.wait(EC.elementToBeClickable(element(by.css('.RadioButton'))), 20000).then(a => {
                if (Direction == 'Export') {
                    directionBtn = element(by.id('DirectionRadio_0E'));

                }
                else if (Direction == 'Import') {
                    directionBtn = element(by.id('DirectionRadio_0I'));

                }
                else if (Direction == 'Domestic') {
                    directionBtn = element(by.id('DirectionRadio_0D'));

                }
                else if (Direction == 'Drop') {
                    directionBtn = element(by.id('DirectionRadio_0R'));
                }
                browser.executeScript("arguments[0].click();", directionBtn.getWebElement());

                transportModeBtn = element(by.id('TransportModeRadio_0I'));
                browser.executeScript("arguments[0].click();", transportModeBtn.getWebElement());

                if (ShipmentType == 'FTL') {
                    shipmentTypeBtn = element(by.id('ShipmentTypeRadio_0FTL'));
                }
                else if (ShipmentType == 'LTL') {
                    shipmentTypeBtn = element(by.id('ShipmentTypeRadio_0LTL'));
                }
                else if ((ShipmentType == 'IG'))
                    shipmentTypeBtn = element(by.id('ShipmentTypeRadio_0MyGI'));

                browser.executeScript("arguments[0].click();", shipmentTypeBtn.getWebElement());

            });
        }
    }

    CreateAndCloseNewShipment(MasterDirectType: string, CancelBtnId: string, Direction: string, TransportMode: string, ShipmentType: string) {
        var AWBToggle = this.Helper.WaitByIdAndClick('NEWSHIP');
        this.Helper.WaitByIdAndClick(MasterDirectType);
        this.SelectDicrctionTransportMode(Direction, TransportMode, ShipmentType);

        this.Helper.WaitByIdAndClick(CancelBtnId);
    }

    AddSelectAirlineStock(usedIn: string) {
        var numbertest = this.generalFun.StockNumbers();
        this.Helper.WaitByIdAndClick('AddStock');
        this.Helper.WaitBusyIndicator();

        var EC = protractor.ExpectedConditions;
        browser.wait(EC.visibilityOf(element(by.id("AirlineSimpleGridBodyId"))), 100000).then(a => {
            browser.wait(EC.elementToBeClickable(element(by.id("AddStocks"))), 100000).then(a => {
            });
        });
        this.Helper.WaitByIdAndClick('AddStocks');
        this.Helper.WaitBusyIndicator();

        this.Helper.WaitByIdAndFill('StartNumber', numbertest);

        var byAmount = element(by.id('ByAmountRadio'));
        browser.executeScript("arguments[0].click();", byAmount.getWebElement());

        this.Helper.WaitByIdAndFill('Amount', '1');

        this.Helper.WaitByIdAndClick('OkAddStock');

        this.Helper.WaitBusyIndicator();

        if (usedIn == 'edit') {
            browser.wait(EC.invisibilityOf(element(by.id("OkAddStock"))), 100000).then(a => {
                browser.wait(EC.visibilityOf(element(by.id("AirlineSimpleGridBodyId"))), 100000).then(a => {
                });
            });
            this.Helper.WaitByIdAndClick('EditBackbutton_1');
        } else if (usedIn == 'wizard') {
            browser.wait(EC.invisibilityOf(element(by.id("OkAddStock"))), 100000).then(a => {
                browser.wait(EC.visibilityOf(element(by.id("AirlineSimpleGridBodyId"))), 100000).then(a => {
                });
            });
            this.Helper.WaitByIdAndClick('EditBackbutton');
        }

        this.Helper.WaitByIdAndClick('GetFromStockBtn');
        this.Helper.WaitBusyIndicator();

        if (usedIn == 'edit') {
            browser.wait(EC.elementToBeClickable(element(by.id("StockSelectionID")))).then(a => {
            });
            element(by.cssContainingText('.GridViewCell', numbertest)).click();
            this.Helper.WaitByIdAndClick('OkBtn');
            this.Helper.WaitByIdAndClick('ConfirmWindow_Yes_0');
            this.Helper.WaitEditComponentBusyIndicator();

        } else if (usedIn == 'wizard') {
            browser.wait(EC.elementToBeClickable(element(by.id("StockSelectionID"))), 100000).then(a => {
            });
            element(by.cssContainingText('.GridViewCell', numbertest)).click();
            this.Helper.WaitByIdAndClick('OkBtn');
            this.Helper.WaitByIdAndClick('ConfirmWindow_Yes_0');
            this.Helper.WaitBusyIndicator();
        }
    }

    AddAirlineStock(LogitudeWizardType: string) {
        console.log(LogitudeWizardType);
        var numbertest = this.generalFun.StockNumbers();
        if (LogitudeWizardType == 'M') {
            this.Helper.WaitByIdAndFill('Master_Master', numbertest);
            console.log('Stock Number is : ' + numbertest);

        } else {
            this.Helper.WaitByIdAndFill('Shipment_Master', numbertest);
            console.log('Stock Number is : ' + numbertest);
        }
    }
}
