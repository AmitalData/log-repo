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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var QuotePMService_1 = require("../../Services/StandardPMs/QuotePMService");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var QuoteUtilities_1 = require("../../Utilities/QuoteUtilities");
var PartnersDomainService_1 = require("../../../Common/Services/PartnersDomainService");
var AddressPM_1 = require("../../../Common/EntityPMs/AddressPM");
var PortListService_1 = require("../../../Common/Services/StandardLists/PortListService");
var CardListService_1 = require("../../../Common/Services/StandardLists/CardListService");
var AddressListService_1 = require("../../../Common/Services/StandardLists/AddressListService");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var Args_1 = require("../../../Common/Args");
var Args_2 = require("../../../Infrastructure/Args");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var QuoteValidator_1 = require("../../Validators/QuoteValidator");
var QuotePMInitService_1 = require("../../EntityPMInitServices/QuotePMInitService");
var QuoteDomainService_1 = require("../../Services/QuoteDomainService");
var NewQuoteComponent = /** @class */ (function (_super) {
    __extends(NewQuoteComponent, _super);
    function NewQuoteComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Quote";
        _this.LabelColumnWidth = 120;
        _this.ControlColumnWidth = 220;
        _this.CardDependencyProperty1 = "CS,PO";
        _this.IsLCLEntity = false;
        _this.IsFCLEntity = false;
        _this.IsInlandDomestic = false;
        _this.QuoteSetting = null;
        _this.IsAddAgentVisible = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsCopyFromQuote = false;
        _this.DefaultCustomerId = null;
        _this.ScreenOpacity = 0.7;
        _this.IsScreenEnabled = false;
        _this.QuoteTypeIsEnabled = false;
        _this.IsFillDimensionsEnabled = false;
        // Filters
        _this.DirectionsList = [];
        _this.TransportModesList = [];
        _this.ShipmentTypesList = [];
        _this.ShipmentTypeName = null;
        // Shipper
        _this.isShipperMyCustomer = false;
        // Consignee
        _this.isConsigneeMyCustomer = false;
        //Customer
        _this.CustomerDependencyProperty1 = "CS";
        _this.CustomerDependencyProperty1IsList = false;
        _this.IsCustomerRequired = false;
        // General
        _this.RoutingHelpHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.F.QuoteTypeCode");
        _this.RoutingHelpText = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.QuoteTypeCodeHelpText");
        // Main Carriage
        _this.FromPortList = null;
        _this.ToPortList = null;
        // Additional Fields
        _this.Retries = 0;
        //Copy Mode
        _this.ButtonContent = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.B.Create");
        _this.IsCopyOtherPartnersVisible = false;
        _this.ShipperCopyIsEnabled = false;
        _this.ConsigneeCopyIsEnabled = false;
        _this.AgentCopyIsEnabled = false;
        _this.NotifyCopyIsEnabled = false;
        _this.MainCarriageCopyIsEnabled = false;
        _this.ChargesTypesCopyIsEnabled = false;
        _this.CopyCostIsChecked = false;
        _this.CopySaleIsChecked = false;
        _this.shipperCopyIsChecked = false;
        _this.consigneeCopyIsChecked = false;
        _this.agentCopyIsChecked = false;
        _this.notifyCopyIsChecked = false;
        _this.mainCarriageCopyIsChecked = false;
        _this.copyPickUpIsChecked = false;
        _this.copyDeliveryIsChecked = false;
        _this.chargesTypesCopyIsChecked = false;
        _this.SessionIndex = SessionLocator_1.SessionLocator.Index;
        _this.InitializeServices();
        _this.EntityPM = _this.myQuotePMService.GetNewEntityPM();
        QuotePMInitService_1.QuotePMInitService.InitValues(_this.EntityPM, true);
        if (SessionLocator_1.SessionLocator.TenantPM.AllowAgentInCustomersLOV) {
            _this.CardDependencyProperty1 = "CS,PO,AG";
            _this.IsAddAgentVisible = true;
        }
        return _this;
    }
    NewQuoteComponent.prototype.ngOnInit = function () {
        this.BuildFiltersLists();
        this.OnFiltersChanged();
        this.BuildAdditionalFields();
        this.LoadAllowedAirline();
    };
    NewQuoteComponent.prototype.InitializeServices = function () {
        this.myPortListService = new PortListService_1.PortListService();
        this.myQuotePMService = new QuotePMService_1.QuotePMService();
        this.myCardListService = new CardListService_1.CardListService();
        this.myAddressListService = new AddressListService_1.AddressListService();
        this.myPartnersDomainService = new PartnersDomainService_1.PartnersDomainService();
    };
    NewQuoteComponent.prototype.LoadAllowedAirline = function () {
        var _this = this;
        if (SessionLocator_1.SessionLocator.TenantManagementJS.IsRestrictedByAirline) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                if (this.EntityPM.TransportModeId == "A") {
                    this.myPartnersDomainService.GetAllowedAirlineId().subscribe(function (myResponse) {
                        if (myResponse != null) {
                            if (myResponse.HasError) {
                                _this.ValidationErrorsList = myResponse.ErrorsArray;
                            }
                            else {
                                var allowedAirlineId = myResponse.Result;
                                if (!Tools_1.AppTool.IsNullOrEmpty(allowedAirlineId)) {
                                    _this.MainCarriageCarrierId = allowedAirlineId;
                                }
                            }
                        }
                    });
                }
            }
        }
    };
    NewQuoteComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.sourceEntityPM = args.Quote;
        this.IsCopyFromQuote = args.IsCopyFromQuote;
        this.DefaultCustomerId = args.DefaultCustomerId;
        this.OpportunityId = args.OpportunityId;
        this.BuildFiltersLists();
        this.SetUIProperties();
        if (this.IsCopyFromQuote) {
            var myDomainService = new QuoteDomainService_1.QuoteDomainService();
            myDomainService.GetQuoteSettings().subscribe(function (myResponse) {
                if (myResponse.HasError == false) {
                    if (myResponse.Result) {
                        if (myResponse.Result.Id) {
                            _this.QuoteSetting = myResponse.Result;
                        }
                        _this.InitializeCopy(_this.sourceEntityPM);
                    }
                }
            });
        }
    };
    NewQuoteComponent.prototype.SetScreenEnabled = function () {
        var isScreenEnabled = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.DirectionId) && !Tools_1.AppTool.IsNullOrEmpty(this.TransportModeId)) {
            if (this.TransportModeId == "A") {
                isScreenEnabled = true;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipmentTypeId)) {
                isScreenEnabled = true;
            }
        }
        this.ScreenOpacity = isScreenEnabled ? 1 : 0.7;
        if (isScreenEnabled) {
            if (this.IsCopyFromQuote) {
                this.QuoteTypeIsEnabled = false;
            }
            else {
                this.QuoteTypeIsEnabled = true;
            }
        }
        this.IsScreenEnabled = isScreenEnabled;
        this.IsAddConsigneeEnabled = this.IsScreenEnabled;
        this.IsAddShipperEnabled = this.IsScreenEnabled;
        this.UIProperties.SetEnabled("ShipperId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ConsigneeId", this.ObjectTableName, isScreenEnabled);
        if (!Tools_1.AppTool.IsNullOrEmpty(this.DefaultCustomerId)) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipperId) && isScreenEnabled) {
                this.UIProperties.SetEnabled("ShipperId", this.ObjectTableName, false);
                this.IsAddShipperEnabled = false;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ConsigneeId) && isScreenEnabled) {
                this.UIProperties.SetEnabled("ConsigneeId", this.ObjectTableName, false);
                this.IsAddConsigneeEnabled = false;
            }
        }
        // Shipper
        //this.UIProperties.SetEnabled("ShipperId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ShipperAddressId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ShipperContactId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ShipperReference1", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ShipperReference2", this.ObjectTableName, isScreenEnabled);
        // Consignee
        //this.UIProperties.SetEnabled("ConsigneeId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ConsigneeAddressId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ConsigneeContactId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ConsigneeReference1", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ConsigneeReference2", this.ObjectTableName, isScreenEnabled);
        //Customer
        this.UIProperties.SetEnabled("CustomerId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("QuoteCustomerTypeCode", this.ObjectTableName, isScreenEnabled);
        // General
        this.UIProperties.SetEnabled("IncotermId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("MoveTypeId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ExpirationDays", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ExpirationDate", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("IsAutomaticallyClosed", this.ObjectTableName, isScreenEnabled);
        // Pickup
        this.UIProperties.SetEnabled("IncludePickUp", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("PickUpAddressId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("FromAddressZipCode", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("FromAddressCity", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("FromAddressCountryId", this.ObjectTableName, isScreenEnabled);
        // Routing
        var isPortsEnabled = false;
        if (isScreenEnabled) {
            if (this.IsCopyFromQuote) {
                if (this.QuoteSetting) {
                    if (this.QuoteSetting.EditMainCarriage) {
                        isPortsEnabled = true;
                    }
                }
            }
            else {
                isPortsEnabled = true;
            }
        }
        this.UIProperties.SetEnabled("FromPortId", this.ObjectTableName, isPortsEnabled);
        this.UIProperties.SetEnabled("ToPortId", this.ObjectTableName, isPortsEnabled);
        this.UIProperties.SetEnabled("MainCarriageCarrierId", this.ObjectTableName, isScreenEnabled);
        // Delivery
        this.UIProperties.SetEnabled("IncludeDelivery", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("DeliveryAddressId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ToAddressZipCode", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ToAddressCity", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ToAddressCountryId", this.ObjectTableName, isScreenEnabled);
        // Expected Order
        this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ChargeableWeight", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("NumberOfPackages", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("PackageType1Quantity", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("PackageType2Quantity", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("PackageType3Quantity", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("PackageType4Quantity", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("PackageType5Quantity", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("PackageType1Id", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("PackageType2Id", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("PackageType3Id", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("PackageType4Id", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("PackageType5Id", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("IsDangerous", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("DescriptionOfGoods", this.ObjectTableName, isScreenEnabled);
        // Additional Fields
        this.SetUIProperties_GeneratedComponent();
        this.SetUIProperties_Dimentions();
    };
    NewQuoteComponent.prototype.OnFiltersChanged = function () {
        this.SetScreenEnabled();
        this.SetUIProperties();
        this.SetLabels();
        this.SetUnits();
        this.SetPartners();
    };
    NewQuoteComponent.prototype.SetUIProperties = function () {
        this.SetUIProperties_Shipper();
        this.SetUIProperties_Consignee();
        this.SetUIProperties_Customer();
        this.SetUIProperties_Domestic();
        this.SetUIProperties_Containers();
        this.SetUIProperties_NumberOfPackages();
        this.SetUIProperties_AutomaticallyClosed();
        this.SetUIProperties_Dimentions();
    };
    NewQuoteComponent.prototype.SetUIProperties_Shipper = function () {
        //var isFieldRequired: boolean = false;
        //if (AppTool.IsNullOrEmpty(this.ShipperId)) {
        //    if (this.IsShipperMyCustomer) {
        //        isFieldRequired = true;
        //    }
        //    else if (this.DirectionId == "E" || this.DirectionId == "D" || this.DirectionId == "R" || this.DirectionId == null) {
        //        isFieldRequired = true;
        //    }
        //}
        //this.UIProperties.SetRequired("ShipperId", this.ObjectTableName, isFieldRequired);
        var isShipperFieldsVisible = !Tools_1.AppTool.IsNullOrEmpty(this.ShipperId);
        this.UIProperties.SetVisibility("ShipperContactId", this.ObjectTableName, isShipperFieldsVisible);
        this.UIProperties.SetVisibility("ShipperReference1", this.ObjectTableName, isShipperFieldsVisible);
        this.UIProperties.SetVisibility("ShipperReference2", this.ObjectTableName, isShipperFieldsVisible);
    };
    NewQuoteComponent.prototype.SetUIProperties_Consignee = function () {
        //var isFieldRequired: boolean = false;
        //if (AppTool.IsNullOrEmpty(this.ConsigneeId)) {
        //    if (this.IsConsigneeMyCustomer) {
        //        isFieldRequired = true;
        //    }
        //    else if (this.EntityPM.DirectionId == "I") {
        //        isFieldRequired = true;
        //    }
        //    else if (this.IsInlandDomestic) {
        //        isFieldRequired = true;
        //    }
        //}
        //this.UIProperties.SetRequired("ConsigneeId", this.ObjectTableName, isFieldRequired);
        var isConsigneeFieldsVisible = !Tools_1.AppTool.IsNullOrEmpty(this.ConsigneeId);
        this.UIProperties.SetVisibility("ConsigneeContactId", this.ObjectTableName, isConsigneeFieldsVisible);
        this.UIProperties.SetVisibility("ConsigneeReference1", this.ObjectTableName, isConsigneeFieldsVisible);
        this.UIProperties.SetVisibility("ConsigneeReference2", this.ObjectTableName, isConsigneeFieldsVisible);
    };
    NewQuoteComponent.prototype.SetUIProperties_Customer = function () {
        //var isFieldEnabled: boolean = true;
        //if (this.ShipmentCustomerTypeCode == "SHI") {
        //    if (!AppTool.IsNullOrEmpty(this.ShipperId)) {
        //        isFieldEnabled = false;
        //    }
        //}
        //else if (this.ShipmentCustomerTypeCode == "CON") {
        //    if (!AppTool.IsNullOrEmpty(this.ConsigneeId)) {
        //        isFieldEnabled = false;
        //    }
        //}
        //this.UIProperties.SetEnabled("CustomerId", this.ObjectTableName, isFieldEnabled);
    };
    NewQuoteComponent.prototype.SetUIProperties_Domestic = function () {
        this.IsInlandDomestic = QuoteUtilities_1.QuoteUtilities.IsInlandDomestic(this.EntityPM);
        var fromPortIsrequired = !this.IsInlandDomestic && Tools_1.AppTool.IsNullOrEmpty(this.FromPortId);
        var toPortIsrequired = !this.IsInlandDomestic && Tools_1.AppTool.IsNullOrEmpty(this.ToPortId);
        this.UIProperties.SetRequired("FromPortId", this.ObjectTableName, fromPortIsrequired);
        this.UIProperties.SetRequired("ToPortId", this.ObjectTableName, toPortIsrequired);
        if (this.IsInlandDomestic) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.ShipperId)) {
                this.UIProperties.SetRequired("ShipperId", this.ObjectTableName, true);
            }
            else {
                this.UIProperties.SetRequired("ShipperId", this.ObjectTableName, false);
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.ConsigneeId)) {
                this.UIProperties.SetRequired("ConsigneeId", this.ObjectTableName, true);
            }
            else {
                this.UIProperties.SetRequired("ConsigneeId", this.ObjectTableName, false);
            }
            this.EntityPM.FromPortId = null;
            this.EntityPM.ToPortId = null;
        }
        else {
            this.UIProperties.SetRequired("ShipperId", this.ObjectTableName, false);
            this.UIProperties.SetRequired("ConsigneeId", this.ObjectTableName, false);
            //if (this.EntityPM.DirectionId == "I") {
            //    if (AppTool.IsNullOrEmpty(this.ConsigneeId)) {
            //        this.UIProperties.SetRequired("ConsigneeId", this.ObjectTableName, true);
            //    }
            //}
            //else {
            //    if (AppTool.IsNullOrEmpty(this.ShipperId)) {
            //        this.UIProperties.SetRequired("ShipperId", this.ObjectTableName, true);
            //    }
            //}
        }
    };
    NewQuoteComponent.prototype.SetUIProperties_Containers = function () {
        if (this.EntityPM.QuoteTypeCode == "P") {
            this.UIProperties.SetEnabled("PackageType1Id", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("PackageType2Id", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("PackageType3Id", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("PackageType4Id", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("PackageType5Id", this.ObjectTableName, true);
            this.EntityPM.PackageType1Quantity = null;
            this.EntityPM.PackageType2Quantity = null;
            this.EntityPM.PackageType3Quantity = null;
            this.EntityPM.PackageType4Quantity = null;
            this.EntityPM.PackageType5Quantity = null;
        }
        else {
            this.UIProperties.SetEnabled("PackageType1Id", this.ObjectTableName, this.PackageType1Quantity > 0);
            this.UIProperties.SetEnabled("PackageType2Id", this.ObjectTableName, this.PackageType2Quantity > 0);
            this.UIProperties.SetEnabled("PackageType3Id", this.ObjectTableName, this.PackageType3Quantity > 0);
            this.UIProperties.SetEnabled("PackageType4Id", this.ObjectTableName, this.PackageType4Quantity > 0);
            this.UIProperties.SetEnabled("PackageType5Id", this.ObjectTableName, this.PackageType5Quantity > 0);
            if (Tools_1.AppTool.IsNullOrZero(this.PackageType1Quantity)) {
                this.PackageType1Id = null;
            }
            if (Tools_1.AppTool.IsNullOrZero(this.PackageType2Quantity)) {
                this.PackageType2Id = null;
            }
            if (Tools_1.AppTool.IsNullOrZero(this.PackageType3Quantity)) {
                this.PackageType3Id = null;
            }
            if (Tools_1.AppTool.IsNullOrZero(this.PackageType4Quantity)) {
                this.PackageType4Id = null;
            }
            if (Tools_1.AppTool.IsNullOrZero(this.PackageType5Quantity)) {
                this.PackageType5Id = null;
            }
        }
    };
    NewQuoteComponent.prototype.SetUIProperties_AutomaticallyClosed = function () {
        this.UIProperties.SetEnabled("AutomaticallyCloseDays", this.ObjectTableName, this.IsAutomaticallyClosed);
        this.UIProperties.SetEnabled("AutomaticallyCloseDate", this.ObjectTableName, this.IsAutomaticallyClosed);
    };
    NewQuoteComponent.prototype.SetUIProperties_Dimentions = function () {
        var fillDimEnabled = false;
        if (this.IsAdhoc) {
            fillDimEnabled = this.IsScreenEnabled;
            var isFieldsEnabled = this.EntityPM.QuotePackages.length == 0;
            this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, isFieldsEnabled);
            this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isFieldsEnabled);
            this.UIProperties.SetEnabled("ChargeableWeight", this.ObjectTableName, isFieldsEnabled);
            this.UIProperties.SetEnabled("NumberOfPackages", this.ObjectTableName, isFieldsEnabled);
        }
        else {
            fillDimEnabled = false;
            this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("Volume", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ChargeableWeight", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("NumberOfPackages", this.ObjectTableName, false);
        }
        this.IsFillDimensionsEnabled = fillDimEnabled;
    };
    NewQuoteComponent.prototype.SetUIProperties_NumberOfPackages = function () {
        if (this.EntityPM.TransportModeId != null) {
            if (this.EntityPM.TransportModeId.toUpperCase() == "A") {
                this.UIProperties.SetVisibility("NumberOfPackages", this.ObjectTableName, true);
            }
            else {
                if (this.EntityPM.ShipmentTypeId != null) {
                    if (this.EntityPM.TransportModeId.toUpperCase() == "A"
                        ||
                            (this.EntityPM.TransportModeId.toUpperCase() == "O" && this.EntityPM.ShipmentTypeId.toUpperCase() == "LCLD")
                        ||
                            (this.EntityPM.TransportModeId.toUpperCase() == "I" && this.EntityPM.ShipmentTypeId.toUpperCase() == "LTL")) {
                        this.UIProperties.SetVisibility("NumberOfPackages", this.ObjectTableName, true);
                    }
                }
            }
        }
    };
    NewQuoteComponent.prototype.BuildFiltersLists = function () {
        this.DirectionsList = [];
        this.TransportModesList = [];
        var direct_E = new FilterClass("E", "Export");
        var direct_I = new FilterClass("I", "Import");
        var direct_D = new FilterClass("D", "Domestic");
        var direct_R = new FilterClass("R", "Drop");
        var transport_A = new FilterClass("A", "Air");
        var transport_O = new FilterClass("O", "Ocean");
        var transport_I = new FilterClass("I", "Inland");
        this.DirectionsList.push(direct_E);
        this.DirectionsList.push(direct_I);
        this.DirectionsList.push(direct_D);
        this.DirectionsList.push(direct_R);
        this.TransportModesList.push(transport_A);
        this.TransportModesList.push(transport_O);
        this.TransportModesList.push(transport_I);
        this.BuildShipmentTypes();
    };
    NewQuoteComponent.prototype.BuildShipmentTypes = function () {
        var _this = this;
        this.ShipmentTypesList = [];
        if (this.DirectionId && this.TransportModeId) {
            switch (this.TransportModeId) {
                case "O": {
                    this.ShipmentTypesList.push(new FilterClass("FCLD", "FCL", "./Images/CellIcons/Container.png"));
                    this.ShipmentTypesList.push(new FilterClass("LCLD", "LCL", "./Images/CellIcons/Package.png"));
                    break;
                }
                case "I": {
                    this.ShipmentTypesList.push(new FilterClass("FTL", "FTL", "./Images/CellIcons/Container.png"));
                    this.ShipmentTypesList.push(new FilterClass("LTL", "LTL", "./Images/CellIcons/Package.png"));
                    break;
                }
            }
        }
        if (this.EntityPM.IsCopy) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ShipmentTypeId)) {
                this.ShipmentTypeName = null;
            }
            else {
                var item = this.ShipmentTypesList.filter(function (f) { return f.Code == _this.EntityPM.ShipmentTypeId; })[0];
                if (item) {
                    this.ShipmentTypeName = item.Name;
                }
            }
        }
    };
    Object.defineProperty(NewQuoteComponent.prototype, "OpportunityId", {
        get: function () { return this.EntityPM.OpportunityId; },
        set: function (value) {
            if (this.EntityPM.OpportunityId != value)
                this.EntityPM.OpportunityId = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "DirectionId", {
        get: function () { return this.EntityPM.DirectionId; },
        set: function (newValue) {
            if (this.EntityPM.DirectionId != newValue) {
                this.EntityPM.DirectionId = newValue;
                this.OnFiltersChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "TransportModeId", {
        get: function () { return this.EntityPM.TransportModeId; },
        set: function (newValue) {
            if (this.EntityPM.TransportModeId != newValue) {
                this.EntityPM.TransportModeId = newValue;
                this.FromPortId = null;
                this.ToPortId = null;
                this.MainCarriageCarrierId = null;
                this.ShipmentTypeId = null;
                this.IsLCLEntity = Tools_1.AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
                this.IsFCLEntity = !this.IsLCLEntity;
                //this.DelOrderDetails();
                this.OnFiltersChanged();
                this.BuildShipmentTypes();
                this.LoadAllowedAirline();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "ShipmentTypeId", {
        get: function () { return this.EntityPM.ShipmentTypeId; },
        set: function (newValue) {
            if (this.EntityPM.ShipmentTypeId != newValue) {
                this.EntityPM.ShipmentTypeId = newValue;
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.ShipmentTypeName = null;
                }
                else {
                    var item = this.ShipmentTypesList.filter(function (f) { return f.Code == newValue; })[0];
                    if (item) {
                        this.ShipmentTypeName = item.Name;
                    }
                }
                this.IsLCLEntity = Tools_1.AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
                this.IsFCLEntity = !this.IsLCLEntity;
                this.SetupOrderDetails();
                this.OnFiltersChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "IsShipperMyCustomer", {
        get: function () { return this.isShipperMyCustomer; },
        set: function (value) {
            if (this.isShipperMyCustomer != value) {
                this.isShipperMyCustomer = value;
                if (value) {
                    if (this.QuoteCustomerTypeCode == "SHI") {
                        this.CustomerId = this.ShipperId;
                    }
                }
                this.SetUIProperties_Shipper();
                this.SetUIProperties_Consignee();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "ShipperId", {
        get: function () { return this.EntityPM.ShipperId; },
        set: function (newValue) {
            if (this.EntityPM.ShipperId != newValue) {
                this.EntityPM.ShipperId = newValue;
                if (this.QuoteCustomerTypeCode == "SHI") {
                    this.CustomerId = newValue;
                }
                if (this.IsCopyFromQuote) {
                    if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                        this.ShipperCopyIsChecked = false;
                    }
                }
                this.SetUIProperties_Shipper();
                this.SetUIProperties_Customer();
                this.SetUIProperties_Domestic();
                this.GetShipperCardData();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "ShipperAddressList", {
        get: function () { return this.myShipperAddressList; },
        set: function (newValue) {
            this.myShipperAddressList = newValue;
            if (newValue == null) {
                this.EntityPM.ShipperMainAddressId = null;
            }
            else {
                this.EntityPM.ShipperMainAddressId = newValue.Id;
            }
            this.UpdatePickUpAddressFields();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "ShipperNote", {
        get: function () { return this.EntityPM.ShipperNote; },
        set: function (newValue) {
            if (this.EntityPM.ShipperNote != newValue) {
                this.EntityPM.ShipperNote = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "ShipperContactId", {
        get: function () { return this.EntityPM.ShipperContactId; },
        set: function (newValue) {
            if (this.EntityPM.ShipperContactId != newValue) {
                this.EntityPM.ShipperContactId = newValue;
                if (this.QuoteCustomerTypeCode == "SHI") {
                    this.EntityPM.CustomerContactId = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "ShipperReference1", {
        get: function () { return this.EntityPM.ShipperReference1; },
        set: function (newValue) {
            if (this.EntityPM.ShipperReference1 != newValue) {
                this.EntityPM.ShipperReference1 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "ShipperReference2", {
        get: function () { return this.EntityPM.ShipperReference2; },
        set: function (newValue) {
            if (this.EntityPM.ShipperReference2 != newValue) {
                this.EntityPM.ShipperReference2 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    NewQuoteComponent.prototype.GetShipperCardData = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.ShipperId)) {
            this.ShipperNote = null;
            this.EntityPM.ShipperName = null;
            this.ShipperAddressList = null;
            this.EntityPM.ShipperContactId = null;
            this.EntityPM.ShipperMainAddressId = null;
            this.EntityPM.ShipperPickAddressId = null;
        }
        else {
            this.myCardListService.getSingle(this.ShipperId).subscribe(function (myResult) {
                var myCardList = myResult.Result;
                if (myCardList) {
                    _this.EntityPM.ShipperName = myCardList.EnglishName;
                    _this.ShipperNote = myCardList.Notes;
                    _this.EntityPM.ShipperContactId = myCardList.PrimaryContactId;
                    _this.EntityPM.ShipperMainAddressId = myCardList.MainAddressId;
                    _this.EntityPM.ShipperPickAddressId = myCardList.PickAddressId;
                    _this.myAddressListService.getSingle(myCardList.MainAddressId).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            _this.ShipperAddressList = myResponse.Result;
                        }
                    });
                }
            });
        }
        this.SetUIProperties_Shipper();
    };
    Object.defineProperty(NewQuoteComponent.prototype, "IsConsigneeMyCustomer", {
        get: function () { return this.isConsigneeMyCustomer; },
        set: function (value) {
            if (this.isConsigneeMyCustomer != value) {
                this.isConsigneeMyCustomer = value;
                if (value) {
                    if (this.QuoteCustomerTypeCode == "CON") {
                        this.CustomerId = this.ConsigneeId;
                    }
                }
                this.SetUIProperties_Shipper();
                this.SetUIProperties_Consignee();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "ConsigneeId", {
        get: function () { return this.EntityPM.ConsigneeId; },
        set: function (newValue) {
            if (this.EntityPM.ConsigneeId != newValue) {
                this.EntityPM.ConsigneeId = newValue;
                if (this.QuoteCustomerTypeCode == "CON") {
                    this.CustomerId = newValue;
                }
                if (this.IsCopyFromQuote) {
                    if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                        this.ConsigneeCopyIsChecked = false;
                    }
                }
                this.SetUIProperties_Consignee();
                this.SetUIProperties_Customer();
                this.SetUIProperties_Domestic();
                this.GetConsigneeCardData();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "ConsigneeAddressList", {
        get: function () { return this.myConsigneeAddressList; },
        set: function (newValue) {
            this.myConsigneeAddressList = newValue;
            if (newValue == null) {
                this.EntityPM.ConsigneeMainAddressId = null;
            }
            else {
                this.EntityPM.ConsigneeMainAddressId = newValue.Id;
            }
            this.UpdateDeliveryAddressFields();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "ConsigneeNote", {
        get: function () { return this.EntityPM.ConsigneeNote; },
        set: function (newValue) {
            if (this.EntityPM.ConsigneeNote != newValue) {
                this.EntityPM.ConsigneeNote = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "ConsigneeContactId", {
        get: function () { return this.EntityPM.ConsigneeContactId; },
        set: function (newValue) {
            if (this.EntityPM.ConsigneeContactId != newValue) {
                this.EntityPM.ConsigneeContactId = newValue;
                if (this.QuoteCustomerTypeCode == "CON") {
                    this.EntityPM.CustomerContactId = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "ConsigneeReference1", {
        get: function () { return this.EntityPM.ConsigneeReference1; },
        set: function (newValue) {
            if (this.EntityPM.ConsigneeReference1 != newValue) {
                this.EntityPM.ConsigneeReference1 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "ConsigneeReference2", {
        get: function () { return this.EntityPM.ConsigneeReference2; },
        set: function (newValue) {
            if (this.EntityPM.ConsigneeReference2 != newValue) {
                this.EntityPM.ConsigneeReference2 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    NewQuoteComponent.prototype.GetConsigneeCardData = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.ConsigneeId)) {
            this.ConsigneeNote = null;
            this.EntityPM.ConsigneeName = null;
            this.ConsigneeAddressList = null;
            this.EntityPM.ConsigneeContactId = null;
            this.EntityPM.ConsigneeMainAddressId = null;
            this.EntityPM.ConsigneePickAddressId = null;
        }
        else {
            this.myCardListService.getSingle(this.ConsigneeId).subscribe(function (myResult) {
                var myCardList = myResult.Result;
                if (myCardList) {
                    _this.EntityPM.ConsigneeName = myCardList.EnglishName;
                    _this.ConsigneeNote = myCardList.Notes;
                    _this.EntityPM.ConsigneeContactId = myCardList.PrimaryContactId;
                    _this.EntityPM.ConsigneeMainAddressId = myCardList.MainAddressId;
                    _this.EntityPM.ConsigneePickAddressId = myCardList.PickAddressId;
                    _this.myAddressListService.getSingle(myCardList.MainAddressId).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            _this.ConsigneeAddressList = myResponse.Result;
                        }
                    });
                }
            });
        }
        this.SetUIProperties_Consignee();
    };
    NewQuoteComponent.prototype.ComputeCustomerDependency = function () {
        switch (this.QuoteCustomerTypeCode) {
            case "SHI":
            case "CON":
                {
                    this.CustomerDependencyProperty1 = "CS,PO";
                    this.CustomerDependencyProperty1IsList = true;
                    if (SessionLocator_1.SessionLocator.TenantPM.AllowAgentInCustomersLOV) {
                        this.CustomerDependencyProperty1 = "CS,PO,AG";
                        this.CustomerDependencyProperty1IsList = true;
                    }
                    break;
                }
            case "AGT":
                {
                    this.CustomerDependencyProperty1 = "AG";
                    this.CustomerDependencyProperty1IsList = false;
                    break;
                }
            case "NOT":
                {
                    this.CustomerDependencyProperty1 = "AG,AL,CG,CS,SG,SL,TR,VD,WH";
                    this.CustomerDependencyProperty1IsList = true;
                    break;
                }
            case "OTH": {
                this.CustomerDependencyProperty1 = "CS";
                this.CustomerDependencyProperty1IsList = false;
                break;
            }
        }
    };
    Object.defineProperty(NewQuoteComponent.prototype, "QuoteCustomerTypeCode", {
        get: function () { return this.EntityPM.QuoteCustomerTypeCode; },
        set: function (newValue) {
            if (this.EntityPM.QuoteCustomerTypeCode != newValue) {
                this.EntityPM.QuoteCustomerTypeCode = newValue;
                this.CustomerId = null;
                this.SetCustomer(newValue);
                this.SetCustomerRequired();
                this.ComputeCustomerDependency();
                this.SetUIProperties_Customer();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "CustomerId", {
        get: function () { return this.EntityPM.CustomerId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.CustomerId != newValue) {
                this.EntityPM.CustomerId = newValue;
                this.SetCustomerRequired();
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.CustomerContactId = null;
                    this.EntityPM.CustomerName = null;
                    this.EntityPM.CustomerNote = null;
                    this.CustomerAddressId = null;
                    this.customerSalesmanId = null;
                    this.customerSalesmanBusinessUnitId = null;
                }
                else {
                    this.myCardListService.getSingle(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var myCardList = myResponse.Result;
                            if (myCardList) {
                                _this.CustomerContactId = myCardList.PrimaryContactId;
                                _this.EntityPM.CustomerName = myCardList.EnglishName;
                                _this.EntityPM.CustomerNote = myCardList.Notes;
                                _this.CustomerAddressId = myCardList.MainAddressId;
                                _this.customerSalesmanId = myCardList.SalesmanUserId;
                                _this.customerSalesmanBusinessUnitId = myCardList.SalesmanBusinessUnitId;
                                _this.SetCustomePartner();
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "CustomerAddressId", {
        get: function () { return this.customerAddressId; },
        set: function (newValue) {
            if (this.customerAddressId != newValue) {
                this.customerAddressId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "CustomerContactId", {
        get: function () { return this.EntityPM.CustomerContactId; },
        set: function (newValue) {
            if (this.EntityPM.CustomerContactId != newValue) {
                this.EntityPM.CustomerContactId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    NewQuoteComponent.prototype.SetCustomePartner = function () {
        //if (!this.IsCopyFromQuote) {
        switch (this.QuoteCustomerTypeCode) {
            case "SHI": {
                this.ShipperId = this.CustomerId;
                this.EntityPM.ShipperMainAddressId = this.CustomerAddressId;
                this.EntityPM.ShipperContactId = this.CustomerContactId;
                break;
            }
            case "CON": {
                this.ConsigneeId = this.CustomerId;
                this.EntityPM.ConsigneeMainAddressId = this.CustomerAddressId;
                this.EntityPM.ConsigneeContactId = this.CustomerContactId;
                break;
            }
            case "AGT": {
                this.EntityPM.AgentId = this.CustomerId;
                this.EntityPM.AgentAddressId = this.CustomerAddressId;
                this.EntityPM.AgentContactId = this.CustomerContactId;
                break;
            }
            case "NOT": {
                this.EntityPM.NotifyId = this.CustomerId;
                this.EntityPM.NotifyAddressId = this.CustomerAddressId;
                this.EntityPM.NotifyContactId = this.CustomerContactId;
                break;
            }
            case "OTH": {
                break;
            }
        }
        //}
    };
    NewQuoteComponent.prototype.SetCustomerRequired = function () {
        var isRequired = false;
        if (Tools_1.AppTool.IsNullOrEmpty(this.CustomerId) || Tools_1.AppTool.IsNullOrEmpty(this.QuoteCustomerTypeCode)) {
            isRequired = true;
        }
        this.IsCustomerRequired = isRequired;
    };
    NewQuoteComponent.prototype.SetQuoteType = function (newValue) {
        if (newValue == "A") {
            this.EntityPM.QuoteTypeCode = "A";
            this.IsAdhoc = true;
        }
        else {
            this.EntityPM.QuoteTypeCode = "P";
            this.IsRoutingRates = true;
            this.Volume = null;
            this.GrossWeight = null;
            this.EntityPM.VolumetricWeight = null;
            this.ChargeableWeight = null;
        }
        this.SetUIProperties_Containers();
        this.SetUIProperties_Dimentions();
    };
    Object.defineProperty(NewQuoteComponent.prototype, "QuoteTypeCode", {
        get: function () { return this.EntityPM.QuoteTypeCode; },
        set: function (newValue) {
            if (this.EntityPM.QuoteTypeCode != newValue) {
                this.EntityPM.QuoteTypeCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "IsAdhoc", {
        get: function () { return (this.EntityPM.QuoteTypeCode == "A" ? true : false); },
        set: function (newValue) {
            if (newValue) {
                this.EntityPM.QuoteTypeCode = "A";
            }
            else {
                this.EntityPM.QuoteTypeCode = "P";
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "IsRoutingRates", {
        get: function () { return (this.EntityPM.QuoteTypeCode == "P" ? true : false); },
        set: function (newValue) {
            if (newValue) {
                this.EntityPM.QuoteTypeCode = "P";
            }
            else {
                this.EntityPM.QuoteTypeCode = "A";
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "IncotermId", {
        get: function () { return this.EntityPM.IncotermId; },
        set: function (newValue) {
            if (this.EntityPM.IncotermId != newValue) {
                this.EntityPM.IncotermId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "MoveTypeId", {
        get: function () { return this.EntityPM.MoveTypeId; },
        set: function (newValue) {
            if (this.EntityPM.MoveTypeId != newValue) {
                this.EntityPM.MoveTypeId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "ExpirationDays", {
        get: function () { return this.EntityPM.ExpirationDays; },
        set: function (newValue) {
            if (this.EntityPM.ExpirationDays != newValue) {
                this.EntityPM.ExpirationDays = newValue;
                if (newValue == null) {
                    this.EntityPM.ExpirationDate = null;
                }
                else {
                    var date = Tools_1.DateTool.GetDateByDay(newValue);
                    if (this.ExpirationDate == null) {
                        this.EntityPM.ExpirationDate = date;
                    }
                    else {
                        if (this.ExpirationDate.valueOf() != date.valueOf()) {
                            this.EntityPM.ExpirationDate = date;
                        }
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "ExpirationDate", {
        get: function () { return this.EntityPM.ExpirationDate; },
        set: function (newValue) {
            if (this.EntityPM.ExpirationDate != newValue) {
                this.EntityPM.ExpirationDate = newValue;
                if (newValue == null) {
                    this.EntityPM.ExpirationDays = null;
                }
                else {
                    var todayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
                    var days = this.GetDaysBetweenDates(newValue, todayDate);
                    if (this.ExpirationDays != days) {
                        this.EntityPM.ExpirationDays = days;
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "IsAutomaticallyClosed", {
        get: function () { return this.EntityPM.IsAutomaticallyClosed; },
        set: function (newValue) {
            if (this.EntityPM.IsAutomaticallyClosed != newValue) {
                this.EntityPM.IsAutomaticallyClosed = newValue;
                if (newValue) {
                    var todayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
                    this.EntityPM.AutomaticallyCloseDays = 30;
                    this.EntityPM.AutomaticallyCloseDate = Tools_1.DateTool.AddDays(todayDate, 30);
                }
                else {
                    this.EntityPM.AutomaticallyCloseDays = null;
                    this.EntityPM.AutomaticallyCloseDate = null;
                    this.EntityPM.QuoteClosingReasonCode = null;
                }
                this.SetUIProperties_AutomaticallyClosed();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "AutomaticallyCloseDays", {
        get: function () { return this.EntityPM.AutomaticallyCloseDays; },
        set: function (newValue) {
            if (this.EntityPM.AutomaticallyCloseDays != newValue) {
                this.EntityPM.AutomaticallyCloseDays = newValue;
                if (newValue == null) {
                    this.EntityPM.AutomaticallyCloseDate = null;
                }
                else {
                    var date = Tools_1.DateTool.GetDateByDay(newValue);
                    if (this.AutomaticallyCloseDate == null) {
                        this.EntityPM.AutomaticallyCloseDate = date;
                    }
                    else {
                        if (this.AutomaticallyCloseDate.valueOf() != date.valueOf()) {
                            this.EntityPM.AutomaticallyCloseDate = date;
                        }
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "AutomaticallyCloseDate", {
        get: function () { return this.EntityPM.AutomaticallyCloseDate; },
        set: function (newValue) {
            if (this.EntityPM.AutomaticallyCloseDate != newValue) {
                this.EntityPM.AutomaticallyCloseDate = newValue;
                if (newValue == null) {
                    this.EntityPM.AutomaticallyCloseDays = null;
                }
                else {
                    var todayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
                    var days = this.GetDaysBetweenDates(newValue, todayDate);
                    if (this.AutomaticallyCloseDays != days) {
                        this.EntityPM.AutomaticallyCloseDays = days;
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    NewQuoteComponent.prototype.GetDaysBetweenDates = function (date1, date2) {
        var myResult = 0;
        if (date1 != null && date2 != null) {
            if (date1 != undefined && date2 != undefined) {
                date1 = this.TruncateTime(date1);
                date2 = this.TruncateTime(date2);
                var d1 = new Date(date1.toString());
                var d2 = new Date(date2.toString());
                var timeDiff = d1.getTime() - d2.getTime();
                var Daysdiff = Math.ceil(timeDiff / (1000 * 3600 * 24));
                myResult = Daysdiff;
            }
        }
        return myResult;
    };
    NewQuoteComponent.prototype.TruncateTime = function (date) {
        var myResult = null;
        if (date != null) {
            var myResult = new Date(date.toString());
            myResult.setUTCHours(0);
            myResult.setUTCMinutes(0);
            myResult.setUTCSeconds(0);
        }
        return myResult;
    };
    Object.defineProperty(NewQuoteComponent.prototype, "IncludePickUp", {
        // Pick up
        get: function () { return this.EntityPM.IncludePickUp; },
        set: function (newValue) {
            if (this.EntityPM.IncludePickUp != newValue) {
                this.EntityPM.IncludePickUp = newValue;
                this.UpdatePickUpAddressFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "PickUpAddressId", {
        get: function () { return this.EntityPM.PickUpAddressId; },
        set: function (newValue) {
            if (this.EntityPM.PickUpAddressId != newValue) {
                this.EntityPM.PickUpAddressId = newValue;
                if (!Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.FromAddressCity = null;
                    this.FromAddressZipCode = null;
                    this.FromAddressCountryId = null;
                }
                this.LoadPickupAddress();
                this.SetUIProperties_PickupFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "FromAddressZipCode", {
        get: function () { return this.EntityPM.FromAddressZipCode; },
        set: function (newValue) {
            if (this.EntityPM.FromAddressZipCode != newValue) {
                this.EntityPM.FromAddressZipCode = newValue;
                this.SetUIProperties_PickupFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "FromAddressCity", {
        get: function () { return this.EntityPM.FromAddressCity; },
        set: function (newValue) {
            if (this.EntityPM.FromAddressCity != newValue) {
                this.EntityPM.FromAddressCity = newValue;
                this.SetUIProperties_PickupFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "FromAddressCountryId", {
        get: function () { return this.EntityPM.FromAddressCountryId; },
        set: function (newValue) {
            if (this.EntityPM.FromAddressCountryId != newValue) {
                this.EntityPM.FromAddressCountryId = newValue;
                this.SetUIProperties_PickupFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    NewQuoteComponent.prototype.LoadPickupAddress = function () {
        var _this = this;
        if (this.PickUpAddressId == null) {
            this.PickupAddressList = null;
        }
        else {
            this.myAddressListService.getSingle(this.PickUpAddressId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    _this.PickupAddressList = myResponse.Result;
                }
            });
        }
    };
    NewQuoteComponent.prototype.UpdatePickUpAddressFields = function () {
        if (!this.IncludePickUp) {
            this.EntityPM.PickUpAddressId = null;
            this.EntityPM.FromAddressCity = null;
            this.EntityPM.FromAddressZipCode = null;
            this.EntityPM.FromAddressCountryId = null;
            this.PickupAddressList = null;
        }
        else if (this.IsCopyFromQuote && this.CopyPickUpIsChecked) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipperId)) {
                this.PickUpAddressId = this.sourceEntityPM.PickUpAddressId;
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.PickUpAddressId)) {
                this.EntityPM.FromAddressCity = this.sourceEntityPM.FromAddressCity;
                this.EntityPM.FromAddressZipCode = this.sourceEntityPM.FromAddressZipCode;
                this.EntityPM.FromAddressCountryId = this.sourceEntityPM.FromAddressCountryId;
            }
        }
        else {
            this.PickupAddressList = null;
            this.EntityPM.PickUpAddressId = null;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ShipperPickAddressId)) {
                this.EntityPM.PickUpAddressId = this.EntityPM.ShipperPickAddressId;
            }
            else {
                this.EntityPM.PickUpAddressId = this.EntityPM.ShipperMainAddressId;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.PickUpAddressId)) {
                this.EntityPM.FromAddressCity = null;
                this.EntityPM.FromAddressZipCode = null;
                this.EntityPM.FromAddressCountryId = null;
            }
            this.LoadPickupAddress();
        }
        this.SetUIProperties_PickupFields();
    };
    NewQuoteComponent.prototype.SetUIProperties_PickupFields = function () {
        var isCityRequired = false;
        var isCountryRequired = false;
        if (this.IncludePickUp) {
            var validateFields = false;
            if (Tools_1.AppTool.IsNullOrEmpty(this.ShipperId)) {
                validateFields = true;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipperId) && Tools_1.AppTool.IsNullOrEmpty(this.PickUpAddressId)) {
                validateFields = true;
            }
            if (validateFields) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.FromAddressCity) && Tools_1.AppTool.IsNullOrEmpty(this.FromAddressZipCode)) {
                    isCityRequired = true;
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.FromAddressCountryId)) {
                    isCountryRequired = true;
                }
            }
        }
        this.UIProperties.SetRequired("FromAddressCity", this.ObjectTableName, isCityRequired);
        this.UIProperties.SetRequired("FromAddressCountryId", this.ObjectTableName, isCountryRequired);
    };
    Object.defineProperty(NewQuoteComponent.prototype, "IsEditPickUpAddressEnabled", {
        get: function () {
            var myResult = false;
            if (this.IncludePickUp) {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipperId) && !Tools_1.AppTool.IsNullOrEmpty(this.PickUpAddressId)) {
                    myResult = true;
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "IsAddPickUpAddressEnabled", {
        get: function () {
            var myResult = false;
            if (this.IncludePickUp) {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipperId)) {
                    myResult = true;
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "FromPortId", {
        get: function () { return this.EntityPM.FromPortId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.FromPortId != value) {
                this.EntityPM.FromPortId = value;
                this.SetUIProperties_Domestic();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.FromPortList = null;
                }
                else {
                    this.myPortListService.getSingle(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            _this.FromPortList = myResponse.Result;
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "ToPortId", {
        get: function () { return this.EntityPM.ToPortId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.ToPortId != value) {
                this.EntityPM.ToPortId = value;
                this.SetUIProperties_Domestic();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.ToPortList = null;
                }
                else {
                    this.myPortListService.getSingle(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            _this.ToPortList = myResponse.Result;
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "MainCarriageCarrierId", {
        get: function () { return this.EntityPM.MainCarriageCarrierId; },
        set: function (newValue) {
            if (this.EntityPM.MainCarriageCarrierId != newValue) {
                this.EntityPM.MainCarriageCarrierId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "IncludeDelivery", {
        // Delivery
        get: function () { return this.EntityPM.IncludeDelivery; },
        set: function (newValue) {
            if (this.EntityPM.IncludeDelivery != newValue) {
                this.EntityPM.IncludeDelivery = newValue;
                this.UpdateDeliveryAddressFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "DeliveryAddressId", {
        get: function () { return this.EntityPM.DeliveryAddressId; },
        set: function (newValue) {
            if (this.EntityPM.DeliveryAddressId != newValue) {
                this.EntityPM.DeliveryAddressId = newValue;
                if (!Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.ToAddressCity = null;
                    this.ToAddressZipCode = null;
                    this.ToAddressCountryId = null;
                }
                this.LoadDeliveryAddress();
                this.SetUIProperties_DeliveryFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "ToAddressZipCode", {
        get: function () { return this.EntityPM.ToAddressZipCode; },
        set: function (newValue) {
            if (this.EntityPM.ToAddressZipCode != newValue) {
                this.EntityPM.ToAddressZipCode = newValue;
                this.SetUIProperties_DeliveryFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "ToAddressCity", {
        get: function () { return this.EntityPM.ToAddressCity; },
        set: function (newValue) {
            if (this.EntityPM.ToAddressCity != newValue) {
                this.EntityPM.ToAddressCity = newValue;
                this.SetUIProperties_DeliveryFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "ToAddressCountryId", {
        get: function () { return this.EntityPM.ToAddressCountryId; },
        set: function (newValue) {
            if (this.EntityPM.ToAddressCountryId != newValue) {
                this.EntityPM.ToAddressCountryId = newValue;
                this.SetUIProperties_DeliveryFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    NewQuoteComponent.prototype.LoadDeliveryAddress = function () {
        var _this = this;
        if (this.DeliveryAddressId == null) {
            this.DeliveryAddressList = null;
        }
        else {
            this.myAddressListService.getSingle(this.DeliveryAddressId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    _this.DeliveryAddressList = myResponse.Result;
                }
            });
        }
    };
    NewQuoteComponent.prototype.UpdateDeliveryAddressFields = function () {
        if (!this.IncludeDelivery) {
            this.EntityPM.DeliveryAddressId = null;
            this.EntityPM.ToAddressCity = null;
            this.EntityPM.ToAddressZipCode = null;
            this.EntityPM.ToAddressCountryId = null;
            this.DeliveryAddressList = null;
        }
        else if (this.IsCopyFromQuote && this.CopyDeliveryIsChecked) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipperId)) {
                this.DeliveryAddressId = this.sourceEntityPM.DeliveryAddressId;
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.DeliveryAddressId)) {
                this.EntityPM.ToAddressCity = this.sourceEntityPM.ToAddressCity;
                this.EntityPM.ToAddressZipCode = this.sourceEntityPM.ToAddressZipCode;
                this.EntityPM.ToAddressCountryId = this.sourceEntityPM.ToAddressCountryId;
            }
        }
        else {
            this.DeliveryAddressList = null;
            this.EntityPM.DeliveryAddressId = null;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ConsigneePickAddressId)) {
                this.EntityPM.DeliveryAddressId = this.EntityPM.ConsigneePickAddressId;
            }
            else {
                this.EntityPM.DeliveryAddressId = this.EntityPM.ConsigneeMainAddressId;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.DeliveryAddressId)) {
                this.EntityPM.ToAddressCity = null;
                this.EntityPM.ToAddressZipCode = null;
                this.EntityPM.ToAddressCountryId = null;
            }
            this.LoadDeliveryAddress();
        }
        this.SetUIProperties_DeliveryFields();
    };
    NewQuoteComponent.prototype.SetUIProperties_DeliveryFields = function () {
        var isCityRequired = false;
        var isCountryRequired = false;
        if (this.IncludeDelivery) {
            var validateFields = false;
            if (Tools_1.AppTool.IsNullOrEmpty(this.ShipperId)) {
                validateFields = true;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipperId) && Tools_1.AppTool.IsNullOrEmpty(this.DeliveryAddressId)) {
                validateFields = true;
            }
            if (validateFields) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.ToAddressCity) && Tools_1.AppTool.IsNullOrEmpty(this.ToAddressZipCode)) {
                    isCityRequired = true;
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.ToAddressCountryId)) {
                    isCountryRequired = true;
                }
            }
        }
        this.UIProperties.SetRequired("ToAddressCity", this.ObjectTableName, isCityRequired);
        this.UIProperties.SetRequired("ToAddressCountryId", this.ObjectTableName, isCountryRequired);
    };
    Object.defineProperty(NewQuoteComponent.prototype, "IsEditDeliveryAddressEnabled", {
        get: function () {
            var myResult = false;
            if (this.IncludeDelivery) {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.ConsigneeId) && !Tools_1.AppTool.IsNullOrEmpty(this.DeliveryAddressId)) {
                    myResult = true;
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "IsAddDeliveryAddressEnabled", {
        get: function () {
            var myResult = false;
            if (this.IncludeDelivery) {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.ConsigneeId)) {
                    myResult = true;
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "PackageType1Quantity", {
        // Expected Order Details  
        get: function () { return this.EntityPM.PackageType1Quantity; },
        set: function (newValue) {
            if (this.EntityPM.PackageType1Quantity != newValue) {
                this.EntityPM.PackageType1Quantity = newValue;
                this.SetUIProperties_Containers();
                this.EntityPM.TEU = QuoteUtilities_1.QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "PackageType2Quantity", {
        get: function () { return this.EntityPM.PackageType2Quantity; },
        set: function (newValue) {
            if (this.EntityPM.PackageType2Quantity != newValue) {
                this.EntityPM.PackageType2Quantity = newValue;
                this.SetUIProperties_Containers();
                this.EntityPM.TEU = QuoteUtilities_1.QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "PackageType3Quantity", {
        get: function () { return this.EntityPM.PackageType3Quantity; },
        set: function (newValue) {
            if (this.EntityPM.PackageType3Quantity != newValue) {
                this.EntityPM.PackageType3Quantity = newValue;
                this.SetUIProperties_Containers();
                this.EntityPM.TEU = QuoteUtilities_1.QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "PackageType4Quantity", {
        get: function () { return this.EntityPM.PackageType4Quantity; },
        set: function (newValue) {
            if (this.EntityPM.PackageType4Quantity != newValue) {
                this.EntityPM.PackageType4Quantity = newValue;
                this.SetUIProperties_Containers();
                this.EntityPM.TEU = QuoteUtilities_1.QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "PackageType5Quantity", {
        get: function () { return this.EntityPM.PackageType5Quantity; },
        set: function (newValue) {
            if (this.EntityPM.PackageType5Quantity != newValue) {
                this.EntityPM.PackageType5Quantity = newValue;
                this.SetUIProperties_Containers();
                this.EntityPM.TEU = QuoteUtilities_1.QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "PackageType1Id", {
        get: function () { return this.EntityPM.PackageType1Id; },
        set: function (newValue) {
            if (this.EntityPM.PackageType1Id != newValue) {
                this.EntityPM.PackageType1Id = newValue;
                this.EntityPM.TEU = QuoteUtilities_1.QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "PackageType2Id", {
        get: function () { return this.EntityPM.PackageType2Id; },
        set: function (newValue) {
            if (this.EntityPM.PackageType2Id != newValue) {
                this.EntityPM.PackageType2Id = newValue;
                this.EntityPM.TEU = QuoteUtilities_1.QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "PackageType3Id", {
        get: function () { return this.EntityPM.PackageType3Id; },
        set: function (newValue) {
            if (this.EntityPM.PackageType3Id != newValue) {
                this.EntityPM.PackageType3Id = newValue;
                this.EntityPM.TEU = QuoteUtilities_1.QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "PackageType4Id", {
        get: function () { return this.EntityPM.PackageType4Id; },
        set: function (newValue) {
            if (this.EntityPM.PackageType4Id != newValue) {
                this.EntityPM.PackageType4Id = newValue;
                this.EntityPM.TEU = QuoteUtilities_1.QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "PackageType5Id", {
        get: function () { return this.EntityPM.PackageType5Id; },
        set: function (newValue) {
            if (this.EntityPM.PackageType5Id != newValue) {
                this.EntityPM.PackageType5Id = newValue;
                this.EntityPM.TEU = QuoteUtilities_1.QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "GrossWeight", {
        get: function () { return this.EntityPM.GrossWeight; },
        set: function (newValue) {
            if (this.EntityPM.GrossWeight != newValue) {
                this.EntityPM.GrossWeight = Tools_1.AppTool.Round(newValue, 2);
                this.ComputeChargeableWeight();
                //this.EntityPM.ChargeableWeight = QuoteUtilities.ComputeChargeableWeight(this.EntityPM);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "Volume", {
        get: function () { return this.EntityPM.Volume; },
        set: function (newValue) {
            if (this.EntityPM.Volume != newValue) {
                this.EntityPM.Volume = Tools_1.AppTool.Round(newValue, 2);
                this.ComputeVolumetricWeight();
                //this.EntityPM.VolumetricWeight = QuoteUtilities.ComputeVolumetricWeight(this.EntityPM);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "VolumetricWeight", {
        get: function () { return this.EntityPM.VolumetricWeight; },
        set: function (value) {
            if (this.EntityPM.VolumetricWeight != value) {
                this.EntityPM.VolumetricWeight = Tools_1.AppTool.Round(value, 2);
                this.ComputeChargeableWeight();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "ChargeableWeight", {
        get: function () { return Tools_1.AppTool.IsNullOrZero(this.EntityPM.ChargeableWeight) ? null : this.EntityPM.ChargeableWeight; },
        set: function (newValue) {
            if (this.EntityPM.ChargeableWeight != newValue) {
                var result = Tools_1.AppTool.Round(newValue, 2);
                this.EntityPM.ChargeableWeight = result;
                if (this.GrossWeight == null && this.EntityPM.VolumetricWeight == null) {
                    this.EntityPM.VolumetricWeight = result;
                    this.EntityPM.GrossWeight = Tools_1.AppTool.GetWeightFromWeight(this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.GrossWeightUnitCode, result);
                    this.EntityPM.Volume = Tools_1.AppTool.GetVolumeFromWeight(this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.VolumeUnitCode, this.EntityPM.VolumetricWeight, this.EntityPM.Ratio);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    NewQuoteComponent.prototype.ComputeChargeableWeight = function () {
        this.ChargeableWeight = Tools_1.AppTool.CalculateChargeableWeight(this.GrossWeight, this.EntityPM.VolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
    };
    NewQuoteComponent.prototype.ComputeVolumetricWeight = function () {
        var myResult = null;
        if (this.Volume != null) {
            myResult = Tools_1.AppTool.GetWeightFromVolume(this.EntityPM.VolumeUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.Volume, this.EntityPM.Ratio);
        }
        else if (this.GrossWeight != null) {
            myResult = Tools_1.AppTool.GetWeightFromWeight(this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.GrossWeight);
        }
        this.VolumetricWeight = myResult;
    };
    Object.defineProperty(NewQuoteComponent.prototype, "NumberOfPackages", {
        get: function () { return this.EntityPM.NumberOfPackages; },
        set: function (newValue) {
            if (this.EntityPM.NumberOfPackages != newValue) {
                this.EntityPM.NumberOfPackages = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "IsDangerous", {
        get: function () { return this.EntityPM.IsDangerous; },
        set: function (newValue) {
            if (this.EntityPM.IsDangerous != newValue) {
                this.EntityPM.IsDangerous = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "DescriptionOfGoods", {
        get: function () { return this.EntityPM.DescriptionOfGoods; },
        set: function (newValue) {
            if (this.EntityPM.DescriptionOfGoods != newValue) {
                this.EntityPM.DescriptionOfGoods = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    NewQuoteComponent.prototype.FillDimensionsClicked = function () {
        var _this = this;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.S.NewQuote.FillDimensions");
        logitudeWindow.Width = 850;
        logitudeWindow.WindowArgs = this.EntityPM;
        logitudeWindow.Show('./Quote/Components/NewEntity/QuoteDimensionsComponent');
        logitudeWindow.WindowClosed.subscribe(function (s) {
            if (s) {
                _this.SetUIProperties_Dimentions();
            }
        });
    };
    NewQuoteComponent.prototype.BuildAdditionalFields = function () {
        this.RunComponent();
    };
    NewQuoteComponent.prototype.RunComponent = function () {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }
        else {
            this.RunComponentTimer();
        }
    };
    NewQuoteComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    NewQuoteComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(function (cmpRef) {
            _this.GeneratedComponent = cmpRef.instance;
            cmpRef.instance.LoadCompleted.subscribe(function (s) {
                _this.SetUIProperties_GeneratedComponent();
            });
            var screenCode = "NewQuote";
            cmpRef.instance.LabelWidth = 110;
            cmpRef.instance.Run(_this.EntityPM, _this.ObjectTableName, screenCode);
        });
    };
    NewQuoteComponent.prototype.SetUIProperties_GeneratedComponent = function () {
        if (this.GeneratedComponent) {
            this.GeneratedComponent.SetEnabled(this.IsScreenEnabled);
        }
    };
    // General Methods
    NewQuoteComponent.prototype.SetCustomer = function (myCode) {
        if (myCode == 'SHI') {
            this.IsShipperMyCustomer = true;
            this.IsConsigneeMyCustomer = false;
        }
        else if (myCode == 'CON') {
            this.IsShipperMyCustomer = false;
            this.IsConsigneeMyCustomer = true;
        }
        else {
            this.IsShipperMyCustomer = false;
            this.IsConsigneeMyCustomer = false;
        }
    };
    NewQuoteComponent.prototype.SelectCityClicked = function (selectCityTypeCode) {
        var _this = this;
        var mySourceCountryId = selectCityTypeCode == "P" ? this.FromAddressCountryId : this.ToAddressCountryId;
        var args = new Args_1.CitySelectionArgs(mySourceCountryId);
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Select City";
        logWindow.WindowArgs = args;
        logWindow.Show("./CommonModules/CommonOthers/Components/CitySelection/CitySelectionComponent");
        logWindow.WindowClosed.subscribe(function ($event) {
            if (args.IsCitySelected) {
                if (selectCityTypeCode == "P") {
                    _this.FromAddressCity = args.CityName;
                    _this.FromAddressCountryId = args.CountryId;
                }
                else {
                    _this.ToAddressCity = args.CityName;
                    _this.ToAddressCountryId = args.CountryId;
                }
            }
        });
    };
    NewQuoteComponent.prototype.EditAddressClicked = function (myAddressCode) {
        var _this = this;
        var myAddressId = null;
        var myPartnerId = null;
        switch (myAddressCode) {
            case "P": {
                if (this.IncludePickUp) {
                    myAddressId = this.PickUpAddressId;
                    myPartnerId = this.ShipperId;
                }
                break;
            }
            case "D": {
                if (this.IncludeDelivery) {
                    myAddressId = this.DeliveryAddressId;
                    myPartnerId = this.ConsigneeId;
                }
                break;
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(myAddressId)) {
            var logeWindow = new LogitudeWindow_1.LogitudeWindow();
            logeWindow.Width = 630;
            logeWindow.Height = 430;
            logeWindow.Title = "Edit Address";
            logeWindow.WindowArgs = { EntityId: myAddressId, CardId: myPartnerId };
            logeWindow.Show("./Quote/Components/NewEntity/NewQuoteAddEditAddressComponent");
            logeWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    switch (myAddressCode) {
                        case "P": {
                            if (_this.PickUpAddressId == _this.EntityPM.ShipperMainAddressId) {
                                _this.myAddressListService.getSingle(_this.PickUpAddressId).subscribe(function (myResponse) {
                                    if (!myResponse.HasError) {
                                        _this.ShipperAddressList = myResponse.Result;
                                    }
                                });
                            }
                            else {
                                _this.PickUpAddressId = null;
                                _this.PickUpAddressId = myAddressId;
                            }
                            break;
                        }
                        case "D": {
                            if (_this.DeliveryAddressId == _this.EntityPM.ConsigneeMainAddressId) {
                                _this.myAddressListService.getSingle(_this.DeliveryAddressId).subscribe(function (myResponse) {
                                    if (!myResponse.HasError) {
                                        _this.ConsigneeAddressList = myResponse.Result;
                                    }
                                });
                            }
                            else {
                                _this.DeliveryAddressId = null;
                                _this.DeliveryAddressId = myAddressId;
                            }
                            break;
                        }
                    }
                }
            });
        }
    };
    NewQuoteComponent.prototype.AddAddressClicked = function (myAddressCode) {
        var _this = this;
        var entityPM = null;
        var myPartnerId = null;
        switch (myAddressCode) {
            case "P": {
                if (this.IncludePickUp) {
                    myPartnerId = this.ShipperId;
                    if (!Tools_1.AppTool.IsNullOrEmpty(myPartnerId)) {
                        entityPM = new AddressPM_1.AddressPM();
                        entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                        entityPM.AddressTypeId = "O";
                        entityPM.CardId = this.ShipperId;
                    }
                }
                break;
            }
            case "D": {
                if (this.IncludeDelivery) {
                    myPartnerId = this.ConsigneeId;
                    if (!Tools_1.AppTool.IsNullOrEmpty(myPartnerId)) {
                        entityPM = new AddressPM_1.AddressPM();
                        entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                        entityPM.AddressTypeId = "O";
                        entityPM.CardId = this.ConsigneeId;
                    }
                }
                break;
            }
        }
        if (entityPM != null) {
            var logeWindow = new LogitudeWindow_1.LogitudeWindow();
            logeWindow.Width = 630;
            logeWindow.Height = 430;
            logeWindow.Title = "Add Address";
            logeWindow.WindowArgs = { EntityPM: entityPM, CardId: myPartnerId };
            logeWindow.Show("./Quote/Components/NewEntity/NewQuoteAddEditAddressComponent");
            logeWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    switch (myAddressCode) {
                        case "P": {
                            _this.PickUpAddressId = null;
                            _this.PickUpAddressId = entityPM.Id;
                            break;
                        }
                        case "D": {
                            _this.DeliveryAddressId = null;
                            _this.DeliveryAddressId = entityPM.Id;
                            break;
                        }
                    }
                }
            });
        }
    };
    NewQuoteComponent.prototype.SetLabels = function () {
        switch (this.TransportModeId) {
            case "A": {
                this.FromLabel = "Quote.S.NewQuote.Gateway";
                this.ToLabel = "Quote.S.NewQuote.Destination";
                this.CarrierLabel = "Quote.S.NewQuote.Airline";
                this.CarrierDependencyProperty1 = "AL";
                break;
            }
            case "O": {
                this.FromLabel = "Quote.S.NewQuote.LoadingPort";
                this.ToLabel = "Quote.S.NewQuote.DischargePort";
                this.CarrierLabel = "Quote.S.NewQuote.Shippingline";
                this.CarrierDependencyProperty1 = "SL";
                break;
            }
            case "I": {
                this.FromLabel = "Quote.S.NewQuote.From";
                this.ToLabel = "Quote.S.NewQuote.To";
                this.CarrierLabel = "Quote.S.NewQuote.Trucker";
                this.CarrierDependencyProperty1 = "TR";
                break;
            }
            default: {
                this.FromLabel = "Quote.S.NewQuote.From";
                this.ToLabel = "Quote.S.NewQuote.To";
                this.CarrierLabel = "Quote.S.NewQuote.Carrier";
                break;
            }
        }
        this.VolumeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.F.Volume").replace("%VolumeCode", this.EntityPM.VolumeUnitCode);
        this.GrossWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.F.GrossWeight").replace("%GrossWeightCode", this.EntityPM.GrossWeightUnitCode);
        if (this.TransportModeId == "A") {
            this.ChargeableWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.F.ChargeableWeight").replace("%ChargWeightCode", this.EntityPM.ChargeableWeightUnitCode);
        }
        else {
            this.ChargeableWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.F.WtMsr.Short").replace("%ChargWeightCode", this.EntityPM.ChargeableWeightUnitCode);
        }
    };
    NewQuoteComponent.prototype.SetUnits = function () {
        var myDimensionsUnitCode = SessionLocator_1.SessionLocator.TenantPM.DimensionsUnitCode;
        var myVolumeUnitCode = SessionLocator_1.SessionLocator.TenantPM.VolumeUnitCode;
        var myGrossWeightUnitCode = SessionLocator_1.SessionLocator.TenantPM.GrossWeightUnitCode;
        var myChargeableWeightUnitCode = Tools_1.AppTool.GetChargeableWeightUnitCode(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
        if (this.EntityPM.DirectionId == "D") {
            if (!Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.TenantPM.CountryCode)) {
                if (SessionLocator_1.SessionLocator.TenantPM.CountryCode.toUpperCase() == "US") {
                    myDimensionsUnitCode = "Inc";
                    myVolumeUnitCode = "CBI";
                    myGrossWeightUnitCode = "LB";
                    myChargeableWeightUnitCode = "LB";
                }
            }
        }
        if (this.IsCopyFromQuote) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DimensionsUnitCode)) {
                this.EntityPM.DimensionsUnitCode = myDimensionsUnitCode;
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.VolumeUnitCode)) {
                this.EntityPM.VolumeUnitCode = myVolumeUnitCode;
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.GrossWeightUnitCode)) {
                this.EntityPM.GrossWeightUnitCode = myGrossWeightUnitCode;
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ChargeableWeightUnitCode)) {
                this.EntityPM.ChargeableWeightUnitCode = myChargeableWeightUnitCode;
            }
            if (this.EntityPM.Ratio == null) {
                this.EntityPM.Ratio = Tools_1.AppTool.GetRatio(this.EntityPM.DirectionId, this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId, SessionLocator_1.SessionLocator.TenantPM.CountryCode);
            }
            if (this.EntityPM.DimFactor == null) {
                this.EntityPM.DimFactor = Tools_1.AppTool.GetDimFactorFromRatio(this.EntityPM.Ratio, this.EntityPM.DimensionsUnitCode, this.EntityPM.ChargeableWeightUnitCode);
            }
        }
        else {
            this.EntityPM.DimensionsUnitCode = myDimensionsUnitCode;
            this.EntityPM.VolumeUnitCode = myVolumeUnitCode;
            this.EntityPM.GrossWeightUnitCode = myGrossWeightUnitCode;
            this.EntityPM.ChargeableWeightUnitCode = myChargeableWeightUnitCode;
            this.EntityPM.Ratio = Tools_1.AppTool.GetRatio(this.EntityPM.DirectionId, this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId, SessionLocator_1.SessionLocator.TenantPM.CountryCode);
            this.EntityPM.DimFactor = Tools_1.AppTool.GetDimFactorFromRatio(this.EntityPM.Ratio, this.EntityPM.DimensionsUnitCode, this.EntityPM.ChargeableWeightUnitCode);
            this.EntityPM.VolumetricWeight = QuoteUtilities_1.QuoteUtilities.ComputeVolumetricWeight(this.EntityPM);
            this.EntityPM.ChargeableWeight = QuoteUtilities_1.QuoteUtilities.ComputeChargeableWeight(this.EntityPM);
        }
    };
    NewQuoteComponent.prototype.SetPartners = function () {
        this.IsShipperMyCustomer = false;
        this.IsConsigneeMyCustomer = false;
        var myCRMCustomerId = null;
        var isTakenFromSourceEntity = false;
        if (this.IsCopyFromQuote) {
            if (this.sourceEntityPM) {
                this.EntityPM.QuoteCustomerTypeCode = this.sourceEntityPM.QuoteCustomerTypeCode;
                isTakenFromSourceEntity = true;
            }
        }
        if (isTakenFromSourceEntity) {
            if (this.EntityPM.QuoteCustomerTypeCode == "SHI") {
                this.IsShipperMyCustomer = true;
            }
            if (this.EntityPM.QuoteCustomerTypeCode == "CON") {
                this.IsConsigneeMyCustomer = true;
            }
        }
        else {
            switch (this.DirectionId) {
                case "I": {
                    this.QuoteCustomerTypeCode = "CON";
                    this.IsConsigneeMyCustomer = true;
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.DefaultCustomerId)) {
                        this.ConsigneeId = this.DefaultCustomerId;
                        if (this.ShipperId == this.DefaultCustomerId) {
                            this.ShipperId = null;
                        }
                    }
                    break;
                }
                default: {
                    this.QuoteCustomerTypeCode = "SHI";
                    this.IsShipperMyCustomer = true;
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.DefaultCustomerId)) {
                        this.ShipperId = this.DefaultCustomerId;
                        if (this.ConsigneeId == this.DefaultCustomerId) {
                            this.ConsigneeId = null;
                        }
                    }
                    break;
                }
            }
        }
    };
    NewQuoteComponent.prototype.SetupOrderDetails = function () {
        var _this = this;
        this.PackageType1Quantity = null;
        this.PackageType2Quantity = null;
        this.PackageType3Quantity = null;
        this.PackageType4Quantity = null;
        this.PackageType5Quantity = null;
        this.PackageType1Id = null;
        this.PackageType2Id = null;
        this.PackageType3Id = null;
        this.PackageType4Id = null;
        this.PackageType5Id = null;
        this.Volume = null;
        this.GrossWeight = null;
        this.EntityPM.VolumetricWeight = null;
        this.ChargeableWeight = null;
        this.EntityPM.QuotePackages.forEach(function (item) {
            if (_this.EntityPM.QuotePackages.indexOf(item) > -1) {
                _this.EntityPM.RemoveQuotePackagePM(item);
            }
        });
        this.IsLCLEntity = Tools_1.AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
        this.IsFCLEntity = !this.IsLCLEntity;
        this.SetUIProperties_NumberOfPackages();
        this.SetUnits();
    };
    NewQuoteComponent.prototype.AddCustomerClicked = function (customerType) {
        var _this = this;
        var myComponentPath = null;
        var title = "";
        if (this.CustomerDependencyProperty1 == "AG") {
            myComponentPath = "./CommonModules/CommonAgent/Components/NewEntity/NewAgentComponent";
        }
        else {
            myComponentPath = "./CommonModules/CommonCustomer/Components/NewEntity/NewCustomerComponent";
        }
        if (customerType == "Customer") {
            title = "New " + this.ComputeAddCustomerTitle();
        }
        else {
            title = "New " + customerType;
        }
        var args = new Args_2.NewEntityArgs();
        if (customerType == "Shipper") {
            if (!this.IsShipperMyCustomer) {
                args.Perspective = "ShippersAndConsignees";
            }
        }
        else if (customerType == "Consignee") {
            if (!this.IsConsigneeMyCustomer) {
                args.Perspective = "ShippersAndConsignees";
            }
        }
        else if (customerType == "Customer") {
            if (this.QuoteCustomerTypeCode == "SHI") {
                if (!this.IsShipperMyCustomer) {
                    args.Perspective = "ShippersAndConsignees";
                }
            }
            else if (this.QuoteCustomerTypeCode == "CON") {
                if (!this.IsConsigneeMyCustomer) {
                    args.Perspective = "ShippersAndConsignees";
                }
            }
        }
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = title;
        logWindow.Width = 960;
        logWindow.Height = 600;
        logWindow.WindowArgs = args;
        logWindow.Show(myComponentPath);
        logWindow.ComponentLoaded.subscribe(function (comp) {
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    if (customerType == "Shipper") {
                        _this.ShipperId = comp.EntityPM.Id;
                        _this.customerSalesmanId = comp.EntityPM.SalesmanUserId;
                    }
                    else if (customerType == "Consignee") {
                        _this.ConsigneeId = comp.EntityPM.Id;
                        _this.customerSalesmanId = comp.EntityPM.SalesmanUserId;
                    }
                    else if (customerType == "Customer") {
                        _this.CustomerId = comp.EntityPM.Id;
                        _this.customerSalesmanId = comp.EntityPM.SalesmanUserId;
                    }
                }
            });
        });
    };
    NewQuoteComponent.prototype.ComputeAddCustomerTitle = function () {
        var myResult = "";
        switch (this.QuoteCustomerTypeCode) {
            case "AGT":
                {
                    myResult = "Agent";
                    break;
                }
            case "CON":
                {
                    myResult = "Consignee";
                    break;
                }
            case "NOT":
                {
                    myResult = "Notify";
                    break;
                }
            case "SHI":
                {
                    myResult = "Shipper";
                    break;
                }
        }
        return myResult;
    };
    NewQuoteComponent.prototype.AddPotentialCustomerClicked = function (customerType) {
        var _this = this;
        var _entityResourceService = new EntityResourceService_1.EntityResourceService();
        _entityResourceService.getEntityResourceByTableName("Customer").subscribe(function (response2) {
            _entityResourceService.getEntityResourceByTableName("Contact").subscribe(function (response2) {
                var args = new Args_2.NewEntityArgs();
                if (customerType == "Shipper") {
                    if (!_this.IsShipperMyCustomer) {
                        args.Perspective = "ShippersAndConsignees";
                    }
                }
                else {
                    if (!_this.IsConsigneeMyCustomer) {
                        args.Perspective = "ShippersAndConsignees";
                    }
                }
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Title = "New Potential " + customerType;
                logWindow.Width = 990;
                logWindow.Height = 600;
                logWindow.WindowArgs = args;
                logWindow.Show("./CommonModules/CommonPartners/Components/NewEntity/NewPotentialCustomerComponent");
                logWindow.ComponentLoaded.subscribe(function (comp) {
                    logWindow.WindowClosed.subscribe(function (s) {
                        if (s) {
                            _this.customerSalesmanId = comp.EntityPM.SalesmanUserId;
                            if (customerType == "Shipper") {
                                _this.ShipperId = comp.EntityPM.Id;
                            }
                            else {
                                _this.ConsigneeId = comp.EntityPM.Id;
                            }
                        }
                    });
                });
            });
        });
    };
    NewQuoteComponent.prototype.AddAgentClicked = function (customerType) {
        var _this = this;
        var args = new Args_2.NewEntityArgs();
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "New Agent";
        logWindow.Width = 960;
        logWindow.Height = 600;
        logWindow.WindowArgs = args;
        logWindow.Show("./CommonModules/CommonAgent/Components/NewEntity/NewAgentComponent");
        logWindow.ComponentLoaded.subscribe(function (comp) {
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    if (customerType == "Shipper") {
                        _this.ShipperId = comp.EntityPM.Id;
                    }
                    else {
                        _this.ConsigneeId = comp.EntityPM.Id;
                    }
                }
            });
        });
    };
    NewQuoteComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewQuoteComponent.prototype.OkButtonClicked = function () {
        this.CurrentSession.StartBusyIndicatorSaving();
        this.SetDataOnFinish();
        var entityValidator = new QuoteValidator_1.QuoteValidator();
        this.ValidationErrorsList = entityValidator.Validate(this.EntityPM);
        if (this.ValidationErrorsList.length == 0) {
            this.InitializeCopy_Charges();
            this.SubmitCreatingNewQuote();
        }
        else {
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
        }
    };
    NewQuoteComponent.prototype.SetDataOnFinish = function () {
        if (this.EntityPM.ExchangeRate == null) {
            this.EntityPM.ExchangeRate = 1;
        }
        var computeNumberOfPackages = false;
        if (this.EntityPM.TransportModeId.toUpperCase() == "O" && this.EntityPM.ShipmentTypeId.toUpperCase() == "FCLD") {
            computeNumberOfPackages = true;
        }
        else if (this.EntityPM.TransportModeId.toUpperCase() == "I" && this.EntityPM.ShipmentTypeId.toUpperCase() == "FTL") {
            computeNumberOfPackages = true;
        }
        if (computeNumberOfPackages) {
            var sum = 0;
            if (this.EntityPM.PackageType1Quantity > 0) {
                sum = sum + this.EntityPM.PackageType1Quantity;
            }
            if (this.EntityPM.PackageType2Quantity > 0) {
                sum = sum + this.EntityPM.PackageType2Quantity;
            }
            if (this.EntityPM.PackageType3Quantity > 0) {
                sum = sum + this.EntityPM.PackageType3Quantity;
            }
            if (this.EntityPM.PackageType4Quantity > 0) {
                sum = sum + this.EntityPM.PackageType4Quantity;
            }
            if (this.EntityPM.PackageType5Quantity > 0) {
                sum = sum + this.EntityPM.PackageType5Quantity;
            }
            if (this.IsLCLEntity) {
                this.EntityPM.NumberOfPackages = sum;
            }
            else {
                this.EntityPM.NumberOfContainers = sum;
            }
        }
        this.SetCustomerDataOnFinish();
        this.SetInlandDomesticOnFinish();
        this.SetPickupDeliveryOnFinish();
        this.SetCountryOnFinish();
    };
    NewQuoteComponent.prototype.SetCustomerDataOnFinish = function () {
        this.SetCustomePartner();
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.SalesmanUserId)) {
            this.EntityPM.SalesmanUserId = Tools_1.AppTool.IsNullOrEmpty(this.customerSalesmanId) ? this.EntityPM.CreatedByUserId : this.customerSalesmanId;
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.BusinessUnitId)) {
            this.EntityPM.BusinessUnitId = Tools_1.AppTool.IsNullOrEmpty(this.customerSalesmanBusinessUnitId) ? SessionLocator_1.SessionLocator.LoggedUserPM.BusinessUnitId : this.customerSalesmanBusinessUnitId;
        }
        //this.EntityPM.CustomerId = null;
        //this.EntityPM.CustomerName = null;
        //this.EntityPM.CustomerNote = null;
        //this.EntityPM.CustomerContactId = null;
        //this.EntityPM.CustomerReference1 = null;
        //this.EntityPM.CustomerReference2 = null;
        //this.EntityPM.QuoteCustomerTypeCode = null;
        //if (this.IsShipperMyCustomer) {
        //    this.EntityPM.QuoteCustomerTypeCode = "SHI";
        //    this.EntityPM.CustomerId = this.EntityPM.ShipperId;
        //    this.EntityPM.CustomerName = this.EntityPM.ShipperName;
        //    this.EntityPM.CustomerNote = this.EntityPM.ShipperNote;
        //    this.EntityPM.CustomerContactId = this.EntityPM.ShipperContactId;
        //    this.EntityPM.CustomerReference1 = this.EntityPM.ShipperReference1;
        //    this.EntityPM.CustomerReference2 = this.EntityPM.ShipperReference2;
        //    this.EntityPM.SalesmanUserId = this.shipperSalesmanId;
        //    this.EntityPM.BusinessUnitId = this.shipperSalesmanBusinessUnitId;
        //}
        //else {
        //    this.EntityPM.QuoteCustomerTypeCode = "CON";
        //    this.EntityPM.CustomerId = this.EntityPM.ConsigneeId;
        //    this.EntityPM.CustomerName = this.EntityPM.ConsigneeName;
        //    this.EntityPM.CustomerNote = this.EntityPM.ConsigneeNote;
        //    this.EntityPM.CustomerContactId = this.EntityPM.ConsigneeContactId;
        //    this.EntityPM.CustomerReference1 = this.EntityPM.ConsigneeReference1;
        //    this.EntityPM.CustomerReference2 = this.EntityPM.ConsigneeReference2;
        //    this.EntityPM.SalesmanUserId = this.consigneeSalesmanId;
        //    this.EntityPM.BusinessUnitId = this.consigneeSalesmanBusinessUnitId;
        //}
    };
    NewQuoteComponent.prototype.SetInlandDomesticOnFinish = function () {
        if (this.IsInlandDomestic) {
            this.IncludePickUp = false;
            this.IncludeDelivery = false;
            this.FromPortId = null;
            this.ToPortId = null;
        }
    };
    NewQuoteComponent.prototype.SetPickupDeliveryOnFinish = function () {
        if (this.IncludePickUp) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PickUpAddressId)) {
                this.EntityPM.FromAddressCity = null;
                this.EntityPM.FromAddressZipCode = null;
                this.EntityPM.FromAddressCountryId = null;
            }
        }
        else {
            this.EntityPM.FromAddressCity = null;
            this.EntityPM.FromAddressZipCode = null;
            this.EntityPM.FromAddressCountryId = null;
            this.EntityPM.PickUpAddressId = null;
        }
        if (this.IncludeDelivery) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DeliveryAddressId)) {
                this.EntityPM.ToAddressCity = null;
                this.EntityPM.ToAddressZipCode = null;
                this.EntityPM.ToAddressCountryId = null;
            }
        }
        else {
            this.EntityPM.ToAddressCity = null;
            this.EntityPM.ToAddressZipCode = null;
            this.EntityPM.ToAddressCountryId = null;
            this.EntityPM.DeliveryAddressId = null;
        }
    };
    NewQuoteComponent.prototype.SetCountryOnFinish = function () {
        if (this.IsInlandDomestic) {
            if (this.ShipperAddressList != null) {
                this.EntityPM.FromCountryId = this.ShipperAddressList.CountryId;
                this.EntityPM.FromCountryIsEC = this.ShipperAddressList.CountryEC;
            }
            if (this.ConsigneeAddressList != null) {
                this.EntityPM.ToCountryId = this.ConsigneeAddressList.CountryId;
                this.EntityPM.ToCountryIsEC = this.ConsigneeAddressList.CountryEC;
            }
        }
        else {
            if (this.FromPortList != null) {
                this.EntityPM.FromCountryId = this.FromPortList.CountryId;
                this.EntityPM.FromCountryIsEC = this.FromPortList.CountryEC;
            }
            if (this.ToPortList != null) {
                this.EntityPM.ToCountryId = this.ToPortList.CountryId;
                this.EntityPM.ToCountryIsEC = this.ToPortList.CountryEC;
            }
        }
    };
    NewQuoteComponent.prototype.SubmitCreatingNewQuote = function () {
        var _this = this;
        var myService = new QuotePMService_1.QuotePMService();
        myService.insert(this.EntityPM).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (myResponse.HasError) {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
            }
            else {
                _this.CurrentSession.CloseCurrentWindowEmit('OK');
                if (_this.IsCopyFromQuote) {
                    _this.RunInEditMode();
                }
            }
        });
    };
    NewQuoteComponent.prototype.RunInEditMode = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: _this.EntityPM.Id, ObjectTableName: "Quote", BackButtonLabel: "Quote: " + _this.sourceEntityPM.QuoteNumber });
            var isEditComponentSaved = false;
            cmpRef.instance.BackCompleted.subscribe(function (bk) {
                if (isEditComponentSaved) {
                    //this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                }
            });
            cmpRef.instance.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    isEditComponentSaved = true;
                }
            });
        });
    };
    NewQuoteComponent.prototype.InitializeCopy = function (myQuote) {
        if (myQuote != null) {
            if (this.IsCopyFromQuote) {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.sourceEntityPM.AgentId)) {
                    this.IsCopyOtherPartnersVisible = true;
                }
                else if (!Tools_1.AppTool.IsNullOrEmpty(this.sourceEntityPM.NotifyId)) {
                    this.IsCopyOtherPartnersVisible = true;
                }
                this.EntityPM.IsCopy = true;
                this.sourceEntityPM = myQuote;
                this.ButtonContent = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.B.Copy");
                if (!Tools_1.AppTool.IsNullOrEmpty(myQuote.OpportunityId)) {
                    this.EntityPM.OpportunityId = myQuote.OpportunityId;
                }
                //if (myQuote.IsCustomerSet) {
                this.QuoteCustomerTypeCode = myQuote.QuoteCustomerTypeCode;
                this.EntityPM.IsCustomerSet = myQuote.IsCustomerSet;
                this.EntityPM.CustomerId = myQuote.CustomerId;
                this.EntityPM.CustomerName = myQuote.CustomerName;
                this.EntityPM.CustomerNote = myQuote.CustomerNote;
                this.EntityPM.CustomerRankName = myQuote.CustomerRankName;
                this.EntityPM.CustomerReference1 = myQuote.CustomerReference1;
                this.EntityPM.CustomerReference2 = myQuote.CustomerReference2;
                this.EntityPM.CustomerContactId = myQuote.CustomerContactId;
                this.ComputeCustomerDependency();
                this.ComputeCustomerAddressForCopy(myQuote);
                QuoteUtilities_1.QuoteUtilities.CopyQuote(this.EntityPM, myQuote);
                QuoteUtilities_1.QuoteUtilities.CopyQuotePackages(this.EntityPM, myQuote);
                this.IsLCLEntity = Tools_1.AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
                this.IsFCLEntity = !this.IsLCLEntity;
                this.DirectionImageSRC = "./Images/Directions/" + this.EntityPM.DirectionId + ".png";
                this.TransportModeImageSRC = "./Images/Icons/" + this.EntityPM.TransportModeId + ".png";
                this.InitializeCopy_Objects();
                this.OnFiltersChanged();
            }
        }
    };
    NewQuoteComponent.prototype.InitializeCopy_Objects = function () {
        // Partners
        if (!Tools_1.AppTool.IsNullOrEmpty(this.sourceEntityPM.ShipperId)) {
            this.ShipperCopyIsEnabled = true;
            if (this.QuoteSetting) {
                if (this.QuoteSetting.CopyShipper) {
                    this.ShipperCopyIsChecked = true;
                }
            }
            else {
                this.ShipperCopyIsChecked = true;
            }
            if (this.sourceEntityPM.CustomerId == this.sourceEntityPM.ShipperId) {
                this.IsShipperMyCustomer = true;
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.sourceEntityPM.ConsigneeId)) {
            this.ConsigneeCopyIsEnabled = true;
            if (this.QuoteSetting) {
                if (this.QuoteSetting.CopyConsignee) {
                    this.ConsigneeCopyIsChecked = true;
                }
            }
            else {
                this.ConsigneeCopyIsChecked = true;
            }
            if (this.sourceEntityPM.CustomerId == this.sourceEntityPM.ConsigneeId) {
                this.IsConsigneeMyCustomer = true;
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.sourceEntityPM.AgentId)) {
            this.AgentCopyIsEnabled = true;
            if (this.QuoteSetting) {
                if (this.QuoteSetting.CopyAgent) {
                    this.AgentCopyIsChecked = true;
                }
            }
            else {
                this.AgentCopyIsChecked = true;
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.sourceEntityPM.NotifyId)) {
            this.NotifyCopyIsEnabled = true;
            if (this.QuoteSetting) {
                if (this.QuoteSetting.CopyNotify) {
                    this.NotifyCopyIsChecked = true;
                }
            }
            else {
                this.NotifyCopyIsChecked = true;
            }
        }
        // Routing
        if (this.QuoteSetting) {
            this.MainCarriageCopyIsEnabled = true;
            if (this.QuoteSetting.CopyMainCarriage) {
                this.MainCarriageCopyIsChecked = true;
            }
            if (this.QuoteSetting.CopyPickup) {
                this.CopyPickUpIsChecked = true;
            }
            if (this.QuoteSetting.CopyDelivery) {
                this.CopyDeliveryIsChecked = true;
            }
        }
        else {
            this.MainCarriageCopyIsEnabled = false;
            this.MainCarriageCopyIsChecked = true;
            this.CopyPickUpIsChecked = true;
            this.CopyDeliveryIsChecked = true;
        }
        // Charges
        if (this.sourceEntityPM.QuoteCharges.length > 0) {
            this.ChargesTypesCopyIsEnabled = true;
            if (this.QuoteSetting) {
                if (this.QuoteSetting.CopyChargesTypes) {
                    this.ChargesTypesCopyIsChecked = true;
                }
            }
            else {
                this.ChargesTypesCopyIsChecked = false;
            }
        }
    };
    NewQuoteComponent.prototype.InitializeCopy_Charges = function () {
        var _this = this;
        if (this.IsCopyFromQuote) {
            if (this.ChargesTypesCopyIsChecked) {
                if (this.EntityPM.QuoteCharges.length > 0) {
                    this.EntityPM.QuoteCharges.forEach(function (item) {
                        item.QuoteChargePriceSteps.forEach(function (priceItem) {
                            item.RemoveQuotePriceStepsPM(priceItem);
                        });
                        _this.EntityPM.RemoveQuoteChargePM(item);
                    });
                }
                QuoteUtilities_1.QuoteUtilities.CopyQuoteCharges(this.EntityPM, this.sourceEntityPM, this.CopySaleIsChecked, this.CopyCostIsChecked);
                this.EntityPM.TEU = QuoteUtilities_1.QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
                if (this.EntityPM.ExchangeRate == null) {
                    if (!Tools_1.AppTool.IsNullOrZero(this.sourceEntityPM.EstimateProfit) && !Tools_1.AppTool.IsNullOrZero(this.sourceEntityPM.ExchangeRate)) {
                        this.EntityPM.EstimateProfit = Math.abs((this.sourceEntityPM.EstimateProfit * this.sourceEntityPM.ExchangeRate));
                    }
                    else {
                        this.EntityPM.EstimateProfit = 0;
                    }
                }
                else {
                    if (this.sourceEntityPM.EstimateProfit == 0 || this.sourceEntityPM.EstimateProfit == null) {
                        this.EntityPM.EstimateProfit = 0;
                    }
                    else {
                        this.EntityPM.EstimateProfit = Math.abs(this.sourceEntityPM.EstimateProfit);
                    }
                }
            }
        }
    };
    NewQuoteComponent.prototype.ComputeCustomerAddressForCopy = function (myQuote) {
        switch (this.QuoteCustomerTypeCode) {
            case "SHI": {
                this.CustomerAddressId = myQuote.ShipperMainAddressId;
                break;
            }
            case "CON": {
                this.CustomerAddressId = myQuote.ConsigneeMainAddressId;
                break;
            }
            case "AGT": {
                this.CustomerAddressId = myQuote.AgentAddressId;
                break;
            }
            case "NOT": {
                this.CustomerAddressId = myQuote.NotifyAddressId;
                break;
            }
            case "OTH": {
                break;
            }
        }
    };
    Object.defineProperty(NewQuoteComponent.prototype, "ShipperCopyIsChecked", {
        get: function () { return this.shipperCopyIsChecked; },
        set: function (newValue) {
            if (this.shipperCopyIsChecked != newValue) {
                this.shipperCopyIsChecked = newValue;
                if (newValue) {
                    this.EntityPM.ShipperId = this.sourceEntityPM.ShipperId;
                    this.EntityPM.ShipperName = this.sourceEntityPM.ShipperName;
                    this.ShipperContactId = this.sourceEntityPM.ShipperContactId;
                    this.ShipperReference1 = this.sourceEntityPM.ShipperReference1;
                    this.ShipperReference2 = this.sourceEntityPM.ShipperReference2;
                    this.ShipperNote = this.sourceEntityPM.ShipperNote;
                    this.GetShipperCardData();
                    if (this.EntityPM.QuoteCustomerTypeCode == "SHI") {
                        this.EntityPM.CustomerId = this.sourceEntityPM.ShipperId;
                    }
                }
                else {
                    this.ShipperId = null;
                    this.EntityPM.ShipperName = null;
                    this.ShipperContactId = null;
                    this.ShipperReference1 = null;
                    this.ShipperReference2 = null;
                    this.ShipperNote = null;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "ConsigneeCopyIsChecked", {
        get: function () { return this.consigneeCopyIsChecked; },
        set: function (newValue) {
            if (this.consigneeCopyIsChecked != newValue) {
                this.consigneeCopyIsChecked = newValue;
                if (newValue) {
                    this.EntityPM.ConsigneeId = this.sourceEntityPM.ConsigneeId;
                    this.EntityPM.ConsigneeName = this.sourceEntityPM.ConsigneeName;
                    this.EntityPM.ConsigneeContactId = this.sourceEntityPM.ConsigneeContactId;
                    this.EntityPM.ConsigneeReference1 = this.sourceEntityPM.ConsigneeReference1;
                    this.EntityPM.ConsigneeReference2 = this.sourceEntityPM.ConsigneeReference2;
                    this.EntityPM.ConsigneeNote = this.sourceEntityPM.ConsigneeNote;
                    this.GetConsigneeCardData();
                    if (this.EntityPM.QuoteCustomerTypeCode == "CON") {
                        this.EntityPM.CustomerId = this.sourceEntityPM.ConsigneeId;
                    }
                }
                else {
                    this.ConsigneeId = null;
                    this.ConsigneeContactId = null;
                    this.ConsigneeReference1 = null;
                    this.ConsigneeReference2 = null;
                    this.ConsigneeNote = null;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "AgentCopyIsChecked", {
        get: function () { return this.agentCopyIsChecked; },
        set: function (newValue) {
            if (this.agentCopyIsChecked != newValue) {
                this.agentCopyIsChecked = newValue;
                this.EntityPM.AgentId = !newValue ? null : this.sourceEntityPM.AgentId;
                this.EntityPM.AgentName = !newValue ? null : this.sourceEntityPM.AgentName;
                this.EntityPM.AgentAddressId = !newValue ? null : this.sourceEntityPM.AgentAddressId;
                this.EntityPM.AgentContactId = !newValue ? null : this.sourceEntityPM.AgentContactId;
                if (this.EntityPM.QuoteCustomerTypeCode == "AGT") {
                    this.EntityPM.CustomerId = this.sourceEntityPM.AgentId;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "NotifyCopyIsChecked", {
        get: function () { return this.notifyCopyIsChecked; },
        set: function (newValue) {
            if (this.notifyCopyIsChecked != newValue) {
                this.notifyCopyIsChecked = newValue;
                this.EntityPM.NotifyId = !newValue ? null : this.sourceEntityPM.NotifyId;
                this.EntityPM.NotifyName = !newValue ? null : this.sourceEntityPM.NotifyName;
                this.EntityPM.NotifyAddressId = !newValue ? null : this.sourceEntityPM.NotifyAddressId;
                this.EntityPM.NotifyContactId = !newValue ? null : this.sourceEntityPM.NotifyContactId;
                this.EntityPM.NotifyNote = !newValue ? null : this.sourceEntityPM.NotifyNote;
                if (this.EntityPM.QuoteCustomerTypeCode == "NOT") {
                    this.EntityPM.CustomerId = this.sourceEntityPM.NotifyId;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "MainCarriageCopyIsChecked", {
        get: function () { return this.mainCarriageCopyIsChecked; },
        set: function (value) {
            if (this.mainCarriageCopyIsChecked != value) {
                this.mainCarriageCopyIsChecked = value;
                if (value) {
                    this.EntityPM.FromPort = this.sourceEntityPM.FromPort;
                    this.EntityPM.FromPortCountry = this.sourceEntityPM.FromPortCountry;
                    this.EntityPM.FromPortId = this.sourceEntityPM.FromPortId;
                    this.EntityPM.FromPortName = this.sourceEntityPM.FromPortName;
                    this.EntityPM.ToPort = this.sourceEntityPM.ToPort;
                    this.EntityPM.ToPortCountry = this.sourceEntityPM.ToPortCountry;
                    this.EntityPM.ToPortId = this.sourceEntityPM.ToPortId;
                    this.EntityPM.ToPortName = this.sourceEntityPM.ToPortName;
                }
                else {
                    this.EntityPM.FromPort = null;
                    this.EntityPM.FromPortCountry = null;
                    this.EntityPM.FromPortId = null;
                    this.EntityPM.FromPortName = null;
                    this.EntityPM.ToPort = null;
                    this.EntityPM.ToPortCountry = null;
                    this.EntityPM.ToPortId = null;
                    this.EntityPM.ToPortName = null;
                }
                this.SetUIProperties_Domestic();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "CopyPickUpIsChecked", {
        get: function () { return this.copyPickUpIsChecked; },
        set: function (newValue) {
            if (this.copyPickUpIsChecked != newValue) {
                this.copyPickUpIsChecked = newValue;
                this.IncludePickUp = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "CopyDeliveryIsChecked", {
        get: function () { return this.copyDeliveryIsChecked; },
        set: function (newValue) {
            if (this.copyDeliveryIsChecked != newValue) {
                this.copyDeliveryIsChecked = newValue;
                this.IncludeDelivery = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewQuoteComponent.prototype, "ChargesTypesCopyIsChecked", {
        get: function () { return this.chargesTypesCopyIsChecked; },
        set: function (value) {
            if (this.chargesTypesCopyIsChecked != value) {
                this.chargesTypesCopyIsChecked = value;
                if (value) {
                    if (this.QuoteSetting) {
                        if (this.QuoteSetting.CopyChargesCost) {
                            this.CopyCostIsChecked = true;
                        }
                        if (this.QuoteSetting.CopyChargesSale) {
                            this.CopySaleIsChecked = true;
                        }
                    }
                }
                else {
                    this.CopyCostIsChecked = false;
                    this.CopySaleIsChecked = false;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], NewQuoteComponent.prototype, "viewContainerRef", void 0);
    NewQuoteComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NewQuoteComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewQuoteComponent);
    return NewQuoteComponent;
}(BaseComponent_1.BaseComponent));
exports.NewQuoteComponent = NewQuoteComponent;
var FilterClass = /** @class */ (function () {
    function FilterClass(code, name, src) {
        if (src === void 0) { src = null; }
        this.Code = code;
        this.Name = name;
        this.SRC = src;
    }
    return FilterClass;
}());
//# sourceMappingURL=NewQuoteComponent.js.map