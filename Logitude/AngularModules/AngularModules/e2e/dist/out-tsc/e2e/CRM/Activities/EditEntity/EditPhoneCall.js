"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var protractor_1 = require("protractor");
var FieldsHelper_1 = require("../../../Helpers/FieldsHelper");
var GeneralFunctions_1 = require("../../../Helpers/GeneralFunctions");
var EditPhoneCalls = /** @class */ (function () {
    function EditPhoneCalls() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
        this.Generator = new GeneralFunctions_1.GeneralFunctions();
    }
    EditPhoneCalls.prototype.EditPhoneCall = function (phoneCallNo) {
        this.EditPhoneCallGeneralTab(phoneCallNo);
        this.Helper.WaitByIdAndClick('Activity-SaveClose');
        this.Helper.WaitBusyIndicator();
        var EC = protractor_1.protractor.ExpectedConditions;
        protractor_1.browser.wait(EC.invisibilityOf(protractor_1.element(protractor_1.by.id('Activity.B.MarkAsComplete'))), 100000).then(function (a) {
        });
    };
    EditPhoneCalls.prototype.EditPhoneCallGeneralTab = function (phoneCallDesc) {
        this.Helper.WaitByIdAndFill('Activity_CustomerId', 'razan j');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitByIdAndFill('Activity_CallWithId', 'razan');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitByIdAndFill('Activity_Subject', 'Edit Subject for ' + phoneCallDesc);
        this.Helper.WaitByIdAndFill('Activity_Description', 'Edit Description for ' + phoneCallDesc); // test random number randomWholeNum
        // this.Helper.WaitByIdAndFill('Activity_OwnerId', 'ra');
        // this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 2);
    };
    return EditPhoneCalls;
}());
exports.EditPhoneCalls = EditPhoneCalls;
//# sourceMappingURL=EditPhoneCall.js.map