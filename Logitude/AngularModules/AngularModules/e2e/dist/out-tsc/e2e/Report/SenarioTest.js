"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var GeneralFunctions_1 = require("../Helpers/GeneralFunctions");
var ReportSearch_1 = require("../Report/ReportSearch");
var ReportGenerator_1 = require("../Report/ReportGenerator");
var SenarioTest = /** @class */ (function () {
    function SenarioTest() {
        //private page: OperationsComp = new OperationsComp();
        this.searchPage = new ReportSearch_1.ReportSearch();
        this.reportGenerator = new ReportGenerator_1.ReportGenerator();
        this.generalFun = new GeneralFunctions_1.GeneralFunctions();
    }
    SenarioTest.prototype.ReportScienarios = function () {
        this.searchPage.QuickSearch('Automation Test Report');
        this.reportGenerator.RunReportSuccessfully('RunReportSucceededDiv');
    };
    SenarioTest.prototype.FailedScienarios = function () {
        this.reportGenerator.RunReportFailed('RunReportFailedDiv');
    };
    return SenarioTest;
}());
exports.SenarioTest = SenarioTest;
//# sourceMappingURL=SenarioTest.js.map