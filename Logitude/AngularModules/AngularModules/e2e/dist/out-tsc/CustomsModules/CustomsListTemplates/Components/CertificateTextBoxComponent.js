"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var MultiCertificatesService_1 = require("../../../Customs/Services/Others/MultiCertificatesService");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var DeclarationEventManager_1 = require("../../../Customs/Utilities/DeclarationEventManager");
var CertificateTextBoxComponent = /** @class */ (function (_super) {
    __extends(CertificateTextBoxComponent, _super);
    function CertificateTextBoxComponent(cd) {
        var _this = _super.call(this) || this;
        _this.cd = cd;
        _this.DataContext = _this;
        _this.multiCertificatesService = new MultiCertificatesService_1.MultiCertificatesService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    CertificateTextBoxComponent.prototype.ngOnDestroy = function () {
        console.log("CertificateTextBoxComponent:ngOnDestroy");
        if (this._SubDisplayModeChanged) {
            this._SubDisplayModeChanged.unsubscribe();
            this._SubDisplayModeChanged = null;
        }
    };
    CertificateTextBoxComponent.prototype.setVariables = function (rowData, fieldName) {
        var _this = this;
        this.rowData = rowData;
        this.CatalogNumber = rowData.CatalogNumber;
        this._SubDisplayModeChanged =
            DeclarationEventManager_1.DeclarationEventManager.DisplayModeChanged.subscribe(function (IsDisplayOnly) {
                if (IsDisplayOnly) {
                    _this.UIProperties.SetEnabled("CatalogNumber", "Customs.SupplierInvoiceItem", false);
                }
                else {
                    _this.UIProperties.SetEnabled("CatalogNumber", "Customs.SupplierInvoiceItem", true);
                }
            });
    };
    Object.defineProperty(CertificateTextBoxComponent.prototype, "CatalogNumber", {
        get: function () { return this.catalogNumber; },
        set: function (newValue) { this.catalogNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    CertificateTextBoxComponent.prototype.clicked = function () {
        this.CurrentSession.PseventRowSelectEvent.emit("certificate");
        this.cellClicked = true;
        this.cd.detectChanges();
    };
    CertificateTextBoxComponent.prototype.onBlur = function () {
        this.cellClicked = false;
        this.cd.detectChanges();
        //InvokeOperation op = trigger.CustomContext.UpdateSuppkierInvoiceItemCatalogNumber(DeclarationId, CatalogNumber, InvoiceCounterKey, LineNumber, trigger.entityPM.Tenant);
        var item = this.rowData;
        item.CatalogNumber = this.CatalogNumber;
        this.multiCertificatesService.PutSupplierInvoiceItemCatalogNumber(item)
            .subscribe(function (response) {
            if (!response.HasError) {
            }
        });
    };
    CertificateTextBoxComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'CertificateTextBoxComponent',
            templateUrl: './CertificateTextBoxComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], CertificateTextBoxComponent);
    return CertificateTextBoxComponent;
}(BaseComponent_1.BaseComponent));
exports.CertificateTextBoxComponent = CertificateTextBoxComponent;
//# sourceMappingURL=CertificateTextBoxComponent.js.map