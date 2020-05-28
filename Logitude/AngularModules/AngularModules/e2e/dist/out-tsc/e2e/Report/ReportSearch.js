"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../Helpers/FieldsHelper");
var GeneralFunctions_1 = require("./../Helpers/GeneralFunctions");
var ReportSearch = /** @class */ (function () {
    function ReportSearch() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
        this.logitudeTab = new GeneralFunctions_1.GeneralFunctions();
    }
    ReportSearch.prototype.QuickSearch = function (reportName) {
        this.logitudeTab.GoToMainMenu('General.MH.Reports');
        // this.UseSearchBox('null_Search', 'Unpaid Invoices');
        this.UseSearchBox('null_Search', reportName);
    };
    ReportSearch.prototype.UseSearchBox = function (searchFeildId, searchByRef) {
        this.Helper.WaitByIdAndFill(searchFeildId, searchByRef);
        this.Helper.WaitByCssAndClick_FromTagInsideList('.HyperlinkQueryButtonControl', 0);
    };
    return ReportSearch;
}());
exports.ReportSearch = ReportSearch;
//# sourceMappingURL=ReportSearch.js.map