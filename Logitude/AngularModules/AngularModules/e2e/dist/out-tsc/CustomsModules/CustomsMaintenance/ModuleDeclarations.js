"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var AddEditRequiredFieldsComponent_1 = require("./Components/RequiredFields/AddEditRequiredFieldsComponent");
var CustomsClosedTablesComponent_1 = require("./Components/CustomsClosedTablesComponent");
var ClosedTableNotExistedComponent_1 = require("./Components/ClosedTableNotExistedComponent");
var CustomsSettingsComponent_1 = require("./Components/CustomsSettingsComponent");
var RequiredFieldsComponent_1 = require("./Components/RequiredFields/RequiredFieldsComponent");
var InterfaceManagementComponent_1 = require("./Components/InterfaceManagementComponent");
var AddEditInterfaceManagementComponent_1 = require("./Components/AddEditInterfaceManagementComponent");
var LoadTestComponent_1 = require("./Components/LoadTestComponent");
var SignStationsComponent_1 = require("./Components/SignStationsComponent");
var DocumentTypeCustomsDataComponent_1 = require("./Components/DocumentTypeCustomsDataComponent");
var GeneralLOVComponent_1 = require("./Components/GeneralLOVComponent");
var CustomsDocumentsDefinitionComponent_1 = require("./Components/CustomsDocumentsDefinitionComponent");
var AddEditCustomsAirlineComponent_1 = require("./Components/AddEditCustomsAirlineComponent");
var CustomsPartnerFtpListComponent_1 = require("./Components/CustomsPartnerFtpListComponent");
//import { CustomsPartnerFtpEditComponent } from './Components/CustomsPartnerFtpEditComponent';
exports.Components = [
    AddEditRequiredFieldsComponent_1.AddEditRequiredFieldsComponent,
    CustomsClosedTablesComponent_1.CustomsClosedTablesComponent,
    ClosedTableNotExistedComponent_1.ClosedTableNotExistedComponent,
    CustomsSettingsComponent_1.CustomsSettingsComponent,
    RequiredFieldsComponent_1.RequiredFieldsComponent,
    InterfaceManagementComponent_1.InterfaceManagementComponent,
    AddEditInterfaceManagementComponent_1.AddEditInterfaceManagementComponent,
    LoadTestComponent_1.LoadTestComponent,
    SignStationsComponent_1.SignStationsComponent,
    DocumentTypeCustomsDataComponent_1.DocumentTypeCustomsDataComponent,
    GeneralLOVComponent_1.GeneralLOVComponent,
    CustomsDocumentsDefinitionComponent_1.CustomsDocumentsDefinitionComponent,
    AddEditCustomsAirlineComponent_1.AddEditCustomsAirlineComponent,
    CustomsPartnerFtpListComponent_1.CustomsPartnerFtpListComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "AddEditRequiredFieldsComponent": {
                myResult = AddEditRequiredFieldsComponent_1.AddEditRequiredFieldsComponent;
                break;
            }
            case "CustomsClosedTablesComponent": {
                myResult = CustomsClosedTablesComponent_1.CustomsClosedTablesComponent;
                break;
            }
            case "ClosedTableNotExistedComponent": {
                myResult = ClosedTableNotExistedComponent_1.ClosedTableNotExistedComponent;
                break;
            }
            case "CustomsSettingsComponent": {
                myResult = CustomsSettingsComponent_1.CustomsSettingsComponent;
                break;
            }
            case "RequiredFieldsComponent": {
                myResult = RequiredFieldsComponent_1.RequiredFieldsComponent;
                break;
            }
            case "InterfaceManagementComponent": {
                myResult = InterfaceManagementComponent_1.InterfaceManagementComponent;
                break;
            }
            case "AddEditInterfaceManagementComponent": {
                myResult = AddEditInterfaceManagementComponent_1.AddEditInterfaceManagementComponent;
                break;
            }
            case "LoadTestComponent": {
                myResult = LoadTestComponent_1.LoadTestComponent;
                break;
            }
            case "SignStationsComponent": {
                myResult = SignStationsComponent_1.SignStationsComponent;
                break;
            }
            case "DocumentTypeCustomsDataComponent": {
                myResult = DocumentTypeCustomsDataComponent_1.DocumentTypeCustomsDataComponent;
                break;
            }
            case "GeneralLOVComponent": {
                myResult = GeneralLOVComponent_1.GeneralLOVComponent;
                break;
            }
            case "CustomsDocumentsDefinitionComponent": {
                myResult = CustomsDocumentsDefinitionComponent_1.CustomsDocumentsDefinitionComponent;
                break;
            }
            case "AddEditCustomsAirlineComponent": {
                myResult = AddEditCustomsAirlineComponent_1.AddEditCustomsAirlineComponent;
                break;
            }
            case "CustomsPartnerFtpListComponent": {
                myResult = CustomsPartnerFtpListComponent_1.CustomsPartnerFtpListComponent;
                break;
            }
            //case "CustomsPartnerFtpEditComponent": { myResult = CustomsPartnerFtpEditComponent; break; }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map