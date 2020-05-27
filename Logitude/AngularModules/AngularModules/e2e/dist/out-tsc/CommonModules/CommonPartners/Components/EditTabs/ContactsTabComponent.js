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
var core_1 = require("@angular/core");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var ContactPM_1 = require("../../../../Common/EntityPMs/ContactPM");
var PartnersDomainService_1 = require("../../../../Common/Services/PartnersDomainService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var CustomerPM_1 = require("../../../../Common/EntityPMs/CustomerPM");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var ContactsTabComponent = /** @class */ (function () {
    function ContactsTabComponent(entityArgs) {
        var _this = this;
        this.entityArgs = entityArgs;
        this.EntityPM = null;
        this.EntityId = null;
        this.Customer = null;
        this.PartnerTypeId = null;
        this.IsCustomerPartner = false;
        this.IsNoDataVisible = false;
        this.IsVisibile = false;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.SaveCompletedEvent = null;
        this.LoadCompletedEvent = null;
        this.IsEditingEnabled = false;
        this.IsBlockingUnifreightCustomer = false;
        this._entityResourceService.getEntityResourceByTableName("Contact", 0).subscribe(function (response) {
            _this.IsVisibile = true;
            _this.ItemsSource = [];
            _this.EntityPM = entityArgs.EntityPM;
            _this.EntityId = entityArgs.EntityPM == null ? null : entityArgs.EntityPM.Id;
            _this.ObjectTableName = entityArgs.ObjectTableName;
            _this.PartnerTypeId = _this.EntityPM.PartnerTypeId;
            if (_this.EntityPM instanceof CustomerPM_1.CustomerPM) {
                _this.Customer = _this.EntityPM;
                _this.IsCustomerPartner = true;
            }
            if (_this.DomainService == null) {
                _this.DomainService = new PartnersDomainService_1.PartnersDomainService();
            }
            _this.Listen();
            _this.SetUIProperties();
            _this.LoadData();
        });
    }
    ContactsTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.SetUIProperties();
                    _this.LoadData();
                }
            });
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.SetUIProperties();
                    _this.LoadData();
                }
            });
        }
    };
    ContactsTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    ContactsTabComponent.prototype.SetUIProperties = function () {
        var isBlockingUnifreightCustomer = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ExternalId)) {
            if (this.Customer != null) {
                if (SessionLocator_1.SessionLocator.TenantPM.IsHybrid && (this.Customer.CustomerStatusCode == "ACT" || this.Customer.CustomerStatusCode == "WAC")) {
                    isBlockingUnifreightCustomer = true;
                }
            }
        }
        this.IsBlockingUnifreightCustomer = isBlockingUnifreightCustomer;
        this.IsEditingEnabled = !isBlockingUnifreightCustomer;
    };
    ContactsTabComponent.prototype.LoadData = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.DomainService.GetAllContactsPMsbyCardId(this.EntityPM.Id).subscribe(function (myResult) {
            _this.BuildItemsSource(myResult);
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    ContactsTabComponent.prototype.BuildItemsSource = function (items) {
        var _this = this;
        this.ItemsSource = [];
        if (items == null) {
            items = [];
        }
        items.forEach(function (item) {
            _this.ItemsSource.push(new ContactItemClass(item, _this, false));
        });
        this.SetIsNoDataVisible();
    };
    ContactsTabComponent.prototype.SetIsNoDataVisible = function () {
        var isNoDataVisible = false;
        if (this.ItemsSource.length == 0) {
            isNoDataVisible = true;
        }
        this.IsNoDataVisible = isNoDataVisible;
    };
    ContactsTabComponent.prototype.NewEntityClicked = function () {
        var item = new ContactPM_1.ContactPM();
        item.Tenant = this.EntityPM.Tenant;
        item.CardId = this.EntityPM.Id;
        var itemComponent = new ContactItemClass(item, this, true);
        this.RunWindow(itemComponent, TextCodeTranslator_1.TextCodeTranslator.Translate("Contact.O.AddContact"));
    };
    ContactsTabComponent.prototype.EditEntityClicked = function (itemComponent) {
        this.RunWindow(itemComponent, TextCodeTranslator_1.TextCodeTranslator.Translate("Contact.O.EditContact"));
    };
    ContactsTabComponent.prototype.RunWindow = function (itemViewModel, windowTitle) {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.Title = windowTitle;
        logWindow.DataContext = itemViewModel;
        logWindow.Show('./CommonModules/CommonPartners/Components/AddEdit/AddEditContactComponent');
    };
    ContactsTabComponent.prototype.DisconnectClicked = function (itemComponent) {
        var _this = this;
        if (!itemComponent.IsPrimary || this.ItemsSource.length == 1) {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Title = "Disconnect";
            confirmWindow.Show('Are you sure you want to disconnect?');
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    _this.CurrentSession.StartBusyIndicatorSaving();
                    var isCardEntityDirty = _this.EntityPM['IsDirty'];
                    if (itemComponent.IsPrimary && _this.ItemsSource.length == 1) {
                        _this.EntityPM['PrimaryContactId'] = null;
                        isCardEntityDirty = true;
                    }
                    var indexOfItemComponent = _this.ItemsSource.indexOf(itemComponent);
                    if (indexOfItemComponent > -1) {
                        _this.ItemsSource.splice(indexOfItemComponent, 1);
                        _this.SetIsNoDataVisible();
                    }
                    var myContactPM = itemComponent.EntityPM;
                    myContactPM.DisconectFromCard = true;
                    var args = new PartnersDomainService_1.PartnerServicePM();
                    args.PartnerId = _this.EntityId;
                    args.PartnerTypeId = _this.PartnerTypeId;
                    args.Tenant = myContactPM.Tenant;
                    args.ContactId = myContactPM.Id;
                    args.PartnerId = myContactPM.CardId;
                    args.Contact = myContactPM;
                    args.IsContactDirty = true;
                    args.IsPartnerDirty = isCardEntityDirty;
                    _this.DomainService.SetPartner(args, _this.EntityPM);
                    _this.DomainService.PostPartnerAddress(args).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            _this.EntityPM['IsDirty'] = isCardEntityDirty;
                        }
                        _this.CurrentSession.StopBusyIndicator();
                    });
                }
            });
        }
        else {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show("Please set another contact as the primary before disconnecting the primary contact.");
        }
    };
    ContactsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ContactsTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], ContactsTabComponent);
    return ContactsTabComponent;
}());
exports.ContactsTabComponent = ContactsTabComponent;
var ContactItemClass = /** @class */ (function () {
    function ContactItemClass(item, fatherComponent, isNewEntity) {
        this.fatherComponent = fatherComponent;
        this.ObjectTableName = "Contact";
        this.IsNewEntity = false;
        this.IsPrimary = false;
        this.EntityPM = item;
        this.IsNewEntity = isNewEntity;
        this.CheckPrimary();
    }
    Object.defineProperty(ContactItemClass.prototype, "Id", {
        // Properties
        get: function () { return this.EntityPM.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactItemClass.prototype, "Name", {
        get: function () { return this.EntityPM.EnglishName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactItemClass.prototype, "Email", {
        get: function () { return this.EntityPM.Email; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactItemClass.prototype, "EnglishName", {
        get: function () { return this.EntityPM.EnglishName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactItemClass.prototype, "LocalName", {
        get: function () { return this.EntityPM.LocalName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactItemClass.prototype, "Position", {
        get: function () { return this.EntityPM.Position; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactItemClass.prototype, "BusinessPhone", {
        get: function () { return this.EntityPM.BusinessPhone; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactItemClass.prototype, "Mobile", {
        get: function () { return this.EntityPM.Mobile; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactItemClass.prototype, "Fax", {
        get: function () { return this.EntityPM.Fax; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactItemClass.prototype, "Birthday", {
        get: function () { return this.EntityPM.Birthday; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactItemClass.prototype, "Anniversary", {
        get: function () { return this.EntityPM.Anniversary; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactItemClass.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactItemClass.prototype, "InActive", {
        get: function () { return this.EntityPM.InActive; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactItemClass.prototype, "BirthdayReminder", {
        get: function () { return this.EntityPM.BirthdayReminder; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactItemClass.prototype, "AnniversaryReminder", {
        get: function () { return this.EntityPM.AnniversaryReminder; },
        enumerable: true,
        configurable: true
    });
    ContactItemClass.prototype.CheckPrimary = function () {
        var isPrimary = false;
        var myCardPrimaryContactId = null;
        if (this.fatherComponent && this.fatherComponent.EntityPM) {
            myCardPrimaryContactId = this.fatherComponent.EntityPM['PrimaryContactId'];
            if (!Tools_1.AppTool.IsNullOrEmpty(myCardPrimaryContactId)) {
                if (myCardPrimaryContactId == this.Id) {
                    isPrimary = true;
                }
            }
        }
        this.IsPrimary = isPrimary;
    };
    ContactItemClass.prototype.SetPrimary = function () {
        this.fatherComponent.EntityPM['PrimaryContactId'] = this.Id;
        this.fatherComponent.EntityPM['PrimaryContactName'] = this.EnglishName;
        this.fatherComponent.EntityPM['PrimaryContactPhone'] = this.BusinessPhone;
        this.fatherComponent.ItemsSource.forEach(function (item) {
            item.CheckPrimary();
        });
    };
    return ContactItemClass;
}());
exports.ContactItemClass = ContactItemClass;
//# sourceMappingURL=ContactsTabComponent.js.map