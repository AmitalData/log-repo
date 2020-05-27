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
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var ServiceResponse_1 = require("../../../Infrastructure/DataContracts/ServiceResponse");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var ClientPM_1 = require("../../EntityPMs/ClientPM");
var ClientAddressPM_1 = require("../../EntityPMs/ClientAddressPM");
var ClientsAddressCommTypePM_1 = require("../../EntityPMs/ClientsAddressCommTypePM");
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
var ClientMessagesService = /** @class */ (function () {
    function ClientMessagesService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/Client';
    }
    ClientMessagesService.prototype.PostUpdateDeleteClientAddressContactRequest = function (entity) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            var hahahah = JSON.stringify(entity);
            return _this._http.post(_this._apiUrl + '/PostUpdateDeleteClientAddressContactRequest/', hahahah, { headers: authHeader }).map(function (res) {
                serviceResponse.Result = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ClientMessagesService.prototype.PostClientRequest = function (entity) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            var hahahahah = JSON.stringify(entity);
            //hahahahah = ({ "PBId": "69fbed82-8f73-45bf-ba9f-e09ee4347800", "IsAngularClient": true, "LoggingEnabled": true, "LoggingUserId": "1-2", "Tenant": 1, "LoggingEntityId": "fffff", "LoggingEntityReference": "321" }) + "";
            return _this._http.post(_this._apiUrl + '/PostClientRequest/', hahahahah, { headers: authHeader }).map(function (res) {
                serviceResponse.Result = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ClientMessagesService.prototype.PostClientSearchByIDRequest = function (entity) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.post(_this._apiUrl + '/PostClientSearchByIDRequest/', JSON.stringify(entity), { headers: authHeader }).map(function (res) {
                serviceResponse.Result = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ClientMessagesService.prototype.CreateClientRequest = function (entity) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.post(_this._apiUrl + '/CreateClientRequest/', JSON.stringify(entity), { headers: authHeader }).map(function (res) {
                serviceResponse.Result = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ClientMessagesService.prototype.GetSingleClientPMByCode = function (code, isIncludeAll) {
        var _this = this;
        if (isIncludeAll === void 0) { isIncludeAll = false; }
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        code = encodeURIComponent(code);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetSingleClientPMByCode?' + 'code=' + code + "&isIncludeAll=" + isIncludeAll, { headers: authHeader }).map(function (response) {
                var pm = response.json();
                var entity;
                if (pm) {
                    entity = _this.MapJsonToEntityPM(pm);
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ClientMessagesService.prototype.PutRecallClientsForCutomsRequest = function (fileUploadParamerter) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Rx_1.Observable.defer(function () {
            return _this._http.put(_this._apiUrl + '/PutRecallClientsForCutomsRequest', JSON.stringify(fileUploadParamerter), {
                headers: authHeader,
            }).map(function (response) {
                var result = response.json();
                var pmresponse;
                pmresponse = new ServiceResponse_1.ServiceResponse();
                pmresponse.Result = result;
                return pmresponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ClientMessagesService.prototype.MapJsonToEntityPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new ClientPM_1.ClientPM();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        this.MapClientAddresses(entityPM, jsonPM, mapParent); // Call composition tables map methods
        entityPM.IsDirty = false;
        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
            entityPM.OldEntityPM.ClientAddresses = [];
            for (var item in entityPM.ClientAddresses) {
                var myClientAddressPM = entityPM.ClientAddresses[item];
                var newClientAddressPM = this.clone(myClientAddressPM);
                newClientAddressPM.ClientsAddressCommTypes = [];
                for (var k in myClientAddressPM.ClientsAddressCommTypes) {
                    var myClientsAddressCommTypePM = myClientAddressPM.ClientsAddressCommTypes[k];
                    var newClientsAddressCommTypePM = this.clone(myClientAddressPM.ClientsAddressCommTypes[k]);
                    newClientAddressPM.ClientsAddressCommTypes.push(newClientsAddressCommTypePM);
                }
                entityPM.OldEntityPM.ClientAddresses.push(newClientAddressPM);
            }
        }
        else {
            entityPM.OldEntityPM = null;
        }
        return entityPM;
    };
    ClientMessagesService.prototype.MapClientAddresses = function (entityPM, jsonPM, mapParent) {
        if (mapParent === void 0) { mapParent = true; }
        var oldClientAddresses = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldClientAddresses = entityPM.OldEntityPM.ClientAddresses;
        }
        entityPM.ClientAddresses = new Array();
        for (var item in jsonPM.ClientAddresses) {
            var jItem = jsonPM.ClientAddresses[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newClientAddressPM;
            if (mapParent) {
                newClientAddressPM = new ClientAddressPM_1.ClientAddressPM(entityPM);
            }
            else {
                newClientAddressPM = new ClientAddressPM_1.ClientAddressPM(null);
            }
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newClientAddressPM[pmProperty] = jItem[pmProperty];
            }
            newClientAddressPM.IsDirty = false;
            if (mapParent) {
                newClientAddressPM.UniqueKey = Guid_1.Guid.newGuid();
                newClientAddressPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newClientAddressPM.OldEntityPM = this.clone(newClientAddressPM);
                this.MapClientsAddressCommTypes(newClientAddressPM, jItem, mapParent);
                newClientAddressPM.OldEntityPM.ClientsAddressCommTypes = [];
                for (var k in newClientAddressPM.ClientsAddressCommTypes) {
                    var clonedInside = this.clone(newClientAddressPM.ClientsAddressCommTypes[k]);
                    newClientAddressPM.OldEntityPM.ClientsAddressCommTypes.push(clonedInside); // clone old ClientsAddressCommTypes//
                }
            }
            else {
                if (newClientAddressPM.UniqueKey) {
                    if (jItem.IsDirty)
                        newClientAddressPM.ChangeSetOp = "Update";
                }
                else {
                    newClientAddressPM.ChangeSetOp = "Insert";
                }
                this.MapClientsAddressCommTypes(newClientAddressPM, jItem, mapParent);
                newClientAddressPM.OldEntityPM = null;
                newClientAddressPM.EntityParentPM = null;
            }
            entityPM.ClientAddresses.push(newClientAddressPM);
        }
        if (oldClientAddresses) {
            for (var itemKey in oldClientAddresses) {
                if (entityPM.ClientAddresses.filter(function (p) { return p.UniqueKey === oldClientAddresses[itemKey].UniqueKey; }).length === 0) {
                    if (oldClientAddresses[itemKey]) {
                        //oldClientAddresses[itemKey].ChangeSetOp = "Delete";
                        //entityPM.ClientAddresses.push(oldClientAddresses[itemKey]);
                        var oldItemJson = oldClientAddresses[itemKey];
                        var deletedPM = new ClientAddressPM_1.ClientAddressPM(null);
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
                        this.MapClientsAddressCommTypes(deletedPM, oldItemJson, mapParent);
                        deletedPM.OldEntityPM = null;
                        entityPM.ClientAddresses.push(deletedPM);
                    }
                }
            }
        }
    };
    ClientMessagesService.prototype.MapClientsAddressCommTypes = function (entityPM, jsonPM, mapParent) {
        if (mapParent === void 0) { mapParent = true; }
        var oldClientsAddressCommTypes = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldClientsAddressCommTypes = entityPM.OldEntityPM.ClientsAddressCommTypes;
        }
        entityPM.ClientsAddressCommTypes = new Array();
        for (var item in jsonPM.ClientsAddressCommTypes) {
            var jItem = jsonPM.ClientsAddressCommTypes[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newClientsAddressCommTypePM;
            if (mapParent) {
                newClientsAddressCommTypePM = new ClientsAddressCommTypePM_1.ClientsAddressCommTypePM(entityPM);
            }
            else {
                newClientsAddressCommTypePM = new ClientsAddressCommTypePM_1.ClientsAddressCommTypePM(null);
            }
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newClientsAddressCommTypePM[pmProperty] = jItem[pmProperty];
            }
            newClientsAddressCommTypePM.IsDirty = false;
            if (mapParent) {
                newClientsAddressCommTypePM.UniqueKey = Guid_1.Guid.newGuid();
                newClientsAddressCommTypePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newClientsAddressCommTypePM.OldEntityPM = this.clone(newClientsAddressCommTypePM);
            }
            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    newClientsAddressCommTypePM.ChangeSetOp = "Delete";
                }
                else {
                    if (newClientsAddressCommTypePM.UniqueKey) {
                        if (jItem.IsDirty)
                            newClientsAddressCommTypePM.ChangeSetOp = "Update";
                    }
                    else {
                        newClientsAddressCommTypePM.ChangeSetOp = "Insert";
                    }
                }
                newClientsAddressCommTypePM.OldEntityPM = null;
                newClientsAddressCommTypePM.EntityParentPM = null;
            }
            entityPM.ClientsAddressCommTypes.push(newClientsAddressCommTypePM);
        }
        if (oldClientsAddressCommTypes) {
            for (var itemKey in oldClientsAddressCommTypes) {
                if (entityPM.ClientsAddressCommTypes.filter(function (p) { return p.UniqueKey === oldClientsAddressCommTypes[itemKey].UniqueKey; }).length === 0) {
                    if (oldClientsAddressCommTypes[itemKey]) {
                        //oldClientsAddressCommTypes[itemKey].ChangeSetOp = "Delete";
                        //entityPM.ClientsAddressCommTypes.push(oldClientsAddressCommTypes[itemKey]);
                        var oldItemJson = oldClientsAddressCommTypes[itemKey];
                        var deletedPM = new ClientsAddressCommTypePM_1.ClientsAddressCommTypePM(null);
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
                        entityPM.ClientsAddressCommTypes.push(deletedPM);
                    }
                }
            }
        }
    };
    ClientMessagesService.prototype.clone = function (jsonPM) {
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
    ClientMessagesService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], ClientMessagesService);
    return ClientMessagesService;
}());
exports.ClientMessagesService = ClientMessagesService;
//# sourceMappingURL=ClientMessagesService.js.map