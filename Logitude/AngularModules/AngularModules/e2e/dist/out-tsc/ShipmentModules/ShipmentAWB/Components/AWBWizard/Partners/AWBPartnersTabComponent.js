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
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var CardListService_1 = require("../../../../../Common/Services/StandardLists/CardListService");
var AddressListService_1 = require("../../../../../Common/Services/StandardLists/AddressListService");
var AddressService_1 = require("../../../../../Common/Services/ExtendedLists/AddressService");
var FeatureLocator_1 = require("../../../../../Infrastructure/Utilities/FeatureLocator");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var ConfirmWindow_1 = require("../../../../../Controls/Windows/ConfirmWindow");
var Tools_2 = require("../../../../../Shipment/Tools");
var Args_1 = require("../../../../../Shipment/Args");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var AWBPartnersTabComponent = /** @class */ (function (_super) {
    __extends(AWBPartnersTabComponent, _super);
    function AWBPartnersTabComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.LabelColumnWidth = 85;
        _this.PartnerBoxHeight = 200;
        _this.IsFirstTime = true;
        _this.IsEditingEnabled = false;
        _this.IsEditSHIEnabled = false;
        _this.IsEditCONEnabled = false;
        _this.IsEditNTFEnabled = false;
        _this.ViaColoaderIsVisible = false;
        _this.ViaColoaderHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.IssuingCarrierAgentId");
        _this.IsBookingConnectWarningVisible = false;
        _this.isSetDefaultTenantAgent = false;
        _this.ShipperWarning = "";
        _this.ConsigneeWarning = "";
        _this.Notify1Warning = "";
        _this.AgentWarning = "";
        _this.ShowWarningConsigneeId = false;
        _this.ShowWarningShipperAddressId = false;
        _this.ShowWarningConsigneeAddressId = false;
        _this.ShowWarningNotify1AddressId = false;
        _this.ShowWarningAgentId = false;
        _this.ShowWarningAgentAddressId = false;
        _this.ShowWarningAgentIATA = false;
        _this.ShowWarningAgentCASS = false;
        _this.ShowWarningAgentReference = false;
        _this.InitializeServices();
        return _this;
    }
    AWBPartnersTabComponent.prototype.InitializeServices = function () {
        if (this.myCardListService == null) {
            this.myCardListService = new CardListService_1.CardListService();
        }
    };
    AWBPartnersTabComponent.prototype.InitTab = function (wizard) {
        this.Wizard = wizard;
        this.EntityPM = this.Wizard.EntityPM;
        this.ObjectTableName = this.Wizard.ObjectTableName;
        this.SetIssuingCarrier();
        this.Listen();
        this.SetWarningInfo();
        this.SetLOVDependency();
        this.SetUIProperties();
        this.InitializePartners();
        this.Validate();
    };
    AWBPartnersTabComponent.prototype.RefreshTab = function () {
        this.Validate();
        this.SetUIProperties();
    };
    AWBPartnersTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.Wizard != null) {
            this.Wizard.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.Wizard.EntityPM;
                    _this.SetUIProperties();
                }
            });
            this.Wizard.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.Wizard.EntityPM;
                    _this.SetUIProperties();
                }
            });
        }
    };
    AWBPartnersTabComponent.prototype.SetUIProperties = function () {
        this.IsEditingEnabled = Tools_2.ShipmentTool.IsEditingEnabled(this.EntityPM);
        this.SetUIProperties_Shipper();
        this.SetUIProperties_Consignee();
        this.SetUIProperties_Notify1();
        this.SetUIProperties_IssuingCarrier();
    };
    AWBPartnersTabComponent.prototype.SetUIProperties_Shipper = function () {
        var isFieldFilled = this.ShipperId == null ? false : true;
        this.IsEditSHIEnabled = this.IsEditingEnabled;
        if (this.IsEditSHIEnabled) {
            this.IsEditSHIEnabled = isFieldFilled;
        }
        if (!this.Wizard.IsImportWizard) {
            this.UIProperties.SetRequired("ShipperId", this.ObjectTableName, !isFieldFilled);
        }
        this.UIProperties.SetEnabled("ShipperId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("ShipperReference1", this.ObjectTableName, this.IsEditingEnabled);
    };
    AWBPartnersTabComponent.prototype.SetUIProperties_Consignee = function () {
        var isFieldFilled = this.ConsigneeId == null ? false : true;
        this.IsEditCONEnabled = this.IsEditingEnabled;
        if (this.IsEditCONEnabled) {
            this.IsEditCONEnabled = isFieldFilled;
        }
        if (!this.Wizard.IsImportWizard) {
            this.UIProperties.SetEnabled("ConsigneeId", this.ObjectTableName, this.IsEditingEnabled);
        }
        else {
            this.UIProperties.SetRequired("ConsigneeId", this.ObjectTableName, !isFieldFilled);
        }
        this.UIProperties.SetEnabled("ConsigneeReference1", this.ObjectTableName, this.IsEditingEnabled);
    };
    AWBPartnersTabComponent.prototype.SetUIProperties_Notify1 = function () {
        var isFieldFilled = this.Notify1Id == null ? false : true;
        this.IsEditNTFEnabled = this.IsEditingEnabled;
        if (this.IsEditNTFEnabled) {
            this.IsEditNTFEnabled = isFieldFilled;
        }
        this.UIProperties.SetEnabled("Notify1Id", this.ObjectTableName, this.IsEditingEnabled);
    };
    AWBPartnersTabComponent.prototype.SetUIProperties_IssuingCarrier = function () {
        var isAgentFieldEnabled = this.IsEditingEnabled;
        if (isAgentFieldEnabled) {
            isAgentFieldEnabled = this.EntityPM.ViaColoader ? true : false;
        }
        if (SessionLocator_1.SessionLocator.TenantManagementJS.AWBMessagesCCSTypeCode == "GLSHK") {
            this.ViaColoaderIsVisible = true;
            this.ViaColoaderHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.S.Partners.IssuingCarrierColoader");
        }
        else {
            if (SessionLocator_1.SessionLocator.TenantManagementJS.PackageCode != "BUBK" && SessionLocator_1.SessionLocator.TenantManagementJS.PackageCode != "EAWB" && SessionLocator_1.SessionLocator.TenantManagementJS.PackageCode != "EACR") {
                this.ViaColoaderIsVisible = true;
                this.ViaColoaderHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.S.Partners.IssuingCarrierColoader");
            }
        }
        if (!this.Wizard.IsImportWizard) {
            this.UIProperties.SetEnabled("IssuingCarrierAgentId", this.ObjectTableName, isAgentFieldEnabled);
            this.UIProperties.SetEnabled("IssuingCarrierAddressId", this.ObjectTableName, isAgentFieldEnabled);
            this.UIProperties.SetEnabled("IssuingCarrierIATACode", this.ObjectTableName, isAgentFieldEnabled);
            this.UIProperties.SetEnabled("CASSCode", this.ObjectTableName, isAgentFieldEnabled);
            this.UIProperties.SetEnabled("IssuingCarrierReference1", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetEnabled("ViaColoader", this.ObjectTableName, this.IsEditingEnabled);
        }
    };
    AWBPartnersTabComponent.prototype.SetWarningInfo = function () {
        var myResult = false;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Booking", "Module")) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                if (this.EntityPM.ShipmentLevelCode != "H" && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.BookingId)) {
                    myResult = true;
                }
            }
        }
        this.IsBookingConnectWarningVisible = myResult;
        this.PartnerBoxHeight = myResult == true ? 200 : 213;
    };
    AWBPartnersTabComponent.prototype.InitializePartners = function () {
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
        if (!Tools_1.AppTool.IsNullOrEmpty(this.Notify1Id)) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.Notify1AddressId)) {
                this.GetNotify1Address();
            }
            else {
                this.GetNotify1MainAddress();
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
        this.SetDefaultIssuingCarrierAgent();
    };
    AWBPartnersTabComponent.prototype.SetDefaultIssuingCarrierAgent = function () {
        if (!this.EntityPM.IsCopyFromShipment) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                this.isSetDefaultTenantAgent = true;
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.BookingId)) {
                    this.CASSCode = this.Wizard.TenantPM.CASSCode;
                    this.IssuingCarrierIATACode = this.Wizard.TenantPM.IATA;
                }
                this.RegulatedAgentRANumber = this.Wizard.TenantPM.RegulatedAgentNumber;
                if (!this.ViaColoader) {
                    this.IssuingCarrierAgentId = this.Wizard.TenantPM.AgentId;
                    if (this.EntityPM.ShipmentLevelCode == "C") {
                        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.BookingId)) {
                            this.ShipperId = this.IssuingCarrierAgentId;
                        }
                        else {
                            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ShipperId)) {
                                this.ShipperId = this.IssuingCarrierAgentId;
                            }
                        }
                    }
                }
            }
            else {
                if (this.IssuingCarrierAgentId == this.Wizard.TenantPM.AgentId) {
                    this.isSetDefaultTenantAgent = true;
                }
            }
        }
    };
    AWBPartnersTabComponent.prototype.SetLOVDependency = function () {
        var myDependency = "CS";
        var myDependencyIsList = false;
        if (this.EntityPM.ShipmentLevelCode == "C") {
            myDependency = "AG";
            myDependencyIsList = false;
        }
        else {
            if (this.Wizard.TenantPM.AllowAgentInCustomersLOV) {
                myDependency = "CS,AG";
                myDependencyIsList = true;
            }
        }
        this.PartnerDependencyProperty1 = myDependency;
        this.PartnerDependencyProperty1IsList = myDependencyIsList;
    };
    AWBPartnersTabComponent.prototype.FireWizardEvent = function () {
        this.Wizard.ValidateScreen_PAR();
    };
    AWBPartnersTabComponent.prototype.Validate = function () {
        if (!this.Wizard.IsImportWizard) {
            this.Validate_SHI();
            this.Validate_CON();
            this.Validate_AGT();
            this.Validate_NTF();
        }
    };
    AWBPartnersTabComponent.prototype.Validate_SHI = function () {
        if (!this.Wizard.IsImportWizard) {
            var warningMessage = "";
            var showAddressWarning = false;
            if (this.ShipperId == null) {
                warningMessage = "Shipper is required";
            }
            else {
                if (this.ShipperAddressId == null) {
                    warningMessage = "Address is required";
                }
                else {
                    var myAddressList = this.ShipperAddressList;
                    if (myAddressList != null) {
                        var myAddress1 = Tools_1.AppTool.IsNullOrEmpty(myAddressList.Address1) ? null : myAddressList.Address1.trim();
                        var myAddress2 = Tools_1.AppTool.IsNullOrEmpty(myAddressList.Address2) ? null : myAddressList.Address2.trim();
                        var myZipCode = Tools_1.AppTool.IsNullOrEmpty(myAddressList.ZipCode) ? null : myAddressList.ZipCode.trim();
                        var myCity = Tools_1.AppTool.IsNullOrEmpty(myAddressList.City) ? null : myAddressList.City.trim();
                        var myFaxNumber = Tools_1.AppTool.IsNullOrEmpty(myAddressList.FaxNumber) ? null : myAddressList.FaxNumber.trim();
                        var myPhoneNumber = Tools_1.AppTool.IsNullOrEmpty(myAddressList.PhoneNumber) ? null : myAddressList.PhoneNumber.trim();
                        if (!Tools_1.FormatTool.IsTextFormatted(myAddress1)) {
                            warningMessage = Tools_1.AppTool.IsNullOrEmpty(warningMessage) ? "Address1" : warningMessage + ",Address1";
                        }
                        if (!Tools_1.FormatTool.IsTextFormatted(myAddress2)) {
                            warningMessage = Tools_1.AppTool.IsNullOrEmpty(warningMessage) ? "Address2" : warningMessage + ",Address2";
                        }
                        if (Tools_1.AppTool.IsNullOrEmpty(myAddress1) && Tools_1.AppTool.IsNullOrEmpty(myAddress2)) {
                            warningMessage = Tools_1.AppTool.IsNullOrEmpty(warningMessage) ? "Address1 or Address2" : warningMessage + ",Address1 or Address2";
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
                        if (Tools_1.AppTool.IsNullOrEmpty(myZipCode)) {
                            warningMessage = Tools_1.AppTool.IsNullOrEmpty(warningMessage) ? "Zip Code" : warningMessage + ",Zip Code";
                        }
                        else if (!Tools_1.FormatTool.IsTextFormatted(myZipCode)) {
                            warningMessage = Tools_1.AppTool.IsNullOrEmpty(warningMessage) ? "Zip Code" : warningMessage + ",Zip Code";
                        }
                        if (this.EntityPM.MainCarriageFinalDestinationPortCountryCode == "CN") {
                            if (Tools_1.AppTool.IsNullOrEmpty(myFaxNumber) && Tools_1.AppTool.IsNullOrEmpty(myPhoneNumber)) {
                                warningMessage = Tools_1.AppTool.IsNullOrEmpty(warningMessage) ? "Phone Or Fax" : warningMessage + ",Phone Or Fax";
                            }
                        }
                    }
                    if (!Tools_1.AppTool.IsNullOrEmpty(warningMessage)) {
                        warningMessage += " is required";
                    }
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(warningMessage)) {
                    showAddressWarning = true;
                }
            }
            this.ShipperWarning = warningMessage;
            this.ShowWarningShipperAddressId = showAddressWarning;
        }
    };
    AWBPartnersTabComponent.prototype.Validate_CON = function () {
        if (!this.Wizard.IsImportWizard) {
            this.ShowWarningConsigneeId = this.ConsigneeId == null ? true : false;
            var warningMessage = "";
            var showAddressWarning = false;
            if (this.ConsigneeId == null) {
                warningMessage = "Consignee is required";
            }
            else {
                if (this.ConsigneeAddressId == null) {
                    warningMessage = "Address is required";
                }
                else {
                    var myAddressList = this.ConsigneeAddressList;
                    if (myAddressList != null) {
                        var myAddress1 = Tools_1.AppTool.IsNullOrEmpty(myAddressList.Address1) ? null : myAddressList.Address1.trim();
                        var myAddress2 = Tools_1.AppTool.IsNullOrEmpty(myAddressList.Address2) ? null : myAddressList.Address2.trim();
                        var myZipCode = Tools_1.AppTool.IsNullOrEmpty(myAddressList.ZipCode) ? null : myAddressList.ZipCode.trim();
                        var myCity = Tools_1.AppTool.IsNullOrEmpty(myAddressList.City) ? null : myAddressList.City.trim();
                        var myFaxNumber = Tools_1.AppTool.IsNullOrEmpty(myAddressList.FaxNumber) ? null : myAddressList.FaxNumber.trim();
                        var myPhoneNumber = Tools_1.AppTool.IsNullOrEmpty(myAddressList.PhoneNumber) ? null : myAddressList.PhoneNumber.trim();
                        if (!Tools_1.FormatTool.IsTextFormatted(myAddress1)) {
                            warningMessage = Tools_1.AppTool.IsNullOrEmpty(warningMessage) ? "Address1" : warningMessage + ",Address1";
                        }
                        if (!Tools_1.FormatTool.IsTextFormatted(myAddress2)) {
                            warningMessage = Tools_1.AppTool.IsNullOrEmpty(warningMessage) ? "Address2" : warningMessage + ",Address2";
                        }
                        if (Tools_1.AppTool.IsNullOrEmpty(myAddress1) && Tools_1.AppTool.IsNullOrEmpty(myAddress2)) {
                            warningMessage = Tools_1.AppTool.IsNullOrEmpty(warningMessage) ? "Address1 or Address2" : warningMessage + ",Address1 or Address2";
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
                        if (Tools_1.AppTool.IsNullOrEmpty(myZipCode)) {
                            warningMessage = Tools_1.AppTool.IsNullOrEmpty(warningMessage) ? "Zip Code" : warningMessage + ",Zip Code";
                        }
                        else if (!Tools_1.FormatTool.IsTextFormatted(myZipCode)) {
                            warningMessage = Tools_1.AppTool.IsNullOrEmpty(warningMessage) ? "Zip Code" : warningMessage + ",Zip Code";
                        }
                        if (this.EntityPM.MainCarriageFinalDestinationPortCountryCode == "CN") {
                            if (Tools_1.AppTool.IsNullOrEmpty(myFaxNumber) && Tools_1.AppTool.IsNullOrEmpty(myPhoneNumber)) {
                                warningMessage = Tools_1.AppTool.IsNullOrEmpty(warningMessage) ? "Phone Or Fax" : warningMessage + ",Phone Or Fax";
                            }
                        }
                    }
                    if (!Tools_1.AppTool.IsNullOrEmpty(warningMessage)) {
                        warningMessage += " is required";
                    }
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(warningMessage)) {
                    showAddressWarning = true;
                }
            }
            this.ConsigneeWarning = warningMessage;
            this.ShowWarningConsigneeAddressId = showAddressWarning;
        }
    };
    AWBPartnersTabComponent.prototype.Validate_AGT = function () {
        if (!this.Wizard.IsImportWizard) {
            if (this.Wizard.IsFWB) {
                this.ShowWarningAgentId = this.IssuingCarrierAgentId == null ? true : false;
                this.ShowWarningAgentAddressId = false;
                this.ShowWarningAgentReference = false;
                if (this.IssuingCarrierAgentId != null) {
                    if (this.ViaColoader) {
                        if (this.Wizard.CCSTypeCode == "GLSHK") {
                            if (Tools_1.AppTool.IsNullOrEmpty(this.IssuingCarrierReference1)) {
                                this.ShowWarningAgentReference = true;
                            }
                        }
                    }
                }
                this.ShowWarningAgentIATA = false;
                if (!Tools_1.AppTool.IsNullOrEmpty(this.IssuingCarrierIATACode)) {
                    if (!Tools_1.FormatTool.Validate_IATACode(this.IssuingCarrierIATACode)) {
                        this.ShowWarningAgentIATA = true;
                    }
                }
                this.ShowWarningAgentCASS = false;
                if (!Tools_1.AppTool.IsNullOrEmpty(this.CASSCode)) {
                    if (!Tools_1.FormatTool.Validate_CASSCode(this.CASSCode)) {
                        this.ShowWarningAgentCASS = true;
                    }
                }
                var warningMessage = "";
                if (this.IssuingCarrierAgentId == null) {
                    warningMessage = "Issuing Carrier Agent is required";
                }
                else {
                    if (this.IssuingCarrierAddressId == null) {
                        warningMessage = "Address is required";
                    }
                    else {
                        if (this.IssuingCarrierAddressList != null) {
                            var myCity = Tools_1.AppTool.IsNullOrEmpty(this.IssuingCarrierAddressList.City) ? null : this.IssuingCarrierAddressList.City.trim();
                            if (Tools_1.AppTool.IsNullOrEmpty(myCity)) {
                                warningMessage = Tools_1.AppTool.IsNullOrEmpty(warningMessage) ? "City" : warningMessage + ",City";
                            }
                            else if (!Tools_1.FormatTool.IsTextFormatted(myCity)) {
                                warningMessage = Tools_1.AppTool.IsNullOrEmpty(warningMessage) ? "City" : warningMessage + ",City";
                            }
                        }
                        if (!Tools_1.AppTool.IsNullOrEmpty(warningMessage)) {
                            warningMessage += " is required";
                        }
                    }
                    if (!Tools_1.AppTool.IsNullOrEmpty(warningMessage)) {
                        this.ShowWarningAgentAddressId = true;
                    }
                }
                this.AgentWarning = warningMessage;
            }
        }
    };
    AWBPartnersTabComponent.prototype.Validate_NTF = function () {
        if (!this.Wizard.IsImportWizard) {
            var warningMessage = "";
            var showAddressWarning = false;
            if (this.Notify1Id != null) {
                if (this.Notify1AddressId == null) {
                    warningMessage = "Address is required";
                }
                else {
                    var myAddressList = this.Notify1AddressList;
                    if (myAddressList != null) {
                        var myAddress1 = Tools_1.AppTool.IsNullOrEmpty(myAddressList.Address1) ? null : myAddressList.Address1.trim();
                        var myAddress2 = Tools_1.AppTool.IsNullOrEmpty(myAddressList.Address2) ? null : myAddressList.Address2.trim();
                        var myZipCode = Tools_1.AppTool.IsNullOrEmpty(myAddressList.ZipCode) ? null : myAddressList.ZipCode.trim();
                        var myCity = Tools_1.AppTool.IsNullOrEmpty(myAddressList.City) ? null : myAddressList.City.trim();
                        var myFaxNumber = Tools_1.AppTool.IsNullOrEmpty(myAddressList.FaxNumber) ? null : myAddressList.FaxNumber.trim();
                        var myPhoneNumber = Tools_1.AppTool.IsNullOrEmpty(myAddressList.PhoneNumber) ? null : myAddressList.PhoneNumber.trim();
                        if (!Tools_1.FormatTool.IsTextFormatted(myAddress1)) {
                            warningMessage = Tools_1.AppTool.IsNullOrEmpty(warningMessage) ? "Address1" : warningMessage + ",Address1";
                        }
                        if (!Tools_1.FormatTool.IsTextFormatted(myAddress2)) {
                            warningMessage = Tools_1.AppTool.IsNullOrEmpty(warningMessage) ? "Address2" : warningMessage + ",Address2";
                        }
                        if (Tools_1.AppTool.IsNullOrEmpty(myAddress1) && Tools_1.AppTool.IsNullOrEmpty(myAddress2)) {
                            warningMessage = Tools_1.AppTool.IsNullOrEmpty(warningMessage) ? "Address1 or Address2" : warningMessage + ",Address1 or Address2";
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
                        if (Tools_1.AppTool.IsNullOrEmpty(myZipCode)) {
                            warningMessage = Tools_1.AppTool.IsNullOrEmpty(warningMessage) ? "Zip Code" : warningMessage + ",Zip Code";
                        }
                        else if (!Tools_1.FormatTool.IsTextFormatted(myZipCode)) {
                            warningMessage = Tools_1.AppTool.IsNullOrEmpty(warningMessage) ? "Zip Code" : warningMessage + ",Zip Code";
                        }
                        if (this.EntityPM.MainCarriageFinalDestinationPortCountryCode == "CN") {
                            if (Tools_1.AppTool.IsNullOrEmpty(myFaxNumber) && Tools_1.AppTool.IsNullOrEmpty(myPhoneNumber)) {
                                warningMessage = Tools_1.AppTool.IsNullOrEmpty(warningMessage) ? "Phone Or Fax" : warningMessage + ",Phone Or Fax";
                            }
                        }
                    }
                    if (!Tools_1.AppTool.IsNullOrEmpty(warningMessage)) {
                        warningMessage += " is required";
                    }
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(warningMessage)) {
                    showAddressWarning = true;
                }
            }
            this.Notify1Warning = warningMessage;
            this.ShowWarningNotify1AddressId = showAddressWarning;
        }
    };
    // Regualted Agent Field Changed
    AWBPartnersTabComponent.prototype.RAFieldChanged = function () {
        if (this.Wizard.IsTabVisible_RAD) {
            Tools_2.ShipmentTool.OnRegulatedAgentFieldChanged(this.EntityPM);
        }
    };
    Object.defineProperty(AWBPartnersTabComponent.prototype, "ShipperId", {
        get: function () { return this.EntityPM.ShipperId; },
        set: function (newValue) {
            if (this.EntityPM.ShipperId != newValue) {
                this.EntityPM.ShipperId = newValue;
                this.isPartnerChanged_Shipper = true;
                this.SetUIProperties_Shipper();
                this.GetShipperCard();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBPartnersTabComponent.prototype, "ShipperAddressId", {
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
    Object.defineProperty(AWBPartnersTabComponent.prototype, "ShipperReference1", {
        get: function () { return this.EntityPM.ShipperReference1; },
        set: function (newValue) {
            if (this.EntityPM.ShipperReference1 != newValue) {
                this.EntityPM.ShipperReference1 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    AWBPartnersTabComponent.prototype.GetShipperCard = function () {
        this.EntityPM.SalesmanUserId = this.EntityPM.CreatedByUserId;
        this.EntityPM.AccountManagerUserId = this.EntityPM.CreatedByUserId;
        if (this.ShipperId == null) {
            this.ShipperAddressId = null;
            this.ShipperReference1 = null;
            this.ShipperAddressList = null;
            this.EntityPM.ShipperName = null;
            this.EntityPM.ShipperNote = null;
            this.EntityPM.ShipperContactId = null;
            this.EntityPM.KnownConsignorNumber = null;
            this.EntityPM.KCExpirationDate = null;
            this.Validate_SHI();
            this.RAFieldChanged();
            this.FireWizardEvent();
        }
        else {
            this.LoadShipperCard();
        }
    };
    AWBPartnersTabComponent.prototype.LoadShipperCard = function () {
        var _this = this;
        this.myCardListService.getSingle(this.ShipperId).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var myCard = myResponse.Result;
                    if (myCard != null) {
                        _this.EntityPM.ShipperName = myCard.EnglishName;
                        _this.EntityPM.ShipperNote = myCard.Notes;
                        _this.EntityPM.ShipperContactId = myCard.PrimaryContactId;
                        _this.EntityPM.KnownConsignorNumber = myCard.KnownConsignor;
                        _this.EntityPM.KCExpirationDate = myCard.KCExpirationDate;
                        _this.RAFieldChanged();
                        if (myCard.SalesmanUserId != null) {
                            _this.EntityPM.SalesmanUserId = myCard.SalesmanUserId;
                        }
                        if (myCard.AccountManagerUserId != null) {
                            _this.EntityPM.AccountManagerUserId = myCard.AccountManagerUserId;
                        }
                    }
                    if (_this.isPartnerChanged_Shipper) {
                        _this.GetShipperMainAddress();
                    }
                    else {
                        _this.GetShipperAddress();
                    }
                    _this.isPartnerChanged_Shipper = false;
                }
            }
        });
    };
    AWBPartnersTabComponent.prototype.GetShipperAddress = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipperAddressId)) {
            var myService = new AddressListService_1.AddressListService();
            myService.getSingle(this.ShipperAddressId).subscribe(function (myResponse) {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        _this.SetShipperAddress(myResponse.Result);
                    }
                }
            });
        }
    };
    AWBPartnersTabComponent.prototype.GetShipperMainAddress = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipperId)) {
            var myService = new AddressService_1.AddressService();
            myService.GetMainAddressByCardId(this.ShipperId, this.Wizard.TenantPM.Id).subscribe(function (myResponse) {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        _this.SetShipperAddress(myResponse.Result);
                    }
                }
            });
        }
    };
    AWBPartnersTabComponent.prototype.SetShipperAddress = function (list) {
        this.ShipperAddressList = list;
        if (list == null) {
            if (this.EntityPM.ShipperAddressId != null) {
                this.EntityPM.ShipperAddressId = null;
            }
        }
        else {
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
            if (this.EntityPM.ShipperFaxNumber != list.FaxNumber) {
                this.EntityPM.ShipperFaxNumber = list.FaxNumber;
            }
            if (this.EntityPM.ShipperPhoneNumber != list.PhoneNumber) {
                this.EntityPM.ShipperPhoneNumber = list.PhoneNumber;
            }
        }
        this.Validate_SHI();
        this.FireWizardEvent();
        this.SetMyCustomer();
    };
    AWBPartnersTabComponent.prototype.SetMyCustomer = function () {
        if (this.EntityPM.ShipmentLevelCode != "C") {
            if (this.Wizard.IsNewEntity) {
                if (this.EntityPM.ShipmentCustomerTypeCode != "SHI") {
                    this.EntityPM.ShipmentCustomerTypeCode = "SHI";
                }
            }
            if (this.EntityPM.ShipmentCustomerTypeCode == "SHI") {
                if (this.EntityPM.CustomerId != this.EntityPM.ShipperId) {
                    this.EntityPM.CustomerId = this.EntityPM.ShipperId;
                }
                if (this.EntityPM.CustomerName != this.EntityPM.ShipperName) {
                    this.EntityPM.CustomerName = this.EntityPM.ShipperName;
                }
                if (this.EntityPM.CustomerNote != this.EntityPM.ShipperNote) {
                    this.EntityPM.CustomerNote = this.EntityPM.ShipperNote;
                }
                if (this.EntityPM.CustomerAddressId != this.EntityPM.ShipperAddressId) {
                    this.EntityPM.CustomerAddressId = this.EntityPM.ShipperAddressId;
                }
                if (this.EntityPM.CustomerContactId != this.EntityPM.ShipperContactId) {
                    this.EntityPM.CustomerContactId = this.EntityPM.ShipperContactId;
                }
            }
        }
    };
    Object.defineProperty(AWBPartnersTabComponent.prototype, "ConsigneeId", {
        get: function () { return this.EntityPM.ConsigneeId; },
        set: function (newValue) {
            if (this.EntityPM.ConsigneeId != newValue) {
                this.EntityPM.ConsigneeId = newValue;
                this.isPartnerChanged_Consignee = true;
                this.SetUIProperties_Consignee();
                this.GetConsigneeCard();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBPartnersTabComponent.prototype, "ConsigneeAddressId", {
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
    Object.defineProperty(AWBPartnersTabComponent.prototype, "ConsigneeReference1", {
        get: function () { return this.EntityPM.ConsigneeReference1; },
        set: function (newValue) {
            if (this.EntityPM.ConsigneeReference1 != newValue) {
                this.EntityPM.ConsigneeReference1 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    AWBPartnersTabComponent.prototype.GetConsigneeCard = function () {
        if (this.ConsigneeId == null) {
            this.ConsigneeAddressId = null;
            this.ConsigneeReference1 = null;
            this.ConsigneeAddressList = null;
            this.EntityPM.ConsigneeName = null;
            this.EntityPM.ConsigneeNote = null;
            this.EntityPM.ConsigneeContactId = null;
            this.Validate_CON();
            this.FireWizardEvent();
        }
        else {
            this.LoadConsigneeCard();
        }
    };
    AWBPartnersTabComponent.prototype.LoadConsigneeCard = function () {
        var _this = this;
        this.myCardListService.getSingle(this.ConsigneeId).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var myCard = myResponse.Result;
                    if (myCard != null) {
                        _this.EntityPM.ConsigneeName = myCard.EnglishName;
                        _this.EntityPM.ConsigneeNote = myCard.Notes;
                        _this.EntityPM.ConsigneeContactId = myCard.PrimaryContactId;
                    }
                    if (_this.isPartnerChanged_Consignee) {
                        _this.GetConsigneeMainAddress();
                    }
                    else {
                        _this.GetConsigneeAddress();
                    }
                    _this.isPartnerChanged_Consignee = false;
                }
            }
        });
    };
    AWBPartnersTabComponent.prototype.GetConsigneeAddress = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ConsigneeAddressId)) {
            var myService = new AddressListService_1.AddressListService();
            myService.getSingle(this.ConsigneeAddressId).subscribe(function (myResponse) {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        _this.SetConsigneeAddress(myResponse.Result);
                    }
                }
            });
        }
    };
    AWBPartnersTabComponent.prototype.GetConsigneeMainAddress = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ConsigneeId)) {
            var myService = new AddressService_1.AddressService();
            myService.GetMainAddressByCardId(this.ConsigneeId, this.Wizard.TenantPM.Id).subscribe(function (myResponse) {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        _this.SetConsigneeAddress(myResponse.Result);
                    }
                }
            });
        }
    };
    AWBPartnersTabComponent.prototype.SetConsigneeAddress = function (list) {
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
            if (this.EntityPM.ConsigneeFaxNumber != list.FaxNumber) {
                this.EntityPM.ConsigneeFaxNumber = list.FaxNumber;
            }
            if (this.EntityPM.ConsigneePhoneNumber != list.PhoneNumber) {
                this.EntityPM.ConsigneePhoneNumber = list.PhoneNumber;
            }
        }
        this.Validate_CON();
        this.FireWizardEvent();
    };
    Object.defineProperty(AWBPartnersTabComponent.prototype, "Notify1Id", {
        get: function () { return this.EntityPM.Notify1Id; },
        set: function (newValue) {
            if (this.EntityPM.Notify1Id != newValue) {
                this.EntityPM.Notify1Id = newValue;
                this.isPartnerChanged_Notify1 = true;
                this.SetUIProperties_Notify1();
                this.GetNotify1Card();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBPartnersTabComponent.prototype, "Notify1AddressId", {
        get: function () { return this.EntityPM.Notify1AddressId; },
        set: function (newValue) {
            if (this.EntityPM.Notify1AddressId != newValue) {
                this.EntityPM.Notify1AddressId = newValue;
                this.GetNotify1Address();
            }
        },
        enumerable: true,
        configurable: true
    });
    AWBPartnersTabComponent.prototype.GetNotify1Card = function () {
        if (this.Notify1Id == null) {
            this.Notify1AddressId = null;
            this.Notify1AddressList = null;
            this.EntityPM.Notify1Name = null;
            this.EntityPM.Notify1Note = null;
            this.EntityPM.Notify1ContactId = null;
            this.Validate_NTF();
            this.FireWizardEvent();
        }
        else {
            this.LoadNotify1Card();
        }
    };
    AWBPartnersTabComponent.prototype.LoadNotify1Card = function () {
        var _this = this;
        this.myCardListService.getSingle(this.Notify1Id).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var myCard = myResponse.Result;
                    if (myCard != null) {
                        _this.EntityPM.Notify1Name = myCard.EnglishName;
                        _this.EntityPM.Notify1Note = myCard.Notes;
                        _this.EntityPM.Notify1ContactId = myCard.PrimaryContactId;
                    }
                    if (_this.isPartnerChanged_Notify1) {
                        _this.GetNotify1MainAddress();
                    }
                    else {
                        _this.GetNotify1Address();
                    }
                    _this.isPartnerChanged_Notify1 = false;
                }
            }
        });
    };
    AWBPartnersTabComponent.prototype.GetNotify1Address = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.Notify1AddressId)) {
            var myService = new AddressListService_1.AddressListService();
            myService.getSingle(this.Notify1AddressId).subscribe(function (myResponse) {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        _this.SetNotify1Address(myResponse.Result);
                    }
                }
            });
        }
    };
    AWBPartnersTabComponent.prototype.GetNotify1MainAddress = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.Notify1Id)) {
            var myService = new AddressService_1.AddressService();
            myService.GetMainAddressByCardId(this.Notify1Id, this.Wizard.TenantPM.Id).subscribe(function (myResponse) {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        _this.SetNotify1Address(myResponse.Result);
                    }
                }
            });
        }
    };
    AWBPartnersTabComponent.prototype.SetNotify1Address = function (list) {
        this.Notify1AddressList = list;
        if (list == null) {
            if (this.EntityPM.Notify1AddressId != null) {
                this.EntityPM.Notify1AddressId = null;
            }
        }
        else {
            if (this.EntityPM.Notify1AddressId != list.Id) {
                this.EntityPM.Notify1AddressId = list.Id;
            }
            if (this.EntityPM.Notify1Address1 != list.Address1) {
                this.EntityPM.Notify1Address1 = list.Address1;
            }
            if (this.EntityPM.Notify1Address2 != list.Address2) {
                this.EntityPM.Notify1Address2 = list.Address2;
            }
            if (this.EntityPM.Notify1City != list.City) {
                this.EntityPM.Notify1City = list.City;
            }
            if (this.EntityPM.Notify1CountryId != list.CountryId) {
                this.EntityPM.Notify1CountryId = list.CountryId;
            }
            if (this.EntityPM.Notify1StateId != list.StateId) {
                this.EntityPM.Notify1StateId = list.StateId;
            }
            if (this.EntityPM.Notify1ZipCode != list.ZipCode) {
                this.EntityPM.Notify1ZipCode = list.ZipCode;
            }
            if (this.EntityPM.Notify1FaxNumber != list.FaxNumber) {
                this.EntityPM.Notify1FaxNumber = list.FaxNumber;
            }
            if (this.EntityPM.Notify1PhoneNumber != list.PhoneNumber) {
                this.EntityPM.Notify1PhoneNumber = list.PhoneNumber;
            }
        }
        this.Validate_NTF();
        this.FireWizardEvent();
    };
    Object.defineProperty(AWBPartnersTabComponent.prototype, "IssuingCarrierAgentId", {
        get: function () { return this.EntityPM.IssuingCarrierAgentId; },
        set: function (newValue) {
            if (this.EntityPM.IssuingCarrierAgentId != newValue) {
                this.EntityPM.IssuingCarrierAgentId = newValue;
                this.isPartnerChanged_Issuing = true;
                this.SetUIProperties_IssuingCarrier();
                this.GetIssuingCarrierCard();
            }
        },
        enumerable: true,
        configurable: true
    });
    AWBPartnersTabComponent.prototype.SetIssuingCarrier = function () {
        if (this.Wizard.IsImportWizard) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.IssuingCarrierAgentId) && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AgentId)) {
                this.EntityPM.AgentId = this.IssuingCarrierAgentId;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AgentId) && Tools_1.AppTool.IsNullOrEmpty(this.IssuingCarrierAgentId)) {
                this.IssuingCarrierAgentId = this.EntityPM.AgentId;
            }
        }
    };
    Object.defineProperty(AWBPartnersTabComponent.prototype, "IssuingCarrierAddressId", {
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
    Object.defineProperty(AWBPartnersTabComponent.prototype, "IssuingCarrierReference1", {
        get: function () { return this.EntityPM.IssuingCarrierReference1; },
        set: function (newValue) {
            if (this.EntityPM.IssuingCarrierReference1 != newValue) {
                this.EntityPM.IssuingCarrierReference1 = newValue;
                this.Validate_AGT();
                this.FireWizardEvent();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBPartnersTabComponent.prototype, "IssuingCarrierIATACode", {
        get: function () { return this.EntityPM.IssuingCarrierIATACode; },
        set: function (newValue) {
            if (this.EntityPM.IssuingCarrierIATACode != newValue) {
                this.EntityPM.IssuingCarrierIATACode = newValue;
                this.Validate_AGT();
                this.FireWizardEvent();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBPartnersTabComponent.prototype, "CASSCode", {
        get: function () { return this.EntityPM.CASSCode; },
        set: function (newValue) {
            if (this.EntityPM.CASSCode != newValue) {
                this.EntityPM.CASSCode = newValue;
                this.Validate_AGT();
                this.FireWizardEvent();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBPartnersTabComponent.prototype, "ColoaderRANumber", {
        get: function () { return this.EntityPM.ColoaderRANumber; },
        set: function (newValue) {
            if (this.EntityPM.ColoaderRANumber != newValue) {
                this.EntityPM.ColoaderRANumber = newValue;
                this.RAFieldChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBPartnersTabComponent.prototype, "RegulatedAgentRANumber", {
        get: function () { return this.EntityPM.RegulatedAgentRANumber; },
        set: function (newValue) {
            if (this.EntityPM.RegulatedAgentRANumber != newValue) {
                this.EntityPM.RegulatedAgentRANumber = newValue;
                this.RAFieldChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    AWBPartnersTabComponent.prototype.GetIssuingCarrierCard = function () {
        if (this.IssuingCarrierAgentId == null) {
            this.IssuingCarrierAddressId = null;
            this.IssuingCarrierAddressList = null;
            this.EntityPM.IssuingCarrierAgentName = null;
            this.EntityPM.IssuingCarrierAgentNote = null;
            this.Validate_AGT();
            this.FireWizardEvent();
        }
        else {
            this.LoadIssuingCarrierCard();
        }
    };
    AWBPartnersTabComponent.prototype.LoadIssuingCarrierCard = function () {
        var _this = this;
        this.myCardListService.getSingle(this.IssuingCarrierAgentId).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var myCard = myResponse.Result;
                    if (myCard != null) {
                        _this.EntityPM.IssuingCarrierAgentName = myCard.EnglishName;
                        _this.EntityPM.IssuingCarrierAgentNote = myCard.Notes;
                        if (_this.isSetDefaultTenantAgent) {
                            _this.CASSCode = _this.Wizard.TenantPM.CASSCode;
                            _this.IssuingCarrierIATACode = _this.Wizard.TenantPM.IATA;
                            _this.RegulatedAgentRANumber = _this.Wizard.TenantPM.RegulatedAgentNumber;
                        }
                        else {
                            _this.CASSCode = myCard.CASSCode;
                            _this.IssuingCarrierIATACode = myCard.IATACode;
                            _this.RegulatedAgentRANumber = _this.Wizard.TenantPM.RegulatedAgentNumber;
                        }
                        if (_this.ViaColoader) {
                            _this.EntityPM.ColoaderId = _this.EntityPM.IssuingCarrierAgentId;
                            _this.EntityPM.ColoaderName = _this.EntityPM.IssuingCarrierAgentName;
                            _this.EntityPM.ColoaderNote = _this.EntityPM.IssuingCarrierAgentNote;
                            _this.EntityPM.ColoaderContactId = myCard.PrimaryContactId;
                            _this.ColoaderRANumber = myCard.RegulatedAgentCode;
                        }
                    }
                    if (_this.isPartnerChanged_Issuing) {
                        _this.GetIssuingCarrierMainAddress();
                    }
                    else {
                        _this.GetIssuingCarrierAddress();
                    }
                    _this.isPartnerChanged_Issuing = false;
                }
            }
        });
    };
    AWBPartnersTabComponent.prototype.GetIssuingCarrierAddress = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.IssuingCarrierAddressId)) {
            var myService = new AddressListService_1.AddressListService();
            myService.getSingle(this.IssuingCarrierAddressId).subscribe(function (myResponse) {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        _this.SetIssuingCarrierAddress(myResponse.Result);
                    }
                }
            });
        }
    };
    AWBPartnersTabComponent.prototype.GetIssuingCarrierMainAddress = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.IssuingCarrierAgentId)) {
            var myService = new AddressService_1.AddressService();
            myService.GetMainAddressByCardId(this.IssuingCarrierAgentId, this.Wizard.TenantPM.Id).subscribe(function (myResponse) {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        _this.SetIssuingCarrierAddress(myResponse.Result);
                    }
                }
            });
        }
    };
    AWBPartnersTabComponent.prototype.SetIssuingCarrierAddress = function (list) {
        this.IssuingCarrierAddressList = list;
        if (list == null) {
            if (this.EntityPM.IssuingCarrierAddressId != null) {
                this.EntityPM.IssuingCarrierAddressId = null;
            }
            if (this.EntityPM.IssuingCarrierCity != null) {
                this.EntityPM.IssuingCarrierCity = null;
            }
        }
        else {
            if (this.EntityPM.IssuingCarrierAddressId != list.Id) {
                this.EntityPM.IssuingCarrierAddressId = list.Id;
            }
            if (this.EntityPM.IssuingCarrierCity != list.City) {
                this.EntityPM.IssuingCarrierCity = list.City;
            }
        }
        if (this.ViaColoader) {
            this.EntityPM.ColoaderAddressId = this.EntityPM.IssuingCarrierAddressId;
        }
        this.Validate_AGT();
        this.FireWizardEvent();
    };
    Object.defineProperty(AWBPartnersTabComponent.prototype, "ViaColoader", {
        // ViaColoader
        get: function () { return this.EntityPM.ViaColoader; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.ViaColoader != newValue) {
                this.EntityPM.ViaColoader = newValue;
                this.Validate_AGT();
                this.FireWizardEvent();
                this.SetUIProperties_IssuingCarrier();
                if (newValue) {
                    this.isSetDefaultTenantAgent = false;
                    this.IssuingCarrierAgentId = null;
                    this.IssuingCarrierAddressId = null;
                    this.IssuingCarrierIATACode = null;
                    this.CASSCode = null;
                    this.EntityPM.CASSCode = null;
                }
                else {
                    var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                    confirmWindow.WindowClosed.subscribe(function (event) {
                        if (confirmWindow.Yes) {
                            _this.isSetDefaultTenantAgent = true;
                            _this.IssuingCarrierAgentId = _this.Wizard.TenantPM.AgentId;
                            _this.ColoaderRANumber = null;
                            _this.CASSCode = _this.Wizard.TenantPM.CASSCode;
                            _this.IssuingCarrierIATACode = _this.Wizard.TenantPM.IATA;
                            _this.RegulatedAgentRANumber = _this.Wizard.TenantPM.RegulatedAgentNumber;
                        }
                        else {
                            _this.EntityPM.ViaColoader = true;
                            _this.SetUIProperties_IssuingCarrier();
                        }
                    });
                    confirmWindow.Show("Restore the default issuing carrier's agent ?");
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    AWBPartnersTabComponent.prototype.Add = function (myPartnerTypeCode) {
        this.AddEdit(myPartnerTypeCode, true);
    };
    AWBPartnersTabComponent.prototype.Edit = function (myPartnerTypeCode) {
        this.AddEdit(myPartnerTypeCode, false);
    };
    AWBPartnersTabComponent.prototype.AddEdit = function (myPartnerTypeCode, isNewPartner) {
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
                case "NTF": {
                    myTitle += "Notify";
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
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Title = myTitle;
            logWindow.WindowArgs = windowArgs;
            logWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/Partners/AWBAddEditPartnerComponent');
            logWindow.ComponentLoaded.subscribe(function (cmp) {
                logWindow.WindowClosed.subscribe(function ($event) {
                    _this.isPartnerWindowOpened = false;
                    if (cmp.IsUpdatingPartner) {
                        _this.UpdatePartner(cmp.PartnerTypeCode, cmp.CurrentPartnerId, cmp.CurrentAddressId);
                    }
                });
            });
        }
    };
    AWBPartnersTabComponent.prototype.UpdatePartner = function (myPartnerTypeCode, myPartnerId, myAddressId) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(myPartnerId)) {
            this.myCardListService.getSingle(myPartnerId).subscribe(function (myResponse) {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        var iCardList = myResponse.Result;
                        if (iCardList) {
                            switch (myPartnerTypeCode) {
                                case "SHI":
                                    {
                                        if (_this.EntityPM.ShipperId != myPartnerId) {
                                            _this.EntityPM.ShipperId = myPartnerId;
                                        }
                                        if (_this.EntityPM.ShipperName != iCardList.EnglishName) {
                                            _this.EntityPM.ShipperName = iCardList.EnglishName;
                                        }
                                        if (_this.EntityPM.ShipperNote != iCardList.Notes) {
                                            _this.EntityPM.ShipperNote = iCardList.Notes;
                                        }
                                        if (_this.EntityPM.ShipperContactId != iCardList.PrimaryContactId) {
                                            _this.EntityPM.ShipperContactId = iCardList.PrimaryContactId;
                                        }
                                        if (_this.EntityPM.KnownConsignorNumber != iCardList.KnownConsignor) {
                                            _this.EntityPM.KnownConsignorNumber = iCardList.KnownConsignor;
                                        }
                                        if (_this.EntityPM.KCExpirationDate != iCardList.KCExpirationDate) {
                                            _this.EntityPM.KCExpirationDate = iCardList.KCExpirationDate;
                                        }
                                        _this.RAFieldChanged();
                                        if (iCardList.SalesmanUserId != null) {
                                            if (_this.EntityPM.SalesmanUserId != iCardList.SalesmanUserId) {
                                                _this.EntityPM.SalesmanUserId = iCardList.SalesmanUserId;
                                            }
                                        }
                                        if (iCardList.AccountManagerUserId != null) {
                                            if (_this.EntityPM.AccountManagerUserId != iCardList.AccountManagerUserId) {
                                                _this.EntityPM.AccountManagerUserId = iCardList.AccountManagerUserId;
                                            }
                                        }
                                        _this.SetUIProperties_Shipper();
                                        break;
                                    }
                                case "CON":
                                    {
                                        if (_this.EntityPM.ConsigneeId != myPartnerId) {
                                            _this.EntityPM.ConsigneeId = myPartnerId;
                                        }
                                        if (_this.EntityPM.ConsigneeName != iCardList.EnglishName) {
                                            _this.EntityPM.ConsigneeName = iCardList.EnglishName;
                                        }
                                        if (_this.EntityPM.ConsigneeNote != iCardList.Notes) {
                                            _this.EntityPM.ConsigneeNote = iCardList.Notes;
                                        }
                                        if (_this.EntityPM.ConsigneeContactId != iCardList.PrimaryContactId) {
                                            _this.EntityPM.ConsigneeContactId = iCardList.PrimaryContactId;
                                        }
                                        _this.SetUIProperties_Consignee();
                                        break;
                                    }
                                case "NTF":
                                    {
                                        if (_this.EntityPM.Notify1Id != myPartnerId) {
                                            _this.EntityPM.Notify1Id = myPartnerId;
                                        }
                                        if (_this.EntityPM.Notify1Name != iCardList.EnglishName) {
                                            _this.EntityPM.Notify1Name = iCardList.EnglishName;
                                        }
                                        if (_this.EntityPM.Notify1Note != iCardList.Notes) {
                                            _this.EntityPM.Notify1Note = iCardList.Notes;
                                        }
                                        if (_this.EntityPM.Notify1ContactId != iCardList.PrimaryContactId) {
                                            _this.EntityPM.Notify1ContactId = iCardList.PrimaryContactId;
                                        }
                                        _this.SetUIProperties_Notify1();
                                        break;
                                    }
                                case "ISS":
                                    {
                                        if (_this.EntityPM.IssuingCarrierAgentId != myPartnerId) {
                                            _this.EntityPM.IssuingCarrierAgentId = myPartnerId;
                                        }
                                        if (_this.EntityPM.IssuingCarrierAgentName != iCardList.EnglishName) {
                                            _this.EntityPM.IssuingCarrierAgentName = iCardList.EnglishName;
                                        }
                                        if (_this.EntityPM.IssuingCarrierAgentNote != iCardList.Notes) {
                                            _this.EntityPM.IssuingCarrierAgentNote = iCardList.Notes;
                                        }
                                        if (_this.isSetDefaultTenantAgent) {
                                            if (_this.CASSCode != _this.Wizard.TenantPM.CASSCode) {
                                                _this.CASSCode = _this.Wizard.TenantPM.CASSCode;
                                            }
                                            if (_this.IssuingCarrierIATACode != _this.Wizard.TenantPM.IATA) {
                                                _this.IssuingCarrierIATACode = _this.Wizard.TenantPM.IATA;
                                            }
                                            if (_this.RegulatedAgentRANumber != _this.Wizard.TenantPM.RegulatedAgentNumber) {
                                                _this.RegulatedAgentRANumber = _this.Wizard.TenantPM.RegulatedAgentNumber;
                                            }
                                        }
                                        else {
                                            if (_this.CASSCode != iCardList.CASSCode) {
                                                _this.CASSCode = iCardList.CASSCode;
                                            }
                                            if (_this.IssuingCarrierIATACode != iCardList.IATACode) {
                                                _this.IssuingCarrierIATACode = iCardList.IATACode;
                                            }
                                            if (_this.RegulatedAgentRANumber != _this.Wizard.TenantPM.RegulatedAgentNumber) {
                                                _this.RegulatedAgentRANumber = _this.Wizard.TenantPM.RegulatedAgentNumber;
                                            }
                                        }
                                        if (_this.ViaColoader) {
                                            if (_this.EntityPM.ColoaderId != _this.EntityPM.IssuingCarrierAgentId) {
                                                _this.EntityPM.ColoaderId = _this.EntityPM.IssuingCarrierAgentId;
                                            }
                                            if (_this.EntityPM.ColoaderName != _this.EntityPM.IssuingCarrierAgentName) {
                                                _this.EntityPM.ColoaderName = _this.EntityPM.IssuingCarrierAgentName;
                                            }
                                            if (_this.EntityPM.ColoaderNote != _this.EntityPM.IssuingCarrierAgentNote) {
                                                _this.EntityPM.ColoaderNote = _this.EntityPM.IssuingCarrierAgentNote;
                                            }
                                            if (_this.EntityPM.ColoaderContactId != iCardList.PrimaryContactId) {
                                                _this.EntityPM.ColoaderContactId = iCardList.PrimaryContactId;
                                            }
                                            if (_this.ColoaderRANumber != iCardList.RegulatedAgentCode) {
                                                _this.ColoaderRANumber = iCardList.RegulatedAgentCode;
                                            }
                                        }
                                        _this.SetUIProperties_IssuingCarrier();
                                        break;
                                    }
                            }
                            var iAddressList = null;
                            if (Tools_1.AppTool.IsNullOrEmpty(myAddressId)) {
                                switch (myPartnerTypeCode) {
                                    case "SHI": {
                                        _this.SetShipperAddress(null);
                                        break;
                                    }
                                    case "CON": {
                                        _this.SetConsigneeAddress(null);
                                        break;
                                    }
                                    case "NTF": {
                                        _this.SetNotify1Address(null);
                                        break;
                                    }
                                    case "ISS": {
                                        _this.SetIssuingCarrierAddress(null);
                                        break;
                                    }
                                }
                            }
                            else {
                                var myService = new AddressListService_1.AddressListService();
                                myService.getSingle(myAddressId).subscribe(function (myResponse2) {
                                    if (myResponse2 != null) {
                                        if (!myResponse2.HasError) {
                                            var iAddressList = myResponse2.Result;
                                            switch (myPartnerTypeCode) {
                                                case "SHI": {
                                                    _this.SetShipperAddress(iAddressList);
                                                    break;
                                                }
                                                case "CON": {
                                                    _this.SetConsigneeAddress(iAddressList);
                                                    break;
                                                }
                                                case "NTF": {
                                                    _this.SetNotify1Address(iAddressList);
                                                    break;
                                                }
                                                case "ISS": {
                                                    _this.SetIssuingCarrierAddress(iAddressList);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                });
                            }
                        }
                    }
                }
            });
        }
    };
    AWBPartnersTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'PartnersTabComponent',
            templateUrl: './AWBPartnersTabComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AWBPartnersTabComponent);
    return AWBPartnersTabComponent;
}(BaseComponent_1.BaseComponent));
exports.AWBPartnersTabComponent = AWBPartnersTabComponent;
//# sourceMappingURL=AWBPartnersTabComponent.js.map