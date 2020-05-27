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
var DeclarationExtendedListService_1 = require("../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService");
var DeclarationFilterRequestParams_1 = require("../../../../Customs/DataContract/RequestParams/DeclarationFilterRequestParams");
var DeclarationFilterResponseData_1 = require("../../../../Customs/DataContract/ResponseData/DeclarationFilterResponseData");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var BaseRequestsSheetMassaging_1 = require("../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging");
var CustomMessageProgressComponent_1 = require("../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var DeclarationFilterComponent = /** @class */ (function (_super) {
    __extends(DeclarationFilterComponent, _super);
    function DeclarationFilterComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.Declaration";
        _this._TapagMessagesService = new TapagMessagesService_1.TapagMessagesService();
        _this._DeclarationExtendedListService = new DeclarationExtendedListService_1.DeclarationExtendedListService();
        _this._MyResponseObjectToShow = null;
        _this._UserMessagehidden = true;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SuperCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        _this.ClaimObservableCollection = new ObservableCollection_1.ObservableCollection([]);
        _this.DeficitObservableCollection = new ObservableCollection_1.ObservableCollection([]);
        _this.UIProperties.SetRequired("DeclarationNumber", _this.ObjectTableName, true);
        return _this;
    }
    DeclarationFilterComponent.prototype.ngAfterViewInit = function () {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();
    };
    DeclarationFilterComponent.prototype.SetMenuArg = function (MenuArg) {
        this.RequestParams = MenuArg;
        this.OnMassageDisplayMethod();
        //this.CustomFileNo = MenuArg.CustomFileNo;
        //this.DeclarationNumber= MenuArg.DeclarationNumber;
    };
    DeclarationFilterComponent.prototype.OnMassageDisplayMethod = function () {
        if (this.RequestParams == null) {
            this.RequestParams = new DeclarationFilterRequestParams_1.DeclarationFilterRequestParams();
            this.UIProperties.SetRequired("GuranteeType", null, true);
            this.UIProperties.SetRequired("FileNumber", null, true);
            this.UIProperties.SetRequired("Numeral", null, true);
        }
        this._MyResponseObjectToShow = null;
        if (this.ResponseData) {
            var myDeclarationFilterResponseData = this.ResponseData;
            if (myDeclarationFilterResponseData.ClaimList) {
                this.ClaimObservableCollection.InsertCollection(myDeclarationFilterResponseData.ClaimList);
            }
            if (myDeclarationFilterResponseData.DeficitList) {
                this.DeficitObservableCollection.InsertCollection(myDeclarationFilterResponseData.ClaimList);
            }
            try {
                this._MyResponseObjectToShow = JSON.parse(myDeclarationFilterResponseData.ResponseStatusXML);
            }
            catch (err) {
                console.log(err);
            }
        }
        else {
            this.ResponseData = new DeclarationFilterResponseData_1.DeclarationFilterResponseData();
        }
    };
    DeclarationFilterComponent.prototype.DueChangeClearChildField = function (sourceIsCostomFile) {
        this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
        this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, true, "");
        if (sourceIsCostomFile) {
            this.DeclarationNumber = "";
        }
        else {
            this.CustomFileNo = "";
        }
        this.DeclarationId = "";
        this.ResponseData = null;
        this._LastFetchDeclarationList = null;
        this.ValidationErrorsList = [];
    };
    DeclarationFilterComponent.prototype.CustomFileNoTextChanged = function (searchtext) {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.CustomFileNo)) {
            return;
        }
        if (this._LastFetchDeclarationList != null) {
            if (this.CustomFileNo == this._LastFetchDeclarationList.CustomFileNo) {
                return;
            }
        }
        this.DueChangeClearChildField(true);
        this.CurrentSession.StartBusyIndicator("");
        this._DeclarationExtendedListService.GetSingleDeclarationByCustomFileNo(this.CustomFileNo)
            .subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            _this.FetchDeclaration(myResponse, true);
        });
    };
    DeclarationFilterComponent.prototype.DeclarationNumberTextChanged = function (DeclarationNumberText) {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.DeclarationNumber)) {
            return;
        }
        if (this._LastFetchDeclarationList != null) {
            if (this.DeclarationNumber == this._LastFetchDeclarationList.DeclarationNumber) {
                return;
            }
        }
        this.DueChangeClearChildField(false);
        this.CurrentSession.StartBusyIndicator("");
        this._DeclarationExtendedListService.GetSingleDeclarationByNumber(this.DeclarationNumber, SessionLocator_1.SessionLocator.Tenant)
            .subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            _this.FetchDeclaration(myResponse, false);
        });
    };
    DeclarationFilterComponent.prototype.FetchDeclaration = function (myResponse, sourceIsCostomFile) {
        this._LastFetchDeclarationList = myResponse.Result;
        if (this._LastFetchDeclarationList != null) {
            this.DeclarationId = this._LastFetchDeclarationList.Id;
            this.DeclarationNumber = this._LastFetchDeclarationList.DeclarationNumber;
            this.CustomFileNo = this._LastFetchDeclarationList.CustomFileNo;
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
    DeclarationFilterComponent.prototype.SetValidityDeclarationNumber = function () {
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationNumberIsMandatory");
        this.ValidationErrorsList.push(msg);
        this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, false, msg);
    };
    DeclarationFilterComponent.prototype.SetValidityCustomFileNo = function () {
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Didntfindcustomfile");
        this.ValidationErrorsList.push(msg);
        this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, false, msg);
    };
    Object.defineProperty(DeclarationFilterComponent.prototype, "CustomFileNo", {
        get: function () { return this.RequestParams ? this.RequestParams.CustomsFile : null; },
        set: function (value) {
            if (this.RequestParams.CustomsFile != value) {
                this.RequestParams.CustomsFile = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationFilterComponent.prototype, "DeclarationNumber", {
        get: function () { return this.RequestParams ? this.RequestParams.DeclarationNumber : null; },
        set: function (value) {
            if (this.RequestParams.DeclarationNumber != value) {
                this.RequestParams.DeclarationNumber = value;
                if (value) {
                    this.UIProperties.SetRequired("DeclarationNumber", this.ObjectTableName, false);
                }
            }
            else {
                this.UIProperties.SetRequired("DeclarationNumber", this.ObjectTableName, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationFilterComponent.prototype, "DeclarationId", {
        get: function () { return this.RequestParams ? this.RequestParams.DeclarationId : null; },
        set: function (value) {
            if (this.RequestParams.DeclarationId != value) {
                this.RequestParams.DeclarationId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    DeclarationFilterComponent.prototype.RefreshScreen = function () {
        this._MyResponseObjectToShow = null;
        this._UserMessagehidden = true;
        if (this.ResponseData == null) {
            return;
        }
        this._UserMessagehidden = !this.ResponseData.IsShowUserMessage;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ResponseData.ResponseStatusXML)) {
            this.UIProperties.SetRequired("DeclarationNumber", this.ObjectTableName, false);
            try {
                this._MyResponseObjectToShow = JSON.parse(this.ResponseData.ResponseStatusXML);
            }
            catch (err) {
                console.log(err);
            }
        }
    };
    Object.defineProperty(DeclarationFilterComponent.prototype, "DisplayFileNumber", {
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
    Object.defineProperty(DeclarationFilterComponent.prototype, "CustomOfficeName", {
        get: function () { return this.ResponseData.CustomOfficeName; },
        set: function (value) {
            if (this.ResponseData.CustomOfficeName != value) {
                this.ResponseData.CustomOfficeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationFilterComponent.prototype, "CreditLimit", {
        get: function () { return this.ResponseData.CreditLimit; },
        set: function (value) {
            if (this.ResponseData.CreditLimit != value) {
                this.ResponseData.CreditLimit = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationFilterComponent.prototype, "StatusName", {
        get: function () { return this.ResponseData.StatusName; },
        set: function (value) {
            if (this.ResponseData.StatusName != value) {
                this.ResponseData.StatusName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationFilterComponent.prototype, "EntityTypeName", {
        get: function () { return this.ResponseData.EntityTypeName; },
        set: function (value) {
            if (this.ResponseData.EntityTypeName != value) {
                this.ResponseData.EntityTypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationFilterComponent.prototype, "CreditBalance", {
        get: function () { return this.ResponseData.CreditBalance; },
        set: function (value) {
            if (this.ResponseData.CreditBalance != value) {
                this.ResponseData.CreditBalance = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationFilterComponent.prototype, "GuaranteedName", {
        get: function () { return this.ResponseData.GuaranteedName; },
        set: function (value) {
            if (this.ResponseData.GuaranteedName != value) {
                this.ResponseData.GuaranteedName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationFilterComponent.prototype, "EntityNumber", {
        get: function () { return this.ResponseData.EntityNumber; },
        set: function (value) {
            if (this.ResponseData.EntityNumber != value) {
                this.ResponseData.EntityNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationFilterComponent.prototype, "GuaranteeExecutedAmountAdjusted", {
        get: function () { return this.ResponseData.GuaranteeExecutedAmountAdjusted; },
        set: function (value) {
            if (this.ResponseData.GuaranteeExecutedAmountAdjusted != value) {
                this.ResponseData.GuaranteeExecutedAmountAdjusted = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationFilterComponent.prototype, "AgentName", {
        get: function () { return this.ResponseData.AgentName; },
        set: function (value) {
            if (this.ResponseData.AgentName != value) {
                this.ResponseData.AgentName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationFilterComponent.prototype, "Validity", {
        get: function () { return this.ResponseData.Validity; },
        set: function (value) {
            if (this.ResponseData.Validity != value) {
                this.ResponseData.Validity = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationFilterComponent.prototype, "GuaranteeAmount", {
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
    DeclarationFilterComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    DeclarationFilterComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (Tools_1.AppTool.IsNullOrEmpty(this.RequestParams.DeclarationId) || Tools_1.AppTool.IsNullOrEmpty(this.DeclarationNumber)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationNumberIsMandatory");
            this.ValidationErrorsList.push(msg);
        }
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this.ClaimObservableCollection.Clear();
        var currRequestParams = new DeclarationFilterRequestParams_1.DeclarationFilterRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.AppicationId = this.RequestParams.DeclarationId;
        currRequestParams.DeclarationId = this.RequestParams.DeclarationId;
        currRequestParams.DeclarationNumber = this.RequestParams.DeclarationNumber;
        currRequestParams.CustomsFile = this.RequestParams.CustomsFile;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, 'שליחת שאילתא לנתוני תפ""ג עבור הצהרה', true)
            .then(function (res) {
            _this.ResponseData = res;
            _this.OnMassageDisplayMethod();
        }).catch(function (err) {
            _this.ValidationErrorsList.push(err);
        });
        this._TapagMessagesService.PostDeclarationFilterRequestParams(currRequestParams)
            .subscribe(function (myServiceResponse) {
        });
    };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent)
    ], DeclarationFilterComponent.prototype, "SuperCustomMessageWrapperComponent", void 0);
    DeclarationFilterComponent = __decorate([
        core_1.Component({
            selector: 'DeclarationFilterComponent',
            moduleId: module.id,
            templateUrl: './DeclarationFilterComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], DeclarationFilterComponent);
    return DeclarationFilterComponent;
}(BaseRequestsSheetMassaging_1.BaseRequestsSheetMassaging));
exports.DeclarationFilterComponent = DeclarationFilterComponent;
//# sourceMappingURL=DeclarationFilterComponent.js.map