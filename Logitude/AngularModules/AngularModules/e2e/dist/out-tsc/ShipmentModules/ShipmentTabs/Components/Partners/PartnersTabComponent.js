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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var CardListService_1 = require("../../../../Common/Services/StandardLists/CardListService");
var AddressListService_1 = require("../../../../Common/Services/StandardLists/AddressListService");
var ContactListService_1 = require("../../../../Common/Services/StandardLists/ContactListService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var Args_1 = require("../../../../Infrastructure/Args");
var Tools_1 = require("../../../../Infrastructure/Tools");
var AddressPM_1 = require("../../../../Common/EntityPMs/AddressPM");
var Args_2 = require("../../../../Shipment/Args");
var Tools_2 = require("../../../../Shipment/Tools");
var PartnersTabComponent = /** @class */ (function () {
    function PartnersTabComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.SessionEvent = null;
        this.TabChangedEvent = null;
        this.SaveCompletedEvent = null;
        this.LoadCompletedEvent = null;
        this.IsEditingEnabled = true;
        this.AllAddresses = [];
        this.AllContacts = [];
        this.IsAddDisabled_SHIPR = false;
        this.IsAddDisabled_CONSI = false;
        this.IsAddDisabled_AGENT = false;
        this.IsAddDisabled_ISSAG = false;
        this.IsAddDisabled_CSAIM = false;
        this.IsAddDisabled_CSAEX = false;
        this.IsAddDisabled_NOTF1 = false;
        this.IsAddDisabled_NOTF2 = false;
        this.IsAddDisabled_SHPNT = false;
        this.IsAddDisabled_CONNT = false;
        this.IsAddDisabled_FRTFR = false;
        this.IsAddDisabled_COLOD = false;
        this.IsAddDisabled_CLERN = false;
        this.IsAddDisabled_CONSL = false;
        this.IsAddDisabled_REAGT = false;
        this.UpdateSalesmanId = null;
        this.UpdateSalesmanName = null;
        this.UpdateSalesmanAccountManagerText = null;
        this.UpdateAccountManagerId = null;
        this.UpdateAccountManagerName = null;
        this.IsUpdateSalesmanAccountManagerVisible = false;
        this.SalesmanUpdated = false;
        this.AccountManagerUpdated = false;
        this.EntityPM = this.entityArgs.EntityPM;
        this.ObjectTableName = this.entityArgs.ObjectTableName;
        this.InitializeServices();
        this.Listen();
    }
    PartnersTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent) {
            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
                if (s == "AWBWizardClosed") {
                    _this.UpdateScreen();
                }
            });
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.UpdateScreen();
                }
            });
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.UpdateScreen();
                }
            });
            this.TabChangedEvent = this.entityArgs.EditComponent.TabChanged.subscribe(function (tabCode) {
                _this.IsUpdateSalesmanAccountManagerVisible = false;
            });
        }
    };
    PartnersTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SessionEvent);
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.TabChangedEvent);
    };
    PartnersTabComponent.prototype.InitializeServices = function () {
        this.CardListService = new CardListService_1.CardListService();
        this.AddressListService = new AddressListService_1.AddressListService();
        this.ContactListService = new ContactListService_1.ContactListService();
    };
    PartnersTabComponent.prototype.ngOnInit = function () {
        if (this.EntityPM != null) {
            this.UpdateScreen();
        }
    };
    PartnersTabComponent.prototype.UpdateScreen = function () {
        this.SetUIProperties();
        this.BuildItemsCollection();
        this.SetAddButtonsIsDisabled();
    };
    PartnersTabComponent.prototype.SetUIProperties = function () {
        this.IsEditingEnabled = Tools_2.ShipmentTool.IsEditingEnabled(this.EntityPM);
    };
    PartnersTabComponent.prototype.BuildItemsCollection = function () {
        this.ItemsCollection = [];
        if (this.EntityPM.ShipperId != null) {
            this.ItemsCollection.push(new PartnerItem(this, "SHIPR"));
        }
        if (this.EntityPM.ConsigneeId != null) {
            this.ItemsCollection.push(new PartnerItem(this, "CONSI"));
        }
        if (this.EntityPM.AgentId != null) {
            this.ItemsCollection.push(new PartnerItem(this, "AGENT"));
        }
        if (this.EntityPM.IssuingCarrierAgentId != null) {
            this.ItemsCollection.push(new PartnerItem(this, "ISSAG"));
        }
        if (this.EntityPM.CustomAgentExportId != null) {
            this.ItemsCollection.push(new PartnerItem(this, "CSAEX"));
        }
        if (this.EntityPM.CustomAgentImportId != null) {
            this.ItemsCollection.push(new PartnerItem(this, "CSAIM"));
        }
        if (this.EntityPM.Notify1Id != null) {
            this.ItemsCollection.push(new PartnerItem(this, "NOTF1"));
        }
        if (this.EntityPM.Notify2Id != null) {
            this.ItemsCollection.push(new PartnerItem(this, "NOTF2"));
        }
        if (this.EntityPM.ShipperNotExporterId != null) {
            this.ItemsCollection.push(new PartnerItem(this, "SHPNT"));
        }
        if (this.EntityPM.ConsigneeNotImporterId != null) {
            this.ItemsCollection.push(new PartnerItem(this, "CONNT"));
        }
        if (this.EntityPM.FreightForwarderId != null) {
            this.ItemsCollection.push(new PartnerItem(this, "FRTFR"));
        }
        if (this.EntityPM.ColoaderId != null) {
            this.ItemsCollection.push(new PartnerItem(this, "COLOD"));
        }
        if (this.EntityPM.CustomClearancePointId != null) {
            this.ItemsCollection.push(new PartnerItem(this, "CLERN"));
        }
        if (this.EntityPM.ConsolidatorId != null) {
            this.ItemsCollection.push(new PartnerItem(this, "CONSL"));
        }
        if (this.EntityPM.ReleasingAgentId != null) {
            this.ItemsCollection.push(new PartnerItem(this, "REAGT"));
        }
    };
    Object.defineProperty(PartnersTabComponent.prototype, "ShowAddPartners", {
        get: function () {
            return SessionLocator_1.SessionLocator.TenantPM.IsHybrid ? false : true;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnersTabComponent.prototype, "IsEditDisabled", {
        get: function () {
            return SessionLocator_1.SessionLocator.TenantPM.IsHybrid ? true : false;
        },
        enumerable: true,
        configurable: true
    });
    PartnersTabComponent.prototype.SetAddButtonsIsDisabled = function () {
        this.IsAddDisabled_SHIPR = this.EntityPM.ShipperId == null ? false : true;
        this.IsAddDisabled_CONSI = this.EntityPM.ConsigneeId == null ? false : true;
        this.IsAddDisabled_AGENT = this.EntityPM.AgentId == null ? false : true;
        this.IsAddDisabled_ISSAG = this.EntityPM.IssuingCarrierAgentId == null ? false : true;
        this.IsAddDisabled_CSAIM = this.EntityPM.CustomAgentImportId == null ? false : true;
        this.IsAddDisabled_CSAEX = this.EntityPM.CustomAgentExportId == null ? false : true;
        this.IsAddDisabled_NOTF1 = this.EntityPM.Notify1Id == null ? false : true;
        this.IsAddDisabled_NOTF2 = this.EntityPM.Notify2Id == null ? false : true;
        this.IsAddDisabled_SHPNT = this.EntityPM.ShipperNotExporterId == null ? false : true;
        this.IsAddDisabled_CONNT = this.EntityPM.ConsigneeNotImporterId == null ? false : true;
        this.IsAddDisabled_FRTFR = this.EntityPM.FreightForwarderId == null ? false : true;
        this.IsAddDisabled_COLOD = this.EntityPM.ColoaderId == null ? false : true;
        this.IsAddDisabled_CLERN = this.EntityPM.CustomClearancePointId == null ? false : true;
        this.IsAddDisabled_CONSL = this.EntityPM.ConsolidatorId == null ? false : true;
        this.IsAddDisabled_REAGT = this.EntityPM.ReleasingAgentId == null ? false : true;
    };
    PartnersTabComponent.prototype.AddPartner = function (myCode) {
        var newPartnerItem = new PartnerItem(this, myCode);
        var myWindowTitle;
        switch (myCode) {
            case "COLOD": {
                myWindowTitle = "Add Coloader";
                break;
            }
            case "CLERN": {
                myWindowTitle = "Add Custom Clearance Point";
                break;
            }
            case "CONSL": {
                myWindowTitle = "Add Consolidator";
                break;
            }
            case "REAGT": {
                myWindowTitle = "Add Releasing Agent";
                break;
            }
            default: {
                myWindowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.S.Partners.Add" + newPartnerItem.FullCode);
                break;
            }
        }
        this.RunAddEditPartner(newPartnerItem, myWindowTitle);
    };
    ;
    PartnersTabComponent.prototype.EditPartner = function (myPartnerItem) {
        var myWindowTitle;
        switch (myPartnerItem.Code) {
            case "COLOD": {
                myWindowTitle = "Edit Coloader";
                break;
            }
            case "CLERN": {
                myWindowTitle = "Edit Custom Clearance Point";
                break;
            }
            case "CONSL": {
                myWindowTitle = "Edit Consolidator";
                break;
            }
            case "REAGT": {
                myWindowTitle = "Edit Releasing Agent";
                break;
            }
            default: {
                myWindowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.S.Partners.Edit" + myPartnerItem.FullCode);
                break;
            }
        }
        this.RunAddEditPartner(myPartnerItem, myWindowTitle);
    };
    PartnersTabComponent.prototype.RunAddEditPartner = function (myPartnerItem, myWindowTitle) {
        var _this = this;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Title = myWindowTitle;
        logitudeWindow.DataContext = myPartnerItem;
        logitudeWindow.Show("./ShipmentModules/ShipmentTabs/Components/Partners/AddEditPartnerComponent");
        logitudeWindow.WindowClosed.subscribe(function (s) {
            if (s == "OK") {
                _this.BuildItemsCollection();
                _this.SetAddButtonsIsDisabled();
            }
        });
    };
    PartnersTabComponent.prototype.DeletePartner = function (myPartnerItem) {
        var _this = this;
        if (myPartnerItem.IsEditingEnabled) {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.DeleteThisPartner"));
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    if (myPartnerItem.IsCustomer) {
                        _this.SetDefaultCustomer();
                    }
                    var index = _this.ItemsCollection.indexOf(myPartnerItem);
                    if (index != -1) {
                        _this.ItemsCollection.splice(index, 1);
                    }
                    myPartnerItem.PartnerId = null;
                    myPartnerItem.AddressId = null;
                    myPartnerItem.ContactId = null;
                    myPartnerItem.Reference1 = null;
                    myPartnerItem.Reference2 = null;
                    _this.SetAddButtonsIsDisabled();
                    _this.CurrentSession.FireEvent("ShipmentPartnersChanged");
                }
            });
        }
    };
    PartnersTabComponent.prototype.SetDefaultCustomer = function () {
        if (this.EntityPM.ShipmentLevelCode == "C") {
            this.EntityPM.CustomerId = null;
            this.EntityPM.CustomerName = null;
            this.EntityPM.CustomerNote = null;
            this.EntityPM.CustomerReference1 = null;
            this.EntityPM.CustomerReference2 = null;
            this.EntityPM.CustomerAddressId = null;
            this.EntityPM.CustomerContactId = null;
            this.EntityPM.ShipmentCustomerTypeCode = null;
        }
        else {
            if (this.EntityPM.DirectionId == "I") {
                this.EntityPM.ShipmentCustomerTypeCode = "CON";
                this.EntityPM.CustomerId = this.EntityPM.ConsigneeId;
                this.EntityPM.CustomerName = this.EntityPM.ConsigneeName;
                this.EntityPM.CustomerNote = this.EntityPM.ConsigneeNote;
                this.EntityPM.CustomerAddressId = this.EntityPM.ConsigneeAddressId;
                this.EntityPM.CustomerContactId = this.EntityPM.ConsigneeContactId;
                this.EntityPM.CustomerReference1 = this.EntityPM.ConsigneeReference1;
                this.EntityPM.CustomerReference2 = this.EntityPM.ConsigneeReference2;
            }
            else {
                this.EntityPM.ShipmentCustomerTypeCode = "SHI";
                this.EntityPM.CustomerId = this.EntityPM.ShipperId;
                this.EntityPM.CustomerName = this.EntityPM.ShipperName;
                this.EntityPM.CustomerNote = this.EntityPM.ShipperNote;
                this.EntityPM.CustomerAddressId = this.EntityPM.ShipperAddressId;
                this.EntityPM.CustomerContactId = this.EntityPM.ShipperContactId;
                this.EntityPM.CustomerReference1 = this.EntityPM.ShipperReference1;
                this.EntityPM.CustomerReference2 = this.EntityPM.ShipperReference2;
            }
        }
        this.OnCustomerChanged();
    };
    PartnersTabComponent.prototype.OnCustomerChanged = function () {
        var _this = this;
        this.UpdateSalesmanId = null;
        this.UpdateSalesmanName = null;
        this.UpdateSalesmanAccountManagerText = null;
        this.UpdateAccountManagerId = null;
        this.UpdateAccountManagerName = null;
        this.IsUpdateSalesmanAccountManagerVisible = false;
        this.SalesmanUpdated = false;
        this.AccountManagerUpdated = false;
        if (this.EntityPM.ShipmentLevelCode != "C") {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.CustomerId)) {
                this.CardListService.getSingle(this.EntityPM.CustomerId).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        var list = myResponse.Result;
                        if (list) {
                            if (list.SalesmanUserId != _this.EntityPM.SalesmanUserId) {
                                if (Tools_1.AppTool.IsNullOrEmpty(list.SalesmanUserId)) {
                                    _this.UpdateSalesmanId = null;
                                    _this.UpdateSalesmanName = null;
                                    _this.UpdateSalesmanAccountManagerText = "Customer changed, update the salesman to Empty ?";
                                    if (list.AccountManagerUserId != null ? list.AccountManagerUserId != _this.EntityPM.AccountManagerUserId : SessionLocator_1.SessionLocator.LoggedUserId != _this.EntityPM.AccountManagerUserId) {
                                        if (!Tools_1.AppTool.IsNullOrEmpty(list.AccountManagerUserId)) {
                                            _this.UpdateAccountManagerId = list.AccountManagerUserId;
                                            _this.UpdateAccountManagerName = list.AccountManagerUserName;
                                            _this.UpdateSalesmanAccountManagerText = "Customer changed, update the salesman to Empty , update the account manager to " + list.AccountManagerUserName + " ?";
                                        }
                                        else {
                                            _this.UpdateAccountManagerId = SessionLocator_1.SessionLocator.LoggedUserId;
                                            _this.UpdateAccountManagerName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
                                            _this.UpdateSalesmanAccountManagerText = "Customer changed, update the salesman to Empty , update the account manager to " + _this.UpdateAccountManagerName + " ?";
                                        }
                                        _this.AccountManagerUpdated = true;
                                    }
                                }
                                else {
                                    _this.UpdateSalesmanId = list.SalesmanUserId;
                                    _this.UpdateSalesmanName = list.SalesmanUserEnglishName;
                                    _this.UpdateSalesmanAccountManagerText = "Customer changed, update the salesman to " + list.SalesmanUserEnglishName + " ?";
                                    if (list.AccountManagerUserId != null ? list.AccountManagerUserId != _this.EntityPM.AccountManagerUserId : SessionLocator_1.SessionLocator.LoggedUserId != _this.EntityPM.AccountManagerUserId) {
                                        if (!Tools_1.AppTool.IsNullOrEmpty(list.AccountManagerUserId)) {
                                            _this.UpdateAccountManagerId = list.AccountManagerUserId;
                                            _this.UpdateAccountManagerName = list.AccountManagerUserName;
                                            _this.UpdateSalesmanAccountManagerText = "Customer changed, update the salesman to " + list.SalesmanUserEnglishName + ", update the account manager to " + list.AccountManagerUserName + " ?";
                                        }
                                        else {
                                            _this.UpdateAccountManagerId = SessionLocator_1.SessionLocator.LoggedUserId;
                                            _this.UpdateAccountManagerName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
                                            _this.UpdateSalesmanAccountManagerText = "Customer changed, update the salesman to " + list.SalesmanUserEnglishName + ", update the account manager to " + _this.UpdateAccountManagerName + " ?";
                                        }
                                        _this.AccountManagerUpdated = true;
                                    }
                                }
                                _this.IsUpdateSalesmanAccountManagerVisible = true;
                                _this.SalesmanUpdated = true;
                            }
                            if ((list.AccountManagerUserId != null ? list.AccountManagerUserId != _this.EntityPM.AccountManagerUserId : SessionLocator_1.SessionLocator.LoggedUserId != _this.EntityPM.AccountManagerUserId) && !_this.IsUpdateSalesmanAccountManagerVisible) {
                                if (!Tools_1.AppTool.IsNullOrEmpty(list.AccountManagerUserId)) {
                                    _this.UpdateAccountManagerId = list.AccountManagerUserId;
                                    _this.UpdateAccountManagerName = list.AccountManagerUserName;
                                    _this.UpdateSalesmanAccountManagerText = "Customer changed, update the account manager to " + _this.UpdateAccountManagerName + " ?";
                                }
                                else {
                                    _this.UpdateAccountManagerId = SessionLocator_1.SessionLocator.LoggedUserId;
                                    _this.UpdateAccountManagerName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
                                    _this.UpdateSalesmanAccountManagerText = "Customer changed, update the account manager to " + _this.UpdateAccountManagerName + " ?";
                                }
                                _this.IsUpdateSalesmanAccountManagerVisible = true;
                                _this.AccountManagerUpdated = true;
                            }
                        }
                    }
                });
            }
        }
    };
    PartnersTabComponent.prototype.UpdateSalesmanClicked = function () {
        if (this.SalesmanUpdated) {
            this.EntityPM.SalesmanUserId = this.UpdateSalesmanId;
            this.EntityPM.SalesmanUserName = this.UpdateSalesmanName;
        }
        if (this.AccountManagerUpdated) {
            this.EntityPM.AccountManagerUserId = this.UpdateAccountManagerId;
            this.EntityPM.AccountManagerUserName = this.UpdateAccountManagerName;
        }
        this.IsUpdateSalesmanAccountManagerVisible = false;
    };
    PartnersTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './PartnersTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], PartnersTabComponent);
    return PartnersTabComponent;
}());
exports.PartnersTabComponent = PartnersTabComponent;
var PartnerItem = /** @class */ (function (_super) {
    __extends(PartnerItem, _super);
    function PartnerItem(fatherComponent, typeCode) {
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.ObjectTableName = "Shipment";
        _this.IsEditingEnabled = true;
        _this.IsRemoveButtonVisible = false;
        // PartnerId
        _this.PartnerCardList = null;
        _this.isPartnerChanged_Issuing = false;
        _this.IsReseting = false;
        _this.isAddressLoaded = false;
        _this.isContactLoaded = false;
        _this.ShowNoTemplateText = false;
        _this.AddressCityText = null;
        // Add|Edit Partner
        _this.isEditButtonClicked = false;
        _this.EntityPM = fatherComponent.EntityPM;
        _this.IsEditingEnabled = fatherComponent.IsEditingEnabled;
        _this.Code = typeCode;
        _this.InitializeProperties();
        _this.SetRemoveButtonVisibility();
        _this.GetPartnerAddress();
        _this.GetPartnerContact();
        return _this;
    }
    PartnerItem.prototype.SetUIProperties = function () {
        var isPartnerFilled = this.PartnerId == null ? false : true;
        var isFieldsEnabled = false;
        if (this.IsEditingEnabled) {
            if (isPartnerFilled) {
                isFieldsEnabled = true;
            }
        }
        this.UIProperties.SetRequired(this.PartnerIdProperty, this.ObjectTableName, !isPartnerFilled);
        this.UIProperties.SetEnabled(this.PartnerIdProperty, this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled(this.PartnerAddressIdProperty, this.ObjectTableName, isFieldsEnabled);
        this.UIProperties.SetEnabled(this.PartnerContactIdProperty, this.ObjectTableName, isFieldsEnabled);
        this.UIProperties.SetEnabled(this.Reference1Property, this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled(this.Reference2Property, this.ObjectTableName, this.IsEditingEnabled);
    };
    PartnerItem.prototype.InitializeProperties = function () {
        switch (this.Code) {
            case "SHIPR": {
                this.PartnerIndex = 0;
                this.FullCode = "Shipper";
                break;
            }
            case "CONSI": {
                this.PartnerIndex = 1;
                this.FullCode = "Consignee";
                break;
            }
            case "CSTMR": {
                this.PartnerIndex = 2;
                this.FullCode = "Customer";
                break;
            }
            case "AGENT": {
                this.PartnerIndex = 3;
                this.FullCode = "Agent";
                break;
            }
            case "ISSAG": {
                this.PartnerIndex = 4;
                this.FullCode = "IssuingCarrierAgent";
                break;
            }
            case "CSAEX": {
                this.PartnerIndex = 5;
                this.FullCode = "CustomAgentExport";
                break;
            }
            case "CSAIM": {
                this.PartnerIndex = 6;
                this.FullCode = "CustomAgentImport";
                break;
            }
            case "NOTF1": {
                this.PartnerIndex = 7;
                this.FullCode = "Notify1";
                break;
            }
            case "NOTF2": {
                this.PartnerIndex = 8;
                this.FullCode = "Notify2";
                break;
            }
            case "SHPNT": {
                this.PartnerIndex = 9;
                this.FullCode = "ShipperNotExporter";
                break;
            }
            case "CONNT": {
                this.PartnerIndex = 10;
                this.FullCode = "ConsigneeNotImporter";
                break;
            }
            case "FRTFR": {
                this.PartnerIndex = 11;
                this.FullCode = "FreightForwarder";
                break;
            }
            case "COLOD": {
                this.PartnerIndex = 12;
                this.FullCode = "Coloader";
                break;
            }
            case "CLERN": {
                this.PartnerIndex = 13;
                this.FullCode = "CustomClearancePoint";
                break;
            }
            case "CONSL": {
                this.PartnerIndex = 14;
                this.FullCode = "Consolidator";
                break;
            }
            case "REAGT": {
                this.PartnerIndex = 15;
                this.FullCode = "ReleasingAgent";
                break;
            }
        }
        this.PartnerTypeName = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F." + this.FullCode + "Id");
    };
    Object.defineProperty(PartnerItem.prototype, "CardDependencyProperty1", {
        get: function () {
            var myResult = null;
            switch (this.Code) {
                case "SHIPR":
                case "CONSI":
                case "CSTMR":
                    {
                        myResult = "CS";
                        if (this.EntityPM.ShipmentLevelCode == "C") {
                            myResult = "AG";
                        }
                        else {
                            if (SessionLocator_1.SessionLocator.TenantPM.AllowAgentInCustomersLOV) {
                                myResult = "CS,AG";
                            }
                        }
                        break;
                    }
                case "AGENT":
                case "ISSAG":
                case "FRTFR":
                case "COLOD":
                    {
                        myResult = "AG";
                        break;
                    }
                case "CSAEX":
                case "CSAIM":
                    {
                        myResult = "CG";
                        break;
                    }
                case "REAGT": //ReleasingAgent
                case "NOTF1":
                case "NOTF2":
                    {
                        myResult = "AG,AL,CG,CS,SG,SL,TR,VD,WH";
                        break;
                    }
                case "SHPNT":
                case "CONNT":
                case "CONSL":
                    {
                        myResult = "AG,CS";
                        break;
                    }
                case "CLERN":
                    {
                        myResult = "WH";
                        break;
                    }
                default: {
                    myResult = "CS";
                    break;
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "CardDependencyProperty1IsList", {
        get: function () {
            var myResult = false;
            switch (this.Code) {
                case "SHIPR":
                case "CONSI":
                case "CSTMR":
                    {
                        if (this.EntityPM.ShipmentLevelCode != "C") {
                            if (SessionLocator_1.SessionLocator.TenantPM.AllowAgentInCustomersLOV) {
                                myResult = true;
                            }
                        }
                        break;
                    }
                case "REAGT":
                case "NOTF1":
                case "NOTF2":
                case "SHPNT":
                case "CONNT":
                case "CONSL":
                    {
                        myResult = true;
                        break;
                    }
                default: {
                    myResult = false;
                    break;
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "Name", {
        get: function () {
            switch (this.Code) {
                case "SHIPR": {
                    return this.EntityPM.ShipperName;
                }
                case "CONSI": {
                    return this.EntityPM.ConsigneeName;
                }
                case "AGENT": {
                    return this.EntityPM.AgentName;
                }
                case "CSTMR": {
                    return this.EntityPM.CustomerName;
                }
                case "ISSAG": {
                    return this.EntityPM.IssuingCarrierAgentName;
                }
                case "CSAEX": {
                    return this.EntityPM.CustomAgentExportName;
                }
                case "CSAIM": {
                    return this.EntityPM.CustomAgentImportName;
                }
                case "NOTF1": {
                    return this.EntityPM.Notify1Name;
                }
                case "NOTF2": {
                    return this.EntityPM.Notify2Name;
                }
                case "SHPNT": {
                    return this.EntityPM.ShipperNotExporterName;
                }
                case "CONNT": {
                    return this.EntityPM.ConsigneeNotImporterName;
                }
                case "FRTFR": {
                    return this.EntityPM.FreightForwarderName;
                }
                case "COLOD": {
                    return this.EntityPM.ColoaderName;
                }
                case "CLERN": {
                    return this.EntityPM.CustomClearancePointName;
                }
                case "CONSL": {
                    return this.EntityPM.ConsolidatorName;
                }
                case "REAGT": {
                    return this.EntityPM.ReleasingAgentName;
                }
                default: {
                    return null;
                }
            }
        },
        set: function (newValue) {
            switch (this.Code) {
                case "SHIPR": {
                    if (this.EntityPM.ShipperName != newValue) {
                        this.EntityPM.ShipperName = newValue;
                    }
                    break;
                }
                case "CONSI": {
                    if (this.EntityPM.ConsigneeName != newValue) {
                        this.EntityPM.ConsigneeName = newValue;
                    }
                    break;
                }
                case "AGENT": {
                    if (this.EntityPM.AgentName != newValue) {
                        this.EntityPM.AgentName = newValue;
                    }
                    break;
                }
                case "CSTMR": {
                    if (this.EntityPM.CustomerName != newValue) {
                        this.EntityPM.CustomerName = newValue;
                    }
                    break;
                }
                case "ISSAG": {
                    if (this.EntityPM.IssuingCarrierAgentName != newValue) {
                        this.EntityPM.IssuingCarrierAgentName = newValue;
                    }
                    break;
                }
                case "CSAEX": {
                    if (this.EntityPM.CustomAgentExportName != newValue) {
                        this.EntityPM.CustomAgentExportName = newValue;
                        break;
                    }
                }
                case "CSAIM": {
                    if (this.EntityPM.CustomAgentImportName != newValue) {
                        this.EntityPM.CustomAgentImportName = newValue;
                    }
                    break;
                }
                case "NOTF1": {
                    if (this.EntityPM.Notify1Name != newValue) {
                        this.EntityPM.Notify1Name = newValue;
                        break;
                    }
                }
                case "NOTF2": {
                    if (this.EntityPM.Notify2Name != newValue) {
                        this.EntityPM.Notify2Name = newValue;
                    }
                    break;
                }
                case "SHPNT": {
                    if (this.EntityPM.ShipperNotExporterName != newValue) {
                        this.EntityPM.ShipperNotExporterName = newValue;
                    }
                    break;
                }
                case "CONNT": {
                    if (this.EntityPM.ConsigneeNotImporterName != newValue) {
                        this.EntityPM.ConsigneeNotImporterName = newValue;
                    }
                    break;
                }
                case "FRTFR": {
                    if (this.EntityPM.FreightForwarderName != newValue) {
                        this.EntityPM.FreightForwarderName = newValue;
                    }
                    break;
                }
                case "COLOD": {
                    if (this.EntityPM.ColoaderName != newValue) {
                        this.EntityPM.ColoaderName = newValue;
                    }
                    break;
                }
                case "CLERN": {
                    if (this.EntityPM.CustomClearancePointName != newValue) {
                        this.EntityPM.CustomClearancePointName = newValue;
                    }
                    break;
                }
                case "CONSL": {
                    if (this.EntityPM.ConsolidatorName != newValue) {
                        this.EntityPM.ConsolidatorName = newValue;
                    }
                    break;
                }
                case "REAGT": {
                    if (this.EntityPM.ReleasingAgentName != newValue) {
                        this.EntityPM.ReleasingAgentName = newValue;
                    }
                    break;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "Note", {
        get: function () {
            switch (this.Code) {
                case "SHIPR": {
                    return this.EntityPM.ShipperNote;
                }
                case "CONSI": {
                    return this.EntityPM.ConsigneeNote;
                }
                case "AGENT": {
                    return this.EntityPM.AgentNote;
                }
                case "CSTMR": {
                    return this.EntityPM.CustomerNote;
                }
                case "ISSAG": {
                    return this.EntityPM.IssuingCarrierAgentNote;
                }
                case "CSAEX": {
                    return this.EntityPM.CustomAgentExportNote;
                }
                case "CSAIM": {
                    return this.EntityPM.CustomAgentImportNote;
                }
                case "NOTF1": {
                    return this.EntityPM.Notify1Note;
                }
                case "NOTF2": {
                    return this.EntityPM.Notify2Note;
                }
                case "SHPNT": {
                    return this.EntityPM.ShipperNotExporterNote;
                }
                case "CONNT": {
                    return this.EntityPM.ConsigneeNotImporterNote;
                }
                case "FRTFR": {
                    return this.EntityPM.FreightForwarderNote;
                }
                case "COLOD": {
                    return this.EntityPM.ColoaderNote;
                }
                case "CLERN": {
                    return this.EntityPM.CustomClearancePointNote;
                }
                case "CONSL": {
                    return this.EntityPM.ConsolidatorNote;
                }
                case "REAGT": {
                    return this.EntityPM.ReleasingAgentNote;
                }
                default: {
                    return null;
                }
            }
        },
        set: function (newValue) {
            switch (this.Code) {
                case "SHIPR": {
                    if (this.EntityPM.ShipperNote != newValue) {
                        this.EntityPM.ShipperNote = newValue;
                    }
                    break;
                }
                case "CONSI": {
                    if (this.EntityPM.ConsigneeNote != newValue) {
                        this.EntityPM.ConsigneeNote = newValue;
                    }
                    break;
                }
                case "AGENT": {
                    if (this.EntityPM.AgentNote != newValue) {
                        this.EntityPM.AgentNote = newValue;
                    }
                    break;
                }
                case "CSTMR": {
                    if (this.EntityPM.CustomerNote != newValue) {
                        this.EntityPM.CustomerNote = newValue;
                    }
                    break;
                }
                case "ISSAG": {
                    if (this.EntityPM.IssuingCarrierAgentNote != newValue) {
                        this.EntityPM.IssuingCarrierAgentNote = newValue;
                    }
                    break;
                }
                case "CSAEX": {
                    if (this.EntityPM.CustomAgentExportNote != newValue) {
                        this.EntityPM.CustomAgentExportNote = newValue;
                    }
                    break;
                }
                case "CSAIM": {
                    if (this.EntityPM.CustomAgentImportNote != newValue) {
                        this.EntityPM.CustomAgentImportNote = newValue;
                    }
                    break;
                }
                case "NOTF1": {
                    if (this.EntityPM.Notify1Note != newValue) {
                        this.EntityPM.Notify1Note = newValue;
                    }
                    break;
                }
                case "NOTF2": {
                    if (this.EntityPM.Notify2Note != newValue) {
                        this.EntityPM.Notify2Note = newValue;
                    }
                    break;
                }
                case "SHPNT": {
                    if (this.EntityPM.ShipperNotExporterNote != newValue) {
                        this.EntityPM.ShipperNotExporterNote = newValue;
                    }
                    break;
                }
                case "CONNT": {
                    if (this.EntityPM.ConsigneeNotImporterNote != newValue) {
                        this.EntityPM.ConsigneeNotImporterNote = newValue;
                    }
                    break;
                }
                case "FRTFR": {
                    if (this.EntityPM.FreightForwarderNote != newValue) {
                        this.EntityPM.FreightForwarderNote = newValue;
                    }
                    break;
                }
                case "COLOD": {
                    if (this.EntityPM.ColoaderNote != newValue) {
                        this.EntityPM.ColoaderNote = newValue;
                    }
                    break;
                }
                case "CLERN": {
                    if (this.EntityPM.CustomClearancePointNote != newValue) {
                        this.EntityPM.CustomClearancePointNote = newValue;
                    }
                    break;
                }
                case "CONSL": {
                    if (this.EntityPM.ConsolidatorNote != newValue) {
                        this.EntityPM.ConsolidatorNote = newValue;
                    }
                    break;
                }
                case "REAGT": {
                    if (this.EntityPM.ReleasingAgentNote != newValue) {
                        this.EntityPM.ReleasingAgentNote = newValue;
                    }
                    break;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "IsCustomer", {
        get: function () {
            return (this.PartnerId == this.EntityPM.CustomerId) ? true : false;
        },
        enumerable: true,
        configurable: true
    });
    PartnerItem.prototype.SetAsCustomer = function () {
        //|| this.CardDependencyProperty1 != "CS"
        if (this.EntityPM.ShipmentLevelCode == "C") {
            this.EntityPM.CustomerName = null;
            this.EntityPM.CustomerNote = null;
            this.EntityPM.CustomerId = null;
            this.EntityPM.CustomerAddressId = null;
            this.EntityPM.CustomerContactId = null;
            this.EntityPM.CustomerReference1 = null;
            this.EntityPM.CustomerReference2 = null;
            this.EntityPM.CustomerRankName = null;
            this.EntityPM.CustomerShipmentNumber = null;
            this.EntityPM.ShipmentCustomerTypeCode = null;
        }
        else {
            this.EntityPM.CustomerName = this.Name;
            this.EntityPM.CustomerNote = this.Note;
            this.EntityPM.CustomerId = this.PartnerId;
            this.EntityPM.CustomerAddressId = this.AddressId;
            this.EntityPM.CustomerContactId = this.ContactId;
            this.EntityPM.CustomerReference1 = this.Reference1;
            this.EntityPM.CustomerReference2 = this.Reference2;
            this.EntityPM.CustomerShipmentNumber = this.EntityPM.ShipmentNumber;
            switch (this.Code) {
                case "SHIPR": {
                    this.EntityPM.ShipmentCustomerTypeCode = "SHI";
                    break;
                }
                case "CONSI": {
                    this.EntityPM.ShipmentCustomerTypeCode = "CON";
                    break;
                }
                case "AGENT": {
                    this.EntityPM.ShipmentCustomerTypeCode = "AGT";
                    break;
                }
                case "ISSAG": {
                    this.EntityPM.ShipmentCustomerTypeCode = "IGT";
                    break;
                }
                case "CSAEX": {
                    this.EntityPM.ShipmentCustomerTypeCode = "CAE";
                    break;
                }
                case "CSAIM": {
                    this.EntityPM.ShipmentCustomerTypeCode = "CAI";
                    break;
                }
                case "NOTF1": {
                    this.EntityPM.ShipmentCustomerTypeCode = "NT1";
                    break;
                }
                case "NOTF2": {
                    this.EntityPM.ShipmentCustomerTypeCode = "NT2";
                    break;
                }
                case "SHPNT": {
                    this.EntityPM.ShipmentCustomerTypeCode = "SNE";
                    break;
                }
                case "CONNT": {
                    this.EntityPM.ShipmentCustomerTypeCode = "CNI";
                    break;
                }
                case "FRTFR": {
                    this.EntityPM.ShipmentCustomerTypeCode = "FOR";
                    break;
                }
                case "COLOD": {
                    this.EntityPM.ShipmentCustomerTypeCode = "COL";
                    break;
                }
                case "CLERN": {
                    this.EntityPM.ShipmentCustomerTypeCode = "CCP";
                    break;
                }
                case "CONSL": {
                    this.EntityPM.ShipmentCustomerTypeCode = "CSD";
                    break;
                }
                case "REAGT": {
                    this.EntityPM.ShipmentCustomerTypeCode = "REA";
                    break;
                }
                default: {
                    this.EntityPM.ShipmentCustomerTypeCode = "OTH";
                    break;
                }
            }
        }
        this.fatherComponent.OnCustomerChanged();
    };
    PartnerItem.prototype.SetRemoveButtonVisibility = function () {
        switch (this.Code) {
            case "SHIPR": {
                this.IsRemoveButtonVisible = (this.EntityPM.DirectionId == "I") ? true : false;
                break;
            }
            case "CONSI": {
                this.IsRemoveButtonVisible = (this.EntityPM.DirectionId == "E") ? true : false;
                break;
            }
            default: {
                this.IsRemoveButtonVisible = true;
                break;
            }
        }
    };
    Object.defineProperty(PartnerItem.prototype, "PartnerIdProperty", {
        get: function () {
            switch (this.Code) {
                case "SHIPR": {
                    return "ShipperId";
                }
                case "CONSI": {
                    return "ConsigneeId";
                }
                case "AGENT": {
                    return "AgentId";
                }
                case "CSTMR": {
                    return "CustomerId";
                }
                case "ISSAG": {
                    return "IssuingCarrierAgentId";
                }
                case "CSAEX": {
                    return "CustomAgentExportId";
                }
                case "CSAIM": {
                    return "CustomAgentImportId";
                }
                case "NOTF1": {
                    return "Notify1Id";
                }
                case "NOTF2": {
                    return "Notify2Id";
                }
                case "SHPNT": {
                    return "ShipperNotExporterId";
                }
                case "CONNT": {
                    return "ConsigneeNotImporterId";
                }
                case "FRTFR": {
                    return "FreightForwarderId";
                }
                case "COLOD": {
                    return "ColoaderId";
                }
                case "CLERN": {
                    return "CustomClearancePointId";
                }
                case "CONSL": {
                    return "ConsolidatorId";
                }
                case "REAGT": {
                    return "ReleasingAgentId";
                }
                default: {
                    return null;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "PartnerId", {
        get: function () {
            switch (this.Code) {
                case "SHIPR": {
                    return this.ShipperId;
                }
                case "CONSI": {
                    return this.ConsigneeId;
                }
                case "AGENT": {
                    return this.AgentId;
                }
                case "CSTMR": {
                    return this.CustomerId;
                }
                case "ISSAG": {
                    return this.IssuingCarrierAgentId;
                }
                case "CSAEX": {
                    return this.CustomAgentExportId;
                }
                case "CSAIM": {
                    return this.CustomAgentImportId;
                }
                case "NOTF1": {
                    return this.Notify1Id;
                }
                case "NOTF2": {
                    return this.Notify2Id;
                }
                case "SHPNT": {
                    return this.ShipperNotExporterId;
                }
                case "CONNT": {
                    return this.ConsigneeNotImporterId;
                }
                case "FRTFR": {
                    return this.FreightForwarderId;
                }
                case "COLOD": {
                    return this.ColoaderId;
                }
                case "CLERN": {
                    return this.CustomClearancePointId;
                }
                case "CONSL": {
                    return this.ConsolidatorId;
                }
                case "REAGT": {
                    return this.ReleasingAgentId;
                }
                default: {
                    return null;
                }
            }
        },
        set: function (newValue) {
            switch (this.Code) {
                case "SHIPR": {
                    this.ShipperId = newValue;
                    break;
                }
                case "CONSI": {
                    this.ConsigneeId = newValue;
                    break;
                }
                case "AGENT": {
                    this.AgentId = newValue;
                    break;
                }
                case "CSTMR": {
                    this.CustomerId = newValue;
                    break;
                }
                case "ISSAG": {
                    this.IssuingCarrierAgentId = newValue;
                    break;
                }
                case "CSAEX": {
                    this.CustomAgentExportId = newValue;
                    break;
                }
                case "CSAIM": {
                    this.CustomAgentImportId = newValue;
                    break;
                }
                case "NOTF1": {
                    this.Notify1Id = newValue;
                    break;
                }
                case "NOTF2": {
                    this.Notify2Id = newValue;
                    break;
                }
                case "SHPNT": {
                    this.ShipperNotExporterId = newValue;
                    break;
                }
                case "CONNT": {
                    this.ConsigneeNotImporterId = newValue;
                    break;
                }
                case "FRTFR": {
                    this.FreightForwarderId = newValue;
                    break;
                }
                case "COLOD": {
                    this.ColoaderId = newValue;
                    break;
                }
                case "CLERN": {
                    this.CustomClearancePointId = newValue;
                    break;
                }
                case "CONSL": {
                    this.ConsolidatorId = newValue;
                    break;
                }
                case "REAGT": {
                    this.ReleasingAgentId = newValue;
                    break;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "ShipperId", {
        get: function () {
            return this.EntityPM.ShipperId;
        },
        set: function (newValue) {
            if (this.EntityPM.ShipperId != newValue) {
                this.EntityPM.ShipperId = newValue;
                this.GetPartnerCard();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "ConsigneeId", {
        get: function () {
            return this.EntityPM.ConsigneeId;
        },
        set: function (newValue) {
            if (this.EntityPM.ConsigneeId != newValue) {
                this.EntityPM.ConsigneeId = newValue;
                this.GetPartnerCard();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "CustomerId", {
        get: function () {
            return this.EntityPM.CustomerId;
        },
        set: function (newValue) {
            if (this.EntityPM.CustomerId != newValue) {
                this.EntityPM.CustomerId = newValue;
                this.GetPartnerCard();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "AgentId", {
        get: function () {
            return this.EntityPM.AgentId;
        },
        set: function (newValue) {
            if (this.EntityPM.AgentId != newValue) {
                this.EntityPM.AgentId = newValue;
                this.GetPartnerCard();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "IssuingCarrierAgentId", {
        get: function () {
            return this.EntityPM.IssuingCarrierAgentId;
        },
        set: function (newValue) {
            if (this.EntityPM.IssuingCarrierAgentId != newValue) {
                this.EntityPM.IssuingCarrierAgentId = newValue;
                this.isPartnerChanged_Issuing = true;
                this.GetPartnerCard();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "CustomAgentExportId", {
        get: function () {
            return this.EntityPM.CustomAgentExportId;
        },
        set: function (newValue) {
            if (this.EntityPM.CustomAgentExportId != newValue) {
                this.EntityPM.CustomAgentExportId = newValue;
                this.GetPartnerCard();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "CustomAgentImportId", {
        get: function () {
            return this.EntityPM.CustomAgentImportId;
        },
        set: function (newValue) {
            if (this.EntityPM.CustomAgentImportId != newValue) {
                this.EntityPM.CustomAgentImportId = newValue;
                this.GetPartnerCard();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "Notify1Id", {
        get: function () {
            return this.EntityPM.Notify1Id;
        },
        set: function (newValue) {
            if (this.EntityPM.Notify1Id != newValue) {
                this.EntityPM.Notify1Id = newValue;
                this.GetPartnerCard();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "Notify2Id", {
        get: function () {
            return this.EntityPM.Notify2Id;
        },
        set: function (newValue) {
            if (this.EntityPM.Notify2Id != newValue) {
                this.EntityPM.Notify2Id = newValue;
                this.GetPartnerCard();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "ShipperNotExporterId", {
        get: function () {
            return this.EntityPM.ShipperNotExporterId;
        },
        set: function (newValue) {
            if (this.EntityPM.ShipperNotExporterId != newValue) {
                this.EntityPM.ShipperNotExporterId = newValue;
                this.GetPartnerCard();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "ConsigneeNotImporterId", {
        get: function () {
            return this.EntityPM.ConsigneeNotImporterId;
        },
        set: function (newValue) {
            if (this.EntityPM.ConsigneeNotImporterId != newValue) {
                this.EntityPM.ConsigneeNotImporterId = newValue;
                this.GetPartnerCard();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "FreightForwarderId", {
        get: function () {
            return this.EntityPM.FreightForwarderId;
        },
        set: function (newValue) {
            if (this.EntityPM.FreightForwarderId != newValue) {
                this.EntityPM.FreightForwarderId = newValue;
                this.GetPartnerCard();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "ColoaderId", {
        get: function () {
            return this.EntityPM.ColoaderId;
        },
        set: function (newValue) {
            if (this.EntityPM.ColoaderId != newValue) {
                this.EntityPM.ColoaderId = newValue;
                this.GetPartnerCard();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "CustomClearancePointId", {
        get: function () {
            return this.EntityPM.CustomClearancePointId;
        },
        set: function (newValue) {
            if (this.EntityPM.CustomClearancePointId != newValue) {
                this.EntityPM.CustomClearancePointId = newValue;
                this.GetPartnerCard();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "ConsolidatorId", {
        get: function () {
            return this.EntityPM.ConsolidatorId;
        },
        set: function (newValue) {
            if (this.EntityPM.ConsolidatorId != newValue) {
                this.EntityPM.ConsolidatorId = newValue;
                this.GetPartnerCard();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "ReleasingAgentId", {
        get: function () {
            return this.EntityPM.ReleasingAgentId;
        },
        set: function (newValue) {
            if (this.EntityPM.ReleasingAgentId != newValue) {
                this.EntityPM.ReleasingAgentId = newValue;
                this.GetPartnerCard();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "PartnerAddressIdProperty", {
        // AddressId
        get: function () {
            switch (this.Code) {
                case "SHIPR": {
                    return "ShipperAddressId";
                }
                case "CONSI": {
                    return "ConsigneeAddressId";
                }
                case "AGENT": {
                    return "AgentAddressId";
                }
                case "CSTMR": {
                    return "CustomerAddressId";
                }
                case "ISSAG": {
                    return "IssuingCarrierAddressId";
                }
                case "CSAEX": {
                    return "CustomAgentExportAddressId";
                }
                case "CSAIM": {
                    return "CustomAgentImportAddressId";
                }
                case "NOTF1": {
                    return "Notify1AddressId";
                }
                case "NOTF2": {
                    return "Notify2AddressId";
                }
                case "SHPNT": {
                    return "ShipperNotExporterAddressId";
                }
                case "CONNT": {
                    return "ConsigneeNotImporterAddressId";
                }
                case "FRTFR": {
                    return "FreightForwarderAddressId";
                }
                case "COLOD": {
                    return "ColoaderAddressId";
                }
                case "CLERN": {
                    return "CustomClearancePointAddressId";
                }
                case "CONSL": {
                    return "ConsolidatorAddressId";
                }
                case "REAGT": {
                    return "ReleasingAgentAddressId";
                }
                default: {
                    return null;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "AddressId", {
        get: function () {
            switch (this.Code) {
                case "SHIPR": {
                    return this.ShipperAddressId;
                }
                case "CONSI": {
                    return this.ConsigneeAddressId;
                }
                case "AGENT": {
                    return this.AgentAddressId;
                }
                case "CSTMR": {
                    return this.CustomerAddressId;
                }
                case "ISSAG": {
                    return this.IssuingCarrierAddressId;
                }
                case "CSAEX": {
                    return this.CustomAgentExportAddressId;
                }
                case "CSAIM": {
                    return this.CustomAgentImportAddressId;
                }
                case "NOTF1": {
                    return this.Notify1AddressId;
                }
                case "NOTF2": {
                    return this.Notify2AddressId;
                }
                case "SHPNT": {
                    return this.ShipperNotExporterAddressId;
                }
                case "CONNT": {
                    return this.ConsigneeNotImporterAddressId;
                }
                case "FRTFR": {
                    return this.FreightForwarderAddressId;
                }
                case "COLOD": {
                    return this.ColoaderAddressId;
                }
                case "CLERN": {
                    return this.CustomClearancePointAddressId;
                }
                case "CONSL": {
                    return this.ConsolidatorAddressId;
                }
                case "REAGT": {
                    return this.ReleasingAgentAddressId;
                }
                default: {
                    return null;
                }
            }
        },
        set: function (newValue) {
            switch (this.Code) {
                case "SHIPR": {
                    this.ShipperAddressId = newValue;
                    break;
                }
                case "CONSI": {
                    this.ConsigneeAddressId = newValue;
                    break;
                }
                case "AGENT": {
                    this.AgentAddressId = newValue;
                    break;
                }
                case "CSTMR": {
                    this.CustomerAddressId = newValue;
                    break;
                }
                case "ISSAG": {
                    this.IssuingCarrierAddressId = newValue;
                    break;
                }
                case "CSAEX": {
                    this.CustomAgentExportAddressId = newValue;
                    break;
                }
                case "CSAIM": {
                    this.CustomAgentImportAddressId = newValue;
                    break;
                }
                case "NOTF1": {
                    this.Notify1AddressId = newValue;
                    break;
                }
                case "NOTF2": {
                    this.Notify2AddressId = newValue;
                    break;
                }
                case "SHPNT": {
                    this.ShipperNotExporterAddressId = newValue;
                    break;
                }
                case "CONNT": {
                    this.ConsigneeNotImporterAddressId = newValue;
                    break;
                }
                case "FRTFR": {
                    this.FreightForwarderAddressId = newValue;
                    break;
                }
                case "COLOD": {
                    this.ColoaderAddressId = newValue;
                    break;
                }
                case "CLERN": {
                    this.CustomClearancePointAddressId = newValue;
                    break;
                }
                case "CONSL": {
                    this.ConsolidatorAddressId = newValue;
                    break;
                }
                case "REAGT": {
                    this.ReleasingAgentAddressId = newValue;
                    break;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "ShipperAddressId", {
        get: function () {
            return this.EntityPM.ShipperAddressId;
        },
        set: function (newValue) {
            if (this.EntityPM.ShipperAddressId != newValue) {
                this.EntityPM.ShipperAddressId = newValue;
                this.GetPartnerAddress();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "ConsigneeAddressId", {
        get: function () {
            return this.EntityPM.ConsigneeAddressId;
        },
        set: function (newValue) {
            if (this.EntityPM.ConsigneeAddressId != newValue) {
                this.EntityPM.ConsigneeAddressId = newValue;
                this.GetPartnerAddress();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "AgentAddressId", {
        get: function () {
            return this.EntityPM.AgentAddressId;
        },
        set: function (newValue) {
            if (this.EntityPM.AgentAddressId != newValue) {
                this.EntityPM.AgentAddressId = newValue;
                this.GetPartnerAddress();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "CustomerAddressId", {
        get: function () {
            return this.EntityPM.CustomerAddressId;
        },
        set: function (newValue) {
            if (this.EntityPM.CustomerAddressId != newValue) {
                this.EntityPM.CustomerAddressId = newValue;
                this.GetPartnerAddress();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "IssuingCarrierAddressId", {
        get: function () {
            return this.EntityPM.IssuingCarrierAddressId;
        },
        set: function (newValue) {
            if (this.EntityPM.IssuingCarrierAddressId != newValue) {
                this.EntityPM.IssuingCarrierAddressId = newValue;
                this.GetPartnerAddress();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "CustomAgentExportAddressId", {
        get: function () {
            return this.EntityPM.CustomAgentExportAddressId;
        },
        set: function (newValue) {
            if (this.EntityPM.CustomAgentExportAddressId != newValue) {
                this.EntityPM.CustomAgentExportAddressId = newValue;
                this.GetPartnerAddress();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "CustomAgentImportAddressId", {
        get: function () {
            return this.EntityPM.CustomAgentImportAddressId;
        },
        set: function (newValue) {
            if (this.EntityPM.CustomAgentImportAddressId != newValue) {
                this.EntityPM.CustomAgentImportAddressId = newValue;
                this.GetPartnerAddress();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "Notify1AddressId", {
        get: function () {
            return this.EntityPM.Notify1AddressId;
        },
        set: function (newValue) {
            if (this.EntityPM.Notify1AddressId != newValue) {
                this.EntityPM.Notify1AddressId = newValue;
                this.GetPartnerAddress();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "Notify2AddressId", {
        get: function () {
            return this.EntityPM.Notify2AddressId;
        },
        set: function (newValue) {
            if (this.EntityPM.Notify2AddressId != newValue) {
                this.EntityPM.Notify2AddressId = newValue;
                this.GetPartnerAddress();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "ShipperNotExporterAddressId", {
        get: function () {
            return this.EntityPM.ShipperNotExporterAddressId;
        },
        set: function (newValue) {
            if (this.EntityPM.ShipperNotExporterAddressId != newValue) {
                this.EntityPM.ShipperNotExporterAddressId = newValue;
                this.GetPartnerAddress();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "ConsigneeNotImporterAddressId", {
        get: function () {
            return this.EntityPM.ConsigneeNotImporterAddressId;
        },
        set: function (newValue) {
            if (this.EntityPM.ConsigneeNotImporterAddressId != newValue) {
                this.EntityPM.ConsigneeNotImporterAddressId = newValue;
                this.GetPartnerAddress();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "FreightForwarderAddressId", {
        get: function () {
            return this.EntityPM.FreightForwarderAddressId;
        },
        set: function (newValue) {
            if (this.EntityPM.FreightForwarderAddressId != newValue) {
                this.EntityPM.FreightForwarderAddressId = newValue;
                this.GetPartnerAddress();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "ColoaderAddressId", {
        get: function () {
            return this.EntityPM.ColoaderAddressId;
        },
        set: function (newValue) {
            if (this.EntityPM.ColoaderAddressId != newValue) {
                this.EntityPM.ColoaderAddressId = newValue;
                this.GetPartnerAddress();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "CustomClearancePointAddressId", {
        get: function () {
            return this.EntityPM.CustomClearancePointAddressId;
        },
        set: function (newValue) {
            if (this.EntityPM.CustomClearancePointAddressId != newValue) {
                this.EntityPM.CustomClearancePointAddressId = newValue;
                this.GetPartnerAddress();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "ConsolidatorAddressId", {
        get: function () {
            return this.EntityPM.ConsolidatorAddressId;
        },
        set: function (newValue) {
            if (this.EntityPM.ConsolidatorAddressId != newValue) {
                this.EntityPM.ConsolidatorAddressId = newValue;
                this.GetPartnerAddress();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "ReleasingAgentAddressId", {
        get: function () {
            return this.EntityPM.ReleasingAgentAddressId;
        },
        set: function (newValue) {
            if (this.EntityPM.ReleasingAgentAddressId != newValue) {
                this.EntityPM.ReleasingAgentAddressId = newValue;
                this.GetPartnerAddress();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "PartnerContactIdProperty", {
        // ContactId
        get: function () {
            switch (this.Code) {
                case "SHIPR": {
                    return "ShipperContactId";
                }
                case "CONSI": {
                    return "ConsigneeContactId";
                }
                case "AGENT": {
                    return "AgentContactId";
                }
                case "CSTMR": {
                    return "CustomerContactId";
                }
                //case "ISSAG": { return "IssuingCarrierContactId"; }
                case "CSAEX": {
                    return "CustomAgentExportContactId";
                }
                case "CSAIM": {
                    return "CustomAgentImportContactId";
                }
                case "NOTF1": {
                    return "Notify1ContactId";
                }
                case "NOTF2": {
                    return "Notify2ContactId";
                }
                case "SHPNT": {
                    return "ShipperNotExporterContactId";
                }
                case "CONNT": {
                    return "ConsigneeNotImporterContactId";
                }
                case "FRTFR": {
                    return "FreightForwarderContactId";
                }
                case "COLOD": {
                    return "ColoaderContactId";
                }
                case "CLERN": {
                    return "CustomClearancePointContactId";
                }
                case "CONSL": {
                    return "ConsolidatorContactId";
                }
                case "REAGT": {
                    return "ReleasingAgentContactId";
                }
                default: {
                    return null;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "ContactId", {
        get: function () {
            switch (this.Code) {
                case "SHIPR": {
                    return this.ShipperContactId;
                }
                case "CONSI": {
                    return this.ConsigneeContactId;
                }
                case "AGENT": {
                    return this.AgentContactId;
                }
                case "CSTMR": {
                    return this.CustomerContactId;
                }
                //case "ISSAG": { return this.IssuingCarrierContactId; }
                case "CSAEX": {
                    return this.CustomAgentExportContactId;
                }
                case "CSAIM": {
                    return this.CustomAgentImportContactId;
                }
                case "NOTF1": {
                    return this.Notify1ContactId;
                }
                case "NOTF2": {
                    return this.Notify2ContactId;
                }
                case "SHPNT": {
                    return this.ShipperNotExporterContactId;
                }
                case "CONNT": {
                    return this.ConsigneeNotImporterContactId;
                }
                case "FRTFR": {
                    return this.FreightForwarderContactId;
                }
                case "COLOD": {
                    return this.ColoaderContactId;
                }
                case "CLERN": {
                    return this.CustomClearancePointContactId;
                }
                case "CONSL": {
                    return this.ConsolidatorContactId;
                }
                case "REAGT": {
                    return this.ReleasingAgentContactId;
                }
                default: {
                    return null;
                }
            }
        },
        set: function (newValue) {
            switch (this.Code) {
                case "SHIPR": {
                    this.ShipperContactId = newValue;
                    break;
                }
                case "CONSI": {
                    this.ConsigneeContactId = newValue;
                    break;
                }
                case "AGENT": {
                    this.AgentContactId = newValue;
                    break;
                }
                case "CSTMR": {
                    this.CustomerContactId = newValue;
                    break;
                }
                //case "ISSAG": { this.IssuingCarrierContactId = newValue; break; }
                case "CSAEX": {
                    this.CustomAgentExportContactId = newValue;
                    break;
                }
                case "CSAIM": {
                    this.CustomAgentImportContactId = newValue;
                    break;
                }
                case "NOTF1": {
                    this.Notify1ContactId = newValue;
                    break;
                }
                case "NOTF2": {
                    this.Notify2ContactId = newValue;
                    break;
                }
                case "SHPNT": {
                    this.ShipperNotExporterContactId = newValue;
                    break;
                }
                case "CONNT": {
                    this.ConsigneeNotImporterContactId = newValue;
                    break;
                }
                case "FRTFR": {
                    this.FreightForwarderContactId = newValue;
                    break;
                }
                case "COLOD": {
                    this.ColoaderContactId = newValue;
                    break;
                }
                case "CLERN": {
                    this.CustomClearancePointContactId = newValue;
                    break;
                }
                case "CONSL": {
                    this.ConsolidatorContactId = newValue;
                    break;
                }
                case "REAGT": {
                    this.ReleasingAgentContactId = newValue;
                    break;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "ShipperContactId", {
        get: function () {
            return this.EntityPM.ShipperContactId;
        },
        set: function (newValue) {
            if (this.EntityPM.ShipperContactId != newValue) {
                this.EntityPM.ShipperContactId = newValue;
                this.GetPartnerContact();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "ConsigneeContactId", {
        get: function () {
            return this.EntityPM.ConsigneeContactId;
        },
        set: function (newValue) {
            if (this.EntityPM.ConsigneeContactId != newValue) {
                this.EntityPM.ConsigneeContactId = newValue;
                this.GetPartnerContact();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "AgentContactId", {
        get: function () {
            return this.EntityPM.AgentContactId;
        },
        set: function (newValue) {
            if (this.EntityPM.AgentContactId != newValue) {
                this.EntityPM.AgentContactId = newValue;
                this.GetPartnerContact();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "CustomerContactId", {
        get: function () {
            return this.EntityPM.CustomerContactId;
        },
        set: function (newValue) {
            if (this.EntityPM.CustomerContactId != newValue) {
                this.EntityPM.CustomerContactId = newValue;
                this.GetPartnerContact();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "CustomAgentExportContactId", {
        get: function () {
            return this.EntityPM.CustomAgentExportContactId;
        },
        set: function (newValue) {
            if (this.EntityPM.CustomAgentExportContactId != newValue) {
                this.EntityPM.CustomAgentExportContactId = newValue;
                this.GetPartnerContact();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "CustomAgentImportContactId", {
        get: function () {
            return this.EntityPM.CustomAgentImportContactId;
        },
        set: function (newValue) {
            if (this.EntityPM.CustomAgentImportContactId != newValue) {
                this.EntityPM.CustomAgentImportContactId = newValue;
                this.GetPartnerContact();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "Notify1ContactId", {
        get: function () {
            return this.EntityPM.Notify1ContactId;
        },
        set: function (newValue) {
            if (this.EntityPM.Notify1ContactId != newValue) {
                this.EntityPM.Notify1ContactId = newValue;
                this.GetPartnerContact();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "Notify2ContactId", {
        get: function () {
            return this.EntityPM.Notify2ContactId;
        },
        set: function (newValue) {
            if (this.EntityPM.Notify2ContactId != newValue) {
                this.EntityPM.Notify2ContactId = newValue;
                this.GetPartnerContact();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "ShipperNotExporterContactId", {
        get: function () {
            return this.EntityPM.ShipperNotExporterContactId;
        },
        set: function (newValue) {
            if (this.EntityPM.ShipperNotExporterContactId != newValue) {
                this.EntityPM.ShipperNotExporterContactId = newValue;
                this.GetPartnerContact();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "ConsigneeNotImporterContactId", {
        get: function () {
            return this.EntityPM.ConsigneeNotImporterContactId;
        },
        set: function (newValue) {
            if (this.EntityPM.ConsigneeNotImporterContactId != newValue) {
                this.EntityPM.ConsigneeNotImporterContactId = newValue;
                this.GetPartnerContact();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "FreightForwarderContactId", {
        get: function () {
            return this.EntityPM.FreightForwarderContactId;
        },
        set: function (newValue) {
            if (this.EntityPM.FreightForwarderContactId != newValue) {
                this.EntityPM.FreightForwarderContactId = newValue;
                this.GetPartnerContact();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "ColoaderContactId", {
        get: function () {
            return this.EntityPM.ColoaderContactId;
        },
        set: function (newValue) {
            if (this.EntityPM.ColoaderContactId != newValue) {
                this.EntityPM.ColoaderContactId = newValue;
                this.GetPartnerContact();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "CustomClearancePointContactId", {
        get: function () {
            return this.EntityPM.CustomClearancePointContactId;
        },
        set: function (newValue) {
            if (this.EntityPM.CustomClearancePointContactId != newValue) {
                this.EntityPM.CustomClearancePointContactId = newValue;
                this.GetPartnerContact();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "ConsolidatorContactId", {
        get: function () {
            return this.EntityPM.ConsolidatorContactId;
        },
        set: function (newValue) {
            if (this.EntityPM.ConsolidatorContactId != newValue) {
                this.EntityPM.ConsolidatorContactId = newValue;
                this.GetPartnerContact();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "ReleasingAgentContactId", {
        get: function () {
            return this.EntityPM.ReleasingAgentContactId;
        },
        set: function (newValue) {
            if (this.EntityPM.ReleasingAgentContactId != newValue) {
                this.EntityPM.ReleasingAgentContactId = newValue;
                this.GetPartnerContact();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "HasReference1", {
        // Reference1
        get: function () {
            var myResult = false;
            switch (this.Code) {
                case "SHIPR":
                case "CONSI":
                case "AGENT":
                case "CSTMR":
                case "ISSAG":
                case "CSAEX":
                case "CSAIM":
                case "FRTFR":
                case "COLOD":
                case "CLERN":
                case "REAGT":
                case "CONSL":
                case "NOTF1":
                case "NOTF2":
                case "SHPNT":
                case "CONNT":
                    {
                        myResult = true;
                    }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "Reference1Property", {
        get: function () {
            switch (this.Code) {
                case "SHIPR": {
                    return "ShipperReference1";
                }
                case "CONSI": {
                    return "ConsigneeReference1";
                }
                case "AGENT": {
                    return "AgentReference1";
                }
                case "CSTMR": {
                    return "CustomerReference1";
                }
                case "ISSAG": {
                    return "IssuingCarrierReference1";
                }
                case "CSAEX": {
                    return "CustomAgentExportReference";
                }
                case "CSAIM": {
                    return "CustomAgentImportReference";
                }
                case "FRTFR": {
                    return "FreightForwarderReference";
                }
                case "COLOD": {
                    return "ColoaderReference1";
                }
                case "CLERN": {
                    return "CustomClearancePointReference1";
                }
                case "CONSL": {
                    return "ConsolidatorReference";
                }
                case "REAGT": {
                    return "ReleasingAgentReference1";
                }
                case "NOTF1": {
                    return "Notify1Reference";
                }
                case "NOTF2": {
                    return "Notify2Reference";
                }
                case "SHPNT": {
                    return "ShipperNotExporterReference";
                }
                case "CONNT": {
                    return "ConsigneeNotImporterReference";
                }
                default: {
                    return null;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "Reference1", {
        get: function () {
            switch (this.Code) {
                case "SHIPR": {
                    return this.ShipperReference1;
                }
                case "CONSI": {
                    return this.ConsigneeReference1;
                }
                case "AGENT": {
                    return this.AgentReference1;
                }
                case "CSTMR": {
                    return this.CustomerReference1;
                }
                case "ISSAG": {
                    return this.IssuingCarrierReference1;
                }
                case "CSAEX": {
                    return this.CustomAgentExportReference;
                }
                case "CSAIM": {
                    return this.CustomAgentImportReference;
                }
                case "FRTFR": {
                    return this.FreightForwarderReference;
                }
                case "COLOD": {
                    return this.ColoaderReference1;
                }
                case "CLERN": {
                    return this.CustomClearancePointReference1;
                }
                case "CONSL": {
                    return this.ConsolidatorReference;
                }
                case "REAGT": {
                    return this.ReleasingAgentReference1;
                }
                case "NOTF1": {
                    return this.Notify1Reference;
                }
                case "NOTF2": {
                    return this.Notify2Reference;
                }
                case "SHPNT": {
                    return this.ShipperNotExporterReference;
                }
                case "CONNT": {
                    return this.ConsigneeNotImporterReference;
                }
                default: {
                    return null;
                }
            }
        },
        set: function (newValue) {
            switch (this.Code) {
                case "SHIPR": {
                    this.ShipperReference1 = newValue;
                    break;
                }
                case "CONSI": {
                    this.ConsigneeReference1 = newValue;
                    break;
                }
                case "AGENT": {
                    this.AgentReference1 = newValue;
                    break;
                }
                case "CSTMR": {
                    this.CustomerReference1 = newValue;
                    break;
                }
                case "ISSAG": {
                    this.IssuingCarrierReference1 = newValue;
                    break;
                }
                case "CSAEX": {
                    this.CustomAgentExportReference = newValue;
                    break;
                }
                case "CSAIM": {
                    this.CustomAgentImportReference = newValue;
                    break;
                }
                case "FRTFR": {
                    this.FreightForwarderReference = newValue;
                    break;
                }
                case "COLOD": {
                    this.ColoaderReference1 = newValue;
                    break;
                }
                case "CLERN": {
                    this.CustomClearancePointReference1 = newValue;
                    break;
                }
                case "CONSL": {
                    this.ConsolidatorReference = newValue;
                    break;
                }
                case "REAGT": {
                    this.ReleasingAgentReference1 = newValue;
                    break;
                }
                case "NOTF1": {
                    this.Notify1Reference = newValue;
                    break;
                }
                case "NOTF2": {
                    this.Notify2Reference = newValue;
                    break;
                }
                case "SHPNT": {
                    this.ShipperNotExporterReference = newValue;
                    break;
                }
                case "CONNT": {
                    this.ConsigneeNotImporterReference = newValue;
                    break;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "ShipperReference1", {
        get: function () {
            return this.EntityPM.ShipperReference1;
        },
        set: function (newValue) {
            if (this.EntityPM.ShipperReference1 != newValue) {
                this.EntityPM.ShipperReference1 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "ConsigneeReference1", {
        get: function () {
            return this.EntityPM.ConsigneeReference1;
        },
        set: function (newValue) {
            if (this.EntityPM.ConsigneeReference1 != newValue) {
                this.EntityPM.ConsigneeReference1 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "AgentReference1", {
        get: function () {
            return this.EntityPM.AgentReference1;
        },
        set: function (newValue) {
            if (this.EntityPM.AgentReference1 != newValue) {
                this.EntityPM.AgentReference1 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "CustomerReference1", {
        get: function () {
            return this.EntityPM.CustomerReference1;
        },
        set: function (newValue) {
            if (this.EntityPM.CustomerReference1 != newValue) {
                this.EntityPM.CustomerReference1 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "IssuingCarrierReference1", {
        get: function () {
            return this.EntityPM.IssuingCarrierReference1;
        },
        set: function (newValue) {
            if (this.EntityPM.IssuingCarrierReference1 != newValue) {
                this.EntityPM.IssuingCarrierReference1 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "CustomAgentExportReference", {
        get: function () { return this.EntityPM.CustomAgentExportReference; },
        set: function (newValue) {
            if (this.EntityPM.CustomAgentExportReference != newValue) {
                this.EntityPM.CustomAgentExportReference = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "CustomAgentImportReference", {
        get: function () { return this.EntityPM.CustomAgentImportReference; },
        set: function (newValue) {
            if (this.EntityPM.CustomAgentImportReference != newValue) {
                this.EntityPM.CustomAgentImportReference = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "FreightForwarderReference", {
        get: function () {
            return this.EntityPM.FreightForwarderReference;
        },
        set: function (newValue) {
            if (this.EntityPM.FreightForwarderReference != newValue) {
                this.EntityPM.FreightForwarderReference = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "ColoaderReference1", {
        get: function () {
            return this.EntityPM.ColoaderReference1;
        },
        set: function (newValue) {
            if (this.EntityPM.ColoaderReference1 != newValue) {
                this.EntityPM.ColoaderReference1 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "CustomClearancePointReference1", {
        get: function () {
            return this.EntityPM.CustomClearancePointReference1;
        },
        set: function (newValue) {
            if (this.EntityPM.CustomClearancePointReference1 != newValue) {
                this.EntityPM.CustomClearancePointReference1 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "ConsolidatorReference", {
        get: function () {
            return this.EntityPM.ConsolidatorReference;
        },
        set: function (newValue) {
            if (this.EntityPM.ConsolidatorReference != newValue) {
                this.EntityPM.ConsolidatorReference = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "ReleasingAgentReference1", {
        get: function () {
            return this.EntityPM.ReleasingAgentReference1;
        },
        set: function (newValue) {
            if (this.EntityPM.ReleasingAgentReference1 != newValue) {
                this.EntityPM.ReleasingAgentReference1 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "Notify1Reference", {
        get: function () { return this.EntityPM.Notify1Reference; },
        set: function (value) {
            if (this.EntityPM.Notify1Reference != value) {
                this.EntityPM.Notify1Reference = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "Notify2Reference", {
        get: function () { return this.EntityPM.Notify2Reference; },
        set: function (value) {
            if (this.EntityPM.Notify2Reference != value) {
                this.EntityPM.Notify2Reference = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "ShipperNotExporterReference", {
        get: function () { return this.EntityPM.ShipperNotExporterReference; },
        set: function (value) {
            if (this.EntityPM.ShipperNotExporterReference != value) {
                this.EntityPM.ShipperNotExporterReference = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "ConsigneeNotImporterReference", {
        get: function () { return this.EntityPM.ConsigneeNotImporterReference; },
        set: function (value) {
            if (this.EntityPM.ConsigneeNotImporterReference != value) {
                this.EntityPM.ConsigneeNotImporterReference = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "HasReference2", {
        // Reference2
        get: function () {
            var myResult = false;
            switch (this.Code) {
                case "SHIPR":
                case "CONSI":
                case "AGENT":
                case "REAGT":
                case "CSTMR":
                    {
                        myResult = true;
                    }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "Reference2Property", {
        get: function () {
            switch (this.Code) {
                case "SHIPR": {
                    return "ShipperReference2";
                }
                case "CONSI": {
                    return "ConsigneeReference2";
                }
                case "AGENT": {
                    return "AgentReference2";
                }
                case "CSTMR": {
                    return "CustomerReference2";
                }
                case "REAGT": {
                    return "ReleasingAgentReference2";
                }
                default: {
                    return null;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "Reference2", {
        get: function () {
            switch (this.Code) {
                case "SHIPR": {
                    return this.ShipperReference2;
                }
                case "CONSI": {
                    return this.ConsigneeReference2;
                }
                case "AGENT": {
                    return this.AgentReference2;
                }
                case "CSTMR": {
                    return this.CustomerReference2;
                }
                case "REAGT": {
                    return this.ReleasingAgentReference2;
                }
                default: {
                    return null;
                }
            }
        },
        set: function (newValue) {
            switch (this.Code) {
                case "SHIPR": {
                    this.ShipperReference2 = newValue;
                    break;
                }
                case "CONSI": {
                    this.ConsigneeReference2 = newValue;
                    break;
                }
                case "AGENT": {
                    this.AgentReference2 = newValue;
                    break;
                }
                case "CSTMR": {
                    this.CustomerReference2 = newValue;
                    break;
                }
                case "REAGT": {
                    this.ReleasingAgentReference2 = newValue;
                    break;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "ShipperReference2", {
        get: function () {
            return this.EntityPM.ShipperReference2;
        },
        set: function (newValue) {
            if (this.EntityPM.ShipperReference2 != newValue) {
                this.EntityPM.ShipperReference2 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "ConsigneeReference2", {
        get: function () {
            return this.EntityPM.ConsigneeReference2;
        },
        set: function (newValue) {
            if (this.EntityPM.ConsigneeReference2 != newValue) {
                this.EntityPM.ConsigneeReference2 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "AgentReference2", {
        get: function () {
            return this.EntityPM.AgentReference2;
        },
        set: function (newValue) {
            if (this.EntityPM.AgentReference2 != newValue) {
                this.EntityPM.AgentReference2 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "CustomerReference2", {
        get: function () {
            return this.EntityPM.CustomerReference2;
        },
        set: function (newValue) {
            if (this.EntityPM.CustomerReference2 != newValue) {
                this.EntityPM.CustomerReference2 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "ReleasingAgentReference2", {
        get: function () {
            return this.EntityPM.ReleasingAgentReference2;
        },
        set: function (newValue) {
            if (this.EntityPM.ReleasingAgentReference2 != newValue) {
                this.EntityPM.ReleasingAgentReference2 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    PartnerItem.prototype.GetPartnerCard = function () {
        var _this = this;
        this.SetUIProperties();
        var myCardId = this.PartnerId;
        if (Tools_1.AppTool.IsNullOrEmpty(myCardId)) {
            this.Name = null;
            this.Note = null;
            this.PartnerCardList = null;
            this.AddressId = null;
            this.ContactId = null;
            if (this.Code == "SHIPR") {
                if (this.EntityPM.KnownConsignorNumber != null) {
                    this.EntityPM.KnownConsignorNumber = null;
                }
                if (this.EntityPM.KCExpirationDate != null) {
                    this.EntityPM.KCExpirationDate = null;
                }
            }
        }
        else {
            var myService = this.fatherComponent.CardListService;
            myService.getSingle(myCardId).subscribe(function (myResponse) {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        _this.PartnerCardList = myResponse.Result;
                        if (_this.PartnerCardList != null) {
                            _this.Name = _this.PartnerCardList.EnglishName;
                            _this.Note = _this.PartnerCardList.Notes;
                            if (_this.Code == "ISSAG") {
                                if (_this.isPartnerChanged_Issuing) {
                                    _this.AddressId = _this.PartnerCardList.MainAddressId;
                                }
                                else {
                                    _this.GetPartnerAddress();
                                }
                                _this.isPartnerChanged_Issuing = false;
                            }
                            if (_this.Code == "SHIPR") {
                                if (_this.EntityPM.KnownConsignorNumber != _this.PartnerCardList.KnownConsignor) {
                                    _this.EntityPM.KnownConsignorNumber = _this.PartnerCardList.KnownConsignor;
                                }
                                if (_this.EntityPM.KCExpirationDate != _this.PartnerCardList.KCExpirationDate) {
                                    _this.EntityPM.KCExpirationDate = _this.PartnerCardList.KCExpirationDate;
                                }
                            }
                            if (!_this.IsReseting) {
                                _this.AddressId = _this.PartnerCardList.MainAddressId;
                                _this.ContactId = _this.PartnerCardList.PrimaryContactId;
                            }
                        }
                    }
                }
            });
        }
    };
    PartnerItem.prototype.GetPartnerAddress = function () {
        var _this = this;
        this.isAddressLoaded = false;
        this.PartnerAddressList = null;
        this.ShowNoTemplateText = false;
        this.AddressCityText = null;
        var myAddressId = this.AddressId;
        if (myAddressId != null) {
            var list = this.fatherComponent.AllAddresses.filter(function (f) { return f.Id == myAddressId; })[0];
            if (list) {
                this.PartnerAddressList = list;
                this.isAddressLoaded = true;
                this.BuildAddressCityText();
                this.OnLoadCompleted();
            }
            else {
                var myService = this.fatherComponent.AddressListService;
                myService.getSingle(myAddressId).subscribe(function (myResponse) {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            _this.PartnerAddressList = myResponse.Result;
                            _this.BuildAddressCityText();
                            if (_this.PartnerAddressList) {
                                _this.fatherComponent.AllAddresses.push(_this.PartnerAddressList);
                            }
                        }
                    }
                    _this.isAddressLoaded = true;
                    _this.OnLoadCompleted();
                });
            }
        }
        else {
            this.isAddressLoaded = true;
            this.OnLoadCompleted();
        }
    };
    PartnerItem.prototype.GetPartnerContact = function () {
        var _this = this;
        this.isContactLoaded = false;
        this.PartnerContactList = null;
        this.ShowNoTemplateText = false;
        var myContactId = this.ContactId;
        if (myContactId != null) {
            var list = this.fatherComponent.AllContacts.filter(function (f) { return f.Id == myContactId; })[0];
            if (list) {
                this.PartnerContactList = list;
                this.isContactLoaded = true;
                this.OnLoadCompleted();
            }
            var myService = this.fatherComponent.ContactListService;
            myService.getSingle(myContactId).subscribe(function (myResponse) {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        _this.PartnerContactList = myResponse.Result;
                        if (_this.PartnerContactList) {
                            _this.fatherComponent.AllContacts.push(_this.PartnerContactList);
                        }
                    }
                }
                _this.isContactLoaded = true;
                _this.OnLoadCompleted();
            });
        }
        else {
            this.isContactLoaded = true;
            this.OnLoadCompleted();
        }
    };
    PartnerItem.prototype.OnLoadCompleted = function () {
        if (this.isAddressLoaded && this.isContactLoaded) {
            if (this.PartnerAddressList == null && this.PartnerContactList == null) {
                this.ShowNoTemplateText = true;
            }
            else {
                this.ShowNoTemplateText = false;
            }
        }
    };
    PartnerItem.prototype.BuildAddressCityText = function () {
        var myResult = null;
        if (this.PartnerAddressList) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.PartnerAddressList.City)) {
                myResult = this.PartnerAddressList.City;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.PartnerAddressList.StateName)) {
                myResult = Tools_1.AppTool.IsNullOrEmpty(myResult) ? this.PartnerAddressList.StateName : myResult + ", " + this.PartnerAddressList.StateName;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.PartnerAddressList.ZipCode)) {
                myResult = Tools_1.AppTool.IsNullOrEmpty(myResult) ? this.PartnerAddressList.ZipCode : myResult + ", " + this.PartnerAddressList.ZipCode;
            }
        }
        this.AddressCityText = myResult;
    };
    PartnerItem.prototype.AddPartnerClicked = function () {
        var _this = this;
        if (this.Code == "ISSAG") {
            this.AddEditIssuingCarrierAgent(true);
        }
        else {
            var myPerspective = null;
            var myComponentPath = null;
            if (this.CardDependencyProperty1 == "AG") {
                myComponentPath = "./CommonModules/CommonAgent/Components/NewEntity/NewAgentComponent";
            }
            else if (this.CardDependencyProperty1 == "WH") {
                myComponentPath = "./CommonModules/CommonPartners/Components/NewEntity/NewWarehouseComponent";
            }
            else if (this.Code == "CSAIM" || this.Code == "CSAEX") {
                myComponentPath = "./CommonModules/CommonPartners/Components/NewEntity/NewCustomAgentComponent";
            }
            else {
                myComponentPath = "./CommonModules/CommonCustomer/Components/NewEntity/NewCustomerComponent";
                if (!this.IsCustomer) {
                    myPerspective = "ShippersAndConsignees";
                }
            }
            if (myComponentPath != null) {
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Width = 960;
                logWindow.Height = 600;
                logWindow.Title = "New " + this.PartnerTypeName;
                if (!Tools_1.AppTool.IsNullOrEmpty(myPerspective)) {
                    var args = new Args_1.NewEntityArgs();
                    args.Perspective = myPerspective;
                    logWindow.WindowArgs = args;
                }
                logWindow.Show(myComponentPath);
                logWindow.ComponentLoaded.subscribe(function (comp) {
                    logWindow.WindowClosed.subscribe(function (s) {
                        if (s) {
                            _this.PartnerId = comp.EntityPM.Id;
                        }
                    });
                });
            }
        }
    };
    PartnerItem.prototype.EditPartnerClicked = function () {
        var _this = this;
        if (this.Code == "ISSAG") {
            this.AddEditIssuingCarrierAgent(false);
        }
        else {
            if (!this.isEditButtonClicked) {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.PartnerId)) {
                    this.isEditButtonClicked = true;
                    var myService = this.fatherComponent.CardListService;
                    myService.getSingle(this.PartnerId).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                var objectTableName = null;
                                switch (list.PartnerTypeId) {
                                    case "CO":
                                    case "AG":
                                        {
                                            objectTableName = "Agent";
                                            break;
                                        }
                                    case "AL": {
                                        objectTableName = "Airline";
                                        break;
                                    }
                                    case "TR": {
                                        objectTableName = "Trucker";
                                        break;
                                    }
                                    case "CG": {
                                        objectTableName = "CustomAgent";
                                        break;
                                    }
                                    case "SG": {
                                        objectTableName = "ShippingAgent";
                                        break;
                                    }
                                    case "SL": {
                                        objectTableName = "ShippingLine";
                                        break;
                                    }
                                    case "VD": {
                                        objectTableName = "Vendor";
                                        break;
                                    }
                                    case "CS":
                                        {
                                            objectTableName = "Customer";
                                            break;
                                        }
                                    case "WH":
                                    case "CC":
                                        {
                                            objectTableName = "Warehouse";
                                            break;
                                        }
                                }
                                if (objectTableName != null) {
                                    var logWindow = new LogitudeWindow_1.LogitudeWindow();
                                    logWindow.Title = "Edit " + _this.PartnerTypeName;
                                    logWindow.IsFillScreen = true;
                                    logWindow.ShowEditComponent(_this.PartnerId, objectTableName);
                                    logWindow.ComponentLoaded.subscribe(function (comp) {
                                        logWindow.WindowClosed.subscribe(function (s) {
                                            _this.Name = comp.EntityPM.EnglishName;
                                            _this.Note = comp.EntityPM.Notes;
                                            if (_this.Code == "SHIPR") {
                                                if (_this.EntityPM.KnownConsignorNumber != comp.EntityPM.KnownConsignor) {
                                                    _this.EntityPM.KnownConsignorNumber = comp.EntityPM.KnownConsignor;
                                                }
                                                if (_this.EntityPM.KCExpirationDate != comp.EntityPM.KCExpirationDate) {
                                                    _this.EntityPM.KCExpirationDate = comp.EntityPM.KCExpirationDate;
                                                }
                                            }
                                            _this.GetPartnerAddress();
                                            _this.GetPartnerContact();
                                            _this.isEditButtonClicked = false;
                                        });
                                    });
                                }
                                else {
                                    _this.isEditButtonClicked = false;
                                }
                            }
                            else {
                                _this.isEditButtonClicked = false;
                            }
                        }
                        else {
                            _this.isEditButtonClicked = false;
                        }
                    });
                }
            }
        }
    };
    // Add|Edit Address
    PartnerItem.prototype.AddAddressClicked = function () {
        var _this = this;
        var entityPM = new AddressPM_1.AddressPM();
        entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        entityPM.AddressTypeId = "O";
        entityPM.CardId = this.PartnerId;
        var myPartnerTypeId = null;
        var isCustomer;
        if (this.PartnerCardList != null) {
            myPartnerTypeId = this.PartnerCardList.PartnerTypeId;
            isCustomer = this.PartnerCardList.IsCustomer;
        }
        if (entityPM != null) {
            var logeWindow = new LogitudeWindow_1.LogitudeWindow();
            logeWindow.Width = 630;
            logeWindow.Height = 430;
            logeWindow.Title = "Add Address";
            logeWindow.WindowArgs = { EntityPM: entityPM, PartnerTypeId: myPartnerTypeId, IsCustomer: isCustomer };
            logeWindow.Show("./ShipmentModules/ShipmentTabs/Components/Partners/AddEditAddressComponent");
            logeWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    _this.AddressId = null;
                    _this.AddressId = entityPM.Id;
                }
            });
        }
    };
    PartnerItem.prototype.EditAddressClicked = function () {
        var _this = this;
        var myAddressId = this.AddressId;
        var myPartnerTypeId = null;
        var isCustomer;
        if (this.PartnerCardList != null) {
            myPartnerTypeId = this.PartnerCardList.PartnerTypeId;
            isCustomer = this.PartnerCardList.IsCustomer;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(myAddressId)) {
            var logeWindow = new LogitudeWindow_1.LogitudeWindow();
            logeWindow.Width = 630;
            logeWindow.Height = 430;
            logeWindow.Title = "Edit Address";
            logeWindow.WindowArgs = { EntityId: myAddressId, PartnerTypeId: myPartnerTypeId, IsCustomer: isCustomer };
            logeWindow.Show("./ShipmentModules/ShipmentTabs/Components/Partners/AddEditAddressComponent");
            logeWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    var list = _this.fatherComponent.AllAddresses.filter(function (f) { return f.Id == myAddressId; })[0];
                    if (list) {
                        var indexOfList = _this.fatherComponent.AllAddresses.indexOf(list);
                        _this.fatherComponent.AllAddresses.splice(indexOfList, 1);
                    }
                    _this.AddressId = null;
                    _this.AddressId = myAddressId;
                }
            });
        }
    };
    PartnerItem.prototype.AddEditIssuingCarrierAgent = function (isNewPartner) {
        var _this = this;
        if (!this.isPartnerWindowOpened) {
            this.isPartnerWindowOpened = true;
            var myTitle = isNewPartner ? "Add " : "Edit ";
            myTitle += "Issuing Carrier's Agent";
            var windowArgs = new Args_2.AddEditPartnerArgs();
            windowArgs.EntityPM = this.EntityPM;
            windowArgs.IsNewEntity = isNewPartner;
            windowArgs.PartnerTypeCode = "AGT";
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Title = myTitle;
            logWindow.WindowArgs = windowArgs;
            logWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/Partners/AWBAddEditPartnerComponent');
            logWindow.ComponentLoaded.subscribe(function (cmp) {
                logWindow.WindowClosed.subscribe(function ($event) {
                    _this.isPartnerWindowOpened = false;
                    if (cmp.IsUpdatingPartner) {
                        var myPartnerId = cmp.CurrentPartnerId;
                        var myAddressId = cmp.CurrentAddressId;
                        if (_this.IssuingCarrierAgentId != myPartnerId) {
                            _this.IssuingCarrierAgentId = myPartnerId;
                        }
                        else {
                            _this.EntityPM.IssuingCarrierAddressId = myAddressId;
                            _this.GetPartnerCard();
                        }
                    }
                });
            });
        }
    };
    return PartnerItem;
}(BaseComponent_1.BaseComponent));
exports.PartnerItem = PartnerItem;
//# sourceMappingURL=PartnersTabComponent.js.map