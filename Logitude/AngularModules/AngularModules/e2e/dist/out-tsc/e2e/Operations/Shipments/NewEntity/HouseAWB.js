"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../../../Helpers/FieldsHelper");
var WizardTabs_1 = require("../EditEntity/WizardTabs");
var HouseAWB = /** @class */ (function () {
    function HouseAWB() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
        this.WizardTabs = new WizardTabs_1.WizardTabComponent();
    }
    HouseAWB.prototype.CreateHouseWizard = function (shipperRef1, LogitudeWizardType) {
        var AWBToggle = this.Helper.WaitByIdAndClick('NewAWB');
        this.Helper.WaitByIdAndClick('HouseAWB');
        this.FillHouseAWBFields(shipperRef1, LogitudeWizardType);
        this.Helper.WaitByCssButtonClick(".EntityChangesButton", "Save");
        this.Helper.WaitBusyIndicator();
    };
    HouseAWB.prototype.FillHouseAWBFields = function (shipperRef1, LogitudeWizardType) {
        this.WizardTabs.FillPartnersTab(shipperRef1, LogitudeWizardType);
        this.WizardTabs.FillRoutingTab(shipperRef1, LogitudeWizardType);
        this.WizardTabs.FillPackagesTab(LogitudeWizardType);
        this.WizardTabs.FillFreightChargesTab(LogitudeWizardType);
        // this.WizardTabs.FillOtherChargesTab(LogitudeWizardType);
        this.WizardTabs.FillGeneralDetailsTab(LogitudeWizardType);
        this.WizardTabs.FillOCITab(LogitudeWizardType);
    };
    return HouseAWB;
}());
exports.HouseAWB = HouseAWB;
//# sourceMappingURL=HouseAWB.js.map