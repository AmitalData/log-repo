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
var GuaranteeFileFilterRequestParams_1 = require("../../../../Customs/DataContract/RequestParams/GuaranteeFileFilterRequestParams");
var GuaranteeFileFilterResponseData_1 = require("../../../../Customs/DataContract/ResponseData/GuaranteeFileFilterResponseData");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var BaseRequestsSheetMassaging_1 = require("../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging");
var CustomMessageProgressComponent_1 = require("../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var GuaranteeFileFilterQueryComponent = /** @class */ (function (_super) {
    __extends(GuaranteeFileFilterQueryComponent, _super);
    function GuaranteeFileFilterQueryComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.Declaration";
        _this._TapagMessagesService = new TapagMessagesService_1.TapagMessagesService();
        _this.GuranteeTypeFilterList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SuperCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        _this.selectedGuranteeTypeFilter = new CodeNameClass();
        _this.GuaranteeLettersList = new ObservableCollection_1.ObservableCollection([]);
        _this.CreditTransactionsList = new ObservableCollection_1.ObservableCollection([]);
        _this.RequireDocumentsList = new ObservableCollection_1.ObservableCollection([]);
        _this.BuildGuranteeTypeGroupFilterList();
        return _this;
    }
    GuaranteeFileFilterQueryComponent.prototype.ngAfterViewInit = function () {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();
    };
    GuaranteeFileFilterQueryComponent.prototype.OnMassageDisplayMethod = function () {
        if (this.RequestParams == null) {
            this.RequestParams = new GuaranteeFileFilterRequestParams_1.GuaranteeFileFilterRequestParams();
            this.UIProperties.SetRequired("GuranteeType", null, true);
            this.UIProperties.SetRequired("FileNumber", null, true);
            this.UIProperties.SetRequired("Numeral", null, true);
        }
        if (this.ResponseData) {
            if (this.ResponseData.GuaranteeLettersList) {
                this.GuaranteeLettersList.InsertCollection(this.ResponseData.GuaranteeLettersList);
            }
            if (this.ResponseData.CreditTransactionsList) {
                this.CreditTransactionsList.InsertCollection(this.ResponseData.CreditTransactionsList);
            }
            if (this.ResponseData.RequireDocumentsList) {
                this.RequireDocumentsList.InsertCollection(this.ResponseData.RequireDocumentsList);
            }
        }
        else {
            this.ResponseData = new GuaranteeFileFilterResponseData_1.GuaranteeFileFilterResponseData();
        }
    };
    Object.defineProperty(GuaranteeFileFilterQueryComponent.prototype, "GuranteeType", {
        //#region Properties
        get: function () { return this.RequestParams.GuranteeType; },
        set: function (value) {
            if (this.RequestParams.GuranteeType != value) {
                this.RequestParams.GuranteeType = value;
            }
            if (value) {
                this.UIProperties.SetRequired("GuranteeType", null, false);
            }
            else {
                this.UIProperties.SetRequired("GuranteeType", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeFileFilterQueryComponent.prototype, "FileNumber", {
        get: function () { return this.RequestParams.FileNumber; },
        set: function (value) {
            if (this.RequestParams.FileNumber != value) {
                this.RequestParams.FileNumber = value;
            }
            if (value) {
                this.UIProperties.SetRequired("FileNumber", null, false);
            }
            else {
                this.UIProperties.SetRequired("FileNumber", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeFileFilterQueryComponent.prototype, "Numeral", {
        get: function () { return this.RequestParams.Numeral; },
        set: function (value) {
            if (this.RequestParams.Numeral != value) {
                this.RequestParams.Numeral = value;
            }
            if (value) {
                this.UIProperties.SetRequired("Numeral", null, false);
            }
            else {
                this.UIProperties.SetRequired("Numeral", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeFileFilterQueryComponent.prototype, "DisplayFileNumber", {
        //#endregion Properties
        //#region Response Properties
        get: function () { return this.ResponseData.DisplayFileNumber; },
        set: function (value) {
            if (this.ResponseData.DisplayFileNumber != value) {
                this.ResponseData.DisplayFileNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeFileFilterQueryComponent.prototype, "CustomOfficeName", {
        get: function () { return this.ResponseData.CustomOfficeName; },
        set: function (value) {
            if (this.ResponseData.CustomOfficeName != value) {
                this.ResponseData.CustomOfficeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeFileFilterQueryComponent.prototype, "CreditLimit", {
        get: function () { return this.ResponseData.CreditLimit; },
        set: function (value) {
            if (this.ResponseData.CreditLimit != value) {
                this.ResponseData.CreditLimit = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeFileFilterQueryComponent.prototype, "StatusName", {
        get: function () { return this.ResponseData.StatusName; },
        set: function (value) {
            if (this.ResponseData.StatusName != value) {
                this.ResponseData.StatusName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeFileFilterQueryComponent.prototype, "EntityTypeName", {
        get: function () { return this.ResponseData.EntityTypeName; },
        set: function (value) {
            if (this.ResponseData.EntityTypeName != value) {
                this.ResponseData.EntityTypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeFileFilterQueryComponent.prototype, "CreditBalance", {
        get: function () { return this.ResponseData.CreditBalance; },
        set: function (value) {
            if (this.ResponseData.CreditBalance != value) {
                this.ResponseData.CreditBalance = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeFileFilterQueryComponent.prototype, "GuaranteedName", {
        get: function () { return this.ResponseData.GuaranteedName; },
        set: function (value) {
            if (this.ResponseData.GuaranteedName != value) {
                this.ResponseData.GuaranteedName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeFileFilterQueryComponent.prototype, "EntityNumber", {
        get: function () { return this.ResponseData.EntityNumber; },
        set: function (value) {
            if (this.ResponseData.EntityNumber != value) {
                this.ResponseData.EntityNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeFileFilterQueryComponent.prototype, "GuaranteeExecutedAmountAdjusted", {
        get: function () { return this.ResponseData.GuaranteeExecutedAmountAdjusted; },
        set: function (value) {
            if (this.ResponseData.GuaranteeExecutedAmountAdjusted != value) {
                this.ResponseData.GuaranteeExecutedAmountAdjusted = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeFileFilterQueryComponent.prototype, "AgentName", {
        get: function () { return this.ResponseData.AgentName; },
        set: function (value) {
            if (this.ResponseData.AgentName != value) {
                this.ResponseData.AgentName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeFileFilterQueryComponent.prototype, "Validity", {
        get: function () { return this.ResponseData.Validity; },
        set: function (value) {
            if (this.ResponseData.Validity != value) {
                this.ResponseData.Validity = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeFileFilterQueryComponent.prototype, "GuaranteeAmount", {
        get: function () { return this.ResponseData.GuaranteeAmount; },
        set: function (value) {
            if (this.ResponseData.GuaranteeAmount != value) {
                this.ResponseData.GuaranteeAmount = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion Properties
    //#region Gurantee Type Group
    GuaranteeFileFilterQueryComponent.prototype.BuildGuranteeTypeGroupFilterList = function () {
        this.GuranteeTypeFilterList = [];
        var myGuranteeType = new CodeNameClass();
        myGuranteeType.Code = "4"; // "Gurantee Type"
        myGuranteeType.Name = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.GuaranteeFileFilterQuery.F.GuranteeType.File");
        ;
        this.GuranteeTypeFilterList.push(myGuranteeType);
        var myGuranteeRequest = new CodeNameClass();
        myGuranteeRequest.Code = "6"; // "Gurantee Request"
        myGuranteeRequest.Name = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.GuaranteeFileFilterQuery.F.GuranteeType.Req");
        ;
        this.GuranteeTypeFilterList.push(myGuranteeRequest);
        if (this.RequestParams != null && !Tools_1.AppTool.IsNullOrEmpty(this.RequestParams.GuranteeType)) {
            if (this.RequestParams.GuranteeType == "4") {
                this.SelectedGuranteeTypeFilter = myGuranteeType;
            }
            else if (this.RequestParams.GuranteeType == "6") {
                this.SelectedGuranteeTypeFilter = myGuranteeRequest;
            }
        }
    };
    Object.defineProperty(GuaranteeFileFilterQueryComponent.prototype, "SelectedGuranteeTypeFilter", {
        get: function () {
            return this.selectedGuranteeTypeFilter;
        },
        set: function (newValue) {
            if (this.selectedGuranteeTypeFilter != newValue) {
                this.selectedGuranteeTypeFilter = newValue;
                this.GuranteeType = newValue.Code;
            }
            if (newValue) {
                this.UIProperties.SetRequired("GuranteeType", null, false);
            }
            else {
                this.UIProperties.SetRequired("GuranteeType", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    //#region Commands
    GuaranteeFileFilterQueryComponent.prototype.FillErrors = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (Tools_1.AppTool.IsNullOrEmpty(this.RequestParams.GuranteeType)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.GuaranteeFileFilterQuery.O.GuaranteeType");
            this.ValidationErrorsList.push(msg);
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.RequestParams.FileNumber)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.GuaranteeFileFilterQuery.O.FileNumberMandatory");
            this.ValidationErrorsList.push(msg);
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.RequestParams.Numeral)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.GuaranteeFileFilterQuery.O.NumeralMandatory");
            this.ValidationErrorsList.push(msg);
        }
    };
    GuaranteeFileFilterQueryComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    GuaranteeFileFilterQueryComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        var _this = this;
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this.GuaranteeLettersList.Clear();
        this.CreditTransactionsList.Clear();
        this.RequireDocumentsList.Clear();
        var currRequestParams = new GuaranteeFileFilterRequestParams_1.GuaranteeFileFilterRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.GuranteeType = this.GuranteeType;
        currRequestParams.FileNumber = this.FileNumber;
        currRequestParams.Numeral = this.Numeral;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "שליחת שאילתא לערבויות", true)
            .then(function (res) {
            _this.ResponseData = res;
            _this.MyLastCustomsRequestSheetId = currRequestParams.PBId;
            _this.OnMassageDisplayMethod();
        }).catch(function (err) {
            _this.ValidationErrorsList.push(err);
        });
        this._TapagMessagesService.PostGuaranteeFileFilterQueryRequest(currRequestParams)
            .subscribe(function (myServiceResponse) {
        });
    };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent)
    ], GuaranteeFileFilterQueryComponent.prototype, "SuperCustomMessageWrapperComponent", void 0);
    GuaranteeFileFilterQueryComponent = __decorate([
        core_1.Component({
            selector: 'GuaranteeFileFilterQueryComponent',
            moduleId: module.id,
            templateUrl: './GuaranteeFileFilterQueryComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], GuaranteeFileFilterQueryComponent);
    return GuaranteeFileFilterQueryComponent;
}(BaseRequestsSheetMassaging_1.BaseRequestsSheetMassaging));
exports.GuaranteeFileFilterQueryComponent = GuaranteeFileFilterQueryComponent;
var CodeNameClass = /** @class */ (function () {
    function CodeNameClass() {
    }
    return CodeNameClass;
}());
//# sourceMappingURL=GuaranteeFileFilterQueryComponent.js.map