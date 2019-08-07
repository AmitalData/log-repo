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
var ImporterDeclarationRequestParams_1 = require("../../../Customs/DataContract/RequestParams/ImporterDeclarationRequestParams");
var DeclarationExtendedListService_1 = require("../../../Customs/Services/ExtendedLists/DeclarationExtendedListService");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../Infrastructure/Tools");
var BaseRequestsSheetMassaging_1 = require("../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging");
var CustomsSettingListService_1 = require("../../../Customs/Services/StandardLists/CustomsSettingListService");
var PartnersDomainService_1 = require("../../../Common/Services/PartnersDomainService");
var ObservableCollection_1 = require("../../../Infrastructure/Utilities/ObservableCollection");
var CustomMessageProgressComponent_1 = require("../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var ImporterDeclarationComponent = /** @class */ (function (_super) {
    __extends(ImporterDeclarationComponent, _super);
    function ImporterDeclarationComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.Declaration";
        _this._IsImporerCodeEnabled = false;
        _this._CodeVisibility = true;
        _this._IIGGeneralMessagesService = new IIGGeneralMessagesService_1.IIGGeneralMessagesService();
        _this._DeclarationExtendedListService = new DeclarationExtendedListService_1.DeclarationExtendedListService();
        _this._CustomsSettingListService = new CustomsSettingListService_1.CustomsSettingListService();
        _this._PartnersDomainService = new PartnersDomainService_1.PartnersDomainService();
        _this.DeclarationConectFilterList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SuperCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        _this.PeriodDeclarationList = new ObservableCollection_1.ObservableCollection([]);
        _this.LoiDeclarationList = new ObservableCollection_1.ObservableCollection([]);
        _this.SecurityDeclarationList = new ObservableCollection_1.ObservableCollection([]);
        _this.BuildEmployeeGroupFilterList();
        return _this;
    }
    ImporterDeclarationComponent.prototype.ngAfterViewInit = function () {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();
    };
    ImporterDeclarationComponent.prototype.OnMassageDisplayMethod = function () {
        var _this = this;
        if (this.RequestParams == null) {
            this.RequestParams = new ImporterDeclarationRequestParams_1.ImporterDeclarationRequestParams();
            this.SetIsByExpireDate(true);
            this.UIProperties.SetRequired("ImporterCode", null, true);
            this.UIProperties.SetRequired("DeclarationExpire", null, true);
            this.UIProperties.SetRequired("DeclarationConect", null, true);
            this.UIProperties.SetRequired("Code", null, true);
            this.UIProperties.SetRequired("FromDate", null, true);
            this.UIProperties.SetRequired("ToDate", null, true);
        }
        else if (this.RequestParams != null && this.RequestParams.IsByType) {
            this.SelectedDeclarationConectFilter = this.DeclarationConectFilterList.filter(function (d) { return d.Code == _this.RequestParams.DeclarationConect; })[0];
            if (this.RequestParams.DeclarationConect == "2") {
                this.CodeVisibility = false;
            }
        }
        if (this.ResponseData) {
            if (this.ResponseData.PeriodDeclarationList) {
                this.PeriodDeclarationList.InsertCollection(this.ResponseData.PeriodDeclarationList);
            }
            if (this.ResponseData.LoiDeclarationList) {
                this.LoiDeclarationList.InsertCollection(this.ResponseData.LoiDeclarationList);
            }
            if (this.ResponseData.SecurityDeclarationList) {
                this.SecurityDeclarationList.InsertCollection(this.ResponseData.SecurityDeclarationList);
            }
        }
    };
    Object.defineProperty(ImporterDeclarationComponent.prototype, "DeclarationExpire", {
        //#region Properties
        get: function () { return this.RequestParams.DeclarationExpire; },
        set: function (value) {
            if (this.RequestParams.DeclarationExpire != value) {
                this.RequestParams.DeclarationExpire = value;
            }
            if (value) {
                this.UIProperties.SetRequired("DeclarationExpire", null, false);
            }
            else {
                this.UIProperties.SetRequired("DeclarationExpire", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    ImporterDeclarationComponent.prototype.SetIsByExpireDate = function (newValue) {
        this.IsByExpireDate = newValue;
    };
    Object.defineProperty(ImporterDeclarationComponent.prototype, "IsByExpireDate", {
        get: function () { return this.RequestParams.IsByExpireDate; },
        set: function (newValue) {
            if (this.RequestParams.IsByExpireDate != newValue) {
                this.RequestParams.IsByExpireDate = newValue;
                if (newValue == true) {
                    this.SetIsByType(false);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    ImporterDeclarationComponent.prototype.SetIsByType = function (newValue) {
        this.IsByType = newValue;
    };
    Object.defineProperty(ImporterDeclarationComponent.prototype, "IsByType", {
        get: function () { return this.RequestParams.IsByType; },
        set: function (newValue) {
            if (this.RequestParams.IsByType != newValue) {
                this.RequestParams.IsByType = newValue;
                if (newValue == true) {
                    this.SetIsByExpireDate(false);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDeclarationComponent.prototype, "CodeVisibility", {
        get: function () { return this._CodeVisibility; },
        set: function (newValue) {
            if (this._CodeVisibility != newValue) {
                this._CodeVisibility = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDeclarationComponent.prototype, "DeclarationConect", {
        get: function () { return this.RequestParams.DeclarationConect; },
        set: function (value) {
            if (this.RequestParams.DeclarationConect != value) {
                this.RequestParams.DeclarationConect = value;
            }
            this.UIProperties.SetEnabled("Code", null, true);
            this.UIProperties.SetRequired("Code", null, true);
            if (value) {
                this.UIProperties.SetRequired("DeclarationConect", null, false);
                if (value == "0") {
                    this.UIProperties.SetEnabled("Code", null, false);
                    this.UIProperties.SetRequired("Code", null, false);
                }
            }
            else {
                this.UIProperties.SetRequired("DeclarationConect", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDeclarationComponent.prototype, "Code", {
        get: function () { return this.RequestParams.Code; },
        set: function (value) {
            if (this.RequestParams.Code != value) {
                this.RequestParams.Code = value;
            }
            if (value) {
                this.UIProperties.SetRequired("Code", null, false);
            }
            else {
                this.UIProperties.SetRequired("Code", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDeclarationComponent.prototype, "FromDate", {
        get: function () { return this.RequestParams.FromDate; },
        set: function (value) {
            if (this.RequestParams.FromDate != value) {
                this.RequestParams.FromDate = value;
            }
            if (value) {
                this.UIProperties.SetRequired("FromDate", null, false);
            }
            else {
                this.UIProperties.SetRequired("FromDate", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDeclarationComponent.prototype, "ToDate", {
        get: function () { return this.RequestParams.ToDate; },
        set: function (value) {
            if (this.RequestParams.ToDate != value) {
                this.RequestParams.ToDate = value;
            }
            if (value) {
                this.UIProperties.SetRequired("ToDate", null, false);
            }
            else {
                this.UIProperties.SetRequired("ToDate", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDeclarationComponent.prototype, "ImporterCode", {
        get: function () { return this.RequestParams.ImporterNumber; },
        set: function (value) {
            if (this.RequestParams.ImporterNumber != value) {
                this.RequestParams.ImporterNumber = value;
            }
            if (value) {
                this.UIProperties.SetRequired("ImporterCode", null, false);
            }
            else {
                this.UIProperties.SetRequired("ImporterCode", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDeclarationComponent.prototype, "ImporterName", {
        get: function () { return this._ImporterName; },
        set: function (value) {
            if (this._ImporterName != value) {
                this._ImporterName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    //#region EmployeeGroup
    ImporterDeclarationComponent.prototype.BuildEmployeeGroupFilterList = function () {
        this.DeclarationConectFilterList = [];
        var myRecordsItem = new CodeNameClass();
        myRecordsItem.Code = "0"; // "ALL"
        myRecordsItem.Name = "הכל";
        this.DeclarationConectFilterList.push(myRecordsItem);
        var allRecordsItem = new CodeNameClass();
        allRecordsItem.Code = "1"; // "ImportDeclaration"
        allRecordsItem.Name = "הצהרת יבוא";
        this.DeclarationConectFilterList.push(allRecordsItem);
        var myRecordsItem = new CodeNameClass();
        myRecordsItem.Code = "2"; // "Vendor"
        myRecordsItem.Name = "ספק";
        this.DeclarationConectFilterList.push(myRecordsItem);
        var allRecordsItem = new CodeNameClass();
        allRecordsItem.Code = "3"; // "Declaration"
        allRecordsItem.Name = "הצהרה";
        this.DeclarationConectFilterList.push(allRecordsItem);
    };
    Object.defineProperty(ImporterDeclarationComponent.prototype, "SelectedDeclarationConectFilter", {
        get: function () {
            return this.selectedDeclarationConectFilter;
        },
        set: function (newValue) {
            if (this.selectedDeclarationConectFilter != newValue) {
                this.selectedDeclarationConectFilter = newValue;
                this.DeclarationConect = newValue.Code;
            }
            if (newValue.Code == "0") {
                this.UIProperties.SetEnabled("Code", null, false);
                this.UIProperties.SetRequired("Code", null, false);
            }
            else {
                this.UIProperties.SetEnabled("Code", null, true);
                this.UIProperties.SetRequired("Code", null, true);
            }
            if (newValue.Code == "2") {
                this.CodeVisibility = false;
            }
            else {
                this.CodeVisibility = true;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    //#region Importer Commands
    ImporterDeclarationComponent.prototype.ImporterClicked = function (type, client) {
        this.ImporterCode = client.Code;
        this.ImporterName = Tools_1.AppTool.IsNullOrEmpty(client) ? "" : client.FullName;
    };
    ImporterDeclarationComponent.prototype.ImporterTextChanged = function (type, item) {
        this.ImporterName = "";
    };
    //#endregion
    //#region General Commands
    ImporterDeclarationComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    ImporterDeclarationComponent.prototype.FillErrors = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (Tools_1.AppTool.IsNullOrEmpty(this.ImporterCode)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.ImporterDeclarationQuery.O.ImporterNumberMandatory");
            this.ValidationErrorsList.push(msg);
        }
        if (this.IsByExpireDate) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.RequestParams.DeclarationExpire)) {
                var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.ImporterDeclarationQuery.O.DeclarationExpireMandatory");
                this.ValidationErrorsList.push(msg);
            }
        }
        if (this.IsByType) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.SelectedDeclarationConectFilter.Code)) {
                var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.ImporterDeclarationQuery.O.DeclarationConectMandatory");
                this.ValidationErrorsList.push(msg);
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.Code) && this.SelectedDeclarationConectFilter.Code != "0") {
                var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.ImporterDeclarationQuery.O.DeclarationNumberMandatory");
                this.ValidationErrorsList.push(msg);
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.FromDate)) {
                var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.ImporterDeclarationQuery.O.FromDateMandatory");
                this.ValidationErrorsList.push(msg);
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.ToDate)) {
                var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.ImporterDeclarationQuery.O.ToDateMandatory");
                this.ValidationErrorsList.push(msg);
            }
        }
    };
    ImporterDeclarationComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        var _this = this;
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this.PeriodDeclarationList.Clear();
        this.LoiDeclarationList.Clear();
        this.SecurityDeclarationList.Clear();
        var currRequestParams = new ImporterDeclarationRequestParams_1.ImporterDeclarationRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.ImporterNumber = this.ImporterCode;
        currRequestParams.IsByExpireDate = this.IsByExpireDate;
        currRequestParams.IsByType = this.IsByType;
        if (this.IsByExpireDate) {
            currRequestParams.DeclarationExpire = this.DeclarationExpire;
        }
        else if (this.IsByType) {
            currRequestParams.DeclarationConect = this.SelectedDeclarationConectFilter.Code;
            currRequestParams.Code = this.Code;
            currRequestParams.FromDate = this.FromDate;
            currRequestParams.ToDate = this.ToDate;
        }
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "שליחת שאילתא לתצהיר יבואן", true)
            .then(function (res) {
            _this.ResponseData = res;
            _this.OnMassageDisplayMethod();
        }).catch(function (err) {
            _this.ValidationErrorsList.push(err);
        });
        this._IIGGeneralMessagesService.PostImporterDeclarationRequest(currRequestParams)
            .subscribe(function (myServiceResponse) {
        });
    };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent)
    ], ImporterDeclarationComponent.prototype, "SuperCustomMessageWrapperComponent", void 0);
    ImporterDeclarationComponent = __decorate([
        core_1.Component({
            selector: 'ImporterDeclarationComponent',
            moduleId: module.id,
            templateUrl: './ImporterDeclarationComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ImporterDeclarationComponent);
    return ImporterDeclarationComponent;
}(BaseRequestsSheetMassaging_1.BaseRequestsSheetMassaging));
exports.ImporterDeclarationComponent = ImporterDeclarationComponent;
var CodeNameClass = /** @class */ (function () {
    function CodeNameClass() {
    }
    return CodeNameClass;
}());
//# sourceMappingURL=ImporterDeclarationComponent.js.map