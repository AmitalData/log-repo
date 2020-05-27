"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldTemplateComponent_1 = require("./Components/Templates/FieldTemplateComponent");
var QuotesComponent_1 = require("./Components/Workspaces/QuotesComponent");
var NewQuoteComponent_1 = require("./Components/NewEntity/NewQuoteComponent");
var QuoteDimensionsComponent_1 = require("./Components/NewEntity/QuoteDimensionsComponent");
var NewQuoteAddEditAddressComponent_1 = require("./Components/NewEntity/NewQuoteAddEditAddressComponent");
var NewQuoteAddEditDimensionsComponent_1 = require("./Components/NewEntity/NewQuoteAddEditDimensionsComponent");
var QuoteShortTitleComponent_1 = require("./Components/ShortTitles/QuoteShortTitleComponent");
var QuoteHelperComponent_1 = require("./Components/Helpers/QuoteHelperComponent");
var QuoteFiltersMenuComponent_1 = require("./Components/FiltersMenu/QuoteFiltersMenuComponent");
var ApproveBuildShipmentComponent_1 = require("./Components/MenuButtons/ApproveBuildShipmentComponent");
var QuoteEventNotesComponent_1 = require("./Components/MenuButtons/QuoteEventNotesComponent");
exports.Components = [
    FieldTemplateComponent_1.FieldTemplateComponent,
    QuotesComponent_1.QuotesComponent,
    NewQuoteComponent_1.NewQuoteComponent,
    QuoteDimensionsComponent_1.QuoteDimensionsComponent,
    NewQuoteAddEditAddressComponent_1.NewQuoteAddEditAddressComponent,
    NewQuoteAddEditDimensionsComponent_1.NewQuoteAddEditDimensionsComponent,
    QuoteShortTitleComponent_1.QuoteShortTitleComponent,
    QuoteHelperComponent_1.QuoteHelperComponent,
    QuoteFiltersMenuComponent_1.QuoteFiltersMenuComponent,
    ApproveBuildShipmentComponent_1.ApproveBuildShipmentComponent,
    QuoteEventNotesComponent_1.QuoteEventNotesComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "FieldTemplateComponent": {
                myResult = FieldTemplateComponent_1.FieldTemplateComponent;
                break;
            }
            case "QuotesComponent": {
                myResult = QuotesComponent_1.QuotesComponent;
                break;
            }
            case "NewQuoteComponent": {
                myResult = NewQuoteComponent_1.NewQuoteComponent;
                break;
            }
            case "QuoteDimensionsComponent": {
                myResult = QuoteDimensionsComponent_1.QuoteDimensionsComponent;
                break;
            }
            case "NewQuoteAddEditAddressComponent": {
                myResult = NewQuoteAddEditAddressComponent_1.NewQuoteAddEditAddressComponent;
                break;
            }
            case "NewQuoteAddEditDimensionsComponent": {
                myResult = NewQuoteAddEditDimensionsComponent_1.NewQuoteAddEditDimensionsComponent;
                break;
            }
            case "QuoteShortTitleComponent": {
                myResult = QuoteShortTitleComponent_1.QuoteShortTitleComponent;
                break;
            }
            case "QuoteHelperComponent": {
                myResult = QuoteHelperComponent_1.QuoteHelperComponent;
                break;
            }
            case "QuoteFiltersMenuComponent": {
                myResult = QuoteFiltersMenuComponent_1.QuoteFiltersMenuComponent;
                break;
            }
            case "ApproveBuildShipmentComponent": {
                myResult = ApproveBuildShipmentComponent_1.ApproveBuildShipmentComponent;
                break;
            }
            case "QuoteEventNotesComponent": {
                myResult = QuoteEventNotesComponent_1.QuoteEventNotesComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map