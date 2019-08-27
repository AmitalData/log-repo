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
var IIGGeneralMessagesService_1 = require("../../../../Customs/Services/WebServices/IIGGeneralMessagesService");
var BlockListInWarehouseRequestParams_1 = require("../../../../Customs/DataContract/RequestParams/BlockListInWarehouseRequestParams");
var WarehouseBlockBalanceResponseData_1 = require("../../../../Customs/DataContract/ResponseData/WarehouseBlockBalanceResponseData");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var BaseRequestsSheetMassaging_1 = require("../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging");
var CustomMessageProgressComponent_1 = require("../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var BlockListInWarehouseComponent = /** @class */ (function (_super) {
    __extends(BlockListInWarehouseComponent, _super);
    function BlockListInWarehouseComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.Declaration";
        _this._DeclarationExtendedListService = new DeclarationExtendedListService_1.DeclarationExtendedListService();
        _this._IIGGeneralMessagesService = new IIGGeneralMessagesService_1.IIGGeneralMessagesService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SuperCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        //#region Properties
        _this._NumberOfBlocksInList = "";
        _this.ShowResetBlocksList =
            [
                { 'EnumId': 0, 'Name': 'No' },
                { 'EnumId': 1, 'Name': 'Yes' },
                { 'EnumId': 2, 'Name': 'All' }
            ];
        _this.BlockListInWarehouseResultList = new ObservableCollection_1.ObservableCollection([]);
        return _this;
    }
    BlockListInWarehouseComponent.prototype.ngAfterViewInit = function () {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();
    };
    BlockListInWarehouseComponent.prototype.OnMassageDisplayMethod = function () {
        if (this.RequestParams == null) {
            this.RequestParams = new BlockListInWarehouseRequestParams_1.BlockListInWarehouseRequestParams();
        }
        if (this.ResponseData) {
            this.NumberOfBlocksInList = this.ResponseData.NumberOfBlocksInList;
            if (this.ResponseData.BlockListInWarehouseResultList) {
                this.BlockListInWarehouseResultList.InsertCollection(this.ResponseData.BlockListInWarehouseResultList);
            }
        }
        else {
            this.ResponseData = new WarehouseBlockBalanceResponseData_1.WarehouseBlockBalanceResponseData();
        }
    };
    BlockListInWarehouseComponent.prototype.OnRowLoaded = function (myRow) {
        if (myRow) {
            myRow.SetExpandaple(true);
        }
    };
    Object.defineProperty(BlockListInWarehouseComponent.prototype, "NumberOfBlocksInList", {
        get: function () { return this._NumberOfBlocksInList; },
        set: function (value) {
            this._NumberOfBlocksInList = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BlockListInWarehouseComponent.prototype, "FromDate", {
        get: function () { return this.RequestParams.FromDate; },
        set: function (value) {
            if (this.RequestParams.FromDate != value) {
                this.RequestParams.FromDate = value;
                if (Tools_1.AppTool.IsNullOrEmpty(this.RequestParams.FromDate)) {
                    this.UIProperties.SetRequired("FromDate", this.ObjectTableName, true);
                }
                else {
                    this.UIProperties.SetRequired("FromDate", this.ObjectTableName, false);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BlockListInWarehouseComponent.prototype, "ToDate", {
        get: function () { return this.RequestParams.ToDate; },
        set: function (value) {
            if (this.RequestParams.ToDate != value) {
                this.RequestParams.ToDate = value;
                if (Tools_1.AppTool.IsNullOrEmpty(this.RequestParams.ToDate)) {
                    this.UIProperties.SetRequired("ToDate", this.ObjectTableName, true);
                }
                else {
                    this.UIProperties.SetRequired("ToDate", this.ObjectTableName, false);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BlockListInWarehouseComponent.prototype, "StorageSiteNumber", {
        get: function () { return this.RequestParams ? this.RequestParams.StorageSiteNumber : null; },
        set: function (value) {
            if (this.RequestParams.StorageSiteNumber != value) {
                this.RequestParams.StorageSiteNumber = value;
                if (Tools_1.AppTool.IsNullOrEmpty(this.RequestParams.StorageSiteNumber)) {
                    this.UIProperties.SetRequired("StorageSiteNumber", this.ObjectTableName, true);
                }
                else {
                    this.UIProperties.SetRequired("StorageSiteNumber", this.ObjectTableName, false);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    BlockListInWarehouseComponent.prototype.ResetBlockListChangeSelected = function (enumvalue) {
        this._ShowResetBlocks = enumvalue;
    };
    //#region General Commands
    BlockListInWarehouseComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    BlockListInWarehouseComponent.prototype.FillErrors = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.FromDate == null) {
            this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsBlockListInWarehouse.O.FromDateMandatory"));
        }
        if (this.ToDate == null) {
            this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsBlockListInWarehouse.O.ToDateMandatory"));
        }
        if (this.StorageSiteNumber == null) {
            this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsBlockListInWarehouse.O.StorageSiteNumberMandatory"));
        }
    };
    BlockListInWarehouseComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        var _this = this;
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        var currRequestParams = new BlockListInWarehouseRequestParams_1.BlockListInWarehouseRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.FromDate = this.FromDate;
        currRequestParams.ToDate = this.ToDate;
        currRequestParams.StorageSiteNumber = this.StorageSiteNumber;
        currRequestParams.ShowResetBlocks = this._ShowResetBlocks;
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "שליחת שאילתא לגושים במחסן", true)
            .then(function (res) {
            _this.ResponseData = res;
            _this.OnMassageDisplayMethod();
        }).catch(function (err) {
            _this.ValidationErrorsList.push(err);
        });
        this._IIGGeneralMessagesService.PostBlockListInWarehouseRequestParams(currRequestParams)
            .subscribe(function (myServiceResponse) {
        });
    };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent)
    ], BlockListInWarehouseComponent.prototype, "SuperCustomMessageWrapperComponent", void 0);
    BlockListInWarehouseComponent = __decorate([
        core_1.Component({
            selector: 'BlockListInWarehouseComponent',
            moduleId: module.id,
            templateUrl: './BlockListInWarehouseComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], BlockListInWarehouseComponent);
    return BlockListInWarehouseComponent;
}(BaseRequestsSheetMassaging_1.BaseRequestsSheetMassaging));
exports.BlockListInWarehouseComponent = BlockListInWarehouseComponent;
//# sourceMappingURL=BlockListInWarehouseComponent.js.map