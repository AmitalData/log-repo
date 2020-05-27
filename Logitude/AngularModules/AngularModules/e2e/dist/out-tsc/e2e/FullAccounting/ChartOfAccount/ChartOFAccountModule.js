"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../../Helpers/FieldsHelper");
var GeneralFunctions_1 = require("../../Helpers/GeneralFunctions");
var NewChartOfAccount_1 = require("./NewEntity/NewChartOfAccount");
var EditChartOfAccount_1 = require("./EditEntity/EditChartOfAccount");
var ChartOFAccountModule = /** @class */ (function () {
    function ChartOFAccountModule() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
        this.GeneralFun = new GeneralFunctions_1.GeneralFunctions();
        this.addChart = new NewChartOfAccount_1.NewChartOfAccount();
        this.editChart = new EditChartOfAccount_1.EditChartOfAccount();
    }
    ChartOFAccountModule.prototype.CreateAndEditChartOfAccount = function () {
        var chartOfAccountNo = this.GeneralFun.RandomNumAcc();
        //this.addChart.CreateNewChartOFAccount(chartOfAccountNo);
        // this.GeneralFun.QuickSearchTextBox('SearchFieldsId_0_0',  chartOfAccountNo);
        // this.Helper.WaitByIdAndFill('SearchFieldsId_0_0',chartOfAccountNo );
        //browser.driver.sleep(6000);
        //this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem',0)
        this.Helper.WaitByIdAndClick('row0col3');
        this.editChart.EditChartOfAccount(chartOfAccountNo);
    };
    return ChartOFAccountModule;
}());
exports.ChartOFAccountModule = ChartOFAccountModule;
//# sourceMappingURL=ChartOFAccountModule.js.map