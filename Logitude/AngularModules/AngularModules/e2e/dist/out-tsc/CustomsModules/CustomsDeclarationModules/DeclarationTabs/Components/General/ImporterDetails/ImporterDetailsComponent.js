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
var BaseComponent_1 = require("../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../../../../Infrastructure/Utilities/TextCodeTranslator");
var DeclarationPM_1 = require("../../../../../../Customs/EntityPMs/DeclarationPM");
var DeclarationPMService_1 = require("../../../../../../Customs/Services/StandardPMs/DeclarationPMService");
var MessageWindow_1 = require("../../../../../../Controls/Windows/MessageWindow");
var ImporterDetailsComponent = /** @class */ (function (_super) {
    __extends(ImporterDetailsComponent, _super);
    function ImporterDetailsComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.Declaration";
        _this.declarationPMService = new DeclarationPMService_1.DeclarationPMService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsEntitleImporterEnabled = true;
        _this.IsTransferImporterEnabled = true;
        _this.IsImporterEnabled = true;
        return _this;
    }
    Object.defineProperty(ImporterDetailsComponent.prototype, "ImporterEntitlementTypeCode", {
        //#region properties
        get: function () { return this.EntityPM.ImporterEntitlementTypeCode; },
        set: function (newValue) { this.EntityPM.ImporterEntitlementTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "EntitleImporterName", {
        get: function () { return this.EntityPM.EntitleImporterName; },
        set: function (newValue) { this.EntityPM.EntitleImporterName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "ImporterName", {
        get: function () { return this.EntityPM.ImporterName; },
        set: function (newValue) {
            this.EntityPM.ImporterName = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "TransferImporterName", {
        get: function () { return this.EntityPM.TransferImporterName; },
        set: function (newValue) { this.EntityPM.TransferImporterName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "ImporterTypeCode", {
        get: function () { return this.EntityPM.ImporterTypeCode; },
        set: function (newValue) {
            this.EntityPM.ImporterTypeCode = newValue;
            this.EntityPM.ImporterCode = null;
            this.EntityPM.ImporterPassportNumber = null;
            this.EntityPM.ImporterPassCountryCode = null;
            this.EntityPM.ImporterAddress = null;
            this.EntityPM.MainImporterEntitlemntTypeCode = null;
            this.ImporterName = null;
            this.SetFieldsEditibility(this.ImporterTypeCode, this.type);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "ImporterTypeName", {
        get: function () { return this.EntityPM.ImporterTypeName; },
        set: function (newValue) {
            this.EntityPM.ImporterTypeName = newValue;
            //   this.SetFieldsEditibility(newValue, this.type);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "TransferImporterTypeCode", {
        get: function () { return this.EntityPM.TransferImporterTypeCode; },
        set: function (newValue) {
            this.EntityPM.TransferImporterTypeCode = newValue;
            this.EntityPM.TransferImporterCode = null;
            this.EntityPM.TransferPassportNumber = null;
            this.EntityPM.TransferImporterCountryCode = null;
            this.TransferImporterAddress = null;
            this.TransImporterEntitleTypeCode = null;
            this.TransferImporterName = null;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "TransferImporterTypeName", {
        get: function () { return this.EntityPM.TransferImporterTypeName; },
        set: function (newValue) {
            this.EntityPM.TransferImporterTypeName = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "EntitleImporterTypeCode", {
        get: function () { return this.EntityPM.EntitleImporterTypeCode; },
        set: function (newValue) {
            this.EntityPM.EntitleImporterTypeCode = newValue;
            this.EntityPM.EntitleImporterCode = null;
            this.EntityPM.EntitlePassportNumber = null;
            this.EntityPM.EntitleImporterCountryCode = null;
            this.EntitleImporterAddress = null;
            this.ImporterEntitlementTypeCode = null;
            this.EntitleImporterName = null;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "EntitleImporterTypeName", {
        get: function () { return this.EntityPM.EntitleImporterTypeName; },
        set: function (newValue) {
            this.EntityPM.EntitleImporterTypeName = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "ImporterPassCountryCode", {
        get: function () { return this.EntityPM.ImporterPassCountryCode; },
        set: function (newValue) { this.EntityPM.ImporterPassCountryCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "TransferImporterCountryCode", {
        get: function () { return this.EntityPM.TransferImporterCountryCode; },
        set: function (newValue) { this.EntityPM.TransferImporterCountryCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "MainImporterEntitlemntTypeCode", {
        get: function () { return this.EntityPM.MainImporterEntitlemntTypeCode; },
        set: function (newValue) { this.EntityPM.MainImporterEntitlemntTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "TransImporterEntitleTypeCode", {
        get: function () { return this.EntityPM.TransImporterEntitleTypeCode; },
        set: function (newValue) { this.EntityPM.TransImporterEntitleTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "ImporterAddress", {
        get: function () { return this.EntityPM.ImporterAddress; },
        set: function (newValue) { this.EntityPM.ImporterAddress = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "EntitleImporterAddress", {
        get: function () { return this.EntityPM.EntitleImporterAddress; },
        set: function (newValue) { this.EntityPM.EntitleImporterAddress = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "TransferImporterAddress", {
        get: function () { return this.EntityPM.TransferImporterAddress; },
        set: function (newValue) { this.EntityPM.TransferImporterAddress = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "IsImporterAddressEnabled", {
        get: function () { return this.isImporterAddressEnabled; },
        set: function (newValue) { this.isImporterAddressEnabled = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "IsImporterNameEnabled", {
        get: function () { return this.isImporterNameEnabled; },
        set: function (newValue) { this.isImporterNameEnabled = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "IsMainImporterEntitlemntTypeCodeEnabled", {
        get: function () { return this.isMainImporterEntitlemntTypeCodeEnabled; },
        set: function (newValue) { this.isMainImporterEntitlemntTypeCodeEnabled = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "IsImporterPassCountryCodeEnabled", {
        get: function () { return this.isImporterPassCountryCodeEnabled; },
        set: function (newValue) { this.isImporterPassCountryCodeEnabled = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "IsImporterPassportNumberEnabled", {
        get: function () { return this.isImporterPassportNumberEnabled; },
        set: function (value) { this.isImporterPassportNumberEnabled = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "IsImporterTypeCodeEnabled", {
        get: function () { return this.isImporterTypeCodeEnabled; },
        set: function (value) { this.isImporterTypeCodeEnabled = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "ImporterVisibility", {
        get: function () { return this.importerVisibility; },
        set: function (value) { this.importerVisibility = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "EntitleVisibility", {
        get: function () { return this.entitleVisibility; },
        set: function (value) { this.entitleVisibility = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "TransferVisibility", {
        get: function () { return this.transferVisibility; },
        set: function (value) { this.transferVisibility = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "ImporterPassportNumber", {
        get: function () { return this.EntityPM.ImporterPassportNumber; },
        set: function (value) { this.EntityPM.ImporterPassportNumber = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "EntitlePassportNumber", {
        get: function () { return this.EntityPM.EntitlePassportNumber; },
        set: function (value) { this.EntityPM.EntitlePassportNumber = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "TransferPassportNumber", {
        get: function () { return this.EntityPM.TransferPassportNumber; },
        set: function (value) { this.EntityPM.TransferPassportNumber = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "EntitleImporterCountryCode", {
        get: function () { return this.EntityPM.EntitleImporterCountryCode; },
        set: function (value) { this.EntityPM.EntitleImporterCountryCode = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "isCourierDeclaration", {
        //#REGION  Task 42204: שינויים במסך פרטי יבואן בהצהרה
        get: function () { return this.EntityPM.IsCourierDeclaration; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "CasualImporterAddress1", {
        get: function () { return this.EntityPM.CasualImporterAddress1; },
        set: function (value) { this.EntityPM.CasualImporterAddress1 = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "CasualImporterAddress2", {
        get: function () { return this.EntityPM.CasualImporterAddress2; },
        set: function (value) { this.EntityPM.CasualImporterAddress2 = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "CasualImporterCity", {
        get: function () { return this.EntityPM.CasualImporterCity; },
        set: function (value) { this.EntityPM.CasualImporterCity = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "CasualImporterZipCode", {
        get: function () { return this.EntityPM.CasualImporterZipCode; },
        set: function (value) { this.EntityPM.CasualImporterZipCode = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "CasualImporterFax", {
        get: function () { return this.EntityPM.CasualImporterFax; },
        set: function (value) { this.EntityPM.CasualImporterFax = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "CasualImporterEmail", {
        get: function () { return this.EntityPM.CasualImporterEmail; },
        set: function (value) { this.EntityPM.CasualImporterEmail = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "CasualImporterTel", {
        get: function () { return this.EntityPM.CasualImporterTel; },
        set: function (value) { this.EntityPM.CasualImporterTel = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "CasualImporterContact", {
        get: function () { return this.EntityPM.CasualImporterContact; },
        set: function (value) { this.EntityPM.CasualImporterContact = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "CustomerIdentifyType", {
        get: function () { return this.customerIdentifyType; },
        set: function (value) {
            if (this.customerIdentifyType != value) {
                this.customerIdentifyType = value;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                this.ImporterTypeName = value.EnglishName;
            }
            else {
                this.ImporterTypeName = null;
                this.ImporterTypeCode = null;
            }
            this.SetFieldsEditibility(this.ImporterTypeCode, this.type);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "EntitleCustomerIdentifyType", {
        get: function () { return this.entitleCustomerIdentifyType; },
        set: function (value) {
            if (this.entitleCustomerIdentifyType != value) {
                this.entitleCustomerIdentifyType = value;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                this.EntitleImporterTypeName = value.EnglishName;
            }
            else {
                this.EntitleImporterTypeName = null;
                this.EntitleImporterTypeCode = null;
            }
            this.SetFieldsEditibility(this.EntitleImporterTypeCode, this.type);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ImporterDetailsComponent.prototype, "TransferCustomerIdentifyType", {
        get: function () { return this.transferCustomerIdentifyType; },
        set: function (value) {
            if (this.transferCustomerIdentifyType != value) {
                this.transferCustomerIdentifyType = value;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                this.TransferImporterTypeName = value.EnglishName;
            }
            else {
                this.TransferImporterTypeName = null;
                this.TransferImporterTypeCode = null;
            }
            this.SetFieldsEditibility(this.TransferImporterTypeCode, this.type);
        },
        enumerable: true,
        configurable: true
    });
    ImporterDetailsComponent.prototype.SetWindowArgs = function (args) {
        if (!Tools_1.AppTool.IsNullOrEmpty(args)) {
            this.EntityPM = args.EntityPM;
            this.OriginalEntityPM = args.EntityPM;
            this.ClonedEntityPM = this.CloneEntity(args.EntityPM);
            this.type = args.Type;
            switch (this.type) {
                case "Importer": {
                    this.ImporterVisibility = true;
                    this.EntitleVisibility = false;
                    this.TransferVisibility = false;
                    this.SetFieldsEditibility(this.EntityPM.ImporterTypeCode, this.type);
                    if (this.EntityPM.ImporterTypeName == "IL") {
                        if (this.EntityPM.ImporterCode != null || this.EntityPM.ImporterId != null) {
                            this.UIProperties.SetEnabled("ImporterTypeCode", this.ObjectTableName, false);
                        }
                    }
                    break;
                }
                case "Transfer": {
                    this.ImporterVisibility = false;
                    this.EntitleVisibility = false;
                    this.TransferVisibility = true;
                    this.SetFieldsEditibility(this.EntityPM.TransferImporterTypeCode, this.type);
                    if (this.EntityPM.TransferImporterTypeCode == "1") {
                        if (this.EntityPM.TransferImporterCode != null || this.EntityPM.TransferImporterId != null) {
                            this.UIProperties.SetEnabled("TransferImporterTypeCode", this.ObjectTableName, false);
                        }
                    }
                    break;
                }
                case "Entitle": {
                    this.ImporterVisibility = false;
                    this.EntitleVisibility = true;
                    this.TransferVisibility = false;
                    this.SetFieldsEditibility(this.EntityPM.EntitleImporterTypeCode, this.type);
                    if (this.EntityPM.EntitleImporterTypeCode == "1") {
                        if (this.EntityPM.EntitleImporterCode != null || this.EntityPM.EntitleImporterId != null) {
                            this.UIProperties.SetEnabled("EntitleImporterTypeCode", this.ObjectTableName, false);
                        }
                    }
                    break;
                }
            }
        }
    };
    ImporterDetailsComponent.prototype.SetFieldsEditibility = function (xxxTypeCode, type) {
        switch (type) {
            case "Importer": {
                if (xxxTypeCode == "1") {
                    if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ImporterCode)) {
                        this.UIProperties.SetEnabled("ImporterName", this.ObjectTableName, true);
                        this.UIProperties.SetEnabled("ImporterAddress", this.ObjectTableName, true);
                        this.UIProperties.SetEnabled("ImporterTypeCode", this.ObjectTableName, true);
                        // this.EntityPM.ImporterCode = this.ImporterName;
                    }
                    else {
                        this.UIProperties.SetEnabled("ImporterTypeCode", this.ObjectTableName, false);
                        this.UIProperties.SetEnabled("ImporterName", this.ObjectTableName, false);
                        this.UIProperties.SetEnabled("ImporterAddress", this.ObjectTableName, false);
                    }
                    this.IsImporterEnabled = false;
                    //   
                    this.UIProperties.SetEnabled("ImporterPassportNumber", this.ObjectTableName, false);
                    this.UIProperties.SetEnabled("ImporterPassCountryCode", this.ObjectTableName, false);
                }
                else if (xxxTypeCode == "2" || xxxTypeCode == "3") {
                    this.IsImporterEnabled = true;
                    this.UIProperties.SetEnabled("ImporterName", this.ObjectTableName, false);
                    this.UIProperties.SetEnabled("ImporterAddress", this.ObjectTableName, false);
                    this.UIProperties.SetEnabled("ImporterPassportNumber", this.ObjectTableName, true);
                    this.UIProperties.SetEnabled("ImporterPassCountryCode", this.ObjectTableName, true);
                }
                this.SetFieldsEditibilityCourier();
                break;
            }
            case "Transfer": {
                if (xxxTypeCode == "1") {
                    if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.TransferImporterCode)) {
                        this.UIProperties.SetEnabled("TransferImporterName", this.ObjectTableName, true);
                        this.UIProperties.SetEnabled("TransferImporterAddress", this.ObjectTableName, true);
                        this.UIProperties.SetEnabled("TransferImporterTypeCode", this.ObjectTableName, true);
                        // this.EntityPM.TransferImporterCode = this.TransferImporterName;
                    }
                    else {
                        this.UIProperties.SetEnabled("TransferImporterName", this.ObjectTableName, false);
                        this.UIProperties.SetEnabled("TransferImporterAddress", this.ObjectTableName, false);
                        this.UIProperties.SetEnabled("TransferImporterTypeCode", this.ObjectTableName, false);
                    }
                    this.IsTransferImporterEnabled = false;
                    // this.UIProperties.SetEnabled("TransferImporterTypeCode", this.ObjectTableName, false);
                    this.UIProperties.SetEnabled("TransferImporterCountryCode", this.ObjectTableName, false);
                    this.UIProperties.SetEnabled("TransferPassportNumber", this.ObjectTableName, false);
                }
                else if (xxxTypeCode == "2" || xxxTypeCode == "3") {
                    this.IsTransferImporterEnabled = true;
                    this.UIProperties.SetEnabled("TransferImporterTypeCode", this.ObjectTableName, true);
                    this.UIProperties.SetEnabled("TransferImporterName", this.ObjectTableName, false);
                    this.UIProperties.SetEnabled("TransferImporterAddress", this.ObjectTableName, false);
                    this.UIProperties.SetEnabled("TransferImporterCountryCode", this.ObjectTableName, true);
                    this.UIProperties.SetEnabled("TransferPassportNumber", this.ObjectTableName, true);
                }
                break;
            }
            case "Entitle": {
                if (xxxTypeCode == "1") {
                    if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.EntitleImporterCode)) {
                        this.UIProperties.SetEnabled("EntitleImporterName", this.ObjectTableName, true);
                        this.UIProperties.SetEnabled("EntitleImporterAddress", this.ObjectTableName, true);
                        this.UIProperties.SetEnabled("EntitleImporterTypeCode", this.ObjectTableName, true);
                        //   this.EntityPM.EntitleImporterCode = this.EntitleImporterName;
                    }
                    else {
                        this.UIProperties.SetEnabled("EntitleImporterName", this.ObjectTableName, false);
                        this.UIProperties.SetEnabled("EntitleImporterAddress", this.ObjectTableName, false);
                        this.UIProperties.SetEnabled("EntitleImporterTypeCode", this.ObjectTableName, false);
                    }
                    this.IsEntitleImporterEnabled = false;
                    // this.UIProperties.SetEnabled("EntitleImporterTypeCode", this.ObjectTableName, false);
                    this.UIProperties.SetEnabled("EntitleImporterCountryCode", this.ObjectTableName, false);
                    this.UIProperties.SetEnabled("EntitlePassportNumber", this.ObjectTableName, false);
                }
                else if (xxxTypeCode == "2" || xxxTypeCode == "3") {
                    this.UIProperties.SetEnabled("EntitleImporterName", this.ObjectTableName, false);
                    this.UIProperties.SetEnabled("EntitleImporterAddress", this.ObjectTableName, false);
                    this.IsEntitleImporterEnabled = true;
                    this.UIProperties.SetEnabled("EntitleImporterCountryCode", this.ObjectTableName, true);
                    this.UIProperties.SetEnabled("EntitlePassportNumber", this.ObjectTableName, true);
                }
                break;
            }
        }
    };
    ImporterDetailsComponent.prototype.SetFieldsEditibilityCourier = function () {
        if (!this.isCourierDeclaration) {
            return;
        }
        //if (AppTool.IsNullOrEmpty(this.EntityPM.ImporterCode)) {
        this.UIProperties.SetEnabled("ImporterName", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("CasualImporterAddress1", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("CasualImporterAddress2", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("CasualImporterCity", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("CasualImporterZipCode", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("CasualImporterFax", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("CasualImporterEmail", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("CasualImporterTel", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("CasualImporterContact", this.ObjectTableName, true);
        //}
        if ( ///!AppTool.IsNullOrEmpty(this.EntityPM.ImporterCode) ||
        this.EntityPM.ImporterTypeCode == "2" /*"P"*/ ||
            this.EntityPM.ImporterTypeCode == "3" /*"F"*/) {
            this.ImporterName = "";
            this.ImporterAddress = "";
            this.EntityPM.CalculatedImporterName = null;
            this.CasualImporterAddress1 = "";
            this.CasualImporterAddress2 = "";
            this.CasualImporterCity = "";
            this.CasualImporterZipCode = "";
            this.CasualImporterFax = "";
            this.CasualImporterEmail = "";
            this.CasualImporterTel = "";
            this.CasualImporterContact = "";
            this.UIProperties.SetEnabled("ImporterName", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CasualImporterAddress1", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CasualImporterAddress2", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CasualImporterCity", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CasualImporterZipCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CasualImporterFax", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CasualImporterEmail", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CasualImporterTel", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CasualImporterContact", this.ObjectTableName, false);
        }
    };
    ImporterDetailsComponent.prototype.ImporterLostFocus = function (type, item) {
        if (item != null) {
            var isNumberTooLong = false;
            switch (type) {
                case 'Importer': {
                    if ((this.ImporterTypeName == "IL" && item.length > 9) || (this.ImporterTypeName != "IL" && item.length > 15)) {
                        isNumberTooLong = true;
                        this.ImporterPassportNumber = null;
                    }
                    break;
                }
                case 'Entitle': {
                    if ((this.EntitleImporterTypeCode == "IL" && item.length > 9) || (this.EntitleImporterTypeCode != "IL" && item.length > 15)) {
                        isNumberTooLong = true;
                        this.EntitlePassportNumber = null;
                    }
                    break;
                }
                case 'Transfer': {
                    if ((this.TransferImporterTypeCode == "IL" && item.length > 9) || (this.TransferImporterTypeCode != "IL" && item.length > 15)) {
                        isNumberTooLong = true;
                        this.TransferPassportNumber = null;
                    }
                    break;
                }
            }
            if (isNumberTooLong) {
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Warning");
                messageWindow.Width = 250;
                messageWindow.Height = 150;
                messageWindow.OkButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
                messageWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.TooLongCode"));
                return;
            }
        }
        switch (type) {
            case 'Importer': {
                this.ImporterPassportNumber = item;
                break;
            }
            case 'Transfer': {
                this.TransferPassportNumber = item;
                break;
            }
            case 'Entitle': {
                this.EntitlePassportNumber = item;
                break;
            }
        }
    };
    ImporterDetailsComponent.prototype.ImporterNumberTextChanged = function (type, code) {
        switch (type) {
            case 'Importer': {
                this.ImporterPassportNumber = code;
                break;
            }
            case 'Transfer': {
                this.TransferPassportNumber = code;
                break;
            }
            case 'Entitle': {
                this.EntitlePassportNumber = code;
                break;
            }
        }
    };
    ImporterDetailsComponent.prototype.ImporterClicked = function (type, client) {
        if (client) {
            switch (type) {
                case 'Importer': {
                    this.ImporterPassportNumber = client != null ? client.PassportNumber : null;
                    break;
                }
                case 'Transfer': {
                    this.TransferPassportNumber = client != null ? client.PassportNumber : null;
                    break;
                }
                case 'Entitle': {
                    this.EntitlePassportNumber = client != null ? client.PassportNumber : null;
                    break;
                }
            }
        }
    };
    ImporterDetailsComponent.prototype.CloneEntity = function (entityToClone) {
        var clonedEntity;
        clonedEntity = new DeclarationPM_1.DeclarationPM();
        this.MapEntitytoEntity(entityToClone, clonedEntity);
        return clonedEntity;
    };
    ImporterDetailsComponent.prototype.RejectChanges = function () {
        this.MapEntitytoEntity(this.ClonedEntityPM, this.OriginalEntityPM, true);
    };
    ImporterDetailsComponent.prototype.MapEntitytoEntity = function (srcEntity, targetEntity, takeKeysFromTarget) {
        if (takeKeysFromTarget === void 0) { takeKeysFromTarget = false; }
        var keys;
        keys = Object.keys(takeKeysFromTarget ? targetEntity : srcEntity);
        for (var key in keys) {
            var property = keys[key];
            targetEntity[property] = srcEntity[property];
        }
    };
    ImporterDetailsComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindowEmit("cancel");
    };
    ImporterDetailsComponent.prototype.GetPassportNumber = function (passportNumber) {
        if (passportNumber.length <= 13) {
            return passportNumber;
        }
        else if (passportNumber.length == 14) {
            return passportNumber.substring(2);
        }
        else if (passportNumber.length == 15) {
            return passportNumber.substring(3);
        }
    };
    ImporterDetailsComponent.prototype.DeleteImporterDetailsButtonClicked = function () {
        //this.ImporterTypeCode = null;
        this.ImporterPassportNumber = null;
        this.ImporterPassCountryCode = null;
        this.MainImporterEntitlemntTypeCode = null;
        this.ImporterName = null;
        this.ImporterTypeName = null;
        this.CasualImporterAddress1 = null;
        this.CasualImporterAddress2 = null;
        this.CasualImporterCity = null;
        this.CasualImporterContact = null;
        this.CasualImporterEmail = null;
        this.CasualImporterFax = null;
        this.CasualImporterTel = null;
        this.CasualImporterZipCode = null;
        this.EntityPM.CalculatedImporterName = null;
        //this.OkButtonClicked();
        //this.CurrentSession.CurrentEditComponent.SaveChanges();
        //this.CurrentSession.CloseCurrentWindowEmit("ok");
    };
    ImporterDetailsComponent.prototype.OkButtonClicked = function () {
        if (this.type == "Importer" && this.isCourierDeclaration) {
            //if (!FormatTool.IsEmail(this.CasualImporterEmail)) {
            //errors.push("Invalid email format!");
            //}
            this.ImporterAddress = null;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CasualImporterAddress1) ||
                !Tools_1.AppTool.IsNullOrEmpty(this.CasualImporterAddress2) ||
                !Tools_1.AppTool.IsNullOrEmpty(this.CasualImporterCity)) {
                this.ImporterAddress = this.CasualImporterAddress1 + " " +
                    this.CasualImporterAddress2 + " " + this.CasualImporterCity;
            }
        }
        this.CurrentSession.CurrentEditComponent.SaveChanges();
        var passportNumber;
        switch (this.type) {
            case "Importer": {
                if (this.ImporterTypeCode == "1" || Tools_1.AppTool.IsNullOrEmpty(this.ImporterTypeName)) {
                    this.doDisable = false;
                    if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ImporterCode)) {
                        this.EntityPM.CalculatedImporterName = this.ImporterName;
                    }
                }
                else {
                    this.doDisable = true;
                    if (this.ImporterPassportNumber) {
                        passportNumber = this.GetPassportNumber(this.ImporterPassportNumber);
                        this.EntityPM.ImporterCode = this.ImporterTypeName + "-" + passportNumber;
                    }
                    else {
                        this.EntityPM.ImporterCode = this.ImporterTypeName;
                    }
                    this.UIProperties.SetEnabled("ImporterCode", this.ObjectTableName, false);
                }
                break;
            }
            case "Transfer": {
                if (this.TransferImporterTypeCode == "1" || Tools_1.AppTool.IsNullOrEmpty(this.TransferImporterTypeName)) {
                    this.doDisable = false;
                    if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.TransferImporterCode)) {
                        this.EntityPM.CalculatedTransferImporterName = this.TransferImporterName;
                    }
                }
                else {
                    this.doDisable = true;
                    if (this.TransferPassportNumber) {
                        passportNumber = this.GetPassportNumber(this.TransferPassportNumber);
                        this.EntityPM.TransferImporterCode = this.TransferImporterTypeName + "-" + passportNumber;
                    }
                    else {
                        this.EntityPM.TransferImporterCode = this.TransferImporterTypeName;
                    }
                    this.UIProperties.SetEnabled("TransferImporterCode", this.ObjectTableName, false);
                }
                break;
            }
            case "Entitle": {
                if (this.EntitleImporterTypeCode == "1" || Tools_1.AppTool.IsNullOrEmpty(this.TransferImporterTypeName)) {
                    this.doDisable = false;
                    if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.EntitleImporterCode)) {
                        this.EntityPM.CalculatedEntitleImporterName = this.EntitleImporterName;
                    }
                }
                else {
                    this.doDisable = true;
                    if (this.EntitlePassportNumber) {
                        passportNumber = this.GetPassportNumber(this.EntitlePassportNumber);
                        this.EntityPM.EntitleImporterCode = this.EntitleImporterTypeName + "-" + passportNumber;
                    }
                    else {
                        this.EntityPM.EntitleImporterCode = this.EntitleImporterTypeName;
                    }
                    this.UIProperties.SetEnabled("EntitleImporterCode", this.ObjectTableName, false);
                }
                break;
            }
        }
        if (this.doDisable) {
            this.CurrentSession.CloseCurrentWindowEmit("ok");
        }
        else {
            this.CurrentSession.CloseCurrentWindowEmit("!ok");
        }
    };
    ImporterDetailsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ImporterDetailsComponent.html',
            selector: 'ImporterDetailsComponent',
        }),
        __metadata("design:paramtypes", [])
    ], ImporterDetailsComponent);
    return ImporterDetailsComponent;
}(BaseComponent_1.BaseComponent));
exports.ImporterDetailsComponent = ImporterDetailsComponent;
//# sourceMappingURL=ImporterDetailsComponent.js.map