"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var MarkUpTypeListService_1 = require("./Services/StandardLists/MarkUpTypeListService");
var QuoteClosingReasonListService_1 = require("./Services/StandardLists/QuoteClosingReasonListService");
var QuoteCustomerTypeListService_1 = require("./Services/StandardLists/QuoteCustomerTypeListService");
var QuoteListService_1 = require("./Services/StandardLists/QuoteListService");
var QuoteStageListService_1 = require("./Services/StandardLists/QuoteStageListService");
var QuoteTemplateListService_1 = require("./Services/StandardLists/QuoteTemplateListService");
var QuoteTypeListService_1 = require("./Services/StandardLists/QuoteTypeListService");
var QuoteRatingListService_1 = require("./Services/StandardLists/QuoteRatingListService");
var QuotePMService_1 = require("./Services/StandardPMs/QuotePMService");
var QuoteStagePMService_1 = require("./Services/StandardPMs/QuoteStagePMService");
//import {QuoteTemplatePMService} from './Services/StandardPMs/QuoteTemplatePMService';
var QuoteMenuButtonsHandler_1 = require("./Components/MenuButtons/QuoteMenuButtonsHandler");
var QuoteFollowUpListService_1 = require("./Services/StandardLists/QuoteFollowUpListService");
var ModuleProviders = /** @class */ (function () {
    function ModuleProviders() {
    }
    ModuleProviders.GetInstance = function (name) {
        var myResult = null;
        switch (name) {
            case "MarkUpTypeListService": {
                myResult = new MarkUpTypeListService_1.MarkUpTypeListService();
                break;
            }
            case "QuoteClosingReasonListService": {
                myResult = new QuoteClosingReasonListService_1.QuoteClosingReasonListService();
                break;
            }
            case "QuoteCustomerTypeListService": {
                myResult = new QuoteCustomerTypeListService_1.QuoteCustomerTypeListService();
                break;
            }
            case "QuoteListService": {
                myResult = new QuoteListService_1.QuoteListService();
                break;
            }
            case "QuoteStageListService": {
                myResult = new QuoteStageListService_1.QuoteStageListService();
                break;
            }
            case "QuoteTemplateListService": {
                myResult = new QuoteTemplateListService_1.QuoteTemplateListService();
                break;
            }
            case "QuoteTypeListService": {
                myResult = new QuoteTypeListService_1.QuoteTypeListService();
                break;
            }
            case "QuoteRatingListService": {
                myResult = new QuoteRatingListService_1.QuoteRatingListService();
                break;
            }
            case "QuotePMService": {
                myResult = new QuotePMService_1.QuotePMService();
                break;
            }
            case "QuoteStagePMService": {
                myResult = new QuoteStagePMService_1.QuoteStagePMService();
                break;
            }
            //case "QuoteTemplatePMService": { myResult = new QuoteTemplatePMService(); break; }
            case "QuoteMenuButtonsHandler": {
                myResult = new QuoteMenuButtonsHandler_1.QuoteMenuButtonsHandler();
                break;
            }
            case "QuoteFollowUpListService": {
                myResult = new QuoteFollowUpListService_1.QuoteFollowUpListService();
                break;
            }
        }
        return myResult;
    };
    return ModuleProviders;
}());
exports.ModuleProviders = ModuleProviders;
//# sourceMappingURL=ModuleProviders.js.map