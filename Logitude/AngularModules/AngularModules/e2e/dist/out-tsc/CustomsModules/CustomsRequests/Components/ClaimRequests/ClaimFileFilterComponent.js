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
var TPG_NG_8244_ClaimFileFilterRequestParams_1 = require("../../../../Customs/DataContract/RequestParams/TPG_NG_8244_ClaimFileFilterRequestParams");
var TPG_NG_8245_ClaimFilesDetailResponseData_1 = require("../../../../Customs/DataContract/ResponseData/TPG_NG_8245_ClaimFilesDetailResponseData");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var BaseRequestsSheetMassaging_1 = require("../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging");
var CustomMessageProgressComponent_1 = require("../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var ClaimFileFilterComponent = /** @class */ (function (_super) {
    __extends(ClaimFileFilterComponent, _super);
    function ClaimFileFilterComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.Declaration";
        _this._DeclarationExtendedListService = new DeclarationExtendedListService_1.DeclarationExtendedListService();
        _this._IIGGeneralMessagesService = new IIGGeneralMessagesService_1.IIGGeneralMessagesService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SuperCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        _this.OpenFilesCollapsList = new ObservableCollection_1.ObservableCollection([]);
        _this.CloseFilesCollapsList = new ObservableCollection_1.ObservableCollection([]);
        _this.RefundOrderList = new ObservableCollection_1.ObservableCollection([]);
        _this.RequireDocumentsList = new ObservableCollection_1.ObservableCollection([]);
        _this.UIProperties.SetRequired("FileNumber", _this.ObjectTableName, true);
        _this.UIProperties.SetRequired("Numeral", _this.ObjectTableName, true);
        return _this;
    }
    ClaimFileFilterComponent.prototype.ngAfterViewInit = function () {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();
    };
    ClaimFileFilterComponent.prototype.OnMassageDisplayMethod = function () {
        if (this.RequestParams == null) {
            this.RequestParams = new TPG_NG_8244_ClaimFileFilterRequestParams_1.TPG_NG_8244_ClaimFileFilterRequestParams();
        }
        if (this.ResponseData) {
            if (this.ResponseData.OpenFilesCollapsList) {
                this.OpenFilesCollapsList.InsertCollection(this.ResponseData.OpenFilesCollapsList);
            }
            if (this.ResponseData.CloseFilesCollapsList) {
                this.CloseFilesCollapsList.InsertCollection(this.ResponseData.CloseFilesCollapsList);
            }
            if (this.ResponseData.RefundOrderList) {
                this.RefundOrderList.InsertCollection(this.ResponseData.RefundOrderList);
            }
            if (this.ResponseData.RequireDocumentsList) {
                this.RequireDocumentsList.InsertCollection(this.ResponseData.RequireDocumentsList);
            }
        }
        else {
            this.ResponseData = new TPG_NG_8245_ClaimFilesDetailResponseData_1.TPG_NG_8245_ClaimFilesDetailResponseData();
        }
        this.ResponseData.GeneralDataDetails = this.ResponseData.GeneralDataDetails || {};
        this.ResponseData.GeneralDetailsData = this.ResponseData.GeneralDetailsData || {};
    };
    Object.defineProperty(ClaimFileFilterComponent.prototype, "FileNumber", {
        //#region Properties
        get: function () { return this.RequestParams.FileNumber; },
        set: function (value) {
            if (this.RequestParams.FileNumber != value) {
                this.RequestParams.FileNumber = value;
                if (Tools_1.AppTool.IsNullOrEmpty(this.RequestParams.FileNumber)) {
                    this.UIProperties.SetRequired("FileNumber", this.ObjectTableName, true);
                }
                else {
                    this.UIProperties.SetRequired("FileNumber", this.ObjectTableName, false);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimFileFilterComponent.prototype, "Numeral", {
        get: function () { return this.RequestParams.Numeral; },
        set: function (value) {
            if (this.RequestParams.Numeral != value) {
                this.RequestParams.Numeral = value;
                if (Tools_1.AppTool.IsNullOrEmpty(this.RequestParams.Numeral)) {
                    this.UIProperties.SetRequired("Numeral", this.ObjectTableName, true);
                }
                else {
                    this.UIProperties.SetRequired("Numeral", this.ObjectTableName, false);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimFileFilterComponent.prototype, "ExternalId", {
        get: function () { return this.ResponseData.GeneralDataDetails.ExternalID; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimFileFilterComponent.prototype, "CustomOfficeName", {
        get: function () { return this.ResponseData.GeneralDataDetails.CustomOfficeName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimFileFilterComponent.prototype, "OpenFileCounter", {
        get: function () { return this.ResponseData.GeneralDataDetails.OpenFileCounter; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimFileFilterComponent.prototype, "ExternalName", {
        get: function () { return this.ResponseData.GeneralDataDetails.ExternalName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimFileFilterComponent.prototype, "AgentName", {
        get: function () { return this.ResponseData.GeneralDataDetails.AgentName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimFileFilterComponent.prototype, "CloseFileCounter", {
        get: function () { return this.ResponseData.GeneralDataDetails.CloseFileCounter; },
        enumerable: true,
        configurable: true
    });
    //#endregion Properties
    //#endregion Response Properties
    //#endregion Response Properties
    //#region Declaration Commands
    //#endregion
    //#region General Commands
    ClaimFileFilterComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    ClaimFileFilterComponent.prototype.FillErrors = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.FileNumber == null) {
            this.ValidationErrorsList.push("");
            return;
        }
        if (this.Numeral == null) {
            this.ValidationErrorsList.push("");
            return;
        }
    };
    ClaimFileFilterComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        var _this = this;
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        var currRequestParams = new TPG_NG_8244_ClaimFileFilterRequestParams_1.TPG_NG_8244_ClaimFileFilterRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.FileNumber = this.FileNumber;
        currRequestParams.Numeral = this.Numeral;
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "שליחת שאילתא לתביעות", true)
            .then(function (res) {
            _this.ResponseData = res;
            _this.OnMassageDisplayMethod();
        }).catch(function (err) {
            _this.ValidationErrorsList.push(err);
        });
        this._IIGGeneralMessagesService.PostTPG_NG_8244_ClaimFileFilterRequestParams(currRequestParams)
            .subscribe(function (myServiceResponse) {
        });
    };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent)
    ], ClaimFileFilterComponent.prototype, "SuperCustomMessageWrapperComponent", void 0);
    ClaimFileFilterComponent = __decorate([
        core_1.Component({
            selector: 'ClaimFileFilterComponent',
            moduleId: module.id,
            templateUrl: './ClaimFileFilterComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ClaimFileFilterComponent);
    return ClaimFileFilterComponent;
}(BaseRequestsSheetMassaging_1.BaseRequestsSheetMassaging));
exports.ClaimFileFilterComponent = ClaimFileFilterComponent;
//# sourceMappingURL=ClaimFileFilterComponent.js.map