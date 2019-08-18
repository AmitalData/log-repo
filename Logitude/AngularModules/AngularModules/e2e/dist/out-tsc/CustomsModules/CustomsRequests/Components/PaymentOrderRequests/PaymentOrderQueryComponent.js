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
var PaymentMessagesService_1 = require("../../../../Customs/Services/WebServices/PaymentMessagesService");
var DeclarationExtendedListService_1 = require("../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService");
var CustomsSettingListService_1 = require("../../../../Customs/Services/StandardLists/CustomsSettingListService");
var CustomBankListService_1 = require("../../../../Customs/Services/StandardLists/CustomBankListService");
var PartnersDomainService_1 = require("../../../../Common/Services/PartnersDomainService");
var PaymentOrderRequestParams_1 = require("../../../../Customs/DataContract/RequestParams/PaymentOrderRequestParams");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var BaseRequestsSheetMassaging_1 = require("../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging");
var CustomMessageProgressComponent_1 = require("../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var PaymentOrderQueryComponent = /** @class */ (function (_super) {
    __extends(PaymentOrderQueryComponent, _super);
    function PaymentOrderQueryComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.Declaration";
        _this._IsImporerCodeEnabled = false;
        _this._PaymentMessagesService = new PaymentMessagesService_1.PaymentMessagesService();
        _this._DeclarationExtendedListService = new DeclarationExtendedListService_1.DeclarationExtendedListService();
        _this._CustomsSettingListService = new CustomsSettingListService_1.CustomsSettingListService();
        _this._PartnersDomainService = new PartnersDomainService_1.PartnersDomainService();
        _this._CustomBankListService = new CustomBankListService_1.CustomBankListService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SuperCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        _this.PaymentsDetailsList = new ObservableCollection_1.ObservableCollection([]);
        return _this;
    }
    PaymentOrderQueryComponent.prototype.ngAfterViewInit = function () {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();
    };
    PaymentOrderQueryComponent.prototype.OnMassageDisplayMethod = function () {
        var _this = this;
        if (this.RequestParams == null) {
            this.RequestParams = new PaymentOrderRequestParams_1.PaymentOrderRequestParams();
            if (this._CustomsSettingListService == null) {
                this._CustomsSettingListService = new CustomsSettingListService_1.CustomsSettingListService();
            }
            this._CustomsSettingListService.getSingleFromCache(SessionLocator_1.SessionLocator.Tenant.toString())
                .subscribe(function (customsSettingList) {
                if (customsSettingList) {
                    _this.AgentExternalId = customsSettingList.Result ? customsSettingList.Result.CustomsAgentId : null;
                    _this.UIProperties.SetEnabled("AgentID", null, false);
                }
            });
        }
        if (this.ResponseData) {
            if (this.ResponseData.PaymentsDetailsList) {
                this.PaymentsDetailsList.InsertCollection(this.ResponseData.PaymentsDetailsList);
            }
        }
    };
    Object.defineProperty(PaymentOrderQueryComponent.prototype, "IsImporerCodeEnabled", {
        //#region Properties
        get: function () { return this._IsImporerCodeEnabled; },
        set: function (newValue) {
            if (this._IsImporerCodeEnabled != newValue) {
                this._IsImporerCodeEnabled = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderQueryComponent.prototype, "AgentExternalId", {
        get: function () { return this.RequestParams.AgentExternalId; },
        set: function (value) {
            if (this.RequestParams.AgentExternalId != value) {
                this.RequestParams.AgentExternalId = value;
            }
            if (value) {
                this.UIProperties.SetEnabled("AgentID", null, false);
            }
            else {
                this.UIProperties.SetEnabled("AgentID", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderQueryComponent.prototype, "AgentID", {
        get: function () { return this.RequestParams.AgentID; },
        set: function (value) {
            if (this.RequestParams.AgentID != value) {
                this.RequestParams.AgentID = value;
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
    Object.defineProperty(PaymentOrderQueryComponent.prototype, "CustomFileNo", {
        get: function () { return this.RequestParams.CustomFileNo; },
        set: function (value) {
            if (this.RequestParams.CustomFileNo != value) {
                this.RequestParams.CustomFileNo = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderQueryComponent.prototype, "PaymentDateFrom", {
        get: function () { return this.RequestParams.PaymentDateFrom; },
        set: function (value) {
            if (this.RequestParams.PaymentDateFrom != value) {
                this.RequestParams.PaymentDateFrom = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderQueryComponent.prototype, "PaymentMethodType", {
        get: function () { return this.RequestParams.PaymentMethodType; },
        set: function (value) {
            if (this.RequestParams.PaymentMethodType != value) {
                this.RequestParams.PaymentMethodType = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderQueryComponent.prototype, "ClientId", {
        get: function () { return this.RequestParams.ClientId; },
        set: function (value) {
            var _this = this;
            if (this.RequestParams.ClientId != value) {
                this.RequestParams.ClientId = value;
            }
            if (value) {
                this._PartnersDomainService.GetCustomerById(value)
                    .subscribe(function (myResponse) {
                    if (!myResponse.HasError && myResponse.Result) {
                        _this.ImporterCode = myResponse.Result.VatNumber;
                        _this.IsImporerCodeEnabled = true;
                    }
                });
            }
            else {
                this.IsImporerCodeEnabled = false;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderQueryComponent.prototype, "EntityType", {
        get: function () { return this.RequestParams.EntityType; },
        set: function (value) {
            if (this.RequestParams.EntityType != value) {
                this.RequestParams.EntityType = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderQueryComponent.prototype, "PaymentDateTo", {
        get: function () { return this.RequestParams.PaymentDateTo; },
        set: function (value) {
            if (this.RequestParams.PaymentDateTo != value) {
                this.RequestParams.PaymentDateTo = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderQueryComponent.prototype, "CustomBankId", {
        get: function () { return this.RequestParams.CustomBankId; },
        set: function (value) {
            var _this = this;
            if (this.RequestParams.CustomBankId != value) {
                this.RequestParams.CustomBankId = value;
            }
            if (value) {
                this._CustomBankListService.getSingleFromCache(value)
                    .subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        _this.BankID = myResponse.Result.BankCode;
                        _this.BranchID = myResponse.Result.BranchCode + "," + myResponse.Result.BankCode;
                        _this.BankAccount = myResponse.Result.AccountNumber;
                        _this.UIProperties.SetEnabled("BankID", "Customs.Bank", false);
                        _this.UIProperties.SetEnabled("BranchID", "Customs.CustomsBranch", false);
                        _this.UIProperties.SetEnabled("BankAccount", null, false);
                    }
                });
            }
            else {
                this.BankID = null;
                this.BranchID = null;
                this.BankAccount = null;
                this.UIProperties.SetEnabled("BankID", "Customs.Bank", true);
                this.UIProperties.SetEnabled("BranchID", "Customs.CustomsBranch", true);
                this.UIProperties.SetEnabled("BankAccount", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderQueryComponent.prototype, "ImporterCode", {
        get: function () { return this.RequestParams.ImporterCode; },
        set: function (value) {
            if (this.RequestParams.ImporterCode != value) {
                this.RequestParams.ImporterCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderQueryComponent.prototype, "ImporterName", {
        get: function () { return this._ImporterName; },
        set: function (value) {
            if (this._ImporterName != value) {
                this._ImporterName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderQueryComponent.prototype, "EntityExternalID", {
        get: function () { return this.RequestParams.EntityExternalID; },
        set: function (value) {
            if (this.RequestParams.EntityExternalID != value) {
                this.RequestParams.EntityExternalID = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderQueryComponent.prototype, "EffectiveDateFrom", {
        get: function () { return this.RequestParams.EffectiveDateFrom; },
        set: function (value) {
            if (this.RequestParams.EffectiveDateFrom != value) {
                this.RequestParams.EffectiveDateFrom = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderQueryComponent.prototype, "BankID", {
        get: function () { return this.RequestParams.BankID; },
        set: function (value) {
            if (this.RequestParams.BankID != value) {
                this.RequestParams.BankID = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderQueryComponent.prototype, "PaymentOrderStatus", {
        get: function () { return this.RequestParams.PaymentOrderStatus; },
        set: function (value) {
            if (this.RequestParams.PaymentOrderStatus != value) {
                this.RequestParams.PaymentOrderStatus = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderQueryComponent.prototype, "PaymentID", {
        get: function () { return this.RequestParams.PaymentID; },
        set: function (value) {
            if (this.RequestParams.PaymentID != value) {
                this.RequestParams.PaymentID = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderQueryComponent.prototype, "EffectiveDateTo", {
        get: function () { return this.RequestParams.EffectiveDateTo; },
        set: function (value) {
            if (this.RequestParams.EffectiveDateTo != value) {
                this.RequestParams.EffectiveDateTo = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderQueryComponent.prototype, "BranchID", {
        get: function () { return this.RequestParams.BranchID; },
        set: function (value) {
            if (this.RequestParams.BranchID != value) {
                this.RequestParams.BranchID = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderQueryComponent.prototype, "PaymentType", {
        get: function () { return this.RequestParams.PaymentType; },
        set: function (value) {
            if (this.RequestParams.PaymentType != value) {
                this.RequestParams.PaymentType = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderQueryComponent.prototype, "PaymentProcess", {
        get: function () { return this.RequestParams.PaymentProcess; },
        set: function (value) {
            if (this.RequestParams.PaymentProcess != value) {
                this.RequestParams.PaymentProcess = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderQueryComponent.prototype, "PaymentAmount", {
        get: function () { return this.RequestParams.PaymentAmount; },
        set: function (value) {
            if (this.RequestParams.PaymentAmount != value) {
                this.RequestParams.PaymentAmount = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderQueryComponent.prototype, "BankAccount", {
        get: function () { return this.RequestParams.BankAccount; },
        set: function (value) {
            if (this.RequestParams.BankAccount != value) {
                this.RequestParams.BankAccount = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion Properties
    //#region Importer Commands
    PaymentOrderQueryComponent.prototype.ImporterClicked = function (type, client) {
        this.ImporterCode = client.Code;
        this.ImporterName = Tools_1.AppTool.IsNullOrEmpty(client) ? "" : client.FullName;
    };
    PaymentOrderQueryComponent.prototype.ImporterTextChanged = function (type, item) {
        this.ImporterName = "";
    };
    //#endregion
    //#region Declaration Commands
    PaymentOrderQueryComponent.prototype.DueChangeClearChildField = function (sourceIsCostomFile) {
        this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
        if (sourceIsCostomFile) {
            this.EntityExternalID = "";
            this.EntityType = "";
        }
        else {
            this.CustomFileNo = "";
        }
        this._LastFetchDeclarationList = null;
    };
    PaymentOrderQueryComponent.prototype.CustomFileNoTextChanged = function (searchtext) {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.CustomFileNo)) {
            this.UIProperties.SetEnabled("EntityType", "Customs.EntityTypeLookup", true);
            this.UIProperties.SetEnabled("EntityExternalID", null, true);
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
    PaymentOrderQueryComponent.prototype.FetchDeclaration = function (myResponse, sourceIsCostomFile) {
        this._LastFetchDeclarationList = myResponse.Result;
        if (this._LastFetchDeclarationList != null) {
            this.EntityType = "1055";
            this.EntityExternalID = this._LastFetchDeclarationList.DeclarationNumber;
            this.CustomFileNo = this._LastFetchDeclarationList.CustomFileNo;
            this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
            this.UIProperties.SetEnabled("EntityType", "Customs.EntityTypeLookup", false);
            this.UIProperties.SetEnabled("EntityExternalID", null, false);
        }
        else {
            if (sourceIsCostomFile) {
                this.SetValidityCustomFileNo();
            }
        }
    };
    PaymentOrderQueryComponent.prototype.SetValidityCustomFileNo = function () {
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Didntfindcustomfile");
        this.ValidationErrorsList.push(msg);
        this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, false, msg);
    };
    //#endregion
    //#region General Commands
    PaymentOrderQueryComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    PaymentOrderQueryComponent.prototype.FillErrors = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
    };
    PaymentOrderQueryComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        var _this = this;
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this.PaymentsDetailsList.Clear();
        var currRequestParams = new PaymentOrderRequestParams_1.PaymentOrderRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.CustomFileNo = this.CustomFileNo;
        currRequestParams.AgentID = this.AgentID;
        currRequestParams.AgentExternalId = this.AgentExternalId;
        currRequestParams.BankAccount = this.BankAccount;
        currRequestParams.BankID = this.BankID;
        currRequestParams.BranchID = this.BranchID;
        currRequestParams.ClientId = this.ClientId;
        currRequestParams.CustomBankId = this.CustomBankId;
        currRequestParams.EffectiveDateFrom = this.EffectiveDateFrom;
        currRequestParams.EffectiveDateTo = this.EffectiveDateTo;
        currRequestParams.EntityExternalID = this.EntityExternalID;
        currRequestParams.EntityType = this.EntityType;
        currRequestParams.ExternalID = this.ImporterCode;
        currRequestParams.PaymentAmount = this.PaymentAmount;
        currRequestParams.paymentDateFrom = this.PaymentDateFrom;
        currRequestParams.paymentDateTo = this.PaymentDateTo;
        currRequestParams.PaymentID = this.PaymentID;
        currRequestParams.PaymentMethodType = this.PaymentMethodType;
        currRequestParams.PaymentOrderStatus = this.PaymentOrderStatus;
        currRequestParams.PaymentProcess = this.PaymentProcess;
        currRequestParams.PaymentType = this.PaymentType;
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "שליחת שאילתא להוראות תשלום", true)
            .then(function (res) {
            _this.ResponseData = res;
            _this.OnMassageDisplayMethod();
        }).catch(function (err) {
            _this.ValidationErrorsList.push(err);
        });
        this._PaymentMessagesService.PostPaymentOrderQueryRequest(currRequestParams)
            .subscribe(function (myServiceResponse) {
        });
    };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent)
    ], PaymentOrderQueryComponent.prototype, "SuperCustomMessageWrapperComponent", void 0);
    PaymentOrderQueryComponent = __decorate([
        core_1.Component({
            selector: 'PaymentOrderQueryComponent',
            moduleId: module.id,
            templateUrl: './PaymentOrderQueryComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], PaymentOrderQueryComponent);
    return PaymentOrderQueryComponent;
}(BaseRequestsSheetMassaging_1.BaseRequestsSheetMassaging));
exports.PaymentOrderQueryComponent = PaymentOrderQueryComponent;
//# sourceMappingURL=PaymentOrderQueryComponent.js.map