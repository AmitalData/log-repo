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
var TapagMessagesService_1 = require("../../../../Customs/Services/WebServices/TapagMessagesService");
var FaultProceduralRequestParams_1 = require("../../../../Customs/DataContract/RequestParams/FaultProceduralRequestParams");
var DeclarationExtendedListService_1 = require("../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var BaseRequestsSheetMassaging_1 = require("../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging");
var CustomsSettingListService_1 = require("../../../../Customs/Services/StandardLists/CustomsSettingListService");
var PartnersDomainService_1 = require("../../../../Common/Services/PartnersDomainService");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var CustomMessageProgressComponent_1 = require("../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var FaultQueryComponent = /** @class */ (function (_super) {
    __extends(FaultQueryComponent, _super);
    function FaultQueryComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.Declaration";
        _this._IsImporerCodeEnabled = false;
        _this._TapagMessagesService = new TapagMessagesService_1.TapagMessagesService();
        _this._DeclarationExtendedListService = new DeclarationExtendedListService_1.DeclarationExtendedListService();
        _this._CustomsSettingListService = new CustomsSettingListService_1.CustomsSettingListService();
        _this._PartnersDomainService = new PartnersDomainService_1.PartnersDomainService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SuperCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        _this.FaultGeneralDetailList = new ObservableCollection_1.ObservableCollection([]);
        return _this;
    }
    FaultQueryComponent.prototype.ngAfterViewInit = function () {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();
    };
    FaultQueryComponent.prototype.OnMassageDisplayMethod = function () {
        var _this = this;
        if (this.RequestParams == null) {
            this.RequestParams = new FaultProceduralRequestParams_1.FaultProceduralRequestParams();
            this.UIProperties.SetRequired("StartDate", null, true);
            if (this._CustomsSettingListService == null) {
                this._CustomsSettingListService = new CustomsSettingListService_1.CustomsSettingListService();
            }
            this._CustomsSettingListService.getSingleFromCache(SessionLocator_1.SessionLocator.Tenant.toString())
                .subscribe(function (customsSettingList) {
                if (customsSettingList) {
                    _this.AgentExternalID = customsSettingList.Result ? customsSettingList.Result.CustomsAgentId : null;
                    _this.UIProperties.SetRequired("AgentExternalID", null, false);
                }
            });
        }
        if (this.ResponseData && this.ResponseData.FaultGeneralDetailList) {
            for (var _i = 0, _a = this.ResponseData.FaultGeneralDetailList; _i < _a.length; _i++) {
                var item = _a[_i];
                item.FaultAdittionalInformationListObs = new ObservableCollection_1.ObservableCollection(item.FaultAdittionalInformationList);
            }
            this.FaultGeneralDetailList.InsertCollection(this.ResponseData.FaultGeneralDetailList);
        }
    };
    FaultQueryComponent.prototype.OnRowLoaded = function (myRow) {
        if (myRow) {
            myRow.SetExpandaple(true);
        }
    };
    Object.defineProperty(FaultQueryComponent.prototype, "FaultCode", {
        //#region Properties
        get: function () { return this.RequestParams.ProceduralFaultCode; },
        set: function (value) {
            if (this.RequestParams.ProceduralFaultCode != value) {
                this.RequestParams.ProceduralFaultCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FaultQueryComponent.prototype, "AgentExternalID", {
        get: function () { return this.RequestParams.AgentExternalID; },
        set: function (value) {
            if (this.RequestParams.AgentExternalID != value) {
                this.RequestParams.AgentExternalID = value;
            }
            if (value) {
                this.UIProperties.SetEnabled("AgentExternalID", null, false);
            }
            else {
                this.UIProperties.SetEnabled("AgentExternalID", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FaultQueryComponent.prototype, "StartDate", {
        get: function () { return this.RequestParams.StartDate; },
        set: function (value) {
            if (this.RequestParams.StartDate != value) {
                this.RequestParams.StartDate = value;
            }
            if (value && !Tools_1.AppTool.IsNullOrEmpty(this.EndDate)) {
                this.UIProperties.SetRequired("StartDate", null, false);
            }
            else {
                this.UIProperties.SetRequired("StartDate", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FaultQueryComponent.prototype, "EndDate", {
        get: function () { return this.RequestParams.EndDate; },
        set: function (value) {
            if (this.RequestParams.EndDate != value) {
                this.RequestParams.EndDate = value;
            }
            if (value && !Tools_1.AppTool.IsNullOrEmpty(this.StartDate)) {
                this.UIProperties.SetRequired("StartDate", null, false);
            }
            else {
                this.UIProperties.SetRequired("StartDate", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FaultQueryComponent.prototype, "CustomFileNo", {
        get: function () { return this.RequestParams.CustomsFile; },
        set: function (value) {
            if (this.RequestParams.CustomsFile != value) {
                this.RequestParams.CustomsFile = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FaultQueryComponent.prototype, "DeclarationNumber", {
        get: function () { return this.RequestParams.DeclarationNumber; },
        set: function (value) {
            if (this.RequestParams.DeclarationNumber != value) {
                this.RequestParams.DeclarationNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FaultQueryComponent.prototype, "IsImporerCodeEnabled", {
        get: function () { return this._IsImporerCodeEnabled; },
        set: function (newValue) {
            if (this._IsImporerCodeEnabled != newValue) {
                this._IsImporerCodeEnabled = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FaultQueryComponent.prototype, "CustomerId", {
        get: function () { return this.RequestParams.CustomerId; },
        set: function (value) {
            var _this = this;
            if (this.RequestParams.CustomerId != value) {
                this.RequestParams.CustomerId = value;
            }
            if (value) {
                this._PartnersDomainService.GetCustomerById(value)
                    .subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        _this.ImporterCode = myResponse.Result.VatNumber;
                        _this.IsImporerCodeEnabled = true;
                    }
                });
            }
            else {
                this.ImporterCode = "";
                this.IsImporerCodeEnabled = false;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FaultQueryComponent.prototype, "ImporterCode", {
        get: function () { return this.RequestParams.ImporterExternalID; },
        set: function (value) {
            if (this.RequestParams.ImporterExternalID != value) {
                this.RequestParams.ImporterExternalID = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FaultQueryComponent.prototype, "ImporterName", {
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
    //#region Importer Commands
    FaultQueryComponent.prototype.ImporterClicked = function (type, client) {
        this.ImporterCode = client.Code;
        this.ImporterName = Tools_1.AppTool.IsNullOrEmpty(client) ? "" : client.FullName;
    };
    FaultQueryComponent.prototype.ImporterTextChanged = function (type, item) {
        if (item == "") {
            this.ImporterCode = null;
            this.ImporterName = "";
        }
    };
    //#endregion
    //#region Declaration Commands
    FaultQueryComponent.prototype.DueChangeClearChildField = function (sourceIsCostomFile) {
        this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
        this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, true, "");
        if (sourceIsCostomFile) {
            this.DeclarationNumber = "";
        }
        else {
            this.CustomFileNo = "";
        }
    };
    FaultQueryComponent.prototype.CustomFileNoTextChanged = function (searchtext) {
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
    FaultQueryComponent.prototype.DeclarationNumberTextChanged = function (DeclarationNumberText) {
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
    FaultQueryComponent.prototype.FetchDeclaration = function (myResponse, sourceIsCostomFile) {
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
            else {
                this.SetValidityDeclarationNumber();
            }
        }
    };
    FaultQueryComponent.prototype.SetValidityDeclarationNumber = function () {
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationNumberIsMandatory");
        this.ValidationErrorsList.push(msg);
        this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, false, msg);
    };
    FaultQueryComponent.prototype.SetValidityCustomFileNo = function () {
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Didntfindcustomfile");
        this.ValidationErrorsList.push(msg);
        this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, false, msg);
    };
    //#endregion
    //#region General Commands
    FaultQueryComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    FaultQueryComponent.prototype.FillErrors = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (Tools_1.AppTool.IsNullOrEmpty(this.RequestParams.StartDate)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.StartDateIsMandatory");
            this.ValidationErrorsList.push(msg);
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.RequestParams.EndDate)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EndDateIsMandatory");
            this.ValidationErrorsList.push(msg);
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.RequestParams.AgentExternalID)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.AgentExternalIDIsMandatory");
            this.ValidationErrorsList.push(msg);
        }
    };
    FaultQueryComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        var _this = this;
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this.FaultGeneralDetailList.Clear();
        var currRequestParams = new FaultProceduralRequestParams_1.FaultProceduralRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.CustomerId = this.CustomerId;
        currRequestParams.ImporterExternalID = this.ImporterCode;
        currRequestParams.CustomsFile = this.CustomFileNo;
        currRequestParams.DeclarationNumber = this.DeclarationNumber;
        currRequestParams.AgentExternalID = this.AgentExternalID;
        currRequestParams.ProceduralFaultCode = this.FaultCode;
        currRequestParams.StartDate = this.StartDate;
        currRequestParams.EndDate = this.EndDate;
        /*currRequestParams.Client = this.GuarantorID;
        currRequestParams.ImporterId = this.GuarantorID;
        currRequestParams.ImporterCode = this.GuarantorID;
        currRequestParams.ImporterName = this.GuarantorID;
*/
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "שליחת שאילתא ליקויים", true)
            .then(function (res) {
            _this.ResponseData = res;
            _this.OnMassageDisplayMethod();
        }).catch(function (err) {
            _this.ValidationErrorsList.push(err);
        });
        this._TapagMessagesService.PostFaultQueryRequest(currRequestParams)
            .subscribe(function (myServiceResponse) {
        });
    };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent)
    ], FaultQueryComponent.prototype, "SuperCustomMessageWrapperComponent", void 0);
    FaultQueryComponent = __decorate([
        core_1.Component({
            selector: 'FaultQueryComponent',
            moduleId: module.id,
            templateUrl: './FaultQueryComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], FaultQueryComponent);
    return FaultQueryComponent;
}(BaseRequestsSheetMassaging_1.BaseRequestsSheetMassaging));
exports.FaultQueryComponent = FaultQueryComponent;
//# sourceMappingURL=FaultQueryComponent.js.map