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
var DeficitPM_1 = require("../../../../../../Customs/EntityPMs/DeficitPM");
var BaseComponent_1 = require("../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ObservableCollection_1 = require("../../../../../../Infrastructure/Utilities/ObservableCollection");
var TapagMessagesService_1 = require("../../../../../../Customs/Services/WebServices/TapagMessagesService");
var DeclarationWebService_1 = require("../../../../../../Customs/Services/WebServices/DeclarationWebService");
var EntityResourceService_1 = require("../../../../../../Infrastructure/Services/EntityResourceService");
var PaymentOrderDeficitComponent = /** @class */ (function (_super) {
    __extends(PaymentOrderDeficitComponent, _super);
    function PaymentOrderDeficitComponent(entityArgs, EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityResourceService = EntityResourceService;
        _this.EntityPM = new DeficitPM_1.DeficitPM();
        _this.ObjectTableName = "Customs.Deficit";
        _this.DataContext = _this;
        _this.PaymentOrderPM = null;
        _this.Tapag = null;
        _this.IsTab = false;
        _this.IsLoaded = false;
        _this.tapagMessagesService = new TapagMessagesService_1.TapagMessagesService();
        _this.declarationWebService = new DeclarationWebService_1.DeclarationWebService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.ConnectedEntitiesList = new ObservableCollection_1.ObservableCollection([]);
        _this.UIProperties.SetRequired("LeadingFileNumber", 'Customs.Tapag', false);
        if (!Tools_1.AppTool.IsNullOrEmpty(entityArgs)) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrder").subscribe(function (response) {
                _this.EntityResourceService.getEntityResourceByTableName("Customs.Tapag").subscribe(function (response) {
                    _this.EntityResourceService.getEntityResourceByTableName("Customs.TapagConnectionTable").subscribe(function (response) {
                        _this.EntityResourceService.getEntityResourceByTableName("Customs.Deficit").subscribe(function (response) {
                            _this.Listen();
                            if (entityArgs.ObjectTableName == "Customs.PaymentOrder") {
                                _this.PaymentOrderPM = entityArgs.EntityPM;
                                _this.LoadDeficitData(_this.PaymentOrderPM.PaymentNumber, null, _this.PaymentOrderPM.Tenant);
                            }
                            _this.IsLoaded = true;
                        });
                    });
                });
            });
        }
        return _this;
    }
    PaymentOrderDeficitComponent.prototype.Listen = function () {
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
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.TabSelected.subscribe(function (tabCode) {
                if (_this.CurrentEditComponentId == _this.CurrentSession.CurrentEditComponent.ComponentId) {
                    if (tabCode == "PODF") {
                        _this.PaymentOrderPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        _this.LoadDeficitData(_this.PaymentOrderPM.PaymentNumber, null, _this.PaymentOrderPM.Tenant);
                    }
                }
            }));
        }
    };
    PaymentOrderDeficitComponent.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            this.Tapag = args.EntityPM;
            this.LoadDeficitData(null, this.Tapag.Id, this.Tapag.Tenant);
            this.IsTab = true;
            this.IsLoaded = true;
        }
    };
    PaymentOrderDeficitComponent.prototype.LoadDeficitData = function (paymentNumber, tapagId, tenant) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(paymentNumber) || !Tools_1.AppTool.IsNullOrEmpty(tapagId)) {
            this.tapagMessagesService.GetDeficitPMByPaymentOrderNumberOrTapagId(paymentNumber, tapagId, tenant)
                .subscribe(function (myResponse) {
                _this.GetDeficitPMByPaymentOrderNumberOrTapagIdOp_Completed(myResponse, false);
            });
        }
    };
    PaymentOrderDeficitComponent.prototype.GetDeficitPMByPaymentOrderNumberOrTapagIdOp_Completed = function (myResponse, sourceIsCostomFile) {
        var _this = this;
        if (myResponse.Result != null) {
            this.EntityPM = myResponse.Result;
            if (this.IsTab) {
                this.GetConnectedDeclarations();
            }
            else if (this.EntityPM != null) {
                this.tapagMessagesService.GetSingleTapagList(this.EntityPM.TapagId, this.EntityPM.Tenant)
                    .subscribe(function (myResponse) {
                    _this.GetSingleTapagListOp_Completed(myResponse, false);
                });
            }
            //to do.....
            //else if (entityPM.CustomsEntityTypeCode == "11122" && !string.IsNullOrEmpty(entityPM.FirstEntityID)) {
            //    LoadOperation tapagOp = context.Load(context.GetSingleTapagByLeadingFileNumberQuery(entityPM.FirstEntityID, entityPM.Tenant), LoadBehavior.RefreshCurrent, true);
            //    tapagOp.Completed += tapagOp_Completed;
            //}
        }
    };
    PaymentOrderDeficitComponent.prototype.GetSingleTapagListOp_Completed = function (myResponse, sourceIsCostomFile) {
        if (myResponse.Result != null) {
            this.Tapag = myResponse.Result;
            this.GetConnectedDeclarations();
        }
    };
    PaymentOrderDeficitComponent.prototype.GetConnectedDeclarations = function () {
        var _this = this;
        if (this.Tapag != null) {
            //this.UIProperties.SetRequired("LeadingFileNumber", 'Customs.Tapag', false);
            this.EntityPM.LeadingFileNumber = this.Tapag.LeadingFileNumber;
            this.declarationWebService.GetDeclarationByTapagConnectionConnection(this.Tapag.Id)
                .subscribe(function (myResponse) {
                _this.GetDeclarationByTapagConnectionConnectionOp_Completed(myResponse, false);
            });
        }
    };
    PaymentOrderDeficitComponent.prototype.GetDeclarationByTapagConnectionConnectionOp_Completed = function (myResponse, sourceIsCostomFile) {
        var _this = this;
        this.ConnectedEntitiesList = new ObservableCollection_1.ObservableCollection([]);
        if (myResponse.Result != null) {
            myResponse.Result.forEach(function (declarationList) {
                _this.ConnectedEntitiesList.Insert(new ConnectedEntityLineComponent(declarationList, _this.EntityPM));
            });
        }
    };
    Object.defineProperty(PaymentOrderDeficitComponent.prototype, "LeadingFileNumber", {
        get: function () { return this.EntityPM.LeadingFileNumber; },
        set: function (newValue) { this.EntityPM.LeadingFileNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderDeficitComponent.prototype, "DebtNotificationNumber", {
        get: function () { return this.EntityPM.DebtNotificationNumber; },
        set: function (newValue) { this.EntityPM.DebtNotificationNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderDeficitComponent.prototype, "CustomerId", {
        get: function () { return this.Tapag ? this.Tapag.CustomerId : null; },
        set: function (newValue) { this.Tapag.CustomerId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderDeficitComponent.prototype, "ImporterId", {
        get: function () { return this.Tapag ? this.Tapag.ImporterId : null; },
        set: function (newValue) { this.Tapag.ImporterId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderDeficitComponent.prototype, "ImporterName", {
        get: function () { return this.EntityPM.ImporterName; },
        set: function (newValue) { this.EntityPM.ImporterName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderDeficitComponent.prototype, "NotificationTypeName", {
        get: function () { return this.EntityPM.NotificationTypeName; },
        set: function (newValue) { this.EntityPM.NotificationTypeName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderDeficitComponent.prototype, "NotificationTypeCode", {
        get: function () { return this.EntityPM.NotificationTypeCode; },
        set: function (newValue) { this.EntityPM.NotificationTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderDeficitComponent.prototype, "ProductionDate", {
        get: function () { return this.EntityPM.ProductionDate; },
        set: function (newValue) { this.EntityPM.ProductionDate = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderDeficitComponent.prototype, "RealesGoodsDescription", {
        get: function () { return this.EntityPM.RealesGoodsDescription; },
        set: function (newValue) { this.EntityPM.RealesGoodsDescription = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderDeficitComponent.prototype, "CustomsBranchName", {
        get: function () { return this.EntityPM.CustomsBranchName; },
        set: function (newValue) { this.EntityPM.CustomsBranchName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderDeficitComponent.prototype, "CustomsBranchCode", {
        get: function () { return this.Tapag ? this.Tapag.CustomsBranchCode : null; },
        set: function (newValue) { this.Tapag.CustomsBranchCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderDeficitComponent.prototype, "ProfessionUnitTypeCode", {
        get: function () { return this.Tapag ? this.Tapag.ProfessionUnitTypeCode : null; },
        set: function (newValue) { this.Tapag.ProfessionUnitTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderDeficitComponent.prototype, "DebtNotificationReason", {
        get: function () { return this.EntityPM.DebtNotificationReason; },
        set: function (newValue) { this.EntityPM.DebtNotificationReason = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderDeficitComponent.prototype, "PaymentOrderNumber", {
        get: function () { return this.EntityPM.PaymentOrderNumber; },
        set: function (newValue) { this.EntityPM.PaymentOrderNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderDeficitComponent.prototype, "ValidityDateTo", {
        get: function () { return this.EntityPM.ValidityDateTo; },
        set: function (newValue) { this.EntityPM.ValidityDateTo = newValue; },
        enumerable: true,
        configurable: true
    });
    PaymentOrderDeficitComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    };
    PaymentOrderDeficitComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './PaymentOrderDeficitComponent.html',
            selector: 'PaymentOrderDeficitComponent',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], PaymentOrderDeficitComponent);
    return PaymentOrderDeficitComponent;
}(BaseComponent_1.BaseComponent));
exports.PaymentOrderDeficitComponent = PaymentOrderDeficitComponent;
var ConnectedEntityLineComponent = /** @class */ (function (_super) {
    __extends(ConnectedEntityLineComponent, _super);
    function ConnectedEntityLineComponent(entityPM, deficitPM) {
        var _this = _super.call(this) || this;
        _this.entityPM = entityPM;
        _this.deficitPM = deficitPM;
        _this.ObjectTableName = "Customs.ClaimsRelatedEntity";
        _this.DataContext = _this;
        _this.totalTax = "0.0";
        _this.tapagMessagesService = new TapagMessagesService_1.TapagMessagesService();
        _this.AmountsList = new ObservableCollection_1.ObservableCollection([]);
        _this.LoadDeficitConnectedFileParagraphTypes();
        return _this;
    }
    Object.defineProperty(ConnectedEntityLineComponent.prototype, "CustomFileNo", {
        get: function () { return this.entityPM.CustomFileNo; },
        set: function (newValue) { this.entityPM.CustomFileNo = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConnectedEntityLineComponent.prototype, "DeclarationNumber", {
        get: function () { return this.entityPM.DeclarationNumber; },
        set: function (newValue) { this.entityPM.DeclarationNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConnectedEntityLineComponent.prototype, "PaymentDate", {
        get: function () { return this.entityPM.PaymentDate; },
        set: function (newValue) { this.entityPM.PaymentDate = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConnectedEntityLineComponent.prototype, "CustomsTapagNumeral", {
        get: function () { return this.entityPM.CustomsTapagNumeral; },
        set: function (newValue) { this.entityPM.CustomsTapagNumeral = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConnectedEntityLineComponent.prototype, "TotalTax", {
        get: function () { return this.totalTax; },
        set: function (newValue) { this.totalTax = newValue; },
        enumerable: true,
        configurable: true
    });
    ConnectedEntityLineComponent.prototype.LoadDeficitConnectedFileParagraphTypes = function () {
        var _this = this;
        if (this.entityPM != null && this.deficitPM != null) {
            this.tapagMessagesService.GetDeficitConnectedFileParagraphTypeList(this.entityPM.Id, this.deficitPM.Id, this.deficitPM.Tenant)
                .subscribe(function (myResponse) {
                _this.GetDeclarationByTapagConnectionConnectionOp_Completed(myResponse, false);
            });
        }
    };
    ConnectedEntityLineComponent.prototype.GetDeclarationByTapagConnectionConnectionOp_Completed = function (myResponse, sourceIsCostomFile) {
        var _this = this;
        this.AmountsList = new ObservableCollection_1.ObservableCollection([]);
        if (myResponse.Result != null) {
            var total_1 = 0;
            myResponse.Result.forEach(function (item) {
                _this.AmountsList.Insert(item);
                total_1 = Number(total_1) + Number(item.Amount);
            });
            this.TotalTax = total_1.toString();
        }
    };
    return ConnectedEntityLineComponent;
}(BaseComponent_1.BaseComponent));
exports.ConnectedEntityLineComponent = ConnectedEntityLineComponent;
//# sourceMappingURL=PaymentOrderDeficitComponent.js.map