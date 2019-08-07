"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var protractor_1 = require("protractor");
var FieldsHelper_1 = require("../../../Helpers/FieldsHelper");
var WizardTabs_1 = require("../EditEntity/WizardTabs");
var MasterAWB = /** @class */ (function () {
    function MasterAWB() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
        this.WizardTabs = new WizardTabs_1.WizardTabComponent();
    }
    MasterAWB.prototype.CreateMasterWizard = function (shipperRef1, LogitudeWizardType) {
        var AWBToggle = this.Helper.WaitByIdAndClick('NewAWB');
        this.Helper.WaitByIdAndClick('MasterAWB');
        this.FillMasterAWBFields(shipperRef1, LogitudeWizardType);
        this.Helper.WaitByCssButtonClick(".EntityChangesButton", "Save");
        this.Helper.WaitBusyIndicator();
        protractor_1.browser.driver.sleep(4000);
    };
    MasterAWB.prototype.FillMasterAWBFields = function (shipperRef1, LogitudeWizardType) {
        this.WizardTabs.FillPartnersTab(shipperRef1, LogitudeWizardType);
        this.WizardTabs.FillRoutingTab(shipperRef1, LogitudeWizardType);
        // this.WizardTabs.FillHousesTab();
        this.WizardTabs.FillPackagesTab(LogitudeWizardType);
        this.WizardTabs.FillFreightChargesTab(LogitudeWizardType);
        this.WizardTabs.FillOtherChargesTab(LogitudeWizardType);
        this.WizardTabs.FillGeneralDetailsTab(LogitudeWizardType);
        this.WizardTabs.FillOCITab(LogitudeWizardType);
        this.WizardTabs.FillOtherPartnersTab(LogitudeWizardType);
    };
    return MasterAWB;
}());
exports.MasterAWB = MasterAWB;
//# sourceMappingURL=MasterAWB.js.map