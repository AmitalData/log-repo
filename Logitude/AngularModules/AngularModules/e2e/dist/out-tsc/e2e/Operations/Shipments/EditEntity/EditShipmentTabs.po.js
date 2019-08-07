"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../../../Helpers/FieldsHelper");
var GeneralFunctions_1 = require("../../../Helpers/GeneralFunctions");
var GeneralTab_1 = require("./GeneralTab");
var OrdersTab_1 = require("./OrdersTab");
var PartnersTab_1 = require("./PartnersTab");
var PackagesTab_1 = require("./PackagesTab");
var RoutingTab_1 = require("./RoutingTab");
var PayablesTab_1 = require("./PayablesTab");
var RecievablesTab_1 = require("./RecievablesTab");
var DocsOutTab_1 = require("./DocsOutTab");
var ShipmentsTab_1 = require("./ShipmentsTab");
var ShipmentSearch_1 = require("../../ShipmentSearch");
var EditTabsComponent = /** @class */ (function () {
    function EditTabsComponent() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
        this.Operation = new GeneralFunctions_1.GeneralFunctions();
        this.GeneralTabScenario = new GeneralTab_1.GeneralTabComponent();
        this.OrderTabScenario = new OrdersTab_1.OrderTabComponent();
        this.PartnersTabScenario = new PartnersTab_1.PartnersTabComponent();
        this.PackagesTabScenario = new PackagesTab_1.PackagesTabComponent();
        this.RoutingTabScenario = new RoutingTab_1.RoutingTabComponent();
        this.PayablesTabScenario = new PayablesTab_1.PayablesTabComponent();
        this.ReceivablesTabScenario = new RecievablesTab_1.ReceivablesTabComponent();
        this.DocsOutTabScenario = new DocsOutTab_1.DocsOutTabComponent();
        this.ShipmentsTabScenario = new ShipmentsTab_1.ShipmentsTabComponent();
        this.QuickSearch = new ShipmentSearch_1.ShipmentSearch();
    }
    EditTabsComponent.prototype.GoToShipment = function () {
        this.Operation.GoToMainMenu('General.MH.Operations');
        this.Operation.SelectMenuWorkSpaceTabs('SHIP');
        // this.QuickSearch.UseQuickSearch('SR1545342');
    };
    EditTabsComponent.prototype.EditTabs = function (shipperRef1, ShipmentLevelCode, ShipmentType, Direction) {
        this.Helper.WaitByIdAndClick('Shipment.TH.Overview');
        this.GeneralTabScenario.GeneralTab(ShipmentLevelCode);
        this.OrderTabScenario.OrderTab(ShipmentLevelCode, ShipmentType);
        this.PartnersTabScenario.PartnersTab(ShipmentLevelCode);
        this.PackagesTabScenario.PackagesTab(ShipmentLevelCode, ShipmentType);
        this.RoutingTabScenario.RoutingTab(ShipmentLevelCode, ShipmentType, Direction);
        this.PayablesTabScenario.PayablesTab(shipperRef1, ShipmentType);
        this.ReceivablesTabScenario.RecievablesTab(ShipmentLevelCode, ShipmentType);
        //this.DocsOutTabScenario.DocsOutTab();
        if (ShipmentLevelCode == 'M') {
            this.ShipmentsTabScenario.ShipmentsTab();
        }
        this.Helper.WaitByIdAndClick('Shipment-Save');
        this.Helper.WaitBusyIndicator();
    };
    return EditTabsComponent;
}());
exports.EditTabsComponent = EditTabsComponent;
//# sourceMappingURL=EditShipmentTabs.po.js.map