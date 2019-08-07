"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var SupplierInvoiceItemsTaxListTemplate = /** @class */ (function () {
    function SupplierInvoiceItemsTaxListTemplate(CD) {
        this.CD = CD;
    }
    SupplierInvoiceItemsTaxListTemplate.prototype.setVariables = function (rowData, fieldName) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.CD.detectChanges();
    };
    SupplierInvoiceItemsTaxListTemplate.prototype.ShowTaxMore = function () {
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 800;
        logitudeWindow.Height = 500;
        logitudeWindow.IsShowCloseButton = logitudeWindow.IsShowCloseButton = true;
        logitudeWindow.ToShowCloseButton(true);
        //logitudeWindow.Title = TextCodeTranslator.Translate("CommunicationLog.O.MoreDetails");;
        logitudeWindow.WindowArgs = this.rowData;
        logitudeWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/Taxes/ItemTaxesMoreFieldsComponent');
    };
    Object.defineProperty(SupplierInvoiceItemsTaxListTemplate.prototype, "MyTaxToPay", {
        get: function () {
            var taxToPay = (this.rowData.TaxAmount == null ? 0 : this.rowData.TaxAmount) + (this.rowData.DeferedTaxAmount == null ? 0 : this.rowData.DeferedTaxAmount);
            if (taxToPay == 0) {
                return null;
            }
            return taxToPay;
        },
        enumerable: true,
        configurable: true
    });
    SupplierInvoiceItemsTaxListTemplate = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './SupplierInvoiceItemsTaxListTemplate.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], SupplierInvoiceItemsTaxListTemplate);
    return SupplierInvoiceItemsTaxListTemplate;
}());
exports.SupplierInvoiceItemsTaxListTemplate = SupplierInvoiceItemsTaxListTemplate;
//# sourceMappingURL=SupplierInvoiceItemsTaxListTemplate.js.map