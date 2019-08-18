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
var CardListService_1 = require("../../../../Common/Services/StandardLists/CardListService");
var AddressListService_1 = require("../../../../Common/Services/StandardLists/AddressListService");
var AddressService_1 = require("../../../../Common/Services/ExtendedLists/AddressService");
var AWBUtilities_1 = require("../../../Utilities/AWBUtilities");
var Tools_1 = require("../../../../Infrastructure/Tools");
var InfraSettings_1 = require("../../../../Infrastructure/Utilities/InfraSettings");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var Args_1 = require("../../../Args");
var Tools_2 = require("../../../Tools");
var PartnersTabComponent = /** @class */ (function (_super) {
    __extends(PartnersTabComponent, _super);
    function PartnersTabComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.LabelColumnWidth = 85;
        _this.isSetDefaultTenantAgent = false;
        _this.IsEditingEnabled = false;
        _this.IsEditSHIEnabled = false;
        _this.IsEditCONEnabled = false;
        _this.IsEditAGTEnabled = false;
        _this.ShipperWarning = "";
        _this.ConsigneeWarning = "";
        _this.AgentWarning = "";
        _this.ShowWarningShipperId = false;
        _this.ShowWarningConsigneeId = false;
        _this.ShowWarningAgentId = false;
        _this.ShowWarningAgentIATA = false;
        _this.ShowWarningAgentCASS = false;
        _this.InitializeServices();
        return _this;
    }
    PartnersTabComponent.prototype.InitializeServices = function () {
        if (this.myCardListService == null) {
            this.myCardListService = new CardListService_1.CardListService();
        }
    };
    PartnersTabComponent.prototype.InitTab = function (wizard) {
        this.Wizard = wizard;
        this.EntityPM = this.Wizard.EntityPM;
        this.ObjectTableName = this.Wizard.ObjectTableName;
        this.Listen();
        this.SetLOVDependency();
        this.SetUIProperties();
        this.InitializePartners();
        this.Validate();
    };
    PartnersTabComponent.prototype.RefreshTab = function () {
        this.Validate();
    };
    PartnersTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.Wizard != null) {
            this.Wizard.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.Wizard.EntityPM;
                    _this.RefreshTab();
                    _this.SetUIProperties();
                }
            });
            this.Wizard.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.Wizard.EntityPM;
                    _this.RefreshTab();
                    _this.SetUIProperties();
                }
            });
        }
    };
    PartnersTabComponent.prototype.InitializePartners = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipperId)) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipperAddressId)) {
                this.GetShipperAddress();
            }
            else {
                this.GetShipperMainAddress();
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ConsigneeId)) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ConsigneeAddressId)) {
                this.GetConsigneeAddress();
            }
            else {
                this.GetConsigneeMainAddress();
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.IssuingCarrierAgentId)) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.IssuingCarrierAddressId)) {
                this.GetIssuingCarrierAddress();
            }
            else {
                this.GetIssuingCarrierMainAddress();
            }
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id) && !this.EntityPM.IsCopyMode) {
            this.SetDefaultIssuingCarrierAgent();
        }
    };
    PartnersTabComponent.prototype.SetDefaultIssuingCarrierAgent = function () {
        this.CASSCode = InfraSettings_1.InfraSettings.TenantPM.CASSCode;
        this.IssuingCarrierIATACode = InfraSettings_1.InfraSettings.TenantPM.IATA;
        this.IssuingCarrierAgentId = InfraSettings_1.InfraSettings.TenantPM.AgentId;
        if (this.EntityPM.BookingLevelCode == "C") {
            this.ShipperId = this.IssuingCarrierAgentId;
        }
    };
    PartnersTabComponent.prototype.SetLOVDependency = function () {
        var myDependency = "CS,AG";
        var myDependencyIsList = true;
        this.PartnerDependencyProperty1 = myDependency;
        this.PartnerDependencyProperty1IsList = myDependencyIsList;
    };
    PartnersTabComponent.prototype.SetUIProperties = function () {
        this.IsEditingEnabled = Tools_2.BookingTool.IsEditingFieldsEnabled_Others(this.EntityPM);
        this.UIProperties.SetEnabled("ShipperId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("ConsigneeId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("IssuingCarrierAgentId", this.ObjectTableName, this.IsEditingEnabled);
        var isShipperFieldEnabled = false;
        var isConsigneeFieldEnabled = false;
        var isIssuingFieldEnabled = false;
        if (this.IsEditingEnabled) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipperId)) {
                isShipperFieldEnabled = true;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ConsigneeId)) {
                isConsigneeFieldEnabled = true;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.IssuingCarrierAgentId)) {
                isIssuingFieldEnabled = true;
            }
        }
        this.UIProperties.SetEnabled("ShipperAddressId", this.ObjectTableName, isShipperFieldEnabled);
        this.UIProperties.SetEnabled("ShipperReference", this.ObjectTableName, isShipperFieldEnabled);
        this.IsEditSHIEnabled = isShipperFieldEnabled;
        this.UIProperties.SetEnabled("ConsigneeAddressId", this.ObjectTableName, isConsigneeFieldEnabled);
        this.UIProperties.SetEnabled("ConsigneeReference", this.ObjectTableName, isConsigneeFieldEnabled);
        this.IsEditCONEnabled = isConsigneeFieldEnabled;
        this.UIProperties.SetEnabled("IssuingCarrierAddressId", this.ObjectTableName, isIssuingFieldEnabled);
        this.UIProperties.SetEnabled("IssuingCarrierIATACode", this.ObjectTableName, isIssuingFieldEnabled);
        this.UIProperties.SetEnabled("CASSCode", this.ObjectTableName, isIssuingFieldEnabled);
        this.IsEditAGTEnabled = isIssuingFieldEnabled;
    };
    PartnersTabComponent.prototype.FireWizardEvent = function () {
        this.Wizard.ValidateScreen_PAR();
        this.Validate();
    };
    PartnersTabComponent.prototype.Validate = function () {
        this.Validate_SHI();
        this.Validate_CON();
        this.Validate_AGT();
    };
    PartnersTabComponent.prototype.Validate_SHI = function () {
        var warningMessage = "";
        if (this.ShipperId != null) {
            if (this.ShipperAddressList == null) {
                warningMessage = "Address is required";
            }
            else {
                var myAddressList = this.ShipperAddressList;
                var myAddress1 = Tools_1.AppTool.IsNullOrEmpty(myAddressList.Address1) ? null : myAddressList.Address1.trim();
                var myAddress2 = Tools_1.AppTool.IsNullOrEmpty(myAddressList.Address2) ? null : myAddressList.Address2.trim();
                var myCity = Tools_1.AppTool.IsNullOrEmpty(myAddressList.City) ? null : myAddressList.City.trim();
                if (!Tools_1.FormatTool.IsTextFormatted(myAddress1)) {
                    warningMessage = Tools_1.AppTool.IsNullOrEmpty(warningMessage) ? "Address1" : warningMessage + ",Address1";
                }
                if (!Tools_1.FormatTool.IsTextFormatted(myAddress2)) {
                    warningMessage = Tools_1.AppTool.IsNullOrEmpty(warningMessage) ? "Address2" : warningMessage + ",Address2";
                }
                if (Tools_1.AppTool.IsNullOrEmpty(myAddress1) && Tools_1.AppTool.IsNullOrEmpty(myAddress2)) {
                    warningMessage = Tools_1.AppTool.IsNullOrEmpty(warningMessage) ? "Address1 or Address2" : warningMessage + ",Address1 or Address2";
                }
                if (Tools_1.AppTool.IsNullOrEmpty(myAddressList.CountryId)) {
                    warningMessage = Tools_1.AppTool.IsNullOrEmpty(warningMessage) ? "Country" : warningMessage + ",Country";
                }
                if (Tools_1.AppTool.IsNullOrEmpty(myAddressList.StateCode)) {
                    if (this.Wizard.AllStates.filter(function (d) { return d.CountryId == myAddressList.CountryId; }).length > 0) {
                        warningMessage = Tools_1.AppTool.IsNullOrEmpty(warningMessage) ? "State" : warningMessage + ",State";
                    }
                }
                if (Tools_1.AppTool.IsNullOrEmpty(myCity)) {
                    warningMessage = Tools_1.AppTool.IsNullOrEmpty(warningMessage) ? "City" : warningMessage + ",City";
                }
                else if (!Tools_1.FormatTool.IsTextFormatted(myCity)) {
                    warningMessage = Tools_1.AppTool.IsNullOrEmpty(warningMessage) ? "City" : warningMessage + ",City";
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(warningMessage)) {
                    warningMessage += " is required";
                }
            }
        }
        this.ShipperWarning = warningMessage;
    };
    PartnersTabComponent.prototype.Validate_CON = function () {
        var warningMessage = "";
        if (this.ConsigneeId != null) {
            if (this.ConsigneeAddressList == null) {
                warningMessage = "Address is required";
            }
            else {
                var myAddressList = this.ConsigneeAddressList;
                var myAddress1 = Tools_1.AppTool.IsNullOrEmpty(myAddressList.Address1) ? null : myAddressList.Address1.trim();
                var myAddress2 = Tools_1.AppTool.IsNullOrEmpty(myAddressList.Address2) ? null : myAddressList.Address2.trim();
                var myZipCode = Tools_1.AppTool.IsNullOrEmpty(myAddressList.ZipCode) ? null : myAddressList.ZipCode.trim();
                var myCity = Tools_1.AppTool.IsNullOrEmpty(myAddressList.City) ? null : myAddressList.City.trim();
                if (!AWBUtilities_1.AWBUtilities.IsText(myAddress1)) {
                    warningMessage = Tools_1.AppTool.IsNullOrEmpty(warningMessage) ? "Address1" : warningMessage + ",Address1";
                }
                if (!AWBUtilities_1.AWBUtilities.IsText(myAddress2)) {
                    warningMessage = Tools_1.AppTool.IsNullOrEmpty(warningMessage) ? "Address2" : warningMessage + ",Address2";
                }
                if (Tools_1.AppTool.IsNullOrEmpty(myAddress1) && Tools_1.AppTool.IsNullOrEmpty(myAddress2)) {
                    warningMessage = Tools_1.AppTool.IsNullOrEmpty(warningMessage) ? "Address1 or Address2" : warningMessage + ",Address1 or Address2";
                }
                if (Tools_1.AppTool.IsNullOrEmpty(myAddressList.CountryId)) {
                    warningMessage = Tools_1.AppTool.IsNullOrEmpty(warningMessage) ? "Country" : warningMessage + ",Country";
                }
                if (Tools_1.AppTool.IsNullOrEmpty(myAddressList.StateCode)) {
                    if (this.Wizard.AllStates.filter(function (d) { return d.CountryId == myAddressList.CountryId; }).length > 0) {
                        warningMessage = Tools_1.AppTool.IsNullOrEmpty(warningMessage) ? "State" : warningMessage + ",State";
                    }
                }
                if (Tools_1.AppTool.IsNullOrEmpty(myCity)) {
                    warningMessage = Tools_1.AppTool.IsNullOrEmpty(warningMessage) ? "City" : warningMessage + ",City";
                }
                else if (!AWBUtilities_1.AWBUtilities.IsText(myCity)) {
                    warningMessage = Tools_1.AppTool.IsNullOrEmpty(warningMessage) ? "City" : warningMessage + ",City";
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(warningMessage)) {
                    warningMessage += " is required";
                }
            }
        }
        this.ConsigneeWarning = warningMessage;
    };
    PartnersTabComponent.prototype.Validate_AGT = function () {
        this.ShowWarningAgentId = this.IssuingCarrierAgentId == null ? true : false;
        this.ShowWarningAgentIATA = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.IssuingCarrierIATACode)) {
            if (!AWBUtilities_1.AWBUtilities.FormateValidate_IATACode(this.IssuingCarrierIATACode)) {
                this.ShowWarningAgentIATA = true;
            }
        }
        this.ShowWarningAgentCASS = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.CASSCode)) {
            if (!AWBUtilities_1.AWBUtilities.FormateValidate_CASSCode(this.CASSCode)) {
                this.ShowWarningAgentCASS = true;
            }
        }
        var warningMessage = "";
        if (this.IssuingCarrierAgentId == null) {
            warningMessage = "Issuing Carrier Agent is required";
        }
        else {
            if (this.IssuingCarrierAddressList == null) {
                warningMessage = "Address is required";
            }
            else {
                var myCity = Tools_1.AppTool.IsNullOrEmpty(this.IssuingCarrierAddressList.City) ? null : this.IssuingCarrierAddressList.City.trim();
                if (Tools_1.AppTool.IsNullOrEmpty(myCity)) {
                    warningMessage = Tools_1.AppTool.IsNullOrEmpty(warningMessage) ? "City" : warningMessage + ",City";
                }
                else if (!AWBUtilities_1.AWBUtilities.IsText(myCity)) {
                    warningMessage = Tools_1.AppTool.IsNullOrEmpty(warningMessage) ? "City" : warningMessage + ",City";
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(warningMessage)) {
                    warningMessage += " is required";
                }
            }
        }
        this.AgentWarning = warningMessage;
    };
    Object.defineProperty(PartnersTabComponent.prototype, "ShipperId", {
        // Shipper
        get: function () { return this.EntityPM.ShipperId; },
        set: function (newValue) {
            if (this.EntityPM.ShipperId != newValue) {
                this.EntityPM.ShipperId = newValue;
                this.SetUIProperties();
                this.GetShipperCard();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnersTabComponent.prototype, "ShipperAddressId", {
        get: function () { return this.EntityPM.ShipperAddressId; },
        set: function (newValue) {
            if (this.EntityPM.ShipperAddressId != newValue) {
                this.EntityPM.ShipperAddressId = newValue;
                this.GetShipperAddress();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnersTabComponent.prototype, "ShipperReference", {
        get: function () { return this.EntityPM.ShipperReference; },
        set: function (newValue) {
            if (this.EntityPM.ShipperReference != newValue) {
                this.EntityPM.ShipperReference = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    PartnersTabComponent.prototype.GetShipperCard = function () {
        var _this = this;
        if (this.ShipperId == null) {
            this.ShipperAddressId = null;
            this.ShipperReference = null;
            this.ShipperAddressList = null;
            this.EntityPM.ShipperName = null;
            this.FireWizardEvent();
        }
        else {
            this.myCardListService.getSingle(this.ShipperId).subscribe(function (myResult) {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    var myCard = myResponse.Result;
                    if (myCard != null) {
                        _this.EntityPM.ShipperName = myCard.EnglishName;
                        _this.GetShipperMainAddress();
                    }
                    else {
                        _this.LoadShipperCard();
                    }
                }
            });
        }
    };
    PartnersTabComponent.prototype.LoadShipperCard = function () {
        var _this = this;
        this.myCardListService.getSingle(this.ShipperId).subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                var myCard = myResponse.Result;
                if (myCard != null) {
                    _this.EntityPM.ShipperName = myCard.EnglishName;
                }
                _this.GetShipperMainAddress();
            }
        });
    };
    PartnersTabComponent.prototype.GetShipperAddress = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipperAddressId)) {
            var myService = new AddressListService_1.AddressListService();
            myService.getSingle(this.ShipperAddressId).subscribe(function (myResult) {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    var myAddress = myResponse.Result;
                    _this.SetShipperAddress(myAddress);
                }
            });
        }
    };
    PartnersTabComponent.prototype.GetShipperMainAddress = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipperId)) {
            var myService = new AddressService_1.AddressService();
            myService.GetMainAddressByCardId(this.ShipperId, this.Wizard.TenantPM.Id).subscribe(function (myResult) {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    var myAddress = myResponse.Result;
                    _this.SetShipperAddress(myAddress);
                }
            });
        }
    };
    PartnersTabComponent.prototype.SetShipperAddress = function (list) {
        this.ShipperAddressList = list;
        if (list == null) {
            if (this.EntityPM.ShipperAddressId != null) {
                this.EntityPM.ShipperAddressId = null;
            }
        }
        else {
            if (this.ShipperAddressId != list.Id) {
                this.ShipperAddressId = list.Id;
            }
            if (this.EntityPM.ShipperAddressId != list.Id) {
                this.EntityPM.ShipperAddressId = list.Id;
            }
            if (this.EntityPM.ShipperAddress1 != list.Address1) {
                this.EntityPM.ShipperAddress1 = list.Address1;
            }
            if (this.EntityPM.ShipperAddress2 != list.Address2) {
                this.EntityPM.ShipperAddress2 = list.Address2;
            }
            if (this.EntityPM.ShipperCity != list.City) {
                this.EntityPM.ShipperCity = list.City;
            }
            if (this.EntityPM.ShipperCountryId != list.CountryId) {
                this.EntityPM.ShipperCountryId = list.CountryId;
            }
            if (this.EntityPM.ShipperStateId != list.StateId) {
                this.EntityPM.ShipperStateId = list.StateId;
            }
            if (this.EntityPM.ShipperZipCode != list.ZipCode) {
                this.EntityPM.ShipperZipCode = list.ZipCode;
            }
        }
        this.FireWizardEvent();
    };
    Object.defineProperty(PartnersTabComponent.prototype, "ConsigneeId", {
        // Consignee
        get: function () { return this.EntityPM.ConsigneeId; },
        set: function (newValue) {
            if (this.EntityPM.ConsigneeId != newValue) {
                this.EntityPM.ConsigneeId = newValue;
                this.SetUIProperties();
                this.GetConsigneeCard();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnersTabComponent.prototype, "ConsigneeAddressId", {
        get: function () { return this.EntityPM.ConsigneeAddressId; },
        set: function (newValue) {
            if (this.EntityPM.ConsigneeAddressId != newValue) {
                this.EntityPM.ConsigneeAddressId = newValue;
                this.GetConsigneeAddress();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnersTabComponent.prototype, "ConsigneeReference", {
        get: function () { return this.EntityPM.ConsigneeReference; },
        set: function (newValue) {
            if (this.EntityPM.ConsigneeReference != newValue) {
                this.EntityPM.ConsigneeReference = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    PartnersTabComponent.prototype.GetConsigneeCard = function () {
        var _this = this;
        if (this.ConsigneeId == null) {
            this.ConsigneeAddressId = null;
            this.ConsigneeReference = null;
            this.ConsigneeAddressList = null;
            this.EntityPM.ConsigneeName = null;
            this.FireWizardEvent();
        }
        else {
            this.myCardListService.getSingle(this.ConsigneeId).subscribe(function (myResult) {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    var myCard = myResponse.Result;
                    if (myCard != null) {
                        _this.EntityPM.ConsigneeName = myCard.EnglishName;
                        _this.GetConsigneeMainAddress();
                    }
                    else {
                        _this.LoadConsigneeCard();
                    }
                }
            });
        }
    };
    PartnersTabComponent.prototype.LoadConsigneeCard = function () {
        var _this = this;
        this.myCardListService.getSingle(this.ConsigneeId).subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                var myCard = myResponse.Result;
                if (myCard != null) {
                    _this.EntityPM.ConsigneeName = myCard.EnglishName;
                }
                _this.GetConsigneeMainAddress();
            }
        });
    };
    PartnersTabComponent.prototype.GetConsigneeAddress = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ConsigneeAddressId)) {
            var myService = new AddressListService_1.AddressListService();
            myService.getSingle(this.ConsigneeAddressId).subscribe(function (myResult) {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    var myAddress = myResponse.Result;
                    _this.SetConsigneeAddress(myAddress);
                }
            });
        }
    };
    PartnersTabComponent.prototype.GetConsigneeMainAddress = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ConsigneeId)) {
            var myService = new AddressService_1.AddressService();
            myService.GetMainAddressByCardId(this.ConsigneeId, this.Wizard.TenantPM.Id).subscribe(function (myResult) {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    var myAddress = myResponse.Result;
                    _this.SetConsigneeAddress(myAddress);
                }
            });
        }
    };
    PartnersTabComponent.prototype.SetConsigneeAddress = function (list) {
        this.ConsigneeAddressList = list;
        if (list == null) {
            if (this.EntityPM.ConsigneeAddressId != null) {
                this.EntityPM.ConsigneeAddressId = null;
            }
        }
        else {
            if (this.EntityPM.ConsigneeAddressId != list.Id) {
                this.EntityPM.ConsigneeAddressId = list.Id;
            }
            if (this.EntityPM.ConsigneeAddress1 != list.Address1) {
                this.EntityPM.ConsigneeAddress1 = list.Address1;
            }
            if (this.EntityPM.ConsigneeAddress2 != list.Address2) {
                this.EntityPM.ConsigneeAddress2 = list.Address2;
            }
            if (this.EntityPM.ConsigneeCity != list.City) {
                this.EntityPM.ConsigneeCity = list.City;
            }
            if (this.EntityPM.ConsigneeCountryId != list.CountryId) {
                this.EntityPM.ConsigneeCountryId = list.CountryId;
            }
            if (this.EntityPM.ConsigneeStateId != list.StateId) {
                this.EntityPM.ConsigneeStateId = list.StateId;
            }
            if (this.EntityPM.ConsigneeZipCode != list.ZipCode) {
                this.EntityPM.ConsigneeZipCode = list.ZipCode;
            }
        }
        this.FireWizardEvent();
    };
    Object.defineProperty(PartnersTabComponent.prototype, "IssuingCarrierAgentId", {
        // IssuingCarrier
        get: function () { return this.EntityPM.IssuingCarrierAgentId; },
        set: function (newValue) {
            if (this.EntityPM.IssuingCarrierAgentId != newValue) {
                this.EntityPM.IssuingCarrierAgentId = newValue;
                this.SetUIProperties();
                this.GetIssuingCarrierCard();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnersTabComponent.prototype, "IssuingCarrierAddressId", {
        get: function () { return this.EntityPM.IssuingCarrierAddressId; },
        set: function (newValue) {
            if (this.EntityPM.IssuingCarrierAddressId != newValue) {
                this.EntityPM.IssuingCarrierAddressId = newValue;
                this.GetIssuingCarrierAddress();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnersTabComponent.prototype, "IssuingCarrierIATACode", {
        get: function () { return this.EntityPM.IssuingCarrierIATACode; },
        set: function (newValue) {
            if (this.EntityPM.IssuingCarrierIATACode != newValue) {
                this.EntityPM.IssuingCarrierIATACode = newValue;
                this.FireWizardEvent();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnersTabComponent.prototype, "CASSCode", {
        get: function () { return this.EntityPM.CASSCode; },
        set: function (newValue) {
            if (this.EntityPM.CASSCode != newValue) {
                this.EntityPM.CASSCode = newValue;
                this.FireWizardEvent();
            }
        },
        enumerable: true,
        configurable: true
    });
    PartnersTabComponent.prototype.GetIssuingCarrierCard = function () {
        var _this = this;
        if (this.IssuingCarrierAgentId == null) {
            this.IssuingCarrierAddressId = null;
            this.IssuingCarrierAddressList = null;
            this.EntityPM.IssuingCarrierAgentName = null;
            this.FireWizardEvent();
        }
        else {
            this.myCardListService.getSingle(this.IssuingCarrierAgentId).subscribe(function (myResult) {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    var myCard = myResponse.Result;
                    if (myCard != null) {
                        _this.EntityPM.IssuingCarrierAgentName = myCard.EnglishName;
                        _this.CASSCode = myCard.CASSCode;
                        _this.IssuingCarrierIATACode = myCard.IATACode;
                        _this.GetIssuingCarrierMainAddress();
                    }
                    else {
                        _this.LoadIssuingCarrierCard();
                    }
                }
            });
        }
    };
    PartnersTabComponent.prototype.LoadIssuingCarrierCard = function () {
        var _this = this;
        this.myCardListService.getSingle(this.IssuingCarrierAgentId).subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                var myCard = myResponse.Result;
                if (myCard != null) {
                    _this.EntityPM.IssuingCarrierAgentName = myCard.EnglishName;
                    _this.CASSCode = myCard.CASSCode;
                    _this.IssuingCarrierIATACode = myCard.IATACode;
                }
                _this.GetIssuingCarrierMainAddress();
            }
        });
    };
    PartnersTabComponent.prototype.GetIssuingCarrierAddress = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.IssuingCarrierAddressId)) {
            var myService = new AddressListService_1.AddressListService();
            myService.getSingle(this.IssuingCarrierAddressId).subscribe(function (myResult) {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    var myAddress = myResponse.Result;
                    _this.SetIssuingCarrierAddress(myAddress);
                }
            });
        }
    };
    PartnersTabComponent.prototype.GetIssuingCarrierMainAddress = function () {
        var _this = this;
        if (!this.isSetDefaultTenantAgent) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.IssuingCarrierAgentId)) {
                var myService = new AddressService_1.AddressService();
                myService.GetMainAddressByCardId(this.IssuingCarrierAgentId, this.Wizard.TenantPM.Id).subscribe(function (myResult) {
                    var myResponse = myResult;
                    if (!myResponse.HasError) {
                        var myAddress = myResponse.Result;
                        _this.SetIssuingCarrierAddress(myAddress);
                    }
                });
            }
        }
    };
    PartnersTabComponent.prototype.SetIssuingCarrierAddress = function (list) {
        this.IssuingCarrierAddressList = list;
        if (list == null) {
            if (this.EntityPM.IssuingCarrierAddressId != null) {
                this.EntityPM.IssuingCarrierAddressId = null;
            }
        }
        else {
            if (this.EntityPM.IssuingCarrierAddressId != list.Id) {
                this.EntityPM.IssuingCarrierAddressId = list.Id;
            }
        }
        this.FireWizardEvent();
    };
    PartnersTabComponent.prototype.Add = function (myPartnerTypeCode) {
        this.AddEdit(myPartnerTypeCode, true);
    };
    PartnersTabComponent.prototype.Edit = function (myPartnerTypeCode) {
        this.AddEdit(myPartnerTypeCode, false);
    };
    PartnersTabComponent.prototype.AddEdit = function (myPartnerTypeCode, isNewPartner) {
        var _this = this;
        if (!this.isPartnerWindowOpened) {
            this.isPartnerWindowOpened = true;
            var myTitle = isNewPartner ? "Add " : "Edit ";
            switch (myPartnerTypeCode) {
                case "SHI": {
                    myTitle += "Shipper";
                    break;
                }
                case "CON": {
                    myTitle += "Consignee";
                    break;
                }
                case "AGT": {
                    myTitle += "Issuing Carrier's Agent";
                    break;
                }
            }
            var windowArgs = new Args_1.AddEditPartnerArgs();
            windowArgs.EntityPM = this.EntityPM;
            windowArgs.IsNewEntity = isNewPartner;
            windowArgs.PartnerTypeCode = myPartnerTypeCode;
            windowArgs.FatherComponent = this;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Title = myTitle;
            logWindow.WindowArgs = windowArgs;
            logWindow.Show('./Booking/Components/BookingWizard/Partners/AddEditPartnerComponent');
            logWindow.WindowClosed.subscribe(function ($event) {
                _this.isPartnerWindowOpened = false;
            });
        }
    };
    PartnersTabComponent.prototype.UpdatePartner = function (myPartnerTypeCode, myPartnerId, myAddressId) {
        switch (myPartnerTypeCode) {
            case "SHI":
                {
                    if (this.ShipperId != myPartnerId) {
                        this.ShipperId = myPartnerId;
                    }
                    else {
                        this.EntityPM.ShipperAddressId = myAddressId;
                        this.LoadShipperCard();
                    }
                    break;
                }
            case "CON":
                {
                    if (this.ConsigneeId != myPartnerId) {
                        this.ConsigneeId = myPartnerId;
                    }
                    else {
                        this.ConsigneeId = null;
                        this.ConsigneeId = myPartnerId;
                        this.EntityPM.ConsigneeAddressId = myAddressId;
                        this.LoadConsigneeCard();
                    }
                    break;
                }
            case "AGT":
                {
                    if (this.IssuingCarrierAgentId != myPartnerId) {
                        this.IssuingCarrierAgentId = myPartnerId;
                    }
                    else {
                        this.EntityPM.IssuingCarrierAddressId = myAddressId;
                        this.LoadIssuingCarrierCard();
                    }
                    break;
                }
        }
    };
    PartnersTabComponent = __decorate([
        core_1.Component({
            selector: 'PartnersTabComponent',
            moduleId: module.id,
            templateUrl: './PartnersTabComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], PartnersTabComponent);
    return PartnersTabComponent;
}(BaseComponent_1.BaseComponent));
exports.PartnersTabComponent = PartnersTabComponent;
//# sourceMappingURL=PartnersTabComponent.js.map