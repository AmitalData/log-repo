"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var BookingAnswerStatusListService_1 = require("./Services/StandardLists/BookingAnswerStatusListService");
var BookingLevelListService_1 = require("./Services/StandardLists/BookingLevelListService");
var BookingListService_1 = require("./Services/StandardLists/BookingListService");
var BookingProductListService_1 = require("./Services/StandardLists/BookingProductListService");
var BookingSpaceAllocationListService_1 = require("./Services/StandardLists/BookingSpaceAllocationListService");
var BookingStatusListService_1 = require("./Services/StandardLists/BookingStatusListService");
var FFRStatusListService_1 = require("./Services/StandardLists/FFRStatusListService");
var FlightsSchedulesRequestListService_1 = require("./Services/StandardLists/FlightsSchedulesRequestListService");
var FlightsSchedulesRequestStatusListService_1 = require("./Services/StandardLists/FlightsSchedulesRequestStatusListService");
var BookingPMService_1 = require("./Services/StandardPMs/BookingPMService");
var FlightsSchedulesRequestPMService_1 = require("./Services/StandardPMs/FlightsSchedulesRequestPMService");
var ModuleProviders = /** @class */ (function () {
    function ModuleProviders() {
    }
    ModuleProviders.GetInstance = function (name) {
        var myResult = null;
        switch (name) {
            //List
            case "BookingAnswerStatusListService": {
                myResult = new BookingAnswerStatusListService_1.BookingAnswerStatusListService();
                break;
            }
            case "BookingLevelListService": {
                myResult = new BookingLevelListService_1.BookingLevelListService();
                break;
            }
            case "BookingListService": {
                myResult = new BookingListService_1.BookingListService();
                break;
            }
            case "BookingProductListService": {
                myResult = new BookingProductListService_1.BookingProductListService();
                break;
            }
            case "BookingSpaceAllocationListService": {
                myResult = new BookingSpaceAllocationListService_1.BookingSpaceAllocationListService();
                break;
            }
            case "BookingStatusListService": {
                myResult = new BookingStatusListService_1.BookingStatusListService();
                break;
            }
            case "FFRStatusListService": {
                myResult = new FFRStatusListService_1.FFRStatusListService();
                break;
            }
            case "FlightsSchedulesRequestListService": {
                myResult = new FlightsSchedulesRequestListService_1.FlightsSchedulesRequestListService();
                break;
            }
            case "FlightsSchedulesRequestStatusListService": {
                myResult = new FlightsSchedulesRequestStatusListService_1.FlightsSchedulesRequestStatusListService();
                break;
            }
            // PM
            case "BookingPMService": {
                myResult = new BookingPMService_1.BookingPMService();
                break;
            }
            case "FlightsSchedulesRequestPMService": {
                myResult = new FlightsSchedulesRequestPMService_1.FlightsSchedulesRequestPMService();
                break;
            }
        }
        return myResult;
    };
    return ModuleProviders;
}());
exports.ModuleProviders = ModuleProviders;
//# sourceMappingURL=ModuleProviders.js.map