import {ViewContainerRef, ComponentRef, Compiler, ModuleWithComponentFactories, ComponentFactoryResolver} from '@angular/core';

import {LoginModuleDeclarations} from '../ModuleDeclarations';
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

export class DynamicLoaderAOT {
    public static Compiler: Compiler;
    public static Resolver: ComponentFactoryResolver
    public static Load(myComponentPath: string, location: ViewContainerRef): Promise<ComponentRef<any>> {
        if (myComponentPath != null) {
            var stringParts: string[] = myComponentPath.split("/");
            let myModuleName = "Logitude" + stringParts[1] + "Module";
            let myModulePath = "./" + stringParts[1] + "/" + myModuleName;

            let myComponentName: any = null;
            if (myComponentPath != null) {
                var urlParts: string[] = myComponentPath.split("/");
                myComponentName = urlParts[urlParts.length - 1];
            }

            return new Promise(resolve => {

                let type: any = null;
                let factory: any = null;

                switch (myModuleName) {
                    case "LogitudeLoginModule": {
                        type = LoginModuleDeclarations.Get(myComponentName);
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
                    factory = this.Resolver.resolveComponentFactory(type);
                }

                if (factory) {
                    let componentRef = location.createComponent(factory);
                    resolve(componentRef);
                }

                else {
                    alert("(AOT) " + myComponentName + " is not declared in " + myModuleName);
                }
            });
        }
    }

    //public static GetInstance(myPath: string): Promise<any> {
    //    if (myPath != null) {
    //        var stringParts: string[] = myPath.split("/");
    //        let myModuleName = "Logitude" + stringParts[1] + "Module";
    //        let myModulePath = "./" + stringParts[1] + "/" + myModuleName;

    //        let instanceName: any = null;
    //        if (myPath != null) {
    //            var urlParts: string[] = myPath.split("/");
    //            instanceName = urlParts[urlParts.length - 1];
    //        }

    //        return new Promise(resolve => {

    //            let instance: any = null;

    //            switch (myModuleName) {
    //                case "LogitudeLoginModule": {
    //                    instance = InfrastructureModuleProviders.GetInstance(instanceName);
    //                    break;
    //                }
    //            }

    //            if (instance) {
    //                resolve(instance);
    //            }

    //            else {
    //                alert("(AOT:Instance) " + instanceName + " is not declared in " + myModuleName);
    //            }
    //        });
    //    }

    //    else {
    //        alert("Instance Path is null");
    //    }
    //}

}