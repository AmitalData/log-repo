"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("./FieldsHelper");
var GeneralFunctions = /** @class */ (function () {
    function GeneralFunctions() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
    }
    GeneralFunctions.prototype.GoToMainMenu = function (menuid) {
        this.Helper.WaitByIdAndClick('PAR');
        var selectMenu = this.Helper.WaitByIdAndClick(menuid);
    };
    GeneralFunctions.prototype.SelectMenuWorkSpaceTabs = function (id) {
        var selectTab = this.Helper.WaitByIdAndClick(id);
    };
    GeneralFunctions.prototype.RandomNum = function () {
        var randomNumber = Math.floor(Math.random() * 1000000).toString();
        return randomNumber;
    };
    GeneralFunctions.prototype.RandomNumAcc = function () {
        var randomNumber = Math.floor(Math.random() * 1000).toString();
        return randomNumber;
    };
    GeneralFunctions.prototype.UseSearchBox = function (searchFeildId, searchByRef) {
        this.Helper.WaitByIdAndFill(searchFeildId, searchByRef);
        this.Helper.WaitByCssAndClick_FromTagInsideList('.ListBoxItem', 0);
    };
    GeneralFunctions.prototype.QuickSearchTextBox = function (searchFeildId, searchByRef) {
        this.Helper.WaitByIdAndFill(searchFeildId, searchByRef);
        this.Helper.WaitByCssAndClick_FromTagInsideList('.LogitudeQuickSearchItem', 0);
    };
    GeneralFunctions.prototype.OpenViews = function (viewId, viewSearchFeildId, searchBy) {
        this.Helper.WaitByIdAndClick(viewId);
        this.Helper.WaitByIdAndFill(viewSearchFeildId, searchBy);
    };
    return GeneralFunctions;
}());
exports.GeneralFunctions = GeneralFunctions;
//# sourceMappingURL=GeneralFunctions.js.map