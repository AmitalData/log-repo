"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var protractor_1 = require("protractor");
var FieldsHelper_1 = require("./../../Helpers/FieldsHelper");
var ShipmentHelper = /** @class */ (function () {
    function ShipmentHelper() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
    }
    ShipmentHelper.prototype.SelectDicrctionTransportMode = function (Direction, TransportMode, ShipmentType) {
        var directionBtn;
        var transportModeBtn;
        var shipmentTypeBtn;
        var EC = protractor_1.protractor.ExpectedConditions;
        var directionID;
        var shipmentTypeID;
        // if (Direction == 'Export') {
        //   directionID = "DirectionRadio_0E";
        // }
        // else if (Direction == 'Import') {
        //   directionID == 'DirectionRadio_0I';
        // }
        // else if (Direction == 'Domestic') {
        //   directionID = 'DirectionRadio_0D';
        // }
        // else if (Direction == 'Drop') {
        //   directionID = 'DirectionRadio_0R';
        // }
        // if (ShipmentType == 'FCL') {
        //   shipmentTypeID = 'ShipmentTypeRadio_0FCLD';
        // }
        // else if (ShipmentType == 'LCL') {
        //   shipmentTypeID == 'ShipmentTypeRadio_0LCLD';
        // }
        // else if (ShipmentType == 'FTL') {
        //   shipmentTypeID = 'ShipmentTypeRadio_0FTL';
        // }
        // else if (ShipmentType == 'LTL') {
        //   shipmentTypeID = 'ShipmentTypeRadio_0LTL';
        // }
        // else
        //   shipmentTypeID = 'ShipmentTypeRadio_0MyGO';
        if (TransportMode == 'A' && ShipmentType == '') {
            var EC = protractor_1.protractor.ExpectedConditions;
            protractor_1.browser.wait(EC.elementToBeClickable(protractor_1.element(protractor_1.by.css('.RadioButton'))), 20000).then(function (a) {
                if (Direction == 'Export') {
                    directionBtn = protractor_1.element(protractor_1.by.id('DirectionRadio_0E'));
                }
                else if (Direction == 'Import') {
                    directionBtn = protractor_1.element(protractor_1.by.id('DirectionRadio_0I'));
                }
                else if (Direction == 'Domestic') {
                    directionBtn = protractor_1.element(protractor_1.by.id('DirectionRadio_0D'));
                }
                else if (Direction == 'Drop') {
                    directionBtn = protractor_1.element(protractor_1.by.id('DirectionRadio_0R'));
                }
                protractor_1.browser.executeScript("arguments[0].click();", directionBtn.getWebElement());
                transportModeBtn = protractor_1.element(protractor_1.by.id('TransportModeRadio_0A'));
                protractor_1.browser.executeScript("arguments[0].click();", transportModeBtn.getWebElement());
            });
        }
        else if (TransportMode == 'O' && ShipmentType != '') {
            var EC = protractor_1.protractor.ExpectedConditions;
            protractor_1.browser.wait(EC.elementToBeClickable(protractor_1.element(protractor_1.by.css('.RadioButton'))), 20000).then(function (a) {
                if (Direction == 'Export') {
                    directionBtn = protractor_1.element(protractor_1.by.id('DirectionRadio_0E'));
                }
                else if (Direction == 'Import') {
                    directionBtn = protractor_1.element(protractor_1.by.id('DirectionRadio_0I'));
                }
                else if (Direction == 'Domestic') {
                    directionBtn = protractor_1.element(protractor_1.by.id('DirectionRadio_0D'));
                }
                else if (Direction == 'Drop') {
                    directionBtn = protractor_1.element(protractor_1.by.id('DirectionRadio_0R'));
                }
                protractor_1.browser.executeScript("arguments[0].click();", directionBtn.getWebElement());
                transportModeBtn = protractor_1.element(protractor_1.by.id('TransportModeRadio_0O'));
                protractor_1.browser.executeScript("arguments[0].click();", transportModeBtn.getWebElement());
                if (ShipmentType == 'FCL') {
                    shipmentTypeBtn = protractor_1.element(protractor_1.by.id('ShipmentTypeRadio_0FCLD'));
                }
                else if (ShipmentType == 'LCL') {
                    shipmentTypeBtn = protractor_1.element(protractor_1.by.id('ShipmentTypeRadio_0LCLD'));
                }
                else if (ShipmentType == 'OG')
                    shipmentTypeBtn = protractor_1.element(protractor_1.by.id('ShipmentTypeRadio_0MyGO'));
                protractor_1.browser.executeScript("arguments[0].click();", shipmentTypeBtn.getWebElement());
            });
        }
        else if (TransportMode == 'I' && ShipmentType != '') {
            var EC = protractor_1.protractor.ExpectedConditions;
            protractor_1.browser.wait(EC.elementToBeClickable(protractor_1.element(protractor_1.by.css('.RadioButton'))), 20000).then(function (a) {
                if (Direction == 'Export') {
                    directionBtn = protractor_1.element(protractor_1.by.id('DirectionRadio_0E'));
                }
                else if (Direction == 'Import') {
                    directionBtn = protractor_1.element(protractor_1.by.id('DirectionRadio_0I'));
                }
                else if (Direction == 'Domestic') {
                    directionBtn = protractor_1.element(protractor_1.by.id('DirectionRadio_0D'));
                }
                else if (Direction == 'Drop') {
                    directionBtn = protractor_1.element(protractor_1.by.id('DirectionRadio_0R'));
                }
                protractor_1.browser.executeScript("arguments[0].click();", directionBtn.getWebElement());
                transportModeBtn = protractor_1.element(protractor_1.by.id('TransportModeRadio_0I'));
                protractor_1.browser.executeScript("arguments[0].click();", transportModeBtn.getWebElement());
                if (ShipmentType == 'FTL') {
                    shipmentTypeBtn = protractor_1.element(protractor_1.by.id('ShipmentTypeRadio_0FTL'));
                }
                else if (ShipmentType == 'LTL') {
                    shipmentTypeBtn = protractor_1.element(protractor_1.by.id('ShipmentTypeRadio_0LTL'));
                }
                else if ((ShipmentType == 'IG'))
                    shipmentTypeBtn = protractor_1.element(protractor_1.by.id('ShipmentTypeRadio_0MyGI'));
                protractor_1.browser.executeScript("arguments[0].click();", shipmentTypeBtn.getWebElement());
            });
        }
    };
    ShipmentHelper.prototype.CreateAndCloseNewShipment = function (MasterDirectType, CancelBtnId, Direction, TransportMode, ShipmentType) {
        var AWBToggle = this.Helper.WaitByIdAndClick('NEWSHIP');
        this.Helper.WaitByIdAndClick(MasterDirectType);
        this.SelectDicrctionTransportMode(Direction, TransportMode, ShipmentType);
        this.Helper.WaitByIdAndClick(CancelBtnId);
    };
    ShipmentHelper.prototype.OperationalCloseShipment = function () {
        this.Helper.WaitByIdAndClick('MenuButtons');
        this.Helper.WaitByIdAndClick('Shipment.B.OperationalClose');
        this.Helper.WaitByCssStringAndClick('.RedButton', 'Confirm');
        this.Helper.WaitBusyIndicator();
    };
    ShipmentHelper.prototype.OperationalReopenShipment = function () {
        this.Helper.WaitByIdAndClick('MenuButtons');
        this.Helper.WaitByIdAndClick('Shipment.B.OperationalReopen');
        this.Helper.WaitByIdAndFill('EventNotes', 'Operational ReOpen - Protractor Testing .. ');
        this.Helper.WaitByCssStringAndClick('.RedButton', 'Confirm');
        this.Helper.WaitBusyIndicator();
    };
    ShipmentHelper.prototype.AccountingCloseShipment = function () {
        this.Helper.WaitByIdAndClick('MenuButtons');
        this.Helper.WaitByIdAndClick('Shipment.B.AccountingClose');
        this.Helper.WaitByCssStringAndClick('.RedButton', 'Confirm');
        this.Helper.WaitBusyIndicator();
    };
    ShipmentHelper.prototype.AccountedReopenShipment = function () {
        this.Helper.WaitByIdAndClick('MenuButtons');
        this.Helper.WaitByIdAndClick('Shipment.B.AccountedReopen');
        this.Helper.WaitByIdAndFill('EventNotes', 'Accounting ReOpen - Protractor Testing .. ');
        this.Helper.WaitByCssStringAndClick('.RedButton', 'Confirm');
        this.Helper.WaitBusyIndicator();
    };
    ShipmentHelper.prototype.CopyShipment = function () {
        this.Helper.WaitByIdAndClick('MenuButtons');
        this.Helper.WaitByIdAndClick('Shipment.B.CopyShipment');
        // this.Helper.WaitByIdAndClick('Shipment.B.CopyShipment');
        // this.Helper.WaitByIdAndClick('CheckBox_0_5');//Include pickup
        this.Helper.WaitByCssStringAndClick('.LogitudeCheckBox', 'Include PickUp');
        // this.Helper.WaitByIdAndClick('CheckBox_0_6');//Include Delivery
        // this.Helper.WaitByIdAndClick('CheckBox_0_7');//Include Flight
        // this.Helper.WaitByIdAndClick('CheckBox_0_8');//Include PreCarriage
        // this.Helper.WaitByIdAndClick('CheckBox_0_9');//Include OnCarriage
        this.Helper.WaitByIdAndClick('ShipmentCreatebtn');
        this.Helper.WaitBusyIndicator();
    };
    return ShipmentHelper;
}());
exports.ShipmentHelper = ShipmentHelper;
//# sourceMappingURL=ShipmentHelper.js.map