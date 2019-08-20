"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../../../Helpers/FieldsHelper");
var EditChartOfAccount = /** @class */ (function () {
    function EditChartOfAccount() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
    }
    EditChartOfAccount.prototype.EditChartOfAccount = function (Code) {
        this.Helper.WaitByIdAndFill('SearchFieldsId_0_0', Code);
        this.Helper.ItemsPresent('ListDataLoaded');
        this.Helper.WaitByIdAndClick('row0col0');
        this.Helper.WaitBusyIndicator();
        //this.Helper.WaitByIdAndFill('ChartOfAccount_Code',ChartOfAccountNo);
        this.Helper.WaitByIdAndFill('ChartOfAccount_EnglishName', 'English Name Modified');
        this.Helper.WaitByIdAndFill('ChartOfAccount_LocalName', 'Local Name Modified');
        //this.EditAppointmentGeneralTab(appointmentNo);
        this.Helper.WaitByIdAndClick('ChartOfAccount-SaveClose');
        this.Helper.WaitBusyIndicator();
        //var EC = protractor.ExpectedConditions;
        //browser.wait(EC.invisibilityOf(element(by.id('Activity.B.MarkAsComplete'))), 100000).then(a => {
        //});
    };
    return EditChartOfAccount;
}());
exports.EditChartOfAccount = EditChartOfAccount;
//# sourceMappingURL=EditChartOfAccount.js.map