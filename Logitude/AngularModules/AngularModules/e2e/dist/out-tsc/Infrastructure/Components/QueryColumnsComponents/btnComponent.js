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
var CommonDomainService_1 = require("../../../Common/Services/CommonDomainService");
var InfraSettings_1 = require("../../../Infrastructure/Utilities/InfraSettings");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var PartnersDomainService_1 = require("../../../Common/Services/PartnersDomainService");
var EntityListService_1 = require("../../../Infrastructure/Services/EntityListService");
var CardList_1 = require("../../../Common/EntityLists/CardList");
var CachedDataManager_1 = require("../../../Infrastructure/Utilities/CachedDataManager");
var btnComponent = /** @class */ (function () {
    function btnComponent(CD, _entityListService) {
        this.CD = CD;
        this._entityListService = _entityListService;
        this.InUseVisibile = true;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsCompleted = false;
        this.ShippingLinesList = [];
        this.TenantPM = InfraSettings_1.InfraSettings.TenantPM;
        this.LoadShippingLineListMethod();
    }
    btnComponent.prototype.setVariables = function (rowData, fieldName) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        //this.Check();
        this.InUseVisibile = this.rowData.InUse;
        var isDestroyed = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    };
    btnComponent.prototype.ngOnInit = function () {
    };
    btnComponent.prototype.Check = function () {
        var _this = this;
        switch (this.fieldName) {
            case "ShippingLine":
                {
                    this.GetInUseCarrier("SL", this.rowData.Code);
                    break;
                }
            case "Airline":
                {
                    this.GetInUseCarrier("AL", this.rowData.Code);
                    break;
                }
            case "Port":
                {
                    var y = window.Ports.filter(function (x) { return x.Tenant === _this.TenantPM.Id && x.Code == _this.rowData.Code && x.CountryCode == _this.rowData.CountryCode; });
                    if (y.length != 0) {
                        // this.AddButtonVisibile = false;
                    }
                    else {
                        // this.AddButtonVisibile = true;
                    }
                    break;
                }
            case "Warehouse":
                {
                    this.GetInUseCarrier("WH", this.rowData.Code);
                    break;
                }
        }
    };
    btnComponent.prototype.DoItClick = function () {
        this.entityId = this.rowData.Id;
        switch (this.fieldName) {
            case "ShippingLine":
            case "Airline":
            case "Warehouse":
                {
                    this.StartBusyIndicator("Adding " + this.fieldName + " to your list");
                    this.GetCarrierCopyToCurrentTenant();
                    break;
                }
            case "Port":
                {
                    this.StartBusyIndicator("Adding " + this.fieldName + " to your list");
                    this.GetPortCopyToCurrentTenant();
                    break;
                }
        }
    };
    btnComponent.prototype.RefreshDate = function () {
        this.InUseVisibile = true;
        var isDestroyed = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
        if (this.IsCompleted) {
            this.FireEvent("TenantImport");
        }
    };
    btnComponent.prototype.FireEvent = function (eventArgs) {
        this.CurrentSession.SessionEvent.emit(eventArgs);
    };
    btnComponent.prototype.GetPortCopyToCurrentTenant = function () {
        var _this = this;
        var myService = new CommonDomainService_1.CommonDomainService();
        myService.GetPortCopyToCurrentTenant(this.entityId).subscribe(function (myResult) {
            var mm = myResult;
            if (!mm.HasError) {
                CachedDataManager_1.CachedDataManager.RefreshTableData(_this.fieldName, true);
                _this.IsCompleted = true;
                _this.RefreshDate();
            }
            _this.StopBusyIndicator();
        });
    };
    btnComponent.prototype.GetCarrierCopyToCurrentTenant = function () {
        var _this = this;
        var myService = new PartnersDomainService_1.PartnersDomainService();
        myService.GetCarrierCopyToCurrentTenant(this.entityId).subscribe(function (myResult) {
            var mm = myResult;
            if (!mm.HasError) {
                CachedDataManager_1.CachedDataManager.RefreshTableData(_this.fieldName, true);
                CachedDataManager_1.CachedDataManager.RefreshTableData("Carrier", true);
                _this.IsCompleted = true;
                _this.RefreshDate();
            }
            _this.StopBusyIndicator();
        });
    };
    btnComponent.prototype.GetInUseCarrier = function (type, code) {
        var _this = this;
        var myService = new PartnersDomainService_1.PartnersDomainService();
        myService.GetInUseCarrier(type, code).subscribe(function (myResult) {
            _this.InUseVisibile = myResult.Result;
        });
    };
    btnComponent.prototype.LoadShippingLineListMethod = function () {
        var m = new CardList_1.CardList();
        m.Code = "ACLU";
        this.ShippingLinesList.push(m);
    };
    btnComponent.prototype.StartBusyIndicator = function (message) {
        this.CurrentSession.StartBusyIndicator(message);
    };
    btnComponent.prototype.StopBusyIndicator = function () {
        this.CurrentSession.StopBusyIndicator();
    };
    btnComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'btnComponent',
            templateUrl: './btnComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef, EntityListService_1.EntityListService])
    ], btnComponent);
    return btnComponent;
}());
exports.btnComponent = btnComponent;
//# sourceMappingURL=btnComponent.js.map