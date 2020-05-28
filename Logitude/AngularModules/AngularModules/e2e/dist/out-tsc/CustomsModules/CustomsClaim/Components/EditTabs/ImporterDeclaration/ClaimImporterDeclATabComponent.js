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
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var ClaimPM_1 = require("../../../../../Customs/EntityPMs/ClaimPM");
var ClaimImporterDeclarsPage3PM_1 = require("../../../../../Customs/EntityPMs/ClaimImporterDeclarsPage3PM");
var ClaimImporterDeclarsPage3APM_1 = require("../../../../../Customs/EntityPMs/ClaimImporterDeclarsPage3APM");
var ClaimImporterDeclarsP3LoiPM_1 = require("../../../../../Customs/EntityPMs/ClaimImporterDeclarsP3LoiPM");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var MessageWindow_1 = require("../../../../../Controls/Windows/MessageWindow");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var ClaimImporterDeclATabComponent = /** @class */ (function (_super) {
    __extends(ClaimImporterDeclATabComponent, _super);
    function ClaimImporterDeclATabComponent(entityArgs, EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityResourceService = EntityResourceService;
        _this.DataContext = _this;
        _this.EntityPM = new ClaimPM_1.ClaimPM();
        _this.ObjectTableName = "Customs.Claim";
        _this.isControlEnabled = true;
        _this.IsLoaded = false;
        _this._ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.ClaimImporterDeclAlist = new ObservableCollection_1.ObservableCollection([]);
        _this.CommercialSalelist = new ObservableCollection_1.ObservableCollection([]);
        _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        _this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(function (response) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.Claim").subscribe(function (response) {
                _this.EntityResourceService.getEntityResourceByTableName("Customs.ClaimImporterDeclarsP3Loi").subscribe(function (response) {
                    _this.EntityResourceService.getEntityResourceByTableName("Customs.ClaimImporterDeclarsPage3A").subscribe(function (response) {
                        _this.EntityResourceService.getEntityResourceByTableName("Customs.ClaimImporterDeclarsPage3").subscribe(function (response) {
                            if (_this.entityArgs.EntityPM != null) {
                                _this.EntityPM = _this.entityArgs.EntityPM;
                                _this.BuildImporterDeclareList();
                                _this.BuildCommercialSaleList();
                            }
                            _this.Listen();
                            _this.IsLoaded = true;
                        });
                    });
                });
            });
        });
        return _this;
    }
    ClaimImporterDeclATabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.BuildImporterDeclareList();
                    _this.BuildCommercialSaleList();
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.BuildImporterDeclareList();
                    _this.BuildCommercialSaleList();
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.TabSelected.subscribe(function (tabCode) {
                if (_this.CurrentEditComponentId == _this.CurrentSession.CurrentEditComponent.ComponentId) {
                    if (tabCode == "CLMA") {
                        _this.BuildImporterDeclareList();
                        _this.BuildCommercialSaleList();
                    }
                }
            }));
        }
    };
    ClaimImporterDeclATabComponent.prototype.RefreshEntity = function () {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    };
    ClaimImporterDeclATabComponent.prototype.getItemTitle = function (Item) {
        return Item.ImporterLoiDeclarationTypeCode + "," + Item.ImporterDeclarationTypeName;
    };
    Object.defineProperty(ClaimImporterDeclATabComponent.prototype, "SelectedTab", {
        get: function () { return this.selectedTab; },
        set: function (tab) {
            this.selectedTab = tab;
        },
        enumerable: true,
        configurable: true
    });
    ClaimImporterDeclATabComponent.prototype.SetTabArgs = function (args, valdationErrorList) {
        if (valdationErrorList === void 0) { valdationErrorList = null; }
        this.EntityPM = args.EntityPM;
        console.log("EntityPM", this.EntityPM);
    };
    Object.defineProperty(ClaimImporterDeclATabComponent.prototype, "IsControlEnabled", {
        get: function () { return this.isControlEnabled; },
        set: function (newValue) { this.isControlEnabled = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimImporterDeclATabComponent.prototype, "ValidationErrorsList", {
        get: function () { return this._ValidationErrorsList; },
        set: function (newValue) { this._ValidationErrorsList = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimImporterDeclATabComponent.prototype, "ImporterAffidavit", {
        get: function () { return this.EntityPM.ImporterAffidavit; },
        set: function (newValue) { this.EntityPM.ImporterAffidavit = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimImporterDeclATabComponent.prototype, "AccountCurrencyTypeCode", {
        get: function () { return this.EntityPM.AccountCurrencyTypeCode; },
        set: function (newValue) { this.EntityPM.AccountCurrencyTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimImporterDeclATabComponent.prototype, "BeneficiaryExternalID", {
        get: function () { return this.EntityPM.BeneficiaryExternalID; },
        set: function (newValue) { this.EntityPM.BeneficiaryExternalID = newValue; },
        enumerable: true,
        configurable: true
    });
    ClaimImporterDeclATabComponent.prototype.BuildImporterDeclareList = function () {
        this.ClaimImporterDeclAlist = new ObservableCollection_1.ObservableCollection([]);
        if (this.EntityPM.ClaimImporterDeclarsPage3 != null && this.EntityPM.ClaimImporterDeclarsPage3.length > 0) {
            for (var _i = 0, _a = this.EntityPM.ClaimImporterDeclarsPage3; _i < _a.length; _i++) {
                var item = _a[_i];
                this.ClaimImporterDeclAlist.Insert(new ClaimImporterDeclarsPage3LineComponent(item, this.EntityPM));
            }
        }
    };
    ClaimImporterDeclATabComponent.prototype.AddImporterDeclareCommand = function () {
        if (!this.IsControlEnabled)
            return;
        this.ValidationErrorCheck();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        var newClaimImporterDeclarsPage3PM = new ClaimImporterDeclarsPage3PM_1.ClaimImporterDeclarsPage3PM(this.EntityPM);
        newClaimImporterDeclarsPage3PM.Tenant = this.EntityPM.Tenant;
        newClaimImporterDeclarsPage3PM.ClaimId = this.EntityPM.Id;
        newClaimImporterDeclarsPage3PM.LineNo = (Tools_1.ArrayTool.Max(this.EntityPM.ClaimImporterDeclarsPage3, "LineNo") + 1);
        this.ClaimImporterDeclAlist.Insert(new ClaimImporterDeclarsPage3LineComponent(newClaimImporterDeclarsPage3PM, this.EntityPM));
        this.EntityPM.AddClaimImporterDeclarsPage3(newClaimImporterDeclarsPage3PM);
    };
    ClaimImporterDeclATabComponent.prototype.DeleteImporterDeclareCommand = function (item) {
        if (!this.IsControlEnabled)
            return;
        if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
            this.ClaimImporterDeclAlist.Remove(item);
            this.EntityPM.RemoveClaimImporterDeclarsPage3(item.entityPM);
        }
        this.ValidationErrorCheck();
    };
    ClaimImporterDeclATabComponent.prototype.MyDeclarationListkeyUp = function ($event, item) {
        //if (!isNaN(parseFloat(item.MyDeclarationList)) && isFinite(item.MyDeclarationList)) {
        //    return true;
        //}
        //if ((!AppTool.IsNullOrEmpty(item.MyDeclarationList) && isNaN(item.MyDeclarationList))
        //    || (AppTool.IsNullOrEmpty(item.MyDeclarationList) && $event.code.includes("Key") && isNaN($event.key))) {
        //    return false;
        //}
        if (!Tools_1.AppTool.IsNullOrEmpty(item.MyDeclarationList)) {
            if (isNaN(item.MyDeclarationList)) {
                return false;
            }
        }
        else {
            if ($event.code.includes("Key") && isNaN($event.key)) {
                return false;
            }
        }
        return true;
    };
    ClaimImporterDeclATabComponent.prototype.IsNumberKey = function (event) {
        var charCode = (event.which) ? event.which : event.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        return true;
    };
    ClaimImporterDeclATabComponent.prototype.EditImporterDeclareCommand = function (item) {
        var _this = this;
        if (!this.IsControlEnabled)
            return;
        if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
            this.ValidationErrorsList = [];
            if (Tools_1.AppTool.IsNullOrEmpty(item.ImporterLoiDeclarationTypeCode)) {
                this.ValidationErrorsList.push("חובה להזין הצהרת יבואן");
                return;
            }
            var windowArgs = {};
            windowArgs.ClaimImporterDeclarsP3Loilist = item.ClaimImporterDeclarsP3Loilist;
            var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
            logitudeWindow.Width = 350;
            logitudeWindow.Height = 400;
            logitudeWindow.IsShowCloseButton = false;
            logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Claim.g.CreateDecList2") + " " + item.ImporterLoiDeclarationTypeCode;
            logitudeWindow.WindowArgs = windowArgs;
            logitudeWindow.WindowClosed.subscribe(function ($event) { return _this.SelectionCompleted($event, item); });
            logitudeWindow.Show('./CustomsModules/CustomsClaim/Components/EditTabs/ImporterDeclaration/ClaimImporterDeclAP3LoisComponent');
        }
    };
    ClaimImporterDeclATabComponent.prototype.SelectionCompleted = function (msg, item) {
        if (msg != "Cancel") {
            if (item.entityPM.ClaimImporterDeclarsP3Loi != null && item.entityPM.ClaimImporterDeclarsP3Loi.length > 0) {
                item.entityPM.ClaimImporterDeclarsP3Loi.forEach(function (declarationitem) {
                    item.entityPM.RemoveClaimImporterDeclarsP3Loi(declarationitem);
                });
            }
            item.ClaimImporterDeclarsP3Loilist.Clear();
            item.entityPM.ClaimImporterDeclarsP3Loi = [];
            if (!Tools_1.AppTool.IsNullOrEmpty(msg)) {
                var listOfDeclarations = msg.split(",");
                for (var _i = 0, listOfDeclarations_1 = listOfDeclarations; _i < listOfDeclarations_1.length; _i++) {
                    var declarationNumber = listOfDeclarations_1[_i];
                    if (!Tools_1.AppTool.IsNullOrEmpty(declarationNumber) && declarationNumber != "null") {
                        item.AddNewImporterDeclarsP3Loi(declarationNumber);
                    }
                }
            }
            item.BuildClaimImporterDeclAP3LoisList();
        }
    };
    ClaimImporterDeclATabComponent.prototype.ValidationErrorCheck = function () {
        this.ValidationErrorsList = [];
        //Importer Declaration
        if (this.ClaimImporterDeclAlist != null && this.ClaimImporterDeclAlist.Length > 0) {
            var nullVM = this.ClaimImporterDeclAlist.Collection.filter(function (vm) { return vm.ImporterLoiDeclarationTypeCode == null; });
            if (nullVM.length > 0) {
                this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Claim.O.UseEmptyRow") + " (תצהיר יבואן)");
                return;
            }
        }
        if (this.ClaimImporterDeclAlist != null && this.ClaimImporterDeclAlist.Length >= 3) {
            this.ValidationErrorsList.push("לא ניתן להוסיף יותר מ 3 שורות לתצהיר היבואן");
            return;
        }
    };
    ClaimImporterDeclATabComponent.prototype.BuildCommercialSaleList = function () {
        this.CommercialSalelist = new ObservableCollection_1.ObservableCollection([]);
        if (this.EntityPM.ClaimImporterDeclarsPage3A != null && this.EntityPM.ClaimImporterDeclarsPage3A.length > 0) {
            for (var _i = 0, _a = this.EntityPM.ClaimImporterDeclarsPage3A; _i < _a.length; _i++) {
                var item = _a[_i];
                this.CommercialSalelist.Insert(new CommercialSaleComponent(item));
            }
        }
    };
    ClaimImporterDeclATabComponent.prototype.AddCommercialSaleCommand = function () {
        if (!this.IsControlEnabled)
            return;
        this.CommercialValidationErrorCheck();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        var newClaimImporterDeclarsPage3APM = new ClaimImporterDeclarsPage3APM_1.ClaimImporterDeclarsPage3APM(this.EntityPM);
        newClaimImporterDeclarsPage3APM.Tenant = this.EntityPM.Tenant;
        newClaimImporterDeclarsPage3APM.ClaimId = this.EntityPM.Id;
        newClaimImporterDeclarsPage3APM.LineNo = (Tools_1.ArrayTool.Max(this.EntityPM.ClaimImporterDeclarsPage3, "LineNo") + 1);
        this.CommercialSalelist.Insert(new CommercialSaleComponent(newClaimImporterDeclarsPage3APM));
        this.EntityPM.AddClaimImporterDeclarsPage3A(newClaimImporterDeclarsPage3APM);
    };
    ClaimImporterDeclATabComponent.prototype.DeleteCommercialSaleCommand = function (item) {
        if (!this.IsControlEnabled)
            return;
        if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
            this.CommercialSalelist.Remove(item);
            this.EntityPM.RemoveClaimImporterDeclarsPage3A(item.entityPM);
        }
        this.CommercialValidationErrorCheck();
    };
    ClaimImporterDeclATabComponent.prototype.CommercialValidationErrorCheck = function () {
        this.ValidationErrorsList = [];
        if (this.CommercialSalelist != null && this.CommercialSalelist.Length > 0) {
            var nullVM = this.CommercialSalelist.Collection.filter(function (vm) { return vm.CommercialSaleTypeCode == null; });
            if (nullVM.length > 0) {
                this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Claim.O.UseEmptyRow") + " (פרטי המישור המסחרי)");
                return;
            }
        }
        if (this.CommercialSalelist != null && this.CommercialSalelist.Length >= 4) {
            this.ValidationErrorsList.push("לא ניתן להוסיף יותר מ 4 שורות לפרטי המישור המסחרי");
            return;
        }
    };
    ClaimImporterDeclATabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ClaimImporterDeclATabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], ClaimImporterDeclATabComponent);
    return ClaimImporterDeclATabComponent;
}(BaseComponent_1.BaseComponent));
exports.ClaimImporterDeclATabComponent = ClaimImporterDeclATabComponent;
var ClaimImporterDeclarsPage3LineComponent = /** @class */ (function (_super) {
    __extends(ClaimImporterDeclarsPage3LineComponent, _super);
    function ClaimImporterDeclarsPage3LineComponent(entityPM, claimPM) {
        var _this = _super.call(this) || this;
        _this.entityPM = entityPM;
        _this.claimPM = claimPM;
        _this.ObjectTableName = "Customs.ClaimImporterDeclarsPage3";
        _this.DataContext = _this;
        _this.ClaimImporterDeclarsP3Loilist = new ObservableCollection_1.ObservableCollection([]);
        _this.BuildClaimImporterDeclAP3LoisList();
        return _this;
    }
    Object.defineProperty(ClaimImporterDeclarsPage3LineComponent.prototype, "ImporterLoiDeclarationTypeCode", {
        get: function () { return this.entityPM.ImporterLoiDeclarationTypeCode; },
        set: function (newValue) { this.entityPM.ImporterLoiDeclarationTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimImporterDeclarsPage3LineComponent.prototype, "ImporterDeclarationTypeName", {
        get: function () { return this.entityPM.ImporterDeclarationTypeName; },
        set: function (newValue) { this.entityPM.ImporterDeclarationTypeName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimImporterDeclarsPage3LineComponent.prototype, "MyDeclarationList", {
        get: function () { return this._MyDeclarationList; },
        set: function (newValue) {
            if (this._MyDeclarationList == "List") {
                return;
            }
            this._MyDeclarationList = newValue;
        },
        enumerable: true,
        configurable: true
    });
    ClaimImporterDeclarsPage3LineComponent.prototype.SetLocalName = function (entity, fieldName) {
        if (!Tools_1.AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        }
        else {
            this[fieldName] = null;
        }
    };
    ClaimImporterDeclarsPage3LineComponent.prototype.BuildClaimImporterDeclAP3LoisList = function () {
        this.ClaimImporterDeclarsP3Loilist = new ObservableCollection_1.ObservableCollection([]);
        this._MyDeclarationList = "";
        if (this.entityPM.ClaimImporterDeclarsP3Loi != null) {
            for (var _i = 0, _a = this.entityPM.ClaimImporterDeclarsP3Loi; _i < _a.length; _i++) {
                var item = _a[_i];
                this.ClaimImporterDeclarsP3Loilist.Insert(item.DeclarationNumber);
            }
            if (this.entityPM.ClaimImporterDeclarsP3Loi.length == 1) {
                this.MyDeclarationList = this.entityPM.ClaimImporterDeclarsP3Loi[0].DeclarationNumber;
                this.UIProperties.SetEnabled("MyDeclarationList", null, false);
            }
            else if (this.entityPM.ClaimImporterDeclarsP3Loi.length > 1) {
                this.MyDeclarationList = "List";
                this.UIProperties.SetEnabled("MyDeclarationList", null, true);
            }
        }
    };
    ClaimImporterDeclarsPage3LineComponent.prototype.ImporterDeclarationLostFocus = function (event) {
        if (Tools_1.AppTool.IsNullOrEmpty(this.ImporterLoiDeclarationTypeCode)
            || this.MyDeclarationList == "List") {
            return;
        }
        if (isNaN(this.MyDeclarationList)) {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Warning");
            messageWindow.Width = 250;
            messageWindow.Height = 150;
            messageWindow.OkButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
            messageWindow.Show("יש להזין מספר הצהרה");
            this.MyDeclarationList = "";
            return;
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.MyDeclarationList) && this.ClaimImporterDeclarsP3Loilist != null && this.ClaimImporterDeclarsP3Loilist.Length == 1) {
            this.ClaimImporterDeclarsP3Loilist.Clear();
            this.entityPM.RemoveClaimImporterDeclarsP3Loi(this.entityPM.ClaimImporterDeclarsP3Loi[0]);
            return;
        }
        this.ClaimImporterDeclarsP3Loilist.Clear();
        if (this.entityPM.ClaimImporterDeclarsP3Loi == null || this.entityPM.ClaimImporterDeclarsP3Loi.length == 0) {
            this.AddNewImporterDeclarsP3Loi(this.MyDeclarationList);
        }
        else {
            this.entityPM.ClaimImporterDeclarsP3Loi[0].DeclarationNumber = this.MyDeclarationList;
        }
        this.ClaimImporterDeclarsP3Loilist.Insert(this.MyDeclarationList);
    };
    ClaimImporterDeclarsPage3LineComponent.prototype.AddNewImporterDeclarsP3Loi = function (declarationNumber) {
        var newClaimImporterDeclarsP3LoiPM = new ClaimImporterDeclarsP3LoiPM_1.ClaimImporterDeclarsP3LoiPM(this.EntityPM);
        newClaimImporterDeclarsP3LoiPM.Tenant = this.entityPM.Tenant;
        newClaimImporterDeclarsP3LoiPM.ClaimId = this.entityPM.ClaimId;
        newClaimImporterDeclarsP3LoiPM.CounterKey = this.entityPM.LineNo;
        newClaimImporterDeclarsP3LoiPM.LineNo = (Tools_1.ArrayTool.Max(this.entityPM.ClaimImporterDeclarsP3Loi, "LineNo") + 1);
        if (!Tools_1.AppTool.IsNullOrEmpty(declarationNumber)) {
            newClaimImporterDeclarsP3LoiPM.DeclarationNumber = declarationNumber;
        }
        this.entityPM.AddClaimImporterDeclarsP3Loi(newClaimImporterDeclarsP3LoiPM);
    };
    return ClaimImporterDeclarsPage3LineComponent;
}(BaseComponent_1.BaseComponent));
exports.ClaimImporterDeclarsPage3LineComponent = ClaimImporterDeclarsPage3LineComponent;
var CommercialSaleComponent = /** @class */ (function (_super) {
    __extends(CommercialSaleComponent, _super);
    function CommercialSaleComponent(entityPM) {
        var _this = _super.call(this) || this;
        _this.entityPM = entityPM;
        _this.ObjectTableName = "Customs.ClaimImporterDeclarsPage3A";
        _this.DataContext = _this;
        return _this;
    }
    Object.defineProperty(CommercialSaleComponent.prototype, "CommercialSaleTypeCode", {
        get: function () { return this.entityPM.CommercialSaleTypeCode; },
        set: function (newValue) { this.entityPM.CommercialSaleTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CommercialSaleComponent.prototype, "CommercialSaleTypeName", {
        get: function () { return this.entityPM.CommercialSaleTypeName; },
        set: function (newValue) { this.entityPM.CommercialSaleTypeName = newValue; },
        enumerable: true,
        configurable: true
    });
    CommercialSaleComponent.prototype.SetLocalName = function (entity, fieldName) {
        if (!Tools_1.AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        }
        else {
            this[fieldName] = null;
        }
    };
    return CommercialSaleComponent;
}(BaseComponent_1.BaseComponent));
exports.CommercialSaleComponent = CommercialSaleComponent;
//# sourceMappingURL=ClaimImporterDeclATabComponent.js.map