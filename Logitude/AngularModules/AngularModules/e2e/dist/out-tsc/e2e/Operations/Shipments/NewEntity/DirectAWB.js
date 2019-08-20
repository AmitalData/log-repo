"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../../../Helpers/FieldsHelper");
var WizardTabs_1 = require("../EditEntity/WizardTabs");
var DirectAWB = /** @class */ (function () {
    function DirectAWB() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
        this.WizardTabs = new WizardTabs_1.WizardTabComponent();
    }
    DirectAWB.prototype.CreateDirectAWB = function (shipperRef1, LogitudeWizardType) {
        var AWBToggle = this.Helper.WaitByIdAndClick('NewAWB');
        this.Helper.WaitByIdAndClick('DirectAWB');
        this.FillDirectAWBFields(shipperRef1, LogitudeWizardType);
        this.Helper.WaitByCssButtonClick(".EntityChangesButton", "Save");
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByCssButtonClick(".Button", "Close");
    };
    DirectAWB.prototype.FillDirectAWBFields = function (shipperRef1, LogitudeWizardType) {
        this.WizardTabs.FillPartnersTab(shipperRef1, LogitudeWizardType);
        this.WizardTabs.FillRoutingTab(shipperRef1, LogitudeWizardType);
        this.WizardTabs.FillPackagesTab(LogitudeWizardType);
        this.WizardTabs.FillFreightChargesTab(LogitudeWizardType);
        // this.WizardTabs.FillOtherChargesTab(LogitudeWizardType);
        this.WizardTabs.FillGeneralDetailsTab(LogitudeWizardType);
        this.WizardTabs.FillOCITab(LogitudeWizardType);
        this.WizardTabs.FillOtherPartnersTab(LogitudeWizardType);
    };
    return DirectAWB;
}());
exports.DirectAWB = DirectAWB;
//# sourceMappingURL=DirectAWB.js.map