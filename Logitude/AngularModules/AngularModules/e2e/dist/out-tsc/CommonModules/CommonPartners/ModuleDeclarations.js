"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var NewPartnerTamplate_1 = require("./Components/Templates/NewPartnerTamplate");
var ContactsTabComponent_1 = require("./Components/EditTabs/ContactsTabComponent");
var ContactInputTemplate_1 = require("./Components/Templates/ContactInputTemplate");
var SearchContactsComponent_1 = require("./Components/Templates/SearchContactsComponent");
var ActionStepsTemplate_1 = require("./Components/Templates/ActionStepsTemplate");
var BillingTabComponent_1 = require("./Components/EditTabs/BillingTabComponent");
var AddressesTabComponent_1 = require("./Components/EditTabs/AddressesTabComponent");
var AddEditContactComponent_1 = require("./Components/AddEdit/AddEditContactComponent");
var AddEditAddressComponent_1 = require("./Components/AddEdit/AddEditAddressComponent");
var ContactGeneralTabComponent_1 = require("./Components/EditTabs/Contact/ContactGeneralTabComponent");
var PartnersTabComponent_1 = require("./Components/EditTabs/Contact/PartnersTabComponent");
var WarehouseGeneralTabComponent_1 = require("./Components/EditTabs/Warehouse/WarehouseGeneralTabComponent");
var ShippingLineInttraTabComponent_1 = require("./Components/EditTabs/ShippingLine/ShippingLineInttraTabComponent");
var NewCustomAgentComponent_1 = require("./Components/NewEntity/NewCustomAgentComponent");
var NewShippingLineComponent_1 = require("./Components/NewEntity/NewShippingLineComponent");
var NewTruckerComponent_1 = require("./Components/NewEntity/NewTruckerComponent");
var NewVendorComponent_1 = require("./Components/NewEntity/NewVendorComponent");
var NewWarehouseComponent_1 = require("./Components/NewEntity/NewWarehouseComponent");
var NewShippingAgentComponent_1 = require("./Components/NewEntity/NewShippingAgentComponent");
var NewContactComponent_1 = require("./Components/NewEntity/NewContactComponent");
var NewPotentialCustomerComponent_1 = require("./Components/NewEntity/NewPotentialCustomerComponent");
var ParticipantGeneralTabComponent_1 = require("./Components/EditTabs/Participant/ParticipantGeneralTabComponent");
var ParticipantNotifyTabComponent_1 = require("./Components/EditTabs/Participant/ParticipantNotifyTabComponent");
var ParticipantDocsInTabComponent_1 = require("./Components/EditTabs/Participant/ParticipantDocsInTabComponent");
var CustomAgentDocsInTabComponent_1 = require("./Components/EditTabs/CustomAgent/CustomAgentDocsInTabComponent");
var ShippingAgentDocsInTabComponent_1 = require("./Components/EditTabs/ShippingAgent/ShippingAgentDocsInTabComponent");
var ShippingLineDocsInTabComponent_1 = require("./Components/EditTabs/ShippingLine/ShippingLineDocsInTabComponent");
var TruckerDocsInTabComponent_1 = require("./Components/EditTabs/Trucker/TruckerDocsInTabComponent");
var VendorDocsInTabComponent_1 = require("./Components/EditTabs/Vendor/VendorDocsInTabComponent");
var WarehouseDocsInTabComponent_1 = require("./Components/EditTabs/Warehouse/WarehouseDocsInTabComponent");
exports.Components = [
    NewPartnerTamplate_1.NewPartnerTamplate,
    ContactsTabComponent_1.ContactsTabComponent,
    ContactInputTemplate_1.ContactInputTemplate,
    SearchContactsComponent_1.SearchContactsComponent,
    ActionStepsTemplate_1.ActionStepsTemplate,
    BillingTabComponent_1.BillingTabComponent,
    AddressesTabComponent_1.AddressesTabComponent,
    AddEditContactComponent_1.AddEditContactComponent,
    AddEditAddressComponent_1.AddEditAddressComponent,
    ContactGeneralTabComponent_1.ContactGeneralTabComponent,
    PartnersTabComponent_1.PartnersTabComponent,
    WarehouseGeneralTabComponent_1.WarehouseGeneralTabComponent,
    ShippingLineInttraTabComponent_1.ShippingLineInttraTabComponent,
    NewCustomAgentComponent_1.NewCustomAgentComponent,
    NewShippingLineComponent_1.NewShippingLineComponent,
    NewTruckerComponent_1.NewTruckerComponent,
    NewVendorComponent_1.NewVendorComponent,
    NewWarehouseComponent_1.NewWarehouseComponent,
    NewShippingAgentComponent_1.NewShippingAgentComponent,
    NewContactComponent_1.NewContactComponent,
    NewPotentialCustomerComponent_1.NewPotentialCustomerComponent,
    ParticipantGeneralTabComponent_1.ParticipantGeneralTabComponent,
    ParticipantNotifyTabComponent_1.ParticipantNotifyTabComponent,
    ParticipantDocsInTabComponent_1.ParticipantDocsInTabComponent,
    CustomAgentDocsInTabComponent_1.CustomAgentDocsInTabComponent,
    ShippingAgentDocsInTabComponent_1.ShippingAgentDocsInTabComponent,
    ShippingLineDocsInTabComponent_1.ShippingLineDocsInTabComponent,
    TruckerDocsInTabComponent_1.TruckerDocsInTabComponent,
    VendorDocsInTabComponent_1.VendorDocsInTabComponent,
    WarehouseDocsInTabComponent_1.WarehouseDocsInTabComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "NewPartnerTamplate": {
                myResult = NewPartnerTamplate_1.NewPartnerTamplate;
                break;
            }
            case "ContactsTabComponent": {
                myResult = ContactsTabComponent_1.ContactsTabComponent;
                break;
            }
            case "ContactInputTemplate": {
                myResult = ContactInputTemplate_1.ContactInputTemplate;
                break;
            }
            case "SearchContactsComponent": {
                myResult = SearchContactsComponent_1.SearchContactsComponent;
                break;
            }
            case "ActionStepsTemplate": {
                myResult = ActionStepsTemplate_1.ActionStepsTemplate;
                break;
            }
            case "BillingTabComponent": {
                myResult = BillingTabComponent_1.BillingTabComponent;
                break;
            }
            case "AddressesTabComponent": {
                myResult = AddressesTabComponent_1.AddressesTabComponent;
                break;
            }
            case "AddEditContactComponent": {
                myResult = AddEditContactComponent_1.AddEditContactComponent;
                break;
            }
            case "AddEditAddressComponent": {
                myResult = AddEditAddressComponent_1.AddEditAddressComponent;
                break;
            }
            case "ContactGeneralTabComponent": {
                myResult = ContactGeneralTabComponent_1.ContactGeneralTabComponent;
                break;
            }
            case "PartnersTabComponent": {
                myResult = PartnersTabComponent_1.PartnersTabComponent;
                break;
            }
            case "WarehouseGeneralTabComponent": {
                myResult = WarehouseGeneralTabComponent_1.WarehouseGeneralTabComponent;
                break;
            }
            case "ShippingLineInttraTabComponent": {
                myResult = ShippingLineInttraTabComponent_1.ShippingLineInttraTabComponent;
                break;
            }
            case "NewCustomAgentComponent": {
                myResult = NewCustomAgentComponent_1.NewCustomAgentComponent;
                break;
            }
            case "NewShippingLineComponent": {
                myResult = NewShippingLineComponent_1.NewShippingLineComponent;
                break;
            }
            case "NewTruckerComponent": {
                myResult = NewTruckerComponent_1.NewTruckerComponent;
                break;
            }
            case "NewVendorComponent": {
                myResult = NewVendorComponent_1.NewVendorComponent;
                break;
            }
            case "NewWarehouseComponent": {
                myResult = NewWarehouseComponent_1.NewWarehouseComponent;
                break;
            }
            case "NewShippingAgentComponent": {
                myResult = NewShippingAgentComponent_1.NewShippingAgentComponent;
                break;
            }
            case "NewContactComponent": {
                myResult = NewContactComponent_1.NewContactComponent;
                break;
            }
            case "NewPotentialCustomerComponent": {
                myResult = NewPotentialCustomerComponent_1.NewPotentialCustomerComponent;
                break;
            }
            case "ParticipantGeneralTabComponent": {
                myResult = ParticipantGeneralTabComponent_1.ParticipantGeneralTabComponent;
                break;
            }
            case "ParticipantNotifyTabComponent": {
                myResult = ParticipantNotifyTabComponent_1.ParticipantNotifyTabComponent;
                break;
            }
            case "ParticipantDocsInTabComponent": {
                myResult = ParticipantDocsInTabComponent_1.ParticipantDocsInTabComponent;
                break;
            }
            case "CustomAgentDocsInTabComponent": {
                myResult = CustomAgentDocsInTabComponent_1.CustomAgentDocsInTabComponent;
                break;
            }
            case "ShippingAgentDocsInTabComponent": {
                myResult = ShippingAgentDocsInTabComponent_1.ShippingAgentDocsInTabComponent;
                break;
            }
            case "ShippingLineDocsInTabComponent": {
                myResult = ShippingLineDocsInTabComponent_1.ShippingLineDocsInTabComponent;
                break;
            }
            case "TruckerDocsInTabComponent": {
                myResult = TruckerDocsInTabComponent_1.TruckerDocsInTabComponent;
                break;
            }
            case "VendorDocsInTabComponent": {
                myResult = VendorDocsInTabComponent_1.VendorDocsInTabComponent;
                break;
            }
            case "WarehouseDocsInTabComponent": {
                myResult = WarehouseDocsInTabComponent_1.WarehouseDocsInTabComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map