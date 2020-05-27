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
var EntityArgs_1 = require("../../../../../../Infrastructure/DataContracts/EntityArgs");
var Tools_1 = require("../../../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../../../Infrastructure/Utilities/SessionLocator");
var DepositPM_1 = require("../../../../../../Customs/EntityPMs/DepositPM");
var TapagList_1 = require("../../../../../../Customs/EntityLists/TapagList");
var BaseComponent_1 = require("../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ObservableCollection_1 = require("../../../../../../Infrastructure/Utilities/ObservableCollection");
var TapagMessagesService_1 = require("../../../../../../Customs/Services/WebServices/TapagMessagesService");
var DeclarationWebService_1 = require("../../../../../../Customs/Services/WebServices/DeclarationWebService");
var EntityResourceService_1 = require("../../../../../../Infrastructure/Services/EntityResourceService");
var CustomsRequestMenuService_1 = require("../../../../../../Customs/Services/Others/CustomsRequestMenuService");
var PaymentOrderDepositDataComponent = /** @class */ (function (_super) {
    __extends(PaymentOrderDepositDataComponent, _super);
    function PaymentOrderDepositDataComponent(entityArgs, EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityResourceService = EntityResourceService;
        _this.EntityPM = new DepositPM_1.DepositPM();
        _this.ObjectTableName = "Customs.Deposit";
        _this.DataContext = _this;
        _this.PaymentOrderPM = null;
        _this.Tapag = new TapagList_1.TapagList();
        _this.IsTab = false;
        _this.IsLoaded = false;
        _this.tapagMessagesService = new TapagMessagesService_1.TapagMessagesService();
        _this.declarationWebService = new DeclarationWebService_1.DeclarationWebService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.DepositConditions = new ObservableCollection_1.ObservableCollection([]);
        _this.ConnectedEntitiesList = new ObservableCollection_1.ObservableCollection([]);
        if (!Tools_1.AppTool.IsNullOrEmpty(entityArgs)) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrder").subscribe(function (response) {
                _this.EntityResourceService.getEntityResourceByTableName("Customs.Tapag").subscribe(function (response) {
                    _this.EntityResourceService.getEntityResourceByTableName("Customs.TapagConnectionTable").subscribe(function (response) {
                        _this.EntityResourceService.getEntityResourceByTableName("Customs.Deposit").subscribe(function (response) {
                            _this.EntityResourceService.getEntityResourceByTableName("Customs.DepositCondition").subscribe(function (response) {
                                _this.Listen();
                                if (entityArgs.ObjectTableName == "Customs.PaymentOrder") {
                                    _this.PaymentOrderPM = entityArgs.EntityPM;
                                    _this.LoadDepositData(_this.PaymentOrderPM.PaymentNumber, null, _this.PaymentOrderPM.Tenant);
                                }
                                _this.InitPaymentOrderDepositDataScreen();
                                _this.IsLoaded = true;
                            });
                        });
                    });
                });
            });
        }
        return _this;
    }
    PaymentOrderDepositDataComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.InitPaymentOrderDepositDataScreen();
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.TabSelected.subscribe(function (tabCode) {
                if (_this.CurrentEditComponentId == _this.CurrentSession.CurrentEditComponent.ComponentId) {
                    if (tabCode == "PODP") {
                        _this.PaymentOrderPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        _this.LoadDepositData(_this.PaymentOrderPM.PaymentNumber, null, _this.PaymentOrderPM.Tenant);
                    }
                }
            }));
        }
    };
    PaymentOrderDepositDataComponent.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            this.Tapag = args.EntityPM;
            this.LoadDepositData(null, this.Tapag.Id, this.Tapag.Tenant);
            this.IsTab = true;
            this.IsLoaded = true;
        }
    };
    PaymentOrderDepositDataComponent.prototype.InitPaymentOrderDepositDataScreen = function () {
        this.UIProperties.SetEnabled("LeadingFileNumber", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("CustomerName", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("CustomerId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ImporterId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ImporterName", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ProfessionUnitTypeName", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("CustomsBranchCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ProfessionUnitTypeCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("CustomsBranchName", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("EntityTypeCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("EntityNumber", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("DepositEssenceTypeCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("DepositAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("TradeMarkNumber", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("LawyerNumber", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("VehicleChassisNumber", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("EngineNumber", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("BirthDate", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("PaymentNumber", this.ObjectTableName, false);
    };
    PaymentOrderDepositDataComponent.prototype.LoadDepositData = function (paymentNumber, tapagId, tenant) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(paymentNumber) || !Tools_1.AppTool.IsNullOrEmpty(tapagId)) {
            this.tapagMessagesService.GetDepositPMByPaymentOrderNumberOrTapagId(paymentNumber, tapagId, tenant)
                .subscribe(function (myResponse) {
                _this.GetDepositPMByPaymentOrderNumberOrTapagIdOp_Completed(myResponse, false);
            });
        }
    };
    PaymentOrderDepositDataComponent.prototype.GetDepositPMByPaymentOrderNumberOrTapagIdOp_Completed = function (myResponse, sourceIsCostomFile) {
        if (myResponse.Result != null) {
            this.EntityPM = myResponse.Result;
            this.FillDepositConditionsList();
        }
    };
    PaymentOrderDepositDataComponent.prototype.FillDepositConditionsList = function () {
        var _this = this;
        this.DepositConditions = new ObservableCollection_1.ObservableCollection([]);
        if (this.EntityPM != null && this.EntityPM.DepositConditions.length > 0) {
            this.EntityPM.DepositConditions.forEach(function (depositConditionItem) {
                _this.DepositConditions.Insert(depositConditionItem);
            });
        }
        if (this.IsTab) {
            this.GetConnectedDeclarations();
        }
        else if (this.EntityPM != null) {
            this.tapagMessagesService.GetSingleTapagList(this.EntityPM.TapagID, this.EntityPM.Tenant)
                .subscribe(function (myResponse) {
                _this.GetSingleTapagListOp_Completed(myResponse, false);
            });
        }
    };
    PaymentOrderDepositDataComponent.prototype.GetConnectedDeclarations = function () {
        var _this = this;
        if (this.Tapag != null) {
            this.EntityPM.LeadingFileNumber = this.Tapag.LeadingFileNumber;
            this.declarationWebService.GetDeclarationByTapagConnectionConnection(this.Tapag.Id)
                .subscribe(function (myResponse) {
                _this.GetDeclarationByTapagConnectionConnectionOp_Completed(myResponse, false);
            });
        }
    };
    PaymentOrderDepositDataComponent.prototype.GetDeclarationByTapagConnectionConnectionOp_Completed = function (myResponse, sourceIsCostomFile) {
        var _this = this;
        this.ConnectedEntitiesList = new ObservableCollection_1.ObservableCollection([]);
        if (myResponse.Result != null) {
            myResponse.Result.forEach(function (declarationList) {
                _this.ConnectedEntitiesList.Insert(declarationList);
            });
        }
    };
    PaymentOrderDepositDataComponent.prototype.GetSingleTapagListOp_Completed = function (myResponse, sourceIsCostomFile) {
        if (myResponse.Result != null) {
            this.Tapag = myResponse.Result;
            this.GetConnectedDeclarations();
        }
    };
    Object.defineProperty(PaymentOrderDepositDataComponent.prototype, "LeadingFileNumber", {
        get: function () { return this.EntityPM.LeadingFileNumber; },
        set: function (newValue) { this.EntityPM.LeadingFileNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderDepositDataComponent.prototype, "CustomerName", {
        get: function () { return this.EntityPM.CustomerName; },
        set: function (newValue) { this.EntityPM.CustomerName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderDepositDataComponent.prototype, "CustomerId", {
        get: function () { return this.Tapag ? this.Tapag.CustomerId : null; },
        set: function (newValue) { this.Tapag.CustomerId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderDepositDataComponent.prototype, "ImporterId", {
        get: function () { return this.Tapag ? this.Tapag.ImporterId : null; },
        set: function (newValue) { this.Tapag.ImporterId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderDepositDataComponent.prototype, "ImporterName", {
        get: function () { return this.EntityPM.ImporterName; },
        set: function (newValue) { this.EntityPM.ImporterName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderDepositDataComponent.prototype, "ProfessionUnitTypeName", {
        get: function () { return this.EntityPM.ProfessionUnitTypeName; },
        set: function (newValue) { this.EntityPM.ProfessionUnitTypeName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderDepositDataComponent.prototype, "CustomsBranchCode", {
        get: function () { return this.Tapag ? this.Tapag.CustomsBranchCode : null; },
        set: function (newValue) { this.Tapag.CustomsBranchCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderDepositDataComponent.prototype, "ProfessionUnitTypeCode", {
        get: function () { return this.Tapag ? this.Tapag.ProfessionUnitTypeCode : null; },
        set: function (newValue) { this.Tapag.ProfessionUnitTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderDepositDataComponent.prototype, "CustomsBranchName", {
        get: function () { return this.EntityPM.CustomsBranchName; },
        set: function (newValue) { this.EntityPM.CustomsBranchName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderDepositDataComponent.prototype, "EntityTypeCode", {
        get: function () { return this.EntityPM.EntityTypeCode; },
        set: function (newValue) { this.EntityPM.EntityTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderDepositDataComponent.prototype, "EntityNumber", {
        get: function () { return this.EntityPM.EntityNumber; },
        set: function (newValue) { this.EntityPM.EntityNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderDepositDataComponent.prototype, "DepositEssenceTypeCode", {
        get: function () { return this.EntityPM.DepositEssenceTypeCode; },
        set: function (newValue) { this.EntityPM.DepositEssenceTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderDepositDataComponent.prototype, "DepositAmount", {
        get: function () { return this.EntityPM.DepositAmount; },
        set: function (newValue) { this.EntityPM.DepositAmount = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderDepositDataComponent.prototype, "TradeMarkNumber", {
        get: function () { return this.EntityPM.TradeMarkNumber; },
        set: function (newValue) { this.EntityPM.TradeMarkNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderDepositDataComponent.prototype, "LawyerNumber", {
        get: function () { return this.EntityPM.LawyerNumber; },
        set: function (newValue) { this.EntityPM.LawyerNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderDepositDataComponent.prototype, "VehicleChassisNumber", {
        get: function () { return this.EntityPM.VehicleChassisNumber; },
        set: function (newValue) { this.EntityPM.VehicleChassisNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderDepositDataComponent.prototype, "EngineNumber", {
        get: function () { return this.EntityPM.EngineNumber; },
        set: function (newValue) { this.EntityPM.EngineNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderDepositDataComponent.prototype, "BirthDate", {
        get: function () { return this.EntityPM.BirthDate; },
        set: function (newValue) { this.EntityPM.BirthDate = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderDepositDataComponent.prototype, "PaymentNumber", {
        get: function () { return this.EntityPM.PaymentNumber; },
        set: function (newValue) { this.EntityPM.PaymentNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    PaymentOrderDepositDataComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    };
    PaymentOrderDepositDataComponent.prototype.BankAccountToRefundButtonClicked = function () {
        var declarationId;
        if (this.ConnectedEntitiesList != null && this.ConnectedEntitiesList.Length > 0) {
            declarationId = this.ConnectedEntitiesList.Collection[0].Id;
        }
        var customsRequestMenuService = new CustomsRequestMenuService_1.CustomsRequestMenuService();
        var my = {
            "DeclarationId": declarationId,
        };
        customsRequestMenuService.WindowClosed.subscribe(function (myarg) { });
        customsRequestMenuService.ShowModalAsEditMenuAction("2018", my);
    };
    PaymentOrderDepositDataComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './PaymentOrderDepositDataComponent.html',
            selector: 'PaymentOrderDepositDataComponent',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], PaymentOrderDepositDataComponent);
    return PaymentOrderDepositDataComponent;
}(BaseComponent_1.BaseComponent));
exports.PaymentOrderDepositDataComponent = PaymentOrderDepositDataComponent;
//# sourceMappingURL=PaymentOrderDepositDataComponent.js.map