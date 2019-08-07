"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../../../Helpers/FieldsHelper");
var GeneralFunctions_1 = require("../../../Helpers/GeneralFunctions");
var NewPhoneCall = /** @class */ (function () {
    function NewPhoneCall() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
        this.Generator = new GeneralFunctions_1.GeneralFunctions();
    }
    NewPhoneCall.prototype.CreateNewPhoneCall = function (phoneCallNo) {
        this.Helper.WaitByIdAndClick('NEWACTIVITY');
        this.Helper.WaitByIdAndClick('NEWPHONECALL');
        this.FillPhoneCallFields(phoneCallNo);
        this.Helper.WaitByIdAndClick('Ok-AddActivity');
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitWindowClosed();
    };
    NewPhoneCall.prototype.FillPhoneCallFields = function (phoneCallNo) {
        this.Helper.WaitByIdAndFill('Activity_CustomerId', 'razan co');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitByIdAndFill('Activity_CallWithId', 'm');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitByIdAndFill('Activity_Subject', phoneCallNo);
        this.Helper.WaitByIdAndFill('Activity_Description', 'Phone Call Description - Protractor '); // test random number randomWholeNum
        this.Helper.WaitByIdAndFill('Activity_OwnerId', 'ra');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 2);
    };
    return NewPhoneCall;
}());
exports.NewPhoneCall = NewPhoneCall;
//# sourceMappingURL=NewPhoneCall.js.map