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
var AccountingNotePMService_1 = require("./../../Services/StandardPMs/AccountingNotePMService");
var core_1 = require("@angular/core");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var AccountingNotePM_1 = require("../../EntityPMs/AccountingNotePM");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var AccountingNoteComponent = /** @class */ (function (_super) {
    __extends(AccountingNoteComponent, _super);
    function AccountingNoteComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "AccountingNote";
        _this.ValidationErrorsList = [];
        _this.isRTL = false;
        _this.isEditForm = false;
        _this._AccountingNotePMService = new AccountingNotePMService_1.AccountingNotePMService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        return _this;
    }
    AccountingNoteComponent.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            this.entityPM = args.AccountingNotePM;
            this.accountPM = args.AccountPM;
            if (this.entityPM) {
                this.isEditForm = true;
            }
            else {
                // new
                this.entityPM = new AccountingNotePM_1.AccountingNotePM();
                var loggedContact = SessionLocator_1.SessionLocator.LoggedUserPM;
                if (this.accountPM.CardId) {
                    this.entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                    this.entityPM.CardId = this.accountPM.CardId;
                    this.entityPM.CreateDate = new Date();
                    this.entityPM.UpdateDate = new Date();
                    this.entityPM.CreatedByUserId = loggedContact.Id;
                    this.entityPM.UpdatedByUserId = loggedContact.Id;
                    this.entityPM.UpdatedByUserName = loggedContact.DontShowLocal ? loggedContact.EnglishName : (loggedContact.LocalName || loggedContact.EnglishName);
                    this.entityPM.CreatedByUserName = loggedContact.DontShowLocal ? loggedContact.EnglishName : (loggedContact.LocalName || loggedContact.EnglishName);
                }
                else {
                    this.ValidationErrorsList = [];
                    this.ValidationErrorsList.push("No card id in selected gl account!!!!!!!!!!");
                    return;
                }
            }
        }
    };
    AccountingNoteComponent.prototype.SetUIProperty = function () {
        this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, true);
    };
    Object.defineProperty(AccountingNoteComponent.prototype, "Notes", {
        //#region Properties
        get: function () {
            return this.entityPM.Notes;
        },
        set: function (v) {
            this.entityPM.Notes = v;
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    //#region Buttons Handlers
    AccountingNoteComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        if (this.isEditForm) {
            this.CurrentSession.StartBusyIndicatorSaving();
            this._AccountingNotePMService.update(this.entityPM).subscribe(function (myResult) {
                var mm = myResult;
                if (!mm.HasError) {
                    _this.CurrentSession.StopBusyIndicator();
                    _this.CurrentSession.CloseCurrentWindow();
                }
                else {
                    _this.ValidationErrorsList = mm.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
            });
        }
        else {
            this.CurrentSession.StartBusyIndicatorSaving();
            this._AccountingNotePMService.insert(this.entityPM).subscribe(function (myResult) {
                var mm = myResult;
                if (!mm.HasError) {
                    _this.CurrentSession.StopBusyIndicator();
                    _this.CurrentSession.CloseCurrentWindow();
                }
                else {
                    _this.ValidationErrorsList = mm.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
            });
        }
    };
    AccountingNoteComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AccountingNoteComponent = __decorate([
        core_1.Component({
            selector: 'AccountingNoteComponent',
            moduleId: './Accounting/Components/Others/',
            templateUrl: 'AccountingNoteComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AccountingNoteComponent);
    return AccountingNoteComponent;
}(BaseComponent_1.BaseComponent));
exports.AccountingNoteComponent = AccountingNoteComponent;
//# sourceMappingURL=AccountingNoteComponent.js.map