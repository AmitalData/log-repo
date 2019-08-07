"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UpdateCurrencyRateComponent_1 = require("./Components/UpdateCurrencyRate/UpdateCurrencyRateComponent");
var UpdateVATPercentageComponent_1 = require("./Components/UpdateVATPercentage/UpdateVATPercentageComponent");
var CitySelectionComponent_1 = require("./Components/CitySelection/CitySelectionComponent");
var LoadSampleDataComponent_1 = require("./Components/LoadSampleData/LoadSampleDataComponent");
var DWQueryBuilderComponent_1 = require("./Components/DWQueryBuilder/DWQueryBuilderComponent");
var DWQueryBuilderFiltersComponent_1 = require("./Components/DWQueryBuilder/DWQueryBuilderFiltersComponent");
var CustomsShipperGeneralTabComponent_1 = require("./Components/Depositions/EditTab/CustomsShipperGeneralTabComponent");
var DWFilterSettings_1 = require("./Components/DWQueryBuilder/DWFilterSettings");
//import { DWAskUserFiltersComponent } from './Components/DWQueryBuilder/DWAskUserFiltersComponent'; 
var ProductTypeGeneralTabComponent_1 = require("./Components/ProductType/EditTabs/ProductTypeGeneralTabComponent");
exports.Components = [
    CitySelectionComponent_1.CitySelectionComponent,
    UpdateCurrencyRateComponent_1.UpdateCurrencyRateComponent,
    UpdateVATPercentageComponent_1.UpdateVATPercentageComponent,
    LoadSampleDataComponent_1.LoadSampleDataComponent,
    DWQueryBuilderComponent_1.DWQueryBuilderComponent,
    DWQueryBuilderFiltersComponent_1.DWQueryBuilderFiltersComponent,
    CustomsShipperGeneralTabComponent_1.CustomsShipperGeneralTabComponent,
    DWFilterSettings_1.DWFilterSettings,
    //DWAskUserFiltersComponent
    ProductTypeGeneralTabComponent_1.ProductTypeGeneralTabComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "UpdateCurrencyRateComponent": {
                myResult = UpdateCurrencyRateComponent_1.UpdateCurrencyRateComponent;
                break;
            }
            case "UpdateVATPercentageComponent": {
                myResult = UpdateVATPercentageComponent_1.UpdateVATPercentageComponent;
                break;
            }
            case "CitySelectionComponent": {
                myResult = CitySelectionComponent_1.CitySelectionComponent;
                break;
            }
            case "LoadSampleDataComponent": {
                myResult = LoadSampleDataComponent_1.LoadSampleDataComponent;
                break;
            }
            case "DWQueryBuilderComponent": {
                myResult = DWQueryBuilderComponent_1.DWQueryBuilderComponent;
                break;
            }
            case "DWQueryBuilderFiltersComponent": {
                myResult = DWQueryBuilderFiltersComponent_1.DWQueryBuilderFiltersComponent;
                break;
            }
            case "CustomsShipperGeneralTabComponent": {
                myResult = CustomsShipperGeneralTabComponent_1.CustomsShipperGeneralTabComponent;
                break;
            }
            case "DWFilterSettings": {
                myResult = DWFilterSettings_1.DWFilterSettings;
                break;
            }
            //case "DWAskUserFiltersComponent": { myResult = DWAskUserFiltersComponent; break; }
            case "ProductTypeGeneralTabComponent": {
                myResult = ProductTypeGeneralTabComponent_1.ProductTypeGeneralTabComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map