"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../../../Helpers/FieldsHelper");
var PrintDocsOutTabComponent = /** @class */ (function () {
    function PrintDocsOutTabComponent() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
    }
    PrintDocsOutTabComponent.prototype.PrintDocsOutTab = function () {
        this.Helper.WaitByIdAndClick('Shipment.TH.DocsOut');
    };
    PrintDocsOutTabComponent.prototype.QuickSearchDocOut = function (docsOutId, docOutRow) {
        this.UseDocsOutSearchBox('SearchFieldsId_0_0', 'HAWB Lable ', docsOutId, docOutRow);
    };
    PrintDocsOutTabComponent.prototype.UseDocsOutSearchBox = function (searchFeildId, searchByRef, docOutId, docOutRow) {
        this.Helper.WaitByIdAndFill(searchFeildId, searchByRef);
        this.Helper.WaitByIdAndClick(docOutRow);
        this.Helper.WaitByIdAndClick(docOutId);
    };
    return PrintDocsOutTabComponent;
}());
exports.PrintDocsOutTabComponent = PrintDocsOutTabComponent;
//# sourceMappingURL=PrintOutDoc.js.map