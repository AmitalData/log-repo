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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var CardListService_1 = require("../../../../Common/Services/StandardLists/CardListService");
var AddressListService_1 = require("../../../../Common/Services/StandardLists/AddressListService");
var ContactListService_1 = require("../../../../Common/Services/StandardLists/ContactListService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var Args_1 = require("../../../../Infrastructure/Args");
var Tools_1 = require("../../../../Infrastructure/Tools");
var WarehouseEntryPartnersTabComponent = /** @class */ (function () {
    function WarehouseEntryPartnersTabComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.ObjectTableName = "WarehouseEntry";
        this.IsInlandDomestic = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.TabSelectedEvent = null;
        this.SaveCompletedEvent = null;
        this.LoadCompletedEvent = null;
        this.IsEditingEnabled = true;
        this.IsAddDisabled_SHIPR = false;
        this.IsAddDisabled_CONSI = false;
        this.WarehouseEntryCustomerTypeCode = null;
        this.EntityPM = this.entityArgs.EntityPM;
        this.InitializeServices();
        this.Listen();
    }
    WarehouseEntryPartnersTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent) {
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
            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe(function (tabCode) {
                if (tabCode == "PAEY") {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.UpdateScreen();
                }
            });
        }
    };
    WarehouseEntryPartnersTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.TabSelectedEvent);
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    WarehouseEntryPartnersTabComponent.prototype.InitializeServices = function () {
        this.CardListService = new CardListService_1.CardListService();
        this.AddressListService = new AddressListService_1.AddressListService();
        this.ContactListService = new ContactListService_1.ContactListService();
    };
    WarehouseEntryPartnersTabComponent.prototype.ngOnInit = function () {
        if (this.EntityPM != null) {
            this.UpdateScreen();
        }
    };
    WarehouseEntryPartnersTabComponent.prototype.UpdateScreen = function () {
        this.SetUIProperties();
        this.BuildItemsCollection();
        this.SetAddButtonsIsDisabled();
    };
    WarehouseEntryPartnersTabComponent.prototype.SetUIProperties = function () {
    };
    WarehouseEntryPartnersTabComponent.prototype.BuildItemsCollection = function () {
        this.ItemsCollection = [];
        if (this.EntityPM.ShipperId != null) {
            this.ItemsCollection.push(new PartnerItem(this, "SHIPR"));
        }
        if (this.EntityPM.ConsigneeId != null) {
            this.ItemsCollection.push(new PartnerItem(this, "CONSI"));
        }
    };
    WarehouseEntryPartnersTabComponent.prototype.SetAddButtonsIsDisabled = function () {
        this.IsAddDisabled_SHIPR = this.EntityPM.ShipperId == null ? false : true;
        this.IsAddDisabled_CONSI = this.EntityPM.ConsigneeId == null ? false : true;
    };
    WarehouseEntryPartnersTabComponent.prototype.AddPartner = function (myCode) {
        var newPartnerItem = new PartnerItem(this, myCode);
        newPartnerItem.IsNewAdded = true;
        var myWindowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("WarehouseEntry.S.Partners.Add" + newPartnerItem.FullCode);
        this.RunAddEditPartner(newPartnerItem, myWindowTitle);
    };
    WarehouseEntryPartnersTabComponent.prototype.EditPartner = function (myPartnerItem) {
        myPartnerItem.IsNewAdded = false;
        myPartnerItem.CopyOriginData();
        var myWindowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("WarehouseEntry.S.Partners.Edit" + myPartnerItem.FullCode);
        this.RunAddEditPartner(myPartnerItem, myWindowTitle);
    };
    WarehouseEntryPartnersTabComponent.prototype.RunAddEditPartner = function (myPartnerItem, myWindowTitle) {
        var _this = this;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Title = myWindowTitle;
        logitudeWindow.DataContext = myPartnerItem;
        logitudeWindow.Show("./Warehouse/Components/EditTabs/PartnersTab/AddEditPartnerComponent");
        logitudeWindow.WindowClosed.subscribe(function (s) {
            if (s == "OK") {
                _this.BuildItemsCollection();
                _this.SetAddButtonsIsDisabled();
            }
        });
    };
    WarehouseEntryPartnersTabComponent.prototype.DeletePartner = function (myPartnerItem) {
        var _this = this;
        if (myPartnerItem.IsEditingEnabled) {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("WarehouseEntry.M.DeleteThisPartner"));
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
                    myPartnerItem.Reference1 = null;
                    myPartnerItem.Reference2 = null;
                    _this.SetAddButtonsIsDisabled();
                }
            });
        }
    };
    WarehouseEntryPartnersTabComponent.prototype.SetDefaultCustomer = function () {
        this.EntityPM.CustomerId = null;
        this.EntityPM.CustomerName = null;
        this.EntityPM.CustomerRef1 = null;
        this.EntityPM.CustomerRef2 = null;
        this.WarehouseEntryCustomerTypeCode = null;
        if (this.EntityPM.DirectionId == "I") {
            this.WarehouseEntryCustomerTypeCode = "CON";
            this.EntityPM.CustomerId = this.EntityPM.ConsigneeId;
            this.EntityPM.CustomerName = this.EntityPM.ConsigneeName;
            this.EntityPM.CustomerRef1 = this.EntityPM.ConsigneeReference1;
            this.EntityPM.CustomerRef2 = this.EntityPM.ConsigneeReference2;
        }
        else {
            this.WarehouseEntryCustomerTypeCode = "SHI";
            this.EntityPM.CustomerId = this.EntityPM.ShipperId;
            this.EntityPM.CustomerName = this.EntityPM.ShipperName;
            this.EntityPM.CustomerRef1 = this.EntityPM.ShipperReference1;
            this.EntityPM.CustomerRef2 = this.EntityPM.ShipperReference2;
        }
    };
    WarehouseEntryPartnersTabComponent = __decorate([
        core_1.Component({
            selector: 'WarehouseEntryPartnersTabComponent',
            moduleId: module.id,
            templateUrl: './WarehouseEntryPartnersTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], WarehouseEntryPartnersTabComponent);
    return WarehouseEntryPartnersTabComponent;
}());
exports.WarehouseEntryPartnersTabComponent = WarehouseEntryPartnersTabComponent;
var PartnerItem = /** @class */ (function (_super) {
    __extends(PartnerItem, _super);
    function PartnerItem(fatherComponent, typeCode) {
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.ObjectTableName = "WarehouseEntry";
        _this.IsMyCustomer = false;
        _this.IsInlandDomestic = false;
        _this.EditPartnerIsEnabled = false;
        _this.IsEditingEnabled = true;
        _this.IsRemoveButtonVisible = false;
        _this.IsReseting = false;
        _this.isAddressLoaded = false;
        _this.isContactLoaded = false;
        _this.ShowNoTemplateText = false;
        _this.AddressCityText = null;
        _this.EntityPM = fatherComponent.EntityPM;
        _this.IsEditingEnabled = fatherComponent.IsEditingEnabled;
        _this.Code = typeCode;
        _this.InitializeProperties();
        _this.SetRemoveButtonVisibility();
        _this.GetPartnerAddress();
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
        this.EditPartnerIsEnabled = isFieldsEnabled;
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
        }
        this.PartnerTypeName = TextCodeTranslator_1.TextCodeTranslator.Translate("WarehouseEntry.F." + this.FullCode + "Id");
    };
    Object.defineProperty(PartnerItem.prototype, "PartnerName", {
        get: function () {
            switch (this.Code) {
                case "SHIPR": {
                    return this.EntityPM.ShipperName;
                }
                case "CONSI": {
                    return this.EntityPM.ConsigneeName;
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
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "CardDependencyProperty1", {
        get: function () {
            var myResult = null;
            switch (this.Code) {
                case "SHIPR":
                case "CONSI":
                    {
                        if (SessionLocator_1.SessionLocator.TenantPM.AllowAgentInCustomersLOV) {
                            myResult = "CS,AG";
                        }
                        else {
                            myResult = "CS";
                        }
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
    Object.defineProperty(PartnerItem.prototype, "IsCustomer", {
        get: function () {
            return (this.PartnerId == this.EntityPM.CustomerId) ? true : false;
        },
        enumerable: true,
        configurable: true
    });
    PartnerItem.prototype.SetAsCustomer = function () {
        this.EntityPM.CustomerId = null;
        this.EntityPM.CustomerName = null;
        this.EntityPM.CustomerRef1 = null;
        this.EntityPM.CustomerRef2 = null;
        switch (this.Code) {
            case "SHIPR":
                {
                    this.fatherComponent.WarehouseEntryCustomerTypeCode = "SHI";
                    break;
                }
            case "CONSI":
                {
                    this.fatherComponent.WarehouseEntryCustomerTypeCode = "CON";
                    break;
                }
            default:
                {
                    this.fatherComponent.WarehouseEntryCustomerTypeCode = "OTH";
                    break;
                }
        }
        this.EntityPM.CustomerId = this.PartnerId;
        this.EntityPM.CustomerName = this.PartnerName;
        this.EntityPM.CustomerRef1 = this.Reference1;
        this.EntityPM.CustomerRef2 = this.Reference2;
    };
    PartnerItem.prototype.CopyOriginData = function () {
        this.PartnerId_Origin = this.PartnerId;
        this.AddressId_Origin = this.AddressId;
        this.Reference1_Origin = this.Reference1;
        this.Reference2_Origin = this.Reference2;
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
        // PartnerId
        get: function () {
            switch (this.Code) {
                case "SHIPR": {
                    return "ShipperId";
                }
                case "CONSI": {
                    return "ConsigneeId";
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
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "ShipperAddressId", {
        get: function () {
            return this.EntityPM.FromAddressId;
        },
        set: function (newValue) {
            if (this.EntityPM.FromAddressId != newValue) {
                this.EntityPM.FromAddressId = newValue;
                this.GetPartnerAddress();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "ConsigneeAddressId", {
        get: function () {
            return this.EntityPM.ToAddressId;
        },
        set: function (newValue) {
            if (this.EntityPM.ToAddressId != newValue) {
                this.EntityPM.ToAddressId = newValue;
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
                default: {
                    return null;
                }
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
    Object.defineProperty(PartnerItem.prototype, "HasReference2", {
        // Reference2
        get: function () {
            var myResult = false;
            switch (this.Code) {
                case "SHIPR":
                case "CONSI":
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
    Object.defineProperty(PartnerItem.prototype, "CustomerReference2", {
        get: function () {
            return this.EntityPM.CustomerRef2;
        },
        set: function (newValue) {
            if (this.EntityPM.CustomerRef2 != newValue) {
                this.EntityPM.CustomerRef2 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    // Add|Edit Partner
    PartnerItem.prototype.AddPartnerClicked = function () {
        var _this = this;
        var myComponentPath = null;
        myComponentPath = "./CommonModules/CommonCustomer/Components/NewEntity/NewCustomerComponent";
        if (myComponentPath != null) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 900;
            logWindow.Height = 570;
            logWindow.Title = "New " + this.PartnerTypeName;
            var args = new Args_1.NewEntityArgs();
            args.Perspective = "ShippersAndConsignees";
            logWindow.WindowArgs = args;
            logWindow.Show(myComponentPath);
            logWindow.ComponentLoaded.subscribe(function (comp) {
                logWindow.WindowClosed.subscribe(function (s) {
                    if (s) {
                        _this.PartnerId = comp.EntityPM.Id;
                        _this.PartnerName = comp.EntityPM.EnglishName;
                    }
                });
            });
        }
    };
    PartnerItem.prototype.EditPartnerClicked = function () {
        var _this = this;
        var objectTableName = null;
        objectTableName = "Customer";
        if (objectTableName != null) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Title = "Edit " + this.PartnerTypeName;
            logWindow.IsFillScreen = true;
            logWindow.ShowEditComponent(this.PartnerId, objectTableName);
            logWindow.ComponentLoaded.subscribe(function (comp) {
                logWindow.WindowClosed.subscribe(function (s) {
                    _this.PartnerName = comp.EntityPM.EnglishName;
                    _this.GetPartnerAddress();
                });
            });
        }
    };
    PartnerItem.prototype.GetPartnerCard = function () {
        var _this = this;
        this.SetUIProperties();
        var myCardId = this.PartnerId;
        if (Tools_1.AppTool.IsNullOrEmpty(myCardId)) {
            this.PartnerName = null;
            this.AddressCityText = null;
            this.PartnerCardList = null;
            this.PartnerAddressList = null;
            this.PartnerContactList = null;
            this.AddressId = null;
        }
        else {
            var myService = this.fatherComponent.CardListService;
            myService.getSingle(myCardId).subscribe(function (myResponse) {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        var list = myResponse.Result;
                        _this.PartnerCardList = list;
                        if (list) {
                            _this.PartnerName = list.EnglishName;
                            _this.AddressId = list.MainAddressId;
                            switch (_this.Code) {
                                case "SHIPR": {
                                    //  this.EntityPM.FromAddressId = list.PickAddressId;
                                    break;
                                }
                                case "CONSI": {
                                    //  this.EntityPM.ToAddressId = list.PickAddressId;
                                    break;
                                }
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
            var myService = this.fatherComponent.AddressListService;
            myService.getSingle(myAddressId).subscribe(function (myResponse) {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        _this.PartnerAddressList = myResponse.Result;
                    }
                }
                _this.isAddressLoaded = true;
                _this.BuildAddressCityText();
                _this.OnLoadCompleted();
            });
        }
        else {
            this.isAddressLoaded = true;
            this.BuildAddressCityText();
            this.OnLoadCompleted();
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
    PartnerItem.prototype.ResetOriginData = function () {
        if (this.IsNewAdded) {
            this.PartnerId = null;
            this.AddressId = null;
            this.Reference1 = null;
            this.Reference2 = null;
        }
        else {
            this.IsReseting = true;
            this.PartnerId = this.PartnerId_Origin;
            this.AddressId = this.AddressId_Origin;
            this.Reference1 = this.Reference1_Origin;
            this.Reference2 = this.Reference2_Origin;
            this.IsReseting = false;
        }
    };
    return PartnerItem;
}(BaseComponent_1.BaseComponent));
exports.PartnerItem = PartnerItem;
//# sourceMappingURL=WarehouseEntryPartnersTabComponent.js.map