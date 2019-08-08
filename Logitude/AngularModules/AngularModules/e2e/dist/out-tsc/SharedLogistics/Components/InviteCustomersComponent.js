"use strict";
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
/// <reference path="../../controls/windows/messagewindow.ts" />
var core_1 = require("@angular/core");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var SharedLogisticContactService_1 = require("../Services/ExtendedPMs/SharedLogisticContactService");
var SessionInfo_1 = require("../../Infrastructure/Utilities/SessionInfo");
var CustomerLineViewModel_1 = require("./ViewModel/CustomerLineViewModel");
var LogitudeWindow_1 = require("../../Controls/Windows/LogitudeWindow");
var ContactsTabComponent_1 = require("../../CommonModules/CommonPartners/Components/EditTabs/ContactsTabComponent");
var MessageWindow_1 = require("../../Controls/Windows/MessageWindow");
var ContactPM_1 = require("../../Common/EntityPMs/ContactPM");
var EntityResourceService_1 = require("../../Infrastructure/Services/EntityResourceService");
var InviteCustomersComponent = /** @class */ (function () {
    function InviteCustomersComponent(_sharedLogisticContactService) {
        this._sharedLogisticContactService = _sharedLogisticContactService;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.CurrentSession.StartBusyIndicatorLoading();
    }
    InviteCustomersComponent.prototype.ngOnInit = function () {
    };
    InviteCustomersComponent.prototype.LoadData = function () {
        var _this = this;
        this.SharedLogisticCustomerLineList = [];
        this._sharedLogisticContactService.getSharedLogisticContactsbyCardId(this.CurrentEntity.Id, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
            var pmResponse = res;
            _this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                result.forEach(function (item) {
                    _this.SharedLogisticCustomerLineList.push(new CustomerLineViewModel_1.CustomerLineViewModel(item, _this));
                });
                if (_this.SharedLogisticCustomerLineList.length == 0)
                    _this.NoContactsVisibility = true;
                else
                    _this.NoContactsVisibility = false;
            }
        });
    };
    InviteCustomersComponent.prototype.SaveChanges = function (item) {
        var _this = this;
        this.sharedLogisticContact = item;
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
        this._sharedLogisticContactService.ContactInternetAccessInvitation(this.sharedLogisticContact).subscribe(function (res) {
            var pmResponse = res;
            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    //// this.currentAssemlyLocator.ListControl.GetSingleList(entityPM.Id);
                    // this.ReloadCustomer();
                    if (_this.sharedLogisticContact.InternetAccess) {
                        _this.ShowMessageWindow("Invitation email sent to " + "\" " + _this.sharedLogisticContact.EnglishName + " \"" + " with temporary password.", "Send Invitation", "gray", 150, true);
                    }
                }
            }
            else {
                if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                    _this.ShowMessage(pmResponse.ErrorsArray[0], "Logitude Message");
                }
            }
        });
    };
    InviteCustomersComponent.prototype.ShowMessage = function (message, title) {
        if (title === void 0) { title = ""; }
        var messageWindow = new MessageWindow_1.MessageWindow();
        messageWindow.Show(message);
        if (title) {
            messageWindow.Title = title;
        }
    };
    InviteCustomersComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    InviteCustomersComponent.prototype.ShowMessageWindow = function (message, title, textColor, windowheight, isShowOkButton) {
        if (isShowOkButton === void 0) { isShowOkButton = false; }
        var windowArgs = {};
        windowArgs.Message = message;
        windowArgs.TextColor = textColor;
        windowArgs.IsShowOkButton = isShowOkButton;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 350;
        logWindow.Height = windowheight;
        logWindow.IsShowCloseButton = !isShowOkButton;
        logWindow.WindowArgs = windowArgs;
        logWindow.Title = title;
        logWindow.Show("./SharedLogistics/Components/SharedMessageComponent");
    };
    InviteCustomersComponent.prototype.NewContactButtonClick = function () {
        var item = new ContactPM_1.ContactPM();
        item.Tenant = this.CurrentEntity.Tenant;
        item.CardId = this.CurrentEntity.Id;
        var itemComponent = new ContactsTabComponent_1.ContactItemClass(item, null, true);
        this.ShowAddEditContactWindow(itemComponent, "Add Contact");
    };
    InviteCustomersComponent.prototype.EditUserButtoClick = function (item) {
        var itemComponent = new ContactsTabComponent_1.ContactItemClass(item.contactPM, null, false);
        this.ShowAddEditContactWindow(itemComponent, "Edit Contact");
    };
    InviteCustomersComponent.prototype.ShowAddEditContactWindow = function (itemComponent, title) {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("Contact").subscribe(function (response) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            logWindow.Title = title;
            logWindow.DataContext = itemComponent;
            logWindow.Show('./CommonModules/CommonPartners/Components/AddEdit/AddEditContactComponent');
            logWindow.WindowClosed.subscribe(function ($event) { return _this.LoadData(); });
        });
    };
    InviteCustomersComponent.prototype.SetWindowArgs = function (args) {
        this.CurrentEntity = args.CurrentEntity;
        this.CustomerName = this.CurrentEntity.EnglishName;
        this.CustomerCode = this.CurrentEntity.Code;
        this.InvitationStatus = this.CurrentEntity.SharedLogisticsInvitationStatusName;
        this.LoadData();
    };
    InviteCustomersComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'InviteCustomersComponent',
            templateUrl: './InviteCustomersComponent.html',
            //inputs: ['PartnerTypeId', , 'DateParameter', 'DataContext', 'OnCloseWindowEvent'],
            providers: [SharedLogisticContactService_1.SharedLogisticContactService],
        }),
        __metadata("design:paramtypes", [SharedLogisticContactService_1.SharedLogisticContactService])
    ], InviteCustomersComponent);
    return InviteCustomersComponent;
}());
exports.InviteCustomersComponent = InviteCustomersComponent;
//# sourceMappingURL=InviteCustomersComponent.js.map