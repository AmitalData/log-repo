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
var CustomMessageWrapperComponent_1 = require("../../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var ReleaseGoodsResponseData_1 = require("../../../../Customs/DataContract/ResponseData/ReleaseGoodsResponseData");
var BaseRequestsSheetMassaging_1 = require("../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var ReleaseGoodsComponent = /** @class */ (function (_super) {
    __extends(ReleaseGoodsComponent, _super);
    function ReleaseGoodsComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.Declaration";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SuperCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        _this.GoodsItemsList = new ObservableCollection_1.ObservableCollection([]);
        _this.CurrentSession.entityResourceService.getEntityResourceByTableName("Customs.ReleaseGoods").subscribe(function (response) {
        });
        return _this;
    }
    ReleaseGoodsComponent.prototype.ngAfterViewInit = function () {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();
    };
    ReleaseGoodsComponent.prototype.OnMassageDisplayMethod = function () {
        if (this.RequestParams == null) {
            //this.RequestParams = new RequestParamsBase();
        }
        if (this.ResponseData) {
            if (this.ResponseData.GoodsItemsList) {
                this.GoodsItemsList.InsertCollection(this.ResponseData.GoodsItemsList);
            }
        }
        else {
            this.ResponseData = new ReleaseGoodsResponseData_1.ReleaseGoodsResponseData();
        }
    };
    ReleaseGoodsComponent.prototype.OnRowLoaded = function (myRow) {
        if (myRow) {
            myRow.SetExpandaple(true);
        }
    };
    Object.defineProperty(ReleaseGoodsComponent.prototype, "DeclarationNumber", {
        //#region Properties
        get: function () { return this.ResponseData ? this.ResponseData.DeclarationNumber : null; },
        set: function (value) {
            if (this.ResponseData.DeclarationNumber != value) {
                this.ResponseData.DeclarationNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ReleaseGoodsComponent.prototype, "FileNumber", {
        get: function () { return this.ResponseData ? this.ResponseData.FileNumber : null; },
        set: function (value) {
            if (this.ResponseData.FileNumber != value) {
                this.ResponseData.FileNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ReleaseGoodsComponent.prototype, "governmentProcedureType", {
        get: function () { return this.ResponseData ? this.ResponseData.governmentProcedureType : null; },
        set: function (value) {
            if (this.ResponseData.governmentProcedureType != value) {
                this.ResponseData.governmentProcedureType = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ReleaseGoodsComponent.prototype, "releaseDate", {
        get: function () { return this.ResponseData ? this.ResponseData.releaseDate : null; },
        set: function (value) {
            if (this.ResponseData.releaseDate != value) {
                this.ResponseData.releaseDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ReleaseGoodsComponent.prototype, "dealValueNIS", {
        get: function () { return this.ResponseData ? this.ResponseData.dealValueNIS : null; },
        set: function (value) {
            if (this.ResponseData.dealValueNIS != value) {
                this.ResponseData.dealValueNIS = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ReleaseGoodsComponent.prototype, "CifValueNis", {
        get: function () { return this.ResponseData ? this.ResponseData.CifValueNis : null; },
        set: function (value) {
            if (this.ResponseData.CifValueNis != value) {
                this.ResponseData.CifValueNis = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ReleaseGoodsComponent.prototype, "CurrencyTypeCode", {
        get: function () { return this.ResponseData ? this.ResponseData.CurrencyTypeCode : null; },
        set: function (value) {
            if (this.ResponseData.CurrencyTypeCode != value) {
                this.ResponseData.CurrencyTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ReleaseGoodsComponent.prototype, "ExchangeRate", {
        get: function () { return this.ResponseData ? this.ResponseData.ExchangeRate : null; },
        set: function (value) {
            if (this.ResponseData.ExchangeRate != value) {
                this.ResponseData.ExchangeRate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ReleaseGoodsComponent.prototype, "TaxationDate", {
        get: function () { return this.ResponseData ? this.ResponseData.TaxationDate : null; },
        set: function (value) {
            if (this.ResponseData.TaxationDate != value) {
                this.ResponseData.TaxationDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ReleaseGoodsComponent.prototype, "importerExpoterExternalID", {
        get: function () { return this.ResponseData ? this.ResponseData.importerExpoterExternalID : null; },
        set: function (value) {
            if (this.ResponseData.importerExpoterExternalID != value) {
                this.ResponseData.importerExpoterExternalID = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ReleaseGoodsComponent.prototype, "cargoIdentifierType", {
        get: function () { return this.ResponseData ? this.ResponseData.cargoIdentifierType : null; },
        set: function (value) {
            if (this.ResponseData.cargoIdentifierType != value) {
                this.ResponseData.cargoIdentifierType = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ReleaseGoodsComponent.prototype, "cargoIdentifierKey1", {
        get: function () { return this.ResponseData ? this.ResponseData.cargoIdentifierKey1 : null; },
        set: function (value) {
            if (this.ResponseData.cargoIdentifierKey1 != value) {
                this.ResponseData.cargoIdentifierKey1 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ReleaseGoodsComponent.prototype, "cargoIdentifierKey2", {
        get: function () { return this.ResponseData ? this.ResponseData.cargoIdentifierKey2 : null; },
        set: function (value) {
            if (this.ResponseData.cargoIdentifierKey2 != value) {
                this.ResponseData.cargoIdentifierKey2 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ReleaseGoodsComponent.prototype, "loadingPort", {
        get: function () { return this.ResponseData ? this.ResponseData.loadingPort : null; },
        set: function (value) {
            if (this.ResponseData.loadingPort != value) {
                this.ResponseData.loadingPort = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ReleaseGoodsComponent.prototype, "unloadingSiteNumber", {
        get: function () { return this.ResponseData ? this.ResponseData.unloadingSiteNumber : null; },
        set: function (value) {
            if (this.ResponseData.unloadingSiteNumber != value) {
                this.ResponseData.unloadingSiteNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ReleaseGoodsComponent.prototype, "storageSiteNumber", {
        get: function () { return this.ResponseData ? this.ResponseData.storageSiteNumber : null; },
        set: function (value) {
            if (this.ResponseData.storageSiteNumber != value) {
                this.ResponseData.storageSiteNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ReleaseGoodsComponent.prototype, "cargoDescription", {
        get: function () { return this.ResponseData ? this.ResponseData.cargoDescription : null; },
        set: function (value) {
            if (this.ResponseData.cargoDescription != value) {
                this.ResponseData.cargoDescription = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ReleaseGoodsComponent.prototype, "packageType", {
        get: function () { return this.ResponseData ? this.ResponseData.packageType : null; },
        set: function (value) {
            if (this.ResponseData.packageType != value) {
                this.ResponseData.packageType = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ReleaseGoodsComponent.prototype, "packageQuantity", {
        get: function () { return this.ResponseData ? this.ResponseData.packageQuantity : null; },
        set: function (value) {
            if (this.ResponseData.packageQuantity != value) {
                this.ResponseData.packageQuantity = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ReleaseGoodsComponent.prototype, "packagesWeight", {
        get: function () { return this.ResponseData ? this.ResponseData.packagesWeight : null; },
        set: function (value) {
            if (this.ResponseData.packagesWeight != value) {
                this.ResponseData.packagesWeight = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#region General Commands
    ReleaseGoodsComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    ReleaseGoodsComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) { };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent)
    ], ReleaseGoodsComponent.prototype, "SuperCustomMessageWrapperComponent", void 0);
    ReleaseGoodsComponent = __decorate([
        core_1.Component({
            selector: 'ReleaseGoodsComponent',
            moduleId: module.id,
            templateUrl: './ReleaseGoodsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ReleaseGoodsComponent);
    return ReleaseGoodsComponent;
}(BaseRequestsSheetMassaging_1.BaseRequestsSheetMassaging));
exports.ReleaseGoodsComponent = ReleaseGoodsComponent;
//# sourceMappingURL=ReleaseGoodsComponent.js.map