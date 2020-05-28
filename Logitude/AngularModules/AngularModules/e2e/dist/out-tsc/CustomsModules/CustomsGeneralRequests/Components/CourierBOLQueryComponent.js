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
var IIGGeneralMessagesService_1 = require("../../../Customs/Services/WebServices/IIGGeneralMessagesService");
var CourierBOLQueryRequestParams_1 = require("../../../Customs/DataContract/RequestParams/CourierBOLQueryRequestParams");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../Infrastructure/Tools");
var BaseRequestsSheetMassaging_1 = require("../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging");
var ObservableCollection_1 = require("../../../Infrastructure/Utilities/ObservableCollection");
var CustomMessageProgressComponent_1 = require("../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var RequestParamsBase_1 = require("../../../Customs/DataContract/RequestParams/RequestParamsBase");
var DeclarationExtendedListService_1 = require("../../../Customs/Services/ExtendedLists/DeclarationExtendedListService");
var CourierBOLQueryComponent = /** @class */ (function (_super) {
    __extends(CourierBOLQueryComponent, _super);
    function CourierBOLQueryComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.Declaration";
        _this._IIGGeneralMessagesService = new IIGGeneralMessagesService_1.IIGGeneralMessagesService();
        _this._DeclarationExtendedListService = new DeclarationExtendedListService_1.DeclarationExtendedListService();
        _this._DeclarationId = "";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SuperCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        _this._IsFromDeclaration = false;
        _this.CourierBOLDetailsObservableList = new ObservableCollection_1.ObservableCollection([]);
        return _this;
    }
    CourierBOLQueryComponent.prototype.ngAfterViewInit = function () {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();
    };
    CourierBOLQueryComponent.prototype.OnMassageDisplayMethod = function () {
        if (this.RequestParams == null) {
            this.RequestParams = new CourierBOLQueryRequestParams_1.CourierBOLQueryRequestParams();
            this.UIProperties.SetRequired("CourierBOL", null, true);
            this.UIProperties.SetRequired("CourierVAT", null, true);
        }
        if (this.ResponseData) {
            if (this.ResponseData.CourierBOLDetailsList) {
                this.CourierBOLDetailsObservableList.InsertCollection(this.ResponseData.CourierBOLDetailsList);
            }
        }
    };
    CourierBOLQueryComponent.prototype.SetMenuArg = function (MenuArg) {
        this.OnMassageDisplayMethod();
        this.IsFromDeclaration = true;
        this.CustomFileNo = MenuArg.CustomFileNo;
        this.CourierBOL = MenuArg.CourierBOL;
        this.CourierVAT = MenuArg.CourierVAT;
        this._DeclarationId = MenuArg.DeclarationId;
        this.UIProperties.SetEnabled("CourierBOL", null, false);
        this.UIProperties.SetEnabled("CourierVAT", null, false);
        var customSendOptionsArgs = new RequestParamsBase_1.CustomSendOptionsArgs();
        customSendOptionsArgs.ForcePersonalSign = false;
        customSendOptionsArgs.RequestVIA = RequestParamsBase_1.SendRequestVIA.WebServiceInteractive;
        this.OnCustomSendOptionsButtonClick(customSendOptionsArgs);
    };
    CourierBOLQueryComponent.prototype.EditButtonClicked = function (item) {
        this.CurrentSession.CloseCurrentWindowEmit(item.cargoIdentifierKey3);
    };
    Object.defineProperty(CourierBOLQueryComponent.prototype, "CustomFileNo", {
        //#region Properties
        get: function () { return this.RequestParams ? this.RequestParams.CustomFileNo : null; },
        set: function (value) {
            if (this.RequestParams.CustomFileNo != value) {
                this.RequestParams.CustomFileNo = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CourierBOLQueryComponent.prototype, "CourierBOL", {
        get: function () { return this.RequestParams ? this.RequestParams.CourierBOL : null; },
        set: function (value) {
            if (this.RequestParams.CourierBOL != value) {
                this.RequestParams.CourierBOL = value;
            }
            if (value) {
                this.UIProperties.SetRequired("CourierBOL", null, false);
            }
            else {
                this.UIProperties.SetRequired("CourierBOL", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CourierBOLQueryComponent.prototype, "CourierVAT", {
        get: function () { return this.RequestParams ? this.RequestParams.CourierVAT : null; },
        set: function (value) {
            if (this.RequestParams.CourierVAT != value) {
                this.RequestParams.CourierVAT = value;
            }
            if (value) {
                this.UIProperties.SetRequired("CourierVAT", null, false);
            }
            else {
                this.UIProperties.SetRequired("CourierVAT", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CourierBOLQueryComponent.prototype, "IsFromDeclaration", {
        get: function () { return this._IsFromDeclaration; },
        set: function (value) {
            if (this._IsFromDeclaration != value) {
                this._IsFromDeclaration = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion Properties
    CourierBOLQueryComponent.prototype.CustomFileNoTextChanged = function (searchtext) {
        var _this = this;
        this.DueChangeClearChildField();
        if (Tools_1.AppTool.IsNullOrEmpty(this.CustomFileNo)) {
            return;
        }
        this.CurrentSession.StartBusyIndicator("");
        this._DeclarationExtendedListService.GetConsignmentListPMByCustomFileNo(this.CustomFileNo)
            .subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            _this.FetchDeclaration(myResponse, true);
        });
    };
    CourierBOLQueryComponent.prototype.DueChangeClearChildField = function () {
        this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
        this._DeclarationId = null;
        this.CourierBOL = "";
        this.CourierVAT = "";
        this.UIProperties.SetEnabled("CourierBOL", null, true);
        this.UIProperties.SetEnabled("CourierVAT", null, true);
        this.ValidationErrorsList = [];
    };
    CourierBOLQueryComponent.prototype.FetchDeclaration = function (myResponse, sourceIsCostomFile) {
        var lastFetchDeclarationList = myResponse.Result;
        if (lastFetchDeclarationList != null) {
            lastFetchDeclarationList = lastFetchDeclarationList[0];
            this._DeclarationId = lastFetchDeclarationList.DeclarationId;
            this.CourierBOL = lastFetchDeclarationList.ManifestNumber;
            this.CourierVAT = lastFetchDeclarationList.SecondCargoID;
            this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
            this.UIProperties.SetEnabled("CourierBOL", null, false);
            this.UIProperties.SetEnabled("CourierVAT", null, false);
        }
        else {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Didntfindcustomfile");
            this.ValidationErrorsList.push(msg);
            this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, false, msg);
        }
    };
    //#region Commands
    CourierBOLQueryComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    CourierBOLQueryComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (Tools_1.AppTool.IsNullOrEmpty(this.RequestParams.CourierBOL) || Tools_1.AppTool.IsNullOrEmpty(this.RequestParams.CourierVAT)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CourierBOLQuery.O.QueryDataMissing");
            this.ValidationErrorsList.push(msg);
        }
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        var currRequestParams = new CourierBOLQueryRequestParams_1.CourierBOLQueryRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.CourierBOL = this.RequestParams.CourierBOL;
        currRequestParams.CourierVAT = this.RequestParams.CourierVAT;
        currRequestParams.DeclarationId = this._DeclarationId;
        currRequestParams.CustomFileNo = this.CustomFileNo;
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "שליחת שאילתא לשטרי מטען בלדר", true)
            .then(function (res) {
            _this.ResponseData = res;
            _this.OnMassageDisplayMethod();
        }).catch(function (err) {
            _this.ValidationErrorsList.push(err);
        });
        this._IIGGeneralMessagesService.PostCourierBOLRequest(currRequestParams)
            .subscribe(function () { });
    };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent)
    ], CourierBOLQueryComponent.prototype, "SuperCustomMessageWrapperComponent", void 0);
    CourierBOLQueryComponent = __decorate([
        core_1.Component({
            selector: 'CourierBOLQueryComponent',
            moduleId: module.id,
            templateUrl: './CourierBOLQueryComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CourierBOLQueryComponent);
    return CourierBOLQueryComponent;
}(BaseRequestsSheetMassaging_1.BaseRequestsSheetMassaging));
exports.CourierBOLQueryComponent = CourierBOLQueryComponent;
//# sourceMappingURL=CourierBOLQueryComponent.js.map