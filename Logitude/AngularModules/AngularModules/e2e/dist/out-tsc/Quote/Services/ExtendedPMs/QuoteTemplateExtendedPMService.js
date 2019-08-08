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
var InfraSettings_1 = require("../../../Infrastructure/Utilities/InfraSettings");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var CustomFieldClass_1 = require("../../../Infrastructure/DataContracts/CustomFieldClass");
var PerformanceLogger_1 = require("../../../Infrastructure/Utilities/PerformanceLogger");
var QuoteTemplatePM_1 = require("../../EntityPMs/QuoteTemplatePM");
var QuoteTemplateSectionPM_1 = require("../../EntityPMs/QuoteTemplateSectionPM");
var QuoteTemplateExtendedPMService = /** @class */ (function () {
    function QuoteTemplateExtendedPMService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/QuoteTemplateExtended';
    }
    QuoteTemplateExtendedPMService.prototype.insert = function (entityPM) {
        var _this = this;
        var callTime = new Date();
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            var mappedEntity;
            mappedEntity = _this.MapJsonToEntityPM(entityPM, false);
            return _this._http.post(_this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map(function (response) {
                var pm = response.json();
                if (pm) {
                    var mappedResult;
                    mappedResult = _this.MapJsonToEntityPM(pm, true, entityPM);
                    serviceResponse.Result = mappedResult;
                }
                var servertime = response.headers.get('ServerExecutionTime');
                PerformanceLogger_1.PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "QuoteTemplate", "SaveChanges", "");
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    QuoteTemplateExtendedPMService.prototype.GetTemplateSectionsByQuoteTemplateIdAndQuoteId = function (id, quoteId, tenant, defultQuoteTemplate, quotationSections) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetTemplateSectionsByQuoteTemplateIdAndQuoteId/?' + 'id=' + id + '&quoteId=' + quoteId + '&tenant=' + tenant + '&defultQuoteTemplate=' + defultQuoteTemplate + '&quotationSections=' + quotationSections, { headers: authHeader }).map(function (response) {
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    QuoteTemplateExtendedPMService.prototype.GetQuoteTemplateListsByQuoteTemplateTypeAndTenant = function (quotetemplatetype, tenant) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetQuoteTemplateListsByQuoteTemplateTypeAndTenant/?' + 'quotetemplatetype=' + quotetemplatetype + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    QuoteTemplateExtendedPMService.prototype.GetQuoteTemplateLists = function (queryName) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetQuoteTemplateLists/?' + 'queryName=' + queryName, { headers: authHeader }).map(function (response) {
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    QuoteTemplateExtendedPMService.prototype.GetQuoteTemplateListsFromLibrary = function (quotetemplatetype) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetQuoteTemplateListsFromLibrary/?quotetemplatetype=' + quotetemplatetype, { headers: authHeader }).map(function (response) {
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    QuoteTemplateExtendedPMService.prototype.GetCopyQuoteTemplateFromLibrary = function (quoteTemplateId, userId) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetCopyQuoteTemplateFromLibrary/?' + 'quoteTemplateId=' + quoteTemplateId + '&userId=' + userId, { headers: authHeader }).map(function (response) {
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    QuoteTemplateExtendedPMService.prototype.GetCopyQuoteTemplate = function (quoteTemplateId, copyName, userid, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetCopyQuoteTemplate/?' + 'quoteTemplateId=' + quoteTemplateId + '&copyName=' + copyName + '&userid=' + userid + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var pm = response.json();
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = _this.MapJsonToEntityPM(pm, true);
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    QuoteTemplateExtendedPMService.prototype.GetQuoteTemplatePdfReport = function (quotePMId, templateId, userId) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetQuoteTemplatePdfReport/?' + 'quoteId=' + quotePMId + '&quoteTemplateId=' + templateId + '&userId=' + userId, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    QuoteTemplateExtendedPMService.prototype.GetUpdatedQuoteDocumentVersion = function (quoteId, versionNumber, quoteTemplateId, updatedByUserId, tenant, isGenerate) {
        if (isGenerate === void 0) { isGenerate = false; }
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetUpdatedQuoteDocumentVersion/?' + 'quoteId=' + quoteId + '&versionNumber=' + versionNumber + '&quoteTemplateId=' + quoteTemplateId + '&updatedByUserId=' + updatedByUserId + '&tenant=' + tenant + '&isGenerate=' + isGenerate, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    QuoteTemplateExtendedPMService.prototype.GetQuoteDocumentVersionsByQuoteId = function (quoteId, tenant) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetQuoteDocumentVersionsByQuoteId/?' + 'quoteId=' + quoteId + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    QuoteTemplateExtendedPMService.prototype.GetQuoteCustomerEmailByContactId = function (contactId) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetQuoteCustomerEmailByContactId/?' + 'contactId=' + contactId, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    QuoteTemplateExtendedPMService.prototype.UpLoadQuoteDocumentVersionFile = function (fileData, tenant) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return this._http.post(this._apiUrl + '/PostUpLoadQuoteDocumentVersionFile/?' + 'tenant=' + tenant, JSON.stringify(fileData), { headers: authHeader }).map(function (response) {
            var result = response.json();
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        //PostUpLoadQuoteDocumentVersionFile
        //PostUpLoadQuoteDocumentVersionFile(byte[] fileData, string quoteId, int versionNumber, string fileExtension, string updatedByUserId, int tenant)
    };
    //quotesContext.GetQuoteDocumentVersionsByQuoteIdQuery(this.QuotePM.Id, TenantContext.Current.Id), 
    QuoteTemplateExtendedPMService.prototype.MapJsonToEntityPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new QuoteTemplatePM_1.QuoteTemplatePM();
        }
        var customFields = [];
        for (var i = 1; i < 11; i++) {
            customFields.push("Field" + i);
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "PropertyChanged") {
                continue;
            }
            var property = jsonPMKeys[key];
            if (customFields.indexOf(property) > -1) {
                if (jsonPM[property]) {
                    var customFieldClass = new CustomFieldClass_1.CustomFieldClass(jsonPM[property].Value, jsonPM[property].FieldName, jsonPM[property].TableName);
                    entityPM[property] = customFieldClass;
                }
            }
            else {
                entityPM[property] = jsonPM[property];
            }
        }
        this.MapTemplateSections(entityPM, jsonPM, mapParent); // Call composition tables map methods
        entityPM.IsDirty = false;
        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
            entityPM.OldEntityPM.TemplateSections = [];
            for (var item in entityPM.TemplateSections) {
                var myQuoteTemplateSectionPM = entityPM.TemplateSections[item];
                var newQuoteTemplateSectionPM = this.clone(myQuoteTemplateSectionPM);
                entityPM.OldEntityPM.TemplateSections.push(newQuoteTemplateSectionPM);
            }
        }
        else {
            entityPM.OldEntityPM = null;
        }
        return entityPM;
    };
    QuoteTemplateExtendedPMService.prototype.MapTemplateSections = function (entityPM, jsonPM, mapParent) {
        if (mapParent === void 0) { mapParent = true; }
        entityPM.TemplateSections = new Array();
        for (var item in jsonPM.TemplateSections) {
            var jItem = jsonPM.TemplateSections[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newQuoteTemplateSectionPM;
            newQuoteTemplateSectionPM = new QuoteTemplateSectionPM_1.QuoteTemplateSectionPM();
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newQuoteTemplateSectionPM[pmProperty] = jItem[pmProperty];
            }
            newQuoteTemplateSectionPM.IsDirty = false;
            entityPM.TemplateSections.push(newQuoteTemplateSectionPM);
        }
    };
    QuoteTemplateExtendedPMService.prototype.clone = function (jsonPM) {
        var entityPM;
        entityPM = {};
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM" || jsonPMKeys[key] === "PropertyChanged") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        return entityPM;
    };
    QuoteTemplateExtendedPMService.prototype.GetNewEntityPM = function () {
        var entityPM;
        entityPM = new QuoteTemplatePM_1.QuoteTemplatePM();
        entityPM.Tenant = InfraSettings_1.InfraSettings.TenantPM.Id;
        return entityPM;
    };
    QuoteTemplateExtendedPMService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], QuoteTemplateExtendedPMService);
    return QuoteTemplateExtendedPMService;
}());
exports.QuoteTemplateExtendedPMService = QuoteTemplateExtendedPMService;
//# sourceMappingURL=QuoteTemplateExtendedPMService.js.map