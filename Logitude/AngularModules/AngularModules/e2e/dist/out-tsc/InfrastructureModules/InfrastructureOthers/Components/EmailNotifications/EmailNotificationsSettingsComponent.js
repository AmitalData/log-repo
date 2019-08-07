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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var SessionInfo_1 = require("../../../../Infrastructure/Utilities/SessionInfo");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var EmailAlertSettingPMService_1 = require("../../../../Infrastructure/Services/ExtendedPMs/EmailAlertSettingPMService");
var UIProperties_1 = require("../../../../Infrastructure/Components/LogitudeComponents/UIProperties");
var Tools_1 = require("../../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var EmailNotificationsSettingsComponent = /** @class */ (function (_super) {
    __extends(EmailNotificationsSettingsComponent, _super);
    function EmailNotificationsSettingsComponent(entityResourceService, emailAlertSettingPMService) {
        var _this = _super.call(this) || this;
        _this.entityResourceService = entityResourceService;
        _this.emailAlertSettingPMService = emailAlertSettingPMService;
        //public EntityPM: EmailAlertSettingPM;
        _this.DataContext = _this;
        _this.ObjectTableName = "EmailAlertSetting";
        _this.IsResourcesReady = false;
        _this.OwnerAlerts = [];
        _this.GeneralAlerts = [];
        _this.OnCloseSendToContactsEvent = new core_1.EventEmitter();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsCloseSendToContact = false;
        _this.PartnersObslist = [];
        _this.InitializeServices();
        _this.LoadData();
        return _this;
    }
    EmailNotificationsSettingsComponent.prototype.LoadData = function () {
        var _this = this;
        this.emailAlertSettingPMService.getAllEmailAlerts(SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (response) {
            if (!response.HasError) {
                _this.AllAlerts = response.Result;
                var ownerArr = _this.AllAlerts.filter(function (a) { return a.SettingLevelCode == "OWNR"; });
                for (var k in ownerArr) {
                    _this.OwnerAlerts.push(new EmailAlertSettingDataViewModel(ownerArr[k]));
                }
                var generalArr = _this.AllAlerts.filter(function (a) { return a.SettingLevelCode == "GNRL"; });
                for (var k in generalArr) {
                    _this.GeneralAlerts.push(new EmailAlertSettingDataViewModel(generalArr[k]));
                }
                _this.IsResourcesReady = true;
            }
        });
    };
    //private myTenantPMService: TenantPMService;
    // private myAccountingSettingPMService: AccountingSettingPMService;
    EmailNotificationsSettingsComponent.prototype.InitializeServices = function () {
        //  this.myTenantPMService = new TenantPMService();
        // this.myAccountingSettingPMService = new AccountingSettingPMService();
    };
    //Commands 
    EmailNotificationsSettingsComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    EmailNotificationsSettingsComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        for (var k in this.GeneralAlerts) {
            var alert = this.GeneralAlerts[k];
            if (alert.IsActive && Tools_1.AppTool.IsNullOrEmpty(alert.To)) {
                this.ValidationErrorsList.push(alert.Description + " must have destination contacts emails");
            }
        }
        if (this.ValidationErrorsList.length > 0)
            return;
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
        this.emailAlertSettingPMService.updateAllAlerts(this.AllAlerts, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (response) {
            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            if (response.HasError) {
                _this.ValidationErrorsList = response.ErrorsArray;
            }
            else {
                _this.CurrentSession.CloseCurrentWindow();
            }
        });
        //if (this.EntityPM.IsDirty) {
        //  }
    };
    EmailNotificationsSettingsComponent.prototype.ShowSendToEmail = function (item) {
        var _this = this;
        //this.PartnersObslist.push(new EntityPartner("All", "1", false))
        this.IsCloseSendToContact = false;
        this.OnCloseSendToContactsEvent.subscribe(function ($event) {
            if (!_this.IsCloseSendToContact && $event) {
                _this.IsCloseSendToContact = true;
                _this.ToEmail = "";
                if ($event.ToEmailLists && $event.ToEmailLists.length > 0) {
                    $event.ToEmailLists.forEach(function (item) {
                        _this.ToEmail += item + ";";
                    });
                    item.To = _this.ToEmail;
                }
                else
                    item.To = null;
            }
        });
        var windowArgs = {};
        windowArgs.PartnersObslist = this.PartnersObslist;
        windowArgs.ToEmail = item.To; //this.ToEmail;
        windowArgs.Cc = "";
        windowArgs.Bcc = "";
        windowArgs.HideCC = true;
        windowArgs.HideBCC = true;
        //windowArgs.EntityId = item.Id;
        //windowArgs.ObjectTableId = this.ObjectTableId;
        windowArgs.OnCloseSendToContactsEvent = this.OnCloseSendToContactsEvent;
        windowArgs.ObjectTableName = "User";
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Users List";
        logWindow.Width = window.innerWidth - 100;
        logWindow.Height = window.innerHeight - 100;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/SendMessageContacts/SendToContactsComponent");
        logWindow.WindowClosed.subscribe(function ($event) {
        });
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], EmailNotificationsSettingsComponent.prototype, "OnCloseSendToContactsEvent", void 0);
    EmailNotificationsSettingsComponent = __decorate([
        core_1.Component({
            selector: 'EmailNotificationsSettingsComponent',
            moduleId: module.id,
            templateUrl: './EmailNotificationsSettingsComponent.html',
            providers: [EmailAlertSettingPMService_1.EmailAlertSettingPMService],
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService, EmailAlertSettingPMService_1.EmailAlertSettingPMService])
    ], EmailNotificationsSettingsComponent);
    return EmailNotificationsSettingsComponent;
}(BaseComponent_1.BaseComponent));
exports.EmailNotificationsSettingsComponent = EmailNotificationsSettingsComponent;
var EmailAlertSettingDataViewModel = /** @class */ (function () {
    function EmailAlertSettingDataViewModel(settingpm) {
        this.SettingPM = settingpm;
        this.UIProperties = new UIProperties_1.UIProperties;
    }
    Object.defineProperty(EmailAlertSettingDataViewModel.prototype, "Description", {
        get: function () { return this.SettingPM.Description; },
        set: function (newValue) { this.SettingPM.Description = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EmailAlertSettingDataViewModel.prototype, "To", {
        get: function () { return this.SettingPM.To; },
        set: function (newValue) { this.SettingPM.To = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EmailAlertSettingDataViewModel.prototype, "IsActive", {
        get: function () { return !this.SettingPM.InActive; },
        set: function (newValue) { this.SettingPM.InActive = !newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EmailAlertSettingDataViewModel.prototype, "Code", {
        get: function () { return this.SettingPM.Code; },
        set: function (newValue) { this.SettingPM.Code = newValue; },
        enumerable: true,
        configurable: true
    });
    return EmailAlertSettingDataViewModel;
}());
exports.EmailAlertSettingDataViewModel = EmailAlertSettingDataViewModel;
//# sourceMappingURL=EmailNotificationsSettingsComponent.js.map