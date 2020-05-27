"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var http_1 = require("@angular/http");
var Rx_1 = require("rxjs/Rx");
var ServiceResponse_1 = require("../../../Infrastructure/DataContracts/ServiceResponse");
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var CustomsDocumentsTicketPM_1 = require("../../EntityPMs/CustomsDocumentsTicketPM");
var CustomsDocumentPointerPM_1 = require("../../EntityPMs/CustomsDocumentPointerPM");
var CustDocsTicketWebService = /** @class */ (function () {
    function CustDocsTicketWebService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/CustDocsTicketWebService';
    }
    CustDocsTicketWebService.prototype.GetCustomsDocumentsTicketsByEntityIdAndChilds = function (entityId, childEntityId1, childEntityId2, childEntityId3, parentEntityCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetCustomsDocumentsTicketsByEntityIdAndChilds?' + 'entityId=' + entityId + '&childEntityId1=' + childEntityId1 + '&childEntityId2=' + childEntityId2 + '&childEntityId3=' + childEntityId3 + '&parentEntityCode=' + parentEntityCode, { headers: authHeader }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                var _mappedListsArray = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {
                        var entity;
                        entity = _this.MapJsonToEntityPM(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);
                    }
                }
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CustDocsTicketWebService.prototype.MapJsonToEntityPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new CustomsDocumentsTicketPM_1.CustomsDocumentsTicketPM();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        this.MapCustomsDocumentPointers(entityPM, jsonPM, mapParent); // Call composition tables map methods
        entityPM.IsDirty = false;
        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
            entityPM.OldEntityPM.CustomsDocumentPointers = [];
            for (var item in entityPM.CustomsDocumentPointers) {
                var myCustomsDocumentPointerPM = entityPM.CustomsDocumentPointers[item];
                var newCustomsDocumentPointerPM = this.clone(myCustomsDocumentPointerPM);
                entityPM.OldEntityPM.CustomsDocumentPointers.push(newCustomsDocumentPointerPM);
            }
        }
        else {
            entityPM.OldEntityPM = null;
        }
        return entityPM;
    };
    CustDocsTicketWebService.prototype.MapCustomsDocumentPointers = function (entityPM, jsonPM, mapParent) {
        if (mapParent === void 0) { mapParent = true; }
        var oldCustomsDocumentPointers = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldCustomsDocumentPointers = entityPM.OldEntityPM.CustomsDocumentPointers;
        }
        entityPM.CustomsDocumentPointers = new Array();
        for (var item in jsonPM.CustomsDocumentPointers) {
            var jItem = jsonPM.CustomsDocumentPointers[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newCustomsDocumentPointerPM;
            if (mapParent) {
                newCustomsDocumentPointerPM = new CustomsDocumentPointerPM_1.CustomsDocumentPointerPM(entityPM);
            }
            else {
                newCustomsDocumentPointerPM = new CustomsDocumentPointerPM_1.CustomsDocumentPointerPM(null);
            }
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newCustomsDocumentPointerPM[pmProperty] = jItem[pmProperty];
            }
            newCustomsDocumentPointerPM.IsDirty = false;
            if (mapParent) {
                newCustomsDocumentPointerPM.UniqueKey = Guid_1.Guid.newGuid();
                newCustomsDocumentPointerPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newCustomsDocumentPointerPM.OldEntityPM = this.clone(newCustomsDocumentPointerPM);
            }
            else {
                if (newCustomsDocumentPointerPM.UniqueKey) {
                    if (jItem.IsDirty)
                        newCustomsDocumentPointerPM.ChangeSetOp = "Update";
                }
                else {
                    newCustomsDocumentPointerPM.ChangeSetOp = "Insert";
                }
                newCustomsDocumentPointerPM.OldEntityPM = null;
                newCustomsDocumentPointerPM.EntityParentPM = null;
            }
            entityPM.CustomsDocumentPointers.push(newCustomsDocumentPointerPM);
        }
        if (oldCustomsDocumentPointers) {
            for (var itemKey in oldCustomsDocumentPointers) {
                if (entityPM.CustomsDocumentPointers.filter(function (p) { return p.UniqueKey === oldCustomsDocumentPointers[itemKey].UniqueKey; }).length === 0) {
                    if (oldCustomsDocumentPointers[itemKey]) {
                        //oldCustomsDocumentPointers[itemKey].ChangeSetOp = "Delete";
                        //entityPM.CustomsDocumentPointers.push(oldCustomsDocumentPointers[itemKey]);
                        var oldItemJson = oldCustomsDocumentPointers[itemKey];
                        var deletedPM = new CustomsDocumentPointerPM_1.CustomsDocumentPointerPM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {
                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM") {
                                continue;
                            }
                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }
                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";
                        deletedPM.OldEntityPM = null;
                        entityPM.CustomsDocumentPointers.push(deletedPM);
                    }
                }
            }
        }
    };
    CustDocsTicketWebService.prototype.clone = function (jsonPM) {
        var entityPM;
        entityPM = {};
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        return entityPM;
    };
    CustDocsTicketWebService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], CustDocsTicketWebService);
    return CustDocsTicketWebService;
}());
exports.CustDocsTicketWebService = CustDocsTicketWebService;
//# sourceMappingURL=CustDocsTicketWebService.js.map