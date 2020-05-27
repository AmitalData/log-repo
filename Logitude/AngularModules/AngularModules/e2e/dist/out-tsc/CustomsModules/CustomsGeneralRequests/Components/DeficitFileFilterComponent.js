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
var CustomMessageWrapperComponent_1 = require("../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var DeclarationExtendedListService_1 = require("../../../Customs/Services/ExtendedLists/DeclarationExtendedListService");
var IIGGeneralMessagesService_1 = require("../../../Customs/Services/WebServices/IIGGeneralMessagesService");
var DeficitFileFilterRequestParams_1 = require("../../../Customs/DataContract/RequestParams/DeficitFileFilterRequestParams");
var DeficitFilesDetailResponseData_1 = require("../../../Customs/DataContract/ResponseData/DeficitFilesDetailResponseData");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../Infrastructure/Tools");
var BaseRequestsSheetMassaging_1 = require("../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging");
var CustomMessageProgressComponent_1 = require("../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var ObservableCollection_1 = require("../../../Infrastructure/Utilities/ObservableCollection");
var DeficitFileFilterComponent = /** @class */ (function (_super) {
    __extends(DeficitFileFilterComponent, _super);
    function DeficitFileFilterComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.Declaration";
        _this._DeclarationExtendedListService = new DeclarationExtendedListService_1.DeclarationExtendedListService();
        _this._IIGGeneralMessagesService = new IIGGeneralMessagesService_1.IIGGeneralMessagesService();
        _this.OpenFilesList = new ObservableCollection_1.ObservableCollection([]);
        _this.CloseFileList = new ObservableCollection_1.ObservableCollection([]);
        _this.PaymentOrderList = new ObservableCollection_1.ObservableCollection([]);
        _this.RequireDocumentsList = new ObservableCollection_1.ObservableCollection([]);
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SuperCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        _this.FooterMethods = 0;
        _this.AmountSumTotal = 0;
        return _this;
    }
    DeficitFileFilterComponent.prototype.ngAfterViewInit = function () {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();
    };
    Object.defineProperty(DeficitFileFilterComponent.prototype, "StatusName", {
        get: function () {
            return this.ResponseData.StatusName;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeficitFileFilterComponent.prototype, "ExternalName", {
        get: function () {
            return this.ResponseData.ExternalName;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeficitFileFilterComponent.prototype, "CustomOfficeName", {
        get: function () {
            return this.ResponseData.CustomOfficeName;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeficitFileFilterComponent.prototype, "AgentName", {
        get: function () {
            return this.ResponseData.AgentName;
        },
        enumerable: true,
        configurable: true
    });
    DeficitFileFilterComponent.prototype.OnMassageDisplayMethod = function () {
        var _this = this;
        if (this.RequestParams == null) {
            this.RequestParams = new DeficitFileFilterRequestParams_1.DeficitFileFilterRequestParams();
        }
        if (this.ResponseData) {
            ////
        }
        else {
            var myDeficitFilesDetailResponseData = new DeficitFilesDetailResponseData_1.DeficitFilesDetailResponseData();
            this.ResponseData = myDeficitFilesDetailResponseData;
        }
        setTimeout(function () {
            if (_this.ResponseData.OpenFilesList) {
                _this.OpenFilesList.InsertCollection(_this.ResponseData.OpenFilesList);
            }
            if (_this.ResponseData.CloseFileList) {
                _this.CloseFileList.InsertCollection(_this.ResponseData.CloseFileList);
            }
            if (_this.ResponseData.PaymentOrderList) {
                _this.AmountSumTotal = 0;
                _this.PaymentOrderList.InsertCollection(_this.ResponseData.PaymentOrderList);
                _this.PaymentOrderList.Collection.forEach(function (p) {
                    _this.AmountSumTotal = _this.AmountSumTotal + Number(p.AmountSum);
                });
                var headerH = 27;
                var rowH = 26;
                var top_1 = headerH + (_this.PaymentOrderList.Length * 26) + 2;
                _this.FooterMethods = top_1;
            }
            if (_this.ResponseData.RequireDocumentsList) {
                _this.RequireDocumentsList.InsertCollection(_this.ResponseData.RequireDocumentsList);
            }
        }, 200);
    };
    DeficitFileFilterComponent.prototype.BuildDummy = function () {
        var myDeficitFilesDetailResponseData = new DeficitFilesDetailResponseData_1.DeficitFilesDetailResponseData();
        this.ResponseData = myDeficitFilesDetailResponseData;
        this.ResponseData.FileStatus = "FileStatus";
        this.ResponseData.StatusName = "StatusName ";
        this.ResponseData.ExternalID = "ExternalID ";
        this.ResponseData.ExternalName = "ExternalName ";
        this.ResponseData.CustomOfficeNumber = "CustomOfficeNumber ";
        this.ResponseData.CustomOfficeName = "CustomOfficeName ";
        this.ResponseData.FilingNumber = "FilingNumber ";
        this.ResponseData.OpenFileCounter = "OpenFileCounter ";
        this.ResponseData.CloseFileCounter = "CloseFileCounter ";
        this.ResponseData.AgentExternalID = "AgentExternalID";
        this.ResponseData.AgentName = "AgentName";
        var myExternalFilesDetailsResult;
        myExternalFilesDetailsResult = {
            "ExternalID": "ExternalID",
            "ExternalName": "ExternalName",
            "FileNumber": "FileNumber",
            "Numeral": "Numeral",
            "DisplayFileNumber": "DisplayFileNumber",
            "DeficitEntityType": "DeficitEntityType",
            "EntityTypeName": "EntityTypeName",
            "DeficitEntityID": "DeficitEntityID",
            "ProductionDate": "ProductionDate",
            "UnpaidBalance": "UnpaidBalance",
            "EstimatedBalance": "EstimatedBalance",
            "EstimatedDate": "EstimatedDate",
            "Status": "Status",
            "StatusName": "StatusName",
            "TotalComponentAmount": "TotalComponentAmount",
            "TotalRefundAmount": "TotalRefundAmount",
            "CloseDate": "CloseDate",
            "SecondaryStatus": "SecondaryStatus",
        };
        myDeficitFilesDetailResponseData.OpenFilesList = [];
        myDeficitFilesDetailResponseData.OpenFilesList.push(myExternalFilesDetailsResult);
        myDeficitFilesDetailResponseData.OpenFilesList.push(myExternalFilesDetailsResult);
        myDeficitFilesDetailResponseData.CloseFileList = [];
        myDeficitFilesDetailResponseData.CloseFileList.push(myExternalFilesDetailsResult);
        myDeficitFilesDetailResponseData.CloseFileList.push(myExternalFilesDetailsResult);
        var myExternalPaymentOrderResult;
        myExternalPaymentOrderResult = {
            "ExternalID": "ExternalID",
            "ExternalName": "ExternalName",
            "PaymentID": "PaymentID",
            "PaymentProcessType": "PaymentProcessType",
            "PaymentProcessName": "PaymentProcessName",
            "AmountSum": 100,
            "ValidityDateTo": "ValidityDateTo",
            "CreateDate": "CreateDate",
            "PaymentOrderPayDate": "PaymentOrderPayDate",
            "PaymentOrderStatus": "PaymentOrderStatus",
            "PaymentOrderStatusName": "PaymentOrderStatusName"
        };
        myDeficitFilesDetailResponseData.PaymentOrderList = [];
        myDeficitFilesDetailResponseData.PaymentOrderList.push(myExternalPaymentOrderResult);
        myDeficitFilesDetailResponseData.PaymentOrderList.push(myExternalPaymentOrderResult);
        myDeficitFilesDetailResponseData.RequireDocumentsList = [];
        var myRequireDocumentsResult = {
            "DocumentCode": "DocumentCode",
            "DocumentTypeName": "DocumentTypeName",
            "FileNumber": "FileNumber",
            "Numeral": "Numeral",
            "DisplayFileNumber": "DisplayFileNumber",
            "DocumentID": "DocumentID"
        };
        myDeficitFilesDetailResponseData.RequireDocumentsList.push(myRequireDocumentsResult);
        myDeficitFilesDetailResponseData.RequireDocumentsList.push(myRequireDocumentsResult);
    };
    Object.defineProperty(DeficitFileFilterComponent.prototype, "FileNumber", {
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
    Object.defineProperty(DeficitFileFilterComponent.prototype, "Numeral", {
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
    //#endregion Properties
    //#endregion Response Properties
    //#endregion Response Properties
    //#region Declaration Commands
    //#endregion
    //#region General Commands
    DeficitFileFilterComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    DeficitFileFilterComponent.prototype.FillErrors = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (Tools_1.AppTool.IsNullOrEmpty(this.FileNumber)) {
            this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.DeficitFileFilterQuery.O.FileNumberMandatory"));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.Numeral)) {
            this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.DeficitFileFilterQuery.O.NumeralMandatory"));
        }
    };
    DeficitFileFilterComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        var _this = this;
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        var currRequestParams = new DeficitFileFilterRequestParams_1.DeficitFileFilterRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.FileNumber = this.FileNumber;
        currRequestParams.Numeral = this.Numeral;
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "שליחת שאילתא לגרעונות", true)
            .then(function (res) {
        }).catch(function (err) {
            _this.ValidationErrorsList.push(err);
        });
        this._IIGGeneralMessagesService.PostDeficitFileFilterRequestParams(currRequestParams)
            .subscribe(function (myServiceResponse) {
            _this.ResponseData = myServiceResponse.Result;
            _this.OnMassageDisplayMethod();
        });
    };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent)
    ], DeficitFileFilterComponent.prototype, "SuperCustomMessageWrapperComponent", void 0);
    DeficitFileFilterComponent = __decorate([
        core_1.Component({
            selector: 'DeficitFileFilterComponent',
            moduleId: module.id,
            templateUrl: './DeficitFileFilterComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], DeficitFileFilterComponent);
    return DeficitFileFilterComponent;
}(BaseRequestsSheetMassaging_1.BaseRequestsSheetMassaging));
exports.DeficitFileFilterComponent = DeficitFileFilterComponent;
//# sourceMappingURL=DeficitFileFilterComponent.js.map