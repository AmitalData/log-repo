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
var DeclarationExtendedListService_1 = require("../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService");
var DeclarationMessagesService_1 = require("../../../../Customs/Services/WebServices/DeclarationMessagesService");
var WarehouseBlockBalanceRequestParams_1 = require("../../../../Customs/DataContract/RequestParams/WarehouseBlockBalanceRequestParams");
var WarehouseBlockBalanceResponseData_1 = require("../../../../Customs/DataContract/ResponseData/WarehouseBlockBalanceResponseData");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var BaseRequestsSheetMassaging_1 = require("../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging");
var CustomMessageProgressComponent_1 = require("../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var WarehouseBlockBalanceComponent = /** @class */ (function (_super) {
    __extends(WarehouseBlockBalanceComponent, _super);
    function WarehouseBlockBalanceComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.Declaration";
        _this._DeclarationExtendedListService = new DeclarationExtendedListService_1.DeclarationExtendedListService();
        _this._DeclarationMessagesService = new DeclarationMessagesService_1.DeclarationMessagesService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SuperCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        _this.BlockSpecialActivitiesList = new ObservableCollection_1.ObservableCollection([]);
        _this.ActionList = new ObservableCollection_1.ObservableCollection([]);
        _this.StorageActionList = new ObservableCollection_1.ObservableCollection([]);
        _this.GoodsItemByInvoiceList = new ObservableCollection_1.ObservableCollection([]);
        return _this;
    }
    WarehouseBlockBalanceComponent.prototype.ngAfterViewInit = function () {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();
    };
    WarehouseBlockBalanceComponent.prototype.OnMassageDisplayMethod = function () {
        if (this.RequestParams == null) {
            this.RequestParams = new WarehouseBlockBalanceRequestParams_1.WarehouseBlockBalanceRequestParams();
            this.SetIsByDeclarationNumber(true);
            this.UIProperties.SetRequired("DeclarationNumber", this.ObjectTableName, true);
        }
        if (this.ResponseData) {
            if (this.ResponseData.BlockSpecialActivitiesList) {
                this.BlockSpecialActivitiesList.InsertCollection(this.ResponseData.BlockSpecialActivitiesList);
            }
            if (this.ResponseData.ActionList) {
                this.ActionList.InsertCollection(this.ResponseData.ActionList);
            }
            if (this.ResponseData.StorageActionList) {
                for (var _i = 0, _a = this.ResponseData.StorageActionList; _i < _a.length; _i++) {
                    var item = _a[_i];
                    item.PackingDetailsListObs = new ObservableCollection_1.ObservableCollection(item.PackingDetailsList);
                }
                this.StorageActionList.InsertCollection(this.ResponseData.StorageActionList);
            }
            if (this.ResponseData.GoodsItemByInvoiceList) {
                this.GoodsItemByInvoiceList.InsertCollection(this.ResponseData.GoodsItemByInvoiceList);
            }
        }
        else {
            this.ResponseData = new WarehouseBlockBalanceResponseData_1.WarehouseBlockBalanceResponseData();
        }
    };
    WarehouseBlockBalanceComponent.prototype.OnRowLoaded = function (myRow) {
        if (myRow) {
            myRow.SetExpandaple(true);
        }
    };
    //#region Properties
    WarehouseBlockBalanceComponent.prototype.SetIsByDeclarationNumber = function (newValue) {
        this.ClearOldValues();
        this.IsByDeclarationNumber = newValue;
    };
    Object.defineProperty(WarehouseBlockBalanceComponent.prototype, "IsByDeclarationNumber", {
        get: function () { return this.RequestParams ? this.RequestParams.DeclarationRadio : null; },
        set: function (newValue) {
            if (this.RequestParams.DeclarationRadio != newValue) {
                this.RequestParams.DeclarationRadio = newValue;
                if (newValue == true) {
                    this.IsByStorageSite = false;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    WarehouseBlockBalanceComponent.prototype.SetIsByStorageSite = function (newValue) {
        this.ClearOldValues();
        this.IsByStorageSite = newValue;
    };
    WarehouseBlockBalanceComponent.prototype.ClearOldValues = function () {
        this.CustomFileNo = "";
        this.DeclarationNumber = "";
        this.StorageSiteNumber = "";
        this.WarehouseBlockNumber = "";
        this.DisplayGoodsItemByInvoice = "";
    };
    Object.defineProperty(WarehouseBlockBalanceComponent.prototype, "IsByStorageSite", {
        get: function () { return this.RequestParams ? this.RequestParams.StorageSiteRadio : null; },
        set: function (newValue) {
            if (this.RequestParams.StorageSiteRadio != newValue) {
                this.RequestParams.StorageSiteRadio = newValue;
                if (newValue == true) {
                    this.IsByDeclarationNumber = false;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseBlockBalanceComponent.prototype, "CustomFileNo", {
        get: function () { return this.RequestParams ? this.RequestParams.CustomFileNo : null; },
        set: function (value) {
            if (this.RequestParams.CustomFileNo != value) {
                this.RequestParams.CustomFileNo = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseBlockBalanceComponent.prototype, "DeclarationNumber", {
        get: function () { return this.RequestParams ? this.RequestParams.DeclarationNumber : null; },
        set: function (value) {
            if (this.RequestParams.DeclarationNumber != value) {
                this.RequestParams.DeclarationNumber = value;
            }
            if (value) {
                this.UIProperties.SetRequired("DeclarationNumber", this.ObjectTableName, false);
            }
            else {
                this.UIProperties.SetRequired("DeclarationNumber", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseBlockBalanceComponent.prototype, "DisplayGoodsItemByInvoice", {
        get: function () { return this.RequestParams ? this.RequestParams.DisplayGoodsItemByInvoice : null; },
        set: function (value) {
            if (this.RequestParams.DisplayGoodsItemByInvoice != value) {
                this.RequestParams.DisplayGoodsItemByInvoice = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseBlockBalanceComponent.prototype, "StorageSiteNumber", {
        get: function () { return this.RequestParams ? this.RequestParams.StorageSiteNumber : null; },
        set: function (value) {
            if (this.RequestParams.StorageSiteNumber != value) {
                this.RequestParams.StorageSiteNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseBlockBalanceComponent.prototype, "WarehouseBlockNumber", {
        get: function () { return this.RequestParams ? this.RequestParams.WarehouseBlockNumber : null; },
        set: function (value) {
            if (this.RequestParams.WarehouseBlockNumber != value) {
                this.RequestParams.WarehouseBlockNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseBlockBalanceComponent.prototype, "SiteText", {
        //#endregion Properties
        //#endregion Response Properties
        get: function () { return this.ResponseData ? this.ResponseData.SiteText : null; },
        set: function (value) {
            if (this.ResponseData.SiteText != value) {
                this.ResponseData.SiteText = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseBlockBalanceComponent.prototype, "OpeningDate", {
        get: function () { return this.ResponseData ? this.ResponseData.OpeningDate : null; },
        set: function (value) {
            if (this.ResponseData.OpeningDate != value) {
                this.ResponseData.OpeningDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseBlockBalanceComponent.prototype, "LogicalPackagesQuantityBalance", {
        get: function () { return this.ResponseData ? this.ResponseData.LogicalPackagesQuantityBalance : null; },
        set: function (value) {
            if (this.ResponseData.LogicalPackagesQuantityBalance != value) {
                this.ResponseData.LogicalPackagesQuantityBalance = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseBlockBalanceComponent.prototype, "ResponseDeclarationNumber", {
        get: function () { return this.ResponseData ? this.ResponseData.DeclarationNumber : null; },
        set: function (value) {
            if (this.ResponseData.DeclarationNumber != value) {
                this.ResponseData.DeclarationNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseBlockBalanceComponent.prototype, "OriginalOpeningDate", {
        get: function () { return this.ResponseData ? this.ResponseData.OriginalOpeningDate : null; },
        set: function (value) {
            if (this.ResponseData.OriginalOpeningDate != value) {
                this.ResponseData.OriginalOpeningDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseBlockBalanceComponent.prototype, "PhysicalPackagesQuantityBalance", {
        get: function () { return this.ResponseData ? this.ResponseData.PhysicalPackagesQuantityBalance : null; },
        set: function (value) {
            if (this.ResponseData.PhysicalPackagesQuantityBalance != value) {
                this.ResponseData.PhysicalPackagesQuantityBalance = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseBlockBalanceComponent.prototype, "ResponseWarehouseBlockNumber", {
        get: function () { return this.ResponseData ? this.ResponseData.WarehouseBlockNumber : null; },
        set: function (value) {
            if (this.ResponseData.WarehouseBlockNumber != value) {
                this.ResponseData.WarehouseBlockNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseBlockBalanceComponent.prototype, "MaxStorageDate", {
        get: function () { return this.ResponseData ? this.ResponseData.MaxStorageDate : null; },
        set: function (value) {
            if (this.ResponseData.MaxStorageDate != value) {
                this.ResponseData.MaxStorageDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseBlockBalanceComponent.prototype, "Value", {
        get: function () { return this.ResponseData ? this.ResponseData.Value : null; },
        set: function (value) {
            if (this.ResponseData.Value != value) {
                this.ResponseData.Value = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseBlockBalanceComponent.prototype, "ImporterTitle", {
        get: function () { return this.ResponseData ? this.ResponseData.ImporterTitle : null; },
        set: function (value) {
            if (this.ResponseData.ImporterTitle != value) {
                this.ResponseData.ImporterTitle = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion Response Properties
    //#region Declaration Commands
    WarehouseBlockBalanceComponent.prototype.DueChangeClearChildField = function (sourceIsCostomFile) {
        this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
        this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, true, "");
        if (sourceIsCostomFile) {
            this.DeclarationNumber = "";
        }
        else {
            this.CustomFileNo = "";
        }
        this.ValidationErrorsList = [];
    };
    WarehouseBlockBalanceComponent.prototype.CustomFileNoTextChanged = function (searchtext) {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.CustomFileNo)) {
            return;
        }
        this.DueChangeClearChildField(true);
        this.CurrentSession.StartBusyIndicator("");
        this._DeclarationExtendedListService.GetSingleDeclarationByCustomFileNo(this.CustomFileNo)
            .subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            _this.FetchDeclaration(myResponse, true);
        });
    };
    WarehouseBlockBalanceComponent.prototype.DeclarationNumberTextChanged = function (DeclarationNumberText) {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.DeclarationNumber)) {
            return;
        }
        this.DueChangeClearChildField(false);
        this.CurrentSession.StartBusyIndicator("");
        this._DeclarationExtendedListService.GetSingleDeclarationByNumber(this.DeclarationNumber, SessionLocator_1.SessionLocator.Tenant)
            .subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            _this.FetchDeclaration(myResponse, false);
        });
    };
    WarehouseBlockBalanceComponent.prototype.FetchDeclaration = function (myResponse, sourceIsCostomFile) {
        var lastFetchDeclarationList = myResponse.Result;
        if (lastFetchDeclarationList != null) {
            this.DeclarationNumber = lastFetchDeclarationList.DeclarationNumber;
            this.CustomFileNo = lastFetchDeclarationList.CustomFileNo;
            this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
            this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, true, "");
        }
        else {
            if (sourceIsCostomFile) {
                this.SetValidityCustomFileNo();
            }
        }
    };
    WarehouseBlockBalanceComponent.prototype.SetValidityDeclarationNumber = function () {
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationNumberIsMandatory");
        this.ValidationErrorsList.push(msg);
        this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, false, msg);
    };
    WarehouseBlockBalanceComponent.prototype.SetValidityCustomFileNo = function () {
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Didntfindcustomfile");
        this.ValidationErrorsList.push(msg);
        this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, false, msg);
    };
    //#endregion
    //#region General Commands
    WarehouseBlockBalanceComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    WarehouseBlockBalanceComponent.prototype.FillErrors = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.IsByDeclarationNumber) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.RequestParams.DeclarationNumber)) {
                var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationNumberIsMandatory");
                this.ValidationErrorsList.push(msg);
            }
        }
        else if (this.IsByStorageSite) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.RequestParams.StorageSiteNumber)) {
                var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.StorageSiteMissing");
                this.ValidationErrorsList.push(msg);
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.RequestParams.WarehouseBlockNumber)) {
                var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.WarehouseBlockMissing");
                this.ValidationErrorsList.push(msg);
            }
        }
    };
    WarehouseBlockBalanceComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        var _this = this;
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        var currRequestParams = new WarehouseBlockBalanceRequestParams_1.WarehouseBlockBalanceRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.DeclarationRadio = this.IsByDeclarationNumber;
        currRequestParams.StorageSiteRadio = this.IsByStorageSite;
        currRequestParams.DeclarationNumber = this.DeclarationNumber;
        currRequestParams.CustomFileNo = this.CustomFileNo;
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "שליחת שאילתא ליתרת מלאי בגוש", true)
            .then(function (res) {
            _this.ResponseData = res;
            _this.OnMassageDisplayMethod();
        }).catch(function (err) {
            _this.ValidationErrorsList.push(err);
        });
        this._DeclarationMessagesService.PostWarehouseBlockBalanceRequest(currRequestParams)
            .subscribe(function (myServiceResponse) {
        });
    };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent)
    ], WarehouseBlockBalanceComponent.prototype, "SuperCustomMessageWrapperComponent", void 0);
    WarehouseBlockBalanceComponent = __decorate([
        core_1.Component({
            selector: 'WarehouseBlockBalanceComponent',
            moduleId: module.id,
            templateUrl: './WarehouseBlockBalanceComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], WarehouseBlockBalanceComponent);
    return WarehouseBlockBalanceComponent;
}(BaseRequestsSheetMassaging_1.BaseRequestsSheetMassaging));
exports.WarehouseBlockBalanceComponent = WarehouseBlockBalanceComponent;
//# sourceMappingURL=WarehouseBlockBalanceComponent.js.map