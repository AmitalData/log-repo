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
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var AddressPM_1 = require("../../../../Common/EntityPMs/AddressPM");
var PartnersDomainService_1 = require("../../../../Common/Services/PartnersDomainService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Args_1 = require("../../../../Common/Args");
var CustomerPM_1 = require("../../../../Common/EntityPMs/CustomerPM");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var CountryFlagPipe_1 = require("../../../../Controls/Pipes/CountryFlagPipe");
var AddressesTabComponent = /** @class */ (function () {
    function AddressesTabComponent(entityArgs) {
        var _this = this;
        this.entityArgs = entityArgs;
        this.EntityPM = null;
        this.EntityId = null;
        this.Customer = null;
        this.PartnerTypeId = null;
        this.IsCustomerPartner = false;
        this.IsVisibile = false;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.SessionEvent = null;
        this.SaveCompletedEvent = null;
        this.LoadCompletedEvent = null;
        this.IsEditingEnabled = false;
        this.IsBlockingUnifreightCustomer = false;
        this.AllAddresses = [];
        this.showInactive = false;
        this._entityResourceService.getEntityResourceByTableName("Address", 0).subscribe(function (response) {
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
    AddressesTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.SetUIProperties();
                }
            });
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.SetUIProperties();
                }
            });
            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
                if (s == "EntityActivated") {
                    if (_this.ObjectTableName == "Customer") {
                        _this.SetUIProperties();
                    }
                }
            });
        }
    };
    AddressesTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SessionEvent);
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    AddressesTabComponent.prototype.SetUIProperties = function () {
        var isBlockingUnifreightCustomer = false;
        if (this.Customer != null) {
            if (SessionLocator_1.SessionLocator.TenantPM.IsHybrid && (this.Customer.CustomerStatusCode == "ACT" || this.Customer.CustomerStatusCode == "WAC")) {
                isBlockingUnifreightCustomer = true;
            }
        }
        this.IsBlockingUnifreightCustomer = isBlockingUnifreightCustomer;
        this.IsEditingEnabled = !isBlockingUnifreightCustomer;
        this.ItemsSource.forEach(function (item) {
            item.SetUIProperties();
        });
    };
    AddressesTabComponent.prototype.LoadData = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.DomainService.GetAllAddressesPMsbyCardId(this.EntityId).subscribe(function (myResult) {
            _this.AllAddresses = myResult;
            _this.BuildItemsSource();
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    AddressesTabComponent.prototype.BuildItemsSource = function () {
        var _this = this;
        this.ItemsSource = [];
        var items = [];
        this.AllAddresses.forEach(function (item) {
            if (item.AddressTypeId != "O") {
                items.push(item);
            }
            else if (_this.ShowInactive) {
                items.push(item);
            }
            else {
                if (item.InActive == false) {
                    items.push(item);
                }
            }
        });
        if (items != null) {
            var isNewAddress_M = false;
            var isNewAddress_B = false;
            var isNewAddress_P = false;
            var myAddress_M = items.filter(function (f) { return f.AddressTypeId == "M"; })[0];
            var myAddress_B = items.filter(function (f) { return f.AddressTypeId == "B"; })[0];
            var myAddress_P = items.filter(function (f) { return f.AddressTypeId == "P"; })[0];
            if (myAddress_M == null) {
                isNewAddress_M = true;
                myAddress_M = new AddressPM_1.AddressPM();
                myAddress_M.Tenant = this.EntityPM.Tenant;
                myAddress_M.AddressTypeId = "M";
                myAddress_M.Description = "Main Address";
                myAddress_M.CardId = this.EntityId;
                myAddress_M.InActive = false;
                this.AllAddresses.push(myAddress_M);
            }
            if (myAddress_B == null) {
                isNewAddress_B = true;
                myAddress_B = new AddressPM_1.AddressPM();
                myAddress_B.Tenant = this.EntityPM.Tenant;
                myAddress_B.AddressTypeId = "B";
                myAddress_B.Description = "Billing Address";
                myAddress_B.CardId = this.EntityId;
                myAddress_B.InActive = false;
                this.AllAddresses.push(myAddress_B);
            }
            if (myAddress_P == null) {
                isNewAddress_P = true;
                myAddress_P = new AddressPM_1.AddressPM();
                myAddress_P.Tenant = this.EntityPM.Tenant;
                myAddress_P.AddressTypeId = "P";
                myAddress_P.Description = "Pickup / Delivery Address";
                myAddress_P.CardId = this.EntityId;
                myAddress_P.InActive = false;
                this.AllAddresses.push(myAddress_P);
            }
            this.ItemsSource.push(new AddressItemClass(myAddress_M, isNewAddress_M, this));
            this.ItemsSource.push(new AddressItemClass(myAddress_B, isNewAddress_B, this));
            this.ItemsSource.push(new AddressItemClass(myAddress_P, isNewAddress_P, this));
            items.filter(function (f) { return f.AddressTypeId == "O" || f.AddressTypeId == "L"; }).forEach(function (item) {
                _this.ItemsSource.push(new AddressItemClass(item, false, _this));
            });
        }
    };
    Object.defineProperty(AddressesTabComponent.prototype, "ShowInactive", {
        get: function () { return this.showInactive; },
        set: function (newValue) {
            if (this.showInactive != newValue) {
                this.showInactive = newValue;
                this.BuildItemsSource();
            }
        },
        enumerable: true,
        configurable: true
    });
    AddressesTabComponent.prototype.AddAddressClicked = function () {
        var item = new AddressPM_1.AddressPM();
        item.Tenant = this.EntityPM.Tenant;
        item.CardId = this.EntityId;
        item.Name = this.EntityPM.EnglishName;
        item.AddressTypeId = 'O';
        item.InActive = false;
        var itemViewModel = new AddressItemClass(item, true, this);
        this.RunAddEditWindow(itemViewModel, TextCodeTranslator_1.TextCodeTranslator.Translate("Address.O.AddAddress"));
    };
    AddressesTabComponent.prototype.EditAddressClicked = function (itemViewModel) {
        var textCode = itemViewModel.IsNewEntity ? "Address.O.AddAddress" : "Address.O.EditAddress";
        this.RunAddEditWindow(itemViewModel, TextCodeTranslator_1.TextCodeTranslator.Translate(textCode));
    };
    AddressesTabComponent.prototype.RunAddEditWindow = function (itemViewModel, windowTitle) {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = windowTitle;
        logWindow.DataContext = itemViewModel;
        logWindow.Show('./CommonModules/CommonPartners/Components/AddEdit/AddEditAddressComponent');
    };
    AddressesTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddressesTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], AddressesTabComponent);
    return AddressesTabComponent;
}());
exports.AddressesTabComponent = AddressesTabComponent;
var AddressItemClass = /** @class */ (function (_super) {
    __extends(AddressItemClass, _super);
    function AddressItemClass(item, isNewEntity, fatherComponent) {
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.ObjectTableName = "Address";
        _this.IsNewEntity = false;
        _this.Src = null;
        _this.IsInActiveVisible = false;
        _this.IsEditingEnabled = false;
        _this.IsBlockingUnifreightCustomer = false;
        _this.country = null;
        _this.state = null;
        _this.IsCopyMainAddress = false;
        _this.EntityPM = item;
        _this.IsNewEntity = isNewEntity;
        _this.SetHeader();
        _this.SetUIProperties();
        var pipe = new CountryFlagPipe_1.CountryFlagPipe();
        _this.Src = pipe.transform(_this.CountryCode);
        return _this;
    }
    AddressItemClass.prototype.SetHeader = function () {
        //if (this.AddressTypeId.toUpperCase() == "M") {
        //    this.Header = TextCodeTranslator.Translate("Address.O.MainAddress");
        //}
        //else if (this.AddressTypeId.toUpperCase() == "B") {
        //    this.Header = TextCodeTranslator.Translate("Address.O.BillingAddress");
        //}
        //else {
        this.Header = this.Description;
        //}
    };
    AddressItemClass.prototype.SetUIProperties = function () {
        this.IsEditingEnabled = this.fatherComponent.IsEditingEnabled;
        this.IsBlockingUnifreightCustomer = this.fatherComponent.IsBlockingUnifreightCustomer;
        this.UIProperties.SetEnabled("Description", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Name", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Address1", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Address2", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("ZipCode", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("City", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("CountryId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("InActive", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("PhoneNumber", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("FaxNumber", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("ATTN", this.ObjectTableName, this.IsEditingEnabled);
        var isInActiveVisible = false;
        if (this.AddressTypeId != "M") {
            if (!this.IsNewEntity) {
                isInActiveVisible = true;
            }
        }
        this.IsInActiveVisible = isInActiveVisible;
        this.SetUIProperties_State();
        this.SetUIProperties_TelFax();
    };
    AddressItemClass.prototype.SetUIProperties_State = function () {
        this.SetUIProperties_StateEnabled();
        this.SetUIProperties_StateRequired();
    };
    AddressItemClass.prototype.SetUIProperties_StateEnabled = function () {
        var isEnabled = false;
        if (this.IsEditingEnabled) {
            isEnabled = this.HasStates;
        }
        this.UIProperties.SetEnabled("StateId", this.ObjectTableName, isEnabled);
    };
    AddressItemClass.prototype.SetUIProperties_StateRequired = function () {
        var isRequired = false;
        if (this.State == null) {
            if (this.IsStateRequired) {
                isRequired = true;
            }
        }
        this.UIProperties.SetRequired("StateId", this.ObjectTableName, isRequired);
    };
    AddressItemClass.prototype.SetUIProperties_TelFax = function () {
        var isTelRequired = false;
        var isFaxRequired = false;
        if (this.fatherComponent.Customer != null) {
            if (this.fatherComponent.Customer.IsCustomer) {
                if (this.fatherComponent.Customer.PartnerTypeId == "CS") {
                    if (SessionLocator_1.SessionLocator.TenantPM.IsCustomerTelRequired) {
                        if (Tools_1.AppTool.IsNullOrEmpty(this.PhoneNumber)) {
                            isTelRequired = true;
                        }
                    }
                    if (SessionLocator_1.SessionLocator.TenantPM.IsCustomerFaxRequired) {
                        if (Tools_1.AppTool.IsNullOrEmpty(this.FaxNumber)) {
                            isFaxRequired = true;
                        }
                    }
                }
                else if (this.fatherComponent.Customer.PartnerTypeId == "PO") {
                    if (SessionLocator_1.SessionLocator.TenantPM.IsPotentialTelRequired) {
                        if (Tools_1.AppTool.IsNullOrEmpty(this.PhoneNumber)) {
                            isTelRequired = true;
                        }
                    }
                    if (SessionLocator_1.SessionLocator.TenantPM.IsPotentialFaxRequired) {
                        if (Tools_1.AppTool.IsNullOrEmpty(this.FaxNumber)) {
                            isFaxRequired = true;
                        }
                    }
                }
            }
        }
        this.UIProperties.SetRequired("PhoneNumber", this.ObjectTableName, isTelRequired);
        this.UIProperties.SetRequired("FaxNumber", this.ObjectTableName, isFaxRequired);
    };
    Object.defineProperty(AddressItemClass.prototype, "AddressTypeId", {
        // Properties
        get: function () { return this.EntityPM.AddressTypeId; },
        set: function (newValue) {
            if (this.EntityPM.AddressTypeId != newValue) {
                this.EntityPM.AddressTypeId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItemClass.prototype, "Description", {
        get: function () { return this.EntityPM.Description; },
        set: function (newValue) {
            if (this.EntityPM.Description != newValue) {
                this.EntityPM.Description = newValue;
                this.SetHeader();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItemClass.prototype, "Name", {
        get: function () { return this.EntityPM.Name; },
        set: function (newValue) {
            if (this.EntityPM.Name != newValue) {
                this.EntityPM.Name = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItemClass.prototype, "Address1", {
        get: function () { return this.EntityPM.Address1; },
        set: function (newValue) {
            if (this.EntityPM.Address1 != newValue) {
                this.EntityPM.Address1 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItemClass.prototype, "Address2", {
        get: function () { return this.EntityPM.Address2; },
        set: function (newValue) {
            if (this.EntityPM.Address2 != newValue) {
                this.EntityPM.Address2 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItemClass.prototype, "City", {
        get: function () { return this.EntityPM.City; },
        set: function (newValue) {
            if (this.EntityPM.City != newValue) {
                this.EntityPM.City = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItemClass.prototype, "ZipCode", {
        get: function () { return this.EntityPM.ZipCode; },
        set: function (newValue) {
            if (this.EntityPM.ZipCode != newValue) {
                this.EntityPM.ZipCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItemClass.prototype, "PhoneNumber", {
        get: function () { return this.EntityPM.PhoneNumber; },
        set: function (newValue) {
            if (this.EntityPM.PhoneNumber != newValue) {
                this.EntityPM.PhoneNumber = newValue;
                this.SetUIProperties_TelFax();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItemClass.prototype, "FaxNumber", {
        get: function () { return this.EntityPM.FaxNumber; },
        set: function (newValue) {
            if (this.EntityPM.FaxNumber != newValue) {
                this.EntityPM.FaxNumber = newValue;
                this.SetUIProperties_TelFax();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItemClass.prototype, "ATTN", {
        get: function () { return this.EntityPM.ATTN; },
        set: function (newValue) {
            if (this.EntityPM.ATTN != newValue) {
                this.EntityPM.ATTN = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItemClass.prototype, "CityLineText", {
        get: function () {
            var myResult = this.City;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.StateEnglishName)) {
                myResult += ", " + this.StateEnglishName;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ZipCode)) {
                myResult += ", " + this.ZipCode;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItemClass.prototype, "Country", {
        get: function () { return this.country; },
        set: function (newValue) {
            if (this.country != newValue) {
                this.country = newValue;
                this.OnCountryChanged(newValue);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItemClass.prototype, "CountryId", {
        get: function () { return this.EntityPM.CountryId; },
        set: function (newValue) {
            if (this.EntityPM.CountryId != newValue) {
                this.EntityPM.CountryId = newValue;
                this.StateId = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItemClass.prototype, "CountryCode", {
        get: function () { return this.EntityPM.CountryCode; },
        set: function (newValue) {
            if (this.EntityPM.CountryCode != newValue) {
                this.EntityPM.CountryCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItemClass.prototype, "CountryName", {
        get: function () { return this.EntityPM.CountryName; },
        set: function (newValue) {
            if (this.EntityPM.CountryName != newValue) {
                this.EntityPM.CountryName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItemClass.prototype, "CountryEnglishName", {
        get: function () { return this.EntityPM.CountryEnglishName; },
        set: function (newValue) {
            if (this.EntityPM.CountryEnglishName != newValue) {
                this.EntityPM.CountryEnglishName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItemClass.prototype, "HasStates", {
        get: function () { return this.EntityPM.HasStates; },
        set: function (newValue) {
            if (this.EntityPM.HasStates != newValue) {
                this.EntityPM.HasStates = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItemClass.prototype, "IsStateRequired", {
        get: function () { return this.EntityPM.IsStateRequired; },
        set: function (newValue) {
            if (this.EntityPM.IsStateRequired != newValue) {
                this.EntityPM.IsStateRequired = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItemClass.prototype, "State", {
        get: function () { return this.state; },
        set: function (newValue) {
            if (this.state != newValue) {
                this.state = newValue;
                this.OnStateChanged(newValue);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItemClass.prototype, "StateId", {
        get: function () { return this.EntityPM.StateId; },
        set: function (newValue) {
            if (this.EntityPM.StateId != newValue) {
                this.EntityPM.StateId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItemClass.prototype, "StateCode", {
        get: function () { return this.EntityPM.StateCode; },
        set: function (newValue) {
            if (this.EntityPM.StateCode != newValue) {
                this.EntityPM.StateCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItemClass.prototype, "StateEnglishName", {
        get: function () { return this.EntityPM.StateEnglishName; },
        set: function (newValue) {
            if (this.EntityPM.StateEnglishName != newValue) {
                this.EntityPM.StateEnglishName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItemClass.prototype, "InActive", {
        get: function () { return this.EntityPM.InActive; },
        set: function (newValue) {
            if (this.EntityPM.InActive != newValue) {
                this.EntityPM.InActive = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItemClass.prototype, "IsLocalLanguage", {
        get: function () { return this.EntityPM.IsLocalLanguage; },
        set: function (newValue) {
            if (this.EntityPM.IsLocalLanguage != newValue) {
                this.EntityPM.IsLocalLanguage = newValue;
                if (this.Country != null) {
                    this.CountryName = this.EntityPM.IsLocalLanguage ? this.Country.LocalName : this.Country.EnglishName;
                }
                if (!newValue) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.Description)) {
                        this.Description = this.Description.replace(/[^\x20-\x7F]/g, "");
                    }
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.Name)) {
                        this.Name = this.Name.replace(/[^\x20-\x7F]/g, "");
                    }
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.Address1)) {
                        this.Address1 = this.Address1.replace(/[^\x20-\x7F]/g, "");
                    }
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.Address2)) {
                        this.Address2 = this.Address2.replace(/[^\x20-\x7F]/g, "");
                    }
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.City)) {
                        this.City = this.City.replace(/[^\x20-\x7F]/g, "");
                    }
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.ATTN)) {
                        this.ATTN = this.ATTN.replace(/[^\x20-\x7F]/g, "");
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    AddressItemClass.prototype.OnCountryChanged = function (list) {
        if (list == null) {
            this.CountryCode = null;
            this.CountryName = null;
            this.CountryEnglishName = null;
            this.HasStates = false;
            this.IsStateRequired = false;
        }
        else {
            this.CountryCode = list.Code;
            this.CountryName = this.IsLocalLanguage ? list.LocalName : list.EnglishName;
            this.CountryEnglishName = list.EnglishName;
            this.HasStates = list.HasStates;
            this.IsStateRequired = list.IsStateRequired;
        }
        this.SetUIProperties_State();
    };
    AddressItemClass.prototype.OnStateChanged = function (list) {
        if (list == null) {
            this.StateCode = null;
            this.StateEnglishName = null;
        }
        else {
            this.StateCode = list.Code;
            this.StateEnglishName = list.EnglishName;
        }
        this.SetUIProperties_StateRequired();
    };
    // Commands
    AddressItemClass.prototype.SelectCityCommand = function () {
        var _this = this;
        var args = new Args_1.CitySelectionArgs(this.CountryId);
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Select City";
        logWindow.WindowArgs = args;
        logWindow.Show("./CommonModules/CommonOthers/Components/CitySelection/CitySelectionComponent");
        logWindow.WindowClosed.subscribe(function ($event) {
            if (args.IsCitySelected) {
                var mySelectedCity = args.CityName;
                if (_this.EntityPM.IsLocalLanguage && args.CityLocalName != null) {
                    mySelectedCity = args.CityLocalName;
                }
                _this.City = mySelectedCity;
                _this.CountryId = args.CountryId;
                _this.StateId = args.StateId;
            }
        });
    };
    AddressItemClass.prototype.CopyMainAddressClicked = function (isFromMainTab) {
        if (this.AddressTypeId == "P") {
            var address = this.fatherComponent.AllAddresses.filter(function (f) { return f.AddressTypeId == 'M'; })[0];
            if (address != null) {
                if (Tools_1.AppTool.IsNullOrEmpty(address.City)) {
                    var messageWindow = new MessageWindow_1.MessageWindow();
                    messageWindow.Show("Please fill the main address city");
                }
                else {
                    if (isFromMainTab) {
                        this.IsCopyMainAddress = true;
                        this.fatherComponent.EditAddressClicked(this);
                    }
                    else {
                        this.CopyMainAddress();
                    }
                }
            }
        }
    };
    AddressItemClass.prototype.CopyMainAddress = function () {
        var address = this.fatherComponent.AllAddresses.filter(function (f) { return f.AddressTypeId == 'M'; })[0];
        if (address != null) {
            this.Name = address.Name;
            this.Address1 = address.Address1;
            this.Address2 = address.Address2;
            this.City = address.City;
            this.ATTN = address.ATTN;
            this.CountryId = address.CountryId;
            this.CountryCode = address.CountryCode;
            this.CountryName = address.CountryName;
            this.CountryEnglishName = address.CountryEnglishName;
            this.StateId = address.StateId;
            this.StateCode = address.StateCode;
            this.StateEnglishName = address.StateEnglishName;
            this.ZipCode = address.ZipCode;
            this.PhoneNumber = address.PhoneNumber;
            this.FaxNumber = address.FaxNumber;
            this.IsLocalLanguage = address.IsLocalLanguage;
        }
        this.IsCopyMainAddress = false;
    };
    return AddressItemClass;
}(BaseComponent_1.BaseComponent));
exports.AddressItemClass = AddressItemClass;
//# sourceMappingURL=AddressesTabComponent.js.map