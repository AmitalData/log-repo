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
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var GuaranteePM_1 = require("../../../../../Customs/EntityPMs/GuaranteePM");
var TapagList_1 = require("../../../../../Customs/EntityLists/TapagList");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
var TapagMessagesService_1 = require("../../../../../Customs/Services/WebServices/TapagMessagesService");
var DeclarationWebService_1 = require("../../../../../Customs/Services/WebServices/DeclarationWebService");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var GuaranteeDataComponent = /** @class */ (function (_super) {
    __extends(GuaranteeDataComponent, _super);
    function GuaranteeDataComponent(entityArgs, EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityResourceService = EntityResourceService;
        _this.EntityPM = new GuaranteePM_1.GuaranteePM();
        _this.ObjectTableName = "Customs.Guarantee";
        _this.DataContext = _this;
        _this.Tapag = new TapagList_1.TapagList();
        _this.tapagMessagesService = new TapagMessagesService_1.TapagMessagesService();
        _this.declarationWebService = new DeclarationWebService_1.DeclarationWebService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.RequiredGuaranteeTypes = new ObservableCollection_1.ObservableCollection([]);
        _this.GuaranteeConditions = new ObservableCollection_1.ObservableCollection([]);
        _this.ConnectedEntitiesList = new ObservableCollection_1.ObservableCollection([]);
        if (!Tools_1.AppTool.IsNullOrEmpty(entityArgs)) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.Tapag").subscribe(function (response) {
                _this.EntityResourceService.getEntityResourceByTableName("Customs.TapagConnectionTable").subscribe(function (response) {
                    _this.EntityResourceService.getEntityResourceByTableName("Customs.Guarantee").subscribe(function (response) {
                        _this.EntityResourceService.getEntityResourceByTableName("Customs.GuaranteeCondition").subscribe(function (response) {
                            _this.EntityResourceService.getEntityResourceByTableName("Customs.RequiredGuaranteeType").subscribe(function (response) {
                                _this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrder").subscribe(function (response) {
                                    _this.Listen();
                                });
                            });
                        });
                    });
                });
            });
        }
        return _this;
    }
    GuaranteeDataComponent.prototype.Listen = function () {
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
                    if (tabCode == "PODP") {
                    }
                }
            }));
        }
    };
    GuaranteeDataComponent.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            this.Tapag = args.EntityPM;
            this.LoadGuaranteeData();
        }
    };
    GuaranteeDataComponent.prototype.LoadGuaranteeData = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.Tapag.Id)) {
            this.tapagMessagesService.GetGuaranteeByTapagId(this.Tapag.Id, this.Tapag.Tenant)
                .subscribe(function (myResponse) {
                _this.GetGuaranteeByTapagIdOp_Completed(myResponse, false);
            });
        }
    };
    GuaranteeDataComponent.prototype.GetGuaranteeByTapagIdOp_Completed = function (myResponse, sourceIsCostomFile) {
        if (myResponse.Result != null) {
            this.EntityPM = myResponse.Result;
            this.FillGuaranteeDataList();
        }
    };
    GuaranteeDataComponent.prototype.FillGuaranteeDataList = function () {
        var _this = this;
        this.RequiredGuaranteeTypes = new ObservableCollection_1.ObservableCollection([]);
        this.GuaranteeConditions = new ObservableCollection_1.ObservableCollection([]);
        if (this.EntityPM != null && this.EntityPM.RequiredGuaranteeTypes.length > 0) {
            this.EntityPM.RequiredGuaranteeTypes.forEach(function (requiredGuaranteeTypePM) {
                _this.RequiredGuaranteeTypes.Insert(requiredGuaranteeTypePM);
            });
        }
        if (this.EntityPM != null && this.EntityPM.GuaranteeConditions.length > 0) {
            this.EntityPM.GuaranteeConditions.forEach(function (guaranteeConditionPM) {
                _this.GuaranteeConditions.Insert(guaranteeConditionPM);
            });
        }
        this.GetConnectedDeclarations();
    };
    GuaranteeDataComponent.prototype.GetConnectedDeclarations = function () {
        var _this = this;
        if (this.Tapag != null) {
            this.EntityPM.LeadingFileNumber = this.Tapag.LeadingFileNumber;
            this.declarationWebService.GetDeclarationByTapagConnectionConnection(this.Tapag.Id)
                .subscribe(function (myResponse) {
                _this.GetDeclarationByTapagConnectionConnectionOp_Completed(myResponse, false);
            });
        }
    };
    GuaranteeDataComponent.prototype.GetDeclarationByTapagConnectionConnectionOp_Completed = function (myResponse, sourceIsCostomFile) {
        var _this = this;
        this.ConnectedEntitiesList = new ObservableCollection_1.ObservableCollection([]);
        if (myResponse.Result != null) {
            myResponse.Result.forEach(function (declarationList) {
                _this.ConnectedEntitiesList.Insert(declarationList);
            });
        }
    };
    Object.defineProperty(GuaranteeDataComponent.prototype, "LeadingFileNumber", {
        get: function () { return this.EntityPM.LeadingFileNumber; },
        set: function (newValue) { this.EntityPM.LeadingFileNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeDataComponent.prototype, "CustomerName", {
        get: function () { return this.Tapag.CustomerName; },
        set: function (newValue) { this.Tapag.CustomerName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeDataComponent.prototype, "CustomerId", {
        get: function () { return this.Tapag.CustomerId; },
        set: function (newValue) { this.Tapag.CustomerId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeDataComponent.prototype, "ImporterId", {
        get: function () { return this.Tapag.ImporterId; },
        set: function (newValue) { this.Tapag.ImporterId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeDataComponent.prototype, "ImporterName", {
        get: function () { return this.EntityPM.ImporterName; },
        set: function (newValue) { this.EntityPM.ImporterName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeDataComponent.prototype, "ClientActivityCode", {
        get: function () { return this.EntityPM.ClientActivityCode; },
        set: function (newValue) { this.EntityPM.ClientActivityCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeDataComponent.prototype, "GuaranteeRequestStatusCode", {
        get: function () { return this.EntityPM.GuaranteeRequestStatusCode; },
        set: function (newValue) { this.EntityPM.GuaranteeRequestStatusCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeDataComponent.prototype, "GuaranteeRequestNumber", {
        get: function () { return this.EntityPM.GuaranteeRequestNumber; },
        set: function (newValue) { this.EntityPM.GuaranteeRequestNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeDataComponent.prototype, "CustomsBranchCode", {
        get: function () { return this.Tapag.CustomsBranchCode; },
        set: function (newValue) { this.Tapag.CustomsBranchCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeDataComponent.prototype, "BrandNumber", {
        get: function () { return this.EntityPM.BrandNumber; },
        set: function (newValue) { this.EntityPM.BrandNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeDataComponent.prototype, "ProfessionUnitTypeCode", {
        get: function () { return this.Tapag.ProfessionUnitTypeCode; },
        set: function (newValue) { this.Tapag.ProfessionUnitTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeDataComponent.prototype, "LawyerNumber", {
        get: function () { return this.EntityPM.LawyerNumber; },
        set: function (newValue) { this.EntityPM.LawyerNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeDataComponent.prototype, "CustomEntityTypeCode", {
        get: function () { return this.EntityPM.CustomEntityTypeCode; },
        set: function (newValue) { this.EntityPM.CustomEntityTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeDataComponent.prototype, "VehicleChassisNumber", {
        get: function () { return this.EntityPM.VehicleChassisNumber; },
        set: function (newValue) { this.EntityPM.VehicleChassisNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeDataComponent.prototype, "CustomEntityNumber", {
        get: function () { return this.EntityPM.CustomEntityNumber; },
        set: function (newValue) { this.EntityPM.CustomEntityNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeDataComponent.prototype, "EngineNumber", {
        get: function () { return this.EntityPM.EngineNumber; },
        set: function (newValue) { this.EntityPM.EngineNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeDataComponent.prototype, "MsgID", {
        get: function () { return this.EntityPM.MsgID; },
        set: function (newValue) { this.EntityPM.MsgID = newValue; },
        enumerable: true,
        configurable: true
    });
    GuaranteeDataComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    };
    GuaranteeDataComponent.prototype.GuaranteeReturnRequestClicked = function () {
        //GuaranteeReturnRequestViewModel viewModel = new GuaranteeReturnRequestViewModel(tapag);
        //GuaranteeReturnRequestControl control = new GuaranteeReturnRequestControl() { DataContext = viewModel };
        //simplogWindow.Height = 900;
        //simplogWindow.Width = 1000;
        //simplogWindow.Add(control);
        //simplogWindow.ShowCloseButtonOnly = true;
        //simplogWindow.Show();
    };
    GuaranteeDataComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './GuaranteeDataComponent.html',
            selector: 'GuaranteeDataComponent',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], GuaranteeDataComponent);
    return GuaranteeDataComponent;
}(BaseComponent_1.BaseComponent));
exports.GuaranteeDataComponent = GuaranteeDataComponent;
//# sourceMappingURL=GuaranteeDataComponent.js.map