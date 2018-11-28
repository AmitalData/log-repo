"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ModuleDeclarations_1 = require("../ModuleDeclarations");
//import {ControlsModuleDeclarations} from '../../Controls/ModuleDeclarations';
//import {CommonModuleDeclarations} from '../../Common/ModuleDeclarations';
//import {ShipmentModuleDeclarations} from '../../Shipment/ModuleDeclarations';
//import {BookingModuleDeclarations} from '../../Booking/ModuleDeclarations';
//import {ReportModuleDeclarations} from '../../Report/ModuleDeclarations';
//import {CRMModuleDeclarations} from '../../CRM/ModuleDeclarations';
//import {InvoiceModuleDeclarations} from '../../Invoice/ModuleDeclarations';
//import {AccountingModuleDeclarations} from '../../Accounting/ModuleDeclarations';
//import {QuoteModuleDeclarations} from '../../Quote/ModuleDeclarations';
//import {DashboardModuleDeclarations} from '../../Dashboard/ModuleDeclarations';
//import {SharedLogisticsModuleDeclarations} from '../../SharedLogistics/ModuleDeclarations';
//import {WarehouseModuleDeclarations} from '../../Warehouse/ModuleDeclarations';
//import {InfrastructureModuleProviders} from '../../Infrastructure/ModuleProviders';
//import {CommonModuleProviders} from '../../Common/ModuleProviders';
//import {ShipmentModuleProviders} from '../../Shipment/ModuleProviders';
//import {BookingModuleProviders} from '../../Booking/ModuleProviders';
//import {QuoteModuleProviders} from '../../Quote/ModuleProviders';
//import {CRMModuleProviders} from '../../CRM/ModuleProviders';
//import {InvoiceModuleProviders} from '../../Invoice/ModuleProviders';
//import {AccountingModuleProviders} from '../../Accounting/ModuleProviders';
//import {DashboardModuleProviders} from '../../Dashboard/ModuleProviders';
//import {SharedLogisticsModuleProviders} from '../../SharedLogistics/ModuleProviders';
//import {WarehouseModuleProviders} from '../../Warehouse/ModuleProviders';
var DynamicLoaderAOT = /** @class */ (function () {
    function DynamicLoaderAOT() {
    }
    DynamicLoaderAOT.Load = function (myComponentPath, location) {
        var _this = this;
        if (myComponentPath != null) {
            var stringParts = myComponentPath.split("/");
            var myModuleName_1 = "Logitude" + stringParts[1] + "Module";
            var myModulePath = "./" + stringParts[1] + "/" + myModuleName_1;
            var myComponentName_1 = null;
            if (myComponentPath != null) {
                var urlParts = myComponentPath.split("/");
                myComponentName_1 = urlParts[urlParts.length - 1];
            }
            return new Promise(function (resolve) {
                var type = null;
                var factory = null;
                switch (myModuleName_1) {
                    case "LogitudeLoginModule": {
                        type = ModuleDeclarations_1.LoginModuleDeclarations.Get(myComponentName_1);
                        break;
                    }
                    //case "LogitudeControlsModule": {
                    //    type = ControlsModuleDeclarations.Get(myComponentName);
                    //    break;
                    //}
                    //case "LogitudeCommonModule": {
                    //    type = CommonModuleDeclarations.Get(myComponentName);
                    //    break;
                    //}
                    //case "LogitudeShipmentModule": {
                    //    type = ShipmentModuleDeclarations.Get(myComponentName);
                    //    break;
                    //}
                    //case "LogitudeBookingModule": {
                    //    type = BookingModuleDeclarations.Get(myComponentName);
                    //    break;
                    //}
                    //case "LogitudeReportModule": {
                    //    type = ReportModuleDeclarations.Get(myComponentName);
                    //    break;
                    //}
                    //case "LogitudeCRMModule": {
                    //    type = CRMModuleDeclarations.Get(myComponentName);
                    //    break;
                    //}
                    //case "LogitudeInvoiceModule": {
                    //    type = InvoiceModuleDeclarations.Get(myComponentName);
                    //    break;
                    //}
                    //case "LogitudeAccountingModule": {
                    //    type = AccountingModuleDeclarations.Get(myComponentName);
                    //    break;
                    //}
                    //case "LogitudeQuoteModule": {
                    //    type = QuoteModuleDeclarations.Get(myComponentName);
                    //    break;
                    //}
                    //case "LogitudeDashboardModule": {
                    //    type = DashboardModuleDeclarations.Get(myComponentName);
                    //    break;
                    //}
                    //case "LogitudeDashboardModule": {
                    //    break;
                    //}
                    //case "LogitudeSharedLogisticsModule": {
                    //    type = SharedLogisticsModuleDeclarations.Get(myComponentName);
                    //    break;
                    //}                       
                    //case "LogitudeWarehouseModule": {
                    //    type = WarehouseModuleDeclarations.Get(myComponentName);
                    //    break;
                    //}
                }
                if (type) {
                    factory = _this.Resolver.resolveComponentFactory(type);
                }
                if (factory) {
                    var componentRef = location.createComponent(factory);
                    resolve(componentRef);
                }
                else {
                    alert("(AOT) " + myComponentName_1 + " is not declared in " + myModuleName_1);
                }
            });
        }
    };
    return DynamicLoaderAOT;
}());
exports.DynamicLoaderAOT = DynamicLoaderAOT;
//# sourceMappingURL=DynamicLoaderAOT.js.map