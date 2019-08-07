"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var DeclarationDisplayOnlyChecks_1 = require("../../../../Customs/Utilities/DeclarationDisplayOnlyChecks");
var ServiceResponse_1 = require("../../../../Infrastructure/DataContracts/ServiceResponse");
var Rx_1 = require("rxjs/Rx");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var http_1 = require("@angular/http");
var SessionInfo_1 = require("../../../../Infrastructure/Utilities/SessionInfo");
var ServiceHelper_1 = require("../../../../Infrastructure/Utilities/ServiceHelper");
var core_1 = require("@angular/core");
var DeclarationCargoSplitController = /** @class */ (function () {
    function DeclarationCargoSplitController(DeclarationCargoSplitPM) {
        this.DeclarationCargoSplitPM = DeclarationCargoSplitPM;
        this.SelectionCompleted = new core_1.EventEmitter();
        this.apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/CustomsRequestSheetExtended';
        this.http = ServiceHelper_1.ServiceHelper.Http;
    }
    DeclarationCargoSplitController.prototype.DisplayOnlyCheck = function () {
        var rresponse = new ServiceResponse_1.ServiceResponse();
        var declarationDisplayOnlyChecks = new DeclarationDisplayOnlyChecks_1.DeclarationDisplayOnlyChecks();
        return Rx_1.Observable.defer(function () {
            var rresponse = new ServiceResponse_1.ServiceResponse();
            rresponse.Result = { IsDisplayOnly: false, DisplayOnlyMessage: "" };
            return Rx_1.Observable.of(rresponse);
        });
    };
    DeclarationCargoSplitController.prototype.CheckRequestsInProgress = function (declarationId) {
        var _this = this;
        // Request sheets in progress check
        var authHeader = new http_1.Headers();
        var table = window.ObjectTables.filter(function (d) { return d.Name === "Customs.DeclarationCargoSplit"; })[0];
        var table2 = window.ObjectTables.filter(function (d) { return d.Name === "Customs.Declaration"; })[0];
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this.http.get(_this.apiUrl + '/GetRequestInProgress/?' + 'tenant=' + _this.DeclarationCargoSplitPM.Tenant + '&interfaceTypeCode=8370' + '&objectTableId1=' + table.Id + '&entityId1=' + _this.DeclarationCargoSplitPM.Id + '&objectTableId2=' + table2.Id + '&entityId2=' + encodeURIComponent(declarationId) + '&customFileNo=' + "" + '&displayOnlyMode= false', { headers: authHeader }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                var requestSheets = response.json();
                if (requestSheets == null || requestSheets.length == 0) {
                    serviceResponse.Result = new DeclarationDisplayOnlyChecks_1.DisplayOnlyCheckResult(false, "");
                }
                else {
                    var RequestInProgressInterfaceTypeName = requestSheets[0].InterfaceTypeName;
                    var text = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.RequestInProgress");
                    text = text.replace('{0}', RequestInProgressInterfaceTypeName);
                    serviceResponse.Result = new DeclarationDisplayOnlyChecks_1.DisplayOnlyCheckResult(true, text);
                }
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    return DeclarationCargoSplitController;
}());
exports.DeclarationCargoSplitController = DeclarationCargoSplitController;
//# sourceMappingURL=DeclarationCargoSplitController.js.map