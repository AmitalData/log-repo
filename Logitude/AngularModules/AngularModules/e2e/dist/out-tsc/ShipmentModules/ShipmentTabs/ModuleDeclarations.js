"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var CustomsTabComponent_1 = require("./Components/Customs/CustomsTabComponent");
var ExportFileComponent_1 = require("./Components/Customs/ExportFileComponent");
var OverviewTabComponent_1 = require("./Components/Overview/OverviewTabComponent");
var OrdersTabComponent_1 = require("./Components/Orders/OrdersTabComponent");
var AddEditOrderPackageComponent_1 = require("./Components/Orders/AddEditOrderPackageComponent");
var PartnersTabComponent_1 = require("./Components/Partners/PartnersTabComponent");
var AddEditPartnerComponent_1 = require("./Components/Partners/AddEditPartnerComponent");
var AddEditAddressComponent_1 = require("./Components/Partners/AddEditAddressComponent");
var ShipmentsTabComponent_1 = require("./Components/Shipments/ShipmentsTabComponent");
var MasterTabComponent_1 = require("./Components/Master/MasterTabComponent");
var CustomsFileTabComponent_1 = require("./Components/CustomsFile/CustomsFileTabComponent");
var ConnectionsTabComponent_1 = require("./Components/Connections/ConnectionsTabComponent");
var AddEditShipmentAssemblyComponent_1 = require("./Components/Connections/AddEditShipmentAssemblyComponent");
var ShipmentDocsOutTabComponent_1 = require("./Components/DocsOut/ShipmentDocsOutTabComponent");
var ShipmentDocsInTabComponent_1 = require("./Components/DocsIn/ShipmentDocsInTabComponent");
var ShipmentAuditTabComponent_1 = require("./Components/Audit/ShipmentAuditTabComponent");
var ReceivablesTabComponent_1 = require("./Components/Receivables/ReceivablesTabComponent");
var AddEditReceivableComponent_1 = require("./Components/Receivables/AddEditReceivableComponent");
var PayablesTabComponent_1 = require("./Components/Payables/PayablesTabComponent");
var AddEditPayableComponent_1 = require("./Components/Payables/AddEditPayableComponent");
var TariffsComponent_1 = require("./Components/Windows/Tariffs/TariffsComponent");
var ProfitComponent_1 = require("./Components/Windows/Profit/ProfitComponent");
var QuotesComponent_1 = require("./Components/Windows/Quotes/QuotesComponent");
var PayablesComponent_1 = require("./Components/Windows/Payables/PayablesComponent");
var GroupageComponent_1 = require("./Components/Windows/Groupage/GroupageComponent");
var GroupageContainerComponent_1 = require("./Components/Windows/Groupage/GroupageContainerComponent");
var HarmonizesComponent_1 = require("./Components/Windows/Harmonizes/HarmonizesComponent");
exports.Components = [
    ShipmentsTabComponent_1.ShipmentsTabComponent,
    MasterTabComponent_1.MasterTabComponent,
    CustomsFileTabComponent_1.CustomsFileTabComponent,
    ConnectionsTabComponent_1.ConnectionsTabComponent,
    AddEditShipmentAssemblyComponent_1.AddEditShipmentAssemblyComponent,
    ShipmentDocsOutTabComponent_1.ShipmentDocsOutTabComponent,
    ShipmentDocsInTabComponent_1.ShipmentDocsInTabComponent,
    ShipmentAuditTabComponent_1.ShipmentAuditTabComponent,
    OverviewTabComponent_1.OverviewTabComponent,
    CustomsTabComponent_1.CustomsTabComponent,
    ExportFileComponent_1.ExportFileComponent,
    OrdersTabComponent_1.OrdersTabComponent,
    AddEditOrderPackageComponent_1.AddEditOrderPackageComponent,
    PartnersTabComponent_1.PartnersTabComponent,
    AddEditPartnerComponent_1.AddEditPartnerComponent,
    AddEditAddressComponent_1.AddEditAddressComponent,
    ReceivablesTabComponent_1.ReceivablesTabComponent,
    AddEditReceivableComponent_1.AddEditReceivableComponent,
    PayablesTabComponent_1.PayablesTabComponent,
    AddEditPayableComponent_1.AddEditPayableComponent,
    TariffsComponent_1.TariffsComponent,
    ProfitComponent_1.ProfitComponent,
    QuotesComponent_1.QuotesComponent,
    PayablesComponent_1.PayablesComponent,
    GroupageComponent_1.GroupageComponent,
    GroupageContainerComponent_1.GroupageContainerComponent,
    HarmonizesComponent_1.HarmonizesComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "OverviewTabComponent": {
                myResult = OverviewTabComponent_1.OverviewTabComponent;
                break;
            }
            case "CustomsTabComponent": {
                myResult = CustomsTabComponent_1.CustomsTabComponent;
                break;
            }
            case "ExportFileComponent": {
                myResult = ExportFileComponent_1.ExportFileComponent;
                break;
            }
            case "OrdersTabComponent": {
                myResult = OrdersTabComponent_1.OrdersTabComponent;
                break;
            }
            case "AddEditOrderPackageComponent": {
                myResult = AddEditOrderPackageComponent_1.AddEditOrderPackageComponent;
                break;
            }
            case "PartnersTabComponent": {
                myResult = PartnersTabComponent_1.PartnersTabComponent;
                break;
            }
            case "AddEditPartnerComponent": {
                myResult = AddEditPartnerComponent_1.AddEditPartnerComponent;
                break;
            }
            case "AddEditAddressComponent": {
                myResult = AddEditAddressComponent_1.AddEditAddressComponent;
                break;
            }
            case "ShipmentsTabComponent": {
                myResult = ShipmentsTabComponent_1.ShipmentsTabComponent;
                break;
            }
            case "MasterTabComponent": {
                myResult = MasterTabComponent_1.MasterTabComponent;
                break;
            }
            case "CustomsFileTabComponent": {
                myResult = CustomsFileTabComponent_1.CustomsFileTabComponent;
                break;
            }
            case "ConnectionsTabComponent": {
                myResult = ConnectionsTabComponent_1.ConnectionsTabComponent;
                break;
            }
            case "AddEditShipmentAssemblyComponent": {
                myResult = AddEditShipmentAssemblyComponent_1.AddEditShipmentAssemblyComponent;
                break;
            }
            case "ShipmentDocsOutTabComponent": {
                myResult = ShipmentDocsOutTabComponent_1.ShipmentDocsOutTabComponent;
                break;
            }
            case "ShipmentAuditTabComponent": {
                myResult = ShipmentAuditTabComponent_1.ShipmentAuditTabComponent;
                break;
            }
            case "ShipmentDocsInTabComponent": {
                myResult = ShipmentDocsInTabComponent_1.ShipmentDocsInTabComponent;
                break;
            }
            case "ReceivablesTabComponent": {
                myResult = ReceivablesTabComponent_1.ReceivablesTabComponent;
                break;
            }
            case "AddEditReceivableComponent": {
                myResult = AddEditReceivableComponent_1.AddEditReceivableComponent;
                break;
            }
            case "PayablesTabComponent": {
                myResult = PayablesTabComponent_1.PayablesTabComponent;
                break;
            }
            case "AddEditPayableComponent": {
                myResult = AddEditPayableComponent_1.AddEditPayableComponent;
                break;
            }
            case "TariffsComponent": {
                myResult = TariffsComponent_1.TariffsComponent;
                break;
            }
            case "ProfitComponent": {
                myResult = ProfitComponent_1.ProfitComponent;
                break;
            }
            case "QuotesComponent": {
                myResult = QuotesComponent_1.QuotesComponent;
                break;
            }
            case "PayablesComponent": {
                myResult = PayablesComponent_1.PayablesComponent;
                break;
            }
            case "GroupageComponent": {
                myResult = GroupageComponent_1.GroupageComponent;
                break;
            }
            case "GroupageContainerComponent": {
                myResult = GroupageContainerComponent_1.GroupageContainerComponent;
                break;
            }
            case "HarmonizesComponent": {
                myResult = HarmonizesComponent_1.HarmonizesComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map