"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var DeclarationEventManager = /** @class */ (function () {
    function DeclarationEventManager() {
    }
    DeclarationEventManager.DisplayModeChanged = new core_1.EventEmitter();
    DeclarationEventManager.DeclarationSplitDocumentSelection = new core_1.EventEmitter();
    DeclarationEventManager.ConsignmentsChanged = new core_1.EventEmitter();
    return DeclarationEventManager;
}());
exports.DeclarationEventManager = DeclarationEventManager;
//# sourceMappingURL=DeclarationEventManager.js.map