"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../Helpers/FieldsHelper");
var ReportGenerator = /** @class */ (function () {
    function ReportGenerator() {
        this.helper = new FieldsHelper_1.FieldsHelper();
    }
    /*  CheckBox() {
    
          browser.executeScript('arguments[1].click();', element(by.id('CreateDate')).getWebElement());
  
  
      }*/
    ReportGenerator.prototype.RunReportSuccessfully = function (expectedId) {
        this.helper.WaitByIdAndClick('CheckBox_0_0_LBL');
        this.helper.WaitByIdAndClick('RunReportButton');
        // this.helper.waitElementByIDPresence(expectedId);
        this.helper.ItemsPresent(expectedId);
        this.helper.WaitBusyIndicator();
    };
    ReportGenerator.prototype.RunReportFailed = function (expectedId) {
        this.helper.WaitByIdAndClick('CheckBox_0_0_LBL');
        this.helper.WaitByIdAndClick('RunReportButton');
        this.helper.WaitByIdAndClick('MessageWindow_Ok_0');
        this.helper.ItemsPresent(expectedId);
        // this.helper.waitElementByIDPresence(expectedId);
        this.helper.WaitBusyIndicator();
    };
    return ReportGenerator;
}());
exports.ReportGenerator = ReportGenerator;
//# sourceMappingURL=ReportGenerator.js.map