"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../../Helpers/FieldsHelper");
var NewPotentialCustomer_1 = require("./NewEntity/NewPotentialCustomer");
var GeneralFunctions_1 = require("../../Helpers/GeneralFunctions");
var CustomerModule = /** @class */ (function () {
    function CustomerModule() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
        this.GeneralFun = new GeneralFunctions_1.GeneralFunctions();
        this.addPotCustomer = new NewPotentialCustomer_1.NewPotentialCustomer();
    }
    CustomerModule.prototype.CreateCustomer = function () {
        var customerNo = this.GeneralFun.RandomNum();
        this.addPotCustomer.CreateNewPotentialCustomer('Customer # ' + customerNo);
        this.GeneralFun.QuickSearchTextBox('Card_Search', 'Customer # ' + customerNo);
        // browser.driver.sleep(6000);
    };
    return CustomerModule;
}());
exports.CustomerModule = CustomerModule;
//# sourceMappingURL=CustomersModule.js.map