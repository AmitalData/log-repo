"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require('@angular/core');
var BaseComponent_1 = require('../../../../Infrastructure/Components/LogitudeComponents/BaseComponent');
var EntityArgs_1 = require('../../../../Infrastructure/DataContracts/EntityArgs');
var ObservableCollection_1 = require('../../../../Infrastructure/Utilities/ObservableCollection');
var JournalExtendedListService_1 = require('../../../Services/ExtendedLists/JournalExtendedListService');
var EntityResourceService_1 = require('../../../../Infrastructure/Services/EntityResourceService');
var LogitudeWindow_1 = require('../../../../Controls/Windows/LogitudeWindow');
var RevaluationDetailsComponent = (function (_super) {
    __extends(RevaluationDetailsComponent, _super);
    function RevaluationDetailsComponent(entityArgs) {
        var _this = this;
        _super.call(this);
        this.entityArgs = entityArgs;
        this.DataContext = this;
        this.ObjectTableName = "Revaluation";
        this.EntityPM = null;
        this.journalExtendedListService = new JournalExtendedListService_1.JournalExtendedListService();
        this.entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.SelectedRow = null;
        this.entityResourceService.getEntityResourceByTableName("Journal").subscribe(function (response) {
            _this.entityResourceService.getEntityResourceByTableName("JournalLine").subscribe(function (response) {
                _this.visible = true;
                _this.EntityPM = entityArgs.EntityPM;
                _this.Journals = new ObservableCollection_1.ObservableCollection([]);
                _this.JournalLines = new ObservableCollection_1.ObservableCollection([]);
                _this.GetJournals();
                _this.UIProperties.SetEnabled("GLAccountId", "Revaluation", false);
                _this.UIProperties.SetEnabled("Message", "Revaluation", false);
                _this.UIProperties.SetEnabled("RevaluationNumber", "Revaluation", false);
                _this.UIProperties.SetEnabled("ChartOfAccountsId", "Revaluation", false);
                _this.UIProperties.SetEnabled("Status", "Revaluation", false);
                _this.UIProperties.SetEnabled("RevaluationDate", "Revaluation", false);
                _this.UIProperties.SetEnabled("RevaluationEnabled", "Revaluation", false);
            });
        });
    }
    Object.defineProperty(RevaluationDetailsComponent.prototype, "GLAccountId", {
        get: function () { if (this.EntityPM)
            return this.EntityPM.GLAccountId;
        else
            return null; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RevaluationDetailsComponent.prototype, "Message", {
        get: function () { if (this.EntityPM)
            return this.EntityPM.Message;
        else
            return null; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RevaluationDetailsComponent.prototype, "RevaluationNumber", {
        get: function () { if (this.EntityPM)
            return this.EntityPM.RevaluationNumber;
        else
            return null; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RevaluationDetailsComponent.prototype, "ChartOfAccountsId", {
        get: function () { if (this.EntityPM)
            return this.EntityPM.ChartOfAccountsId;
        else
            return null; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RevaluationDetailsComponent.prototype, "Status", {
        get: function () { if (this.EntityPM)
            return this.EntityPM.Status;
        else
            return null; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RevaluationDetailsComponent.prototype, "RevaluationDate", {
        get: function () { if (this.EntityPM)
            return this.EntityPM.RevaluationDate;
        else
            return null; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RevaluationDetailsComponent.prototype, "RevaluationEnabled", {
        get: function () { if (this.EntityPM)
            return this.EntityPM.RevaluationEnabled;
        else
            return null; },
        enumerable: true,
        configurable: true
    });
    RevaluationDetailsComponent.prototype.GetJournals = function () {
        var _this = this;
        this.journalExtendedListService.GetJournalsByAccountingEntityId(this.EntityPM.Id).subscribe(function (myResponse) {
            if (myResponse) {
                if (myResponse.Result) {
                    _this.Journals.InsertCollection(myResponse.Result);
                }
            }
        });
    };
    RevaluationDetailsComponent.prototype.JournalHyperlinkClicked = function (item) {
        this.EditEntity("Journal", item.Id, null, "JNDT");
    };
    RevaluationDetailsComponent.prototype.EditEntity = function (objectTableName, entityId, windowTitle, defaultSelectedTabCode) {
        var editWindow = new LogitudeWindow_1.LogitudeWindow();
        editWindow.ShowHeaderButtons = true;
        editWindow.Title = windowTitle;
        editWindow.Height = 770;
        editWindow.Width = 1500;
        editWindow.ShowEditComponent(entityId, objectTableName, defaultSelectedTabCode);
        editWindow.WindowClosed.subscribe(function (res) {
        });
    };
    RevaluationDetailsComponent.prototype.OnRowSelected = function (item) {
        //   GetJournalLinesByJournalId
        var _this = this;
        this.journalExtendedListService.GetJournalLinesByJournalId(item.Id).subscribe(function (myResponse) {
            if (myResponse) {
                if (myResponse.Result) {
                    _this.JournalLines.InsertCollection(myResponse.Result);
                }
            }
        });
    };
    RevaluationDetailsComponent = __decorate([
        core_1.Component({
            moduleId: './Accounting/Components/EditTabs/Revaluation/',
            templateUrl: 'RevaluationDetailsComponent.html',
        }), 
        __metadata('design:paramtypes', [EntityArgs_1.EntityArgs])
    ], RevaluationDetailsComponent);
    return RevaluationDetailsComponent;
}(BaseComponent_1.BaseComponent));
exports.RevaluationDetailsComponent = RevaluationDetailsComponent;
//# sourceMappingURL=RevaluationDetailsComponent.js.map