"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var CustomsRequestsSheetsComponent_1 = require("./Components/CustomsRequestsSheetsComponent");
var CustomsErrorsComponent_1 = require("./Components/CustomsErrorsComponent");
var CustomSendOptionsComponent_1 = require("./Components/CustomSendOptionsComponent");
var DropdownButtonComponent_1 = require("./Components/DropdownButtonComponent");
var CustomMessageWrapperComponent_1 = require("./Components/CustomMessageWrapperComponent");
var CustomMessageProgressComponent_1 = require("./Components/CustomMessageProgressComponent");
var NotificationComponent_1 = require("./Components/NotificationComponent");
var ObjectViewerComponent_1 = require("./Components/ObjectViewerComponent");
exports.Components = [
    CustomsRequestsSheetsComponent_1.CustomsRequestsSheetsComponent,
    CustomsErrorsComponent_1.CustomsErrorsComponent,
    CustomSendOptionsComponent_1.CustomSendOptionsComponent,
    DropdownButtonComponent_1.DropdownButtonComponent,
    CustomMessageWrapperComponent_1.CustomMessageWrapperComponent,
    CustomMessageProgressComponent_1.CustomMessageProgressComponent,
    NotificationComponent_1.NotificationComponent,
    ObjectViewerComponent_1.ObjectViewerComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "CustomsRequestsSheetsComponent": {
                myResult = CustomsRequestsSheetsComponent_1.CustomsRequestsSheetsComponent;
                break;
            }
            case "CustomsErrorsComponent": {
                myResult = CustomsErrorsComponent_1.CustomsErrorsComponent;
                break;
            }
            case "CustomSendOptionsComponent": {
                myResult = CustomSendOptionsComponent_1.CustomSendOptionsComponent;
                break;
            }
            case "DropdownButtonComponent": {
                myResult = DropdownButtonComponent_1.DropdownButtonComponent;
                break;
            }
            case "CustomMessageWrapperComponent": {
                myResult = CustomMessageWrapperComponent_1.CustomMessageWrapperComponent;
                break;
            }
            case "CustomMessageProgressComponent": {
                myResult = CustomMessageProgressComponent_1.CustomMessageProgressComponent;
                break;
            }
            case "NotificationComponent": {
                myResult = NotificationComponent_1.NotificationComponent;
                break;
            }
            case "ObjectViewerComponent": {
                myResult = ObjectViewerComponent_1.ObjectViewerComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map