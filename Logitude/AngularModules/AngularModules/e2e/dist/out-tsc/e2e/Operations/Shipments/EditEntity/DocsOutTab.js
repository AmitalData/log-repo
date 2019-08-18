"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../../../Helpers/FieldsHelper");
var DocsOutTabComponent = /** @class */ (function () {
    function DocsOutTabComponent() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
    }
    DocsOutTabComponent.prototype.DocsOutTab = function () {
        this.Helper.WaitByIdAndClick('Shipment.TH.DocsOut');
    };
    DocsOutTabComponent.prototype.QuickSearchDocOut = function (docsOutId, docOutRow, searchTerm) {
        this.UseDocsOutSearchBox('SearchFieldsId_0_0', searchTerm, docsOutId, docOutRow);
    };
    DocsOutTabComponent.prototype.UseDocsOutSearchBox = function (searchFeildId, searchByRef, docOutId, docOutRow) {
        this.Helper.WaitByIdAndFill(searchFeildId, searchByRef);
        this.Helper.WaitByIdAndClick(docOutRow);
        this.Helper.WaitByIdAndClick(docOutId);
    };
    return DocsOutTabComponent;
}());
exports.DocsOutTabComponent = DocsOutTabComponent;
//# sourceMappingURL=DocsOutTab.js.map