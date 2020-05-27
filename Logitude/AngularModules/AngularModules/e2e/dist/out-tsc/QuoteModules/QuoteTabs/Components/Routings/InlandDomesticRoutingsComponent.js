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
var QuoteDomainService_1 = require("../../../../Quote/Services/QuoteDomainService");
var QuoteUtilities_1 = require("../../../../Quote/Utilities/QuoteUtilities");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var AddressPM_1 = require("../../../../Common/EntityPMs/AddressPM");
var AddressListService_1 = require("../../../../Common/Services/StandardLists/AddressListService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var InlandDomesticRoutingsComponent = /** @class */ (function (_super) {
    __extends(InlandDomesticRoutingsComponent, _super);
    function InlandDomesticRoutingsComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityPM = null;
        _this.ObjectTableName = "Quotes";
        _this.DataContext = _this;
        _this.IsSubjectVisible = false;
        _this.TabSelectedEvent = null;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.IsQuoteEditEnabled = false;
        _this.IsSubjectVisible = SessionLocator_1.SessionLocator.TenantPM.IsQuoteSubjectEdited ? true : false;
        _this.Listen();
        return _this;
    }
    InlandDomesticRoutingsComponent.prototype.InitTab = function (entityPM, tableName) {
        this.EntityPM = entityPM;
        this.ObjectTableName = tableName;
        this.SetLabels();
        this.SetUIProperties();
        this.GetFromPartnerData();
        this.GetToPartnerData();
    };
    InlandDomesticRoutingsComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent) {
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
            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe(function (tabCode) {
                if (tabCode == "QTRT") {
                    _this.GetFromPartnerData();
                    _this.GetToPartnerData();
                }
            });
        }
    };
    InlandDomesticRoutingsComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.TabSelectedEvent);
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    InlandDomesticRoutingsComponent.prototype.SetUIProperties = function () {
        this.IsQuoteEditEnabled = QuoteUtilities_1.QuoteUtilities.IsQuoteEditEnabled(this.EntityPM);
        this.UIProperties.SetEnabled("Subject", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("ETD", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("ETA", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("MainCarriageCarrierId", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("FromPartnerAddressId", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("ToPartnerAddressId", this.ObjectTableName, this.IsQuoteEditEnabled);
    };
    Object.defineProperty(InlandDomesticRoutingsComponent.prototype, "IsSubjectEdited", {
        // Subject
        get: function () { return this.EntityPM.IsSubjectEdited; },
        set: function (value) {
            if (this.EntityPM.IsSubjectEdited != value) {
                this.EntityPM.IsSubjectEdited = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InlandDomesticRoutingsComponent.prototype, "Subject", {
        get: function () { return this.EntityPM.Subject; },
        set: function (newValue) {
            if (this.EntityPM.Subject != newValue) {
                this.EntityPM.Subject = newValue;
                this.IsSubjectEdited = true;
            }
        },
        enumerable: true,
        configurable: true
    });
    InlandDomesticRoutingsComponent.prototype.ResetSubjectEdited = function () {
        this.IsSubjectEdited = false;
        this.GetSubjectField();
    };
    InlandDomesticRoutingsComponent.prototype.GetSubjectField = function () {
        var _this = this;
        if (SessionLocator_1.SessionLocator.TenantPM.IsQuoteSubjectEdited) {
            var myQuoteDomainService = new QuoteDomainService_1.QuoteDomainService();
            myQuoteDomainService.ComputeQuoteAutomaticSubject(this.EntityPM).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    _this.EntityPM.Subject = myResponse.Result;
                }
            });
        }
    };
    Object.defineProperty(InlandDomesticRoutingsComponent.prototype, "ETD", {
        //////////// Dates ////////////
        get: function () { return this.EntityPM.ETD; },
        set: function (newValue) {
            if (this.EntityPM.ETD != newValue) {
                this.EntityPM.ETD = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InlandDomesticRoutingsComponent.prototype, "ETA", {
        get: function () { return this.EntityPM.ETA; },
        set: function (newValue) {
            if (this.EntityPM.ETA != newValue) {
                this.EntityPM.ETA = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    InlandDomesticRoutingsComponent.prototype.SetLabels = function () {
        this.CarrierLabel = "Quote.S.NewQuote.Carrier";
        switch (this.EntityPM.TransportModeId) {
            case "A": {
                this.CarrierLabel = "Quote.S.NewQuote.Airline";
                this.CarrierDependencyProperty1 = "AL";
                break;
            }
            case "O": {
                this.CarrierLabel = "Quote.S.NewQuote.Shippingline";
                this.CarrierDependencyProperty1 = "SL";
                break;
            }
            case "I": {
                this.CarrierLabel = "Quote.S.NewQuote.Trucker";
                this.CarrierDependencyProperty1 = "TR";
                break;
            }
        }
    };
    Object.defineProperty(InlandDomesticRoutingsComponent.prototype, "TransportModeId", {
        get: function () { return this.EntityPM.TransportModeId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InlandDomesticRoutingsComponent.prototype, "MainCarriageCarrierId", {
        get: function () { return this.EntityPM.MainCarriageCarrierId; },
        set: function (newValue) {
            if (this.EntityPM.MainCarriageCarrierId != newValue) {
                this.EntityPM.MainCarriageCarrierId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InlandDomesticRoutingsComponent.prototype, "FromPartnerId", {
        //////////// From Partner ////////////
        get: function () { return this.EntityPM.FromPartnerId; },
        set: function (newValue) {
            if (this.EntityPM.FromPartnerId != newValue) {
                this.EntityPM.FromPartnerId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InlandDomesticRoutingsComponent.prototype, "FromPartnerAddressId", {
        get: function () { return this.EntityPM.FromPartnerAddressId; },
        set: function (newValue) {
            if (this.EntityPM.FromPartnerAddressId != newValue) {
                this.EntityPM.FromPartnerAddressId = newValue;
                this.GetFromPartnerData();
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InlandDomesticRoutingsComponent.prototype, "FromPartnerName", {
        get: function () { return this.EntityPM.FromPartnerName; },
        set: function (newValue) {
            if (this.EntityPM.FromPartnerName != newValue) {
                this.EntityPM.FromPartnerName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    InlandDomesticRoutingsComponent.prototype.GetFromPartnerData = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.FromPartnerId)) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.FromPartnerAddressId)) {
                this.FromAddressList = null;
            }
            else {
                var myService = new AddressListService_1.AddressListService();
                myService.getSingle(this.FromPartnerAddressId).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        _this.FromAddressList = myResponse.Result;
                    }
                });
            }
        }
    };
    Object.defineProperty(InlandDomesticRoutingsComponent.prototype, "IsEditFromAddressEnabled", {
        get: function () {
            var myResult = false;
            if (this.IsQuoteEditEnabled) {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.FromPartnerAddressId)) {
                    myResult = true;
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InlandDomesticRoutingsComponent.prototype, "ToPartnerId", {
        //////////// To Partner ////////////
        get: function () { return this.EntityPM.ToPartnerId; },
        set: function (newValue) {
            if (this.EntityPM.ToPartnerId != newValue) {
                this.EntityPM.ToPartnerId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InlandDomesticRoutingsComponent.prototype, "ToPartnerAddressId", {
        get: function () { return this.EntityPM.ToPartnerAddressId; },
        set: function (newValue) {
            if (this.EntityPM.ToPartnerAddressId != newValue) {
                this.EntityPM.ToPartnerAddressId = newValue;
                this.GetToPartnerData();
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InlandDomesticRoutingsComponent.prototype, "ToPartnerName", {
        get: function () { return this.EntityPM.ToPartnerName; },
        set: function (newValue) {
            if (this.EntityPM.ToPartnerName != newValue) {
                this.EntityPM.ToPartnerName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    InlandDomesticRoutingsComponent.prototype.GetToPartnerData = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ToPartnerId)) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ToPartnerAddressId)) {
                this.ToAddressList = null;
            }
            else {
                var myService = new AddressListService_1.AddressListService();
                myService.getSingle(this.ToPartnerAddressId).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        _this.ToAddressList = myResponse.Result;
                    }
                });
            }
        }
    };
    Object.defineProperty(InlandDomesticRoutingsComponent.prototype, "IsEditToAddressEnabled", {
        get: function () {
            var myResult = false;
            if (this.IsQuoteEditEnabled) {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.ToPartnerAddressId)) {
                    myResult = true;
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    InlandDomesticRoutingsComponent.prototype.EditAddressClicked = function (myAddressCode) {
        var _this = this;
        var myAddressId = null;
        var myPartnerId = null;
        switch (myAddressCode) {
            case "F": {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.FromPartnerId)) {
                    myPartnerId = this.FromPartnerId;
                    myAddressId = this.FromPartnerAddressId;
                }
                break;
            }
            case "T": {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.ToPartnerId)) {
                    myPartnerId = this.ToPartnerId;
                    myAddressId = this.ToPartnerAddressId;
                }
                break;
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(myAddressId) && !Tools_1.AppTool.IsNullOrEmpty(myPartnerId)) {
            var logeWindow = new LogitudeWindow_1.LogitudeWindow();
            logeWindow.Width = 630;
            logeWindow.Height = 430;
            logeWindow.Title = "Edit Address";
            logeWindow.WindowArgs = { EntityId: myAddressId, CardId: myPartnerId };
            logeWindow.Show("./QuoteModules/QuoteTabs/Components/Routings/RoutingsAddEditAddressComponent");
            logeWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    switch (myAddressCode) {
                        case "F": {
                            _this.GetFromPartnerData();
                            break;
                        }
                        case "T": {
                            _this.GetToPartnerData();
                            break;
                        }
                    }
                }
            });
        }
    };
    InlandDomesticRoutingsComponent.prototype.AddAddressClicked = function (myAddressCode) {
        var _this = this;
        var entityPM = null;
        var myPartnerId = null;
        switch (myAddressCode) {
            case "F": {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.FromPartnerId)) {
                    myPartnerId = this.FromPartnerId;
                    entityPM = new AddressPM_1.AddressPM();
                    entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                    entityPM.AddressTypeId = "O";
                    entityPM.CardId = myPartnerId;
                }
                break;
            }
            case "T": {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.ToPartnerId)) {
                    myPartnerId = this.ToPartnerId;
                    entityPM = new AddressPM_1.AddressPM();
                    entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                    entityPM.AddressTypeId = "O";
                    entityPM.CardId = myPartnerId;
                }
                break;
            }
        }
        if (entityPM != null && !Tools_1.AppTool.IsNullOrEmpty(myPartnerId)) {
            var logeWindow = new LogitudeWindow_1.LogitudeWindow();
            logeWindow.Width = 630;
            logeWindow.Height = 430;
            logeWindow.Title = "Add Address";
            logeWindow.WindowArgs = { EntityPM: entityPM, CardId: myPartnerId };
            logeWindow.Show("./QuoteModules/QuoteTabs/Components/Routings/RoutingsAddEditAddressComponent");
            logeWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    switch (myAddressCode) {
                        case "P": {
                            _this.FromPartnerAddressId = null;
                            _this.FromPartnerAddressId = entityPM.Id;
                            break;
                        }
                        case "D": {
                            _this.ToPartnerAddressId = null;
                            _this.ToPartnerAddressId = entityPM.Id;
                            break;
                        }
                    }
                }
            });
        }
    };
    InlandDomesticRoutingsComponent = __decorate([
        core_1.Component({
            selector: 'InlandDomesticRoutingsComponent',
            moduleId: module.id,
            templateUrl: './InlandDomesticRoutingsComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], InlandDomesticRoutingsComponent);
    return InlandDomesticRoutingsComponent;
}(BaseComponent_1.BaseComponent));
exports.InlandDomesticRoutingsComponent = InlandDomesticRoutingsComponent;
//# sourceMappingURL=InlandDomesticRoutingsComponent.js.map