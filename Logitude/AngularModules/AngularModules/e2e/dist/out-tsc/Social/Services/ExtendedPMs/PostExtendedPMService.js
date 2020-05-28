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
var CustomFieldClass_1 = require("../../../Infrastructure/DataContracts/CustomFieldClass");
var PostPM_1 = require("../../EntityPMs/PostPM");
var PostLikePM_1 = require("../../EntityPMs/PostLikePM");
var PostExtendedPMService = /** @class */ (function () {
    function PostExtendedPMService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/PostExtended';
    }
    PostExtendedPMService.prototype.GetSocialContact = function (id, tenant) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetSocialContact/?' + 'id=' + id + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    PostExtendedPMService.prototype.GetPostSummaryData = function (userId, loggedUserId, tenant) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetPostSummaryData/?' + 'userId=' + userId + '&loggedUserId=' + loggedUserId + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    PostExtendedPMService.prototype.DeletePostLike = function (postId, userId, tenant) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetDeletePostLike/?' + 'postId=' + postId + '&userId=' + userId + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    PostExtendedPMService.prototype.InsertPostLike = function (postLike) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            return _this._http.post(_this._apiUrl + '/PostInsertPostLike', JSON.stringify(postLike), { headers: authHeader }).map(function (response) {
                var result = response.json();
                var entity;
                entity = result;
                if (result) {
                    entity = _this.MapPostLikes2(result);
                }
                var pmresponse;
                pmresponse = new ServiceResponse_1.ServiceResponse();
                pmresponse.Result = entity;
                return pmresponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PostExtendedPMService.prototype.GetSinglePostComment = function (id, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetSinglePostComment/?' + 'id=' + id + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var entity;
            var PostPMLists;
            PostPMLists = new Array();
            result.forEach(function (item) {
                entity = _this.MapJsonToEntityPM(item);
                PostPMLists.push(entity);
            });
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = PostPMLists;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    PostExtendedPMService.prototype.PostFilteredPosts = function (postFilter) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            return _this._http.post(_this._apiUrl + '/PostFilteredPosts', JSON.stringify(postFilter), { headers: authHeader }).map(function (response) {
                var result = response.json();
                var entity;
                var postPMLists;
                postPMLists = new Array();
                result.forEach(function (item) {
                    entity = _this.MapJsonToEntityPM(item);
                    postPMLists.push(entity);
                });
                var pmresponse;
                pmresponse = new ServiceResponse_1.ServiceResponse();
                pmresponse.Result = postPMLists;
                return pmresponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PostExtendedPMService.prototype.GetCountPostPMsByFilter = function (postFilter) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            return _this._http.post(_this._apiUrl + '/PostGetCountPostPMsByFilter', JSON.stringify(postFilter), { headers: authHeader }).map(function (response) {
                var result = response.json();
                var pmresponse;
                pmresponse = new ServiceResponse_1.ServiceResponse();
                pmresponse.Result = result;
                return pmresponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PostExtendedPMService.prototype.MapJsonToEntityPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new PostPM_1.PostPM();
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
        this.MapPostComments(entityPM, jsonPM, mapParent); // Call composition tables map methods
        this.MapPostLikes(entityPM, jsonPM, mapParent); // Call composition tables map methods
        entityPM.IsDirty = false;
        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
            entityPM.OldEntityPM.PostComments = [];
            for (var item in entityPM.PostComments) {
                var myPostPM = entityPM.PostComments[item];
                var newPostPM = this.clone(myPostPM);
                newPostPM.PostLikes = [];
                for (var k in myPostPM.PostLikes) {
                    var myPostLikePM = myPostPM.PostLikes[k];
                    var newPostLikePM = this.clone(myPostPM.PostLikes[k]);
                    newPostPM.PostLikes.push(newPostLikePM);
                }
                entityPM.OldEntityPM.PostComments.push(newPostPM);
            }
            entityPM.OldEntityPM.PostLikes = [];
            for (var item in entityPM.PostLikes) {
                var myPostLikePM = entityPM.PostLikes[item];
                var newPostLikePM2 = this.clone(myPostLikePM);
                entityPM.OldEntityPM.PostLikes.push(newPostLikePM2);
            }
        }
        else {
            entityPM.OldEntityPM = null;
        }
        return entityPM;
    };
    PostExtendedPMService.prototype.MapPostComments = function (entityPM, jsonPM, mapParent) {
        if (mapParent === void 0) { mapParent = true; }
        entityPM.PostComments = new Array();
        for (var item in jsonPM.PostComments) {
            var jItem = jsonPM.PostComments[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newPostPM;
            newPostPM = new PostPM_1.PostPM();
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newPostPM[pmProperty] = jItem[pmProperty];
            }
            newPostPM.IsDirty = false;
            entityPM.PostComments.push(newPostPM);
        }
    };
    PostExtendedPMService.prototype.MapPostLikes = function (entityPM, jsonPM, mapParent) {
        if (mapParent === void 0) { mapParent = true; }
        var oldPostLikes = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldPostLikes = entityPM.OldEntityPM.PostLikes;
        }
        entityPM.PostLikes = new Array();
        for (var item in jsonPM.PostLikes) {
            var jItem = jsonPM.PostLikes[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newPostLikePM;
            if (mapParent) {
                newPostLikePM = new PostLikePM_1.PostLikePM(entityPM);
            }
            else {
                newPostLikePM = new PostLikePM_1.PostLikePM(null);
            }
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newPostLikePM[pmProperty] = jItem[pmProperty];
            }
            newPostLikePM.IsDirty = false;
            if (mapParent) {
                newPostLikePM.UniqueKey = Guid_1.Guid.newGuid();
                newPostLikePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newPostLikePM.OldEntityPM = this.clone(newPostLikePM);
            }
            else {
                if (newPostLikePM.UniqueKey) {
                    if (jItem.IsDirty)
                        newPostLikePM.ChangeSetOp = "Update";
                }
                else {
                    newPostLikePM.ChangeSetOp = "Insert";
                }
                newPostLikePM.OldEntityPM = null;
                newPostLikePM.EntityParentPM = null;
            }
            entityPM.PostLikes.push(newPostLikePM);
        }
        if (oldPostLikes) {
            for (var itemKey in oldPostLikes) {
                if (entityPM.PostLikes.filter(function (p) { return p.UniqueKey === oldPostLikes[itemKey].UniqueKey; }).length === 0) {
                    if (oldPostLikes[itemKey]) {
                        //oldPostLikes[itemKey].ChangeSetOp = "Delete";
                        //entityPM.PostLikes.push(oldPostLikes[itemKey]);
                        var oldItemJson = oldPostLikes[itemKey];
                        var deletedPM = new PostLikePM_1.PostLikePM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {
                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM" || pmKeys[key] === "PropertyChanged") {
                                continue;
                            }
                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }
                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";
                        deletedPM.OldEntityPM = null;
                        entityPM.PostLikes.push(deletedPM);
                    }
                }
            }
        }
    };
    PostExtendedPMService.prototype.MapPostLikes2 = function (jsonPM) {
        var entityPM;
        entityPM = new PostLikePM_1.PostLikePM(null);
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        entityPM.IsDirty = false;
        return entityPM;
    };
    PostExtendedPMService.prototype.clone = function (jsonPM) {
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
    PostExtendedPMService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], PostExtendedPMService);
    return PostExtendedPMService;
}());
exports.PostExtendedPMService = PostExtendedPMService;
//# sourceMappingURL=PostExtendedPMService.js.map