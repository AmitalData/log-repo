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
var ClaimImporterDeclarsP3LoiPM_1 = require("../../../../../Customs/EntityPMs/ClaimImporterDeclarsP3LoiPM");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var ClaimImporterDeclAP3LoisComponent = /** @class */ (function (_super) {
    __extends(ClaimImporterDeclAP3LoisComponent, _super);
    function ClaimImporterDeclAP3LoisComponent(entityArgs, EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityResourceService = EntityResourceService;
        _this.DataContext = _this;
        _this.EntityPM = new ClaimImporterDeclarsP3LoiPM_1.ClaimImporterDeclarsP3LoiPM(null);
        _this.ObjectTableName = "Customs.ClaimImporterDeclarsP3Loi";
        _this.isControlEnabled = true;
        _this.ValidationErrors = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.ValidationErrors = [];
        _this.ClaimImporterDeclarsP3Loilist = new ObservableCollection_1.ObservableCollection([]);
        _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        _this.EntityResourceService.getEntityResourceByTableName("Customs.ClaimImporterDeclarsP3Loi").subscribe(function (response) { });
        return _this;
    }
    ClaimImporterDeclAP3LoisComponent.prototype.SetWindowArgs = function (args) {
        //this.ClaimImporterDeclarsP3Loilist = args.ClaimImporterDeclarsP3Loilist;
        if (args.ClaimImporterDeclarsP3Loilist != null && args.ClaimImporterDeclarsP3Loilist.Collection.length > 0) {
            for (var _i = 0, _a = args.ClaimImporterDeclarsP3Loilist.Collection; _i < _a.length; _i++) {
                var item = _a[_i];
                this.ClaimImporterDeclarsP3Loilist.Insert(new DeclarationNumberComponent(item));
            }
        }
    };
    ClaimImporterDeclAP3LoisComponent.prototype.RefreshEntity = function () {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    };
    Object.defineProperty(ClaimImporterDeclAP3LoisComponent.prototype, "SelectedTab", {
        get: function () { return this.selectedTab; },
        set: function (tab) {
            this.selectedTab = tab;
        },
        enumerable: true,
        configurable: true
    });
    ClaimImporterDeclAP3LoisComponent.prototype.SetTabArgs = function (args, valdationErrorList) {
        if (valdationErrorList === void 0) { valdationErrorList = null; }
        this.EntityPM = args.EntityPM;
        console.log("EntityPM", this.EntityPM);
    };
    Object.defineProperty(ClaimImporterDeclAP3LoisComponent.prototype, "IsControlEnabled", {
        get: function () { return this.isControlEnabled; },
        set: function (newValue) { this.isControlEnabled = newValue; },
        enumerable: true,
        configurable: true
    });
    ClaimImporterDeclAP3LoisComponent.prototype.AddNewImporterDeclarsP3LoiCommand = function () {
        if (!this.IsControlEnabled)
            return;
        this.ClaimImporterDeclarsP3Loilist.Insert(new DeclarationNumberComponent(""));
    };
    ClaimImporterDeclAP3LoisComponent.prototype.DeleteImporterDeclarsP3LoiCommand = function (item) {
        if (!this.IsControlEnabled)
            return;
        if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
            this.ClaimImporterDeclarsP3Loilist.Remove(item);
        }
    };
    ClaimImporterDeclAP3LoisComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    };
    ClaimImporterDeclAP3LoisComponent.prototype.OkButtonClicked = function () {
        var listOfDeclarations = null;
        for (var _i = 0, _a = this.ClaimImporterDeclarsP3Loilist.Collection; _i < _a.length; _i++) {
            var item = _a[_i];
            if (!Tools_1.AppTool.IsNullOrEmpty(item.DeclarationNumber)) {
                listOfDeclarations = listOfDeclarations + "," + item.DeclarationNumber;
            }
        }
        this.CurrentSession.CloseCurrentWindowEmit(listOfDeclarations);
    };
    ClaimImporterDeclAP3LoisComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ClaimImporterDeclAP3LoisComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], ClaimImporterDeclAP3LoisComponent);
    return ClaimImporterDeclAP3LoisComponent;
}(BaseComponent_1.BaseComponent));
exports.ClaimImporterDeclAP3LoisComponent = ClaimImporterDeclAP3LoisComponent;
var DeclarationNumberComponent = /** @class */ (function (_super) {
    __extends(DeclarationNumberComponent, _super);
    function DeclarationNumberComponent(declarationNumber) {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this._DeclarationNumber = declarationNumber;
        return _this;
    }
    Object.defineProperty(DeclarationNumberComponent.prototype, "DeclarationNumber", {
        get: function () { return this._DeclarationNumber; },
        set: function (newValue) { this._DeclarationNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    return DeclarationNumberComponent;
}(BaseComponent_1.BaseComponent));
exports.DeclarationNumberComponent = DeclarationNumberComponent;
//# sourceMappingURL=ClaimImporterDeclAP3LoisComponent.js.map