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
var InfraSettings_1 = require("../../../Infrastructure/Utilities/InfraSettings");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var PartnersDomainService_1 = require("../../../Common/Services/PartnersDomainService");
var EntityListService_1 = require("../../../Infrastructure/Services/EntityListService");
var CardList_1 = require("../../../Common/EntityLists/CardList");
var CachedDataManager_1 = require("../../../Infrastructure/Utilities/CachedDataManager");
var btnUpdateComponent = /** @class */ (function () {
    function btnUpdateComponent(CD, _entityListService) {
        this.CD = CD;
        this._entityListService = _entityListService;
        this.InUseVisibile = true;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsCompleted = false;
        this.ShippingLinesList = [];
        this.TenantPM = InfraSettings_1.InfraSettings.TenantPM;
        this.LoadShippingLineListMethod();
    }
    btnUpdateComponent.prototype.setVariables = function (rowData, fieldName) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.InUseVisibile = this.rowData.InUse;
        //this.Check();
        var isDestroyed = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    };
    btnUpdateComponent.prototype.ngOnInit = function () {
    };
    btnUpdateComponent.prototype.Check = function () {
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
                    break;
                }
        }
    };
    btnUpdateComponent.prototype.DoItClick = function () {
        this.entityId = this.rowData.Id;
        switch (this.fieldName) {
            case "ShippingLine":
            case "Airline":
                {
                    this.StartBusyIndicator("Updating" + this.fieldName + " to your list");
                    this.GetCarrierUpdate();
                    break;
                }
        }
    };
    btnUpdateComponent.prototype.RefreshDate = function () {
        this.InUseVisibile = true;
        var isDestroyed = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
        if (this.IsCompleted) {
            this.FireEvent("TenantImport");
        }
    };
    btnUpdateComponent.prototype.RefreshDateUpdated = function () {
        var isDestroyed = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
        if (this.IsCompleted) {
            this.FireEvent("TenantImport");
        }
    };
    btnUpdateComponent.prototype.FireEvent = function (eventArgs) {
        this.CurrentSession.SessionEvent.emit(eventArgs);
    };
    btnUpdateComponent.prototype.GetCarrierUpdate = function () {
        var _this = this;
        var myService = new PartnersDomainService_1.PartnersDomainService();
        myService.GetCarrierUpdate(this.entityId).subscribe(function (myResult) {
            var mm = myResult;
            _this.StopBusyIndicator();
            if (!mm.HasError) {
                CachedDataManager_1.CachedDataManager.RefreshTableData(_this.fieldName, true);
                CachedDataManager_1.CachedDataManager.RefreshTableData("Carrier", true);
                _this.IsCompleted = true;
                _this.RefreshDateUpdated();
            }
        });
    };
    btnUpdateComponent.prototype.GetInUseCarrier = function (type, code) {
        var _this = this;
        var myService = new PartnersDomainService_1.PartnersDomainService();
        myService.GetInUseCarrier(type, code).subscribe(function (myResult) {
            _this.InUseVisibile = myResult.Result;
        });
    };
    btnUpdateComponent.prototype.LoadShippingLineListMethod = function () {
        var m = new CardList_1.CardList();
        m.Code = "ACLU";
        this.ShippingLinesList.push(m);
    };
    btnUpdateComponent.prototype.StartBusyIndicator = function (message) {
        this.CurrentSession.StartBusyIndicator(message);
    };
    btnUpdateComponent.prototype.StopBusyIndicator = function () {
        this.CurrentSession.StopBusyIndicator();
    };
    btnUpdateComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'btnUpdateComponent',
            templateUrl: './btnUpdateComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef, EntityListService_1.EntityListService])
    ], btnUpdateComponent);
    return btnUpdateComponent;
}());
exports.btnUpdateComponent = btnUpdateComponent;
//# sourceMappingURL=btnUpdateComponent.js.map