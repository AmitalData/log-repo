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
var Validator_1 = require("../../../../../Infrastructure/Validators/Validator");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var ConfirmWindow_1 = require("../../../../../Controls/Windows/ConfirmWindow");
var Args_1 = require("../../../../../Common/Args");
var CardListService_1 = require("../../../../../Common/Services/StandardLists/CardListService");
var AddressPM_1 = require("../../../../../Common/EntityPMs/AddressPM");
var AgentPM_1 = require("../../../../../Common/EntityPMs/AgentPM");
var CustomerPM_1 = require("../../../../../Common/EntityPMs/CustomerPM");
var AddressPMService_1 = require("../../../../../Common/Services/StandardPMs/AddressPMService");
var AgentPMService_1 = require("../../../../../Common/Services/StandardPMs/AgentPMService");
var CustomerPMService_1 = require("../../../../../Common/Services/StandardPMs/CustomerPMService");
var CustomAgentPMService_1 = require("../../../../../Common/Services/StandardPMs/CustomAgentPMService");
var ShippingAgentPMService_1 = require("../../../../../Common/Services/StandardPMs/ShippingAgentPMService");
var VendorPMService_1 = require("../../../../../Common/Services/StandardPMs/VendorPMService");
var WarehousePMService_1 = require("../../../../../Common/Services/StandardPMs/WarehousePMService");
var AirlinePMService_1 = require("../../../../../Common/Services/StandardPMs/AirlinePMService");
var ShippingLinePMService_1 = require("../../../../../Common/Services/StandardPMs/ShippingLinePMService");
var TruckerPMService_1 = require("../../../../../Common/Services/StandardPMs/TruckerPMService");
var PartnersDomainService_1 = require("../../../../../Common/Services/PartnersDomainService");
var AddressValidator_1 = require("../../../../../Infrastructure/Validators/AddressValidator");
var VatNumberValidator_1 = require("../../../../../Infrastructure/Validators/VatNumberValidator");
var InfraSettings_1 = require("../../../../../Infrastructure/Utilities/InfraSettings");
var AWBAddEditPartnerComponent = /** @class */ (function (_super) {
    __extends(AWBAddEditPartnerComponent, _super);
    function AWBAddEditPartnerComponent() {
        var _this = _super.call(this) || this;
        _this.EntityPM = null;
        _this.IsNewEntity = false;
        _this.IsCancelled = false;
        _this.IsUpdatingPartner = false;
        _this.DataContext = _this;
        _this.ObjectTableName = "Address";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.HelpMessage = null;
        _this.ShowHelpMessage = false;
        _this.isPartnerLoaded = false;
        _this.isAddressLoaded = false;
        _this.country = null;
        _this.state = null;
        _this.isPartnerDirty = false;
        return _this;
    }
    AWBAddEditPartnerComponent.prototype.SetWindowArgs = function (windowArgs) {
        this.shipmentPM = windowArgs.EntityPM;
        this.IsNewEntity = windowArgs.IsNewEntity;
        this.PartnerTypeCode = windowArgs.PartnerTypeCode;
        this.InitializeComponent();
    };
    AWBAddEditPartnerComponent.prototype.ngAfterViewInit = function () {
        this.SetUIProperties();
    };
    Object.defineProperty(AWBAddEditPartnerComponent.prototype, "SelectedAddressId", {
        get: function () { return this.selectedAddressId; },
        set: function (newValue) {
            var _this = this;
            if (this.selectedAddressId != newValue) {
                this.oldAddressId = this.selectedAddressId;
                this.selectedAddressId = newValue;
                var isLoadingAddress = false;
                if (this.EntityPM == null) {
                    isLoadingAddress = true;
                }
                else if (!this.EntityPM.IsDirty) {
                    isLoadingAddress = true;
                }
                if (isLoadingAddress) {
                    this.CurrentSession.StartBusyIndicatorLoading();
                    this.CurrentAddressId = newValue;
                    this.LoadAddress();
                }
                else {
                    var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                    confirmWindow.Width = 450;
                    confirmWindow.Height = 190;
                    confirmWindow.ShowCancelButton = true;
                    confirmWindow.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.DontSave");
                    confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Save");
                    confirmWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.UnSavedChanges");
                    confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.ThisEntityhasunsavedchanges").replace("%Entity", this.ObjectTableName));
                    confirmWindow.WindowClosed.subscribe(function (event) {
                        if (confirmWindow.Yes) {
                            var isValid = _this.Validate();
                            if (isValid) {
                                _this.CurrentSession.StartBusyIndicatorSaving();
                                _this.Save(true);
                            }
                        }
                        else if (confirmWindow.Cancel) {
                            _this.selectedAddressId = _this.oldAddressId;
                        }
                        else if (confirmWindow.No) {
                            _this.CurrentSession.StartBusyIndicatorLoading();
                            _this.CurrentAddressId = newValue;
                            _this.LoadAddress();
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    AWBAddEditPartnerComponent.prototype.InitializeComponent = function () {
        var _this = this;
        if (this.IsNewEntity) {
            if (this.PartnerTypeCode == "AGT" || this.shipmentPM.ShipmentLevelCode == "C") {
                this.PartnerTypeId = "AG";
                this.myAgentPM = new AgentPM_1.AgentPM();
                this.myAgentPM.Tenant = this.shipmentPM.Tenant;
                this.myAgentPM.PartnerTypeId = this.PartnerTypeId;
                this.myAgentPM.Code = "new";
                this.EntityPM = new AddressPM_1.AddressPM();
                this.EntityPM.Tenant = this.shipmentPM.Tenant;
                this.EntityPM.AddressTypeId = "M";
                this.EntityPM.Description = "Main Address";
                this.myAgentPM.Addresses.push(this.EntityPM);
            }
            else {
                this.PartnerTypeId = "CS";
                this.myCustomerPM = new CustomerPM_1.CustomerPM();
                this.myCustomerPM.Tenant = this.shipmentPM.Tenant;
                this.myCustomerPM.PartnerTypeId = this.PartnerTypeId;
                this.myCustomerPM.CustomerStatusCode = "ACT";
                this.myCustomerPM.IsCustomer = this.PartnerTypeCode == "SHI" ? true : false;
                this.myCustomerPM.Code = "new";
                this.EntityPM = new AddressPM_1.AddressPM();
                this.EntityPM.Tenant = this.shipmentPM.Tenant;
                this.EntityPM.AddressTypeId = "M";
                this.EntityPM.Description = "Main Address";
                this.myCustomerPM.Addresses.push(this.EntityPM);
            }
            if (this.shipmentPM.ShipmentLevelCode == "C") {
                switch (this.PartnerTypeCode) {
                    case "SHI": {
                        this.HelpMessage = "Shipper will be added as Agent";
                        this.ShowHelpMessage = true;
                        break;
                    }
                    case "CON": {
                        this.HelpMessage = "Consignee will be added as Agent";
                        this.ShowHelpMessage = true;
                        break;
                    }
                    case "NTF": {
                        this.HelpMessage = "Notify will be added as Agent";
                        this.ShowHelpMessage = true;
                        break;
                    }
                }
            }
            this.SetUIProperties();
        }
        else {
            switch (this.PartnerTypeCode) {
                case "SHI":
                    {
                        this.CurrentPartnerId = this.shipmentPM.ShipperId;
                        this.CurrentAddressId = this.shipmentPM.ShipperAddressId;
                        break;
                    }
                case "CON":
                    {
                        this.CurrentPartnerId = this.shipmentPM.ConsigneeId;
                        this.CurrentAddressId = this.shipmentPM.ConsigneeAddressId;
                        break;
                    }
                case "AGT":
                    {
                        this.CurrentPartnerId = this.shipmentPM.IssuingCarrierAgentId;
                        this.CurrentAddressId = this.shipmentPM.IssuingCarrierAddressId;
                        break;
                    }
                case "NTF":
                    {
                        this.CurrentPartnerId = this.shipmentPM.Notify1Id;
                        this.CurrentAddressId = this.shipmentPM.Notify1AddressId;
                        break;
                    }
            }
            this.selectedAddressId = this.CurrentAddressId;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CurrentPartnerId)) {
                this.CurrentSession.StartBusyIndicatorLoading();
                var myService = new CardListService_1.CardListService();
                myService.getSingle(this.CurrentPartnerId).subscribe(function (myResponse) {
                    if (myResponse != null) {
                        if (myResponse.HasError) {
                            _this.ValidationErrorsList = myResponse.ErrorsArray;
                            _this.CurrentSession.StopBusyIndicator();
                        }
                        else {
                            _this.PartnerTypeId = myResponse.Result.PartnerTypeId;
                            _this.LoadPartner();
                            _this.LoadAddress();
                        }
                    }
                    else {
                        _this.CurrentSession.StopBusyIndicator();
                    }
                });
            }
        }
    };
    AWBAddEditPartnerComponent.prototype.LoadPartner = function () {
        var _this = this;
        this.isPartnerLoaded = false;
        switch (this.PartnerTypeId) {
            case "AG": {
                var myAgentService = new AgentPMService_1.AgentPMService();
                myAgentService.get(this.CurrentPartnerId).subscribe(function (myResponse) {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            _this.myAgentPM = myResponse.Result;
                            _this.isPartnerLoaded = true;
                            _this.OnLoadCompleted();
                        }
                    }
                });
                break;
            }
            case "CS": {
                var myCustomerService = new CustomerPMService_1.CustomerPMService();
                myCustomerService.get(this.CurrentPartnerId).subscribe(function (myResponse) {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            _this.myCustomerPM = myResponse.Result;
                            _this.isPartnerLoaded = true;
                            _this.OnLoadCompleted();
                        }
                    }
                });
                break;
            }
            case "CG": {
                var myCustomAgentService = new CustomAgentPMService_1.CustomAgentPMService();
                myCustomAgentService.get(this.CurrentPartnerId).subscribe(function (myResponse) {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            _this.myCustomAgentPM = myResponse.Result;
                            _this.isPartnerLoaded = true;
                            _this.OnLoadCompleted();
                        }
                    }
                });
                break;
            }
            case "SG": {
                var myShippingAgentService = new ShippingAgentPMService_1.ShippingAgentPMService();
                myShippingAgentService.get(this.CurrentPartnerId).subscribe(function (myResponse) {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            _this.myShippingAgentPM = myResponse.Result;
                            _this.isPartnerLoaded = true;
                            _this.OnLoadCompleted();
                        }
                    }
                });
                break;
            }
            case "VD": {
                var myVendorService = new VendorPMService_1.VendorPMService();
                myVendorService.get(this.CurrentPartnerId).subscribe(function (myResponse) {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            _this.myVendorPM = myResponse.Result;
                            _this.isPartnerLoaded = true;
                            _this.OnLoadCompleted();
                        }
                    }
                });
                break;
            }
            case "WH": {
                var myWarehouseService = new WarehousePMService_1.WarehousePMService();
                myWarehouseService.get(this.CurrentPartnerId).subscribe(function (myResponse) {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            _this.myWarehousePM = myResponse.Result;
                            _this.isPartnerLoaded = true;
                            _this.OnLoadCompleted();
                        }
                    }
                });
                break;
            }
            case "AL": {
                var myAirlineService = new AirlinePMService_1.AirlinePMService();
                myAirlineService.get(this.CurrentPartnerId).subscribe(function (myResponse) {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            _this.myAirlinePM = myResponse.Result;
                            _this.isPartnerLoaded = true;
                            _this.OnLoadCompleted();
                        }
                    }
                });
                break;
            }
            case "SL": {
                var myShippingLineService = new ShippingLinePMService_1.ShippingLinePMService();
                myShippingLineService.get(this.CurrentPartnerId).subscribe(function (myResponse) {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            _this.myShippingLinePM = myResponse.Result;
                            _this.isPartnerLoaded = true;
                            _this.OnLoadCompleted();
                        }
                    }
                });
                break;
            }
            case "TR": {
                var myTruckerService = new TruckerPMService_1.TruckerPMService();
                myTruckerService.get(this.CurrentPartnerId).subscribe(function (myResponse) {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            _this.myTruckerPM = myResponse.Result;
                            _this.isPartnerLoaded = true;
                            _this.OnLoadCompleted();
                        }
                    }
                });
                break;
            }
        }
    };
    AWBAddEditPartnerComponent.prototype.LoadAddress = function () {
        var _this = this;
        this.isAddressLoaded = false;
        var myService = new AddressPMService_1.AddressPMService();
        myService.get(this.CurrentAddressId).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    _this.EntityPM = myResponse.Result;
                    _this.isAddressLoaded = true;
                    _this.OnLoadCompleted();
                }
            }
        });
    };
    AWBAddEditPartnerComponent.prototype.OnLoadCompleted = function () {
        if (this.isPartnerLoaded && this.isAddressLoaded) {
            this.SetUIProperties();
            this.CurrentSession.StopBusyIndicator();
        }
    };
    // SetUIProperties
    AWBAddEditPartnerComponent.prototype.SetUIProperties = function () {
        if (this.EntityPM != null) {
            this.UIProperties.SetRequired("CardEnglishName", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.CardEnglishName) ? true : false);
            if (this.EntityPM.AddressTypeId == "O") {
                this.UIProperties.SetVisibility("Description", this.ObjectTableName, true);
                this.UIProperties.SetVisibility("InActive", this.ObjectTableName, true);
            }
            else {
                this.UIProperties.SetVisibility("Description", this.ObjectTableName, false);
                this.UIProperties.SetVisibility("InActive", this.ObjectTableName, false);
            }
            this.SetUIProperties_VAT();
            this.SetUIProperties_State();
            this.SetUIProperties_TelFax();
        }
    };
    AWBAddEditPartnerComponent.prototype.SetUIProperties_VAT = function () {
        if (this.myCustomerPM != null) {
            if (this.myCustomerPM.IsCustomer) {
                var args = new VatNumberValidator_1.VATValidatorArgs();
                args.VATNumber = this.VatNumber;
                args.IsCustomer = this.myCustomerPM.IsCustomer;
                args.PartnerTypeId = this.PartnerTypeId;
                args.CountryId = this.CountryId;
                args.CountryName = this.CountryName;
                args.CountryEnglishName = this.CountryEnglishName;
                args.SetReady = false;
                VatNumberValidator_1.VatNumberValidator.ValidateVatFormat(args);
                VatNumberValidator_1.VatNumberValidator.ValidateVatMandatory(args);
                this.UIProperties.SetRequired("VatNumber", this.ObjectTableName, args.Errors.length > 0 ? true : false);
            }
        }
    };
    AWBAddEditPartnerComponent.prototype.SetUIProperties_State = function () {
        if (this.EntityPM != null) {
            this.SetUIProperties_StateEnabled();
            this.SetUIProperties_StateRequired();
        }
    };
    AWBAddEditPartnerComponent.prototype.SetUIProperties_StateEnabled = function () {
        var isEnabled = false;
        if (this.Country != null) {
            if (this.Country.HasStates) {
                isEnabled = true;
            }
        }
        this.UIProperties.SetEnabled("StateId", this.ObjectTableName, isEnabled);
    };
    AWBAddEditPartnerComponent.prototype.SetUIProperties_StateRequired = function () {
        var isRequired = false;
        if (this.Country != null) {
            if (this.State == null) {
                if (this.Country.IsStateRequired) {
                    isRequired = true;
                }
            }
        }
        this.UIProperties.SetRequired("StateId", this.ObjectTableName, isRequired);
    };
    AWBAddEditPartnerComponent.prototype.SetUIProperties_TelFax = function () {
        var isTelRequired = false;
        var isFaxRequired = false;
        if (this.myCustomerPM != null) {
            if (this.myCustomerPM.IsCustomer) {
                if (this.myCustomerPM.PartnerTypeId == "CS") {
                    if (InfraSettings_1.InfraSettings.TenantPM.IsCustomerTelRequired) {
                        if (Tools_1.AppTool.IsNullOrEmpty(this.PhoneNumber)) {
                            isTelRequired = true;
                        }
                    }
                    if (InfraSettings_1.InfraSettings.TenantPM.IsCustomerFaxRequired) {
                        if (Tools_1.AppTool.IsNullOrEmpty(this.FaxNumber)) {
                            isFaxRequired = true;
                        }
                    }
                }
                else if (this.myCustomerPM.PartnerTypeId == "PO") {
                    if (InfraSettings_1.InfraSettings.TenantPM.IsPotentialTelRequired) {
                        if (Tools_1.AppTool.IsNullOrEmpty(this.PhoneNumber)) {
                            isTelRequired = true;
                        }
                    }
                    if (InfraSettings_1.InfraSettings.TenantPM.IsPotentialFaxRequired) {
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
    Object.defineProperty(AWBAddEditPartnerComponent.prototype, "Description", {
        // Properties
        get: function () { return this.EntityPM == null ? null : this.EntityPM.Description; },
        set: function (newValue) {
            if (this.EntityPM != null) {
                if (this.EntityPM.Description != newValue) {
                    this.EntityPM.Description = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBAddEditPartnerComponent.prototype, "CardEnglishName", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.CardEnglishName; },
        set: function (newValue) {
            if (this.EntityPM != null) {
                if (this.EntityPM.CardEnglishName != newValue) {
                    this.EntityPM.Name = newValue;
                    this.EntityPM.CardEnglishName = newValue;
                    if (!Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                        if (newValue.length > 70) {
                            this.EntityPM.Name = newValue.substr(0, 70);
                        }
                    }
                    this.UIProperties.SetRequired("CardEnglishName", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(newValue) ? true : false);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBAddEditPartnerComponent.prototype, "Address1", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.Address1; },
        set: function (newValue) {
            if (this.EntityPM != null) {
                if (this.EntityPM.Address1 != newValue) {
                    this.EntityPM.Address1 = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBAddEditPartnerComponent.prototype, "Address2", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.Address2; },
        set: function (newValue) {
            if (this.EntityPM != null) {
                if (this.EntityPM.Address2 != newValue) {
                    this.EntityPM.Address2 = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBAddEditPartnerComponent.prototype, "City", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.City; },
        set: function (newValue) {
            if (this.EntityPM != null) {
                if (this.EntityPM.City != newValue) {
                    this.EntityPM.City = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBAddEditPartnerComponent.prototype, "ATTN", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.ATTN; },
        set: function (newValue) {
            if (this.EntityPM != null) {
                if (this.EntityPM.ATTN != newValue) {
                    this.EntityPM.ATTN = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBAddEditPartnerComponent.prototype, "ZipCode", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.ZipCode; },
        set: function (newValue) {
            if (this.EntityPM != null) {
                if (this.EntityPM.ZipCode != newValue) {
                    this.EntityPM.ZipCode = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBAddEditPartnerComponent.prototype, "PhoneNumber", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.PhoneNumber; },
        set: function (newValue) {
            if (this.EntityPM != null) {
                if (this.EntityPM.PhoneNumber != newValue) {
                    this.EntityPM.PhoneNumber = newValue;
                    this.SetUIProperties_TelFax();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBAddEditPartnerComponent.prototype, "FaxNumber", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.FaxNumber; },
        set: function (newValue) {
            if (this.EntityPM != null) {
                if (this.EntityPM.FaxNumber != newValue) {
                    this.EntityPM.FaxNumber = newValue;
                    this.SetUIProperties_TelFax();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBAddEditPartnerComponent.prototype, "VatNumber", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.VatNumber; },
        set: function (newValue) {
            if (this.EntityPM != null) {
                if (this.EntityPM.VatNumber != newValue) {
                    this.EntityPM.VatNumber = newValue;
                    this.SetUIProperties_VAT();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBAddEditPartnerComponent.prototype, "Country", {
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
    Object.defineProperty(AWBAddEditPartnerComponent.prototype, "CountryId", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.CountryId; },
        set: function (newValue) {
            if (this.EntityPM != null) {
                if (this.EntityPM.CountryId != newValue) {
                    this.EntityPM.CountryId = newValue;
                    this.StateId = null;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBAddEditPartnerComponent.prototype, "CountryCode", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.CountryCode; },
        set: function (newValue) {
            if (this.EntityPM != null) {
                if (this.EntityPM.CountryCode != newValue) {
                    this.EntityPM.CountryCode = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBAddEditPartnerComponent.prototype, "CountryName", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.CountryName; },
        set: function (newValue) {
            if (this.EntityPM != null) {
                if (this.EntityPM.CountryName != newValue) {
                    this.EntityPM.CountryName = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBAddEditPartnerComponent.prototype, "CountryEnglishName", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.CountryEnglishName; },
        set: function (newValue) {
            if (this.EntityPM != null) {
                if (this.EntityPM.CountryEnglishName != newValue) {
                    this.EntityPM.CountryEnglishName = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBAddEditPartnerComponent.prototype, "State", {
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
    Object.defineProperty(AWBAddEditPartnerComponent.prototype, "StateId", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.StateId; },
        set: function (newValue) {
            if (this.EntityPM != null) {
                if (this.EntityPM.StateId != newValue) {
                    this.EntityPM.StateId = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBAddEditPartnerComponent.prototype, "StateCode", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.StateCode; },
        set: function (newValue) {
            if (this.EntityPM != null) {
                if (this.EntityPM.StateCode != newValue) {
                    this.EntityPM.StateCode = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBAddEditPartnerComponent.prototype, "StateEnglishName", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.StateEnglishName; },
        set: function (newValue) {
            if (this.EntityPM != null) {
                if (this.EntityPM.StateEnglishName != newValue) {
                    this.EntityPM.StateEnglishName = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBAddEditPartnerComponent.prototype, "IsLocalLanguage", {
        get: function () { return this.EntityPM == null ? false : this.EntityPM.IsLocalLanguage; },
        set: function (newValue) {
            if (this.EntityPM != null) {
                if (this.EntityPM.IsLocalLanguage != newValue) {
                    this.EntityPM.IsLocalLanguage = newValue;
                    if (this.Country != null) {
                        this.CountryName = this.EntityPM.IsLocalLanguage ? this.Country.LocalName : this.Country.EnglishName;
                    }
                    if (!newValue) {
                        if (!Tools_1.AppTool.IsNullOrEmpty(this.Description)) {
                            this.Description = this.Description.replace(/[^\x20-\x7F]/g, "");
                        }
                        if (!Tools_1.AppTool.IsNullOrEmpty(this.CardEnglishName)) {
                            this.CardEnglishName = this.CardEnglishName.replace(/[^\x20-\x7F]/g, "");
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
            }
        },
        enumerable: true,
        configurable: true
    });
    AWBAddEditPartnerComponent.prototype.OnCountryChanged = function (list) {
        if (list == null) {
            this.CountryCode = null;
            this.CountryName = null;
            this.CountryEnglishName = null;
        }
        else {
            this.CountryCode = list.Code;
            this.CountryName = this.IsLocalLanguage ? list.LocalName : list.EnglishName;
            this.CountryEnglishName = list.EnglishName;
        }
        this.SetUIProperties_VAT();
        this.SetUIProperties_State();
    };
    AWBAddEditPartnerComponent.prototype.OnStateChanged = function (list) {
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
    // SelectCity
    AWBAddEditPartnerComponent.prototype.SelectCityCommand = function () {
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
    AWBAddEditPartnerComponent.prototype.CancelButtonClicked = function () {
        this.IsCancelled = true;
        this.CurrentSession.CloseCurrentWindow();
    };
    AWBAddEditPartnerComponent.prototype.OkButtonClicked = function () {
        this.CurrentSession.StartBusyIndicatorSaving();
        var isValid = this.Validate();
        if (!isValid) {
            this.CurrentSession.StopBusyIndicator();
        }
        else {
            if (!this.EntityPM.IsDirty) {
                this.IsUpdatingPartner = true;
                this.CurrentSession.CloseCurrentWindow();
            }
            else {
                switch (this.PartnerTypeId) {
                    case "AG":
                        {
                            if (this.myAgentPM != null) {
                                if (this.myAgentPM.EnglishName != this.CardEnglishName) {
                                    this.myAgentPM.EnglishName = this.CardEnglishName;
                                }
                                if (this.myAgentPM.VatNumber != this.VatNumber) {
                                    this.myAgentPM.VatNumber = this.VatNumber;
                                }
                                this.isPartnerDirty = this.myAgentPM.IsDirty;
                            }
                            break;
                        }
                    case "CS":
                        {
                            if (this.myCustomerPM != null) {
                                if (this.myCustomerPM.EnglishName != this.CardEnglishName) {
                                    this.myCustomerPM.EnglishName = this.CardEnglishName;
                                }
                                if (this.myCustomerPM.VatNumber != this.VatNumber) {
                                    this.myCustomerPM.VatNumber = this.VatNumber;
                                }
                                this.isPartnerDirty = this.myCustomerPM.IsDirty;
                            }
                            break;
                        }
                    case "CG":
                        {
                            if (this.myCustomAgentPM != null) {
                                if (this.myCustomAgentPM.EnglishName != this.CardEnglishName) {
                                    this.myCustomAgentPM.EnglishName = this.CardEnglishName;
                                }
                                if (this.myCustomAgentPM.VatNumber != this.VatNumber) {
                                    this.myCustomAgentPM.VatNumber = this.VatNumber;
                                }
                                this.isPartnerDirty = this.myCustomAgentPM.IsDirty;
                            }
                            break;
                        }
                    case "SG":
                        {
                            if (this.myShippingAgentPM != null) {
                                if (this.myShippingAgentPM.EnglishName != this.CardEnglishName) {
                                    this.myShippingAgentPM.EnglishName = this.CardEnglishName;
                                }
                                if (this.myShippingAgentPM.VatNumber != this.VatNumber) {
                                    this.myShippingAgentPM.VatNumber = this.VatNumber;
                                }
                                this.isPartnerDirty = this.myShippingAgentPM.IsDirty;
                            }
                            break;
                        }
                    case "VD":
                        {
                            if (this.myVendorPM != null) {
                                if (this.myVendorPM.EnglishName != this.CardEnglishName) {
                                    this.myVendorPM.EnglishName = this.CardEnglishName;
                                }
                                if (this.myVendorPM.VatNumber != this.VatNumber) {
                                    this.myVendorPM.VatNumber = this.VatNumber;
                                }
                                this.isPartnerDirty = this.myVendorPM.IsDirty;
                            }
                            break;
                        }
                    case "WH":
                        {
                            if (this.myWarehousePM != null) {
                                if (this.myWarehousePM.EnglishName != this.CardEnglishName) {
                                    this.myWarehousePM.EnglishName = this.CardEnglishName;
                                }
                                if (this.myWarehousePM.VatNumber != this.VatNumber) {
                                    this.myWarehousePM.VatNumber = this.VatNumber;
                                }
                                this.isPartnerDirty = this.myWarehousePM.IsDirty;
                            }
                            break;
                        }
                    case "AL":
                        {
                            if (this.myAirlinePM != null) {
                                if (this.myAirlinePM.EnglishName != this.CardEnglishName) {
                                    this.myAirlinePM.EnglishName = this.CardEnglishName;
                                }
                                if (this.myAirlinePM.VatNumber != this.VatNumber) {
                                    this.myAirlinePM.VatNumber = this.VatNumber;
                                }
                                this.isPartnerDirty = this.myAirlinePM.IsDirty;
                            }
                            break;
                        }
                    case "SL":
                        {
                            if (this.myShippingLinePM != null) {
                                if (this.myShippingLinePM.EnglishName != this.CardEnglishName) {
                                    this.myShippingLinePM.EnglishName = this.CardEnglishName;
                                }
                                if (this.myShippingLinePM.VatNumber != this.VatNumber) {
                                    this.myShippingLinePM.VatNumber = this.VatNumber;
                                }
                                this.isPartnerDirty = this.myShippingLinePM.IsDirty;
                            }
                            break;
                        }
                    case "TR":
                        {
                            if (this.myTruckerPM != null) {
                                if (this.myTruckerPM.EnglishName != this.CardEnglishName) {
                                    this.myTruckerPM.EnglishName = this.CardEnglishName;
                                }
                                if (this.myTruckerPM.VatNumber != this.VatNumber) {
                                    this.myTruckerPM.VatNumber = this.VatNumber;
                                }
                                this.isPartnerDirty = this.myTruckerPM.IsDirty;
                            }
                            break;
                        }
                }
                this.Save(false);
            }
        }
    };
    AWBAddEditPartnerComponent.prototype.Validate = function () {
        var isValid = true;
        var errors = [];
        if (this.EntityPM != null) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
            Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
            var isLanguageValid = AddressValidator_1.AddressValidator.IsMainAddressEnglishCharacters(this.EntityPM);
            if (!isLanguageValid) {
                errors.push("Main address does not allow non-english characters");
            }
            if (this.Country != null) {
                if (this.State == null) {
                    if (this.Country.IsStateRequired) {
                        errors.push(msg.replace("%FieldName", "State"));
                    }
                }
            }
            this.ValidateCustomerFields(errors);
            if (errors.length == 0) {
                var myCity = this.City;
                var myName = this.CardEnglishName;
                if (!Tools_1.AppTool.IsNullOrEmpty(myCity)) {
                    myCity = myCity.trim();
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(myName)) {
                    myName = myName.trim();
                }
                if (Tools_1.AppTool.IsNullOrEmpty(myCity)) {
                    errors.push(msg.replace("%FieldName", "City"));
                }
                if (Tools_1.AppTool.IsNullOrEmpty(myName)) {
                    errors.push(msg.replace("%FieldName", "Name"));
                }
            }
        }
        if (errors.length > 0) {
            isValid = false;
        }
        this.ValidationErrorsList = errors;
        return isValid;
    };
    AWBAddEditPartnerComponent.prototype.ValidateCustomerFields = function (errors) {
        if (this.myCustomerPM != null) {
            if (this.myCustomerPM.IsCustomer) {
                var args = new VatNumberValidator_1.VATValidatorArgs();
                args.VATNumber = this.VatNumber;
                args.IsCustomer = this.myCustomerPM.IsCustomer;
                args.PartnerTypeId = this.PartnerTypeId;
                args.CountryId = this.CountryId;
                args.CountryName = this.CountryName;
                args.CountryEnglishName = this.CountryEnglishName;
                args.SetReady = false;
                VatNumberValidator_1.VatNumberValidator.ValidateVatFormat(args);
                VatNumberValidator_1.VatNumberValidator.ValidateVatMandatory(args);
                args.Errors.forEach(function (item) {
                    errors.push(item);
                });
                if (this.PartnerTypeId == "CS") {
                    if (InfraSettings_1.InfraSettings.TenantPM.IsCustomerTelRequired) {
                        if (Tools_1.AppTool.IsNullOrEmpty(this.PhoneNumber)) {
                            errors.push("Phone Number is required");
                        }
                    }
                    if (InfraSettings_1.InfraSettings.TenantPM.IsCustomerFaxRequired) {
                        if (Tools_1.AppTool.IsNullOrEmpty(this.FaxNumber)) {
                            errors.push("Fax Number is required");
                        }
                    }
                }
                else if (this.PartnerTypeId == "PO") {
                    if (InfraSettings_1.InfraSettings.TenantPM.IsPotentialTelRequired) {
                        if (Tools_1.AppTool.IsNullOrEmpty(this.PhoneNumber)) {
                            errors.push("Phone Number is required");
                        }
                    }
                    if (InfraSettings_1.InfraSettings.TenantPM.IsPotentialFaxRequired) {
                        if (Tools_1.AppTool.IsNullOrEmpty(this.FaxNumber)) {
                            errors.push("Fax Number is required");
                        }
                    }
                }
            }
        }
    };
    AWBAddEditPartnerComponent.prototype.Save = function (isSelectedAddressSaving) {
        var _this = this;
        if (this.myPartnersDomainService == null) {
            this.myPartnersDomainService = new PartnersDomainService_1.PartnersDomainService();
        }
        var args = new PartnersDomainService_1.PartnerServicePM();
        args.Tenant = this.EntityPM.Tenant;
        args.AddressId = this.CurrentAddressId;
        args.PartnerId = this.CurrentPartnerId;
        args.PartnerTypeId = this.PartnerTypeId;
        args.IsAddressDirty = this.EntityPM.IsDirty;
        args.IsPartnerDirty = this.isPartnerDirty;
        args.Address = this.EntityPM;
        args.Agent = this.myAgentPM;
        args.Customer = this.myCustomerPM;
        args.CustomAgent = this.myCustomAgentPM;
        args.ShippingAgent = this.myShippingAgentPM;
        args.Vendor = this.myVendorPM;
        args.Warehouse = this.myWarehousePM;
        args.Airline = this.myAirlinePM;
        args.ShippingLine = this.myShippingLinePM;
        args.Trucker = this.myTruckerPM;
        this.myPartnersDomainService.PostPartnerAddress(args).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                if (isSelectedAddressSaving) {
                    _this.CurrentSession.StartBusyIndicatorLoading();
                    _this.CurrentAddressId = _this.selectedAddressId;
                    _this.LoadAddress();
                }
                else {
                    _this.CurrentAddressId = myResponse.Result.AddressId;
                    _this.CurrentPartnerId = myResponse.Result.PartnerId;
                    _this.IsUpdatingPartner = true;
                    _this.CurrentSession.CloseCurrentWindow();
                }
            }
            else {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
                if (isSelectedAddressSaving) {
                    _this.selectedAddressId = _this.oldAddressId;
                }
            }
        });
    };
    AWBAddEditPartnerComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AWBAddEditPartnerComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AWBAddEditPartnerComponent);
    return AWBAddEditPartnerComponent;
}(BaseComponent_1.BaseComponent));
exports.AWBAddEditPartnerComponent = AWBAddEditPartnerComponent;
//# sourceMappingURL=AWBAddEditPartnerComponent.js.map