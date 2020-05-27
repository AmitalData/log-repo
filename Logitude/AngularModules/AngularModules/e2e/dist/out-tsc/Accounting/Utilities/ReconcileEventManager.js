"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var ReconcileEventManager = /** @class */ (function () {
    function ReconcileEventManager() {
    }
    ReconcileEventManager.CheckBoxChecked = new core_1.EventEmitter(); // for ledger transactions
    ReconcileEventManager.BankCheckBoxChecked = new core_1.EventEmitter(); // for bank account page lines
    ReconcileEventManager.RowUnselected = new core_1.EventEmitter();
    return ReconcileEventManager;
}());
exports.ReconcileEventManager = ReconcileEventManager;
//# sourceMappingURL=ReconcileEventManager.js.map