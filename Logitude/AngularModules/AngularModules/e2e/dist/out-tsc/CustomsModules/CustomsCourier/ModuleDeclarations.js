"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var CMConnectedDeclarationTabComponent_1 = require("./Components/EditTabs/CMConnectedDeclarationTabComponent");
var CourierPendingReasonGeneralComponent_1 = require("./Components/CourierPendingReason/CourierPendingReasonGeneralComponent");
var NewCourierComponent_1 = require("./Components/NewEntity/NewCourierComponent");
var AddEditCouriersVatComponent_1 = require("./Components/CourierVat/AddEditCouriersVatComponent");
var AddEditCourierPendingReasonComponent_1 = require("./Components/CourierPendingReason/AddEditCourierPendingReasonComponent");
var DropdownMenuFilterComponent_1 = require("./Components/CourierWorkSheet/DropdownMenuFilterComponent");
var CourierMasterGeneralTabComponent_1 = require("./Components/EditTabs/CourierMasterGeneralTabComponent");
var CourierWorksheetComponent_1 = require("./Components/CourierWorkSheet/CourierWorksheetComponent");
var GetInternalBankComponent_1 = require("./Components/CourierWorkSheet/GetInternalBankComponent");
var AddEditMamanStickerComponent_1 = require("./Components/MamanSpecialAction/AddEditMamanStickerComponent");
var AddCourierPendingToUnifreightStatusComponent_1 = require("./Components/CourierPendingReason/AddCourierPendingToUnifreightStatusComponent");
exports.Components = [
    CMConnectedDeclarationTabComponent_1.CMConnectedDeclarationTabComponent,
    CourierPendingReasonGeneralComponent_1.CourierPendingReasonGeneralComponent,
    NewCourierComponent_1.NewCourierComponent,
    AddEditCouriersVatComponent_1.AddEditCouriersVatComponent,
    AddEditCourierPendingReasonComponent_1.AddEditCourierPendingReasonComponent,
    DropdownMenuFilterComponent_1.DropdownMenuFilterComponent,
    CourierMasterGeneralTabComponent_1.CourierMasterGeneralTabComponent,
    CourierWorksheetComponent_1.CourierWorksheetComponent,
    GetInternalBankComponent_1.GetInternalBankComponent,
    AddEditMamanStickerComponent_1.AddEditMamanStickerComponent,
    AddCourierPendingToUnifreightStatusComponent_1.AddCourierPendingToUnifreightStatusComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "CMConnectedDeclarationTabComponent": {
                myResult = CMConnectedDeclarationTabComponent_1.CMConnectedDeclarationTabComponent;
                break;
            }
            case "CourierPendingReasonGeneralComponent": {
                myResult = CourierPendingReasonGeneralComponent_1.CourierPendingReasonGeneralComponent;
                break;
            }
            case "NewCourierComponent": {
                myResult = NewCourierComponent_1.NewCourierComponent;
                break;
            }
            case "AddEditCouriersVatComponent": {
                myResult = AddEditCouriersVatComponent_1.AddEditCouriersVatComponent;
                break;
            }
            case "AddEditCourierPendingReasonComponent": {
                myResult = AddEditCourierPendingReasonComponent_1.AddEditCourierPendingReasonComponent;
                break;
            }
            case "DropdownMenuFilterComponent": {
                myResult = DropdownMenuFilterComponent_1.DropdownMenuFilterComponent;
                break;
            }
            case "CourierMasterGeneralTabComponent": {
                myResult = CourierMasterGeneralTabComponent_1.CourierMasterGeneralTabComponent;
                break;
            }
            case "CourierWorksheetComponent": {
                myResult = CourierWorksheetComponent_1.CourierWorksheetComponent;
                break;
            }
            case "GetInternalBankComponent": {
                myResult = GetInternalBankComponent_1.GetInternalBankComponent;
                break;
            }
            case "AddEditMamanStickerComponent": {
                myResult = AddEditMamanStickerComponent_1.AddEditMamanStickerComponent;
                break;
            }
            case "AddCourierPendingToUnifreightStatusComponent": {
                myResult = AddCourierPendingToUnifreightStatusComponent_1.AddCourierPendingToUnifreightStatusComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map